using JoinRpg.Common.PrimitiveTypes;

namespace Twilight.Domain;

public class Player
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public bool IsVisualImpaired { get; set; }
    public UserIdentification? JoinrpgUserId { get; set; }
    public string? AvatarUrl { get; set; }
}
