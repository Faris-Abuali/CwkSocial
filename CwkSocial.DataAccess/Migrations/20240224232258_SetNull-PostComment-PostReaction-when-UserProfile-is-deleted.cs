using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CwkSocial.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SetNullPostCommentPostReactionwhenUserProfileisdeleted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostComment_UserProfiles_UserProfileId",
                table: "PostComment");

            migrationBuilder.DropForeignKey(
                name: "FK_PostReaction_UserProfiles_UserProfileId",
                table: "PostReaction");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserProfileId",
                table: "PostReaction",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserProfileId",
                table: "PostComment",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_PostComment_UserProfiles_UserProfileId",
                table: "PostComment",
                column: "UserProfileId",
                principalTable: "UserProfiles",
                principalColumn: "UserProfileId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_PostReaction_UserProfiles_UserProfileId",
                table: "PostReaction",
                column: "UserProfileId",
                principalTable: "UserProfiles",
                principalColumn: "UserProfileId",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostComment_UserProfiles_UserProfileId",
                table: "PostComment");

            migrationBuilder.DropForeignKey(
                name: "FK_PostReaction_UserProfiles_UserProfileId",
                table: "PostReaction");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserProfileId",
                table: "PostReaction",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UserProfileId",
                table: "PostComment",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PostComment_UserProfiles_UserProfileId",
                table: "PostComment",
                column: "UserProfileId",
                principalTable: "UserProfiles",
                principalColumn: "UserProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostReaction_UserProfiles_UserProfileId",
                table: "PostReaction",
                column: "UserProfileId",
                principalTable: "UserProfiles",
                principalColumn: "UserProfileId");
        }
    }
}
