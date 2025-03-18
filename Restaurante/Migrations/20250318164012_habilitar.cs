using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restaurante.Migrations
{
    /// <inheritdoc />
    public partial class habilitar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Activo",
                table: "Meseros",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Disponible",
                table: "Menus",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Activo",
                table: "Clientes",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Activo",
                table: "Meseros");

            migrationBuilder.DropColumn(
                name: "Disponible",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "Activo",
                table: "Clientes");
        }
    }
}
