using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Models;
using UrlShortener.Services;
using UrlShortener.Entities;

namespace UrlShortener.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShortenController : ControllerBase
    {
        private readonly UrlShorteningService _urlShorteningService;
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<ShortenController> _logger;

        public ShortenController(
            UrlShorteningService urlShorteningService,
            ApplicationDbContext dbContext,
            ILogger<ShortenController> logger)
        {
            _urlShorteningService = urlShorteningService;
            _dbContext = dbContext;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Post(ShortenUrlRequest request)
        {
            if (!Uri.TryCreate(request.Url, UriKind.Absolute, out _))
            {
                _logger.LogInformation("The specified Url '{Url}' is not valid", request.Url);
                return BadRequest("The specified Url is invalid");
            }

            var code = await _urlShorteningService.GenerateUniqueCode();

            var shortenedUrl = new ShortenedUrl()
            {
                Id = Guid.NewGuid(),
                LongUrl = request.Url,
                Code = code,
                ShortUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/api/{code}",
            };

            _dbContext.ShortenedUrls.Add(shortenedUrl);

            await _dbContext.SaveChangesAsync();

            return Ok(shortenedUrl.ShortUrl);
        }

        [HttpGet("{code}")]
        public async Task<IActionResult> Get(string code)
        {
            var record = await _dbContext.ShortenedUrls.FirstOrDefaultAsync(c => c.Code == code);

            if (record is null)
            {
                return NotFound();
            }

            return Ok(record.LongUrl);
        }

        [HttpGet]
        public async IAsyncEnumerable<string> Get()
        {
            var list = await _dbContext.ShortenedUrls.ToListAsync();

            foreach (var item in list)
            {
                yield return item.LongUrl;
            }
        }
    }
}
