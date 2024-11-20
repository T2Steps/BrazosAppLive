using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrazosApp.DataAccess.Migrations
{
    public partial class AddRefundedByRefundedDateOldPayIdAndReasonToPaymentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte>(
                name: "PaymentStatus",
                table: "Payments",
                type: "tinyint",
                nullable: false,
                comment: "1 = Pending, 2 = Paid, 3 = Cancelled, 4 = Failure, 5 = Refunded",
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldComment: "1 = Pending, 2 = Paid, 3 = Cancelled, 4 = Failure");

            migrationBuilder.AddColumn<int>(
                name: "OldPaymentId",
                table: "Payments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReasonForRefunding",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RefundedBy",
                table: "Payments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RefundedDate",
                table: "Payments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "PaymentStatus",
                table: "PaymentDetailsTable",
                type: "tinyint",
                nullable: false,
                comment: "1 = Pending, 2 = Paid, 3 = Cancelled, 4 = Failure, 5 = Refunded",
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldComment: "1 = Pending, 2 = Paid, 3 = Cancelled, 4 = Failure");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OldPaymentId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "ReasonForRefunding",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "RefundedBy",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "RefundedDate",
                table: "Payments");

            migrationBuilder.AlterColumn<byte>(
                name: "PaymentStatus",
                table: "Payments",
                type: "tinyint",
                nullable: false,
                comment: "1 = Pending, 2 = Paid, 3 = Cancelled, 4 = Failure",
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldComment: "1 = Pending, 2 = Paid, 3 = Cancelled, 4 = Failure, 5 = Refunded");

            migrationBuilder.AlterColumn<byte>(
                name: "PaymentStatus",
                table: "PaymentDetailsTable",
                type: "tinyint",
                nullable: false,
                comment: "1 = Pending, 2 = Paid, 3 = Cancelled, 4 = Failure",
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldComment: "1 = Pending, 2 = Paid, 3 = Cancelled, 4 = Failure, 5 = Refunded");
        }
    }
}
