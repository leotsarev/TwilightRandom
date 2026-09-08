using Twilight.Domain;

namespace TwilightRandom;

public static class PlayerSlotFactory
{
    public static PlayerSlot CreateSlot(PlayerRandomizeItemResult result)
    {
        return new PlayerSlot
        {
            Color = result.Color,
            Player = result.Player,
            Slug = result.Player.JoinrpgUserId is null ? SlugGenerator.Generate(20) : null,
            SelectedFaction = null,
            PossibleFactions = result.Factions.ToList(),
            ChoosePlace = result.ChoosePlace,
            Speaker = result.Speaker,
            AlliedWith = result.AlliedWtih,
        };
    }
}
