using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrazosApp.DataAccess.Migrations
{
    public partial class ModifyApplicationForTableDataToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ApplicationFor",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "ApplicationTypeId", "Code", "Name" },
                values: new object[] { 1, "FHEA", "Food Handler Enrollment Application" });

            migrationBuilder.UpdateData(
                table: "ApplicationFor",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "ApplicationTypeId", "Code", "LanguageTypeId", "Name" },
                values: new object[] { 1, "FHEA", 2, "Food Handler Enrollment Application (Spanish)" });

            migrationBuilder.UpdateData(
                table: "ApplicationFor",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "ApplicationTypeId", "Code", "Name" },
                values: new object[] { 2, "FH", "Foster Home" });

            migrationBuilder.UpdateData(
                table: "ApplicationFor",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "ApplicationTypeId", "Code", "LanguageTypeId", "Name" },
                values: new object[] { 3, "DS", 1, "Daycare Sanitation" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ApplicationFor",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "ApplicationTypeId", "Code", "Name" },
                values: new object[] { 2, "FH", "Foster Home" });

            migrationBuilder.UpdateData(
                table: "ApplicationFor",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "ApplicationTypeId", "Code", "LanguageTypeId", "Name" },
                values: new object[] { 3, "DS", 1, "Daycare Sanitation" });

            migrationBuilder.UpdateData(
                table: "ApplicationFor",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "ApplicationTypeId", "Code", "Name" },
                values: new object[] { 1, "FHEA", "Food Handler Enrollment Application" });

            migrationBuilder.UpdateData(
                table: "ApplicationFor",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "ApplicationTypeId", "Code", "LanguageTypeId", "Name" },
                values: new object[] { 1, "FHEA", 2, "Food Handler Enrollment Application (Spanish)" });
        }
    }
}
