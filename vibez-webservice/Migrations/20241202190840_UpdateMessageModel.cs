using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SOA_CA2.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMessageModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Sent_At",
                table: "Message",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "Sender_User_ID",
                table: "Message",
                newName: "SenderId");

            migrationBuilder.RenameColumn(
                name: "Receiver_User_ID",
                table: "Message",
                newName: "ReceiverId");

            migrationBuilder.RenameColumn(
                name: "Message_ID",
                table: "Message",
                newName: "MessageId");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeletedByReceiver",
                table: "Message",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeletedBySender",
                table: "Message",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Message_ReceiverId",
                table: "Message",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_SenderId",
                table: "Message",
                column: "SenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Message_Users_ReceiverId",
                table: "Message",
                column: "ReceiverId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Message_Users_SenderId",
                table: "Message",
                column: "SenderId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Message_Users_ReceiverId",
                table: "Message");

            migrationBuilder.DropForeignKey(
                name: "FK_Message_Users_SenderId",
                table: "Message");

            migrationBuilder.DropIndex(
                name: "IX_Message_ReceiverId",
                table: "Message");

            migrationBuilder.DropIndex(
                name: "IX_Message_SenderId",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "IsDeletedByReceiver",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "IsDeletedBySender",
                table: "Message");

            migrationBuilder.RenameColumn(
                name: "SenderId",
                table: "Message",
                newName: "Sender_User_ID");

            migrationBuilder.RenameColumn(
                name: "ReceiverId",
                table: "Message",
                newName: "Receiver_User_ID");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Message",
                newName: "Sent_At");

            migrationBuilder.RenameColumn(
                name: "MessageId",
                table: "Message",
                newName: "Message_ID");
        }
    }
}
