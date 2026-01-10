using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FiberJobManager.Api.Migrations
{
    /// <inheritdoc />
    public partial class NewFixFirmaRegion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Region_montage_ort",
                table: "Jobs",
                newName: "Region");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Region",
                table: "Jobs",
                newName: "Region_montage_ort");
        }
    }
}
