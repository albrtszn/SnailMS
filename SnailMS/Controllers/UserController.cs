using CRUD;
using DataBase.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Microsoft.VisualBasic.FileIO;
using SnailMS.Models;
using System.Linq;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
        public IActionResult GetUsers(string name, string number, string sortType, string filtType)// !access check and self find and lowcase
        {
            logger.LogInformation($"/User/GetUsers -> name:{name}, sort:{sortType}, filt:{filtType}");
            string userId = HttpContext.User.Claims.ToList().Find(x => x.Type.Equals(ClaimTypes.NameIdentifier)).Value;
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                ViewBag.userId = userId;
                ViewBag.role = HttpContext.User.Claims.ToList().Find(x => x.Type.Equals(ClaimTypes.Role)).Value;
            }
            if (service.Users.GetUserDtoById(userId).Status.Equals(Status.banned.ToString()))
            {
                return PartialView("Пополните баланс для использования поиска");
            }

            List<UserDto> userDtos = service.Users.GetAllUserDto().Where(x => !x.Id.Equals(userId) && !x.Access.Equals(Access.@private.ToString()) && !x.Status.Equals(Status.banned.ToString()) ).ToList();
            //  query
            if (!string.IsNullOrEmpty(name))
            {
                userDtos = userDtos.Where(x => (x.FirstName.ToLower().Contains(name.ToLower()) || x.SecondName.ToLower().Contains(name.ToLower()) || x.LastName.ToLower().Contains(name.ToLower()) )).ToList();
            }
            if (!string.IsNullOrEmpty(number))
            {
                userDtos = userDtos.Where(x => (x.Number.Contains(number))).ToList();
            }
            //  filt
            if (!string.IsNullOrEmpty(filtType))
            {
                switch (filtType)
                {
                    case "rus":
                        userDtos = userDtos.Where(x => (x.Number.Contains("+7"))).ToList();
                        break;
                    case "by":
                        userDtos = userDtos.Where(x => (x.Number.Contains("+375"))).ToList();
                        break;
                    case "mts":
                        userDtos = userDtos.Where(x => (x.Number.Contains("+37529"))).ToList();
                        break;
                    case "life":
                        userDtos = userDtos.Where(x => (x.Number.Contains("+37525"))).ToList();
                        break;
                }
            }
            //  sort
            if (!string.IsNullOrEmpty(sortType))
            {
                switch (sortType)
                {
                    case "up":
                        userDtos = userDtos.OrderBy(x => x.FirstName).ToList();
                        break;
                    case "down":
                        userDtos = userDtos.OrderByDescending(x => x.FirstName).ToList();
                        break;
                }
            }
            return PartialView(userDtos);
        }
        [HttpGet("/User/Calls")]
        public IActionResult GetCalls(string fromDate, string toDate)
        {
            logger.LogInformation($"/User/GetCalls -> fromDate:{fromDate}, toDate:{toDate}");
            string userId = HttpContext.User.Claims.ToList().Find(x => x.Type.Equals(ClaimTypes.NameIdentifier)).Value;
            UserDto currentUserDto = service.Users.GetUserDtoById(userId);

            List<CallDto> callDtos = service.Calls.GetAllCallDto().ToList().Where(x => x.UserId.Equals(userId) || x.Number.Equals(currentUserDto.Number)).ToList();
            if (!string.IsNullOrEmpty(fromDate))
            {
                DateTime from = DateTime.Parse(fromDate);
                callDtos = callDtos.Where(x => x.StartTime.Date >= from).ToList();
            }
            if (!string.IsNullOrEmpty(toDate))
            {
                DateTime to = DateTime.Parse(toDate);
                callDtos = callDtos.Where(x => x.EndTime.Date <= to).ToList();
            }

            foreach (var callDto in callDtos)
            {
                if (callDto.Number.Equals(currentUserDto.Number))
                {
                    callDto.Number = service.Users.GetUserDtoById(callDto.UserId).Number;
                }
            }

            return PartialView(callDtos);
        }
        [HttpPost("/User/Deposit")]
        public IActionResult DepositBalance(string cardId, string value)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                string userId = HttpContext.User.Claims.ToList().Find(x => x.Type.Equals(ClaimTypes.NameIdentifier)).Value;
                ViewBag.userId = userId;
                ViewBag.role = HttpContext.User.Claims.ToList().Find(x => x.Type.Equals(ClaimTypes.Role)).Value;
            }
            if (!string.IsNullOrEmpty(cardId) && !string.IsNullOrEmpty(value))
            {
                logger.LogInformation($"/User/DepositBalance -> cardId:{cardId}, value:{decimal.Parse(value).ToString()}");
                var userDto = service.Users.GetUserDtoById(HttpContext.User.Claims.ToList().Find(x => x.Type.Equals(ClaimTypes.NameIdentifier)).Value);
                userDto.Balance = userDto.Balance + decimal.Parse(value);
                service.Users.SaveUserDto(userDto);
                return Content($"Баланс пополнен на {value} BYN");
            }
            return Content("Не удалось пополнить баланс");
        }
    }
}
