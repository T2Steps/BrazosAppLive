using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrazosApp.DataAccess.Migrations
{
    public partial class AddRenewalTempHistoryTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte>(
                name: "PaymentStatus",
                table: "Payments",
                type: "tinyint",
                nullable: false,
                comment: "1 = Pending, 2 = Paid, 3 = Cancelled, 4 = Failure, 5 = Refunded, 6 = Voided, 7 = Renewal",
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldComment: "1 = Pending, 2 = Paid, 3 = Cancelled, 4 = Failure, 5 = Refunded, 6 = Voided");

            migrationBuilder.AlterColumn<byte>(
                name: "PaymentStatus",
                table: "PaymentDetailsTable",
                type: "tinyint",
                nullable: false,
                comment: "1 = Pending, 2 = Paid, 3 = Cancelled, 4 = Failure, 5 = Refunded, 6 = Voided, 7 = Renewal",
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldComment: "1 = Pending, 2 = Paid, 3 = Cancelled, 4 = Failure, 5 = Refunded, 6 = Voided");

            migrationBuilder.CreateTable(
                name: "RenewalTempHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EstId = table.Column<int>(type: "int", nullable: false),
                    Est_Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PermitNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Owner = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FeesAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RenewalTempHistories", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RenewalTempHistories");

            migrationBuilder.AlterColumn<byte>(
                name: "PaymentStatus",
                table: "Payments",
                type: "tinyint",
                nullable: false,
                comment: "1 = Pending, 2 = Paid, 3 = Cancelled, 4 = Failure, 5 = Refunded, 6 = Voided",
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldComment: "1 = Pending, 2 = Paid, 3 = Cancelled, 4 = Failure, 5 = Refunded, 6 = Voided, 7 = Renewal");

            migrationBuilder.AlterColumn<byte>(
                name: "PaymentStatus",
                table: "PaymentDetailsTable",
                type: "tinyint",
                nullable: false,
                comment: "1 = Pending, 2 = Paid, 3 = Cancelled, 4 = Failure, 5 = Refunded, 6 = Voided",
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldComment: "1 = Pending, 2 = Paid, 3 = Cancelled, 4 = Failure, 5 = Refunded, 6 = Voided, 7 = Renewal");
        }
    }
}
