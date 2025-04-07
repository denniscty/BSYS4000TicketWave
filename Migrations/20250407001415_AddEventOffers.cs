using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketWave.Migrations
{
    /// <inheritdoc />
    public partial class AddEventOffers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EventOffers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OfferedByUserId = table.Column<string>(type: "TEXT", nullable: false),
                    EventId = table.Column<int>(type: "INTEGER", nullable: false),
                    OfferDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventOffers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventOffers_EventTickets_EventId",
                        column: x => x.EventId,
                        principalTable: "EventTickets",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventOffers_EventId",
                table: "EventOffers",
                column: "EventId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventOffers");
        }
    }
}
