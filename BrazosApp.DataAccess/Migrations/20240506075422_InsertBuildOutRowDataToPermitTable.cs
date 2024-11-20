using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrazosApp.DataAccess.Migrations
{
    public partial class InsertBuildOutRowDataToPermitTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PermitStatus",
                keyColumn: "Id",
                keyValue: 6,
                column: "Name",
                value: "Pending Build-Out");

            migrationBuilder.UpdateData(
                table: "PermitStatus",
                keyColumn: "Id",
                keyValue: 7,
                column: "Name",
                value: "Pending Payment");

            migrationBuilder.UpdateData(
                table: "PermitStatus",
                keyColumn: "Id",
                keyValue: 8,
                column: "Name",
                value: "Opening Inspection");

            migrationBuilder.UpdateData(
                table: "PermitStatus",
                keyColumn: "Id",
                keyValue: 9,
                column: "Name",
                value: "Active");

            migrationBuilder.UpdateData(
                table: "PermitStatus",
                keyColumn: "Id",
                keyValue: 10,
                column: "Name",
                value: "Renewal");

            migrationBuilder.UpdateData(
                table: "PermitStatus",
                keyColumn: "Id",
                keyValue: 11,
                column: "Name",
                value: "Expired");

            migrationBuilder.UpdateData(
                table: "PermitStatus",
                keyColumn: "Id",
                keyValue: 12,
                column: "Name",
                value: "Area 51");

            migrationBuilder.UpdateData(
                table: "PermitStatus",
                keyColumn: "Id",
                keyValue: 13,
                column: "Name",
                value: "Inactive");

            migrationBuilder.InsertData(
                table: "PermitStatus",
                columns: new[] { "Id", "IsActive", "Name" },
                values: new object[] { 14, true, "Closed" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PermitStatus",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.UpdateData(
                table: "PermitStatus",
                keyColumn: "Id",
                keyValue: 6,
                column: "Name",
                value: "Pending Payment");

            migrationBuilder.UpdateData(
                table: "PermitStatus",
                keyColumn: "Id",
                keyValue: 7,
                column: "Name",
                value: "Opening Inspection");

            migrationBuilder.UpdateData(
                table: "PermitStatus",
                keyColumn: "Id",
                keyValue: 8,
                column: "Name",
                value: "Active");

            migrationBuilder.UpdateData(
                table: "PermitStatus",
                keyColumn: "Id",
                keyValue: 9,
                column: "Name",
                value: "Renewal");

            migrationBuilder.UpdateData(
                table: "PermitStatus",
                keyColumn: "Id",
                keyValue: 10,
                column: "Name",
                value: "Expired");

            migrationBuilder.UpdateData(
                table: "PermitStatus",
                keyColumn: "Id",
                keyValue: 11,
                column: "Name",
                value: "Area 51");

            migrationBuilder.UpdateData(
                table: "PermitStatus",
                keyColumn: "Id",
                keyValue: 12,
                column: "Name",
                value: "Inactive");

            migrationBuilder.UpdateData(
                table: "PermitStatus",
                keyColumn: "Id",
                keyValue: 13,
                column: "Name",
                value: "Closed");
        }
    }
}
