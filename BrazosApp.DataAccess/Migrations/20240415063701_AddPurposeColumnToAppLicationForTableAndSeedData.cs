using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrazosApp.DataAccess.Migrations
{
    public partial class AddPurposeColumnToAppLicationForTableAndSeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Purpose",
                table: "ApplicationFor",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "ApplicationFor",
                keyColumn: "Id",
                keyValue: 1,
                column: "Purpose",
                value: "NewPermit");

            migrationBuilder.UpdateData(
                table: "ApplicationFor",
                keyColumn: "Id",
                keyValue: 2,
                column: "Purpose",
                value: "NewPermit");

            migrationBuilder.UpdateData(
                table: "ApplicationFor",
                keyColumn: "Id",
                keyValue: 3,
                column: "Purpose",
                value: "NewPermit");

            migrationBuilder.UpdateData(
                table: "ApplicationFor",
                keyColumn: "Id",
                keyValue: 4,
                column: "Purpose",
                value: "NewPermit");

            migrationBuilder.UpdateData(
                table: "ApplicationFor",
                keyColumn: "Id",
                keyValue: 5,
                column: "Purpose",
                value: "OwnerChange");

            migrationBuilder.UpdateData(
                table: "ApplicationFor",
                keyColumn: "Id",
                keyValue: 6,
                column: "Purpose",
                value: "OwnerChange");

            migrationBuilder.UpdateData(
                table: "ApplicationFor",
                keyColumn: "Id",
                keyValue: 7,
                column: "Purpose",
                value: "OwnerChange");

            migrationBuilder.UpdateData(
                table: "ApplicationFor",
                keyColumn: "Id",
                keyValue: 8,
                column: "Purpose",
                value: "OwnerChange");

            migrationBuilder.UpdateData(
                table: "ApplicationFor",
                keyColumn: "Id",
                keyValue: 9,
                column: "Purpose",
                value: "NewPermit");

            migrationBuilder.UpdateData(
                table: "ApplicationFor",
                keyColumn: "Id",
                keyValue: 10,
                column: "Purpose",
                value: "NewPermit");

            migrationBuilder.UpdateData(
                table: "ApplicationFor",
                keyColumn: "Id",
                keyValue: 11,
                column: "Purpose",
                value: "NewPermit");

            migrationBuilder.UpdateData(
                table: "ApplicationFor",
                keyColumn: "Id",
                keyValue: 12,
                column: "Purpose",
                value: "NewPermit");

            migrationBuilder.UpdateData(
                table: "ApplicationFor",
                keyColumn: "Id",
                keyValue: 13,
                column: "Purpose",
                value: "NewPermit");

            migrationBuilder.UpdateData(
                table: "ApplicationFor",
                keyColumn: "Id",
                keyValue: 14,
                column: "Purpose",
                value: "NewPermit");

            migrationBuilder.UpdateData(
                table: "ApplicationFor",
                keyColumn: "Id",
                keyValue: 15,
                column: "Purpose",
                value: "NewPermit");

            migrationBuilder.UpdateData(
                table: "ApplicationFor",
                keyColumn: "Id",
                keyValue: 16,
                column: "Purpose",
                value: "Complaint");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Purpose",
                table: "ApplicationFor");
        }
    }
}
