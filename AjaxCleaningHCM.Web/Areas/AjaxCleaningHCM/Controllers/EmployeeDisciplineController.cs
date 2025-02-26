using AjaxCleaningHCM.Core.MasterData.Interface;
using AjaxCleaningHCM.Domain.Models.MasterData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;
using static AjaxCleaningHCM.Domain.Enums.Common;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace AjaxCleaningHCM.Web.Areas.AjaxCleaningHCM.Controllers
{
    [Authorize]
    [Area("AjaxCleaningHCM")]
    public class EmployeeDisciplineController : Controller
    {
        private readonly ILogger<EmployeeDisciplineController> _logger;
        private readonly IEmployeeDiscipline _EmployeeDiscipline;
        private readonly IEmployee _employee;
        private readonly IDisciplineCategory _disciplineCategory;
        public EmployeeDisciplineController(IEmployeeDiscipline EmployeeDiscipline, IEmployee employee, IDisciplineCategory disciplineCategory, ILogger<EmployeeDisciplineController> logger)
        {
            _EmployeeDiscipline = EmployeeDiscipline;
            _logger = logger;
            _employee = employee;
            _disciplineCategory = disciplineCategory;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewData["ControllerName"] = "EmployeeDiscipline";
            ViewData["ActionName"] = "Index";
            try
            {
                var EmployeeDisciplines = await _EmployeeDiscipline.GetAllAsync();

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
                return View("Index", EmployeeDisciplines);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred while fetching all EmployeeDisciplines.", ex);
                return View("Error");
            }
        }
        [HttpGet]
        public async Task<IActionResult> Details(long id)
        {
            ViewData["ControllerName"] = "EmployeeDiscipline";
            try
            {
                var EmployeeDiscipline = await _EmployeeDiscipline.GetByIdAsync(id);

                if (EmployeeDiscipline == null)
                {
                    return NotFound();
                }

                return View(EmployeeDiscipline);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while fetching EmployeeDiscipline with ID {id}.", ex);
                return View("Error");
            }
        }
        [HttpGet]
        public async Task<IActionResult> Edit(long id)
        {
            ViewData["ControllerName"] = "EmployeeDiscipline";
            try
            {
                var EmployeeDiscipline = await _EmployeeDiscipline.GetByIdAsync(id);

                if (EmployeeDiscipline == null)
                {
                    return NotFound();
                }
                ViewData["ControllerName"] = "EmployeeDiscipline";
                var employees = await _employee.GetAllAsync();
                var employeeList = new SelectList(employees.EmployeeDtos, "Id", "EmployeeId").ToList();
                ViewBag.Employees = new SelectList(employeeList, "Value", "Text", null);

                var disciplineCategory = await _disciplineCategory.GetAllAsync();
                var disciplineCategoryList = new SelectList(disciplineCategory.DisciplineCategoryDtos, "Id", "Name").ToList();
                ViewBag.DisciplineCategories = new SelectList(disciplineCategoryList, "Value", "Text", null);
                return PartialView(EmployeeDiscipline.EmployeeDisciplineDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while fetching EmployeeDiscipline with ID {id} for editing.", ex);
                return View("Error"); // You can customize the error view
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EmployeeDiscipline request)
        {
            ViewData["ControllerName"] = "EmployeeDiscipline";
            try
            {
                var result = await _EmployeeDiscipline.UpdateAsync(request);
                if (result.Status == OperationStatus.SUCCESS)
                {
                    TempData["SuccessAlertMessage"] = "EmployeeDiscipline successfully updated.";
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, result.Message);
                    TempData["FailureAlertMessage"] = "Error occurred while updating EmployeeDiscipline.";

                    return View(request);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred while updating EmployeeDiscipline.", ex);
                return View("Error");
            }
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["ControllerName"] = "EmployeeDiscipline";
            var employees = await _employee.GetAllAsync();
            var employeeList = new SelectList(employees.EmployeeDtos, "Id", "EmployeeId").ToList();
            ViewBag.Employees = new SelectList(employeeList, "Value", "Text", null);

            var disciplineCategory = await _disciplineCategory.GetAllAsync();
            var disciplineCategoryList = new SelectList(disciplineCategory.DisciplineCategoryDtos, "Id", "Name").ToList();
            ViewBag.DisciplineCategories = new SelectList(disciplineCategoryList, "Value", "Text", null);

            return PartialView();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeDiscipline request)
        {
            ViewData["ControllerName"] = "EmployeeDiscipline";
            try
            {
                var result = await _EmployeeDiscipline.CreateAsync(request);

                if (result.Status == OperationStatus.SUCCESS)
                {
                    TempData["SuccessAlertMessage"] = "EmployeeDiscipline successfully inserted.";

                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, result.Message);
                    TempData["FailureAlertMessage"] = "Error occurred while creating EmployeeDiscipline.";

                    return View(request);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred while creating EmployeeDiscipline.", ex);
                return View("Error");
            }
        }
        [HttpPost]
        public async Task<IActionResult> Delete(long id)
        {
            ViewData["ControllerName"] = "EmployeeDiscipline";
            try
            {
                var result = await _EmployeeDiscipline.DeleteAsync(id);
                if (result.Status == OperationStatus.SUCCESS)
                {
                    return Json(new { success = true, message = "EmployeeDiscipline successfully deleted." });
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
