using MailKit.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MimeKit;
using Newtonsoft.Json;
using Society_DataAccess.Data;
using Society_Models;
using Society_Models.ViewModels;
using Society_Utility;
using System.Security.Claims;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using Society_DataAccess.Services;


namespace Maintenance_Realation.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;
        public AdminController
            ( ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IWebHostEnvironment webHostEnvironment
,           IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
             IEmailSender emailsender
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _roleManager = roleManager;
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _emailSender = emailsender;
        }
       
        [HttpGet]
        public IActionResult Register()
        {
            RegisterVM RegisterVM = new RegisterVM()
            {

               RoleList = _roleManager.Roles.Select(x => x.Name).Select( i => new SelectListItem
               {
                   Text = i,
                   Value = i
               })
            };
            
            return View(RegisterVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterVM model , IFormFile file)
        {


            string wwwRootPath = _webHostEnvironment.WebRootPath;

            if(file != null)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string productPath = Path.Combine(wwwRootPath, @"images");

                using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                model.Image = @"images" + fileName;
            }



            var society = new Society
            {
                Image = model.Image,
                Name = model.societyName,
                Address = model.Address,
                PinCode = model.PinCode,
                State = model.State,
                City = model.City,
                LandMark = model.LandMark,
                BuiltYear = model.BuiltYear,
                RegistrationNo = model.RegistrationNo,
                CreatationDate = DateTime.Now.ToString("yyyy-MMMM")

        };

            await _context.Society.AddAsync(society);
            await _context.SaveChangesAsync();



            var user = new ApplicationUser
            {

                    UserName = model.Email,
                    Email = model.Email,
                    Name = model.Name,
                    SocietyId = society.Id
           
            };

             var result = await _userManager.CreateAsync(user , model.Password);
             
            if (result.Succeeded)
            {

                _httpContextAccessor.HttpContext?.Session.SetString("User", model.Name);

                if (!await _roleManager.RoleExistsAsync(SD.Role_Admin))
                {
                    await _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin));
                }

                await _userManager.AddToRoleAsync(user, SD.Role_Admin);
               
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                var callBackUrl = Url.Action("ConfirmEmail", "Admin", new
                {
                    userId = user.Id,
                    code

                },protocol: Request.Scheme);
                await _emailSender.SendEmailAsync(model.Email, "Confirm-Email Society-Maintenance",
                        $"Please confirm your email by clicking here: <a href='{callBackUrl}'>link</a>"
                    );
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Dashboard");
  
            }

         
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

            ModelState.Clear();
            return View(model);
        }

        [HttpGet]
        
        public async Task<IActionResult> ConfirmEmail(string code , string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if(user == null)
            {
                return View("Error");
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                return View();

            }
            return View();

        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure:true);
                if (result.Succeeded)
                {


                    _httpContextAccessor.HttpContext?.Session.SetString("User", model.Email);
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    if (user != null)
                    {
                        var roles = await _userManager.GetRolesAsync(user);
                        if (roles.Contains(SD.Role_Admin))
                        {
                            return RedirectToAction("Dashboard", "Admin");
                        }
                    }

                }
                if(result.IsLockedOut)
                {
                    return RedirectToAction("Lockout");
                }
                else
                {
                    
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }

            return View(model);
        }

        public IActionResult Lockout()
        {
            return View();
        }
        public async  Task<IActionResult> Logout()
       {
            await _signInManager.SignOutAsync();

            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            _httpContextAccessor.HttpContext?.Session.Clear();
            return RedirectToAction("Index", "Home");
            
           
       }

        public IActionResult DashBoard()
        {
            return View();
        }

        public IActionResult LoginUsers()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginUsers(LoginUsersVM model)
        {
            if(ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email!, model.Password!, model.RememberMe, false);
                if(result.Succeeded)
                {
                    return RedirectToAction("Index", "DashBoard");
                }
                ModelState.AddModelError("", "Invalid Login Attempt");
                return View(model);
                
            }
            return View(model);
        }

        public async Task<IActionResult> RegisterUsers()
        {
            ApplicationUser user = null;
            int societyId = 0;
            if (_httpContextAccessor.HttpContext?.User != null)
            {
                var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
                if(userId != null)
                {
                    user = await _context.ApplicationUser.FindAsync(userId);
                    societyId = user.SocietyId;
                }
            }
            RegisterUserVM registerUserVM = new()
            {
                FlatOwners = await _context.FlatOwner
                .Where(f => f.SocietyId == societyId)
                .Select(u => new SelectListItem
                {
                    Text = u.FirstName + " " + u.LastName,
                    Value = u.Id.ToString()
                }).ToListAsync(),

                Roles = await _context.Disignations
                .Select(u => new SelectListItem
                {
                    Text = u.DisignationName,
                    Value = u.DisignationName
                }).ToListAsync()
            };

            return View(registerUserVM);
        }

        [HttpPost]
        public async  Task<IActionResult> RegisterUsers(RegisterUserVM model)
        {
            if(ModelState.IsValid)
            {
                var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                int ownerId = int.Parse(model.Name); 
                FlatOwner owner = await _context.FlatOwner.FindAsync(ownerId);
                ApplicationUser userInfo = null;
                if(userId != null)
                {
                    userInfo = await _context.ApplicationUser.FindAsync(userId);
                }

                ApplicationUser user = new()
                {
                    Name = model.Name,
                    UserName = owner.Email,
                    Email = owner.Email,
                    SocietyId = userInfo.SocietyId
                    
                };

                var result =await _userManager.CreateAsync(user, user.Email + user.Id);

                if(result.Succeeded)
                {
                    List<Disignation> roles = await _context.Disignations.ToListAsync();



                    foreach (var RoleName in roles)
                    {
                        var ExitRole = await _roleManager.RoleExistsAsync(RoleName.DisignationName);

                        if(!ExitRole)
                        {
                            await _roleManager.CreateAsync(new IdentityRole(RoleName.DisignationName));
                        }
                    }


                    await _userManager.AddToRoleAsync(user, model.Role);
                    //await _signInManager.SignInAsync(user, false);
                    var htmlMessage = $"Subject: Welcome to {{SocietyName}} Society\r\n\r\nDear {{ReceipientName}},\r\n\r\nWe are delighted to inform you that your account has been successfully registered with the {{SocietyName}} society. Your designated role is {{RoleName}}, granting you authorized access to the {{SocietyName}} society account.\r\n\r\nBelow are the credentials required for accessing your account:\r\n\r\nUsername: {{Username}}\r\nPassword: {{Password}}\r\nPlease change your password using {{ChangePasswordLink}} link.\r\n\r\nWelcome aboard!\r\n\r\nWarm regards,\r\n{{AdminName}}\r\n{{SocietyName}} {{Designation}}\r\n{{ContactDetails}}";
                    await _emailSender.SendEmailAsync(user.Email,"Society-Mainteanance Credentials", htmlMessage);
                    TempData["success"] = "User created Successfully";
                    return RedirectToAction("Index", "DashBoard");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if(user  == null)
                {
                    return View("FogotPasswordConfirmation");
                }

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackurl = Url.Action("ResetPassword", "Admin", new
                {
                    userId = user.Id,
                    code
                },protocol:HttpContext.Request.Scheme);

                await _emailSender.SendEmailAsync(model.Email, "Reset Password - IDentity Manager",
                       $"Please reset your password by clicking here: <a href='{callbackurl}'>link </a>"
                   );

                return RedirectToAction("FogotPasswordConfirmation");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult FogotPasswordConfirmation()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ResetPassword(string code = null)
        {
            return code == null ? View("Error") : View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if(user == null)
            {
                return RedirectToAction("ResetPasswordConfirmation");
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if(result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation");
            }
            return View();
        }

        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }


    }
}
