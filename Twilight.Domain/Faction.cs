namespace Twilight.Domain;

public class Faction 
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string? RussianName { get; set; }

    public override string ToString()
    {
        return RussianName ?? Name;
    }
}
