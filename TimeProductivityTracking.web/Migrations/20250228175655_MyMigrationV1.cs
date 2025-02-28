using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeProductivityTracking.web.Migrations
{
    /// <inheritdoc />
    public partial class MyMigrationV1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Productivities",
                columns: table => new
                {
                    ProductivitiesId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date_Planned = table.Column<DateOnly>(type: "date", nullable: false),
                    SEC_Registered = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EMP_Application = table.Column<int>(type: "int", nullable: false),
                    EMP_Planned = table.Column<int>(type: "int", nullable: false),
                    ActiveProject = table.Column<int>(type: "int", nullable: false),
                    YearToDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PlannedDays = table.Column<int>(type: "int", nullable: false),
                    Tasks_TBC_planned = table.Column<int>(type: "int", nullable: false),
                    Date_Achieved = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Achieved_days = table.Column<int>(type: "int", nullable: false),
                    Tasks_TBC_ach = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    SECContractId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Productivities", x => x.ProductivitiesId);
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
                    RateID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInfo", x => x.UserId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Productivities");

            migrationBuilder.DropTable(
                name: "SECContract");

            migrationBuilder.DropTable(
                name: "UserInfo");
        }
    }
}
