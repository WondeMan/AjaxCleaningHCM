using System.Threading.Tasks;

namespace AjaxCleaningHCM.Domain.Identity
{
    public interface IRoleManager
    {
        Task<bool> RoleExists(string roleName);
        Task<bool> CreateRole(string roleName, string description = "");
    }
}
