using System.Net.Http.Headers;
using UrlShortener.Models;

namespace UrlShortener.Services;

public interface IGithubService
{
    Task<IEnumerable<string>> GetTopUsersIds(int num, CancellationToken token);
    Task<IEnumerable<GithubUser>?> GetUsers(CancellationToken token);
    Task<IEnumerable<GithubUser?>> GetUsersByIds(IEnumerable<string> ids, CancellationToken token);
}

public class GithubService : IGithubService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<GithubService> _logger;

    public GithubService(HttpClient httpClient, IConfiguration configuration, ILogger<GithubService> logger)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _logger = logger;
        SetDefaults();
    }

    private void SetDefaults()
    {
        var config = _configuration.GetSection("HttpClient:Github");

        if (config is null)
        {
            _logger.LogError("HttpClient:Github config not available");
        }

        _httpClient.BaseAddress = new Uri(config?["BaseAddress"] ?? "");
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "C# HttpClient");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", config?["Token"]);
    }

    public async Task<IEnumerable<string>> GetTopUsersIds(int num, CancellationToken token)
    {
        return  (await GetUsers(token) ?? []).Select(c => c.Login).Take(num);
    }

    public Task<IEnumerable<GithubUser>?> GetUsers(CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        return _httpClient.GetFromJsonAsync<IEnumerable<GithubUser>>("users", token);
    }

    public Task<GithubUser?> GetUser(string id, CancellationToken token)
    {
        return _httpClient.GetFromJsonAsync<GithubUser>($"users/{id}", token);
    }

    public async Task<IEnumerable<GithubUser?>> GetUsersByIds(IEnumerable<string> ids, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        var usersTasks = ids.Select(c => GetUser(c, token));
        return await Task.WhenAll(usersTasks);
    }
}
