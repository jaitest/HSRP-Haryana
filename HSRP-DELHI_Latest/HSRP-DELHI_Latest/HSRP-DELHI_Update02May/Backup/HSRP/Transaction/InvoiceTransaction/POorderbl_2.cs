using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HSRP.Transaction.InvoiceTransaction
{
    class POorderbl
    {
        public int ddlVendorID, ddlProductID, txtQuantity, ID;
        public float txtRate;
        public string TransporterName, GoodsReceiptNote, TransportVia, txtLaserCodeNoto, txtMesurementUnit, txtLaserCodeNoform, ddlProductName, VendorName;

        public string O_Staus
        {

            get;
            set;
        }
        public string O_ddlProductName
        {

            get
            {
                return ddlProductName;
            }
            set
            {
                ddlProductName = value;
            }
        }
        public string O_VendorName
        {

            get
            {
                return VendorName;
            }
            set
            {
                VendorName = value;
            }
        }


        public int O_ID
        {

            get
            {
                return ID;
            }
            set
            {
                ID = value;
            }
        }
        public int O_VendorID
        {

            get
            {
                return ddlVendorID;
            }
            set
            {
                ddlVendorID = value;
            }
        }

        public string O_Remark
        {

            get;

            set;

        }

        public string O_PaymentTerm
        {

            get;

            set;

        }

        public string O_Date
        {

            get;

            set;

        }

        public int O_ddlProductID
        {
            get
            {
                return ddlProductID;
            }
            set
            {
                ddlProductID = value;
            }
        }
        public int O_txtQuantity
        {
            get
            {
                return txtQuantity;
            }
            set
            {
                txtQuantity = value;
            }
        }
        public float O_txtRate
        {
            get
            {
                return txtRate;
            }
            set
            {
                txtRate = value;
            }
        }
        public string O_txtMesurementUnit
        {
            get
            {
                return txtMesurementUnit;
            }
            set
            {
                txtMesurementUnit = value;
            }
        }
        public string O_txtLaserCodeNoform
        {
            get
            {
                return txtLaserCodeNoform;
            }
            set
            {
                txtLaserCodeNoform = value;
            }
        }
        public string O_txtLaserCodeNoto
        {
            get
            {
                return txtLaserCodeNoto;
            }
            set
            {
                txtLaserCodeNoto = value;
            }
        }
        public string billingAddress
        {
            get;
            set;
        }
        public string Shippingaddress
        {
            get;
            set;
        }
    }
}
