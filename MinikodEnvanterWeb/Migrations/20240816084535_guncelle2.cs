using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MinikodEnvanterWeb.Migrations
{
    /// <inheritdoc />
    public partial class guncelle2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Urunler_Kategoriler_KategorilerKategoriId",
                table: "Urunler");

            migrationBuilder.DropForeignKey(
                name: "FK_Urunler_Markalar_MarkalarMarkaID",
                table: "Urunler");

            migrationBuilder.DropIndex(
                name: "IX_Urunler_KategorilerKategoriId",
                table: "Urunler");

            migrationBuilder.DropIndex(
                name: "IX_Urunler_MarkalarMarkaID",
                table: "Urunler");

            migrationBuilder.DropColumn(
                name: "KategorilerKategoriId",
                table: "Urunler");

            migrationBuilder.DropColumn(
                name: "MarkalarMarkaID",
                table: "Urunler");

            migrationBuilder.AddColumn<int>(
                name: "MarkalarMarkaID",
                table: "Kategoriler",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Urunler_KategoriID",
                table: "Urunler",
                column: "KategoriID");

            migrationBuilder.CreateIndex(
                name: "IX_Urunler_MarkaID",
                table: "Urunler",
                column: "MarkaID");

            migrationBuilder.CreateIndex(
                name: "IX_Kategoriler_MarkalarMarkaID",
                table: "Kategoriler",
                column: "MarkalarMarkaID");

            migrationBuilder.AddForeignKey(
                name: "FK_Kategoriler_Markalar_MarkalarMarkaID",
                table: "Kategoriler",
                column: "MarkalarMarkaID",
                principalTable: "Markalar",
                principalColumn: "MarkaID");

            migrationBuilder.AddForeignKey(
                name: "FK_Urunler_Kategoriler_KategoriID",
                table: "Urunler",
                column: "KategoriID",
                principalTable: "Kategoriler",
                principalColumn: "KategoriId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Urunler_Markalar_MarkaID",
                table: "Urunler",
                column: "MarkaID",
                principalTable: "Markalar",
                principalColumn: "MarkaID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Kategoriler_Markalar_MarkalarMarkaID",
                table: "Kategoriler");

            migrationBuilder.DropForeignKey(
                name: "FK_Urunler_Kategoriler_KategoriID",
                table: "Urunler");

            migrationBuilder.DropForeignKey(
                name: "FK_Urunler_Markalar_MarkaID",
                table: "Urunler");

            migrationBuilder.DropIndex(
                name: "IX_Urunler_KategoriID",
                table: "Urunler");

            migrationBuilder.DropIndex(
                name: "IX_Urunler_MarkaID",
                table: "Urunler");

            migrationBuilder.DropIndex(
                name: "IX_Kategoriler_MarkalarMarkaID",
                table: "Kategoriler");

            migrationBuilder.DropColumn(
                name: "MarkalarMarkaID",
                table: "Kategoriler");

            migrationBuilder.AddColumn<int>(
                name: "KategorilerKategoriId",
                table: "Urunler",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MarkalarMarkaID",
                table: "Urunler",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Urunler_KategorilerKategoriId",
                table: "Urunler",
                column: "KategorilerKategoriId");

            migrationBuilder.CreateIndex(
                name: "IX_Urunler_MarkalarMarkaID",
                table: "Urunler",
                column: "MarkalarMarkaID");

            migrationBuilder.AddForeignKey(
                name: "FK_Urunler_Kategoriler_KategorilerKategoriId",
                table: "Urunler",
                column: "KategorilerKategoriId",
                principalTable: "Kategoriler",
                principalColumn: "KategoriId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Urunler_Markalar_MarkalarMarkaID",
                table: "Urunler",
                column: "MarkalarMarkaID",
                principalTable: "Markalar",
                principalColumn: "MarkaID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
