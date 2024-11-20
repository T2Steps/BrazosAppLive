using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrazosApp.DataAccess.Migrations
{
    public partial class RemoveInvoiceNoFromFeesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InvoiceNo",
                table: "Fees");

            migrationBuilder.AlterColumn<byte>(
                name: "Status",
                table: "Fees",
                type: "tinyint",
                nullable: false,
                comment: "1 = Pending, 2 = Success, 3 = Failure",
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldComment: "1='Pending', 2='Success', 3='Failure'");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte>(
                name: "Status",
                table: "Fees",
                type: "tinyint",
                nullable: false,
                comment: "1='Pending', 2='Success', 3='Failure'",
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldComment: "1 = Pending, 2 = Success, 3 = Failure");

            migrationBuilder.AddColumn<string>(
                name: "InvoiceNo",
                table: "Fees",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}
