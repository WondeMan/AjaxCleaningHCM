using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;

namespace AjaxCleaningHCM.Core.Utils
{
    public static class EmailValidator
    {
        public static bool IsEmailValid(string email)
        {
            string pattern = "^([0-9a-zA-Z]([-\\.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$";
            return !string.IsNullOrEmpty(email) && Regex.IsMatch(email, pattern);
        }        
    }
}
