using AjaxCleaningHCM.Core.Helper.Interface;
using AjaxCleaningHCM.Domain.Models.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using static AjaxCleaningHCM.Domain.Enums.Common;

namespace AjaxCleaningHCM.Web.Areas.AjaxCleaningHCM.Controllers
{
    [Authorize]
    [Area("AjaxCleaningHCM")]
    public class MenuBuilderController : Controller
    {
        private readonly ILogger<MenuBuilderController> _logger;
        private readonly IMenuBuilder _MenuBuilder;
        public MenuBuilderController(IMenuBuilder MenuBuilder, ILogger<MenuBuilderController> logger)
        {
            _MenuBuilder = MenuBuilder;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewData["ControllerName"] = "MenuBuilder";
            ViewData["ActionName"] = "Index";
            try
            {
                var MenuBuilders = await _MenuBuilder.GetAllAsync();

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
                return View("Index", MenuBuilders);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred while fetching all MenuBuilders.", ex);
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(long id)
        {
            ViewData["ControllerName"] = "MenuBuilder";
            try
            {
                var MenuBuilder = await _MenuBuilder.GetByIdAsync(id);

                if (MenuBuilder == null)
                {
                    return NotFound();
                }

                return View(MenuBuilder);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while fetching MenuBuilder with ID {id}.", ex);
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(long id)
        {
            ViewData["ControllerName"] = "MenuBuilder";
            try
            {
                var MenuBuilder = await _MenuBuilder.GetByIdAsync(id);

                if (MenuBuilder == null)
                {
                    return NotFound();
                }
                var MenuBuilders = await _MenuBuilder.GetAllAsync();
                var MenuBuilderList = new SelectList(MenuBuilders.MenuBuilderDtos, "Id", "LinkText").ToList();
                MenuBuilderList.Insert(0, new SelectListItem { Text = "Please select", Value = "" });
                ViewBag.MenuBuilder = new SelectList(MenuBuilderList, "Value", "Text", null);
                return PartialView(MenuBuilder.MenuBuilderDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while fetching MenuBuilder with ID {id} for editing.", ex);
                return View("Error"); // You can customize the error view
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MenuBuilder request)
        {
            ViewData["ControllerName"] = "MenuBuilder";
            try
            {
                var result = await _MenuBuilder.UpdateAsync(request);
                if (result.Status == OperationStatus.SUCCESS)
                {
                    TempData["SuccessAlertMessage"] = "MenuBuilder successfully updated.";
                    return RedirectToAction("Index");

                }

                else
                {
                    ModelState.AddModelError(string.Empty, result.Message);
                    TempData["FailureAlertMessage"] = "Error occurred while updating MenuBuilder.";
                    var MenuBuilders = await _MenuBuilder.GetAllAsync();
                    var MenuBuilderList = new SelectList(MenuBuilders.MenuBuilderDtos, "Id", "LinkText").ToList();
                    MenuBuilderList.Insert(0, new SelectListItem { Text = "Please select", Value = "" });
                    ViewBag.MenuBuilder = new SelectList(MenuBuilderList, "Value", "Text", null);
                    return View(request);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred while updating MenuBuilder.", ex);
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["ControllerName"] = "MenuBuilder";
            var MenuBuilders = await _MenuBuilder.GetAllAsync();
            var MenuBuilderList = new SelectList(MenuBuilders.MenuBuilderDtos, "Id", "LinkText").ToList();
            MenuBuilderList.Insert(0, new SelectListItem { Text = "Please select", Value = "" });
            ViewBag.MenuBuilder = new SelectList(MenuBuilderList, "Value", "Text", null);
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MenuBuilder request)
        {
            ViewData["ControllerName"] = "MenuBuilder";
            try
            {
                var result = await _MenuBuilder.CreateAsync(request);

                if (result.Status == OperationStatus.SUCCESS)
                {
                    TempData["SuccessAlertMessage"] = "MenuBuilder successfully inserted.";

                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, result.Message);
                    TempData["FailureAlertMessage"] = "Error occurred while creating MenuBuilder.";
                    var MenuBuilders = await _MenuBuilder.GetAllAsync();
                    var MenuBuilderList = new SelectList(MenuBuilders.MenuBuilderDtos, "Id", "LinkText").ToList();
                    MenuBuilderList.Insert(0, new SelectListItem { Text = "Please select", Value = "" });
                    ViewBag.MenuBuilder = new SelectList(MenuBuilderList, "Value", "Text", null);
                    return View(request);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred while creating MenuBuilder.", ex);
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(long id)
        {
            ViewData["ControllerName"] = "MenuBuilder";
            try
            {
                var result = await _MenuBuilder.DeleteAsync(id);
                if (result.Status == OperationStatus.SUCCESS)
                {
                    TempData["SuccessAlertMessage"] = "MenuBuilder successfully deleted.";
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error occurred while deleteding MenuBuilder.");
                    TempData["FailureAlertMessage"] = "Error occurred while deleteding MenuBuilder.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while deleting MenuBuilder with ID {id}.", ex);
                return View("Error");
            }
        }
        public string GetItemCategorieTree(List<MenuBuilder> ItemCategories)
        {
            try
            {
                var rootNodes = ItemCategories.Where(x => x.ParentId == null).ToList();
                var tree = new List<TreeNode>();
                foreach (var rootNode in rootNodes)
                {

                    var node = new TreeNode
                    {
                        text = rootNode.LinkText,
                        state = new State() { opened = true, selected = false },
                        children = GetChildren(rootNode.Id, ItemCategories),
                        Id = rootNode.Id,
                        name = rootNode.LinkText,
                        menubuilderid = rootNode.ParentId,

                    };
                    tree.Add(node);
                }

                var restult = JsonConvert.SerializeObject(tree);
                return JsonConvert.SerializeObject(tree);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private List<TreeNode> GetChildren(long MenuBuilderId, List<MenuBuilder> ItemCategories)
        {
            try
            {
                if (MenuBuilderId == 0) return null;
                var childrenNodes = ItemCategories.Where(x => x.ParentId == MenuBuilderId).ToList();
                if (childrenNodes.Count == 0)
                {
                    return null;
                }
                var children = new List<TreeNode>();

                foreach (var childNode in childrenNodes)
                {
                    if (childNode != null)
                    {
                        var node = new TreeNode
                        {


                            text = childNode.LinkText,
                            state = new State() { opened = true, selected = false },
                            children = GetChildren(childNode.Id, ItemCategories),
                            Id = childNode.Id,
                            name = childNode?.LinkText,
                            menubuilderid = childNode.ParentId,

                        };
                        children.Add(node);
                    }

                }
                return children;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public class TreeNode
        {
            public long Id { get; set; }
            public string text { get; set; }
            public string name { get; set; }
            public long? menubuilderid { get; set; }
            public State state { get; set; }
            public List<TreeNode> children { get; set; }
        }
        public class State
        {
            public bool opened { get; set; }
            public bool selected { get; set; }
        }

        public async Task<string> GetMenuBuilder()
        {
            var result = await _MenuBuilder.GetAllAsync();
            var tree = GetItemCategorieTree(result.MenuBuilderDtos);
            return tree;
        }
    }
}
