using AjaxCleaningHCM.Core.MasterData.Interface;
using AjaxCleaningHCM.Domain.Models.MasterData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;
using static AjaxCleaningHCM.Domain.Enums.Common;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace AjaxCleaningHCM.Web.Areas.AjaxCleaningHCM.Controllers
{
    [Authorize]
    [Area("AjaxCleaningHCM")]
    public class LeaveRequestController : Controller
    {
        private readonly ILogger<LeaveRequestController> _logger;
        private readonly ILeaveRequest _LeaveRequest;
        private readonly IEmployee _employee;
        private readonly ILeaveType _leaveType;
        public LeaveRequestController(ILeaveRequest LeaveRequest,IEmployee employee,ILeaveType leaveType, ILogger<LeaveRequestController> logger)
        {
            _LeaveRequest = LeaveRequest;
            _logger = logger;
            _employee = employee;
            _LeaveRequest= LeaveRequest;
            _leaveType= leaveType;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewData["ControllerName"] = "LeaveRequest";
            ViewData["ActionName"] = "Index";
            try
            {
                var LeaveRequests = await _LeaveRequest.GetAllAsync();

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
                return View("Index", LeaveRequests);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred while fetching all LeaveRequests.", ex);
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> LeavBalance()
        {
            ViewData["ControllerName"] = "LeaveRequest";
            ViewData["ActionName"] = "LeavBalance";
            try
            {
                var LeaveRequests = await _LeaveRequest.GetAllAsync();

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
                return View(LeaveRequests);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred while fetching all LeaveRequests.", ex);
                return View("Error");
            }
        }

        public async Task<IActionResult> LeavBalance(DateTime startDate, DateTime endDate, string employeeId, int year)
        {
            var employees = new List<Employee>();
            var LeaveType = new List<LeaveType>();

            ViewBag.Employee = employees;
            ViewBag.LeaveType = LeaveType;
            ViewBag.Message = TempData["message"];
            var result =await _LeaveRequest.FiltereavBalanceByDate(startDate, endDate, employeeId, year);
            if (result.Status == OperationStatus.ERROR)
            {
                TempData["FailureAlertMessage"] = result.Message;
                ViewBag.FailureAlertMessage = TempData["FailureAlertMessage"];
            }

            return View("LeavBalance", result);
        }


        [HttpGet]
        public async Task<IActionResult> Details(long id)
        {
            ViewData["ControllerName"] = "LeaveRequest";
            try
            {
                var LeaveRequest = await _LeaveRequest.GetByIdAsync(id);

                if (LeaveRequest == null)
                {
                    return NotFound();
                }

                return View(LeaveRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while fetching LeaveRequest with ID {id}.", ex);
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(long id)
        {
            ViewData["ControllerName"] = "LeaveRequest";
            try
            {
                var LeaveRequest = await _LeaveRequest.GetByIdAsync(id);

                if (LeaveRequest == null)
                {
                    return NotFound();
                }
                var employees = await _employee.GetAllAsync();
                var employeeList = new SelectList(employees.EmployeeDtos, "Id", "EmployeeId").ToList();
                ViewBag.Employees = new SelectList(employeeList, "Value", "Text", null);

                var leaveType = await _leaveType.GetAllAsync();
                var leaveTypeList = new SelectList(leaveType.LeaveTypeDtos, "Id", "Name").ToList();
                ViewBag.LeaveType = new SelectList(leaveTypeList, "Value", "Text", null);

                var years = GetLastFiveYears();
                ViewBag.Years = new SelectList(years);

                return PartialView(LeaveRequest.LeaveRequestDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while fetching LeaveRequest with ID {id} for editing.", ex);
                return View("Error"); // You can customize the error view
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(LeaveRequest request)
        {
            ViewData["ControllerName"] = "LeaveRequest";
            try
            {
                var result = await _LeaveRequest.UpdateAsync(request);
                if (result.Status == OperationStatus.SUCCESS)
                {
                    TempData["SuccessAlertMessage"] = "LeaveRequest successfully updated.";
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, result.Message);
                    TempData["FailureAlertMessage"] = "Error occurred while updating LeaveRequest.";
                    return View(request);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred while updating LeaveRequest.", ex);
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["ControllerName"] = "LeaveRequest";
            ViewData["ControllerName"] = "EmployeeDiscipline";
            var employees = await _employee.GetAllAsync();
            var employeeList = new SelectList(employees.EmployeeDtos, "Id", "EmployeeId").ToList();
            ViewBag.Employees = new SelectList(employeeList, "Value", "Text", null);

            var leaveType = await _leaveType.GetAllAsync();
            var leaveTypeList = new SelectList(leaveType.LeaveTypeDtos, "Id", "Name").ToList();
            ViewBag.LeaveTypes = new SelectList(leaveTypeList, "Value", "Text", null);
            var years = GetLastFiveYears();
            ViewBag.Years = new SelectList(years);
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LeaveRequest request)
        {
            ViewData["ControllerName"] = "LeaveRequest";
            try
            {
                var result = await _LeaveRequest.CreateAsync(request);

                if (result.Status == OperationStatus.SUCCESS)
                {
                    TempData["SuccessAlertMessage"] = "LeaveRequest successfully inserted.";

                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, result.Message);
                    TempData["FailureAlertMessage"] = "Error occurred while creating LeaveRequest.";
                    var employees = await _employee.GetAllAsync();
                    var employeeList = new SelectList(employees.EmployeeDtos, "Id", "EmployeeId").ToList();
                    ViewBag.Employees = new SelectList(employeeList, "Value", "Text", null);

                    var leaveType = await _leaveType.GetAllAsync();
                    var leaveTypeList = new SelectList(leaveType.LeaveTypeDtos, "Id", "Name").ToList();
                    ViewBag.LeaveTypes = new SelectList(leaveTypeList, "Value", "Text", null);
                    var years = GetLastFiveYears();
                    ViewBag.Years = new SelectList(years);
                    return View(request);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred while creating LeaveRequest.", ex);
                return View("Error");
            }
        }


        [HttpPost]

        public async Task<IActionResult> Delete(long id)
        {
            ViewData["ControllerName"] = "LeaveRequest";
            try
            {
                var result = await _LeaveRequest.DeleteAsync(id);
                if (result.Status == OperationStatus.SUCCESS)
                {
                    return Json(new { success = true, message = "LeaveRequest successfully deleted." });
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
        public List<int> GetLastFiveYears()
        {
            int currentYear = DateTime.Now.Year;

            // Generate a list of the last 5 years
            List<int> last5Years = new List<int>();
            for (int i = 0; i < 5; i++)
            {
                last5Years.Add(currentYear - i);
            }
            return last5Years;
        }
    }
}
