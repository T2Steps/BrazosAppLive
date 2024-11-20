using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrazosApp.DataAccess.Migrations
{
    public partial class SeedWaterSourceSewageDataToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "PrivateSeptic",
                columns: new[] { "Id", "IsActive", "Name" },
                values: new object[] { 1, true, "OSSF" });

            migrationBuilder.InsertData(
                table: "PublicSewage",
                columns: new[] { "Id", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1, true, "City of Bryan" },
                    { 2, true, "City of College Station" },
                    { 3, true, "Meadow Creek Sewer Company" },
                    { 4, true, "NI America Texas Development" }
                });

            migrationBuilder.InsertData(
                table: "WaterSources",
                columns: new[] { "Id", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1, true, "City of Bryan" },
                    { 2, true, "City of College Station" },
                    { 3, true, "Wellborn SUD" },
                    { 4, true, "Wickson Creek SUD" },
                    { 5, true, "Undine Texas LLC" },
                    { 6, true, "Texas A&M University Main Campus" },
                    { 7, true, "Benchley Oaks Subdivision" },
                    { 8, true, "Sanderson Farms Bryan Facility" },
                    { 9, true, "Ramblewood Mobile Home Park" },
                    { 10, true, "Wheelock Express" },
                    { 11, true, "Carousel Mobile Home Park" },
                    { 12, true, "Al Leonard Ranch" },
                    { 13, true, "Lakewood Estates" },
                    { 14, true, "Smetana Forest" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PrivateSeptic",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PublicSewage",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PublicSewage",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "PublicSewage",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "PublicSewage",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "WaterSources",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "WaterSources",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "WaterSources",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "WaterSources",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "WaterSources",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "WaterSources",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "WaterSources",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "WaterSources",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "WaterSources",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "WaterSources",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "WaterSources",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "WaterSources",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "WaterSources",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "WaterSources",
                keyColumn: "Id",
                keyValue: 14);
        }
    }
}
