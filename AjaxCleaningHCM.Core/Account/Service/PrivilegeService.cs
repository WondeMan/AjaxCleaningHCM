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
using AjaxCleaningHCM.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace AjaxCleaningHCM.Core.Account.Service
{
    public class PrivilegeService : IPrivilege
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        public PrivilegeService(ApplicationDbContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _configuration = configuration;
        }
        public async Task<OperationStatusResponse> CreateAsync(Privilege request)
        {
            try
            {
                request.StartDate = DateTime.Now;
                request.EndDate = DateTime.MaxValue;
                request.RegisteredDate = DateTime.Now;
                request.RegisteredBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                var privilege  =await _context.Privileges.FirstOrDefaultAsync(i => i.Action.Equals(request.Action));
                if (privilege == null)
                {
                    request.Id = Guid.NewGuid().ToString();
                    _context.Privileges.Add(request);
                    _context.SaveChanges();
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
                var privilege = await _context.Privileges.FirstOrDefaultAsync(u => u.Id == id);

                if (privilege == null)
                    return new OperationStatusResponse { Message = "Record Does Not Exist", Status = OperationStatus.ERROR };
                _context.Privileges.Remove(privilege);
                _context.SaveChanges();
                return new OperationStatusResponse { Message = "Operation Successfully Completed", Status = OperationStatus.SUCCESS };
            }
            catch (Exception ex)
            {
                return new OperationStatusResponse { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
            }
        }
        public async Task<List<Privilege>> GetAllAsync()
        {
            try
            {
                var privilegResponses = await _context.Privileges.ToListAsync();
                return privilegResponses;
            }
            catch (Exception ex)
            {
                return new List<Privilege>();
            }
        }
        public async Task<Privilege> GetByIdAsync(string id)
        {
            try
            {
                var User = await _context.Privileges.Where(x => x.Id == id).ToListAsync();
                return User.FirstOrDefault();
            }
            catch (Exception ex)
            {
                return new Privilege();
            }
        }
        public async Task<OperationStatusResponse> UpdateAsync(Privilege request)
        {
            var privileges = await _context.Privileges.FindAsync(request.Id);
            if (privileges == null)
                return new OperationStatusResponse() { Status = OperationStatus.ERROR, Message = "Record Does Not Exist" };
            try
            {
                privileges.Action = request.Action;
                privileges.Description = request.Description;
                privileges.UpdatedBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                privileges.LastUpdateDate = DateTime.UtcNow;
                _context.Privileges.Update(privileges);
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
        public UserRolePrivilege GetUserRolesPrivileges()
        {
            List<UserRole> Roles = new List<UserRole>();
            string username = _httpContextAccessor.HttpContext.User.Identity.Name.ToLower();
            var user = _context.Users.Where(u => u.UserName == username).FirstOrDefault();
            if (user != null)
            {
                var userRoles = _context.UserRoles.Where(ur => ur.UserId == user.Id).ToList();
                foreach (var role in userRoles)
                {
                    Roles.Add(new UserRole { RoleId = role.RoleId });
                }
            }
            var rolePrivilegelist = _context.RolePrivileges.ToList();
            var privileges = _context.Privileges.ToList();
            UserRolePrivilege userRolePrivilege = new UserRolePrivilege()
            {
                UserRoles = Roles,
                Privileges = privileges,
                RolePrivileges = rolePrivilegelist
            };
            return userRolePrivilege;
        }

    }
}
