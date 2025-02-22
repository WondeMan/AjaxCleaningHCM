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
        }
    }
}
