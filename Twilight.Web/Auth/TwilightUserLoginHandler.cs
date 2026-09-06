using System.Security.Claims;
using JoinRpg.Common.PrimitiveTypes;
using Microsoft.EntityFrameworkCore;
using Twilight.Dal;
using Twilight.Domain;

namespace Twilight.Web.Auth;

internal class TwilightUserLoginHandler(TwilightDbContext dbContext) : IJoinUserLoginHandler
{
    public async Task HandleLoginAsync(UserIdentification userId, ClaimsPrincipal externalPrincipal, List<Claim> claims, CancellationToken cancellationToken)
    {
        var name = externalPrincipal.FindFirstValue("name") ?? $"Игрок {userId}";
        var avatarUrl = externalPrincipal.FindFirstValue("picture");

        var player = await dbContext.Players.FirstOrDefaultAsync(p => p.JoinrpgUserId == userId, cancellationToken);
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

        await dbContext.SaveChangesAsync(cancellationToken);

        claims.Add(new Claim(ClaimTypes.Name, player.Name));
    }
}
