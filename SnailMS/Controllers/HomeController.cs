using CRUD;
using DataBase;
using Microsoft.AspNetCore.Mvc;
using SnailMS.Models;
using System.Diagnostics;

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
            return View();
        }

        [HttpGet("/Home/login")]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel userDto, string returnUrl)
        {
            logger.LogInformation($"try of login into-> login:{userDto.Number}, password:{userDto.Password}");

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("error","Данные не верны");
                //return Us();
            }

            return Content("неправильная информация");
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