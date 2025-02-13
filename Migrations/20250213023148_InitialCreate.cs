using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketWave.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EventTickets",
                columns: table => new
                {
                    EventId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EventListUserID = table.Column<int>(type: "INTEGER", nullable: false),
                    EventDateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EventName = table.Column<string>(type: "TEXT", nullable: true),
                    EventLocation = table.Column<string>(type: "TEXT", nullable: true),
                    EventTicketPrice = table.Column<decimal>(type: "TEXT", nullable: false),
                    EventNumOfTicket = table.Column<int>(type: "INTEGER", nullable: false),
                    EventUserContactEmail = table.Column<string>(type: "TEXT", nullable: true),
                    EventDescription = table.Column<string>(type: "TEXT", nullable: true),
                    EventBuyerID = table.Column<int>(type: "INTEGER", nullable: false),
                    EventBuyOfferAccepted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventTickets", x => x.EventId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventTickets");
        }
    }
}
