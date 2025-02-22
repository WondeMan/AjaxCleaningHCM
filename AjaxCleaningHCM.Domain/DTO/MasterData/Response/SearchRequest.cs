using System;
using System.Collections.Generic;
using System.Text;
using static AjaxCleaningHCM.Domain.Enums.Common;

namespace AjaxCleaningHCM.Domain.DTO.MasterData.Response
{
    public class SearchRequest
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public long BankId { get; set; }
        public SearchBy SearchBy { get; set; }
        public TerminationReason SearchByTerminationReason { get; set; }
    }
    public enum SearchBy
    {
        CreatedDat = 1,
        OrderedDate,
        ReceivingDate
    }
}
