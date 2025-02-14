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
    public class SumyPowierzchniController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SumyPowierzchniController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SumyPowierzchni
        public async Task<IActionResult> Index()
        {
            return View(await _context.SumyPowierzchni.ToListAsync());
        }

        // GET: SumyPowierzchni/Details/5
        public async Task<IActionResult> Details(DateTime? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sumaPowierzchni = await _context.SumyPowierzchni
                .FirstOrDefaultAsync(m => m.RokMiesiac == id);
            if (sumaPowierzchni == null)
            {
                return NotFound();
            }

            return View(sumaPowierzchni);
        }
    }
}
