using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketWave.Migrations
{
    /// <inheritdoc />
    public partial class AddOfferStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "EventOffers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_EventOffers_OfferedByUserId",
                table: "EventOffers",
                column: "OfferedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_EventOffers_AspNetUsers_OfferedByUserId",
                table: "EventOffers",
                column: "OfferedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventOffers_AspNetUsers_OfferedByUserId",
                table: "EventOffers");

            migrationBuilder.DropIndex(
                name: "IX_EventOffers_OfferedByUserId",
                table: "EventOffers");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "EventOffers");
        }
    }
}
