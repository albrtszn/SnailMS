using CRUD;
using DataBase.Entity;
using DataBase.Enum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic.FileIO;
using SnailMS.Models;
using System.Security.AccessControl;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Security.Claims;

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
        public IActionResult Users()
        {
            return View(service.Users.GetAllUserDto());
        }
        [HttpGet("/Admin/GetUsers")]
        public IActionResult GetUsers(string fio, string number, string sortType, string filtType)
        {
            logger.LogInformation($"/Admin/GetUsers -> fio:{fio}, number:{number}, sort:{sortType}, filt:{filtType}");
            List<UserDto> userDtos = service.Users.GetAllUserDto().ToList();
            //  query
            if (!string.IsNullOrEmpty(fio))
            {
                userDtos = userDtos.Where(x => (x.FirstName.ToLower().Contains(fio.ToLower()) || x.SecondName.ToLower().Contains(fio.ToLower()) || x.LastName.ToLower().Contains(fio.ToLower()))).ToList();
            }
            if (!string.IsNullOrEmpty(number))
            {
                userDtos = userDtos.Where(x => (x.Number.Contains(number))).ToList();
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
                    case "balance-":
                        userDtos = userDtos.Where(x => x.Balance < 0).ToList();
                        break;
                    case "balance+":
                        userDtos = userDtos.Where(x => x.Balance >= 0).ToList();
                        break;
                }
            }
            return PartialView(userDtos);
        }
        [HttpPost("/Admin/User/Edit")]
        public IActionResult EditUser(UserDto editUserDto)
        {
            logger.LogInformation($"/Admin/User/EditUser -> id:{editUserDto.Id}, firstName:{editUserDto.FirstName} " +
                                  $"access:{editUserDto.Access}, status:{editUserDto.Status}");
            /*if (HttpContext.Request.Form.Files.Count != 0)
            {
                IFormFileCollection files = HttpContext.Request.Form.Files;
                editUserDto.Picture = service.Users.IFormFileToByteArray(files[0]);
                logger.LogInformation($"register picture-> {service.Users.IFormFileToByteArray(files[0])}");
            }
            else
            {
                var userDto = service.Users.GetUserDtoById(editUserDto.Id);
                if (userDto.Picture!=null)
                {
                    editUserDto.Picture = userDto.Picture;
                }
            }*/
            editUserDto.Picture = service.Users.GetUserDtoById(editUserDto.Id).Picture;
            service.Users.DeleteUserById(editUserDto.Id);
            service.Users.SaveUserDto(editUserDto);
            /*data.UserRoles.SaveUserRole(new UserRole
            {
                UserId = editUserDto.Id,
                RoleName = "user"
            });*/
            return Content("Данные отредактированы");
        }
        [HttpPost("/Admin/User/Add")]
        public IActionResult AddUser(UserDto userDtoToSave)
        {
            logger.LogInformation($"/Admin/User/EditUser -> id:{userDtoToSave.Id}, firstName:{userDtoToSave.FirstName} " +
                                  $"access:{userDtoToSave.Access}, status:{userDtoToSave.Status}");
            if (!string.IsNullOrEmpty(userDtoToSave.FirstName) && !string.IsNullOrEmpty(userDtoToSave.SecondName) &&
                !string.IsNullOrEmpty(userDtoToSave.LastName) && !string.IsNullOrEmpty(userDtoToSave.Number) &&
                !string.IsNullOrEmpty(userDtoToSave.Password) && !string.IsNullOrEmpty(userDtoToSave.Status) &&
                !string.IsNullOrEmpty(userDtoToSave.Access) && !string.IsNullOrEmpty(userDtoToSave.Adress) )
            {
                userDtoToSave.EntryDate = DateTime.Now;
                //userDtoToSave.Balance = 0m;
                userDtoToSave.Id = Guid.NewGuid().ToString();
                service.Users.SaveUserDto(userDtoToSave);
                return Content("Пользователь добавлен");
            }
            return Content("Ошибка->Пользователь не добавлен");
        }
        [HttpPost("/Admin/User/DeleteUser")]
        public IActionResult DeleteUser(string userId)
        {
            logger.LogInformation($"/Admin/User/DleteUser-> userId:{userId}");
            //data.Users.DeleteUserById(userId);
            service.Users.DeleteUserById(userId);
            return Content("Пользователь удален");
        }
        /*
         *      Manager
         */
        [HttpGet("/Admin/Manager")]
        public IActionResult Managers()
        {
            return View(service.Managers.GetAllManagerDto());
        }
        [HttpPost("/Admin/Manager/Edit")]
        public IActionResult EditManager(ManagerDto editManagerDto)
        {
            logger.LogInformation($"/Admin/Manager/Edit -> {editManagerDto.Id},{editManagerDto.Login},{editManagerDto.FirstName}" +
                                                        $",{editManagerDto.SecondName},{editManagerDto.DepartmentName}");
            if (!ModelState.IsValid)
            {
                return Content("Model is not valid");
            }
            return StatusCode(200);
        }
        [HttpPost("/Admin/Manager/Delete")]
        public IActionResult DeleteManager(string managerId)
        {
            logger.LogInformation($"/Admin/Manager/Delete -> {managerId}");
            return Content("successful delete");
        }
        /*
         *      Admin
         */
        [HttpGet("/Admin/Admin")]
        public IActionResult Admins()
        {
            return View(service.Admins.GetAllAdminDto());
        }
        [HttpPost("/Admin/Admin/Edit")]
        public IActionResult EditAdmin(AdminDto editAdminDto)
        {
            logger.LogInformation($"/Admin/Admin/Edit -> {editAdminDto.Id},{editAdminDto.Login},{editAdminDto.FirstName}" +
                                                        $",{editAdminDto.SecondName},{editAdminDto.DepartmentName}");
            if (!ModelState.IsValid)
            {
                return Content("Model is not valid");
            }
            return StatusCode(200);
        }
        [HttpPost("/Admin/Admin/Delete")]
        public IActionResult DeleteAdmin(string adminId)
        {
            logger.LogInformation($"/Admin/Admin/Edit -> {adminId}");
            return Content("successful delete");
        }
        /*
         *      Notification
         */
        [HttpGet("/Admin/Notification")]
        public IActionResult Notification()
        {
            return View();
        }
        [HttpPost("/Admin/User/SendNotifications")]
        public IActionResult SendNotifications()
        {
            logger.LogInformation($"/Admin/User/SendNotifications");
            int countOfUsers = 0;
            var userDtos = service.Users.GetAllUserDto().ToList();
            foreach (var userDto in userDtos)
            {
                if (userDto.Balance <= 0)
                {
                    if (service.Notifications.GetAllNotificationDto().ToList().Count(x => x.UserId.Equals(userDto.Id)) >= 3 && !userDto.Status.Equals(Status.banned))
                    {
                        var userToBlock = service.Users.GetUserDtoById(userDto.Id);
                        userToBlock.Status = Status.banned.ToString();
                        service.Users.DeleteUserById(userDto.Id);
                        service.Users.SaveUserDto(userToBlock);
                    }
                    else
                    {
                        service.Notifications.SaveNotificationDto(new NotificationDto
                        {
                            UserId = userDto.Id,
                            Message = $"Пополните баланс. {DateTime.Now.ToString()}",
                        });
                        countOfUsers++;
                    }
                }
            }
            if (countOfUsers > 0)
            {
                return Content("Уведомления отправлены " + countOfUsers + "пользователям(лю)");
            }
            else
            {
                return Content("Уведомления не отправлены, т.к. нет должников");
            }
        }
        [HttpGet("/Admin/GetNotifications")]
        public IActionResult GetNotifications()
        {
            return PartialView(service.Notifications.GetAllNotificationDto());
        }
        [HttpPost("/Admin/Notification/Delete")]
        public IActionResult DeleteNotification(int noteId)
        {
            logger.LogInformation($"/Admin/Notification/Delete -> {noteId}");
            service.Notifications.DeleteNotificationById(noteId);
            return Content("Уведомление удалено");
        }
        [HttpPost("/Admin/Notification/Add")]
        public IActionResult AddNotification(NotificationDto noteDto)
        {
            logger.LogInformation($"/Admin/Notifications/Add -> {noteDto.Id}, {noteDto.UserId}, {noteDto.Message}");
            if (noteDto != null)
            {
                service.Notifications.SaveNotificationDto(noteDto);
                return Content("Уведомление отправлено");
            }
            return Content("Ошибка при отправки уведомления");
        }
        /*
         *      Call
         */
        [HttpGet("/Admin/Call")]
        public IActionResult Call()
        {
            return View();
        }
        [HttpGet("/Admin/GetCalls")]
        public IActionResult GetCalls(string from, string to)
        {
            logger.LogInformation($"/Admin/GetCalls -> fromDate:{from}, toDate:{to}");
            string userId = HttpContext.User.Claims.ToList().Find(x => x.Type.Equals(ClaimTypes.NameIdentifier)).Value;
            AdminDto currentAdminDto = service.Admins.GetAdminDtoById(userId);

            List<CallDto> callDtos = service.Calls.GetAllCallDto().ToList();
            if (!string.IsNullOrEmpty(from))
            {
                DateTime fromDate = DateTime.Parse(from);
                callDtos = callDtos.Where(x => x.StartTime.Date >= fromDate).ToList();
            }
            if (!string.IsNullOrEmpty(to))
            {
                DateTime toDate = DateTime.Parse(to);
                callDtos = callDtos.Where(x => x.EndTime.Date <= toDate).ToList();
            }
            return PartialView(callDtos);
        }
    }
}
