using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Twilight.Web.Migrations
{
    /// <inheritdoc />
    public partial class Faction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FactionPlayerSlot_Faction_PossibleFactionsId",
                table: "FactionPlayerSlot");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerSlot_Faction_SelectedFactionId",
                table: "PlayerSlot");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Faction",
                table: "Faction");

            migrationBuilder.RenameTable(
                name: "Faction",
                newName: "Factions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Factions",
                table: "Factions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FactionPlayerSlot_Factions_PossibleFactionsId",
                table: "FactionPlayerSlot",
                column: "PossibleFactionsId",
                principalTable: "Factions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerSlot_Factions_SelectedFactionId",
                table: "PlayerSlot",
                column: "SelectedFactionId",
                principalTable: "Factions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FactionPlayerSlot_Factions_PossibleFactionsId",
                table: "FactionPlayerSlot");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerSlot_Factions_SelectedFactionId",
                table: "PlayerSlot");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Factions",
                table: "Factions");

            migrationBuilder.RenameTable(
                name: "Factions",
                newName: "Faction");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Faction",
                table: "Faction",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FactionPlayerSlot_Faction_PossibleFactionsId",
                table: "FactionPlayerSlot",
                column: "PossibleFactionsId",
                principalTable: "Faction",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerSlot_Faction_SelectedFactionId",
                table: "PlayerSlot",
                column: "SelectedFactionId",
                principalTable: "Faction",
                principalColumn: "Id");
        }
    }
}
