using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrazosApp.DataAccess.Migrations
{
    public partial class AddApplicationTypeTableAndSeedDataToTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LanguageTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationType_LanguageType_LanguageTypeId",
                        column: x => x.LanguageTypeId,
                        principalTable: "LanguageType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ApplicationType",
                columns: new[] { "Id", "IsActive", "LanguageTypeId", "Name" },
                values: new object[,]
                {
                    { 1, true, 1, "Retail Food New" },
                    { 2, true, 2, "Retail Food New (Spanish)" },
                    { 3, true, 1, "Mobile Food New" },
                    { 4, true, 2, "Mobile Food New (Spanish)" },
                    { 5, true, 1, "Retail Food Change of Owner" },
                    { 6, true, 2, "Retail Food Change of Owner (Spanish)" },
                    { 7, true, 1, "Mobile Food Change of Owner" },
                    { 8, true, 2, "Mobile Food Change of Owner (Spanish)" },
                    { 9, true, 1, "Temporary Food" },
                    { 10, true, 2, "Temporary Food (Spanish)" },
                    { 11, true, 1, "Foster Home" },
                    { 12, true, 1, "Daycare Sanitation" },
                    { 13, true, 1, "Food Handler Enrollment Application" },
                    { 14, true, 2, "Food Handler Enrollment Application (Spanish)" },
                    { 15, true, 1, "Pools" },
                    { 16, true, 1, "Complaints" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationType_LanguageTypeId",
                table: "ApplicationType",
                column: "LanguageTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationType");
        }
    }
}
