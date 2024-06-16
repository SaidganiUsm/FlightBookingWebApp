using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlightBookingApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FlightTableUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RankPriceRatio",
                table: "Rank",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BusinessTicketsAmount",
                table: "Flights",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EconomyTicketsAmount",
                table: "Flights",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FirstClassTicketsAmount",
                table: "Flights",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StandartPriceForFlight",
                table: "Flights",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RankPriceRatio",
                table: "Rank");

            migrationBuilder.DropColumn(
                name: "BusinessTicketsAmount",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "EconomyTicketsAmount",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "FirstClassTicketsAmount",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "StandartPriceForFlight",
                table: "Flights");
        }
    }
}
