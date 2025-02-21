using AjaxCleaningHCM.Domain.Models.Helper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AjaxCleaningHCM.Core.Helper
{
    public interface ICrud<Response, Responses, Request> 
    {
        Task<Responses> GetAllAsync();
        Task<Response> GetByIdAsync(long id);
        Task<Response> CreateAsync(Request request);
        Task<Response> UpdateAsync(Request request);
        Task<OperationStatusResponse> DeleteAsync(long id);
    }
}
