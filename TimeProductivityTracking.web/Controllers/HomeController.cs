using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Diagnostics;
using TimeProductivityTracking.web.Areas.Identity.Data;
using TimeProductivityTracking.web.Data;
using TimeProductivityTracking.web.Models;

namespace TimeProductivityTracking.web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ProductivitiesContext _context;
        private readonly UserManager<IdentityAuthUser> _userManager;


        public HomeController(ILogger<HomeController> logger, UserManager<IdentityAuthUser> userManager
            , ProductivitiesContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);


            if (user == null)
            {

                ViewBag.Message = "Please Login to system. ";
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }
            else
            {

                var result = from a in _context.Users.Where(aa => aa.Email == user.Email)
                             select new
                             {
                                 a.FName,
                                 a.LName
                             };

                string? Fname = null;
                string? Lname = null;
                foreach (var i in result)
                {
                    Fname = i.FName;
                    Lname = i.LName;

                }
                ViewBag.Message = Fname + " " + Lname;
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
