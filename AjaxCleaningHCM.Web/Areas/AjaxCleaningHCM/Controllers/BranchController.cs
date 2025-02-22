using AjaxCleaningHCM.Core.MasterData.Interface;
using AjaxCleaningHCM.Domain.DTO.MasterData.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using static AjaxCleaningHCM.Domain.Enums.Common;
using Microsoft.AspNetCore.Authorization;
using AjaxCleaningHCM.Domain.Models.MasterData;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;

namespace AjaxCleaningHCM.Web.Areas.AjaxCleaningHCM.Controllers
{
    [Authorize]
    [Area("AjaxCleaningHCM")]
    public class BranchController : Controller
    {
        private readonly ILogger<BranchController> _logger;
        private readonly IBranch _Branch;
        public BranchController(IBranch Branch, ILogger<BranchController> logger)
        {
            _Branch = Branch;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewData["ControllerName"] = "Branch";
            ViewData["ActionName"] = "Index";
            try
            {
                var Branchs = await _Branch.GetAllAsync();

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
                return View("Index", Branchs);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred while fetching all Branchs.", ex);
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(long id)
        {
            ViewData["ControllerName"] = "Branch";
            try
            {
                var Branch = await _Branch.GetByIdAsync(id);

                if (Branch == null)
                {
                    return NotFound();
                }

                return View(Branch);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while fetching Branch with ID {id}.", ex);
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(long id)
        {
            ViewData["ControllerName"] = "Branch";
            try
            {
                var Branch = await _Branch.GetByIdAsync(id);

                if (Branch == null)
                {
                    return NotFound();
                }

                return PartialView(Branch.BranchDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while fetching Branch with ID {id} for editing.", ex);
                return View("Error"); // You can customize the error view
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Branch request)
        {
            ViewData["ControllerName"] = "Branch";
            try
            {
                var result = await _Branch.UpdateAsync(request);
                if (result.Status == OperationStatus.SUCCESS)
                {
                    TempData["SuccessAlertMessage"] = "Branch successfully updated.";
                    return RedirectToAction("Index");

                }

                else
                {
                    ModelState.AddModelError(string.Empty, result.Message);
                    TempData["FailureAlertMessage"] = result.Message;

                    return View(request);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred while updating Branch.", ex);
                return View("Error");
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["ControllerName"] = "Branch";
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Branch request)
        {
            ViewData["ControllerName"] = "Branch";
            try
            {
                var result = await _Branch.CreateAsync(request);

                if (result.Status == OperationStatus.SUCCESS)
                {
                    TempData["SuccessAlertMessage"] = "Branch successfully inserted.";

                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, result.Message);
                    TempData["FailureAlertMessage"] = result.Message;

                    return View(request);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred while creating Branch.", ex);
                return View("Error");
            }
        }
        
        [HttpPost]

        public async Task<IActionResult> Delete(long id)
        {
            ViewData["ControllerName"] = "Branch";
            try
            {
                var result = await _Branch.DeleteAsync(id);
                if (result.Status == OperationStatus.SUCCESS)
                {
                    return Json(new { success = true, message = "Branch successfully deleted." });
                }
                else
                {
                    return Json(new { success = false, message = "Error occurred while deleting branch." });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while deleting Branch with ID {id}.", ex);
                return Json(new { success = false, message = "An error occurred: " + ex.Message });
            }
        }
    }

}
