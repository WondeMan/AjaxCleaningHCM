using AjaxCleaningHCM.Core.MasterData.Interface;
using AjaxCleaningHCM.Domain.Models.MasterData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;
using static AjaxCleaningHCM.Domain.Enums.Common;

namespace AjaxCleaningHCM.Web.Areas.AjaxCleaningHCM.Controllers
{
    [Authorize]
    [Area("AjaxCleaningHCM")]
    public class DisciplineCategoryController : Controller
    {
        private readonly ILogger<DisciplineCategoryController> _logger;
        private readonly IDisciplineCategory _DisciplineCategory;
        public DisciplineCategoryController(IDisciplineCategory DisciplineCategory, ILogger<DisciplineCategoryController> logger)
        {
            _DisciplineCategory = DisciplineCategory;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewData["ControllerName"] = "DisciplineCategory";
            ViewData["ActionName"] = "Index";
            try
            {
                var DisciplineCategorys = await _DisciplineCategory.GetAllAsync();

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
                return View("Index", DisciplineCategorys);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred while fetching all DisciplineCategorys.", ex);
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(long id)
        {
            ViewData["ControllerName"] = "DisciplineCategory";
            try
            {
                var DisciplineCategory = await _DisciplineCategory.GetByIdAsync(id);

                if (DisciplineCategory == null)
                {
                    return NotFound();
                }

                return View(DisciplineCategory);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while fetching DisciplineCategory with ID {id}.", ex);
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(long id)
        {
            ViewData["ControllerName"] = "DisciplineCategory";
            try
            {
                var DisciplineCategory = await _DisciplineCategory.GetByIdAsync(id);

                if (DisciplineCategory == null)
                {
                    return NotFound();
                }

                return PartialView(DisciplineCategory.DisciplineCategoryDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while fetching DisciplineCategory with ID {id} for editing.", ex);
                return View("Error"); // You can customize the error view
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DisciplineCategory request)
        {
            ViewData["ControllerName"] = "DisciplineCategory";
            try
            {
                var result = await _DisciplineCategory.UpdateAsync(request);
                if (result.Status == OperationStatus.SUCCESS)
                {
                    TempData["SuccessAlertMessage"] = "DisciplineCategory successfully updated.";
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, result.Message);
                    TempData["FailureAlertMessage"] = "Error occurred while updating DisciplineCategory.";
                    return View(request);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred while updating DisciplineCategory.", ex);
                return View("Error");
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["ControllerName"] = "DisciplineCategory";
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DisciplineCategory request)
        {
            ViewData["ControllerName"] = "DisciplineCategory";
            try
            {
                var result = await _DisciplineCategory.CreateAsync(request);

                if (result.Status == OperationStatus.SUCCESS)
                {
                    TempData["SuccessAlertMessage"] = "DisciplineCategory successfully inserted.";

                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, result.Message);
                    TempData["FailureAlertMessage"] = "Error occurred while creating DisciplineCategory.";

                    return View(request);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred while creating DisciplineCategory.", ex);
                return View("Error");
            }
        }


        [HttpPost]

        public async Task<IActionResult> Delete(long id)
        {
            ViewData["ControllerName"] = "DisciplineCategory";
            try
            {
                var result = await _DisciplineCategory.DeleteAsync(id);
                if (result.Status == OperationStatus.SUCCESS)
                {
                    return Json(new { success = true, message = "DisciplineCategory successfully deleted." });
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
