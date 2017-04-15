using System;
using System.Data;
using System.Windows.Forms;
using HSRPTransferData;
using System.ComponentModel;

using System.IO;
using System.Drawing.Printing;
using System.Drawing;
using System.Net;
using System.Net.NetworkInformation;
namespace HSRPDataEntry
{
    public partial class CashReceiptNB : Form
    {

        #region Creating Object Of Web Service
        HSRPDataEntry.HSRPService.HSRPService objHSRP = new HSRPDataEntry.HSRPService.HSRPService();
        #endregion

        #region Variable Declaration
        string Query = string.Empty;
        string Sticker = string.Empty;
        string Vip = string.Empty;
        string recno = string.Empty;
        string address = string.Empty;
        string vehiclemake = string.Empty;
        string vehiclemodel = string.Empty;
        int RTO_CD = 0;
        DataTable dt1 = new DataTable();
        string Detial = string.Empty;
        string Stateid = string.Empty;
        string StateName = string.Empty;
        string RTOLocationCode = string.Empty;
        string ReceiptSizeA4 = string.Empty;
        string PrinterName1 = string.Empty;
        string DataFolder = string.Empty;
        string DataFileFolder = string.Empty;
        string EXLFileFolder = string.Empty;
        string CompanyName = string.Empty;
        string RTOLocationAddress = string.Empty;

        string Prefix;
        string CashRecieptPrinterType = string.Empty;
        string CashRecieptPrinterName = string.Empty;
        string orderType;
        float amount = 0;
        string GetAddress;

        string AuthNo = string.Empty;
        string OwnerName = string.Empty;
        string VehicleType = string.Empty;
        string OrderType = string.Empty;
        string VehicleRegno = string.Empty;
        string VehicleModel = string.Empty;
        string VehicleClass = string.Empty;
        string EngineNo = string.Empty;
        string AuthDate1 = string.Empty;
        string ChassisNo = string.Empty;
        string CreatedBy = string.Empty;
        #endregion

        #region Print Function
        private void PrintDocument()
        {
            HSRPDataEntry.HSRPService.HSRPService objHSRP = new HSRPDataEntry.HSRPService.HSRPService();
            Detial = utils.GetLocalDBConnectionFromINI();
            #region Fetching Value from String Using ^ Keyword
            string[] DetailData = Detial.Split('^');
            DataFolder = DetailData[0];
            DataFileFolder = DetailData[1];
            EXLFileFolder = DetailData[2];
            Stateid = DetailData[3];
            StateName = DetailData[4];
            RTOLocationCode = DetailData[5];
            CompanyName = DetailData[6];
            RTOLocationAddress = DetailData[7];
            ReceiptSizeA4 = DetailData[8];
            PrinterName1 = DetailData[9];
            dt1 = objHSRP.Chalan(Stateid);
            #endregion


            string companyname = CompanyName;
            string ComAdd = objHSRP.GetRtoAddress("4", utils.RtoLocationId);
            int a = 18;

            if (radioButton1.Checked == true)
            {
                Sticker = "Y";
            }
            else
            {
                Sticker = "N";
            }

            if (chkVIP.Checked == true)
            {
                Vip = "Y";
            }
            else
            {
                Vip = "N";
            }

            PrintDocument p = new PrintDocument();
            // printerName
            p.PrinterSettings.PrinterName = CashRecieptPrinterName;

            p.PrintPage += delegate(object sender1, PrintPageEventArgs ee)
            {
                //dt1.Rows[0]["PrifixNo"].ToString()
                ee.Graphics.DrawString(CompanyName, new Font("Times New Roman", 7), new SolidBrush(Color.Black), new Rectangle(0, 0, 950, 950));
                ee.Graphics.DrawString(ComAdd, new Font("Times New Roman", 8), new SolidBrush(Color.Black), new Rectangle(0, 18, 300, 300));
                ee.Graphics.DrawString("Receipt Date       : " + System.DateTime.Now.ToString("dd-MM-yyyy"), new Font("Times New Roman", 10), new SolidBrush(Color.Black), new Rectangle(0, 60, 300, 300));
                ee.Graphics.DrawString("Receipt No         :  " + recno, new Font("Times New Roman", 10), new SolidBrush(Color.Black), new Rectangle(0, 80, 300, 300));
                ee.Graphics.DrawString("Authorization No :  " + txtAuthorizationNo.Text, new Font("Times New Roman", 10), new SolidBrush(Color.Black), new Rectangle(0, 100, 300, 300));
                ee.Graphics.DrawString("Vehicle Reg No   :  " + txtVehRegNo.Text, new Font("Times New Roman", 10), new SolidBrush(Color.Black), new Rectangle(0, 120, 300, 300));
                ee.Graphics.DrawString("Owner Name       :  " + txtOwnerName.Text, new Font("Times New Roman", 10), new SolidBrush(Color.Black), new Rectangle(0, 140, 300, 300));
                ee.Graphics.DrawString("Mobile No          :  " + txtMobileNo.Text, new Font("Times New Roman", 10), new SolidBrush(Color.Black), new Rectangle(0, 160, 300, 300));
                ee.Graphics.DrawString("Vehicle Class      :  " + ddlVehicleClass.Text, new Font("Times New Roman", 10), new SolidBrush(Color.Black), new Rectangle(0, 180, 300, 300));
                ee.Graphics.DrawString("Vehicle Type      :  " + ddlVehType.Text, new Font("Times New Roman", 10), new SolidBrush(Color.Black), new Rectangle(0, 200, 300, 300));
                ee.Graphics.DrawString("Order Type         :  " + DDLOrderType.Text, new Font("Times New Roman", 10), new SolidBrush(Color.Black), new Rectangle(0, 220, 300, 300));
                ee.Graphics.DrawString("Sticker               :  " + Sticker, new Font("Times New Roman", 10), new SolidBrush(Color.Black), new Rectangle(0, 240, 300, 300));
                ee.Graphics.DrawString("VIP                    :  " + Vip, new Font("Times New Roman", 10), new SolidBrush(Color.Black), new Rectangle(0, 260, 300, 100));
                ee.Graphics.DrawString("Tax                    :  " + txtTax.Text, new Font("Times New Roman", 10), new SolidBrush(Color.Black), new Rectangle(0, 280, 300, 300));
                ee.Graphics.DrawString("Amount              :  " + txtAmount.Text, new Font("Times New Roman", 10), new SolidBrush(Color.Black), new Rectangle(0, 300, 300, 300));
                ee.Graphics.DrawString("Net Amount              :  " + System.Math.Round(Convert.ToDecimal(txtAmount.Text), 0), new Font("Times New Roman", 10), new SolidBrush(Color.Black), new Rectangle(0, 320, 300, 300));
                ee.Graphics.DrawString("Disclaimer : ", new Font("Times New Roman", 9), new SolidBrush(Color.Black), new Rectangle(0, 340, 300, 300));
                ee.Graphics.DrawString("Before leaving the counter Vehicle  owner should check receipt details, failing which the company shall not be responsible in any manner.", new Font("Times New Roman", 8), new SolidBrush(Color.Black), new Rectangle(0, 360, 300, 300));
                ee.Graphics.DrawString("Vehicle Owner is requested to please check the Correctness of the cash slip. The company shall not be responsible for any clerical mistake what so ever.", new Font("Times New Roman", 8), new SolidBrush(Color.Black), new Rectangle(0, 410, 260, 300));

                ee.Graphics.DrawString(".", new Font("Times New Roman", 5), new SolidBrush(Color.Black), new Rectangle(0, 470, 300, 300));
            };
            try
            {
                for (int i = 0; i < 2; i++)
                {
                    p.Print();
                }
                //Refresh();
            }
            catch (Exception ex)
            {
                throw new Exception("Exception Occured While Printing", ex);
            }
        }

