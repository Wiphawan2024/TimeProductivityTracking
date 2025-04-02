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
                    .Where(i => i.Contractor.Email == user.Email)
                    .ToListAsync();
            }
            

            if (!invoices.Any())
            {
                return NotFound("No invoices found for the given contractor");
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
            if (invoice == null)
            {
                return NotFound("Not foud data for the given contractor and month");
            }

            return View(invoice);
        }

       

    }



}
