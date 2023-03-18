using CRUD;
using DataBase;
using DataBase.Entity;
using DataBase.Enum;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json.Linq;
using SnailMS.Models;
using System;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json.Serialization;

namespace SnailMS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private Data data;
        private Service.Service service;

        public HomeController(ILogger<HomeController> _logger,
                              Data _data, Service.Service _service)
        {
            logger = _logger;
            data = _data;
            service = _service;
        }

        public IActionResult Index()
        {
            if (HttpContext.User.Identity.IsAuthenticated) {
                string role = HttpContext.User.Claims.ToList().Find(x => x.Type.Equals(ClaimTypes.Role)).Value;
                ViewBag.role = role;
                logger.LogInformation($"user role-> {role}");
            }
            return View();
        }

        [HttpGet("/Home/Login")]
        public IActionResult Login(string? returnUrl) // проверка на забаненного
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginModel loginModel, string returnUrl)
        {
            logger.LogInformation($"try of login into-> login:{loginModel.Login}, password:{loginModel.Password}");

            /*if (!ModelState.IsValid)
            {
                ModelState.AddModelError("error","Данные не верны");
                return Error();
            }*/

            var props = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTime.UtcNow.Add(TimeSpan.FromHours(1))
            };

            UserDto? userDto = service.Users.GetAllUserDto().ToList().FirstOrDefault(x => x.Number.Equals(loginModel.Login));
            ManagerDto? managerDto = service.Managers.GetAllManagerDto().ToList().FirstOrDefault(x => x.Login.Equals(loginModel.Login));
            AdminDto? adminDto = service.Admins.GetAllAdminDto().ToList().FirstOrDefault(x => x.Login.Equals(loginModel.Login));
            // user
            if (userDto != null && userDto.Password.Equals(loginModel.Password))
            {
                List<Claim> claims = new List<Claim> {
                    new Claim(ClaimTypes.NameIdentifier, userDto.Id),
                    new Claim(ClaimTypes.Role, Roles.user.ToString())
                };
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), props);

                logger.LogInformation($"success login-> login:{loginModel.Login}, password:{loginModel.Password}");
                return Redirect(returnUrl ?? "/");
            }
            // manager
            if (managerDto != null && managerDto.Password.Equals(loginModel.Password))
            {
                List<Claim> claims = new List<Claim> {
                    new Claim(ClaimTypes.NameIdentifier, managerDto.Id),
                    new Claim(ClaimTypes.Role, Roles.manager.ToString())
                };
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), props);

                logger.LogInformation($"success login-> login:{loginModel.Login}, password:{loginModel.Password}");
                return Redirect(returnUrl ?? "/");
            }
            // admin
            if (adminDto != null && adminDto.Password.Equals(loginModel.Password))
            {
                List<Claim> claims = new List<Claim> {
                    new Claim(ClaimTypes.NameIdentifier, adminDto.Id),
                    new Claim(ClaimTypes.Role, Roles.admin.ToString())
                };
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), props);

                logger.LogInformation($"success login-> login:{loginModel.Login}, password:{loginModel.Password}");
                return Redirect(returnUrl ?? "/");
            }

            //ModelState.AddModelError("error","Введенные данные не совпадают");
            TempData["error"] = "Введенные данные не совпадают";
            return View(loginModel);//Redirect("/");//Content("неправильная информация");
        }

        [HttpGet]
        public IActionResult Register() // проверка на существующего
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(UserDto userDto)
        {
            userDto.Id = Guid.NewGuid().ToString();
            userDto.Access = Access.@public.ToString();
            userDto.Status = Status.active.ToString();  
            userDto.EntryDate = DateTime.Now;
            userDto.Balance = 0.10m;

            service.Users.SaveUserDto(userDto);
            data.UserRoles.SaveUserRole(new UserRole() { 
                UserId = userDto.Id, 
                RoleName = Roles.user.ToString() }
            );

            var props = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.Add(TimeSpan.FromHours(1))
            };

            List<Claim> claims = new List<Claim> {
                    new Claim(ClaimTypes.NameIdentifier, userDto.Id),
                    new Claim(ClaimTypes.Role, Roles.user.ToString())
                };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");
            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), props);
            return Redirect("/");
        }

        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/");
        }

        [Authorize(Roles = "user")]
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