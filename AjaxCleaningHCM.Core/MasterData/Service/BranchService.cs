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
using Microsoft.Extensions.Caching.Memory;

namespace AjaxCleaningHCM.Core.MasterData.Service
{
    public class BranchService : IBranch
    {
        private readonly IRepositoryBase<Branch> _BranchRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmployee _employee;
        private readonly IMemoryCache _cache;

        public BranchService(IRepositoryBase<Branch> BranchRepository, IMemoryCache cache, IEmployee employee, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _BranchRepository = BranchRepository;
            _employee = employee;
            _cache = cache;
        }
        public async Task<BranchResponseDto> CreateAsync(Branch request)
        {
            try
            {
                if (_BranchRepository.ExistWhere(x => x.Name == request.Name && x.RecordStatus == RecordStatus.Active))
                    return new BranchResponseDto { Message = "Record already exist with the same name", Status = OperationStatus.ERROR };
                request.StartDate = DateTime.Now;
                request.EndDate = DateTime.MaxValue;
                request.RegisteredDate = DateTime.Now;
                request.RegisteredBy = _httpContextAccessor.HttpContext.User.Identity.Name;
                request.RecordStatus = RecordStatus.Active;
                request.Code = "HB000" + GetLastInserted().ToString();
                request.Remark = "test";
                request.IsReadOnly = false;

                if (await _BranchRepository.AddAsync(request))
                {
                    return new BranchResponseDto { Message = "Operation Successfully Completed", Status = OperationStatus.SUCCESS };
                }
                else
                {
                    return new BranchResponseDto { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
                }
            }
            catch (Exception ex)
            {
                return new BranchResponseDto { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
            }
        }
        public async Task<OperationStatusResponse> DeleteAsync(long id)
        {
            try
            {
                var Branch = await _BranchRepository.FirstOrDefaultAsync(u => u.Id == id);

                if (Branch == null)
                    return new OperationStatusResponse { Message = "Record Does Not Exist", Status = OperationStatus.ERROR };

                Branch.RecordStatus = RecordStatus.Deleted;
                if (await _BranchRepository.UpdateAsync(Branch))
                    return new OperationStatusResponse { Message = "Operation Successfully Completed", Status = OperationStatus.SUCCESS };
                else
                    return new BranchResponseDto { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
            }
            catch (Exception ex)
            {
                return new BranchResponseDto { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
            }
        }
        public async Task<BranchResponseDtos> GetAllAsync()
        {
            try
            {
                var result = new BranchResponseDtos();
                var BranchResponses = await _BranchRepository.WhereAsync(x => x.RecordStatus == RecordStatus.Active, "EmployeeBranches");
                var BranchDTOs = new List<BranchResponseDtos>();
                result.Status = OperationStatus.SUCCESS;
                result.Message = "Operation Successfully Completed";
                result.BranchDtos = BranchResponses.ToList();
                return result;
            }
            catch (Exception ex)
            {
                return new BranchResponseDtos { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
            }
        }
        public List<long> GetBranchId()
        {
            try
            {
                var userId = _httpContextAccessor.HttpContext.User.Identity.Name;
                var cacheKey = $"BranchIds_{userId}";
                if (!_cache.TryGetValue(cacheKey, out List<long> branchIds))
                {
                    branchIds = _employee.GetEmployee()?.EmployeeBranches?.Select(a => a.BranchId)?.ToList() ?? new List<long>();
                    var cacheOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
                    };
                    _cache.Set(cacheKey, branchIds, cacheOptions);
                }
                return branchIds;
            }
            catch (Exception ex)
            {
                return new List<long>();
            }
        }
        public async Task<BranchResponseDto> GetByIdAsync(long id)
        {
            try
            {
                var result = new BranchResponseDto();
                var Branch = await _BranchRepository.WhereAsync(x => x.Id == id && x.RecordStatus == RecordStatus.Active, "Employees");
                if (Branch == null)
                    return new BranchResponseDto { Status = OperationStatus.ERROR, Message = "Record Does Not Exist" };
                result.BranchDto = Branch.FirstOrDefault();
                result.Status = OperationStatus.SUCCESS;
                result.Message = "Operation Successfully Completed";
                return result;
            }
            catch (Exception ex)
            {
                return new BranchResponseDto { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
            }
        }
        public long GetLastInserted()
        {
            return _BranchRepository.LastOrDefault()?.Id ?? 0;
        }
        public async Task<BranchResponseDto> UpdateAsync(Branch request)
        {
            var Branch = await _BranchRepository.FindAsync(request.Id);
            if (Branch == null)
                return new BranchResponseDto() { Status = OperationStatus.ERROR, Message = "Record Does Not Exist" };
            if ((Branch.Name != request.Name) && _BranchRepository.ExistWhere(x => x.Name == request.Name && x.RecordStatus == RecordStatus.Active))
                return new BranchResponseDto { Message = "Record already exist with the same name", Status = OperationStatus.ERROR };
            try
            {
                Branch.Name = request.Name;
                Branch.Description = request.Description;
                Branch.Address = request.Address;
                Branch.City = request.City;
                Branch.PhysicalLocation = request.PhysicalLocation;
                Branch.Phone = request.Phone;
                Branch.NumberOfEmployees = request.NumberOfEmployees;

                request.LastUpdateDate = DateTime.UtcNow;

                if (await _BranchRepository.UpdateAsync(Branch))
                {
                    return new BranchResponseDto
                    {
                        Message = "Operation Successfully Completed",
                        Status = OperationStatus.SUCCESS
                    };
                }
                else
                {
                    return new BranchResponseDto
                    {
                        Message = "Error Has Occurred While Processing Your Request",
                        Status = OperationStatus.ERROR
                    };
                }
            }
            catch (Exception ex)
            {
                return new BranchResponseDto
                {
                    Message = "Error Has Occurred While Processing Your Request",
                    Status = OperationStatus.ERROR
                };
            }
        }
    }

}
