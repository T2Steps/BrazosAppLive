using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrazosApp.DataAccess.Migrations
{
    public partial class AddTitleFieldAndDropEstTypeForeignKeyInFeesDetTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeesDetailsTables_EstablishmentTypes_EstablishmentTypeId",
                table: "FeesDetailsTables");

            migrationBuilder.DropIndex(
                name: "IX_FeesDetailsTables_EstablishmentTypeId",
                table: "FeesDetailsTables");

            migrationBuilder.AlterColumn<int>(
                name: "EstablishmentTypeId",
                table: "FeesDetailsTables",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "FeesDetailsTables",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "FeesDetailsTables");

            migrationBuilder.AlterColumn<int>(
                name: "EstablishmentTypeId",
                table: "FeesDetailsTables",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FeesDetailsTables_EstablishmentTypeId",
                table: "FeesDetailsTables",
                column: "EstablishmentTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_FeesDetailsTables_EstablishmentTypes_EstablishmentTypeId",
                table: "FeesDetailsTables",
                column: "EstablishmentTypeId",
                principalTable: "EstablishmentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
