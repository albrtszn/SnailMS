using CRUD;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SnailMS.Models;
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
            return View(service.TempCalls.GetAllTempCallDto());
        }
        [HttpGet("/Manager/TempCall/Edit/{id}")]
        public IActionResult EditTempCall(string tempCallId)
        {
            return View(service.TempCalls.GetTempCallDtoById(tempCallId));
        }
        [HttpPost("/Manager/TempCall/Edit/{id}")]
        public IActionResult EditTempCall(TempCallDto editTempCallDto)
        {
            service.TempCalls.SaveTempCallDto(editTempCallDto);
            return Redirect("/Manager/TempCall");
        }
        [HttpGet("/Manager/TempCall/Add/{id}")]
        public IActionResult AddCall(string tempCallId)
        {
            return View(service.TempCalls.GetTempCallDtoById(tempCallId));
        }
        [HttpPost("/Manager/TempCall/Add/{id}")]
        public IActionResult AddCall(CallDto callDtoToSave)
        {
            service.TempCalls.DeleteTempCallById(callDtoToSave.Id);
            service.Calls.SaveCallDto(callDtoToSave);
            return Redirect("/Manager/TempCall");
        }

        /*
         *      Call
         */
        [HttpGet("/Manager/Call")]
        public IActionResult Call()
        {
            return View(service.Calls.GetAllCallDto());
        }
        [HttpGet("/Manager/Call/Edit/{id}")]
        public IActionResult EditCall(string callId)
        {
            return View(service.Calls.GetCallDtoById(callId));
        }
        [HttpPost("/Manager/Call/Edit/{id}")]
        public IActionResult EditCall(CallDto editCallDto)
        {
            service.Calls.SaveCallDto(editCallDto);
            return Redirect("/Manager/Call");
        }
    }
}
