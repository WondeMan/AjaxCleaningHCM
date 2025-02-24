using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AjaxCleaningHCM.Core.Operational.Interface
{
    public interface IPayrollService
    {
        Task ProcessPayroll(long employeeId);
    }
}
