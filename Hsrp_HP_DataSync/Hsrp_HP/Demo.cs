using System;
using System.Data;
using System.Windows.Forms;
using HSRPTransferData;
using System.ComponentModel;
using System.IO;
using System.Drawing.Printing;
using System.Drawing;
using System.Net;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Hsrp_HP.ServiceReference1;

namespace Hsrp_HP
{
 

    public partial class Demo : Form
    {


         static string ConnectionStringAPP = string.Empty;    


        public Demo()
        {
            InitializeComponent();
        }


        Hsrp_HP.ServiceReference1.HSRPServiceSoapClient obj = new HSRPServiceSoapClient();


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


      


        private void Demo_Load(object sender, EventArgs e)
        {
            btnUpdate.Visible = false;
            dataGridView1.Visible = false;
            lbltotalrec.Visible = false;
            lblRec.Visible = false;
            label2.Visible = false;
            label3.Visible = false;
            label6.Visible = false;
            label1.Visible = false;

            if (isConnected())
            {


                label4.Text = DateTime.Now.ToString("HH:mm:ss");
                btnUpdate.Visible = true;

                timer1.Start();


            }
            else
            {
                MessageBox.Show("Internet Is Not Connected");
            }          

        }



        DataTable dt = new DataTable();

        string frontlasercode = string.Empty;
        string RearLaserCoder = string.Empty;
        string OrderEmbossingDate = string.Empty;
        string OrderClosedDate = string.Empty;
        string query = string.Empty;



        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }    



