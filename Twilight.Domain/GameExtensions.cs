namespace Twilight.Domain;

public static class GameExtensions
{
    public static IEnumerable<Faction> ExceptAlreadyUsedIn(this List<Faction> allFactions, Game game)
    {
        return allFactions
            .Except(game.PlayerSlots.SelectMany(f => f.PossibleFactions))
            .Except(game.PlayerSlots.Select(f => f.SelectedFaction).WhereNotNull());
    }

    public static IEnumerable<Faction> ExceptAlreadyUsedInForUser(this List<Faction> allFactions, Game game, int? slotId)
    {
        Faction? playerSelectedFaction = game.PlayerSlots.FirstOrDefault(p => p.Id == slotId)?.SelectedFaction;
        return allFactions
            .Except(game.PlayerSlots.SelectMany(f => f.PossibleFactions))
            .Where(f => f != playerSelectedFaction);
    }
}
