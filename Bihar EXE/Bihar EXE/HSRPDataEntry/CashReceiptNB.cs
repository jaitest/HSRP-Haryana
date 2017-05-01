using System;
using System.Data;
using System.Windows.Forms;
using HSRPTransferData;
using System.ComponentModel;

using System.IO;
using System.Drawing.Printing;
using System.Drawing;
using System.Net;
using System.Text;
namespace HSRPDataEntry
{
    public partial class CashReceiptNB : Form
    {
        public CashReceiptNB()
        {
            InitializeComponent();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

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
        string RTOLocationAddress = string.Empty;
        string ReceiptSizeA4 = string.Empty;
        string PrinterName1 = string.Empty;
        string DataFolder = string.Empty;
        string DataFileFolder = string.Empty;
        string EXLFileFolder = string.Empty;
        string CompanyName = string.Empty;
        string RTOLocationAddress1 = string.Empty;
        string strAffexaddress = string.Empty;
        string strholiday = string.Empty;
        string mobileNo = string.Empty;

        string ManufacturerMaker = string.Empty;
        string ManufacturerModel = string.Empty;


            



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


        private void btnSave_Click(object sender, EventArgs e)
        {
            lblMessage.Text = "";

            bool i = isConnected();
            if (i == true)
            {

                string CreatedBy = string.Empty;

                CreatedBy = Login.UserID;
                 string RTOLocationID = Login.RTOlocationID;

                 if (string.IsNullOrEmpty(CreatedBy) || Convert.ToInt32(CreatedBy) == 0 || string.IsNullOrWhiteSpace(CreatedBy) || CreatedBy == "")
                {
                    MessageBox.Show("User Not Valid For Collection");
                }



                 if (string.IsNullOrEmpty(RTOLocationID) || Convert.ToInt32(RTOLocationID) == 0 || string.IsNullOrWhiteSpace(RTOLocationID) || RTOLocationID == "")
                {
                    MessageBox.Show("User Not Valid For Collection");
                }

                if (string.IsNullOrEmpty(txtVehRegNo.Text.Trim().ToString())  || string.IsNullOrWhiteSpace(txtVehRegNo.Text.Trim().ToString()))
                {
                    MessageBox.Show("Please Insert Vehicle Registration No.");
                    txtVehRegNo.Text = "";
                    txtVehRegNo.Focus();
                    return;
                }

                string strtxtVehRegNo = txtVehRegNo.Text.Trim().ToString();
                char[] chararrinput = strtxtVehRegNo.ToCharArray();


                if (chararrinput[0].ToString() != "B")
                {
                    MessageBox.Show("Please Insert Correct Vehicle Registration No.");
                    txtVehRegNo.Text = "";
                    txtVehRegNo.Focus();
                    return;
                }


                for (int k = 0; k < chararrinput.Length; k++)
                {
                    if (chararrinput[k].ToString() == "." || chararrinput[k].ToString() == "|" || chararrinput[k].ToString() == ">" || chararrinput[k].ToString() == "<" || chararrinput[k].ToString() == "!" || chararrinput[k].ToString() == "@" || chararrinput[k].ToString() == "," || chararrinput[k].ToString() == "#" || chararrinput[k].ToString() == "*" || chararrinput[k].ToString() == "%" || chararrinput[k].ToString() == "&" || chararrinput[k].ToString() == "~" || chararrinput[k].ToString() == "-" || chararrinput[k].ToString() == "_" || chararrinput[k].ToString() == "(" || chararrinput[k].ToString() == ")" || chararrinput[k].ToString() == "`" || chararrinput[k].ToString() == " " || chararrinput[k].ToString() == "=" )
                    {
                        MessageBox.Show("Special Character Not Allow In Vehicle Registration No.");
                        txtVehRegNo.Text = "";
                        txtVehRegNo.Focus();
                        return;

                    }

                }


                if (string.IsNullOrEmpty(txtAuthorizationNo.Text.Trim()) || string.IsNullOrWhiteSpace(txtAuthorizationNo.Text) )
                {
                    //|| txtAuthorizationNo.Text == "0" || txtAuthorizationNo.Text == "00" || txtAuthorizationNo.Text == "000" || txtAuthorizationNo.Text == "00000" || txtAuthorizationNo.Text == "000000"
                    MessageBox.Show("Please Insert Authorization No.");
                    txtAuthorizationNo.Text = "";
                    txtAuthorizationNo.Focus();
                    return; 
                }


                string strtxtAuthNo = txtAuthorizationNo.Text.Trim().ToString();
                char[] charAuthNo = strtxtAuthNo.ToCharArray();

                int AuthNo = 0;
                for (int A = 0; A < charAuthNo.Length; A++)
                {
                    if (charAuthNo[A].ToString() == " ")
                    {
                        MessageBox.Show(" Authorization No Not Valid.");
                        txtAuthorizationNo.Text = "";
                        txtAuthorizationNo.Focus();
                        return; 
                    }

                    if (charAuthNo[A].ToString() == "0")
                    {
                        AuthNo += 1;
                    }
                    else
                    {
                        AuthNo = AuthNo - 1;
                    }

                }

                if (AuthNo.ToString() == charAuthNo.Length.ToString())
                {
                    MessageBox.Show(" Authorization No Not Valid.");
                    txtAuthorizationNo.Text = "";
                    txtAuthorizationNo.Focus();
                    return; 

                }

                



                
                if (string.IsNullOrEmpty(txtOwnerName.Text.Trim()) || string.IsNullOrWhiteSpace(txtOwnerName.Text.Trim()))
                {
                    MessageBox.Show("Please Insert Owner Name");

                    txtOwnerName.Text = "";
                    txtOwnerName.Focus();
                    return;
                }



                if (string.IsNullOrEmpty(txtMobileNo.Text) || txtMobileNo.Text.Trim().Length < 10 || txtMobileNo.Text.Trim().Length > 10 || string.IsNullOrWhiteSpace(txtMobileNo.Text))
                {
                    MessageBox.Show("Please Insert Mobile No.");
                    txtMobileNo.Text = "";
                    txtMobileNo.Focus();
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

             


                if (string.IsNullOrEmpty(txtChassis.Text.Trim()))
                {
                    MessageBox.Show("Please Insert Chassis No.");
                    txtChassis.Text = "";
                    txtChassis.Focus();
                    return;
                }


                string strChessis = txtChassis.Text.Trim().ToString();
                char[] charchessisno = strChessis.ToCharArray();

                for (int A = 0; A < charchessisno.Length; A++)
                {
                    if (charchessisno[A].ToString() == " ")
                    {
                        MessageBox.Show("Chassis No Not Valid.");
                        txtChassis.Text = "";
                        txtChassis.Focus();
                        return;
                    }
                }


                if (string.IsNullOrEmpty(txtEngine.Text.Trim()))
                {
                    MessageBox.Show("Please Insert Engine No.");
                    txtEngine.Text = "";
                    txtEngine.Focus();
                    return;
                }


                string strEngine = txtEngine.Text.Trim().ToString();
                char[] charEngineNo = strEngine.ToCharArray();


                for (int A = 0; A < charEngineNo.Length; A++)
                {
                    if (charEngineNo[A].ToString() == " ")
                    {
                        MessageBox.Show("Engine No Not Valid.");
                        txtEngine.Text = "";
                        txtEngine.Focus();
                        return;
                    }
                }

                if (ddlVehicleClass.Text == "--Select Vehicle Class--" || String.IsNullOrEmpty(ddlVehicleClass.SelectedItem.ToString()))
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

                HSRPDataEntry.HsrpService.HSRPServiceSoapClient objHSRP = new HSRPDataEntry.HsrpService.HSRPServiceSoapClient();

                if (DDLOrderType.Text == "--Select Order Type--" || String.IsNullOrEmpty(DDLOrderType.SelectedItem.ToString()))
                {
                    MessageBox.Show("Please Select Order Type.");
                    DDLOrderType.Focus();
                    return;
                }

          
                    
                    string HSRPRecord_CreationDate = DateTime.Now.ToString("yyyy-MM-dd");
                    string HSRP_StateID = Stateid;


                    string HSRPRecord_AuthorizationNo = txtAuthorizationNo.Text.Trim().ToString();
                    string HSRPRecord_AuthorizationDate = DateTime.Now.ToString("yyyy-MM-dd");
                    string hsrp_holidaydate = DateTime.Now.ToString("yyyy-MM-dd");

                    string VehicleRegNo = txtVehRegNo.Text.Trim();
                    string OwnerName = txtOwnerName.Text;

                    string ownerFatherName = "";
                    string Address1 = "";


                    string OrderType = DDLOrderType.Text;


                    string amount = objHSRP.GetRateAndTaxForVehicle(HSRP_StateID, ddlVehType.Text, ddlVehicleClass.Text, OrderType);
                    string[] amount1 = amount.Split('^');
                    txtAmount.Text = amount1[0].ToString();
                    txtTax.Text = amount1[1].ToString();

                    string NetAmount = (Math.Round(Convert.ToDecimal(amount1[0]), 0)).ToString();  // amount1[2].ToString(); ;


             

                
              
                if (txtAmount.Text == "")
                {
                    MessageBox.Show("Please Insert Amount.");
                    txtAmount.Text = "";
                    txtAmount.Focus();
                    return;
                }



                if (ddlAffixCenter.Text == "--Select Affixation Center --" || String.IsNullOrEmpty(ddlAffixCenter.SelectedItem.ToString()))
                {
                    MessageBox.Show("Please Select Affixation Center Name ");
                    ddlAffixCenter.Focus();
                    return;
                }

                if (ddlDealerCenter.Visible != false)
                {

                    if (ddlDealerCenter.Text == "--Select Dealer Name Center--" || String.IsNullOrEmpty(ddlDealerCenter.SelectedItem.ToString()))
                    {
                        MessageBox.Show("Please Select Dealer Name");
                        ddlDealerCenter.Focus();
                        return;
                    }

                }


                if (txtdealer.Visible != false)
                {

                    if (txtdealer.Text == "")
                    {
                        MessageBox.Show("Please Insert Dealer Name.");
                        txtdealer.Text = "";
                        txtdealer.Focus();
                        return;
                    }
                }


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


                string Roundoff_NetAmount = (Math.Round(Convert.ToDecimal(NetAmount), 2)).ToString();
                string VehicleType = ddlVehType.Text;
                string OrderStatus = "NEW ORDER";
                string CashReceiptNo = "";
                string ChassisNo = txtChassis.Text;
                string EngineNo = txtEngine.Text;
                string DealerCode = "0";
               
                string SaveMacAddress = string.Empty;
                string Addrecordby = string.Empty;
                string CounterNo = txtCounterNo.Text;

                
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

                string Reference = string.Empty;

                if (ddlManufacturerMaker.Text == "--Select Vehicle Maker--")
                {
                    ManufacturerMaker = string.Empty;
                }
                else
                {
                    ManufacturerMaker = ddlManufacturerMaker.Text;
                }


                if (ddlManufacturerModel.Text == "--Select Vehicle Model--")
                {
                    ManufacturerModel = string.Empty;  
                }
                else
                {
                    ManufacturerModel = ddlManufacturerModel.Text;
                }
                
        

                string vehicleref = string.Empty;

                string issave = objHSRP.CheckDuplicateEntry(HSRP_StateID, txtAuthorizationNo.Text, txtVehRegNo.Text.Trim(), DDLOrderType.Text);
                string[] issave1 = issave.Split('^');
                string flag = issave1[0].ToString();
                


                if (flag == "Fail")
                {
                    lblMessage.Text = "Duplicate Records";
                }



                else
                {
                    string a = objHSRP.GetDateByHoliday(hsrp_holidaydate);

                    if (a != "0")
                    {
                        lblMessage.Text = "Collection not be done Bcz Today is Holiday";
                        return;

                    }

                    string DealerName;

                    if (ddlDealerCenter.Text.ToString() == "Other" || string.IsNullOrEmpty(ddlDealerCenter.Text.ToString()))
                    {

                        DealerName = txtdealer.Text.ToString();
                        int k = objHSRP.InserNewDealerName(Convert.ToInt32(HSRP_StateID), Convert.ToInt32(RTOLocationID), DealerName);

                    }
                    else
                    {
                        DealerName = ddlDealerCenter.Text.ToString();
                    }

                    if (!string.IsNullOrEmpty(DealerName))
                    {
                        string save = objHSRP.BiharDataCashCollection(HSRP_StateID, RTOLocationID, txtAuthorizationNo.Text, AuthDate.Value.ToString("yyyy-MM-dd"), txtVehRegNo.Text.Trim(), txtOwnerName.Text.ToString(), ownerFatherName, Address1, txtMobileNo.Text, ddlVehicleClass.Text, DDLOrderType.Text, Sticker, Vip, NetAmount, Roundoff_NetAmount, ddlVehType.Text, OrderStatus, CashReceiptNo, txtChassis.Text, EngineNo, DealerCode, CreatedBy, SaveMacAddress, Addrecordby, ISFrontPlateSize, ISRearPlateSize, FrontPlateSize, RearPlateSize, Reference, ManufacturerModel, vehicleref, ManufacturerMaker, CounterNo, ddlAffixCenter.SelectedValue.ToString(), DealerName);

                        string[] save1 = save.Split('^');

                        if (save1[0].ToString() != "Record Not Saved")
                        {
                            recno = objHSRP.CashReceiptForHR(txtAuthorizationNo.Text);
                            lblMessage.Text = "Record saved Successfully.";


                            if (CashRecieptPrinterType == "A4")
                            {
                                PrintDocument pd = new PrintDocument();
                                pd.PrinterSettings.PrinterName = CashRecieptPrinterName;
                                pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
                                pd.Print();

                            }
                            else
                            {
                                PrintDocument();
                                Refresh();
                            }
                        }
                        else
                        {
                            lblMessage.Text = "Record not Save";
                        }
                    }
                    else
                    {
                        if (ddlDealerCenter.Text == "--Select Dealername Center--" || String.IsNullOrEmpty(ddlDealerCenter.SelectedItem.ToString()))
                        {
                            MessageBox.Show("Please Select Dealer Name ");
                            ddlDealerCenter.Focus();
                            return;
                        }
                        string save = objHSRP.BiharDataCashCollection(HSRP_StateID, RTOLocationID, txtAuthorizationNo.Text, AuthDate.Value.ToString("yyyy-MM-dd"), txtVehRegNo.Text, txtOwnerName.Text, ownerFatherName, Address1, txtMobileNo.Text, ddlVehicleClass.Text, DDLOrderType.Text, Sticker, Vip, NetAmount, Roundoff_NetAmount, ddlVehType.Text, OrderStatus, CashReceiptNo, txtChassis.Text, EngineNo, DealerCode, CreatedBy, SaveMacAddress, Addrecordby, ISFrontPlateSize, ISRearPlateSize, FrontPlateSize, RearPlateSize, Reference, ManufacturerModel, vehicleref, ManufacturerMaker, CounterNo, ddlAffixCenter.SelectedValue.ToString(), ddlDealerCenter.SelectedValue.ToString());
                        string[] save1 = save.Split('^');

                        if (save1[0].ToString() != "Record Not Saved")
                        {
                            recno = objHSRP.CashReceiptForHR(txtAuthorizationNo.Text);
                            lblMessage.Text = "Record saved Successfully.";

                            if (CashRecieptPrinterType == "A4")
                            {
                                PrintDocument pd = new PrintDocument();
                                pd.PrinterSettings.PrinterName = CashRecieptPrinterName;
                                pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
                                pd.Print();

                            }
                            else
                            {
                                PrintDocument();
                                Refresh();
                            }
                        }
                        else
                        {
                            lblMessage.Text = "Record not Save";
                            return;
                        }
                    }



                }
            }
            else
            {

                MessageBox.Show("Internet Is Not Connected");
                return;

            }

        }
        public static bool isConnected()
        {
            try
            {
                string myAddress = "www.yahoo.com";
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


        string orderType;
        float amount = 0;

        string GetAddress;
        private void PrintDocument()
        {
            // string s = "Cost" + System.DateTime.Today.ToShortDateString() + Environment.NewLine + "-------------------------------------" + Environment.NewLine;


            //   HSRPDataEntry.HsrpService.HSRPServiceSoapClient objHSRP = new HSRPDataEntry.HsrpService.HSRPServiceSoapClient();
            //  Detial = utils.GetLocalDBConnectionFromINI();
            //string[] DetailData = Detial.Split('^');
            //DataFolder = DetailData[0];
            //DataFileFolder = DetailData[1];
            //EXLFileFolder = DetailData[2];
            //Stateid = DetailData[3];
            //StateName = DetailData[4];
            //RTOLocationCode = DetailData[5];
            //CompanyName = DetailData[6];
            //RTOLocationAddress = DetailData[7];
            //ReceiptSizeA4 = DetailData[8];
            //PrinterName1 = DetailData[9];
            //dt1 = objHSRP.Chalan(Stateid);

            HSRPDataEntry.HsrpService.HSRPServiceSoapClient objHSRP = new HSRPDataEntry.HsrpService.HSRPServiceSoapClient();
            Detial = utils.GetLocalDBConnectionFromINI();
            string[] DetailData = Detial.Split('^');
            DataFolder = DetailData[0];
            DataFileFolder = DetailData[1];
            EXLFileFolder = DetailData[2];
            Stateid = DetailData[3];
            StateName = DetailData[4];
            RTOLocationCode = DetailData[5];
            // CompanyName = DetailData[6];
            CompanyName = "UTSAV SAFETY SYSTEM PVT. LTD.In consitium with Linkpoint Infrastructure Pvt. Ltd.";
            RTOLocationAddress = DetailData[7];
            ReceiptSizeA4 = DetailData[8];
            PrinterName1 = DetailData[9];
            dt1 = objHSRP.Chalan(Stateid);

            // DataTable dtadd = new DataTable();
            //dtadd = objHSRP.FillAddrss(ddlAffixCenter.SelectedValue.ToString());

            string companyname = CompanyName;
            //string ComAdd = dtadd.Rows[0]["address1"].ToString();
            DataTable dtAffixd = new DataTable();
            dtAffixd = objHSRP.FillAddrss(ddlAffixCenter.SelectedValue.ToString());
            //clsINI.strAffexaddress = dtAffixd.ToString();
            for (int i = 0; i < dtAffixd.Rows.Count; i++)
            {
                clsINI.strAffexaddress = dtAffixd.Rows[i]["address1"].ToString();
            }
            string ComAdd = " 	UTSAV SAFETY SYSTEM PVT. LTD.In consitium with Linkpoint Infrastructure Pvt. Ltd.";

            //string ComAdd1 = "For Affixation Kindly visit :- Patna, Opp-Transport Nagar, Gate no-2,phari,patna.";

            // string ComAdd1 = "For Affixation Kindly visit :-";

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
                //ee.Graphics.DrawString("For Affixation Kindly visit :-"+ clsINI.strAffexaddress, new Font("Times New Roman", 8), new SolidBrush(Color.Black), new Rectangle(0, 18, 300, 300));
                ee.Graphics.DrawString("Receipt Date       : " + System.DateTime.Now.ToString("dd-MM-yyyy"), new Font("Times New Roman", 10), new SolidBrush(Color.Black), new Rectangle(0, 60, 300, 300));
                ee.Graphics.DrawString("Receipt No         :  " + recno, new Font("Times New Roman", 10), new SolidBrush(Color.Black), new Rectangle(0, 80, 300, 300));
                ee.Graphics.DrawString("Authorization No :  " + txtAuthorizationNo.Text, new Font("Times New Roman", 10), new SolidBrush(Color.Black), new Rectangle(0, 100, 300, 300));
                ee.Graphics.DrawString("Vehicle Reg No   :  " + txtVehRegNo.Text, new Font("Times New Roman", 10), new SolidBrush(Color.Black), new Rectangle(0, 120, 300, 300));
                ee.Graphics.DrawString("Owner Name       :  " + txtOwnerName.Text, new Font("Times New Roman", 10), new SolidBrush(Color.Black), new Rectangle(0, 140, 300, 300));
                ee.Graphics.DrawString("Mobile No          :  " + txtMobileNo.Text, new Font("Times New Roman", 10), new SolidBrush(Color.Black), new Rectangle(0, 160, 300, 300));
                ee.Graphics.DrawString("Vehicle Class      :  " + ddlVehicleClass.Text, new Font("Times New Roman", 10), new SolidBrush(Color.Black), new Rectangle(0, 180, 300, 300));
                ee.Graphics.DrawString("Vehicle Type      :  " + ddlVehType.Text, new Font("Times New Roman", 10), new SolidBrush(Color.Black), new Rectangle(0, 200, 300, 300));
                ee.Graphics.DrawString("Order Type         :  " + DDLOrderType.Text, new Font("Times New Roman", 10), new SolidBrush(Color.Black), new Rectangle(0, 220, 300, 300));
                // ee.Graphics.DrawString("Sticker               :  " + Sticker, new Font("Times New Roman", 10), new SolidBrush(Color.Black), new Rectangle(0, 240, 300, 300));
                //ee.Graphics.DrawString("VIP                    :  " + Vip, new Font("Times New Roman", 10), new SolidBrush(Color.Black), new Rectangle(0, 260, 300, 100));
                //ee.Graphics.DrawString("Tax                    :  " + txtTax.Text, new Font("Times New Roman", 10), new SolidBrush(Color.Black), new Rectangle(0, 280, 300, 300));
                //ee.Graphics.DrawString("Amount              :  " + txtAmount.Text, new Font("Times New Roman", 10), new SolidBrush(Color.Black), new Rectangle(0, 300, 300, 300));
                ee.Graphics.DrawString("Net Amount              :  " + System.Math.Round(Convert.ToDecimal(txtAmount.Text), 0), new Font("Times New Roman", 10), new SolidBrush(Color.Black), new Rectangle(0, 240, 300, 300));
                //  ee.Graphics.DrawString("Dealer Name              :  " + txtdealer.Text, new Font("Times New Roman", 10), new SolidBrush(Color.Black), new Rectangle(0, 260, 300, 300));
                ee.Graphics.DrawString("Disclaimer : ", new Font("Times New Roman", 9), new SolidBrush(Color.Black), new Rectangle(0, 260, 300, 300));
                
                ee.Graphics.DrawString("For Affixation Kindly visit :-" + clsINI.strAffexaddress, new Font("Times New Roman", 8), new SolidBrush(Color.Black), new Rectangle(0, 280, 300, 300));
                ee.Graphics.DrawString("Before leaving the counter Vehicle  owner should check receipt details, failing which the company shall not be responsible in any manner.", new Font("Times New Roman", 8), new SolidBrush(Color.Black), new Rectangle(0, 330, 300, 300));
                ee.Graphics.DrawString("Vehicle Owner is requested to please check the Correctness of the cash slip. The company shall not be responsible for any clerical mistake what so ever.", new Font("Times New Roman", 8), new SolidBrush(Color.Black), new Rectangle(0, 380, 300, 300));
                ee.Graphics.DrawString("For Any Query,complaint and suggestion and tracking Your Vehicle Please Visit www.hsrpbr.com", new Font("Times New Roman", 8), new SolidBrush(Color.Black), new Rectangle(0, 440, 210, 300));
                ee.Graphics.DrawString(Login.username, new Font("Times New Roman", 8), new SolidBrush(Color.Black), new Rectangle(0, 510, 300, 300));

                ee.Graphics.DrawString(".", new Font("Times New Roman", 5), new SolidBrush(Color.Black), new Rectangle(0, 480, 300, 300));

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




        private void PrintDocument12()
        {
          

            HSRPDataEntry.HsrpService.HSRPServiceSoapClient objHSRP = new HSRPDataEntry.HsrpService.HSRPServiceSoapClient();
            Detial = utils.GetLocalDBConnectionFromINI();
            string[] DetailData = Detial.Split('^');
            DataFolder = DetailData[0];
            DataFileFolder = DetailData[1];
            EXLFileFolder = DetailData[2];
            Stateid = DetailData[3];
            StateName = DetailData[4];
            RTOLocationCode = DetailData[5];
            // CompanyName = DetailData[6];
            CompanyName = "UTSAV SAFETY SYSTEM PVT. LTD.In consitium with Linkpoint Infrastructure Pvt. Ltd.";
            RTOLocationAddress = DetailData[7];
            ReceiptSizeA4 = DetailData[8];
            PrinterName1 = DetailData[9];
            dt1 = objHSRP.Chalan(Stateid);

            // DataTable dtadd = new DataTable();
            //dtadd = objHSRP.FillAddrss(ddlAffixCenter.SelectedValue.ToString());

            string companyname = CompanyName;
            //string ComAdd = dtadd.Rows[0]["address1"].ToString();

            DataTable dtAffixd = new DataTable();
            dtAffixd = objHSRP.FillAddrss(ddlAffixCenter.SelectedValue.ToString());
            //clsINI.strAffexaddress = dtAffixd.ToString();
            for (int i = 0; i < dtAffixd.Rows.Count; i++)
            {
                clsINI.strAffexaddress = dtAffixd.Rows[i]["address1"].ToString();
            }

            string ComAdd = " 	UTSAV SAFETY SYSTEM PVT. LTD.In consitium with Linkpoint Infrastructure Pvt. Ltd.";

            //string ComAdd1 = "For Affixation Kindly visit :- Patna, Opp-Transport Nagar, Gate no-2,phari,patna.";

            // string ComAdd1 = "For Affixation Kindly visit :-";

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
                //ee.Graphics.DrawString("For Affixation Kindly visit :-"+ clsINI.strAffexaddress, new Font("Times New Roman", 8), new SolidBrush(Color.Black), new Rectangle(0, 18, 300, 300));
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

                //ee.Graphics.DrawString(ComAdd, new Font("Times New Roman", 10), new SolidBrush(Color.Black), new Rectangle(0, 350, 300, 300));

                ee.Graphics.DrawString("For Affixation Kindly visit :-" + clsINI.strAffexaddress, new Font("Times New Roman", 8), new SolidBrush(Color.Black), new Rectangle(0, 360, 300, 300));
                ee.Graphics.DrawString("Before leaving the counter Vehicle  owner should check receipt details, failing which the company shall not be responsible in any manner.", new Font("Times New Roman", 8), new SolidBrush(Color.Black), new Rectangle(0, 400, 300, 300));
                ee.Graphics.DrawString("Vehicle Owner is requested to please check the Correctness of the cash slip. The company shall not be responsible for any clerical mistake what so ever.", new Font("Times New Roman", 8), new SolidBrush(Color.Black), new Rectangle(0, 460, 300, 300));
                ee.Graphics.DrawString("For Any Query,complaint and suggestion and tracking Your Vehicle Please Visit www.hsrpbr.com", new Font("Times New Roman", 8), new SolidBrush(Color.Black), new Rectangle(0, 520, 210, 300));
                ee.Graphics.DrawString(Login.username, new Font("Times New Roman", 8), new SolidBrush(Color.Black), new Rectangle(0, 600, 300, 300));

                ee.Graphics.DrawString(".", new Font("Times New Roman", 5), new SolidBrush(Color.Black), new Rectangle(0, 470, 300, 300));



            };
            try
            {
                p.Print();

                //Refresh();
            }
            catch (Exception ex)
            {
                throw new Exception("Exception Occured While Printing", ex);
            }
        }

        private void PrintDocument2()
        {
            // string s = "Cost" + System.DateTime.Today.ToShortDateString() + Environment.NewLine + "-------------------------------------" + Environment.NewLine;


            HSRPDataEntry.HsrpService.HSRPServiceSoapClient objHSRP = new HSRPDataEntry.HsrpService.HSRPServiceSoapClient();
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
            dtadd = objHSRP.FillAddrss(ddlAffixCenter.SelectedValue.ToString());

            string companyname = CompanyName;
            string ComAdd = dtadd.Rows[0]["address1"].ToString();
            // string Heading = "CASH RECEIPT";

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
        public void Refresh()
        {
            txtVehRegNo.Text = "";
            txtOwnerName.Text = "";
            txtMobileNo.Text = "0000000000";
            txtAuthorizationNo.Text = "0";
            txtAmount.Text = "";           
            ddlVehType.Text = "";          
            ddlVehicleClass.Text = "--Select Vehicle Class--";
            txtRegNo.Text = "";
            txtEngineNo.Text = "";
            txtEngine.Text = "";
            txtChassis.Text = "";
            ddlVehType.Text = "--Select Vehicle Type--";
            DDLOrderType.Text = "--Select Order Type--";
            ddlManufacturerMaker.Text = "--Select Vehicle Maker--";
            ddlManufacturerModel.Text = "--Select Vehicle Model--";
            ddlAffixCenter.Text = "--Select Affx Center--";
            ddlDealerCenter.Text = "--Select Dealer Name Center--";
            chkVIP.Checked = false;
            radioButton1.Checked = true;
            radioButton2.Checked = false;
            txtTax.Text = "";
            txtdealer.Text = "";
            txtCounterNo.Text = "";
            bindddldealername();
          
        }



        private void button2_Click(object sender, EventArgs e)
        {
            PrintDocument();


        }


        //  static string stateID, LocationID, state, RTOLocationAddress, CompanyName, CurrentDate, UserLoginID;




        private void button1_Click(object sender, EventArgs e)
        {
            Refresh();
        }
        string Prefix;

        private void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtVehRegNo.Text))
            {
                MessageBox.Show("Please Insert Vehicle Registration No.");

                txtVehRegNo.Text = "";
                txtVehRegNo.Focus();
                return;
            }



            if (string.IsNullOrEmpty(txtAuthorizationNo.Text))
            {
                MessageBox.Show("Please Insert Authorization No.");

                txtAuthorizationNo.Text = "";
                txtAuthorizationNo.Focus();
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

            if (ddlVehicleClass.Text == "--Select Vehicle Class--" || String.IsNullOrEmpty(ddlVehicleClass.SelectedItem.ToString()))
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
            //if (DDLOrderType.Text == "--Select Order Type--" || String.IsNullOrEmpty(ddlOrderType.SelectedItem.ToString()))
            //{
            //    MessageBox.Show("Please Select Order Type.");
            //    ddlOrderType.Focus();
            //    return;
            //}
            if (txtAmount.Text == "")
            {
                MessageBox.Show("Please Insert Amount.");
                txtAmount.Text = "";
                txtAmount.Focus();
                return;
            }

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
            string vehcleRegNo = string.Empty;
            if (txtRegNo.Text.Replace(" ", "") == "")
            {
                vehcleRegNo = txtVehRegNo.Text.Trim().Replace(" ", "");
            }
            else
            {

                vehcleRegNo = txtRegNo.Text.Trim().Replace(" ", "");
            }

            string qry = "select vehicleregno from OrderBookingOffLine where REPLACE(vehicleregno,space(1),'')='" + vehcleRegNo + "' and ordertype ='" + orderType + "'";
            //  string checkvalue = utils.getDataSingleValue(qry, utils.getCnnHSRPApp, "vehicleregno");
            //if (checkvalue == "0")
            //{
            //    MessageBox.Show("Record Not Booked Yet.");
            //    txtRegNo.Focus();
            //    return;
            //}
            //PrintDocument();
            if (CashRecieptPrinterType == "A4")
            {
                PrintDocument pd = new PrintDocument();

                //   Set PrinterName as the selected printer in the printers list
                //   CashReceipt.Form1 OBJ = new CashReceipt.Form1();
                // pd.PrinterSettings.PrinterName = utils.PrinterName;

                pd.PrinterSettings.PrinterName = CashRecieptPrinterName;
                //     Add PrintPage event handler
                pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
                //   pd.PrintPage + = new PrintPageEventHandler (pd_PrintPage);

                //   Print the document
                pd.Print();
                // PrintDocumentA4();
            }
            else
            {
                PrintDocument();
            }
        }

        public void pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            HSRPDataEntry.HsrpService.HSRPServiceSoapClient objHSRP = new HSRPDataEntry.HsrpService.HSRPServiceSoapClient();
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
            dtadd = objHSRP.FillAddrss(ddlAffixCenter.SelectedValue.ToString());

            string companyname = CompanyName;
            string ComAdd = dtadd.Rows[0]["address1"].ToString();
            string Heading = "CASH RECEIPT";
            string CashReceiptNo = recno;
            string DATE = System.DateTime.Now.ToString("dd-MM-yyyy");
            string TINNO = dt1.Rows[0]["TinNo"].ToString(), TIME = System.DateTime.Now.ToString("HH:MM");

            string AUTHNO = txtAuthorizationNo.Text, AUTHDATE = AuthDate.Text, ORDERBOOKINGNO = " ", OWNERNAME = txtOwnerName.Text, CONTACTNO = txtMobileNo.Text, OWNERADDRESS = " ", VEHICLEREG = txtVehRegNo.Text, VEHICLEMODEL = ddlVehType.Text, ENGINENO = txtEngine.Text, CHASSISNO = txtChassis.Text;
            string AMOUNT = txtAmount.Text;
            string ROUNDOFAMOUNT = txtAmount.Text;
            string NETAMOUNT = txtAmount.Text;
            // string DealerName = txtdealer.Text;

            FontFamily fontFamily = new FontFamily("Lucida Console");

            StringFormat stringFormat = new StringFormat();
            SolidBrush solidBrush = new SolidBrush(Color.FromArgb(255, 0, 0, 255));

            // stringFormat.FormatFlags = StringFormatFlags.DirectionVertical;

            // stringFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft;

            // HEADING OF PDF
            Font font1111 = new Font(fontFamily, 20, FontStyle.Regular, GraphicsUnit.Point);
            PointF pointF = new PointF(180, 20);
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




            PointF pointF34 = new PointF(500, 460);   // left side
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
            PointF pointF38 = new PointF(180, 550);
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






            PointF pointF72 = new PointF(500, 970);   // left side
            e.Graphics.DrawString(companyname, font3, solidBrush, pointF72, stringFormat);


            PointF pointF73 = new PointF(550, 1020);   // left side
            e.Graphics.DrawString("(AUTH. SIGH.) ", font3, solidBrush, pointF73, stringFormat);
        }




        private void button1_Click_1(object sender, EventArgs e)
        {
            Refresh();
        }

        string CashRecieptPrinterType = string.Empty;
        string CashRecieptPrinterName = string.Empty;

        
        private void CashReciept_Load(object sender, EventArgs e)
        {

            groupBox1.Visible = false;
            Detial = utils.GetLocalDBConnectionFromINI();
            string[] DetailData = Detial.Split('^');
            DataFolder = DetailData[0];
            DataFileFolder = DetailData[1];
            EXLFileFolder = DetailData[2];

            RTOLocationCode = Login.RTOlocationID;
            Stateid = Login.Stateid;
            StateName = DetailData[4];          
            CompanyName = DetailData[6];           
            RTOLocationAddress1 = Login.RTOLocationAddress;
            ReceiptSizeA4 = DetailData[8];
            PrinterName1 = DetailData[9];


            ddlVehicleClass.Text = "--Select Vehicle Class--";
            //ddlVehType.Text = "--Select Vehicle Type--";
            //DDLOrderType.Text = "--Select Order Type--";
            ddlManufacturerMaker.Text = "--Select Vehicle Maker--";
            ddlManufacturerModel.Text = "--Select Vehicle Model--";
         
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
            bindddldealername();

           
         

        }

        public void bindddlaffixCenter()
        {

            Detial = utils.GetLocalDBConnectionFromINI();
            string[] DetailData = Detial.Split('^');
            DataFolder = DetailData[0];
            DataFileFolder = DetailData[1];
            EXLFileFolder = DetailData[2];
            Stateid = DetailData[3];
            StateName = DetailData[4];
            //RTOLocationCode = DetailData[5];
            RTOLocationCode = Login.RTOlocationID;
            CompanyName = DetailData[6];
            // RTOLocationAddress = DetailData[7];
            RTOLocationAddress = Login.RTOLocationAddress;
            ReceiptSizeA4 = DetailData[8];
            PrinterName1 = DetailData[9];


            HSRPDataEntry.HsrpService.HSRPServiceSoapClient objHSRP = new HSRPDataEntry.HsrpService.HSRPServiceSoapClient();
            dt2 = objHSRP.filldropdownlistAFfix(Stateid, RTOLocationCode);

            DataRow dr;
            dr = dt2.NewRow();
            dr.ItemArray = new object[] { "--Select Affixation Center --", "0" };
            dt2.Rows.InsertAt(dr, 0);
            ddlAffixCenter.DataSource = dt2;
            ddlAffixCenter.DisplayMember = "affixcenterdesc";
            ddlAffixCenter.ValueMember = "Affix_id";


        }
        DataTable dt3 = new DataTable();
        public void bindddldealername()
        {

            Detial = utils.GetLocalDBConnectionFromINI();
            string[] DetailData = Detial.Split('^');
            DataFolder = DetailData[0];
            DataFileFolder = DetailData[1];
            EXLFileFolder = DetailData[2];
            Stateid = DetailData[3];
            StateName = DetailData[4];
            RTOLocationCode = Login.RTOlocationID;
            CompanyName = DetailData[6];
            RTOLocationAddress = Login.RTOLocationAddress;
            ReceiptSizeA4 = DetailData[8];
            PrinterName1 = DetailData[9];


            HSRPDataEntry.HsrpService.HSRPServiceSoapClient objHSRP = new HSRPDataEntry.HsrpService.HSRPServiceSoapClient();
            dt3 = objHSRP.filldropdowndealername(Stateid, RTOLocationCode);

            DataRow dr;
            DataRow drr;
            dr = dt3.NewRow();
            drr = dt3.NewRow();
            if (dt3.Rows.Count > 0)
            {
                int i = dt3.Rows.Count;
                i = i + 1;

                dr.ItemArray = new object[] { "--Select Dealer Name Center--", "0" };
                drr.ItemArray = new object[] { "Other", Convert.ToString(i) };

                dt3.Rows.InsertAt(dr, 0);
                dt3.Rows.InsertAt(drr, i);
                ddlDealerCenter.DataSource = dt3;
                ddlDealerCenter.DisplayMember = "dealerName";
                ddlDealerCenter.ValueMember = "dealerid";
                ddlDealerCenter.Visible = true;
                txtdealer.Visible = false;
            }
            else
            {
                ddlDealerCenter.Visible = false;
                txtdealer.Visible = true;

            }


        }



        HSRPDataEntry.HsrpService.HSRPServiceSoapClient objHSRP = new HSRPDataEntry.HsrpService.HSRPServiceSoapClient();

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
        //string OrderType = string.Empty;



        private void btnRegNoSearch_Click(object sender, EventArgs e)
        {


            //string strVehicleRegNo = txtRegNo.Text.Trim();
            //if ((strVehicleRegNo == ""))
            //{
            //    MessageBox.Show("Please Enter Vehicle Reg No");
            //    txtRegNo.Text = string.Empty;
            //    txtRegNo.Focus();
            //    return;

            //}

            //// authno^onername^vehicletype^ordertype^vehicleregno^vehiclemodel^vehicleclass^Engineno^

            //HSRPDataEntry.HsrpService.HSRPServiceSoapClient objHSRP = new HSRPDataEntry.HsrpService.HSRPServiceSoapClient();

            //string StrVehicledata;
            //    //= objHSRP.GetDataByVehicleRegNo(strVehicleRegNo);
            //if (StrVehicledata.Length > 0)
            //{

            //    //"AuthNo^OwnerName^VehicleType^OrderType^VehicleRegno^VehicleModel^VehicleClass^EngineNo^AuthDate^ChassisNo";

            //    string[] VehicleArray = StrVehicledata.Split('^');

            //    //  StrRecord = dt.Rows[0]["OwnerName"].ToString() + "^" + dt.Rows[0]["OwnerFatherName"].ToString() + "^" + dt.Rows[0]["EngineNo"].ToString() + "^" + dt.Rows[0]["ChassisNo"].ToString() + "^" + dt.Rows[0]["OlaAddress"].ToString() + "^" + dt.Rows[0]["Reg_Date"].ToString() + "^" + dt.Rows[0]["OrderStatus"].ToString() + "^" + dt.Rows[0]["vehicletype"].ToString() + "^" + dt.Rows[0]["ManufacturerName"].ToString() + "^" + dt.Rows[0]["ManufacturerModel"].ToString();  VEHICLE TYPE, VEHICLEREG NO

            //    //StrRecord = dt.Rows[0]["OwnerName"].ToString() + "^" + dt.Rows[0]["OwnerFatherName"].ToString() + "^" + dt.Rows[0]["EngineNo"].ToString() + "^" + dt.Rows[0]["ChassisNo"].ToString() + "^" + dt.Rows[0]["OlaAddress"].ToString() + "^" + dt.Rows[0]["Reg_Date"].ToString() + "^" + dt.Rows[0]["OrderStatus"].ToString() + "^" + dt.Rows[0]["vehicletype"].ToString() + "^" + dt.Rows[0]["ManufacturerName"].ToString() + "^" + dt.Rows[0]["ManufacturerModel"].ToString() +"^" + dt.Rows[0]["VehicleRegNo"].ToString();

            //    AuthNo = "123";
            //    OwnerName = VehicleArray[0].ToString();
            //    VehicleType = VehicleArray[7].ToString();
            //    //txtOrderType.Text = "OB";
            //    //string OrderType = "OB";
            //    if (VehicleArray[6].ToString() == "Old")
            //    {
            //        DDLOrderType.Text = "OB";
            //    }

            //    VehicleRegno = VehicleArray[10].ToString();
            //    VehicleModel = VehicleArray[9].ToString();
            //    // VehicleClass    = VehicleArray[7].ToString();
            //    EngineNo = VehicleArray[2].ToString();
            //    AuthDate1 = VehicleArray[5].ToString();
            //    ChassisNo = VehicleArray[3].ToString();

            //    //txtManufacturerMaker.Text = VehicleArray[8].ToString();
            //    //txtmanufacturerModel.Text = VehicleArray[9].ToString();

            //    txtAuthorizationNo.Text = AuthNo;
            //    txtOwnerName.Text = OwnerName;

            //    ddlVehType.Text = VehicleType;
            //    //txtOrderType.Text = OrderType;
            //    txtVehRegNo.Text = VehicleRegno;
            //    OrderType = DDLOrderType.Text;

            //    // txtVehicleClass.Text = VehicleClass;
            //    txtEngine.Text = EngineNo;
            //    AuthDate.Value = DateTime.Parse(AuthDate1.ToString());
            //    txtChassis.Text = ChassisNo;

            //    string amount = objHSRP.GetRateAndTaxForVehicle(Stateid, ddlVehType.Text, ddlVehicleClass.Text, OrderType);
            //    string[] amount1 = amount.Split('^');
            //    txtAmount.Text = amount1[0].ToString();
            //    txtTax.Text = amount1[1].ToString();
            //}
            //else
            //{
            //    MessageBox.Show("Record Not Found");
            //}
            //lblMessage.Text = "";
        }

        private void btn_EngineNo_Click(object sender, EventArgs e)
        {

            //string strEngineNo = txtEngineNo.Text.Trim();
            //if ((strEngineNo == ""))
            //{
            //    MessageBox.Show("Please Enter Engine No");
            //    txtRegNo.Text = string.Empty;
            //    txtRegNo.Focus();
            //    return;

            //}

            //// authno^onername^vehicletype^ordertype^vehicleregno^vehiclemodel^vehicleclass^Engineno^
            ////string AuthNo = string.Empty;
            ////string OwnerName = string.Empty;
            ////string VehicleType = string.Empty;
            ////string OrderType = string.Empty;
            ////string VehicleRegno = string.Empty;
            ////string VehicleModel = string.Empty;
            ////string VehicleClass = string.Empty;
            ////string EngineNo = string.Empty;
            ////string AuthDate1 = string.Empty;
            ////string ChassisNo = string.Empty;
            //HSRPDataEntry.HsrpService.HSRPServiceSoapClient objHSRP = new HSRPDataEntry.HsrpService.HSRPServiceSoapClient();

            //string StrVehicledata = objHSRP.GetDataByEngineNumber(strEngineNo);
            //if (strEngineNo.Length > 0)
            //{
            //    //"AuthNo^OwnerName^VehicleType^OrderType^VehicleRegno^VehicleModel^VehicleClass^EngineNo^AuthDate^ChassisNo";
            //    // StrRecord = dt.Rows[0]["OwnerName"].ToString() + "^" + dt.Rows[0]["OwnerFatherName"].ToString() + "^" + dt.Rows[0]["VehicleRegNo"].ToString() + "^" + dt.Rows[0]["ChassisNo"].ToString() + "^" + dt.Rows[0]["OlaAddress"].ToString() + "^" + dt.Rows[0]["Reg_Date"].ToString() + "^" + dt.Rows[0]["vehicletype"].ToString() + "^" + dt.Rows[0]["engineNo"].ToString() + "^"  + dt.Rows[0]["ManufacturerName"].ToString() + "^" + dt.Rows[0]["ManufacturerModel"].ToString();
            //    string[] VehicleArray = StrVehicledata.Split('^');


            //    AuthNo = "123";
            //    OwnerName = VehicleArray[0].ToString();
            //    VehicleType = VehicleArray[6].ToString();
            //    if (VehicleArray[6].ToString() == "Old")
            //    {
            //        DDLOrderType.Text = "OB";
            //    }

            //    VehicleRegno = VehicleArray[2].ToString();
            //    VehicleModel = VehicleArray[9].ToString();
            //    VehicleClass = ddlVehicleClass.Text;
            //    EngineNo = VehicleArray[7].ToString();
            //    AuthDate1 = VehicleArray[5].ToString();
            //    ChassisNo = VehicleArray[3].ToString();

            //    txtManufacturerMaker.Text = VehicleArray[8].ToString();
            //    txtmanufacturerModel.Text = VehicleArray[9].ToString();

            //    txtAuthorizationNo.Text = AuthNo;
            //    txtOwnerName.Text = OwnerName;

            //    ddlVehType.Text = VehicleType;
            //    //txtOrderType.Text = OrderType;
            //    txtVehRegNo.Text = VehicleRegno;
            //    OrderType = DDLOrderType.Text;

            //    txtEngine.Text = EngineNo;
            //    AuthDate.Value = DateTime.Parse(AuthDate1.ToString());
            //    txtChassis.Text = ChassisNo;
            //    string amount = objHSRP.GetRateAndTaxForVehicle(Stateid, ddlVehType.Text, ddlVehicleClass.Text.ToString(), OrderType);
            //    string[] amount1 = amount.Split('^');
            //    txtAmount.Text = amount1[0].ToString();
            //    txtTax.Text = amount1[1].ToString();
            //}
            //else
            //{
            //    MessageBox.Show("Record Not Found");
            //}
            //lblMessage.Text = "";
        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            bool i = isConnected();
            if (i == true)
            {
                if (string.IsNullOrEmpty(txtVehRegNo.Text))
                {
                    MessageBox.Show("Please Insert Vehicle Registration No.");

                    txtVehRegNo.Text = "";
                    txtVehRegNo.Focus();
                    return;
                }



                if (string.IsNullOrEmpty(txtAuthorizationNo.Text))
                {
                    MessageBox.Show("Please Insert Authorization No.");
                    txtAuthorizationNo.Text = "";
                    txtAuthorizationNo.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(txtOwnerName.Text))
                {
                    MessageBox.Show("Please Insert Owner Name");

                    txtOwnerName.Text = "";
                    txtOwnerName.Focus();
                    return;
                }

                //int success =  
                //(int.Parse(txtMobileNo.Text)==true)

                if ((string.IsNullOrEmpty(txtMobileNo.Text)) || (txtMobileNo.Text.Trim().Length < 10) || (txtMobileNo.Text.Trim().Length > 10))
                {
                    MessageBox.Show("Please Insert Mobile No.");

                    txtMobileNo.Text = "";
                    txtMobileNo.Focus();
                    return;
                }

                if (ddlVehicleClass.Text == "--Select Vehicle Class--" || String.IsNullOrEmpty(ddlVehicleClass.SelectedItem.ToString()))
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
                //if (ddlOrderType.Text == "--Select Order Type--" || String.IsNullOrEmpty(ddlOrderType.SelectedItem.ToString()))
                //{
                //    MessageBox.Show("Please Select Order Type.");
                //    ddlOrderType.Focus();
                //    return;
                //} 

                if (txtAmount.Text == "")
                {
                    MessageBox.Show("Please Insert Amount.");
                    txtAmount.Text = "";
                    txtAmount.Focus();
                    return;
                }

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




            }
            else
            {

                MessageBox.Show("Internet Is Not Connected");

            }



        }



        private void btnvehicleGO_Click(object sender, EventArgs e)
        {
            HSRPDataEntry.HsrpService.HSRPServiceSoapClient objHSRP = new HSRPDataEntry.HsrpService.HSRPServiceSoapClient();
            dt1 = objHSRP.GetDataByVehicleRegNo(txtVehRegNo.Text);
            if (dt1.Rows.Count > 0)
            {
                lblMessage.Text = "";
                txtAuthorizationNo.Text = dt1.Rows[0]["HSRPRecord_AuthorizationNo"].ToString();
                txtOwnerName.Text = dt1.Rows[0]["OwnerName"].ToString();
                txtMobileNo.Text = dt1.Rows[0]["MobileNo"].ToString();
                txtChassis.Text = dt1.Rows[0]["ChassisNo"].ToString();
                txtEngine.Text = dt1.Rows[0]["EngineNo"].ToString();
                ddlVehicleClass.Text = dt1.Rows[0]["VehicleClass"].ToString();
                ddlVehType.Text = dt1.Rows[0]["VehicleType"].ToString();
                DDLOrderType.Text = dt1.Rows[0]["OrderType"].ToString();
                if (!string.IsNullOrEmpty(dt1.Rows[0]["ManufacturerName"].ToString()))
                {
                    ddlManufacturerMaker.Text = dt1.Rows[0]["ManufacturerName"].ToString();
                }
                
                if (!string.IsNullOrEmpty(dt1.Rows[0]["ManufacturerModel"].ToString()))
                {
                  ddlManufacturerModel.Text = dt1.Rows[0]["ManufacturerModel"].ToString();
                }            
                

                txtAmount.Text = dt1.Rows[0]["TotalAmount"].ToString();  //NetAmount
                txtTax.Text = dt1.Rows[0]["ServiceTax_Amount"].ToString();
                ddlDealerCenter.Text = dt1.Rows[0]["DealerName"].ToString();
            }
            else
            {
                MessageBox.Show("Record Not Found");
            }
        }

        private void btnAuthGO_Click(object sender, EventArgs e)
        {

            HSRPDataEntry.HsrpService.HSRPServiceSoapClient objHSRP = new HSRPDataEntry.HsrpService.HSRPServiceSoapClient();
            dt1 = objHSRP.GetDataByAuthNo(txtAuthorizationNo.Text);
            if (dt1.Rows.Count > 0)
            {
                lblMessage.Text = "";
                txtAuthorizationNo.Text = dt1.Rows[0]["HSRPRecord_AuthorizationNo"].ToString();
                //  AuthDate.Value = dt1.Rows[0]["[HSRPRecord_AuthorizationDate]"];
                txtOwnerName.Text = dt1.Rows[0]["OwnerName"].ToString();
                txtMobileNo.Text = dt1.Rows[0]["MobileNo"].ToString();
                txtChassis.Text = dt1.Rows[0]["ChassisNo"].ToString();
                txtEngine.Text = dt1.Rows[0]["EngineNo"].ToString();
                ddlVehicleClass.Text = dt1.Rows[0]["VehicleClass"].ToString();
                ddlVehType.Text = dt1.Rows[0]["VehicleType"].ToString();
                DDLOrderType.Text = dt1.Rows[0]["OrderType"].ToString();


                if (!string.IsNullOrEmpty(dt1.Rows[0]["ManufacturerName"].ToString()))
                {
                    ddlManufacturerMaker.Text = dt1.Rows[0]["ManufacturerName"].ToString();
                }

                if (!string.IsNullOrEmpty(dt1.Rows[0]["ManufacturerModel"].ToString()))
                {
                    ddlManufacturerModel.Text = dt1.Rows[0]["ManufacturerModel"].ToString();
                }            
                                
                txtAmount.Text = dt1.Rows[0]["TotalAmount"].ToString();  //NetAmount
                txtTax.Text = dt1.Rows[0]["ServiceTax_Amount"].ToString();
            }
            else
            {
                MessageBox.Show("Record Not Found");
            }

        }

        private void MethodVehicleModel1()
        {
           
            dt1 = objHSRP.PopulateVehicleModel(ddlManufacturerMaker.SelectedValue.ToString());
            DataRow dr;
            dr = dt1.NewRow();
            dr.ItemArray = new object[] { "--Select Vehicle Model--", "0" };
            dt1.Rows.InsertAt(dr, 0);
            ddlManufacturerModel.DataSource = dt1;
            ddlManufacturerModel.DisplayMember = "VehicleModelDescription";
            ddlManufacturerModel.ValueMember = "VehiclemodelID";
        }
        DataTable dt2 = new DataTable();
        private void MethodVehicleMaker()
        {
            dt2 = objHSRP.PopulateVehicleMake();
            DataRow dr;
            dr = dt2.NewRow();
            dr.ItemArray = new object[] { "--Select Vehicle Maker--", "0" };
            dt2.Rows.InsertAt(dr, 0);
            ddlManufacturerMaker.DataSource = dt2;

             }

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


             if (e.KeyChar == (char)Keys.Delete)
            {
                ddlVehicleClass.Text = "--Select Vehicle Class--";
            } 
           
            e.KeyChar =(char)Keys.None;
            
        }

        private void DDLOrderType_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = (char)Keys.None;
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
                HSRPDataEntry.HsrpService.HSRPServiceSoapClient objHSRP = new HSRPDataEntry.HsrpService.HSRPServiceSoapClient();
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

     

        private void TxtBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
           
            if (char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void txtOwnerName_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsLetter(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Space);
        }

        private void txtCounterNo_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void txtdealer_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsLetter(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Space);
        }

     

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

