using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FiberJobManager.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddJobFiberFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FirstMeasurement",
                table: "Jobs",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HK",
                table: "Jobs",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "MontageOrt",
                table: "Jobs",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "NVT",
                table: "Jobs",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Region",
                table: "Jobs",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "SM",
                table: "Jobs",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstMeasurement",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "HK",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "MontageOrt",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "NVT",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "Region",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "SM",
                table: "Jobs");
        }
    }
}
