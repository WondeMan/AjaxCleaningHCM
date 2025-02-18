using AjaxCleaningHCM.Domain.Models.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AjaxCleaningHCM.Core.Helper.Interface
{
    public interface ICrudBase<TEntity> where TEntity : class
    {
        Task<IQueryable<TEntity>> GetAllAsync();
        Task<TEntity> GetByIdAsync(long id);
        Task<TEntity> CreateAsync(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
        Task<OperationStatusResponse> DeleteAsync(long id);
    }

}
