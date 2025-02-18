using AjaxCleaningHCM.Core.Account.Interface;
using AjaxCleaningHCM.Core.Helper.Interface;
using AjaxCleaningHCM.Core.UserManagment.Identity;
using AjaxCleaningHCM.Domain.DTO.MasterData.Response;
using AjaxCleaningHCM.Domain.Models.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using static AjaxCleaningHCM.Domain.Enums.Common;
using System.Threading.Tasks;

namespace AjaxCleaningHCM.Core.Helper.Service
{
    public class MenuBuilderService : IMenuBuilder
    {

        private readonly IRole _role;
        private readonly IPrivilege _privilege;
        private readonly IUserAccount _userAccount;
        private readonly IRepositoryBase<MenuBuilder> _MenuBuilderRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public IHostEnvironment _hostEnvironment;


        private UserRolePrivilege userRolePrivilege;
        public MenuBuilderService(IRepositoryBase<MenuBuilder> MenuBuilderRepository, IHostEnvironment hostEnvironment, IUserAccount userAccount, IRole role, IPrivilege privilege, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _MenuBuilderRepository = MenuBuilderRepository;
            _role = role;
            _privilege = privilege;
            _userAccount = userAccount;
            _hostEnvironment = hostEnvironment;
        }
        public async Task<MenuBuilderResponseDto> CreateAsync(MenuBuilder request)
        {
            try
            {
                if (_MenuBuilderRepository.ExistWhere(x => x.LinkText == request.LinkText && x.RecordStatus == RecordStatus.Active))
                    return new MenuBuilderResponseDto { Message = "Record already exist with the same name", Status = OperationStatus.ERROR };
                request.StartDate = DateTime.Now;
                request.EndDate = DateTime.MaxValue;
                request.RegisteredDate = DateTime.Now;
                request.RegisteredBy = _httpContextAccessor.HttpContext.User.Identity.Name;
                request.RecordStatus = RecordStatus.Active;
                request.IsReadOnly = false;

                if (await _MenuBuilderRepository.AddAsync(request))
                {
                    return new MenuBuilderResponseDto { Message = "Operation Successfully Completed", Status = OperationStatus.SUCCESS };
                }
                else
                {
                    return new MenuBuilderResponseDto { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
                }
            }
            catch (Exception ex)
            {
                return new MenuBuilderResponseDto { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
            }
        }
        public async Task<OperationStatusResponse> DeleteAsync(long id)
        {
            try
            {
                var MenuBuilder = await _MenuBuilderRepository.FirstOrDefaultAsync(u => u.Id == id);

                if (MenuBuilder == null)
                    return new OperationStatusResponse { Message = "Record Does Not Exist", Status = OperationStatus.ERROR };

                MenuBuilder.RecordStatus = RecordStatus.Deleted;
                if (await _MenuBuilderRepository.UpdateAsync(MenuBuilder))
                    return new OperationStatusResponse { Message = "Operation Successfully Completed", Status = OperationStatus.SUCCESS };
                else
                    return new MenuBuilderResponseDto { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
            }
            catch (Exception ex)
            {
                return new MenuBuilderResponseDto { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
            }
        }
        public async Task<MenuBuilderResponseDtos> GetAllAsync()
        {
            try
            {
                var result = new MenuBuilderResponseDtos();
                var MenuBuilderResponses = await _MenuBuilderRepository.WhereAsync(a => a.RecordStatus == RecordStatus.Active);
                var MenuBuilderDTOs = new List<MenuBuilderResponseDtos>();
                result.Status = OperationStatus.SUCCESS;
                result.Message = "Operation Successfully Completed";
                result.MenuBuilderDtos = MenuBuilderResponses.ToList();
                return result;
            }
            catch (Exception ex)
            {
                return new MenuBuilderResponseDtos { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
            }
        }
        public async Task<MenuBuilderResponseDto> GetByIdAsync(long id)
        {
            try
            {
                var result = new MenuBuilderResponseDto();
                var MenuBuilder = await _MenuBuilderRepository.WhereAsync(x => x.Id == id && x.RecordStatus == RecordStatus.Active);
                if (MenuBuilder == null)
                    return new MenuBuilderResponseDto { Status = OperationStatus.ERROR, Message = "Record Does Not Exist" };
                result.MenuBuilderDto = MenuBuilder.FirstOrDefault();
                result.Status = OperationStatus.SUCCESS;
                result.Message = "Operation Successfully Completed";
                return result;
            }
            catch (Exception ex)
            {
                return new MenuBuilderResponseDto { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
            }
        }
        public async Task<MenuBuilderResponseDto> UpdateAsync(MenuBuilder request)
        {
            var MenuBuilder = await _MenuBuilderRepository.FindAsync(request.Id);
            if (MenuBuilder == null)
                return new MenuBuilderResponseDto() { Status = OperationStatus.ERROR, Message = "Record Does Not Exist" };
            if ((MenuBuilder.LinkText != request.LinkText) && _MenuBuilderRepository.ExistWhere(x => x.LinkText == request.LinkText && x.RecordStatus == RecordStatus.Active))
                return new MenuBuilderResponseDto { Message = "Record already exist with the same name", Status = OperationStatus.ERROR };

            try
            {
                MenuBuilder.LinkText = request.LinkText;
                MenuBuilder.LinkUrl = request.LinkUrl;
                MenuBuilder.ParentId = request.ParentId;
                MenuBuilder.Icon = request.Icon;
                MenuBuilder.Order = request.Order;
                MenuBuilder.UpdatedBy = _httpContextAccessor.HttpContext.User.Identity.Name;
                MenuBuilder.LastUpdateDate = DateTime.UtcNow;

                if (await _MenuBuilderRepository.UpdateAsync(MenuBuilder))
                {
                    return new MenuBuilderResponseDto
                    {
                        Message = "Operation Successfully Completed",
                        Status = OperationStatus.SUCCESS
                    };
                }
                else
                {
                    return new MenuBuilderResponseDto
                    {
                        Message = "Error Has Occurred While Processing Your Request",
                        Status = OperationStatus.ERROR
                    };
                }
            }
            catch (Exception ex)
            {
                return new MenuBuilderResponseDto
                {
                    Message = "Error Has Occurred While Processing Your Request",
                    Status = OperationStatus.ERROR
                };
            }
        }
        public string MenuBuilderTree()
        {
            try
            {
                //SaveDataFromExcelFile();
                userRolePrivilege = _privilege.GetUserRolesPrivileges();

                var MenuBuilder = _MenuBuilderRepository.Where(a => a.RecordStatus == RecordStatus.Active).OrderBy(a => a.Order).ToList();
                var rootNodes = MenuBuilder.Where(x => x.ParentId == null).ToList();
                var html = "";
                foreach (var rootNode in rootNodes)
                {
                    html += "<li class=\"nav-item\">";
                    html += "<a href=\"" + rootNode.LinkUrl + "\" class=\"nav-link\">";
                    html += "<i class=\"" + rootNode.Icon + "\"></i>";
                    html += "<p>" + rootNode.LinkText + "<i class=\"right fas fa-angle-left\"></i></p>";
                    html += "</a>";
                    // Add children recursively
                    html += GetChildrenMenu(rootNode.Id, MenuBuilder);
                    html += "</li>";
                }

                return html;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private string GetChildrenMenu(long MenuBuilderId, List<MenuBuilder> MenuBuilders)
        {
            var childrenNodes = MenuBuilders.Where(x => x.ParentId == MenuBuilderId).ToList();

            if (childrenNodes.Count == 0)
            {
                return ""; // Return empty string if no children
            }

            var html = "<ul class=\"nav nav-treeview pl-2 \">";

            foreach (var childNode in childrenNodes)
            {
                var childrenNodess = MenuBuilders.Where(x => x.ParentId == childNode.Id).ToList();
                if (!string.IsNullOrEmpty(childNode.LinkUrl))
                {
                    var controllerAction = childNode.LinkUrl.Trim().Split("/");
                    if (controllerAction.Length >= 3)
                    {
                        var requiredPrevilage = controllerAction[2] + "-" + controllerAction[3];
                        if (HasPrivilege(requiredPrevilage))
                        {
                            html += "<li class=\"nav-item \">";
                            html += "<a href=\"" + childNode.LinkUrl + "\" class=\"nav-link\">";
                            html += "<i class=\"" + childNode.Icon + "\"></i>";

                            if (childrenNodess.Count == 0)
                            {
                                html += "<p>" + childNode.LinkText + "</p>";
                            }
                            else
                            {
                                html += "<p>" + childNode.LinkText + "<i class=\"right fas fa-angle-left\"></i> </p>";
                            }
                            html += "</a>";
                            // Recursively add children

                        };
                    }

                }
                html += GetChildrenMenu(childNode.Id, MenuBuilders);
                html += "</li>";

            }

            html += "</ul>";
            return html;
        }
        public bool HasPrivilege(string requiredPrivilege)
        {
            bool found = false;
            foreach (UserRole userRole in userRolePrivilege.UserRoles)
            {
                List<RolePrivilege> rolePrivilege = userRolePrivilege.RolePrivileges.Where(r => r.RoleId == userRole.RoleId).ToList();
                foreach (var privilege in rolePrivilege)
                {
                    found = userRolePrivilege.Privileges.Where(p => p.Action.ToLower() == requiredPrivilege.ToLower() && privilege.PrivilegeId == p.Id).ToList().Count > 0;
                    if (found)
                        break;
                }
                if (found)
                    break;
            }
            return found;
        }
    }

}
