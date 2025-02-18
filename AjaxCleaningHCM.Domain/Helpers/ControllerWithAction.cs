using System;
using System.Collections.Generic;
using System.Text;

namespace AjaxCleaningHCM.Domain.Helpers
{
    public class ControllerWithAction
    {
        public Type ControllerType { get; set; }
        public string ControllerName { get; set; }
        public List<string> Actions { get; set; }
    }
}
