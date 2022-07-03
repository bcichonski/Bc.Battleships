using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bc.Battleships.Persistence.Migrations
{
    public partial class FixIdProblemForShotCoordinates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ShotCoordinates",
                table: "ShotCoordinates");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "ShotCoordinates",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShotCoordinates",
                table: "ShotCoordinates",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ShotCoordinates_OwnerGameId",
                table: "ShotCoordinates",
                column: "OwnerGameId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ShotCoordinates",
                table: "ShotCoordinates");

            migrationBuilder.DropIndex(
                name: "IX_ShotCoordinates_OwnerGameId",
                table: "ShotCoordinates");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "ShotCoordinates",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "TEXT");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShotCoordinates",
                table: "ShotCoordinates",
                columns: new[] { "OwnerGameId", "Id" });
        }
    }
}
