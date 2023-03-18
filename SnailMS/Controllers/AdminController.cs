using CRUD;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SnailMS.Models;

namespace SnailMS.Controllers
{
    public class AdminController : Controller
    {
        private ILogger<AdminController> logger;
        private Data data;
        private Service.Service service;
        public AdminController(ILogger<AdminController> _logger, Data _data, Service.Service _service)
        {
            logger = _logger;
            data = _data;
            service = _service;
        }
        public IActionResult Index()
        {
            return View();
        }

        /*
         *      User
         */
        [HttpGet("/Admin/User")]
        public IActionResult User()
        {
            return View(service.Users.GetAllUserDto());
        }
        [HttpGet("/Admin/User/Edit/{id}")]
        public IActionResult EditUser(string userId)
        {
            return View(service.Users.GetUserDtoById(userId));
        }
        [HttpPost("/Admin/User/Edit/{id}")]
        public IActionResult EditUser(UserDto editUserDto)
        {
            return Redirect("/Admin/User");
        }
        [HttpGet("/Admin/User/Add")]
        public IActionResult AddUser()
        {
            return View();
        }
        [HttpPost("/Admin/User/Add")]
        public IActionResult AddUser(UserDto userDtoToSave)
        {
            service.Users.SaveUserDto(userDtoToSave);
            return Redirect("/Admin/User");
        }
    }
}
