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
    public class LeaveTypeService : ILeaveType
    {
        private readonly IRepositoryBase<LeaveType> _LeaveTypeRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public LeaveTypeService(IRepositoryBase<LeaveType> LeaveTypeRepository, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _LeaveTypeRepository = LeaveTypeRepository;
        }
        public async Task<LeaveTypeResponseDto> CreateAsync(LeaveType request)
        {
            try
            {
                if (_LeaveTypeRepository.ExistWhere(x => x.Name == request.Name && x.RecordStatus == RecordStatus.Active))
                    return new LeaveTypeResponseDto { Message = "Record already exist with the same name", Status = OperationStatus.ERROR };
                request.StartDate = DateTime.Now;
                request.EndDate = DateTime.MaxValue;
                request.RegisteredDate = DateTime.Now;
                request.RegisteredBy = _httpContextAccessor.HttpContext.User.Identity.Name;
                request.RecordStatus = RecordStatus.Active;
                request.Remark = "test";
                request.IsReadOnly = false;

                if (await _LeaveTypeRepository.AddAsync(request))
                {
                    return new LeaveTypeResponseDto { Message = "Operation Successfully Completed", Status = OperationStatus.SUCCESS };
                }
                else
                {
                    return new LeaveTypeResponseDto { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
                }
            }
            catch (Exception ex)
            {
                return new LeaveTypeResponseDto { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
            }
        }
        public async Task<OperationStatusResponse> DeleteAsync(long id)
        {
            try
            {
                var LeaveType = await _LeaveTypeRepository.FirstOrDefaultAsync(u => u.Id == id);

                if (LeaveType == null)
                    return new OperationStatusResponse { Message = "Record Does Not Exist", Status = OperationStatus.ERROR };

                LeaveType.RecordStatus = RecordStatus.Deleted;
                if (await _LeaveTypeRepository.UpdateAsync(LeaveType))
                    return new OperationStatusResponse { Message = "Operation Successfully Completed", Status = OperationStatus.SUCCESS };
                else
                    return new LeaveTypeResponseDto { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
            }
            catch (Exception ex)
            {
                return new LeaveTypeResponseDto { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
            }
        }
        public async Task<LeaveTypeResponseDtos> GetAllAsync()
        {
            try
            {
                var result = new LeaveTypeResponseDtos();
                var LeaveTypeResponses = await _LeaveTypeRepository.WhereAsync(x => x.RecordStatus == RecordStatus.Active);
                var LeaveTypeDTOs = new List<LeaveTypeResponseDtos>();
                result.Status = OperationStatus.SUCCESS;
                result.Message = "Operation Successfully Completed";
                result.LeaveTypeDtos = LeaveTypeResponses.ToList();
                return result;
            }
            catch (Exception ex)
            {
                return new LeaveTypeResponseDtos { LeaveTypeDtos = new List<LeaveType>(), Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
            }
        }
        public async Task<LeaveTypeResponseDto> GetByIdAsync(long id)
        {
            try
            {
                var result = new LeaveTypeResponseDto();
                var LeaveType = await _LeaveTypeRepository.WhereAsync(x => x.Id == id && x.RecordStatus == RecordStatus.Active);
                if (LeaveType == null)
                    return new LeaveTypeResponseDto { Status = OperationStatus.ERROR, Message = "Record Does Not Exist" };
                result.LeaveTypeDto = LeaveType.FirstOrDefault();
                result.Status = OperationStatus.SUCCESS;
                result.Message = "Operation Successfully Completed";
                return result;
            }
            catch (Exception ex)
            {
                return new LeaveTypeResponseDto { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
            }
        }
        public async Task<LeaveTypeResponseDto> UpdateAsync(LeaveType request)
        {
            var LeaveType = await _LeaveTypeRepository.FindAsync(request.Id);
            if (LeaveType == null)
                return new LeaveTypeResponseDto() { Status = OperationStatus.ERROR, Message = "Record Does Not Exist" };
            if ((LeaveType.Name != request.Name) && _LeaveTypeRepository.ExistWhere(x => x.Name == request.Name && x.RecordStatus == RecordStatus.Active))
                return new LeaveTypeResponseDto { Message = "Record already exist with the same name", Status = OperationStatus.ERROR };

            try
            {
                LeaveType.Name = request.Name;
                LeaveType.Code = request.Code;
                LeaveType.NumberOfDay = request.NumberOfDay;
                LeaveType.LeaveDayType = request.LeaveDayType;
                LeaveType.UpdatedBy = _httpContextAccessor.HttpContext.User.Identity.Name;
                LeaveType.LastUpdateDate = DateTime.UtcNow;

                if (await _LeaveTypeRepository.UpdateAsync(LeaveType))
                {
                    return new LeaveTypeResponseDto
                    {
                        Message = "Operation Successfully Completed",
                        Status = OperationStatus.SUCCESS
                    };
                }
                else
                {
                    return new LeaveTypeResponseDto
                    {
                        Message = "Error Has Occurred While Processing Your Request",
                        Status = OperationStatus.ERROR
                    };
                }
            }
            catch (Exception ex)
            {
                return new LeaveTypeResponseDto
                {
                    Message = "Error Has Occurred While Processing Your Request",
                    Status = OperationStatus.ERROR
                };
            }
        }
    }
}
