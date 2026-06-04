namespace PortfolioSite.Services;

/// <summary>
/// Strongly-typed view of wwwroot/appsettings.json. Populated from configuration
/// in Program.cs and injected wherever the GitHub username / Supabase keys are needed.
/// </summary>
public class AppSettings
{
    public GitHubOptions GitHub { get; set; } = new();
    public SupabaseOptions Supabase { get; set; } = new();

    public class GitHubOptions
    {
        public string Username { get; set; } = "";
        public string Topic { get; set; } = "portfolio";
    }

    public class SupabaseOptions
    {
        public string Url { get; set; } = "";
        public string AnonKey { get; set; } = "";

        /// <summary>True once real values (not the scaffold placeholders) are present.</summary>
        public bool IsConfigured =>
            !string.IsNullOrWhiteSpace(Url) &&
            !Url.Contains("YOUR_PROJECT_REF") &&
            !string.IsNullOrWhiteSpace(AnonKey) &&
            !AnonKey.Contains("YOUR_SUPABASE");
    }
}
