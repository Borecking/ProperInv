using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProperTax.Data.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Nieruchomosci",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NrKsiegiWieczystej = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Adres = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NrObrebu = table.Column<int>(type: "int", nullable: true),
                    IdDzialki = table.Column<int>(type: "int", nullable: true),
                    Udzial100m = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PowierzchniaUzytkowaBudynku = table.Column<double>(type: "float", nullable: true),
                    KategoriaGruntyPowierzchniaDzialkiMieszkalnej = table.Column<double>(type: "float", nullable: true),
                    KategoriaGruntyPowierzchniaDzialkiNiemieszkalnej = table.Column<double>(type: "float", nullable: true),
                    KategoriaBudynkiPowierzchniaUzytkowaMieszkalna = table.Column<double>(type: "float", nullable: true),
                    KategoriaBudynkiPowierzchniaUzytkowaNiemieszkalna = table.Column<double>(type: "float", nullable: true),
                    KategoriaWartoscBudowli = table.Column<double>(type: "float", nullable: true),
                    FormaWladania = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataKupienia = table.Column<DateTime>(type: "date", nullable: false),
                    DataSprzedania = table.Column<DateTime>(type: "date", nullable: true),
                    Komentarz = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nieruchomosci", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StawkiPodatkow",
                columns: table => new
                {
                    Rok = table.Column<int>(type: "int", nullable: false),
                    StawkaKategoriiGruntyPowierzchniaDzialkiMieszkalnej = table.Column<double>(type: "float", nullable: false),
                    StawkaKategoriiGruntyPowierzchniaDzialkiNiemieszkalnej = table.Column<double>(type: "float", nullable: false),
                    StawkaKategoriiBudynkiPowierzchniaUzytkowaMieszkalna = table.Column<double>(type: "float", nullable: false),
                    StawkaKategoriiBudynkiPowierzchniaUzytkowaNiemieszkalna = table.Column<double>(type: "float", nullable: false),
                    StawkaKategoriiWartoscBudowli = table.Column<double>(type: "float", nullable: false),
                    Komentarz = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StawkiPodatkow", x => x.Rok);
                });

            migrationBuilder.CreateTable(
                name: "SumyPowierzchni",
                columns: table => new
                {
                    RokMiesiac = table.Column<DateTime>(type: "date", nullable: false),
                    SumaPowierzchniKategoriaGruntyPowierzchniaDzialkiMieszkalnej = table.Column<double>(type: "float", nullable: false),
                    SumaPowierzchniKategoriaGruntyPowierzchniaDzialkiNiemieszkalnej = table.Column<double>(type: "float", nullable: false),
                    SumaPowierzchniKategoriaBudynkiPowierzchniaUzytkowaMieszkalna = table.Column<double>(type: "float", nullable: false),
                    SumaPowierzchniKategoriaBudynkiPowierzchniaUzytkowaNiemieszkalna = table.Column<double>(type: "float", nullable: false),
                    SumaPowierzchniKategoriaWartoscBudowli = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SumyPowierzchni", x => x.RokMiesiac);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Nieruchomosci");

            migrationBuilder.DropTable(
                name: "StawkiPodatkow");

            migrationBuilder.DropTable(
                name: "SumyPowierzchni");
        }
    }
}
