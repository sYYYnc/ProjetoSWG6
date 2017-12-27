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
    public class CandidatosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CandidatosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Candidatos
        public async Task<IActionResult> Index()
        {
            return View(await _context.Candidatos.ToListAsync());
        }

        // GET: Candidatos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var candidatos = await _context.Candidatos
                .SingleOrDefaultAsync(m => m.CandidatoID == id);
            if (candidatos == null)
            {
                return NotFound();
            }

            string tipo = "NA";
            foreach (var cand in _context.Candidatura)
            {
                if (candidatos.CandidaturaID == cand.CandidaturaID)
                {
                    foreach (var programa in _context.ProgramaMobilidade)
                    {
                        if (programa.ProgramaMobilidadeID == cand.ProgramaMobilidadeID)
                        {
                            tipo = programa.Nome;
                            break;
                        }
                    }
                }
            }
                
            
            ViewBag.Message = tipo;
            return View(candidatos);
        }
        // GET: Candidatos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Candidatos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CandidatoID,Nome,Email,DataNascimento,NumeroAluno,Password,PasswordConfirmacao,DestinoID,BolsaID,CandidaturaID,EntervistaID")] Candidatos candidatos)
        {
            if (ModelState.IsValid)
            {
                _context.Add(candidatos);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(candidatos);
        }

        // GET: Candidatos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var candidatos = await _context.Candidatos.SingleOrDefaultAsync(m => m.CandidatoID == id);
            if (candidatos == null)
            {
                return NotFound();
            }
            return View(candidatos);
        }

        // POST: Candidatos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CandidatoID,Nome,Email,DataNascimento,NumeroAluno,Password,PasswordConfirmacao,DestinoID,BolsaID,CandidaturaID,EntervistaID")] Candidatos candidatos)
        {
            if (id != candidatos.CandidatoID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(candidatos);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CandidatosExists(candidatos.CandidatoID))
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
            return View(candidatos);
        }

        // GET: Candidatos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var candidatos = await _context.Candidatos
                .SingleOrDefaultAsync(m => m.CandidatoID == id);
            if (candidatos == null)
            {
                return NotFound();
            }

            return View(candidatos);
        }

        // POST: Candidatos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var candidatos = await _context.Candidatos.SingleOrDefaultAsync(m => m.CandidatoID == id);
            _context.Candidatos.Remove(candidatos);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CandidatosExists(int id)
        {
            return _context.Candidatos.Any(e => e.CandidatoID == id);
        }
    }
}
