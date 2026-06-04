using Client = Supabase.Client;

namespace PortfolioSite.Services;

/// <summary>
/// Wraps Supabase email/password auth for the admin area. Exposes the current
/// sign-in state and raises a change event so the UI can react.
/// </summary>
public class AuthService
{
    private readonly Client _client;
    private readonly AppSettings _settings;
    private bool _initialized;

    public AuthService(Client client, AppSettings settings)
    {
        _client = client;
        _settings = settings;
    }

    public event Action? AuthStateChanged;

    public bool IsConfigured => _settings.Supabase.IsConfigured;

    public bool IsSignedIn => _client.Auth.CurrentSession is not null;

    public string? CurrentEmail => _client.Auth.CurrentUser?.Email;

    private async Task EnsureInitAsync()
    {
        if (_initialized || !IsConfigured) return;
        await _client.InitializeAsync();
        _initialized = true;
    }

    /// <summary>Returns null on success, or an error message on failure.</summary>
    public async Task<string?> SignInAsync(string email, string password)
    {
        if (!IsConfigured)
            return "Supabase isn't configured yet. Add your URL and anon key to appsettings.json.";

        try
        {
            await EnsureInitAsync();
            await _client.Auth.SignIn(email, password);
            AuthStateChanged?.Invoke();
            return IsSignedIn ? null : "Sign-in failed. Check your credentials.";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public async Task SignOutAsync()
    {
        await EnsureInitAsync();
        await _client.Auth.SignOut();
        AuthStateChanged?.Invoke();
    }
}
