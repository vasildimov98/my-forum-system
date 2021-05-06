namespace ForumSystem.Data.Migrations
{
    using System;

    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class ChangeVoteEntityToDeletable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Votes_IsDeleted",
                table: "Votes");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Votes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Votes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Votes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Votes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Votes_IsDeleted",
                table: "Votes",
                column: "IsDeleted");
        }
    }
}
