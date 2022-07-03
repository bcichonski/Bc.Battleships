using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bc.Battleships.Persistence.Migrations
{
    public partial class AddListOfFieldsThatHaveBeenShootAt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ship_Game_ComputerPlayerGameId",
                table: "Ship");

            migrationBuilder.RenameColumn(
                name: "ComputerPlayerGameId",
                table: "Ship",
                newName: "OwnerGameId");

            migrationBuilder.RenameIndex(
                name: "IX_Ship_ComputerPlayerGameId",
                table: "Ship",
                newName: "IX_Ship_OwnerGameId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Ship_Game_OwnerGameId",
                table: "Ship",
                column: "OwnerGameId",
                principalTable: "Game",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ship_Game_OwnerGameId",
                table: "Ship");

            migrationBuilder.DropTable(
                name: "Coordinates");

            migrationBuilder.RenameColumn(
                name: "OwnerGameId",
                table: "Ship",
                newName: "ComputerPlayerGameId");

            migrationBuilder.RenameIndex(
                name: "IX_Ship_OwnerGameId",
                table: "Ship",
                newName: "IX_Ship_ComputerPlayerGameId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ship_Game_ComputerPlayerGameId",
                table: "Ship",
                column: "ComputerPlayerGameId",
                principalTable: "Game",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
