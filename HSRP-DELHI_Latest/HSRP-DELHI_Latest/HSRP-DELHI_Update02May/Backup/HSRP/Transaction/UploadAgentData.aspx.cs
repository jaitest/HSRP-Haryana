using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.OleDb;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.IO;

namespace HSRP.Transaction
{
    public partial class UploadAgentData : System.Web.UI.Page
    {
        
        string SaveLocation = string.Empty;
        string SQLString = string.Empty;
        string CnnString = string.Empty;
        string getExcelSheetName = string.Empty;
        string Id = string.Empty;
        string HSRP_StateID = string.Empty, RTOLocationID = string.Empty;
        bool FlagIsDirty = false;
        int UserType;
        string strUserID = string.Empty;
        int intHSRPStateID;
        string CnnStringupload = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
       
        protected void Page_Load(object sender, EventArgs e)
        {

            Utils.GZipEncodePage();
            lbltotaluploadrecords.Text = "";
            lbltotladuplicaterecords.Text = "";


            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {

                UserType = Convert.ToInt32(Session["UserType"]);
                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                strUserID = Session["UID"].ToString();
                //ComputerIP = Request.UserHostAddress;

                Id = Session["UID"].ToString();
                HSRP_StateID = Session["UserHSRPStateID"].ToString();
                if (!IsPostBack)
                {
                    try
                    {
                        if (UserType.Equals(0))
                        {
                            labelOrganization.Visible = true;
                            DropDownListStateName.Visible = true;
                            labelClient.Visible = true;

                            dropDownListClient.Visible = true;
                            FilldropDownListOrganization();

                            FilldropDownListClient();
                        }
                        else
                        {

                            labelOrganization.Visible = true;
                            DropDownListStateName.Visible = true;
                            labelClient.Visible = true;

                            dropDownListClient.Visible = true;
                            FilldropDownListOrganization();

                            FilldropDownListClient();
                        }


                    }
                    catch (Exception err)
                    {

                    }
                }
            }

        }

        private void FilldropDownListOrganization()
        {

            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            if (UserType.Equals(0))
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where ActiveStatus='Y' Order by HSRPStateName";
                Utils.PopulateDropDownList(DropDownListStateName, SQLString.ToString(), CnnString, "--Select State--");
            }
            else
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID=" + HSRP_StateID + " and ActiveStatus='Y' Order by HSRPStateName";
                DataSet dts = Utils.getDataSet(SQLString, CnnString);
                DropDownListStateName.DataSource = dts;
                DropDownListStateName.DataBind();

                // DropDownListStateName.SelectedIndex = DropDownListStateName.Items.Count - 1;

            }


