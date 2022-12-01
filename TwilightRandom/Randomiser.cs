using MoreLinq;

namespace TwilightRandom;

public class Randomiser
{
    private HashSet<string> Players { get; }
    private HashSet<string> Colors { get; }
    private HashSet<string> Factions { get; }

    public Randomiser(GameRequest gameModel)
    {
        Players = new HashSet<string>(gameModel.Players ?? Array.Empty<string>());
        Colors = new HashSet<string>(DefaultData.Colors);
        Factions = new HashSet<string>(DefaultData.Factions);
    }

    private class PlayerRandomizeCell
    {
        public required string PlayerName { get; set; }
        public string? Color { get; set; }
        public string[]? Factions { get; set; }
    }



    public RandomizeResult Randomize()
    {
        while (Players.Count < 8)
        {
            Players.Add($"Запасной&nbsp;игрок&nbsp;{Players.Count + 1}");
        }

        var playersDict = Players.ToDictionary(player => player, player => new PlayerRandomizeCell { PlayerName = player });

        if (playersDict.TryGetValue("@Germesina", out var germesina))
        {
            germesina.Color = "Черный";
            Colors.Remove(germesina.Color);
        }

        foreach (var result in playersDict.Values.Where(r => r.Color is null))
        {
            result.Color = SelectAndRemoveRandom(Colors);
        }

        foreach (var result in playersDict.Values.Where(r => r.Factions is null))
        {
            result.Factions = new[] { SelectAndRemoveRandom(Factions), SelectAndRemoveRandom(Factions) };
        }

        var items = playersDict.Values.Shuffle().Select(x => new PlayerRandomizeItemResult(x.PlayerName, x.Color!, x.Factions!)).ToArray();
        return new RandomizeResult(items, Factions.ToArray());
    }

    private static string SelectAndRemoveRandom(HashSet<string> set)
    {
        var idx = Random.Shared.Next(0, set.Count);
        var selected = set.ElementAt(idx);
        set.Remove(selected);
        return selected.Replace(" ", "&nbsp;");
    }
}
