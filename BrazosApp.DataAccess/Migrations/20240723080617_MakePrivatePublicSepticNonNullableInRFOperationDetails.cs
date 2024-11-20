using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrazosApp.DataAccess.Migrations
{
    public partial class MakePrivatePublicSepticNonNullableInRFOperationDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RFOperationDetails_PrivateSeptic_PrivateSepticId",
                table: "RFOperationDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_RFOperationDetails_PublicSewage_PublicSewageId",
                table: "RFOperationDetails");

            migrationBuilder.AlterColumn<int>(
                name: "PublicSewageId",
                table: "RFOperationDetails",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "PrivateSepticId",
                table: "RFOperationDetails",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_RFOperationDetails_PrivateSeptic_PrivateSepticId",
                table: "RFOperationDetails",
                column: "PrivateSepticId",
                principalTable: "PrivateSeptic",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RFOperationDetails_PublicSewage_PublicSewageId",
                table: "RFOperationDetails",
                column: "PublicSewageId",
                principalTable: "PublicSewage",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RFOperationDetails_PrivateSeptic_PrivateSepticId",
                table: "RFOperationDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_RFOperationDetails_PublicSewage_PublicSewageId",
                table: "RFOperationDetails");

            migrationBuilder.AlterColumn<int>(
                name: "PublicSewageId",
                table: "RFOperationDetails",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PrivateSepticId",
                table: "RFOperationDetails",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RFOperationDetails_PrivateSeptic_PrivateSepticId",
                table: "RFOperationDetails",
                column: "PrivateSepticId",
                principalTable: "PrivateSeptic",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RFOperationDetails_PublicSewage_PublicSewageId",
                table: "RFOperationDetails",
                column: "PublicSewageId",
                principalTable: "PublicSewage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
