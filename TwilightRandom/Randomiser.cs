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
        if (Players.Count > 8)
        {
            throw new Exception("Too many players");
        }
        if (Players.Count < 2)
        {
            throw new Exception("Too little players");
        }
        Colors = new HashSet<PlayerColor>(Enum.GetValues<PlayerColor>());
        Factions = new HashSet<Faction>(factions);
    }

    private class PlayerRandomizeCell
    {
        public required string PlayerName { get; init; }
        public PlayerColor? Color { get; set; }
        public Faction[]? Factions { get; set; }

        public bool Speaker { get; set; }
        public bool ChoosePlace { get; set; }
    }

    public RandomizeResult Randomize()
    {
        var players = Players.Select(player => new PlayerRandomizeCell { PlayerName = player }).ToArray();

        if (players.SingleOrDefault(p => p.PlayerName == "@Germesina") is PlayerRandomizeCell germesina)
        {
            germesina.Color = SelectAndRemoveColorForImpaired();
        }

        foreach (var result in players.Where(r => r.Color is null))
        {
            result.Color = SelectAndRemoveRandom(Colors);
        }

        foreach (var result in players)
        {
            result.Factions = new[] { SelectAndRemoveRandom(Factions), SelectAndRemoveRandom(Factions) };
        }

        var speakerNum = Random.Shared.Next(0, Players.Count);

        players[speakerNum].Speaker = true;

        var chooserNum = Random.Shared.Next(0, Players.Count - 1);

        if (chooserNum >= speakerNum)
        {
            chooserNum++;
        }

        players[chooserNum].ChoosePlace = true;

        var items = players.Shuffle()
            .Select(cell => new PlayerRandomizeItemResult(cell.PlayerName, cell.Color!.Value, cell.Factions!, cell.Speaker, cell.ChoosePlace))
            .ToArray();
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
