using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjetoG6.Data;
using ProjetoG6.Models;

namespace ProjetoG6.Controllers
{
    public class ProgramaMobilidadePaisController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProgramaMobilidadePaisController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ProgramaMobilidadePais
        public async Task<IActionResult> Index()
        {
            return View(await _context.ProgramaMobilidadePais.ToListAsync());
        }

        // GET: ProgramaMobilidadePais/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var programaMobilidadePais = await _context.ProgramaMobilidadePais
                .SingleOrDefaultAsync(m => m.ProgramaMobilidadePaisID == id);
            if (programaMobilidadePais == null)
            {
                return NotFound();
            }

            return View(programaMobilidadePais);
        }

        // GET: ProgramaMobilidadePais/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProgramaMobilidadePais/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProgramaMobilidadePaisID")] ProgramaMobilidadePais programaMobilidadePais)
        {
            if (ModelState.IsValid)
            {
                _context.Add(programaMobilidadePais);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(programaMobilidadePais);
        }

        // GET: ProgramaMobilidadePais/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var programaMobilidadePais = await _context.ProgramaMobilidadePais.SingleOrDefaultAsync(m => m.ProgramaMobilidadePaisID == id);
            if (programaMobilidadePais == null)
            {
                return NotFound();
            }
            return View(programaMobilidadePais);
        }

        // POST: ProgramaMobilidadePais/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProgramaMobilidadePaisID")] ProgramaMobilidadePais programaMobilidadePais)
        {
            if (id != programaMobilidadePais.ProgramaMobilidadePaisID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(programaMobilidadePais);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProgramaMobilidadePaisExists(programaMobilidadePais.ProgramaMobilidadePaisID))
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
            return View(programaMobilidadePais);
        }

        // GET: ProgramaMobilidadePais/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var programaMobilidadePais = await _context.ProgramaMobilidadePais
                .SingleOrDefaultAsync(m => m.ProgramaMobilidadePaisID == id);
            if (programaMobilidadePais == null)
            {
                return NotFound();
            }

            return View(programaMobilidadePais);
        }

        // POST: ProgramaMobilidadePais/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var programaMobilidadePais = await _context.ProgramaMobilidadePais.SingleOrDefaultAsync(m => m.ProgramaMobilidadePaisID == id);
            _context.ProgramaMobilidadePais.Remove(programaMobilidadePais);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProgramaMobilidadePaisExists(int id)
        {
            return _context.ProgramaMobilidadePais.Any(e => e.ProgramaMobilidadePaisID == id);
        }
    }
}
