using Twilight.Domain;

namespace TwilightRandom;

public class GameRequest
{
    public Player[]? Players { get; set; }
    public int FactionsPerPlayer { get; set; } = 2;
}
