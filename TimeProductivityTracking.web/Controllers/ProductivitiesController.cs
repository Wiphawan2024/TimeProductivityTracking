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
    public class ProductivitiesController : Controller
    {
        private readonly ProductivitiesContext _context;

        public ProductivitiesController(ProductivitiesContext context)
        {
            _context = context;
        }

        // GET: Productivities
        public async Task<IActionResult> Index()
        {
            return View(await _context.ProductivitieS.ToListAsync());
        }

        // GET: Productivities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productivities = await _context.ProductivitieS
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productivities == null)
            {
                return NotFound();
            }

            return View(productivities);
        }

        // GET: Productivities/Create
        public IActionResult Create()
        {
            var SECName = _context.SECContracts.ToList();
            var NewProductivity = new List<Productivities>();


            foreach (var item in SECName)
            {
                var product = new Productivities
                {
                    Monthly = DateTime.Now,
                    SECName = item.SECName,
                    County = null,
                    PlannedDays = null,
                    Task_P = null,
                    CounryMentor_P = null,
                    AchevedDays = null,

                    ContractorId_FK = item.SECContractId
                };
                NewProductivity.Add(product);
            }


            foreach (var n in NewProductivity)
            {
                Console.WriteLine(n.SECName);
            }

            var plannedDay = new List<SelectDays>
            {
                new SelectDays { id = 1, name = "Choie1", PlannedDay = 0 },
                new SelectDays { id = 2, name = "Choie2", PlannedDay = 0.1m },
                new SelectDays { id=3,name="Choie3",PlannedDay=0.2m},
                new SelectDays { id=4,name="Choie4",PlannedDay=0.3m},
                new SelectDays { id=5,name="Choie5",PlannedDay=0.4m},
                new SelectDays { id=6,name="Choie6",PlannedDay=0.5m},
                new SelectDays { id=7,name="Choie7",PlannedDay=0.6m},
                new SelectDays { id=8,name="Choie8",PlannedDay=0.7m},
                new SelectDays { id=9,name="Choie9",PlannedDay=0.8m},
                new SelectDays { id=10,name="Choie10",PlannedDay=0.9m},
                new SelectDays { id=11,name="Choie11",PlannedDay=10},
            };

            ViewBag.SelectPlanDay = new SelectList(plannedDay, "id", "PlannedDay");
            var sec = _context.SECContracts.ToList();
            ViewData["SEC"] = sec;

            return View(NewProductivity);
        }

        // POST: Productivities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Monthly,SECName,County,PlannedDays,Task_P,CounryMentor_P,AchevedDays,Tasks_A,CounryMentor_A,ContractorId_FK")] List< Productivities> productivities)
        {


            if (ModelState.IsValid)
            {


                for (int i = 0; i < productivities.Count; i++)
                {
                    foreach (var item in productivities)
                    {
                        item.Monthly = DateTime.Now;
                        item.AchevedDays = null;
                        item.Tasks_A = null;
                        item.CounryMentor_A = null;
                    }
                }

                _context.Add(productivities);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productivities);
        }




        // GET: Productivities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productivities = await _context.ProductivitieS.FindAsync(id);
            if (productivities == null)
            {
                return NotFound();
            }
            return View(productivities);
        }

        // POST: Productivities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Monthly,SECName,County,PlannedDays,Task_P,CounryMentor_P,AchevedDays,Tasks_A,CounryMentor_A,ContractorId_FK")] Productivities productivities)
        {
            if (id != productivities.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productivities);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductivitiesExists(productivities.Id))
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
            return View(productivities);
        }

        // GET: Productivities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productivities = await _context.ProductivitieS
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productivities == null)
            {
                return NotFound();
            }

            return View(productivities);
        }

        // POST: Productivities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productivities = await _context.ProductivitieS.FindAsync(id);
            if (productivities != null)
            {
                _context.ProductivitieS.Remove(productivities);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductivitiesExists(int id)
        {
            return _context.ProductivitieS.Any(e => e.Id == id);
        }
    }
}
