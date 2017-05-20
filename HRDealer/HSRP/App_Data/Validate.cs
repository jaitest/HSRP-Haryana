using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HSRP
{
    public  class Validate
    {
        public   bool ValidateStringValue(string value)
        {
            bool IsValidate = false;
            if (value != null && value != "" && value != string.Empty)
            {
                IsValidate = true;
            }

            return IsValidate;


        }
    }
}