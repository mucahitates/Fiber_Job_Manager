using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FiberJobManager.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddRevisionFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "RevisionDate",
                table: "Jobs",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RevisionReason",
                table: "Jobs",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RevisionDate",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "RevisionReason",
                table: "Jobs");
        }
    }
}
