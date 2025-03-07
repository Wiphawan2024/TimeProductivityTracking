using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeProductivityTracking.web.Migrations
{
    /// <inheritdoc />
    public partial class MigrationV4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Productivities_Contractor_ContractorID",
                table: "Productivities");

            migrationBuilder.DropTable(
                name: "Contractor");

            migrationBuilder.DropIndex(
                name: "IX_Productivities_ContractorID",
                table: "Productivities");

            migrationBuilder.DropColumn(
                name: "ContractorID",
                table: "Productivities");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ContractorID",
                table: "Productivities",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Contractor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contractor", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Productivities_ContractorID",
                table: "Productivities",
                column: "ContractorID");

            migrationBuilder.AddForeignKey(
                name: "FK_Productivities_Contractor_ContractorID",
                table: "Productivities",
                column: "ContractorID",
                principalTable: "Contractor",
                principalColumn: "Id");
        }
    }
}
