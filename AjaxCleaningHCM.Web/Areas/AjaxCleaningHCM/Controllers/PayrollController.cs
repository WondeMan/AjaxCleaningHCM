using AjaxCleaningHCM.Core.MasterData.Interface;
using AjaxCleaningHCM.Core.Operational.Interface;
using AjaxCleaningHCM.Domain.DTO.MasterData.Response;
using AjaxCleaningHCM.Domain.Models.MasterData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System;
using System.Linq;
using System.Threading.Tasks;
using static AjaxCleaningHCM.Domain.Enums.Common;

namespace AjaxCleaningHCM.Web.Areas.AjaxCleaningHCM.Controllers
{
    [Authorize]
    [Area("AjaxCleaningHCM")]
    public class PayrollController : Controller
    {
        private readonly ILogger<PayrollController> _logger;
        private readonly IPayrollService _Payroll;
        private readonly IEmployee _employee;

        public PayrollController( ILogger<PayrollController> logger, IPayrollService payrollService, IEmployee employee)
        {
            _employee = employee;
            _logger = logger;
            _Payroll = payrollService;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["ControllerName"] = "Payroll";
            ViewData["ActionName"] = "Index";

            var result = await _Payroll.GetAllAsync();
            return PartialView(result);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["ControllerName"] = "Attendance";
            var result = await _employee.GetAllAsync();
            return PartialView(result);
        }
        [HttpPost]

        public  async Task<IActionResult> Create(PayrollRequst payrollRequst)
        {
            string Payroll = Request.Form["Payroll"];
            var payrolls = Payroll?.Split(',')
                          .Select(p => long.TryParse(p, out var id) ? id : (long?)null)
                          .Where(id => id.HasValue)
                          .Select(id => id.Value)
                          .ToList();
            string date = Request.Form["Month"];
            int year = 0;
            int month = 0;
            if (!string.IsNullOrEmpty(date) && date.Contains("-"))
            {
                string[] parts = date.Split('-');
                string stringYear = parts[0]; 
                string stringMonth = parts[1];
                 year = int.Parse(stringYear);
                 month = int.Parse(stringMonth);
            }


            var result = await _Payroll.ProcessPayrollForBulkEmployees(payrolls, year, month);
            if (result.Status == OperationStatus.SUCCESS)
            {
                TempData["SuccessAlertMessage"] = "Bank successfully inserted.";

                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, result.Message);
                TempData["FailureAlertMessage"] = "Error occurred while creating Bank.";
                var employeeresult = await _employee.GetAllAsync();
                return View(employeeresult);
            }
        }

        [HttpPost]

        public async Task<IActionResult> Delete(long id)
        {
            ViewData["ControllerName"] = "Payroll";
            try
            {
                var result = await _Payroll.DeleteAsync(id);
                if (result.Status == OperationStatus.SUCCESS)
                {
                    return Json(new { success = true, message = "Payroll successfully deleted." });
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

        public async Task<IActionResult> Search(DateTime requestDate)
        {
            ViewData["ControllerName"] = "Payroll";
            ViewData["ActionName"] = "Index";


            string date = Request.Form["Month"];
            int year = 0;
            int month = 0;
            if (!string.IsNullOrEmpty(date) && date.Contains("-"))
            {
                string[] parts = date.Split('-');
                string stringYear = parts[0];
                string stringMonth = parts[1];
                year = int.Parse(stringYear);
                month = int.Parse(stringMonth);
            }
            var result = await _Payroll.Search(year,month);
            return View("Index", result);

        }
    }
}
