using AjaxCleaningHCM.Core.MasterData.Interface;
using AjaxCleaningHCM.Domain.DTO.MasterData.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using AjaxCleaningHCM.Domain.Models;
using static AjaxCleaningHCM.Domain.Enums.Common;

namespace AjaxCleaningHCM.Web.Areas.AjaxCleaningHCM.Controllers
{
    [Area("AjaxCleaningHCM")]
        public class ProductController : Controller
        {
            private readonly ILogger<ProductController> _logger;
            private readonly IProduct _product;
            public ProductController(IProduct product, ILogger<ProductController> logger)
            {
                _product = product;
                _logger = logger;
            }
            [HttpGet]
            public async Task<IActionResult> Index()
            {
            ViewData["ControllerName"] = "Product";
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
            ViewData["ControllerName"] = "Product";
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
            ViewData["ControllerName"] = "Product";
            try
            {
                    var product = await _product.GetByIdAsync(id);

                    if (product == null)
                    {
                        return NotFound();
                    }

                    return PartialView(product.ProductDto);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error occurred while fetching product with ID {id} for editing.", ex);
                    return View("Error"); // You can customize the error view
                }
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Edit(ProductDto request)
            {
            ViewData["ControllerName"] = "Product";
            try
            {
                    var result = await _product.UpdateAsync(request);
                    if (result.Status == OperationStatus.SUCCESS)
                    {
                        TempData["SuccessAlertMessage"] = "Product successfully updated.";
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
            ViewData["ControllerName"] = "Product";
            return PartialView();
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Create(ProductDto request)
            {
            ViewData["ControllerName"] = "Product";
            try
            {
                    var result = await _product.CreateAsync(request);

                    if (result.Status == OperationStatus.SUCCESS)
                    {
                        TempData["SuccessAlertMessage"] = "Product successfully inserted.";

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
            ViewData["ControllerName"] = "Product";
            try
            {
               var result = await _product.DeleteAsync(id);
                if (result.Status == OperationStatus.SUCCESS)
                {
                    TempData["SuccessAlertMessage"] = "Product successfully deleted.";
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
