using CRUD;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SnailMS.Models;
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
        public IActionResult GetUsers(string name, string sortType, string filtType)// !access check
        {
            logger.LogInformation($"/GetUsers -> name:{name}");
            List<UserDto> userDtos = service.Users.GetAllUserDto().ToList();
            if (!string.IsNullOrEmpty(name))
            {
                userDtos = userDtos.Where(x => x.FirstName.Contains(name) || x.SecondName.Contains(name) || x.LastName.Contains(name)).ToList();
            }
            return PartialView(userDtos);
        }

    }
}
