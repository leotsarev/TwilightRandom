using System.Security.Claims;
using JoinRpg.Common.PrimitiveTypes;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Client.AspNetCore;
using Twilight.Dal;
using Twilight.Domain;

namespace Twilight.Web.Auth;

internal static class AuthEndpoints
{
    internal static void MapAuthEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/login", (HttpContext context) =>
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = "/signin-joinrpg",
            };
            return Results.Challenge(properties, [OpenIddictClientAspNetCoreDefaults.AuthenticationScheme]);
        });

        endpoints.MapGet("/signin-joinrpg", async (HttpContext context, TwilightDbContext dbContext, ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("Auth");

            var result = await context.AuthenticateAsync(OpenIddictClientAspNetCoreDefaults.AuthenticationScheme);
            if (!result.Succeeded)
            {
                logger.LogWarning("OIDC authentication failed: {Error}", result.Failure?.Message);
                return Results.Problem(
                    detail: result.Failure?.Message ?? "Authentication callback failed",
                    statusCode: StatusCodes.Status401Unauthorized);
            }

            var sub = result.Principal?.FindFirstValue("sub");
            if (sub is null || !UserIdentification.TryParse(sub, provider: null, out var userId))
            {
                logger.LogWarning("Failed to extract user ID from claims. sub={Sub}, claims=[{Claims}]",
                    sub, string.Join(", ", result.Principal?.Claims.Select(c => $"{c.Type}={c.Value}") ?? []));
                return Results.Problem(
                    detail: $"Could not extract numeric user ID from 'sub' claim (got: {sub})",
                    statusCode: StatusCodes.Status401Unauthorized);
            }

            var name = result.Principal?.FindFirstValue("name") ?? $"Игрок {userId}";
            var avatarUrl = result.Principal?.FindFirstValue("picture");

            var player = await dbContext.Players.FirstOrDefaultAsync(p => p.JoinrpgUserId == userId);
            if (player is null)
            {
                player = new Player
                {
                    Name = name,
                    JoinrpgUserId = userId,
                    AvatarUrl = avatarUrl,
                };
                dbContext.Players.Add(player);
            }
            else
            {
                player.Name = name;
                player.AvatarUrl = avatarUrl;
            }

            await dbContext.SaveChangesAsync();

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, userId.Value.ToString()),
                new(ClaimTypes.Name, player.Name),
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await context.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));

            return Results.Redirect("/");
        });

        endpoints.MapGet("/logout", async (HttpContext context) =>
        {
            await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Results.Redirect("/");
        });
    }
}
