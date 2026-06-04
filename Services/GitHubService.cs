using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace PortfolioSite.Services;

/// <summary>
/// Fetches the user's public repositories from GitHub's REST API and keeps only the
/// ones carrying the configured topic (default "portfolio"). Results are cached for the
/// lifetime of the app session so revisiting the page doesn't hit the rate limit.
/// </summary>
public class GitHubService
{
    private readonly HttpClient _http;
    private readonly AppSettings _settings;
    private List<GitHubRepo>? _cache;

    public GitHubService(HttpClient http, AppSettings settings)
    {
        _http = http;
        _settings = settings;
    }

    public async Task<IReadOnlyList<GitHubRepo>> GetFeaturedReposAsync()
    {
        if (_cache is not null)
            return _cache;

        var username = _settings.GitHub.Username;
        var topic = _settings.GitHub.Topic;

        var url = $"https://api.github.com/users/{username}/repos?per_page=100&sort=updated&type=owner";
        var repos = await _http.GetFromJsonAsync<List<GitHubRepo>>(url) ?? new();

        _cache = repos
            .Where(r => !r.Fork && !r.Archived)
            .Where(r => string.IsNullOrEmpty(topic)
                        || (r.Topics?.Contains(topic, StringComparer.OrdinalIgnoreCase) ?? false))
            .OrderByDescending(r => r.StargazersCount)
            .ThenByDescending(r => r.PushedAt)
            .ToList();

        return _cache;
    }
}

public class GitHubRepo
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = "";

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("html_url")]
    public string HtmlUrl { get; set; } = "";

    [JsonPropertyName("homepage")]
    public string? Homepage { get; set; }

    [JsonPropertyName("language")]
    public string? Language { get; set; }

    [JsonPropertyName("stargazers_count")]
    public int StargazersCount { get; set; }

    [JsonPropertyName("forks_count")]
    public int ForksCount { get; set; }

    [JsonPropertyName("topics")]
    public List<string>? Topics { get; set; }

    [JsonPropertyName("fork")]
    public bool Fork { get; set; }

    [JsonPropertyName("archived")]
    public bool Archived { get; set; }

    [JsonPropertyName("pushed_at")]
    public DateTime? PushedAt { get; set; }
}
