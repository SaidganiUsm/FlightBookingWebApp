using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlightBookingApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TypeFixInTableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flights_FlightsStatuses_FlightStatusId",
                table: "Flights");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FlightsStatuses",
                table: "FlightsStatuses");

            migrationBuilder.RenameTable(
                name: "FlightsStatuses",
                newName: "FlightStatuses");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FlightStatuses",
                table: "FlightStatuses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Flights_FlightStatuses_FlightStatusId",
                table: "Flights",
                column: "FlightStatusId",
                principalTable: "FlightStatuses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flights_FlightStatuses_FlightStatusId",
                table: "Flights");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FlightStatuses",
                table: "FlightStatuses");

            migrationBuilder.RenameTable(
                name: "FlightStatuses",
                newName: "FlightsStatuses");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FlightsStatuses",
                table: "FlightsStatuses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Flights_FlightsStatuses_FlightStatusId",
                table: "Flights",
                column: "FlightStatusId",
                principalTable: "FlightsStatuses",
                principalColumn: "Id");
        }
    }
}
