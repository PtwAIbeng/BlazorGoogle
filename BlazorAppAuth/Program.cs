using BlazorAppAuth.Components;
using BlazorAppAuth.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMicrosoftIdentityWebAppAuthentication(builder.Configuration);

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<AuthenticatedUserInfo>();

// Needed for Azure AD
builder.Services.AddControllersWithViews(options =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
}).AddMicrosoftIdentityUI();


builder.Services.AddDistributedMemoryCache();

// Configure session with secure cookies
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true; // Prevents JavaScript from accessing the cookie
    options.Cookie.IsEssential = true; // Marks cookie as essential for the app's functionality
    options.Cookie.SameSite = SameSiteMode.Strict; // Protect against CSRF
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Ensures the cookie is sent only over HTTPS
});

// Add custom services
builder.Services.AddScoped<SessionService>();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddRazorPages();

// Add authorization services.
// Needed for Azure AD
builder.Services.AddAuthorization();

// Register the cascading authentication state.
// Needed for Azure AD
builder.Services.AddCascadingAuthenticationState();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Add HTTPS redirection
app.UseHttpsRedirection();

// Serve static files (if applicable)
app.UseStaticFiles(); // Optional, depends on your setup
// Use session middleware to enable session features
app.UseSession();

// Add authorization middleware
app.UseAuthorization();

// Add antiforgery middleware
app.UseAntiforgery();

// Needed for Azure AD
app.MapControllers();

// Map Razor components and routing
app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
