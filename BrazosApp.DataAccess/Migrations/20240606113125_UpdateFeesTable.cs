using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrazosApp.DataAccess.Migrations
{
    public partial class UpdateFeesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte>(
                name: "Status",
                table: "Fees",
                type: "tinyint",
                nullable: false,
                comment: "1='Pending', 2='Success', 3='Failure'",
                oldClrType: typeof(bool),
                oldType: "bit");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                table: "Fees",
                type: "bit",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldComment: "1='Pending', 2='Success', 3='Failure'");
        }
    }
}
