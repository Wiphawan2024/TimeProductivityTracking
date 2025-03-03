using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeProductivityTracking.web.Migrations
{
    /// <inheritdoc />
    public partial class MyMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rate",
                columns: table => new
                {
                    RateID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RateName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HourlyWage = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rate", x => x.RateID);
                });

            migrationBuilder.CreateTable(
                name: "SECContract",
                columns: table => new
                {
                    SECContractId = table.Column<int>(type: "int", nullable: false),
                    SECName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    County = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrimaryContract = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SECContract", x => x.SECContractId);
                });

            migrationBuilder.CreateTable(
                name: "UserInfo",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    HireDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RateID = table.Column<int>(type: "int", nullable: false),
                    Register = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInfo", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Contractor",
                columns: table => new
                {
                    SECContractId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    Monthly = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId_FK = table.Column<int>(type: "int", nullable: false),
                    UserInfoUserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contractor", x => x.SECContractId);
                    table.ForeignKey(
                        name: "FK_Contractor_SECContract_SECContractId",
                        column: x => x.SECContractId,
                        principalTable: "SECContract",
                        principalColumn: "SECContractId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Contractor_UserInfo_UserInfoUserId",
                        column: x => x.UserInfoUserId,
                        principalTable: "UserInfo",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Productivities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Monthly = table.Column<DateTime>(type: "datetime2", nullable: false),
                    County = table.Column<int>(type: "int", nullable: false),
                    PlannedDays = table.Column<int>(type: "int", nullable: false),
                    Task = table.Column<int>(type: "int", nullable: false),
                    Mentor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContractorId_FK = table.Column<int>(type: "int", nullable: false),
                    contractorSECContractId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Productivities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Productivities_Contractor_contractorSECContractId",
                        column: x => x.contractorSECContractId,
                        principalTable: "Contractor",
                        principalColumn: "SECContractId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contractor_UserInfoUserId",
                table: "Contractor",
                column: "UserInfoUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Productivities_contractorSECContractId",
                table: "Productivities",
                column: "contractorSECContractId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Productivities");

            migrationBuilder.DropTable(
                name: "Rate");

            migrationBuilder.DropTable(
                name: "Contractor");

            migrationBuilder.DropTable(
                name: "SECContract");

            migrationBuilder.DropTable(
                name: "UserInfo");
        }
    }
}
