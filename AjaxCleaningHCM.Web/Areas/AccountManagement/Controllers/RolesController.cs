using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AjaxCleaningHCM.Domain.Identity;
using AjaxCleaningHCM.Core.UserManagment.Identity;
using AjaxCleaningHCM.Domain.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AjaxCleaningHCM.Infrastructure.Data;
using AjaxCleaningHCM.Core.Account.Interface;
using System.Net.NetworkInformation;
using AjaxCleaningHCM.Domain.Models;
using static AjaxCleaningHCM.Domain.Enums.Common;

namespace SDMIS.Web.Controllers
{
    [Area("AccountManagement")]
    [AjaxCleaningHCMAuthorizationFilter]
    public class RolesController : Controller
    {
        //private ApplicationDbContext context;
        //readonly IRoleManager roleManager;
        //readonly IUserManager userManager;
        private readonly IRole _role;
        private readonly IPrivilege _privilege;
        private readonly IRolePrivilege _rolePrivilege;

        public RoleManager<Role> RoleManager { get; private set; }

        public RolesController(IRoleManager _roleManager, IRolePrivilege rolePrivilege,
            IRole role,
            IPrivilege privilege,
            ApplicationDbContext context, 
            RoleManager<Role> roleManager, 
            IUserManager _userManager)
        {
           // this.context = context;
            //this.roleManager = _roleManager;
            //this.RoleManager = roleManager;
            //this.userManager = _userManager;
            _role = role;
            _privilege = privilege;
            _rolePrivilege = rolePrivilege;
        }

        [HttpGet]
        //[AdmiLteAuthorizationFilter]
        public async Task<ActionResult> Index()
        {
            var rolesList =await _role.GetAllAsync();

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
            return View(rolesList);
        }

        [HttpGet]
        public async Task<ActionResult> Create(string message = "")
        {
            var privilege=await _privilege.GetAllAsync();
            ViewBag.Privileges = privilege.OrderBy(pr => pr.Action).ToList();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RoleViewModel model)
        {
            string pri = Request.Form["privilege"];
            if (ModelState.IsValid && pri != null)
            {
               var result=await _role.CreateRoleWithPrivilegeAsync(model, pri);
                if (result.Status== OperationStatus.ERROR)
                    return View(result.Message);
                else
                {
                    TempData["SuccessAlertMessage"] = result.Message;
                    return RedirectToAction("Index");
                }
            }
            TempData["FailureAlertMessage"] = "The role must have at least one corresponding privilege"; 
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> Edit(string name)
        {
            string selected = "";
            var role =await _role.GetByRoleNameAsync(name);
            List<RolePrivilege> rolePrivileges =await _rolePrivilege.GetByRoleIdAsync(role.Id);
            foreach (var item in rolePrivileges)
            {
                selected += item.PrivilegeId + ",";
            }
            var privileges=await _privilege.GetAllAsync();
            ViewBag.Selected = selected;
            ViewBag.Privileges = privileges.OrderBy(pr => pr.Action).ToList();
            ViewBag.OldRole = role.Name;

            var model = new RoleViewModel
            {
                RoleName = role.Name,
                Description = role.Description
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(RoleViewModel model)
        {
            if (model != null)
            {
                string privilege = Request.Form["privilege"];
                string oldRole = Request.Form["OldRole"];

                if (privilege != null && oldRole != null)
                {
                  var result=  await _role.UpdateRoleWithPrivilegeAsync(model, oldRole, privilege);
                    TempData["SuccessAlertMessage"] = "Role successfully updated.";
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Delete(string name)
        {
            if (name == null)
            {
                return RedirectToAction("BadRequest", "Errors");
            }
  
            var result =await _role.DeleteAsync(name);
            if (result.Status== OperationStatus.ERROR)
            {
                TempData["FailureAlertMessage"] =result.Message;
                return RedirectToAction("Index");
            }
            if (result.Status == OperationStatus.SUCCESS)
            {
                TempData["SuccessAlertMessage"] = result.Message;
                return RedirectToAction("Index");
            }

            return View();
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string name)
        {
          var result=await _role.DeleteAsync(name);

            TempData["SuccessAlertMessage"] = result.Message;

            return RedirectToAction("Index");
        }
    }
}