        private void PrintDocument2()
        {
            HSRPDataEntry.HSRPService.HSRPService objHSRP = new HSRPDataEntry.HSRPService.HSRPService();
            Detial = utils.GetLocalDBConnectionFromINI();
            string[] DetailData = Detial.Split('^');
            DataFolder = DetailData[0];
            DataFileFolder = DetailData[1];
            EXLFileFolder = DetailData[2];
            Stateid = DetailData[3];
            StateName = DetailData[4];
            RTOLocationCode = DetailData[5];
            CompanyName = DetailData[6];
            RTOLocationAddress = DetailData[7];
            ReceiptSizeA4 = DetailData[8];
            PrinterName1 = DetailData[9];
            dt1 = objHSRP.Chalan(Stateid);

            DataTable dtadd = new DataTable();

            string companyname = CompanyName;
            string ComAdd = objHSRP.GetRtoAddress("4", utils.RtoLocationId);
            string Heading = "CASH RECEIPT";

            if (radioButton1.Checked == true)
            {
                Sticker = "Y";
            }
            else
            {
                Sticker = "N";
            }

            if (chkVIP.Checked == true)
            {
                Vip = "Y";
            }
            else
            {
                Vip = "N";
            }

            PrintDocument p = new PrintDocument();
            // printerName
            p.PrinterSettings.PrinterName = CashRecieptPrinterName;
            p.PrintPage += delegate(object sender1, PrintPageEventArgs ee)
            {
                ee.Graphics.DrawString(CompanyName, new Font("Times New Roman", 7), new SolidBrush(Color.Black), new Rectangle(0, 0, 950, 950));
                ee.Graphics.DrawString(ComAdd, new Font("Times New Roman", 8), new SolidBrush(Color.Black), new Rectangle(0, 18, 300, 300));
                ee.Graphics.DrawString("Receipt Date       : " + System.DateTime.Now.ToString("dd-MM-yyyy"), new Font("Times New Roman", 10), new SolidBrush(Color.Black), new Rectangle(0, 60, 300, 300));
                ee.Graphics.DrawString("Receipt No         :  " + recno, new Font("Times New Roman", 10), new SolidBrush(Color.Black), new Rectangle(0, 80, 300, 300));
                ee.Graphics.DrawString("Authorization No :  " + txtAuthorizationNo.Text, new Font("Times New Roman", 10), new SolidBrush(Color.Black), new Rectangle(0, 100, 300, 300));
                ee.Graphics.DrawString("Vehicle Reg No   :  " + txtVehRegNo.Text, new Font("Times New Roman", 10), new SolidBrush(Color.Black), new Rectangle(0, 120, 300, 300));
                ee.Graphics.DrawString("Owner Name       :  " + txtOwnerName.Text, new Font("Times New Roman", 10), new SolidBrush(Color.Black), new Rectangle(0, 140, 300, 300));
                ee.Graphics.DrawString("Mobile No          :  " + txtMobileNo.Text, new Font("Times New Roman", 10), new SolidBrush(Color.Black), new Rectangle(0, 160, 300, 300));
                ee.Graphics.DrawString("Vehicle Class      :  " + ddlVehicleClass.Text, new Font("Times New Roman", 10), new SolidBrush(Color.Black), new Rectangle(0, 180, 300, 300));
                ee.Graphics.DrawString("Vehicle Type      :  " + ddlVehType.Text, new Font("Times New Roman", 10), new SolidBrush(Color.Black), new Rectangle(0, 200, 300, 300));
                ee.Graphics.DrawString("Order Type         :  " + DDLOrderType.Text, new Font("Times New Roman", 10), new SolidBrush(Color.Black), new Rectangle(0, 220, 300, 300));
                ee.Graphics.DrawString("Sticker               :  " + Sticker, new Font("Times New Roman", 10), new SolidBrush(Color.Black), new Rectangle(0, 240, 300, 300));
                ee.Graphics.DrawString("VIP                    :  " + Vip, new Font("Times New Roman", 10), new SolidBrush(Color.Black), new Rectangle(0, 260, 300, 100));
                ee.Graphics.DrawString("Tax                    :  " + txtTax.Text, new Font("Times New Roman", 10), new SolidBrush(Color.Black), new Rectangle(0, 280, 300, 300));
                ee.Graphics.DrawString("Amount              :  " + txtAmount.Text, new Font("Times New Roman", 10), new SolidBrush(Color.Black), new Rectangle(0, 300, 300, 300));
                ee.Graphics.DrawString("Net Amount              :  " + System.Math.Round(Convert.ToDecimal(txtAmount.Text), 0), new Font("Times New Roman", 10), new SolidBrush(Color.Black), new Rectangle(0, 320, 300, 300));
                ee.Graphics.DrawString("Disclaimer : ", new Font("Times New Roman", 9), new SolidBrush(Color.Black), new Rectangle(0, 340, 300, 300));
                ee.Graphics.DrawString("Before leaving the counter Vehicle  owner should check receipt details, failing which the company shall not be responsible in any manner.", new Font("Times New Roman", 9), new SolidBrush(Color.Black), new Rectangle(0, 360, 300, 300));
                ee.Graphics.DrawString("Vehicle Owner is requested to please check the Correctness of the cash slip. The company shall not be responsible for any clerical mistake what so ever.", new Font("Times New Roman", 10), new SolidBrush(Color.Black), new Rectangle(0, 400, 260, 300));

                ee.Graphics.DrawString(".", new Font("Times New Roman", 5), new SolidBrush(Color.Black), new Rectangle(0, 410, 300, 300));

            };
            try
            {
                p.Print();
                Refresh();
            }
            catch (Exception ex)
            {
                throw new Exception("Exception Occured While Printing", ex);
            }
        }

