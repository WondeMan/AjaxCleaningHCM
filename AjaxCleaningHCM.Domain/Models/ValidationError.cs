﻿using System.Collections.Generic;

namespace AjaxCleaningHCM.Domain.Models
{
    public class ValidationError
    {
        public string ProcessStatus { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }
        public List<string> Exceptions { get; set; }
    }
}