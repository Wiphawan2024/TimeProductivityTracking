using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeProductivityTracking.web.Migrations
{
    /// <inheritdoc />
    public partial class MyMigration2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Productivities_Contractor_contractorSECContractId",
                table: "Productivities");

            migrationBuilder.DropIndex(
                name: "IX_Productivities_contractorSECContractId",
                table: "Productivities");

            migrationBuilder.DropColumn(
                name: "contractorSECContractId",
                table: "Productivities");

            migrationBuilder.AddColumn<int>(
                name: "SECContractId",
                table: "Productivities",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Productivities_SECContractId",
                table: "Productivities",
                column: "SECContractId");

            migrationBuilder.AddForeignKey(
                name: "FK_Productivities_SECContract_SECContractId",
                table: "Productivities",
                column: "SECContractId",
                principalTable: "SECContract",
                principalColumn: "SECContractId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Productivities_SECContract_SECContractId",
                table: "Productivities");

            migrationBuilder.DropIndex(
                name: "IX_Productivities_SECContractId",
                table: "Productivities");

            migrationBuilder.DropColumn(
                name: "SECContractId",
                table: "Productivities");

            migrationBuilder.AddColumn<int>(
                name: "contractorSECContractId",
                table: "Productivities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Productivities_contractorSECContractId",
                table: "Productivities",
                column: "contractorSECContractId");

            migrationBuilder.AddForeignKey(
                name: "FK_Productivities_Contractor_contractorSECContractId",
                table: "Productivities",
                column: "contractorSECContractId",
                principalTable: "Contractor",
                principalColumn: "SECContractId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
