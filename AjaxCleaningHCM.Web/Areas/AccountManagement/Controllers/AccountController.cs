using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using AjaxCleaningHCM.Domain.Identity;
using AjaxCleaningHCM.Domain.Utils;
using AjaxCleaningHCM.Domain.Models;
using AjaxCleaningHCM.Core.UserManagment.Identity;
using AjaxCleaningHCM.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AjaxCleaningHCM.Infrastructure.Data;
using AjaxCleaningHCM.Core.Utils;
using AjaxCleaningHCM.Core.Account.Interface;
using System.Xml.Linq;
using static AjaxCleaningHCM.Domain.Enums.Common;

namespace AjaxCleaningHCM.Web.Areas.AccountManagement.Controllers
{
    [Area("AccountManagement")]
    public class AccountController : Controller
    {
        [TempData]
        public string ErrorMessage { get; set; }

        LogWriter logWriter = new LogWriter();
        private readonly IRole _role;
        private readonly IUserAccount _UserAccount;
        public AccountController(IRole role,IUserAccount UserAccount)
        {
            _role = role;
            _UserAccount = UserAccount;
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl)
        {
            try
            {
                // Clear the existing external cookie to ensure a clean login process
                await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

                ViewBag.Messege = TempData["Messege"];
                ViewBag.ReturnUrl = returnUrl;
                return View();
            }
            catch (Exception ex)
            {
                logWriter.CreateLog(ex.Message,"Account/Login", "Error");
                return View();
            
            }
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _UserAccount.Login(model);

                    if (result.Status == OperationStatus.SUCCESS && result.Message == "User is locked out")
                    {
                        ModelState.AddModelError(string.Empty, "User is locked out.");
                        return View(model);
                    }
                    if (result.Status == OperationStatus.SUCCESS && result.Message == "Invalid login attempt.")
                    {
                        TempData["Messege"] = result.Message;
                        return RedirectToAction("Login");
                    }
                    if (result.Status == OperationStatus.SUCCESS && result.Message == "Redirect To ChangePassword")
                        return RedirectToAction("ChangePassword");
                    else
                        return RedirectToLocal(returnUrl);
                }
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(model);
            }
            catch (Exception ex)
            {
                logWriter.CreateLog(ex.Message, "Account/Login", "Error");
                return View();

            }


        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Lockout(string id, bool lockedOut)
        {
            try
            {
                var result = await _UserAccount.Lockout(id, lockedOut);
                if (result.Status == OperationStatus.SUCCESS)
                {
                    ViewBag.LockedOut = lockedOut;
                    TempData["SuccessAlertMessage"] = result.Message;
                    return RedirectToAction("Index");
                }
                TempData["FailureAlertMessage"] = result.Message;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                logWriter.CreateLog(ex.Message, "Account/Lockout", "Error");
                return View();

            }

        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Lockout(User model)
        {
            try
            {
                var result = await _UserAccount.Lockout(model);
                if (result.Status == OperationStatus.SUCCESS)
                {
                    if (model.LockedOut)
                        TempData["SuccessAlertMessage"] = "User successfully locked.";
                    else
                        TempData["SuccessAlertMessage"] = "User successfully unlocked.";

                    return RedirectToAction("Index");
                }
                TempData["FailureAlertMessage"] = result.Message;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                logWriter.CreateLog(ex.Message, "Account/Lockout", "Error");
                return View();

            }

        }

        public async Task<bool> IsUserValid(string userName, string password)
        {
            try
            {
                var result = await _UserAccount.IsUserValid(userName, password);
                if (result)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                logWriter.CreateLog(ex.Message, "Account/IsUserValid", "Error");
                return false;

            }

        }

        // GET: /Account/Index
        [HttpGet]
        [AjaxCleaningHCMAuthorizationFilter]
        public async Task<ActionResult> Index(string role)
        {
            try
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
                TempData["SuccessAlertMessage"] = null;
                TempData["FailureAlertMessage"] = null;
                var accounts = await _UserAccount.GetAllAsync(role);
                ViewBag.UsersUnderAGivenRole = role;
                return View(accounts);
            }
            catch (Exception ex)
            {
                logWriter.CreateLog(ex.Message, "Account/Index", "Error");
                return View();

            }

        }