        private void ddlDealerCenter_SelectedIndexChanged_1(object sender, EventArgs e)
        {

            if (ddlDealerCenter.Text.ToString() == "Other")
            {
                txtdealer.Visible = true;
                ddlDealerCenter.Visible = false;

            }

            else
            {
                txtdealer.Visible = false;
                ddlDealerCenter.Visible = true;
            }


        }

        private void txtVehRegNo_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (e.KeyChar == '$' || e.KeyChar == '!' || e.KeyChar == '%' || e.KeyChar == '&' || e.KeyChar == '.' || e.KeyChar == '<' || e.KeyChar == '>' || e.KeyChar == '?' || e.KeyChar == ',' || e.KeyChar == '-' || e.KeyChar == '/' || e.KeyChar == ' ' || e.KeyChar == '@' || e.KeyChar == '*' || e.KeyChar == '(' || e.KeyChar == ')' || e.KeyChar == '[' || e.KeyChar == ']' || e.KeyChar == '~' || e.KeyChar == '`' || e.KeyChar == '_' || e.KeyChar == '+' || e.KeyChar == '=')
            {
                e.Handled = false;

                MessageBox.Show("Special Character Not Allow In Vehicle Registration No.");
                txtVehRegNo.Text = "";
                return;

            }


        }

        private void ddlAffixCenter_KeyPress(object sender, KeyPressEventArgs e)
        {
          
            e.KeyChar = (char)Keys.None;
        }

