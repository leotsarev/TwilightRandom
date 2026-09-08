using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Twilight.Dal;
using Twilight.Domain;
using Twilight.Web.Auth;
using JoinRpg.Common.WebInfrastructure.Auth;

namespace Twilight.Web.Pages
{
    public class GameViewModel : PageModel
    {
        private readonly IGameRepository gameRepository;
        private readonly TwilightDbContext dbContext;

        public GameViewModel(IGameRepository gameRepository, TwilightDbContext dbContext)
        {
            this.gameRepository = gameRepository;
            this.dbContext = dbContext;
        }
        [BindProperty(SupportsGet = true)]
        public int? Id { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SlotSlug { get; set; }

        public int? SlotId { get; set; }

        public Game Game { get; set; } = null!;

        public bool AllSelected { get; set; }

        public bool Alliances { get; set; }

        public bool CanManage { get; set; }

        public bool CanDeleteGame { get; set; }

        public bool CanCancelGame { get; set; }

        public bool ShowAddPlayer { get; set; }

        public string? AddPlayerDisabledReason { get; set; }

        public List<Domain.Faction> UnUsedFactions { get; set; } = new();

        [TempData]
        public string? Message { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (Id is null)
            {
                return NotFound();
            }

            var game = await gameRepository.LoadGameById(Id.Value);

            if (game is null)
            {
                return NotFound();
            }
            Game = game;

            var currentUserId = User.TryGetJoinrpgUserId();

            foreach (var slot in game.PlayerSlots)
            {
                if (slot.Slug == SlotSlug || (currentUserId is not null && slot.Player.JoinrpgUserId == currentUserId))
                {
                    SlotId = slot.Id;
                }
            }

            AllSelected = game.PlayerSlots.All(p => p.SelectedFaction is not null);
            Alliances = game.PlayerSlots.Any(p => p.AlliedWith is not null);
            CanManage = GameAuthorization.CanManage(game, currentUserId);
            CanDeleteGame = CanManage && game.PlayerSlots.All(p => p.SelectedFaction is null);
            CanCancelGame = CanManage
                && game.Status != GameStatus.Played
                && game.Status != GameStatus.Cancelled;

            var possibleFactions = await dbContext.Factions.ToListAsync();

            if (AllSelected || CanManage)
            {
                UnUsedFactions = possibleFactions.ExceptAlreadyUsedIn(game).ToList();
            }
            else
            {
                UnUsedFactions = possibleFactions.ExceptAlreadyUsedInForUser(game, SlotId).ToList();
            }

            ShowAddPlayer = game.Status is GameStatus.ChoosingSides or GameStatus.Planned;
            if (ShowAddPlayer)
            {
                var maxPlayers = Enum.GetValues<PlayerColor>().Length;
                if (!CanManage)
                {
                    AddPlayerDisabledReason = "Добавлять игроков может только создатель игры";
                }
                else if (Alliances)
                {
                    AddPlayerDisabledReason = "Нельзя добавить игрока: в игре включён режим союзов";
                }
                else if (game.PlayerSlots.Count >= maxPlayers)
                {
                    AddPlayerDisabledReason = $"В игре уже максимальное количество игроков ({maxPlayers})";
                }
                else if (possibleFactions.ExceptAlreadyUsedIn(game).Count() < 3)
                {
                    AddPlayerDisabledReason = "Недостаточно свободных фракций (нужно минимум 3)";
                }
            }

            return Page();

        }
    }
}
