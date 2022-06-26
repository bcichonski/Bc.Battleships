using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bc.Battleships.Persistence.Migrations
{
    public partial class AddShipsCompartmentsFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Coordinates",
                table: "ShipCompartment",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "ShipCompartment",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Coordinates",
                table: "Ship",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Layout",
                table: "Ship",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Length",
                table: "Ship",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Coordinates",
                table: "ShipCompartment");

            migrationBuilder.DropColumn(
                name: "State",
                table: "ShipCompartment");

            migrationBuilder.DropColumn(
                name: "Coordinates",
                table: "Ship");

            migrationBuilder.DropColumn(
                name: "Layout",
                table: "Ship");

            migrationBuilder.DropColumn(
                name: "Length",
                table: "Ship");
        }
    }
}
