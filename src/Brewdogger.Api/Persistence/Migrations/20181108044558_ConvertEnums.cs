using Microsoft.EntityFrameworkCore.Migrations;

namespace Brewdogger.Api.Persistence.Migrations
{
    public partial class ConvertEnums : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "BeerStyle",
                table: "Beers",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.UpdateData(
                table: "Beers",
                keyColumn: "BeerId",
                keyValue: 1,
                column: "BeerStyle",
                value: "Ipa");

            migrationBuilder.UpdateData(
                table: "Beers",
                keyColumn: "BeerId",
                keyValue: 2,
                column: "BeerStyle",
                value: "DoubleIpa");

            migrationBuilder.UpdateData(
                table: "Beers",
                keyColumn: "BeerId",
                keyValue: 3,
                column: "BeerStyle",
                value: "Lager");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "BeerStyle",
                table: "Beers",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.UpdateData(
                table: "Beers",
                keyColumn: "BeerId",
                keyValue: 1,
                column: "BeerStyle",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Beers",
                keyColumn: "BeerId",
                keyValue: 2,
                column: "BeerStyle",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Beers",
                keyColumn: "BeerId",
                keyValue: 3,
                column: "BeerStyle",
                value: 0);
        }
    }
}
