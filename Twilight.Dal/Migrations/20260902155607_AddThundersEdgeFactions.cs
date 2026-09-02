using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Twilight.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddThundersEdgeFactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Factions",
                columns: new[] { "Id", "Name", "RussianName", "WikiLink" },
                values: new object[,]
                {
                    { 25, "Last Bastion", "Последний Оплот", "https://twilight-imperium.fandom.com/wiki/Last_Bastion" },
                    { 26, "The Ral Nel Consortium", "Консорциум Рал-Нел", "https://twilight-imperium.fandom.com/wiki/The_Ral_Nel_Consortium" },
                    { 27, "The Crimson Rebellion", "Багровый Мятеж", "https://twilight-imperium.fandom.com/wiki/The_Crimson_Rebellion" },
                    { 28, "The Deepwrought Scholarate", "Школа Витой Бездны", "https://twilight-imperium.fandom.com/wiki/The_Deepwrought_Scholarate" },
                    { 29, "The Firmament / The Obsidian", "Небесная Твердь / Обсидиан", "https://twilight-imperium.fandom.com/wiki/The_Firmament_%2F_The_Obsidian" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Factions",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Factions",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Factions",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Factions",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Factions",
                keyColumn: "Id",
                keyValue: 29);
        }
    }
}
