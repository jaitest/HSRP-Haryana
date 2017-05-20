using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Xml;
using System.Data;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Configuration;
using System.Data.SqlClient;
using System.Net;
using System.Text;

namespace HSRP.Transaction
{
    public partial class HRACVehicle : System.Web.UI.Page
    {
        public static string AddRecordBy = "";
        public static string CounterNo = "";
        static String ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        SqlConnection con = new SqlConnection(ConnectionString);

        string HSRPStateID = string.Empty;
        string RTOLocationID = string.Empty;
        string ProductivityID = string.Empty;
        string UserType = string.Empty;
        string UserName = string.Empty;
        string Sticker = string.Empty;
        string VIP = string.Empty;
        string USERID = string.Empty;
        DataTable dt = new DataTable();
        string macbase = string.Empty;
        string Query = string.Empty;
        int intDepositAmount = 0;
        int inttotcoll = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["UserType"].ToString() == null)
                {
                    Response.Redirect("~/Login.aspx");
                }
                else
                {
                    UserType = Session["UserType"].ToString();

                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx");
            }
            HSRPStateID = Session["UserHSRPStateID"].ToString();
            RTOLocationID = Session["UserRTOLocationID"].ToString();
            UserName = Session["UID"].ToString();
            USERID = Session["UID"].ToString();
            macbase = Session["MacAddress"].ToString();
            if (!IsPostBack)
            {
                if (UserType.Equals("0"))
                {

                }
                else
                {

                }
            }
            else
            {

            }
        }
        string SQLString = string.Empty;
        public static int CounterN = 1;

        protected void Button3_Click(object sender, EventArgs e)
        {
            if (txtVehicle.Text == null)
            {
                lblErrMess.Visible = true;
                lblErrMess.Text = "";
                lblErrMess.Text = "Please Enter Vehicle.";
                return;
            }
            if (txtVehicle.Text == "")
            {
                lblErrMess.Visible = true;
                lblErrMess.Text = "";
                lblErrMess.Text = "Please Enter Vehicle.";
                return;
            }
            if (txtAC.Text == null)
            {
                lblErrMess.Visible = true;
                lblErrMess.Text = "";
                lblErrMess.Text = "Please Enter AC NO.";
                return;
            }
            if (txtAC.Text == "")
            {
                lblErrMess.Visible = true;
                lblErrMess.Text = "";
                lblErrMess.Text = "Please Enter AC NO.";
                return;
            }
            string query = "insert into Vendor_HSRPRecordsAC(SNO,HSRP_StateID,RTOLocationID,affixationcode,VehicleRegNo,CreatedBy,UserId,ACNO)values('" + CounterN + "','" + HSRPStateID + "','" + RTOLocationID + "','" + RTOLocationID + "','" + txtVehicle.Text + "','" + USERID + "','" + USERID + "','" + txtAC.Text + "')";
            int i = Utils.ExecNonQuery(query, ConnectionString);
            if (i == 2627)
            {
                lblErrMess.Visible = true;
                lblErrMess.Text = "";
                lblErrMess.Text = "Duplicate Vehicle No";
                return;
            }
            lblCounter.Text = CounterN.ToString();
            CounterN += i;
            lblErrMess.Text = "";
            txtVehicle.Text = string.Empty;
        }

        static DataTable dt1;
        protected void lnkRefresh_Click(object sender, EventArgs e)
        {
            CounterN = 1;
            lblCounter.Text = "0";
            lblErrMess.Text = "";
            string query = "update Vendor_HSRPRecordsAC set HSRPRecord_AuthorizationNo=a.HSRPRecord_AuthorizationNo,HSRPRecord_AuthorizationDate=a.RegistrationDate,OwnerName=a.OwnerName,OwnerFatherName=a.OwnerFatherName,Address1=a.Address1,MobileNo=a.MobileNo,VehicleClass=a.VehicleClass,VehicleType=a.VehicleType,vehiclemake=a.ManufacturerName,modelname=a.ManufacturerModel,ChassisNo=a.ChassisNo,EngineNo=a.EngineNo,StaggingProcess='Y' from hsrprecordsstaggingarea a where replace(Vendor_HSRPRecordsAC.vehicleregno,'.','')=REPLACE(a.vehicleregno,'.','') and Vendor_HSRPRecordsAC.hsrp_stateid='" + HSRPStateID + "'and Vendor_HSRPRecordsAC.CreatedBy='" + USERID + "' and StaggingProcess='N' and ReadyToProcess='N'and convert(date,Vendor_HSRPRecordsAC.HSRPRecord_CreationDate)=convert(date,getdate())";
            int i = Utils.ExecNonQuery(query, ConnectionString);
            //lblErrMess.Visible = true;
            //lblErrMess.Text = "Record saved successfully";
            //dt1 = new DataTable();
            //string Query = "Select SNO,VehicleRegNo,VehicleType,OwnerName,Convert(date,HSRPRecord_CreationDate) as HSRPRecord_CreationDate from Vendor_HSRPRecordsAC where [HSRPRecord_AuthorizationNo] is null and [HSRPRecord_AuthorizationDate] is null and [OwnerName] is null and [OwnerFatherName] is null and [Address1] is null and [MobileNo] is null and [VehicleClass] is null and [VehicleType] is null and [vehiclemake] is null and [modelname] is null and [ChassisNo] is null and [EngineNo] is null and StaggingProcess ='N' and ReadyToProcess='N' and CreatedBy='" + USERID + "' and convert(date,HSRPRecord_CreationDate)=convert(date,getdate())";
            //dt1 = Utils.GetDataTable(Query, ConnectionString);                                 
            //if (dt1.Rows.Count > 0)
            //{
            //    GridView1.DataSource = dt1;
            //    GridView1.DataBind();
            //    btnDeleteFromGridData.Visible = true;
            //}
            //else
            //{
            //    GridView1.DataSource = null;
            //    GridView1.DataBind();
            //    btnDeleteFromGridData.Visible = false;
            //}
            //string Query1 = "Select SNO,VehicleRegNo,VehicleType,OwnerName,Convert(date,HSRPRecord_CreationDate) as HSRPRecord_CreationDate from Vendor_HSRPRecordsAC where StaggingProcess IN('Y','N') and ReadyToProcess='N' and CreatedBy='" + USERID + "' and convert(date,HSRPRecord_CreationDate)=convert(date,getdate())";
            string Query1 = "ACExcel '"+HSRPStateID+"','"+USERID+"'";
            //string Query1 = "select '2351' as DealerID,'RamGupta' as DealerName,'2351' as Dealercode, HSRPRecord_AuthorizationDate,convert(date,HSRPRecord_CreationDate) as HSRPRecord_CreationDate,VehicleClass,OrderType,AffixationCode,vehicleregno,OwnerName,[Address1] as Address,MobileNo,vehicletype,HSRPRecord_AuthorizationNo,EngineNo,ChassisNo,vehiclemake,ModelName,'10000' as PRICE from Vendor_HSRPRecordsAC where StaggingProcess IN('Y','N') and ReadyToProcess='N' and hsrp_stateid='"+HSRPStateID+"' and CreatedBy='"+USERID+"' and convert(date,HSRPRecord_CreationDate)=convert(date,getdate())";

            DataTable dt2 = new DataTable();
            dt2 = Utils.GetDataTable(Query1, ConnectionString);
            if (dt2.Rows.Count > 0)
            {
                for (int J = 0; J < dt2.Rows.Count; J++)
                {
                    string Qry = "update Vendor_HSRPRecordsAC set StaggingProcess='D', process='Y',ReadyToProcess='Y' where VehicleRegNo='" + dt2.Rows[J]["VehicleRegNo"] + "' and hsrp_stateid='" + HSRPStateID + "' and CreatedBy='" + USERID + "'and convert(date,HSRPRecord_CreationDate)=convert(date,getdate())";
                    Utils.ExecNonQuery(Qry, ConnectionString);
                }
                Export(dt2);
            }
            else
            {
                lblErrMess.Visible = true;
                lblErrMess.Text = "";
                lblErrMess.Text = "Record Not Found";
                return;
            }
        }

        //protected void btnDeleteFromGridData_Click(object sender, EventArgs e)
        //{
        //    if (dt1.Rows.Count > 0)
        //    {
        //        for (int i = 0; i < dt1.Rows.Count; i++)
        //        {
        //            string Qry = "update Vendor_HSRPRecordsAC set StaggingProcess='D', process='Y' where [HSRPRecord_AuthorizationNo] is null and [HSRPRecord_AuthorizationDate] is null and [OwnerName] is null and [OwnerFatherName] is null and [Address1] is null and [MobileNo] is null and [VehicleClass] is null and [VehicleType] is null and [vehiclemake] is null and [modelname] is null and [ChassisNo] is null and [EngineNo] is null and VehicleRegNo='" + dt1.Rows[i]["VehicleRegNo"] + "'";
        //            Utils.ExecNonQuery(Qry, ConnectionString);
        //        }
        //        GridView1.DataSource = null;
        //        GridView1.DataBind();
        //        btnDeleteFromGridData.Visible = false;
        //    }
        //}

        public void Export(DataTable dt)
        {
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "Records.xls"));
            Response.ContentType = "application/ms-excel";

            string str = string.Empty;
            foreach (DataColumn dtcol in dt.Columns)
            {
                Response.Write(str + dtcol.ColumnName);
                str = "\t";
            }
            Response.Write("\n");
            foreach (DataRow dr in dt.Rows)
            {
                str = "";
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    Response.Write(str + Convert.ToString(dr[j]));
                    str = "\t";
                }
                Response.Write("\n");
            }
            Response.End();

            //dt = city.GetAllCity();//your datatable
            //string attachment = "attachment; filename=Records.xls";
            //Response.ClearContent();
            //Response.AddHeader("content-disposition", attachment);
            //Response.ContentType = "application/vnd.ms-excel";
            //string tab = "";
            //foreach (DataColumn dc in dt.Columns)
            //{
            //    Response.Write(tab + dc.ColumnName);
            //    tab = "\t";
            //}
            //Response.Write("\n");
            //int i;
            //foreach (DataRow dr in dt.Rows)
            //{
            //    tab = "";
            //    for (i = 0; i < dt.Columns.Count; i++)
            //    {
            //        Response.Write(tab + dr[i].ToString());
            //        tab = "\t";
            //    }
            //    Response.Write("\n");
            //}
            //Response.End();
        }

        //protected void lnkSyncOnServer_Click(object sender, EventArgs e)
        //{
        //    using (var conn = new SqlConnection(ConnectionString))
        //    using (var cmd = conn.CreateCommand())
        //    {
        //        conn.Open();
        //        try
        //        {
        //            cmd.CommandText = "DataUpload_Insert_VendorAC";
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.ExecuteNonQuery();
        //            lblErrMess.Text = "Record Sync Sucessfully.";
        //            string strQuery = "select vehicleregno from Vendor_HSRPRecordsAC where process='D' and remarks='allready in system'";
        //            DataTable dtVehicle = Utils.GetDataTable(strQuery, ConnectionString);
        //            string strVeh = string.Empty;
        //            if (dtVehicle.Rows.Count > 0)
        //            {

        //                strVeh = dtVehicle.Rows[0][0].ToString();
        //                for (int i = 1; i < dtVehicle.Rows.Count; i++)
        //                {
        //                    strVeh = strVeh + "," + dtVehicle.Rows[i][0].ToString();

        //                }
        //                lblvehicle1.Text = strVeh + " are already exist";
        //                lblvehicle1.Text = "";
        //                string strUpdateQuery = "update  Vendor_HSRPRecordsAC set remarks='allready in system Send' where process='D' and remarks='allready in system'";
        //                Utils.ExecNonQuery(strUpdateQuery, ConnectionString);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            lblErrMess.Text = "Error in Sync :- " + ex.Message.ToString();
        //        }
        //    }
        //}
    }
}