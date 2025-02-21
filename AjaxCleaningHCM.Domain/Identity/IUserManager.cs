using System.Threading.Tasks;

namespace AjaxCleaningHCM.Domain.Identity
{
    public interface IUserManager
    {
        Task<bool> AddUserToRole(string userId, string roleName);
        Task ClearUserRoles(string userId);
        Task RemoveFromRole(string userId, string roleName);
        Task DeleteRole(string roleId);
    }
}
