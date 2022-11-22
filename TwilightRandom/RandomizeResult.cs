namespace TwilightRandom;

    public record class RandomizeResult(PlayerRandomizeItemResult[] Players, string[] UnselectedFactions) { }
    
    public record class PlayerRandomizeItemResult(string PlayerName, string Color, string[] Factions) { }
