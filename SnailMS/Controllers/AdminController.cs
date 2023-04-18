using CRUD;
using DataBase.Entity;
using DataBase.Enum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic.FileIO;
using SnailMS.Models;
using System.Security.AccessControl;

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
                        userDtos.ForEach(x => Console.WriteLine(x.FirstName));
                        break;
                    case "down":
                        userDtos = userDtos.OrderByDescending(x => x.FirstName).ToList();
                        userDtos.ForEach(x => Console.WriteLine(x.FirstName));
                        break;
                }
            }
            //  filt
            if (!string.IsNullOrEmpty(filtType))
            {
                switch (filtType)
                {
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
            service.Users.DeleteUserById(editUserDto.Id);
            service.Users.SaveUserDto(editUserDto);
            data.UserRoles.SaveUserRole(new UserRole
            {
                UserId = editUserDto.Id,
                RoleName = "user"
            });
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
                            Message = "Пополните баланс для избежании блокировки."
                        });
                        countOfUsers++;
                    }
                }
            }
            if (countOfUsers > 0)
            {
                return Content("Уведомления отправлены " + countOfUsers + "пользователям(лю)");
            } else {
                return Content("Уведомления не отправлены, т.к. нет должников");
            }
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
        public IActionResult Notifications()
        {
            return View(service.Notifications.GetAllNotificationDto());
        }
        [HttpPost("/Admin/Notification/Edit")]
        public IActionResult EditNotification(NotificationDto editNoteDto)
        {
            logger.LogInformation($"/Admin/Notification/Edit -> {editNoteDto.Id}, {editNoteDto.UserId}, {editNoteDto.Message}");
            return Content("");
        }
        [HttpPost("/Admin/Notification/Delete")]
        public IActionResult DeleteNotification(int id)
        {
            logger.LogInformation($"/Admin/Notification/Delete -> {id}");
            return Content("");
        }
        [HttpPost("/Admin/Notification/Add")]
        public IActionResult AddNotification(NotificationDto noteDto)
        {
            logger.LogInformation($"/Admin/Notifications/Add -> {noteDto.Id}, {noteDto.UserId}, {noteDto.Message}");
            return Content("");
        }
    }
}