        public void pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            HSRPDataEntry.HSRPService.HSRPService objHSRP = new HSRPDataEntry.HSRPService.HSRPService();
            Detial = utils.GetLocalDBConnectionFromINI();
            string[] DetailData = Detial.Split('^');
            DataFolder = DetailData[0];
            DataFileFolder = DetailData[1];
            EXLFileFolder = DetailData[2];
            Stateid = DetailData[3];
            StateName = DetailData[4];
            RTOLocationCode = DetailData[5];
            CompanyName = DetailData[6];
            RTOLocationAddress = DetailData[7];
            ReceiptSizeA4 = DetailData[8];
            PrinterName1 = DetailData[9];
            dt1 = objHSRP.Chalan(Stateid);




            string companyname = CompanyName;
            string ComAdd = objHSRP.GetRtoAddress("4", utils.RtoLocationId);
            string Heading = "CASH RECEIPT";
            string CashReceiptNo = recno;
            string DATE = System.DateTime.Now.ToString("dd-MM-yyyy");
            string TINNO = dt1.Rows[0]["TinNo"].ToString(), TIME = System.DateTime.Now.ToString("HH:MM");

            string AUTHNO = txtAuthorizationNo.Text, AUTHDATE = AuthDate.Text, ORDERBOOKINGNO = " ", OWNERNAME = txtOwnerName.Text, CONTACTNO = txtMobileNo.Text, OWNERADDRESS = " ", VEHICLEREG = txtVehRegNo.Text, VEHICLEMODEL = ddlVehType.Text, ENGINENO = txtEngine.Text, CHASSISNO = txtChassis.Text;
            string AMOUNT = txtAmount.Text;
            string ROUNDOFAMOUNT = txtAmount.Text;
            string NETAMOUNT = txtAmount.Text;

            FontFamily fontFamily = new FontFamily("Lucida Console");

            StringFormat stringFormat = new StringFormat();
            SolidBrush solidBrush = new SolidBrush(Color.FromArgb(255, 0, 0, 255));

            // stringFormat.FormatFlags = StringFormatFlags.DirectionVertical;

            // stringFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft;

            // HEADING OF PDF
            Font font1111 = new Font(fontFamily, 18, FontStyle.Regular, GraphicsUnit.Point);
            PointF pointF = new PointF(120, 20);
            e.Graphics.DrawString(companyname, font1111, solidBrush, pointF, stringFormat);


            Font font1 = new Font(fontFamily, 9, FontStyle.Regular, GraphicsUnit.Point);
            PointF pointF1 = new PointF(100, 50);
            e.Graphics.DrawString(ComAdd, font1, solidBrush, pointF1, stringFormat);


            Font font2 = new Font(fontFamily, 13, FontStyle.Regular, GraphicsUnit.Point);
            PointF pointF2 = new PointF(365, 80);
            e.Graphics.DrawString(Heading, font2, solidBrush, pointF2, stringFormat);


            // header 

            Font font3 = new Font(fontFamily, 10, FontStyle.Regular, GraphicsUnit.Point);
            PointF pointF3 = new PointF(70, 110);   // left side
            PointF pointF4 = new PointF(550, 110);   // right side


            e.Graphics.DrawString("Cash Receipt No : " + CashReceiptNo, font3, solidBrush, pointF3, stringFormat);
            e.Graphics.DrawString("DATE : " + DATE, font3, solidBrush, pointF4, stringFormat);

            PointF pointF5 = new PointF(70, 130);   // left side
            PointF pointF6 = new PointF(550, 130);   // right side
            e.Graphics.DrawString("TIN NO. : " + TINNO, font3, solidBrush, pointF5, stringFormat);
            e.Graphics.DrawString("TIME : " + TIME, font3, solidBrush, pointF6, stringFormat);

            PointF pointF7 = new PointF(50, 150);
            e.Graphics.DrawString("---------------------------------------------------------------------------------------", font3, solidBrush, pointF7, stringFormat);


            //middlepart
            // ORDERBOOKINGNO = "anil singh ", OWNERNAME = "SKY GOURMET CATERING PVT OWNER", CONTACTNO = "989097878612", OWNERADDRESS = "delhi", VEHICLEREG = "DL1GB7078", VEHICLEMODEL = "MCV/HCV/TRAILERS", ENGINENO = "E683CD9L501471", CHASSISNO = "MC230MRC09L015552";
            PointF pointF8 = new PointF(70, 180);   // left side
            PointF pointF9 = new PointF(500, 180);   // right side
            e.Graphics.DrawString("AUTH NO : " + AUTHNO, font3, solidBrush, pointF8, stringFormat);
            e.Graphics.DrawString("AUTH DATE : " + AUTHDATE, font3, solidBrush, pointF9, stringFormat);

            //PointF pointF10 = new PointF(70, 180);   // left side
            //PointF pointF11 = new PointF(500, 180);   // right side
            //e.Graphics.DrawString("ORDER BOOKING NO : " + ORDERBOOKINGNO, font3, solidBrush, pointF10, stringFormat);


            PointF pointF12 = new PointF(70, 200);   // left side
            PointF pointF13 = new PointF(500, 200);   // right side
            e.Graphics.DrawString("OWNER NAME : " + OWNERNAME, font3, solidBrush, pointF12, stringFormat);
            e.Graphics.DrawString("TIME : " + TIME, font3, solidBrush, pointF13, stringFormat);

            PointF pointF14 = new PointF(70, 220);   // left side
            PointF pointF15 = new PointF(500, 220);   // right side
            e.Graphics.DrawString("AUTH NO : " + AUTHNO, font3, solidBrush, pointF14, stringFormat);
            e.Graphics.DrawString("CONTACT NO : " + CONTACTNO, font3, solidBrush, pointF15, stringFormat);

            //PointF pointF16 = new PointF(70, 260);   // left side
            //PointF pointF17 = new PointF(500, 260);   // right side
            //e.Graphics.DrawString("OWNER ADDRESS : " + OWNERADDRESS, font3, solidBrush, pointF16, stringFormat);


            PointF pointF18 = new PointF(70, 240);   // left side
            PointF pointF19 = new PointF(500, 240);   // right side
            e.Graphics.DrawString("VEHICLE REG : " + VEHICLEREG, font3, solidBrush, pointF18, stringFormat);
            e.Graphics.DrawString("VEHICLE MODEL : " + VEHICLEMODEL, font3, solidBrush, pointF19, stringFormat);


            PointF pointF20 = new PointF(70, 260);   // left side
            PointF pointF21 = new PointF(500, 260);   // right side
            e.Graphics.DrawString("ENGINE NO : " + ENGINENO, font3, solidBrush, pointF20, stringFormat);
            e.Graphics.DrawString("CHASSISNO : " + CHASSISNO, font3, solidBrush, pointF21, stringFormat);

