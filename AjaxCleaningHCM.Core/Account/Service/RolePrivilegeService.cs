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

namespace AjaxCleaningHCM.Core.Account.Service
{
    public class RolePrivilegeService : IRolePrivilege
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        public RolePrivilegeService(ApplicationDbContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _configuration = configuration;
        }
        public async Task<OperationStatusResponse> CreateAsync(RolePrivilege request)
        {
            try
            {
                request.StartDate = DateTime.Now;
                request.EndDate = DateTime.MaxValue;
                request.RegisteredDate = DateTime.Now;
                request.RegisteredBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
  
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
                //var RolePrivilege = await _context.RolePrivileges.FirstOrDefaultAsync(u => u.Id == id);

                //if (RolePrivilege == null)
                //    return new OperationStatusResponse { Message = "Record Does Not Exist", Status = OperationStatus.ERROR };
                //RolePrivilege.RecordStatus = RecordStatus.Deleted;
                //_context.RolePrivileges.Update(RolePrivilege);
                //_context.SaveChanges();
                return new OperationStatusResponse { Message = "Operation Successfully Completed", Status = OperationStatus.SUCCESS };
            }
            catch (Exception ex)
            {
                return new OperationStatusResponse { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
            }
        }
        public async Task<List<RolePrivilege>> GetAllAsync()
        {
            try
            {
                var privilegResponses = await _context.RolePrivileges.Where(x => x.RecordStatus == RecordStatus.Active).ToListAsync();
                return privilegResponses;
            }
            catch (Exception ex)
            {
                return new List<RolePrivilege>();
            }
        }
        public async Task<List<RolePrivilege>> GetByRoleIdAsync(string Id)
        {
            try
            {
                var rolePrivileges = await _context.RolePrivileges.Where(x => x.RoleId == Id).ToListAsync();
                return rolePrivileges;
            }
            catch (Exception ex)
            {
                return new List<RolePrivilege>();
            }
        }
        public async Task<RolePrivilege> GetByPrivilegeIdAsync(string Id)
        {
            try
            {
                //var id = Convert.ToUInt32(id);

                //var User = await _context.RolePrivileges.Where(x => x.Id == id && x.RecordStatus == RecordStatus.Active).ToListAsync();
                //return User.FirstOrDefault();

                return new RolePrivilege();
            }
            catch (Exception ex)
            {
                return new RolePrivilege();
            }
        }
        public async Task<OperationStatusResponse> UpdateAsync(RolePrivilege request)
        {
           //// var id = Convert.ToUInt32(request.Id);
           // var RolePrivileges = await _context.RolePrivileges.FindAsync(id);
           // if (RolePrivileges == null)
           //     return new OperationStatusResponse() { Status = OperationStatus.ERROR, Message = "Record Does Not Exist" };
            try
            {

                //RolePrivileges.UpdatedBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                //RolePrivileges.LastUpdateDate = DateTime.UtcNow;
                //_context.RolePrivileges.Update(RolePrivileges);
                //_context.SaveChanges();
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
    }
}
