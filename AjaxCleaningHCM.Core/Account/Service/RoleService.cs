using AjaxCleaningHCM.Core.Account.Interface;
using AjaxCleaningHCM.Core.UserManagment.Identity;
using AjaxCleaningHCM.Domain.Models.Helper;
using AjaxCleaningHCM.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using static AjaxCleaningHCM.Domain.Enums.Common;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AjaxCleaningHCM.Domain.Models;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using AjaxCleaningHCM.Domain.Identity;
using AjaxCleaningHCM.Domain.ViewModels;
using AjaxCleaningHCM.Domain.UserManagment.Services;
using AjaxCleaningHCM.Core.UserManagment.Services;

namespace AjaxCleaningHCM.Core.Account.Service
{
    public class RoleService : IRole
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly IRoleManager _roleManager;
        private readonly IUserManager _userManager;

        public RoleService(ApplicationDbContext context, IRoleManager roleManager, IUserManager userManager, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _configuration = configuration;
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public async Task<OperationStatusResponse> CreateAsync(Role request)
        {
            try
            {
                request.StartDate = DateTime.Now;
                request.EndDate = DateTime.MaxValue;
                request.RegisteredDate = DateTime.Now;
                request.RegisteredBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                var Role = await _context.Roles.FirstOrDefaultAsync(i => i.Name.Equals(request.Name));
                if (Role == null)
                {
                    request.Id = Guid.NewGuid().ToString();
                    _context.Roles.Add(request);
                    _context.SaveChanges();
                }
                return new OperationStatusResponse { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
            }
            catch (Exception ex)
            {
                return new OperationStatusResponse { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
            }
        }
        public async Task<OperationStatusResponse> CreateRoleWithPrivilegeAsync(RoleViewModel model, string Privilege)
        {
            try
            {
                if (await _roleManager.RoleExists(model.RoleName))
                     return new OperationStatusResponse { Message = "That role name has already been used", Status = OperationStatus.ERROR };
                else
                {
                    if (await _roleManager.CreateRole(model.RoleName, model.Description))
                    {
                        var role = _context.Roles.First(r => r.Name == model.RoleName);
                        string[] privileges = Privilege.Split(',');
                        foreach (var item in privileges)
                        {
                            _context.RolePrivileges.Add(new RolePrivilege { RoleId = role.Id, PrivilegeId = item });
                        }
                        _context.SaveChanges();
                    }
                    return new OperationStatusResponse { Message = "Role successfully inserted.", Status = OperationStatus.SUCCESS };
                }
            }
            catch (Exception ex)
            {
                return new OperationStatusResponse { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
            }
        }
        public async Task<OperationStatusResponse> DeleteAsync(string name)
        {
            try
            {
                var role =await _context.Roles.FirstOrDefaultAsync(r => r.Name == name);
                var result = await GetUserRoleByIdAsync(role.Id);
                if (result != null && result.Count() > 0)
                {
                    return new OperationStatusResponse { Message = "You cannot delete this role. It is given to some users.", Status = OperationStatus.ERROR };
                }
                var privilege = _context.RolePrivileges.Where(p => p.RoleId == role.Id);

                foreach (var item in privilege)
                    _context.RolePrivileges.Remove(item);

                await _userManager.DeleteRole(role.Id);
                
                return new OperationStatusResponse { Message = "Role has been deleted successfuly.", Status = OperationStatus.SUCCESS };
            }
            catch (Exception ex)
            {
                return new OperationStatusResponse { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
            }
        }
        public async Task<List<Role>> GetAllAsync()
        {
            try
            {
                var rollesResponses = await _context.Roles.Where(x => x.RecordStatus == RecordStatus.Active).ToListAsync();
                return rollesResponses;
            }
            catch (Exception ex)
            {
                return new List<Role>();
            }
        }
        public async Task<List<IdentityUserRole<string>>> GetUserRoleByIdAsync(string id)
        {
            try
            {
                var userRollesResponses = await _context.UserRoles.Where(r => r.UserId == id).ToListAsync();
                return userRollesResponses;
            }
            catch (Exception ex)
            {
                return new List<IdentityUserRole<string>>();
            }
        }
        public async Task<Role> GetByIdAsync(string id)
        {
            try
            {
                var User = await _context.Roles.Where(x => x.Id == id && x.RecordStatus == RecordStatus.Active).ToListAsync();
                return User.FirstOrDefault();
            }
            catch (Exception ex)
            {
                return new Role();
            }
        }
        public async Task<OperationStatusResponse> UpdateAsync(Role request)
        {
            var id = Convert.ToUInt32(request.Id);
            var Roles = await _context.Roles.FindAsync(id);
            if (Roles == null)
                return new OperationStatusResponse() { Status = OperationStatus.ERROR, Message = "Record Does Not Exist" };
            try
            {

                Roles.UpdatedBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                Roles.LastUpdateDate = DateTime.UtcNow;
                _context.Roles.Update(Roles);
                _context.SaveChanges();
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
        public async Task<Role> GetByRoleNameAsync(string roleName)
        {
            try
            {
                var User = await _context.Roles.Where(x => x.Name == roleName && x.RecordStatus == RecordStatus.Active).ToListAsync();
                return User.FirstOrDefault();
            }
            catch (Exception ex)
            {
                return new Role();
            }
        }
        public async Task<OperationStatusResponse> UpdateRoleWithPrivilegeAsync(RoleViewModel model,string oldRole, string privilege)
        {
            if (privilege != null && oldRole != null)
            {
                var role =await _context.Roles.FirstOrDefaultAsync(x => x.Name == oldRole.ToString());
                if (role != null)
                {
                    List<RolePrivilege> privileges = _context.RolePrivileges.Where(x => x.RoleId == role.Id).ToList();
                    _context.RolePrivileges.RemoveRange(privileges);
                    string[] roles = privilege.Split(',');
                    foreach (var item in roles)
                    {
                        RolePrivilege pri = new RolePrivilege();
                        pri.PrivilegeId = item;
                        pri.RoleId = role.Id;
                        _context.RolePrivileges.Add(pri);
                    }
                    _context.SaveChanges();

                    if (!string.IsNullOrEmpty(model.Description) && !string.Equals(model.Description, role.Description, System.StringComparison.OrdinalIgnoreCase))
                    {
                        role.Description = model.Description;
                        _context.Entry(role).State = EntityState.Modified;
                        _context.SaveChanges();
                    }

                    if (model.RoleName != oldRole)
                    {
                        role.Name = model.RoleName;
                        _context.Entry(role).State = EntityState.Modified;
                        _context.SaveChanges();
                    }
                    return new OperationStatusResponse
                    {
                        Message = "Role successfully updated.",
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
    }
}
