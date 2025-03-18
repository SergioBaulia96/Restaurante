using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restaurante.Migrations
{
    /// <inheritdoc />
    public partial class MigracionInicial8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_Clientes_ClienteID",
                table: "Pedidos");

            migrationBuilder.AlterColumn<int>(
                name: "ClienteID",
                table: "Pedidos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_Clientes_ClienteID",
                table: "Pedidos",
                column: "ClienteID",
                principalTable: "Clientes",
                principalColumn: "ClienteID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_Clientes_ClienteID",
                table: "Pedidos");

            migrationBuilder.AlterColumn<int>(
                name: "ClienteID",
                table: "Pedidos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_Clientes_ClienteID",
                table: "Pedidos",
                column: "ClienteID",
                principalTable: "Clientes",
                principalColumn: "ClienteID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
