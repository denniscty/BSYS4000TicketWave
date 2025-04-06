using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketWave.Migrations
{
    /// <inheritdoc />
    public partial class PwdSalt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventTickets_Users_EventListUserID",
                table: "EventTickets");

            migrationBuilder.DropIndex(
                name: "IX_EventTickets_EventListUserID",
                table: "EventTickets");

            migrationBuilder.AddColumn<byte[]>(
                name: "PasswordSalt",
                table: "Users",
                type: "BLOB",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AlterColumn<int>(
                name: "EventBuyerID",
                table: "EventTickets",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordSalt",
                table: "Users");

            migrationBuilder.AlterColumn<int>(
                name: "EventBuyerID",
                table: "EventTickets",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EventTickets_EventListUserID",
                table: "EventTickets",
                column: "EventListUserID");

            migrationBuilder.AddForeignKey(
                name: "FK_EventTickets_Users_EventListUserID",
                table: "EventTickets",
                column: "EventListUserID",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
