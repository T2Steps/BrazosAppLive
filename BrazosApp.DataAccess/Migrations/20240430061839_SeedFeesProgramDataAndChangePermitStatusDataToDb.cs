using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrazosApp.DataAccess.Migrations
{
    public partial class SeedFeesProgramDataAndChangePermitStatusDataToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PermitStatus",
                keyColumn: "Id",
                keyValue: 7,
                column: "Name",
                value: "Opening Inspection");

            migrationBuilder.InsertData(
                table: "Programs",
                columns: new[] { "Id", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1, true, "Retail Food" },
                    { 2, true, "Mobile Food" },
                    { 3, true, "Temporary Food" },
                    { 4, true, "Pools" },
                    { 5, true, "Foster Care" },
                    { 6, true, "Daycare" },
                    { 7, true, "OSSF" },
                    { 8, true, "FHC & L" },
                    { 9, true, "Clinic - C4" },
                    { 10, true, "Clinic - CHS" },
                    { 11, true, "General" },
                    { 12, true, "Other Service" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Programs",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Programs",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Programs",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Programs",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Programs",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Programs",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Programs",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Programs",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Programs",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Programs",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Programs",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Programs",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.UpdateData(
                table: "PermitStatus",
                keyColumn: "Id",
                keyValue: 7,
                column: "Name",
                value: "Pre-Op");
        }
    }
}
