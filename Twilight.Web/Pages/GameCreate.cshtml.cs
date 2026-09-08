using System.ComponentModel.DataAnnotations;
using JoinRpg.Common.PrimitiveTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Twilight.Dal;
using Twilight.Domain;
using Twilight.Web.GamePlayers;
using JoinRpg.Common.WebInfrastructure.Auth;
using TwilightRandom;

namespace Twilight.Web.Pages;

[Authorize]
public class GameCreateModel : PageModel
{
    public GameCreateModel(TwilightDbContext dbContext, IGameRepository gameRepository)
    {
        DbContext = dbContext;
        GameRepository = gameRepository;
    }

    private TwilightDbContext DbContext { get; }
    private IGameRepository GameRepository { get; }

    [Required]
    [BindProperty]
    public string Name { get; set; } = "";
    [Required, DataType(DataType.MultilineText)]
    [BindProperty]
    public string PlayerList { get; set; } = "";

    [BindProperty]
    public bool AddToEightPlayers { get; set; } = true;

    [BindProperty]
    public AllianceMode AllianceMode { get; set; } = AllianceMode.None;

    [Required]
    [BindProperty]
    public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Today);

    [Range(2, 3)]
    [BindProperty]
    public int FactionsPerPlayer { get; set; } = 2;

    public async Task OnGet()
    {
        var lastGame = await GameRepository.LoadLastGameOrDefault();
        if (lastGame is not null)
        {
            PlayerList = string.Join('\n', lastGame.PlayerSlots.Where(ps => !ps.Player.Name.StartsWith("Запасной")).Select(ps => ps.Player.Name));
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        var creatorUserId = User.GetJoinrpgUserId();
        var creator = await DbContext.Players.FirstOrDefaultAsync(p => p.JoinrpgUserId == creatorUserId);

        var game = new Game
        {
            Name = Name,
            CreatedByPlayer = creator,
            Date = Date,
        };

        var playerInputs = ParsePlayerList();
        var players = await ResolveDistinctPlayersAsync(playerInputs);

        var gameRequest = new GameRequest
        {
            Players = players.ToArray(),
            FactionsPerPlayer = FactionsPerPlayer,
        };

        var randomizer = new Randomiser(gameRequest, await DbContext.Factions.ToListAsync(), AllianceMode);
        var result = randomizer.Randomize();

        foreach (var res in result.Players)
        {
            game.PlayerSlots.Add(PlayerSlotFactory.CreateSlot(res));
        }

        DbContext.Games.Add(game);

        await DbContext.SaveChangesAsync();

        return RedirectToPage("GameView", new { game.Id });

    }

    private List<PlayerInput> ParsePlayerList()
    {
        var lines = PlayerList.Split("\n", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var playerList = lines.Select(ParsePlayerLine).ToList();

        if (AddToEightPlayers)
        {
            while (playerList.Count < 8)
            {
                playerList.Add(new PlayerInput($"Запасной игрок {playerList.Count + 1}", null));
            }
        }

        return playerList;
    }

    private static PlayerInput ParsePlayerLine(string line)
    {
        var parts = line.Split('#', 2, StringSplitOptions.TrimEntries);
        if (parts.Length == 2 && int.TryParse(parts[1], out var joinrpgUserId))
        {
            return new PlayerInput(parts[0], new UserIdentification(joinrpgUserId));
        }

        return new PlayerInput(line, null);
    }

    private async Task<List<Player>> ResolveDistinctPlayersAsync(List<PlayerInput> playerInputs)
    {
        var resolved = new List<Player>();
        foreach (var input in playerInputs)
        {
            resolved.Add(await PlayerResolver.ResolvePlayerAsync(DbContext, input.Name, input.JoinrpgUserId));
        }

        if (resolved.Distinct().Count() != playerInputs.Count)
        {
            throw new Exception("В списке игроков есть дубликаты (одинаковое имя или joinrpg id)");
        }

        return resolved;
    }

    private record PlayerInput(string Name, UserIdentification? JoinrpgUserId);
}
