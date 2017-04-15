using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using HSRPTransferData;
using System.Net;
using System.IO;

namespace HSRPDataEntryNew
{
    public partial class Search : Form
    {
        public Search()
        {
            InitializeComponent();
        }

        string ConnectionString = utils.getCnnHSRPApp; 

        DataTable dt1=new DataTable();
        string Query;
        private void btnCollection_Click(object sender, EventArgs e)
        {
            string from = DateTime.Now.ToString();

            string[] from1 = from.Split(' ');

            Query = "select Count (*) as Pending from OrderBookingOffLine where Record_CreationDate between  '" + from1[0].ToString() + " 00:00:00" + "' and '" + from1[0].ToString() + " 23:59:59" + "' and TransferToServer='N'";
            dt1 = utils.GetDataTable(Query, ConnectionString);

            lblShrinkingServer.Text = dt1.Rows[0]["Pending"].ToString();

            Query = "select count(*) as Entry from dbo.OrderBookingOffLine where  Record_CreationDate between '" + from1[0].ToString() + " 00:00:00" + "' and '" + from1[0].ToString() + " 23:59:59" + "' ";
            dt1 = utils.GetDataTable(Query, ConnectionString);

            lblEntryCount.Text = dt1.Rows[0]["Entry"].ToString();

            Query = "select sum(Amount) as amount from dbo.OrderBookingOffLine   where  Record_CreationDate between '" + from1[0].ToString() + " 00:00:00" + "' and '" + from1[0].ToString() + " 23:59:59" + "' ";
            dt1 = utils.GetDataTable(Query, ConnectionString);

            lblTodayCollection.Text = dt1.Rows[0]["amount"].ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Query = "select * from dbo.OrderBookingOffLine where vehicleregno ='"+txtCashReceiptNo.Text.Replace(" ","") +"'";
            dt1 = utils.GetDataTable(Query, ConnectionString);
            dataGridView1.DataSource=dt1;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            Query = "select FirstSMSServerReponseID from dbo.OrderBookingOffLine where vehicleregno ='" + txtCashReceiptNo.Text.Replace(" ", "") + "'";
            string ResponseVal = utils.getScalarValue(Query, utils.getCnnHSRPApp);
            //Message GID=141304663 ID=141304663-1
               string [] ResponseValArr =ResponseVal.Split('=');
               string url = "http://alerts.smseasy.in/api/status.php?workingkey=627691h463b66gov5j83&messageid=" + ResponseValArr[2].ToString();

            //string url = utils.getOldDataURL.ToString() + "REGN_NO=" + txtRegNo.Text + "&CHASI_NO=" + txtChasisNo.Text + "&REQ_TYPE=" + orderType;
               HttpWebRequest MyRequest = (HttpWebRequest)WebRequest.Create(url);
               MyRequest.Method = "GET";
               WebResponse myRespose = MyRequest.GetResponse();
               StreamReader sr = new StreamReader(myRespose.GetResponseStream(), System.Text.Encoding.UTF8);
               string result = sr.ReadToEnd();
               sr.Close();
               myRespose.Close();

               string[] ResultArr = result.Split('\n');
               Query = "update OrderBookingOffLine set FirstSMSServerResponseText ='" + ResultArr[2].ToString() + "' where vehicleregno ='" + txtCashReceiptNo.Text + "' ";
               int j = utils.ExecNonQuery(Query, utils.getCnnHSRPApp);
               Query = "update HSRP_DTLS_SMS set FirstSMSServerResponseText ='" + ResultArr[2].ToString() + "' where REGN_NO ='"+ txtCashReceiptNo.Text  +"'";
                j = utils.ExecNonQuery(Query,utils.getCnnHSRPVahan);
                
            Query = "select * from dbo.OrderBookingOffLine where vehicleregno ='" + txtCashReceiptNo.Text.Replace(" ", "") + "'";
                dt1 = utils.GetDataTable(Query, ConnectionString);
                dataGridView1.DataSource = dt1;

        }
    }
}
