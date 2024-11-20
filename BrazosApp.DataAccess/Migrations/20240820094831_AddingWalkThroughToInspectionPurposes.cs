using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrazosApp.DataAccess.Migrations
{
    public partial class AddingWalkThroughToInspectionPurposes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "InspectionPurposes",
                columns: new[] { "Id", "Code", "IsActive", "Name" },
                values: new object[] { 7, "COM", true, "Walk Through" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "InspectionPurposes",
                keyColumn: "Id",
                keyValue: 7);
        }
    }
}
