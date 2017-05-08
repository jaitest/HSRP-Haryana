using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.IO;
using System.Data;
using System.Configuration;

namespace HSRP.Transaction
{
    public partial class CreateFlatFile : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        int UserType;
        int HSRPStateID;
        int RTOLocationID;
        int intOrgID;
        int intClientID;
        int UID;
        string SaveLocation = string.Empty;
        StringBuilder SBSQL = new StringBuilder();
        protected void Page_Load(object sender, EventArgs e)
        {
            //Utils.GZipEncodePage();
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {
                int.TryParse(Session["UserType"].ToString(), out UserType);
                int.TryParse(Session["UserHSRPStateID"].ToString(), out HSRPStateID);
                int.TryParse(Session["UserRTOLocationID"].ToString(), out RTOLocationID);
                int.TryParse(Session["UID"].ToString(), out UID);

                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                //strUserID = Session["UID"].ToString();
                ComputerIP = Request.UserHostAddress;

                string usrname1 = Utils.getDataSingleValue("Select UserFirstName + space(2)+UserLastName as UserName From Users where UserID=" + UID.ToString(), CnnString, "UserName");
                LabelCreatedID.Text = usrname1;
                LabelCreatedDateTime.Text = DateTime.Now.ToString("dd MMM yyyy");

                if (!IsPostBack)
                {
                    FilldropDownState();
                    FilldropDownRTOLocation();
                    string StateID = Session["UserHSRPStateID"].ToString();
                   // Uploaddata(StateID);
                    FileDropdown();
                }
            }
        }

        #region DropDown

