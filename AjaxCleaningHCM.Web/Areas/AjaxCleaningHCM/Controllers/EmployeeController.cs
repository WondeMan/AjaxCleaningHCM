using AjaxCleaningHCM.Core.MasterData.Interface;
using AjaxCleaningHCM.Domain.DTO.MasterData.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using static AjaxCleaningHCM.Domain.Enums.Common;
using Microsoft.AspNetCore.Authorization;
using AjaxCleaningHCM.Domain.Models.MasterData;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using AjaxCleaningHCM.Core.Utils;

namespace AjaxCleaningHCM.Web.Areas.AjaxCleaningHCM.Controllers
{
    [Authorize]
    [Area("AjaxCleaningHCM")]
    public class EmployeeController : Controller
    {
        private readonly ILogger<EmployeeController> _logger;
        private readonly IEmployee _Employee;
        private readonly IBranch _branch;
        private readonly IBank _bank;
        public EmployeeController(IEmployee Employee, IBranch branch, IBank bank, ILogger<EmployeeController> logger)
        {
            _Employee = Employee;
            _logger = logger;
            _branch = branch;
            _bank = bank;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewData["ControllerName"] = "Employee";
            ViewData["ActionName"] = "Index";
            try
            {
                var Employees = await _Employee.GetAllAsync();

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
                return View("Index", Employees);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred while fetching all Employees.", ex);
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(long id)
        {
            ViewData["ControllerName"] = "Employee";
            try
            {
                var Employee = await _Employee.GetByIdAsync(id);

                if (Employee == null)
                {
                    return NotFound();
                }

                return View(Employee);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while fetching Employee with ID {id}.", ex);
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(long id)
        {
            ViewData["ControllerName"] = "Employee";
            try
            {
                var Employee = await _Employee.GetByIdAsync(id);

                if (Employee == null)
                {
                    return NotFound();
                }
                var branchs = await _branch.GetAllAsync();
                var branchList = new SelectList(branchs.BranchDtos, "Id", "Name").ToList();
                branchList.Insert(0, new SelectListItem { Text = "Please select", Value = "" });
                ViewBag.Branch = new SelectList(branchList, "Value", "Text", null);
                ViewBag.SelectedBranch = string.Join(",", Employee.EmployeeDto.EmployeeBranches.Select(a => a.BranchId));

                var banks = await _bank.GetAllAsync();
                var bankList = new SelectList(banks.BankDtos, "Id", "Name").ToList();
                bankList.Insert(0, new SelectListItem { Text = "Please select", Value = "" });
                ViewBag.Bank = new SelectList(bankList, "Value", "Text", null);
                return PartialView(Employee.EmployeeDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while fetching Employee with ID {id} for editing.", ex);
                return View("Error"); // You can customize the error view
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Employee request)
        {
            ViewData["ControllerName"] = "Employee";
            try
            {
                request.CirtificationImg = request.CirtificationImg ?? FileUploader.GetFormFileFromPath(@request.CirtificationPath);
                request.EmployeeKebeleIDImg = request.EmployeeKebeleIDImg ?? FileUploader.GetFormFileFromPath(request.EmployeeKebeleIDPath);
                request.EmpoloyePhotoImg = request.EmpoloyePhotoImg ?? FileUploader.GetFormFileFromPath(request.EmpoloyePhotoPath);
                request.GuaranteeDocumentImg = request.GuaranteeDocumentImg ?? FileUploader.GetFormFileFromPath(request.GurrentDocumentPath);
                request.GurrentKebeleIDImg = request.GurrentKebeleIDImg ?? FileUploader.GetFormFileFromPath(request.GurrentKebeleIDPath);
                var result = await _Employee.UpdateAsync(request);
                if (result.Status == OperationStatus.SUCCESS)
                {
                    TempData["SuccessAlertMessage"] = "Employee successfully updated.";
                    return RedirectToAction("Index");

                }

                else
                {
                    ModelState.AddModelError(string.Empty, "Error occurred while updating Employee. "+ result.Message);
                    TempData["FailureAlertMessage"] = "Error occurred while updating Employee. " + result.Message;
                    var branchs = await _branch.GetAllAsync();
                    var branchList = new SelectList(branchs.BranchDtos, "Id", "Name").ToList();
                    branchList.Insert(0, new SelectListItem { Text = "Please select", Value = "" });
                    ViewBag.Branch = new SelectList(branchList, "Value", "Text", null);

                    var banks = await _bank.GetAllAsync();
                    var bankList = new SelectList(banks.BankDtos, "Id", "Name").ToList();
                    bankList.Insert(0, new SelectListItem { Text = "Please select", Value = "" });
                    ViewBag.Bank = new SelectList(bankList, "Value", "Text", null);
                    return View(request);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred while updating Employee.", ex);
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["ControllerName"] = "Employee";
            var branchs = await _branch.GetAllAsync();
            var branchList = new SelectList(branchs.BranchDtos, "Id", "Name").ToList();
            branchList.Insert(0, new SelectListItem { Text = "Please select", Value = "" });
            ViewBag.Branch = new SelectList(branchList, "Value", "Text", null);

            var banks = await _bank.GetAllAsync();
            var bankList = new SelectList(banks.BankDtos, "Id", "Name").ToList();
            bankList.Insert(0, new SelectListItem { Text = "Please select", Value = "" });
            ViewBag.Bank = new SelectList(bankList, "Value", "Text", null);

            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee request)
        {
            ViewData["ControllerName"] = "Employee";
            try
            {
                var result = await _Employee.CreateAsync(request);

                if (result.Status == OperationStatus.SUCCESS)
                {
                    TempData["SuccessAlertMessage"] = "Employee successfully inserted.";

                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error occurred while creating Employee.");
                    TempData["FailureAlertMessage"] = "Error occurred while creating Employee.";

                    var branchs = await _branch.GetAllAsync();
                    var branchList = new SelectList(branchs.BranchDtos, "Id", "Name").ToList();
                    branchList.Insert(0, new SelectListItem { Text = "Please select", Value = "" });
                    ViewBag.Branch = new SelectList(branchList, "Value", "Text", null);

                    var banks = await _bank.GetAllAsync();
                    var bankList = new SelectList(banks.BankDtos, "Id", "Name").ToList();
                    bankList.Insert(0, new SelectListItem { Text = "Please select", Value = "" });
                    ViewBag.Bank = new SelectList(bankList, "Value", "Text", null);

                    return View(request);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred while creating Employee.", ex);
                return View("Error");
            }
        }
        [HttpPost]
        public async Task<IActionResult> Search(EmployeeSearchRequest EmployeeSearchRequest)
        {
            var filterdEmployee = await _Employee.Search(EmployeeSearchRequest);
            return View("Index", filterdEmployee);
        }

        [HttpPost]

        public async Task<IActionResult> Delete(long id)
        {
            ViewData["ControllerName"] = "Employee";
            try
            {
                var result = await _Employee.DeleteAsync(id);
                if (result.Status == OperationStatus.SUCCESS)
                {
                    return Json(new { success = true, message = "Employee successfully deleted." });
                }
                else
                {
                    return Json(new { success = false, message = "Error occurred while deleting Employee." });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while deleting Employee with ID {id}.", ex);
                return Json(new { success = false, message = "An error occurred: " + ex.Message });
            }
        }
        public ActionResult EmpoloyePhoto(string file)
        {
            try
            {
                return File(file, "application/jpg");
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public ActionResult Cirtification(string file)
        {
            try
            {
                return File(file, "application/pdf");
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public ActionResult EmployeeKebeleID(string file)
        {
            try
            {
                return File(file, "application/pdf");
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public ActionResult GurrentKebeleID(string file)
        {
            try
            {
                return File(file, "application/pdf");
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public ActionResult GurrentDocumnet(string file)
        {
            try
            {
                return File(file, "application/pdf");
            }
            catch (Exception ex)
            {

                return null;
            }
        }
    }
}
