using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrazosApp.DataAccess.Migrations
{
    public partial class SeedPermitStatusDataToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "PermitStatus",
                columns: new[] { "Id", "IsActive", "Name" },
                values: new object[] { 15, true, "Inspection" });

            migrationBuilder.InsertData(
                table: "PermitStatus",
                columns: new[] { "Id", "IsActive", "Name" },
                values: new object[] { 16, true, "Completed" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PermitStatus",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "PermitStatus",
                keyColumn: "Id",
                keyValue: 16);
        }
    }
}
