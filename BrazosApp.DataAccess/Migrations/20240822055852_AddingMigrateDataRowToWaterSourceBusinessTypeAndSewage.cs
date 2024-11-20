using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrazosApp.DataAccess.Migrations
{
    public partial class AddingMigrateDataRowToWaterSourceBusinessTypeAndSewage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "BusinessTypes",
                columns: new[] { "Id", "Code", "IsActive", "Name", "SpName" },
                values: new object[] { 21, "RF", true, "Migrate", "Emigrar" });

            migrationBuilder.InsertData(
                table: "PublicSewage",
                columns: new[] { "Id", "IsActive", "Name" },
                values: new object[] { 5, true, "Migrate" });

            migrationBuilder.InsertData(
                table: "WaterSources",
                columns: new[] { "Id", "IsActive", "Name" },
                values: new object[] { 15, true, "Migrate" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BusinessTypes",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "PublicSewage",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "WaterSources",
                keyColumn: "Id",
                keyValue: 15);
        }
    }
}