            PointF pointF22 = new PointF(50, 280);
            e.Graphics.DrawString("---------------------------------------------------------------------------------------", font3, solidBrush, pointF22, stringFormat);


            PointF pointF23 = new PointF(70, 300);   // left side
            PointF pointF24 = new PointF(500, 300);   // right side
            e.Graphics.DrawString("DESCRIPTION ", font3, solidBrush, pointF23, stringFormat);
            e.Graphics.DrawString("AMOUNT (RS) ", font3, solidBrush, pointF24, stringFormat);



            PointF pointF25 = new PointF(70, 320);   // left side
            PointF pointF26 = new PointF(500, 320);   // right side
            e.Graphics.DrawString("SET OF HSRP PLATE", font3, solidBrush, pointF25, stringFormat);
            e.Graphics.DrawString(AMOUNT.ToString(), font3, solidBrush, pointF26, stringFormat);

            PointF pointF27 = new PointF(50, 340);
            e.Graphics.DrawString("---------------------------------------------------------------------------------------", font3, solidBrush, pointF27, stringFormat);

            PointF pointF28 = new PointF(70, 360);   // left side
            PointF pointF29 = new PointF(500, 360);   // right side
            e.Graphics.DrawString("NET AMOUNT ", font3, solidBrush, pointF28, stringFormat);
            e.Graphics.DrawString(NETAMOUNT.ToString(), font3, solidBrush, pointF29, stringFormat);



            PointF pointF30 = new PointF(70, 380);   // left side
            PointF pointF31 = new PointF(500, 380);   // right side
            e.Graphics.DrawString("ROUND OF AMOUNT", font3, solidBrush, pointF30, stringFormat);
            e.Graphics.DrawString(ROUNDOFAMOUNT.ToString(), font3, solidBrush, pointF31, stringFormat);

            Font font12 = new Font(fontFamily, 8, FontStyle.Regular, GraphicsUnit.Point);
            PointF pointF32 = new PointF(70, 400);   // left side
            e.Graphics.DrawString("Before leaving the counter Vehicle  owner should check receipt details, failing which the company shall not", font12, solidBrush, pointF32, stringFormat);

            PointF pointF322 = new PointF(70, 420);   // left side
            e.Graphics.DrawString("be responsible in any manner.Vehicle Owner is requested to please check the Correctness of the cash slip. ", font12, solidBrush, pointF322, stringFormat);



            PointF pointF33 = new PointF(70, 440);   // left side
            e.Graphics.DrawString("The company shall not be responsible for any clerical mistake what so ever.", font12, solidBrush, pointF33, stringFormat);




            PointF pointF34 = new PointF(480, 460);   // left side
            e.Graphics.DrawString(companyname, font3, solidBrush, pointF34, stringFormat);


            PointF pointF35 = new PointF(550, 510);   // left side
            e.Graphics.DrawString("(AUTH. SIGH.) ", font3, solidBrush, pointF35, stringFormat);


            // send page


            //FontFamily fontFamily = new FontFamily("Lucida Console");

            //StringFormat stringFormat = new StringFormat();
            //SolidBrush solidBrush = new SolidBrush(Color.FromArgb(255, 0, 0, 255));

            // stringFormat.FormatFlags = StringFormatFlags.DirectionVertical;

            // stringFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft;

            // HEADING OF PDF

            PointF pointF36 = new PointF(50, 530);
            e.Graphics.DrawString("---------------------------------------------------------------------------------------", font3, solidBrush, pointF36, stringFormat);



            Font font38 = new Font(fontFamily, 20, FontStyle.Regular, GraphicsUnit.Point);
            PointF pointF38 = new PointF(120, 550);
            e.Graphics.DrawString(companyname, font38, solidBrush, pointF38, stringFormat);


            Font font39 = new Font(fontFamily, 9, FontStyle.Regular, GraphicsUnit.Point);
            PointF pointF39 = new PointF(70, 580);
            e.Graphics.DrawString(ComAdd, font39, solidBrush, pointF39, stringFormat);


            Font font40 = new Font(fontFamily, 13, FontStyle.Regular, GraphicsUnit.Point);
            PointF pointF40 = new PointF(365, 610);
            e.Graphics.DrawString(Heading, font40, solidBrush, pointF40, stringFormat);


            // header 

            //Font font3 = new Font(fontFamily, 10, FontStyle.Regular, GraphicsUnit.Point);
            PointF pointF41 = new PointF(70, 630);   // left side
            PointF pointF42 = new PointF(550, 630);   // right side


            e.Graphics.DrawString("Cash Receipt No : " + CashReceiptNo, font3, solidBrush, pointF41, stringFormat);
            e.Graphics.DrawString("DATE : " + DATE, font3, solidBrush, pointF42, stringFormat);

            PointF pointF43 = new PointF(70, 650);   // left side
            PointF pointF44 = new PointF(550, 650);   // right side
            e.Graphics.DrawString("TIN NO. : " + TINNO, font3, solidBrush, pointF43, stringFormat);
            e.Graphics.DrawString("TIME : " + TIME, font3, solidBrush, pointF44, stringFormat);

            PointF pointF45 = new PointF(50, 670);
            e.Graphics.DrawString("---------------------------------------------------------------------------------------", font3, solidBrush, pointF45, stringFormat);


            //middlepart
            // ORDERBOOKINGNO = "anil singh ", OWNERNAME = "SKY GOURMET CATERING PVT OWNER", CONTACTNO = "989097878612", OWNERADDRESS = "delhi", VEHICLEREG = "DL1GB7078", VEHICLEMODEL = "MCV/HCV/TRAILERS", ENGINENO = "E683CD9L501471", CHASSISNO = "MC230MRC09L015552";
            PointF pointF46 = new PointF(70, 690);   // left side
            PointF pointF47 = new PointF(500, 690);   // right side
            e.Graphics.DrawString("AUTH NO : " + AUTHNO, font3, solidBrush, pointF46, stringFormat);
            e.Graphics.DrawString("AUTH DATE : " + AUTHDATE, font3, solidBrush, pointF47, stringFormat);

            //PointF pointF48 = new PointF(70, 710);   // left side
            //PointF pointF49 = new PointF(500, 710);   // right side
            //e.Graphics.DrawString("ORDER BOOKING NO : " + ORDERBOOKINGNO, font3, solidBrush, pointF48, stringFormat);


            PointF pointF50 = new PointF(70, 710);   // left side
            PointF pointF51 = new PointF(500, 710);   // right side
            e.Graphics.DrawString("OWNER NAME : " + OWNERNAME, font3, solidBrush, pointF50, stringFormat);
            e.Graphics.DrawString("TIME : " + TIME, font3, solidBrush, pointF51, stringFormat);

