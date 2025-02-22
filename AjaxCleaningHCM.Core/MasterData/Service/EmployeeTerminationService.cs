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
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AjaxCleaningHCM.Core.MasterData.Service
{

    public class EmployeeTerminationService : IEmployeeTermination
    {
        private readonly IRepositoryBase<EmployeeTermination> _EmployeeTerminationRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmployee _employee;
        public EmployeeTerminationService(IRepositoryBase<EmployeeTermination> EmployeeTerminationRepository,IEmployee employee, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _EmployeeTerminationRepository = EmployeeTerminationRepository;
            _employee = employee;
        }
        public async Task<EmployeeTerminationResponseDto> CreateAsync(EmployeeTermination request)
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

                if (await _EmployeeTerminationRepository.AddAsync(request))
                {
                    _employee.Remove(request.EmployeeId);
                    return new EmployeeTerminationResponseDto { Message = "Operation Successfully Completed", Status = OperationStatus.SUCCESS };
                }
                else
                {
                    return new EmployeeTerminationResponseDto { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
                }
            }
            catch (Exception ex)
            {
                return new EmployeeTerminationResponseDto { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
            }
        }
        public async Task<OperationStatusResponse> DeleteAsync(long id)
        {
            try
            {
                var EmployeeTermination = await _EmployeeTerminationRepository.FirstOrDefaultAsync(u => u.Id == id);

                if (EmployeeTermination == null)
                    return new OperationStatusResponse { Message = "Record Does Not Exist", Status = OperationStatus.ERROR };

                EmployeeTermination.RecordStatus = RecordStatus.Deleted;
                if (await _EmployeeTerminationRepository.UpdateAsync(EmployeeTermination))
                    return new OperationStatusResponse { Message = "Operation Successfully Completed", Status = OperationStatus.SUCCESS };
                else
                    return new EmployeeTerminationResponseDto { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
            }
            catch (Exception ex)
            {
                return new EmployeeTerminationResponseDto { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
            }
        }
        public async Task<EmployeeTerminationResponseDtos> GetAllAsync()
        {
            try
            {
                var result = new EmployeeTerminationResponseDtos();
                var EmployeeTerminationResponses = await _EmployeeTerminationRepository.WhereAsync(x => x.RecordStatus == RecordStatus.Active,"Employee");
                var EmployeeTerminationDTOs = new List<EmployeeTerminationResponseDtos>();
                result.Status = OperationStatus.SUCCESS;
                result.Message = "Operation Successfully Completed";
                result.EmployeeTerminationDtos = EmployeeTerminationResponses.ToList();
                return result;
            }
            catch (Exception ex)
            {
                return new EmployeeTerminationResponseDtos { EmployeeTerminationDtos = new List<EmployeeTermination>(), Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
            }
        }
        public async Task<EmployeeTerminationResponseDto> GetByIdAsync(long id)
        {
            try
            {
                var result = new EmployeeTerminationResponseDto();
                var EmployeeTermination = await _EmployeeTerminationRepository.WhereAsync(x => x.Id == id && x.RecordStatus == RecordStatus.Active, "Employee");
                if (EmployeeTermination == null)
                    return new EmployeeTerminationResponseDto { Status = OperationStatus.ERROR, Message = "Record Does Not Exist" };
                result.EmployeeTerminationDto = EmployeeTermination.FirstOrDefault();
                result.Status = OperationStatus.SUCCESS;
                result.Message = "Operation Successfully Completed";
                return result;
            }
            catch (Exception ex)
            {
                return new EmployeeTerminationResponseDto { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
            }
        }
        public async Task<EmployeeTerminationResponseDto> UpdateAsync(EmployeeTermination request)
        {
            var result = await _EmployeeTerminationRepository.FindAsync(request.Id);
            if (result == null)
                return new EmployeeTerminationResponseDto() { Status = OperationStatus.ERROR, Message = "Record Does Not Exist" };

            try
            {
                result.TerminationDate = request.TerminationDate;
                result.TerminationReason = request.TerminationReason;
                result.LetterType = request.LetterType;
                result.Letter = request.Letter;

                if (result.EmployeeId != request.EmployeeId)
                {
                    _employee.ActivateEmployee(result.EmployeeId);
                    _employee.Remove(request.EmployeeId);
                }
                result.EmployeeId = request.EmployeeId;
                result.Remark = request.Remark;
                result.LastUpdateDate = DateTime.Now;

                if (await _EmployeeTerminationRepository.UpdateAsync(result))
                {
                    return new EmployeeTerminationResponseDto
                    {
                        Message = "Operation Successfully Completed",
                        Status = OperationStatus.SUCCESS
                    };
                }
                else
                {
                    return new EmployeeTerminationResponseDto
                    {
                        Message = "Error Has Occurred While Processing Your Request",
                        Status = OperationStatus.ERROR
                    };
                }
            }
            catch (Exception ex)
            {
                return new EmployeeTerminationResponseDto
                {
                    Message = "Error Has Occurred While Processing Your Request",
                    Status = OperationStatus.ERROR
                };
            }
        }
        public async Task<EmployeeTerminationResponseDtos> Search(SearchRequest request)
        {
            try
            {
                if (request.Start == DateTime.MinValue || request.Start == null)
                {
                    request.Start = DateTime.Now;
                }
                if (request.End == DateTime.MinValue || request.End == null)
                {
                    request.End = DateTime.Now;
                }
                List<EmployeeTermination> employeeTerminations = new List<EmployeeTermination>();
                if (request.SearchByTerminationReason == 0)
                {
                    employeeTerminations = await _EmployeeTerminationRepository.Where(c => c.RecordStatus == RecordStatus.Active && c.RegisteredDate >= request.Start && c.RegisteredDate <= request.End).Include(a => a.Employee).ThenInclude(a => a.EmployeeBranches).ToListAsync();
                }
                else
                {
                    employeeTerminations = await _EmployeeTerminationRepository.Where(c => c.RecordStatus == RecordStatus.Active && c.TerminationReason == request.SearchByTerminationReason && c.RegisteredDate >= request.Start && c.RegisteredDate <= request.End).Include(a => a.Employee).ThenInclude(a => a.EmployeeBranches).ToListAsync();
                }
                return new EmployeeTerminationResponseDtos()
                {
                    EmployeeTerminationDtos = employeeTerminations,
                    Status = OperationStatus.SUCCESS,
                };
            }
            catch (Exception ex)
            {
                return new EmployeeTerminationResponseDtos()
                {
                    EmployeeTerminationDtos = new List<EmployeeTermination>(),
                    Status = OperationStatus.ERROR,
                };
            }
        }
        public async Task<EmployeeTerminationResponseDto> GetByEmplyeeId(long id)
        {
            try
            {
                return new EmployeeTerminationResponseDto()
                {
                    EmployeeTerminationDto =await _EmployeeTerminationRepository.Where(a => a.EmployeeId == id).Include(a => a.Employee).FirstOrDefaultAsync(),
                    Status = OperationStatus.SUCCESS,
                };
            }
            catch (Exception ex)
            {
                return new EmployeeTerminationResponseDto()
                {
                    EmployeeTerminationDto = new EmployeeTermination(),
                    Status = OperationStatus.SUCCESS,
                };
            }
        }

    }
}
