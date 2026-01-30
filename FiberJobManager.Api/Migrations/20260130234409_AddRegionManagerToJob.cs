using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FiberJobManager.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddRegionManagerToJob : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RegionManager",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_RegionManager",
                table: "Users",
                column: "RegionManager");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_RegionManager",
                table: "Users",
                column: "RegionManager",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_RegionManager",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_RegionManager",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RegionManager",
                table: "Users");
        }
    }
}
