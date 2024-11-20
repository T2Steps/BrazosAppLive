using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrazosApp.DataAccess.Migrations
{
    public partial class RemoveRenewalTempHistoryTableFromDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RenewalTempHistories");

            migrationBuilder.AddColumn<bool>(
                name: "IsFollowUpInspection",
                table: "Inspections",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ParentInspectionId",
                table: "Inspections",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFollowUpInspection",
                table: "Inspections");

            migrationBuilder.DropColumn(
                name: "ParentInspectionId",
                table: "Inspections");

            migrationBuilder.CreateTable(
                name: "RenewalTempHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EstId = table.Column<int>(type: "int", nullable: false),
                    Est_Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FeesAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Owner = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentId = table.Column<int>(type: "int", nullable: false),
                    PermitNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PermitStatus = table.Column<byte>(type: "tinyint", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RenewalTempHistories", x => x.Id);
                });
        }
    }
}