        [HttpGet]
        [AjaxCleaningHCMAuthorizationFilter]
        public async Task<ActionResult> Details(string id)
        {            
            try
            {
                var account = await _UserAccount.GetByIdAsync(id);
                var userRoles = await _role.GetUserRoleByIdAsync(id);
                var roles = await _role.GetAllAsync();
                ViewBag.Roles = roles.Where(r => userRoles.Count(u => u.RoleId == r.Id) != 0).ToList();
                return View(account);
            }
            catch (Exception ex)
            {
                logWriter.CreateLog(ex.Message, "Account/Details", "Error");
                return View();

            }
        }

        [HttpGet]
       // [AjaxCleaningHCMAuthorizationFilter]
        public async Task<ActionResult> Register()
        {
            try
            {
                var roles = await _role.GetAllAsync();
                ViewBag.Roles = new SelectList(roles.ToList(), "Id", "Name");
                return View();
            }
            catch (Exception ex)
            {
                logWriter.CreateLog(ex.Message, "Account/Register", "Error");
                return View();

            }

        }

        [HttpPost]
        public async Task<ActionResult> Register(User user)
        {
           
            try
            {
                if (ModelState.IsValid)
                {
                    var roles = Request.Form["Roles"].ToString();
                    var result = await _UserAccount.CreateAsync(user, roles);
                    return RedirectToAction("Index", "Account");
                }
                var roleResult = await _role.GetAllAsync();
                ViewBag.Roles = new SelectList(roleResult.ToList(), "Id", "Name");
                return View(user);
            }
            catch (Exception ex)
            {
                logWriter.CreateLog(ex.Message, "Account/Register", "Error");
                return View();

            }
        }

        [HttpGet]
        [AjaxCleaningHCMAuthorizationFilter]
        public async Task<ActionResult> Edit(string id)
        {
            try
            {
                var roleResult = await _role.GetAllAsync();
                ViewBag.Roles = new SelectList(roleResult.ToList(), "Id", "Name");
                var user = await _UserAccount.GetByIdAsync(id);
                return View(user);
            }
            catch (Exception ex)
            {
                logWriter.CreateLog(ex.Message, "Account/Edit", "Error");
                return View();

            }

        }

