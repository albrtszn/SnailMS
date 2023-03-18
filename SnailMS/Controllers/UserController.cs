using CRUD;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace SnailMS.Controllers
{
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
            return View(service.Users.GetUserDtoById(HttpContext.User.Claims.ToList().Find(x => x.Type.Equals(ClaimTypes.NameIdentifier)).Value));
        }

    }
}
