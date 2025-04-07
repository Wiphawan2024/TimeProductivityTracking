using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeProductivityTracking.web.Migrations.Productivities
{
    /// <inheritdoc />
    public partial class UpdateIcollectProductivities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserInfoUserId",
                table: "Productivities",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Productivities_UserInfoUserId",
                table: "Productivities",
                column: "UserInfoUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Productivities_UserInfo_UserInfoUserId",
                table: "Productivities",
                column: "UserInfoUserId",
                principalTable: "UserInfo",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Productivities_UserInfo_UserInfoUserId",
                table: "Productivities");

            migrationBuilder.DropIndex(
                name: "IX_Productivities_UserInfoUserId",
                table: "Productivities");

            migrationBuilder.DropColumn(
                name: "UserInfoUserId",
                table: "Productivities");
        }
    }
}
