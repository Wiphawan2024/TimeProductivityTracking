using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        private readonly UserManager<IdentityAuthUser> _userManager;
        public InvoicesController(ProductivitiesContext context,
 
            UserManager<IdentityAuthUser> userManager)
        {
            _context = context;
     
            _userManager = userManager;
        }

        // GET: Invoices
        public async Task<IActionResult> Index()
        {
           
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            List<Invoice> invoices;

            if (await _userManager.IsInRoleAsync(user, "Manager")||await _userManager.IsInRoleAsync(user, "Admin"))
            {
                //Manager sees all invoices 
                invoices = await _context.Invoices
                    .Include(i => i.Contractor)
                    .ToListAsync();
            }
            else
            {
                //contractor sees only his invoices
                invoices = await _context.Invoices
                    .Include(i => i.Contractor)
                    .Where(i =>  i.Contractor !=null 
                                && i.Contractor.Email == user.Email)
                    .ToListAsync();
            }
            

        
            return View(invoices);


        }
        // GET: Invoice/Details/5
       public async Task<IActionResult> Details(int? contractorId, string? month,int? Id)
        {
            if (!ModelState.IsValid || contractorId == null)
            {
                return NotFound("Invalid contractorId or month");
            }

            var invoice = await _context.Invoices
                .Include(i => i.Contractor)
                 .FirstOrDefaultAsync(m => m.Id == Id && m.Month == month && m.ContractorId == contractorId); 
          

            return View(invoice);
        }

        [HttpPost]
        public async Task<IActionResult> ApproveInvoice(int id)
        {
            var invoice = await _context.Invoices.Include(i => i.Contractor).FirstOrDefaultAsync(i => i.Id == id);
            if (invoice == null) return NotFound();

            invoice.statusApproval = "Approved";
/*
            var productivity = await _context.Productivities
                .FirstOrDefaultAsync(p => p.ContractorId == invoice.ContractorId && p.Monthly == invoice.Month);

            if (productivity != null)
            {
                productivity.statusApproval = "Approved";
            }
*/

            var productivities = await _context.Productivities
    .Where(p => p.ContractorId == invoice.ContractorId && p.Monthly == invoice.Month)
    .ToListAsync();

            foreach (var p in productivities)
            {
                p.statusApproval = "Approved";
            }


            await _context.SaveChangesAsync();

            //  return RedirectToAction("Details", new { contractorId = invoice.ContractorId, month = invoice.Month, Id = invoice.Id });
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> RejectInvoice(int id)
        {
            var invoice = await _context.Invoices.Include(i => i.Contractor).FirstOrDefaultAsync(i => i.Id == id);
            if (invoice == null) return NotFound();

            invoice.statusApproval = "Rejected";

            var productivity = await _context.Productivities
                .FirstOrDefaultAsync(p => p.ContractorId == invoice.ContractorId && p.Monthly == invoice.Month);

            if (productivity != null)
            {
                productivity.statusApproval = "Rejected";
            }

            await _context.SaveChangesAsync();


            var toWaiting = await _context.Productivities
             .Where(p => p.Monthly == productivity!.Monthly && p.ContractorId == productivity.ContractorId)
             .ToListAsync();

                    if (toWaiting.Count == 0)
                    {
                        return NotFound("No productivities found for the given month and contractor");
                    }

                    //Load contractor info
                    foreach (var item in toWaiting)
                    {
                       item.statusApproval = "Waiting";
                    }
                   
                    await _context.SaveChangesAsync();// Save approval updates


            // return RedirectToAction("Details", new { contractorId = invoice.ContractorId, month = invoice.Month, Id = invoice.Id });
            return RedirectToAction("Index");
        }





    }



}
