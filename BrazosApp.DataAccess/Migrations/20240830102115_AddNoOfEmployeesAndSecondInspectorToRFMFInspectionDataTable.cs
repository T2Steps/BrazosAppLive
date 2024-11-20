using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrazosApp.DataAccess.Migrations
{
    public partial class AddNoOfEmployeesAndSecondInspectorToRFMFInspectionDataTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NumberOfEmployees",
                table: "RFMFInspectionData",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SecondaryInspector",
                table: "RFMFInspectionData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SecondaryInspectorSign",
                table: "RFMFInspectionData",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfEmployees",
                table: "RFMFInspectionData");

            migrationBuilder.DropColumn(
                name: "SecondaryInspector",
                table: "RFMFInspectionData");

            migrationBuilder.DropColumn(
                name: "SecondaryInspectorSign",
                table: "RFMFInspectionData");
        }
    }
}
