using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using UrlShortener.Models;
using UrlShortener.Services;
using UrlShortener.SwaggerResponse;

namespace UrlShortener.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GithubController(IGithubService githubService) : ControllerBase
{
    private readonly IGithubService _githubService = githubService;

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
    [SwaggerResponse(200, "Successful response", typeof(GithubUser))]
    [SwaggerResponseExample(200, typeof(GithubUserResponse))]
    public async Task<IActionResult> GetUserById(string userId)
    {
        var user = await _githubService.GetUserAsync(userId, HttpContext.RequestAborted);
        return Ok(user);
    }
}
