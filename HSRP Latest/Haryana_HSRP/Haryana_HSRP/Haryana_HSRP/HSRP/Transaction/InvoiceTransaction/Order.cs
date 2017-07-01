using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Collections;

namespace HSRP.Master.InvoiceMaster
{
    public class Order:IEnumerable
    {

        public int CustomerID,ddlProductID, txtQuantity,ID;
        public float txtRate;
        public string TransporterName, GoodsReceiptNote, TransportVia, txtLaserCodeNoto, txtMesurementUnit, txtLaserCodeNoform, ddlProductName, CustomerName;

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
        public string O_CustomerName
        {

            get
            {
                return CustomerName;
            }
            set
            {
                CustomerName = value;
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
        public int O_CustomerID
        {

            get
            {
                return CustomerID;
            }
            set
            {
                CustomerID = value;
            }
        }
        public string O_TransporterName
        {
            get
            {
                return TransporterName ;
            }
            set
            {
                TransporterName=value;
            }
        }
        public string O_TransportVia
        {
            get
            {
                return TransportVia ;
            }
            set
            {
                TransportVia=value;
            }
        }
        public string O_GoodsReceiptNote
        {
            get
            {
                return GoodsReceiptNote ;
            }
            set
            {
                GoodsReceiptNote=value;
            }
        }
         public int O_ddlProductID
        {
            get
            {
                return ddlProductID ;
            }
            set
            {
                ddlProductID=value;
            }
        }
         public int O_txtQuantity
        {
            get
            {
                return txtQuantity ;
            }
            set
            {
                txtQuantity=value;
            }
        }
         public float O_txtRate
        {
            get
            {
                return txtRate ;
            }
            set
            {
                txtRate=value;
            }
        }
         public string O_txtMesurementUnit
        {
            get
            {
                return txtMesurementUnit ;
            }
            set
            {
                txtMesurementUnit=value;
            }
        }
         public string O_txtLaserCodeNoform
        {
            get
            {
                return txtLaserCodeNoform ;
            }
            set
            {
                txtLaserCodeNoform=value;
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
        

         //internal Order insertorder(List<string> lst)
         //{
             
         //    OCustomerID = int.Parse(lst[0]);
         //    OTransporterName = lst[1];
         //    OGoodsReceiptNote = lst[2];
         //    OTransportVia = lst[3];
         //    OddlProductName = int.Parse(lst[4]);
         //    OtxtQuantity = int.Parse(lst[5]);
         //    OtxtRate = int.Parse(lst[6]);
         //    OtxtMesurementUnit = lst[7];
         //    OtxtLaserCodeNoform = lst[8];
         //    OtxtLaserCodeNoto = lst[9];

         //    Order obj = new Order();
         //    return obj;
            

         //}

         public IEnumerator GetEnumerator()
         {
             throw new NotImplementedException();
         }
    }
}