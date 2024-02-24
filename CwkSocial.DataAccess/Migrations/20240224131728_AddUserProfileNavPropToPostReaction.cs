using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CwkSocial.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddUserProfileNavPropToPostReaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_PostReaction_UserProfileId",
                table: "PostReaction",
                column: "UserProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostReaction_UserProfiles_UserProfileId",
                table: "PostReaction",
                column: "UserProfileId",
                principalTable: "UserProfiles",
                principalColumn: "UserProfileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostReaction_UserProfiles_UserProfileId",
                table: "PostReaction");

            migrationBuilder.DropIndex(
                name: "IX_PostReaction_UserProfileId",
                table: "PostReaction");
        }
    }
}
