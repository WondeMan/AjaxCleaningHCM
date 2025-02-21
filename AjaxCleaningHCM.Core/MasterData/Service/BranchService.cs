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

namespace AjaxCleaningHCM.Core.MasterData.Service
{
    public class BranchService : IBranch
    {
        private readonly IRepositoryBase<Branch> _productRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        public BranchService(IRepositoryBase<Branch> productRepository, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _productRepository = productRepository;
            _mapper = mapper;
        }
        public async Task<BranchResponseDto> CreateAsync(BranchDto request)
        {
            try
            {
                var product = _mapper.Map<Branch>(request);
                product.StartDate = DateTime.Now;
                product.EndDate = DateTime.MaxValue;
                product.RegisteredDate = DateTime.Now;
                product.RegisteredBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                product.RecordStatus = RecordStatus.Active;
                product.Remark = "test";
                product.IsReadOnly = false;

                if (await _productRepository.AddAsync(product))
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
                var product = await _productRepository.FirstOrDefaultAsync(u => u.Id == id);

                if (product == null)
                    return new OperationStatusResponse { Message = "Record Does Not Exist", Status = OperationStatus.ERROR };

                product.RecordStatus = RecordStatus.Deleted;
                var productDto = _mapper.Map<Branch>(product);
                if (await _productRepository.UpdateAsync(productDto))
                    return new OperationStatusResponse { Message = "Operation Successfully Completed", Status = OperationStatus.SUCCESS };
                else
                    return new BranchResponseDto { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
            }
            catch (Exception ex)
            {
                return new BranchResponseDto { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
            }
        }
        public async Task<BranchsResponseDto> GetAllAsync()
        {
            try
            {
                var result = new BranchsResponseDto();
                var productResponses = await _productRepository.WhereAsync(x => x.RecordStatus == RecordStatus.Active);
                var productDTOs = _mapper.Map<List<BranchDto>>(productResponses);
                result.Status = OperationStatus.SUCCESS;
                result.Message = "Operation Successfully Completed";
                result.BranchDtos = productDTOs;
                return result;
            }
            catch (Exception ex)
            {
                return new BranchsResponseDto { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
            }
        }
        public async Task<BranchResponseDto> GetByIdAsync(long id)
        {
            try
            {
                var result = new BranchResponseDto();
                var product = await _productRepository.WhereAsync(x => x.Id == id && x.RecordStatus == RecordStatus.Active);
                if (product == null)
                    return new BranchResponseDto { Status = OperationStatus.ERROR, Message = "Record Does Not Exist" };
                var productDTO = _mapper.Map<BranchDto>(product.FirstOrDefault());
                result.BranchDto = productDTO;
                result.Status = OperationStatus.SUCCESS;
                result.Message = "Operation Successfully Completed";
                return result;
            }
            catch (Exception ex)
            {
                return new BranchResponseDto { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
            }
        }
        public async Task<BranchResponseDto> UpdateAsync(BranchDto request)
        {
            var product = await _productRepository.FindAsync(request.Id);
            if (product == null)
                return new BranchResponseDto() { Status = OperationStatus.ERROR, Message = "Record Does Not Exist" };

            try
            {
                _mapper.Map(request, product);
                product.UpdatedBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                product.LastUpdateDate = DateTime.UtcNow;

                if (await _productRepository.UpdateAsync(product))
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
