using Microsoft.EntityFrameworkCore.Migrations;

namespace Brewdogger.Api.Persistence.Migrations
{
    public partial class ConfigureRelationships : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Beers_BeerStyle",
                table: "Beers",
                column: "BeerStyle");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Beers_BeerStyle",
                table: "Beers");
        }
    }
}
