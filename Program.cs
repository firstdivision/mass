using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using mass.Components;
using mass.Components.Account;
using mass.Data;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging(logging => 
{
    logging.ClearProviders();
    logging.AddSimpleConsole(options =>
    {
        options.TimestampFormat = "[yyyy-MM-dd HH:mm:ss] ";
        options.ColorBehavior = Microsoft.Extensions.Logging.Console.LoggerColorBehavior.Enabled;
    });
});

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

var GCP_CLIENT_ID = Environment.GetEnvironmentVariable("GCP_CLIENT_ID");
var GCP_CLIENT_SECRET = Environment.GetEnvironmentVariable("GCP_CLIENT_SECRET");

// Make sure GCP credentials are not null
if (string.IsNullOrEmpty(GCP_CLIENT_ID) || string.IsNullOrEmpty(GCP_CLIENT_SECRET))
{
    throw new InvalidOperationException("Google Client ID or Client Secret is not set in environment variables.");
}   

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddGoogle(googleOptions =>
    {
        googleOptions.ClientId = GCP_CLIENT_ID;
        googleOptions.ClientSecret = GCP_CLIENT_SECRET;
    }).AddIdentityCookies();

var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION");

// Make sure connection string is not null
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Database connection string is not set in environment variables.");
}

builder.Services.AddDbContext<MassDbContext>(options =>
    options
        .UseNpgsql(connectionString)
        .UseSnakeCaseNamingConvention()
);

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<MassIdentityUser>(options =>
    {
        //options.SignIn.RequireConfirmedAccount = true;
        options.Stores.SchemaVersion = IdentitySchemaVersions.Version3;
    })
    .AddRoles<MassApplicationRole>()
    .AddEntityFrameworkStores<MassDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddDataProtection().PersistKeysToDbContext<MassDbContext>();

builder.Services.AddSingleton<IEmailSender<MassIdentityUser>, IdentityNoOpEmailSender>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    //app.UseHsts();
}

//add useforwarded headers to support reverse proxy scenarios
var forwardedOpts = new ForwardedHeadersOptions {
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
};
// in container/proxy setups it's common to clear KnownNetworks/Proxies:
forwardedOpts.KnownIPNetworks.Clear();
forwardedOpts.KnownProxies.Clear();

app.UseForwardedHeaders(forwardedOpts); // BEFORE authentication middleware

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
//app.UseHttpsRedirection();

app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Lax,
    HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.Always
});

app.MapStaticAssets();

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();
