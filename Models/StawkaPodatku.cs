using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProperTax.Models
{
    public class StawkaPodatku
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]   //Ten atrybut ustawia ze Rok nie jest automatycznie inkrementowany w bazie danych tylko ustawiany przez uzytkownika
        [Range(2000, 2100, ErrorMessage = "Czy to dobry rok? 🤔")]
        public int Rok { get; set; }
        [Range(0, 2147483647, ErrorMessage = "Stawka nie może być ujemna i ma 2 miejsca po przecinku.")]
        [Display(Name = "[Grunty] Stauwka od powierzchni działki mieszkalnej [zł/m^2]")]
        public double StawkaKategoriiGruntyPowierzchniaDzialkiMieszkalnej { get; set; }
        [Range(0, 2147483647, ErrorMessage = "Stawka nie może być ujemna i ma 2 miejsca po przecinku.")]
        [Display(Name = "[Grunty] Stawka od powierzchni działki NIEmieszkalnej [zł/m^2]")]
        public double StawkaKategoriiGruntyPowierzchniaDzialkiNiemieszkalnej { get; set; }
        [Range(0, 2147483647, ErrorMessage = "Stawka nie może być ujemna i ma 2 miejsca po przecinku.")]
        [Display(Name = "[Budynki] Stawka od powierzchni użytkowej mieszkalnej [zł/m^2]")]
        public double StawkaKategoriiBudynkiPowierzchniaUzytkowaMieszkalna { get; set; }
        [Range(0, 2147483647, ErrorMessage = "Stawka nie może być ujemna i ma 2 miejsca po przecinku.")]
        [Display(Name = "[Budynki] Stawka od powierzchni użytkowej NIEmieszkalnej [zł/m^2]")]
        public double StawkaKategoriiBudynkiPowierzchniaUzytkowaNiemieszkalna { get; set; }
        [Range(0, 2147483647, ErrorMessage = "Stawka nie może być ujemna i ma 2 miejsca po przecinku.")]
        [Display(Name = "[Budowle] Stawka od wartości budowli [% wartości]")]
        public double StawkaKategoriiWartoscBudowli { get; set; }
        public string? Komentarz { get; set; }
    }
}
