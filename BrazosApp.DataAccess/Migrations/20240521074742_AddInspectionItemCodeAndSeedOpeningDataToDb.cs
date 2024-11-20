using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrazosApp.DataAccess.Migrations
{
    public partial class AddInspectionItemCodeAndSeedOpeningDataToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InspectionItemCodes",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "tinyint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InspectionItemCodes", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "InspectionItemCodes",
                columns: new[] { "Id", "Name" },
                values: new object[] { (byte)1, "RFO" });

            migrationBuilder.InsertData(
                table: "InspectionItemCodes",
                columns: new[] { "Id", "Name" },
                values: new object[] { (byte)2, "MFO" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InspectionItemCodes");
        }
    }
}
