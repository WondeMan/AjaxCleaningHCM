using AjaxCleaningHCM.Core.MasterData.Interface;
using AjaxCleaningHCM.Domain.Models.MasterData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;
using static AjaxCleaningHCM.Domain.Enums.Common;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace AjaxCleaningHCM.Web.Areas.AjaxCleaningHCM.Controllers
{
    [Authorize]
    [Area("AjaxCleaningHCM")]
    public class IssueRegistrationController : Controller
    {
        private readonly ILogger<IssueRegistrationController> _logger;
        private readonly IIssueRegistration _IssueRegistration;
        private readonly IBranch _branch;

        public IssueRegistrationController(IIssueRegistration IssueRegistration, IBranch branch, ILogger<IssueRegistrationController> logger)
        {
            _IssueRegistration = IssueRegistration;
            _logger = logger;
            _branch = branch;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewData["ControllerName"] = "IssueRegistration";
            ViewData["ActionName"] = "Index";
            try
            {
                var IssueRegistrations = await _IssueRegistration.GetAllAsync();

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
                return View("Index", IssueRegistrations);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred while fetching all IssueRegistrations.", ex);
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(long id)
        {
            ViewData["ControllerName"] = "IssueRegistration";
            try
            {
                var IssueRegistration = await _IssueRegistration.GetByIdAsync(id);

                if (IssueRegistration == null)
                {
                    return NotFound();
                }

                return View(IssueRegistration);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while fetching IssueRegistration with ID {id}.", ex);
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(long id)
        {
            ViewData["ControllerName"] = "IssueRegistration";
            try
            {
                var IssueRegistration = await _IssueRegistration.GetByIdAsync(id);
                var branchs = await _branch.GetAllAsync();
                var branchList = new SelectList(branchs.BranchDtos, "Id", "Name").ToList();
                branchList.Insert(0, new SelectListItem { Text = "Please select", Value = "" });
                ViewBag.Branchs = new SelectList(branchList, "Value", "Text", null);

                if (IssueRegistration == null)
                {
                    return NotFound();
                }

                return PartialView(IssueRegistration.IssueRegistrationDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while fetching IssueRegistration with ID {id} for editing.", ex);
                return View("Error"); // You can customize the error view
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(IssueRegistration request)
        {
            ViewData["ControllerName"] = "IssueRegistration";
            try
            {
                var result = await _IssueRegistration.UpdateAsync(request);
                if (result.Status == OperationStatus.SUCCESS)
                {
                    TempData["SuccessAlertMessage"] = "IssueRegistration successfully updated.";
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, result.Message);
                    TempData["FailureAlertMessage"] = "Error occurred while updating IssueRegistration.";
                    return View(request);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred while updating IssueRegistration.", ex);
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["ControllerName"] = "IssueRegistration";
            var branchs = await _branch.GetAllAsync();
            var branchList = new SelectList(branchs.BranchDtos, "Id", "Name").ToList();
            branchList.Insert(0, new SelectListItem { Text = "Please select", Value = "" });
            ViewBag.Branchs = new SelectList(branchList, "Value", "Text", null);

            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IssueRegistration request)
        {
            ViewData["ControllerName"] = "IssueRegistration";
            try
            {
                var result = await _IssueRegistration.CreateAsync(request);

                if (result.Status == OperationStatus.SUCCESS)
                {
                    TempData["SuccessAlertMessage"] = "IssueRegistration successfully inserted.";

                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, result.Message);
                    TempData["FailureAlertMessage"] = "Error occurred while creating IssueRegistration.";

                    return View(request);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred while creating IssueRegistration.", ex);
                return View("Error");
            }
        }


        [HttpPost]

        public async Task<IActionResult> Delete(long id)
        {
            ViewData["ControllerName"] = "IssueRegistration";
            try
            {
                var result = await _IssueRegistration.DeleteAsync(id);
                if (result.Status == OperationStatus.SUCCESS)
                {
                    return Json(new { success = true, message = "IssueRegistration successfully deleted." });
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
        [HttpPost]
        public async Task<ActionResult> IssueProcessing(IssueRegistration IssueRegistration)
        {
            try
            {
                ViewData["ControllerName"] = "IssueRegistration";

                if (ModelState.IsValid)
                {
                    IssueRegistration.ActionTakenBy = User.Identity.Name;
                    var result =await _IssueRegistration.IssueProcessing(IssueRegistration);
                    if (result.Status == OperationStatus.SUCCESS)
                    {
                        TempData["SuccessAlertMessage"] = "Record has been successfully updated.";
                        return RedirectToAction("Index");
                    }
                }
                return View(IssueRegistration);
            }
            catch
            {
                return View();
            }
        }
        // GET: AttendanceController1/Delete/5

        // GET: AttendanceController1/Edit/5
        public async Task<ActionResult> IssueProcessing(long id)
        {
            ViewData["ControllerName"] = "IssueRegistration";

            var IssueRegistration = await _IssueRegistration.GetByIdAsync(id);
            return View(IssueRegistration.IssueRegistrationDto);
          
        }

    }
}
