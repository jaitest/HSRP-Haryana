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

namespace HSRPDataEntryNew
{
    public partial class TransferRecord : Form
    {
        public TransferRecord()
        {
            InitializeComponent();
        }
      
        string ConnectionString = System.Configuration.ConfigurationSettings.AppSettings["ConnectionString"].ToString();
        string ServerConnectionString = System.Configuration.ConfigurationSettings.AppSettings["ConnectionStringServer"].ToString();
        string CnnString = String.Empty;
        string SqlString = String.Empty;

        DataTable dt = new DataTable();
        DataTable dt1 = new DataTable();
        DataTable dt2 = new DataTable();
        StringBuilder strLocalUpdate = new StringBuilder();
        StringBuilder strServerInsert = new StringBuilder();
        private void BtnSave_Click(object sender, EventArgs e)
        {
            int i=0;
            string from  = FromDate.Value.ToString();
            string [] from1 = from.Split(' ');

            string To = ToDate.Value.ToString();
            string[] To1 = To.Split(' ');
            string Query = "select * from HSRPRecords where HSRPRecord_CreationDate between '" + from1[0].ToString() + " 00:00:00" + "' and '" + To1[0].ToString() + " 23:59:59" + "' and IsProcessed='N' ";
            dt = utils.GetDataTable(Query, ConnectionString);
         
            int count = dt.Rows.Count;
            if (dt.Rows.Count > 0)
            {
                for (i = 0; i < count; i++)
                {
                    string Query1 = "select * from HSRPRecords where VehicleRegNo='" + dt.Rows[i]["VehicleRegNo"].ToString() + "'";
                    dt1 = utils.GetDataTable(Query1, ServerConnectionString);
                    if (dt1.Rows.Count == 0)
                    {
                      
                        strServerInsert.Append("INSERT INTO [dbo.HSRPRecords_Bihar]([HSRP_StateID],[RTOLocationName],[HSRPRecord_AuthorizationNo],[HSRPRecord_AuthorizationDate]" +
                       ",[OwnerName],[Address1],[MobileNo],[OrderType],[OrderStatus],[VehicleClass]" +
                       ",[VehicleType],[ManufacturerName],[ManufacturerModel],[ChassisNo],[EngineNo]" +
                       ",[VehicleRegNo],[HSRP_Front_LaserCode],[HSRP_Rear_LaserCode],[FrontPlateSize],[RearPlateSize]" +
                       ",[StickerMandatory],CashReceipt,[CashReceiptDateTime],[TotalAmount],VehicleColor,Remarks)" +
                       "Values('" + dt.Rows[i]["HSRP_StateID"].ToString() + "','" + dt.Rows[i]["RTOLocationName"].ToString() + "','" + dt.Rows[i]["HSRPRecord_AuthorizationNo"].ToString() + "','" + dt.Rows[i]["HSRPRecord_AuthorizationDate"].ToString() + "'," +
                       "'" + dt.Rows[i]["OwnerName"].ToString() + "','" + dt.Rows[i]["Address1"].ToString() + "','" + dt.Rows[i]["MobileNo"].ToString() + "','" + dt.Rows[i]["OrderType"].ToString() + "','" + dt.Rows[i]["OrderStatus"].ToString() + "','" + dt.Rows[i]["VehicleClass"].ToString() + "','" + dt.Rows[i]["VehicleType"].ToString() + "'," +
                       "'" + dt.Rows[i]["ManufacturerName"].ToString() + "','" + dt.Rows[i]["ManufacturerModel"].ToString() + "','" + dt.Rows[i]["ChassisNo"].ToString() + "','" + dt.Rows[i]["EngineNo"].ToString() + "','" + dt.Rows[i]["VehicleRegNo"].ToString() + "'," +
                       "'" + dt.Rows[i]["HSRP_Front_LaserCode"].ToString() + "','" + dt.Rows[i]["HSRP_Rear_LaserCode"].ToString() + "','" + dt.Rows[i]["FrontPlateSize"].ToString() + "','" + dt.Rows[i]["RearPlateSize"].ToString() + "','" + dt.Rows[i]["StickerMandatory"].ToString() + "','" + dt.Rows[i]["CashReceipt"].ToString() + "','" + dt.Rows[i]["CashReceiptDateTime"].ToString() + "','" + dt.Rows[i]["TotalAmount"].ToString() + "','" + dt.Rows[i]["VehicleColor"].ToString() + "','" + dt.Rows[i]["Remarks"].ToString() + "') " + Environment.NewLine + " ");


                       
                    }
                  
                    strLocalUpdate.Append("update dbo.HSRPRecords set IsProcessed='Y' where id = '" + dt.Rows[i]["HSRPRecordID"].ToString() + "'; ");
                }

                try
                {
                    utils.ExecNonQuery(strServerInsert.ToString(), ServerConnectionString);
                    utils.ExecNonQuery(strLocalUpdate.ToString(), ConnectionString);
                    strServerInsert.Clear();

                }
                catch
                {
                }

             
            }

        }
    }
}
