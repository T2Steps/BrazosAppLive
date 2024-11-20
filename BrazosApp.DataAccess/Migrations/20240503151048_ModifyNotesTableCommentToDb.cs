using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrazosApp.DataAccess.Migrations
{
    public partial class ModifyNotesTableCommentToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte>(
                name: "Status",
                table: "Notes",
                type: "tinyint",
                nullable: false,
                comment: "0='Not Edited', 1='Edited', 2='Deleted'",
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldComment: "0='Not Edited', 1='Edited', 3='Deleted'");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte>(
                name: "Status",
                table: "Notes",
                type: "tinyint",
                nullable: false,
                comment: "0='Not Edited', 1='Edited', 3='Deleted'",
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldComment: "0='Not Edited', 1='Edited', 2='Deleted'");
        }
    }
}
