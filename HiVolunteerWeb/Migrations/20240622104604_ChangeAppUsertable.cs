using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HiVolunteerWeb.Migrations
{
    /// <inheritdoc />
    public partial class ChangeAppUsertable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_AspNetUsers_NotificationSendingUserId",
                table: "Notifications");

            migrationBuilder.AlterColumn<string>(
                name: "NotificationSendingUserId",
                table: "Notifications",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_AspNetUsers_NotificationSendingUserId",
                table: "Notifications",
                column: "NotificationSendingUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_AspNetUsers_NotificationSendingUserId",
                table: "Notifications");

            migrationBuilder.AlterColumn<string>(
                name: "NotificationSendingUserId",
                table: "Notifications",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_AspNetUsers_NotificationSendingUserId",
                table: "Notifications",
                column: "NotificationSendingUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
