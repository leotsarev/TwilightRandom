namespace Twilight.Web.Auth;

public class JoinRpgOidcOptions
{
    public required Uri Issuer { get; set; }
    public required string ClientId { get; set; }
    public string? ClientSecret { get; set; }
}
