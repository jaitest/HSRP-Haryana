using System;
using System.Data;
using System.Windows.Forms;
using HSRPTransferData;
using System.ComponentModel;

using System.IO;
using System.Drawing.Printing;
using System.Drawing;
using System.Net;
using System.Drawing.Text;
using System.Text;
using HSRPDataEntryNew.WebReferenceHP;
namespace HSRPDataEntryNew
{
    public partial class OrderEmbossing : Form
    {
        public OrderEmbossing()
        {
            InitializeComponent();
            //  Refresh();
        }

        HSRPService objWebServiceHP = new HSRPService();
        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        string OrderType = string.Empty;
        string Query = string.Empty;
        string Sticker = string.Empty;
        string Vip = string.Empty;
        string recno = string.Empty;
        string strAuthno = string.Empty;
        DataTable dt1 = new DataTable();

        private void btnSave_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(txtVehRegNo.Text))
            {
                MessageBox.Show("Please Insert Vehicle Registration No.");
                txtVehRegNo.Text = "";
                txtVehRegNo.Focus();
                return;
            }

            string Message = objWebServiceHP.ValidateLaserCode(txtFlaser.Text, txtRlaser.Text);
            string[] Message1 = Message.Split('^');


            #region Switch Case For OrderType Checking
            switch (lblOrderType.Text.ToUpper())
            {
                case "DF":
                    if (checkFront(Message1))
                    {
                        return;
                    }
                    if (string.IsNullOrEmpty(txtFlaser.Text))
                    {
                        MessageBox.Show("Please Insert Front Laser No.");
                        txtFlaser.Text = "";
                        txtFlaser.Focus();
                    }
                    if (txtFlaser.Text.Length > 10)
                    {
                        updateRecord();
                    }
                    else
                    {
                        MessageBox.Show("Length Laser No. should be more than 10");
                        txtFlaser.Focus();
                        return;
                    }
                    break;
                case "DR":

                    if (checkRear(Message1))
                    {
                        return;
                    }
                    if (string.IsNullOrEmpty(txtRlaser.Text))
                    {
                        MessageBox.Show("Please Insert Rear Laser No.");
                        txtRlaser.Text = "";
                        txtRlaser.Focus();
                    }
                    if (txtRlaser.Text.Length > 10)
                    {
                        updateRecord();
                    }
                    else
                    {
                        MessageBox.Show("Length Laser No. should be more than 10");
                        txtFlaser.Focus();
                        return;
                    }
                    break;

                default:
                    if (checkFront(Message1))
                    {
                        return;
                    }
                    if (checkRear(Message1))
                    {
                        return;
                    }
                    if (string.IsNullOrEmpty(txtFlaser.Text))
                    {
                        MessageBox.Show("Please Insert Front Laser No.");
                        txtFlaser.Text = "";
                        txtFlaser.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(txtRlaser.Text))
                    {
                        MessageBox.Show("Please Insert Rear Laser No.");
                        txtRlaser.Text = "";
                        txtRlaser.Focus();
                        return;
                    }
                    if (txtFlaser.Text.Length > 10 && txtRlaser.Text.Length > 10)
                    {
                        if (txtFlaser.Text.Trim() == txtRlaser.Text.Trim())
                        {
                            MessageBox.Show("Both Laser No. can not be same..");
                            txtRlaser.Focus();
                            return;
                        }
                        else
                        {
                            updateRecord();

                        }
                    }
                    else
                    {
                        MessageBox.Show("Length Laser No. should be more than 10");
                        txtFlaser.Focus();
                        return;
                    }

                    break;
            }
            #endregion
        }



        public bool checkFront(string[] Message1)
        {
            bool bReturn = false;
            if (Message1[0].ToString() == "FrontUsed")
            {
                MessageBox.Show("Front Laser No Already Used.");
                txtFlaser.Focus();
                bReturn = true;
            }
            else if (Message1[0].ToString() == "FrontNotFound")
            {
                MessageBox.Show("Front Laser No Not Found.");
                txtFlaser.Focus();
                bReturn = true;
            }
            return bReturn;
        }

        public bool checkRear(string[] Message1)
        {
            bool bReturn = false;
            if (Message1[1].ToString() == "RearUsed")
            {
                MessageBox.Show("Rear Laser No Already Used.");
                txtFlaser.Focus();
                bReturn = true;
            }
            else if (Message1[1].ToString() == "RearNotFound")
            {
                MessageBox.Show("Rear Laser No Not Found.");
                txtFlaser.Focus();
                bReturn = true;
            }
            return bReturn;
        }

        public void Refresh()
        {
            txtVehRegNo.Text = "";
            lblEngineno.Text = "";
            lblchassisno.Text = "";
            txtFlaser.Text = "";
            txtRlaser.Text = "";
            //checkBox1.
        }

        private void txtVehRegNo_TextChanged(object sender, EventArgs e)
        {
            lblMessage.Text = "";

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Refresh();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(txtVehRegNo.Text))
            {
                MessageBox.Show("Please Insert Vehicle Registration No.");

                txtVehRegNo.Text = "";
                txtVehRegNo.Focus();
                return;
            }

            checkBox1.Checked = false;
            checkBox1.Enabled = false;
            search_veh();

        }

        private void search_veh()
        {
            DataTable dtCheckOrder = objWebServiceHP.CheckOrderstatusForEmbossing("3", txtVehRegNo.Text.Replace(" ", ""));
            if (dtCheckOrder.Rows.Count > 0)
            {
                if (dtCheckOrder.Rows[0]["OrderStatus"].ToString().ToUpper() == "EMBOSSING DONE")
                {
                    MessageBox.Show("Vehicle No Already Embossed.....");
                    txtVehRegNo.Focus();
                    return;
                }
            }
            else
            {

                DataTable dtResult = objWebServiceHP.GetDeatilsUsingVehcileRegNo("3", txtVehRegNo.Text.Replace(" ", ""));

                if (dtResult.Rows.Count > 0)
                {
                    strAuthno = dtResult.Rows[0]["HsrpRecord_AuthorizationNo"].ToString();
                    txtVehRegNo.Text = dtResult.Rows[0]["VehicleRegNo"].ToString();
                    lblchassisno.Text = dtResult.Rows[0]["chassisno"].ToString();
                    lblEngineno.Text = dtResult.Rows[0]["Engineno"].ToString();

                    OrderType = dtResult.Rows[0]["OrderType"].ToString();
                    lblOrderType.Text = OrderType;

                    if ((dtResult.Rows[0]["Vehicletype"].ToString() == "LMV") || (dtResult.Rows[0]["Vehicletype"].ToString() == "LMV(Class)") || (dtResult.Rows[0]["Vehicletype"].ToString() == "Three Wheeler") || (dtResult.Rows[0]["Vehicletype"].ToString() == "MCV/HCV/TRAILERS"))
                    {
                        checkBox1.Checked = true;
                        checkBox1.Enabled = false;
                    }
                    else
                    {
                        checkBox1.Checked = false;
                        checkBox1.Enabled = false;
                    }
                    if (OrderType == "DF")
                    {
                        txtFlaser.Enabled = true;
                        txtRlaser.Enabled = false;
                    }
                    else if (OrderType == "DR")
                    {
                        txtFlaser.Enabled = false;
                        txtRlaser.Enabled = true;
                    }
                    else
                    {
                        txtFlaser.Enabled = true;
                        txtRlaser.Enabled = true;
                    }

                }
                else
                {
                    MessageBox.Show("Record Not Found");
                    txtVehRegNo.Focus();
                    return;
                }

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

        private void updateRecord()
        {
            string qry = string.Empty;
            string laservalue = string.Empty;

            string Message = objWebServiceHP.CheckLaserInHsrpRecords(lblOrderType.Text, txtFlaser.Text, txtRlaser.Text);
            if (Message == "FrontUsed")
            {
                MessageBox.Show("Front Laser No Already Used.");
                txtFlaser.Focus();
                return;
            }
            else if (Message == "RearUsed")
            {
                MessageBox.Show("Rear Laser No Already Used.");
                txtRlaser.Focus();
                return;

            }

            //if (lblOrderType.Text != "DR")     //added by amit on date 23/10/2013
            //{
            //    qry = "select vehicleregno from OrderBookingOffLine where hsrp_front_lasercode='" + txtFlaser.Text + "' or hsrp_rear_lasercode='" + txtFlaser.Text + "'";
            //    laservalue = utils.getDataSingleValue(qry, utils.getCnnHSRPApp, "vehicleregno");
            //    if (laservalue != "0")
            //    {
            //        MessageBox.Show("Front Laser No Already Used.");
            //        txtFlaser.Focus();
            //        return;
            //    }
            //}

            //if (lblOrderType.Text != "DF")      //added by amit on date 23/10/2013
            //{
            //    qry = "select vehicleregno from OrderBookingOffLine where hsrp_front_lasercode='" + txtRlaser.Text + "' or hsrp_rear_lasercode='" + txtRlaser.Text + "'";
            //    laservalue = utils.getDataSingleValue(qry, utils.getCnnHSRPApp, "vehicleregno");
            //    if (laservalue != "0")
            //    {
            //        MessageBox.Show("Rear Laser No Already Used.");
            //        txtRlaser.Focus();
            //        return;
            //    }
            //}

            string strResult = objWebServiceHP.UpdateLaserCode("3", txtVehRegNo.Text, lblOrderType.Text, txtFlaser.Text, txtRlaser.Text);
            if (strResult == "Success")
            {
                Query = "select * from OrderBookingOffLine where vehicleregno='" + txtVehRegNo.Text + "' and orderstatus='New Order'";
                dt1 = utils.GetDataTable(Query, utils.getCnnHSRPApp);
                if (dt1.Rows.Count > 0)
                {
                    txtVehRegNo.Text = dt1.Rows[0]["vehicleregno"].ToString();

                    //Commented by amit rohila on date 23/10/2013
                    //qry = "update OrderBookingOffLine set  orderembossingdate = getdate(),orderstatus='Embossing Done', hsrp_front_lasercode='" + txtFlaser.Text + "', hsrp_rear_lasercode ='" + txtRlaser.Text + "' where vehicleregno ='" + txtVehRegNo.Text + "' and orderstatus='New Order'";

                    if (lblOrderType.Text == "DF")      //added by amit rohila on date 23/10/2013
                    {
                        qry = "update OrderBookingOffLine set  orderembossingdate = getdate(),orderstatus='Embossing Done', hsrp_front_lasercode='" + txtFlaser.Text + "' where vehicleregno ='" + txtVehRegNo.Text + "' and orderstatus='New Order'";
                    }
                    else if (lblOrderType.Text == "DR")
                    {
                        qry = "update OrderBookingOffLine set  orderembossingdate = getdate(),orderstatus='Embossing Done', hsrp_rear_lasercode ='" + txtRlaser.Text + "' where vehicleregno ='" + txtVehRegNo.Text + "' and orderstatus='New Order'";
                    }
                    else
                    {
                        qry = "update OrderBookingOffLine set  orderembossingdate = getdate(),orderstatus='Embossing Done', hsrp_front_lasercode='" + txtFlaser.Text + "', hsrp_rear_lasercode ='" + txtRlaser.Text + "' where vehicleregno ='" + txtVehRegNo.Text + "' and orderstatus='New Order'";
                    }

                    int j = utils.ExecNonQuery(qry, utils.getCnnHSRPApp);

                    
                    if (lblOrderType.Text == "DF")      //added by amit rohila on date 23/10/2013
                    {
                        qry = "update HSRP_DTLS set HSRP_NO_FRONT='" + txtFlaser.Text + "',HSRP_ISSUE_DT =getdate() where replace(REGN_NO,' ','') ='" + txtVehRegNo.Text + "'";
                    }
                    else if (lblOrderType.Text == "DR")
                    {
                        qry = "update HSRP_DTLS set HSRP_NO_back= '" + txtRlaser.Text + "',HSRP_ISSUE_DT =getdate() where replace(REGN_NO,' ','') ='" + txtVehRegNo.Text + "'";
                    }
                    else
                    {
                        qry = "update HSRP_DTLS set HSRP_NO_FRONT='" + txtFlaser.Text + "',HSRP_NO_back= '" + txtRlaser.Text + "',HSRP_ISSUE_DT =getdate() where replace(REGN_NO,' ','') ='" + txtVehRegNo.Text + "'";
                    }

                    j = utils.ExecNonQuery(qry, utils.getCnnHSRPVahan);

                    
                    if (isConnected())
                    {

                        DateTime affdate = Convert.ToDateTime(dt1.Rows[0]["Record_CreationDate"].ToString()).AddDays(4);
                        string MobileNo = "91" + dt1.Rows[0]["MobileNo"].ToString();
                        string smsmessage = "Affixation date for your HSRP plate(s) for vehicle Reg. No. " + txtVehRegNo.Text.Trim() + " is " + affdate.ToString("dd-MM-yyyy") + ". at your HSRP affixation center. Team HSRP";
                        string url = "http://alerts.smseasy.in/api/web2sms.php?workingkey=627691h463b66gov5j83&sender=HPHSRP&to=" + MobileNo + "&message=" + smsmessage + "";
                        HttpWebRequest MyRequest = (HttpWebRequest)WebRequest.Create(url);
                        MyRequest.Method = "GET";
                        WebResponse myRespose = MyRequest.GetResponse();
                        StreamReader sr = new StreamReader(myRespose.GetResponseStream(), System.Text.Encoding.UTF8);
                        string result = sr.ReadToEnd();
                        sr.Close();
                        myRespose.Close();
                        //txtVehRegNo.Text = dt1.Rows[0]["vehicleregno"].ToString();
                        string qry0 = "update OrderBookingOffLine set  FirstSMSText ='" + smsmessage + "',FirstSMSDateTime=getdate(),FirstSMSServerReponseID='" + result.ToString() + "'  where vehicleregno = '" + txtVehRegNo.Text + "' and orderstatus='Embossing Done'";
                        utils.ExecNonQuery(qry0, utils.getCnnHSRPApp);
                        // string qry1 = "insert into HSRP_DTLS_SMS([MobileNo],[REGN_NO],[AUTH_NO],[HSRP_FLAG],[FirstSMSText],[FirstSMSSentDateTime],[FirstSMSServerResponseID]) values ('" + MobileNo + "','" + txtVehRegNo.Text + "','" + dt1.Rows[0]["HSRPRecord_AuthorizationNo"].ToString() + "','" + dt1.Rows[0]["ordertype"].ToString() + "','" + smsmessage + "',getdate(),'" + result.ToString() + "') ";

                        string strMessage = objWebServiceHP.UpdateHpSmsLog(MobileNo, smsmessage, result.ToString(), strAuthno);
                        if (strMessage == "Success")
                        {
                            string qry1 = "update HSRP_DTLS_SMS set [FirstSMSText]='" + smsmessage + "',[FirstSMSSentDateTime]=getdate(),[FirstSMSServerResponseID]='" + result.ToString() + "' where [MobileNo] ='" + MobileNo + "' and [AUTH_NO] ='" + dt1.Rows[0]["HSRPRecord_AuthorizationNo"].ToString() + "'";
                            utils.ExecNonQuery(qry1, utils.getCnnHSRPVahan);

                            MessageBox.Show("Record Saved and SMS Sent To Client On : " + MobileNo);
                            // Update offline order booking
                            // Update HSRP_HP DB for SMS
                        }
                        else
                        {
                            MessageBox.Show("Record Not Saved");
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Record Saved and SMS Not Sent as No Internet.");

                    }


                    return;


                }
            }
            else
            {
                MessageBox.Show("Record Not Found");
                txtVehRegNo.Focus();
                return;
            }

            Refresh();

        }

        private void button3_Click(object sender, EventArgs e)
        {

            Regno = "select top 1 [VehicleRegNo] from OrderBookingOffLine where  VehicleRegNo='" + txtVehRegNo.Text + "'";
            dt1 = utils.GetDataTable(Regno, utils.getCnnHSRPApp);
            if (dt1.Rows.Count > 0)
            {
                // check vehicle type
                VehicleType = "select * from OrderBookingOffLine where VehicleType='LMV' and  VehicleRegNo='" + txtVehRegNo.Text + "' or VehicleType='MCV/HCV/TRAILERS' and  VehicleRegNo='" + txtVehRegNo.Text + "' or VehicleType='LMV(CLASS)' and  VehicleRegNo='" + txtVehRegNo.Text + "'";
                dt2 = utils.GetDataTable(VehicleType, utils.getCnnHSRPApp);
                if (dt1.Rows.Count > 0)
                {
                    // check frant laser code and rear laser code assign are not
                    Embossing = "select top 1 [VehicleRegNo] from OrderBookingOffLine where orderstatus='Embossing Done' and VehicleRegNo='" + txtVehRegNo.Text + "' or OrderStatus='Closed' and VehicleRegNo='" + txtVehRegNo.Text + "'";
                    dt3 = utils.GetDataTable(Embossing, utils.getCnnHSRPApp);
                    if (dt3.Rows.Count > 0)
                    {
                        PrintDocument2();
                    }
                    else
                    {
                        MessageBox.Show("Laser Code Not Assign");
                    }
                }
                else
                {
                    MessageBox.Show("Vehicle Reg. No. is not Four wheeler");
                }

            }
            else
            {
                MessageBox.Show("Vehicle Reg. No. is not found");
            }

            //     PrintDocument p = new PrintDocument();
            //     p.PrintPage += delegate(object sender1, PrintPageEventArgs ee)
            //     {
            //         //dt1.Rows[0]["PrifixNo"].ToString()
            //         ee.Graphics.DrawString("TRANSPORT DEPARTMENT", new Font("Times New Roman", 25), new SolidBrush(Color.Black), new  Rectangle(0, 0, 950, 950));
            //         ee.Graphics.DrawString("GOVERNMENT OF HIMACHAL PRADESH", new Font("Times New Roman",25 ), new SolidBrush(Color.Black), new Rectangle(0, 18, 300, 300));
            //         ee.Graphics.DrawString("HP63C7098" , new Font("Times New Roman", 40), new SolidBrush(Color.Black), new Rectangle(0, 60, 300, 300));
            //         ee.Graphics.DrawString("AA220075353 - AA220075354", new Font("Times New Roman", 25), new SolidBrush(Color.Black), new Rectangle(0, 78, 300, 300));
            //         ee.Graphics.DrawString("ENGINE NO - GHD4E55454", new Font("Times New Roman", 25), new SolidBrush(Color.Black), new Rectangle(0, 78, 300, 300));
            //         ee.Graphics.DrawString("CHASSIS NO - MA1RU4GHKD3E39361", new Font("Times New Roman", 25), new SolidBrush(Color.Black), new Rectangle(0, 78, 300, 300));
            //     };

            //p.Print();

        }

        string HSRPStateName = string.Empty, Regno = string.Empty;
        public void PrintDocument2()
        {



            Regno = "select HSRPState.HSRPStateName,(HSRPState.statetext+' '+HSRPState.HSRPStateName) as name,OrderBookingOffLine.VehicleRegNo,OrderBookingOffLine.hsrp_front_lasercode,OrderBookingOffLine.hsrp_rear_lasercode,OrderBookingOffLine.EngineNo,OrderBookingOffLine.ChassisNo from OrderBookingOffLine inner join HSRPState on OrderBookingOffLine.HSRP_StateID=HSRPState.HSRP_StateID where OrderBookingOffLine.VehicleRegNo='" + txtVehRegNo.Text + "'";
            dt1 = utils.GetDataTable(Regno, utils.getCnnHSRPApp);
            HSRPStateName = dt1.Rows[0]["HSRPStateName"].ToString();
            PrintDocument p = new PrintDocument();
            // printerName
            p.PrinterSettings.PrinterName = utils.ThridStickerPrinterName();

            p.PrintPage += delegate(object sender1, PrintPageEventArgs ee)
            {

                string myText = "TRANSPORT DEPARTMENT";

                FontFamily fontFamily = new FontFamily("Lucida Console");
                Font font = new Font(fontFamily, 20, FontStyle.Regular, GraphicsUnit.Point);
                PointF pointF = new PointF(230, 64);
                StringFormat stringFormat = new StringFormat();
                SolidBrush solidBrush = new SolidBrush(Color.FromArgb(255, 0, 0, 255));

                stringFormat.FormatFlags = StringFormatFlags.DirectionVertical;

                ee.Graphics.DrawString(myText, font, solidBrush, pointF, stringFormat);


                Font font1 = new Font(fontFamily, 13, FontStyle.Regular, GraphicsUnit.Point);
                if (HSRPStateName == "HIMACHAL PRADESH" || HSRPStateName == "MADHYA PRADESH" || HSRPStateName == "ANDHRA PRADESH" || HSRPStateName == "UTTRAKHAND")
                {
                    PointF pointF1 = new PointF(200, 70);
                    ee.Graphics.DrawString(dt1.Rows[0]["name"].ToString(), font1, solidBrush, pointF1, stringFormat);
                }
                else
                {
                    PointF pointF1 = new PointF(200, 160);
                    ee.Graphics.DrawString(dt1.Rows[0]["name"].ToString(), font1, solidBrush, pointF1, stringFormat);
                }
                Font font2 = new Font(fontFamily, 20, FontStyle.Regular, GraphicsUnit.Point);
                PointF pointF2 = new PointF(160, 140);
                ee.Graphics.DrawString(dt1.Rows[0]["VehicleRegNo"].ToString(), font2, solidBrush, pointF2, stringFormat);

                PointF pointF3 = new PointF(120, 60);
                ee.Graphics.DrawString(dt1.Rows[0]["hsrp_front_lasercode"].ToString() + " - " + dt1.Rows[0]["hsrp_rear_lasercode"].ToString(), font1, solidBrush, pointF3, stringFormat);

                PointF pointF4 = new PointF(90, 60);
                ee.Graphics.DrawString("ENGINE NO - " + dt1.Rows[0]["EngineNo"].ToString(), font1, solidBrush, pointF4, stringFormat);

                PointF pointF5 = new PointF(60, 60);
                ee.Graphics.DrawString("CHASIS NO - " + dt1.Rows[0]["ChassisNo"].ToString(), font1, solidBrush, pointF5, stringFormat);


                //string myText = "TRANSPORT DEPARTMENT";

                //FontFamily fontFamily = new FontFamily("Lucida Console");
                //Font font = new Font(fontFamily, 20, FontStyle.Regular, GraphicsUnit.Point);
                //PointF pointF = new PointF(210, 38);
                //StringFormat stringFormat = new StringFormat();
                //SolidBrush solidBrush = new SolidBrush(Color.FromArgb(255, 0, 0, 255));

                //stringFormat.FormatFlags = StringFormatFlags.DirectionVertical;

                //ee.Graphics.DrawString(myText, font, solidBrush, pointF, stringFormat);


                //Font font1 = new Font(fontFamily, 13, FontStyle.Regular, GraphicsUnit.Point);

                //PointF pointF1 = new PointF(160, 120);
                //ee.Graphics.DrawString("NCT OF DELHI", font1, solidBrush, pointF1, stringFormat);

                //PointF pointF2 = new PointF(130, 140);
                //ee.Graphics.DrawString("DL1PC9857", font1, solidBrush, pointF2, stringFormat);

                //PointF pointF3 = new PointF(100, 50);
                //ee.Graphics.DrawString("AA140096815 - AA140096816", font1, solidBrush, pointF3, stringFormat);

                //PointF pointF4 = new PointF(70, 50);
                //ee.Graphics.DrawString("ENGINE NO - 82803406", font1, solidBrush, pointF4, stringFormat);

                //PointF pointF5 = new PointF(40, 50);
                //ee.Graphics.DrawString("CHASIS NO - MB1REEXC5AAVB0026", font1, solidBrush, pointF5, stringFormat);

                //PrintDocument doc = new PrintDocument();
                //string myText = "TRANSPORT DEPARTMENT";

                //FontFamily fontFamily = new FontFamily("Lucida Console");
                //Font font = new Font(fontFamily, 20, FontStyle.Regular, GraphicsUnit.Point);
                //PointF pointF = new PointF(195, 28);
                //StringFormat stringFormat = new StringFormat();
                //SolidBrush solidBrush = new SolidBrush(Color.FromArgb(255, 0, 0, 255));

                //stringFormat.FormatFlags = StringFormatFlags.DirectionVertical;

                //ee.Graphics.DrawString(myText, font, solidBrush, pointF, stringFormat);


                //Font font1 = new Font(fontFamily, 14, FontStyle.Regular, GraphicsUnit.Point);

                //PointF pointF1 = new PointF(160, 72);
                //ee.Graphics.DrawString("NCT OF DELHI", font1, solidBrush, pointF1, stringFormat);

                //PointF pointF2 = new PointF(130, 90);
                //ee.Graphics.DrawString("DL1PC9857", font1, solidBrush, pointF2, stringFormat);

                //PointF pointF3 = new PointF(100, 10);
                //ee.Graphics.DrawString("AA140096815 - AA140096816", font1, solidBrush, pointF3, stringFormat);

                //PointF pointF4 = new PointF(70, 40);
                //ee.Graphics.DrawString("ENGINE NO - 82803406", font1, solidBrush, pointF4, stringFormat);

                //PointF pointF5 = new PointF(40, 9);
                //ee.Graphics.DrawString("CHASIS NO - MB1REEXC5AAVB0026", font1, solidBrush, pointF5, stringFormat);


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

        private void button1_Click_1(object sender, EventArgs e)
        {

            txtVehRegNo.Text = "";
            lblchassisno.Text = "";
            lblEngineno.Text = "";
            txtFlaser.Enabled = true;
            txtRlaser.Enabled = true;
            txtFlaser.Text = "";
            txtRlaser.Text = "";
            lblOrderType.Text = "";
            txtVehRegNo.Focus();
        }
        string VehicleType = string.Empty, Embossing = string.Empty;
        DataTable dt2 = new DataTable();
        DataTable dt3 = new DataTable();

        private void button4_Click(object sender, EventArgs e)
        {
            Regno = "select top 1 [VehicleRegNo] from OrderBookingOffLine where  VehicleRegNo='" + txtVehRegNo.Text + "'";
            dt1 = utils.GetDataTable(Regno, utils.getCnnHSRPApp);
            if (dt1.Rows.Count > 0)
            {
                // check vehicle type
                VehicleType = "select * from OrderBookingOffLine where VehicleType='LMV' and  VehicleRegNo='" + txtVehRegNo.Text + "' or VehicleType='MCV/HCV/TRAILERS' and  VehicleRegNo='" + txtVehRegNo.Text + "' or VehicleType='LMV(CLASS)' and  VehicleRegNo='" + txtVehRegNo.Text + "'";
                dt2 = utils.GetDataTable(VehicleType, utils.getCnnHSRPApp);
                if (dt1.Rows.Count > 0)
                {
                    // check frant laser code and rear laser code assign are not
                    Embossing = "select top 1 [VehicleRegNo] from OrderBookingOffLine where orderstatus='Embossing Done' and VehicleRegNo='" + txtVehRegNo.Text + "' or OrderStatus='Closed' and VehicleRegNo='" + txtVehRegNo.Text + "'";
                    dt3 = utils.GetDataTable(Embossing, utils.getCnnHSRPApp);
                    if (dt3.Rows.Count > 0)
                    {
                        MerrerPrintDocument2();
                    }
                    else
                    {
                        MessageBox.Show("Laser Code Not Assign");
                    }
                }
                else
                {
                    MessageBox.Show("Vehicle Reg. No. is not Four wheeler");
                }

            }
            else
            {
                MessageBox.Show("Vehicle Reg. No. is not found");
            }
        }

        string fontpath = string.Empty;
        public void MerrerPrintDocument2()
        {



            Regno = "select HSRPState.HSRPStateName as statename,REVERSE(HSRPState.HSRPStateName) as HSRPStateName,REVERSE((HSRPState.statetext+' '+HSRPState.HSRPStateName)) as name,REVERSE(OrderBookingOffLine.VehicleRegNo) as VehicleRegNo,REVERSE(OrderBookingOffLine.hsrp_front_lasercode) as hsrp_front_lasercode,REVERSE(OrderBookingOffLine.hsrp_rear_lasercode) as hsrp_rear_lasercode,REVERSE(OrderBookingOffLine.EngineNo) as EngineNo,REVERSE(OrderBookingOffLine.ChassisNo) as ChassisNo from OrderBookingOffLine inner join HSRPState on OrderBookingOffLine.HSRP_StateID=HSRPState.HSRP_StateID  where OrderBookingOffLine.VehicleRegNo='" + txtVehRegNo.Text + "'";
            dt1 = utils.GetDataTable(Regno, utils.getCnnHSRPApp);
            HSRPStateName = dt1.Rows[0]["statename"].ToString();

            //  fontpath = System.Configuration.ConfigurationSettings.AppSettings["DataFolder"].ToString() + "PRSANSR.TTF";
            PrintDocument p = new PrintDocument();

            // printerName
            //   p.PrinterSettings.PrinterName = utils.ThridStickerPrinterName();

            //  p.PrinterSettings.PrinterName = ThridStickerPrinterName.ToString();


            //PrivateFontCollection myFonts = new PrivateFontCollection();
            //myFonts.AddFontFile(fontpath);

            // PrivateFontCollection pfc = new PrivateFontCollection();
            // pfc.AddFontFile("C:\\Users\\user\\Desktop\\backup_hp\\HSRPSoftware25092013dharmshala\\HSRPSoftware\\HSRPDataEntry\\Font\\PRSANSR.TTF");
            //  pfc.AddFontFile("C:\\Users\\user\\Desktop\\backup_hp\\HSRPSoftware25092013dharmshala\\HSRPSoftware\\HSRPDataEntry\\PRSANSR.TTF");
            //  label5.Font = new Font(pfc.Families[0], 16, FontStyle.Regular);

            //FontFamily fontFamily = new FontFamily("PRMirror");
            // FontFamily fontFamily = new FontFamily("PRMirror");


            fontpath = System.Configuration.ConfigurationSettings.AppSettings["DataFolder"].ToString() + "PRSANSR.TTF";
            PrivateFontCollection pfc = new PrivateFontCollection();
            pfc.AddFontFile(fontpath.ToString());

            //label5.Font = new Font(pfc.Families[0], 16, FontStyle.Regular);

            FontFamily fontFamily = new FontFamily("PRMirror");


            p.PrintPage += delegate(object sender1, PrintPageEventArgs ee)
            {

                //  fontpath = Environment.GetEnvironmentVariable("http://localhost:51047") + "C:\\Users\\user\\Desktop\\backup_hp\\HSRPSoftware\\HSRPSoftware\\HSRPDataEntry\\font\\PRSANSR.TTF";
                //  fontpath = System.Configuration.ConfigurationSettings.AppSettings["DataFolder"].ToString() + "PRSANSR.TTF";


                string myText = "TRANSPORT DEPARTMENT";
                StringBuilder sbtrnasportname = new StringBuilder();
                //  trnasportname = "TRANSPORT DEPARTMENT";

                for (int i = myText.Length - 1; i >= 0; i--)
                {
                    sbtrnasportname.Append(myText[i].ToString());
                }

                // FontFamily fontFamily = new FontFamily("Lucida Console");
                Font font = new Font(fontFamily, 18, FontStyle.Regular, GraphicsUnit.Point);
                PointF pointF = new PointF(220, 70);
                StringFormat stringFormat = new StringFormat();
                SolidBrush solidBrush = new SolidBrush(Color.FromArgb(255, 0, 0, 255));

                stringFormat.FormatFlags = StringFormatFlags.DirectionVertical;

                ee.Graphics.DrawString(sbtrnasportname.ToString(), font, solidBrush, pointF, stringFormat);


                Font font1 = new Font(fontFamily, 12, FontStyle.Regular, GraphicsUnit.Point);
                if (HSRPStateName == "HIMACHAL PRADESH" || HSRPStateName == "MADHYA PRADESH" || HSRPStateName == "ANDHRA PRADESH" || HSRPStateName == "UTTRAKHAND")
                {
                    PointF pointF1 = new PointF(190, 70);
                    ee.Graphics.DrawString(dt1.Rows[0]["name"].ToString(), font1, solidBrush, pointF1, stringFormat);
                }
                else
                {
                    PointF pointF1 = new PointF(200, 160);
                    ee.Graphics.DrawString(dt1.Rows[0]["name"].ToString(), font1, solidBrush, pointF1, stringFormat);
                }
                Font font2 = new Font(fontFamily, 18, FontStyle.Regular, GraphicsUnit.Point);
                PointF pointF2 = new PointF(150, 140);
                ee.Graphics.DrawString(dt1.Rows[0]["VehicleRegNo"].ToString(), font2, solidBrush, pointF2, stringFormat);

                PointF pointF3 = new PointF(120, 65);
                ee.Graphics.DrawString(dt1.Rows[0]["hsrp_front_lasercode"].ToString() + " - " + dt1.Rows[0]["hsrp_rear_lasercode"].ToString(), font1, solidBrush, pointF3, stringFormat);

                PointF pointF4 = new PointF(90, 65);
                ee.Graphics.DrawString("ENGINE NO - " + dt1.Rows[0]["EngineNo"].ToString(), font1, solidBrush, pointF4, stringFormat);

                PointF pointF5 = new PointF(60, 65);
                ee.Graphics.DrawString("CHASIS NO - " + dt1.Rows[0]["ChassisNo"].ToString(), font1, solidBrush, pointF5, stringFormat);


                //string myText = "TRANSPORT DEPARTMENT";

                //FontFamily fontFamily = new FontFamily("Lucida Console");
                //Font font = new Font(fontFamily, 20, FontStyle.Regular, GraphicsUnit.Point);
                //PointF pointF = new PointF(210, 38);
                //StringFormat stringFormat = new StringFormat();
                //SolidBrush solidBrush = new SolidBrush(Color.FromArgb(255, 0, 0, 255));

                //stringFormat.FormatFlags = StringFormatFlags.DirectionVertical;

                //ee.Graphics.DrawString(myText, font, solidBrush, pointF, stringFormat);


                //Font font1 = new Font(fontFamily, 13, FontStyle.Regular, GraphicsUnit.Point);

                //PointF pointF1 = new PointF(160, 120);
                //ee.Graphics.DrawString("NCT OF DELHI", font1, solidBrush, pointF1, stringFormat);

                //PointF pointF2 = new PointF(130, 140);
                //ee.Graphics.DrawString("DL1PC9857", font1, solidBrush, pointF2, stringFormat);

                //PointF pointF3 = new PointF(100, 50);
                //ee.Graphics.DrawString("AA140096815 - AA140096816", font1, solidBrush, pointF3, stringFormat);

                //PointF pointF4 = new PointF(70, 50);
                //ee.Graphics.DrawString("ENGINE NO - 82803406", font1, solidBrush, pointF4, stringFormat);

                //PointF pointF5 = new PointF(40, 50);
                //ee.Graphics.DrawString("CHASIS NO - MB1REEXC5AAVB0026", font1, solidBrush, pointF5, stringFormat);

                //PrintDocument doc = new PrintDocument();
                //string myText = "TRANSPORT DEPARTMENT";

                //FontFamily fontFamily = new FontFamily("Lucida Console");
                //Font font = new Font(fontFamily, 20, FontStyle.Regular, GraphicsUnit.Point);
                //PointF pointF = new PointF(195, 28);
                //StringFormat stringFormat = new StringFormat();
                //SolidBrush solidBrush = new SolidBrush(Color.FromArgb(255, 0, 0, 255));

                //stringFormat.FormatFlags = StringFormatFlags.DirectionVertical;

                //ee.Graphics.DrawString(myText, font, solidBrush, pointF, stringFormat);


                //Font font1 = new Font(fontFamily, 14, FontStyle.Regular, GraphicsUnit.Point);

                //PointF pointF1 = new PointF(160, 72);
                //ee.Graphics.DrawString("NCT OF DELHI", font1, solidBrush, pointF1, stringFormat);

                //PointF pointF2 = new PointF(130, 90);
                //ee.Graphics.DrawString("DL1PC9857", font1, solidBrush, pointF2, stringFormat);

                //PointF pointF3 = new PointF(100, 10);
                //ee.Graphics.DrawString("AA140096815 - AA140096816", font1, solidBrush, pointF3, stringFormat);

                //PointF pointF4 = new PointF(70, 40);
                //ee.Graphics.DrawString("ENGINE NO - 82803406", font1, solidBrush, pointF4, stringFormat);

                //PointF pointF5 = new PointF(40, 9);
                //ee.Graphics.DrawString("CHASIS NO - MB1REEXC5AAVB0026", font1, solidBrush, pointF5, stringFormat);


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

        string ThridStickerPrinterType = string.Empty;
        string ThridStickerPrinterName = string.Empty;
        private void OrderEmbossing_Load(object sender, EventArgs e)
        {
            ThridStickerPrinterType = utils.ThridStickerPrinterType();

            if (ThridStickerPrinterType == "White")
            {
                ThridStickerPrinterName = utils.ThridStickerPrinterName();
                // enable true 52 mm button and false a4 print button

            } if (ThridStickerPrinterType == "Yellow")
            {
                // enable false  52 mm button and true a4 print button
                ThridStickerPrinterName = utils.ThridStickerPrinterName();
            }
        }

    }

}
