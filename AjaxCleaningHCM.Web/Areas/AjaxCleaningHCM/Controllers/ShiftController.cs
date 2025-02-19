using AjaxCleaningHCM.Core.MasterData.Interface;
using AjaxCleaningHCM.Domain.DTO.MasterData.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using static AjaxCleaningHCM.Domain.Enums.Common;
using Microsoft.AspNetCore.Authorization;

namespace AjaxCleaningHCM.Web.Areas.AjaxCleaningHCM.Controllers
{
    [Authorize]
    [Area("AjaxCleaningHCM")]
    public class ShiftController : Controller
    {
        private readonly ILogger<ShiftController> _logger;
        private readonly IShift _Shift;
        public ShiftController(IShift Shift, ILogger<ShiftController> logger)
        {
            _Shift = Shift;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewData["ControllerName"] = "Shift";
            ViewData["ActionName"] = "Index";
            try
            {
                var Shifts = await _Shift.GetAllAsync();

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
                return View("Index", Shifts);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred while fetching all Shifts.", ex);
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(long id)
        {
            ViewData["ControllerName"] = "Shift";
            try
            {
                var Shift = await _Shift.GetByIdAsync(id);

                if (Shift == null)
                {
                    return NotFound();
                }

                return View(Shift);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while fetching Shift with ID {id}.", ex);
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(long id)
        {
            ViewData["ControllerName"] = "Shift";
            try
            {
                var Shift = await _Shift.GetByIdAsync(id);

                if (Shift == null)
                {
                    return NotFound();
                }

                return PartialView(Shift.ShiftDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while fetching Shift with ID {id} for editing.", ex);
                return View("Error"); // You can customize the error view
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ShiftDto request)
        {
            ViewData["ControllerName"] = "Shift";
            try
            {
                var result = await _Shift.UpdateAsync(request);
                if (result.Status == OperationStatus.SUCCESS)
                {
                    TempData["SuccessAlertMessage"] = "Shift successfully updated.";
                    return RedirectToAction("Index");

                }

                else
                {
                    ModelState.AddModelError(string.Empty, "Error occurred while updating Shift.");
                    TempData["FailureAlertMessage"] = "Error occurred while updating Shift.";

                    return View(request);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred while updating Shift.", ex);
                return View("Error");
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["ControllerName"] = "Shift";
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ShiftDto request)
        {
            ViewData["ControllerName"] = "Shift";
            try
            {
                var result = await _Shift.CreateAsync(request);

                if (result.Status == OperationStatus.SUCCESS)
                {
                    TempData["SuccessAlertMessage"] = "Shift successfully inserted.";

                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error occurred while creating Shift.");
                    TempData["FailureAlertMessage"] = "Error occurred while creating Shift.";

                    return View(request);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred while creating Shift.", ex);
                return View("Error");
            }
        }


        [HttpPost]

        public async Task<IActionResult> Delete(long id)
        {
            ViewData["ControllerName"] = "Shift";
            try
            {
                var result = await _Shift.DeleteAsync(id);
                if (result.Status == OperationStatus.SUCCESS)
                {
                    return Json(new { success = true, message = "Shift successfully deleted." });
                }
                else
                {
                    return Json(new { success = false, message = "Error occurred while deleting Shift." });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while deleting Shift with ID {id}.", ex);
                return Json(new { success = false, message = "An error occurred: " + ex.Message });
            }
        }
    }
}
