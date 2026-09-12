using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Twilight.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddCouncilKeleresFaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Factions",
                columns: new[] { "Id", "Name", "RussianName", "WikiLink" },
                values: new object[] { 30, "The Council Keleres", "Совет Келерес", "https://twilight-imperium.fandom.com/wiki/The_Council_Keleres" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Factions",
                keyColumn: "Id",
                keyValue: 30);
        }
    }
}
