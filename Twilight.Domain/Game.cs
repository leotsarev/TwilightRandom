using System.ComponentModel.DataAnnotations;

namespace Twilight.Domain;

public class Game
{
    public int Id { get; set; }
    public virtual List<PlayerSlot> PlayerSlots { get; set; } = new();

    public required string Name { get; set; }

    public int? CreatedByPlayerId { get; set; }
    public virtual Player? CreatedByPlayer { get; set; }

    public DateOnly? Date { get; set; }
    public GameStatus Status { get; set; } = GameStatus.ChoosingSides;
    public string? Notes { get; set; }
}

public class PlayerSlot
{
    public int Id { get; set; }
    public int GameId { get; set; }
    public virtual Game Game { get; set; } = null!;
    public int PlayerId { get; set; }
    public virtual Player Player { get; set; } = null!;
    public PlayerColor Color { get; set; }
    public List<Faction> PossibleFactions { get; set; } = new();
    public Faction? SelectedFaction { get; set; }

    [MaxLength(20)]
    public string? Slug { get; set; }

    public required bool Speaker { get; set; }
    public required bool ChoosePlace { get; set; }

    public required string? AlliedWith { get; set; }

    public int? Points { get; set; }
    public bool IsWinner { get; set; }
}
