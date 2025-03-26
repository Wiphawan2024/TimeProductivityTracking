using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TimeProductivityTracking.web.Areas.Identity.Data;
using TimeProductivityTracking.web.Data;
using TimeProductivityTracking.web.Models;

namespace TimeProductivityTracking.web.Controllers
{
    [Authorize]
    public class InvoicesController : Controller
    {
        private readonly ProductivitiesContext _context;
        private readonly IConverter _pdfConverter;
        private readonly UserManager<IdentityAuthUser> _userManager;
        public InvoicesController(ProductivitiesContext context,
            IConverter pdfConverter,
            UserManager<IdentityAuthUser> userManager)
        {
            _context = context;
            _pdfConverter = pdfConverter;
            _userManager = userManager;
        }

        // GET: Invoices
        public async Task<IActionResult> Index()
        {
           
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var invoices = await _context.Invoices
                .Include(i => i.Contractor)
                .Where(i => i.Contractor.Email == user.Email)
                .ToListAsync();

         
            return View(invoices);
        }
        // GET: Invoice/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var invoice = await _context.Invoices
                .Include(i => i.Contractor)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }
        public async Task<IActionResult> GenerateInvoice(int month, int year)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var contractor = await _context.Users
                .Include(c => c.Rate)// include the rate
                .FirstOrDefaultAsync(u => u.Email == user.Email);

            if (contractor == null) return NotFound();

            if (year < 1 || month < 1 || month > 12)
            {
                return BadRequest("Invalid month or year.");
            }
            var selectedMonth = new DateTime(year, month, 1).ToString("MMMM yyyy"); // Example: "February 2025"

            var projects = await _context.Productivities
                .Where(p => p.CountryMentor_A == contractor.Email
                            && p.Monthly == selectedMonth) // Compare with the stored format
                .ToListAsync();

            var totalDays = (double)projects.Sum(p => p.AchevedDays ?? 0);
            var dailyRate = contractor.Rate != null ? contractor.Rate.HourlyWage *8 :250 ;
            var totalPayment =totalDays * dailyRate;

            var invoice = new Invoice
            {
                ContractorId = contractor.UserId,
                Month = new DateTime(year, month, 1),
                TotalDaysWorked = totalDays,
                //TotalPayment is calculate automatically in the model, 
               
                CreatedAt = DateTime.UtcNow
            };

            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DownloadInvoice(int invoiceId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var invoice = await _context.Invoices
                .Include(i => i.Contractor)
                .FirstOrDefaultAsync(i => i.Id == invoiceId && i.Contractor.Email == user.Email);

            if (invoice == null) return NotFound();

            var htmlContent = $@"
                <html>
                <head><style> body {{ font-family: Arial; }} table {{ width: 100%; border-collapse: collapse; }} th, td {{ border: 1px solid black; padding: 5px; }} </style></head>
                <body>
                    <h2>Invoice for {invoice.Contractor.FName} {invoice.Contractor.LName}</h2>
                    <p>Month: {invoice.Month.ToString("MMMM yyyy")}</p>
                    <table>
                        <tr><th>Total Days Worked</th><th>Total Payment (€)</th></tr>
                        <tr><td>{invoice.TotalDaysWorked}</td><td>{invoice.TotalPayment}</td></tr>
                    </table>
                    <p>Generated on {invoice.CreatedAt}</p>
                </body>
                </html>
            ";

            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = new GlobalSettings()
                {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4,
                },
                Objects = { new ObjectSettings() { HtmlContent = htmlContent } }
            };

            var pdfBytes = _pdfConverter.Convert(pdf);
            return File(pdfBytes, "application/pdf", $"Invoice_{invoice.Contractor.FName}_{invoice.Month:yyyy_MM}.pdf");
        }


    }



}