        private void btnUpdate_Click_1(object sender, EventArgs e)
        {
            btnUpdate.Text = "Sync Data............";

            label2.Visible = false;
            label1.Visible = false;
            label3.Visible = false;


          // string DataSource = ".";

             string DataSource = @".\SQLEXPRESS";
           
              //string s = @"HSRP_HP";
             string s = @"HSRP_HP_Nahan";


            ConnectionStringAPP = "Data Source=" + DataSource + ";Initial Catalog=" + s + ";Integrated Security=True";


           // string sss = @"HSRP_APP_HP";
            string sss = @"HSRP_APP_HP_Nahan";

            string ConnectionStringAPP_HP = "Data Source=" + DataSource + ";Initial Catalog=" + sss + ";Integrated Security=True";



            //-----------------------------------------------------SMS---------------------------------

           // string query = "Select distinct  AUTH_NO  from HSRP_DTLS where    isnull([AUTH_NO],'')!='' and  isnull([HSRP_AMT_TAKEN_ON],'')!=''  ";

            string query = "Select distinct  dt.AUTH_NO  from HSRP_DTLS   dt  join HSRP_DTLS_SMS  sms   on  sms.AUTH_NO = dt.AUTH_NO  where    isnull(dt.AUTH_NO,'')!='' and  isnull(dt.HSRP_AMT_TAKEN_ON,'')!='' and sms.FirstSMSText is null  and   sms.FirstSMSServerResponseID  is null  and sms.FirstSMSServerResponseText is null ";

            DataTable dtt = utils.GetDataTableVahan(query.ToString(), ConnectionStringAPP);

            if (dtt.Rows.Count > 0)
            { 

            DataSet ds = new DataSet();

            ds.Tables.Add(dtt);

  

            DataTable dtl = obj.GetSMSRecords(ds.Tables[0]);
            if (dtl.Rows.Count > 0)
            {
                string query3 = string.Empty;
                string query4 = string.Empty;

                for (int i = 0; i < dtl.Rows.Count; i++)
                {
                    query3 = " update  HSRP_DTLS_SMS set FirstSMSText= '" + Convert.ToString(dtl.Rows[i]["FirstSMSText"]) + "',FirstSMStDateTime='" + Convert.ToString(dtl.Rows[i]["FirstSMSSentDateTime"]) + "',FirstSMSServerResponseID='" + Convert.ToString(dtl.Rows[i]["FirstSMSServerResponseID"]) + "',FirstSMSServerResponseText='" + Convert.ToString(dtl.Rows[i]["FirstSMSServerResponseText"]) + "' where  AUTH_NO ='" + Convert.ToString(dtl.Rows[i]["Auth_NO"]).ToString() + "' ";
                    utils.ExecNonQuery(query3.ToString(), ConnectionStringAPP);

                    query4 = " update   OrderBookingOffLine set FirstSMSText= '" + Convert.ToString(dtl.Rows[i]["FirstSMSText"]) + "',FirstSMSDateTime='" + Convert.ToString(dtl.Rows[i]["FirstSMSSentDateTime"]) + "',FirstSMSServerResponseID='" + Convert.ToString(dtl.Rows[i]["FirstSMSServerResponseID"]) + "',FirstSMSServerResponseText='" + Convert.ToString(dtl.Rows[i]["FirstSMSServerResponseText"]) + "' where  HSRPRecord_AuthorizationNo ='" + Convert.ToString(dtl.Rows[i]["Auth_NO"]).ToString() + "' ";
                    utils.ExecNonQuery(query4.ToString(), ConnectionStringAPP_HP);
                }

            }

            }

            //------------------------------------------------------End Sms ---------------------------------


            //isnull([HSRP_AMT_TAKEN_ON],'')!=''and

            #region GridBind
            string queryy = "Select  ROW_NUMBER() Over (Order by HSRP_AMT_TAKEN_ON) As SNo , REGN_NO, AUTH_NO, HSRP_FIX_AMT as Amount  , HSRP_AMT_TAKEN_ON  as AmountDate, HSRP_NO_FRONT as  HSRPFrontLaserCode   ,  HSRP_NO_BACK as HSRPRearLaserCode   from HSRP_DTLS where    isnull([AUTH_NO],'')!='' and  ( isnull(HSRP_ISSUE_DT,'')='' or HSRP_ISSUE_DT ='' or HSRP_FIX_DT  is null or   HSRP_FIX_DT = '' ) ";

            dt = utils.GetDataTableVahan(queryy.ToString(), ConnectionStringAPP);

            if (dt.Rows.Count > 0)
            {
                btnUpdate.Visible = true;
                dataGridView1.Visible = true;
                dataGridView1.DataSource = dt;
                dataGridView1.ReadOnly = true;
                lbltotalrec.Visible = true;
                label3.Visible = true;
                label3.Text = dt.Rows.Count.ToString();

            }
            else
            {

                dataGridView1.Visible = true;
                lbltotalrec.Visible = false;
                lblRec.Visible = false;
                label2.Visible = false;
                label3.Visible = false;
                label6.Visible = false;
                label1.Visible = false;
                MessageBox.Show("Reocrd Not Available on For Sync.");
                btnUpdate.Text = DateTime.Now.ToString("HH:mm:ss");
                return;
            }


            #endregion GridBind end code

           
            string query1 = "Select distinct  AUTH_NO  from HSRP_DTLS where    isnull([AUTH_NO],'')!=''  and ( isnull(HSRP_ISSUE_DT,'')='' or HSRP_ISSUE_DT ='' or HSRP_FIX_DT  is null or   HSRP_FIX_DT = '' ) ";

            DataTable dttt = utils.GetDataTableVahan(query1.ToString(), ConnectionStringAPP);

            DataSet ds1 = new DataSet();

            ds1.Tables.Add(dttt);
            

            DataTable dtbl = obj.GetRecords(ds1.Tables[0]);
                       
            if (dtbl.Rows.Count > 0)
            {
               
                int CountOfNullRecords = 0;
                int CountOfWithoutNullRecords = 0;

                        
                     
                for (int i = 0; i < dtbl.Rows.Count; i++)
                {
                    if (Convert.ToString(dtbl.Rows[i]["HSRPRecord_AuthorizationNo"]).Trim() != "")
                    {

                        query = "update  HSRP_DTLS  set    HSRP_FIX_AMT='" + Convert.ToString(dtbl.Rows[i]["RoundOff_NetAmount"]).Trim() + "' ,  HSRP_AMT_TAKEN_ON='" + Convert.ToString(dtbl.Rows[i]["hsrprecord_creationdate"]).Trim() + "'  ,   HSRP_NO_FRONT='" + Convert.ToString(dtbl.Rows[i]["hsrp_front_lasercode"]).Trim() + "' ,HSRP_NO_BACK ='" + Convert.ToString(dtbl.Rows[i]["HSRP_Rear_LaserCode"]).Trim() + "'  ,HSRP_ISSUE_DT = '" + Convert.ToString(dtbl.Rows[i]["OrderEmbossingDate"]).Trim() + "' , HSRP_FIX_DT='" + Convert.ToString(dtbl.Rows[i]["OrderClosedDate"]).Trim() + "' where   AUTH_NO = '" + Convert.ToString(dtbl.Rows[i]["HSRPRecord_AuthorizationNo"]) + "' ";
                        int k = utils.ExecNonQuery(query.ToString(), ConnectionStringAPP);


                        string query2 = " update  OrderBookingOffLine   set   Record_CreationDate ='" + Convert.ToString(dtbl.Rows[i]["hsrprecord_creationdate"]).Trim() + "'  , Amount='" + Convert.ToString(dtbl.Rows[i]["RoundOff_NetAmount"]).Trim() + "'   , hsrp_front_lasercode = '" + Convert.ToString(dtbl.Rows[i]["hsrp_front_lasercode"]).Trim() + "' ,hsrp_rear_lasercode= '" + Convert.ToString(dtbl.Rows[i]["HSRP_Rear_LaserCode"]).Trim() + "' , orderEmbossingdate= '" + Convert.ToString(dtbl.Rows[i]["OrderEmbossingDate"]).Trim() + "' , orderclosedDate = '" + Convert.ToString(dtbl.Rows[i]["OrderClosedDate"]).Trim() + "' where    hsrpRecord_authorizationNo = '" + Convert.ToString(dtbl.Rows[i]["HSRPRecord_AuthorizationNo"]) + "' ";

                        int l = utils.ExecNonQuery(query2, ConnectionStringAPP_HP);

                        CountOfWithoutNullRecords += 1;
                    }

                    else
                    {
                        CountOfNullRecords += 1;

                    }


                }


                lbltotalrec.Visible = true;
                lblRec.Visible = true;
                label2.Visible = true;
                label6.Visible = true;
                label1.Visible = true;
                label3.Visible = true;
                label2.Text = Convert.ToString(CountOfWithoutNullRecords);
                label1.Text = Convert.ToString(Convert.ToUInt32(label3.Text) - Convert.ToUInt32(CountOfWithoutNullRecords));
                btnUpdate.Text = "Update";
                label4.Text=   DateTime.Now.ToString("HH:mm:ss");
               
            }
            else
            {

                MessageBox.Show("Reocrd Not Available for Sync....");
                dataGridView1.Visible = true;
                lbltotalrec.Visible = false;
                lblRec.Visible = false;
                label2.Visible = false;
                label3.Visible = false;
                label6.Visible = false;
                label1.Visible = false;
                btnUpdate.Text = "Update";
                label4.Text=  DateTime.Now.ToString("HH:mm:ss");
                return;

            }
        }    


        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void lblRec_Click(object sender, EventArgs e)
        {


        }

