using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using PortfolioSite;
using PortfolioSite.Services;
using Client = Supabase.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Strongly-typed settings from wwwroot/appsettings.json
var settings = new AppSettings();
builder.Configuration.Bind(settings);
builder.Services.AddSingleton(settings);

// Supabase client (single instance for the app). Persists the session in localStorage
// so an admin stays logged in across refreshes.
builder.Services.AddSingleton(sp =>
{
    var s = sp.GetRequiredService<AppSettings>();
    var options = new Supabase.SupabaseOptions
    {
        AutoConnectRealtime = false,
        AutoRefreshToken = true
    };
    // Use safe placeholders when not configured so construction never throws;
    // the data/auth services guard every call with IsConfigured.
    var url = s.Supabase.IsConfigured ? s.Supabase.Url : "https://placeholder.supabase.co";
    var key = s.Supabase.IsConfigured ? s.Supabase.AnonKey : "placeholder";
    return new Client(url, key, options);
});

builder.Services.AddScoped<GitHubService>();
builder.Services.AddScoped<PortfolioDataService>();
builder.Services.AddScoped<AuthService>();

await builder.Build().RunAsync();