            PointF pointF52 = new PointF(70, 730);   // left side
            PointF pointF53 = new PointF(500, 730);   // right side
            e.Graphics.DrawString("AUTH NO : " + AUTHNO, font3, solidBrush, pointF52, stringFormat);
            e.Graphics.DrawString("CONTACT NO : " + CONTACTNO, font3, solidBrush, pointF53, stringFormat);

            //PointF pointF54 = new PointF(70, 770);   // left side
            //PointF pointF55 = new PointF(500, 770);   // right side
            //e.Graphics.DrawString("OWNER ADDRESS : " + OWNERADDRESS, font3, solidBrush, pointF54, stringFormat);


            PointF pointF56 = new PointF(70, 750);   // left side
            PointF pointF57 = new PointF(500, 750);   // right side
            e.Graphics.DrawString("VEHICLE REG : " + VEHICLEREG, font3, solidBrush, pointF56, stringFormat);
            e.Graphics.DrawString("VEHICLE MODEL : " + VEHICLEMODEL, font3, solidBrush, pointF57, stringFormat);


            PointF pointF58 = new PointF(70, 770);   // left side
            PointF pointF59 = new PointF(500, 770);   // right side
            e.Graphics.DrawString("ENGINE NO : " + ENGINENO, font3, solidBrush, pointF58, stringFormat);
            e.Graphics.DrawString("CHASSISNO : " + CHASSISNO, font3, solidBrush, pointF59, stringFormat);

            PointF pointF60 = new PointF(50, 790);
            e.Graphics.DrawString("---------------------------------------------------------------------------------------", font3, solidBrush, pointF60, stringFormat);


            PointF pointF61 = new PointF(70, 810);   // left side
            PointF pointF62 = new PointF(500, 810);   // right side
            e.Graphics.DrawString("DESCRIPTION ", font3, solidBrush, pointF61, stringFormat);
            e.Graphics.DrawString("AMOUNT (RS) ", font3, solidBrush, pointF62, stringFormat);



            PointF pointF63 = new PointF(70, 830);   // left side
            PointF pointF64 = new PointF(500, 830);   // right side
            e.Graphics.DrawString("SET OF HSRP PLATE", font3, solidBrush, pointF63, stringFormat);
            e.Graphics.DrawString(AMOUNT.ToString(), font3, solidBrush, pointF64, stringFormat);

            PointF pointF65 = new PointF(50, 850);
            e.Graphics.DrawString("---------------------------------------------------------------------------------------", font3, solidBrush, pointF65, stringFormat);

            PointF pointF66 = new PointF(70, 870);   // left side
            PointF pointF67 = new PointF(500, 870);   // right side
            e.Graphics.DrawString("NET AMOUNT ", font3, solidBrush, pointF66, stringFormat);
            e.Graphics.DrawString(NETAMOUNT.ToString(), font3, solidBrush, pointF67, stringFormat);



            PointF pointF68 = new PointF(70, 890);   // left side
            PointF pointF69 = new PointF(500, 890);   // right side
            e.Graphics.DrawString("ROUND OF AMOUNT", font3, solidBrush, pointF68, stringFormat);
            e.Graphics.DrawString(ROUNDOFAMOUNT.ToString(), font3, solidBrush, pointF69, stringFormat);

            PointF pointF70 = new PointF(70, 910);   // left side
            e.Graphics.DrawString("Before leaving the counter Vehicle  owner should check receipt details, failing which the company shall not ", font12, solidBrush, pointF70, stringFormat);

            PointF pointF323 = new PointF(70, 930);   // left side
            e.Graphics.DrawString("be responsible in any manner.Vehicle Owner is requested to please check the Correctness of the cash slip. ", font12, solidBrush, pointF323, stringFormat);

            PointF pointF71 = new PointF(70, 950);   // left side
            e.Graphics.DrawString("The company shall not be responsible for any clerical mistake what so ever.", font12, solidBrush, pointF71, stringFormat);






            PointF pointF72 = new PointF(480, 970);   // left side
            e.Graphics.DrawString(companyname, font3, solidBrush, pointF72, stringFormat);


            PointF pointF73 = new PointF(550, 1020);   // left side
            e.Graphics.DrawString("(AUTH. SIGH.) ", font3, solidBrush, pointF73, stringFormat);
        }
        #endregion

