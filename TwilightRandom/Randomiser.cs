using Twilight.Domain;

namespace TwilightRandom;

public class Randomiser
{
    private HashSet<Player> Players { get; }
    private HashSet<Faction> Factions { get; }
    public AllianceMode Alliance { get; }
    private int FactionsPerPlayer { get; }

    public Randomiser(GameRequest gameModel, IEnumerable<Faction> factions, AllianceMode alliance)
    {
        var requestedPlayers = gameModel.Players ?? Array.Empty<Player>();
        Players = new HashSet<Player>(requestedPlayers);
        if (Players.Count != requestedPlayers.Length)
        {
            throw new Exception("Duplicate players in game request");
        }
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
        Factions = new HashSet<Faction>(factions);
        Alliance = alliance;

        FactionsPerPlayer = gameModel.FactionsPerPlayer;
        if (FactionsPerPlayer < 2 || FactionsPerPlayer > 3)
        {
            throw new Exception("Factions per player must be 2 or 3");
        }
        if (Players.Count * FactionsPerPlayer > Factions.Count)
        {
            throw new Exception("Not enough factions for requested factions per player");
        }
    }

    public RandomizeResult Randomize()
    {
        // Visually impaired players are assigned first, so AddPlayer can still guarantee them Black.
        var orderedPlayers = Players.OrderByDescending(p => p.IsVisualImpaired);

        var usedColors = new HashSet<PlayerColor>();
        var availableFactions = new HashSet<Faction>(Factions);
        var results = new List<PlayerRandomizeItemResult>();

        foreach (var player in orderedPlayers)
        {
            var result = AddPlayer(player, usedColors, availableFactions, FactionsPerPlayer);
            usedColors.Add(result.Color);
            availableFactions.ExceptWith(result.Factions);
            results.Add(result);
        }

        var speakerNum = Random.Shared.Next(0, results.Count);
        results[speakerNum] = results[speakerNum] with { Speaker = true };

        var chooserNum = Random.Shared.Next(0, results.Count - 1);
        if (chooserNum >= speakerNum)
        {
            chooserNum++;
        }
        results[chooserNum] = results[chooserNum] with { ChoosePlace = true };

        var shuffled = results.Shuffle().ToList();

        SetAlliance(shuffled);

        return new RandomizeResult(shuffled.ToArray(), availableFactions.ToArray());
    }

    private void SetAlliance(List<PlayerRandomizeItemResult> players)
    {
        switch (Alliance)
        {
            case AllianceMode.None:
                break;
            case AllianceMode.Enabled:
                for (int i = 0; i < players.Count / 2; i++)
                {
                    MakeAllied(i, (i + players.Count / 2) % players.Count);
                }
                break;
            default:
                break;
        }

        void MakeAllied(int firstIdx, int secondIdx)
        {
            players[firstIdx] = players[firstIdx] with { AlliedWtih = players[secondIdx].Player.Name };
            players[secondIdx] = players[secondIdx] with { AlliedWtih = players[firstIdx].Player.Name };
        }
    }

    /// <summary>
    /// Randomly assigns a color and factions to a single player. Used both for a fresh <see cref="Randomize"/>
    /// (called once per player, in a loop) and for adding one player to an already-existing game.
    /// </summary>
    public static PlayerRandomizeItemResult AddPlayer(Player player, IEnumerable<PlayerColor> usedColors, IEnumerable<Faction> availableFactions, int factionsPerPlayer)
    {
        if (factionsPerPlayer < 2 || factionsPerPlayer > 3)
        {
            throw new Exception("Factions per player must be 2 or 3");
        }

        var colors = new HashSet<PlayerColor>(Enum.GetValues<PlayerColor>().Except(usedColors));
        if (colors.Count == 0)
        {
            throw new Exception("No free colors left");
        }

        var factions = new HashSet<Faction>(availableFactions);
        if (factions.Count < factionsPerPlayer)
        {
            throw new Exception("Not enough factions for requested factions per player");
        }

        var color = player.IsVisualImpaired && colors.Remove(PlayerColor.Black)
            ? PlayerColor.Black
            : SelectAndRemoveRandom(colors);

        var pickedFactions = Enumerable.Range(0, factionsPerPlayer).Select(_ => SelectAndRemoveRandom(factions)).ToArray();

        return new PlayerRandomizeItemResult(player, color, pickedFactions, Speaker: false, ChoosePlace: false, AlliedWtih: null);
    }

    private static T SelectAndRemoveRandom<T>(HashSet<T> set)
    {
        var idx = Random.Shared.Next(0, set.Count);
        var selected = set.ElementAt(idx);
        set.Remove(selected);
        return selected;
    }
}
