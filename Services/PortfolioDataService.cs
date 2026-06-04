using Supabase;
using Supabase.Postgrest;
using Client = Supabase.Client;

namespace PortfolioSite.Services;

/// <summary>
/// Thin wrapper over the Supabase client exposing the read/write operations the site needs.
/// All reads degrade gracefully (return empty/null) when Supabase isn't configured yet,
/// so the app renders during local development before keys are filled in.
/// </summary>
public class PortfolioDataService
{
    private readonly Client _client;
    private readonly AppSettings _settings;
    private bool _initialized;

    public PortfolioDataService(Client client, AppSettings settings)
    {
        _client = client;
        _settings = settings;
    }

    public bool IsConfigured => _settings.Supabase.IsConfigured;

    /// <summary>Lazily initialize the Supabase client on first use.</summary>
    private async Task EnsureInitAsync()
    {
        if (_initialized || !IsConfigured)
            return;
        await _client.InitializeAsync();
        _initialized = true;
    }

    // ---- Profile (About me) ----

    public async Task<Profile?> GetProfileAsync()
    {
        if (!IsConfigured) return null;
        await EnsureInitAsync();
        var res = await _client.From<Profile>().Limit(1).Get();
        return res.Models.FirstOrDefault();
    }

    public async Task UpsertProfileAsync(Profile profile)
    {
        await EnsureInitAsync();
        await _client.From<Profile>().Upsert(profile);
    }

    // ---- Skills ----

    public async Task<List<Skill>> GetSkillsAsync()
    {
        if (!IsConfigured) return new();
        await EnsureInitAsync();
        var res = await _client.From<Skill>()
            .Order("sort_order", Constants.Ordering.Ascending)
            .Get();
        return res.Models;
    }

    public async Task SaveSkillAsync(Skill skill) =>
        await (skill.Id == 0 ? InsertAsync(skill) : _client.From<Skill>().Update(skill));

    public async Task DeleteSkillAsync(Skill skill)
    {
        await EnsureInitAsync();
        await _client.From<Skill>().Delete(skill);
    }

    // ---- Upcoming projects ----

    public async Task<List<UpcomingProject>> GetUpcomingProjectsAsync()
    {
        if (!IsConfigured) return new();
        await EnsureInitAsync();
        var res = await _client.From<UpcomingProject>()
            .Order("sort_order", Constants.Ordering.Ascending)
            .Get();
        return res.Models;
    }

    public async Task SaveUpcomingProjectAsync(UpcomingProject project) =>
        await (project.Id == 0 ? InsertAsync(project) : _client.From<UpcomingProject>().Update(project));

    public async Task DeleteUpcomingProjectAsync(UpcomingProject project)
    {
        await EnsureInitAsync();
        await _client.From<UpcomingProject>().Delete(project);
    }

    // ---- Contact messages ----

    public async Task SendMessageAsync(ContactMessage message)
    {
        await EnsureInitAsync();
        await _client.From<ContactMessage>().Insert(message);
    }

    public async Task<List<ContactMessage>> GetMessagesAsync()
    {
        await EnsureInitAsync();
        var res = await _client.From<ContactMessage>()
            .Order("created_at", Constants.Ordering.Descending)
            .Get();
        return res.Models;
    }

    private async Task InsertAsync<T>(T model) where T : Supabase.Postgrest.Models.BaseModel, new()
    {
        await EnsureInitAsync();
        await _client.From<T>().Insert(model);
    }
}
