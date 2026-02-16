using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Twilight.Web.Migrations
{
    /// <inheritdoc />
    public partial class FixData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
    table: "Factions",
    keyColumn: "Id",
    keyValue: 8,
    column: "WikiLink",
    value: "https://twilight-imperium.fandom.com/wiki/The_L1Z1X_Mindnet");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
