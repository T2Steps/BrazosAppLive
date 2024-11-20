using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrazosApp.DataAccess.Migrations
{
    public partial class AddVoidAndReceiptColumnToFeesAndPaymentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReceiptNo",
                table: "Payments",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "Status",
                table: "FeesDetailsTables",
                type: "tinyint",
                nullable: false,
                comment: "1 = Pending, 2 = Paid, 3 = Cancelled, 4 = Renewal Pending, 5= Late Renewal Pending, 6 = 30 days, 7 = 60 days, 8 = Delinquent, 9 = Voided, 10 = Refund Requested, 11 = Refunded",
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldComment: "1 = Pending, 2 = Paid, 3 = Cancelled, 4 = Renewal Pending, 5= Late Renewal Pending, 6 = 30 days, 7 = 60 days, 8 = Delinquent");

            migrationBuilder.AlterColumn<byte>(
                name: "Status",
                table: "Fees",
                type: "tinyint",
                nullable: false,
                comment: "1 = Pending, 2 = Paid, 3 = Cancelled, 4 = Renewal Pending, 5= Late Renewal Pending, 6 = 30 days, 7 = 60 days, 8 = Delinquent, 9 = Voided, 10 = Refund Requested, 11 = Refunded",
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldComment: "1 = Pending, 2 = Paid, 3 = Cancelled, 4 = Renewal Pending, 5= Late Renewal Pending, 6 = 30 days, 7 = 60 days, 8 = Delinquent");

            migrationBuilder.AddColumn<string>(
                name: "ReasonForVoiding",
                table: "Fees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VoidedBy",
                table: "Fees",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "VoidedDate",
                table: "Fees",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VoidedTransactionNo",
                table: "Fees",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReceiptNo",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "ReasonForVoiding",
                table: "Fees");

            migrationBuilder.DropColumn(
                name: "VoidedBy",
                table: "Fees");

            migrationBuilder.DropColumn(
                name: "VoidedDate",
                table: "Fees");

            migrationBuilder.DropColumn(
                name: "VoidedTransactionNo",
                table: "Fees");

            migrationBuilder.AlterColumn<byte>(
                name: "Status",
                table: "FeesDetailsTables",
                type: "tinyint",
                nullable: false,
                comment: "1 = Pending, 2 = Paid, 3 = Cancelled, 4 = Renewal Pending, 5= Late Renewal Pending, 6 = 30 days, 7 = 60 days, 8 = Delinquent",
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldComment: "1 = Pending, 2 = Paid, 3 = Cancelled, 4 = Renewal Pending, 5= Late Renewal Pending, 6 = 30 days, 7 = 60 days, 8 = Delinquent, 9 = Voided, 10 = Refund Requested, 11 = Refunded");

            migrationBuilder.AlterColumn<byte>(
                name: "Status",
                table: "Fees",
                type: "tinyint",
                nullable: false,
                comment: "1 = Pending, 2 = Paid, 3 = Cancelled, 4 = Renewal Pending, 5= Late Renewal Pending, 6 = 30 days, 7 = 60 days, 8 = Delinquent",
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldComment: "1 = Pending, 2 = Paid, 3 = Cancelled, 4 = Renewal Pending, 5= Late Renewal Pending, 6 = 30 days, 7 = 60 days, 8 = Delinquent, 9 = Voided, 10 = Refund Requested, 11 = Refunded");
        }
    }
}
