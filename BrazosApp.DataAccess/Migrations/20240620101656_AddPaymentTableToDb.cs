using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrazosApp.DataAccess.Migrations
{
    public partial class AddPaymentTableToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FeesId = table.Column<int>(type: "int", nullable: false),
                    InvoiceNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    RedirectApiCallStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, comment: "0 = Cancelled, 1 = Declined, 2 = Approved"),
                    RedirectApiMessage = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    RedirectApiCallOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RedirectUrlCallApiStatus = table.Column<byte>(type: "tinyint", nullable: true),
                    RedirectUrlCallOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovalStatus = table.Column<byte>(type: "tinyint", nullable: true),
                    PaymentReceiptConfirmation = table.Column<int>(type: "int", nullable: true),
                    EffectiveDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PaymentMethod = table.Column<byte>(type: "tinyint", nullable: true),
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
                    GetPaymentApiCallOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PaymentStatus = table.Column<byte>(type: "tinyint", nullable: false, comment: "1 = Pending, 2 = Success, 3 = Failure"),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_Fees_FeesId",
                        column: x => x.FeesId,
                        principalTable: "Fees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Payments_FeesId",
                table: "Payments",
                column: "FeesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payments");
        }
    }
}
