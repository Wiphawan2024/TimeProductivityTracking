using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeProductivityTracking.web.Migrations.Productivities
{
    /// <inheritdoc />
    public partial class MyMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlannecNextMonth",
                table: "Productivities");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PlannecNextMonth",
                table: "Productivities",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
