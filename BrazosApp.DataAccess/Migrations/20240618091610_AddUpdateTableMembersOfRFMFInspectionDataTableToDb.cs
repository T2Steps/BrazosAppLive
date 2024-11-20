using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrazosApp.DataAccess.Migrations
{
    public partial class AddUpdateTableMembersOfRFMFInspectionDataTableToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comment",
                table: "RFMFInspectionData");

            migrationBuilder.AddColumn<bool>(
                name: "SampleCollected",
                table: "RFMFInspectionData",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SampleCollected",
                table: "RFMFInspectionData");

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "RFMFInspectionData",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
