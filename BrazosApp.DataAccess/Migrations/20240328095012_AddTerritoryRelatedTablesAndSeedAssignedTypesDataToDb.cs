using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrazosApp.DataAccess.Migrations
{
    public partial class AddTerritoryRelatedTablesAndSeedAssignedTypesDataToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Territories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ColorCode = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Territories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TerritoryAssignedTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TerritoryAssignedTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TerritoryWiseInspectors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TerritoryId = table.Column<int>(type: "int", nullable: false),
                    AssignedUserId = table.Column<int>(type: "int", nullable: false),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TerritoryWiseInspectors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TerritoryWiseInspectors_Territories_TerritoryId",
                        column: x => x.TerritoryId,
                        principalTable: "Territories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TerritoryWiseInspectors_TerritoryAssignedTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "TerritoryAssignedTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TerritoryWiseInspectors_Users_AssignedUserId",
                        column: x => x.AssignedUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "TerritoryAssignedTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "System Assigned" });

            migrationBuilder.InsertData(
                table: "TerritoryAssignedTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "Default Assigned" });

            migrationBuilder.InsertData(
                table: "TerritoryAssignedTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { 3, "Normal Assigned" });

            migrationBuilder.CreateIndex(
                name: "IX_TerritoryWiseInspectors_AssignedUserId",
                table: "TerritoryWiseInspectors",
                column: "AssignedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TerritoryWiseInspectors_TerritoryId",
                table: "TerritoryWiseInspectors",
                column: "TerritoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TerritoryWiseInspectors_TypeId",
                table: "TerritoryWiseInspectors",
                column: "TypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TerritoryWiseInspectors");

            migrationBuilder.DropTable(
                name: "Territories");

            migrationBuilder.DropTable(
                name: "TerritoryAssignedTypes");
        }
    }
}
