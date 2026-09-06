using Microsoft.AspNetCore.Authentication.Cookies;

namespace Twilight.Web.Auth;

internal static class AuthRegistration
{
    internal static void AddJoinRpgAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IJoinUserLoginHandler, TwilightUserLoginHandler>();

        services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.LoginPath = "/login";
            });

        var oidcOptions = configuration.GetSection("JoinRpgOidc").Get<JoinRpgOidcOptions>()
            ?? throw new InvalidOperationException("JoinRpgOidc configuration section is required");

        services.AddOpenIddict()
            .AddClient(options =>
            {
                options.AllowAuthorizationCodeFlow();

                options.AddDevelopmentEncryptionCertificate()
                    .AddDevelopmentSigningCertificate();

                options.UseAspNetCore()
                    .EnableRedirectionEndpointPassthrough();

                options.UseSystemNetHttp();

                options.AddRegistration(new OpenIddict.Client.OpenIddictClientRegistration
                {
                    Issuer = oidcOptions.Issuer,
                    ClientId = oidcOptions.ClientId,
                    ClientSecret = oidcOptions.ClientSecret,
                    Scopes = { "openid", "profile" },
                    RedirectUri = new Uri("/signin-joinrpg", UriKind.Relative),
                });
            });

        services.AddAuthorization();
    }
}
