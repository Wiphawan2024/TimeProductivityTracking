using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
        private readonly ILogger<UserInfoesController> _logger;
        public UserInfoesController(ProductivitiesContext context
            , RoleManager<IdentityRole> roleManager
            ,UserManager<IdentityAuthUser> userManager,
            ILogger<UserInfoesController> logger)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
            _logger = logger;
            
        }

        // GET: UserInfoes
        public async Task<IActionResult> Index()
        {     var users= await _context.Users
                .Include(u=>u.Rate)
                .ToListAsync();
         
            return View(users);
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
            ViewBag.Rate = new SelectList(_context.Rates
                .OrderBy(r=>r.RateName) 
                .Select(r => new
                {
                    r.RateID,
                    DisplayText = r.RateName + " - € " + r.HourlyWage
                }),
                    "RateID", "DisplayText"
                );

            return View();
        }

        // POST: UserInfoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,FName,LName,Phone,Email,Role,HireDate,RateID,Register=0")] UserInfo userInfo)
        {
            ViewBag.Rate = new SelectList(_context.Rates.Select(r=> new 
            { r.RateID,
              DisplayText=r.RateName+ " - €" + r.HourlyWage
            }),
              "RateID", "DisplayText"
                );


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

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ModelState.AddModelError("", "User not found.");
                return RedirectToAction(nameof(Index));
            }

            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                ModelState.AddModelError("", "Role does not exist.");
                return RedirectToAction(nameof(Index));
            }

            if (await _userManager.IsInRoleAsync(user, roleName))
            {
                ModelState.AddModelError("", "User is already in this role.");
                return RedirectToAction(nameof(Index));
            }

            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (result.Succeeded)
            {
                _logger.LogInformation($"Successfully assigned role '{roleName}' to user '{user.UserName}'.");
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Failed to assign role.");
            _logger.LogError($"Failed to assign role '{roleName}' to user '{user.UserName}'. Errors: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public async Task<JsonResult> CheckAndRegisterUser(string userId, string roleName)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(roleName))
            {
                return Json(new { success = false, message = "User ID and Role Name are required." });
            }

            // Find the user in AspNetUsers table
            var user = await _userManager.FindByIdAsync(userId) ?? await _userManager.FindByEmailAsync(userId);
            if (user == null)
            {
                return Json(new { success = false, message = "User not found." });
            }

            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                return Json(new { success = false, message = "Role not found." });
            }

            // Remove current roles
            var currentRoles = await _userManager.GetRolesAsync(user);
            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeResult.Succeeded)
            {
                return Json(new { success = false, message = $"Failed to remove existing roles: {string.Join(", ", removeResult.Errors.Select(e => e.Description))}" });
            }

            // Assign new role
            var addResult = await _userManager.AddToRoleAsync(user, roleName);
            if (!addResult.Succeeded)
            {
                return Json(new { success = false, message = $"Failed to assign new role: {string.Join(", ", addResult.Errors.Select(e => e.Description))}" });
            }

            // Update role in UserInfo table **without resetting Register**
            var userInfo = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            if (userInfo != null)
            {
                if (Enum.TryParse<Roles>(roleName, true, out var parsedRole))
                {
                    userInfo.Role = parsedRole;

                    // **Prevent resetting Register field to 0**
                    if (userInfo.Register == 0)
                    {
                        userInfo.Register = 1; // Mark as registered if not already
                    }

                    _context.Update(userInfo);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    return Json(new { success = false, message = "Invalid role name." });
                }
            }
            _logger.LogInformation($"User role updated successfully for {user.Email}");
            return Json(new { success = true, message = "User role updated successfully." });
        }

        public async Task<IActionResult>CheckAfterRegister(string email)
        {

            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("Invalid email.");
            }

            //Retrieve user information based on email
            var userInfo = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (userInfo == null)
            {
                return NotFound("User not found.");
            }

            try
            {
                //  Update the Register field if it’s not already set
                if (userInfo.Register == 0)
                {
                    userInfo.Register = 1;
                }

                //  Update Role in AspNetUserRoles
                var user = await _userManager.FindByEmailAsync(userInfo.Email);
                if (user != null)
                {
                    var currentRoles = await _userManager.GetRolesAsync(user);
                    if (currentRoles.IsNullOrEmpty())
                    {
                       /// await _userManager.RemoveFromRolesAsync(user, currentRoles);
                        await _userManager.AddToRoleAsync(user, userInfo.Role.ToString());
                    }
                }

                //  Save changes to database
                _context.Update(userInfo);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error updating user: {ex.Message}");
            }

            return Ok(new { success = true }); //  Return JSON response for AJAX
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
      

            var userInfo = await _context.Users.FindAsync(id);
            if (userInfo == null)
            {
                return NotFound();
            }

            ViewBag.Rate = new SelectList(_context.Rates.Select(r => new
            {
                r.RateID,
                DisplayText = r.RateName + " - €" + r.HourlyWage
            }),
             "RateID", 
             "DisplayText"
             );


       
            if (id == null)
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
        public async Task<IActionResult> Edit(int id, [Bind("UserId,FName,LName,Phone,Email,Role,HireDate,RateID,Register")] UserInfo userInfo)
        {
            if (id != userInfo.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Retrieve the existing UserInfo record
                    var existingUserInfo = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == id);
                    if (existingUserInfo == null)
                    {
                        return NotFound();
                    }

                    // Preserve Register value (don't reset it)
                    userInfo.Register = existingUserInfo.Register;

                    // Check if the role has changed
                    if (existingUserInfo.Role != userInfo.Role)
                    {
                        // Get the user from AspNetUsers
                        var user = await _userManager.FindByEmailAsync(userInfo.Email);
                        if (user != null)
                        {
                            // Remove the old role
                            var currentRoles = await _userManager.GetRolesAsync(user);
                            await _userManager.RemoveFromRolesAsync(user, currentRoles);

                            // Add the new role in AspNetUserRoles
                            await _userManager.AddToRoleAsync(user, userInfo.Role.ToString());
                        }
                    }

                    // Update UserInfo table
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
            ViewBag.Rate = new SelectList(_context.Rates, "RateID", "RateName", userInfo.RateID);
            return View(userInfo);
        }

        // GET: Productivities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
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
