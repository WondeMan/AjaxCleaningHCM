using AjaxCleaningHCM.Core.MasterData.Interface;
using AjaxCleaningHCM.Domain.DTO.MasterData.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using static AjaxCleaningHCM.Domain.Enums.Common;
using Microsoft.AspNetCore.Authorization;

namespace AjaxCleaningHCM.Web.Areas.AjaxCleaningHCM.Controllers
{
    [Authorize]
    [Area("AjaxCleaningHCM")]
    public class ShiftController : Controller
    {
        private readonly ILogger<ShiftController> _logger;
        private readonly IShift _product;
        public ShiftController(IShift product, ILogger<ShiftController> logger)
        {
            _product = product;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewData["ControllerName"] = "Shift";
            ViewData["ActionName"] = "Index";
            try
            {
                var products = await _product.GetAllAsync();

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
                return View("Index", products);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred while fetching all products.", ex);
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(long id)
        {
            ViewData["ControllerName"] = "Shift";
            try
            {
                var product = await _product.GetByIdAsync(id);

                if (product == null)
                {
                    return NotFound();
                }

                return View(product);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while fetching product with ID {id}.", ex);
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(long id)
        {
            ViewData["ControllerName"] = "Shift";
            try
            {
                var product = await _product.GetByIdAsync(id);

                if (product == null)
                {
                    return NotFound();
                }

                return PartialView(product.ShiftDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while fetching product with ID {id} for editing.", ex);
                return View("Error"); // You can customize the error view
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ShiftDto request)
        {
            ViewData["ControllerName"] = "Shift";
            try
            {
                var result = await _product.UpdateAsync(request);
                if (result.Status == OperationStatus.SUCCESS)
                {
                    TempData["SuccessAlertMessage"] = "Shift successfully updated.";
                    return RedirectToAction("Index");

                }

                else
                {
                    ModelState.AddModelError(string.Empty, "Error occurred while updating product.");
                    TempData["FailureAlertMessage"] = "Error occurred while updating product.";

                    return View(request);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred while updating product.", ex);
                return View("Error");
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["ControllerName"] = "Shift";
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ShiftDto request)
        {
            ViewData["ControllerName"] = "Shift";
            try
            {
                var result = await _product.CreateAsync(request);

                if (result.Status == OperationStatus.SUCCESS)
                {
                    TempData["SuccessAlertMessage"] = "Shift successfully inserted.";

                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error occurred while creating product.");
                    TempData["FailureAlertMessage"] = "Error occurred while creating product.";

                    return View(request);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred while creating product.", ex);
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(long id)
        {
            ViewData["ControllerName"] = "Shift";
            try
            {
                var result = await _product.DeleteAsync(id);
                if (result.Status == OperationStatus.SUCCESS)
                {
                    TempData["SuccessAlertMessage"] = "Shift successfully deleted.";
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error occurred while deleteding product.");
                    TempData["FailureAlertMessage"] = "Error occurred while deleteding product.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while deleting product with ID {id}.", ex);
                return View("Error");
            }
        }
    }
}
