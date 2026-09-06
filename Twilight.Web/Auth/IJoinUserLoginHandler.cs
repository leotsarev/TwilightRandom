using System.Security.Claims;
using JoinRpg.Common.PrimitiveTypes;

namespace Twilight.Web.Auth;

/// <summary>
/// Приложение-специфичная часть логина через id.joinrpg.ru: сохраняет/обновляет пользователя
/// и дополняет claims, с которыми он будет залогинен по cookie-схеме.
/// </summary>
internal interface IJoinUserLoginHandler
{
    Task HandleLoginAsync(UserIdentification userId, ClaimsPrincipal externalPrincipal, List<Claim> claims, CancellationToken cancellationToken);
}
