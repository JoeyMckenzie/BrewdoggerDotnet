using Microsoft.EntityFrameworkCore.Migrations;

namespace Brewdogger.Api.Persistence.Migrations
{
    public partial class AddBreweriesToBeers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BreweryId",
                table: "Beers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Beers",
                keyColumn: "BeerId",
                keyValue: 1,
                column: "BreweryId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Beers",
                keyColumn: "BeerId",
                keyValue: 2,
                columns: new[] { "Abv", "BeerName", "BreweryId", "Ibu" },
                values: new object[] { 9.2, "Widowmaker", 1, 120 });

            migrationBuilder.UpdateData(
                table: "Beers",
                keyColumn: "BeerId",
                keyValue: 3,
                columns: new[] { "Abv", "BeerName", "BeerStyle", "BreweryId", "Ibu" },
                values: new object[] { 5.5, "Sierra Nevada Pale Ale", "PaleAle", 2, 75 });

            migrationBuilder.CreateIndex(
                name: "IX_Beers_BreweryId",
                table: "Beers",
                column: "BreweryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Beers_Breweries_BreweryId",
                table: "Beers",
                column: "BreweryId",
                principalTable: "Breweries",
                principalColumn: "BreweryId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Beers_Breweries_BreweryId",
                table: "Beers");

            migrationBuilder.DropIndex(
                name: "IX_Beers_BreweryId",
                table: "Beers");

            migrationBuilder.DropColumn(
                name: "BreweryId",
                table: "Beers");

            migrationBuilder.UpdateData(
                table: "Beers",
                keyColumn: "BeerId",
                keyValue: 2,
                columns: new[] { "Abv", "BeerName", "Ibu" },
                values: new object[] { 6.0, "Pliny the Elder", 100 });

            migrationBuilder.UpdateData(
                table: "Beers",
                keyColumn: "BeerId",
                keyValue: 3,
                columns: new[] { "Abv", "BeerName", "BeerStyle", "Ibu" },
                values: new object[] { 3.8, "Coors Light", "Lager", 12 });
        }
    }
}
