using Microsoft.EntityFrameworkCore.Migrations;

namespace Brewdogger.Api.Persistence.Migrations
{
    public partial class SeedBreweries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Breweries",
                columns: new[] { "BreweryId", "BreweryName", "City", "State" },
                values: new object[] { 1, "Fall River Brewery", "Redding", "CA" });

            migrationBuilder.InsertData(
                table: "Breweries",
                columns: new[] { "BreweryId", "BreweryName", "City", "State" },
                values: new object[] { 2, "Sierra Nevada Brewing Company", "Chico", "CA" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Breweries",
                keyColumn: "BreweryId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Breweries",
                keyColumn: "BreweryId",
                keyValue: 2);
        }
    }
}
