using AjaxCleaningHCM.Core.Helper;
using AjaxCleaningHCM.Core.MasterData.Interface;
using AjaxCleaningHCM.Domain.DTO.MasterData.Response;
using AjaxCleaningHCM.Domain.Models.Helper;
using AjaxCleaningHCM.Domain.Models.MasterData;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using static AjaxCleaningHCM.Domain.Enums.Common;
using System.Threading.Tasks;
using System.Linq;

namespace AjaxCleaningHCM.Core.MasterData.Service
{
    public class DisciplineCategoryService : IDisciplineCategory
    {
        private readonly IRepositoryBase<DisciplineCategory> _DisciplineCategoryRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public DisciplineCategoryService(IRepositoryBase<DisciplineCategory> DisciplineCategoryRepository, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _DisciplineCategoryRepository = DisciplineCategoryRepository;
        }
        public async Task<DisciplineCategoryResponseDto> CreateAsync(DisciplineCategory request)
        {
            try
            {
                if (_DisciplineCategoryRepository.ExistWhere(x => x.Name == request.Name && x.RecordStatus == RecordStatus.Active))
                    return new DisciplineCategoryResponseDto { Message = "Record already exist with the same name", Status = OperationStatus.ERROR };
                request.StartDate = DateTime.Now;
                request.EndDate = DateTime.MaxValue;
                request.RegisteredDate = DateTime.Now;
                request.RegisteredBy = _httpContextAccessor.HttpContext.User.Identity.Name;
                request.RecordStatus = RecordStatus.Active;
                request.Remark = "test";
                request.IsReadOnly = false;

                if (await _DisciplineCategoryRepository.AddAsync(request))
                {
                    return new DisciplineCategoryResponseDto { Message = "Operation Successfully Completed", Status = OperationStatus.SUCCESS };
                }
                else
                {
                    return new DisciplineCategoryResponseDto { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
                }
            }
            catch (Exception ex)
            {
                return new DisciplineCategoryResponseDto { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
            }
        }
        public async Task<OperationStatusResponse> DeleteAsync(long id)
        {
            try
            {
                var DisciplineCategory = await _DisciplineCategoryRepository.FirstOrDefaultAsync(u => u.Id == id);

                if (DisciplineCategory == null)
                    return new OperationStatusResponse { Message = "Record Does Not Exist", Status = OperationStatus.ERROR };

                DisciplineCategory.RecordStatus = RecordStatus.Deleted;
                if (await _DisciplineCategoryRepository.UpdateAsync(DisciplineCategory))
                    return new OperationStatusResponse { Message = "Operation Successfully Completed", Status = OperationStatus.SUCCESS };
                else
                    return new DisciplineCategoryResponseDto { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
            }
            catch (Exception ex)
            {
                return new DisciplineCategoryResponseDto { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
            }
        }
        public async Task<DisciplineCategoryResponseDtos> GetAllAsync()
        {
            try
            {
                var result = new DisciplineCategoryResponseDtos();
                var DisciplineCategoryResponses = await _DisciplineCategoryRepository.WhereAsync(x => x.RecordStatus == RecordStatus.Active);
                var DisciplineCategoryDTOs = new List<DisciplineCategoryResponseDtos>();
                result.Status = OperationStatus.SUCCESS;
                result.Message = "Operation Successfully Completed";
                result.DisciplineCategoryDtos = DisciplineCategoryResponses.ToList();
                return result;
            }
            catch (Exception ex)
            {
                return new DisciplineCategoryResponseDtos { DisciplineCategoryDtos = new List<DisciplineCategory>(), Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
            }
        }
        public async Task<DisciplineCategoryResponseDto> GetByIdAsync(long id)
        {
            try
            {
                var result = new DisciplineCategoryResponseDto();
                var DisciplineCategory = await _DisciplineCategoryRepository.WhereAsync(x => x.Id == id && x.RecordStatus == RecordStatus.Active);
                if (DisciplineCategory == null)
                    return new DisciplineCategoryResponseDto { Status = OperationStatus.ERROR, Message = "Record Does Not Exist" };
                result.DisciplineCategoryDto = DisciplineCategory.FirstOrDefault();
                result.Status = OperationStatus.SUCCESS;
                result.Message = "Operation Successfully Completed";
                return result;
            }
            catch (Exception ex)
            {
                return new DisciplineCategoryResponseDto { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
            }
        }
        public async Task<DisciplineCategoryResponseDto> UpdateAsync(DisciplineCategory request)
        {
            var DisciplineCategory = await _DisciplineCategoryRepository.FindAsync(request.Id);
            if (DisciplineCategory == null)
                return new DisciplineCategoryResponseDto() { Status = OperationStatus.ERROR, Message = "Record Does Not Exist" };
            if ((DisciplineCategory.Name != request.Name) && _DisciplineCategoryRepository.ExistWhere(x => x.Name == request.Name && x.RecordStatus == RecordStatus.Active))
                return new DisciplineCategoryResponseDto { Message = "Record already exist with the same name", Status = OperationStatus.ERROR };

            try
            {
                DisciplineCategory.Name = request.Name;
                DisciplineCategory.Code = request.Code;
                DisciplineCategory.Description = request.Description;
                DisciplineCategory.UpdatedBy = _httpContextAccessor.HttpContext.User.Identity.Name;
                DisciplineCategory.LastUpdateDate = DateTime.UtcNow;

                if (await _DisciplineCategoryRepository.UpdateAsync(DisciplineCategory))
                {
                    return new DisciplineCategoryResponseDto
                    {
                        Message = "Operation Successfully Completed",
                        Status = OperationStatus.SUCCESS
                    };
                }
                else
                {
                    return new DisciplineCategoryResponseDto
                    {
                        Message = "Error Has Occurred While Processing Your Request",
                        Status = OperationStatus.ERROR
                    };
                }
            }
            catch (Exception ex)
            {
                return new DisciplineCategoryResponseDto
                {
                    Message = "Error Has Occurred While Processing Your Request",
                    Status = OperationStatus.ERROR
                };
            }
        }
    }
}
