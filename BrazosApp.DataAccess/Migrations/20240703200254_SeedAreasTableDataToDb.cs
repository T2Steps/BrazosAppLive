using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrazosApp.DataAccess.Migrations
{
    public partial class SeedAreasTableDataToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Areas",
                columns: new[] { "Id", "Description", "IsActive", "TerritoryNumber" },
                values: new object[,]
                {
                    { 1, "Unassigned - new establishments", true, 0 },
                    { 2, "Assigned to inspector", true, 1 },
                    { 3, "Assigned to inspector", true, 2 },
                    { 4, "Assigned to inspector", true, 3 },
                    { 5, "Assigned to inspector", true, 4 },
                    { 6, "Assigned to inspector", true, 5 },
                    { 7, "Assigned to inspector", true, 6 },
                    { 8, "Assigned to inspector", true, 7 },
                    { 9, "Currently food establishments on OSSF - will be fed into inspector assigned areas prior to migration", true, 8 },
                    { 10, "Texas A&M owned permits", true, 12 },
                    { 11, "Establishments not in current operation", true, 51 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 11);
        }
    }
}