        #region Drop Down
        private void ddlManufacturerMaker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlManufacturerMaker.Text == "--Select Vehicle Maker--")
            {
                ddlManufacturerMaker.Focus();
                return;
            }
            else
            {
                MethodVehicleModel1();
            }
        }

        private void ddlVehicleClass_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = (char)Keys.None;
        }

        private void ddlVehType_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = (char)Keys.None;
        }

        private void DDLOrderType_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = (char)Keys.None;
        }

        private void DDLOrderType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlVehicleClass.Text == "--Select Vehicle Class--")
            {
                MessageBox.Show("Please select Vehicle Class");
                ddlVehicleClass.Focus();
                return;
            }
            else if (ddlVehType.Text == "--Select Vehicle Type--")
            {
                MessageBox.Show("Please select Vehicle Type");
                ddlVehType.Focus();
                return;
            }
            else
            {
                HSRPDataEntry.HSRPService.HSRPService objHSRP = new HSRPDataEntry.HSRPService.HSRPService();
                string amount = objHSRP.GetRateAndTaxForVehicle(Stateid, ddlVehType.Text, ddlVehicleClass.Text, DDLOrderType.Text);
                string[] amount1 = amount.Split('^');
                txtAmount.Text = amount1[0].ToString();
                txtTax.Text = amount1[1].ToString();
            }
        }

        private void ddlManufacturerMaker_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = (char)Keys.None;
        }

        private void ddlManufacturerModel_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = (char)Keys.None;
        }

        private void MethodVehicleModel1()
        {
            DataTable dtVehicleModel = objHSRP.PopulateVehicleModel(ddlManufacturerMaker.SelectedValue.ToString());


            DataRow dr;
            dr = dtVehicleModel.NewRow();
            dr.ItemArray = new object[] { "--Select Vehicle Model--", "0" };
            dtVehicleModel.Rows.InsertAt(dr, 0);
            ddlManufacturerModel.DataSource = dtVehicleModel;
            ddlManufacturerModel.DisplayMember = "VehicleModelDescription";
            ddlManufacturerModel.ValueMember = "VehiclemodelID";
        }

        private void MethodVehicleMaker()
        {
            DataTable dtMaker = objHSRP.PopulateVehicleMake();
            DataRow dr;
            dr = dtMaker.NewRow();
            dr.ItemArray = new object[] { "--Select Vehicle Maker--", "0" };
            dtMaker.Rows.InsertAt(dr, 0);
            ddlManufacturerMaker.DataSource = dtMaker;
        }
        #endregion    

        #region Connection Check
        public static bool isConnected()
        {
            try
            {
                string myAddress = "www.google.com";
                IPAddress[] addresslist = Dns.GetHostAddresses(myAddress);

                if (addresslist[0].ToString().Length > 6)
                {
                    return true;
                }
                else
                    return false;

            }
            catch
            {
                return false;
            }

        }
        #endregion

        public CashReceiptNB(String strUserId)
        {
            CreatedBy = strUserId;
            InitializeComponent();
            RegistrationDate.Format = DateTimePickerFormat.Custom;
            RegistrationDate.CustomFormat = " ";
            AuthDate.Format = DateTimePickerFormat.Custom;
            AuthDate.CustomFormat = " ";
            label26.Visible = false;
            label27.Visible = false;
            textBox1.Visible = false;
           
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (CreatedBy == "5311")
            {
                System.Threading.Thread.Sleep(120000);
            }
            if (CreatedBy == "5354")
            {
                System.Threading.Thread.Sleep(120000);
            }

            if (CreatedBy == "0" || CreatedBy == "")
            {
                MessageBox.Show("User Not Valid For Collection.");
                return;
            }
            if (utils.RtoLocationId == "" || utils.RtoLocationId == "0")
            {
                MessageBox.Show("User Not Valid For Collection.");
                return;
            }
            //string addrecordby = "";
            bool i = isConnected();
            #region Validation Check

            if (i == true)
            {
                if (string.IsNullOrEmpty(txtVehRegNo.Text) || txtVehRegNo.Text== "")
                {
                    MessageBox.Show("Please Insert Vehicle Registration No.");

                    txtVehRegNo.Text = "";
                    txtVehRegNo.Focus();
                    return;
              }
              
                
              string strtxtVehRegNo=txtVehRegNo.Text.Trim().ToString();
              char[] chararrinput=strtxtVehRegNo.ToCharArray();

             for (int k = 0; k <  chararrinput.Length ; k++)
			 {
               if (chararrinput[k].ToString() == "." || chararrinput[k].ToString() == "|" || chararrinput[k].ToString() == ">" || chararrinput[k].ToString() == "<" || chararrinput[k].ToString() == "!" || chararrinput[k].ToString() == "@" || chararrinput[k].ToString() == "," || chararrinput[k].ToString() == "#" || chararrinput[k].ToString() == "*" || chararrinput[k].ToString() == "%" || chararrinput[k].ToString() == "&" || chararrinput[k].ToString() == "~" || chararrinput[k].ToString() == "-" || chararrinput[k].ToString() == "_" || chararrinput[k].ToString() == "(" || chararrinput[k].ToString() == ")" || chararrinput[k].ToString() == "`" || chararrinput[k].ToString() == " " || chararrinput[k].ToString() == "=")
              {
                MessageBox.Show("Special Character Not Allow In Vehicle Registration No.");
                txtVehRegNo.Text = ""; 
                txtVehRegNo.Focus();
                return;
                
              }

			 }



                HSRPDataEntry.HSRPService.HSRPService objHSRP = new HSRPDataEntry.HSRPService.HSRPService();


                DataTable dtbl = objHSRP.GetvehicleRegNo();

                if (dtbl.Rows.Count > 0)
                {

                    string VehicleNo = Convert.ToString(txtVehRegNo.Text).Trim();

                    for (int k = 0; k < dtbl.Rows.Count; k++)
                    {
                        if (VehicleNo.ToUpper() == Convert.ToString(dtbl.Rows[k]["VehicleregNo"]).ToUpper().Trim())
                        {

                            MessageBox.Show(" Please Contact With Administrator.");
                            txtVehRegNo.Text = "";
                            txtVehRegNo.Focus();
                            return;

                        }


                    }
                }

           
                string str = objHSRP.CheckDuplicateVehicleRegNo(txtVehRegNo.Text, CreatedBy);             

                if (str== "Fail")
                {
                    lblMessage.Text = "Duplicate Vehicle Registration No.";
                    return;
                }



                if (string.IsNullOrEmpty(txtAuthorizationNo.Text) || txtAuthorizationNo.Text == "" || txtAuthorizationNo.Text.ToString()=="0")
                {
                    MessageBox.Show("Please Insert Authorization No.");

                    txtAuthorizationNo.Text = "";
                    txtAuthorizationNo.Focus();
                    return;
                }
                if (AuthDate.Text == " ")
                {
                    MessageBox.Show("Please Insert Authorization Date.");
                    AuthDate.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(txtOwnerName.Text))
                {
                    MessageBox.Show("Please Insert Owner Name");

                    txtOwnerName.Text = "";
                    txtOwnerName.Focus();
                    return;
                }

                if ((string.IsNullOrEmpty(txtMobileNo.Text)) || (txtMobileNo.Text.Trim().Length < 10) || (txtMobileNo.Text.Trim().Length > 10))
                {
                    MessageBox.Show("Please Insert Mobile No.");

                    txtMobileNo.Text = "";
                    txtMobileNo.Focus();
                    return;
                }


                if (string.IsNullOrEmpty(txtChassis.Text))
                {
                    MessageBox.Show("Please Insert Chassis No.");

                    txtChassis.Text = "";
                    txtChassis.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(txtEngine.Text))
                {
                    MessageBox.Show("Please Insert Engine No.");

                    txtEngine.Text = "";
                    txtEngine.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(ddlVehicleClass.Text) || ddlVehicleClass.Text == "--Select Vehicle Class--" || String.IsNullOrEmpty(ddlVehicleClass.Text))
                {
                    MessageBox.Show("Please Select Vehicle Class.");
                    ddlVehicleClass.Focus();
                    return;
                }
                if (ddlVehType.Text == "--Select Vehicle Type--" || String.IsNullOrEmpty(ddlVehType.SelectedItem.ToString()))
                {
                    MessageBox.Show("Please Select Vehicle Type.");
                    ddlVehType.Focus();
                    return;
                }
                if (DDLOrderType.Text == "--Select Order Type--" || String.IsNullOrEmpty(DDLOrderType.SelectedItem.ToString()))
                {
                    MessageBox.Show("Please Select Order Type.");
                    DDLOrderType.Focus();
                    return;
                }



                if (txtAmount.Text == "")
                {
                    MessageBox.Show("Please Insert Amount.");
                    txtAmount.Text = "";
                    txtAmount.Focus();
                    return;
                }
                if (RegistrationDate.Text == " ")
                {
                    MessageBox.Show("Please Insert Vehicle Registration Date.");
                    RegistrationDate.Focus();
                    return;
                }

                double dmobile = unchecked(Convert.ToDouble(txtMobileNo.Text));
                bool checkValidation = Enum.IsDefined(typeof(MobileNoCheck), (long)Math.Round(dmobile));

                if (checkValidation)
                {
                    MessageBox.Show("Mobile No Is Not Valid");
                    txtMobileNo.Focus();
                    return;
                }

            #endregion

              
                string SaveMacAddress = string.Empty;
                string Addrecordby = "1";
                string CounterNo = string.Empty;
              
                
                if (radioButton1.Checked == true)
                {
                    Sticker = "Y";
                }
                else
                {
                    Sticker = "N";
                }

                if (chkVIP.Checked == true)
                {
                    Vip = "Y";
                }
                else
                {
                    Vip = "N";
                }


                

                string HSRPRecord_CreationDate = DateTime.Now.ToString("yyyy-MM-dd");
                string HSRP_StateID = Stateid;
                string RTOLocationID = RTOLocationCode;
                string HSRPRecord_AuthorizationNo = txtAuthorizationNo.Text.Trim();
                string HSRPRecord_AuthorizationDate = DateTime.Now.ToString("yyyy-MM-dd");
                string VehicleRegNo = txtVehRegNo.Text;
                string OwnerName = txtOwnerName.Text;

                string ownerFatherName = "";
                string Address1 = "";
                string OrderType = DDLOrderType.Text;

                #region Get Rate and Tax Detils For Vehicle
                string amount = objHSRP.GetRateAndTaxForVehicle(HSRP_StateID, ddlVehType.Text, ddlVehicleClass.Text, OrderType);
                string[] amount1 = amount.Split('^');
                txtAmount.Text = amount1[0].ToString();
                txtTax.Text = amount1[1].ToString();

                string NetAmount = (Math.Round(Convert.ToDecimal(amount1[2]), 0)).ToString();// amount1[2].ToString(); ;
                string Roundoff_NetAmount = (Math.Round(Convert.ToDecimal(NetAmount), 2)).ToString();
                string VehicleType = ddlVehType.Text;
                string OrderStatus = "NEW ORDER";
                string CashReceiptNo = "";
                string ChassisNo = txtChassis.Text;
                string EngineNo = txtEngine.Text;
                string DealerCode = "0";               
                
                #endregion         

                       
     
    
                #region Get Plate Rate Detail
                string getdata = objHSRP.GetRateAndTaxDetail(HSRP_StateID, ddlVehType.Text, ddlVehicleClass.Text, OrderType);
                string[] getdatavalue = getdata.Split('^');
                string FrontPlatePrize = getdatavalue[0].ToString();
                string ISFrontPlateSize = getdatavalue[1].ToString();
                string RearPlatePrize = getdatavalue[2].ToString();
                string ISRearPlateSize = getdatavalue[3].ToString();
                string StickerID = getdatavalue[4].ToString();
                string screwrate = getdatavalue[5].ToString();
                string Discount = getdatavalue[6].ToString();
                string VATAMOUNT = getdatavalue[7].ToString();
                string FrontPlateSize = getdatavalue[8].ToString();
                string RearPlateSize = getdatavalue[9].ToString();

                string ManufacturerMaker = string.Empty;
                string ManufacturerModel = string.Empty;
                string Reference = string.Empty;
                if ((ddlManufacturerMaker.SelectedValue == null) || (ddlManufacturerModel.SelectedValue == null))
                {
                    ManufacturerMaker = "";
                    ManufacturerModel = "";
                }
                else
                {
                    ManufacturerMaker = ddlManufacturerMaker.SelectedValue.ToString();
                    ManufacturerModel = ddlManufacturerModel.SelectedValue.ToString();
                }

               
                    string vehicleref = string.Empty;
                #endregion

                string issave = objHSRP.CheckDuplicateEntry(HSRP_StateID, txtAuthorizationNo.Text, txtVehRegNo.Text.Replace(" ", ""), DDLOrderType.Text);
                string[] issave1 = issave.Split('^');

                string flag = issave1[0].ToString();

                if (flag == "Fail")
                {
                    lblMessage.Text = "Duplicate Records";
                    return;
                }
                else if (flag == "HSRPRecord_AuthorizationNoDuplicate")
                {
                    lblMessage.Text = "Authorization No Duplicate";
                }
                else if (flag == "StatusAlreadyNew")
                {
                    lblMessage.Text = "Please first close previous order on This Vehicle No " + issave1[1].ToString()+"";
                }
                //else if (flag == "RecordExists")
                //{
                //    lblMessage.Text = "Record recived from government \n Please Book From launch 1.6 ";
                //}
                else
                {
                    SaveMacAddress = MacAddress();
                    string save = objHSRP.InsertDataCashCollection(HSRP_StateID, utils.RtoLocationId, txtAuthorizationNo.Text, AuthDate.Value.ToString("yyyy-MM-dd"), txtVehRegNo.Text.Replace(" ", ""), txtOwnerName.Text, ownerFatherName, Address1, txtMobileNo.Text, ddlVehicleClass.Text, DDLOrderType.Text, Sticker, Vip, NetAmount, Roundoff_NetAmount, ddlVehType.Text, OrderStatus, CashReceiptNo, txtChassis.Text, EngineNo, DealerCode, CreatedBy, SaveMacAddress, Addrecordby, ISFrontPlateSize, ISRearPlateSize, FrontPlateSize, RearPlateSize, Reference, ManufacturerModel, vehicleref, ManufacturerMaker, CounterNo, "",Convert.ToDateTime(RegistrationDate.Value.ToString("yyyy-MM-dd")));
                    string[] save1 = save.Split('^');
                    if (save1[0].ToString() == "Record Saved")
                    {
                       
                        recno = objHSRP.CashReceiptForHR(txtAuthorizationNo.Text);
                      
                        if (CashRecieptPrinterType == "A4")
                        {
                            PrintDocument pd = new PrintDocument();
                            pd.PrinterSettings.PrinterName = CashRecieptPrinterName;

                            pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);

                            pd.Print();
                            Refresh();
                        }
                        else
                        {
                          
                            PrintDocument();
                            Refresh();
                        }

                       
                    }
                    else if (save1[0].ToString() == "Mobile No Is Not Valid")
                    {
                        lblMessage.Text = "Mobile No Is Not Valid";
                    }
                    else
                    {
                        lblMessage.Text = "Record not Save";
                    }

                }
            }
            else
            {

                MessageBox.Show("No Internet Connection");

            }

        }

        public void Refresh()
        {
            txtVehRegNo.Text = "";
            txtOwnerName.Text = "";
            txtMobileNo.Text = "0000000000";
            txtAuthorizationNo.Text = "0";
            txtAmount.Text = "";
            //txtVehicleClass.Text = "";
            ddlVehType.Text = "";
            // txtOrderType.Text = "";
            ddlVehicleClass.Text = "--Select Vehicle Class--";
            txtRegNo.Text = "";
            txtEngineNo.Text = "";
            txtEngine.Text = "";
            txtChassis.Text = "";
            ddlVehType.Text = "--Select Vehicle Type--";
            DDLOrderType.Text = "--Select Order Type--";
            ddlManufacturerMaker.Text = "--Select Vehicle Maker--";
            ddlManufacturerModel.Text = "--Select Vehicle Model--";
            chkVIP.Checked = false;
            radioButton1.Checked = true;
            radioButton2.Checked = false;
            txtTax.Text = "";
            lblMessage.Text = "";
        }              

        private void Refresh1_Click_1(object sender, EventArgs e)
        {
            Refresh();
        }

        private void CashReciept_Load(object sender, EventArgs e)
        {
            groupBox1.Visible = false;

            Detial = utils.GetLocalDBConnectionFromINI();
            string[] DetailData = Detial.Split('^');
            DataFolder = DetailData[0];
            DataFileFolder = DetailData[1];
            EXLFileFolder = DetailData[2];
            Stateid = DetailData[3];
            StateName = DetailData[4];
            RTOLocationCode = DetailData[5];
            CompanyName = DetailData[6];
            RTOLocationAddress = DetailData[7];
            ReceiptSizeA4 = DetailData[8];
            PrinterName1 = DetailData[9];


            txtRegNo.Focus();

            //  --Select RTO STA--

            CashRecieptPrinterType = ReceiptSizeA4;

            if (CashRecieptPrinterType == "52")
            {
                CashRecieptPrinterName = PrinterName1;
                // enable true 52 mm button and false a4 print button

            }
            if (CashRecieptPrinterType == "A4")
            {
                // enable false  52 mm button and true a4 print button
                CashRecieptPrinterName = PrinterName1;
            }
            //select printer for 3rd sticler from DB
            MethodVehicleMaker();
            bindddlaffixCenter();


        }

        private void bindddlaffixCenter()
        {
            Detial = utils.GetLocalDBConnectionFromINI();
            string[] DetailData = Detial.Split('^');
            DataFolder = DetailData[0];
            DataFileFolder = DetailData[1];
            EXLFileFolder = DetailData[2];
            Stateid = DetailData[3];
            StateName = DetailData[4];
            //RTOLocationCode = DetailData[5];
            CompanyName = DetailData[6];
            RTOLocationAddress = DetailData[7];
            ReceiptSizeA4 = DetailData[8];
            PrinterName1 = DetailData[9];
        }

        private void btnAuthGO_Click(object sender, EventArgs e)
        {
            bool i = isConnected();
            if (i == true)
            {
                #region Validation Check           
                if (string.IsNullOrEmpty(txtVehRegNo.Text))
                {
                    MessageBox.Show("Please Insert Vehicle Registration No.");

                    txtVehRegNo.Text = "";
                    txtVehRegNo.Focus();
                    return;
                }
                #endregion

                HSRPDataEntry.HSRPService.HSRPService objHSRP = new HSRPDataEntry.HSRPService.HSRPService();
                dt1 = objHSRP.GetDataByVehicleRegNo(txtVehRegNo.Text.Replace(" ", ""));
                if (dt1.Rows.Count > 0)
                {
                    txtAuthorizationNo.Text = dt1.Rows[0]["HSRPRecord_AuthorizationNo"].ToString();
                    //  AuthDate.Value = dt1.Rows[0]["[HSRPRecord_AuthorizationDate]"];
                    txtOwnerName.Text = dt1.Rows[0]["OwnerName"].ToString();
                    txtMobileNo.Text = dt1.Rows[0]["MobileNo"].ToString();
                    txtChassis.Text = dt1.Rows[0]["ChassisNo"].ToString();
                    txtEngine.Text = dt1.Rows[0]["EngineNo"].ToString();
                    ddlVehicleClass.Text = dt1.Rows[0]["VehicleClass"].ToString();
                    ddlVehType.Text = dt1.Rows[0]["VehicleType"].ToString();
                    DDLOrderType.Text = dt1.Rows[0]["OrderType"].ToString();
                    ddlManufacturerMaker.Text = dt1.Rows[0]["ManufacturerName"].ToString();
                    ddlManufacturerModel.Text = dt1.Rows[0]["ManufacturerModel"].ToString();
                    txtAmount.Text = dt1.Rows[0]["TotalAmount"].ToString();  //NetAmount
                    txtTax.Text = dt1.Rows[0]["ServiceTax_Amount"].ToString();
                }

                else
                {
                    MessageBox.Show("Record Not Found");
                }
            }
            else
            {
                MessageBox.Show("No Internet Connection");
            }


        }

        #region Get MAC Address

        public string MacAddress()
        {
            String Mad=string.Empty;
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in nics)
            {
                IPInterfaceProperties properties = adapter.GetIPProperties();
                Mad = adapter.GetPhysicalAddress().ToString();

                break;

            }
            return Mad;
        }
        #endregion

        private void txtMobileNo_KeyPress(object sender, KeyPressEventArgs e)
        {
           if (char.IsDigit(e.KeyChar) || e.KeyChar=='\b')
            {
            }
            else
            {
                e.Handled = true;
            }
        }

        #region MobileNo Check Validation
        public enum MobileNoCheck : long
        {
            Zero = 0000000000,
            One = 1111111111,
            Two = 2222222222,
            Three = 3333333333,
            Four = 4444444444,
            Five = 5555555555,
            Six = 6666666666,
            Seven = 7777777777,
            Eight = 8888888888,
            Nine = 9999999999
        }
        #endregion
        string startDate;
        private void AuthDate_ValueChanged(object sender, EventArgs e)
        {
            startDate = Convert.ToString(AuthDate.Value);
            AuthDate.Format = DateTimePickerFormat.Short;
        }
        string startDate1;
        private void RegistrationDate_ValueChanged(object sender, EventArgs e)
        {
            startDate1 = Convert.ToString(RegistrationDate.Value);
            RegistrationDate.Format = DateTimePickerFormat.Short;
        }

        private void txtVehRegNo_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (e.KeyChar == '$' || e.KeyChar == '!' || e.KeyChar == '%' || e.KeyChar == '&' || e.KeyChar == '.' || e.KeyChar == '<' || e.KeyChar == '>' || e.KeyChar == '?' || e.KeyChar == ',' || e.KeyChar == '-' || e.KeyChar == '/' || e.KeyChar == ' ' || e.KeyChar == '@' || e.KeyChar == '*' || e.KeyChar == '(' || e.KeyChar == ')' || e.KeyChar == '[' || e.KeyChar == ']' || e.KeyChar == '~' || e.KeyChar == '`' || e.KeyChar == '_' || e.KeyChar == '+'|| e.KeyChar == '=')
            {
                e.Handled = false;        
                             
                MessageBox.Show("Special Character Not Allow In Vehicle Registration No.");
                txtVehRegNo.Text = ""; 
                return;
                
            }


        }
    }
}
