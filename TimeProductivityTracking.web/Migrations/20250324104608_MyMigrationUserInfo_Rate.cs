using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeProductivityTracking.web.Migrations
{
    /// <inheritdoc />
    public partial class MyMigrationUserInfo_Rate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_UserInfo_RateID",
                table: "UserInfo",
                column: "RateID");

            migrationBuilder.AddForeignKey(
                name: "FK_UserInfo_Rate_RateID",
                table: "UserInfo",
                column: "RateID",
                principalTable: "Rate",
                principalColumn: "RateID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserInfo_Rate_RateID",
                table: "UserInfo");

            migrationBuilder.DropIndex(
                name: "IX_UserInfo_RateID",
                table: "UserInfo");
        }
    }
}