        [HttpPost]
        [AjaxCleaningHCMAuthorizationFilter]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var roles = Request.Form["Roles"].ToString();
                    await _UserAccount.UpdateAsync(user, roles);
                    TempData["SuccessAlertMessage"] = "User successfully updated.";
                    return RedirectToAction("Index");
                }
                var roleResult = await _role.GetAllAsync();
                ViewBag.Roles = new SelectList(roleResult.ToList(), "Id", "Name");
                return View(user);
            }
            catch (Exception ex)
            {
                logWriter.CreateLog(ex.Message, "Account/Edit", "Error");
                return View();

            }
           
        }

        [HttpGet]
        [AjaxCleaningHCMAuthorizationFilter]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                if (id == null)
                {
                    return RedirectToAction("BadRequest", "Errors");
                }
                var result = await _UserAccount.DeleteAsync(id);
                if (result.Status == OperationStatus.SUCCESS)
                {
                    TempData["SuccessAlertMessage"] = result.Message;
                    return RedirectToAction("Index");
                }
                TempData["FailureAlertMessage"] = result.Message;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                logWriter.CreateLog(ex.Message, "Account/Delete", "Error");
                return View();

            }
        }

        //[HttpPost]
        //[AjaxCleaningHCMAuthorizationFilter]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Delete(User model)
        //{
            

        //    if (ModelState.IsValid)
        //    {
        //        var user = context.Users.FirstOrDefault(x => x.Id == model.Id);
        //        var userRoles = context.UserRoles.Where(ur => ur.UserId == user.Id).ToList();
        //        if (userRoles.Count > 0)
        //        {
        //            context.UserRoles.RemoveRange(userRoles);
        //            await context.SaveChangesAsync();
        //        }

        //        var result = await UserManager.DeleteAsync(user);

        //        if (result.Succeeded)
        //        {
        //           // accessLog.Save(this.ControllerContext.RouteData.Values["action"].ToString(), User.Identity.Name, "Account with Username = " + user.UserName + " and Email = " + user.Email + " has been deleted!", accessor.HttpContext.Connection.LocalIpAddress.ToString(), this.ControllerContext.RouteData.Values["controller"].ToString());

        //            TempData["SuccessAlertMessage"] = "User successfully removed.";
        //            return RedirectToAction("Index");
        //        }

        //        //AddErrors(result);
        //        TempData["FailureAlertMessage"] = GetErrors(result);
        //        return RedirectToAction("Index");
        //    }
        //    return View(user);
        //}

        //
        // GET: /Account/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

             var result=  await _UserAccount.ChangePassword(model, userId);
                if (result.Status == OperationStatus.SUCCESS)
                {
                    TempData["SuccessAlertMessage"] = result.Message;
                    return RedirectToAction("Index", "Main", new { area = "Main" });
                }
            }
            return View(model);
        }

        //
        // GET: /Account/ForgotPassword
        [HttpGet]
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _UserAccount.FindByNameAsync(model.Username);
                if (user == null)// || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }
                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                string code = await _UserAccount.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Scheme);
                await _UserAccount.ForgotPassword(model, callbackUrl);                
                return RedirectToAction("ForgotPasswordConfirmation", "Account", new { area = "AccountManagement", username=user.FirstName, email=user.Email});
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [HttpGet]
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation(string username,string email)
        {
            ViewBag.Username = username;
            ViewBag.email = email;
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [HttpGet]
        //[AjaxCleaningHCMAuthorizationFilter]
        public async Task<ActionResult> ResetPassword(string userId, string code)
        {
            if (userId != null && code != null)
            {
                var user =await _UserAccount.GetByIdAsync(userId);
                return View(new LoginViewModel { Username = user.UserName });
            }
            var users = await _UserAccount.GetAllAsync();
            var orderedUser=users.OrderBy(e => e.Email).ToList();
            ViewData["UserId"] = new SelectList(orderedUser, "UserName", "FullNameWithId");
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> ResetPasswordByAdmin(string name)
        {
            
            var result = await _UserAccount.ResetPassword(name);
            if (result.Status == OperationStatus.SUCCESS)
            {
                TempData["SuccessAlertMessage"] = result.Message;
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        //
        // POST: /Account/ResetPassword
        [HttpPost]
       // [ValidateAntiForgeryToken]
        //[AjaxCleaningHCMAuthorizationFilter]
        public async Task<ActionResult> ResetPassword(LoginViewModel model)
        {
            if (model.Username == null)
            {
                TempData["FailureAlertMessage"] = "Unable to find user.";
                return RedirectToAction(nameof(Index));
            }
            var user = await _UserAccount.FindByNameAsync(model.Username);
            var code = await _UserAccount.GeneratePasswordResetTokenAsync(user);
            var randomPassword = RandomPasswordGenerator.CreateRandomPassword();

            var result = await _UserAccount.ResetPassword(model);
            if (result.Status==OperationStatus.SUCCESS)
            {
                TempData["SuccessAlertMessage"] = result.Message;
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        [HttpGet]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/LogOff
        [HttpGet]
        [AjaxCleaningHCMAuthorizationFilter]
        public async Task<ActionResult> LogOff()
        {
            await _UserAccount.LogOff();
            return RedirectToAction("Login", "Account", new { area = "AccountManagement" });
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Main", new { area = "Main" });
            }
        }
        private string GetErrors(IdentityResult result)
        {
            string message = "";
            foreach (var error in result.Errors)
            {
                message = message + error + " ";
            }
            return message;
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        if (UserManager != null)
        //        {
        //            UserManager.Dispose();
        //            UserManager = null;
        //        }

        //        //if (SignInManager != null)
        //        //{
        //        //    SignInManager.Dispose();
        //        //    SignInManager = null;
        //        //}
        //    }

        //    base.Dispose(disposing);
        //}
    }
}
