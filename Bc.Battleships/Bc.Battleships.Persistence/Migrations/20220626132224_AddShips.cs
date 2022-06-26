using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bc.Battleships.Persistence.Migrations
{
    public partial class AddShips : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Player");

            migrationBuilder.AddColumn<Guid>(
                name: "ComputerPlayer_Id",
                table: "Game",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Ship",
                columns: table => new
                {
                    ComputerPlayerGameId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ship", x => new { x.ComputerPlayerGameId, x.Id });
                    table.ForeignKey(
                        name: "FK_Ship_Game_ComputerPlayerGameId",
                        column: x => x.ComputerPlayerGameId,
                        principalTable: "Game",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShipCompartment",
                columns: table => new
                {
                    ShipComputerPlayerGameId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ShipId = table.Column<int>(type: "INTEGER", nullable: false),
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShipCompartment", x => new { x.ShipComputerPlayerGameId, x.ShipId, x.Id });
                    table.ForeignKey(
                        name: "FK_ShipCompartment_Ship_ShipComputerPlayerGameId_ShipId",
                        columns: x => new { x.ShipComputerPlayerGameId, x.ShipId },
                        principalTable: "Ship",
                        principalColumns: new[] { "ComputerPlayerGameId", "Id" },
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShipCompartment");

            migrationBuilder.DropTable(
                name: "Ship");

            migrationBuilder.DropColumn(
                name: "ComputerPlayer_Id",
                table: "Game");

            migrationBuilder.CreateTable(
                name: "Player",
                columns: table => new
                {
                    GameId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Id = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Player", x => new { x.GameId, x.Id });
                    table.ForeignKey(
                        name: "FK_Player_Game_GameId",
                        column: x => x.GameId,
                        principalTable: "Game",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }
    }
}
