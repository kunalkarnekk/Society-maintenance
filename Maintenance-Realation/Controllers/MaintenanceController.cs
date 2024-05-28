using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Society_DataAccess.Data;
using Society_Models;
using Society_Models.ViewModels;
using System.Globalization;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Maintenance_Realation.Controllers
{
    public class MaintenanceController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        
        public MaintenanceController(ApplicationDbContext context , IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;

        }
        public async Task<IActionResult> Index(string? month , int? Year)
        { 
            
          
            if (string.IsNullOrEmpty(month))
            {
                month = DateTime.Now.ToString("MMMM");
            }

            if(Year == null)
            {
                Year = DateTime.Now.Year;
            }
            

            ClaimsPrincipal currentUser = this.User as ClaimsPrincipal;

            string UserId = null;

            if (currentUser != null)
            {
                UserId = currentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            }

            var user = _context.ApplicationUser.Find(UserId);
            var societyId = user.SocietyId;

          

            var ReceivedAmount = await _context.Maintenances
                .Where(u => u.Month == month  && u.Year == Year &&  u.FlatOwner.SocietyId == societyId)
                .Select(u => u.ReceivedAmount).SumAsync();

            var MaintenanceAmount = await _context.Maintenances
                  .Where(u => u.Month == month && u.Year == Year && u.FlatOwner.SocietyId == societyId)
                  .Select(s => s.Amount).SumAsync();

            var PendingAmount = await _context.Maintenances
                .Where(u => u.Month == month && u.Year == Year && u.FlatOwner.SocietyId == societyId)
                .Select(u => u.PendingAmount).SumAsync();

            var PendingFineAmount = await _context.Maintenances
                    .Where(u => u.Month == month && u.Year == Year && u.FlatOwner.SocietyId == societyId)
                    .Select(u => u.PendingAmount).SumAsync();

            int currentMonth = DateTime.Now.Month;
            int currentYear = DateTime.Now.Year;

            var ExpenseAmount = await _context.Expenses
                .Where(u => u.ExpenseDate.Month == currentMonth && u.ExpenseDate.Year == currentYear && u.SocietyId == societyId)
                .Select(u => u.ExpenseAmount).SumAsync();

            ViewBag.maintenanceAmount = MaintenanceAmount;
            ViewBag.ReceivedAmount = ReceivedAmount;
            ViewBag.PendingAmount = PendingAmount;
            ViewBag.PendingFineAmount = PendingFineAmount;
            ViewBag.ExpenseAmount = ExpenseAmount;


            List<FlatOwner> flatOwner = await _context.FlatOwner.Where(f => f.SocietyId == societyId)
                                 .ToListAsync();

            float OwnerAmount = 0;
            float RentAmount = 0;
            foreach (var owner in flatOwner)
            {
                Maintenance maintenance = new Maintenance();

                maintenance.Amount = owner.OwnerType == OwnerType.Own ? 500 : 700;
                maintenance.ReceivedAmount = 0;
                maintenance.PendingAmount = 0;
                maintenance.FineAmount = 0;
                maintenance.OwnerId = owner.Id;
                maintenance.Month = DateTime.Now.ToString("MMMM");
                maintenance.Year = DateTime.Now.Year;
                maintenance.ReceivedDate = DateOnly.FromDateTime(DateTime.Today);
                maintenance.EntryDate = DateOnly.FromDateTime(DateTime.Today);


                string currentMonth1 = DateTime.Now.ToString("MMMM");
                int CurrentYear = DateTime.Now.Year;

                bool maintenanceExist = await _context.Maintenances
                        .AnyAsync(s => s.OwnerId == owner.Id
                                       && s.Month == currentMonth1
                                       && s.Year == CurrentYear);

                if (!maintenanceExist)
                {
                    _context.Maintenances.AddAsync(maintenance);
                    _context.SaveChanges();
                }


            }





            if (user != null)
            {
                MaintenanceVM maintenanceVM = new()
                {
                    Maintenance1 = await _context.Maintenances
                    .Include(f => f.FlatOwner)
                    .Where(m => m.FlatOwner.SocietyId == societyId && m.Month == month && m.Year == Year).ToListAsync(),
                   
                    MonthsList = new List<SelectListItem>(),

                    
                };

                foreach (var maintenance in maintenanceVM.Maintenance1)
                {
                    var ApplicationUser = await _context.ApplicationUser.FindAsync(maintenance.ModifiedBy);
                    if(ApplicationUser != null)
                    {
                        maintenance.ModifiedBy = ApplicationUser.Name;
                    }
                }
                return View(maintenanceVM);       
            }

            return View();
        }

        [HttpPost]
        public IActionResult Index(int id)
        {
            return View();
            
        }





        [HttpGet]
        public IActionResult UpdateMonth(string direction)
        {
            // Get current month
            var currentMonth = DateTime.Now.Month;

            // Calculate new month based on direction
            if (direction == "prev")
            {
                currentMonth--;
                if (currentMonth == 0)
                    currentMonth = 12;
            }
            else if (direction == "next")
            {
                currentMonth++;
                if (currentMonth == 13)
                    currentMonth = 1;
            }

            // Get month name based on new month
            var newMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(currentMonth);

            return Content(newMonth);
        }

        public async Task<IActionResult> GetDataByMonthYear(string month)
        {
            var data = await _context.Maintenances.Where(m => m.Month == month).ToListAsync();
            return View(data);
        }

        public IActionResult Create()
        {
            
            
                ClaimsPrincipal CurrentUser = this.User as ClaimsPrincipal;
                string userId = null;

                if (CurrentUser != null)
                {
                    userId = CurrentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                }

                var MaintenanceVM = _context.ApplicationUser.Where(u => u.Id == userId)
                     .Select(u => new MaintenanceVM
                     {
                         FlatOwnersList = _context.FlatOwner.Where(f => f.SocietyId == u.SocietyId).Select(x =>
                            new SelectListItem
                            {
                                Text = x.FirstName + " " + x.LastName,
                                Value = x.Id.ToString()
                            })
                         .ToList()
                     })
                     .FirstOrDefault();

                return View(MaintenanceVM);

           
            
            
        }

        [HttpPost]
        public async Task<IActionResult> Create(MaintenanceVM m )
        {
            int settings = await _context.Settings.CountAsync();

            if (settings > 0)
            {
                string currentMonth = DateTime.Now.ToString("yyyy-MM");
                int CurrentYear = DateTime.Now.Year;

                bool maintenanceExist = await _context.Maintenances
                        .AnyAsync(s => s.OwnerId == m.Maintenance.OwnerId
                                       && m.Maintenance.Month == currentMonth
                                       && m.Maintenance.Year == CurrentYear);

                if (maintenanceExist)
                {
                    TempData["success"] = $"Maintenance is Already Added {DateTime.Now.ToString("MMMM")} month";

                }
                else
                {
                    var owner = await _context.FlatOwner.FindAsync(m.Maintenance.OwnerId);

                    float? amount = null;
                    float? PendingAmount = 0;
                    float? FineAmount = 0;
                    if (owner != null)
                    {
                        if (owner.OwnerType == OwnerType.Own)
                        {
                            amount = await _context.Settings.Select(u => u.OwnerMaintenanceCost).FirstOrDefaultAsync();
                        }
                        else if (owner.OwnerType == OwnerType.Rent)
                        {
                            amount = await _context.Settings.Select(u => u.RentMaintenanceCost).FirstOrDefaultAsync();
                        }

                    }
                    if (m.Maintenance.ReceivedAmount < amount)
                    {
                        PendingAmount = amount - m.Maintenance.ReceivedAmount;
                    }

                    var date = await _context.Settings.Select(u => u.MaintenanceDate).FirstOrDefaultAsync();

                    //if (date < m.Maintenance.ReceivedDate)
                    //{
                    //    FineAmount = await _context.Settings.Select(u => u.FineAmount).FirstOrDefaultAsync();
                    //}

                    m.Maintenance.Amount = (float)amount;
                    m.Maintenance.PendingAmount = (float)PendingAmount;
                    m.Maintenance.FineAmount = (float)FineAmount;
                    var month = m.Maintenance.Month;

                    
                    var parts = month.Split("-");

                    if (parts.Length > 1)
                    {
                        var year = parts[0];

                        if (int.TryParse(parts[1], out int monthNumber))
                        {
                            string monthName = new DateTime(Convert.ToInt32(year), monthNumber, 1).ToString("MMMM");

                            m.Maintenance.Month = monthName;
                        }
                    }


                    await _context.Maintenances.AddAsync(m.Maintenance);




                    await _context.SaveChangesAsync();
                    TempData["success"] = "Maintenance Added successfully";


                }

                return RedirectToAction(nameof(Index));

            }
            else
            {
                TempData["success"] = "First we need add settings";
                return RedirectToAction("Index", "Settings");
            }


            return View(m);
           
        }


        public IActionResult Upsert(int? id )
        {
            Maintenance obj = new();

            if(id == 0 || id == null)
            {
                return View(obj);
            }
            //edit

            obj = _context.Maintenances.Where(m => m.Id == id).FirstOrDefault();
            if(obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }
        

        public async Task<IActionResult> Edit(int? id)
        {
            ClaimsPrincipal CurrentUser = this.User as ClaimsPrincipal;
            string userId = null;

            if (CurrentUser != null)
            {
                userId = CurrentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            }

            MaintenanceVM maintenanceVM = new MaintenanceVM();
          

            Maintenance obj = await _context.Maintenances.Where(u => u.Id == id).FirstOrDefaultAsync();
            if(obj == null)
            {
                return NotFound();
            }
            var flatOwner = await _context.FlatOwner.FindAsync(obj.OwnerId);
            if(flatOwner != null)
            {
                ViewData["OwnerName"] = flatOwner.FirstName + " " + flatOwner.LastName;
            }
            maintenanceVM.Maintenance = obj;
            return View(maintenanceVM);
        }

        [HttpPost]

        public async Task<IActionResult> Edit(MaintenanceVM m)
        {
            Maintenance maintenance = await _context.Maintenances.FindAsync(m.Maintenance.Id);

                
                if (maintenance != null)
                {
                   var UserID = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    var date = await _context.Settings
                            .Select(s => s.MaintenanceDate).FirstOrDefaultAsync();
                   
                    float PendingAmount = m.Maintenance.Amount - m.Maintenance.ReceivedAmount;
                    maintenance.OwnerId = m.Maintenance.OwnerId;
                    maintenance.Amount = m.Maintenance.Amount;
                    maintenance.ReceivedAmount = m.Maintenance.ReceivedAmount;
                    maintenance.PendingAmount = PendingAmount + m.Maintenance.FineAmount;
                  
                    maintenance.ReceivedDate = m.Maintenance.ReceivedDate;
                    maintenance.EntryDate = m.Maintenance.EntryDate;
                    maintenance.CreationDate = DateTime.Now.ToString("MMMM-yyyy");
                    maintenance.ModifiedDate = DateTime.Now.ToString("MMMM-yyyy");
                    
                    if(UserID != null)
                    {
                      maintenance.ModifiedBy = UserID;
                    }



                if (m.Maintenance.ReceivedDate > date)
                {
                    maintenance.FineAmount = await _context.Settings.Select(s => s.FineAmount).FirstOrDefaultAsync();
                }


                try
                {
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        
                        throw;
                    }

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return NotFound(); 
                }
            

            return View(m); 
        }

        public async Task<IActionResult> PayPendingAmount(int? id)
        {
            Maintenance obj = await _context.Maintenances.Where(u => u.Id == id).FirstOrDefaultAsync();

            if(obj != null)
            {
                obj.PendingAmount = obj.PendingAmount + obj.FineAmount;
                return View(obj);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PayPendingAmount(Maintenance m)
        {
            Maintenance maintenance = await _context.Maintenances.FindAsync(m.Id);
            if(maintenance != null)
            {

                maintenance.ReceivedAmount = maintenance.ReceivedAmount + m.PendingAmount;
                maintenance.PendingAmount = 0;
                maintenance.PendingAmountReceivedDate = m.PendingAmountReceivedDate;
            }
           
                
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            
          
        }


        public IActionResult Delete(int? id)
        {
            if(id == 0 || id == null)
            {
                return NotFound();
            }

            Maintenance obj = _context.Maintenances.Where(u => u.Id == id).FirstOrDefault();

            if(obj == null)
            {
                return NotFound();
            }

            _context.Maintenances.Remove(obj);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> YearlyMaintenance()
        {
            ClaimsPrincipal currentUser = this.User as ClaimsPrincipal;

            string UserId = null;

            if (currentUser != null)
            {
                UserId = currentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            }

            ApplicationUser user = await _context.ApplicationUser.FindAsync(UserId);

            int currentYear = DateTime.Now.Year;

            List<Maintenance> maintenances = await _context.Maintenances
                            .Where(m => m.FlatOwner.SocietyId == user.SocietyId
                             && m.Year == currentYear).ToListAsync();

            return View(maintenances);
        }
    }
}
