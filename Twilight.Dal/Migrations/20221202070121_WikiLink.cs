using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Twilight.Web.Migrations
{
    /// <inheritdoc />
    public partial class WikiLink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WikiLink",
                table: "Factions",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Factions",
                keyColumn: "Id",
                keyValue: 1,
                column: "WikiLink",
                value: "https://twilight-imperium.fandom.com/wiki/The_Arborec");

            migrationBuilder.UpdateData(
                table: "Factions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "RussianName", "WikiLink" },
                values: new object[] { "Баронство Летнев", "https://twilight-imperium.fandom.com/wiki/The_Barony_of_Letnev" });

            migrationBuilder.UpdateData(
                table: "Factions",
                keyColumn: "Id",
                keyValue: 3,
                column: "WikiLink",
                value: "https://twilight-imperium.fandom.com/wiki/The_Clan_of_Saar");

            migrationBuilder.UpdateData(
                table: "Factions",
                keyColumn: "Id",
                keyValue: 4,
                column: "WikiLink",
                value: "https://twilight-imperium.fandom.com/wiki/The_Embers_of_Muaat");

            migrationBuilder.UpdateData(
                table: "Factions",
                keyColumn: "Id",
                keyValue: 5,
                column: "WikiLink",
                value: "https://twilight-imperium.fandom.com/wiki/The_Emirates_of_Hacan");

            migrationBuilder.UpdateData(
                table: "Factions",
                keyColumn: "Id",
                keyValue: 6,
                column: "WikiLink",
                value: "https://twilight-imperium.fandom.com/wiki/The_Federation_of_Sol");

            migrationBuilder.UpdateData(
                table: "Factions",
                keyColumn: "Id",
                keyValue: 7,
                column: "WikiLink",
                value: "https://twilight-imperium.fandom.com/wiki/The_Ghosts_of_Creuss");

            migrationBuilder.UpdateData(
                table: "Factions",
                keyColumn: "Id",
                keyValue: 8,
                column: "WikiLink",
                value: "https://twilight-imperium.fandom.com/wiki/The_Mentak_Coalition");

            migrationBuilder.UpdateData(
                table: "Factions",
                keyColumn: "Id",
                keyValue: 9,
                column: "WikiLink",
                value: "https://twilight-imperium.fandom.com/wiki/The_Mentak_Coalition");

            migrationBuilder.UpdateData(
                table: "Factions",
                keyColumn: "Id",
                keyValue: 10,
                column: "WikiLink",
                value: "https://twilight-imperium.fandom.com/wiki/The_Naalu_Collective");

            migrationBuilder.UpdateData(
                table: "Factions",
                keyColumn: "Id",
                keyValue: 11,
                column: "WikiLink",
                value: "https://twilight-imperium.fandom.com/wiki/The_Nekro_Virus");

            migrationBuilder.UpdateData(
                table: "Factions",
                keyColumn: "Id",
                keyValue: 12,
                column: "WikiLink",
                value: "https://twilight-imperium.fandom.com/wiki/Sardakk_N%27orr");

            migrationBuilder.UpdateData(
                table: "Factions",
                keyColumn: "Id",
                keyValue: 13,
                column: "WikiLink",
                value: "https://twilight-imperium.fandom.com/wiki/The_Universities_of_Jol-Nar");

            migrationBuilder.UpdateData(
                table: "Factions",
                keyColumn: "Id",
                keyValue: 14,
                column: "WikiLink",
                value: "https://twilight-imperium.fandom.com/wiki/The_Winnu");

            migrationBuilder.UpdateData(
                table: "Factions",
                keyColumn: "Id",
                keyValue: 15,
                column: "WikiLink",
                value: "https://twilight-imperium.fandom.com/wiki/The_Xxcha_Kingdom");

            migrationBuilder.UpdateData(
                table: "Factions",
                keyColumn: "Id",
                keyValue: 16,
                column: "WikiLink",
                value: "https://twilight-imperium.fandom.com/wiki/The_Yin_Brotherhood");

            migrationBuilder.UpdateData(
                table: "Factions",
                keyColumn: "Id",
                keyValue: 17,
                column: "WikiLink",
                value: "https://twilight-imperium.fandom.com/wiki/The_Yssaril_Tribes");

            migrationBuilder.UpdateData(
                table: "Factions",
                keyColumn: "Id",
                keyValue: 18,
                column: "WikiLink",
                value: "https://twilight-imperium.fandom.com/wiki/The_Argent_Flight");

            migrationBuilder.UpdateData(
                table: "Factions",
                keyColumn: "Id",
                keyValue: 19,
                column: "WikiLink",
                value: "https://twilight-imperium.fandom.com/wiki/The_Empyrean");

            migrationBuilder.UpdateData(
                table: "Factions",
                keyColumn: "Id",
                keyValue: 20,
                column: "WikiLink",
                value: "https://twilight-imperium.fandom.com/wiki/The_Mahact_Gene-Sorcerers");

            migrationBuilder.UpdateData(
                table: "Factions",
                keyColumn: "Id",
                keyValue: 21,
                column: "WikiLink",
                value: "https://twilight-imperium.fandom.com/wiki/The_Naaz-Rokha_Alliance");

            migrationBuilder.UpdateData(
                table: "Factions",
                keyColumn: "Id",
                keyValue: 22,
                column: "WikiLink",
                value: "https://twilight-imperium.fandom.com/wiki/The_Nomad");

            migrationBuilder.UpdateData(
                table: "Factions",
                keyColumn: "Id",
                keyValue: 23,
                column: "WikiLink",
                value: "https://twilight-imperium.fandom.com/wiki/The_Titans_of_Ul");

            migrationBuilder.UpdateData(
                table: "Factions",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "RussianName", "WikiLink" },
                values: new object[] { "Кабал вуил’райт", "https://twilight-imperium.fandom.com/wiki/The_Vuil%27Raith_Cabal" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WikiLink",
                table: "Factions");

            migrationBuilder.UpdateData(
                table: "Factions",
                keyColumn: "Id",
                keyValue: 2,
                column: "RussianName",
                value: null);

            migrationBuilder.UpdateData(
                table: "Factions",
                keyColumn: "Id",
                keyValue: 24,
                column: "RussianName",
                value: "Кабал вуил’райт)");
        }
    }
}
