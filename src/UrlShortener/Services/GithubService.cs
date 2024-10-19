using Microsoft.Extensions.Caching.Hybrid;
using System.Net.Http.Headers;
using UrlShortener.Models;

namespace UrlShortener.Services;

public interface IGithubService
{
    Task<IEnumerable<string>> GetTopUsersIdsAsync(int num, CancellationToken token);
    Task<IEnumerable<GithubUser>?> GetUsersAsync(CancellationToken token);
    Task<GithubUser?> GetUserAsync(string userId, CancellationToken token);
    Task<IEnumerable<GithubUser?>> GetUsersByIdsAsync(IEnumerable<string> ids, CancellationToken token);
}

public class GithubService : IGithubService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<GithubService> _logger;
    private readonly HybridCache _hybridCache;

    public GithubService(HttpClient httpClient,
                            IConfiguration configuration,
                            ILogger<GithubService> logger,
                            HybridCache hybridCache)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _logger = logger;
        _hybridCache = hybridCache;
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

    public async Task<IEnumerable<string>> GetTopUsersIdsAsync(int num, CancellationToken token)
    {
        return (await GetUsersAsync(token) ?? []).Select(c => c.Login).Take(num);
    }

    public Task<IEnumerable<GithubUser>?> GetUsersAsync(CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        return _httpClient.GetFromJsonAsync<IEnumerable<GithubUser>>("users", token);
    }

    public async Task<GithubUser?> GetUserAsync(string userId, CancellationToken token)
    {
        string key = $"GitUser:{userId}";
        return await _hybridCache.GetOrCreateAsync<GithubUser?>(key,
                        async cancel => await GetUserByIdAsync(userId, token));
    }

    public async Task<IEnumerable<GithubUser?>> GetUsersByIdsAsync(IEnumerable<string> ids, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        var usersTasks = ids.Select(c => GetUserAsync(c, token));
        return await Task.WhenAll(usersTasks);
    }

    private async Task<GithubUser?> GetUserByIdAsync(string userId, CancellationToken token)
    {
        return await _httpClient.GetFromJsonAsync<GithubUser>($"users/{userId}", token);
    }
}
