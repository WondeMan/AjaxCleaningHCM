using AjaxCleaningHCM.Domain.Models.MasterData;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using AjaxCleaningHCM.Domain.DTO.MasterData.Request;
using AjaxCleaningHCM.Domain.DTO.MasterData.Response;

namespace AjaxCleaningHCM.Core.Helper.Utils
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<ProductDto, Product>().ReverseMap();

            CreateMap<Employee, EmployeeDto>().ReverseMap();

            CreateMap<Bank, BankDto>().ReverseMap();

            CreateMap<Branch, BranchDto>().ReverseMap();

            CreateMap<Shift, ShiftDto>().ReverseMap();

            CreateMap<EmployeeHistory, ProductDto>().ReverseMap();


        }
    }
}
