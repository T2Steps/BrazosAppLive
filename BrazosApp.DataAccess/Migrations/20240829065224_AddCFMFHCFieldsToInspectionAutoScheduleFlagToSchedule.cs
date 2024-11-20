using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrazosApp.DataAccess.Migrations
{
    public partial class AddCFMFHCFieldsToInspectionAutoScheduleFlagToSchedule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ChequeNumber",
                table: "Payments",
                newName: "ReferenceNumber");

            migrationBuilder.AddColumn<bool>(
                name: "IsAutoSchedule",
                table: "Schedules",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "CFM",
                table: "RFMFInspectionData",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CFMExpiryDate",
                table: "RFMFInspectionData",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FHC",
                table: "RFMFInspectionData",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "PaymentMethod",
                table: "Payments",
                type: "tinyint",
                nullable: true,
                comment: "0 = CreditCard, 1 = ECheck, 2 = Cash, 3 = Cheque, 4 = Card, 5 = Money Order",
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true,
                oldComment: "0 = CreditCard, 1 = ECheck, 2 = Cash, 3 = Cheque, 4 = Card");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAutoSchedule",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "CFM",
                table: "RFMFInspectionData");

            migrationBuilder.DropColumn(
                name: "CFMExpiryDate",
                table: "RFMFInspectionData");

            migrationBuilder.DropColumn(
                name: "FHC",
                table: "RFMFInspectionData");

            migrationBuilder.RenameColumn(
                name: "ReferenceNumber",
                table: "Payments",
                newName: "ChequeNumber");

            migrationBuilder.AlterColumn<byte>(
                name: "PaymentMethod",
                table: "Payments",
                type: "tinyint",
                nullable: true,
                comment: "0 = CreditCard, 1 = ECheck, 2 = Cash, 3 = Cheque, 4 = Card",
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true,
                oldComment: "0 = CreditCard, 1 = ECheck, 2 = Cash, 3 = Cheque, 4 = Card, 5 = Money Order");
        }
    }
}
