using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TimeProductivityTracking.web.Data;
using TimeProductivityTracking.web.Models;

namespace TimeProductivityTracking.web.Controllers
{
    public class SECContractsController : Controller
    {
        private readonly ProductivitiesContext _context;

        public SECContractsController(ProductivitiesContext context)
        {
            _context = context;
        }

        // GET: SECContracts
        public async Task<IActionResult> Index()
        {
            return View(await _context.SECContracts.ToListAsync());
        }

        // GET: SECContracts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sECContract = await _context.SECContracts
                .FirstOrDefaultAsync(m => m.SECContractId == id);
            if (sECContract == null)
            {
                return NotFound();
            }

            return View(sECContract);
        }
        private static IEnumerable<SelectListItem> GetCountiesList()
        {
            return Enum.GetValues(typeof(Counties))
                       .Cast<Counties>()
                       .Select(c => new SelectListItem
                       {
                           Value = c.ToString(),
                           Text = c.ToString()
                       });
        }


        // GET: SECContracts/Create
        public IActionResult Create()
        {
            ViewBag.Counties = GetCountiesList();
            return View();
        }

        // POST: SECContracts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SECContractId,SECName,County,Address,PrimaryContract,Phone,Email")] SECContract SECContract)
        {
            if (ModelState.IsValid)
            {
                _context.Add(SECContract);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Counties=GetCountiesList();
            return View(SECContract);
        }

        // GET: SECContracts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sECContract = await _context.SECContracts.FindAsync(id);
            if (sECContract == null)
            {
                return NotFound();
            }
            ViewBag.Counties = GetCountiesList();
            return View(sECContract);
        }

        // POST: SECContracts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SECContractId,SECName,County,Address,PrimaryContract,Phone,Email")] SECContract SecContract)
        {
            if (id != SecContract.SECContractId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(SecContract);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SECContractExists(SecContract.SECContractId))
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
            ViewBag.Counties = GetCountiesList();
            return View(SecContract);
        }

        // GET: SECContracts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sECContract = await _context.SECContracts
                .FirstOrDefaultAsync(m => m.SECContractId == id);
            if (sECContract == null)
            {
                return NotFound();
            }

            return View(sECContract);
        }

        // POST: SECContracts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sECContract = await _context.SECContracts.FindAsync(id);
            if (sECContract != null)
            {
                _context.SECContracts.Remove(sECContract);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SECContractExists(int id)
        {
            return _context.SECContracts.Any(e => e.SECContractId == id);
        }
    }
}
