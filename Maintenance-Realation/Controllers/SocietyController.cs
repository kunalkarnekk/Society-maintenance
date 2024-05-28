using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Society_DataAccess.Data;
using Society_Models;
using Society_Models.ViewModels;
using System.Security.Claims;

namespace Maintenance_Realation.Controllers
{
    [Authorize]
    public class SocietyController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SocietyController(ApplicationDbContext context , IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<IActionResult> AddExpense()
        {
            var societyId = 0;
            if (_httpContextAccessor.HttpContext.User != null)
            {
                var userId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (userId != null)
                {
                    var user = await _context.ApplicationUser.FindAsync(userId);

                    societyId = user.SocietyId;
                }
            }

            var SocietyExpenseVm = new SocietyExpenseViewModel()
            {
                ExpensesList = await _context.Expenses
                        .Where(x => x.SocietyId == societyId).Select(u => new SelectListItem
                        {
                            Text = u.ExpenseName,
                            Value = u.ExpenseId.ToString()
                        }).ToListAsync(),


                SocietyExpense = new SocietyExpense()

            };

           
            return View(SocietyExpenseVm);
        }

        [HttpPost]
        public async Task<IActionResult> AddExpense(SocietyExpenseViewModel expense)
        {
            if(ModelState.IsValid)
            {
                if(expense.SocietyExpense.ExpenseId == 0)
                {
                    await _context.AddAsync(expense.SocietyExpense);
                    TempData["success"] = "Society Expense Added Successfully";
                }
                else
                {
                    _context.Update(expense.SocietyExpense);
                    TempData["success"] = "Society Updated Added Successfully";

                }

                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index), "DashBoard");
        }

    }

}
