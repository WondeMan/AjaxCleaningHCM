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
        private readonly IRepositoryBase<Bank> _productRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        public BankService(IRepositoryBase<Bank> productRepository, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _productRepository = productRepository;
            _mapper = mapper;
        }
        public async Task<BankResponseDto> CreateAsync(BankDto request)
        {
            try
            {
                var product = _mapper.Map<Bank>(request);
                product.StartDate = DateTime.Now;
                product.EndDate = DateTime.MaxValue;
                product.RegisteredDate = DateTime.Now;
                product.RegisteredBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                product.RecordStatus = RecordStatus.Active;
                product.Remark = "test";
                product.IsReadOnly = false;

                if (await _productRepository.AddAsync(product))
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
                var product = await _productRepository.FirstOrDefaultAsync(u => u.Id == id);

                if (product == null)
                    return new OperationStatusResponse { Message = "Record Does Not Exist", Status = OperationStatus.ERROR };

                product.RecordStatus = RecordStatus.Deleted;
                var productDto = _mapper.Map<Bank>(product);
                if (await _productRepository.UpdateAsync(productDto))
                    return new OperationStatusResponse { Message = "Operation Successfully Completed", Status = OperationStatus.SUCCESS };
                else
                    return new BankResponseDto { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
            }
            catch (Exception ex)
            {
                return new BankResponseDto { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
            }
        }
        public async Task<BanksResponseDto> GetAllAsync()
        {
            try
            {
                var result = new BanksResponseDto();
                var productResponses = await _productRepository.WhereAsync(x => x.RecordStatus == RecordStatus.Active);
                var productDTOs = _mapper.Map<List<BankDto>>(productResponses);
                result.Status = OperationStatus.SUCCESS;
                result.Message = "Operation Successfully Completed";
                result.BankDtos = productDTOs;
                return result;
            }
            catch (Exception ex)
            {
                return new BanksResponseDto { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
            }
        }
        public async Task<BankResponseDto> GetByIdAsync(long id)
        {
            try
            {
                var result = new BankResponseDto();
                var product = await _productRepository.WhereAsync(x => x.Id == id && x.RecordStatus == RecordStatus.Active);
                if (product == null)
                    return new BankResponseDto { Status = OperationStatus.ERROR, Message = "Record Does Not Exist" };
                var productDTO = _mapper.Map<BankDto>(product.FirstOrDefault());
                result.BankDto = productDTO;
                result.Status = OperationStatus.SUCCESS;
                result.Message = "Operation Successfully Completed";
                return result;
            }
            catch (Exception ex)
            {
                return new BankResponseDto { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
            }
        }
        public async Task<BankResponseDto> UpdateAsync(BankDto request)
        {
            var product = await _productRepository.FindAsync(request.Id);
            if (product == null)
                return new BankResponseDto() { Status = OperationStatus.ERROR, Message = "Record Does Not Exist" };

            try
            {
                _mapper.Map(request, product);
                product.UpdatedBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                product.LastUpdateDate = DateTime.UtcNow;

                if (await _productRepository.UpdateAsync(product))
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
