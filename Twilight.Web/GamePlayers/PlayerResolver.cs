using JoinRpg.Common.PrimitiveTypes;
using Microsoft.EntityFrameworkCore;
using Twilight.Dal;
using Twilight.Domain;

namespace Twilight.Web.GamePlayers;

/// <summary>
/// Shared between initial game creation (<see cref="Pages.GameCreateModel"/>) and adding a single
/// player to an existing game (<see cref="Pages.GamePlayerAddModel"/>).
/// </summary>
public static class PlayerResolver
{
    public static async Task<Player> ResolvePlayerAsync(TwilightDbContext dbContext, string playerName, UserIdentification? joinrpgUserId)
    {
        if (joinrpgUserId is not null)
        {
            var playerByJoinrpgUserId = await dbContext.Players.FirstOrDefaultAsync(p => p.JoinrpgUserId == joinrpgUserId);
            if (playerByJoinrpgUserId is not null)
            {
                return playerByJoinrpgUserId;
            }
        }

        var playerByName = await dbContext.Players.FirstOrDefaultAsync(p => p.Name == playerName);
        if (playerByName is not null)
        {
            return playerByName;
        }

        var player = new Player
        {
            Name = playerName,
            IsVisualImpaired = false,
            JoinrpgUserId = joinrpgUserId,
        };
        dbContext.Players.Add(player);
        await dbContext.SaveChangesAsync();
        return player;
    }
}
