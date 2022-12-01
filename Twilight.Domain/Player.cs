namespace Twilight.Domain;

public class Player
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public bool IsVisualImpaired { get; set; }
}
