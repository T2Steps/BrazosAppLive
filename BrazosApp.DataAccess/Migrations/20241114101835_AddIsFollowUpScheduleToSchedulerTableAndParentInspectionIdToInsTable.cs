using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrazosApp.DataAccess.Migrations
{
    public partial class AddIsFollowUpScheduleToSchedulerTableAndParentInspectionIdToInsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFollowUpInspection",
                table: "Inspections");

            migrationBuilder.AddColumn<bool>(
                name: "IsFollowUpSchedule",
                table: "Schedules",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFollowUpSchedule",
                table: "Schedules");

            migrationBuilder.AddColumn<bool>(
                name: "IsFollowUpInspection",
                table: "Inspections",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