        private void ddlDealerCenter_KeyPress(object sender, KeyPressEventArgs e)
        {
          
            e.KeyChar = (char)Keys.None;
          
          
        }

        private void ddlAffixCenter_SelectedIndexChanged(object sender, EventArgs e)
        {

          

        }

        private void ddlVehicleClass_KeyDown(object sender, KeyEventArgs e)
        {
          
          
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

       
        private void DDLOrderType_TabIndexChanged(object sender, EventArgs e)
        {
          
        }
        

        private void ddlVehicleClass_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (ddlVehicleClass.Text == "--Select Vehicle Class--")
            {
                txtAmount.Text = "";
                txtTax.Text = "";
                ddlVehType.Text = "--Select Vehicle Type--";
                DDLOrderType.Text = "--Select Order Type--";


            }


            else  if ( ddlVehType.Text == "--Select Vehicle Type--" && DDLOrderType.Text == "--Select Order Type--" )
            {
                txtAmount.Text = "";
                txtTax.Text = "";
                ddlVehType.Text = "--Select Vehicle Type--";
                DDLOrderType.Text = "--Select Order Type--";

            }


            else if (ddlVehicleClass.Text == "--Select Vehicle Class--" && DDLOrderType.Text == "--Select Order Type--" && ddlVehType.Text == "--Select Vehicle Type--")
            {
                txtAmount.Text = "";
                txtTax.Text = "";
                ddlVehType.Text = "--Select Vehicle Type--";
                DDLOrderType.Text = "--Select Order Type--";

            }
           

            else
            {

                HSRPDataEntry.HsrpService.HSRPServiceSoapClient objHSRP = new HSRPDataEntry.HsrpService.HSRPServiceSoapClient();
                string amount = objHSRP.GetRateAndTaxForVehicle(Stateid, ddlVehType.Text, ddlVehicleClass.Text, DDLOrderType.Text);
                string[] amount1 = amount.Split('^');
                txtAmount.Text = amount1[0].ToString();
                txtTax.Text = amount1[1].ToString();
               
            }
            
         
        }

