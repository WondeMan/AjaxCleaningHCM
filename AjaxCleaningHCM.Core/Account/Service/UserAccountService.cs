using AjaxCleaningHCM.Core.Account.Interface;
using AjaxCleaningHCM.Core.UserManagment.Identity;
using AjaxCleaningHCM.Domain.Models.Helper;
using AjaxCleaningHCM.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using static AjaxCleaningHCM.Domain.Enums.Common;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AjaxCleaningHCM.Domain.Models;
using System.Security.Claims;
using System.Data;
using System.Web.Mvc;
using AjaxCleaningHCM.Domain.ViewModels;
using AjaxCleaningHCM.Core.Utils;
using System.Security.Policy;
using AjaxCleaningHCM.Domain.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace AjaxCleaningHCM.Core.Account.Service
{
    public class UserAccountService : IUserAccount
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;
        public SignInManager<User> SignInManager { get; private set; }
        public UserManager<User> UserManager { get; private set; }
        public UserAccountService(ApplicationDbContext context, UserManager<User> _userManager,
            SignInManager<User> _signInManager, IEmailSender emailSender, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _configuration = configuration;
            _emailSender = emailSender;
            SignInManager = _signInManager;
            UserManager = _userManager;
        }
        public async Task<OperationStatusResponse> CreateAsync(User User, string roles)
        {
            try
            {
                User.StartDate = DateTime.Now;
                User.EndDate = DateTime.MaxValue;
                User.RegisteredDate = DateTime.Now;
                User.RecordStatus = RecordStatus.Active;
                User.RegisteredBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                if (_context.Users.Where(a => a.RecordStatus != RecordStatus.Deleted).Count(c => c.UserName == User.UserName) == 0)
                {
                    var result = await UserManager.CreateAsync(User, _configuration.GetSection("UserSettings")["DefaultPassword"]);

                    if (result.Succeeded)
                    {
                        var Roles = new List<Role>();
                        if (!string.IsNullOrEmpty(roles))
                        {
                            var _ids = roles.Split(",");
                            var items = await _context.Roles.ToListAsync();
                            for (int i = 0; i < _ids.Length; i++)
                            {
                                var role = items.FirstOrDefault(f => f.Id.Equals(_ids[i]));
                                if (role != null)
                                    Roles.Add(role);
                            }
                        }
                        if (Roles.Count > 0)
                        {
                            foreach (var role in Roles)
                            {
                                _context.Add(new UserRole
                                {
                                    UserId = User.Id,
                                    RoleId = role.Id
                                });
                            }
                            _context.SaveChanges();
                        }
                    }
                    return new OperationStatusResponse
                    {
                        Message = "Operation Successfully Completed",
                        Status = OperationStatus.SUCCESS
                    };
                }
                return new OperationStatusResponse { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
            }
            catch (Exception ex)
            {
                return new OperationStatusResponse { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
            }
        }
        public async Task<OperationStatusResponse> CreateFromEmployeeAsync(User User, string roles)
        {
            try
            {
                User.StartDate = DateTime.Now;
                User.EndDate = DateTime.MaxValue;
                User.RegisteredDate = DateTime.Now;
                User.RecordStatus = RecordStatus.Active;
                User.LockedOut = true;
                User.RegisteredBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                if (_context.Users.Where(a => a.RecordStatus != RecordStatus.Deleted).Count(c => c.RecordStatus != RecordStatus.Deleted && c.UserName == User.UserName) == 0)
                {
                    var result = await UserManager.CreateAsync(User, _configuration.GetSection("UserSettings")["DefaultPassword"]);

                    if (result.Succeeded)
                    {
                        var Roles = new List<Role>();
                        if (!string.IsNullOrEmpty(roles))
                        {
                            var _ids = roles.Split(",");
                            var items = await _context.Roles.ToListAsync();
                            for (int i = 0; i < _ids.Length; i++)
                            {
                                var role = items.FirstOrDefault(f => f.Id.Equals(_ids[i]));
                                if (role != null)
                                    Roles.Add(role);
                            }
                        }
                        if (Roles.Count > 0)
                        {
                            foreach (var role in Roles)
                            {
                                _context.Add(new UserRole
                                {
                                    UserId = User.Id,
                                    RoleId = role.Id
                                });
                            }
                            _context.SaveChanges();
                        }
                    }
                    return new OperationStatusResponse
                    {
                        Message = "Operation Successfully Completed",
                        Status = OperationStatus.SUCCESS
                    };
                }
                return new OperationStatusResponse { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
            }
            catch (Exception ex)
            {
                return new OperationStatusResponse { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
            }
        }
        public async Task<OperationStatusResponse> DeleteAsync(string id)
        {
            try
            {
                var User = await _context.Users.Where(a => a.RecordStatus != RecordStatus.Deleted).FirstOrDefaultAsync(u => u.Id == id);

                if (User == null)
                    return new OperationStatusResponse { Message = "Record Does Not Exist", Status = OperationStatus.ERROR };
                var user = _context.Users.Where(a => a.RecordStatus != RecordStatus.Deleted).FirstOrDefault(x => x.Id == id);
                var userRoles = _context.UserRoles.Where(ur => ur.UserId == user.Id).ToList();
                if (userRoles.Count > 0)
                {
                    _context.UserRoles.RemoveRange(userRoles);
                    await _context.SaveChangesAsync();
                }

                var result = await UserManager.DeleteAsync(user);
                return new OperationStatusResponse { Message = "Operation Successfully Completed", Status = OperationStatus.SUCCESS };
            }
            catch (Exception ex)
            {
                return new OperationStatusResponse { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
            }
        }

        public async Task<List<User>> GetAllAsync()
        {
            try
            {
                var accounts = await _context.Users.Where(a => a.RecordStatus != RecordStatus.Deleted).Include(ur => ur.UserRoles).ToListAsync();
                return accounts;
            }
            catch (Exception)
            {
                return new List<User>();
                throw;
            }
        }

        public async Task<List<User>> GetAllAsync(string role)
        {
            try
            {
                var accounts = await _context.Users.Where(a => a.RecordStatus != RecordStatus.Deleted).Include(ur => ur.UserRoles).ToListAsync();

                if (!string.IsNullOrEmpty(role))
                {
                    var Role = _context.Roles.Where(r => r.Name == role).FirstOrDefault();
                    if (Role != null)
                    {
                        var userRoles = _context.UserRoles.Where(ur => ur.RoleId == Role.Id).ToList();
                        accounts = new List<User>();
                        foreach (var user in accounts)
                            if (userRoles.Where(ur => ur.UserId == user.Id).ToList().Count > 0)
                                accounts.Add(user);
                    }
                }

                return accounts;
            }
            catch (Exception ex)
            {
                return new List<User>();
            }
        }
        public async Task<User> GetByIdAsync(string id)
        {
            try
            {
                var User = await _context.Users.Where(x => x.RecordStatus != RecordStatus.Deleted && x.Id == id).ToListAsync();
                return User.FirstOrDefault();
            }
            catch (Exception ex)
            {
                return new User();
            }
        }
        public async Task<User> FindByNameAsync(string userName)
        {
            try
            {
                var User = await _context.Users.Where(x => x.UserName == userName && x.RecordStatus == RecordStatus.Active).ToListAsync();
                return User.FirstOrDefault();
            }
            catch (Exception ex)
            {
                return new User();
            }
        }
        public async Task<OperationStatusResponse> UpdateAsync(User request, string roles)
        {

            var user = await _context.Users.FindAsync(request.Id);
            if (user == null)
                return new OperationStatusResponse() { Status = OperationStatus.ERROR, Message = "Record Does Not Exist" };

            try
            {
                if (user != null)
                {
                    user.FirstName = request.FirstName;
                    user.MiddleName = request.MiddleName;
                    user.LastName = request.LastName;
                    user.UserName = request.UserName;
                    user.Email = request.Email;
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                    var Roles = new List<Role>();
                    if (!string.IsNullOrEmpty(roles))
                    {
                        var _ids = roles.Split(",");
                        var items = await _context.Roles.ToListAsync();
                        for (int i = 0; i < _ids.Length; i++)
                        {
                            var role = items.FirstOrDefault(f => f.Id.Equals(_ids[i]));
                            if (role != null)
                                Roles.Add(role);
                        }
                    }
                    if (Roles.Count > 0)
                    {
                        var exitingRoles = _context.UserRoles.Where(ur => ur.UserId == request.Id).ToList();
                        if (exitingRoles.Count > 0)
                        {
                            _context.UserRoles.RemoveRange(exitingRoles);
                            await _context.SaveChangesAsync();
                        }

                        foreach (var role in Roles)
                        {
                            _context.Add(new UserRole
                            {
                                UserId = request.Id,
                                RoleId = role.Id
                            });
                        }
                        _context.SaveChanges();
                    }
                }
                return new OperationStatusResponse
                {
                    Message = "Operation Successfully Completed",
                    Status = OperationStatus.SUCCESS
                };
            }
            catch (Exception ex)
            {
                return new OperationStatusResponse
                {
                    Message = "Error Has Occurred While Processing Your Request",
                    Status = OperationStatus.ERROR
                };
            }
        }
        public async Task<OperationStatusResponse> ChangePassword(ChangePasswordViewModel model, string userId)
        {
            try
            {
                var user = await UserManager.FindByIdAsync(userId);

                if (user != null)
                {
                    var result = await UserManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

                    if (result.Succeeded)
                    {
                        user.FirstLogin = false;

                        await UserManager.UpdateAsync(user);
                        await SignInManager.SignInAsync(user, isPersistent: false);

                        return new OperationStatusResponse
                        {
                            Message = "Operation Successfully Completed",
                            Status = OperationStatus.SUCCESS
                        };
                    }
                }
                return new OperationStatusResponse
                {
                    Message = "Error Has Occurred While Processing Your Request",
                    Status = OperationStatus.ERROR
                };
            }
            catch (Exception ex)
            {

                return new OperationStatusResponse
                {
                    Message = "Error Has Occurred While Processing Your Request",
                    Status = OperationStatus.ERROR
                };
            }
        }
        public async Task<OperationStatusResponse> ForgotPassword(ForgotPasswordViewModel model, string callbackUrl)
        {
            try
            {
                var user = await UserManager.FindByNameAsync(model.Username);

                if (user != null)// || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    _emailSender.SendEmail("<p>Dear SMS User,</p><p>Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a></p><p>Thank you,</p>", new List<string> { user.Email }, new List<string> { }, "Reset Password - AjaxCleaningHCM", "Account Management");
                }
                return new OperationStatusResponse
                {
                    Message = "Operation Successfully Completed",
                    Status = OperationStatus.SUCCESS
                };

            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<OperationStatusResponse> Login(LoginViewModel model)
        {
            try
            {
                var result = await SignInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    var user = _context.Users.Where(u => u.RecordStatus == RecordStatus.Active && u.UserName == model.Username).ToList().FirstOrDefault();

                    if (user.LockedOut)
                    {
                        await SignInManager.SignOutAsync();
                        return new OperationStatusResponse
                        {
                            Message = "User is locked out",
                            Status = OperationStatus.SUCCESS
                        };
                    }

                    if (user.FirstLogin)
                        return new OperationStatusResponse
                        {
                            Message = "Redirect To ChangePassword",
                            Status = OperationStatus.SUCCESS
                        };

                    else
                        return new OperationStatusResponse
                        {
                            Message = "Redirect To Local returnUrl",
                            Status = OperationStatus.SUCCESS
                        };
                }

                if (result.IsLockedOut)
                    return new OperationStatusResponse
                    {
                        Message = "Lockout",
                        Status = OperationStatus.SUCCESS
                    };
                else
                {
                    return new OperationStatusResponse
                    {
                        Message = "Invalid login attempt.",
                        Status = OperationStatus.SUCCESS
                    };

                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<OperationStatusResponse> Lockout(string id, bool lockout)
        {
            try
            {
                var user = await _context.Users.Where(a => a.RecordStatus != RecordStatus.Deleted).FirstOrDefaultAsync(f => f.Id == id);
                if (user != null)
                {
                    user.LockedOut = lockout;
                    _context.SaveChanges();
                    return new OperationStatusResponse()
                    {
                        Message = "User has been lock sucessfully.",
                        Status = OperationStatus.SUCCESS
                    };
                }
                else
                {
                    return new OperationStatusResponse()
                    {
                        Message = "User is not found.",
                        Status = OperationStatus.ERROR
                    };
                }

            }
            catch (Exception ex)
            {
                return new OperationStatusResponse()
                {
                    Message = "Something went wrong.",
                    Status = OperationStatus.ERROR
                };
            }

        }
        public async Task<OperationStatusResponse> Lockout(User model)
        {
            var user = _context.Users.Where(a => a.RecordStatus != RecordStatus.Deleted).FirstOrDefault(x => x.Id == model.Id);
            user.LockedOut = model.LockedOut;
            user.LockoutEnd = DateTime.UtcNow.AddDays(0);
            var result = await UserManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                if (model.LockedOut)
                {
                    return new OperationStatusResponse
                    {
                        Message = "User successfully locked.",
                        Status = OperationStatus.SUCCESS
                    };
                }
                else
                {
                    return new OperationStatusResponse
                    {
                        Message = "User successfully unlocked.",
                        Status = OperationStatus.SUCCESS
                    };
                }


            }
            return new OperationStatusResponse
            {
                Message = "Something went wrong",
                Status = OperationStatus.ERROR
            };

        }
        public async Task<bool> IsUserValid(string userName, string password)
        {
            var result = await SignInManager.PasswordSignInAsync(userName, password, false, lockoutOnFailure: false);

            if (result.Succeeded)
                return true;
            else
                return false;
        }
        public async Task<string> GeneratePasswordResetTokenAsync(User user)
        {
            string token = await UserManager.GeneratePasswordResetTokenAsync(user);
            return token;
        }
        public async Task<OperationStatusResponse> ResetPassword(LoginViewModel model)
        {
            var user = await FindByNameAsync(model.Username);
            var code = await GeneratePasswordResetTokenAsync(user);
            var randomPassword = RandomPasswordGenerator.CreateRandomPassword();

            var result = await UserManager.ResetPasswordAsync(user, code, randomPassword);
            if (result.Succeeded)
            {
                string body = "<p>Dear SMS User,</p> <p>Your password is reset as per your request. Please use this temporary password until you change it. Password = <b>" + randomPassword + "</b>. " +
                    "This is auto generated notification. Please do not reply to this email.</p><p>Thank you,</p>";
                string subject = "AjaxCleaningHCM Password Reset";
                _emailSender.SendEmail(body, new List<string> { user.Email }, new List<string> { }, subject, "AjaxCleaningHCM Notification");
                return new OperationStatusResponse
                {
                    Message = "Password reset successfully.",
                    Status = OperationStatus.SUCCESS
                };
            }
            return new OperationStatusResponse
            {
                Message = "Something went wrong please try again.",
                Status = OperationStatus.ERROR
            };
        }
        public async Task<OperationStatusResponse> ResetPassword(string userName)
        {
            try
            {
                var user = await GetByIdAsync(userName);
                user.FirstLogin = true;
                await UpdateAsync(user, null);
                var code = await GeneratePasswordResetTokenAsync(user);
                var defualtPassword = "Abcd@1234";

                var result = await UserManager.ResetPasswordAsync(user, code, defualtPassword);
                if (result.Succeeded)
                {
                    string body = "<p>Dear COCO COTTON CORNER User,</p> <p>Your password is reset as per your request. Please use this temporary password until you change it. Password = <b>" + defualtPassword + "</b>. " +
                        "This is auto generated notification. Please do not reply to this email.</p><p>Thank you,</p>";
                    string subject = "COCO COTTON CORNER Password Reset";
                    _emailSender.SendEmail(body, new List<string> { user.Email }, new List<string> { }, subject, "COCO COTTON CORNER Notification");
                    return new OperationStatusResponse
                    {
                        Message = "Password reset successfully.",
                        Status = OperationStatus.SUCCESS
                    };
                }
                return new OperationStatusResponse
                {
                    Message = "Something went wrong please try again.",
                    Status = OperationStatus.ERROR
                };
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task LogOff()
        {
            await SignInManager.SignOutAsync();
        }
    }
}
