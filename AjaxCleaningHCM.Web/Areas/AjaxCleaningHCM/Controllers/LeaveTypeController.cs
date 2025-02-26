using AjaxCleaningHCM.Core.MasterData.Interface;
using AjaxCleaningHCM.Domain.Models.MasterData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;
using static AjaxCleaningHCM.Domain.Enums.Common;

namespace AjaxCleaningHCM.Web.Areas.AjaxCleaningHCM.Controllers
{
    [Authorize]
    [Area("AjaxCleaningHCM")]
    public class LeaveTypeController : Controller
    {
        private readonly ILogger<LeaveTypeController> _logger;
        private readonly ILeaveType _LeaveType;
        public LeaveTypeController(ILeaveType LeaveType, ILogger<LeaveTypeController> logger)
        {
            _LeaveType = LeaveType;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewData["ControllerName"] = "LeaveType";
            ViewData["ActionName"] = "Index";
            try
            {
                var LeaveTypes = await _LeaveType.GetAllAsync();

                if (TempData["SuccessAlertMessage"] != null)
                {
                    ViewBag.SuccessAlertMessage = TempData["SuccessAlertMessage"];
                    TempData["SuccessAlertMessage"] = null;
                }

                if (TempData["FailureAlertMessage"] != null)
                {
                    ViewBag.FailureAlertMessage = TempData["FailureAlertMessage"];
                    TempData["FailureAlertMessage"] = null;
                }
                return View("Index", LeaveTypes);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred while fetching all LeaveTypes.", ex);
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(long id)
        {
            ViewData["ControllerName"] = "LeaveType";
            try
            {
                var LeaveType = await _LeaveType.GetByIdAsync(id);

                if (LeaveType == null)
                {
                    return NotFound();
                }

                return View(LeaveType);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while fetching LeaveType with ID {id}.", ex);
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(long id)
        {
            ViewData["ControllerName"] = "LeaveType";
            try
            {
                var LeaveType = await _LeaveType.GetByIdAsync(id);

                if (LeaveType == null)
                {
                    return NotFound();
                }

                return PartialView(LeaveType.LeaveTypeDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while fetching LeaveType with ID {id} for editing.", ex);
                return View("Error"); // You can customize the error view
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(LeaveType request)
        {
            ViewData["ControllerName"] = "LeaveType";
            try
            {
                var result = await _LeaveType.UpdateAsync(request);
                if (result.Status == OperationStatus.SUCCESS)
                {
                    TempData["SuccessAlertMessage"] = "LeaveType successfully updated.";
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, result.Message);
                    TempData["FailureAlertMessage"] = "Error occurred while updating LeaveType.";
                    return View(request);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred while updating LeaveType.", ex);
                return View("Error");
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["ControllerName"] = "LeaveType";
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LeaveType request)
        {
            ViewData["ControllerName"] = "LeaveType";
            try
            {
                var result = await _LeaveType.CreateAsync(request);

                if (result.Status == OperationStatus.SUCCESS)
                {
                    TempData["SuccessAlertMessage"] = "LeaveType successfully inserted.";

                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, result.Message);
                    TempData["FailureAlertMessage"] = "Error occurred while creating LeaveType.";

                    return View(request);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred while creating LeaveType.", ex);
                return View("Error");
            }
        }


        [HttpPost]

        public async Task<IActionResult> Delete(long id)
        {
            ViewData["ControllerName"] = "LeaveType";
            try
            {
                var result = await _LeaveType.DeleteAsync(id);
                if (result.Status == OperationStatus.SUCCESS)
                {
                    return Json(new { success = true, message = "LeaveType successfully deleted." });
                }
                else
                {
                    return Json(new { success = false, message = "Error occurred while deleting bank." });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while deleting bank with ID {id}.", ex);
                return Json(new { success = false, message = "An error occurred: " + ex.Message });
            }
        }
    }
}
