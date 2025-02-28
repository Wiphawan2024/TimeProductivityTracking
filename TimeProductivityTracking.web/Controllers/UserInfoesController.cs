using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TimeProductivityTracking.web.Areas.Identity.Data;
using TimeProductivityTracking.web.Data;
using TimeProductivityTracking.web.Models;

namespace TimeProductivityTracking.web.Controllers
{
    public class UserInfoesController : Controller
    {
        private readonly ProductivitiesContext _context;
        private UserManager<IdentityAuthUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public UserInfoesController(ProductivitiesContext context
            , RoleManager<IdentityRole> roleManager
            ,UserManager<IdentityAuthUser> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // GET: UserInfoes
        public async Task<IActionResult> Index()
        {
            //var users=await _context.Users.Where(a=>a.Role==Roles.User || a.Role==Roles.);
           
            return View(await _context.Users.ToListAsync());


        }

        // GET: UserInfoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userInfo = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (userInfo == null)
            {
                return NotFound();
            }

            return View(userInfo);
        }

        // GET: UserInfoes/Create
        public IActionResult Create()

        {
            ViewBag.Rate = new SelectList(_context.Rates, "RateID", "RateName");
            return View();
        }

        // POST: UserInfoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,FName,LName,Phone,Email,Role,HireDate,RateID")] UserInfo userInfo)
        {
            ViewBag.Rate = new SelectList(_context.Rates, "RateID", "RateName");
            if (ModelState.IsValid)
            {
                string roleName = userInfo.Role.ToString();
                string userId = userInfo.UserId.ToString();

                _context.Add(userInfo);
                // await _userManager.AddToRoleAsync(user, roleName);//Add Role To AspNetRole
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

                /*
                var user =await _userManager.FindByNameAsync(userId);

                if (user !=null)
                {
                    _context.Add(userInfo);
                   await _userManager.AddToRoleAsync(user, roleName);//Add Role To AspNetRole
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));

                } */


            }
            return View(userInfo);
        }

        // GET: UserInfoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Rate = new SelectList(_context.Rates, "RateID", "RateName");
            if (id == null)
            {
                return NotFound();
            }

            var userInfo = await _context.Users.FindAsync(id);
            if (userInfo == null)
            {
                return NotFound();
            }
            ViewBag.HireDate=userInfo.HireDate;
            return View(userInfo);
        }

        // POST: UserInfoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,FName,LName,Phone,Email,Role,HireDate,RateID")] UserInfo userInfo)
        {
            IdentityResult result;
            string role=userInfo.Role.ToString();   

            if (id != userInfo.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userInfo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserInfoExists(userInfo.UserId))
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
            return View(userInfo);
        }

        // GET: UserInfoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userInfo = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (userInfo == null)
            {
                return NotFound();
            }

            return View(userInfo);
        }

        // POST: UserInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userInfo = await _context.Users.FindAsync(id);
            if (userInfo != null)
            {
                _context.Users.Remove(userInfo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserInfoExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}
