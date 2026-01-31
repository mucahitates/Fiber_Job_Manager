using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FiberJobManager.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddRegionManagerToJob2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           
            migrationBuilder.AddColumn<int>(
                name: "RegionManager",
                table: "Jobs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_RegionManager",
                table: "Jobs",
                column: "RegionManager");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_Users_RegionManager",
                table: "Jobs",
                column: "RegionManager",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
