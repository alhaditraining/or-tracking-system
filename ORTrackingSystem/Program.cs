using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using ORTrackingSystem.Components;
using ORTrackingSystem.Components.Account;
using ORTrackingSystem.Data;

var builder = WebApplication.CreateBuilder(args);

// -------------------------
// Services
// -------------------------

// Razor Components (Blazor Server interactive)
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// MudBlazor
builder.Services.AddMudServices();

// Authentication state for Blazor
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

// Cookies auth (Identity)
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
})
.AddIdentityCookies();

// DB
var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// IdentityCore + Roles + SignInManager + Tokens
builder.Services.AddIdentityCore<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddSignInManager()
.AddDefaultTokenProviders();

// Email sender (No-op)
builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

// App services
builder.Services.AddScoped<ORTrackingSystem.Services.ExportService>();

// Authorization policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("ViewerOrAdmin", policy => policy.RequireRole("Admin", "Viewer"));
});

var app = builder.Build();

// -------------------------
// Middleware pipeline
// -------------------------

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// ✅ مهم جدًا: Authentication/Authorization قبل Antiforgery و قبل MapRazorComponents
app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

// -------------------------
// Extra endpoints (SSR-safe)
// -------------------------

// ✅ Logout endpoint (GET) - ثابت ومضمون، بدون 405 وبدون returnUrl مشاكل
app.MapGet("/logout", async (HttpContext ctx) =>
{
    // sign out from both schemes (safe)
    await ctx.SignOutAsync(IdentityConstants.ApplicationScheme);
    await ctx.SignOutAsync(IdentityConstants.ExternalScheme);

    // hard-delete common identity cookie (extra safety)
    ctx.Response.Cookies.Delete(".AspNetCore.Identity.Application");

    // avoid caching
    ctx.Response.Headers.CacheControl = "no-store";

    // redirect to login (SSR)
    ctx.Response.Redirect("/Account/Login");
});

// ✅ Login shortcut (GET) يضمن returnUrl محلي فقط
app.MapGet("/login", (HttpContext ctx) =>
{
    // رجّع للداشبورد بعد الدخول
    ctx.Response.Redirect("/Account/Login?returnUrl=/reports/dashboard");
});

// ✅ Debug ping (اختياري - احذفه بعد ما تتأكد كلشي تمام)
// app.MapGet("/__ping", () => Results.Text("PING OK"));

// -------------------------
// Endpoints
// -------------------------

app.MapRazorComponents<App>()
   .AddInteractiveServerRenderMode();

// Identity endpoints for /Account Razor components
app.MapAdditionalIdentityEndpoints();

// -------------------------
// Seed roles/admin
// -------------------------
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await DbInitializer.SeedRolesAndAdminAsync(services);
}

app.Run();
