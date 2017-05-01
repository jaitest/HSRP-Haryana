using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using HSRPDataEntryNew;
using System.IO;
using HSRPTransferData;



namespace HSRPDataEntryNew
{
    public partial class SendDataToServer : Form
    {
        string ConnectionString = System.Configuration.ConfigurationSettings.AppSettings["ConnectionString"].ToString();
        string ConnectionString1 = System.Configuration.ConfigurationSettings.AppSettings["ConnectionStringlive"].ToString();
        string btnclick = "0";
        public SendDataToServer()
        {
            InitializeComponent();
        }

        string CnnString = String.Empty;
        string SqlString = String.Empty;
        string queryString = String.Empty;
        int i = 0;

        DataTable dt1 = new DataTable();
        DataTable dt2 = new DataTable();
        DataTable dt3 = new DataTable();
        


        private void btnview_Click(object sender, EventArgs e)
        {
            string queryString2 = "select IsCashReciptSentToServer,count(*) as total from [dbo].[OrderBookingOffLine] group by IsCashReciptSentToServer";

            dt3 = utils.GetDataTable(queryString2, ConnectionString);            
            if ( dt3.Rows.Count > 0)
            {
                if (dt3.Rows[0]["IsCashReciptSentToServer"].ToString() == "N")
                {
                    label4.Text = dt3.Rows[0]["total"].ToString();
                    btnsend.Enabled = true;
                    btnsend.Visible = true;
                    if (dt3.Rows.Count > 1)
                    {
                        label5.Text = dt3.Rows[1]["total"].ToString();
                    }
                    else
                    {
                        label5.Text = "0";
                    }
                }
                else
                {
                    label5.Text = dt3.Rows[0]["total"].ToString();
                    label4.Text = "0";
                }                             
                
            }          
            else
            {
                label4.Text = "0";
                 label5.Text = "0";
                label1.Text = "No Record Found";
            }

        }

        private void btnclose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnsend_Click(object sender, EventArgs e)
        {
           
            string queryString = "select top 100 * from OrderBookingOffLine where IsCashReciptSentToServer='N'";
            //DataTable dt=utils.GetDataTable(string SQLString, string CnnString);
            dt1 = HSRPTransferData.utils.GetDataTable(queryString, ConnectionString);

            string strVehicleRegNO = string.Empty;
            if (dt1.Rows.Count > 0)
            {
                for (i = 0; i < dt1.Rows.Count; i++)
                {
                    //System.Threading.Thread.Sleep(100);
                   // timer1.Enabled = true;
                    strVehicleRegNO = dt1.Rows[i]["vehicleregno"].ToString();
                    string Sqlstring = "select count(*) from temp_hsrprecords where vehicleregno='" + strVehicleRegNO + "'";
                    int i1 = utils.getScalarCount(Sqlstring, ConnectionString1);
                    if (i1 == 0)
                    {
                        String rec_id = dt1.Rows[i]["RecordId"].ToString();

                        DateTime rec_date = Convert.ToDateTime(dt1.Rows[i]["Record_CreationDate"].ToString());

                        String state_id = dt1.Rows[i]["HSRP_StateID"].ToString();

                        String rto_id = dt1.Rows[i]["RTO_CD"].ToString();

                        String rtocd = dt1.Rows[i]["RTO_CD"].ToString();

                        String rec_auth_no = dt1.Rows[i]["HSRPRecord_AuthorizationNo"].ToString();

                        string[] date = dt1.Rows[i]["HSRPRecord_AuthorizationDate"].ToString().Split('/');
                        String rec_auth_date = date[2].Split(' ')[0] + "-" + date[1] + "-" + date[0] + " " + date[2].Split(' ')[1];

                        //  DateTime=Convert.ToDateTime(auth_date.ToString());

                        String veh_rg_no = dt1.Rows[i]["VehicleRegNo"].ToString();

                        String owner_name = dt1.Rows[i]["OwnerName"].ToString();

                        String add = dt1.Rows[i]["address"].ToString();

                        String mobile_no = dt1.Rows[i]["MobileNo"].ToString();

                        String vehicle_class = dt1.Rows[i]["VehicleClass"].ToString();

                        String order_type = dt1.Rows[i]["OrderType"].ToString();

                        String sticker = dt1.Rows[i]["StickerMandatory"].ToString();

                        String manf_name = dt1.Rows[i]["ManufacturerName"].ToString();

                        String manf_model = dt1.Rows[i]["ManufacturerModel"].ToString();

                        String vip = dt1.Rows[i]["VIP"].ToString();

                        String amount = dt1.Rows[i]["Amount"].ToString();

                        String vehicle_type = dt1.Rows[i]["VehicleType"].ToString();

                        String isrec_sent = dt1.Rows[i]["isRecordSentToServer"].ToString();

                        String cash_rec_no = dt1.Rows[i]["CashReceiptNo"].ToString();

                        String tax = dt1.Rows[i]["Tax"].ToString();

                        String chasis_no = dt1.Rows[i]["ChassisNo"].ToString();

                        String engine_no = dt1.Rows[i]["EngineNo"].ToString();

                        String front_laser = dt1.Rows[i]["hsrp_front_lasercode"].ToString();

                        String rear_laser = dt1.Rows[i]["hsrp_rear_lasercode"].ToString();

                        String order_status = dt1.Rows[i]["OrderStatus"].ToString();

                        string embo_date = dt1.Rows[i]["OrderEmbossingDate"].ToString();

                        string close_date = dt1.Rows[i]["OrderClosedDate"].ToString();

                        String cashrec_sms = dt1.Rows[i]["CashReceiptSMSText"].ToString();

                        //DateTime cash_sms_date = Convert.ToDateTime(dt1.Rows[i]["CashReceiptSMSDateTime"].ToString());

                        String cash_sms_responseid = dt1.Rows[i]["CashReceiptSMSServerResponseID"].ToString();

                        String cashReceiptSmsResponseText = dt1.Rows[i]["CashReceiptSMSServerResponseText"].ToString();


                        Sqlstring = "insert into HSRPRecords(HSRP_StateID,RTOLocationID,HSRPRecord_AuthorizationNo,HSRPRecord_AuthorizationDate, VehicleRegNo,"

                                + "OwnerName,[Address1],MobileNo,VehicleClass,OrderType,StickerMandatory,ManufacturerName,ManufacturerModel,IsVIP,NetAmount,VehicleType,CashReceiptNo,"

                                + "[VAT_Amount],ChassisNo,EngineNo,hsrp_front_lasercode,hsrp_rear_lasercode,OrderStatus,Addrecordby)"

                                + "Values ('" + state_id + "','" + rto_id + "','" + rec_auth_no + "','" + rec_auth_date + "','" + veh_rg_no + "','" + owner_name + "','" + add + "','" + mobile_no + "','" + vehicle_class + "','" + order_type + "','" + sticker + "','" + manf_name + "','" + manf_model + "','" + vip + "','" + amount + "','" + vehicle_type + "','" + cash_rec_no + "','" + tax + "','" + chasis_no + "','" + engine_no + "','" + front_laser + "','" + rear_laser + "','" + order_status + "','HPTRANS')";

                        utils.ExecNonQuery(Sqlstring, ConnectionString1);

                        Sqlstring = "update OrderBookingOffLine set IsCashReciptSentToServer='Y' where recordid='" + rec_id + "'";
                        utils.ExecNonQuery(Sqlstring, ConnectionString);
                    }
                    btnview_Click(sender, e);
                }
                
              //  timer1.Enabled = false;
                label1.Text = "Data Uploaded";
            }
            

        }

        



    }
}
 
 

