using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrazosApp.DataAccess.Migrations
{
    public partial class AddApplicationRelatedTablesAndSeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_ApplicationType_ApplicationTypeId",
                table: "Applications");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationType_LanguageType_LanguageTypeId",
                table: "ApplicationType");

            migrationBuilder.DropTable(
                name: "PermitType");

            migrationBuilder.DropTable(
                name: "SubCategory");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationType_LanguageTypeId",
                table: "ApplicationType");

            migrationBuilder.DeleteData(
                table: "ApplicationType",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "ApplicationType",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "ApplicationType",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "ApplicationType",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "ApplicationType",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "ApplicationType",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "ApplicationType",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "ApplicationType",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "ApplicationType",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "ApplicationType",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "ApplicationType",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DropColumn(
                name: "LanguageTypeId",
                table: "ApplicationType");

            migrationBuilder.DropColumn(
                name: "IsExpired",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "IsVerified",
                table: "Applications");

            migrationBuilder.RenameColumn(
                name: "ApplicationTypeId",
                table: "Applications",
                newName: "ApplicationForId");

            migrationBuilder.RenameIndex(
                name: "IX_Applications_ApplicationTypeId",
                table: "Applications",
                newName: "IX_Applications_ApplicationForId");

            migrationBuilder.AddColumn<byte>(
                name: "Status",
                table: "Applications",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                comment: "1='Not Verified', 2='Verified', 3='Expired'");

            migrationBuilder.CreateTable(
                name: "ApplicationFor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationTypeId = table.Column<int>(type: "int", nullable: false),
                    LanguageTypeId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationFor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationFor_ApplicationType_ApplicationTypeId",
                        column: x => x.ApplicationTypeId,
                        principalTable: "ApplicationType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationFor_LanguageType_LanguageTypeId",
                        column: x => x.LanguageTypeId,
                        principalTable: "LanguageType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ApplicationFor",
                columns: new[] { "Id", "ApplicationTypeId", "Code", "IsActive", "LanguageTypeId", "Name" },
                values: new object[,]
                {
                    { 1, 1, "RF", true, 1, "Retail Food New" },
                    { 2, 1, "RF", true, 2, "Retail Food New (Spanish)" },
                    { 3, 1, "MF", true, 1, "Mobile Food New" },
                    { 4, 1, "MF", true, 2, "Mobile Food New (Spanish)" },
                    { 5, 1, "RF", true, 1, "Retail Food Change of Owner" },
                    { 6, 1, "RF", true, 2, "Retail Food Change of Owner (Spanish)" },
                    { 7, 1, "MF", true, 1, "Mobile Food Change of Owner" },
                    { 8, 1, "MF", true, 2, "Mobile Food Change of Owner (Spanish)" },
                    { 9, 1, "TF", true, 1, "Temporary Food" },
                    { 10, 1, "TF", true, 2, "Temporary Food (Spanish)" },
                    { 11, 2, "FH", true, 1, "Foster Home" },
                    { 12, 3, "DS", true, 1, "Daycare Sanitation" },
                    { 13, 1, "FHEA", true, 1, "Food Handler Enrollment Application" },
                    { 14, 1, "FHEA", true, 2, "Food Handler Enrollment Application (Spanish)" },
                    { 15, 4, "PL", true, 1, "Pools" },
                    { 16, 5, "C", true, 1, "Complaints" }
                });

            migrationBuilder.UpdateData(
                table: "ApplicationType",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Food");

            migrationBuilder.UpdateData(
                table: "ApplicationType",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Foster Home");

            migrationBuilder.UpdateData(
                table: "ApplicationType",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "Daycare Sanitation");

            migrationBuilder.UpdateData(
                table: "ApplicationType",
                keyColumn: "Id",
                keyValue: 4,
                column: "Name",
                value: "Pools");

            migrationBuilder.UpdateData(
                table: "ApplicationType",
                keyColumn: "Id",
                keyValue: 5,
                column: "Name",
                value: "Complaints");

            migrationBuilder.InsertData(
                table: "PermitStatus",
                columns: new[] { "Id", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1, true, "Incomplete" },
                    { 2, true, "Pending Admin Review" },
                    { 3, true, "Admin Review" },
                    { 4, true, "Pending Plan Review" },
                    { 5, true, "Plan Review" },
                    { 6, true, "Pending Payment" },
                    { 7, true, "Pre-Opening" },
                    { 8, true, "Active" },
                    { 9, true, "Renewal" },
                    { 10, true, "Expired" },
                    { 11, true, "Area 51" },
                    { 12, true, "Inactive" },
                    { 13, true, "Closed" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationFor_ApplicationTypeId",
                table: "ApplicationFor",
                column: "ApplicationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationFor_LanguageTypeId",
                table: "ApplicationFor",
                column: "LanguageTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_ApplicationFor_ApplicationForId",
                table: "Applications",
                column: "ApplicationForId",
                principalTable: "ApplicationFor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_ApplicationFor_ApplicationForId",
                table: "Applications");

            migrationBuilder.DropTable(
                name: "ApplicationFor");

            migrationBuilder.DeleteData(
                table: "PermitStatus",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PermitStatus",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "PermitStatus",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "PermitStatus",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "PermitStatus",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "PermitStatus",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "PermitStatus",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "PermitStatus",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "PermitStatus",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "PermitStatus",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "PermitStatus",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "PermitStatus",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "PermitStatus",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Applications");

            migrationBuilder.RenameColumn(
                name: "ApplicationForId",
                table: "Applications",
                newName: "ApplicationTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Applications_ApplicationForId",
                table: "Applications",
                newName: "IX_Applications_ApplicationTypeId");

            migrationBuilder.AddColumn<int>(
                name: "LanguageTypeId",
                table: "ApplicationType",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsExpired",
                table: "Applications",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVerified",
                table: "Applications",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PermitType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermitType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubCategory_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "ApplicationType",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "LanguageTypeId", "Name" },
                values: new object[] { 1, "Retail Food New" });

            migrationBuilder.UpdateData(
                table: "ApplicationType",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "LanguageTypeId", "Name" },
                values: new object[] { 2, "Retail Food New (Spanish)" });

            migrationBuilder.UpdateData(
                table: "ApplicationType",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "LanguageTypeId", "Name" },
                values: new object[] { 1, "Mobile Food New" });

            migrationBuilder.UpdateData(
                table: "ApplicationType",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "LanguageTypeId", "Name" },
                values: new object[] { 2, "Mobile Food New (Spanish)" });

            migrationBuilder.UpdateData(
                table: "ApplicationType",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "LanguageTypeId", "Name" },
                values: new object[] { 1, "Retail Food Change of Owner" });

            migrationBuilder.InsertData(
                table: "ApplicationType",
                columns: new[] { "Id", "IsActive", "LanguageTypeId", "Name" },
                values: new object[,]
                {
                    { 6, true, 2, "Retail Food Change of Owner (Spanish)" },
                    { 7, true, 1, "Mobile Food Change of Owner" },
                    { 8, true, 2, "Mobile Food Change of Owner (Spanish)" },
                    { 9, true, 1, "Temporary Food" },
                    { 10, true, 2, "Temporary Food (Spanish)" },
                    { 11, true, 1, "Foster Home" },
                    { 12, true, 1, "Daycare Sanitation" },
                    { 13, true, 1, "Food Handler Enrollment Application" },
                    { 14, true, 2, "Food Handler Enrollment Application (Spanish)" },
                    { 15, true, 1, "Pools" },
                    { 16, true, 1, "Complaints" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationType_LanguageTypeId",
                table: "ApplicationType",
                column: "LanguageTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SubCategory_CategoryId",
                table: "SubCategory",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_ApplicationType_ApplicationTypeId",
                table: "Applications",
                column: "ApplicationTypeId",
                principalTable: "ApplicationType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationType_LanguageType_LanguageTypeId",
                table: "ApplicationType",
                column: "LanguageTypeId",
                principalTable: "LanguageType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
