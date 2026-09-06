using JoinRpg.Common.PrimitiveTypes;
using Twilight.Domain;

namespace Twilight.Web.Auth;

public static class GameAuthorization
{
    private static readonly UserIdentification AdminUserId = new(1);

    public static bool CanManage(Game game, UserIdentification? currentUserId)
        => currentUserId is not null
            && (currentUserId == AdminUserId || currentUserId == game.CreatedByPlayer?.JoinrpgUserId);
}
