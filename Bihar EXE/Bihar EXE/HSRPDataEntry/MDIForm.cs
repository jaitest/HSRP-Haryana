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
using System.Net.NetworkInformation;

namespace HSRPDataEntry
{
    public partial class MDIForm : Form
    {
        string CnnString = String.Empty;
        string SqlString = String.Empty;
        private int childFormNumber = 0;

        public MDIForm()
        {
          
            InitializeComponent();
            //MacAddress();
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
          //  CashReciept cr = new CashReciept();
            //cr.MdiParent = this;
            //cr.StartPosition = FormStartPosition.CenterScreen;
            //cr.Show();
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


            CashReceiptNB cr = new CashReceiptNB();
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

            //Query = "select Count (*) as Pending from OrderBookingOffLine where Record_CreationDate between  '" + from1[0].ToString() + " 00:00:00" + "' and '" + from1[0].ToString() + " 23:59:59" + "' and TransferToServer='N'";
            //dt1 = utils.GetDataTable(Query, utils.getCnnHSRPApp);

            //lblShrinkingServer.Text = dt1.Rows[0]["Pending"].ToString();

            //Query = "select count(*) as Entry from dbo.OrderBookingOffLine where  Record_CreationDate between '" + from1[0].ToString() + " 00:00:00" + "' and '" + from1[0].ToString() + " 23:59:59" + "' ";
            //dt1 = utils.GetDataTable(Query, utils.getCnnHSRPApp);

            //lblEntryCount.Text = dt1.Rows[0]["Entry"].ToString();

            //Query = "select sum(Amount) as amount from dbo.OrderBookingOffLine   where  Record_CreationDate between '" + from1[0].ToString() + " 00:00:00" + "' and '" + from1[0].ToString() + " 23:59:59" + "' ";
            //dt1 = utils.GetDataTable(Query, utils.getCnnHSRPApp);

            //lblTodayCollection.Text = dt1.Rows[0]["amount"].ToString();
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
      //  string ConnectionString = System.Configuration.ConfigurationSettings.AppSettings["ConnectionString"].ToString();
      // string ServerConnectionString = System.Configuration.ConfigurationSettings.AppSettings["ConnectionString"].ToString();
      

        //DataTable dt = new DataTable();
        //DataTable dt1 = new DataTable();
       
        StringBuilder strLocalUpdate = new StringBuilder();
        StringBuilder strServerInsert = new StringBuilder();

        public void Transfer()
        {
            //string qry = string.Empty;
            //int j;
            //DataTable dt = new DataTable();
            //string Query = "select top 10 * from OrderBookingOffLine   WHERE OrderStatus ='Embossing Done' and DATEDIFF(Day,Record_CreationDate,GETDATE()) >8 and fourthsmstext is null   order by RecordID desc";
            //dt = utils.GetDataTable (Query,utils.getCnnHSRPApp);

            //int count = dt.Rows.Count;
            //if (dt.Rows.Count > 0)
            //{
            //    for (int i = 0; i < count; i++)
            //    {

            //        if (isConnected())
            //        {

            //            DateTime affdate = Convert.ToDateTime(dt.Rows[i]["Record_CreationDate"].ToString()).AddDays(4);
            //            string MobileNo = "91" +dt.Rows[i]["MobileNo"].ToString();
            //            string smsmessage = "Reminder 1:  HSRP plate(s) for vehicle Reg. No. " + dt.Rows[i]["Vehicleregno"].ToString() + " are ready for affixation please contact HSRP affixation center Transport Dept HP (Link Utsav).";
            //            //string smsmessage ="Affixation date for your HSRP plate(s) for vehicle Reg. No. " + txtVehRegNo.Text.Trim() + " is " + affdate.ToString("dd-MM-yyyy")  + ". at your HSRP affixation center. Team HSRP";
            //            string url = "http://alerts.smseasy.in/api/web2sms.php?workingkey=627691h463b66gov5j83&sender=HPHSRP&to=" + MobileNo + "&message=" + smsmessage + "";
            //            HttpWebRequest MyRequest = (HttpWebRequest)WebRequest.Create(url);
            //            MyRequest.Method = "GET";
            //            WebResponse myRespose = MyRequest.GetResponse();
            //            StreamReader sr = new StreamReader(myRespose.GetResponseStream(), System.Text.Encoding.UTF8);
            //            string result = sr.ReadToEnd();
            //            sr.Close();
            //            myRespose.Close();
            //            //txtVehRegNo.Text = dt1.Rows[0]["vehicleregno"].ToString();
            //            qry = "update OrderBookingOffLine set  fourthsmstext ='" + smsmessage + "',FourthSMSDateTime=getdate(),FourthSMSServerReponseID='" + result.ToString() + "'  where vehicleregno = '" + dt.Rows[i]["Vehicleregno"].ToString() + "' and orderstatus='Embossing Done'";
            //            j = utils.ExecNonQuery(qry, utils.getCnnHSRPApp);
            //          //  qry = "insert into HSRP_DTLS_SMS([MobileNo],[REGN_NO],[AUTH_NO],[HSRP_FLAG],[FourthSMSText],[FourthSMSDateTime],[FourthSMSServerReponseID]) values ('" + MobileNo + "','" + dt.Rows[i]["Vehicleregno"].ToString() + "','" + dt.Rows[i]["HSRPRecord_AuthorizationNo"].ToString() + "','" + dt.Rows[i]["ordertype"].ToString() + "','" + smsmessage + "',getdate(),'" + result.ToString() + "') ";
            //            //qry = "insert into HSRP_DTLS_SMS([MobileNo],[REGN_NO],[AUTH_NO],[HSRP_FLAG],[ThirdsMSText],[ThirstSMSSentDateTime],[ThirsdSMSServerResponseID]) values ('" + MobileNo + "','" + dt.Rows[i]["Vehicleregno"].ToString() + "','" + dt.Rows[i]["HSRPRecord_AuthorizationNo"].ToString() + "','" + dt.Rows[i]["ordertype"].ToString() + "','" + smsmessage + "',getdate(),'" + result.ToString() + "') ";
            //            qry = "update HSRP_DTLS_SMS set [ThirdsMSText]='" + smsmessage + "',[ThirstSMSSentDateTime]=getdate(),[ThirsdSMSServerResponseID]='" + result.ToString() + "' where [MobileNo] ='" + MobileNo + "' and [AUTH_NO] ='" + dt.Rows[i]["HSRPRecord_AuthorizationNo"].ToString() + "'"; 
            //            j = utils.ExecNonQuery(qry, utils.getCnnHSRPVahan);

            //          //  MessageBox.Show("Record Saved and SMS Sent To Client On : " + MobileNo);
            //            // Update offline order booking
            //            // Update HSRP_HP DB for SMS

            //        }
            //    }
            //}

                
            
        }
        public void Transfer_FIFTH()
        {
            //string qry = string.Empty;
            //int j;
            //DataTable dt = new DataTable();
            //string Query = "select top 5 * from OrderBookingOffLine   WHERE OrderStatus ='Embossing Done' and DATEDIFF(Day,Record_CreationDate,GETDATE()) >12 and DATEDIFF(Day,fourthsmsdatetime,GETDATE()) > 4 and fifthsmstext is null   order by RecordID desc";
            //dt = utils.GetDataTable(Query, utils.getCnnHSRPApp);

            //int count = dt.Rows.Count;
            //if (dt.Rows.Count > 0)
            //{
            //    for (int i = 0; i < count; i++)
            //    {

            //        if (isConnected())
            //        {

            //            DateTime affdate = Convert.ToDateTime(dt.Rows[i]["Record_CreationDate"].ToString()).AddDays(4);
            //            string MobileNo = "91" +dt.Rows[i]["MobileNo"].ToString();
            //            string smsmessage = "Reminder 2:  HSRP plate(s) for vehicle Reg. No. " + dt.Rows[i]["Vehicleregno"].ToString() + " are ready for affixation please contact HSRP affixation center Transport Dept HP. (Link Utsav)";
            //            //string smsmessage ="Affixation date for your HSRP plate(s) for vehicle Reg. No. " + txtVehRegNo.Text.Trim() + " is " + affdate.ToString("dd-MM-yyyy")  + ". at your HSRP affixation center. Team HSRP";
            //            string url = "http://alerts.smseasy.in/api/web2sms.php?workingkey=627691h463b66gov5j83&sender=HPHSRP&to=" + MobileNo + "&message=" + smsmessage + "";
            //            HttpWebRequest MyRequest = (HttpWebRequest)WebRequest.Create(url);
            //            MyRequest.Method = "GET";
            //            WebResponse myRespose = MyRequest.GetResponse();
            //            StreamReader sr = new StreamReader(myRespose.GetResponseStream(), System.Text.Encoding.UTF8);
            //            string result = sr.ReadToEnd();
            //            sr.Close();
            //            myRespose.Close();
            //            //txtVehRegNo.Text = dt1.Rows[0]["vehicleregno"].ToString();
            //            qry = "update OrderBookingOffLine set  FifthSMSText ='" + smsmessage + "',FifthSMSDateTime=getdate(),FifthSMSServerReponseID='" + result.ToString() + "'  where vehicleregno = '" + dt.Rows[i]["Vehicleregno"].ToString() + "' and orderstatus='Embossing Done'";
            //            j = utils.ExecNonQuery(qry, utils.getCnnHSRPApp);
            //          //  qry = "insert into HSRP_DTLS_SMS([MobileNo],[REGN_NO],[AUTH_NO],[HSRP_FLAG],[FifthSMSText],[FifthSMSDateTime],[FifthSMSServerReponseID]) values ('" + MobileNo + "','" + dt.Rows[i]["Vehicleregno"].ToString() + "','" + dt.Rows[i]["HSRPRecord_AuthorizationNo"].ToString() + "','" + dt.Rows[i]["ordertype"].ToString() + "','" + smsmessage + "',getdate(),'" + result.ToString() + "') ";
            //            //qry = "insert into HSRP_DTLS_SMS([MobileNo],[REGN_NO],[AUTH_NO],[HSRP_FLAG],[FourthSMSText],[FourthSMSDateTime],[FourthSMSServerReponseID]) values ('" + MobileNo + "','" + dt.Rows[i]["Vehicleregno"].ToString() + "','" + dt.Rows[i]["HSRPRecord_AuthorizationNo"].ToString() + "','" + dt.Rows[i]["ordertype"].ToString() + "','" + smsmessage + "',getdate(),'" + result.ToString() + "') ";
            //            string qry1 = "update HSRP_DTLS_SMS set [FourthSMSText]='" + smsmessage + "',[FourthSMSDateTime]=getdate(),[FourthSMSServerReponseID]='" + result.ToString() + "' where [MobileNo] ='" + MobileNo + "' and [AUTH_NO]= '" + dt.Rows[i]["HSRPRecord_AuthorizationNo"].ToString() + "'"; 
            //            j = utils.ExecNonQuery(qry, utils.getCnnHSRPVahan);

            //            //  MessageBox.Show("Record Saved and SMS Sent To Client On : " + MobileNo);
            //            // Update offline order booking
            //            // Update HSRP_HP DB for SMS

            //        }
            //    }
            //}



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
                Transfer();
                Transfer_FIFTH();
            }
            else
            {
                MessageBox.Show("Internet Is Not Connected");
            }
            
        }

