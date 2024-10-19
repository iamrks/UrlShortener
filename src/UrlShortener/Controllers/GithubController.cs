using Microsoft.AspNetCore.Mvc;
using UrlShortener.Services;

namespace UrlShortener.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GithubController(GithubService githubService) : ControllerBase
{
    private readonly GithubService _githubService = githubService;

    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        return Ok(await _githubService.GetUsersAsync(HttpContext.RequestAborted));
    }

    [HttpGet("/topUsers/:num")]
    public async Task<IActionResult> GetTopUsers(int num)
    {
        var cancellationToken = HttpContext.RequestAborted;
        var ids = await _githubService.GetTopUsersIdsAsync(num, cancellationToken);
        var users = await _githubService.GetUsersByIdsAsync(ids, cancellationToken);
        return Ok(users);
    }

    [HttpGet("/topUser/:userId")]
    public async Task<IActionResult> GetUserById(string userId)
    {
        var user = await _githubService.GetUserAsync(userId, HttpContext.RequestAborted);
        return Ok(user);
    }
}
