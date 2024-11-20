using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrazosApp.DataAccess.Migrations
{
    public partial class AddingCommentOnPaymentType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte>(
                name: "PaymentMethod",
                table: "Payments",
                type: "tinyint",
                nullable: true,
                comment: "0 = CreditCard, 1 = ECheck, 2 = Cash, 3 = Cheque, 4 = Card",
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true,
                oldComment: "0 = CreditCard, 1 = ECheck, 2 = Cash, 3 = Cheque");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte>(
                name: "PaymentMethod",
                table: "Payments",
                type: "tinyint",
                nullable: true,
                comment: "0 = CreditCard, 1 = ECheck, 2 = Cash, 3 = Cheque",
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true,
                oldComment: "0 = CreditCard, 1 = ECheck, 2 = Cash, 3 = Cheque, 4 = Card");
        }
    }
}
