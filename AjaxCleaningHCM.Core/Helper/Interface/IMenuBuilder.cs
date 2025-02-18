using AjaxCleaningHCM.Domain.DTO.MasterData.Response;
using AjaxCleaningHCM.Domain.Models.Helper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AjaxCleaningHCM.Core.Helper.Interface
{
    public interface IMenuBuilder
    {
        Task<MenuBuilderResponseDtos> GetAllAsync();
        Task<MenuBuilderResponseDto> GetByIdAsync(long id);
        Task<MenuBuilderResponseDto> CreateAsync(MenuBuilder request);
        Task<MenuBuilderResponseDto> UpdateAsync(MenuBuilder request);
        Task<OperationStatusResponse> DeleteAsync(long id);
        string MenuBuilderTree();
    }
}
