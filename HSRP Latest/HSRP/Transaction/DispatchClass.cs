using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HSRP.Transaction
{
    public class DispatchClass
    {
        string stateid, stateName, rtlolocaitonid, rtlolocaitonName, address, productsizeid, qty, prifix, lasercodefrom, lasercodeend, id;

        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        public string RtlolocaitonName
        {
            get { return rtlolocaitonName; }
            set { rtlolocaitonName = value; }
        }

        public string StateName
        {
            get { return stateName; }
            set { stateName = value; }
        }

        public string Stateid
        {
            get { return stateid; }
            set { stateid = value; }
        }

        public string Rtlolocaitonid
        {
            get { return rtlolocaitonid; }
            set { rtlolocaitonid = value; }
        }

        //public string Dispatchto
        //{
        //    get { return dispatchto; }
        //    set { dispatchto = value; }
        //}

        public string Address
        {
            get { return address; }
            set { address = value; }
        }

        //public string Refno
        //{
        //    get { return refno; }
        //    set { refno = value; }
        //}

        public string Productsizeid
        {
            get { return productsizeid; }
            set { productsizeid = value; }
        }

        public string Qty
        {
            get { return qty; }
            set { qty = value; }
        }

        public string Prifix
        {
            get { return prifix; }
            set { prifix = value; }
        }

        public string LaserCodeFrom
        {
            get { return lasercodefrom; }
            set { lasercodefrom = value; }
        }

        public string LaserCodeEnd
        {
            get { return lasercodeend; }
            set { lasercodeend = value; }
        }
    }
}