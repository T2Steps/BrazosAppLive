using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrazosApp.DataAccess.Migrations
{
    public partial class AddSecondInspectorToOpeningInspectionData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SecondaryInspector",
                table: "OpeningInspectionData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SecondaryInspectorSign",
                table: "OpeningInspectionData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SecondaryInspectorSignFile",
                table: "OpeningInspectionData",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SecondaryInspector",
                table: "OpeningInspectionData");

            migrationBuilder.DropColumn(
                name: "SecondaryInspectorSign",
                table: "OpeningInspectionData");

            migrationBuilder.DropColumn(
                name: "SecondaryInspectorSignFile",
                table: "OpeningInspectionData");
        }
    }
}
