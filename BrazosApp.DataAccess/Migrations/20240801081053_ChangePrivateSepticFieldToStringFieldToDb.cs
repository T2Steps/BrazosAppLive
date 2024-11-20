using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrazosApp.DataAccess.Migrations
{
    public partial class ChangePrivateSepticFieldToStringFieldToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RFOperationDetails_PrivateSeptic_PrivateSepticId",
                table: "RFOperationDetails");

            migrationBuilder.DropIndex(
                name: "IX_RFOperationDetails_PrivateSepticId",
                table: "RFOperationDetails");

            migrationBuilder.DropColumn(
                name: "PrivateSepticId",
                table: "RFOperationDetails");

            migrationBuilder.AddColumn<string>(
                name: "PrivateSeptic",
                table: "RFOperationDetails",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrivateSeptic",
                table: "RFOperationDetails");

            migrationBuilder.AddColumn<int>(
                name: "PrivateSepticId",
                table: "RFOperationDetails",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RFOperationDetails_PrivateSepticId",
                table: "RFOperationDetails",
                column: "PrivateSepticId");

            migrationBuilder.AddForeignKey(
                name: "FK_RFOperationDetails_PrivateSeptic_PrivateSepticId",
                table: "RFOperationDetails",
                column: "PrivateSepticId",
                principalTable: "PrivateSeptic",
                principalColumn: "Id");
        }
    }
}