        private void lbltotalrec_Click(object sender, EventArgs e)
        {

        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {          

            btnUpdate_Click_1(sender, e);        


        }

        //private void BtnSMS_Click(object sender, EventArgs e)
        //{
        //    BtnSMS.Text = "Sync Sms............";


        //    //string DataSource = ".";
        //    string DataSource = @".\SQLEXPRESS";
        //    string s = @"HSRP_HP";


        //    ConnectionStringAPP = "Data Source=" + DataSource + ";Initial Catalog=" + s + ";Integrated Security=True";


        //    string sss = @"HSRP_APP_HP";

        //    string ConnectionStringAPP_HP = "Data Source=" + DataSource + ";Initial Catalog=" + sss + ";Integrated Security=True";



        //    string query = "Select distinct  AUTH_NO  from HSRP_DTLS where    isnull([AUTH_NO],'')!='' and  isnull([HSRP_AMT_TAKEN_ON],'')!='' and isnull([HSRP_NO_FRONT],'')='' and isnull([HSRP_NO_BACK],'')=''  ";

        //    DataTable dtt = utils.GetDataTableVahan(query.ToString(), ConnectionStringAPP);

        //    DataSet ds = new DataSet();
        //    ds.Tables.Add(dtt);

        //    //DataTable dtl = obj.GetSMSRecords(ds.Tables[0]);
        //    //if(dtl.Rows.Count>0)
        //    //{
        //    //    string query3 = string.Empty;
        //    //    string query4 = string.Empty;

        //    //    for (int i = 0; i < dtl.Rows.Count; i++)
        //    //    {
        //    //        query3 = " update  HSRP_DTLS_SMS set FirstSMSText= '" + Convert.ToString(dtl.Rows[i]["FirstSMSText"]) + "',FirstSMSSentDateTime='" + Convert.ToString(dtl.Rows[i]["FirstSMSSentDateTime"]) + "',FirstSMSServerResponseID='" + Convert.ToString(dtl.Rows[i]["FirstSMSServerResponseID"]) + "',FirstSMSServerResponseText='" + Convert.ToString(dtl.Rows[i]["FirstSMSServerResponseText"]) + "' where  AUTH_NO ='" + Convert.ToString(dtl.Rows[i]["Auth_NO"]).ToString() + "';  ";
        //    //        utils.ExecNonQuery(query3, ConnectionStringAPP);

        //    //        query4 = " update   OrderBookingOffLine set FirstSMSText= '" + Convert.ToString(dtl.Rows[i]["FirstSMSText"]) + "',FirstSMSDateTime='" + Convert.ToString(dtl.Rows[i]["FirstSMSSentDateTime"]) + "',FirstSMSServerResponseID='" + Convert.ToString(dtl.Rows[i]["FirstSMSServerResponseID"]) + "',FirstSMSServerResponseText='" + Convert.ToString(dtl.Rows[i]["FirstSMSServerResponseText"]) + "' where  HSRPRecord_AuthorizationNo ='" + Convert.ToString(dtl.Rows[i]["Auth_NO"]).ToString() + "';  ";
        //    //        utils.ExecNonQuery(query4, ConnectionStringAPP_HP);  
        //    //    }





        //    //}



        //}






    }
}
