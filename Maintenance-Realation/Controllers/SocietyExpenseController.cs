using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Society_DataAccess.Data;
using Society_DataAccess.Repository.IRepository;
using Society_Models;
using Society_Models.ViewModels;
using System.Security.Claims;

namespace Maintenance_Realation.Controllers
{
    public class SocietyExpenseController : Controller
    {
        private readonly IUnitOfWorkRepository _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ApplicationDbContext _context;


        public SocietyExpenseController(IUnitOfWorkRepository unitOfWork,
            IHttpContextAccessor contextAccessor , ApplicationDbContext context)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
            _context = context;
        }
        public async Task<IActionResult> Index(DateOnly? date)
        {
            int currentMonth = 0;
            int currentYear = 0;

           if(date != null)
            {
                var dates = date.ToString();
                string[]? datesParts = null;
                if (dates != null)
                {
                    datesParts = dates.Split("-");
                }
               

                if (datesParts?.Length == 3)
                {
                    currentMonth = int.Parse(datesParts[1]);
                    currentYear = int.Parse(datesParts[2]);
                }

            }
            else 
            {
                 DateTime now = DateTime.Now;
                 currentMonth = now.Month;
                 currentYear = now.Year;
            }
            

            List<Expense> expenses = await _context.Expenses
                .Where(e => e.ExpenseDate != null && e.ExpenseDate.Month == currentMonth && e.ExpenseDate.Year == currentYear).ToListAsync();


            foreach (Expense e in expenses)
            {
                ApplicationUser  user = _unitOfWork.ApplicationUser.Get(u => u.Id == e.ExpenseBy);

                if(user != null)
                {
                    e.ExpenseBy = user.Name;
                }

                
            }

            decimal ExpenseAmountMonthly = await _context.Expenses
                .Where(e => e.ExpenseDate.Month == currentMonth && e.ExpenseDate.Year == currentYear)
                .Select(s => s.ExpenseAmount).SumAsync();

            ViewBag.ExpenseAmountMonthly = ExpenseAmountMonthly;



            return View(expenses);
        }

        public IActionResult Upsert(int? id)
        {
            if(id == 0 || id == null)
            {
                return View(new Expense());
            }

            Expense expense = new Expense();
            expense = _unitOfWork.SocietyExpense.Get(s => s.ExpenseId == id);

            if(expense == null)
            {
                return NotFound();
            }


            return View(expense);
        }

        [HttpPost]
        public async Task<IActionResult> Upsert(Expense expense)
        {
            var societyId = 0;
            if(_contextAccessor.HttpContext.User != null)
            {
                var AdminId = _contextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if(AdminId != null)
                {
                    var admin =await _context.ApplicationUser.FindAsync(AdminId);
                    if(admin != null)
                    {
                        societyId = admin.SocietyId;
                    }
                }
            }

                if(expense.ExpenseId == 0 )
                {

                    expense.ExpenseDate = DateTime.Now;
                    string userId = _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                    expense.ExpenseBy = userId;
                    expense.SocietyId = societyId;
                    _unitOfWork.SocietyExpense.Add(expense);
                    
                    TempData["success"] = "Society Maintenance Added Successfully";
                   
                }
                else
                {
                   _unitOfWork.SocietyExpense.Update(expense);
                   TempData["success"] = "Society Maintenance Updated succeessfully";
                       
                    
                }
                _unitOfWork.Save();
                return RedirectToAction("Index");

            

            return View(expense);
        }

    
        public IActionResult Delete(int? id)
        {
            if(id == 0 || id == null)
            {
                return NotFound();
            }

            Expense expense = _unitOfWork.SocietyExpense.Get(s => s.ExpenseId == id);

            if(expense == null)
            {
                return NotFound();
            }
            else
            {
                _unitOfWork.SocietyExpense.Remove(expense);
                _unitOfWork.Save();
                
            }



            return RedirectToAction("Index");
        }
    }
}
