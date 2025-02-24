using AjaxCleaningHCM.Core.MasterData.Interface;
using AjaxCleaningHCM.Domain.Models.MasterData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using static AjaxCleaningHCM.Domain.Enums.Common;
using AjaxCleaningHCM.Domain.DTO.MasterData.Response;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace AjaxCleaningHCM.Web.Areas.AjaxCleaningHCM.Controllers
{
    [Authorize]
    [Area("AjaxCleaningHCM")]
    public class EmployeeTerminationController : Controller
    {
        private readonly ILogger<EmployeeTerminationController> _logger;
        private readonly IEmployeeTermination _EmployeeTermination;
        private readonly IEmployee _Employee;

        public EmployeeTerminationController(IEmployeeTermination EmployeeTermination, IEmployee Employee, ILogger<EmployeeTerminationController> logger)
        {
            _EmployeeTermination = EmployeeTermination;
            _Employee = Employee;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewData["ControllerName"] = "EmployeeTermination";
            ViewData["ActionName"] = "Index";
            try
            {
                var EmployeeTerminations = await _EmployeeTermination.GetAllAsync();

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
                return View("Index", EmployeeTerminations);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred while fetching all EmployeeTerminations.", ex);
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(long id)
        {
            ViewData["ControllerName"] = "EmployeeTermination";
            try
            {
                var EmployeeTermination = await _EmployeeTermination.GetByIdAsync(id);

                if (EmployeeTermination == null)
                {
                    return NotFound();
                }

                return View(EmployeeTermination);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while fetching EmployeeTermination with ID {id}.", ex);
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(long id)
        {
            ViewData["ControllerName"] = "EmployeeTermination";
            try
            {
                var EmployeeTermination = await _EmployeeTermination.GetByIdAsync(id);
                var employee = await _Employee.GetAllAsync();
                var employeeList = new SelectList(employee.EmployeeDtos, "Id", "EmployeeId").ToList();
                employeeList.Insert(0, new SelectListItem { Text = "Please select", Value = "" });
                ViewBag.Employee = new SelectList(employeeList, "Value", "Text", null);
                ViewBag.SelectedEmployee = string.Join(",", employee.EmployeeDtos.Select(a => a.EmployeeId));

                if (EmployeeTermination == null)
                {
                    return NotFound();
                }

                return PartialView(EmployeeTermination.EmployeeTerminationDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while fetching EmployeeTermination with ID {id} for editing.", ex);
                return View("Error"); // You can customize the error view
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EmployeeTermination request)
        {
            ViewData["ControllerName"] = "EmployeeTermination";
            try
            {
                var result = await _EmployeeTermination.UpdateAsync(request);
                if (result.Status == OperationStatus.SUCCESS)
                {
                    TempData["SuccessAlertMessage"] = "EmployeeTermination successfully updated.";
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, result.Message);
                    TempData["FailureAlertMessage"] = "Error occurred while updating EmployeeTermination.";
                    return View(request);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred while updating EmployeeTermination.", ex);
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var employee = await _Employee.GetAllAsync();
            var employeeList = new SelectList(employee.EmployeeDtos, "Id", "EmployeeId").ToList();
            employeeList.Insert(0, new SelectListItem { Text = "Please select", Value = "" });
            ViewBag.Employee = new SelectList(employeeList, "Value", "Text", null);
            ViewData["ControllerName"] = "EmployeeTermination";
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeTermination request)
        {
            ViewData["ControllerName"] = "EmployeeTermination";
            try
            {
                var result = await _EmployeeTermination.CreateAsync(request);

                if (result.Status == OperationStatus.SUCCESS)
                {
                    TempData["SuccessAlertMessage"] = "EmployeeTermination successfully inserted.";

                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, result.Message);
                    TempData["FailureAlertMessage"] = "Error occurred while creating EmployeeTermination.";

                    return View(request);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred while creating EmployeeTermination.", ex);
                return View("Error");
            }
        }


        [HttpPost]

        public async Task<IActionResult> Delete(long id)
        {
            ViewData["ControllerName"] = "EmployeeTermination";
            try
            {
                var result = await _EmployeeTermination.DeleteAsync(id);
                if (result.Status == OperationStatus.SUCCESS)
                {
                    return Json(new { success = true, message = "EmployeeTermination successfully deleted." });
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
        public async Task<ActionResult> Rehire(long employeeId, long terminationId)
        {
            var employee =await _Employee.GetByIdAsync(employeeId);
            var termination =await _EmployeeTermination.GetByEmplyeeId(employeeId);

            if (employee.EmployeeDto == null || termination.EmployeeTerminationDto == null)
            {
                TempData["ErrorAlertMessage"] = "Employee or termination record not found.";
                return RedirectToAction("Index");
            }
            employee.EmployeeDto.RecordStatus=RecordStatus.Active;
            employee.EmployeeDto.LastUpdateDate = DateTime.Now;
            termination.EmployeeTerminationDto.LastUpdateDate= DateTime.Now;
            termination.EmployeeTerminationDto.EmployeeStatus= EmployeeStatus.Rehire;
            var updateEmployee = await _Employee.UpdateEmployeeStatusAsync(employee.EmployeeDto);
            var updateEmployeeTermination = await _EmployeeTermination.UpdateAsync(termination.EmployeeTerminationDto);
          if (updateEmployee.Status == OperationStatus.SUCCESS && updateEmployeeTermination.Status==OperationStatus.SUCCESS)
            {
                return Json(new { success = true, message = "Employee has been successfully rehired!" });
            }
            else
            {
                return Json(new { success = false, message = "An error occurred while rehiring the employee." });
            }
        }
        public async Task<ActionResult> TerminationLetter(int id)
        {
            return View(await _EmployeeTermination.GetByIdAsync(id));
        }
        public async Task<ActionResult> ExperienceLetter(int id)
        {
            var employee =await _Employee.GetByIdAsync(id);
            var terminatedHistory =await _EmployeeTermination.GetByEmplyeeId(id);
            ViewBag.TerminationDate = DateTime.Now;
            if (terminatedHistory.EmployeeTerminationDto != null)
            {
                ViewBag.TerminationDate = terminatedHistory.EmployeeTerminationDto.TerminationDate;
            }
            return View(employee);
        }
        [HttpPost]
        public async Task<IActionResult> Search(SearchRequest request)
        {
            try
            {
                var result =await _EmployeeTermination.Search(request);
                TempData["message"] = result.Message;
                if (result.Status == OperationStatus.SUCCESS)
                {
                    return View("Index", result);
                }
                return View("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
