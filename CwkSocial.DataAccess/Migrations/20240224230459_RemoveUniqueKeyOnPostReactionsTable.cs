using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CwkSocial.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUniqueKeyOnPostReactionsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PostReaction_PostId_UserProfileId",
                table: "PostReaction");

            migrationBuilder.CreateIndex(
                name: "IX_PostReaction_PostId",
                table: "PostReaction",
                column: "PostId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PostReaction_PostId",
                table: "PostReaction");

            migrationBuilder.CreateIndex(
                name: "IX_PostReaction_PostId_UserProfileId",
                table: "PostReaction",
                columns: new[] { "PostId", "UserProfileId" },
                unique: true);
        }
    }
}