        private void ddlVehType_SelectedIndexChanged_1(object sender, EventArgs e)
        {

            if (ddlVehType.Text == "--Select Vehicle Type--")
            {

                txtAmount.Text = "";
                txtTax.Text = "";
                DDLOrderType.Text = "--Select Order Type--";

            }

            else if (ddlVehicleClass.Text == "--Select Vehicle Class--" && DDLOrderType.Text == "--Select Order Type--")
            {
                txtAmount.Text = "";
                txtTax.Text = "";
                DDLOrderType.Text = "--Select Order Type--";

            }



            else if (ddlVehType.Text == "--Select Vehicle Type--" && ddlVehicleClass.Text == "--Select Vehicle Class--" && DDLOrderType.Text == "--Select Order Type--")
            {
                txtAmount.Text = "";
                txtTax.Text = "";
                DDLOrderType.Text = "--Select Order Type--";
            }

            else if (ddlVehType.Text == "--Select Vehicle Type--" && ddlVehicleClass.Text == "--Select Vehicle Class--" && DDLOrderType.Text == "--Select Order Type--")
            {
                txtAmount.Text = "";
                txtTax.Text = "";
                DDLOrderType.Text = "--Select Order Type--";
            }

            else if (ddlVehType.Text != "--Select Vehicle Type--" && ddlVehicleClass.Text != "--Select Vehicle Class--" && DDLOrderType.Text == "--Select Order Type--")
            {
                txtAmount.Text = "";
                txtTax.Text = "";
                DDLOrderType.Text = "--Select Order Type--";
            }



            else
            {

                HSRPDataEntry.HsrpService.HSRPServiceSoapClient objHSRP = new HSRPDataEntry.HsrpService.HSRPServiceSoapClient();
                string amount = objHSRP.GetRateAndTaxForVehicle(Stateid, ddlVehType.Text, ddlVehicleClass.Text, DDLOrderType.Text);
                string[] amount1 = amount.Split('^');
                txtAmount.Text = amount1[0].ToString();
                txtTax.Text = amount1[1].ToString();

            }


        }

       



