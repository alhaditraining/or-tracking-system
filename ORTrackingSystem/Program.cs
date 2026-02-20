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

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMudServices(cfg =>
{
    cfg.SnackbarConfiguration.ShowCloseIcon = true;
    cfg.SnackbarConfiguration.VisibleStateDuration = 3000;
});

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
})
.AddIdentityCookies();

// ✅ هنا المهم: خلّي اسم باراميتر الرجوع نفس اللي يظهر عندك (ReturnUrl)
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.ReturnUrlParameter = "ReturnUrl"; // ✅ حرف كبير
});

var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddSignInManager()
.AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

builder.Services.AddScoped<ORTrackingSystem.Services.ExportService>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("ViewerOrAdmin", policy => policy.RequireRole("Admin", "Viewer"));
});
//remove when deploying to production
builder.WebHost.UseUrls("https://localhost:7176");

var app = builder.Build();

// -------------------------
// Middleware
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

app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

// -------------------------
// SSR-safe Endpoints
// -------------------------
app.MapGet("/logout", async (HttpContext ctx) =>
{
    await ctx.SignOutAsync(IdentityConstants.ApplicationScheme);
    await ctx.SignOutAsync(IdentityConstants.ExternalScheme);
    ctx.Response.Redirect("/Account/Login");
});

app.MapGet("/login", (HttpContext ctx) =>
{
    // ✅ نفس الاسم ReturnUrl حرف كبير
    ctx.Response.Redirect("/Account/Login?ReturnUrl=/reports/dashboard");
});

// -------------------------
// Blazor + Identity endpoints
// -------------------------
app.MapRazorComponents<App>()
   .AddInteractiveServerRenderMode();

app.MapAdditionalIdentityEndpoints();

// Seed
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await DbInitializer.SeedRolesAndAdminAsync(services);
}

app.Run();
