using AjaxCleaningHCM.Domain.Models.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static AjaxCleaningHCM.Domain.Enums.Common;

namespace AjaxCleaningHCM.Domain.DTO.MasterData.Response
{
    public class ShiftResponseDto : OperationStatusResponse
    {
        public ShiftDto ShiftDto { get; set; }
    }
    public class ShiftsResponseDto : OperationStatusResponse
    {
        public List<ShiftDto> ShiftDtos { get; set; }
    }
    public class ShiftDto : OperationStatusResponse
    {
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
