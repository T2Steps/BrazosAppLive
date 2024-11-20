using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrazosApp.DataAccess.Migrations
{
    public partial class AddInspectionInsItemTablesToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Inspections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EstablishmentId = table.Column<int>(type: "int", nullable: false),
                    RiskId = table.Column<int>(type: "int", nullable: false),
                    PurposeId = table.Column<int>(type: "int", nullable: false),
                    ScheduleId = table.Column<int>(type: "int", nullable: false),
                    InspectionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TimeIn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TimeOut = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FollowUp = table.Column<bool>(type: "bit", nullable: false),
                    FollowUpDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inspections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inspections_Establishments_EstablishmentId",
                        column: x => x.EstablishmentId,
                        principalTable: "Establishments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Inspections_InspectionPurposes_PurposeId",
                        column: x => x.PurposeId,
                        principalTable: "InspectionPurposes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Inspections_RiskCategory_RiskId",
                        column: x => x.RiskId,
                        principalTable: "RiskCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Inspections_Schedules_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "Schedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "InspectionItemDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InspectionId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cos = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    R = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InspectionItemDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InspectionItemDetails_Inspections_InspectionId",
                        column: x => x.InspectionId,
                        principalTable: "Inspections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InspectionItemDetails_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OpeningInspectionData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InspectionId = table.Column<int>(type: "int", nullable: false),
                    PermitApproval = table.Column<bool>(type: "bit", nullable: false),
                    InspectedBy = table.Column<int>(type: "int", nullable: false),
                    InspectedBySign = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InspectorSignFile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PersonInCharge = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PersonInChargeSign = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpeningInspectionData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OpeningInspectionData_Inspections_InspectionId",
                        column: x => x.InspectionId,
                        principalTable: "Inspections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InspectionItemAdjuncTable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InspectionItemId = table.Column<int>(type: "int", nullable: false),
                    ApproveStatus = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InspectionItemAdjuncTable", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InspectionItemAdjuncTable_InspectionItemDetails_InspectionItemId",
                        column: x => x.InspectionItemId,
                        principalTable: "InspectionItemDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InspectionItemAdjuncTable_InspectionItemId",
                table: "InspectionItemAdjuncTable",
                column: "InspectionItemId");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionItemDetails_InspectionId",
                table: "InspectionItemDetails",
                column: "InspectionId");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionItemDetails_ItemId",
                table: "InspectionItemDetails",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Inspections_EstablishmentId",
                table: "Inspections",
                column: "EstablishmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Inspections_PurposeId",
                table: "Inspections",
                column: "PurposeId");

            migrationBuilder.CreateIndex(
                name: "IX_Inspections_RiskId",
                table: "Inspections",
                column: "RiskId");

            migrationBuilder.CreateIndex(
                name: "IX_Inspections_ScheduleId",
                table: "Inspections",
                column: "ScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_OpeningInspectionData_InspectionId",
                table: "OpeningInspectionData",
                column: "InspectionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InspectionItemAdjuncTable");

            migrationBuilder.DropTable(
                name: "OpeningInspectionData");

            migrationBuilder.DropTable(
                name: "InspectionItemDetails");

            migrationBuilder.DropTable(
                name: "Inspections");
        }
    }
}