            //Utils.PopulateDropDownList(DropDownListStateName, SQLString.ToString(), CnnString, "--Select State--");
            //DropDownListStateName.SelectedIndex = DropDownListStateName.Items.Count - 1;
        }
        private void FilldropDownListClient()
        {
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            if (UserType.Equals(0))
            {
                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                //if (Session["UID"].ToString() == "0")
                //{

                SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + intHSRPStateID + " and ActiveStatus!='N' Order by RTOLocationName";
                Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), CnnString, "--Select Location--");
            }
            else
            {

                //SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + Session["UserHSRPStateID"].ToString() + " and ActiveStatus!='N' Order by RTOLocationName";
                SQLString = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID where  a.LocationType!='District' and UserRTOLocationMapping.UserID='" + strUserID + "' ";
                DataSet dss = Utils.getDataSet(SQLString, CnnString);
                dropDownListClient.DataSource = dss;
                dropDownListClient.DataBind();
            }
            //Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), CnnString, "--Select Client--");
            //dropDownListClient.SelectedIndex = dropDownListClient.Items.Count - 1;
        }

        protected void btnupload_Click(object sender, EventArgs e)
        {
            flush();
            llbMSGError.Text = string.Empty;
            ShowDuplicateRecords.Text = string.Empty;
            DataTable dtExcelRecords = new DataTable();
            if ((FileUpload1.PostedFile != null) && (FileUpload1.PostedFile.ContentLength > 0))
            {

                InsertDataInstage();
                btnsyn.Visible = true;
               


            }
            else
            {
                llbMSGError.Text = "Please select a file to upload.";
                llbMSGSuccess.Text = "";
            }
            
        }

        string fileLocation = string.Empty;
        private void InsertDataInstage()
        {
            //string filename = System.IO.Path.GetFileName(FileUpload1.PostedFile.FileName);
            string filename = "Agent-" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
            string fileExtension = System.IO.Path.GetExtension(FileUpload1.PostedFile.FileName);



            //fileLocation = Server.MapPath("~/DealerFolder/" + filename);

            string fileLocation = System.Configuration.ConfigurationManager.AppSettings["AgentFolder"].ToString();
            fileLocation += filename.Replace("\\\\", "\\");

            if (fileExtension != ".xls" && fileExtension != ".xlsx")
            {
                llbMSGError.Text = "";
                llbMSGError.Text = "Please UpLoad Excel File. ";
                return;
            }

            if (fileExtension == ".xls")
            {
                CnnString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
            }
            else if (fileExtension == ".xlsx")
            {
                CnnString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
            }



            FileUpload1.PostedFile.SaveAs(fileLocation);


            OleDbConnection con = new OleDbConnection(CnnString);
            OleDbCommand cmd = new OleDbCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Connection = con;
            OleDbDataAdapter dAdapter = new OleDbDataAdapter(cmd);
            DataTable dtExcelRecords = new DataTable();
            con.Open();
            DataTable dtExcelSheetName = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

            ////>>>> Loop for all sheets in Excel Sheet
            for (int i = 0; i <= dtExcelSheetName.Rows.Count; i++)
            {
                try
                {
                    getExcelSheetName = dtExcelSheetName.Rows[i]["Table_Name"].ToString();
                    if (getExcelSheetName.EndsWith("$"))
                    {

                        //cmd.CommandText = "select Distinct RegistrationNumber,LOCATION,[DEALER NAME],[DEALER CODE],AUTHDATE,VEHICLECLASS,CREATIONDATE,ORDERTYPE,[AFFIXATION CODE],RegistrationNumber,CustomerName,ADDRESS,Mobiles,[VEHICLE TYPE],[HSRP AUTHORISATION NUMBER],[Engine Number],[Chassis Number],[VEHICLE MAKS],[MODEL NAME],PRICE from [" + getExcelSheetName + "]";
                        cmd.CommandText = "select VehicleRegno,VehicleType,AgentCode,vehiclemodel,vehicleclass from [" + getExcelSheetName + "]";



                        dAdapter.SelectCommand = cmd;
                        dAdapter.Fill(dtExcelRecords);

                        DataView ExcelRecordView = new DataView();
                        ExcelRecordView = dtExcelRecords.DefaultView;
                        //ExcelRecordView.RowFilter = "Location<>''";
                        //ExcelRecordView.RowFilter = "[DealerName]<>''";
                        dtExcelRecords = ExcelRecordView.ToTable();
                        ///>>>> Validation Check On All Excel Sheets
                        ValidationCheckOnRecords(dtExcelRecords);
                        if (FlagIsDirty)
                        {
                            return;
                        }
                        InsertionOfRecords(dtExcelRecords);


                    }
                }
                catch (Exception)
                {
                    // llbMSGError.Text = string.Empty;
                    // llbMSGError.Text = "Their is some Error in Excel File.";
                    // llbMSGError.Text = errorinexcel.ToString();
                    lbltotladuplicaterecords.Text = countDuplicate.ToString();
                    return;
                }
            }

            con.Close();
            con.Dispose();
            if (File.Exists(fileLocation))
            {

                File.Delete(fileLocation);

            }
        }
        int countDuplicate = 0, countupload = 0, errorinexcel = 0;
        string tt = string.Empty;
        int NextLine = 0;
        int NextLine1 = 1;
        private void InsertionOfRecords(DataTable dt)
        {


            string VehicleRegno = string.Empty;
            string VehicleType = string.Empty;
            string AgentCode = string.Empty;
            string vehiclemodel = string.Empty;
            string vehicleclass = string.Empty;


            StringBuilder sb = new StringBuilder();

            foreach (DataRow dr in dt.Rows)
            {
                
                //VehicleRegno = dr["VehicleRegno"].ToString().Trim();
                VehicleType = dr["VehicleType"].ToString().Trim();
                AgentCode = dr["AgentCode"].ToString().Trim();
                vehiclemodel = dr["vehiclemodel"].ToString().Trim();
                vehicleclass = dr["vehicleclass"].ToString().Trim();
                tt = dr["VehicleRegno"].ToString().Trim();
                VehicleRegno = string.Empty;

                byte[] asciiBytes = Encoding.ASCII.GetBytes(tt);
                foreach (Byte b in asciiBytes)
                {
                    // int finalValue = b.ToString("");
                    if (b >= 32 && b <= 47 || b >= 58 && b <= 64 || b >= 91 && b <= 96 || b >= 123 && b <= 126 || b >= 161 && b <= 255)
                    {
                        
                    }
                    else
                    {
                        VehicleRegno = VehicleRegno + char.ConvertFromUtf32(b);
                    }

                }

                 
                string SQLString1 = "select count(*) from dbo.hsrpagentdata where vehicleregno='" + VehicleRegno.ToString() + "'";
                int count = Utils.getScalarCount(SQLString1, CnnStringupload);

                if (count > 0)
                {
                    ShowDuplicateRecords.Visible = true;
                    countDuplicate = countDuplicate + 1;
                    lbltotladuplicaterecords.Text = countDuplicate.ToString();
                    
                   
                   
                    NextLine++;
                    if (NextLine % 10 == 0)
                    {
                      
                        ShowDuplicateRecords.Text += Environment.NewLine;
                        ShowDuplicateRecords.Text += VehicleRegno.ToString() + ",";
                    }
                    else
                    {
                        ShowDuplicateRecords.Text += VehicleRegno.ToString() + ",";
                    }
                }
                else
                {
                    if ((vehicleclass.ToUpper() == "TRANSPORT" || vehicleclass.ToUpper() == "NON-TRANSPORT") && (VehicleType.ToUpper() == "SCOOTER" || VehicleType.ToUpper() == "MOTOR CYCLE" || VehicleType.ToUpper() == "LMV" || VehicleType.ToUpper() == "LMV(CLASS)" || VehicleType.ToUpper() == "THREE WHEELER" || VehicleType.ToUpper() == "MCV/HCV/TRAILERS" || VehicleType.ToUpper() == "TRACTOR"))
                    {
                        SQLString1 = "Insert into dbo.hsrpagentdata (HSRP_StateID,RTOLocationID,vehicleregno,vehicletype,agentcode,vehiclemodel,vehicleclass,Process,flag,userid) values ('" + DropDownListStateName.SelectedValue.ToString() + "','" + dropDownListClient.SelectedValue.ToString() + "','" + VehicleRegno + "','" + VehicleType + "','" + AgentCode + "','" + vehiclemodel + "','" + vehicleclass + "','N','" + strUserID + "','" + strUserID + "')";
                        Utils.ExecNonQuery(SQLString1, CnnStringupload);
                        NextLine1++;
                        if (NextLine1 % 12 == 0)
                        {
                           vehicleno1.Text += Environment.NewLine;
                           vehicleno1.Text += VehicleRegno + ",";
                        }
                        else
                        {
                            vehicleno1.Text += VehicleRegno + ",";
                        }              
                        
                        countupload = countupload + 1;
                        lbltotaluploadrecords.Text = countupload.ToString();
                    }
                    else
                    {
                        errorinexcel++;
                        llbMSGError.Text = "Vehicle No" + " " + VehicleRegno + " Has Some Error in Vehicle Type";
                    }
                }
            }           
            if (countupload > 0)
            {
                llbMSGSuccess.Text = "Record Save Sucessfully.";
                lbltotaluploadrecords.Text = countupload.ToString();
                lbltotladuplicaterecords.Text = countDuplicate.ToString();
                return;
            }
            else
            {
                lbltotladuplicaterecords.Text = countDuplicate.ToString();

            }

        }

        private void ValidationCheckOnRecords(DataTable ExcelSheet)
        {
            // DataRow dr = new DataRow();
            for (int i = 0; i < ExcelSheet.Rows.Count; i++)
            {

                if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["VehicleRegno"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : " + getExcelSheetName.Substring(0, getExcelSheetName.Length - 1) + " has <b>VehicleRegno</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;
                }
                int check_int_or_not = 0;
                
                if (!int.TryParse(ExcelSheet.Rows[i]["agentcode"].ToString().Trim().Replace(" ", ""),out check_int_or_not))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : " + getExcelSheetName.Substring(0, getExcelSheetName.Length - 1) + " has Invalid <b>Agent Code</b> it must be a numeric At Row : " + i;
                    FlagIsDirty = true;
                    return;
                }
                if (ExcelSheet.Rows[i]["VehicleRegno"].ToString().Trim().Replace(" ", "").Length > 10)
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : " + getExcelSheetName.Substring(0, getExcelSheetName.Length - 1) + " has <b>VehicleRegno</b> Field more than 10 characters At Row : " + i;
                    FlagIsDirty = true;
                    return;
                }
                if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["VehicleType"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : " + getExcelSheetName.Substring(0, getExcelSheetName.Length - 1) + " has <b>VehicleType</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;
                }

                if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["vehicleclass"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : " + getExcelSheetName.Substring(0, getExcelSheetName.Length - 1) + " has <b>vehicleclass</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;
                }
               
            }
        }

        protected void DropDownListStateName_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilldropDownListClient();
        }

        protected void dropDownListClient_SelectedIndexChanged(object sender, EventArgs e)
        {

        }        

        protected void btnsyn_Click(object sender, EventArgs e)
        {
            int NextLine = 1;
            using (var conn = new SqlConnection(CnnString))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = "InsertAgentData";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                llbMSGSuccess.Text = "Record Sync Sucessfully."; 
               
            }
            DataTable dt = Utils.GetDataTable("select process, count(Process) as count1 from hsrpagentdata  where flag = '"+strUserID+"'  group by process ", CnnString);
           for(int i=0;i<dt.Rows.Count;i++)
            {

                if (dt.Rows[i]["process"].ToString() == "A")
                {

                    Label1.Text = "Not Matched : " + dt.Rows[0]["Count1"].ToString();
                    vehicleno1.Visible = true;
                    //   Label1.Text = "Not Matched : " + dt.Rows[0]["Count1"].ToString()  ;
                }
                else if (dt.Rows[i]["process"].ToString() == "D")
                {
                    Label2.Text = "Duplicate : " + dt.Rows[1]["Count1"].ToString() + "(" + ShowDuplicateRecords.Text + ")";
                }
                else if (dt.Rows[i]["process"].ToString() == "Y")
                {
                    Label3.Text = "Uploaded : " + dt.Rows[2]["Count1"].ToString();
                }               
            }
            if(dt.Rows.Count==0)
            {
                flush();
            }
            
            Utils.ExecNonQuery("update hsrpagentdata set flag = null where Flag ='" + strUserID + "'", CnnString);
        }

        public void flush()
        {

            Label1.Text = "Not Matched : 0";
            Label2.Text = "Duplicate : 0";
            Label3.Text = "Uploaded : 0";
            vehicleno1.Text = "";
            ShowDuplicateRecords.Text = "";
        }
    }
}