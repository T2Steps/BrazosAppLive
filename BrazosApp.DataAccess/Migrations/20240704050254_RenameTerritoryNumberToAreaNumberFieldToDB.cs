using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrazosApp.DataAccess.Migrations
{
    public partial class RenameTerritoryNumberToAreaNumberFieldToDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TerritoryNumber",
                table: "Areas",
                newName: "AreaNumber");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AreaNumber",
                table: "Areas",
                newName: "TerritoryNumber");
        }
    }
}
