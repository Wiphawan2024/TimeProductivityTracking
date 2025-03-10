using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeProductivityTracking.web.Migrations
{
    /// <inheritdoc />
    public partial class MyMigrationProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Monthly",
                table: "Productivities",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Productivities",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "Productivities");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Monthly",
                table: "Productivities",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
