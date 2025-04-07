using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketWave.Migrations
{
    /// <inheritdoc />
    public partial class AddListingTypeAndImageToEventTickets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageFileName",
                table: "EventTickets",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ListingType",
                table: "EventTickets",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageFileName",
                table: "EventTickets");

            migrationBuilder.DropColumn(
                name: "ListingType",
                table: "EventTickets");
        }
    }
}
