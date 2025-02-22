using AjaxCleaningHCM.Core.MasterData.Interface;
using AjaxCleaningHCM.Domain.DTO.MasterData.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using AjaxCleaningHCM.Domain.DTO.MasterData.Response;
using static AjaxCleaningHCM.Domain.Enums.Common;
using Microsoft.AspNetCore.Authorization;
using AjaxCleaningHCM.Domain.Models.MasterData;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;

namespace AjaxCleaningHCM.Web.Areas.AjaxCleaningHCM.Controllers
{
    [Authorize]
    [Area("AjaxCleaningHCM")]
    public class BankController : Controller
    {
        private readonly ILogger<BankController> _logger;
        private readonly IBank _Bank;
        public BankController(IBank Bank, ILogger<BankController> logger)
        {
            _Bank = Bank;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewData["ControllerName"] = "Bank";
            ViewData["ActionName"] = "Index";
            try
            {
                var Banks = await _Bank.GetAllAsync();

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
                return View("Index", Banks);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred while fetching all Banks.", ex);
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(long id)
        {
            ViewData["ControllerName"] = "Bank";
            try
            {
                var Bank = await _Bank.GetByIdAsync(id);

                if (Bank == null)
                {
                    return NotFound();
                }

                return View(Bank);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while fetching Bank with ID {id}.", ex);
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(long id)
        {
            ViewData["ControllerName"] = "Bank";
            try
            {
                var Bank = await _Bank.GetByIdAsync(id);

                if (Bank == null)
                {
                    return NotFound();
                }

                return PartialView(Bank.BankDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while fetching Bank with ID {id} for editing.", ex);
                return View("Error"); // You can customize the error view
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Bank request)
        {
            ViewData["ControllerName"] = "Bank";
            try
            {
                var result = await _Bank.UpdateAsync(request);
                if (result.Status == OperationStatus.SUCCESS)
                {
                    TempData["SuccessAlertMessage"] = "Bank successfully updated.";
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, result.Message);
                    TempData["FailureAlertMessage"] = "Error occurred while updating Bank.";
                    return View(request);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred while updating Bank.", ex);
                return View("Error");
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["ControllerName"] = "Bank";
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Bank request)
        {
            ViewData["ControllerName"] = "Bank";
            try
            {
                var result = await _Bank.CreateAsync(request);

                if (result.Status == OperationStatus.SUCCESS)
                {
                    TempData["SuccessAlertMessage"] = "Bank successfully inserted.";

                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, result.Message);
                    TempData["FailureAlertMessage"] = "Error occurred while creating Bank.";

                    return View(request);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred while creating Bank.", ex);
                return View("Error");
            }
        }


        [HttpPost]

        public async Task<IActionResult> Delete(long id)
        {
            ViewData["ControllerName"] = "Bank";
            try
            {
                var result = await _Bank.DeleteAsync(id);
                if (result.Status == OperationStatus.SUCCESS)
                {
                    return Json(new { success = true, message = "Bank successfully deleted." });
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
