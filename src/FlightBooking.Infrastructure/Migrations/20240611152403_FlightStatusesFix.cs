using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlightBookingApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FlightStatusesFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flights_FlightsStatuses_FlightStatusId",
                table: "Flights");

            migrationBuilder.AddForeignKey(
                name: "FK_Flights_FlightsStatuses_FlightStatusId",
                table: "Flights",
                column: "FlightStatusId",
                principalTable: "FlightsStatuses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flights_FlightsStatuses_FlightStatusId",
                table: "Flights");

            migrationBuilder.AddForeignKey(
                name: "FK_Flights_FlightsStatuses_FlightStatusId",
                table: "Flights",
                column: "FlightStatusId",
                principalTable: "FlightsStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
