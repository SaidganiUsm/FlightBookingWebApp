using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlightBookingApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FlightStatuses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FlightStatusId",
                table: "Flights",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "FlightsStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlightsStatuses", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Flights_FlightStatusId",
                table: "Flights",
                column: "FlightStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Flights_FlightsStatuses_FlightStatusId",
                table: "Flights",
                column: "FlightStatusId",
                principalTable: "FlightsStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flights_FlightsStatuses_FlightStatusId",
                table: "Flights");

            migrationBuilder.DropTable(
                name: "FlightsStatuses");

            migrationBuilder.DropIndex(
                name: "IX_Flights_FlightStatusId",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "FlightStatusId",
                table: "Flights");
        }
    }
}
