using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrazosApp.DataAccess.Migrations
{
    public partial class AddEstablishmentTypeTableAndSeedRiskTableData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Establishments_RiskCategory_RiskCategoryId",
                table: "Establishments");

            migrationBuilder.DropForeignKey(
                name: "FK_Establishments_Territories_TerritoryId",
                table: "Establishments");

            migrationBuilder.DropIndex(
                name: "IX_Establishments_RiskCategoryId",
                table: "Establishments");

            migrationBuilder.DropIndex(
                name: "IX_Establishments_TerritoryId",
                table: "Establishments");

            migrationBuilder.DropColumn(
                name: "RiskCategoryId",
                table: "Establishments");

            migrationBuilder.DropColumn(
                name: "TerritoryId",
                table: "Establishments");

            migrationBuilder.AddColumn<string>(
                name: "RiskCategory",
                table: "Establishments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Territory",
                table: "Establishments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EstablishmentTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JurisdictionId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Q1Fees = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    Q2Fees = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    Q3Fees = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    Q4Fees = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    FeeCalculation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPorated = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstablishmentTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EstablishmentTypes_Jurisdictions_JurisdictionId",
                        column: x => x.JurisdictionId,
                        principalTable: "Jurisdictions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "RiskCategory",
                columns: new[] { "Id", "Code", "Description", "Name" },
                values: new object[] { 1, "L", "Low (1 Routine Inspection in 12 Months From Activation Date", "Low" });

            migrationBuilder.InsertData(
                table: "RiskCategory",
                columns: new[] { "Id", "Code", "Description", "Name" },
                values: new object[] { 2, "M", "Low (1 Routine Inspection in 6 Months From Activation Date", "Medium" });

            migrationBuilder.InsertData(
                table: "RiskCategory",
                columns: new[] { "Id", "Code", "Description", "Name" },
                values: new object[] { 3, "H", "Low (1 Routine Inspection in 4 Months From Activation Date", "High" });

            migrationBuilder.CreateIndex(
                name: "IX_EstablishmentTypes_JurisdictionId",
                table: "EstablishmentTypes",
                column: "JurisdictionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EstablishmentTypes");

            migrationBuilder.DeleteData(
                table: "RiskCategory",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "RiskCategory",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "RiskCategory",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DropColumn(
                name: "RiskCategory",
                table: "Establishments");

            migrationBuilder.DropColumn(
                name: "Territory",
                table: "Establishments");

            migrationBuilder.AddColumn<int>(
                name: "RiskCategoryId",
                table: "Establishments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TerritoryId",
                table: "Establishments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Establishments_RiskCategoryId",
                table: "Establishments",
                column: "RiskCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Establishments_TerritoryId",
                table: "Establishments",
                column: "TerritoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Establishments_RiskCategory_RiskCategoryId",
                table: "Establishments",
                column: "RiskCategoryId",
                principalTable: "RiskCategory",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Establishments_Territories_TerritoryId",
                table: "Establishments",
                column: "TerritoryId",
                principalTable: "Territories",
                principalColumn: "Id");
        }
    }
}
