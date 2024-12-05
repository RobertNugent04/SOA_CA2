using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SOA_CA2.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserProfilePhoto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProfilePictureUrl",
                table: "Users",
                newName: "ProfilePicturePath");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProfilePicturePath",
                table: "Users",
                newName: "ProfilePictureUrl");
        }
    }
}
