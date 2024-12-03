using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SOA_CA2.Migrations
{
    /// <inheritdoc />
    public partial class UpdateNotificationModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "User_ID",
                table: "Notification",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "Created_At",
                table: "Notification",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "Notification_ID",
                table: "Notification",
                newName: "NotificationId");

            migrationBuilder.AddColumn<bool>(
                name: "IsRead",
                table: "Notification",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ReferenceId",
                table: "Notification",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Notification",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_UserId",
                table: "Notification",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_Users_UserId",
                table: "Notification",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notification_Users_UserId",
                table: "Notification");

            migrationBuilder.DropIndex(
                name: "IX_Notification_UserId",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "IsRead",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "ReferenceId",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Notification");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Notification",
                newName: "User_ID");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Notification",
                newName: "Created_At");

            migrationBuilder.RenameColumn(
                name: "NotificationId",
                table: "Notification",
                newName: "Notification_ID");
        }
    }
}
