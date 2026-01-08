using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MinikodEnvanterWeb.Migrations
{
    /// <inheritdoc />
    public partial class mmsj : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DigerMarka",
                table: "Kategoriler",
                newName: "DigerKategori");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DigerKategori",
                table: "Kategoriler",
                newName: "DigerMarka");
        }
    }
}
