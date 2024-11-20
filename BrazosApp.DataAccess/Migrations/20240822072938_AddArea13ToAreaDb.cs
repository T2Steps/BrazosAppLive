using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrazosApp.DataAccess.Migrations
{
    public partial class AddArea13ToAreaDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.InsertData(
            //    table: "Areas",
            //    columns: new[] { "Id", "AreaNumber", "Description", "IsActive" },
            //    values: new object[] { 12, 13, "Migrate", true });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DeleteData(
            //    table: "Areas",
            //    keyColumn: "Id",
            //    keyValue: 12);
        }
    }
}