        private void DDLOrderType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDLOrderType.Text == "--Select Order Type--")
            {
                txtAmount.Text = "";
                txtTax.Text = "";                

            }

            else if (ddlVehType.Text == "--Select Vehicle Type--" && ddlVehicleClass.Text == "--Select Vehicle Class--")
            {
                txtAmount.Text = "";
                txtTax.Text = "";
                DDLOrderType.Text = "--Select Order Type--";
               
                

            }

            else if (ddlVehType.Text == "--Select Order Type--" && ddlVehicleClass.Text == "--Select Vehicle Class--" && DDLOrderType.Text == "--Select Order Type--")
            {
                txtAmount.Text = "";
                txtTax.Text = "";
             
            }

            else if (ddlVehType.Text != "--Select Order Type--" && ddlVehicleClass.Text == "--Select Vehicle Class--" && DDLOrderType.Text == "--Select Order Type--")
            {
                txtAmount.Text = "";
                txtTax.Text = "";

            }

            else
            {

                HSRPDataEntry.HsrpService.HSRPServiceSoapClient objHSRP = new HSRPDataEntry.HsrpService.HSRPServiceSoapClient();
                string amount = objHSRP.GetRateAndTaxForVehicle(Stateid, ddlVehType.Text, ddlVehicleClass.Text, DDLOrderType.Text);
                string[] amount1 = amount.Split('^');
                txtAmount.Text = amount1[0].ToString();
                txtTax.Text = amount1[1].ToString();

            }

        }

        private void txtAuthorizationNo_TextChanged(object sender, EventArgs e)
        {

        }



      


    }
}
