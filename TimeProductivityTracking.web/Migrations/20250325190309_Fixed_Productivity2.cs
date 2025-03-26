using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeProductivityTracking.web.Migrations
{
    /// <inheritdoc />
    public partial class Fixed_Productivity2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Productivities_UserInfo_ContractorId",
                table: "Productivities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Productivities",
                table: "Productivities");

            migrationBuilder.RenameTable(
                name: "Productivities",
                newName: "Productivity");

            migrationBuilder.RenameIndex(
                name: "IX_Productivities_ContractorId",
                table: "Productivity",
                newName: "IX_Productivity_ContractorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Productivity",
                table: "Productivity",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Productivity_UserInfo_ContractorId",
                table: "Productivity",
                column: "ContractorId",
                principalTable: "UserInfo",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Productivity_UserInfo_ContractorId",
                table: "Productivity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Productivity",
                table: "Productivity");

            migrationBuilder.RenameTable(
                name: "Productivity",
                newName: "Productivities");

            migrationBuilder.RenameIndex(
                name: "IX_Productivity_ContractorId",
                table: "Productivities",
                newName: "IX_Productivities_ContractorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Productivities",
                table: "Productivities",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Productivities_UserInfo_ContractorId",
                table: "Productivities",
                column: "ContractorId",
                principalTable: "UserInfo",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