        private void FilldropDownState()
        {
            if (UserType.Equals(0))
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState where ActiveStatus='Y' Order by HSRPStateName";
                Utils.PopulateDropDownList(dropDownListOrg, SQLString.ToString(), CnnString, "--Select State--");
            }
            else
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID=" + HSRPStateID + " and ActiveStatus='Y' Order by HSRPStateName";
                DataSet dts = Utils.getDataSet(SQLString, CnnString);
                dropDownListOrg.DataSource = dts;
                dropDownListOrg.DataBind();
            } 
        } 
        private void FilldropDownRTOLocation()
        {
            if (UserType.Equals(0))
            {
                int.TryParse(dropDownListOrg.SelectedValue, out intOrgID);
                SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + intOrgID + " and ActiveStatus='Y' and LocationType='Sub-Urban' Order by RTOLocationName";
                Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), CnnString, "--Select RTO--");
            }
            else
            {
                 
                //SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where RTOLocationID=" + RTOLocationID + " and ActiveStatus='Y' and LocationType='Sub-Urban' Order by RTOLocationName";
                SQLString = "SELECT distinct (a.RTOLocationName) as RTOLocationName, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID where  a.LocationType!='District' and UserRTOLocationMapping.UserID='" + UID + "' ";
                DataSet dss = Utils.getDataSet(SQLString, CnnString);
                dropDownListClient.DataSource = dss;
                dropDownListClient.DataBind();

                string RTOCode = string.Empty;
                SQLString = "SELECT distinct (a.RTOLocationCode+' , ') as RTOLocationName, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID where  a.LocationType!='District' and UserRTOLocationMapping.UserID='" + UID + "' ";
                DataTable dts = Utils.GetDataTable(SQLString, CnnString);
                if (dts.Rows.Count > 0)
                {
                    for (int i = 0; i <= dts.Rows.Count - 1; i++)
                    {
                        RTOCode += dts.Rows[i]["RTOLocationName"].ToString();
                    }
                    
                    lblRTOCode.Text = RTOCode.Remove(RTOCode.LastIndexOf(","));
                }

            }
        }

        private void FilldropDownFileName()
        {
            //string rtolocid=dropDownListClient.SelectedValue;
            //SQLString = "select distinct filename from HSRPRecordsStaggingArea where rtolocationid='" + rtolocid +"' order by filename" ;
            //DataSet dts = Utils.getDataSet(SQLString, CnnString);
            //dropDownListFilename.DataSource = dts;
            //dropDownListFilename.DataBind(); 
        }

        #endregion
        

        protected void dropDownListOrg_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dropDownListOrg.SelectedItem.Text != "--Select State--")
            {
                dropDownListClient.Visible = true;
                FilldropDownRTOLocation();
            }
        }

    

        protected void ButImpData_Click(object sender, EventArgs e)
        {
            string StateID = dropDownListOrg.SelectedValue;
            if (StateID == "4")
            {

                //string getDataQry = "SELECT case when len(b.nicvehicleRegNo)<=9 then left(b.NicVehicleRegNo,5)+' '+(right(b.nicvehicleRegNo,4)) else b.nicvehicleRegNo end as nicvehicleRegNo,replace(b.ChassisNo,'*','') as chassisNo,b.EngineNo,b.OrderType,case when len(b.nicvehicletype)<=25 then  b.nicvehicletype+'*NOT AVAILABLE*NOT AVAILABLE' else b.nicvehicletype end,b.OwnerName,b.Manufacturer,b.hsrpRecord_AuthorizationNo,a.HSRP_Front_LaserCode,a.HSRP_Rear_LaserCode, (select CASE WHEN datediff(DAY, a.HSRPRecord_CreationDate, a.OrderClosedDate ) <5  THEN replace (convert(varchar, a.HSRPRecord_CreationDate+5,106),' ','-')  ELSE replace (convert(varchar, a.OrderClosedDate,106),' ','-')  END) as OrderCloseDate, convert(varchar(50), a.netamount) as netamount, replace (convert(varchar, a.HSRPRecord_CreationDate,106),' ','-') AS CashReceiptDateTime FROM  HSRPRecords a INNER JOIN HSRPRecordsStaggingArea b ON a.VehicleRegNo = b.VehicleRegNo where a.orderstatus='Closed' and a.ownername is not null and a.netamount is not null and a.HSRP_Front_LaserCode is not null and a.HSRP_Rear_LaserCode is not null and  b.filename = '" + dropDownListFilenameforlocation.SelectedItem.Text + "'";
               //string getDataQry = "SELECT case when len(b.nicvehicleRegNo)<=9 then left(b.NicVehicleRegNo,5)+' '+(right(b.nicvehicleRegNo,4)) else b.nicvehicleRegNo end as nicvehicleRegNo,replace(b.ChassisNo,'*','') as chassisNo,b.EngineNo,b.OrderType,b.nicvehicletype,b.OwnerName,b.Manufacturer,b.hsrpRecord_AuthorizationNo,b.Address1,b.NICDate,b.NICVehicleClass,b.NICLocationID,b.mobileno,a.HSRP_Front_LaserCode,a.HSRP_Rear_LaserCode,replace (convert(varchar, a.HSRPRecord_CreationDate,105),'-','.') AS CreateionDate, convert(varchar(50), a.netamount) as netamount, replace (convert(varchar, a.OrderClosedDate,105),'-','.') AS CashReceiptDateTime FROM  HSRPRecords a INNER JOIN HSRPRecordsStaggingArea b ON a.VehicleRegNo = b.VehicleRegNo where a.orderstatus='Closed' and a.ownername is not null and a.netamount is not null and a.HSRP_Front_LaserCode is not null and a.HSRP_Rear_LaserCode is not null  and  b.filename = '" + dropDownListFilenameforlocation.SelectedItem.Text + "'";

                //string getDataQry = "SELECT   case when len(b.nicvehicleRegNo)<=9 then left(b.NicVehicleRegNo,5)+' '+(right(b.nicvehicleRegNo,4)) else b.nicvehicleRegNo end as nicvehicleRegNo,replace(b.ChassisNo,'*','') as chassisNo,b.EngineNo,b.OrderType,b.nicvehicletype,b.OwnerName,b.Manufacturer, b.hsrpRecord_AuthorizationNo,b.Address1,b.NICDate,b.NICVehicleClass,b.NICLocationID,b.mobileno, a.HSRP_Front_LaserCode,a.HSRP_Rear_LaserCode, replace (convert(varchar, a.HSRPRecord_CreationDate,105),'-','.') AS CashReceiptDateTime,convert(varchar(50), a.netamount) as netamount, (select CASE WHEN datediff(DAY, a.HSRPRecord_CreationDate, a.OrderClosedDate ) >3  THEN replace (convert(varchar, a.HSRPRecord_CreationDate+3,105),'-','.')  ELSE replace (convert(varchar, a.OrderClosedDate,105),'-','.')  END) as OrderCloseDate FROM  HSRPRecords a INNER JOIN HSRPRecordsStaggingArea b ON a.VehicleRegNo = b.VehicleRegNo where a.orderstatus='Closed' and  a.ownername is not null and a.netamount is not null and a.HSRP_Front_LaserCode is not null and a.HSRP_Rear_LaserCode is not null   and a.rtolocationID in ('481','482')  and a.VehicleRegNo like 'hr51%' and b.Address1 is not null  ";
                string getDataQry = "SELECT [REGN_NO],[chasi_no],[eng_no],[HSRP_FLAG],[VH_CLASS],[O_NAME],[DEALER],[AUTH_NO],[Address],[OP_DT],[VEHICLE_TYPE],[rto_cd],[mobile_no],[HSRP_Front_LaserCode],[HSRP_Rear_LaserCode],[CashReceiptDateTime],[RoundOff_NetAmount],[OrderClosedDate] FROM [Temp_HR_NIC_Data] where rto_code='" + dropDownListClient.SelectedValue + "' and flat_file='"+dropDownListFilenameforlocation.SelectedItem.ToString()+"' and  [O_NAME] is not null and RoundOff_NetAmount is not null and HSRP_Front_LaserCode is not null and HSRP_Rear_LaserCode is not null  and Address is not null";
              
                DataTable dt = Utils.GetDataTable(getDataQry, CnnString);
                if (dt.Rows.Count > 0)
                {
                    //string updateStatus = "UPDATE HSRPRecords SET SendRecordToNic='Y' from HSRPRecords a,HSRPRecordsStaggingArea b where a.VehicleRegNo=b.VehicleRegNo and b.HSRP_StateID='" + dropDownListOrg.SelectedValue + "' and b.RTOLocationID='" + dropDownListClient.SelectedValue + "' and a.OrderStatus='Closed' and a.SendRecordToNic='N' ";
                    //Utils.ExecNonQuery(updateStatus, CnnString);
                    llbMSGSuccess.Text = " Total Record Transfered to NIC is : " + dt.Rows.Count;
                    string fileName = ConfigurationManager.AppSettings["DataFolder"].ToString() + "HSRP-" + dropDownListClient.SelectedItem.ToString().Replace(" ", "") + '-' + DateTime.Now.ToString("ddmmyyyyhhmmss") + ".dat";
                    //string fileName = ConfigurationManager.AppSettings["DataFolder"].ToString() + dropDownListFilename.SelectedValue;
                    CreateCSVFile(dt, fileName);
                }
                else
                {
                    llbMSGError.Text = "Closed Records Not Found";
                }
            }
             
        }

        #region Export Grid to CSV
        public void CreateCSVFile(DataTable dt, string strFilePath)
        {
            // Create the CSV file to which grid data will be exported.

            StreamWriter sw = new StreamWriter(strFilePath, false);

            // First we will write the headers.

            //DataTable dt = m_dsProducts.Tables[0];

            int iColCount = dt.Columns.Count;
           
            //for (int i = 0; i < iColCount; i++)
            //{

            //    sw.Write(dt.Columns[i]);

            //    if (i < iColCount - 1)
            //    {

            //        sw.Write(";");

            //    }

            //}

            //sw.Write(sw.NewLine);

            // Now write all the rows.

            foreach (DataRow dr in dt.Rows)
            {
                for (int i = 0; i < iColCount; i++)
                {
                    //if (i == 5)
                    //{
                    //    dr[5] = dr[5].ToString().Replace("`", "'");
                    //}
                    //if (i == 6)
                    //{
                    //    dr[6] = dr[6].ToString().Replace("`", "'");
                    //}
                    if (!Convert.IsDBNull(dr[i]))
                    {
                        sw.Write(dr[i].ToString());
                    }
                    if (i < iColCount - 1)
                    {
                        sw.Write(";");
                    }
                }
                sw.Write(sw.NewLine);
            }
            sw.Close();

            System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
            response.ClearContent();
            response.Clear();
            response.ContentType = "text/plain";

            if (Session["UserHSRPStateID"].ToString() == "2" || Session["UserHSRPStateID"].ToString() == "4" || Session["UserHSRPStateID"].ToString() == "1")
            {
                
                response.AddHeader("Content-Disposition", "attachment; filename=" + dropDownListFilenameforlocation.SelectedValue + ";");
            }
            else
            {
                response.AddHeader("Content-Disposition", "attachment; filename=" + strFilePath + ";");
            }
            
            response.TransmitFile(strFilePath);
            response.Flush();
            response.End();
        }
        #endregion

        protected void dropDownListClient_SelectedIndexChanged(object sender, EventArgs e)
        {
            FileDropdown();
        }

        private void FileDropdown()
        {
            if (Session["UserHSRPStateID"].ToString() == "4" || Session["UserHSRPStateID"].ToString() == "1")
            {
                if (dropDownListClient.SelectedItem.Text != "--Select RTO--")
                {
                    divLocationFileName.Visible = true;
                    SQLString = "select distinct flat_file as filename from [Temp_HR_NIC_Data] where rto_code='" + dropDownListClient.SelectedValue + "'  and flat_file is not null order by flat_file desc";
                    Utils.PopulateDropDownList(dropDownListFilenameforlocation, SQLString.ToString(), CnnString, "--Select FileName--");
                }
                else
                {
                    divLocationFileName.Visible = false;
                }
            }
            else
            {
                if (Session["UserHSRPStateID"].ToString() == "2")
                {
                    divLocationFileName.Visible = false;
                }
                else
                {
                    divLocationFileName.Visible = true;
                }
            }
        }

        


        //protected void dropDownListClient_SelectedIndexChanged(object sender, EventArgs e)
        //{

        //    string StateID = dropDownListOrg.SelectedValue;
        //    Uploaddata(StateID);
        //}

        //public void Uploaddata(string StateID)
        //{
        //    if (StateID == "4")
        //    {

        //        if (dropDownListClient.SelectedItem.Text != "--Select RTO--")
        //        {
        //            LlbSelectFilename.Visible = true;
        //            dropDownListFilename.Visible = true;
        //            FilldropDownFileName();
        //        }
        //    }
        //    else
        //    {
        //        LlbSelectFilename.Visible = false;
        //        dropDownListFilename.Visible = false;
        //    }
        //}
    }
}