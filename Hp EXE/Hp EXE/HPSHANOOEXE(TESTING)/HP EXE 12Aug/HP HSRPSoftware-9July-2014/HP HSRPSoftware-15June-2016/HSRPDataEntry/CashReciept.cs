using System;
using System.Data;
using System.Windows.Forms;
using HSRPTransferData;
using System.ComponentModel;
using System.IO;
using System.Drawing.Printing;
using System.Drawing;
using System.Net;
using HSRPDataEntryNew.WebReferenceHP;
using System.Data.SqlClient;


namespace HSRPDataEntryNew
{
    public partial class CashReciept : Form
    {
        HSRPService objWebServiceHP = new HSRPService();
      

        public CashReciept()
        {
            InitializeComponent();
        }
      

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();      
        }

        string Query   = string.Empty;
        string Sticker = string.Empty ;
        string Vip     = string.Empty;
        string recno = string.Empty;
        string address = string.Empty;
        string vehiclemake = string.Empty;
        string vehiclemodel = string.Empty;
        int RTO_CD=0;
        DataTable dt1  = new DataTable();
        
        private void btnSave_Click(object sender, EventArgs e)
        {
           

            if (string.IsNullOrEmpty(txtVehRegNo.Text.Trim()))
            {
                MessageBox.Show("Please Insert Vehicle Registration No.");

                txtVehRegNo.Text = "";
                txtVehRegNo.Focus();
                return;
            }
                        
            

            if (string.IsNullOrEmpty(txtAuthorizationNo.Text.Trim()))
            {
                MessageBox.Show("Please Insert Authorization No.");

                txtAuthorizationNo.Text = "";
                txtAuthorizationNo.Focus();
                return;
            }

            if (string.IsNullOrEmpty(txtOwnerName.Text.Trim()))
            {
                MessageBox.Show("Please Insert Owner Name");

                txtOwnerName.Text = "";
                txtOwnerName.Focus();
                return;
            }

        

            if ((string.IsNullOrEmpty(txtMobileNo.Text)) || (txtMobileNo.Text.Trim().Length < 10) ||  (txtMobileNo.Text.Trim().Length > 10))
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
            if (ddlOrderType.Text == "--Select Order Type--" || String.IsNullOrEmpty(ddlOrderType.SelectedItem.ToString()))
            { 
                MessageBox.Show("Please Select Order Type.");
                ddlOrderType.Focus();
                return;
            } 
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

            if (RTOSTA.Text == "--Select RTO STA--")
            {
                MessageBox.Show("Please Select RTO STA");

                RTOSTA.Focus();
                return;
            }


             string rtocode = RTOSTA.Text.ToString();


             string RTOLocationID = objWebServiceHP.GetRtoLocationID(rtocode, "3");
             string SaveMacAddress = utils.MacBase;
             recno = objWebServiceHP.GetCashInvoiceChallan("3", RTOLocationID, "Cash Receipt No");
           
           
            string HSRPRecord_CreationDate = DateTime.Now.ToString("yyyy-MM-dd");
            string HSRP_StateID = "3";
            string HSRPRecord_AuthorizationNo = txtAuthorizationNo.Text.Trim();
            string HSRPRecord_AuthorizationDate = AuthDate.Value.ToString();
            string VehicleRegNo = txtVehRegNo.Text.Replace(" ","");
            string OwnerName = txtOwnerName.Text;
            string ownerFatherName = string.Empty;
            string Address1 = string.Empty;

            string amount = objWebServiceHP.GetRateAndTaxForVehicle(HSRP_StateID, ddlVehType.Text, ddlVehicleClass.Text,orderType);
            string[] amount1 = amount.Split('^');
            txtAmount.Text = amount1[0].ToString();
            txtTax.Text = amount1[1].ToString();
            // int netamou=int.Parse(amount1[2].);
            string NetAmount = (Math.Round(Convert.ToDecimal(amount1[2]), 0)).ToString();// amount1[2].ToString(); ;
            string Roundoff_NetAmount = (Math.Round(Convert.ToDecimal(NetAmount), 2)).ToString();
            string VehicleType = ddlVehType.Text;
            string OrderStatus = "NEW ORDER";
            
            string ChassisNo = txtChassis.Text;
            string EngineNo = txtEngine.Text;
            string DealerCode = "0";
            string CreatedBy = utils.UserID;
            
            string Addrecordby = string.Empty;
            string ManufacturerMaker = string.Empty;
            string ManufacturerModel = string.Empty;

            string counter = string.Empty;
            string Affixid = string.Empty;

            string getdata = objWebServiceHP.GetRateAndTaxDetail(HSRP_StateID, ddlVehType.Text, ddlVehicleClass.Text, orderType);
            string[] getdatavalue = getdata.Split('^');
            // StrFrontPlatePrice + "^" + StrIsFrontPlate + "^" + StrRearPlatePrice + "^" + StrIsRearPlate + "^" + StickerID + "^" + screwrate + "^" + Discount + "^" + VATAMOUNT;
            // StrFrontPlatePrice + "^" + StrIsFrontPlate + "^" + StrRearPlatePrice + "^" + StrIsRearPlate + "^" + StickerID + "^" + screwrate + "^" + Discount + "^" + VATAMOUNT + "^" + FrontPlateID + "^" + RearPlateID;
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
            string vehicleref = string.Empty;
            
            string issave = objWebServiceHP.CheckDuplicateEntry(HSRP_StateID, txtAuthorizationNo.Text, txtVehRegNo.Text, ddlOrderType.SelectedItem.ToString());
            string[] issave1 = issave.Split('^');

            string flag = issave1[0].ToString();
            if (flag == "Fail")
            {
                lblMessage.Text = "Duplicate Records";
                return;
            }
            else
            {
                string save = objWebServiceHP.InsertDataCashCollection(HSRP_StateID, RTOLocationID, txtAuthorizationNo.Text, AuthDate.Value.ToString("yyyy-MM-dd"), txtVehRegNo.Text.Replace(" ", ""), txtOwnerName.Text, ownerFatherName, Address1, txtMobileNo.Text, ddlVehicleClass.Text, orderType, Sticker, Vip, NetAmount, Roundoff_NetAmount, ddlVehType.Text, OrderStatus, recno, txtChassis.Text, EngineNo, DealerCode, CreatedBy, SaveMacAddress, Addrecordby, ISFrontPlateSize, ISRearPlateSize, FrontPlateSize, RearPlateSize, Reference, ManufacturerModel, vehicleref, ManufacturerMaker, counter ,Affixid);

                string[] save1 = save.Split('^');
                if (save1[0].ToString() == "Record Not Saved")
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "Record saved Successfully.";
                }
                else if(save1[0].ToString()=="Mobile No Is Not Valid")
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "";
                    lblMessage.Text = "Mobile No Is Not Valid";
                    return;
                }
                else
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "";
                    lblMessage.Text = "Record not Save";
                    return;
                }

            }

           


            String SqlCheck = "select COUNT(*) from dbo.OrderBookingOffLine where hsrprecord_authorizationno ='" + txtAuthorizationNo.Text.Trim() + "'";
           
           
            float z = utils.getScalarCount(SqlCheck, utils.getCnnHSRPApp);
            if (z <= 0)
            {
                Query = "INSERT INTO OrderBookingOffLine (RTO_CD,address,ManufacturerName,ManufacturerModel,orderstatus, ChassisNo,Engineno, Record_CreationDate,HSRP_StateID,RTOLocationID,HSRPRecord_AuthorizationNo,HSRPRecord_AuthorizationDate,VehicleRegNo,OwnerName,MobileNo,VehicleClass,OrderType,StickerMandatory,VIP,Amount,VehicleType,CashReceiptNo,Tax,IsClosedEntrySentToServer,IsEmbossingSentToServer,IsCashReciptSentToServer,isRecordSentToServer)" + "values ('" + RTO_CD + "', '" + address + "', '" + vehiclemake + "','" + vehiclemodel + "', 'New Order','" + txtChassis.Text + "','" + txtEngine.Text + "', GetDate(), '" + utils.getStateId() + "','" + utils.getRtoLocationCode() + "','" + txtAuthorizationNo.Text + "','" + AuthDate.Value.ToString() + "','" + txtVehRegNo.Text.Replace(" ", "") + "','" + txtOwnerName.Text + "','" + txtMobileNo.Text + "','" + ddlVehicleClass.SelectedItem.ToString() + "','" + orderType + "','" + Sticker + "','" + Vip + "','" + txtAmount.Text + "','" + ddlVehType.SelectedItem.ToString() + "','" + recno + "','" + txtTax.Text + "','N','N','N','Y')";
                            

                int i = utils.ExecNonQuery(Query, utils.getCnnHSRPApp);
                if (i > 0)
                {

                    lblMessage.Text = "Save Records";
                    string query1 = "update prefix set lastno=lastno+1 where hsrp_stateid='"+utils.getStateId() +"' and rtolocationid ='"+utils.getRtoLocationCode() +"' and prefixfor='Cash Receipt No'";
                    int j = utils.ExecNonQuery(query1, utils.getCnnHSRPApp);
                    query1 = "update HSRP_DTLS set HSRP_FIX_AMT='" + txtAmount.Text.Trim() + "',HSRP_AMT_TAKEN_ON=getdate() where AUTH_NO='" + txtAuthorizationNo.Text.Trim() + "'";
                    j = utils.ExecNonQuery(query1, utils.getCnnHSRPVahan);

                    if (CashRecieptPrinterType == "A4")
                    {
                        PrintDocument pd = new PrintDocument();

                        //Set PrinterName as the selected printer in the printers list
                       
                       // pd.PrinterSettings.PrinterName = utils.PrinterName;

                        pd.PrinterSettings.PrinterName = CashRecieptPrinterName;

                        //Add PrintPage event handler
                        pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
                        //pd.PrintPage + = new PrintPageEventHandler (pd_PrintPage);

                        //Print the document
                        pd.Print();


                        // PrintDocumentA4();
                    }
                    else
                    {
                        PrintDocument();
                    }
                  

                }

               
                if (isConnected())
                {
                    
                    string MobileNo = "91" + txtMobileNo.Text ;
                    

                    string smsmessage = "Cash Rs." + txtAmount.Text.Trim() + " received against HSRP " + txtVehRegNo.Text.Trim() + " on " + DateTime.Now.ToString("dd-MM-yyyy") + " receipt number " + recno + ". Transport Dept HP.";// is " + affdate.ToString("dd-MM-yyyy") + ". at your HSRP affixation center. Team HSRP";
                    string url = "http://alerts.smseasy.in/api/web2sms.php?workingkey=627691h463b66gov5j83&sender=HPHSRP&to=" + MobileNo + "&message=" + smsmessage + "";
                    HttpWebRequest MyRequest = (HttpWebRequest)WebRequest.Create(url);
                    MyRequest.Method = "GET";
                    WebResponse myRespose = MyRequest.GetResponse();
                    StreamReader sr = new StreamReader(myRespose.GetResponseStream(), System.Text.Encoding.UTF8);
                    string result = sr.ReadToEnd();
                    sr.Close();
                    myRespose.Close();
                    objWebServiceHP.SaveSMSLog("3", MobileNo, txtVehRegNo.Text.Replace(" ", ""), txtAuthorizationNo.Text.Trim(), orderType, smsmessage,result.ToString(),"");
                    string qry = "insert into HSRP_DTLS_SMS([MobileNo],[REGN_NO],[AUTH_NO],[HSRP_FLAG],[CashReceiptSmsText],[CashReceiptSMSDateTime],[CashReceiptSMSServerResponseID]) values ('" + MobileNo + "','" + txtVehRegNo.Text + "','" + txtAuthorizationNo.Text.Trim() + "','" + orderType + "','" + smsmessage + "',getdate(),'" + result.ToString() + "') ";
                    int j = utils.ExecNonQuery(qry, utils.getCnnHSRPVahan);
                   
                    MessageBox.Show("Record Saved and SMS Sent To Client On : " + MobileNo);
                }
            }
            else
            {
                lblMessage.Text = "Vehicle Already Registred With This Authorization No, Please Use Other Transaction Type.";
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
        float amount=0;
        private void ddlVehType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlVehType.SelectedItem.ToString() == "LMV" || ddlVehType.SelectedItem.ToString() == "LMV(CLASS)" || ddlVehType.SelectedItem.ToString() == "MCV/HCV/TRAILERS")
            {
                radioButton1.Checked = true;
                radioButton2.Checked = false;
            }
            else
            {
                radioButton1.Checked = false;
                radioButton2.Checked = true;
            }
            txtAmount.Text = "";
            txtTax.Text = "";
            ddlOrderType.Text = "--Select Order Type--";
        }



        private void ddlOrderType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ( ddlVehicleClass.Text != "--Select Vehicle Class--"  && ddlVehType.Text  != "--Select Vehicle Type--"  && ddlOrderType.Text != "--Select Order Type--")
            { 

            if (ddlOrderType.SelectedText != "--Select Order Type--" )
            {
                if (ddlOrderType.SelectedItem.ToString() == "NEW BOTH PLATES")
                {
                    orderType = "NB";
                }
                else if (ddlOrderType.SelectedItem.ToString() == "OLD BOTH PLATES")
                {
                    orderType = "OB";
                }
                else if (ddlOrderType.SelectedItem.ToString() == "DAMAGED BOTH PLATES")
                {
                    orderType = "DB";
                }
                else if (ddlOrderType.SelectedItem.ToString() == "DAMAGED FRONT PLATE")
                {
                    orderType = "DF";
                }
                else if (ddlOrderType.SelectedItem.ToString() == "DAMAGED REAR PLATE")
                {
                    orderType = "DR";
                }
                else if (ddlOrderType.SelectedItem.ToString() == "ONLY STICKER")
                {
                    orderType = "OS";
                }
                else
                {
                    MessageBox.Show("Please Select Correct Order Type.");
                    ddlOrderType.Focus();
                    return;
                }

              

                DataTable dt = new DataTable();
              
                dt = utils.GetDataTable("select dbo.hsrpplateamt ('" + utils.getStateId() + "','" + ddlVehType.SelectedItem.ToString() + "','" + ddlVehicleClass.SelectedItem.ToString() + "','" + orderType + "') as Amount", utils.getCnnHSRPApp);
               
                string[] a = dt.Rows[0]["Amount"].ToString().Split('.');
                if (int.Parse(a[1]) > 50)
                {
                    a[0] = (int.Parse(a[0]) + 1).ToString();
                }
                
                txtAmount.Text = a[0];
                txtAmount.Enabled = false;

                dt = utils.GetDataTable("select dbo.hsrpplatetax ('" + utils.getStateId() + "','" + ddlVehType.SelectedItem.ToString() + "','" + ddlVehicleClass.SelectedItem.ToString() + "','" + orderType + "') as Tax", utils.getCnnHSRPApp);
            
                txtTax.Text = dt.Rows[0]["Tax"].ToString();
                txtTax.Enabled = false;

            }

            }
        }

        private void PrintDocument()
        {
           // string s = "Cost" + System.DateTime.Today.ToShortDateString() + Environment.NewLine + "-------------------------------------" + Environment.NewLine;

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
                ee.Graphics.DrawString(utils.getCompanyName().ToString() , new Font("Times New Roman", 7), new SolidBrush(Color.Black), new Rectangle(0, 0, 950, 950));
                ee.Graphics.DrawString(utils.getRTOLocationAddress().ToString(), new Font("Times New Roman", 8), new SolidBrush(Color.Black), new Rectangle(0, 18, 300, 300));
                ee.Graphics.DrawString("Receipt Date       : " + System.DateTime.Now.ToString("dd-MM-yyyy"), new Font("Times New Roman", 10), new SolidBrush(Color.Black), new Rectangle(0, 60, 300, 300));
                ee.Graphics.DrawString("Receipt No         :  " + recno , new Font("Times New Roman", 10), new SolidBrush(Color.Black), new Rectangle(0, 78, 300, 300));
                //ee.Graphics.DrawString("Authorization No :  "+txtAuthorizationNo.Text, new Font("Times New Roman", 10), new SolidBrush(Color.Black), new Rectangle(0,78,300,300));
                ee.Graphics.DrawString("Vehicle Reg No   :  "+txtVehRegNo.Text, new Font("Times New Roman", 10), new SolidBrush(Color.Black), new Rectangle(0, 96, 300, 300));
                ee.Graphics.DrawString("Owner Name       :  " + txtOwnerName.Text, new Font("Times New Roman", 10), new SolidBrush(Color.Black), new Rectangle(0, 114, 300, 300));
                //ee.Graphics.DrawString("Mobile No          :  " + txtMobileNo.Text, new Font("Times New Roman", 10), new SolidBrush(Color.Black), new Rectangle(0, 132, 300, 300));
                ee.Graphics.DrawString("Vehicle Class      :  " + ddlVehicleClass.SelectedItem.ToString(), new Font("Times New Roman", 10), new SolidBrush(Color.Black), new Rectangle(0, 132, 300, 300));
                ee.Graphics.DrawString("Vehicle Type      :  " + ddlVehType.SelectedItem.ToString(), new Font("Times New Roman", 10), new SolidBrush(Color.Black), new Rectangle(0, 150, 300, 300));
                //ee.Graphics.DrawString("Order Type         :  " + orderType, new Font("Times New Roman", 10), new SolidBrush(Color.Black), new Rectangle(0,162, 300, 300));
                //ee.Graphics.DrawString("Sticker               :  " + Sticker, new Font("Times New Roman", 10), new SolidBrush(Color.Black), new Rectangle(0,180, 300, 300));
                //ee.Graphics.DrawString("VIP                    :  " + Vip, new Font("Times New Roman", 10), new SolidBrush(Color.Black), new Rectangle(0, 198, 300, 100));
                ee.Graphics.DrawString("Tax                    :  " + txtTax.Text, new Font("Times New Roman", 10), new SolidBrush(Color.Black), new Rectangle(0, 166, 300, 300));
                ee.Graphics.DrawString("Amount              :  " + txtAmount.Text, new Font("Times New Roman", 10), new SolidBrush(Color.Black), new Rectangle(0,184, 300, 300));
                //ee.Graphics.DrawString("Net Amount              :  " + System.Math.Round(Convert.ToDecimal(txtAmount.Text), 0), new Font("Times New Roman", 10), new SolidBrush(Color.Black), new Rectangle(0, 184, 300, 300));
                ee.Graphics.DrawString("Disclaimer : ", new Font("Times New Roman", 9), new SolidBrush(Color.Black), new Rectangle(0, 202, 300, 300)); 
               
                ee.Graphics.DrawString("Vehicle Owner is requested to please check the Correctness of the cash slip. The company shall not be responsible for any clerical mistake what so ever.", new Font("Times New Roman", 10), new SolidBrush(Color.Black), new Rectangle(0, 220, 260, 300));

                 ee.Graphics.DrawString(".", new Font("Times New Roman", 5), new SolidBrush(Color.Black), new Rectangle(0, 298, 300, 300));
                  


            };
            try
            {
                p.Print();

              
            }
            catch (Exception ex)
            {
                throw new Exception("Exception Occured While Printing", ex);
            }
        }

      

        private void PrintDocument2()
        {
            // string s = "Cost" + System.DateTime.Today.ToShortDateString() + Environment.NewLine + "-------------------------------------" + Environment.NewLine;

           

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
                ee.Graphics.DrawString(utils.getCompanyName().ToString(), new Font("Times New Roman", 7), new SolidBrush(Color.Black), new Rectangle(0, 0, 950, 950));
                ee.Graphics.DrawString(utils.getRTOLocationAddress().ToString(), new Font("Times New Roman", 8), new SolidBrush(Color.Black), new Rectangle(0, 18, 300, 300));
                ee.Graphics.DrawString("Receipt Date       : " + System.DateTime.Now.ToString("dd-MM-yyyy"), new Font("Times New Roman", 10), new SolidBrush(Color.Black), new Rectangle(0, 60, 300, 300));
                ee.Graphics.DrawString("Receipt No         :  " + recno, new Font("Times New Roman", 10), new SolidBrush(Color.Black), new Rectangle(0, 78, 300, 300));
                //ee.Graphics.DrawString("Authorization No :  "+txtAuthorizationNo.Text, new Font("Times New Roman", 10), new SolidBrush(Color.Black), new Rectangle(0,78,300,300));
                ee.Graphics.DrawString("Vehicle Reg No   :  " + txtVehRegNo.Text, new Font("Times New Roman", 10), new SolidBrush(Color.Black), new Rectangle(0, 96, 300, 300));
                ee.Graphics.DrawString("Owner Name       :  " + txtOwnerName.Text, new Font("Times New Roman", 10), new SolidBrush(Color.Black), new Rectangle(0, 114, 300, 300));
                //ee.Graphics.DrawString("Mobile No          :  " + txtMobileNo.Text, new Font("Times New Roman", 10), new SolidBrush(Color.Black), new Rectangle(0, 132, 300, 300));
                ee.Graphics.DrawString("Vehicle Class      :  " + ddlVehicleClass.SelectedItem.ToString(), new Font("Times New Roman", 10), new SolidBrush(Color.Black), new Rectangle(0, 132, 300, 300));
                ee.Graphics.DrawString("Vehicle Type      :  " + ddlVehType.SelectedItem.ToString(), new Font("Times New Roman", 10), new SolidBrush(Color.Black), new Rectangle(0, 150, 300, 300));
                //ee.Graphics.DrawString("Order Type         :  " + orderType, new Font("Times New Roman", 10), new SolidBrush(Color.Black), new Rectangle(0,162, 300, 300));
                //ee.Graphics.DrawString("Sticker               :  " + Sticker, new Font("Times New Roman", 10), new SolidBrush(Color.Black), new Rectangle(0,180, 300, 300));
                //ee.Graphics.DrawString("VIP                    :  " + Vip, new Font("Times New Roman", 10), new SolidBrush(Color.Black), new Rectangle(0, 198, 300, 100));
                ee.Graphics.DrawString("Tax                    :  " + txtTax.Text, new Font("Times New Roman", 10), new SolidBrush(Color.Black), new Rectangle(0, 166, 300, 300));
                ee.Graphics.DrawString("Amount              :  " + txtAmount.Text, new Font("Times New Roman", 10), new SolidBrush(Color.Black), new Rectangle(0, 184, 300, 300));
                //ee.Graphics.DrawString("Net Amount              :  " + System.Math.Round(Convert.ToDecimal(txtAmount.Text),0), new Font("Times New Roman", 10), new SolidBrush(Color.Black), new Rectangle(0, 184, 300, 300));
                ee.Graphics.DrawString("Disclaimer : ", new Font("Times New Roman", 9), new SolidBrush(Color.Black), new Rectangle(0, 202, 300, 300));
                //ee.Graphics.DrawString("Before leaving the counter Vehicle  owner should check receipt details, failing which the company shall not be responsible in any manner.", new Font("Times New Roman", 9), new SolidBrush(Color.Black), new Rectangle(0, 230, 300, 300));
                ee.Graphics.DrawString("Vehicle Owner is requested to please check the Correctness of the cash slip. The company shall not be responsible for any clerical mistake what so ever.", new Font("Times New Roman", 10), new SolidBrush(Color.Black), new Rectangle(0, 220, 260, 300));
                ee.Graphics.DrawString(".", new Font("Times New Roman", 5), new SolidBrush(Color.Black), new Rectangle(0, 298, 300, 300));

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
            txtMobileNo.Text = "";
            txtAuthorizationNo.Text = "";
            txtAmount.Text = "";
            //ddlVehicleClass.Text = "--Select Vehicle Class--";
            //ddlVehType.Text = "--Select Vehicle Type--";
            //ddlOrderType.Text = "--Select Order Type--";
            chkVIP.Checked = false;
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            txtTax.Text = "";
        }

           

        private void button2_Click(object sender, EventArgs  e)
        {
            PrintDocument();
            //PrintDoc printer = new PrintDoc();

           
            //printer.PrintText = txttest.Text;

            //printer.Print();
             

        }


        static string stateID, LocationID, state, RTOLocationAddress, CompanyName, CurrentDate, UserLoginID;

        

        private void txtVehRegNo_TextChanged(object sender, EventArgs e)
        {
            lblMessage.Text = "";

            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Refresh();
        }
        string Prefix;
      
        private void button3_Click(object sender, EventArgs e)
        {
            //if (string.IsNullOrEmpty(txtVehRegNo.Text))
            //{
            //    MessageBox.Show("Please Insert Vehicle Registration No.");

            //    txtVehRegNo.Text = "";
            //    txtVehRegNo.Focus();
            //    return;
            //}



            //if (string.IsNullOrEmpty(txtAuthorizationNo.Text))
            //{
            //    MessageBox.Show("Please Insert Authorization No.");

            //    txtAuthorizationNo.Text = "";
            //    txtAuthorizationNo.Focus();
            //    return;
            //}

            //if (string.IsNullOrEmpty(txtOwnerName.Text))
            //{
            //    MessageBox.Show("Please Insert Owner Name");

            //    txtOwnerName.Text = "";
            //    txtOwnerName.Focus();
            //    return;
            //}

            ////int success =  
            ////(int.Parse(txtMobileNo.Text)==true)

            //if ((string.IsNullOrEmpty(txtMobileNo.Text)) || (txtMobileNo.Text.Trim().Length < 10) || (txtMobileNo.Text.Trim().Length > 10))
            //{
            //    MessageBox.Show("Please Insert Mobile No.");

            //    txtMobileNo.Text = "";
            //    txtMobileNo.Focus();
            //    return;
            //}

            //if (ddlVehicleClass.Text == "--Select Vehicle Class--" || String.IsNullOrEmpty(ddlVehicleClass.SelectedItem.ToString()))
            //{
            //    MessageBox.Show("Please Select Vehicle Class.");
            //    ddlVehicleClass.Focus();
            //    return;
            //}
            //if (ddlVehType.Text == "--Select Vehicle Type--" || String.IsNullOrEmpty(ddlVehType.SelectedItem.ToString()))
            //{
            //    MessageBox.Show("Please Select Vehicle Type.");
            //    ddlVehType.Focus();
            //    return;
            //}
            //if (ddlOrderType.Text == "--Select Order Type--" || String.IsNullOrEmpty(ddlOrderType.SelectedItem.ToString()))
            //{
            //    MessageBox.Show("Please Select Order Type.");
            //    ddlOrderType.Focus();
            //    return;
            //}
            //if (txtAmount.Text == "")
            //{
            //    MessageBox.Show("Please Insert Amount.");
            //    txtAmount.Text = "";
            //    txtAmount.Focus();
            //    return;
            //}

            //if (radioButton1.Checked == true)
            //{
            //    Sticker = "Y";
            //}
            //else
            //{
            //    Sticker = "N";
            //}

            //if (chkVIP.Checked == true)
            //{
            //    Vip = "Y";
            //}
            //else
            //{
            //    Vip = "N";
            //}
            //string vehcleRegNo  = string.Empty;
            //if (txtRegNo.Text.Replace(" ", "")=="")
            //{
            //  vehcleRegNo=  txtVehRegNo.Text.Trim().Replace(" ","");
            //}else{

            //    vehcleRegNo = txtRegNo.Text.Trim().Replace(" ", "");
            //}

            //string qry = "select vehicleregno from OrderBookingOffLine where REPLACE(vehicleregno,space(1),'')='" + vehcleRegNo + "' and ordertype ='" + orderType + "'";
            //string checkvalue = utils.getDataSingleValue(qry, utils.getCnnHSRPApp, "vehicleregno");
            //if (checkvalue == "0")
            //{
            //    MessageBox.Show("Record Not Booked Yet.");
            //    txtRegNo.Focus();
            //    return;
            //}
            ////PrintDocument();
            //if (CashRecieptPrinterType == "A4")
            //{
            //    PrintDocument pd = new PrintDocument();

            //    //Set PrinterName as the selected printer in the printers list
            //    // CashReceipt.Form1 OBJ = new CashReceipt.Form1();
            //  //  pd.PrinterSettings.PrinterName = utils.PrinterName;

            //    pd.PrinterSettings.PrinterName = CashRecieptPrinterName;
            //    //Add PrintPage event handler
            //    pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
            //    //pd.PrintPage + = new PrintPageEventHandler (pd_PrintPage);

            //    //Print the document
            //    pd.Print();
            //    // PrintDocumentA4();
            //}
            //else
            //{
            //    PrintDocument();
            //}
        }

        public void pd_PrintPage(object sender, PrintPageEventArgs e)
        {

            string companyname = utils.getCompanyName();
            string ComAdd = utils.getRTOLocationAddress();
            string Heading = "CASH RECEIPT";
            string CashReceiptNo = recno;
            string DATE = System.DateTime.Now.ToString("dd-MM-yyyy");
            string TINNO = "07460419550", TIME = System.DateTime.Now.ToString("HH:MM");

            string AUTHNO = txtAuthorizationNo.Text, AUTHDATE = AuthDate.Text, ORDERBOOKINGNO = " ", OWNERNAME = txtOwnerName.Text, CONTACTNO = txtMobileNo.Text, OWNERADDRESS = " ", VEHICLEREG = txtVehRegNo.Text, VEHICLEMODEL = ddlVehType.SelectedItem.ToString(), ENGINENO = txtEngine.Text, CHASSISNO = txtChassis.Text;
            string AMOUNT = txtAmount.Text;
            string ROUNDOFAMOUNT = txtAmount.Text;
            string NETAMOUNT = txtAmount.Text;

            FontFamily fontFamily = new FontFamily("Lucida Console");

            StringFormat stringFormat = new StringFormat();
            SolidBrush solidBrush = new SolidBrush(Color.FromArgb(255, 0, 0, 255));

            // stringFormat.FormatFlags = StringFormatFlags.DirectionVertical;

            // stringFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft;

            // HEADING OF PDF
            Font font1111 = new Font(fontFamily, 20, FontStyle.Regular, GraphicsUnit.Point);
            PointF pointF = new PointF(180, 20);
            e.Graphics.DrawString(companyname, font1111, solidBrush, pointF, stringFormat);


            Font font1 = new Font(fontFamily, 10, FontStyle.Regular, GraphicsUnit.Point);
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



            PointF pointF36 = new PointF(50, 530);
            e.Graphics.DrawString("---------------------------------------------------------------------------------------", font3, solidBrush, pointF36, stringFormat);



            Font font38 = new Font(fontFamily, 20, FontStyle.Regular, GraphicsUnit.Point);
            PointF pointF38 = new PointF(180, 550);
            e.Graphics.DrawString(companyname, font38, solidBrush, pointF38, stringFormat);


            Font font39 = new Font(fontFamily, 10, FontStyle.Regular, GraphicsUnit.Point);
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


        private void search_veh(string flag,string vehicleNo)
        {
            string Query;
            if (flag =="OLD")
                Query = "select * from HSRP_DTLS where REPLACE(REGN_NO,space(1),'')='" + vehicleNo.Replace(" ","") + "' and HSRP_FLAG ='" + orderType + "'";
            else
                Query = "select * from HSRP_DTLS where REPLACE(REGN_NO,space(1),'')='" + vehicleNo.Replace(" ", "") + "' and HSRP_FLAG ='NB'";

            dt1 = utils.GetDataTable(Query, utils.getCnnHSRPVahan);
            if (dt1.Rows.Count > 0)
            {
                txtVehRegNo.Text = dt1.Rows[0]["REGN_NO"].ToString();
                txtAuthorizationNo.Text = dt1.Rows[0]["AUTH_NO"].ToString();
                AuthDate.Value = DateTime.Parse(dt1.Rows[0]["REGN_DT"].ToString());
                txtOwnerName.Text = dt1.Rows[0]["O_NAME"].ToString();
                txtChassis.Text =  dt1.Rows[0]["CHASI_NO"].ToString();
                txtEngine.Text = dt1.Rows[0]["ENG_NO"].ToString();                    
                if ((dt1.Rows[0]["VEH_TYPE"].ToString() == "TRANSPORT") || (dt1.Rows[0]["VEH_TYPE"].ToString() == "Transport"))
                {
                    ddlVehicleClass.SelectedItem = "Transport";
                }
                else {
                    ddlVehicleClass.SelectedItem = "Non-Transport";
                }
                vehiclemake = dt1.Rows[0]["MAKER"].ToString(); ;
                vehiclemodel = dt1.Rows[0]["MAKER_MODEL"].ToString();
                address = dt1.Rows[0]["O_ADDRESS"].ToString();
                RTO_CD = Convert.ToInt32(dt1.Rows[0]["RTO_CD"].ToString());
                string vtype = utils.getScalarValue("select OurValue from Mapping_Vahan_HSRP where VahanValue='" + dt1.Rows[0]["VH_CLASS"].ToString() + "' ", utils.getCnnHSRPApp);
                ddlVehType.SelectedItem = vtype;
                
                if (dt1.Rows[0]["HSRP_FLAG"].ToString() == "NB")
                {
                    ddlOrderType.SelectedItem = "NEW BOTH PLATES";

                }
                else if (dt1.Rows[0]["HSRP_FLAG"].ToString() == "OB")
                {
                    ddlOrderType.SelectedItem = "OLD BOTH PLATES";

                }
                else if (dt1.Rows[0]["HSRP_FLAG"].ToString() == "DB")
                {
                    ddlOrderType.SelectedItem = "DAMAGED BOTH PLATES";

                }
                else if (dt1.Rows[0]["HSRP_FLAG"].ToString() == "DF")
                {
                    ddlOrderType.SelectedItem = "DAMAGED FRONT PLATE";

                }
                else if (dt1.Rows[0]["HSRP_FLAG"].ToString() == "DR")
                {
                    ddlOrderType.SelectedItem = "DAMAGED REAR PLATE";

                }
                else if (dt1.Rows[0]["HSRP_FLAG"].ToString() == "OS")
                {
                    ddlOrderType.SelectedItem = "ONLY STICKER";

                }
                else
                {
                    MessageBox.Show("Please Select Correct Order Type.");
                    ddlOrderType.Focus();
                    return;
                }


            }
            else
            {
                MessageBox.Show("Record Not Found");
                txtVehRegNo.Focus();
                return;
            }

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void txtTax_TextChanged(object sender, EventArgs e)
        {

        }

        private void ddlVehicleClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtAmount.Text = "";
            txtTax.Text = "";
            ddlVehType.Text = "--Select Vehicle Type--";
            ddlOrderType.Text = "--Select Order Type--";
        }

        private void ddlordertypeOLd_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlordertypeOLd.SelectedText != "--Select Order Type--")
            {
                if (ddlordertypeOLd.SelectedItem.ToString() == "NEW BOTH PLATES")
                {
                    orderType = "NB";
                }
                else if (ddlordertypeOLd.SelectedItem.ToString() == "OLD BOTH PLATES")
                {
                    orderType = "OB";
                }
                else if (ddlordertypeOLd.SelectedItem.ToString() == "DAMAGED BOTH PLATES")
                {
                    orderType = "DB";
                }
                else if (ddlordertypeOLd.SelectedItem.ToString() == "DAMAGED FRONT PLATE")
                {
                    orderType = "DF";
                }
                else if (ddlordertypeOLd.SelectedItem.ToString() == "DAMAGED REAR PLATE")
                {
                    orderType = "DR";
                }
                else if (ddlordertypeOLd.SelectedItem.ToString() == "ONLY STICKER")
                {
                    orderType = "OS";
                }
                else
                {
                    MessageBox.Show("Please Select Correct Order Type.");
                    ddlordertypeOLd.Focus();
                    return;
                }
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Refresh(); 
        }

        private void btn_Go_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(txtRegNo.Text))
            {
                MessageBox.Show("Please Insert  Vehicle Registration No.");

                txtRegNo.Text = "";
                txtRegNo.Focus();
                return;
            }



            if (string.IsNullOrEmpty(txtChasisNo.Text))
            {
                MessageBox.Show("Please Insert Chassis No.");

                txtChasisNo.Text = "";
                txtChasisNo.Focus();
                return;
            }
            if (RTOSTA.Text == "--Select RTO STA--")
            {
                MessageBox.Show("Please Select RTO STA");
                              
                RTOSTA.Focus();
                return;
            }
            if (ddlordertypeOLd.Text =="--Select Order Type--")
            {
                MessageBox.Show("Please Select Order Type");

                ddlordertypeOLd.Focus();
                return;
            }

            if (orderType == "NB")
            {
                search_veh("NEW", txtRegNo.Text);
            }          
            else
            {
                   search_veh("OLD", txtRegNo.Text);
               
            }

        }
        string CashRecieptPrinterType = string.Empty;
        string CashRecieptPrinterName = string.Empty;
        private void CashReciept_Load(object sender, EventArgs e)
        {

            if (isConnected())
            {
                lblInternetStatus.Visible = true;
                btnSave.Visible = true;
                button1.Visible = true;
               // button3.Visible = true;
                BtnClose.Visible = true;
                groupBox5.Visible = true;
                groupBox1.Visible = true;
            }
            else
            {
                MessageBox.Show("Internet Is Not Connected");
                
            }
            string ws = "select * from RTOCodeForDropdown";
            DataTable dt = utils.GetDataTable(ws.ToString(),utils.getCnnHSRPApp);

            DataRow dr;
            dr = dt.NewRow();
            dr.ItemArray = new object[] { "--Select RTO STA--", "--Select RTO STA--" };
            dt.Rows.InsertAt(dr, 0);


            RTOSTA.DataSource = dt;
            RTOSTA.DisplayMember = "webservices_URL_Key";
            RTOSTA.ValueMember = "webservices_URL";
     
         
            CashRecieptPrinterType = utils.CashRecieptPrinterType();
           
            if (CashRecieptPrinterType== "52")
            {
                CashRecieptPrinterName = utils.CashRecieptPrinterName();
                // enable true 52 mm button and false a4 print button

            }
            
            if (CashRecieptPrinterType == "A4")
            {
                // enable false  52 mm button and true a4 print button
                CashRecieptPrinterName = utils.CashRecieptPrinterName();
            }
            //select printer for 3rd sticler from DB
            

        }

        private void RTOSTA_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void txtMobileNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || e.KeyChar == '\b')
            {
            }
            else
            {
                e.Handled = true;
            }
        }

        private void btn_ExportDtls_Click(object sender, EventArgs e)
        {

        }
        
        
        
    }
}
