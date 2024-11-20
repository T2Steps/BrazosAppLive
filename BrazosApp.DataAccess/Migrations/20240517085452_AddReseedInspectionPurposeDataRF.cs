using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrazosApp.DataAccess.Migrations
{
    public partial class AddReseedInspectionPurposeDataRF : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "InspectionPurposes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Code", "Name" },
                values: new object[] { "COM", "Opening Inspection" });

            migrationBuilder.InsertData(
                table: "InspectionPurposes",
                columns: new[] { "Id", "Code", "IsActive", "Name" },
                values: new object[] { 6, "RF", true, "Other" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "InspectionPurposes",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.UpdateData(
                table: "InspectionPurposes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Code", "Name" },
                values: new object[] { "RF", "Opening" });
        }
    }
}
