using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Printing;

namespace HSRPDataEntryNew
{
    class PrintDoc : PrintDocument
    {

        private string textout;

        public string PrintText
        {
            get { return textout; }
            set { textout = value; }
        }
    }
}
