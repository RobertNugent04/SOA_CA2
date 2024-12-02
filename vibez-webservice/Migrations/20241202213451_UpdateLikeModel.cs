using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SOA_CA2.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLikeModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "User_ID",
                table: "Likes",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "Post_ID",
                table: "Likes",
                newName: "PostId");

            migrationBuilder.RenameColumn(
                name: "Created_At",
                table: "Likes",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "Like_ID",
                table: "Likes",
                newName: "LikeId");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_PostId",
                table: "Likes",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_UserId",
                table: "Likes",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Posts_PostId",
                table: "Likes",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "PostId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Users_UserId",
                table: "Likes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Posts_PostId",
                table: "Likes");

            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Users_UserId",
                table: "Likes");

            migrationBuilder.DropIndex(
                name: "IX_Likes_PostId",
                table: "Likes");

            migrationBuilder.DropIndex(
                name: "IX_Likes_UserId",
                table: "Likes");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Likes",
                newName: "User_ID");

            migrationBuilder.RenameColumn(
                name: "PostId",
                table: "Likes",
                newName: "Post_ID");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Likes",
                newName: "Created_At");

            migrationBuilder.RenameColumn(
                name: "LikeId",
                table: "Likes",
                newName: "Like_ID");
        }
    }
}
