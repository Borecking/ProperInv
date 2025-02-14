using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProperTax.Models
{
    public class SumaPowierzchni
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]   //Ten atrybut ustawia ze RokMiesiac nie jest automatycznie inkrementowany w bazie danych tylko ustawiany przez uzytkownika
        [Display(Name = "Rok-Miesiąc")]
        public DateTime RokMiesiac { get; set; }
        [Display(Name = "[Grunty] Suma powierzchni działki mieszkalnej [m^2]")]
        public double SumaPowierzchniKategoriaGruntyPowierzchniaDzialkiMieszkalnej { get; set; }
        [Display(Name = "[Grunty] Suma powierzchni działki NIEmieszkalnej [m^2]")]
        public double SumaPowierzchniKategoriaGruntyPowierzchniaDzialkiNiemieszkalnej { get; set; }
        [Display(Name = "[Budynki] Suma powierzchni użytkowa mieszkalna [m^2]")]
        public double SumaPowierzchniKategoriaBudynkiPowierzchniaUzytkowaMieszkalna { get; set; }
        [Display(Name = "[Budynki] Suma powierzchni użytkowa NIEmieszkalna [m^2]")]
        public double SumaPowierzchniKategoriaBudynkiPowierzchniaUzytkowaNiemieszkalna { get; set; }
        [Display(Name = "[Budowle] Suma wartości budowli [zł]")]
        public double SumaPowierzchniKategoriaWartoscBudowli { get; set; }
    }
}
