using AjaxCleaningHCM.Core.MasterData.Interface;
using AjaxCleaningHCM.Domain.DTO.MasterData.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using static AjaxCleaningHCM.Domain.Enums.Common;
using Microsoft.AspNetCore.Authorization;
using AjaxCleaningHCM.Domain.Models.MasterData;

namespace AjaxCleaningHCM.Web.Areas.AjaxCleaningHCM.Controllers
{
    [Authorize]
    [Area("AjaxCleaningHCM")]
    public class BranchController : Controller
    {
        private readonly ILogger<BranchController> _logger;
        private readonly IBranch _branch;
        public BranchController(IBranch branch, ILogger<BranchController> logger)
        {
            _branch = branch;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewData["ControllerName"] = "Branch";
            ViewData["ActionName"] = "Index";
            try
            {
                var branchs = await _branch.GetAllAsync();

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
                return View("Index", branchs);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred while fetching all branchs.", ex);
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(long id)
        {
            ViewData["ControllerName"] = "Branch";
            try
            {
                var branch = await _branch.GetByIdAsync(id);

                if (branch == null)
                {
                    return NotFound();
                }

                return View(branch);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while fetching branch with ID {id}.", ex);
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(long id)
        {
            ViewData["ControllerName"] = "Branch";
            try
            {
                var branch = await _branch.GetByIdAsync(id);

                if (branch == null)
                {
                    return NotFound();
                }

                return PartialView(branch.BranchDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while fetching branch with ID {id} for editing.", ex);
                return View("Error"); // You can customize the error view
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BranchDto request)
        {
            ViewData["ControllerName"] = "Branch";
            try
            {
                var result = await _branch.UpdateAsync(request);
                if (result.Status == OperationStatus.SUCCESS)
                {
                    TempData["SuccessAlertMessage"] = "Branch successfully updated.";
                    return RedirectToAction("Index");

                }

                else
                {
                    ModelState.AddModelError(string.Empty, "Error occurred while updating branch.");
                    TempData["FailureAlertMessage"] = "Error occurred while updating branch.";

                    return View(request);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred while updating branch.", ex);
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
        public async Task<IActionResult> Create(BranchDto request)
        {
            ViewData["ControllerName"] = "Branch";
            try
            {
                var result = await _branch.CreateAsync(request);

                if (result.Status == OperationStatus.SUCCESS)
                {
                    TempData["SuccessAlertMessage"] = "Branch successfully inserted.";

                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error occurred while creating branch.");
                    TempData["FailureAlertMessage"] = "Error occurred while creating branch.";

                    return View(request);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred while creating branch.", ex);
                return View("Error");
            }
        }

        [HttpPost]

        public async Task<IActionResult> Delete(long id)
        {
            ViewData["ControllerName"] = "Branch";
            try
            {
                var result = await _branch.DeleteAsync(id);
                if (result.Status == OperationStatus.SUCCESS)
                {
                    return Json(new { success = true, message = "Branch successfully deleted." });
                }
                else
                {
                    return Json(new { success = false, message = "Error occurred while deleting Branch." });
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
