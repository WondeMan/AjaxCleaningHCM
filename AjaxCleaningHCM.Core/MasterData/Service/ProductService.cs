using AjaxCleaningHCM.Core.Helper;
using AjaxCleaningHCM.Domain.DTO.MasterData;
using AjaxCleaningHCM.Domain.Models.Helper;
using AjaxCleaningHCM.Domain.Models.MasterData;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using static AjaxCleaningHCM.Domain.Enums.Common;
using System.Threading.Tasks;
using AjaxCleaningHCM.Domain.Models;
using System.Linq;
using AjaxCleaningHCM.Domain.DTO.MasterData.Response;
using AjaxCleaningHCM.Domain.DTO.MasterData.Request;
using Microsoft.EntityFrameworkCore;
using AjaxCleaningHCM.Core.MasterData.Interface;

namespace AjaxCleaningHCM.Core.MasterData.Service
{
    public class ProductService :IProduct
    {
        private readonly IRepositoryBase<Product> _productRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        public ProductService(IRepositoryBase<Product> productRepository, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _productRepository = productRepository;
            _mapper = mapper;
        }
        public async Task<ProductResponseDto> CreateAsync(ProductDto request)
        {
            try
            {
                var product = _mapper.Map<Product>(request);
                product.StartDate = DateTime.Now;
                product.EndDate = DateTime.MaxValue;
                product.RegisteredDate = DateTime.Now;
                product.RegisteredBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                product.RecordStatus = RecordStatus.Active;
                product.Remark = "test";
                product.IsReadOnly = false;

                if (await _productRepository.AddAsync(product))
                {
                    return new ProductResponseDto { Message = "Operation Successfully Completed", Status = OperationStatus.SUCCESS };
                }
                else
                {
                    return new ProductResponseDto { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
                }
            }
            catch (Exception ex)
            {
                return new ProductResponseDto { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
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
                var productDto=_mapper.Map<Product>(product);
                if (await _productRepository.UpdateAsync(productDto)) 
                    return new OperationStatusResponse { Message = "Operation Successfully Completed", Status = OperationStatus.SUCCESS };
                else
                    return new ProductResponseDto { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
            }
            catch (Exception ex)
            {
                return new ProductResponseDto { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
            }
        }
        public async Task<ProductsResponseDto> GetAllAsync()
        {
            try
            {
                var result = new ProductsResponseDto();
                var productResponses = await _productRepository.WhereAsync(x => x.RecordStatus == RecordStatus.Active);
                var productDTOs = _mapper.Map<List<ProductDto>>(productResponses);
                result.Status = OperationStatus.SUCCESS;
                result.Message = "Operation Successfully Completed";
                result.ProductDtos = productDTOs;
                return result;
            }
            catch (Exception ex)
            {
                return new ProductsResponseDto { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
            }
        }
        public async Task<ProductResponseDto> GetByIdAsync(long id)
        {
            try
            {
                var result = new ProductResponseDto();
                var product = await _productRepository.WhereAsync(x => x.Id == id && x.RecordStatus == RecordStatus.Active);
                if (product == null)
                    return new ProductResponseDto { Status = OperationStatus.ERROR, Message = "Record Does Not Exist" };
                var productDTO = _mapper.Map<ProductDto>(product.FirstOrDefault());
                result.ProductDto = productDTO;
                result.Status = OperationStatus.SUCCESS;
                result.Message = "Operation Successfully Completed";
                return result;
            }
            catch (Exception ex)
            {
                return new ProductResponseDto { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
            }
        }
        public async Task<ProductResponseDto> UpdateAsync(ProductDto request)
        {
            var product = await _productRepository.FindAsync(request.Id);
            if (product == null)
                return new ProductResponseDto() { Status = OperationStatus.ERROR, Message = "Record Does Not Exist" };

            try
            {
                _mapper.Map(request, product);
                product.UpdatedBy = _httpContextAccessor.HttpContext.Session.GetString("CurrentUsername");
                product.LastUpdateDate = DateTime.UtcNow;

                if (await _productRepository.UpdateAsync(product))
                {
                    return new ProductResponseDto
                    {
                        Message = "Operation Successfully Completed",
                        Status = OperationStatus.SUCCESS
                    };
                }
                else
                {
                    return new ProductResponseDto
                    {
                        Message = "Error Has Occurred While Processing Your Request",
                        Status = OperationStatus.ERROR
                    };
                }
            }
            catch (Exception ex)
            {
                return new ProductResponseDto
                {
                    Message = "Error Has Occurred While Processing Your Request",
                    Status = OperationStatus.ERROR
                };
            }
        }
    }
}
