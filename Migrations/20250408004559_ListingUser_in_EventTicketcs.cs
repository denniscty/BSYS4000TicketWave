using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketWave.Migrations
{
    /// <inheritdoc />
    public partial class ListingUser_in_EventTicketcs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_EventTickets_EventListUserID",
                table: "EventTickets",
                column: "EventListUserID");

            migrationBuilder.AddForeignKey(
                name: "FK_EventTickets_AspNetUsers_EventListUserID",
                table: "EventTickets",
                column: "EventListUserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventTickets_AspNetUsers_EventListUserID",
                table: "EventTickets");

            migrationBuilder.DropIndex(
                name: "IX_EventTickets_EventListUserID",
                table: "EventTickets");
        }
    }
}
