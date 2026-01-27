using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FiberJobManager.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddRevisionAssignedBy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RevisionAssignedBy",
                table: "Jobs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RevisionAssignedByUserId",
                table: "Jobs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_RevisionAssignedByUserId",
                table: "Jobs",
                column: "RevisionAssignedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_Users_RevisionAssignedByUserId",
                table: "Jobs",
                column: "RevisionAssignedByUserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_Users_RevisionAssignedByUserId",
                table: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_RevisionAssignedByUserId",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "RevisionAssignedBy",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "RevisionAssignedByUserId",
                table: "Jobs");
        }
    }
}
