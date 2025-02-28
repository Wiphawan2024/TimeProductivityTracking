using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeProductivityTracking.web.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Register",
                table: "UserInfo",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Register",
                table: "UserInfo");
        }
    }
}
