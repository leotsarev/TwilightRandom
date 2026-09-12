using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Twilight.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddGameDateAndStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "Date",
                table: "Games",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Games",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql("""
                UPDATE "Games" g
                SET "Status" = 1 -- GameStatus.Planned
                WHERE EXISTS (SELECT 1 FROM "PlayerSlot" ps WHERE ps."GameId" = g."Id")
                  AND NOT EXISTS (
                      SELECT 1 FROM "PlayerSlot" ps
                      WHERE ps."GameId" = g."Id" AND ps."SelectedFactionId" IS NULL
                  );
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Games");
        }
    }
}
