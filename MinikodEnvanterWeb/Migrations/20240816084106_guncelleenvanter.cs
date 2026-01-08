using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MinikodEnvanterWeb.Migrations
{
    /// <inheritdoc />
    public partial class guncelleenvanter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UrunMarkasi",
                table: "Urunler");

            migrationBuilder.AddColumn<int>(
                name: "MarkaID",
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

            migrationBuilder.CreateTable(
                name: "Markalar",
                columns: table => new
                {
                    MarkaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MarkaAdi = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Markalar", x => x.MarkaID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Urunler_MarkalarMarkaID",
                table: "Urunler",
                column: "MarkalarMarkaID");

            migrationBuilder.AddForeignKey(
                name: "FK_Urunler_Markalar_MarkalarMarkaID",
                table: "Urunler",
                column: "MarkalarMarkaID",
                principalTable: "Markalar",
                principalColumn: "MarkaID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Urunler_Markalar_MarkalarMarkaID",
                table: "Urunler");

            migrationBuilder.DropTable(
                name: "Markalar");

            migrationBuilder.DropIndex(
                name: "IX_Urunler_MarkalarMarkaID",
                table: "Urunler");

            migrationBuilder.DropColumn(
                name: "MarkaID",
                table: "Urunler");

            migrationBuilder.DropColumn(
                name: "MarkalarMarkaID",
                table: "Urunler");

            migrationBuilder.AddColumn<string>(
                name: "UrunMarkasi",
                table: "Urunler",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
