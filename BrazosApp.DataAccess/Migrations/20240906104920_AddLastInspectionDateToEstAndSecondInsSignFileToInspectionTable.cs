using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrazosApp.DataAccess.Migrations
{
    public partial class AddLastInspectionDateToEstAndSecondInsSignFileToInspectionTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SecondaryInspectorSignFile",
                table: "RFMFInspectionData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "Status",
                table: "FeesDetailsTables",
                type: "tinyint",
                nullable: false,
                comment: "1 = Pending, 2 = Paid, 3 = Cancelled, 4 = Renewal Pending, 5= Late Renewal Pending, 6 = 30 days, 7 = 60 days, 8 = Delinquent",
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldComment: "1 = Pending, 2 = Paid, 3 = Cancelled");

            migrationBuilder.AlterColumn<byte>(
                name: "Status",
                table: "Fees",
                type: "tinyint",
                nullable: false,
                comment: "1 = Pending, 2 = Paid, 3 = Cancelled, 4 = Renewal Pending, 5= Late Renewal Pending, 6 = 30 days, 7 = 60 days, 8 = Delinquent",
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldComment: "1 = Pending, 2 = Paid, 3 = Cancelled");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastInspectionDate",
                table: "Establishments",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SecondaryInspectorSignFile",
                table: "RFMFInspectionData");

            migrationBuilder.DropColumn(
                name: "LastInspectionDate",
                table: "Establishments");

            migrationBuilder.AlterColumn<byte>(
                name: "Status",
                table: "FeesDetailsTables",
                type: "tinyint",
                nullable: false,
                comment: "1 = Pending, 2 = Paid, 3 = Cancelled",
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldComment: "1 = Pending, 2 = Paid, 3 = Cancelled, 4 = Renewal Pending, 5= Late Renewal Pending, 6 = 30 days, 7 = 60 days, 8 = Delinquent");

            migrationBuilder.AlterColumn<byte>(
                name: "Status",
                table: "Fees",
                type: "tinyint",
                nullable: false,
                comment: "1 = Pending, 2 = Paid, 3 = Cancelled",
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldComment: "1 = Pending, 2 = Paid, 3 = Cancelled, 4 = Renewal Pending, 5= Late Renewal Pending, 6 = 30 days, 7 = 60 days, 8 = Delinquent");
        }
    }
}
