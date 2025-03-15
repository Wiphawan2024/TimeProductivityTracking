using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
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
                   _context.Add(userInfo);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
            }
            return View(userInfo);
        }

        public async Task<IActionResult> Register(string userId, string roleName)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(roleName))
            {
                ModelState.AddModelError("", "User ID and Role Name are required.");
                return RedirectToAction(nameof(Index));
            }

            var user = await _userManager.FindByIdAsync(userId); // : Use FindByIdAsync 
            if (user == null)
            {
                ModelState.AddModelError("", "User not found.");
                return RedirectToAction(nameof(Index));
            }

        
            // Check if user already has the role
            if (await _userManager.IsInRoleAsync(user, roleName))
            {
                ModelState.AddModelError("", "User is already in this role.");
                return RedirectToAction(nameof(Index));
            }

            // Assign the role
            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home"); // ✅ Redirect to Home after successful role assignment
            }

            ModelState.AddModelError("", "Failed to assign role.");
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<JsonResult> CheckAndRegisterUser(string userId, string roleName)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(roleName))
            {
                return Json(new { success = false, message = "User ID and Role Name are required." });
            }

            // Find the user by Email (or use FindByIdAsync if using UserId)
            var user = await _userManager.FindByEmailAsync(userId);
            if (user == null)
            {
                return Json(new { success = false, message = "User not found." });
            }

            // Get all current roles assigned to the user
            var currentRoles = await _userManager.GetRolesAsync(user);

            // If the user already has the correct role, return success
            if (currentRoles.Contains(roleName))
            {
                return Json(new { success = true, message = "User is already in the correct role." });
            }

            // Remove the user from all current roles
            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeResult.Succeeded)
            {
                return Json(new { success = false, message = "Failed to remove existing roles." });
            }

            // Add user to the new role
            var addResult = await _userManager.AddToRoleAsync(user, roleName);
            if (addResult.Succeeded)
            {
                return Json(new { success = true, message = "User role updated successfully." });
            }
            else
            {
                return Json(new { success = false, message = "Failed to assign new role." });
            }
        }

        public async Task<IActionResult> AddUserToRole(string userId, string roleName)
        {
            var user = await _userManager.FindByNameAsync(userId);
            if (user != null)
            {
                await _userManager.AddToRoleAsync(user, roleName);
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
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
