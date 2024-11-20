using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrazosApp.DataAccess.Migrations
{
    public partial class UpdateScheduleStatusDataToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ScheduleStatus",
                columns: new[] { "Id", "IsActive", "Name" },
                values: new object[] { 6, true, "Inactive" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ScheduleStatus",
                keyColumn: "Id",
                keyValue: 6);
        }
    }
}
