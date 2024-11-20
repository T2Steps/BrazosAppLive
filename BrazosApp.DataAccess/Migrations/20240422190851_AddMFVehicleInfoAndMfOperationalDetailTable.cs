using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrazosApp.DataAccess.Migrations
{
    public partial class AddMFVehicleInfoAndMfOperationalDetailTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MFOperationDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EstablishmentId = table.Column<int>(type: "int", nullable: false),
                    BusinessTypeId = table.Column<int>(type: "int", nullable: false),
                    OperationTypeId = table.Column<int>(type: "int", nullable: false),
                    CentralProcessingFacility = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Zip = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    MobileOperationLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PublicWaterSouce = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WasteWaterDisposalsite = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Portablewatertanksize = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Wastewatertanksize = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CertifiedFoodManager = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CertificateExpirationDt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MFOperationDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MFOperationDetails_BusinessTypes_BusinessTypeId",
                        column: x => x.BusinessTypeId,
                        principalTable: "BusinessTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MFOperationDetails_Establishments_EstablishmentId",
                        column: x => x.EstablishmentId,
                        principalTable: "Establishments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MFOperationDetails_OperationTypes_OperationTypeId",
                        column: x => x.OperationTypeId,
                        principalTable: "OperationTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MFVehicleInformation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EstablishmentId = table.Column<int>(type: "int", nullable: false),
                    Make = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Model = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Year = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    Color = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    License = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    VIN = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MFVehicleInformation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MFVehicleInformation_Establishments_EstablishmentId",
                        column: x => x.EstablishmentId,
                        principalTable: "Establishments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MFOperationDetails_BusinessTypeId",
                table: "MFOperationDetails",
                column: "BusinessTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MFOperationDetails_EstablishmentId",
                table: "MFOperationDetails",
                column: "EstablishmentId");

            migrationBuilder.CreateIndex(
                name: "IX_MFOperationDetails_OperationTypeId",
                table: "MFOperationDetails",
                column: "OperationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MFVehicleInformation_EstablishmentId",
                table: "MFVehicleInformation",
                column: "EstablishmentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MFOperationDetails");

            migrationBuilder.DropTable(
                name: "MFVehicleInformation");
        }
    }
}
