using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Brewdogger.Api.Persistence.Migrations
{
    public partial class InitialDatabaseCreation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Beers",
                columns: table => new
                {
                    BeerId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    BeerName = table.Column<string>(nullable: true),
                    BeerStyle = table.Column<int>(nullable: false),
                    Abv = table.Column<double>(nullable: false),
                    Ibu = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Beers", x => x.BeerId);
                });

            migrationBuilder.CreateTable(
                name: "Breweries",
                columns: table => new
                {
                    BreweryId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    BreweryName = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Breweries", x => x.BreweryId);
                });

            migrationBuilder.InsertData(
                table: "Beers",
                columns: new[] { "BeerId", "Abv", "BeerName", "BeerStyle", "Ibu" },
                values: new object[] { 1, 7.2, "Hexagenia", 3, 80 });

            migrationBuilder.InsertData(
                table: "Beers",
                columns: new[] { "BeerId", "Abv", "BeerName", "BeerStyle", "Ibu" },
                values: new object[] { 2, 6.0, "Pliny the Elder", 4, 100 });

            migrationBuilder.InsertData(
                table: "Beers",
                columns: new[] { "BeerId", "Abv", "BeerName", "BeerStyle", "Ibu" },
                values: new object[] { 3, 3.8, "Coors Light", 0, 12 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Beers");

            migrationBuilder.DropTable(
                name: "Breweries");
        }
    }
}
