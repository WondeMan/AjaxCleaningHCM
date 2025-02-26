using AjaxCleaningHCM.Core.Helper;
using AjaxCleaningHCM.Core.MasterData.Interface;
using AjaxCleaningHCM.Domain.DTO.MasterData.Response;
using AjaxCleaningHCM.Domain.Models.Helper;
using AjaxCleaningHCM.Domain.Models.MasterData;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using static AjaxCleaningHCM.Domain.Enums.Common;
using System.Threading.Tasks;
using System.Linq;
using AjaxCleaningHCM.Core.Account.Interface;
using AjaxCleaningHCM.Core.UserManagment.Identity;
using AjaxCleaningHCM.Core.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace AjaxCleaningHCM.Core.MasterData.Service
{
    public class EmployeeService : IEmployee
    {
        private readonly IRepositoryBase<Employee> _EmployeeRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserAccount _userAccount;
        private readonly IMemoryCache _cache;
        public EmployeeService(IRepositoryBase<Employee> EmployeeRepository, IMemoryCache cache, IHttpContextAccessor httpContextAccessor, IUserAccount userAccount)
        {
            _httpContextAccessor = httpContextAccessor;
            _EmployeeRepository = EmployeeRepository;
            _userAccount = userAccount;
            _cache = cache;
        }
        public async Task<EmployeeResponseDto> CreateAsync(Employee request)
        {
            try
            {
                request.StartDate = DateTime.Now;
                request.EndDate = DateTime.MaxValue;
                request.RegisteredDate = DateTime.Now;
                request.RegisteredBy = _httpContextAccessor.HttpContext.User.Identity.Name;
                request.RecordStatus = RecordStatus.Active;
                request.EmployeeId = "HE000" + GetLastInserted().ToString();
                request.CirtificationPath = FileUploader.UploadFile(request.CirtificationImg);
                request.EmpoloyePhotoPath = FileUploader.UploadFile(request.EmpoloyePhotoImg);
                request.EmployeeKebeleIDPath = FileUploader.UploadFile(request.EmployeeKebeleIDImg);
                request.GurrentKebeleIDPath = FileUploader.UploadFile(request.GurrentKebeleIDImg);
                request.GurrentDocumentPath = FileUploader.UploadFile(request.GuaranteeDocumentImg);

                request.IsReadOnly = false;
                request.EmployeeBranches = request.EmployeeBranches ?? new List<EmployeeBranch>();

                // Clear existing relationships
                request.EmployeeBranches.Clear();
                if (request.BrancheIds != null)
                {
                    foreach (var branchId in request.BrancheIds)
                    {
                        request.EmployeeBranches.Add(new EmployeeBranch
                        {
                            EmployeeId = request.Id,
                            BranchId = branchId
                        });
                    }
                }
                if (await _EmployeeRepository.AddAsync(request))
                {
                    var user = new User()
                    {
                        FirstName = request.FirstName,
                        LastName = request.LastName,
                        MiddleName = request.MiddleName,
                        UserName = request.EmployeeId,
                        Email = request.EmailAddress,
                    };
                    var role = "User";
                    await _userAccount.CreateFromEmployeeAsync(user, role);

                    return new EmployeeResponseDto { Message = "Operation Successfully Completed", Status = OperationStatus.SUCCESS };
                }
                else
                {
                    return new EmployeeResponseDto { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
                }
            }
            catch (Exception ex)
            {
                return new EmployeeResponseDto { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
            }
        }
        public async Task<OperationStatusResponse> DeleteAsync(long id)
        {
            try
            {
                var Employee = await _EmployeeRepository.FirstOrDefaultAsync(u => u.Id == id);

                if (Employee == null)
                    return new OperationStatusResponse { Message = "Record Does Not Exist", Status = OperationStatus.ERROR };

                Employee.RecordStatus = RecordStatus.Deleted;
                if (await _EmployeeRepository.UpdateAsync(Employee))
                    return new OperationStatusResponse { Message = "Operation Successfully Completed", Status = OperationStatus.SUCCESS };
                else
                    return new EmployeeResponseDto { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
            }
            catch (Exception ex)
            {
                return new EmployeeResponseDto { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
            }
        }
        public async Task<EmployeeResponseDtos> GetAllAsync()
        {
            try
            {
                var result = new EmployeeResponseDtos();
                var EmployeeResponses = await _EmployeeRepository.WhereAsync(a => a.RecordStatus == RecordStatus.Active, "EmployeeBranches.Branch");
                var EmployeeDTOs = new List<EmployeeResponseDtos>();
                result.Status = OperationStatus.SUCCESS;
                result.Message = "Operation Successfully Completed";
                result.EmployeeDtos = EmployeeResponses.ToList();
                return result;
            }
            catch (Exception ex)
            {
                return new EmployeeResponseDtos { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
            }
        }
        public async Task<EmployeeResponseDtos> GetBulkEmployeeByIdAsync(List<long> employeeIds)
        {
            try
            {

                var result = new EmployeeResponseDtos();
                var EmployeeResponses = await _EmployeeRepository.WhereAsync(a => a.RecordStatus == RecordStatus.Active && employeeIds.Contains(a.Id), "EmployeeBranches.Branch");
                var EmployeeDTOs = new List<EmployeeResponseDtos>();
                result.Status = OperationStatus.SUCCESS;
                result.Message = "Operation Successfully Completed";
                result.EmployeeDtos = EmployeeResponses.ToList();
                return result;
            }
            catch (Exception ex)
            {
                return new EmployeeResponseDtos { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
            }
        }

        public async Task<EmployeeResponseDto> GetByIdAsync(long id)
        {
            try
            {
                var result = new EmployeeResponseDto();
                var Employee = await _EmployeeRepository.WhereAsync(x => x.Id == id, "EmployeeBranches.Branch", "Bank");
                if (Employee == null)
                    return new EmployeeResponseDto { Status = OperationStatus.ERROR, Message = "Record Does Not Exist" };
                result.EmployeeDto = Employee.FirstOrDefault();
                result.Status = OperationStatus.SUCCESS;
                result.Message = "Operation Successfully Completed";
                return result;
            }
            catch (Exception ex)
            {
                return new EmployeeResponseDto { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
            }
        }
        public async Task<EmployeeResponseDto> GetByUserNameAsync(string userName)
        {
            try
            {
                var result = new EmployeeResponseDto();
                var Employee = await _EmployeeRepository.WhereAsync(x => x.EmployeeId == userName && x.RecordStatus == RecordStatus.Active, "EmployeeBranches.Branch", "Bank");
                if (Employee == null)
                    return new EmployeeResponseDto { Status = OperationStatus.ERROR, Message = "Record Does Not Exist" };
                result.EmployeeDto = Employee.FirstOrDefault();
                result.Status = OperationStatus.SUCCESS;
                result.Message = "Operation Successfully Completed";
                return result;
            }
            catch (Exception ex)
            {
                return new EmployeeResponseDto { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
            }
        }
        public Employee GetEmployee()
        {
            var userName = _httpContextAccessor.HttpContext.User.Identity.Name;
            var employee = _EmployeeRepository.Where(a => a.EmployeeId == userName && a.RecordStatus == RecordStatus.Active, "EmployeeBranches.Branch").FirstOrDefault();
            return employee;
        }
        public long GetLastInserted()
        {
            return _EmployeeRepository.LastOrDefault()?.Id ?? 0;
        }
        public async Task<EmployeeResponseDtos> Search(EmployeeSearchRequest EmployeeSearchRequest)
        {
            try
            {
                if (EmployeeSearchRequest == null)
                {
                    return new EmployeeResponseDtos
                    {
                        Message = "Error Has Occured While Processing Your Request",
                        Status = OperationStatus.ERROR
                    };
                }
                var result = new EmployeeResponseDtos();
                if (EmployeeSearchRequest.Start == DateTime.MinValue || EmployeeSearchRequest.Start == null)
                {
                    EmployeeSearchRequest.Start = DateTime.Now;
                }
                if (EmployeeSearchRequest.End == DateTime.MinValue || EmployeeSearchRequest.End == null)
                {
                    EmployeeSearchRequest.End = DateTime.Now;
                }
                var fiveYearsAgo = DateTime.Now;
                if (EmployeeSearchRequest.SearchBygroup == SearchBygroup.Experience)
                {
                    fiveYearsAgo = fiveYearsAgo.AddYears(-EmployeeSearchRequest.Experience);
                }
                List<Employee> EmployeeResponses = EmployeeSearchRequest.SearchBygroup switch
                {
                    SearchBygroup.DateRange => await _EmployeeRepository
                                                .Where(x => x.RecordStatus == RecordStatus.Active &&
                                                            x.RegisteredDate >= EmployeeSearchRequest.Start &&
                                                            x.RegisteredDate <= EmployeeSearchRequest.End)
                                                .Include(a => a.Bank)
                                                .Include(a => a.EmployeeBranches)
                                                .ToListAsync(),
                    SearchBygroup.Gender => await _EmployeeRepository
                                                .Where(x => x.RecordStatus == RecordStatus.Active &&
                                                            x.Gender == EmployeeSearchRequest.Gender)
                                                  .Include(a => a.Bank)
                                                .Include(a => a.EmployeeBranches)
                                                .ToListAsync(),
                    SearchBygroup.EducationStatus => await _EmployeeRepository
                                                       .Where(x => x.RecordStatus == RecordStatus.Active &&
                                                                   x.EducationStatus == EmployeeSearchRequest.EducationStatus)
                                                         .Include(a => a.Bank)
                                                       .Include(a => a.EmployeeBranches)
                                                       .ToListAsync(),
                    SearchBygroup.Salary => await _EmployeeRepository
                                                   .Where(x => x.RecordStatus == RecordStatus.Active &&
                                                               x.MonthlySalary >= EmployeeSearchRequest.MinSalary &&
                                                               x.MonthlySalary <= EmployeeSearchRequest.MaxSalary)
                                                     .Include(a => a.Bank)
                                                   .Include(a => a.EmployeeBranches)
                                                   .ToListAsync(),
                    SearchBygroup.Experience => await _EmployeeRepository
                                                   .Where(x => x.RecordStatus == RecordStatus.Active && x.EmploymentStartDate <= fiveYearsAgo)

                                                     .Include(a => a.Bank)
                                                   .Include(a => a.EmployeeBranches)
                                                   .ToListAsync(),
                    _ => await _EmployeeRepository
                                                .Where(x => x.RecordStatus == RecordStatus.Active)
                                               .Include(a => a.Bank)
                                                .Include(a => a.EmployeeBranches)
                                                .ToListAsync(),
                };
                result.EmployeeDtos = EmployeeResponses;
                result.Status = OperationStatus.SUCCESS;
                result.Message = "Operation Sucessfully Completed";
                return result;
            }
            catch (Exception)
            {
                return new EmployeeResponseDtos
                {
                    Message = "Error Has Occured While Processing Your Request",
                    Status = OperationStatus.ERROR
                };
            }
        }
        public async Task<EmployeeResponseDto> UpdateEmployeeStatusAsync(Employee request)
        {
            if (await _EmployeeRepository.UpdateAsync(request))
            {
                return new EmployeeResponseDto
                {
                    Message = "Operation Successfully Completed",
                    Status = OperationStatus.SUCCESS
                };
            }
            else
            {
                return new EmployeeResponseDto
                {
                    Message = "Error Has Occurred While Processing Your Request",
                    Status = OperationStatus.ERROR
                };
            }
        }
        public async Task<EmployeeResponseDto> UpdateAsync(Employee request)
        {
            var existingEmployee = await _EmployeeRepository.Where(a => a.Id == request.Id, "EmployeeBranches.Branch").FirstOrDefaultAsync();
            if (existingEmployee == null)
                return new EmployeeResponseDto() { Status = OperationStatus.ERROR, Message = "Record Does Not Exist" };

            try
            {
                // Copy unchanged properties
                existingEmployee.UpdatedBy = _httpContextAccessor.HttpContext.User.Identity.Name;
                existingEmployee.LastUpdateDate = DateTime.UtcNow;

                // Handle file uploads
                existingEmployee.CirtificationPath = request.CirtificationImg == null ? existingEmployee.CirtificationPath : FileUploader.UploadFile(request.CirtificationImg);
                existingEmployee.EmpoloyePhotoPath = request.EmpoloyePhotoImg == null ? existingEmployee.EmpoloyePhotoPath : FileUploader.UploadFile(request.EmpoloyePhotoImg);
                existingEmployee.EmployeeKebeleIDPath = request.EmployeeKebeleIDImg == null ? existingEmployee.EmployeeKebeleIDPath : FileUploader.UploadFile(request.EmployeeKebeleIDImg);
                existingEmployee.GurrentKebeleIDPath = request.GurrentKebeleIDImg == null ? existingEmployee.GurrentKebeleIDPath : FileUploader.UploadFile(request.GurrentKebeleIDImg);
                existingEmployee.GurrentDocumentPath = request.GuaranteeDocumentImg == null ? existingEmployee.GurrentDocumentPath : FileUploader.UploadFile(request.GuaranteeDocumentImg);

                // Manage branches - Update without clearing the list
                if (request.BrancheIds != null)
                {

                    #region basic fileds
                    existingEmployee.MonthlySalary = request.MonthlySalary > 0 ? request.MonthlySalary : 0;
                    existingEmployee.JobPosition = request.JobPosition == null ? existingEmployee.JobPosition : request.JobPosition;
                    existingEmployee.JobTitle = request.JobPosition == null ? existingEmployee.JobTitle : request.JobPosition;
                    existingEmployee.EndDate = request.EndDate == null ? existingEmployee.EndDate : request.EndDate;
                    existingEmployee.BankId = request.BankId == null ? existingEmployee.BankId : request.BankId;
                    existingEmployee.BankAccountNumber = request.BankAccountNumber == null ? existingEmployee.BankAccountNumber : request.BankAccountNumber;
                    existingEmployee.EmailAddress = request.EmailAddress == null ? existingEmployee.EmailAddress : request.EmailAddress;
                    existingEmployee.FirstName = request.FirstName == null ? existingEmployee.FirstName : request.FirstName;
                    existingEmployee.MiddleName = request.MiddleName == null ? existingEmployee.MiddleName : request.MiddleName;
                    existingEmployee.LastName = request.LastName == null ? existingEmployee.LastName : request.LastName;

                    #endregion
                    // Convert BrancheIds to HashSet for easier lookup
                    var updatedBranchIds = new HashSet<long>(request.BrancheIds);

                    // Find branches to remove (present in existing but not in updatedBranchIds)
                    var branchesToRemove = existingEmployee.EmployeeBranches
                        .Where(eb => !updatedBranchIds.Contains(eb.BranchId))
                        .ToList();

                    foreach (var branch in branchesToRemove)
                    {
                        existingEmployee.EmployeeBranches.Remove(branch);
                    }

                    // Find branches to add (present in updatedBranchIds but not in existing)
                    var existingBranchIds = new HashSet<long>(existingEmployee.EmployeeBranches.Select(eb => eb.BranchId));
                    var branchesToAdd = updatedBranchIds
                        .Where(branchId => !existingBranchIds.Contains(branchId))
                        .ToList();

                    foreach (var branchId in branchesToAdd)
                    {
                        existingEmployee.EmployeeBranches.Add(new EmployeeBranch
                        {
                            EmployeeId = existingEmployee.Id, // Use the ID of the existing employee
                            BranchId = branchId
                        });
                    }
                }
                else
                {
                    return new EmployeeResponseDto
                    {
                        Message = "Employee Branch is Required",
                        Status = OperationStatus.ERROR
                    };
                }

                // Update the employee in the repository
                if (await _EmployeeRepository.UpdateAsync(existingEmployee))
                {
                    var userId = _httpContextAccessor.HttpContext.User.Identity.Name;
                    var BranchIdsCacheKey = $"BranchIds_{userId}";
                    var WarehousIdsCacheKey = $"WarehousIds_{userId}";
                    _cache.Remove(BranchIdsCacheKey);
                    _cache.Remove(WarehousIdsCacheKey);
                    return new EmployeeResponseDto
                    {
                        Message = "Operation Successfully Completed",
                        Status = OperationStatus.SUCCESS
                    };
                }
                else
                {
                    return new EmployeeResponseDto
                    {
                        Message = "Error Has Occurred While Processing Your Request",
                        Status = OperationStatus.ERROR
                    };
                }
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                return new EmployeeResponseDto
                {
                    Message = "Error Has Occurred While Processing Your Request",
                    Status = OperationStatus.ERROR
                };
            }
        }
        public OperationStatus Remove(long id)
        {
            try
            {
                var employee = _EmployeeRepository.Find(id);
                employee.LastUpdateDate = DateTime.Now;
                employee.RecordStatus = RecordStatus.Deleted;
                _EmployeeRepository.Update(employee);
                return OperationStatus.SUCCESS;
            }
            catch (Exception ex)
            {
                return OperationStatus.ERROR;
            }
        }
        public OperationStatus ActivateEmployee(long id)
        {
            try
            {
                var employee = _EmployeeRepository.Find(id);
                employee.LastUpdateDate = DateTime.Now;
                employee.RecordStatus = RecordStatus.Active;
                _EmployeeRepository.Update(employee);
                return OperationStatus.SUCCESS;
            }
            catch (Exception ex)
            {
                return OperationStatus.ERROR;
            }
        }
    }

}
