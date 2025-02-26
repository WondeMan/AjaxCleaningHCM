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
using Microsoft.AspNetCore.Mvc;

namespace AjaxCleaningHCM.Core.MasterData.Service
{
    public class LeaveRequestService : ILeaveRequest
    {
        private readonly IRepositoryBase<LeaveRequest> _LeaveRequestRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmployee _employee;
        private readonly ILeaveType _leaveType;
        public LeaveRequestService(IRepositoryBase<LeaveRequest> LeaveRequestRepository,IEmployee employee,ILeaveType leaveType, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _LeaveRequestRepository = LeaveRequestRepository;
            _employee = employee;
            _leaveType = leaveType;
        }
        public async Task<LeaveRequestResponseDto> CreateAsync(LeaveRequest request)
        {
            try
            {
                request.TotalLeaveDay = request.NumberOfHolidayDay + request.NumberOfWorkingDay;
                request.StartDate = DateTime.Now;
                request.EndDate = DateTime.MaxValue;
                request.RegisteredDate = DateTime.Now;
                request.RegisteredBy = _httpContextAccessor.HttpContext.User.Identity.Name;
                request.RecordStatus = RecordStatus.Active;
                request.Remark = "test";
                request.IsReadOnly = false;
                var eaveRequest =await _LeaveRequestRepository.Where(a => a.EmployeeId == request.EmployeeId && a.RecordStatus == RecordStatus.Active).Include(c => c.Employee)?.ToListAsync();
                var lastLeaveRequest = eaveRequest.LastOrDefault();
                var LeaveType =await _leaveType.GetByIdAsync(request.LeaveTypeId);
                // check if the employee has been request before if exist use last recored for calculation if not use employee record for calculation
                if (lastLeaveRequest != null)
                {
                    //add one day vaction per year
                    TimeSpan difference = DateTime.Now - lastLeaveRequest.EmployeeStartDate;
                    int years = difference.Days / 365;
                    int totalVacationDaysEarned = years * 1;
                    int totalVacationDaysNow = totalVacationDaysEarned + lastLeaveRequest.AllowedDay;
                    lastLeaveRequest.AllowedDay = totalVacationDaysNow;
                    // check if ther request is vacation if so deduct request vacation from allowed vaction
                    if (LeaveType.LeaveTypeDto.LeaveDayType == LeaveDayType.Vacation)
                    {
                        if (request.NumberOfWorkingDay <= lastLeaveRequest.RemainingDay)
                        {
                            request.AllowedDay = lastLeaveRequest.AllowedDay;
                            request.RemainingDay = lastLeaveRequest.RemainingDay - request.NumberOfWorkingDay;
                            request.EmployeeStartDate = lastLeaveRequest.EmployeeStartDate;
                        }
                        else
                        {
                            return new LeaveRequestResponseDto()
                            {
                                Status = OperationStatus.ERROR,
                                Message = "You are requesting the highest limit than expectd, you have only " + lastLeaveRequest.RemainingDay + " Days"
                            };
                        }
                    }
                    else if (LeaveType.LeaveTypeDto.LeaveDayType == LeaveDayType.Known && request.NumberOfWorkingDay > LeaveType.LeaveTypeDto.NumberOfDay)
                    {
                        return new LeaveRequestResponseDto()
                        {
                            Status = OperationStatus.ERROR,
                            Message = "You are requesting the highest limit than expectd, you have only " + LeaveType.LeaveTypeDto.NumberOfDay + " Days"
                        };
                    }
                    else
                    {
                        request.AllowedDay = lastLeaveRequest.AllowedDay;
                        request.RemainingDay = lastLeaveRequest.RemainingDay;
                        request.EmployeeStartDate = lastLeaveRequest.EmployeeStartDate;
                    }
                }
                else
                {
                    var employeeResult =await _employee.GetByIdAsync(request.EmployeeId);
                    var employee = employeeResult.EmployeeDto;
                    TimeSpan difference = DateTime.Now - employee.EmploymentStartDate;
                    int years = difference.Days / 365;
                    int totalVacationDaysEarned = years * 1;
                    int totalVacationDaysNow = totalVacationDaysEarned + employee.NumberOfVacation;
                    employee.NumberOfVacation = totalVacationDaysNow;

                    if (LeaveType.LeaveTypeDto.LeaveDayType == LeaveDayType.Vacation)
                    {
                        if (request.NumberOfWorkingDay <= employee.NumberOfVacation)
                        {
                            request.AllowedDay = employee.NumberOfVacation;
                            request.RemainingDay = employee.NumberOfVacation - request.NumberOfWorkingDay;
                            request.EmployeeStartDate = employee.EmploymentStartDate;
                            // LeaveRequest.LeaveTypeId = null;

                        }
                        else
                        {
                            return new LeaveRequestResponseDto()
                            {
                                Status = OperationStatus.ERROR,
                                Message = "You are requesting the highest limit than expectd, you have only " + employee.NumberOfVacation + " Days"
                            };
                        }
                    }
                    else if (LeaveType.LeaveTypeDto.LeaveDayType == LeaveDayType.Known && request.NumberOfWorkingDay > LeaveType.LeaveTypeDto.NumberOfDay)
                    {
                        return new LeaveRequestResponseDto()
                        {
                            Status = OperationStatus.ERROR,
                            Message = "You are requesting the highest limit than expectd, you have only " + LeaveType.LeaveTypeDto.NumberOfDay + " Days"
                        };
                    }
                    else
                    {
                        request.AllowedDay = employee.NumberOfVacation;
                        request.RemainingDay = employee.NumberOfVacation;
                        request.EmployeeStartDate = employee.EmploymentStartDate;
                    }
                }
                if (await _LeaveRequestRepository.AddAsync(request))
                {
                    return new LeaveRequestResponseDto { Message = "Operation Successfully Completed", Status = OperationStatus.SUCCESS };
                }
                else
                {
                    return new LeaveRequestResponseDto { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
                }
            }
            catch (Exception ex)
            {
                return new LeaveRequestResponseDto { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
            }
        }
        public async Task<OperationStatusResponse> DeleteAsync(long id)
        {
            try
            {
                var LeaveRequest = await _LeaveRequestRepository.FirstOrDefaultAsync(u => u.Id == id);

                if (LeaveRequest == null)
                    return new OperationStatusResponse { Message = "Record Does Not Exist", Status = OperationStatus.ERROR };

                LeaveRequest.RecordStatus = RecordStatus.Deleted;
                if (await _LeaveRequestRepository.UpdateAsync(LeaveRequest))
                    return new OperationStatusResponse { Message = "Operation Successfully Completed", Status = OperationStatus.SUCCESS };
                else
                    return new LeaveRequestResponseDto { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
            }
            catch (Exception ex)
            {
                return new LeaveRequestResponseDto { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
            }
        }
        public async Task<LeaveRequestResponseDtos> GetAllAsync()
        {
            try
            {
                var result = new LeaveRequestResponseDtos();
                var LeaveRequestResponses = await _LeaveRequestRepository.Where(x => x.RecordStatus == RecordStatus.Active).Include(a=>a.Employee).Include(a=>a.LeaveType).ToListAsync();
                var LeaveRequestDTOs = new List<LeaveRequestResponseDtos>();
                result.Status = OperationStatus.SUCCESS;
                result.Message = "Operation Successfully Completed";
                result.LeaveRequestDtos = LeaveRequestResponses.ToList();
                return result;
            }
            catch (Exception ex)
            {
                return new LeaveRequestResponseDtos { LeaveRequestDtos = new List<LeaveRequest>(), Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
            }
        }
        public async Task<LeaveRequestResponseDto> GetByIdAsync(long id)
        {
            try
            {
                var result = new LeaveRequestResponseDto();
                var LeaveRequest = await _LeaveRequestRepository.WhereAsync(x => x.Id == id && x.RecordStatus == RecordStatus.Active);
                if (LeaveRequest == null)
                    return new LeaveRequestResponseDto { Status = OperationStatus.ERROR, Message = "Record Does Not Exist" };
                result.LeaveRequestDto = LeaveRequest.FirstOrDefault();
                result.Status = OperationStatus.SUCCESS;
                result.Message = "Operation Successfully Completed";
                return result;
            }
            catch (Exception ex)
            {
                return new LeaveRequestResponseDto { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
            }
        }
        public async Task<LeaveRequestResponseDto> UpdateAsync(LeaveRequest request)
        {
            var LeaveRequest = await _LeaveRequestRepository.FindAsync(request.Id);
            if (LeaveRequest == null)
                return new LeaveRequestResponseDto() { Status = OperationStatus.ERROR, Message = "Record Does Not Exist" };
           
            try
            {
                LeaveRequest.EmployeeId = request.EmployeeId;
                LeaveRequest.LeaveTypeId = request.LeaveTypeId;
                LeaveRequest.Year = request.Year;
                LeaveRequest.NumberOfHolidayDay = request.NumberOfHolidayDay;
                LeaveRequest.NumberOfWorkingDay = request.NumberOfWorkingDay;
                LeaveRequest.TotalLeaveDay = request.NumberOfHolidayDay + request.NumberOfWorkingDay;
                LeaveRequest.StartDate = request.StartDate;
                LeaveRequest.EndDate = request.EndDate;
                LeaveRequest.AllowedDay = request.AllowedDay;
                LeaveRequest.RemainingDay = request.RemainingDay;
                LeaveRequest.EmployeeStartDate = request.EmployeeStartDate;
                LeaveRequest.Remark = request.Remark;
                LeaveRequest.UpdatedBy = _httpContextAccessor.HttpContext.User.Identity.Name;
                LeaveRequest.LastUpdateDate = DateTime.UtcNow;

                if (await _LeaveRequestRepository.UpdateAsync(LeaveRequest))
                {
                    return new LeaveRequestResponseDto
                    {
                        Message = "Operation Successfully Completed",
                        Status = OperationStatus.SUCCESS
                    };
                }
                else
                {
                    return new LeaveRequestResponseDto
                    {
                        Message = "Error Has Occurred While Processing Your Request",
                        Status = OperationStatus.ERROR
                    };
                }
            }
            catch (Exception ex)
            {
                return new LeaveRequestResponseDto
                {
                    Message = "Error Has Occurred While Processing Your Request",
                    Status = OperationStatus.ERROR
                };
            }
        }
        public async Task<LeaveRequestResponseDtos> FiltereavBalanceByDate(DateTime startDate, DateTime endDate, string employeeId, int? year)
        {
            try
            {
                string message = "";
                List<LeaveRequest> LeaveRequests = new List<LeaveRequest>();
                if (!String.IsNullOrEmpty(employeeId) && year != null)
                {
                    LeaveRequests =await _LeaveRequestRepository.Where(c => c.RecordStatus == RecordStatus.Active && c.Employee.EmployeeId == employeeId && c.Year == year).Include(a => a.Employee).Include(a=>a.LeaveType).ToListAsync();

                    if (LeaveRequests.Count() <= 0)
                    {
                        message = "Leave balance data for the employee ID " + employeeId + " and year " + year + " is not found";
                        return new LeaveRequestResponseDtos()
                        {
                            LeaveRequestDtos = LeaveRequests,
                            Status = OperationStatus.ERROR,
                            Message = message,
                        };
                    }

                    return new LeaveRequestResponseDtos()
                    {
                        LeaveRequestDtos = LeaveRequests,
                        Status = OperationStatus.SUCCESS,
                        Message = message,
                    };
                }
                else if (!String.IsNullOrEmpty(employeeId) && (startDate != DateTime.MinValue || startDate == null) && (endDate != DateTime.MinValue || endDate == null))
                {
                    LeaveRequests =await _LeaveRequestRepository.Where(c => c.RecordStatus == RecordStatus.Active && c.Employee.EmployeeId == employeeId && c.RegisteredDate >= startDate && c.RegisteredDate <= endDate).Include(a => a.Employee).ToListAsync();

                    if (LeaveRequests.Count() <= 0)
                    {
                        message = "Leave balance data for the employee ID " + employeeId + " and date between " + startDate.ToString("yyyy-MM-dd") + " to " + endDate.ToString("yyyy-MM-dd") + " is not found";
                        return new LeaveRequestResponseDtos()
                        {
                            LeaveRequestDtos = LeaveRequests,
                            Status = OperationStatus.ERROR,
                            Message = message,
                        };
                    }

                    return new LeaveRequestResponseDtos()
                    {
                        LeaveRequestDtos = LeaveRequests,
                        Status = OperationStatus.SUCCESS,
                        Message = message,
                    };
                }
                else if (!String.IsNullOrEmpty(employeeId) && (startDate == DateTime.MinValue || startDate == null) && (endDate == DateTime.MinValue || endDate == null))
                {

                    LeaveRequests =await _LeaveRequestRepository.Where(c => c.RecordStatus == RecordStatus.Active && c.Employee.EmployeeId == employeeId).Include(a => a.Employee).Include(a=>a.LeaveType).ToListAsync();

                    if (LeaveRequests.Count() <= 0)
                    {
                        message = "Leave balance request data for employee ID " + employeeId + " is not found";
                        return new LeaveRequestResponseDtos()
                        {
                            LeaveRequestDtos= LeaveRequests,
                            Status = OperationStatus.ERROR,
                            Message = message,
                        };
                    }

                    return new LeaveRequestResponseDtos()
                    {
                        LeaveRequestDtos = LeaveRequests,
                        Status = OperationStatus.SUCCESS,
                        Message = message,
                    };
                }
                else
                {
                    if (startDate == DateTime.MinValue || startDate == null)
                    {
                        startDate = DateTime.Now;
                    }
                    if (endDate == DateTime.MinValue || endDate == null)
                    {
                        endDate = DateTime.Now.AddDays(1);
                    }
                    LeaveRequests = await _LeaveRequestRepository.Where(c => c.RecordStatus == RecordStatus.Active && c.RegisteredDate >= startDate && c.RegisteredDate <= endDate).Include(a => a.Employee).Include(a=>a.LeaveType).ToListAsync();

                    if (LeaveRequests.Count() <= 0)
                    {
                        return new LeaveRequestResponseDtos()
                        {
                            LeaveRequestDtos = LeaveRequests,
                            Status = OperationStatus.ERROR,
                            Message = "Leave balance requests data is not found."

                        };
                    }
                    else
                    {
                        return new LeaveRequestResponseDtos()
                        {
                            LeaveRequestDtos = LeaveRequests,
                            Status = OperationStatus.SUCCESS,
                        };
                    }

                }
            }
            catch (Exception ex)
            {
                return new LeaveRequestResponseDtos()
                {
                    Status = OperationStatus.ERROR,
                    Message = "Something went wrong please try again",
                    LeaveRequestDtos = new List<LeaveRequest>()

                };
            }
        }

    }
}
