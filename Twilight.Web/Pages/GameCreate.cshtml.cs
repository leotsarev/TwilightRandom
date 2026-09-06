using System.ComponentModel.DataAnnotations;
using JoinRpg.Common.PrimitiveTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Twilight.Dal;
using Twilight.Domain;
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
            Slug = SlugGenerator.Generate(20),
            CreatedByPlayer = creator,
            Date = Date,
        };

        var playerInputs = ParsePlayerList();
        var gameRequest = ConvertToGameRequest(playerInputs);

        var randomizer = new Randomiser(gameRequest, await DbContext.Factions.ToListAsync(), AllianceMode);
        var result = randomizer.Randomize();

        var joinrpgUserIdsByName = playerInputs
            .Where(p => p.JoinrpgUserId is not null)
            .ToDictionary(p => p.Name, p => p.JoinrpgUserId!);

        foreach (var res in result.Players)
        {
            var playerName = res.PlayerName;
            joinrpgUserIdsByName.TryGetValue(playerName, out var joinrpgUserId);

            var player = await ResolvePlayer(playerName, joinrpgUserId);

            game.PlayerSlots.Add(CreatePlayerSlot(player, res));
        }

        DbContext.Games.Add(game);

        await DbContext.SaveChangesAsync();

        return RedirectToPage("GameView", new { game.Slug, game.Id });

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

    private GameRequest ConvertToGameRequest(List<PlayerInput> playerList)
    {
        return new GameRequest
        {
            Players = playerList.Select(p => p.Name).ToArray(),
            FactionsPerPlayer = FactionsPerPlayer,
        };
    }

    private async Task<Player> ResolvePlayer(string playerName, UserIdentification? joinrpgUserId)
    {
        if (joinrpgUserId is not null)
        {
            var playerByJoinrpgUserId = await DbContext.Players.FirstOrDefaultAsync(p => p.JoinrpgUserId == joinrpgUserId);
            if (playerByJoinrpgUserId is not null)
            {
                return playerByJoinrpgUserId;
            }
        }

        var playerByName = await DbContext.Players.Where(p => p.Name == playerName).FirstOrDefaultAsync();
        if (playerByName is not null)
        {
            return playerByName;
        }

        return CreatePlayer(playerName, joinrpgUserId);
    }

    private record PlayerInput(string Name, UserIdentification? JoinrpgUserId);

    private PlayerSlot CreatePlayerSlot(Player player, PlayerRandomizeItemResult res)
    {
        return new PlayerSlot()
        {
            Color = res.Color,
            Player = player,
            Slug = SlugGenerator.Generate(20),
            SelectedFaction = null,
            PossibleFactions = res.Factions.ToList(),
            ChoosePlace = res.ChoosePlace,
            Speaker = res.Speaker,
            AlliedWith = res.AlliedWtih,
        };
    }

    private static Player CreatePlayer(string playerName, UserIdentification? joinrpgUserId)
    {
        return new Player()
        {
            Name = playerName,
            IsVisualImpaired = false,
            JoinrpgUserId = joinrpgUserId,
        };
    }
}
