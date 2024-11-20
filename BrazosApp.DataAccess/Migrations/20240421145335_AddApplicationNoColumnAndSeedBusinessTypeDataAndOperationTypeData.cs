using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrazosApp.DataAccess.Migrations
{
    public partial class AddApplicationNoColumnAndSeedBusinessTypeDataAndOperationTypeData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationNo",
                table: "Applications",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "BusinessTypes",
                columns: new[] { "Id", "Code", "IsActive", "Name", "SpName" },
                values: new object[,]
                {
                    { 1, "RF", true, "Restaurant/food production", "Restaurante/producción de comida" },
                    { 2, "RF", true, "Establishment with TCS food production", "Establecimiento con preparación de alimentos o bebidas TCS" },
                    { 3, "RF", true, "Establishment without TCS food production", "Establecimiento sin preparación de alimentos o bebidas TCS" },
                    { 4, "RF", true, "Bar without TCS ingredients (beverages only)", "Bar con ingredientes TCS o alimentos" },
                    { 5, "RF", true, "Bar with TCS ingredients and/or food", "Bar sin ingredientes TCS" },
                    { 6, "RF", true, "Retail with TCS pre-packaged products (no preparation)", "Venta con productos TCS preenvasados (sin preparación)" },
                    { 7, "RF", true, "Grocery Store", "Tienda de comida" },
                    { 8, "RF", true, "Long term care facility", "Centro de cuidados a largo plazo" },
                    { 9, "RF", true, "Hospital", "Hospital" },
                    { 10, "RF", true, "Farmer’s Market", "Mercado de agricultores" },
                    { 11, "RF", true, "School", "Escuela" },
                    { 12, "RF", true, "Daycare", "Guardería de niños" },
                    { 13, "RF", true, "Central Preparation Facility", "Instalación central de preparación" },
                    { 14, "RF", true, "Non-profit 501C (must include proof)", "Sin fines de lucro 501C (debe incluir prueba)" },
                    { 15, "MF", true, "Food Truck with TCS foods", "Camión de comida con alimentos TCS" },
                    { 16, "MF", true, "Food Truck without TCS foods", "Camión de comida sin alimentos TCS" },
                    { 17, "MF", true, "Food Pushcart with TCS foods", "Carrito de comida con alimentos TCS" },
                    { 18, "MF", true, "Food Pushcart without TCS foods", "Carrito de comida sin alimentos TCS" },
                    { 19, "MF", true, "Roadside Vendor with TCS foods", "Vendedor ambulante con alimentos TCS" },
                    { 20, "MF", true, "Roadside Vendor without TCS foods", "Vendedor ambúlate sin alimentos TCS" }
                });

            migrationBuilder.InsertData(
                table: "OperationTypes",
                columns: new[] { "Id", "Code", "IsActive", "Name", "SpName" },
                values: new object[,]
                {
                    { 1, "RF", true, "Annually (12 months)", "Anual (12 meses)" },
                    { 2, "RF", true, "Seasonal (4 months)", "Temporal (4 meses)" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BusinessTypes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "BusinessTypes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "BusinessTypes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "BusinessTypes",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "BusinessTypes",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "BusinessTypes",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "BusinessTypes",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "BusinessTypes",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "BusinessTypes",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "BusinessTypes",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "BusinessTypes",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "BusinessTypes",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "BusinessTypes",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "BusinessTypes",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "BusinessTypes",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "BusinessTypes",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "BusinessTypes",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "BusinessTypes",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "BusinessTypes",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "BusinessTypes",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "OperationTypes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "OperationTypes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DropColumn(
                name: "ApplicationNo",
                table: "Applications");
        }
    }
}
