using MoreLinq;
using Twilight.Domain;

namespace TwilightRandom;

public class Randomiser
{
    private HashSet<string> Players { get; }
    private HashSet<PlayerColor> Colors { get; }
    private HashSet<Faction> Factions { get; }

    public Randomiser(GameRequest gameModel, IEnumerable<Faction> factions)
    {
        Players = new HashSet<string>(gameModel.Players ?? Array.Empty<string>());
        Colors = new HashSet<PlayerColor>(Enum.GetValues<PlayerColor>());
        Factions = new HashSet<Faction>(factions);
    }

    private class PlayerRandomizeCell
    {
        public required string PlayerName { get; set; }
        public PlayerColor? Color { get; set; }
        public Faction[]? Factions { get; set; }
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
            germesina.Color = SelectAndRemoveColorForImpaired();

        }

        foreach (var result in playersDict.Values.Where(r => r.Color is null))
        {
            result.Color = SelectAndRemoveRandom(Colors);
        }

        foreach (var result in playersDict.Values.Where(r => r.Factions is null))
        {
            result.Factions = new[] { SelectAndRemoveRandom(Factions), SelectAndRemoveRandom(Factions) };
        }

        var items = playersDict.Values.Shuffle().Select(x => new PlayerRandomizeItemResult(x.PlayerName, x.Color!.Value, x.Factions!)).ToArray();
        return new RandomizeResult(items, Factions.ToArray());
    }

    private PlayerColor SelectAndRemoveColorForImpaired()
    {
        var black = PlayerColor.Black;
        return Colors.Remove(black) ? black : SelectAndRemoveRandom(Colors);
    }

    private static T SelectAndRemoveRandom<T>(HashSet<T> set)
    {
        var idx = Random.Shared.Next(0, set.Count);
        var selected = set.ElementAt(idx);
        set.Remove(selected);
        return selected;
    }
}
