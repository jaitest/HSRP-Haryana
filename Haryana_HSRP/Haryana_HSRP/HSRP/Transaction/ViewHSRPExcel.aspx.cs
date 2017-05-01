using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Net;
using System.IO;

namespace HSRP.Transaction
{
    public partial class ViewDealerExcel : System.Web.UI.Page
    {
        string SQLString = string.Empty;
        string CnnString = string.Empty;
        string UserID = string.Empty;
        string HSRPStateID = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            UserID = Session["UID"].ToString();
            HSRPStateID = Session["UserHSRPStateID"].ToString();
            if (!IsPostBack)
            {
                Show();
            }
        }

        DataTable dt = new DataTable();
        public void Show()
        {
            ///>> For Company [A]
            SQLString = "select * from dbo.HSRPExcelUpload where StateID=" + HSRPStateID + "";
            dt = Utils.GetDataTable(SQLString, CnnString);
            Grid1.DataSource = dt;
            Grid1.DataBind();
        }
        protected void Grid1_ItemCommand(object sender, ComponentArt.Web.UI.GridItemCommandEventArgs e)
        {
            String ID = e.Item["ID"].ToString();

            SQLString = "Select RenamedFileName From HSRPExcelUpload where ID=" + ID;
            string FileName = Utils.getDataSingleValue(SQLString, CnnString, "RenamedFileName");
           // FileName = "HSRP-123201311216.xls";
            string FileURL="http://180.151.100.242/HSRPExcel/"+FileName;
            HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(FileURL);
            HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
            int bufferSize = 1;
            Response.Clear();
            Response.ClearHeaders();
            Response.ClearContent();
            Response.AppendHeader("Content-Disposition:", "attachment; filename=" + FileName + "");
            Response.AppendHeader("Content-Length", objResponse.ContentLength.ToString());
            Response.ContentType = "application/download";
            byte[] byteBuffer = new byte[bufferSize + 1];
            MemoryStream memStrm = new MemoryStream(byteBuffer, true);
            Stream strm = objRequest.GetResponse().GetResponseStream();
            byte[] bytes = new byte[bufferSize + 1];
            while (strm.Read(byteBuffer, 0, byteBuffer.Length) > 0)
            {
                Response.BinaryWrite(memStrm.ToArray());
                Response.Flush();
            }
            Response.Close();
            Response.End();
            memStrm.Close();
            memStrm.Dispose();
            strm.Dispose();



            //if (e.Item["DeviceID"] == null)
            //{
            //    return;
            //}
            //String DeviceID = e.Item["DeviceID"].ToString();
            //StringBuilder sb = new StringBuilder();
            ////sb.Append("Update Vehicle Set DeviceID=null,VehicleActiveStatus='N' Where VehicleID=" + VehicleID + ";");
            //////sb.Append("Delete From VehicleSensorRelation Where VehicleID=" + VehicleID + ";");
            ////sb.Append("update VehicleSensorRelation set DeviceID=null,OrgID=null,ClientID=null,Fuel=0,AC=0,Door=0,Panic=0,Immobilize=0,RFID=0 Where VehicleID=" + VehicleID + ";");
            ////sb.Append("Update CurrentVehicleString Set  VehicleRegNo=null,VehicleType=null,VehicleID=null Where DeviceID=" + DeviceID + ";");
            ////sb.Append("Update DeviceMaster set InstallationStatus='Not Installed' ,DeviceStatus='Y' where DeviceID=" + DeviceID + ";");



            ////sb.Append("Update Vehicle Set DeviceID=null,orgid=null,clientid=null,VehicleActiveStatus='N' Where VehicleID=" + VehicleID + ";");
            //sb.Append("Update Vehicle Set DeviceID=null,VehicleActiveStatus='N' Where DeviceID=" + DeviceID + ";");

            //sb.Append("update VehicleSensorRelation set DeviceID=null,OrgID=null,ClientID=null,Fuel=0,AC=0,Door=0,Panic=0,Immobilize=0,RFID=0 Where DeviceID=" + DeviceID + ";");

            //sb.Append("Update CurrentVehicleString Set  VehicleRegNo=null,orgid=null,clientid=null,VehicleType=null,VehicleID=null Where DeviceID=" + DeviceID + ";");
            //sb.Append("Update DeviceMaster set InstallationStatus='Not Installed' ,DeviceStatus='Y' where DeviceID=" + DeviceID + ";");
            //Utils.ExecNonQuery(sb.ToString(), CnnString);
            Show();
        }
    }
}