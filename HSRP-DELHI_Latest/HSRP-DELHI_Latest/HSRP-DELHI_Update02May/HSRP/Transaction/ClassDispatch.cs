using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HSRP.Transaction
{
    public class ClassDispatch
    {
        string stateid, rtolocationid, dispatchCode, Address, RefNo, ProductSize, QTY, prifix, start, end;

        public string End
        {
            get { return end; }
            set { end = value; }
        }

        public string Start
        {
            get { return start; }
            set { start = value; }
        }

        public string Prifix
        {
            get { return prifix; }
            set { prifix = value; }
        }

        public string QTY1
        {
            get { return QTY; }
            set { QTY = value; }
        }

        public string ProductSize1
        {
            get { return ProductSize; }
            set { ProductSize = value; }
        }

        public string RefNo1
        {
            get { return RefNo; }
            set { RefNo = value; }
        }

        public string Address1
        {
            get { return Address; }
            set { Address = value; }
        }

        public string DispatchCode
        {
            get { return dispatchCode; }
            set { dispatchCode = value; }
        }

        public string Rtolocationid
        {
            get { return rtolocationid; }
            set { rtolocationid = value; }
        }

        public string Stateid
        {
            get { return stateid; }
            set { stateid = value; }
        }
    }
}