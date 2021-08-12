using Microsoft.EntityFrameworkCore.Migrations;

namespace ForumSystem.Data.Migrations
{
    public partial class AddIsApprovePropertyToCategoryModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_ProfileImages_ProfileImageId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_ProfileImages_UserId",
                table: "ProfileImages");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ProfileImageId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<bool>(
                name: "IsApprovedByAdmin",
                table: "Categories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "ProfileImageId",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProfileImages_UserId",
                table: "ProfileImages",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProfileImages_UserId",
                table: "ProfileImages");

            migrationBuilder.DropColumn(
                name: "IsApprovedByAdmin",
                table: "Categories");

            migrationBuilder.AlterColumn<string>(
                name: "ProfileImageId",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProfileImages_UserId",
                table: "ProfileImages",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ProfileImageId",
                table: "AspNetUsers",
                column: "ProfileImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_ProfileImages_ProfileImageId",
                table: "AspNetUsers",
                column: "ProfileImageId",
                principalTable: "ProfileImages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
