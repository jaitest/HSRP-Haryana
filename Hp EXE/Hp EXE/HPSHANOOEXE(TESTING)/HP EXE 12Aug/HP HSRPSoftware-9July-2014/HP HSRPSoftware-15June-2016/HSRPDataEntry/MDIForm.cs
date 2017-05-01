using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HSRPTransferData;
using System.Net;
using System.IO;
using HSRPDataEntryNew.WebReferenceHP;

namespace HSRPDataEntryNew
{
    public partial class MDIForm : Form
    {
        string CnnString = String.Empty;
        string SqlString = String.Empty;

        string StrConnLocal = utils.GetLocalDBConnectionFromINI();
        string StrConnLocalApp = utils.GetVahanDBConnectionFromINI();
        private int childFormNumber = 0;
        HSRPService objWebServiceHP = new HSRPService();
        public MDIForm()
        {
            InitializeComponent();
        }
        
        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }


    

        private void cashReceiptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
            CashReciept cr = new CashReciept();
            cr.MdiParent = this;
            cr.StartPosition = FormStartPosition.CenterScreen;
            cr.Show();
        }
        
        private void MDIForm_Load(object sender, EventArgs e)
        {
           //  Timer MyTimer = new Timer();
          ////  MyTimer.Interval = (60000*60); // 45 mins
          //  MyTimer.Interval = (60000*10);
          //  MyTimer.Tick += new EventHandler(MyTimer_Tick);
          //  MyTimer.Start();
      

          //  Timer MyTimerServer = new Timer();
          //  //  MyTimer.Interval = (60000*10); // 45 mins
          //  MyTimerServer.Interval = (60000*10);
          //  MyTimerServer.Tick += new EventHandler(MyTimerServer_Tick);
          //  MyTimerServer.Start();
          // // panel1.Hide();
          // // MDIForm mdi = new MDIForm();
          // //Panel p=new Panel();
          // //p.Controls.Add(mdi);


            CashReciept cr = new CashReciept();
            cr.MdiParent = this;
            cr.StartPosition = FormStartPosition.CenterScreen;
            cr.Show();
            //CashReciept cr = new CashReciept();
            //cr.Show();


           

        }
        DataTable dt1 = new DataTable();
        string Query;
        DateTime dt;
        private void MyTimer_Tick(object sender, EventArgs e)
       {

            string from = DateTime.Now.ToString();
                
            string[] from1 = from.Split(' ');

            Query = "select Count (*) as Pending from OrderBookingOffLine where Record_CreationDate between  '" + from1[0].ToString() + " 00:00:00" + "' and '" + from1[0].ToString() + " 23:59:59" + "' and TransferToServer='N'";
            dt1 = utils.GetDataTable(Query, utils.getCnnHSRPApp);

            lblShrinkingServer.Text = dt1.Rows[0]["Pending"].ToString();

            Query = "select count(*) as Entry from dbo.OrderBookingOffLine where  Record_CreationDate between '" + from1[0].ToString() + " 00:00:00" + "' and '" + from1[0].ToString() + " 23:59:59" + "' ";
            dt1 = utils.GetDataTable(Query, utils.getCnnHSRPApp);

            lblEntryCount.Text = dt1.Rows[0]["Entry"].ToString();

            Query = "select sum(Amount) as amount from dbo.OrderBookingOffLine   where  Record_CreationDate between '" + from1[0].ToString() + " 00:00:00" + "' and '" + from1[0].ToString() + " 23:59:59" + "' ";
            dt1 = utils.GetDataTable(Query, utils.getCnnHSRPApp);

            lblTodayCollection.Text = dt1.Rows[0]["amount"].ToString();
           // this.Close();
        }

        private void MyTimerServer_Tick(object sender, EventArgs e)
        {
           bool i= isConnected();
           if (i == true)
           {
               Transfer();
           }

        }
  
       
        StringBuilder strLocalUpdate = new StringBuilder();
        StringBuilder strServerInsert = new StringBuilder();

        public void Transfer()
        {
            string qry = string.Empty;
            int j;
            DataTable dt = new DataTable();
            string Query = "select top 10 * from OrderBookingOffLine   WHERE OrderStatus ='Embossing Done' and DATEDIFF(Day,Record_CreationDate,GETDATE()) >8 and fourthsmstext is null   order by RecordID desc";
            dt = utils.GetDataTable (Query,utils.getCnnHSRPApp);

            int count = dt.Rows.Count;
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < count; i++)
                {

                    if (isConnected())
                    {

                        DateTime affdate = Convert.ToDateTime(dt.Rows[i]["Record_CreationDate"].ToString()).AddDays(4);
                        string MobileNo = "91" +dt.Rows[i]["MobileNo"].ToString();
                        string smsmessage = "Reminder 1:  HSRP plate(s) for vehicle Reg. No. " + dt.Rows[i]["Vehicleregno"].ToString() + " are ready for affixation please contact HSRP affixation center Transport Dept HP (Link Utsav).";

                       
                        string url = "http://alerts.smseasy.in/api/web2sms.php?workingkey=627691h463b66gov5j83&sender=HPHSRP&to=" + MobileNo + "&message=" + smsmessage + "";
                        HttpWebRequest MyRequest = (HttpWebRequest)WebRequest.Create(url);
                        MyRequest.Method = "GET";
                        WebResponse myRespose = MyRequest.GetResponse();
                        StreamReader sr = new StreamReader(myRespose.GetResponseStream(), System.Text.Encoding.UTF8);
                        string result = sr.ReadToEnd();
                        sr.Close();
                        myRespose.Close();
                        //txtVehRegNo.Text = dt1.Rows[0]["vehicleregno"].ToString();

                       qry = "update OrderBookingOffLine set  fourthsmstext ='" + smsmessage + "',FourthSMSSentDateTime=getdate(),FourthSMSServerResponseID='" + result.ToString() + "'  where vehicleregno = '" + dt.Rows[i]["Vehicleregno"].ToString() + "' and orderstatus='Embossing Done'";
                       j = utils.ExecNonQuery(qry, utils.getCnnHSRPApp);
                        objWebServiceHP.updateFourthSms("3", dt.Rows[i]["Vehicleregno"].ToString(), smsmessage, result.ToString());
                       
                        qry = "update HSRP_DTLS_SMS set [ThirdsMSText]='" + smsmessage + "',[ThirdSMSSentDateTime]=getdate(),[ThirdSMSServerResponseID]='" + result.ToString() + "' where [MobileNo] ='" + MobileNo + "' and [AUTH_NO] ='" + dt.Rows[i]["HSRPRecord_AuthorizationNo"].ToString() + "'"; 
                        j = utils.ExecNonQuery(qry, utils.getCnnHSRPVahan);

                      

                    }
                }
            }

                
            
        }
        public void Transfer_FIFTH()
        {
            string qry = string.Empty;
            int j;
            DataTable dt = new DataTable();
            string Query = "select top 5 * from OrderBookingOffLine   WHERE OrderStatus ='Embossing Done' and DATEDIFF(Day,Record_CreationDate,GETDATE()) >12 and DATEDIFF(Day,fourthsmsdatetime,GETDATE()) > 4 and fifthsmstext is null   order by RecordID desc";
            dt = utils.GetDataTable(Query, utils.getCnnHSRPApp);

            int count = dt.Rows.Count;
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < count; i++)
                {

                    if (isConnected())
                    {

                        DateTime affdate = Convert.ToDateTime(dt.Rows[i]["Record_CreationDate"].ToString()).AddDays(4);
                        string MobileNo = "91" +dt.Rows[i]["MobileNo"].ToString();
                        string smsmessage = "Reminder 2:  HSRP plate(s) for vehicle Reg. No. " + dt.Rows[i]["Vehicleregno"].ToString() + " are ready for affixation please contact HSRP affixation center Transport Dept HP. (Link Utsav)";
                      
                        string url = "http://alerts.smseasy.in/api/web2sms.php?workingkey=627691h463b66gov5j83&sender=HPHSRP&to=" + MobileNo + "&message=" + smsmessage + "";
                        HttpWebRequest MyRequest = (HttpWebRequest)WebRequest.Create(url);
                        MyRequest.Method = "GET";
                        WebResponse myRespose = MyRequest.GetResponse();
                        StreamReader sr = new StreamReader(myRespose.GetResponseStream(), System.Text.Encoding.UTF8);
                        string result = sr.ReadToEnd();
                        sr.Close();
                        myRespose.Close();
                        
                        qry = "update OrderBookingOffLine set  FifthSMSText ='" + smsmessage + "',FifthSMSDateTime=getdate(),FifthSMSServerResponseID='" + result.ToString() + "'  where vehicleregno = '" + dt.Rows[i]["Vehicleregno"].ToString() + "' and orderstatus='Embossing Done'";
                        j = utils.ExecNonQuery(qry, utils.getCnnHSRPApp);
                        objWebServiceHP.updateFifthSms("3", dt.Rows[i]["Vehicleregno"].ToString(), smsmessage, result.ToString());
                     
                        
                        string qry1 = "update HSRP_DTLS_SMS set [FourthSMSText]='" + smsmessage + "',[FourthSMSSentDateTime]=getdate(),[FourthSMSServerResponseID]='" + result.ToString() + "' where [MobileNo] ='" + MobileNo + "' and [AUTH_NO]= '" + dt.Rows[i]["HSRPRecord_AuthorizationNo"].ToString() + "'"; 
                        j = utils.ExecNonQuery(qry1, utils.getCnnHSRPVahan);


                    }
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


        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MDIForm md = new MDIForm();
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            bool i = isConnected();
            if (i == true)
            {
               //Transfer();
               // Transfer_FIFTH ();
            }
            
        }

        private void searchToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            HSRPDataEntryNew.Search search = new HSRPDataEntryNew.Search();
            search.MdiParent = this;
            search.Show();
        }

        private void orderbooToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //MDIForm MDI = new MDIForm();
            ////MDI.MdiParent = this;
            
            //MDI.Show();
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
            OrderBooking ob = new OrderBooking();
            ob.MdiParent = this;
            ob.StartPosition = FormStartPosition.CenterScreen;
            ob.Show();
        }

        private void rejectionPlateToolStripMenuItem_Click(object sender, EventArgs e)
        {

            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
            RejectionPlate RP = new RejectionPlate();
            RP.MdiParent = this;
            RP.StartPosition = FormStartPosition.CenterScreen;
            RP.Show();
        }

        private void orderEmbossingToolStripMenuItem_Click(object sender, EventArgs e)
        {
             foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
            OrderEmbossing RP = new OrderEmbossing();
            RP.MdiParent = this;
            RP.StartPosition = FormStartPosition.CenterScreen;
            RP.Show();
        }

        private void orderAffixationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
            OrderClosed RP = new OrderClosed();
            RP.MdiParent = this;
            RP.StartPosition = FormStartPosition.CenterScreen;
            RP.Show();

        }

        private void searchToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
            Search RP = new Search();
            RP.MdiParent = this;
            RP.StartPosition = FormStartPosition.CenterScreen;
            RP.Show();

        }

        private void dailyReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
            DailyReport RP = new DailyReport();
            RP.MdiParent = this;
            RP.StartPosition = FormStartPosition.CenterScreen;
            RP.Show();
        }

        private void inventoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
            Inventory RP = new Inventory();
            RP.MdiParent = this;
            RP.StartPosition = FormStartPosition.CenterScreen;
            RP.Show();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void EmborssingDone_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            HtmlElementCollection hec = EmborssingDone.Document.GetElementsByTagName("input");
            foreach (HtmlElement curElement in hec)
            {
                string AttributeName = string.Empty;
                string AttributeValue = string.Empty;
                string Sql = string.Empty;

                AttributeName = curElement.Name.ToString();
                if (AttributeName == "txtPushData")
                {
                    AttributeValue = curElement.GetAttribute("value");
                    if (AttributeValue != "")
                    {
                        string[] words = AttributeValue.Split('^');
                        string strStatus = words[0].ToString();
                        string strVehicleRegNo = words[1].ToString();
                        string strAuthorisatioNo = words[2].ToString();
                        string strRecordid = words[3].ToString();
                        if (strStatus == "Saved")
                        {
                            Sql = "update OrderBookingOffLine set IsEmbossingSentToServer='Y' where RecordID='" + strRecordid + "'";
                            utils.ExecNonQuery(Sql, utils.getCnnHSRPApp);
                        }
                    }
                }
            }

        }

        private void ClosingDone_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            HtmlElementCollection hec = ClosingDone.Document.GetElementsByTagName("input");
            foreach (HtmlElement curElement in hec)
            {
                string AttributeName = string.Empty;
                string AttributeValue = string.Empty;
                string Sql = string.Empty;

                AttributeName = curElement.Name.ToString();
                if (AttributeName == "txtPushData")
                {
                    AttributeValue = curElement.GetAttribute("value");
                    if (AttributeValue != "")
                    {
                        string[] words = AttributeValue.Split('^');
                        string strStatus = words[0].ToString();
                        string strVehicleRegNo = words[1].ToString();
                        string strAuthorisatioNo = words[2].ToString();
                        string strRecordid = words[3].ToString();
                        if (strStatus == "Saved")
                        {
                            Sql = "update OrderBookingOffLine set IsClosedEntrySentToServer='Y' where RecordID='" + strRecordid + "'";
                            utils.ExecNonQuery(Sql, utils.getCnnHSRPApp);
                        }
                    }
                }

            }
        }

        private void CashCollection_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            HtmlElementCollection hec = CashCollection.Document.GetElementsByTagName("input");
            foreach (HtmlElement curElement in hec)
            {
                string AttributeName = string.Empty;
                string AttributeValue = string.Empty;
                string Sql = string.Empty;

                AttributeName = curElement.Name.ToString();
                if (AttributeName == "txtPushData")
                {
                    AttributeValue = curElement.GetAttribute("value");
                    if (AttributeValue != "")
                    {
                        string[] words = AttributeValue.Split('^');
                        string strStatus = words[0].ToString();
                        string strVehicleRegNo = words[1].ToString();
                        string strAuthorisatioNo = words[2].ToString();
                        string strRecordid = words[3].ToString();

                        if (strStatus == "Saved")
                        {
                            Sql = "update OrderBookingOffLine set IsCashReciptSentToServer='Y' where RecordID='" + strRecordid + "'";
                            utils.ExecNonQuery(Sql, utils.getCnnHSRPApp);
                        }
                    }
                }

            }
        }
        

        public void PushCashCollection()
        {
            SqlString = "SELECT Top 1* FROM OrderBookingOffLine where IsClosedEntrySentToServer='N' and IsEmbossingSentToServer='N' and IsCashReciptSentToServer='N'  order by RecordId desc";
            DataTable dt1 = utils.GetDataTable(SqlString, utils.getCnnHSRPApp);
            int count1 = dt1.Rows.Count;
            //  textBox1.Text = count1.ToString();
            if (count1 > 0)
            {

                String rec_id = dt1.Rows[0]["RecordId"].ToString();

                String rec_date = dt1.Rows[0]["Record_CreationDate"].ToString();

                String state_id = dt1.Rows[0]["HSRP_StateID"].ToString();

                String rto_id = dt1.Rows[0]["RTO_CD"].ToString();

                String rtocd = dt1.Rows[0]["RTO_CD"].ToString();

                String rec_auth_no = dt1.Rows[0]["HSRPRecord_AuthorizationNo"].ToString();

                String rec_auth_date = dt1.Rows[0]["HSRPRecord_AuthorizationDate"].ToString();

                String veh_rg_no = dt1.Rows[0]["VehicleRegNo"].ToString();

                String owner_name = dt1.Rows[0]["OwnerName"].ToString();

                String add = dt1.Rows[0]["address"].ToString();

                String mobile_no = dt1.Rows[0]["MobileNo"].ToString();

                String vehicle_class = dt1.Rows[0]["VehicleClass"].ToString();

                String order_type = dt1.Rows[0]["OrderType"].ToString();

                String sticker = dt1.Rows[0]["StickerMandatory"].ToString();

                String manf_name = dt1.Rows[0]["ManufacturerName"].ToString();

                String manf_model = dt1.Rows[0]["ManufacturerModel"].ToString();

                String vip = dt1.Rows[0]["VIP"].ToString();

                String amount = dt1.Rows[0]["Amount"].ToString();

                String vehicle_type = dt1.Rows[0]["VehicleType"].ToString();

                String isrec_sent = dt1.Rows[0]["isRecordSentToServer"].ToString();

                String cash_rec_no = dt1.Rows[0]["CashReceiptNo"].ToString();

                String tax = dt1.Rows[0]["Tax"].ToString();

                String chasis_no = dt1.Rows[0]["ChassisNo"].ToString();

                String engine_no = dt1.Rows[0]["EngineNo"].ToString();

                String front_laser = dt1.Rows[0]["hsrp_front_lasercode"].ToString();

                String rear_laser = dt1.Rows[0]["hsrp_rear_lasercode"].ToString();

                String order_status = dt1.Rows[0]["OrderStatus"].ToString();

                String embo_date = dt1.Rows[0]["OrderEmbossingDate"].ToString();

                String close_date = dt1.Rows[0]["OrderClosedDate"].ToString();

                String cashrec_sms = dt1.Rows[0]["CashReceiptSMSText"].ToString();

                String cash_sms_date = dt1.Rows[0]["CashReceiptSMSDateTime"].ToString();

                String cash_sms_responseid = dt1.Rows[0]["CashReceiptSMSServerResponseID"].ToString();

                String cashReceiptSmsResponseText = dt1.Rows[0]["CashReceiptSMSServerResponseText"].ToString();

              //  String strCashURL = "http://localhost:54549/WebSite2/getCashReceiptData.aspx?" + "&RecordId=" + rec_id + "&Record_CreationDate=" + rec_date + "&HSRP_StateID="
               
                String strCashURL = "http://180.151.100.246/GetDataHP/getCashReceiptData.aspx?" + "&RecordId=" + rec_id + "&Record_CreationDate=" + rec_date + "&HSRP_StateID="
                    + state_id + "&RTOLocationID=" + rto_id + "&RTO_CD=" + rtocd + "&HSRPRecord_AuthorizationNo=" + rec_auth_no +
                     "&HSRPRecord_AuthorizationDate=" + rec_auth_date + "&VehicleRegNo=" + veh_rg_no + "&OwnerName=" + owner_name + "&address=" +
                    add + "&MobileNo=" + mobile_no + "&VehicleClass=" + vehicle_class + "&OrderType=" + order_type + "&StickerMandatory=" + sticker
                    + "&ManufacturerName=" + manf_name + "&ManufacturerModel=" + manf_model + "&VIP=" + vip + "&Amount=" + amount + "&VehicleType="
                    + vehicle_type + "&isRecordSentToServer=" + isrec_sent + "&CashReceiptNo=" + cash_rec_no + "&Tax=" + tax + "&ChassisNo=" + chasis_no +
                     "&EngineNo=" + engine_no + "&hsrp_front_lasercode=" + front_laser + "&hsrp_rear_lasercode=" + rear_laser + "&OrderStatus=" +
                     order_status + "&OrderEmbossingDate=" + embo_date + "&OrderClosedDate=" + close_date + "&CashReceiptSMSText="
                    + cashrec_sms + "&CashReceiptSMSDateTime=" + cash_sms_date + "&CashReceiptSMSServerResponseID=" + cash_sms_responseid
                    + "&CashReceiptSMSServerResponseText=" + cashReceiptSmsResponseText;

                CashCollection.Navigate(strCashURL);
            }
        }

        
        public void PushEmborss()
        {
            SqlString = "SELECT Top 1* FROM OrderBookingOffLine where hsrp_front_lasercode is not null and hsrp_rear_lasercode is not null and OrderEmbossingDate is not null and IsClosedEntrySentToServer ='N' and IsEmbossingSentToServer='N' and IsCashReciptSentToServer='Y'  order by RecordId desc";
            DataTable dt1 = utils.GetDataTable(SqlString, utils.getCnnHSRPApp);
            int count1 = dt1.Rows.Count;
            //  textBox1.Text = count1.ToString();

            if (count1 > 0)
            {
                String rec_id = dt1.Rows[0]["RecordId"].ToString();

                String state_id = dt1.Rows[0]["HSRP_StateID"].ToString();

                String rto_id = dt1.Rows[0]["RTO_CD"].ToString();

                String rec_auth_no = dt1.Rows[0]["HSRPRecord_AuthorizationNo"].ToString();

                String veh_rg_no = dt1.Rows[0]["VehicleRegNo"].ToString();

                String order_type = dt1.Rows[0]["OrderType"].ToString();

                String chasis_no = dt1.Rows[0]["ChassisNo"].ToString();

                String engine_no = dt1.Rows[0]["EngineNo"].ToString();

                String front_laser = dt1.Rows[0]["hsrp_front_lasercode"].ToString();

                String rear_laser = dt1.Rows[0]["hsrp_rear_lasercode"].ToString();

                String embo_date = dt1.Rows[0]["OrderEmbossingDate"].ToString();

            //   String strCashURL = "http://localhost:54549/WebSite2/getEmbossingData.aspx?" + "&RecordId=" + rec_id + "&HSRP_StateID="

               String strCashURL = "http://180.151.100.246/GetDataHP/getEmbossingData.aspx?" + "&RecordId=" + rec_id + "&HSRP_StateID="
                    + state_id + "&RTOLocationID=" + rto_id + "&VehicleRegNo=" + veh_rg_no + "&OrderType=" + order_type + "&ChassisNo=" + chasis_no +
                     "&EngineNo=" + engine_no + "&HSRPRecord_AuthorizationNo=" + rec_auth_no + "&hsrp_front_lasercode=" + front_laser + "&hsrp_rear_lasercode=" + rear_laser + "&OrderEmbossingDate=" + embo_date;

                EmborssingDone.Navigate(strCashURL);
            }
        }

       
        public void PushClosedEntry()
        {
            SqlString = "SELECT Top 1* FROM OrderBookingOffLine where OrderClosedDate is not null and IsClosedEntrySentToServer='N' and IsEmbossingSentToServer='Y' and IsCashReciptSentToServer='Y' order by RecordId desc";
            DataTable dt1 = utils.GetDataTable(SqlString, utils.getCnnHSRPApp);
            int count1 = dt1.Rows.Count;
            //  textBox1.Text = count1.ToString();

            if (count1 > 0)
            {
                String rec_id = dt1.Rows[0]["RecordId"].ToString();

                String state_id = dt1.Rows[0]["HSRP_StateID"].ToString();

                String rto_id = dt1.Rows[0]["RTO_CD"].ToString();

                String veh_rg_no = dt1.Rows[0]["VehicleRegNo"].ToString();

                String close_date = dt1.Rows[0]["OrderClosedDate"].ToString();

                String rec_auth_no = dt1.Rows[0]["HSRPRecord_AuthorizationNo"].ToString();

               // String strCashURL = "http://localhost:54549/WebSite2/getAffixationData.aspx?" + "&RecordId=" + rec_id + "&HSRP_StateID="

                String strCashURL = "http://180.151.100.246/GetDataHP/getAffixationData.aspx?" + "&RecordId=" + rec_id + "&HSRP_StateID="
                    + state_id + "&RTOLocationID=" + rto_id + "&VehicleRegNo=" + veh_rg_no + "&OrderClosedDate=" + close_date + "&HSRPRecord_AuthorizationNo=" + rec_auth_no;
                ClosingDone.Navigate(strCashURL);
            }
        }

        private void Emborss_Done_Tick_1(object sender, EventArgs e)
        {
            PushEmborss();
        }

        private void Cashcollection_Done_Tick_1(object sender, EventArgs e)
        {
            PushCashCollection();
        }

        private void Closed_Done_Tick_1(object sender, EventArgs e)
        {
            PushClosedEntry();
        }

        private void dataSyncToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
            SendDataToServer obj = new SendDataToServer();
            obj.MdiParent = this;
            obj.StartPosition = FormStartPosition.CenterScreen;
            obj.Show();
           
        }

        #region Push and Pull Service Code

        DataSet dsData = null;
        DataSet dsDataNew = null;

        private void PushTimer_Tick(object sender, EventArgs e)
        {
            if (isConnected())
            {              
               // PushToServer();
            }            
        }
        //private void PushToServer()
        //{
        //    string sql = "select top(1500) *,'3' as stateIDAPP from HSRP_HP.dbo.HSRP_DTLS a where a.AUTH_NO not in (select Distinct [HSRPRecord_AuthorizationNo] from [HSRP_APP_HP].[dbo].[OrderBookingOffline] where  isRecordSentToServer = 'Y') ";
        //    DataTable dtLocalNotProcessed = utils.GetDataTable(sql, StrConnLocal);
        //    if (dtLocalNotProcessed.Rows.Count > 0)
        //    {
        //        dsData = new DataSet();
        //        dsDataNew = new DataSet();
        //        dsData.Tables.Add(dtLocalNotProcessed);
        //        HPWebService.HSRPService aa = new HPWebService.HSRPService();
        //        dsDataNew = aa.PushUpdateWithDataSet(dsData, "9d$s3T%W@D@2dsdfkj234$f");
        //        foreach (DataRow dtrows in dsDataNew.Tables[0].Rows) // Loop over the rows.
        //        {
        //            String Query = "select Count(*) as Total from [OrderBookingOffline] where [HSRPRecord_AuthorizationNo]='" + dtrows["AUTH_NO"].ToString() + "'";
        //            DataTable dtcheck = utils.GetDataTable(Query, StrConnLocalApp);
        //            if (int.Parse(dtcheck.Rows[0]["total"].ToString()) == 0)
        //            {
        //                //Query = "INSERT INTO [OrderBookingOffline] (Record_CreationDate,HSRP_StateID,RTO_CD,HSRPRecord_AuthorizationNo,HSRPRecord_AuthorizationDate,VehicleRegNo,OwnerName,address,VehicleClass,VehicleType,ChassisNo,EngineNo,OrderStatus,OrderPushDatetime,isRecordSentToServer)" +
        //                //        "values" +
        //                //        "(getdate(),'3','" + dtrows["RTO_CD"].ToString() + "','" + dtrows["AUTH_NO"].ToString() + "','" + dtrows["REGN_DT"].ToString() + "','" + dtrows["REGN_NO"].ToString() + "','" + dtrows["O_NAME"].ToString() + "','" + dtrows["O_ADDRESS"].ToString() + "','" + dtrows["VH_CLASS"].ToString() + "','" + dtrows["VEH_TYPE"].ToString() + "','" + dtrows["CHASI_NO"].ToString() + "','" + dtrows["ENG_NO"].ToString() + "' ,'New Order',getdate(),'" + dtrows["StatusSS"].ToString() + "' )";
        //                //utils.ExecNonQuery(Query, StrConnLocalApp);
        //            }
        //        }
        //    }
        //}

        private void PullTimer_Tick(object sender, EventArgs e)
        {
            if (isConnected())
            {
                //PullFromServer();
            }
        }
        //private void PullFromServer()
        //{
        //    string sql = "select top(500) VehicleRegNo as Regn_No,HSRPRecord_AuthorizationNo as Auth_No,HSRP_stateID as 'STATE_CD',RTO_CD from [HSRP_APP_HP].[dbo].[OrderBookingOffline] where OrderStatus Not In ('CashCollected','Closed') and RecordLastUpdated is null order by [OrderPushDateTime] desc";
        //    DataTable dtLocalNotProcessed = utils.GetDataTable(sql, StrConnLocal);
        //    if (dtLocalNotProcessed.Rows.Count > 0)
        //    {
        //        dsData = new DataSet();
        //        dsDataNew = new DataSet();
        //        dsData.Tables.Add(dtLocalNotProcessed);
        //        HPWebService.HSRPService aa = new HPWebService.HSRPService();
        //        dsDataNew = aa.PullUpdateWithDataSet(dsData, "9d$s3T%W@D@2dsdfkj234$f");
        //        foreach (DataRow dtrows in dsDataNew.Tables[0].Rows) // Loop over the rows.
        //        {
        //            #region When No Record Found
        //            if (dtrows["status"].ToString().ToUpper() == "N")
        //            {
        //                utils.ExecNonQuery("update [HSRP_APP_HP].[dbo].[OrderBookingOffline] set RecordLastUpdated=getdate() where [HSRPRecord_AuthorizationNo]='" + dtrows["Auth"].ToString() + "'", StrConnLocalApp);
        //            }
        //            #endregion
        //            else
        //            {
        //                #region Duplicate Laser No Check
        //                DataTable dtcheckduplicate = utils.GetDataTable("Select * from HSRP_DTLS where [HSRP_NO_FRONT] in ('" + dtrows["Front_LaserCode"].ToString() + "','" + dtrows["Rear_LaserCode"].ToString() + "') or [HSRP_NO_BACK] in ('" + dtrows["Front_LaserCode"].ToString() + "','" + dtrows["Rear_LaserCode"].ToString() + "')", StrConnLocal);
        //                if (dtcheckduplicate.Rows.Count > 0)
        //                {
        //                    utils.ExecNonQuery("update [HSRP_APP_HP].[dbo].[OrderBookingOffline] set RecordLastUpdated=getdate() where [HSRPRecord_AuthorizationNo]='" + dtrows["Auth_No"].ToString() + "'", StrConnLocalApp);
        //                }
        //                #endregion

        //                #region For Closed,Embossed,New Order
        //                else
        //                {
        //                    if (dtrows["Orderstatus"].ToString() == "Closed")
        //                    {
        //                        utils.ExecNonQuery("Update [HSRP_HP].[dbo].[HSRP_DTLS] set [HSRP_NO_FRONT]='" + dtrows["Front_LaserCode"].ToString() + "',[HSRP_NO_BACK]='" + dtrows["Rear_LaserCode"].ToString() + "',[HSRP_FIX_AMT]='" + dtrows["NetAmount"].ToString() + "',[HSRP_AMT_TAKEN_ON]='" + dtrows["OrderDate"].ToString() + "',[HSRP_ISSUE_DT]='" + dtrows["EmbossingDate"].ToString() + "',[HSRP_FIX_DT]='" + dtrows["FixDate"].ToString() + "' where Auth_No='" + dtrows["Auth_No"].ToString() + "'", StrConnLocal);
        //                        utils.ExecNonQuery("Update [HSRP_HP].[dbo].[HSRP_DTLS_SMS] set [SecondReceiptSmsText]='" + dtrows["SmsText"].ToString() + "',[SecondReceiptSMSDateTime]='" + dtrows["SmsDateTime"].ToString() + "',[SecondReceiptSMSServerResponseID]='" + dtrows["ResponseID"].ToString() + "',[SecondReceiptSMSServerResponseText]='" + dtrows["ResponseText"].ToString() + "' where Auth_No='" + dtrows["Auth_No"].ToString() + "'", StrConnLocal);
        //                        utils.ExecNonQuery("update [HSRP_APP_HP].[dbo].[OrderBookingOffline] set orderstatus ='Closed',RecordLastUpdated=getdate(),[OrderClosedDate]=getdate() where [HSRPRecord_AuthorizationNo]='" + dtrows["Auth_No"].ToString() + "'", StrConnLocalApp);

        //                    }
        //                    else if (dtrows["Orderstatus"].ToString() == "Embossing Done")
        //                    {
        //                        utils.ExecNonQuery("Update [HSRP_HP].[dbo].[HSRP_DTLS] set [HSRP_NO_FRONT]='" + dtrows["Front_LaserCode"].ToString() + "',[HSRP_NO_BACK]='" + dtrows["Rear_LaserCode"].ToString() + "',[HSRP_FIX_AMT]='" + dtrows["NetAmount"].ToString() + "',[HSRP_AMT_TAKEN_ON]='" + dtrows["OrderDate"].ToString() + "',[HSRP_ISSUE_DT]='" + dtrows["EmbossingDate"].ToString() + "' where Auth_No='" + dtrows["Auth_No"].ToString() + "'", StrConnLocal);
        //                        utils.ExecNonQuery("Update [HSRP_HP].[dbo].[HSRP_DTLS_SMS] set [FirstReceiptSmsText]='" + dtrows["SmsText"].ToString() + "',[FirstReceiptSMSDateTime]='" + dtrows["SmsDateTime"].ToString() + "',[FirstReceiptSMSServerResponseID]='" + dtrows["ResponseID"].ToString() + "',[FirstReceiptSMSServerResponseText]='" + dtrows["ResponseText"].ToString() + "' where Auth_No='" + dtrows["Auth_No"].ToString() + "'", StrConnLocal);
        //                        utils.ExecNonQuery("update [HSRP_APP_HP].[dbo].[OrderBookingOffline] set orderstatus ='Embossing Done',RecordLastUpdated=getdate(),OrderEmbossingDate=getdate() where [HSRPRecord_AuthorizationNo]='" + dtrows["Auth_No"].ToString() + "'", StrConnLocalApp);
        //                    }
        //                    else if (dtrows["Orderstatus"].ToString() == "New Order")
        //                    {
        //                        utils.ExecNonQuery("Update [HSRP_HP].[dbo].[HSRP_DTLS] set [HSRP_FIX_AMT]='" + dtrows["NetAmount"].ToString() + "',[HSRP_AMT_TAKEN_ON]='" + dtrows["OrderDate"].ToString() + "' where Auth_No='" + dtrows["Auth_No"].ToString() + "'", StrConnLocal);
        //                        utils.ExecNonQuery("Update [HSRP_HP].[dbo].[HSRP_DTLS_SMS] set [CashReceiptSmsText]='" + dtrows["SmsText"].ToString() + "',[CashReceiptSMSDateTime]='" + dtrows["SmsDateTime"].ToString() + "',[CashReceiptSMSServerResponseID]='" + dtrows["ResponseID"].ToString() + "',[CashReceiptSMSServerResponseText]='" + dtrows["ResponseText"].ToString() + "' where Auth_No='" + dtrows["Auth_No"].ToString() + "'", StrConnLocal);
        //                        utils.ExecNonQuery("update [HSRP_APP_HP].[dbo].[OrderBookingOffline] set orderstatus ='CashCollected',RecordLastUpdated=getdate() where [HSRPRecord_AuthorizationNo]='" + dtrows["Auth_No"].ToString() + "'", StrConnLocalApp);
        //                    }
        //                }
        //                #endregion
        //            }
        //        }
        //    }
        //    else
        //    {
        //        utils.ExecNonQuery("update OrderBookingOffline set recordlastupdate=null where OrderStatus Not In ('CashCollected','Closed')", StrConnLocalApp);
        //    }
        //}

        #endregion

        private void orderEmbossingToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            OrderEmbossing objEmb = new OrderEmbossing();
            objEmb.MdiParent = this;
            objEmb.StartPosition = FormStartPosition.CenterScreen;
            objEmb.Show();
        }

        private void orderClosedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OrderClosed objClose = new OrderClosed();
            objClose.MdiParent = this;
            objClose.StartPosition = FormStartPosition.CenterScreen;
            objClose.Show();
        }

        private void inventoryToolStripMenuItem_Click_1(object sender, EventArgs e)
        {

            //Inventory objInventory = new Inventory();
            //objInventory.MdiParent = this;
            //objInventory.StartPosition = FormStartPosition.CenterScreen;
            //objInventory.Show();
        }

        private void dailyReportToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
           
        }

        private void dailyReportToolStripMenuItem_Click_2(object sender, EventArgs e)
        {
            
        }

        private void dailyReportToolStripMenuItem_Click_3(object sender, EventArgs e)
        {
            DailyReport objDailyReport = new DailyReport();
            objDailyReport.MdiParent = this;
            objDailyReport.StartPosition = FormStartPosition.CenterScreen;
            objDailyReport.Show();
        }

        private void backUpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Backupdata objBackup = new Backupdata();
            objBackup.MdiParent = this;
            objBackup.StartPosition = FormStartPosition.CenterScreen;
            objBackup.Show();
        }

      

    }
}
