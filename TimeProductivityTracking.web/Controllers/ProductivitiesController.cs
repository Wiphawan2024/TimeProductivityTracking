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
        public async Task<IActionResult> Index(string selectedMonth)
        {
          
            //Get monts from database
            ViewBag.Months=await _context.Productivities
                .Select(p=>p.Monthly).Distinct().OrderBy(m=>m)
                .ToListAsync();
            //Fetch productivities based on the selected month
            var productivities = _context.Productivities.AsQueryable();
            if (!string.IsNullOrEmpty(selectedMonth))
            {
                productivities = productivities.Where(p => p.Monthly == selectedMonth);

            }
            return View(await _context.Productivities.ToListAsync());
        }
        public IActionResult Chart()
        {
            /*

            // Fetch data from database (replace with your actual database model)
            var productivityData = _context.Productivities
                .Where(p => p.PlannedDays != null && p.AchevedDays != null) // Ensure data exists
                .Select(p => new
                {
                    SECName = p.SECName,
                    PlannedDays = p.PlannedDays ?? 0,
                    AchievedDays = p.AchevedDays ?? 0
                })
                .ToList();

            if (productivityData.Count == 0)
            {
                return View("Error"); // Handle empty data scenario
            }

            // Pass data to ViewBag for the chart
            ViewBag.SECNames = productivityData.Select(p => p.SECName).ToList();
            ViewBag.PlannedDays = productivityData.Select(p => p.PlannedDays).ToList();
            ViewBag.AchievedDays = productivityData.Select(p => p.AchievedDays).ToList();

            return View();


            */



            // Sample Data: You can replace this with database data from _context.Productivities
            var productivityData = new List<Productivity>
        {
            new Productivity { Monthly = "January", PlannedDays = 20, AchevedDays = 18 },
            new Productivity { Monthly = "February", PlannedDays = 18, AchevedDays = 20 },
            new Productivity { Monthly = "March", PlannedDays = 22, AchevedDays = 20 },
            new Productivity { Monthly = "April", PlannedDays = 25, AchevedDays = 23 },
            new Productivity { Monthly = "May", PlannedDays = 20, AchevedDays = 25 },
            new Productivity { Monthly = "June", PlannedDays = 21, AchevedDays = 30 }
        };

            // Extracting Data for Chart.js
            ViewBag.Months = productivityData.ConvertAll(m => m.Monthly);
            ViewBag.PlannedDays = productivityData.ConvertAll(p => p.PlannedDays ?? 0);
            ViewBag.AchievedDays = productivityData.ConvertAll(a => a.AchevedDays ?? 0);

            return View();
        }

        // GET: Productivities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productivities = await _context.Productivities
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
            DateTime month;

            // Generate a list of months
            List<SelectListItem> months = Enumerable.Range(1, 12).Select(i => new SelectListItem
            {
                Value = i.ToString(),
                Text = new DateTime(2022, i, 1).ToString("MMMM") // Get month name
            }).ToList();

            // Pass to the ViewBag 
            ViewBag.Months = months;
         
          
            var user=_context.Users.FirstOrDefault(u=>u.Email==User.Identity.Name);
            if (user != null)
            {
                ViewBag.userId = user.UserId;
             
            }

            var SECName = _context.SECContracts.ToList();
            var NewProductivity = new List<Productivity>();

            foreach (var item in SECName)
            {
                var product = new Productivity
                {
                  
                    Date=DateTime.Now,
                 
                    SECName = item.SECName

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
                new SelectDays { id=11,name="Choie11",PlannedDay=1.0m},
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
        public async Task<IActionResult> Create(string SelectedMonth,[Bind("Id,Date,Monthly,SECName,County,PlannedDays,Task_P,CounryMentor_P,AchevedDays,Tasks_A,CounryMentor_A")] List< Productivity> productivities)
      {


            if (ModelState.IsValid)
            {
               
                int i = 0;

                foreach (var item in productivities)
                        {
                        item.Date = DateTime.Now;

                    // Convert month number to month name + year 2025
                    if (!string.IsNullOrEmpty(SelectedMonth) && int.TryParse(SelectedMonth, out int monthNumber) && monthNumber >= 1 && monthNumber <= 12)
                    {
                        item.Monthly = new DateTime(2025, monthNumber, 1).ToString("MMMM yyyy"); // Example: "January 2025"
                    }
                    else
                    {
                        item.Monthly = DateTime.Now.ToString("MMMM yyyy"); // Default to current month + year 2025
                    }
                

                item.SECName = productivities[i].SECName;
                        item.County = productivities[i].County;
                        item.PlannedDays = productivities[i].PlannedDays;
                        item.Task_P = productivities[i].Task_P;
                        item.CounryMentor_P = productivities[i].CounryMentor_P;
                        item.AchevedDays = productivities[i].AchevedDays;
                        item.Tasks_A = productivities[i].Tasks_A;
                        item.CounryMentor_A = productivities[i].CounryMentor_A;
                    i++;
                }


                _context.AddRange(productivities);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }




        // GET: Productivities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productivities = await _context.Productivities.FindAsync(id);
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,Monthly,SECName,County,PlannedDays,Task_P,CounryMentor_P,AchevedDays,Tasks_A,CounryMentor_A")] Productivity productivities)
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

            var productivities = await _context.Productivities
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
            var productivities = await _context.Productivities.FindAsync(id);
            if (productivities != null)
            {
                _context.Productivities.Remove(productivities);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductivitiesExists(int id)
        {
            return _context.Productivities.Any(e => e.Id == id);
        }

        //  Helper method to generate month list
        private List<SelectListItem> GetMonthList()
        {
            return Enumerable.Range(1, 12).Select(i => new SelectListItem
            {
                Value = i.ToString(), // Stores month number (1-12)
                Text = new DateTime(2025, i, 1).ToString("MMMM yyyy") // Displays month + year (e.g., "January 2025")
            }).ToList();
        }

    }
}
