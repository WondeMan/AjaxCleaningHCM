using AjaxCleaningHCM.Core.Helper;
using AjaxCleaningHCM.Domain.DTO.MasterData.Request;
using AjaxCleaningHCM.Domain.DTO.MasterData.Response;

namespace AjaxCleaningHCM.Core.MasterData.Interface
{
    public interface IProduct : ICrud<ProductResponseDto, ProductsResponseDto, ProductDto>
    {

    }
}
