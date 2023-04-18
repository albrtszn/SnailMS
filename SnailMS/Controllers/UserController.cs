using CRUD;
using DataBase.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using SnailMS.Models;
using System.Linq;
using System.Security.Claims;

namespace SnailMS.Controllers
{
    [Authorize(Roles = "user")]
    public class UserController : Controller
    {
        private ILogger<UserController> logger;
        private Data data;
        private Service.Service service;
        public UserController(ILogger<UserController> _logger, Data _data, Service.Service _service)
        {
            logger = _logger;
            data = _data;
            service = _service;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("/User/Account")]
        public IActionResult Account()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                ViewBag.userId = HttpContext.User.Claims.ToList().Find(x => x.Type.Equals(ClaimTypes.NameIdentifier)).Value;
                ViewBag.role = HttpContext.User.Claims.ToList().Find(x => x.Type.Equals(ClaimTypes.Role)).Value;
            }
            return View(service.Users.GetUserDtoById(HttpContext.User.Claims.ToList().Find(x => x.Type.Equals(ClaimTypes.NameIdentifier)).Value));
        }
        [HttpGet("/User/Users")]
        public IActionResult SearchUser()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                ViewBag.userId = HttpContext.User.Claims.ToList().Find(x => x.Type.Equals(ClaimTypes.NameIdentifier)).Value;
                ViewBag.role = HttpContext.User.Claims.ToList().Find(x => x.Type.Equals(ClaimTypes.Role)).Value;
            }
            return View();
        }
        [HttpGet]
        public IActionResult GetUsers(string name, string sort, string filt)// !access check and self find and lowcase
        {
            logger.LogInformation($"/GetUsers -> name:{name}, sort:{sort}, filt:{filt}");
            string userId = HttpContext.User.Claims.ToList().Find(x => x.Type.Equals(ClaimTypes.NameIdentifier)).Value;
            List<UserDto> userDtos = service.Users.GetAllUserDto().Where(x => !x.Id.Equals(userId) && !x.Access.Equals(Access.@private.ToString()) ).ToList();
            if (!string.IsNullOrEmpty(name))
            {
                userDtos = userDtos.Where(x => (x.FirstName.ToLower().Contains(name.ToLower()) || x.SecondName.ToLower().Contains(name.ToLower()) || x.LastName.ToLower().Contains(name.ToLower()) )).ToList();
            }
            return PartialView(userDtos);
        }
        [HttpGet("/User/Calls")]
        public IActionResult GetCalls(string fromDate, string toDate)
        {
            return PartialView(service.Calls.GetAllCallDto());
        }
    }
}
