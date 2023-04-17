using CRUD;
using DataBase.Entity;
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
        public IActionResult Users()
        {
            return View(service.Users.GetAllUserDto());
        }
        [HttpGet("/Admin/GetUsers")]
        public IActionResult GetUsers(string fio, string number, string sortType, string filtType)
        {
            return PartialView(service.Users.GetAllUserDto());
        }
        [HttpPost("/Admin/User/Edit")]
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
