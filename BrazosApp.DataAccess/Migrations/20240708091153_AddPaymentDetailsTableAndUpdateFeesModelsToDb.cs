using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrazosApp.DataAccess.Migrations
{
    public partial class AddPaymentDetailsTableAndUpdateFeesModelsToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fees_EstablishmentTypes_EstablishmentTypeId",
                table: "Fees");

            migrationBuilder.DropIndex(
                name: "IX_Fees_EstablishmentTypeId",
                table: "Fees");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "FeesDetailsTables");

            migrationBuilder.DropColumn(
                name: "EstablishmentTypeId",
                table: "Fees");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Payments",
                newName: "PaymentBy");

            migrationBuilder.RenameColumn(
                name: "BaseAmount",
                table: "FeesDetailsTables",
                newName: "Amount");

            migrationBuilder.AlterColumn<byte>(
                name: "PaymentStatus",
                table: "Payments",
                type: "tinyint",
                nullable: false,
                comment: "1 = Pending, 2 = Paid, 3 = Cancelled, 4 = Failure",
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldComment: "1 = Pending, 2 = Success, 3 = Failure");

            migrationBuilder.AlterColumn<byte>(
                name: "PaymentMethod",
                table: "Payments",
                type: "tinyint",
                nullable: true,
                comment: "0 = CreditCard, 1 = ECheck, 2 = Cash, 3 = Cheque",
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentOn",
                table: "Payments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "PaymentType",
                table: "Payments",
                type: "tinyint",
                nullable: true,
                comment: "1 = Online, 2 = Offline");

            migrationBuilder.AddColumn<byte>(
                name: "Status",
                table: "FeesDetailsTables",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                comment: "1 = Pending, 2 = Paid, 3 = Cancelled");

            migrationBuilder.AlterColumn<byte>(
                name: "Status",
                table: "Fees",
                type: "tinyint",
                nullable: false,
                comment: "1 = Pending, 2 = Paid, 3 = Cancelled",
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldComment: "1 = Pending, 2 = Success, 3 = Failure");

            migrationBuilder.AddColumn<string>(
                name: "InvoiceNo",
                table: "Fees",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PaymentDetailsTable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PaymentStatus = table.Column<byte>(type: "tinyint", nullable: false, comment: "1 = Pending, 2 = Paid, 3 = Cancelled, 4 = Failure"),
                    PaymentBy = table.Column<int>(type: "int", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentDetailsTable", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentDetailsTable_Payments_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "Payments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentDetailsTable_PaymentId",
                table: "PaymentDetailsTable",
                column: "PaymentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentDetailsTable");

            migrationBuilder.DropColumn(
                name: "PaymentOn",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "PaymentType",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "FeesDetailsTables");

            migrationBuilder.DropColumn(
                name: "InvoiceNo",
                table: "Fees");

            migrationBuilder.RenameColumn(
                name: "PaymentBy",
                table: "Payments",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "FeesDetailsTables",
                newName: "BaseAmount");

            migrationBuilder.AlterColumn<byte>(
                name: "PaymentStatus",
                table: "Payments",
                type: "tinyint",
                nullable: false,
                comment: "1 = Pending, 2 = Success, 3 = Failure",
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldComment: "1 = Pending, 2 = Paid, 3 = Cancelled, 4 = Failure");

            migrationBuilder.AlterColumn<byte>(
                name: "PaymentMethod",
                table: "Payments",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true,
                oldComment: "0 = CreditCard, 1 = ECheck, 2 = Cash, 3 = Cheque");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Payments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "FeesDetailsTables",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<byte>(
                name: "Status",
                table: "Fees",
                type: "tinyint",
                nullable: false,
                comment: "1 = Pending, 2 = Success, 3 = Failure",
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldComment: "1 = Pending, 2 = Paid, 3 = Cancelled");

            migrationBuilder.AddColumn<int>(
                name: "EstablishmentTypeId",
                table: "Fees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Fees_EstablishmentTypeId",
                table: "Fees",
                column: "EstablishmentTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Fees_EstablishmentTypes_EstablishmentTypeId",
                table: "Fees",
                column: "EstablishmentTypeId",
                principalTable: "EstablishmentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
