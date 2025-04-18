﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using TimeProductivityTracking.web.Areas.Identity.Data;
using TimeProductivityTracking.web.Data;
using TimeProductivityTracking.web.Models;
using TimeProductivityTracking.web.ViewModels;

namespace TimeProductivityTracking.web.Controllers
{
    public class ProductivitySummaryViewModelsController : Controller
    {
        private readonly ProductivitiesContext _context;
        private readonly UserManager<IdentityAuthUser> _userManager;
        public ProductivitySummaryViewModelsController(ProductivitiesContext context, UserManager<IdentityAuthUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        //Get: Index
        public async Task<IActionResult>Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();


            var data = await _context.Productivities
                    .Include(c => c.Contractor)
                    .Where(c => (c.statusApproval == "Waiting" ) 
                    && c.Contractor !=null
                    && c.Contractor.Email == user.Email)
                    .ToListAsync();

                var contractor = data
                    .GroupBy(c => new { c.Monthly, c.ContractorId, c.Contractor?.FName, c.Contractor?.LName })
                    .Select(g => new ViewProductivities
                    {
                        ProductivityId = g.First().Id,
                        ContractorId = g.Key.ContractorId,
                        Month = g.Key.Monthly,
                        FName = g.Key.FName ?? "Unknown",
                        LName = g.Key.LName ?? "Unknown",
                        TotalDays = g.Sum(p => p.AchevedDays)
                    })
                    .OrderBy(c => c.Month)
                    .ToList();

                return View(contractor);
            

        }

        public async Task<IActionResult> Details(int? ContractorId, string? month)
        {
            if (ContractorId == null || string.IsNullOrEmpty(month))
            {
                return BadRequest("Contractor ID and month are required.");
            }

            var productivities = await _context.Productivities
                .Include(p => p.Contractor)
                .Where(p => p.ContractorId == ContractorId && p.Monthly == month)
                .ToListAsync(); 

            if (productivities.Count==0)
            {
                return NotFound("No data found for the selected contractor and month.");
            }

            ViewBag.Monthly = month;
            ViewBag.ContractorId = ContractorId;
            ViewBag.TotalDays = productivities.Sum(p => p.AchevedDays); 

            return View(productivities);
        }

        public IActionResult ProductivityByInvoice(string month, int contractorId,int InvoiceId)
        {
            var records = _context.Productivities
                .Where(p => p.Monthly == month && p.ContractorId == contractorId)
                .ToList();
            var user =_context.Users.FirstOrDefault(u => u.UserId == contractorId);
            ViewBag.Contractor = user!=null? $"{user.FName} {user.LName}": "Unknown";
            ViewBag.Month = month;
            ViewBag.InvoiceID = InvoiceId;
            ViewBag.ContractorId = contractorId;

            return View(records);
        }

        [HttpGet]
        public async Task<IActionResult> Approve(string month, int ContractorId)
        {
            var toApprove =await _context.Productivities
                .Where(p => p.Monthly == month && p.ContractorId == ContractorId)
                .ToListAsync();
           if (toApprove.Count==0)
                {
                return NotFound("No productivities found for the given month and contractor");
            }
         
           //Load contractor info
           foreach (var productivity in toApprove)
            {
                productivity.statusApproval = "Invoiced";
            }
            await _context.SaveChangesAsync();// Save approval updates


            //prepare invoice data

            var contractor=await _context.Users
                .Include(u=>u.Rate)
                .Where(u=>u.UserId==ContractorId)
                .FirstOrDefaultAsync();

            decimal hourlyRate = (decimal)(contractor?.Rate?.HourlyWage ?? 0);
            decimal totalHours = toApprove.Sum(p => p.AchevedDays) ;
            decimal totalAmout = totalHours * hourlyRate;


            // Prepare Invoice view model
            var invoice = new InvoiceViewModel
            {
                ContractorName = $"{contractor?.FName} {contractor?.LName}",
                ContractorEmail = contractor?.Email,
                Month = month,
                InvoiceDate = DateTime.Now,
                InvoiceNumber = $"INV--{ContractorId}--{DateTime.Now:yyyyMMddHHmmss}",
                InvoiceProductivities = toApprove,
                TotalAmount = totalAmout,
                HourlyRate = hourlyRate,
                TotalHours=totalHours
             };


            var invoiceEntity = new Invoice
            {
                InvoiceNumber = invoice.InvoiceNumber,
                InvoiceDate = invoice.InvoiceDate,
                Month = invoice.Month,
                ContractorId = ContractorId,
                TotalHours = invoice.TotalHours,
                HourlyRate = invoice.HourlyRate,
                TotalAmount = invoice.TotalAmount
            };

            _context.Invoices.Add(invoiceEntity);
            await _context.SaveChangesAsync(); //  Save to Invoice table

            return View("Invoice", invoice);


        }


        [HttpGet]
        public async Task<IActionResult> Reject(string month, int ContractorId)
        {
            var productivities = await _context.Productivities
                .Where(p => p.Monthly == month && p.ContractorId == ContractorId)
                .ToListAsync();
            if (productivities.Count==0)
            {
                return NotFound("No productivities found for the given month and contractor");
            }
        
            return RedirectToAction("Index"); 
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
                .GroupBy(p => new {
                    p.SECName, p.Monthly, 
                    FName=p.Contractor !=null?p.Contractor.FName :" ", 
                    LName=p.Contractor !=null?p.Contractor.LName :" "})
            .Select(g => new ProductivitySummaryViewModel
            {
                SecName = g.Key.SECName,
                Month = g.Key.Monthly,
                FName = g.Key.FName,
                LName = g.Key.LName,
                TotalAchevedDays = g.Sum(p => p.AchevedDays)
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


            return View(data); 
        }
    }

}
