using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeProductivityTracking.web.Migrations
{
    /// <inheritdoc />
    public partial class MyMigrationV4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Mentor",
                table: "Productivities");

            migrationBuilder.AlterColumn<int>(
                name: "Task",
                table: "Productivities",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "PlannedDays",
                table: "Productivities",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "County",
                table: "Productivities",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "AchevedDays",
                table: "Productivities",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CounryMentor_A",
                table: "Productivities",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CounryMentor_P",
                table: "Productivities",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Tasks_A",
                table: "Productivities",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AchevedDays",
                table: "Productivities");

            migrationBuilder.DropColumn(
                name: "CounryMentor_A",
                table: "Productivities");

            migrationBuilder.DropColumn(
                name: "CounryMentor_P",
                table: "Productivities");

            migrationBuilder.DropColumn(
                name: "Tasks_A",
                table: "Productivities");

            migrationBuilder.AlterColumn<int>(
                name: "Task",
                table: "Productivities",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PlannedDays",
                table: "Productivities",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "County",
                table: "Productivities",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Mentor",
                table: "Productivities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
