using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjetoG6.Data;
using ProjetoG6.Models;

namespace ProjetoG6.Controllers
{
    public class ProgramaMobilidadesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProgramaMobilidadesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ProgramaMobilidades
        public async Task<IActionResult> Index()
        {
            return View(await _context.ProgramaMobilidade.ToListAsync());
        }

        // GET: ProgramaMobilidades/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var programaMobilidade = await _context.ProgramaMobilidade
                .SingleOrDefaultAsync(m => m.ProgramaMobilidadeID == id);
            if (programaMobilidade == null)
            {
                return NotFound();
            }

            return View(programaMobilidade);
        }

        // GET: ProgramaMobilidades/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProgramaMobilidades/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProgramaMobilidadeID,Nome")] ProgramaMobilidade programaMobilidade)
        {
            if (ModelState.IsValid)
            {
                _context.Add(programaMobilidade);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(programaMobilidade);
        }

        // GET: ProgramaMobilidades/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var programaMobilidade = await _context.ProgramaMobilidade.SingleOrDefaultAsync(m => m.ProgramaMobilidadeID == id);
            if (programaMobilidade == null)
            {
                return NotFound();
            }
            return View(programaMobilidade);
        }

        // POST: ProgramaMobilidades/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProgramaMobilidadeID,Nome")] ProgramaMobilidade programaMobilidade)
        {
            if (id != programaMobilidade.ProgramaMobilidadeID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(programaMobilidade);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProgramaMobilidadeExists(programaMobilidade.ProgramaMobilidadeID))
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
            return View(programaMobilidade);
        }

        // GET: ProgramaMobilidades/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var programaMobilidade = await _context.ProgramaMobilidade
                .SingleOrDefaultAsync(m => m.ProgramaMobilidadeID == id);
            if (programaMobilidade == null)
            {
                return NotFound();
            }

            return View(programaMobilidade);
        }

        // POST: ProgramaMobilidades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var programaMobilidade = await _context.ProgramaMobilidade.SingleOrDefaultAsync(m => m.ProgramaMobilidadeID == id);
            _context.ProgramaMobilidade.Remove(programaMobilidade);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProgramaMobilidadeExists(int id)
        {
            return _context.ProgramaMobilidade.Any(e => e.ProgramaMobilidadeID == id);
        }
    }
}
