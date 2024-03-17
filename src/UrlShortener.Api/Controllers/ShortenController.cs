using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Api.Models;
using UrlShortener.Api.Services;
using UrlShortener.Entities;

namespace UrlShortener.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShortenController : ControllerBase
    {
        private readonly UrlShorteningService _urlShorteningService;
        private readonly ApplicationDbContext _dbContext;

        public ShortenController(
            UrlShorteningService urlShorteningService,
            ApplicationDbContext dbContext)
        {
            _urlShorteningService = urlShorteningService;
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> Post(ShortenUrlRequest request)
        {
            if (!Uri.TryCreate(request.Url, UriKind.Absolute, out _))
                return BadRequest("The specified Url is invalid");

            var code = await _urlShorteningService.GenerateUnqiueCode();

            var shortenedUrl = new ShortenedUrl()
            {
                Id = Guid.NewGuid(),
                LongUrl = request.Url,
                Code = code,
                ShortUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/api/{code}",
                CreatedOnUtc = DateTime.UtcNow
            };

            _dbContext.ShortenedUrls.Add(shortenedUrl);

            await _dbContext.SaveChangesAsync();

            return Ok(shortenedUrl.ShortUrl);
        }

        [HttpGet("/{code}")]
        public async Task<IActionResult> Get(string code)
        {
            var record = await _dbContext.ShortenedUrls.FirstOrDefaultAsync(c => c.Code == code);

            if (record is null)
            {
                return NotFound();
            }

            return Ok(record.LongUrl);
        }
    }
}
