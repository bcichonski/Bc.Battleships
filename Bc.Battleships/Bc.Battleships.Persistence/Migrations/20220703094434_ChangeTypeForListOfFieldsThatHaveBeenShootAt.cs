using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bc.Battleships.Persistence.Migrations
{
    public partial class ChangeTypeForListOfFieldsThatHaveBeenShootAt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Coordinates");

            migrationBuilder.CreateTable(
                name: "ShotCoordinates",
                columns: table => new
                {
                    OwnerGameId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Coordinates = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShotCoordinates", x => new { x.OwnerGameId, x.Id });
                    table.ForeignKey(
                        name: "FK_ShotCoordinates_Game_OwnerGameId",
                        column: x => x.OwnerGameId,
                        principalTable: "Game",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShotCoordinates");

            migrationBuilder.CreateTable(
                name: "Coordinates",
                columns: table => new
                {
                    ComputerPlayerGameId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Column = table.Column<int>(type: "INTEGER", nullable: false),
                    Row = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coordinates", x => new { x.ComputerPlayerGameId, x.Id });
                    table.ForeignKey(
                        name: "FK_Coordinates_Game_ComputerPlayerGameId",
                        column: x => x.ComputerPlayerGameId,
                        principalTable: "Game",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }
    }
}
