using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bc.Battleships.Persistence.Migrations
{
    public partial class FixShipsIdProblem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShipCompartment_Ship_ShipComputerPlayerGameId_ShipId",
                table: "ShipCompartment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShipCompartment",
                table: "ShipCompartment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ship",
                table: "Ship");

            migrationBuilder.DropColumn(
                name: "ShipComputerPlayerGameId",
                table: "ShipCompartment");

            migrationBuilder.AlterColumn<Guid>(
                name: "ShipId",
                table: "ShipCompartment",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Ship",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShipCompartment",
                table: "ShipCompartment",
                columns: new[] { "ShipId", "Id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ship",
                table: "Ship",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Ship_ComputerPlayerGameId",
                table: "Ship",
                column: "ComputerPlayerGameId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShipCompartment_Ship_ShipId",
                table: "ShipCompartment",
                column: "ShipId",
                principalTable: "Ship",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShipCompartment_Ship_ShipId",
                table: "ShipCompartment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShipCompartment",
                table: "ShipCompartment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ship",
                table: "Ship");

            migrationBuilder.DropIndex(
                name: "IX_Ship_ComputerPlayerGameId",
                table: "Ship");

            migrationBuilder.AlterColumn<int>(
                name: "ShipId",
                table: "ShipCompartment",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "TEXT");

            migrationBuilder.AddColumn<Guid>(
                name: "ShipComputerPlayerGameId",
                table: "ShipCompartment",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Ship",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "TEXT");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShipCompartment",
                table: "ShipCompartment",
                columns: new[] { "ShipComputerPlayerGameId", "ShipId", "Id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ship",
                table: "Ship",
                columns: new[] { "ComputerPlayerGameId", "Id" });

            migrationBuilder.AddForeignKey(
                name: "FK_ShipCompartment_Ship_ShipComputerPlayerGameId_ShipId",
                table: "ShipCompartment",
                columns: new[] { "ShipComputerPlayerGameId", "ShipId" },
                principalTable: "Ship",
                principalColumns: new[] { "ComputerPlayerGameId", "Id" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
