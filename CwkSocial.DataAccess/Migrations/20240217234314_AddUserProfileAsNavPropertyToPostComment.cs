using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CwkSocial.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddUserProfileAsNavPropertyToPostComment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_PostComment_UserProfileId",
                table: "PostComment",
                column: "UserProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostComment_UserProfiles_UserProfileId",
                table: "PostComment",
                column: "UserProfileId",
                principalTable: "UserProfiles",
                principalColumn: "UserProfileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostComment_UserProfiles_UserProfileId",
                table: "PostComment");

            migrationBuilder.DropIndex(
                name: "IX_PostComment_UserProfileId",
                table: "PostComment");
        }
    }
}
