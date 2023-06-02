using Twilight.Domain;

namespace TwilightRandom;

    public record class RandomizeResult(PlayerRandomizeItemResult[] Players, Faction[] UnselectedFactions) { }
    
    public record class PlayerRandomizeItemResult(string PlayerName, PlayerColor Color, Faction[] Factions, bool Speaker, bool ChoosePlace, string? AlliedWtih) { }
