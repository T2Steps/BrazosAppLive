using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrazosApp.DataAccess.Migrations
{
    public partial class AddMFOperationLocationTableToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MobileOperationLocation",
                table: "MFOperationDetails");

            migrationBuilder.CreateTable(
                name: "MFOperationLocations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EstablishmentId = table.Column<int>(type: "int", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MFOperationLocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MFOperationLocations_Establishments_EstablishmentId",
                        column: x => x.EstablishmentId,
                        principalTable: "Establishments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MFOperationLocations_EstablishmentId",
                table: "MFOperationLocations",
                column: "EstablishmentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MFOperationLocations");

            migrationBuilder.AddColumn<string>(
                name: "MobileOperationLocation",
                table: "MFOperationDetails",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
