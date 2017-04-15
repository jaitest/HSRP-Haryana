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
        String StrUserid = string.Empty;
        private int childFormNumber = 0;        

        public MDIForm(string strUID)
        {
            StrUserid=strUID;
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
        }

        private void MDIForm_Load(object sender, EventArgs e)
        {
            CashReceiptNB cr = new CashReceiptNB(StrUserid);
            cr.MdiParent = this;
            cr.StartPosition = FormStartPosition.CenterScreen;
            cr.Show();          
        }

        DataTable dt1 = new DataTable();
        string Query;
        DateTime dt;

        private void MyTimer_Tick(object sender, EventArgs e)
        {
            string from = DateTime.Now.ToString();
            string[] from1 = from.Split(' ');

        }

       

        StringBuilder strLocalUpdate = new StringBuilder();
        StringBuilder strServerInsert = new StringBuilder();   

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
            MDIForm md = new MDIForm("0");
            this.Close();
        }       

        private void cashToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }

            CashReceiptNB crNB = new CashReceiptNB(StrUserid);
            crNB.MdiParent = this;
            crNB.StartPosition = FormStartPosition.CenterScreen;
            crNB.Show();
        }

        #region Push and Pull Service Code
        
        DataSet dsData = null;
        DataSet dsDataNew = null;

        private void cashReceiptOBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }

            CashReceiptOB crOB = new CashReceiptOB(StrUserid);
            crOB.MdiParent = this;
            crOB.StartPosition = FormStartPosition.CenterScreen;
            crOB.Show();
        }

        //private void PushTimer_Tick(object sender, EventArgs e)
        //{
        //    if (isConnected())
        //    {                
        //        PushToServer();
        //    }            
        //}
        //private void PushToServer()
        //{
        //    string sql = "select top 1000 from Hsrp_dtls.dbo.hsrp_dtls where auth_no in (select AUTH_NO from [HSRP_HR_APP].[dbo].[OrderStatusTable] where (isRecordSentToServer not in ('Y') or  isRecordSentToServer is null ))";
        //    DataTable dtLocalNotProcessed = utils.GetDataTable(sql, StrConnLocal);
        //    if (dtLocalNotProcessed.Rows.Count > 0)
        //    {
        //        dsData = new DataSet();
        //        dsDataNew = new DataSet();
        //        dsData.Tables.Add(dtLocalNotProcessed);
        //        HRWebService.HSRPService ObjHRWebService = new HRWebService.HSRPService();
        //        dsDataNew = ObjHRWebService.PushData(dsData, "9d$s3T%W@D@2dsdfkj234$f");
        //        foreach (DataRow dtrows in dsDataNew.Tables[0].Rows) // Loop over the rows.
        //        {
        //            //String Query = "select Count(*) as Total from [OrderStatusTable] where [Auth_no]='" + dtrows["AUTH_NO"].ToString() + "'";
        //            //DataTable dtcheck = utils.GetDataTable(Query, StrConnLocalApp);
        //            //if (int.Parse(dtcheck.Rows[0]["total"].ToString()) > 0)
        //            //{                        
        //            //    Query = "Update [OrderStatusTable] set [OrderPushDateTime]=getdate(),[isRecordSentToServer]='" + dtrows["Status"].ToString() + "' where [Auth_no] ='" + dtrows["AUTH_NO"].ToString() + "'";
        //            //    utils.ExecNonQuery(Query, StrConnLocalApp);
        //            //}
        //        }
        //    }
        //}

        //private void PullTimer_Tick(object sender, EventArgs e)
        //{
        //    if (isConnected())
        //    {               
        //        PullFromServer();

        //        //utils.getScalarCount("select count(*) from HSRP_DTLS", StrConnLocal).ToString();
        //        //utils.getScalarCount("select  count(*) from hsrp_dtls.dbo.HSRP_DTLS a where a.AUTH_NO not in (select Distinct AUTH_NO from [HSRP_HR_APP].[dbo].[OrderStatusTable] where  isRecordSentToServer != 'N') ", StrConnLocal).ToString();

        //        //string SqlQuery1 = "SELECT top (1) [OrderPushDateTime] FROM [HSRP_HR_APP].[dbo].[OrderStatusTable] order by OrderPushDateTime desc";
        //        //DataTable dt = utils.GetDataTable(SqlQuery1, StrConnLocalApp);
        //        //if (dt.Rows.Count > 0)
        //        //{
        //        //    dt.Rows[0]["OrderPushDateTime"].ToString();
        //        //}
        //    }           
        //}
        //private void PullFromServer()
        //{
        //    string sql = "select top(1000) * from OrderStatusTable where OrderStatus Not In ('CashCollected','Closed') and RecordLastUpdated is null order by [OrderPushDateTime] desc";
        //    DataTable dtLocalNotProcessed = utils.GetDataTable(sql, StrConnLocalApp);
        //    if (dtLocalNotProcessed.Rows.Count > 0)
        //    {
        //        dsData = new DataSet();
        //        dsDataNew = new DataSet();
        //        dsData.Tables.Add(dtLocalNotProcessed);
        //        HRWebService.HSRPService ObjWebService = new HRWebService.HSRPService();
        //        dsDataNew = ObjWebService.PullData(dsData, "9d$s3T%W@D@2dsdfkj234$f");
        //        foreach (DataRow dtrows in dsDataNew.Tables[0].Rows) // Loop over the rows.
        //        {
        //            #region When No Record Found
        //            if (dtrows["status"].ToString().ToUpper() == "N")
        //            {
        //                utils.ExecNonQuery("update [HSRP_HR_APP].[dbo].[OrderStatusTable] set RecordLastUpdated=getdate() where Auth_No='" + dtrows["AUTH_NO"].ToString() + "'", StrConnLocalApp);
        //                return;
        //            }
        //            #endregion
        //            else
        //            {
        //                #region Duplicate Laser No Check
        //                DataTable dtcheckduplicate = utils.GetDataTable("Select * from HSRP_DTLS where [HSRP_NO_FRONT] is null or [HSRP_NO_BACK] is null", StrConnLocal);
        //                // DataTable dtcheckduplicate = utils.GetDataTable("Select * from HSRP_DTLS where [HSRP_NO_FRONT] in ('" + dtrows["Front_LaserCode"].ToString() + "','" + dtrows["Rear_LaserCode"].ToString() + "') or [HSRP_NO_BACK] in ('" + dtrows["Front_LaserCode"].ToString() + "','" + dtrows["Rear_LaserCode"].ToString() + "')", StrConnLocal);
        //                if (dtcheckduplicate.Rows.Count > 0)
        //                {
        //                    HRWebService.HSRPService objexce = new HRWebService.HSRPService();
        //                    objexce.ExceptionCase(dtrows["AUTH_NO"].ToString(), dtrows["Front_LaserCode"].ToString(), dtrows["Rear_LaserCode"].ToString(), "4", "9d$s3T%W@D@2dsdfkj234$f");
        //                }
        //                #endregion

        //                #region For Closed,Embossed,New Order
        //                if (dtrows["Orderstatus"].ToString() == "Closed")
        //                {
        //                    utils.ExecNonQuery("Update [HSRP_DTLS].dbo.HSRP_DTLS set [HSRP_NO_FRONT]='" + dtrows["Front_LaserCode"].ToString() + "',[HSRP_NO_BACK]='" + dtrows["Rear_LaserCode"].ToString() + "',[HSRP_FIX_AMT]='" + dtrows["NetAmount"].ToString() + "',[HSRP_AMT_TAKEN_ON]='" + dtrows["OrderDate"].ToString() + "',[HSRP_ISSUE_DT]='" + dtrows["EmbossingDate"].ToString() + "',[HSRP_FIX_DT]='" + dtrows["OrderClosedDate"].ToString() + "' where Auth_No='" + dtrows["Auth_No"].ToString() + "'", StrConnLocal);
        //                    utils.ExecNonQuery("update [HSRP_HR_APP].[dbo].[OrderStatusTable] set orderstatus ='Closed',RecordLastUpdated=getdate(),[ClosedDataReceiveDateTime]=getdate() where Auth_No='" + dtrows["Auth_No"].ToString() + "'", StrConnLocalApp);

        //                }
        //                else if (dtrows["Orderstatus"].ToString() == "Embossing Done")
        //                {
        //                    utils.ExecNonQuery("Update [HSRP_DTLS].dbo.HSRP_DTLS set [HSRP_NO_FRONT]='" + dtrows["Front_LaserCode"].ToString() + "',[HSRP_NO_BACK]='" + dtrows["Rear_LaserCode"].ToString() + "',[HSRP_FIX_AMT]='" + dtrows["NetAmount"].ToString() + "',[HSRP_AMT_TAKEN_ON]='" + dtrows["OrderDate"].ToString() + "',[HSRP_ISSUE_DT]='" + dtrows["EmbossingDate"].ToString() + "' where Auth_No='" + dtrows["Auth_No"].ToString() + "'", StrConnLocal);
        //                    utils.ExecNonQuery("update [HSRP_HR_APP].[dbo].[OrderStatusTable] set orderstatus ='Embossing Done',RecordLastUpdated=getdate(),EmbossingDataRecieveDateTime=getdate() where Auth_No='" + dtrows["Auth_No"].ToString() + "'", StrConnLocalApp);
        //                }
        //                else if (dtrows["Orderstatus"].ToString() == "New Order")
        //                {
        //                    utils.ExecNonQuery("Update [HSRP_DTLS].dbo.HSRP_DTLS set [HSRP_FIX_AMT]='" + dtrows["NetAmount"].ToString() + "',[HSRP_AMT_TAKEN_ON]='" + dtrows["OrderDate"].ToString() + "' where Auth_No='" + dtrows["Auth_No"].ToString() + "'", StrConnLocal);
        //                    utils.ExecNonQuery("update [HSRP_HR_APP].[dbo].[OrderStatusTable] set orderstatus ='CashCollected',RecordLastUpdated=getdate() where Auth_No='" + dtrows["Auth_No"].ToString() + "'", StrConnLocalApp);
        //                }
        //                #endregion
        //            }
        //        }
        //    }
        //    else
        //    {
        //        utils.ExecNonQuery("update OrderStatusTable set recordlastupdate=null where OrderStatus Not In ('CashCollected','Closed')", StrConnLocalApp);
        //    }
        //}

        #endregion
    }
}
