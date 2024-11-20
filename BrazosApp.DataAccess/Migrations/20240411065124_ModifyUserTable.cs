using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrazosApp.DataAccess.Migrations
{
    public partial class ModifyUserTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TerritoryWiseInspectors_AssignedUserId",
                table: "TerritoryWiseInspectors");

            migrationBuilder.CreateIndex(
                name: "IX_TerritoryWiseInspectors_AssignedUserId",
                table: "TerritoryWiseInspectors",
                column: "AssignedUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TerritoryWiseInspectors_AssignedUserId",
                table: "TerritoryWiseInspectors");

            migrationBuilder.CreateIndex(
                name: "IX_TerritoryWiseInspectors_AssignedUserId",
                table: "TerritoryWiseInspectors",
                column: "AssignedUserId",
                unique: true);
        }
    }
}
