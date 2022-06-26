using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bc.Battleships.Persistence.Migrations
{
    public partial class FixShipsCompartmentsIdProblem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ShipCompartment",
                table: "ShipCompartment");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "ShipCompartment",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShipCompartment",
                table: "ShipCompartment",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ShipCompartment_ShipId",
                table: "ShipCompartment",
                column: "ShipId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ShipCompartment",
                table: "ShipCompartment");

            migrationBuilder.DropIndex(
                name: "IX_ShipCompartment_ShipId",
                table: "ShipCompartment");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "ShipCompartment",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "TEXT");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShipCompartment",
                table: "ShipCompartment",
                columns: new[] { "ShipId", "Id" });
        }
    }
}
