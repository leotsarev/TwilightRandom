using Twilight.Domain;

namespace TwilightRandom;

public class Randomiser
{
    private HashSet<string> Players { get; }
    private HashSet<PlayerColor> Colors { get; }
    private HashSet<Faction> Factions { get; }
    public AllianceMode Alliance { get; }

    public Randomiser(GameRequest gameModel, IEnumerable<Faction> factions, AllianceMode alliance)
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

        if (alliance != AllianceMode.None && Players.Count % 2 != 0)
        {
            throw new Exception("Odd number of player incompatible with alliance");
        }
        Colors = new HashSet<PlayerColor>(Enum.GetValues<PlayerColor>());
        Factions = new HashSet<Faction>(factions);
        Alliance = alliance;
    }

    private class PlayerRandomizeCell
    {
        public required string PlayerName { get; init; }
        public PlayerColor? Color { get; set; }
        public Faction[]? Factions { get; set; }

        public bool Speaker { get; set; }
        public bool ChoosePlace { get; set; }
        public string? AlliedWtih { get; set; }
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

        players = players.Shuffle().ToArray();

        SetAlliance(players);

        var items = players
            .Select(cell => new PlayerRandomizeItemResult(cell.PlayerName, cell.Color!.Value, cell.Factions!, cell.Speaker, cell.ChoosePlace, cell.AlliedWtih))
            .ToArray();
        return new RandomizeResult(items, Factions.ToArray());
    }

    private void SetAlliance(PlayerRandomizeCell[] players)
    {
        switch (Alliance)
        {
            case AllianceMode.None:
                break;
            case AllianceMode.Enabled:
                for (int i = 0; i < players.Length / 2; i++)
                {
                    MakeAllied(i, (i + players.Length / 2) % players.Length);
                }
                break;
            default:
                break;
        }

        void MakeAllied(int firstIdx, int secondIdx)
        {
            players[firstIdx].AlliedWtih = players[secondIdx].PlayerName;
            players[secondIdx].AlliedWtih = players[firstIdx].PlayerName;
        }
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
