using CRUD;
using DataBase.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SnailMS.Models;
using System.Security.Claims;
using System.Xml.Serialization;

namespace SnailMS.Controllers
{
    [Authorize(Roles = "manager")]
    public class ManagerController : Controller
    {
        private ILogger<ManagerController> logger;
        private Data data;
        private Service.Service service;

        public ManagerController(ILogger<ManagerController> _logger, Data _data,
                                 Service.Service _service)
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
         *      TempCall
         */
        [HttpGet("/Manager/TempCall")]
        public IActionResult TempCall()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                ViewBag.userId = HttpContext.User.Claims.ToList().Find(x => x.Type.Equals(ClaimTypes.NameIdentifier)).Value;
                ViewBag.role = HttpContext.User.Claims.ToList().Find(x => x.Type.Equals(ClaimTypes.Role)).Value;
            }
            return View(service.TempCalls.GetAllTempCallDto());
        }
        [HttpPost("/Manager/TempCall/Delete")]
        public IActionResult DeleteTempCall(string callDtoId) // check on possible
        {
            logger.LogInformation($"/TempCall/Delete -> {callDtoId}");
            if (!string.IsNullOrEmpty(callDtoId))
            {
                service.TempCalls.DeleteTempCallById(callDtoId);
            }
            return Content("Удаление успешно");//View(service.TempCalls.GetTempCallDtoById(callDtoId));
        }
        [HttpPost("/Manager/TempCall/Add")]
        public IActionResult AddCall(CallDto callDtoToSave) // check on possible and refresh in js
        {
            logger.LogInformation($"/TempCall/Add -> {callDtoToSave.Id}, {callDtoToSave.UserId}, {callDtoToSave.Number}, {callDtoToSave.StartTime}, {callDtoToSave.EndTime}, {callDtoToSave.ManagerId}");
            if (callDtoToSave!=null) {
                service.TempCalls.DeleteTempCallById(callDtoToSave.Id);
                service.Calls.SaveCallDto(callDtoToSave);
            }
            return Content("Запись добавлена");
        }

    }
}
