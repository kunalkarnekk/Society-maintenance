using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Society_DataAccess.Data;
using Society_DataAccess.Services;
using Society_Models;
using Society_Models.ViewModels;
using System.Security.Claims;
using static System.Net.WebRequestMethods;

namespace Maintenance_Realation.Controllers
{
    public class FlatOwnerController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationUser> _roleManager;
        private readonly IEmailSender _emailSender;

        public FlatOwnerController
            (ApplicationDbContext db, 
            IHttpContextAccessor httpContextAccessor , 
            UserManager<ApplicationUser> userManager,
            IEmailSender emailSender
            )
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _emailSender = emailSender;
        }


        public async Task<IActionResult> Index()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId != null)
            {
                var user = _db.ApplicationUser.Find(userId);

                List<FlatOwner> obj = await _db.FlatOwner.Where(f => f.SocietyId == user.SocietyId).ToListAsync();
                return View(obj);
            }

            return View();
        }


        public IActionResult Create()
        {

            FlatOwnerVM owner = new FlatOwnerVM();
            owner.DesignationList = _db.Disignations.Select(s => new SelectListItem
            {
                Text = s.DisignationName,
                Value = s.DisignationName
            });
            owner.FlatOwner = new FlatOwner();
           
            return View(owner);

        }

        [HttpPost]
        public async Task<IActionResult> Create(FlatOwnerVM owner)
        {
            int societyId = 0;
            var AdminName = "";
            var AdminEmail = "";



            if (_httpContextAccessor.HttpContext.User != null)
            {
                var userId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var user = await _db.ApplicationUser.FindAsync(userId);

                if (user != null)
                {
                    societyId = user.SocietyId;
                    AdminName = user.Name;
                    AdminEmail = user.Email;
                    
                }
            }
            owner.FlatOwner.SocietyId = societyId;
            await _db.FlatOwner.AddAsync(owner.FlatOwner);
            await _db.SaveChangesAsync();

            var societyName = "";
            if (societyId != null)
            {
                var society = await _db.Society.FindAsync(societyId);
                societyName = society.Name;
            }

            if (owner.FlatOwner.Email != null || societyId != null || owner.FlatOwner.Id != null || owner.FlatOwner.DesiginationName != null)
            {
                ApplicationUser applicationUser = new()
                {
                    UserName = owner.FlatOwner.Email,
                    Email = owner.FlatOwner.Email,
                    SocietyId = societyId,
                    Name = owner.FlatOwner.FirstName + " " + owner.FlatOwner.LastName
                    

                };

                var password = owner.FlatOwner.Email + owner.FlatOwner.Id;

                var result = await _userManager.CreateAsync(applicationUser , password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(applicationUser, owner.FlatOwner.DesiginationName);

                    var passwordChangeLink = "https://localhost:7208/Admin/ForgotPassword";

                    var htmlMessage = $"Subject: Welcome to {societyName} Society {Environment.NewLine} Dear {owner.FlatOwner.FirstName + " " + owner.FlatOwner.LastName} {Environment.NewLine} We are delighted to inform you that your account has been successfully registered with the {societyName} society. Your designated role is {owner.FlatOwner.DesiginationName}, granting you authorized access to the {societyName} society account.{Environment.NewLine} Below are the credentials required for accessing your account: {Environment.NewLine} Username: {owner.FlatOwner.Email} {Environment.NewLine} Password: {password} {Environment.NewLine} Please change your password using {passwordChangeLink} link. {Environment.NewLine} Welcome aboard! {Environment.NewLine} Warm regards, {Environment.NewLine}  { AdminName} {Environment.NewLine} { societyName} {owner.FlatOwner.DesiginationName} {Environment.NewLine} { AdminEmail} ";

                    await _emailSender.SendEmailAsync(applicationUser.Email, "Society Maintenance - Change Password ", htmlMessage);
                }
            }

            TempData["success"] = "Flat Owner Added SuccessFully";
            return RedirectToAction(nameof(Index));


        }

        public async Task<IActionResult> Edit(int? id)
        {

            var UserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (UserId != null)
            {
                FlatOwnerVM flatOwnerVm = new FlatOwnerVM();
                if (id == null)
                {
                    return NotFound();
                }

                var user = await _db.FlatOwner.Where(u => u.Id == id).FirstOrDefaultAsync();

                if (user == null)
                {
                    return NotFound();
                }

                flatOwnerVm.FlatOwner = user;

                return View(flatOwnerVm);
            }

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Edit(FlatOwnerVM owner)
        {

            try
            {
                var existringOwner = await _db.FlatOwner.FindAsync(owner.FlatOwner.Id);

                if (existringOwner != null)
                {
                    existringOwner.FirstName = owner.FlatOwner.FirstName;
                    existringOwner.MiddleName = owner.FlatOwner.MiddleName;
                    existringOwner.LastName = owner.FlatOwner.LastName;
                    existringOwner.LastName = owner.FlatOwner.LastName;
                    existringOwner.Email = owner.FlatOwner.Email;
                    existringOwner.ContactNumber = owner.FlatOwner.ContactNumber;
                    existringOwner.FlatNumber = owner.FlatOwner.FlatNumber;
                    existringOwner.OwnerType = owner.FlatOwner.OwnerType;
                    existringOwner.IsSocietyMember = owner.FlatOwner.IsSocietyMember;
                    existringOwner.IsActive = owner.FlatOwner.IsActive;

                    _db.FlatOwner.Update(existringOwner);
                    await _db.SaveChangesAsync();
                    TempData["success"] = "Flat Owner Update Successfully";
                    return RedirectToAction(nameof(Index));



                }
                else
                {
                    TempData["error"] = "Flat Owner not found";
                }
            }
            catch (DbUpdateException ex)
            {

                TempData["error"] = "An error occurred while updating the Flat Owner";

            }

            return RedirectToAction(nameof(Index));


        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            FlatOwner obj = await _db.FlatOwner.Where(u => u.Id == id).FirstOrDefaultAsync();

            if (obj == null)
            {
                return NotFound();
            }

            _db.FlatOwner.Remove(obj);
            await _db.SaveChangesAsync();
            TempData["success"] = "Flat Owner Update SuccessFully";
            return RedirectToAction(nameof(Index));


        }


    }
}
