using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrazosApp.DataAccess.Migrations
{
    public partial class MovingInspectedByFieldsToInspectionTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InspectedBy",
                table: "RFMFInspectionData");

            migrationBuilder.DropColumn(
                name: "InspectedBySign",
                table: "RFMFInspectionData");

            migrationBuilder.DropColumn(
                name: "InspectorSignFile",
                table: "RFMFInspectionData");

            migrationBuilder.DropColumn(
                name: "InspectedBy",
                table: "OpeningInspectionData");

            migrationBuilder.DropColumn(
                name: "InspectedBySign",
                table: "OpeningInspectionData");

            migrationBuilder.DropColumn(
                name: "InspectorSignFile",
                table: "OpeningInspectionData");

            migrationBuilder.AddColumn<int>(
                name: "InspectedBy",
                table: "Inspections",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "InspectedBySign",
                table: "Inspections",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InspectorSignFile",
                table: "Inspections",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InspectedBy",
                table: "Inspections");

            migrationBuilder.DropColumn(
                name: "InspectedBySign",
                table: "Inspections");

            migrationBuilder.DropColumn(
                name: "InspectorSignFile",
                table: "Inspections");

            migrationBuilder.AddColumn<int>(
                name: "InspectedBy",
                table: "RFMFInspectionData",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "InspectedBySign",
                table: "RFMFInspectionData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InspectorSignFile",
                table: "RFMFInspectionData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InspectedBy",
                table: "OpeningInspectionData",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "InspectedBySign",
                table: "OpeningInspectionData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InspectorSignFile",
                table: "OpeningInspectionData",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
