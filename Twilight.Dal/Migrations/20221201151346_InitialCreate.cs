using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Twilight.Web.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:player_color", "black,yellow,violet,green,blue,orange,red,pink");

            migrationBuilder.CreateTable(
                name: "Faction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    RussianName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Faction", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Slug = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    IsVisualImpaired = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlayerSlot",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GameId = table.Column<int>(type: "integer", nullable: false),
                    PlayerId = table.Column<int>(type: "integer", nullable: false),
                    Color = table.Column<int>(type: "integer", nullable: false),
                    SelectedFactionId = table.Column<int>(type: "integer", nullable: true),
                    Slug = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerSlot", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerSlot_Faction_SelectedFactionId",
                        column: x => x.SelectedFactionId,
                        principalTable: "Faction",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PlayerSlot_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerSlot_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FactionPlayerSlot",
                columns: table => new
                {
                    PlayerSlotId = table.Column<int>(type: "integer", nullable: false),
                    PossibleFactionsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FactionPlayerSlot", x => new { x.PlayerSlotId, x.PossibleFactionsId });
                    table.ForeignKey(
                        name: "FK_FactionPlayerSlot_Faction_PossibleFactionsId",
                        column: x => x.PossibleFactionsId,
                        principalTable: "Faction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FactionPlayerSlot_PlayerSlot_PlayerSlotId",
                        column: x => x.PlayerSlotId,
                        principalTable: "PlayerSlot",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Faction",
                columns: new[] { "Id", "Name", "RussianName" },
                values: new object[,]
                {
                    { 1, "The Arborec", null },
                    { 2, "The Barony of Letnev", null },
                    { 3, "The Clan of Saar", null },
                    { 4, "The Embers of Muaat", "Тлеющие с Муаата" },
                    { 5, "The Emirates of Hacan", null },
                    { 6, "The Federation of Sol", null },
                    { 7, "The Ghosts of Creuss", "Призраки Креусса" },
                    { 8, "The L1Z1X Mindnet", null },
                    { 9, "The Mentak Coalition", null },
                    { 10, "The Naalu Collective", null },
                    { 11, "The Nekro Virus", "Некровирус" },
                    { 12, "Sardakk N’orr", null },
                    { 13, "The Universities of Jol-Nar", "Университеты Жол-Нара" },
                    { 14, "The Winnu", null },
                    { 15, "The Xxcha Kingdom", null },
                    { 16, "The Yin Brotherhood", null },
                    { 17, "The Yssaril Tribes", null },
                    { 18, "The Argent Flight", null },
                    { 19, "The Empyrean", null },
                    { 20, "The Mahact Gene-Sorcerers", null },
                    { 21, "The Naaz-Rokha Alliance", null },
                    { 22, "The Nomad", "Кочевник" },
                    { 23, "The Titans of Ul", "Титаны Ула" },
                    { 24, "The Vuil'Raith Cabal", "Кабал вуил’райт)" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_FactionPlayerSlot_PossibleFactionsId",
                table: "FactionPlayerSlot",
                column: "PossibleFactionsId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerSlot_GameId",
                table: "PlayerSlot",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerSlot_PlayerId",
                table: "PlayerSlot",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerSlot_SelectedFactionId",
                table: "PlayerSlot",
                column: "SelectedFactionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FactionPlayerSlot");

            migrationBuilder.DropTable(
                name: "PlayerSlot");

            migrationBuilder.DropTable(
                name: "Faction");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "Players");
        }
    }
}
