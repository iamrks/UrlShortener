using Microsoft.AspNetCore.Mvc;
using UrlShortener.Services;

namespace UrlShortener.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GithubController : ControllerBase
    {
        private readonly GithubService _githubService;

        public GithubController(GithubService githubService)
        {
            _githubService = githubService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            return Ok(await _githubService.GetUsers(HttpContext.RequestAborted));
        }

        [HttpGet("/topUsers/:num")]
        public async Task<IActionResult> GetTopUsers(int num)
        {
            var token = HttpContext.RequestAborted;
            var ids = await _githubService.GetTopUsersIds(num, token);
            var users = await _githubService.GetUsersByIds(ids, token);
            return Ok(users);
        }
    }
}
