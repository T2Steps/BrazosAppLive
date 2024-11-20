using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrazosApp.DataAccess.Migrations
{
    public partial class AddLanguageTypeTableAndSeedValues : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LanguageType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LanguageType", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "LanguageType",
                columns: new[] { "Id", "Code", "IsActive", "Name" },
                values: new object[] { 1, "E", true, "English" });

            migrationBuilder.InsertData(
                table: "LanguageType",
                columns: new[] { "Id", "Code", "IsActive", "Name" },
                values: new object[] { 2, "SP", true, "Spanish" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LanguageType");
        }
    }
}
