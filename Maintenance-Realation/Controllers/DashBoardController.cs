using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Society_DataAccess.Data;
using Society_Models;
using System.Security.Claims;

namespace Maintenance_Realation.Controllers
{
    public class DashBoardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DashBoardController(ApplicationDbContext context , IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;

        }
        public async Task<IActionResult> Index()
        {

            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            

            Society society = null;
            if (userId != null)
            {
                
                var User = await _context.ApplicationUser.FindAsync(userId);
                society = await _context.Society.FindAsync(User.SocietyId);
                _httpContextAccessor.HttpContext.Session.SetString("SocietyName" , society.Name);
            }

            ApplicationUser? user = null;
            if(userId != null)
            {
                user = _context.ApplicationUser.Find(userId);
            }

            var societyId = user.SocietyId;

            


            int flatOwnerOwn = await _context.FlatOwner
                        .Where(f => f.SocietyId == societyId 
                        && f.OwnerType == OwnerType.Own
                        ).CountAsync();
            ViewBag.FlatOwnersCountOwn = flatOwnerOwn;

            int flatOwnerRent = await _context.FlatOwner
                       .Where(f => f.SocietyId == societyId
                       && f.OwnerType == OwnerType.Rent
                       ).CountAsync();
            ViewBag.FlatOwnersRentCount = flatOwnerRent;

            

          
            string currentMonth = DateTime.Now.ToString("MMMM");
            int currentYear = DateTime.Now.Year;

            float CreditAmount = await _context.Maintenances
                        .Where(m => m.FlatOwner.SocietyId == societyId
                               && m.Month == currentMonth
                               && m.Year == currentYear)
                        .Select(x => x.ReceivedAmount).SumAsync();

            float PendingAmount = await _context.Maintenances
                        .Where(m => m.FlatOwner.SocietyId == societyId
                        && m.Month == currentMonth
                        && m.Year == currentYear)
                        .Select(u => u.PendingAmount).SumAsync();

            float PendingFineAmount = await _context.Maintenances
                        .Where(m => m.FlatOwner.SocietyId == societyId
                        && m.Month == currentMonth
                        && m.Year == currentYear)
                        .Select(u => u.FineAmount).SumAsync();

            ViewBag.creditAmount = CreditAmount;
            ViewBag.pendingAmount = PendingAmount;
            ViewBag.PendingFineAmount = PendingFineAmount;

            float CreditAmountYear = await _context.Maintenances
                        .Where(m => m.FlatOwner.SocietyId == societyId
                               && m.Year == currentYear)
                        .Select(x => x.ReceivedAmount).SumAsync();

            float PendingAmountYear = await _context.Maintenances
                        .Where(m => m.FlatOwner.SocietyId == societyId
                        && m.Year == currentYear)
                        .Select(u => u.PendingAmount).SumAsync();

            float PendingAmountFineAmountYear = await _context.Maintenances
                        .Where(m => m.FlatOwner.SocietyId == societyId
                        && m.Year == currentYear)
                        .Select(u => u.FineAmount).SumAsync();

            ViewBag.CreditAmountYear = CreditAmountYear;
            ViewBag.PendingAmountYear = PendingAmountYear;
            ViewBag.PendingFineAmountYear = PendingAmountFineAmountYear;

            DateTime now = DateTime.Now;
            int currentMonthExpense = now.Month;
            int currentYearExpense = now.Year;


            decimal MonthlyExpense = await _context.Expenses
                .Where(e => e.ExpenseDate.Month == currentMonthExpense && e.ExpenseDate.Year == currentYearExpense)
                .Select(s => s.ExpenseAmount).SumAsync();

            decimal YearlyExpense = await _context.Expenses
             .Where(e => e.ExpenseDate.Year == currentYearExpense)
             .Select(s => s.ExpenseAmount).SumAsync();

            ViewBag.MonthlyExpense = MonthlyExpense;
            ViewBag.YearlyExpense = YearlyExpense;



            return View();
        }
    }
}
