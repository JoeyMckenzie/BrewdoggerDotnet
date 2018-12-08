using Microsoft.EntityFrameworkCore.Migrations;

namespace Brewdogger.Api.Persistence.Migrations
{
    public partial class ModifyIndicies : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Beers_BeerStyle",
                table: "Beers");

            migrationBuilder.CreateIndex(
                name: "IX_Breweries_State",
                table: "Breweries",
                column: "State");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Breweries_State",
                table: "Breweries");

            migrationBuilder.CreateIndex(
                name: "IX_Beers_BeerStyle",
                table: "Beers",
                column: "BeerStyle");
        }
    }
}
