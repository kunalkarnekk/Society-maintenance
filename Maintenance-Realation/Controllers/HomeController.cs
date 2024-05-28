using Maintenance_Realation.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Society_DataAccess.Data;
using Society_Models;
using System.Diagnostics;
using System.Security.Claims;

namespace Maintenance_Realation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _context;


      

        public HomeController(ILogger<HomeController> logger , IHttpContextAccessor httpContextAccessor, ApplicationDbContext context )
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public async  Task<IActionResult> Index()
        {

            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);



            Society society = null;
            if (userId != null)
            {
                return RedirectToAction("Index", "DashBoard");
                var User = await _context.ApplicationUser.FindAsync(userId);
                society = await _context.Society.FindAsync(User.SocietyId);
                ViewBag.societyName = society.Name;
            }





            if (HttpContext.Session.GetString("User") != null)
            {
                return RedirectToAction("DashBoard", "Admin");
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
