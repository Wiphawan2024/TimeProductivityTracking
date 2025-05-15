using System.Globalization;
using Microsoft.AspNetCore.Authorization;
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
            var userEmail = User?.Identity?.Name;
            //Get months from database
            var rawMonths = await _context.Productivities
               .Where(p =>  userEmail !=null &&
                            p.UserEmail == userEmail && 
                            !string.IsNullOrEmpty(p.Monthly))
               .Select(p => p.Monthly)
               .Distinct()
               .ToListAsync();

            // Convert to DateTime and order
            var orderedMonths = rawMonths
                .Where(m => !string.IsNullOrWhiteSpace(m) && DateTime.TryParseExact(m, "MMMM yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                .Select(m => new
                {
                    Value = m,
                    Date = DateTime.ParseExact(m!, "MMMM yyyy", CultureInfo.InvariantCulture)
                })
                .OrderBy(x => x.Date)
                .Select(x => new SelectListItem
                {
                    Value = x.Value,
                    Text = x.Value,
                    Selected = (x.Value == selectedMonth)
                })
                .ToList();

            ViewBag.Months = orderedMonths;

            //If no month is selected, return an empty list
            if (string.IsNullOrEmpty(selectedMonth))
            {
                return View(new List<Productivity>());// 
            }

            var productivities = _context.Productivities.AsQueryable();
            if (!string.IsNullOrEmpty(selectedMonth))
            {
                productivities = productivities
                    .Include(p => p.Contractor)
                    .Where(p => p.Monthly == selectedMonth && p.UserEmail == userEmail);

            }
            //Fetch and filter productivities based on the selected month
            var productivitiesList = await _context.Productivities
                .Include(p => p.Contractor)
                .Where(p => p.Monthly == selectedMonth && p.UserEmail == userEmail).ToListAsync();


            return View(productivitiesList);
        }


        [Authorize]//Ensure only logged-in user
        public async Task<IActionResult> ChartByContractor(int? contractorId)
        {


            ViewBag.Contractors = await _context.Users  
                .Where(u=>u.Role==Roles.Contractor )  // Filter by role
                .Select(u => new SelectListItem
                {
                    Value = u.UserId.ToString(),
                    Text = $"{u.FName} {u.LName} ({u.Email})" // 
                }).ToListAsync();

            if (contractorId == null)
            {
                ViewBag.ChartMonths = new List<string>();
                ViewBag.ChartPlanned = new List<decimal>();
                ViewBag.ChartNextMonth = new List<decimal>();
                ViewBag.ChartAchieved = new List<decimal>();
                return View();
            }

            var data = await _context.Productivities
                .Where(p => p.ContractorId == contractorId)
                .ToListAsync();

            var grouped = data
                .GroupBy(p => DateTime.ParseExact(p.Monthly!, "MMMM yyyy", CultureInfo.InvariantCulture))
                .OrderBy(g => g.Key)
                .ToList();

            ViewBag.ChartMonths = grouped.Select(g => g.Key.ToString("MMMM yyyy")).ToList();
            ViewBag.ChartPlanned = grouped.Select(g => g.Sum(p => p.PlannedDays)).ToList();
            ViewBag.ChartNextMonth = grouped.Select(g => g.Sum(p => p.PlannedNextMonth)).ToList();
            ViewBag.ChartAchieved = grouped.Select(g => g.Sum(p => p.AchevedDays)).ToList();

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
        public IActionResult Create(int? SelectedMonth, int? SelectedYear)
        {
            var userEmail = User?.Identity?.Name;           
            
            // Generate months
            ViewBag.Months = Enumerable.Range(1, 12).Select(i => new SelectListItem
            {
                Value = i.ToString(),
                Text = new DateTime(1, i, 1).ToString("MMMM")
            }).ToList();

            // Generate years: current year to current + 2
            ViewBag.Years = Enumerable.Range(DateTime.Now.Year, 2).Select(y => new SelectListItem
            {
                Value = y.ToString(),
                Text = y.ToString()
            }).ToList();

            var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);
            if (user != null)
            {
                ViewBag.userId = user.UserId;
            }

            if (SelectedMonth == null || SelectedYear == null)
            {
                // Don't proceed with data load until both are selected
                return View(new List<Productivity>());
            }

            //Load SEC data and previous month productivity
            var SECName = _context.SECContracts.ToList();
            var NewProductivity = new List<Productivity>();


            //Get current and orevioous month 
            var prevMonth =SelectedMonth == 1 ? 12 : SelectedMonth.Value - 1;
            var prevYear = SelectedMonth == 1 ? SelectedYear.Value - 1 : SelectedYear.Value;
            string prevMonthOj = $"{CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(prevMonth)}{prevYear}";

            //Load previous month's productivity
            var preMonthProductivity = _context.Productivities
                .Where(p => p.Monthly.Trim().Replace(" ","") == prevMonthOj
                && p.CountryMentor_P==userEmail)
                .ToList();

            foreach (var item in SECName)
            {
                //get previous month record 
             
             var previous = preMonthProductivity
                .FirstOrDefault(p => p.SECName?.Trim().ToLower() == item.SECName?.Trim().ToLower());
          
                NewProductivity.Add(new Productivity
                {
                    Date = DateTime.Now,
                    SECName = item.SECName,
                    County = Enum.Parse<Counties>(item.County!),
                    PlannedDays=previous?.PlannedNextMonth ?? 0,                   
                    Task_P = previous?.Task_N,
                    PlannedNextMonth=0,
                    Task_N = null,
                    CountryMentor_P=userEmail,
                    Tasks_A = null,
                    CountryMentor_A=userEmail
                    
                });
            }

        

          //  ViewData["SEC"] = _context.SECContracts.ToList();

            return View(NewProductivity);

        }

        // POST: Productivities/Create
    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string SelectedMonth, string SelectedYear, [Bind("Id,Date,Monthly,SECName,County,PlannedDays,PlannedNextMonth ,Task_P,CounryMentor_P,AchevedDays,Tasks_A,CounryMentor_A")] List< Productivity> productivities)
      {

                var contractorId = 0;
                var currentUserEmail = User?.Identity?.Name;
                var contractor = _context.Users.FirstOrDefault(u => u.Email == currentUserEmail);
                if (contractor != null)
                {
                    contractorId = contractor.UserId;
                }

                if (ModelState.IsValid)
                {
                    // Convert SelectedMonth and SelectedYear into "MMMM yyyy"
                    string selectedMonthFormatted = "";
                    if (!string.IsNullOrEmpty(SelectedMonth) &&
                        int.TryParse(SelectedMonth, out int monthNumber) &&
                        !string.IsNullOrEmpty(SelectedYear) &&
                        int.TryParse(SelectedYear, out int yearNumber))
                    {
                        selectedMonthFormatted = new DateTime(yearNumber, monthNumber, 1).ToString("MMMM yyyy");
                    }
                    else
                    {
                        selectedMonthFormatted = DateTime.Now.ToString("MMMM yyyy");
                    }

                    // Check for duplicate entries for this user in that month
                    bool exists = await _context.Productivities
                        .AnyAsync(p => p.Monthly == selectedMonthFormatted && p.UserEmail == currentUserEmail);

                    if (exists)
                    {
                        TempData["ErrorMessage"] = $"You already submitted productivity for {selectedMonthFormatted}.";
                        return RedirectToAction(nameof(Create));
                    }

                    int i = 0;
                    foreach (var item in productivities)
                    {
                        item.Date = DateTime.Now;
                        item.Monthly = selectedMonthFormatted;
                        item.SECName = productivities[i].SECName;
                        item.County = productivities[i].County;
                        item.PlannedDays = productivities[i].PlannedDays;
                        item.PlannedNextMonth = productivities[i].PlannedNextMonth;
                        item.Task_P = productivities[i].Task_P;
                        item.CountryMentor_P = productivities[i].CountryMentor_P;
                        item.AchevedDays = productivities[i].AchevedDays;
                        item.Tasks_A = productivities[i].Tasks_A;
                        item.CountryMentor_A = productivities[i].CountryMentor_A;
                        item.UserEmail = currentUserEmail;
                        item.ContractorId = contractorId;
                        item.statusApproval = "Waiting";
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

            var productivities = await _context.Productivities
                .Include(p=>p.Contractor)
                .FirstOrDefaultAsync(p=>p.Id==id);  
            if (productivities == null)
            {
                return NotFound();
            }
            return View(productivities);
        }

        // POST: Productivities/Edit/5
  
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,Monthly,SECName,County,PlannedDays,PlannedNextMonth ,Task_P,CountryMentor_P,AchevedDays,Tasks_A,CountryMentor_A,statusApproval")] Productivity productivity)
        {
            var userEmail = User?.Identity?.Name;
            if (id != productivity.Id)
            {
                return NotFound();
            }

            var contractor = await _context.Users.FirstOrDefaultAsync(u => u.Email ==userEmail);
            if (contractor == null)
            {
                return Unauthorized(); // user not found
            }

            // Fetch the original record from DB
            var originalProductivity = await _context.Productivities.FirstOrDefaultAsync(p => p.Id == id);
            if (originalProductivity == null)
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {
                try
                {

                    // Update fields
                    originalProductivity.Date = productivity.Date;
                    originalProductivity.Monthly = productivity.Monthly;
                    originalProductivity.SECName = productivity.SECName;
                    originalProductivity.County = productivity.County;
                    originalProductivity.PlannedDays = productivity.PlannedDays;
                    originalProductivity.PlannedNextMonth= productivity.PlannedNextMonth;
                    originalProductivity.Task_P = productivity.Task_P;
                    originalProductivity.CountryMentor_P = productivity.CountryMentor_P;
                    originalProductivity.AchevedDays = productivity.AchevedDays;
                    originalProductivity.Tasks_A = productivity.Tasks_A;
                    originalProductivity.CountryMentor_A = productivity.CountryMentor_A;


                    // Keep contractor consistent (based on logged-in user)
                    originalProductivity.ContractorId = contractor.UserId;
                    originalProductivity.UserEmail = contractor.Email;
                    originalProductivity.statusApproval = productivity.statusApproval;


                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductivitiesExists(productivity.Id))
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
            return View(productivity);
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
                Text = new DateTime(2025, i, 1).ToString("MMMM yyyy") 
            }).ToList();
        }

        

    }
}
