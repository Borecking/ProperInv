using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProperTax.Data;
using ProperTax.Models;

namespace ProperTax.Controllers
{
    [Authorize]
    public class NieruchomosciController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NieruchomosciController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Nieruchomosci
        public async Task<IActionResult> Index()
        {
            return View(await _context.Nieruchomosci.ToListAsync());
        }

        // GET: Nieruchomosci/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nieruchomosc = await _context.Nieruchomosci
                .FirstOrDefaultAsync(m => m.Id == id);
            if (nieruchomosc == null)
            {
                return NotFound();
            }

            return View(nieruchomosc);
        }

        // GET: Nieruchomosci/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Nieruchomosci/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NrKsiegiWieczystej,Adres,NrObrebu,IdDzialki,Udzial100m,PowierzchniaUzytkowaBudynku,KategoriaGruntyPowierzchniaDzialkiMieszkalnej,KategoriaGruntyPowierzchniaDzialkiNiemieszkalnej,KategoriaBudynkiPowierzchniaUzytkowaMieszkalna,KategoriaBudynkiPowierzchniaUzytkowaNiemieszkalna,KategoriaWartoscBudowli,FormaWladania,DataKupienia,DataSprzedania,Komentarz")] Nieruchomosc nieruchomosc)
        {
            //Dzieki tej ifologii w formularzu zostaną przesłane domyślne wartości gdy użytkownik zostawi te pola puste
            //Probowalem ustawic domyslne wartosci w klasie Nieruchomosc ale nie dzialalo i do bazy przesylane byly wartosci null
            if (!nieruchomosc.NrObrebu.HasValue)
            {
                nieruchomosc.NrObrebu = 0; // Ustaw domyślną wartość
            }
            if (!nieruchomosc.IdDzialki.HasValue)
            {
                nieruchomosc.IdDzialki = 0; // Ustaw domyślną wartość
            }
            if (!nieruchomosc.PowierzchniaUzytkowaBudynku.HasValue)
            {
                nieruchomosc.PowierzchniaUzytkowaBudynku = 0; // Ustaw domyślną wartość
            }
            if (!nieruchomosc.KategoriaGruntyPowierzchniaDzialkiMieszkalnej.HasValue)
            {
                nieruchomosc.KategoriaGruntyPowierzchniaDzialkiMieszkalnej = 0; // Ustaw domyślną wartość
            }
            if (!nieruchomosc.KategoriaGruntyPowierzchniaDzialkiNiemieszkalnej.HasValue)
            {
                nieruchomosc.KategoriaGruntyPowierzchniaDzialkiNiemieszkalnej = 0; // Ustaw domyślną wartość
            }
            if (!nieruchomosc.KategoriaBudynkiPowierzchniaUzytkowaMieszkalna.HasValue)
            {
                nieruchomosc.KategoriaBudynkiPowierzchniaUzytkowaMieszkalna = 0; // Ustaw domyślną wartość
            }
            if (!nieruchomosc.KategoriaBudynkiPowierzchniaUzytkowaNiemieszkalna.HasValue)
            {
                nieruchomosc.KategoriaBudynkiPowierzchniaUzytkowaNiemieszkalna = 0; // Ustaw domyślną wartość
            }
            if (!nieruchomosc.KategoriaWartoscBudowli.HasValue)
            {
                nieruchomosc.KategoriaWartoscBudowli = 0; // Ustaw domyślną wartość
            }
            if (string.IsNullOrWhiteSpace(nieruchomosc.Komentarz))
            {
                nieruchomosc.Komentarz = ""; // Ustaw domyślną wartość
            }
            if (string.IsNullOrWhiteSpace(nieruchomosc.FormaWladania))
            {
                nieruchomosc.FormaWladania = "własność"; // Ustaw domyślną wartość
            }
            nieruchomosc.Udzial100m = string.Concat((Math.Max(nieruchomosc.KategoriaBudynkiPowierzchniaUzytkowaMieszkalna ?? 0, nieruchomosc.KategoriaBudynkiPowierzchniaUzytkowaNiemieszkalna ?? 0) * 100).ToString(), "/", (nieruchomosc.PowierzchniaUzytkowaBudynku * 100).ToString());

            //Sprawdza czy data kupienia jest mniejsza niz sprzedania
            if (nieruchomosc.DataSprzedania.HasValue)
            {
                if (nieruchomosc.DataSprzedania < nieruchomosc.DataKupienia)
                {
                    ModelState.AddModelError("DataSprzedania", "Data sprzedaży nie może być wcześniejsza niż data kupienia.");
                }
            }

            // Sprawdzenie, czy rok zakupu istnieje w tabeli StawkiPodatkow
            bool rokZakupuIstnieje = _context.StawkiPodatkow.Any(s => s.Rok == nieruchomosc.DataKupienia.Year);
            if (!rokZakupuIstnieje)
            {
                ModelState.AddModelError("DataKupienia", "Przed wpisaniem daty z tym rokiem musisz dodać go w Stawki podatków.");
            }

            // Sprawdzenie, czy rok sprzedaży istnieje w tabeli StawkiPodatkow
            if (nieruchomosc.DataSprzedania.HasValue)
            {
                bool rokSprzedazyIstnieje = _context.StawkiPodatkow.Any(s => s.Rok == nieruchomosc.DataSprzedania.Value.Year);
                if (!rokSprzedazyIstnieje)
                {
                    ModelState.AddModelError("DataSprzedania", "Przed wpisaniem daty z tym rokiem musisz dodać go w Stawki podatków.");
                }
            }

            if (ModelState.IsValid)
            {
                _context.Add(nieruchomosc);
                await _context.SaveChangesAsync();

                // Pobranie listy miesięcy posiadania tej nieruchomosci
                var miesiace = await PobierzMiesiace(nieruchomosc.Id);

                // Wypisanie listy miesięcy w konsoli
                /*Debug.WriteLine("====================================================");
                Debug.WriteLine("Miesiące: " + string.Join(", ", miesiace.Select(m => m.ToString("yyyy-MM"))));
                Debug.WriteLine("====================================================");*/

                // Pobranie z tabeli SumyPowierzchni rekordów, które pasują do pobranych miesięcy
                var pasujacePowierzchnie = await _context.SumyPowierzchni
                    .Where(sp => miesiace.Contains(sp.RokMiesiac)) // Filtrujemy rekordy, które pasują do miesięcy
                    .ToListAsync();

                // Wypisanie pasujących rekordów w konsoli (jeśli chcesz)
                Debug.WriteLine("====================================================");
                Debug.WriteLine("Pasujące rekordy: " + string.Join(", ", pasujacePowierzchnie.Select(sp => sp.RokMiesiac.ToString("yyyy-MM"))));
                Debug.WriteLine("====================================================");


                foreach (var miesiac in pasujacePowierzchnie)
                {
                    Debug.WriteLine($"Przed aktualizacją: RokMiesiac: {miesiac.RokMiesiac.ToString("yyyy-MM")}");
                    Debug.WriteLine($"SumaPowierzchniKategoriaGruntyPowierzchniaDzialkiMieszkalnej: {miesiac.SumaPowierzchniKategoriaGruntyPowierzchniaDzialkiMieszkalnej}");

                    // Aktualizacja wartości
                    miesiac.SumaPowierzchniKategoriaGruntyPowierzchniaDzialkiMieszkalnej += nieruchomosc.KategoriaGruntyPowierzchniaDzialkiMieszkalnej ?? 0;
                    miesiac.SumaPowierzchniKategoriaGruntyPowierzchniaDzialkiNiemieszkalnej += nieruchomosc.KategoriaGruntyPowierzchniaDzialkiNiemieszkalnej ?? 0;
                    miesiac.SumaPowierzchniKategoriaBudynkiPowierzchniaUzytkowaMieszkalna += nieruchomosc.KategoriaBudynkiPowierzchniaUzytkowaMieszkalna ?? 0;
                    miesiac.SumaPowierzchniKategoriaBudynkiPowierzchniaUzytkowaNiemieszkalna += nieruchomosc.KategoriaBudynkiPowierzchniaUzytkowaNiemieszkalna ?? 0;
                    miesiac.SumaPowierzchniKategoriaWartoscBudowli += nieruchomosc.KategoriaWartoscBudowli ?? 0;

                    Debug.WriteLine($"Po aktualizacji: RokMiesiac: {miesiac.RokMiesiac.ToString("yyyy-MM")}");
                    Debug.WriteLine($"SumaPowierzchniKategoriaGruntyPowierzchniaDzialkiMieszkalnej: {miesiac.SumaPowierzchniKategoriaGruntyPowierzchniaDzialkiMieszkalnej}");
                }
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(nieruchomosc);
        }

        // GET: Nieruchomosci/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nieruchomosc = await _context.Nieruchomosci.FindAsync(id);
            if (nieruchomosc == null)
            {
                return NotFound();
            }
            return View(nieruchomosc);
        }

        // POST: Nieruchomosci/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NrKsiegiWieczystej,Adres,NrObrebu,IdDzialki,Udzial100m,PowierzchniaUzytkowaBudynku,KategoriaGruntyPowierzchniaDzialkiMieszkalnej,KategoriaGruntyPowierzchniaDzialkiNiemieszkalnej,KategoriaBudynkiPowierzchniaUzytkowaMieszkalna,KategoriaBudynkiPowierzchniaUzytkowaNiemieszkalna,KategoriaWartoscBudowli,FormaWladania,DataKupienia,DataSprzedania,Komentarz")] Nieruchomosc nieruchomosc)
        {
            if (id != nieruchomosc.Id)
            {
                return NotFound();
            }

            //Dzieki tej ifologii w formularzu zostaną przesłane domyślne wartości gdy użytkownik zostawi te pola puste
            //Probowalem ustawic domyslne wartosci w klasie Nieruchomosc ale nie dzialalo i do bazy przesylane byly wartosci null
            if (!nieruchomosc.NrObrebu.HasValue)
            {
                nieruchomosc.NrObrebu = 0; // Ustaw domyślną wartość
            }
            if (!nieruchomosc.IdDzialki.HasValue)
            {
                nieruchomosc.IdDzialki = 0; // Ustaw domyślną wartość
            }
            if (!nieruchomosc.PowierzchniaUzytkowaBudynku.HasValue)
            {
                nieruchomosc.PowierzchniaUzytkowaBudynku = 0; // Ustaw domyślną wartość
            }
            if (!nieruchomosc.KategoriaGruntyPowierzchniaDzialkiMieszkalnej.HasValue)
            {
                nieruchomosc.KategoriaGruntyPowierzchniaDzialkiMieszkalnej = 0; // Ustaw domyślną wartość
            }
            if (!nieruchomosc.KategoriaGruntyPowierzchniaDzialkiNiemieszkalnej.HasValue)
            {
                nieruchomosc.KategoriaGruntyPowierzchniaDzialkiNiemieszkalnej = 0; // Ustaw domyślną wartość
            }
            if (!nieruchomosc.KategoriaBudynkiPowierzchniaUzytkowaMieszkalna.HasValue)
            {
                nieruchomosc.KategoriaBudynkiPowierzchniaUzytkowaMieszkalna = 0; // Ustaw domyślną wartość
            }
            if (!nieruchomosc.KategoriaBudynkiPowierzchniaUzytkowaNiemieszkalna.HasValue)
            {
                nieruchomosc.KategoriaBudynkiPowierzchniaUzytkowaNiemieszkalna = 0; // Ustaw domyślną wartość
            }
            if (!nieruchomosc.KategoriaWartoscBudowli.HasValue)
            {
                nieruchomosc.KategoriaWartoscBudowli = 0; // Ustaw domyślną wartość
            }
            if (string.IsNullOrWhiteSpace(nieruchomosc.Komentarz))
            {
                nieruchomosc.Komentarz = ""; // Ustaw domyślną wartość
            }
            if (string.IsNullOrWhiteSpace(nieruchomosc.FormaWladania))
            {
                nieruchomosc.FormaWladania = "własność"; // Ustaw domyślną wartość
            }
            nieruchomosc.Udzial100m = string.Concat((Math.Max(nieruchomosc.KategoriaBudynkiPowierzchniaUzytkowaMieszkalna ?? 0, nieruchomosc.KategoriaBudynkiPowierzchniaUzytkowaNiemieszkalna ?? 0) * 100).ToString(), "/", (nieruchomosc.PowierzchniaUzytkowaBudynku * 100).ToString());

            //Sprawdza czy data kupienia jest mniejsza niz sprzedania
            if (nieruchomosc.DataSprzedania.HasValue)
            {
                if (nieruchomosc.DataSprzedania < nieruchomosc.DataKupienia)
                {
                    ModelState.AddModelError("DataSprzedania", "Data sprzedaży nie może być wcześniejsza niż data kupienia.");
                }
            }

            // Sprawdzenie, czy rok zakupu istnieje w tabeli StawkiPodatkow
            bool rokZakupuIstnieje = _context.StawkiPodatkow.Any(s => s.Rok == nieruchomosc.DataKupienia.Year);
            if (!rokZakupuIstnieje)
            {
                ModelState.AddModelError("DataKupienia", "Przed wpisaniem daty z tym rokiem musisz dodać go w Stawki podatków.");
            }

            // Sprawdzenie, czy rok sprzedaży istnieje w tabeli StawkiPodatkow
            if (nieruchomosc.DataSprzedania.HasValue)
            {
                bool rokSprzedazyIstnieje = _context.StawkiPodatkow.Any(s => s.Rok == nieruchomosc.DataSprzedania.Value.Year);
                if (!rokSprzedazyIstnieje)
                {
                    ModelState.AddModelError("DataSprzedania", "Przed wpisaniem daty z tym rokiem musisz dodać go w Stawki podatków.");
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //zapisanie wartosci przed edit
                    double KategoriaGruntyPowierzchniaDzialkiMieszkalnejPrzedZmiana = nieruchomosc.KategoriaGruntyPowierzchniaDzialkiMieszkalnej ?? 0;
                    double KategoriaGruntyPowierzchniaDzialkiNiemieszkalnejPrzedZmiana = nieruchomosc.KategoriaGruntyPowierzchniaDzialkiNiemieszkalnej ?? 0;
                    double KategoriaBudynkiPowierzchniaUzytkowaMieszkalnaPrzedZmiana = nieruchomosc.KategoriaBudynkiPowierzchniaUzytkowaMieszkalna ?? 0;
                    double KategoriaBudynkiPowierzchniaUzytkowaNiemieszkalnaPrzedZmiana = nieruchomosc.KategoriaBudynkiPowierzchniaUzytkowaNiemieszkalna ?? 0;
                    double KategoriaWartoscBudowliPrzedZmiana = nieruchomosc.KategoriaWartoscBudowli ?? 0;

                    

                    _context.Update(nieruchomosc);
                    await _context.SaveChangesAsync();

                    // Pobranie listy miesięcy posiadania tej nieruchomości
                    var miesiace = await PobierzMiesiace(nieruchomosc.Id);

                    // Pobranie pasujących rekordów z SumyPowierzchni
                    var pasujacePowierzchnie = await _context.SumyPowierzchni
                        .Where(sp => miesiace.Contains(sp.RokMiesiac))
                        .ToListAsync();

                    foreach (var miesiac in pasujacePowierzchnie)
                    {
                        miesiac.SumaPowierzchniKategoriaGruntyPowierzchniaDzialkiMieszkalnej = miesiac.SumaPowierzchniKategoriaGruntyPowierzchniaDzialkiMieszkalnej + (nieruchomosc.KategoriaGruntyPowierzchniaDzialkiMieszkalnej ?? 0) - KategoriaGruntyPowierzchniaDzialkiMieszkalnejPrzedZmiana;
                        miesiac.SumaPowierzchniKategoriaGruntyPowierzchniaDzialkiNiemieszkalnej += (nieruchomosc.KategoriaGruntyPowierzchniaDzialkiNiemieszkalnej ?? 0) - KategoriaGruntyPowierzchniaDzialkiNiemieszkalnejPrzedZmiana;
                        miesiac.SumaPowierzchniKategoriaBudynkiPowierzchniaUzytkowaMieszkalna += (nieruchomosc.KategoriaBudynkiPowierzchniaUzytkowaMieszkalna ?? 0) - KategoriaBudynkiPowierzchniaUzytkowaMieszkalnaPrzedZmiana;
                        miesiac.SumaPowierzchniKategoriaBudynkiPowierzchniaUzytkowaNiemieszkalna += (nieruchomosc.KategoriaBudynkiPowierzchniaUzytkowaNiemieszkalna ?? 0) - KategoriaBudynkiPowierzchniaUzytkowaNiemieszkalnaPrzedZmiana;
                        miesiac.SumaPowierzchniKategoriaWartoscBudowli += (nieruchomosc.KategoriaWartoscBudowli ?? 0) - KategoriaWartoscBudowliPrzedZmiana;

                        /*Debug.WriteLine("====================================================");
                        Debug.WriteLine((nieruchomosc.KategoriaGruntyPowierzchniaDzialkiMieszkalnej ?? 0) + " - " + KategoriaGruntyPowierzchniaDzialkiMieszkalnejPrzedZmiana);
                        Debug.WriteLine("====================================================");
                        */
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NieruchomoscExists(nieruchomosc.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            return View(nieruchomosc);
        }

        // GET: Nieruchomosci/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nieruchomosc = await _context.Nieruchomosci
                .FirstOrDefaultAsync(m => m.Id == id);
            if (nieruchomosc == null)
            {
                return NotFound();
            }

            return View(nieruchomosc);
        }

        // POST: Nieruchomosci/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var nieruchomosc = await _context.Nieruchomosci.FindAsync(id);
            if (nieruchomosc != null)
            {
                // Pobranie listy miesięcy posiadania tej nieruchomości
                var miesiace = await PobierzMiesiace(nieruchomosc.Id);

                // Pobranie pasujących rekordów z SumyPowierzchni
                var pasujacePowierzchnie = await _context.SumyPowierzchni
                    .Where(sp => miesiace.Contains(sp.RokMiesiac))
                    .ToListAsync();

                foreach (var miesiac in pasujacePowierzchnie)
                {
                    // Odejmowanie wartości
                    miesiac.SumaPowierzchniKategoriaGruntyPowierzchniaDzialkiMieszkalnej -= nieruchomosc.KategoriaGruntyPowierzchniaDzialkiMieszkalnej ?? 0;
                    miesiac.SumaPowierzchniKategoriaGruntyPowierzchniaDzialkiNiemieszkalnej -= nieruchomosc.KategoriaGruntyPowierzchniaDzialkiNiemieszkalnej ?? 0;
                    miesiac.SumaPowierzchniKategoriaBudynkiPowierzchniaUzytkowaMieszkalna -= nieruchomosc.KategoriaBudynkiPowierzchniaUzytkowaMieszkalna ?? 0;
                    miesiac.SumaPowierzchniKategoriaBudynkiPowierzchniaUzytkowaNiemieszkalna -= nieruchomosc.KategoriaBudynkiPowierzchniaUzytkowaNiemieszkalna ?? 0;
                    miesiac.SumaPowierzchniKategoriaWartoscBudowli -= nieruchomosc.KategoriaWartoscBudowli ?? 0;
                }

                _context.Nieruchomosci.Remove(nieruchomosc);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool NieruchomoscExists(int id)
        {
            return _context.Nieruchomosci.Any(e => e.Id == id);
        }


        public async Task<List<DateTime>> PobierzMiesiace(int id)
        {
            var nieruchomosc = await _context.Nieruchomosci.FindAsync(id);

            //jak jest data kupienia i sprzedazy
            if (nieruchomosc?.DataKupienia != null && nieruchomosc?.DataSprzedania != null)
            {
                var listaMiesiecy = new List<DateTime>();

                var data = new DateTime(nieruchomosc.DataKupienia.Year, nieruchomosc.DataKupienia.Month, 1);
                var koniec = nieruchomosc.DataSprzedania.Value;

                while (data <= koniec)
                {
                    listaMiesiecy.Add(new DateTime(data.Year, data.Month, 1));
                    data = data.AddMonths(1);
                }

                return listaMiesiecy;
            }

            //jak jest tylko data kupienia
            if (nieruchomosc?.DataKupienia != null && nieruchomosc?.DataSprzedania == null)
            {
                var listaMiesiecy = new List<DateTime>();

                var data = new DateTime(nieruchomosc.DataKupienia.Year, nieruchomosc.DataKupienia.Month, 1); 
                var rokKoniec = await _context.SumyPowierzchni
                    .OrderByDescending(sp => sp.RokMiesiac.Year)
                    .Select(sp => sp.RokMiesiac.Year)
                    .FirstOrDefaultAsync();
                var koniec = new DateTime(rokKoniec, 12, 1);

                while (data <= koniec)
                {
                    listaMiesiecy.Add(new DateTime(data.Year, data.Month, 1));
                    data = data.AddMonths(1);
                }

                return listaMiesiecy;
            }

            return new List<DateTime>();
        }
    }
}
