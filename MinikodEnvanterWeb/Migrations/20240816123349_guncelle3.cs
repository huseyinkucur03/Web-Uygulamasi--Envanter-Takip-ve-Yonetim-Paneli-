using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MinikodEnvanterWeb.Migrations
{
    /// <inheritdoc />
    public partial class guncelle3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CalismayanSayisi",
                table: "Urunler",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CalismayanSayisi",
                table: "Urunler");
        }
    }
}
