using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Twilight.Domain;
using TwilightRandom;

namespace Twilight.Web.Pages;

public class GameCreateModel : PageModel
{
    public GameCreateModel(DbContext dbContext, GameRepository gameRepository)
    {
        DbContext = dbContext;
        GameRepository = gameRepository;
    }

    private DbContext DbContext { get; }
    private GameRepository GameRepository { get; }

    [Required]
    [BindProperty]
    public string Name { get; set; } = "";
    [Required, DataType(DataType.MultilineText)]
    [BindProperty]
    public string PlayerList { get; set; } = "";
    
    [BindProperty]
    public bool AddToEightPlayers { get; set; } = true;

    public async Task OnGet()
    {
        var lastGame = await GameRepository.LoadLastGameOrDefault();
        if (lastGame is not null)
        {
            PlayerList = string.Join('\n', lastGame.PlayerSlots.Select(ps => ps.Player.Name));
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        var game = new Game
        {
            Name = Name,
            Slug = SlugGenerator.Generate(20)
        };

        var gameRequest = ConvertToGameRequest();

        var randomizer = new Randomiser(gameRequest, await DbContext.Factions.ToListAsync());
        var result = randomizer.Randomize();

        foreach (var res in result.Players)
        {
            var playerName = res.PlayerName;

            var playerFromDb = await DbContext.Players.Where(p => p.Name == playerName).FirstOrDefaultAsync();
            var player = playerFromDb ?? CreatePlayer(playerName);

            game.PlayerSlots.Add(CreatePlayerSlot(player, res));
        }

        DbContext.Games.Add(game);

        await DbContext.SaveChangesAsync();

        return RedirectToPage("GameView", new { game.Slug });

    }

    private GameRequest ConvertToGameRequest()
    {
        var playerList = PlayerList.Split("\n", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();

        if (AddToEightPlayers)
        {
            while (playerList.Count < 8)
            {
                playerList.Add($"Запасной игрок {playerList.Count + 1}");
            }
        }

        return new GameRequest
        {
            Players = playerList.ToArray(),
        };
    }

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
        };
    }

    private Player CreatePlayer(string playerName)
    {
        return new Player()
        {
            Name = playerName,
            IsVisualImpaired = false,
        };
    }
}
