using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrazosApp.DataAccess.Migrations
{
    public partial class UpdateFeesAndPaymentStructure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountNumber",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "ApprovalStatus",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "BankName",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "CardNumber",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "CardType",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "CollectionMode",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "EffectiveDate",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "GetPaymentApiCallOn",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "GetPaymentApiCallStatus",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "GetPaymentApiMessage",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "HostAuthorizationCode",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "HostTransactionId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "NameOnCard",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "PaymentReceiptConfirmation",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "RedirectApiCallOn",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "RedirectApiCallStatus",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "RedirectApiMessage",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "RedirectUrlCallApiStatus",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "RoutingNumber",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "VoidCredit",
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

            migrationBuilder.RenameColumn(
                name: "RefundedDate",
                table: "Payments",
                newName: "RefundVoidDate");

            migrationBuilder.RenameColumn(
                name: "RefundedBy",
                table: "Payments",
                newName: "RefundVoidBy");

            migrationBuilder.RenameColumn(
                name: "RedirectUrlCallOn",
                table: "Payments",
                newName: "InvoiceDate");

            migrationBuilder.RenameColumn(
                name: "ReasonForRefunding",
                table: "Payments",
                newName: "ReasonForRefundVoid");

            migrationBuilder.AlterColumn<byte>(
                name: "PaymentStatus",
                table: "Payments",
                type: "tinyint",
                nullable: false,
                comment: "1 = Pending, 2 = Paid, 3 = Cancelled, 4 = Failure, 5 = Refunded, 6 = Voided",
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldComment: "1 = Pending, 2 = Paid, 3 = Cancelled, 4 = Failure, 5 = Refunded");

            migrationBuilder.AlterColumn<int>(
                name: "PaymentBy",
                table: "Payments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "EstablishmentId",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "InvoiceBy",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "VoidedTransactionNo",
                table: "Payments",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "PaymentStatus",
                table: "PaymentDetailsTable",
                type: "tinyint",
                nullable: false,
                comment: "1 = Pending, 2 = Paid, 3 = Cancelled, 4 = Failure, 5 = Refunded, 6 = Voided",
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldComment: "1 = Pending, 2 = Paid, 3 = Cancelled, 4 = Failure, 5 = Refunded");

            migrationBuilder.AlterColumn<int>(
                name: "PaymentBy",
                table: "PaymentDetailsTable",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "RefundVoidBy",
                table: "PaymentDetailsTable",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RefundVoidDate",
                table: "PaymentDetailsTable",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PaymentOnlineInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentId = table.Column<int>(type: "int", nullable: false),
                    RedirectApiCallStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, comment: "0 = Cancelled, 1 = Declined, 2 = Approved"),
                    RedirectApiMessage = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    RedirectApiCallOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RedirectUrlCallApiStatus = table.Column<byte>(type: "tinyint", nullable: true),
                    RedirectUrlCallOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovalStatus = table.Column<byte>(type: "tinyint", nullable: true),
                    PaymentReceiptConfirmation = table.Column<int>(type: "int", nullable: true),
                    EffectiveDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CardType = table.Column<byte>(type: "tinyint", nullable: true),
                    NameOnCard = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CardNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    BankName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RoutingNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    AccountNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CollectionMode = table.Column<byte>(type: "tinyint", nullable: true),
                    HostTransactionId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    HostAuthorizationCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    VoidCredit = table.Column<byte>(type: "tinyint", nullable: true),
                    GetPaymentApiCallStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    GetPaymentApiMessage = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    GetPaymentApiCallOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentOnlineInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentOnlineInfo_Payments_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "Payments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentOnlineInfo_PaymentId",
                table: "PaymentOnlineInfo",
                column: "PaymentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentOnlineInfo");

            migrationBuilder.DropColumn(
                name: "EstablishmentId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "InvoiceBy",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "VoidedTransactionNo",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "RefundVoidBy",
                table: "PaymentDetailsTable");

            migrationBuilder.DropColumn(
                name: "RefundVoidDate",
                table: "PaymentDetailsTable");

            migrationBuilder.RenameColumn(
                name: "RefundVoidDate",
                table: "Payments",
                newName: "RefundedDate");

            migrationBuilder.RenameColumn(
                name: "RefundVoidBy",
                table: "Payments",
                newName: "RefundedBy");

            migrationBuilder.RenameColumn(
                name: "ReasonForRefundVoid",
                table: "Payments",
                newName: "ReasonForRefunding");

            migrationBuilder.RenameColumn(
                name: "InvoiceDate",
                table: "Payments",
                newName: "RedirectUrlCallOn");

            migrationBuilder.AlterColumn<byte>(
                name: "PaymentStatus",
                table: "Payments",
                type: "tinyint",
                nullable: false,
                comment: "1 = Pending, 2 = Paid, 3 = Cancelled, 4 = Failure, 5 = Refunded",
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldComment: "1 = Pending, 2 = Paid, 3 = Cancelled, 4 = Failure, 5 = Refunded, 6 = Voided");

            migrationBuilder.AlterColumn<int>(
                name: "PaymentBy",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AccountNumber",
                table: "Payments",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "ApprovalStatus",
                table: "Payments",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankName",
                table: "Payments",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CardNumber",
                table: "Payments",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "CardType",
                table: "Payments",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "CollectionMode",
                table: "Payments",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EffectiveDate",
                table: "Payments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "GetPaymentApiCallOn",
                table: "Payments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GetPaymentApiCallStatus",
                table: "Payments",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GetPaymentApiMessage",
                table: "Payments",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HostAuthorizationCode",
                table: "Payments",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HostTransactionId",
                table: "Payments",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameOnCard",
                table: "Payments",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PaymentReceiptConfirmation",
                table: "Payments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RedirectApiCallOn",
                table: "Payments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RedirectApiCallStatus",
                table: "Payments",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                comment: "0 = Cancelled, 1 = Declined, 2 = Approved");

            migrationBuilder.AddColumn<string>(
                name: "RedirectApiMessage",
                table: "Payments",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "RedirectUrlCallApiStatus",
                table: "Payments",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoutingNumber",
                table: "Payments",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "VoidCredit",
                table: "Payments",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "PaymentStatus",
                table: "PaymentDetailsTable",
                type: "tinyint",
                nullable: false,
                comment: "1 = Pending, 2 = Paid, 3 = Cancelled, 4 = Failure, 5 = Refunded",
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldComment: "1 = Pending, 2 = Paid, 3 = Cancelled, 4 = Failure, 5 = Refunded, 6 = Voided");

            migrationBuilder.AlterColumn<int>(
                name: "PaymentBy",
                table: "PaymentDetailsTable",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

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
    }
}
