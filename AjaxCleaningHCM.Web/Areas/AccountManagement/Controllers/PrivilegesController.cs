using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AjaxCleaningHCM.Core.UserManagment.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using AjaxCleaningHCM.Core.UserManagment.Identity;
using AjaxCleaningHCM.Infrastructure.Data;
using AjaxCleaningHCM.Core.Account.Interface;
using System.Threading.Tasks;

namespace SDMIS.Web.Controllers
{
    [Area("AccountManagement")]
    [AjaxCleaningHCMAuthorizationFilter]
    public class PrivilegesController : Controller
    {
        //private ApplicationDbContext context;
        private readonly IPrivilege _privilege;
        public PrivilegesController(ApplicationDbContext _db, IPrivilege privilege)
        {
            //context = _db;
            //serviceProvider = _serviceProvider;
            _privilege=privilege;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
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
            var privileges=await _privilege.GetAllAsync();
            return View(privileges.OrderBy(a=>a.Action).ToList());
        }

        [HttpGet]
        public ActionResult Create(string message = "")
        {
            ViewBag.Message = message;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Privilege privilege)
        {
            if (ModelState.IsValid)
            {
                  await  _privilege.CreateAsync(privilege);
                    TempData["SuccessAlertMessage"] = "Privilege has been successfully created.";
                    return RedirectToAction("Index");        
            }
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> Edit(string id)
        {
          var previlege = await  _privilege.GetByIdAsync(id);
            return View(previlege);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Privilege previlege)
        {
            if (ModelState.IsValid)
            {
                _privilege.UpdateAsync(previlege);
                TempData["SuccessAlertMessage"] = "Privilege has been successfully updated.";
                return RedirectToAction("Index");
            }
            return View(previlege);
        }

        [HttpGet]
        public async Task<ActionResult> Delete(string Id)
        {
            await _privilege.DeleteAsync(Id);   
            TempData["SuccessAlertMessage"] = "Privilege has been successfully updated.";
            return RedirectToAction("Index");
        }

        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(Privilege p)
        //{
        //    Privilege privilege = context.Privileges.FirstOrDefault(p => p.Id == p.Id);
        //    List<RolePrivilege> rolePrivilege =
        //        context.RolePrivileges.Where(rp => rp.PrivilegeId == privilege.Id).ToList();
        //    context.Privileges.Remove(privilege);
        //    context.RolePrivileges.RemoveRange(rolePrivilege);
        //    context.SaveChanges();
        //    TempData["SuccessAlertMessage"] = "Privilege has been successfully deleted.";
        //    return RedirectToAction("Index");
        //}
    }
}
