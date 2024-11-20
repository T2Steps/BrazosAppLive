using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrazosApp.DataAccess.Migrations
{
    public partial class AddAgencyStaffReqFieldsTableToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AgencyStaffReqFields",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EstablishmentId = table.Column<int>(type: "int", nullable: false),
                    RiskCategoryId = table.Column<int>(type: "int", nullable: false),
                    TerritoryId = table.Column<int>(type: "int", nullable: false),
                    EstablishmentSizeId = table.Column<int>(type: "int", nullable: false),
                    EstablishmentTypeId = table.Column<int>(type: "int", nullable: false),
                    IsPlanReview = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgencyStaffReqFields", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AgencyStaffReqFields_Establishments_EstablishmentId",
                        column: x => x.EstablishmentId,
                        principalTable: "Establishments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AgencyStaffReqFields_EstablishmentSizes_EstablishmentSizeId",
                        column: x => x.EstablishmentSizeId,
                        principalTable: "EstablishmentSizes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AgencyStaffReqFields_EstablishmentTypes_EstablishmentTypeId",
                        column: x => x.EstablishmentTypeId,
                        principalTable: "EstablishmentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AgencyStaffReqFields_RiskCategory_RiskCategoryId",
                        column: x => x.RiskCategoryId,
                        principalTable: "RiskCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AgencyStaffReqFields_Territories_TerritoryId",
                        column: x => x.TerritoryId,
                        principalTable: "Territories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AgencyStaffReqFields_EstablishmentId",
                table: "AgencyStaffReqFields",
                column: "EstablishmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AgencyStaffReqFields_EstablishmentSizeId",
                table: "AgencyStaffReqFields",
                column: "EstablishmentSizeId");

            migrationBuilder.CreateIndex(
                name: "IX_AgencyStaffReqFields_EstablishmentTypeId",
                table: "AgencyStaffReqFields",
                column: "EstablishmentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AgencyStaffReqFields_RiskCategoryId",
                table: "AgencyStaffReqFields",
                column: "RiskCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_AgencyStaffReqFields_TerritoryId",
                table: "AgencyStaffReqFields",
                column: "TerritoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AgencyStaffReqFields");
        }
    }
}
