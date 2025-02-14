using System.ComponentModel.DataAnnotations;

namespace ProperTax.Models
{
    public class Nieruchomosc
    {
        public int Id { get; set; }
        [Display(Name = "Nr Księgi wieczystej")]
        public required string NrKsiegiWieczystej { get; set; }
        [Display(Name = "Adres")]
        public required string Adres { get; set; }
        [Range(0, 2147483647, ErrorMessage = "Liczba nie może być ujemna.")]
        [Display(Name = "Nr Obrębu")]
        public int? NrObrebu { get; set; }
        [Range(0, 2147483647, ErrorMessage = "Liczba nie może być ujemna.")]
        [Display(Name = "Identyfikator działki")]
        public int? IdDzialki { get; set; }
        [Display(Name = "Udział [100m]")]
        public string? Udzial100m { get; set; }
        [Range(0, 2147483647, ErrorMessage = "Powierzchnia nie może być ujemna i ma 2 miejsca po przecinku.")]
        [Display(Name = "Powierzchnia użytkowa budynku [m^2]")]
        public double? PowierzchniaUzytkowaBudynku { get; set; }
        [Range(0, 2147483647, ErrorMessage = "Powierzchnia nie może być ujemna i ma 2 miejsca po przecinku.")]
        [Display(Name = "[Grunty] Powierzchnia działki mieszkalnej [m^2]")]
        public double? KategoriaGruntyPowierzchniaDzialkiMieszkalnej { get; set; }
        [Range(0, 2147483647, ErrorMessage = "Powierzchnia nie może być ujemna i ma 2 miejsca po przecinku.")]
        [Display(Name = "[Grunty] Powierzchnia działki NIEmieszkalnej [m^2]")]
        public double? KategoriaGruntyPowierzchniaDzialkiNiemieszkalnej { get; set; }
        [Range(0, 2147483647, ErrorMessage = "Powierzchnia nie może być ujemna i ma 2 miejsca po przecinku.")]
        [Display(Name = "[Budynki] Powierzchnia użytkowa mieszkalna [m^2]")]
        public double? KategoriaBudynkiPowierzchniaUzytkowaMieszkalna { get; set; }
        [Range(0, 2147483647, ErrorMessage = "Powierzchnia nie może być ujemna i ma 2 miejsca po przecinku.")]
        [Display(Name = "[Budynki] Powierzchnia użytkowa NIEmieszkalna [m^2]")]
        public double? KategoriaBudynkiPowierzchniaUzytkowaNiemieszkalna { get; set; }
        [Range(0, 2147483647, ErrorMessage = "Wartość nie może być ujemna i ma 2 miejsca po przecinku.")]
        [Display(Name = "[Budowle] Wartość budowli [zł]")]
        public double? KategoriaWartoscBudowli { get; set; }
        [Display(Name = "Forma władania")]
        public string? FormaWladania { get; set; }
        [Display(Name = "Data zakupienia")]
        public required DateTime DataKupienia { get; set; }
        [Display(Name = "Data sprzedaży")]
        public DateTime? DataSprzedania { get; set; }
        public string? Komentarz { get; set; }
    }
}
