using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using TimeProductivityTracking.web.Data;
using TimeProductivityTracking.web.Models;

namespace TimeProductivityTracking.web.Controllers
{
    public class ProductivitySummaryViewModelsController : Controller
    {
        private readonly ProductivitiesContext _context;

        public ProductivitySummaryViewModelsController(ProductivitiesContext context)
        {
            _context = context;
        }

        // GET: ProductivitySummaryViewModels/Summary
        public async Task<IActionResult> Summary(string userEmail, string selectedMonth)
        {
            var query =  _context.Productivities
               .Include(p=>p.Contractor)
               .AsQueryable();


            if (!string.IsNullOrEmpty(userEmail))
                query = query.Where(p => p.UserEmail == userEmail);

            if (!string.IsNullOrEmpty(selectedMonth))
                query = query.Where(p => p.Monthly == selectedMonth);

            var data = await query
                .GroupBy(p => new { p.SECName, p.Monthly, p.Contractor.FName, p.Contractor.LName })
            .Select(g => new ProductivitySummaryViewModel
            {
                SecName = g.Key.SECName,
                Month = g.Key.Monthly,
                FName = g.Key.FName,
                LName = g.Key.LName,
                TotalAchevedDays = g.Sum(p => p.AchevedDays ?? 0)
            })
                            .OrderBy(g => g.Month)
                .ThenBy(g => g.SecName)
                .ToListAsync();

            var availableMonths = await _context.Productivities
                .Select(p => p.Monthly)
                .Distinct()
                .ToListAsync();
           
            ViewBag.UserEmail = userEmail;
            ViewBag.SelectedMonth = selectedMonth;
            ViewBag.AvailableMonths = availableMonths;
            // Calculate total achieved days
            ViewBag.TotalAchievedDays = query.Sum(p => p.AchevedDays);


            return View(data); // data is List<ProductivitySummaryViewModel>
        }
    }

}
