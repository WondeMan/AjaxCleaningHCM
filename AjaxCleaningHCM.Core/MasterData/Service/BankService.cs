using AjaxCleaningHCM.Core.Helper;
using AjaxCleaningHCM.Core.MasterData.Interface;
using AjaxCleaningHCM.Domain.DTO.MasterData.Request;
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

namespace AjaxCleaningHCM.Core.MasterData.Service
{
    public class BankService : IBank
    {
        private readonly IRepositoryBase<Bank> _BankRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public BankService(IRepositoryBase<Bank> BankRepository, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _BankRepository = BankRepository;
        }
        public async Task<BankResponseDto> CreateAsync(Bank request)
        {
            try
            {
                if (_BankRepository.ExistWhere(x => x.Name == request.Name && x.RecordStatus == RecordStatus.Active))
                    return new BankResponseDto { Message = "Record already exist with the same name", Status = OperationStatus.ERROR };
                request.StartDate = DateTime.Now;
                request.EndDate = DateTime.MaxValue;
                request.RegisteredDate = DateTime.Now;
                request.RegisteredBy = _httpContextAccessor.HttpContext.User.Identity.Name;
                request.RecordStatus = RecordStatus.Active;
                request.Remark = "test";
                request.IsReadOnly = false;

                if (await _BankRepository.AddAsync(request))
                {
                    return new BankResponseDto { Message = "Operation Successfully Completed", Status = OperationStatus.SUCCESS };
                }
                else
                {
                    return new BankResponseDto { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
                }
            }
            catch (Exception ex)
            {
                return new BankResponseDto { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
            }
        }
        public async Task<OperationStatusResponse> DeleteAsync(long id)
        {
            try
            {
                var Bank = await _BankRepository.FirstOrDefaultAsync(u => u.Id == id);

                if (Bank == null)
                    return new OperationStatusResponse { Message = "Record Does Not Exist", Status = OperationStatus.ERROR };

                Bank.RecordStatus = RecordStatus.Deleted;
                if (await _BankRepository.UpdateAsync(Bank))
                    return new OperationStatusResponse { Message = "Operation Successfully Completed", Status = OperationStatus.SUCCESS };
                else
                    return new BankResponseDto { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
            }
            catch (Exception ex)
            {
                return new BankResponseDto { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
            }
        }
        public async Task<BankResponseDtos> GetAllAsync()
        {
            try
            {
                var result = new BankResponseDtos();
                var BankResponses = await _BankRepository.WhereAsync(x => x.RecordStatus == RecordStatus.Active);
                var BankDTOs = new List<BankResponseDtos>();
                result.Status = OperationStatus.SUCCESS;
                result.Message = "Operation Successfully Completed";
                result.BankDtos = BankResponses.ToList();
                return result;
            }
            catch (Exception ex)
            {
                return new BankResponseDtos { BankDtos = new List<Bank>(), Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
            }
        }
        public async Task<BankResponseDto> GetByIdAsync(long id)
        {
            try
            {
                var result = new BankResponseDto();
                var Bank = await _BankRepository.WhereAsync(x => x.Id == id && x.RecordStatus == RecordStatus.Active);
                if (Bank == null)
                    return new BankResponseDto { Status = OperationStatus.ERROR, Message = "Record Does Not Exist" };
                result.BankDto = Bank.FirstOrDefault();
                result.Status = OperationStatus.SUCCESS;
                result.Message = "Operation Successfully Completed";
                return result;
            }
            catch (Exception ex)
            {
                return new BankResponseDto { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
            }
        }
        public async Task<BankResponseDto> UpdateAsync(Bank request)
        {
            var Bank = await _BankRepository.FindAsync(request.Id);
            if (Bank == null)
                return new BankResponseDto() { Status = OperationStatus.ERROR, Message = "Record Does Not Exist" };
            if ((Bank.Name != request.Name) && _BankRepository.ExistWhere(x => x.Name == request.Name && x.RecordStatus == RecordStatus.Active))
                return new BankResponseDto { Message = "Record already exist with the same name", Status = OperationStatus.ERROR };

            try
            {
                Bank.Name = request.Name;
                Bank.Code = request.Code;
                Bank.Description = request.Description;
                Bank.UpdatedBy = _httpContextAccessor.HttpContext.User.Identity.Name;
                Bank.LastUpdateDate = DateTime.UtcNow;

                if (await _BankRepository.UpdateAsync(Bank))
                {
                    return new BankResponseDto
                    {
                        Message = "Operation Successfully Completed",
                        Status = OperationStatus.SUCCESS
                    };
                }
                else
                {
                    return new BankResponseDto
                    {
                        Message = "Error Has Occurred While Processing Your Request",
                        Status = OperationStatus.ERROR
                    };
                }
            }
            catch (Exception ex)
            {
                return new BankResponseDto
                {
                    Message = "Error Has Occurred While Processing Your Request",
                    Status = OperationStatus.ERROR
                };
            }
        }
    }

}
