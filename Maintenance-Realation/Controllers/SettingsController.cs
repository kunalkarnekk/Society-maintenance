using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Society_DataAccess.Data;
using Society_DataAccess.Repository.IRepository;
using Society_Models;
using Society_Models.ViewModels;
using System.Security.Claims;

namespace Maintenance_Realation.Controllers
{
    public class SettingsController : Controller
    {
        private readonly IUnitOfWorkRepository _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _context;

        public SettingsController(IUnitOfWorkRepository unitOfWork, IHttpContextAccessor httpContextAccessor, ApplicationDbContext context)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }
        public IActionResult Index()
        {
            List<Settings> obj = _unitOfWork.Settings.GetAll().ToList();
            return View(obj);
        }

        public async Task<IActionResult> Upsert()
        {
            var SocietyId = 0;
            if (_httpContextAccessor.HttpContext.User != null)
            {
                var userId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value;

                if (userId != null)
                {
                    var user = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);

                    if (user != null)
                    {
                        SocietyId = user.SocietyId;
                    }
                }
            }

            var settings = await _context.Settings.FirstOrDefaultAsync();

            if (settings == null)
            {
                var model1 = new Settings();
                model1.SocietyId = SocietyId;
                return View(model1);
            }

            var model = new Settings
            {
                Id = settings.Id,
                RentMaintenanceCost = settings.RentMaintenanceCost,
                FineAmount = settings.FineAmount,
                MaintenanceDate = settings.MaintenanceDate,
                OwnerMaintenanceCost = settings.OwnerMaintenanceCost,
                SocietyId = SocietyId
            };
            return View(model);


        }

        [HttpPost]
        [Authorize]
        public IActionResult Upsert(Settings settings)
        {


            if (settings.Id == 0)
            {
                _unitOfWork.Settings.Add(settings);
                TempData["success"] = "Settings Added Successfully";

            }
            else
            {
                _unitOfWork.Settings.Update(settings);
                TempData["success"] = "Settings Updated Successfully";

            }
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index), "DashBoard");


        }

        public IActionResult Delete(int? id)
        {
            if (id != null)
            {
                Settings obj = _unitOfWork.Settings.Get(u => u.Id == id);

                if (obj == null)
                {
                    return NotFound();
                }

                _unitOfWork.Settings.Remove(obj);
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
