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
using Microsoft.EntityFrameworkCore;

namespace AjaxCleaningHCM.Core.MasterData.Service
{
    public class EmployeeDisciplineService : IEmployeeDiscipline
    {
        private readonly IRepositoryBase<EmployeeDiscipline> _EmployeeDisciplineRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public EmployeeDisciplineService(IRepositoryBase<EmployeeDiscipline> EmployeeDisciplineRepository, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _EmployeeDisciplineRepository = EmployeeDisciplineRepository;
        }
        public async Task<EmployeeDisciplineResponseDto> CreateAsync(EmployeeDiscipline request)
        {
            try
            {
                request.StartDate = DateTime.Now;
                request.EndDate = DateTime.MaxValue;
                request.RegisteredDate = DateTime.Now;
                request.RegisteredBy = _httpContextAccessor.HttpContext.User.Identity.Name;
                request.RecordStatus = RecordStatus.Active;
                request.Remark = "test";
                request.IsReadOnly = false;
                request.ActionTakenDate = DateTime.Now;
                if (await _EmployeeDisciplineRepository.AddAsync(request))
                {
                    return new EmployeeDisciplineResponseDto { Message = "Operation Successfully Completed", Status = OperationStatus.SUCCESS };
                }
                else
                {
                    return new EmployeeDisciplineResponseDto { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
                }
            }
            catch (Exception ex)
            {
                return new EmployeeDisciplineResponseDto { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
            }
        }
        public async Task<OperationStatusResponse> DeleteAsync(long id)
        {
            try
            {
                var EmployeeDiscipline = await _EmployeeDisciplineRepository.FirstOrDefaultAsync(u => u.Id == id);

                if (EmployeeDiscipline == null)
                    return new OperationStatusResponse { Message = "Record Does Not Exist", Status = OperationStatus.ERROR };

                EmployeeDiscipline.RecordStatus = RecordStatus.Deleted;
                if (await _EmployeeDisciplineRepository.UpdateAsync(EmployeeDiscipline))
                    return new OperationStatusResponse { Message = "Operation Successfully Completed", Status = OperationStatus.SUCCESS };
                else
                    return new EmployeeDisciplineResponseDto { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
            }
            catch (Exception ex)
            {
                return new EmployeeDisciplineResponseDto { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
            }
        }
        public async Task<EmployeeDisciplineResponseDtos> GetAllAsync()
        {
            try
            {
                var result = new EmployeeDisciplineResponseDtos();
                var EmployeeDisciplineResponses = await _EmployeeDisciplineRepository.Where(x => x.RecordStatus == RecordStatus.Active).Include(a=>a.Employee).Include(a=>a.DisciplineCategory).ToListAsync();
                var EmployeeDisciplineDTOs = new List<EmployeeDisciplineResponseDtos>();
                result.Status = OperationStatus.SUCCESS;
                result.Message = "Operation Successfully Completed";
                result.EmployeeDisciplineDtos = EmployeeDisciplineResponses.ToList();
                return result;
            }
            catch (Exception ex)
            {
                return new EmployeeDisciplineResponseDtos { EmployeeDisciplineDtos = new List<EmployeeDiscipline>(), Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
            }
        }
        public async Task<EmployeeDisciplineResponseDto> GetByIdAsync(long id)
        {
            try
            {
                var result = new EmployeeDisciplineResponseDto();
                var EmployeeDiscipline = await _EmployeeDisciplineRepository.Where(x => x.Id == id && x.RecordStatus == RecordStatus.Active).Include(a => a.Employee).Include(a => a.DisciplineCategory).FirstOrDefaultAsync();
                if (EmployeeDiscipline == null)
                    return new EmployeeDisciplineResponseDto { Status = OperationStatus.ERROR, Message = "Record Does Not Exist" };
                result.EmployeeDisciplineDto = EmployeeDiscipline;
                result.Status = OperationStatus.SUCCESS;
                result.Message = "Operation Successfully Completed";
                return result;
            }
            catch (Exception ex)
            {
                return new EmployeeDisciplineResponseDto { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
            }
        }
        public async Task<EmployeeDisciplineResponseDto> UpdateAsync(EmployeeDiscipline request)
        {
            var EmployeeDiscipline = await _EmployeeDisciplineRepository.FindAsync(request.Id);
            if (EmployeeDiscipline == null)
                return new EmployeeDisciplineResponseDto() { Status = OperationStatus.ERROR, Message = "Record Does Not Exist" };
            try
            {
                EmployeeDiscipline.EmployeeId = request.EmployeeId;
                EmployeeDiscipline.DisciplineCategoryId = request.DisciplineCategoryId;
                EmployeeDiscipline.ActionTaken = request.ActionTaken;
                EmployeeDiscipline.Description = request.Description;
                EmployeeDiscipline.UpdatedBy = _httpContextAccessor.HttpContext.User.Identity.Name;
                EmployeeDiscipline.LastUpdateDate = DateTime.UtcNow;

                if (await _EmployeeDisciplineRepository.UpdateAsync(EmployeeDiscipline))
                {
                    return new EmployeeDisciplineResponseDto
                    {
                        Message = "Operation Successfully Completed",
                        Status = OperationStatus.SUCCESS
                    };
                }
                else
                {
                    return new EmployeeDisciplineResponseDto
                    {
                        Message = "Error Has Occurred While Processing Your Request",
                        Status = OperationStatus.ERROR
                    };
                }
            }
            catch (Exception ex)
            {
                return new EmployeeDisciplineResponseDto
                {
                    Message = "Error Has Occurred While Processing Your Request",
                    Status = OperationStatus.ERROR
                };
            }
        }
    }
}
