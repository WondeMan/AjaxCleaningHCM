using AjaxCleaningHCM.Core.UserManagment.Identity;
using AjaxCleaningHCM.Domain.Models.Helper;
using AjaxCleaningHCM.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AjaxCleaningHCM.Core.Account.Interface
{
    public interface IUserAccount
    {
        Task<List<User>> GetAllAsync(string role);
        Task<List<User>> GetAllAsync();
        Task<User> GetByIdAsync(string id);
        Task<OperationStatusResponse> CreateAsync(User request, string roles);
        Task<OperationStatusResponse> UpdateAsync(User request, string roles);
        Task<OperationStatusResponse> DeleteAsync(string id);

        Task<OperationStatusResponse> ChangePassword(ChangePasswordViewModel model,string userId);
        Task<OperationStatusResponse> ForgotPassword(ForgotPasswordViewModel model,string callbackUrl);
        Task<OperationStatusResponse> Login(LoginViewModel model);
        Task<OperationStatusResponse> Lockout(string id, bool lockout);
        Task<OperationStatusResponse> Lockout(User model);
        Task<bool> IsUserValid(string userName, string password);
        Task<User> FindByNameAsync(string userName);
        Task<string> GeneratePasswordResetTokenAsync(User user);
        Task<OperationStatusResponse> ResetPassword(LoginViewModel model);
        Task<OperationStatusResponse> ResetPassword(string userName);
        Task LogOff();
    }
}
