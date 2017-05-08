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
    public partial class ImpExpData : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        int UserType;
        int HSRPStateID;
        int RTOLocationID;
        int intOrgID;
     
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
                   // FilldropDownFileName();
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
//hsrp
        private void FilldropDownFileName()
        {
            //string rtolocid = dropDownListClient.SelectedValue;
            //SQLString = "select distinct filename from HSRPRecordsStaggingArea where rtolocationid='" + rtolocid + "' order by filename";
            //DataSet dts = Utils.getDataSet(SQLString, CnnString);
            //dropDownListFilenameforlocation.DataSource = dts;
            //dropDownListFilenameforlocation.DataBind(); 
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

        protected void Button1_Click(object sender, EventArgs e)
        {
            if ((FileUpload1.PostedFile != null) && (FileUpload1.PostedFile.ContentLength > 0))
            {
                InsertDataInstage(dropDownListOrg.SelectedValue);
            }
            else
            {
                llbMSGError.Text = "Please select a file to upload.";
                llbMSGSuccess.Text = "";
            }
        }

        private void InsertDataInstage(string StateID)
        {
            llbMSGError.Text = "";
            string filename = System.IO.Path.GetFileName(FileUpload1.PostedFile.FileName);

            //string shortStateName=Utils.getScalarValue("Select HSRPStateShortName from HSRPState where HSRP_StateID='"+ dropDownListOrg.SelectedValue +"'",CnnString);
            //string RTOCode=Utils.getScalarValue("Select Rtocode from RTOLocation where RTOLocationID='"+ dropDownListClient.SelectedValue +"'",CnnString);


            //SaveLocation = Server.MapPath("C:\\inetpub\\wwwroot\\HSRP\\Transaction\\180.151.100.242\\HSRP\\bgimages") + "\\" + filename;
            //string PdfFolder = ConfigurationManager.AppSettings["PdfFolder"].ToString() + filename;
            SaveLocation = ConfigurationManager.AppSettings["DataFolder"].ToString() + filename;
            if (StateID == "4")
            {
                try
                {
                    FileUpload1.PostedFile.SaveAs(SaveLocation);
                    string CSVFilePathName = SaveLocation;
                    string[] Lines = File.ReadAllLines(CSVFilePathName);
                    string[] Fields;
                    int countInsert = 0;
                    int countDuplicate = 0;
                    Fields = Lines[0].Split(new char[] { ';' });

                    if (Fields.Length == 12)
                    {

                        int Cols = Fields.GetLength(0);
                        DataTable dt = new DataTable();
                        for (int i = 0; i < Cols; i++)
                            dt.Columns.Add(Fields[i].ToLower(), typeof(string));
                        DataRow Row;                        
                        for (int i = 0; i < Lines.GetLength(0); i++)
                        {
                            Fields = Lines[i].Split(new char[] { ';' });
                            Row = dt.NewRow();
                            for (int f = 0; f < Cols; f++)
                                Row[f] = Fields[f];
                            dt.Rows.Add(Row);
                        }

                        StringBuilder sbColumns = new StringBuilder();
                        StringBuilder sbRows = new StringBuilder();
                        string VehicleReg = string.Empty;
                        string HSRP_Authrization_no = string.Empty;
                        string NICVehicleType = string.Empty;
                        string NICVehicleRegNo = string.Empty;
                        string StrOwnerName = string.Empty;
                        string StrDealerName = string.Empty;

                        for (int i = 0; i <= dt.Rows.Count - 1; i++)
                        {
                            
                            NICVehicleRegNo = dt.Rows[i][0].ToString();
                            NICVehicleType = dt.Rows[i][5].ToString();
                            StrOwnerName = dt.Rows[i][4].ToString().Replace("'", "`");
                            StrDealerName = dt.Rows[i][6].ToString().Replace("'", "`");
                            HSRP_Authrization_no = dt.Rows[i][7].ToString();
                            string ourvehicleType = Utils.getScalarValue("select ourvalue from Mapping_Vahan_HSRP where vahanvalue='" + NICVehicleType + "'", CnnString);
                            //sbRows.Replace(NICVehicleType, ourvehicleType);
                            sbRows.Append("'" + dt.Rows[i][0].ToString() + "','" + dt.Rows[i][1].ToString() + "','" + dt.Rows[i][2].ToString() + "','" + dt.Rows[i][3].ToString().Trim() + "','" + ourvehicleType + "','" + StrOwnerName + "','" + StrDealerName + "','" + dt.Rows[i][7].ToString() + "','" + dt.Rows[i][8].ToString() + "','" + dt.Rows[i][9].ToString() + "','" + dt.Rows[i][10].ToString() + "','" + dt.Rows[i][11].ToString() + "','" + dropDownListOrg.SelectedValue + "','" + dropDownListClient.SelectedValue + "'");
                            string chkRecord = "SELECT COUNT(*) FROM HSRPRecordsStaggingArea WHERE HSRPRecord_AuthorizationNo='" + HSRP_Authrization_no + "' and RTOLocationID='" + dropDownListClient.SelectedValue + "'";
                            int count = Utils.getScalarCount(chkRecord, CnnString);
                            if (count < 1)
                            {
                                string Hsrp_History = string.Empty;

                                if (dt.Rows[i][3].ToString().Trim() == "NB" || dt.Rows[i][3].ToString().Trim() == "OB" || dt.Rows[i][3].ToString().Trim() == "DB")
                                {
                                    Hsrp_History = "INSERT INTO HSRPRecordsStaggingArea (VehicleRegNo,ChassisNo,EngineNo,OrderType,VehicleType,OwnerName,Manufacturer,HSRPRecord_AuthorizationNo,Address1,NICDate,NICVehicleClass,NICLocationID,HSRP_StateID,RTOLocationID,ISFrontPlateSize,ISRearPlateSize,VehicleClass,FileName,NICVehicleType,NICVehicleRegNo) values (" + sbRows.ToString() + ",'Y','Y','" + dt.Rows[i][10].ToString() + "','" + filename + "','" + NICVehicleType + "','" + NICVehicleRegNo + "');";
                                }
                                else if (dt.Rows[i][3].ToString().Trim() == "DF")
                                {
                                    Hsrp_History = "INSERT INTO HSRPRecordsStaggingArea (VehicleRegNo,ChassisNo,EngineNo,OrderType,VehicleType,OwnerName,Manufacturer,HSRPRecord_AuthorizationNo,Address1,NICDate,NICVehicleClass,NICLocationID,HSRP_StateID,RTOLocationID,ISFrontPlateSize,ISRearPlateSize,VehicleClass,FileName,NICVehicleType,NICVehicleRegNo) values (" + sbRows.ToString() + ",'Y','N','" + dt.Rows[i][10].ToString() + "','" + filename + "','" + NICVehicleType + "','" + NICVehicleRegNo + "');";
                                    //Hsrp_History = "INSERT INTO HSRPRecordsStaggingArea (VehicleRegNo,ChassisNo,EngineNo,OrderType,VehicleType,OwnerName,Manufacturer,HSRPRecord_AuthorizationNo,HSRP_StateID,RTOLocationID,ISFrontPlateSize,ISRearPlateSize,VehicleClass,FileName,NICVehicleType,NICVehicleRegNo) values (" + sbRows.ToString() + ",'Y','N','Non-Transport','" + filename + "','" + NICVehicleType + "','" + NICVehicleRegNo + "');";
                                }
                                else if (dt.Rows[i][3].ToString().Trim() == "DR")
                                {
                                    Hsrp_History = "INSERT INTO HSRPRecordsStaggingArea (VehicleRegNo,ChassisNo,EngineNo,OrderType,VehicleType,OwnerName,Manufacturer,HSRPRecord_AuthorizationNo,Address1,NICDate,NICVehicleClass,NICLocationID,HSRP_StateID,RTOLocationID,ISFrontPlateSize,ISRearPlateSize,VehicleClass,FileName,NICVehicleType,NICVehicleRegNo) values (" + sbRows.ToString() + ",'N','Y','" + dt.Rows[i][10].ToString() + "','" + filename + "','" + NICVehicleType + "','" + NICVehicleRegNo + "');";
                                    //Hsrp_History = "INSERT INTO HSRPRecordsStaggingArea (VehicleRegNo,ChassisNo,EngineNo,OrderType,VehicleType,OwnerName,Manufacturer,HSRPRecord_AuthorizationNo,HSRP_StateID,RTOLocationID,ISFrontPlateSize,ISRearPlateSize,VehicleClass,FileName,NICVehicleType,NICVehicleRegNo) values (" + sbRows.ToString() + ",'N','Y','Non-Transport','" + filename + "','" + NICVehicleType + "','" + NICVehicleRegNo + "');";
                                }
                                else if (dt.Rows[i][3].ToString().Trim() == "OS")
                                {
                                    Hsrp_History = "INSERT INTO HSRPRecordsStaggingArea (VehicleRegNo,ChassisNo,EngineNo,OrderType,VehicleType,OwnerName,Manufacturer,HSRPRecord_AuthorizationNo,Address1,NICDate,NICVehicleClass,NICLocationID,HSRP_StateID,RTOLocationID,ISFrontPlateSize,ISRearPlateSize,VehicleClass,FileName,NICVehicleType,NICVehicleRegNo) values (" + sbRows.ToString() + ",'N','N','" + dt.Rows[i][10].ToString() + "','" + filename + "','" + NICVehicleType + "','" + NICVehicleRegNo + "');";
                                    //Hsrp_History = "INSERT INTO HSRPRecordsStaggingArea (VehicleRegNo,ChassisNo,EngineNo,OrderTypee,VhicleType,OwnerName,Manufacturer,HSRPRecord_AuthorizationNo,HSRP_StateID,RTOLocationID,ISFrontPlateSize,ISRearPlateSize,VehicleClass,FileName,NICVehicleType,NICVehicleRegNo) values (" + sbRows.ToString() + ",'N','N','Non-Transport','" + filename + "','" + NICVehicleType + "','" + NICVehicleRegNo + "');";
                                }

                                Utils.ExecNonQuery(Hsrp_History, CnnString);
                                countInsert = countInsert + 1;
                            }
                            else
                            {
                                countDuplicate = countDuplicate + 1;
                            }
                            sbColumns.Clear();
                            sbRows.Clear();
                        }

                        Utils.ExecNonQuery("UPDATE HSRPRecordsStaggingArea set VehicleRegNo=replace(VehicleRegNo,' ','') where HSRP_StateID='" + dropDownListOrg.SelectedValue + "' and RTOLocationID='" + dropDownListClient.SelectedValue + "' ", CnnString);
                        Utils.ExecNonQuery("Insert INTO UploadFileLog (UploadedBy,FileName) VALUES ('" + UID + "','" + filename + "');", CnnString);

                        llbFileName.Text = " File Name : " + filename;
                        llbMSGSuccess.Text = " Total Inserted Records is : " + countInsert;
                        llbMSGError.Text = " And Duplicate Records is Found: " + countDuplicate;
                    }
                    

                    if (Fields.Length == 13)
                    {

                        int Cols = Fields.GetLength(0);
                        DataTable dt = new DataTable();
                        //1st row must be column names; force lower case to ensure matching later on.
                        for (int i = 0; i < Cols; i++)
                            dt.Columns.Add(Fields[i].ToLower(), typeof(string));
                        DataRow Row;
                        for (int i = 0; i < Lines.GetLength(0)-1; i++)
                        {
                            Fields = Lines[i].Split(new char[] { ';' });
                            Row = dt.NewRow();
                            for (int f = 0; f < Cols-1; f++)
                                Row[f] = Fields[f];
                            dt.Rows.Add(Row);
                        }

                        StringBuilder sbColumns = new StringBuilder();
                        StringBuilder sbRows = new StringBuilder();
                        string VehicleReg = string.Empty;
                        string HSRP_Authrization_no = string.Empty;
                        string NICVehicleType = string.Empty;
                        string NICVehicleRegNo = string.Empty;
                        string StrOwnerName = string.Empty;
                        string StrDealerName = string.Empty;

                        for (int i = 0; i <= dt.Rows.Count - 1; i++)
                        {

                            NICVehicleRegNo = dt.Rows[i][0].ToString();
                            NICVehicleType = dt.Rows[i][5].ToString();
                            StrOwnerName = dt.Rows[i][4].ToString().Replace("'", "`");
                            StrDealerName = dt.Rows[i][6].ToString().Replace("'", "`");
                            HSRP_Authrization_no = dt.Rows[i][7].ToString();
                            string ourvehicleType = Utils.getScalarValue("select ourvalue from Mapping_Vahan_HSRP where vahanvalue='" + NICVehicleType + "'", CnnString);
                            //sbRows.Replace(NICVehicleType, ourvehicleType);
                            sbRows.Append("'" + dt.Rows[i][0].ToString() + "','" + dt.Rows[i][1].ToString() + "','" + dt.Rows[i][2].ToString() + "','" + dt.Rows[i][3].ToString().Trim() + "','" + ourvehicleType + "','" + StrOwnerName + "','" + StrDealerName + "','" + dt.Rows[i][7].ToString() + "','" + dt.Rows[i][8].ToString() + "','" + dt.Rows[i][9].ToString() + "','" + dt.Rows[i][10].ToString() + "','" + dt.Rows[i][11].ToString() + "','" + dropDownListOrg.SelectedValue + "','" + dropDownListClient.SelectedValue + "'");
                            string chkRecord = "SELECT COUNT(*) FROM HSRPRecordsStaggingArea WHERE HSRPRecord_AuthorizationNo='" + HSRP_Authrization_no + "' and RTOLocationID='" + dropDownListClient.SelectedValue + "'";
                            int count = Utils.getScalarCount(chkRecord, CnnString);
                            if (count < 1)
                            {
                                string Hsrp_History = string.Empty;

                                if (dt.Rows[i][3].ToString().Trim() == "NB" || dt.Rows[i][3].ToString().Trim() == "OB" || dt.Rows[i][3].ToString().Trim() == "DB")
                                {
                                    Hsrp_History = "INSERT INTO HSRPRecordsStaggingArea (VehicleRegNo,ChassisNo,EngineNo,OrderType,VehicleType,OwnerName,Manufacturer,HSRPRecord_AuthorizationNo,Address1,NICDate,NICVehicleClass,NICLocationID,HSRP_StateID,RTOLocationID,ISFrontPlateSize,ISRearPlateSize,VehicleClass,FileName,NICVehicleType,NICVehicleRegNo) values (" + sbRows.ToString() + ",'Y','Y','" + dt.Rows[i][10].ToString() + "','" + filename + "','" + NICVehicleType + "','" + NICVehicleRegNo + "');";
                                }
                                else if (dt.Rows[i][3].ToString().Trim() == "DF")
                                {
                                    Hsrp_History = "INSERT INTO HSRPRecordsStaggingArea (VehicleRegNo,ChassisNo,EngineNo,OrderType,VehicleType,OwnerName,Manufacturer,HSRPRecord_AuthorizationNo,Address1,NICDate,NICVehicleClass,NICLocationID,HSRP_StateID,RTOLocationID,ISFrontPlateSize,ISRearPlateSize,VehicleClass,FileName,NICVehicleType,NICVehicleRegNo) values (" + sbRows.ToString() + ",'Y','N','" + dt.Rows[i][10].ToString() + "','" + filename + "','" + NICVehicleType + "','" + NICVehicleRegNo + "');";
                                    //Hsrp_History = "INSERT INTO HSRPRecordsStaggingArea (VehicleRegNo,ChassisNo,EngineNo,OrderType,VehicleType,OwnerName,Manufacturer,HSRPRecord_AuthorizationNo,HSRP_StateID,RTOLocationID,ISFrontPlateSize,ISRearPlateSize,VehicleClass,FileName,NICVehicleType,NICVehicleRegNo) values (" + sbRows.ToString() + ",'Y','N','Non-Transport','" + filename + "','" + NICVehicleType + "','" + NICVehicleRegNo + "');";
                                }
                                else if (dt.Rows[i][3].ToString().Trim() == "DR")
                                {
                                    Hsrp_History = "INSERT INTO HSRPRecordsStaggingArea (VehicleRegNo,ChassisNo,EngineNo,OrderType,VehicleType,OwnerName,Manufacturer,HSRPRecord_AuthorizationNo,Address1,NICDate,NICVehicleClass,NICLocationID,HSRP_StateID,RTOLocationID,ISFrontPlateSize,ISRearPlateSize,VehicleClass,FileName,NICVehicleType,NICVehicleRegNo) values (" + sbRows.ToString() + ",'N','Y','" + dt.Rows[i][10].ToString() + "','" + filename + "','" + NICVehicleType + "','" + NICVehicleRegNo + "');";
                                    //Hsrp_History = "INSERT INTO HSRPRecordsStaggingArea (VehicleRegNo,ChassisNo,EngineNo,OrderType,VehicleType,OwnerName,Manufacturer,HSRPRecord_AuthorizationNo,HSRP_StateID,RTOLocationID,ISFrontPlateSize,ISRearPlateSize,VehicleClass,FileName,NICVehicleType,NICVehicleRegNo) values (" + sbRows.ToString() + ",'N','Y','Non-Transport','" + filename + "','" + NICVehicleType + "','" + NICVehicleRegNo + "');";
                                }
                                else if (dt.Rows[i][3].ToString().Trim() == "OS")
                                {
                                    Hsrp_History = "INSERT INTO HSRPRecordsStaggingArea (VehicleRegNo,ChassisNo,EngineNo,OrderType,VehicleType,OwnerName,Manufacturer,HSRPRecord_AuthorizationNo,Address1,NICDate,NICVehicleClass,NICLocationID,HSRP_StateID,RTOLocationID,ISFrontPlateSize,ISRearPlateSize,VehicleClass,FileName,NICVehicleType,NICVehicleRegNo) values (" + sbRows.ToString() + ",'N','N','" + dt.Rows[i][10].ToString() + "','" + filename + "','" + NICVehicleType + "','" + NICVehicleRegNo + "');";
                                    //Hsrp_History = "INSERT INTO HSRPRecordsStaggingArea (VehicleRegNo,ChassisNo,EngineNo,OrderType,VehicleType,OwnerName,Manufacturer,HSRPRecord_AuthorizationNo,HSRP_StateID,RTOLocationID,ISFrontPlateSize,ISRearPlateSize,VehicleClass,FileName,NICVehicleType,NICVehicleRegNo) values (" + sbRows.ToString() + ",'N','N','Non-Transport','" + filename + "','" + NICVehicleType + "','" + NICVehicleRegNo + "');";
                                }

                                Utils.ExecNonQuery(Hsrp_History, CnnString);
                                countInsert = countInsert + 1;
                            }
                            else
                            {
                                countDuplicate = countDuplicate + 1;
                            }
                            sbColumns.Clear();
                            sbRows.Clear();
                        }

                        Utils.ExecNonQuery("UPDATE HSRPRecordsStaggingArea set VehicleRegNo=replace(VehicleRegNo,' ','') where HSRP_StateID='" + dropDownListOrg.SelectedValue + "' and RTOLocationID='" + dropDownListClient.SelectedValue + "' ", CnnString);
                        Utils.ExecNonQuery("Insert INTO UploadFileLog (UploadedBy,FileName) VALUES ('" + UID + "','" + filename + "');", CnnString);

                        llbFileName.Text = " File Name : " + filename;
                        llbMSGSuccess.Text = " Total Inserted Records is : " + countInsert;
                        llbMSGError.Text = " And Duplicate Records is Found: " + countDuplicate;
                    }
                     
 
                    if (Fields.Length == 14)
                    {

                        int Cols = Fields.GetLength(0);
                        DataTable dt = new DataTable();
                        //1st row must be column names; force lower case to ensure matching later on.
                        for (int i = 0; i < Cols; i++)
                            dt.Columns.Add(Fields[i].ToLower(), typeof(string));
                        DataRow Row;
                        for (int i = 0; i < Lines.GetLength(0); i++)
                        {
                            Fields = Lines[i].Split(new char[] { ';' });
                            Row = dt.NewRow();
                            for (int f = 0; f < Cols; f++)
                                Row[f] = Fields[f];
                            dt.Rows.Add(Row);
                        }

                        StringBuilder sbColumns = new StringBuilder();
                        StringBuilder sbRows = new StringBuilder();
                        string VehicleReg = string.Empty;
                        string HSRP_Authrization_no = string.Empty;
                        string NICVehicleType = string.Empty;
                        string NICVehicleRegNo = string.Empty;
                        string StrOwnerName = string.Empty;
                        string StrDealerName = string.Empty;

                        for (int i = 0; i <= dt.Rows.Count - 1; i++)
                        {

                            NICVehicleRegNo = dt.Rows[i][0].ToString();
                            NICVehicleType = dt.Rows[i][5].ToString();
                            StrOwnerName = dt.Rows[i][4].ToString().Replace("'", "`");
                            StrDealerName = dt.Rows[i][6].ToString().Replace("'", "`");
                            HSRP_Authrization_no = dt.Rows[i][7].ToString();

                            string ourvehicleType = Utils.getScalarValue("select ourvalue from Mapping_Vahan_HSRP where vahanvalue='" + NICVehicleType + "'", CnnString);
                            //sbRows.Replace(NICVehicleType, ourvehicleType);
                            sbRows.Append("'" + dt.Rows[i][0].ToString() + "','" + dt.Rows[i][1].ToString() + "','" + dt.Rows[i][2].ToString() + "','" + dt.Rows[i][3].ToString().Trim() + "','" + ourvehicleType + "','" + StrOwnerName + "','" + StrDealerName + "','" + dt.Rows[i][7].ToString() + "','" + dt.Rows[i][8].ToString() + "','" + dt.Rows[i][9].ToString() + "','" + dt.Rows[i][10].ToString() + "','" + dt.Rows[i][11].ToString() + "','" + dropDownListOrg.SelectedValue + "','" + dropDownListClient.SelectedValue + "'");

                            string chkRecord = "SELECT COUNT(*) FROM HSRPRecordsStaggingArea WHERE HSRPRecord_AuthorizationNo='" + HSRP_Authrization_no + "' and RTOLocationID='" + dropDownListClient.SelectedValue + "'";
                            int count = Utils.getScalarCount(chkRecord, CnnString);
                            if (count < 1)
                            {
                                string Hsrp_History = string.Empty;

                                if (dt.Rows[i][3].ToString().Trim() == "NB" || dt.Rows[i][3].ToString().Trim() == "OB" || dt.Rows[i][3].ToString().Trim() == "DB")
                                {
                                    Hsrp_History = "INSERT INTO HSRPRecordsStaggingArea (VehicleRegNo,ChassisNo,EngineNo,OrderType,VehicleType,OwnerName,Manufacturer,HSRPRecord_AuthorizationNo,Address1,NICDate,NICVehicleClass,NICLocationID,HSRP_StateID,RTOLocationID,ISFrontPlateSize,ISRearPlateSize,VehicleClass,FileName,NICVehicleType,NICVehicleRegNo,mobileno) values (" + sbRows.ToString() + ",'Y','Y','" + dt.Rows[i][10].ToString() + "','" + filename + "','" + NICVehicleType + "','" + NICVehicleRegNo + "','" + dt.Rows[i][12].ToString() + "');";
                                }
                                else if (dt.Rows[i][3].ToString().Trim() == "DF")
                                {
                                    Hsrp_History = "INSERT INTO HSRPRecordsStaggingArea (VehicleRegNo,ChassisNo,EngineNo,OrderType,VehicleType,OwnerName,Manufacturer,HSRPRecord_AuthorizationNo,Address1,NICDate,NICVehicleClass,NICLocationID,HSRP_StateID,RTOLocationID,ISFrontPlateSize,ISRearPlateSize,VehicleClass,FileName,NICVehicleType,NICVehicleRegNo,mobileno) values (" + sbRows.ToString() + ",'Y','N','" + dt.Rows[i][10].ToString() + "','" + filename + "','" + NICVehicleType + "','" + NICVehicleRegNo + "','" + dt.Rows[i][12].ToString() + "');";
                                    //Hsrp_History = "INSERT INTO HSRPRecordsStaggingArea (VehicleRegNo,ChassisNo,EngineNo,OrderType,VehicleType,OwnerName,Manufacturer,HSRPRecord_AuthorizationNo,HSRP_StateID,RTOLocationID,ISFrontPlateSize,ISRearPlateSize,VehicleClass,FileName,NICVehicleType,NICVehicleRegNo) values (" + sbRows.ToString() + ",'Y','N','Non-Transport','" + filename + "','" + NICVehicleType + "','" + NICVehicleRegNo + "');";
                                }
                                else if (dt.Rows[i][3].ToString().Trim() == "DR")
                                {
                                    Hsrp_History = "INSERT INTO HSRPRecordsStaggingArea (VehicleRegNo,ChassisNo,EngineNo,OrderType,VehicleType,OwnerName,Manufacturer,HSRPRecord_AuthorizationNo,Address1,NICDate,NICVehicleClass,NICLocationID,HSRP_StateID,RTOLocationID,ISFrontPlateSize,ISRearPlateSize,VehicleClass,FileName,NICVehicleType,NICVehicleRegNo,mobileno) values (" + sbRows.ToString() + ",'N','Y','" + dt.Rows[i][10].ToString() + "','" + filename + "','" + NICVehicleType + "','" + NICVehicleRegNo + "','" + dt.Rows[i][12].ToString() + "');";
                                    //Hsrp_History = "INSERT INTO HSRPRecordsStaggingArea (VehicleRegNo,ChassisNo,EngineNo,OrderType,VehicleType,OwnerName,Manufacturer,HSRPRecord_AuthorizationNo,HSRP_StateID,RTOLocationID,ISFrontPlateSize,ISRearPlateSize,VehicleClass,FileName,NICVehicleType,NICVehicleRegNo) values (" + sbRows.ToString() + ",'N','Y','Non-Transport','" + filename + "','" + NICVehicleType + "','" + NICVehicleRegNo + "');";
                                }
                                else if (dt.Rows[i][3].ToString().Trim() == "OS")
                                {
                                    Hsrp_History = "INSERT INTO HSRPRecordsStaggingArea (VehicleRegNo,ChassisNo,EngineNo,OrderType,VehicleType,OwnerName,Manufacturer,HSRPRecord_AuthorizationNo,Address1,NICDate,NICVehicleClass,NICLocationID,HSRP_StateID,RTOLocationID,ISFrontPlateSize,ISRearPlateSize,VehicleClass,FileName,NICVehicleType,NICVehicleRegNo,mobileno) values (" + sbRows.ToString() + ",'N','N','" + dt.Rows[i][10].ToString() + "','" + filename + "','" + NICVehicleType + "','" + NICVehicleRegNo + "','" + dt.Rows[i][12].ToString() + "');";
                                    //Hsrp_History = "INSERT INTO HSRPRecordsStaggingArea (VehicleRegNo,ChassisNo,EngineNo,OrderType,VehicleType,OwnerName,Manufacturer,HSRPRecord_AuthorizationNo,HSRP_StateID,RTOLocationID,ISFrontPlateSize,ISRearPlateSize,VehicleClass,FileName,NICVehicleType,NICVehicleRegNo) values (" + sbRows.ToString() + ",'N','N','Non-Transport','" + filename + "','" + NICVehicleType + "','" + NICVehicleRegNo + "');";
                                }

                                Utils.ExecNonQuery(Hsrp_History, CnnString);
                                countInsert = countInsert + 1;
                            }
                            else
                            {
                                countDuplicate = countDuplicate + 1;
                            }
                            sbColumns.Clear();
                            sbRows.Clear();
                        }

                        Utils.ExecNonQuery("UPDATE HSRPRecordsStaggingArea set VehicleRegNo=replace(VehicleRegNo,' ','') where HSRP_StateID='" + dropDownListOrg.SelectedValue + "' and RTOLocationID='" + dropDownListClient.SelectedValue + "' ", CnnString);
                        Utils.ExecNonQuery("Insert INTO UploadFileLog (UploadedBy,FileName) VALUES ('" + UID + "','" + filename + "');", CnnString);

                        llbFileName.Text = " File Name : " + filename;
                        llbMSGSuccess.Text = " Total Inserted Records is : " + countInsert;
                        llbMSGError.Text = " And Duplicate Records is Found: " + countDuplicate;
                    }





                }
                catch (Exception ex)
                {
                    Response.Write("Error: " + ex.Message);
                    //Note: Exception.Message returns detailed message that describes the current exception. 
                    //For security reasons, we do not recommend you return Exception.Message to end users in 
                    //production environments. It would be better just to put a generic error message. 
                }
                finally
                {
                    //File.Delete(SaveLocation);
                }
            }

            else if (StateID == "2")
            {
                try
                {
                    FileUpload1.PostedFile.SaveAs(SaveLocation);
                    string CSVFilePathName = SaveLocation;
                    string[] Lines = File.ReadAllLines(CSVFilePathName);
                    string[] Fields;
                    int countInsert = 0;
                    int countDuplicate = 0;
                    Fields = Lines[0].Split(new char[] { ';', '*' });
                    int Cols = Fields.GetLength(0);
                    DataTable dt = new DataTable();
                    CreateDynamicDataTable(dt, "VehicleRegNo", "ChassisNo", "EngineNo", "OrderType",
                           "VehicleType", "Manufacturername", "Manufacturemodel", "OwnerName", "Manufacturer", "HSRPRecord_AuthorizationNo");

                    DataRow Row;
                    for (int i = 0; i < Lines.GetLength(0); i++)
                    {
                        Fields = Lines[i].Split(new char[] { ';', '*' });
                        if (Fields.Length == Cols)
                        {
                            Row = dt.NewRow();
                            for (int f = 0; f < Cols; f++)
                            {
                                Row[f] = Fields[f];
                            }
                            dt.Rows.Add(Row);
                        }
                        else if (Fields.Length > 0)
                        {
                            llbMSGError.Text = "Error In This Line " + i;
                            return;
                        }
                    }

                    StringBuilder sbColumns = new StringBuilder();
                    StringBuilder sbRows = new StringBuilder();
                    string VehicleReg = string.Empty;
                    string HSRP_Authrization_no = string.Empty;
                    string NICVehicleType = string.Empty;
                    string NICVehicleRegNo = string.Empty;
                    string StrOwnerName = string.Empty;
                    string StrDealerName = string.Empty;
                    string strManufacturername = string.Empty;
                    string strManufacturemodel = string.Empty;
                    for (int i = 0; i <= dt.Rows.Count - 1; i++)
                    {
                        //string test = dt.Rows[i][0].ToString().Substring(2, 2);

                        for (int j = 0; j <= dt.Columns.Count - 1; j++)
                        {

                            if (j < dt.Columns.Count - 1)
                            {
                                //if (j == 0)
                                //{
                                //    sbRows.Append("'" + dt.Rows[i][0].ToString().Replace(" ", "") + "',");
                                //}
                                //sbRows.Append("'" + dt.Rows[i][j].ToString() + "',");
                                HSRP_Authrization_no = dt.Rows[i][9].ToString();
                            }
                            else
                            {
                                NICVehicleRegNo = dt.Rows[i][0].ToString();
                                NICVehicleType = dt.Rows[i][4].ToString().Split('*')[0];

                                StrOwnerName = dt.Rows[i][7].ToString().Replace("'", "`");

                                StrDealerName = dt.Rows[i][8].ToString().Replace("'", "`");
                                strManufacturername = dt.Rows[i][5].ToString().Replace("'", "`");
                                strManufacturemodel = dt.Rows[i][6].ToString().Replace("'", "`");

                                string ourvehicleType = string.Empty;
                                DataTable dtResult = Utils.GetDataTable("select ourvalue from Mapping_Vahan_HSRP where vahanvalue='" + NICVehicleType + "'", CnnString);
                                if (dtResult.Rows.Count > 0)
                                {
                                    ourvehicleType = dtResult.Rows[0]["ourvalue"].ToString();
                                }
                                else
                                {
                                    llbMSGError.Text = "This " + NICVehicleType + " Vehicle Type Is Invalid.";
                                    return;
                                }

                                sbRows.Append("'" + dt.Rows[i][0].ToString() + "','" + dt.Rows[i][1].ToString() + "','" + dt.Rows[i][2].ToString() + "','" + dt.Rows[i][3].ToString().Trim() + "','" + ourvehicleType + "','" + StrOwnerName + "','" + StrDealerName + "','" + dt.Rows[i][5].ToString() + "','" + dt.Rows[i][6].ToString() + "','" + dt.Rows[i][9].ToString() + "','" + dropDownListOrg.SelectedValue + "','" + dropDownListClient.SelectedValue + "'");
                            }

                        }
                        string chkRecord = "SELECT COUNT(*) FROM HSRPRecordsStaggingArea WHERE HSRPRecord_AuthorizationNo='" + HSRP_Authrization_no + "' and RTOLocationID='" + dropDownListClient.SelectedValue + "'";
                        int count = Utils.getScalarCount(chkRecord, CnnString);
                        if (count < 1)
                        {
                            string Hsrp_History = string.Empty;

                            if (dt.Rows[i][3].ToString().Trim().Trim() == "NB" || dt.Rows[i][3].ToString().Trim() == "OB" || dt.Rows[i][3].ToString().Trim() == "DB")
                            {
                                Hsrp_History = "INSERT INTO HSRPRecordsStaggingArea (VehicleRegNo,ChassisNo,EngineNo,OrderType,VehicleType,OwnerName,Manufacturer,ManufacturerName,ManufacturerModel,HSRPRecord_AuthorizationNo,HSRP_StateID,RTOLocationID,ISFrontPlateSize,ISRearPlateSize,VehicleClass,FileName,NICVehicleType,NICVehicleRegNo) values (" + sbRows.ToString() + ",'Y','Y','Non-Transport','" + filename + "','" + NICVehicleType + "','" + NICVehicleRegNo + "');";
                            }
                            else if (dt.Rows[i][3].ToString().Trim() == "DF")
                            {
                                Hsrp_History = "INSERT INTO HSRPRecordsStaggingArea (VehicleRegNo,ChassisNo,EngineNo,OrderType,VehicleType,OwnerName,Manufacturer,ManufacturerName,ManufacturerModel,HSRPRecord_AuthorizationNo,HSRP_StateID,RTOLocationID,ISFrontPlateSize,ISRearPlateSize,VehicleClass,FileName,NICVehicleType,NICVehicleRegNo) values (" + sbRows.ToString() + ",'Y','N','Non-Transport','" + filename + "','" + NICVehicleType + "','" + NICVehicleRegNo + "');";
                            }
                            else if (dt.Rows[i][3].ToString().Trim() == "DR")
                            {
                                Hsrp_History = "INSERT INTO HSRPRecordsStaggingArea (VehicleRegNo,ChassisNo,EngineNo,OrderType,VehicleType,OwnerName,Manufacturer,ManufacturerName,ManufacturerModel,HSRPRecord_AuthorizationNo,HSRP_StateID,RTOLocationID,ISFrontPlateSize,ISRearPlateSize,VehicleClass,FileName,NICVehicleType,NICVehicleRegNo) values (" + sbRows.ToString() + ",'N','Y','Non-Transport','" + filename + "','" + NICVehicleType + "','" + NICVehicleRegNo + "');";
                            }
                            else if (dt.Rows[i][3].ToString().Trim() == "OS")
                            {
                                Hsrp_History = "INSERT INTO HSRPRecordsStaggingArea (VehicleRegNo,ChassisNo,EngineNo,OrderType,VehicleType,OwnerName,Manufacturer,ManufacturerName,ManufacturerModel,HSRPRecord_AuthorizationNo,HSRP_StateID,RTOLocationID,ISFrontPlateSize,ISRearPlateSize,VehicleClass,FileName,NICVehicleType,NICVehicleRegNo) values (" + sbRows.ToString() + ",'N','N','Non-Transport','" + filename + "','" + NICVehicleType + "','" + NICVehicleRegNo + "');";
                            }

                            Utils.ExecNonQuery(Hsrp_History, CnnString);
                            countInsert = countInsert + 1;
                        }
                        else
                        {
                            countDuplicate = countDuplicate + 1;
                        }
                        sbColumns.Clear();
                        sbRows.Clear();
                    }
                    Utils.ExecNonQuery("UPDATE HSRPRecordsStaggingArea set VehicleRegNo=replace(VehicleRegNo,' ','') where HSRP_StateID='" + dropDownListOrg.SelectedValue + "' and RTOLocationID='" + dropDownListClient.SelectedValue + "' ", CnnString);
                    Utils.ExecNonQuery("Insert INTO UploadFileLog (UploadedBy,FileName) VALUES ('" + UID + "','" + filename + "');", CnnString);
                    llbFileName.Text = " File Name : " + filename;
                    llbMSGSuccess.Text = " Total Inserted Records is : " + countInsert;
                    llbMSGError.Text = " And Duplicate Records is Found: " + countDuplicate;
                }
                catch (Exception ex)
                {
                    llbMSGError.Text = "Error: " + ex.Message;
                    //Note: Exception.Message returns detailed message that describes the current exception. 
                    //For security reasons, we do not recommend you return Exception.Message to end users in 
                    //production environments. It would be better just to put a generic error message. 
                }
                finally
                {
                    //File.Delete(SaveLocation);
                }
            }
            else
            {
                try
                {
                    FileUpload1.PostedFile.SaveAs(SaveLocation);
                    string CSVFilePathName = SaveLocation;
                    string[] Lines = File.ReadAllLines(CSVFilePathName);
                    string[] Fields;
                    int countInsert = 0;
                    int countDuplicate = 0;
                    Fields = Lines[0].Split(new char[] { ';' });
                    int Cols = Fields.GetLength(0);
                    DataTable dt = new DataTable();
                     CreateDynamicDataTable(dt, "VehicleRegNo", "ChassisNo", "EngineNo", "OrderType",
                            "VehicleType","Manufacturername","Manufacturemodel", "OwnerName", "Manufacturer", "HSRPRecord_AuthorizationNo");                           

                    DataRow Row;
                    for (int i = 0; i < Lines.GetLength(0); i++)
                    {
                        Fields = Lines[i].Split(new char[] { ';' });
                        if (Fields.Length == Cols)
                        {
                            Row = dt.NewRow();
                            for (int f = 0; f < Cols; f++)
                            {
                                Row[f] = Fields[f];
                            }
                            dt.Rows.Add(Row);
                        }
                        else if (Fields.Length > 0)
                        {
                            llbMSGError.Text = "Error In This Line " + i;
                            return;
                        }
                    }

                    StringBuilder sbColumns = new StringBuilder();
                    StringBuilder sbRows = new StringBuilder();
                    string VehicleReg = string.Empty;
                    string HSRP_Authrization_no = string.Empty;
                    string NICVehicleType = string.Empty;
                    string NICVehicleRegNo = string.Empty;
                    string StrOwnerName = string.Empty;
                    string StrDealerName = string.Empty;
                    string strManufacturername = string.Empty;
                    string strManufacturemodel = string.Empty;
                    for (int i = 0; i <= dt.Rows.Count - 1; i++)
                    {
                        //string test = dt.Rows[i][0].ToString().Substring(2, 2);

                        for (int j = 0; j <= dt.Columns.Count - 1; j++)
                        {

                            if (j < dt.Columns.Count - 1)
                            {
                                //if (j == 0)
                                //{
                                //    sbRows.Append("'" + dt.Rows[i][0].ToString().Replace(" ", "") + "',");
                                //}
                                //sbRows.Append("'" + dt.Rows[i][j].ToString() + "',");
                                HSRP_Authrization_no = dt.Rows[i][9].ToString();
                            }
                            else
                            {
                                NICVehicleRegNo = dt.Rows[i][0].ToString();
                                NICVehicleType = dt.Rows[i][4].ToString().Split('*')[0];
                                
                                StrOwnerName = dt.Rows[i][7].ToString().Replace("'", "`");

                                StrDealerName = dt.Rows[i][8].ToString().Replace("'", "`");
                                strManufacturername = dt.Rows[i][5].ToString().Replace("'", "`");
                                strManufacturemodel = dt.Rows[i][6].ToString().Replace("'", "`");

                                string ourvehicleType = string.Empty;
                                DataTable dtResult = Utils.GetDataTable("select ourvalue from Mapping_Vahan_HSRP where vahanvalue='" + NICVehicleType + "'", CnnString);
                                if (dtResult.Rows.Count > 0)
                                {
                                    ourvehicleType = dtResult.Rows[0]["ourvalue"].ToString();
                                }
                                else
                                {
                                    llbMSGError.Text = "This " + NICVehicleType + " Vehicle Type Is Invalid.";
                                    return;
                                }

                                sbRows.Append("'" + dt.Rows[i][0].ToString() + "','" + dt.Rows[i][1].ToString() + "','" + dt.Rows[i][2].ToString() + "','" + dt.Rows[i][3].ToString().Trim() + "','" + ourvehicleType + "','" + StrOwnerName + "','" + StrDealerName + "','" + dt.Rows[i][5].ToString() + "','" + dt.Rows[i][6].ToString() + "','" + dt.Rows[i][9].ToString() + "','" + dropDownListOrg.SelectedValue + "','" + dropDownListClient.SelectedValue + "'");
                            }

                        }
                        string chkRecord = "SELECT COUNT(*) FROM HSRPRecordsStaggingArea WHERE HSRPRecord_AuthorizationNo='" + HSRP_Authrization_no + "' and RTOLocationID='" + dropDownListClient.SelectedValue + "'";
                        int count = Utils.getScalarCount(chkRecord, CnnString);
                        if (count < 1)
                        {
                            string Hsrp_History = string.Empty;

                            if (dt.Rows[i][3].ToString().Trim().Trim() == "NB" || dt.Rows[i][3].ToString().Trim() == "OB" || dt.Rows[i][3].ToString().Trim() == "DB")
                            {
                                Hsrp_History = "INSERT INTO HSRPRecordsStaggingArea (VehicleRegNo,ChassisNo,EngineNo,OrderType,VehicleType,OwnerName,Manufacturer,ManufacturerName,ManufacturerModel,HSRPRecord_AuthorizationNo,HSRP_StateID,RTOLocationID,ISFrontPlateSize,ISRearPlateSize,VehicleClass,FileName,NICVehicleType,NICVehicleRegNo) values (" + sbRows.ToString() + ",'Y','Y','Non-Transport','" + filename + "','" + NICVehicleType + "','" + NICVehicleRegNo + "');";
                            }
                            else if (dt.Rows[i][3].ToString().Trim() == "DF")
                            {
                                Hsrp_History = "INSERT INTO HSRPRecordsStaggingArea (VehicleRegNo,ChassisNo,EngineNo,OrderType,VehicleType,OwnerName,Manufacturer,ManufacturerName,ManufacturerModel,HSRPRecord_AuthorizationNo,HSRP_StateID,RTOLocationID,ISFrontPlateSize,ISRearPlateSize,VehicleClass,FileName,NICVehicleType,NICVehicleRegNo) values (" + sbRows.ToString() + ",'Y','N','Non-Transport','" + filename + "','" + NICVehicleType + "','" + NICVehicleRegNo + "');";
                            }
                            else if (dt.Rows[i][3].ToString().Trim() == "DR")
                            {
                                Hsrp_History = "INSERT INTO HSRPRecordsStaggingArea (VehicleRegNo,ChassisNo,EngineNo,OrderType,VehicleType,OwnerName,Manufacturer,ManufacturerName,ManufacturerModel,HSRPRecord_AuthorizationNo,HSRP_StateID,RTOLocationID,ISFrontPlateSize,ISRearPlateSize,VehicleClass,FileName,NICVehicleType,NICVehicleRegNo) values (" + sbRows.ToString() + ",'N','Y','Non-Transport','" + filename + "','" + NICVehicleType + "','" + NICVehicleRegNo + "');";
                            }
                            else if (dt.Rows[i][3].ToString().Trim() == "OS")
                            {
                                Hsrp_History = "INSERT INTO HSRPRecordsStaggingArea (VehicleRegNo,ChassisNo,EngineNo,OrderType,VehicleType,OwnerName,Manufacturer,ManufacturerName,ManufacturerModel,HSRPRecord_AuthorizationNo,HSRP_StateID,RTOLocationID,ISFrontPlateSize,ISRearPlateSize,VehicleClass,FileName,NICVehicleType,NICVehicleRegNo) values (" + sbRows.ToString() + ",'N','N','Non-Transport','" + filename + "','" + NICVehicleType + "','" + NICVehicleRegNo + "');";
                            }

                            Utils.ExecNonQuery(Hsrp_History, CnnString);
                            countInsert = countInsert + 1;
                        }
                        else
                        {
                            countDuplicate = countDuplicate + 1;
                        }
                        sbColumns.Clear();
                        sbRows.Clear();
                    }
                    Utils.ExecNonQuery("UPDATE HSRPRecordsStaggingArea set VehicleRegNo=replace(VehicleRegNo,' ','') where HSRP_StateID='" + dropDownListOrg.SelectedValue + "' and RTOLocationID='" + dropDownListClient.SelectedValue + "' ", CnnString);
                    Utils.ExecNonQuery("Insert INTO UploadFileLog (UploadedBy,FileName) VALUES ('" + UID + "','" + filename + "');", CnnString);
                    llbFileName.Text = " File Name : " + filename;
                    llbMSGSuccess.Text = " Total Inserted Records is : " + countInsert;
                    llbMSGError.Text = " And Duplicate Records is Found: " + countDuplicate;
                }
                catch (Exception ex)
                {
                    llbMSGError.Text="Error: " + ex.Message;
                    //Note: Exception.Message returns detailed message that describes the current exception. 
                    //For security reasons, we do not recommend you return Exception.Message to end users in 
                    //production environments. It would be better just to put a generic error message. 
                }
                finally
                {
                    //File.Delete(SaveLocation);
                }
            }
        }

        protected void ButImpData_Click(object sender, EventArgs e)
        {
            try
            {
                string StateID = dropDownListOrg.SelectedValue;
                if (StateID == "4")
                {

                    //string getDataQry = "SELECT  distinct  case when len(b.nicvehicleRegNo)<=9 then left(b.NicVehicleRegNo,5)+' '+(right(b.nicvehicleRegNo,4)) else b.nicvehicleRegNo end as nicvehicleRegNo,replace(b.ChassisNo,'*','') as chassisNo,b.EngineNo,b.OrderType,b.nicvehicletype,b.OwnerName,b.Manufacturer, b.hsrpRecord_AuthorizationNo,b.Address1,b.NICDate,b.NICVehicleClass,b.NICLocationID,b.mobileno, a.HSRP_Front_LaserCode,a.HSRP_Rear_LaserCode, replace (convert(varchar, a.HSRPRecord_CreationDate,105),'-','.') AS CashReceiptDateTime,convert(varchar(50), a.netamount) as netamount, (select CASE WHEN datediff(DAY, a.HSRPRecord_CreationDate, a.OrderClosedDate ) >3  THEN replace (convert(varchar, a.HSRPRecord_CreationDate+3,105),'-','.')  ELSE replace (convert(varchar, a.OrderClosedDate,105),'-','.')  END) as OrderCloseDate FROM  HSRPRecords a INNER JOIN HSRPRecordsStaggingArea b ON a.VehicleRegNo = b.VehicleRegNo where a.orderstatus='Closed' and  a.ownername is not null and a.netamount is not null and a.HSRP_Front_LaserCode is not null and a.HSRP_Rear_LaserCode is not null and a.hsrp_stateid='" + dropDownListOrg.SelectedValue + "' and a.rtolocationid = '" + dropDownListClient.SelectedValue + "' and b.Address1 is not null";

                    //DataTable dt = Utils.GetDataTable(getDataQry, CnnString);
                    //if (dt.Rows.Count > 0)
                    //{                       
                    //    llbMSGSuccess.Text = " Total Record Transfered to NIC is : " + dt.Rows.Count;
                    //    string fileName = ConfigurationManager.AppSettings["DataFolder"].ToString() + "HSRP-" + dropDownListClient.SelectedItem.ToString().Replace(" ", "") + '-' + DateTime.Now.ToString("ddmmyyyyhhmmss") + ".dat";                      
                    //    CreateCSVFile(dt, fileName);
                    //}
                    //else
                    //{
                    //    llbMSGError.Text = "Closed Records Not Found";
                    //}
                }
                else
                {
                    string getDataQry = string.Empty;
                    if (RTOLocationID == 254 || RTOLocationID == 255)
                    {
                        getDataQry = "select b.NICVehicleRegNo,b.ChassisNo,b.EngineNo,b.OrderType,b.NICVehicleType,b.OwnerName,b.NICVehicleType,b.HSRPRecord_AuthorizationNo,a.HSRP_Front_LaserCode,a.HSRP_Rear_LaserCode,CONVERT(VARCHAR(10), a.OrderClosedDate, 103) AS AffixationDate,a.TotalAmount,CONVERT(VARCHAR(10), a.OrderClosedDate, 103) AS CashReceiptDateTime from HSRPRecords a,HSRPRecordsStaggingArea b where a.VehicleRegNo=b.VehicleRegNo and b.HSRP_StateID='" + dropDownListOrg.SelectedValue + "' and b.RTOLocationID='" + dropDownListClient.SelectedValue + "' and a.OrderStatus='Closed' and a.SendRecordToNic='N' and a.ordercloseddate<=dateadd(dd,4,a.hsrprecord_creationdate)";
                    }
                    else
                    {
                        string sqlquery = "select a.Addrecordby from hsrprecords a , rtolocation b where a.rtolocationid=b.rtolocationid and a.hsrp_stateid=2 and a.rtolocationID='" + dropDownListClient.SelectedValue + "' ";//and createdby= '" + Session["UID"].ToString() + "'";
                        DataTable dtdealer = Utils.GetDataTable(sqlquery, CnnString);

                        if (dtdealer.Rows[0]["Addrecordby"].ToString().Trim() == "Dealer")
                        {

                            if (Session["UserHSRPStateID"].ToString() == "2")
                            {
                                if (Session["UserHSRPStateID"].ToString() == "2" && RTOLocationID == 138)
                                {
                                    //01/10/2015
                                    getDataQry = "select b.HSRPRecord_AuthorizationNo,b.NICVehicleRegNo,b.OrderType,a.HSRP_Front_LaserCode,a.HSRP_Rear_LaserCode,CONVERT(VARCHAR(10), a.OrderClosedDate, 105) AS AffixationDate,a.NetAmount,CONVERT(VARCHAR(10), a.hsrprecord_creationdate, 105) AS hsrprecord_creationdate from HSRPRecords a,HSRPRecordsStaggingArea b where a.VehicleRegNo=b.VehicleRegNo and  b.HSRP_StateID='2' and b.rtolocationid='" + dropDownListClient.SelectedValue.ToString() + "' and a.OrderStatus='Closed' and a.SendRecordToNic='N' and a.ordercloseddate<=dateadd(dd,4,a.hsrprecord_creationdate)";
                                   
                                    
                                }
                                else
                                {
                                    //01/10/2015
                                    getDataQry = "select b.HSRPRecord_AuthorizationNo,b.NICVehicleRegNo,b.OrderType,a.HSRP_Front_LaserCode,a.HSRP_Rear_LaserCode,CONVERT(VARCHAR(10), a.OrderClosedDate, 105) AS AffixationDate,a.NetAmount,CONVERT(VARCHAR(10), a.hsrprecord_creationdate, 105) AS hsrprecord_creationdate from HSRPRecords a,HSRPRecordsStaggingArea b where a.VehicleRegNo=b.VehicleRegNo and  b.HSRP_StateID='2' and b.rtolocationid='" + dropDownListClient.SelectedValue.ToString() + "' and a.OrderStatus='Closed' and a.SendRecordToNic='N' and a.ordercloseddate<=dateadd(dd,4,a.hsrprecord_creationdate)";
                                }

                            }

                        }
                        else
                        {
                            if (Session["UserHSRPStateID"].ToString() == "2")
                            {
                                getDataQry = "select b.HSRPRecord_AuthorizationNo,b.NICVehicleRegNo,b.OrderType,a.HSRP_Front_LaserCode,a.HSRP_Rear_LaserCode,CONVERT(VARCHAR(10), a.OrderClosedDate, 105) AS AffixationDate,a.NetAmount,CONVERT(VARCHAR(10), a.hsrprecord_creationdate, 105) AS hsrprecord_creationdate from HSRPRecords a,HSRPRecordsStaggingArea b where a.VehicleRegNo=b.VehicleRegNo and  b.HSRP_StateID='2' and b.rtolocationid='" + dropDownListClient.SelectedValue.ToString() + "' and a.OrderStatus='Closed' and a.SendRecordToNic='N' ";
                                //getDataQry = "select b.HSRPRecord_AuthorizationNo,b.NICVehicleRegNo,b.OrderType,a.HSRP_Front_LaserCode,a.HSRP_Rear_LaserCode,CONVERT(VARCHAR(10), a.OrderClosedDate, 105) AS AffixationDate,a.NetAmount,CONVERT(VARCHAR(10), a.hsrprecord_creationdate, 105) AS hsrprecord_creationdate from HSRPRecords a,HSRPRecordsStaggingArea b where a.VehicleRegNo=b.VehicleRegNo and  b.HSRP_StateID='6' and b.rtolocationid='" + dropDownListClient.SelectedValue.ToString() + "' and a.OrderStatus='Closed' and a.SendRecordToNic='N' and a.ordercloseddate<=dateadd(dd,4,a.hsrprecord_creationdate)";
                            }
                            else 
                            {
                            }

                        }
                    }
                    DataTable dt = Utils.GetDataTable(getDataQry, CnnString);
                    if (dt.Rows.Count > 0)
                    {
                        string fileName = string.Empty;

                        string updateStatus = "UPDATE HSRPRecords SET SendRecordToNic='Y' from HSRPRecords a,HSRPRecordsStaggingArea b where a.VehicleRegNo=b.VehicleRegNo and b.HSRP_StateID='" + dropDownListOrg.SelectedValue + "' and b.RTOLocationID='" + dropDownListClient.SelectedValue + "' and a.OrderStatus='Closed' and a.SendRecordToNic='N'";
                        Utils.ExecNonQuery(updateStatus, CnnString);
                        llbMSGSuccess.Text = " Total Record Transfered to NIC is : " + dt.Rows.Count;
                        if (RTOLocationID == 135)
                        {
                            fileName = ConfigurationManager.AppSettings["DataFolder"].ToString() + "HSRP-" + dropDownListClient.SelectedItem.ToString().Replace(" ", "") + '-' + DateTime.Now.ToString("ddmmyyyyhhmmss") + ".txt";
                           
                        }
                        else
                        {
                            fileName = ConfigurationManager.AppSettings["DataFolder"].ToString() + "HSRP-" + dropDownListClient.SelectedItem.ToString().Replace(" ", "") + '-' + DateTime.Now.ToString("ddmmyyyyhhmmss") + ".txt";
                        }



                        CreateCSVFile(dt, fileName);
                    }
                    else
                    {
                        llbMSGError.Text = "Closed Records Not Found";
                    }

                }
            }
            catch (Exception)
            {
                
                throw;
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

            if (Session["UserHSRPStateID"].ToString() == "4" || Session["UserHSRPStateID"].ToString() == "1")
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
            if (Session["UserHSRPStateID"].ToString() == "4" || Session["UserHSRPStateID"].ToString() == "1" || Session["UserHSRPStateID"].ToString() == "6")
            {
                if (dropDownListClient.SelectedItem.Text != "--Select RTO--")
                {
                    divLocationFileName.Visible = true;
                    SQLString = "select distinct filename from HSRPRecordsStaggingArea where hsrp_StateID='" + dropDownListOrg.SelectedValue + "' and rtolocationID='" + dropDownListClient.SelectedValue + "' and filename is not null order by filename desc";
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




        public void CreateDynamicDataTable(DataTable dtResult, params string[] dtColName)
        {
            for (int idtColCount = 0; idtColCount < dtColName.Length; idtColCount++)
            {                
                dtResult.Columns.Add(dtColName[idtColCount], typeof(string));
            }
        }
    }
}