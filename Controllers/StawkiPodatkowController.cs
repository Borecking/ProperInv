using System;
using System.Collections.Generic;
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
    public class StawkiPodatkowController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StawkiPodatkowController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: StawkiPodatkow
        public async Task<IActionResult> Index()
        {
            return View(await _context.StawkiPodatkow.ToListAsync());
        }

        // GET: StawkiPodatkow/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stawkaPodatku = await _context.StawkiPodatkow
                .FirstOrDefaultAsync(m => m.Rok == id);
            if (stawkaPodatku == null)
            {
                return NotFound();
            }

            return View(stawkaPodatku);
        }

        // GET: StawkiPodatkow/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: StawkiPodatkow/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Rok,StawkaKategoriiGruntyPowierzchniaDzialkiMieszkalnej,StawkaKategoriiGruntyPowierzchniaDzialkiNiemieszkalnej,StawkaKategoriiBudynkiPowierzchniaUzytkowaMieszkalna,StawkaKategoriiBudynkiPowierzchniaUzytkowaNiemieszkalna,StawkaKategoriiWartoscBudowli,Komentarz")] StawkaPodatku stawkaPodatku)
        {
            // Sprawdzenie, czy Rok jest unikalny
            if (_context.StawkiPodatkow.Any(s => s.Rok == stawkaPodatku.Rok))
            {
                // Dodanie komunikatu z linkiem do widoku Index
                ModelState.AddModelError("Rok", "Ten rok został już zapisany. Możesz go znaleźć i zedytować w Stawki podatków.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(stawkaPodatku);

                //sprawdzenie czy istnieja juz miesiace dla tego roku
                if (_context.SumyPowierzchni.Any(s => s.RokMiesiac.Year == stawkaPodatku.Rok))
                {
                    ModelState.AddModelError("", "Miesiące dla tego roku już istnieją w SumaPowierzchni");
                    return View(stawkaPodatku);
                }

                // Tworzenie 12 instancji SumaPowierzchni czyli 12 miesiecy dodawanego roku
                List<SumaPowierzchni> miesiace = new List<SumaPowierzchni>();
                for (int month = 1; month <= 12; month++)
                {
                    var miesiac = new SumaPowierzchni
                    {
                        RokMiesiac = new DateTime(stawkaPodatku.Rok, month, 1),
                        SumaPowierzchniKategoriaGruntyPowierzchniaDzialkiMieszkalnej = 0,
                        SumaPowierzchniKategoriaGruntyPowierzchniaDzialkiNiemieszkalnej = 0,
                        SumaPowierzchniKategoriaBudynkiPowierzchniaUzytkowaMieszkalna = 0,
                        SumaPowierzchniKategoriaBudynkiPowierzchniaUzytkowaNiemieszkalna = 0,
                        SumaPowierzchniKategoriaWartoscBudowli = 0
                    };

                    miesiace.Add(miesiac);
                }

                // Dodanie listy do bazy danych
                _context.SumyPowierzchni.AddRange(miesiace);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(stawkaPodatku);
        }

        // GET: StawkiPodatkow/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stawkaPodatku = await _context.StawkiPodatkow.FindAsync(id);
            if (stawkaPodatku == null)
            {
                return NotFound();
            }

            // Pobierz powiązane nieruchomości
            var powiazaneNieruchomosci = _context.Nieruchomosci
                .Where(n => n.DataKupienia.Year <= id &&
                            (n.DataSprzedania == null || n.DataSprzedania.Value.Year >= id))
                .ToList();

            // Przekaż informacje do widoku
            ViewBag.PowiazaneNieruchomosci = powiazaneNieruchomosci;
            ViewBag.IloscNieruchomosci = powiazaneNieruchomosci.Count;

            return View(stawkaPodatku);
        }

        // POST: StawkiPodatkow/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Rok,StawkaKategoriiGruntyPowierzchniaDzialkiMieszkalnej,StawkaKategoriiGruntyPowierzchniaDzialkiNiemieszkalnej,StawkaKategoriiBudynkiPowierzchniaUzytkowaMieszkalna,StawkaKategoriiBudynkiPowierzchniaUzytkowaNiemieszkalna,StawkaKategoriiWartoscBudowli,Komentarz")] StawkaPodatku stawkaPodatku)
        {
            if (id != stawkaPodatku.Rok)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stawkaPodatku);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StawkaPodatkuExists(stawkaPodatku.Rok))
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
            return View(stawkaPodatku);
        }

        // GET: StawkiPodatkow/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stawkaPodatku = await _context.StawkiPodatkow
                .FirstOrDefaultAsync(m => m.Rok == id);
            if (stawkaPodatku == null)
            {
                return NotFound();
            }

            // Pobierz powiązane nieruchomości
            var powiazaneNieruchomosci = _context.Nieruchomosci
                .Where(n => stawkaPodatku.Rok >= n.DataKupienia.Year &&
                    (n.DataSprzedania == null || stawkaPodatku.Rok <= n.DataSprzedania.Value.Year)).ToList();

            // Przygotuj widok z modelem
            ViewBag.PowiazaneNieruchomosci = powiazaneNieruchomosci;
            ViewBag.IloscNieruchomosci = powiazaneNieruchomosci.Count;

            return View(stawkaPodatku);
        }

        // POST: StawkiPodatkow/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Pobranie obiektu stawki podatkowej do usunięcia
            var stawkaPodatku = await _context.StawkiPodatkow.FindAsync(id);

            if (stawkaPodatku == null)
            {
                return NotFound();
            }

            // Pobranie powiązanych miesięcy z SumaPowierzchni dla usuwanego roku
            var powiazaneMiesiace = _context.SumyPowierzchni
                .Where(s => s.RokMiesiac.Year == id)
                .ToList();

            // Usunięcie powiązanych miesięcy
            _context.SumyPowierzchni.RemoveRange(powiazaneMiesiace);

            // Pobranie powiązanych nieruchomości, których rok zakupu i rok sprzedaży (jeśli istnieje) 
            // mieszczą się w zakresie roku stawki podatkowej
            var powiazaneNieruchomosci = _context.Nieruchomosci
                .Where(n => n.DataKupienia.Year <= id &&
                            (n.DataSprzedania == null || n.DataSprzedania.Value.Year >= id))
                .ToList();

            // Usunięcie powiązanych nieruchomości
            _context.Nieruchomosci.RemoveRange(powiazaneNieruchomosci);

            // Usunięcie stawki podatkowej
            _context.StawkiPodatkow.Remove(stawkaPodatku);

            // Zapisanie zmian w bazie danych
            await _context.SaveChangesAsync();

            // Przekierowanie do widoku Index po usunięciu
            return RedirectToAction(nameof(Index));
        }

        private bool StawkaPodatkuExists(int id)
        {
            return _context.StawkiPodatkow.Any(e => e.Rok == id);
        }
    }
}
