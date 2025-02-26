using AjaxCleaningHCM.Domain.Models.Helper;
using AjaxCleaningHCM.Domain.Models.MasterData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AjaxCleaningHCM.Domain.DTO.MasterData.Response
{
    public class PayrollResponseDto : OperationStatusResponse
    {
        public Payroll PayrollDto { get; set; }
    }
    public class PayrollResponseDtos : OperationStatusResponse
    {
        public List<Payroll> PayrollDtos { get; set; }
    }

    public class PayrollRequst {
        public List<long> EmployeeId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
    }

}
