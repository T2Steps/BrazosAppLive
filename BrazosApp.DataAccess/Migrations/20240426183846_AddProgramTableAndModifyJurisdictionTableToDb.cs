using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrazosApp.DataAccess.Migrations
{
    public partial class AddProgramTableAndModifyJurisdictionTableToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Jurisdictions",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Jurisdictions",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Jurisdictions",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.AddColumn<string>(
                name: "AccountCode",
                table: "Jurisdictions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AccountDescription",
                table: "Jurisdictions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProgramId",
                table: "Jurisdictions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Programs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Programs", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Jurisdictions_ProgramId",
                table: "Jurisdictions",
                column: "ProgramId");

            migrationBuilder.AddForeignKey(
                name: "FK_Jurisdictions_Programs_ProgramId",
                table: "Jurisdictions",
                column: "ProgramId",
                principalTable: "Programs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jurisdictions_Programs_ProgramId",
                table: "Jurisdictions");

            migrationBuilder.DropTable(
                name: "Programs");

            migrationBuilder.DropIndex(
                name: "IX_Jurisdictions_ProgramId",
                table: "Jurisdictions");

            migrationBuilder.DropColumn(
                name: "AccountCode",
                table: "Jurisdictions");

            migrationBuilder.DropColumn(
                name: "AccountDescription",
                table: "Jurisdictions");

            migrationBuilder.DropColumn(
                name: "ProgramId",
                table: "Jurisdictions");

            migrationBuilder.InsertData(
                table: "Jurisdictions",
                columns: new[] { "Id", "IsActive", "Name" },
                values: new object[] { 1, true, "City of Bryan" });

            migrationBuilder.InsertData(
                table: "Jurisdictions",
                columns: new[] { "Id", "IsActive", "Name" },
                values: new object[] { 2, true, "City of College Station" });

            migrationBuilder.InsertData(
                table: "Jurisdictions",
                columns: new[] { "Id", "IsActive", "Name" },
                values: new object[] { 3, true, "Brazos County" });
        }
    }
}
