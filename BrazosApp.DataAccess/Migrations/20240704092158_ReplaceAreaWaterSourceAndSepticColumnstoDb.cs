using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrazosApp.DataAccess.Migrations
{
    public partial class ReplaceAreaWaterSourceAndSepticColumnstoDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AgencyStaffReqFields_Territories_TerritoryId",
                table: "AgencyStaffReqFields");

            migrationBuilder.DropColumn(
                name: "PrivateWaterSeptic",
                table: "RFOperationDetails");

            migrationBuilder.DropColumn(
                name: "PublicWaterSouce",
                table: "RFOperationDetails");

            migrationBuilder.DropColumn(
                name: "Publicsewage",
                table: "RFOperationDetails");

            migrationBuilder.DropColumn(
                name: "PublicWaterSouce",
                table: "MFOperationDetails");

            migrationBuilder.DropColumn(
                name: "Territory",
                table: "Establishments");

            migrationBuilder.RenameColumn(
                name: "TerritoryId",
                table: "AgencyStaffReqFields",
                newName: "AreaId");

            migrationBuilder.RenameIndex(
                name: "IX_AgencyStaffReqFields_TerritoryId",
                table: "AgencyStaffReqFields",
                newName: "IX_AgencyStaffReqFields_AreaId");

            migrationBuilder.AddColumn<int>(
                name: "PrivateSepticId",
                table: "RFOperationDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PublicSewageId",
                table: "RFOperationDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WaterSourceId",
                table: "RFOperationDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WaterSourceId",
                table: "MFOperationDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Area",
                table: "Establishments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RFOperationDetails_PrivateSepticId",
                table: "RFOperationDetails",
                column: "PrivateSepticId");

            migrationBuilder.CreateIndex(
                name: "IX_RFOperationDetails_PublicSewageId",
                table: "RFOperationDetails",
                column: "PublicSewageId");

            migrationBuilder.CreateIndex(
                name: "IX_RFOperationDetails_WaterSourceId",
                table: "RFOperationDetails",
                column: "WaterSourceId");

            migrationBuilder.CreateIndex(
                name: "IX_MFOperationDetails_WaterSourceId",
                table: "MFOperationDetails",
                column: "WaterSourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_AgencyStaffReqFields_Areas_AreaId",
                table: "AgencyStaffReqFields",
                column: "AreaId",
                principalTable: "Areas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MFOperationDetails_WaterSources_WaterSourceId",
                table: "MFOperationDetails",
                column: "WaterSourceId",
                principalTable: "WaterSources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RFOperationDetails_PrivateSeptic_PrivateSepticId",
                table: "RFOperationDetails",
                column: "PrivateSepticId",
                principalTable: "PrivateSeptic",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RFOperationDetails_PublicSewage_PublicSewageId",
                table: "RFOperationDetails",
                column: "PublicSewageId",
                principalTable: "PublicSewage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RFOperationDetails_WaterSources_WaterSourceId",
                table: "RFOperationDetails",
                column: "WaterSourceId",
                principalTable: "WaterSources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AgencyStaffReqFields_Areas_AreaId",
                table: "AgencyStaffReqFields");

            migrationBuilder.DropForeignKey(
                name: "FK_MFOperationDetails_WaterSources_WaterSourceId",
                table: "MFOperationDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_RFOperationDetails_PrivateSeptic_PrivateSepticId",
                table: "RFOperationDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_RFOperationDetails_PublicSewage_PublicSewageId",
                table: "RFOperationDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_RFOperationDetails_WaterSources_WaterSourceId",
                table: "RFOperationDetails");

            migrationBuilder.DropIndex(
                name: "IX_RFOperationDetails_PrivateSepticId",
                table: "RFOperationDetails");

            migrationBuilder.DropIndex(
                name: "IX_RFOperationDetails_PublicSewageId",
                table: "RFOperationDetails");

            migrationBuilder.DropIndex(
                name: "IX_RFOperationDetails_WaterSourceId",
                table: "RFOperationDetails");

            migrationBuilder.DropIndex(
                name: "IX_MFOperationDetails_WaterSourceId",
                table: "MFOperationDetails");

            migrationBuilder.DropColumn(
                name: "PrivateSepticId",
                table: "RFOperationDetails");

            migrationBuilder.DropColumn(
                name: "PublicSewageId",
                table: "RFOperationDetails");

            migrationBuilder.DropColumn(
                name: "WaterSourceId",
                table: "RFOperationDetails");

            migrationBuilder.DropColumn(
                name: "WaterSourceId",
                table: "MFOperationDetails");

            migrationBuilder.DropColumn(
                name: "Area",
                table: "Establishments");

            migrationBuilder.RenameColumn(
                name: "AreaId",
                table: "AgencyStaffReqFields",
                newName: "TerritoryId");

            migrationBuilder.RenameIndex(
                name: "IX_AgencyStaffReqFields_AreaId",
                table: "AgencyStaffReqFields",
                newName: "IX_AgencyStaffReqFields_TerritoryId");

            migrationBuilder.AddColumn<string>(
                name: "PrivateWaterSeptic",
                table: "RFOperationDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PublicWaterSouce",
                table: "RFOperationDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Publicsewage",
                table: "RFOperationDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PublicWaterSouce",
                table: "MFOperationDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Territory",
                table: "Establishments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AgencyStaffReqFields_Territories_TerritoryId",
                table: "AgencyStaffReqFields",
                column: "TerritoryId",
                principalTable: "Territories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
