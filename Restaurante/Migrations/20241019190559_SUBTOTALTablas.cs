using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restaurante.Migrations
{
    /// <inheritdoc />
    public partial class SUBTOTALTablas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Subtotal",
                table: "DetallesPedidos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Subtotal",
                table: "DetallesPedidos",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
