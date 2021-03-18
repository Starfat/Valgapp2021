using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Valgapplikasjon.Data;
using Valgapplikasjon.Models;


namespace Valgapplikasjon.Controllers
{   
    [Authorize]
    public class MittKandidaturController : Controller
    {
        private readonly KandidaturDbContext _context;

        public MittKandidaturController(KandidaturDbContext context)
        {
            _context = context;
        }

        // GET: MittKandidatur
        public async Task<IActionResult> Index()
        {
            return View(await _context.Kandidat.ToListAsync());
        }

        // GET: MittKandidatur/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mittKandidaturModel = await _context.Kandidat
                .FirstOrDefaultAsync(m => m.KandidatId == id);
            if (mittKandidaturModel == null)
            {
                return NotFound();
            }

            return View(mittKandidaturModel);
        }

        // GET: MittKandidatur/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MittKandidatur/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("KandidatId,BrukerId,Sjekkboks")] MittKandidaturModel mittKandidaturModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mittKandidaturModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid) {
                return View("Home/Error");
            }
            return View(mittKandidaturModel);
        }

        // GET: MittKandidatur/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mittKandidaturModel = await _context.Kandidat.FindAsync(id);
            if (mittKandidaturModel == null)
            {
                return NotFound();
            }
            return View(mittKandidaturModel);
        }

        // POST: MittKandidatur/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("KandidatId,BrukerId,Sjekkboks")] MittKandidaturModel mittKandidaturModel)
        {
            if (id != mittKandidaturModel.KandidatId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mittKandidaturModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MittKandidaturModelExists(mittKandidaturModel.KandidatId))
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
            return View(mittKandidaturModel);
        }

        // GET: MittKandidatur/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mittKandidaturModel = await _context.Kandidat
                .FirstOrDefaultAsync(m => m.KandidatId == id);
            if (mittKandidaturModel == null)
            {
                return NotFound();
            }

            return View(mittKandidaturModel);
        }

        // POST: MittKandidatur/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mittKandidaturModel = await _context.Kandidat.FindAsync(id);
            _context.Kandidat.Remove(mittKandidaturModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MittKandidaturModelExists(int id)
        {
            return _context.Kandidat.Any(e => e.KandidatId == id);
        }
    }
}
