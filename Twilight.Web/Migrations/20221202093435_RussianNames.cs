using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Twilight.Web.Migrations
{
    /// <inheritdoc />
    public partial class RussianNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Factions",
                keyColumn: "Id",
                keyValue: 1,
                column: "RussianName",
                value: "Арбореки");

            migrationBuilder.UpdateData(
                table: "Factions",
                keyColumn: "Id",
                keyValue: 2,
                column: "RussianName",
                value: "Баронат Летнев");

            migrationBuilder.UpdateData(
                table: "Factions",
                keyColumn: "Id",
                keyValue: 3,
                column: "RussianName",
                value: "Клан Сааров");

            migrationBuilder.UpdateData(
                table: "Factions",
                keyColumn: "Id",
                keyValue: 5,
                column: "RussianName",
                value: "Хаканские Эмираты");

            migrationBuilder.UpdateData(
                table: "Factions",
                keyColumn: "Id",
                keyValue: 6,
                column: "RussianName",
                value: "Федерация Сол");

            migrationBuilder.UpdateData(
                table: "Factions",
                keyColumn: "Id",
                keyValue: 8,
                column: "RussianName",
                value: "Психосеть Л131КС");

            migrationBuilder.UpdateData(
                table: "Factions",
                keyColumn: "Id",
                keyValue: 9,
                column: "RussianName",
                value: "Коалиция Ментака");

            migrationBuilder.UpdateData(
                table: "Factions",
                keyColumn: "Id",
                keyValue: 10,
                column: "RussianName",
                value: "Клубок Наалу");

            migrationBuilder.UpdateData(
                table: "Factions",
                keyColumn: "Id",
                keyValue: 12,
                column: "RussianName",
                value: "Сардак Н'орр");

            migrationBuilder.UpdateData(
                table: "Factions",
                keyColumn: "Id",
                keyValue: 13,
                column: "RussianName",
                value: "Жол-Нарские университеты");

            migrationBuilder.UpdateData(
                table: "Factions",
                keyColumn: "Id",
                keyValue: 14,
                column: "RussianName",
                value: "Винну");

            migrationBuilder.UpdateData(
                table: "Factions",
                keyColumn: "Id",
                keyValue: 15,
                column: "RussianName",
                value: "Королевство Ззча");

            migrationBuilder.UpdateData(
                table: "Factions",
                keyColumn: "Id",
                keyValue: 16,
                column: "RussianName",
                value: "Братство Инь");

            migrationBuilder.UpdateData(
                table: "Factions",
                keyColumn: "Id",
                keyValue: 17,
                column: "RussianName",
                value: "Племена Иссарилов");

            migrationBuilder.UpdateData(
                table: "Factions",
                keyColumn: "Id",
                keyValue: 18,
                column: "RussianName",
                value: "Серебряная стая");

            migrationBuilder.UpdateData(
                table: "Factions",
                keyColumn: "Id",
                keyValue: 19,
                column: "RussianName",
                value: "Возвышенные");

            migrationBuilder.UpdateData(
                table: "Factions",
                keyColumn: "Id",
                keyValue: 20,
                column: "RussianName",
                value: "Генные чародеи Мэхакт");

            migrationBuilder.UpdateData(
                table: "Factions",
                keyColumn: "Id",
                keyValue: 21,
                column: "RussianName",
                value: "Альянс Нааз-Роха");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Factions",
                keyColumn: "Id",
                keyValue: 1,
                column: "RussianName",
                value: null);

            migrationBuilder.UpdateData(
                table: "Factions",
                keyColumn: "Id",
                keyValue: 2,
                column: "RussianName",
                value: "Баронство Летнев");

            migrationBuilder.UpdateData(
                table: "Factions",
                keyColumn: "Id",
                keyValue: 3,
                column: "RussianName",
                value: null);

            migrationBuilder.UpdateData(
                table: "Factions",
                keyColumn: "Id",
                keyValue: 5,
                column: "RussianName",
                value: null);

            migrationBuilder.UpdateData(
                table: "Factions",
                keyColumn: "Id",
                keyValue: 6,
                column: "RussianName",
                value: null);

            migrationBuilder.UpdateData(
                table: "Factions",
                keyColumn: "Id",
                keyValue: 8,
                column: "RussianName",
                value: null);

            migrationBuilder.UpdateData(
                table: "Factions",
                keyColumn: "Id",
                keyValue: 9,
                column: "RussianName",
                value: null);

            migrationBuilder.UpdateData(
                table: "Factions",
                keyColumn: "Id",
                keyValue: 10,
                column: "RussianName",
                value: null);

            migrationBuilder.UpdateData(
                table: "Factions",
                keyColumn: "Id",
                keyValue: 12,
                column: "RussianName",
                value: null);

            migrationBuilder.UpdateData(
                table: "Factions",
                keyColumn: "Id",
                keyValue: 13,
                column: "RussianName",
                value: "Университеты Жол-Нара");

            migrationBuilder.UpdateData(
                table: "Factions",
                keyColumn: "Id",
                keyValue: 14,
                column: "RussianName",
                value: null);

            migrationBuilder.UpdateData(
                table: "Factions",
                keyColumn: "Id",
                keyValue: 15,
                column: "RussianName",
                value: null);

            migrationBuilder.UpdateData(
                table: "Factions",
                keyColumn: "Id",
                keyValue: 16,
                column: "RussianName",
                value: null);

            migrationBuilder.UpdateData(
                table: "Factions",
                keyColumn: "Id",
                keyValue: 17,
                column: "RussianName",
                value: null);

            migrationBuilder.UpdateData(
                table: "Factions",
                keyColumn: "Id",
                keyValue: 18,
                column: "RussianName",
                value: null);

            migrationBuilder.UpdateData(
                table: "Factions",
                keyColumn: "Id",
                keyValue: 19,
                column: "RussianName",
                value: null);

            migrationBuilder.UpdateData(
                table: "Factions",
                keyColumn: "Id",
                keyValue: 20,
                column: "RussianName",
                value: null);

            migrationBuilder.UpdateData(
                table: "Factions",
                keyColumn: "Id",
                keyValue: 21,
                column: "RussianName",
                value: null);
        }
    }
}
