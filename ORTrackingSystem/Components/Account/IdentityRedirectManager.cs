using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;

namespace ORTrackingSystem.Components.Account;

internal sealed class IdentityRedirectManager(NavigationManager navigationManager)
{
    public const string StatusCookieName = "Identity.StatusMessage";

    private static readonly CookieBuilder StatusCookieBuilder = new()
    {
        SameSite = SameSiteMode.Strict,
        HttpOnly = true,
        IsEssential = true,
        MaxAge = TimeSpan.FromSeconds(5),
    };

    // ✅ عدّل هذا إذا تريد صفحة افتراضية غير الداشبورد
    private const string DefaultAfterLoginPath = "/reports/dashboard";

    [DoesNotReturn]
    public void RedirectTo(string? uri)
    {
        var safe = NormalizeAndMakeSafeRedirect(uri);

        // During static rendering, NavigateTo throws a NavigationException which is handled by the framework as a redirect.
        navigationManager.NavigateTo(safe, forceLoad: true);

        throw new InvalidOperationException($"{nameof(IdentityRedirectManager)} can only be used during static rendering.");
    }

    [DoesNotReturn]
    public void RedirectTo(string uri, Dictionary<string, object?> queryParameters)
    {
        // Keep existing behavior but still sanitize at the end
        var uriWithoutQuery = navigationManager.ToAbsoluteUri(uri).GetLeftPart(UriPartial.Path);
        var newUri = navigationManager.GetUriWithQueryParameters(uriWithoutQuery, queryParameters);
        RedirectTo(newUri);
    }

    [DoesNotReturn]
    public void RedirectToWithStatus(string uri, string message, HttpContext context)
    {
        context.Response.Cookies.Append(StatusCookieName, message, StatusCookieBuilder.Build(context));
        RedirectTo(uri);
    }

    private string CurrentPath => navigationManager.ToAbsoluteUri(navigationManager.Uri).GetLeftPart(UriPartial.Path);

    [DoesNotReturn]
    public void RedirectToCurrentPage() => RedirectTo(CurrentPath);

    [DoesNotReturn]
    public void RedirectToCurrentPageWithStatus(string message, HttpContext context)
        => RedirectToWithStatus(CurrentPath, message, context);

    // -------------------------
    // Helpers
    // -------------------------

    private string NormalizeAndMakeSafeRedirect(string? uri)
    {
        // 1) null/empty -> default
        if (string.IsNullOrWhiteSpace(uri))
            return DefaultAfterLoginPath;

        uri = uri.Trim();

        // 2) Prevent open redirects. If absolute, convert to base-relative.
        //    Example: https://localhost:7162/reports/dashboard  -> reports/dashboard
        if (!Uri.IsWellFormedUriString(uri, UriKind.Relative))
        {
            uri = navigationManager.ToBaseRelativePath(uri);
        }

        // 3) Ensure leading slash
        uri = "/" + uri.TrimStart('/');

        // 4) Break redirect loops:
        //    If it points to Account/Login (or any Account page), send to default page.
        //    This prevents:
        //    /Account/Login?returnUrl=/Account/Login?returnUrl=...
        if (IsAccountPath(uri))
            return DefaultAfterLoginPath;

        // 5) If redirecting to the exact current path (rare), send to default to avoid loops.
        var currentRel = "/" + navigationManager.ToBaseRelativePath(navigationManager.Uri).TrimStart('/');
        if (string.Equals(StripQuery(uri), StripQuery(currentRel), StringComparison.OrdinalIgnoreCase))
            return DefaultAfterLoginPath;

        return uri;
    }

    private static bool IsAccountPath(string uri)
    {
        // We only check path part
        var path = StripQuery(uri);

        // Treat all /Account/* as not valid return targets (prevents login-loop)
        return path.StartsWith("/Account", StringComparison.OrdinalIgnoreCase)
               || path.Equals("/login", StringComparison.OrdinalIgnoreCase)
               || path.Equals("/logout", StringComparison.OrdinalIgnoreCase);
    }

    private static string StripQuery(string uri)
    {
        var q = uri.IndexOf('?', StringComparison.Ordinal);
        return q >= 0 ? uri[..q] : uri;
    }
}
