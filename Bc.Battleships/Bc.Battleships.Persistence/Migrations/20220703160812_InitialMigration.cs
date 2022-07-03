using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bc.Battleships.Persistence.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Game",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ComputerPlayer_Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    GameState = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Game", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ship",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    OwnerGameId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Layout = table.Column<int>(type: "INTEGER", nullable: false),
                    Coordinates = table.Column<string>(type: "TEXT", nullable: false),
                    Length = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ship", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ship_Game_OwnerGameId",
                        column: x => x.OwnerGameId,
                        principalTable: "Game",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShotCoordinates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Coordinates = table.Column<string>(type: "TEXT", nullable: false),
                    ComputerPlayerGameId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShotCoordinates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShotCoordinates_Game_ComputerPlayerGameId",
                        column: x => x.ComputerPlayerGameId,
                        principalTable: "Game",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShipCompartment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Coordinates = table.Column<string>(type: "TEXT", nullable: false),
                    State = table.Column<int>(type: "INTEGER", nullable: false),
                    ShipId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShipCompartment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShipCompartment_Ship_ShipId",
                        column: x => x.ShipId,
                        principalTable: "Ship",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Game_GameState",
                table: "Game",
                column: "GameState");

            migrationBuilder.CreateIndex(
                name: "IX_Ship_OwnerGameId",
                table: "Ship",
                column: "OwnerGameId");

            migrationBuilder.CreateIndex(
                name: "IX_ShipCompartment_ShipId",
                table: "ShipCompartment",
                column: "ShipId");

            migrationBuilder.CreateIndex(
                name: "IX_ShotCoordinates_ComputerPlayerGameId",
                table: "ShotCoordinates",
                column: "ComputerPlayerGameId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShipCompartment");

            migrationBuilder.DropTable(
                name: "ShotCoordinates");

            migrationBuilder.DropTable(
                name: "Ship");

            migrationBuilder.DropTable(
                name: "Game");
        }
    }
}
