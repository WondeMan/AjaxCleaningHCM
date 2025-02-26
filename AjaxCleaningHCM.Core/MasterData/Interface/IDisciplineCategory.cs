using AjaxCleaningHCM.Domain.DTO.MasterData.Response;
using AjaxCleaningHCM.Domain.Models.Helper;
using AjaxCleaningHCM.Domain.Models.MasterData;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AjaxCleaningHCM.Core.MasterData.Interface
{
    public interface IDisciplineCategory
    {
        Task<DisciplineCategoryResponseDtos> GetAllAsync();
        Task<DisciplineCategoryResponseDto> GetByIdAsync(long id);
        Task<DisciplineCategoryResponseDto> CreateAsync(DisciplineCategory request);
        Task<DisciplineCategoryResponseDto> UpdateAsync(DisciplineCategory request);
        Task<OperationStatusResponse> DeleteAsync(long id);
    }
}
