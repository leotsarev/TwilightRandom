using System.Security.Claims;
using JoinRpg.Common.PrimitiveTypes;

namespace Twilight.Web.Auth;

internal static class ClaimsPrincipalExtensions
{
    internal static UserIdentification GetJoinrpgUserId(this ClaimsPrincipal user)
    {
        var value = user.FindFirstValue(ClaimTypes.NameIdentifier);
        return UserIdentification.TryParse(value, provider: null, out var userId)
            ? userId
            : throw new InvalidOperationException("User has no valid JoinrpgUserId claim");
    }

    internal static UserIdentification? TryGetJoinrpgUserId(this ClaimsPrincipal user)
    {
        var value = user.FindFirstValue(ClaimTypes.NameIdentifier);
        return UserIdentification.TryParse(value, provider: null, out var userId) ? userId : null;
    }
}