        private void searchToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //HSRPDataEntry.Search search = new HSRPDataEntry.Search();
            //search.MdiParent = this;
            //search.Show();
        }

        private void orderbooToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //MDIForm MDI = new MDIForm();
            ////MDI.MdiParent = this;
            
            //MDI.Show();
            //foreach (Form childForm in MdiChildren)
            //{
            //    childForm.Close();
            //}
            //OrderBooking ob = new OrderBooking();
            //ob.MdiParent = this;
            //ob.StartPosition = FormStartPosition.CenterScreen;
            //ob.Show();
        }

        private void rejectionPlateToolStripMenuItem_Click(object sender, EventArgs e)
        {

            //foreach (Form childForm in MdiChildren)
            //{
            //    childForm.Close();
            //}
            //RejectionPlate RP = new RejectionPlate();
            //RP.MdiParent = this;
            //RP.StartPosition = FormStartPosition.CenterScreen;
            //RP.Show();
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
            //foreach (Form childForm in MdiChildren)
            //{
            //    childForm.Close();
            //}
            //Search RP = new Search();
            //RP.MdiParent = this;
            //RP.StartPosition = FormStartPosition.CenterScreen;
            //RP.Show();

        }

        private void dailyReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //foreach (Form childForm in MdiChildren)
            //{
            //    childForm.Close();
            //}
            //DailyReport RP = new DailyReport();
            //RP.MdiParent = this;
            //RP.StartPosition = FormStartPosition.CenterScreen;
            //RP.Show();
        }

        private void inventoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //foreach (Form childForm in MdiChildren)
            //{
            //    childForm.Close();
            //}
            //Inventory RP = new Inventory();
            //RP.MdiParent = this;
            //RP.StartPosition = FormStartPosition.CenterScreen;
            //RP.Show();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void cashToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }

            CashReceiptNB crNB = new CashReceiptNB();
            crNB.MdiParent = this;
            crNB.StartPosition = FormStartPosition.CenterScreen;
            crNB.Show();
        }
    }
}
