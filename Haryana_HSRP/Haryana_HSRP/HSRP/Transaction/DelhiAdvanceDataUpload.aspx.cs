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
using Excel;
using ICSharpCode;
using System.Globalization;

namespace HSRP.Transaction
{
    public partial class DelhiAdvanceDataUpload : System.Web.UI.Page
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
        string strEmbID = string.Empty;
        string CnnStringupload = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();

        protected void Page_Load(object sender, EventArgs e)
        {
            //  SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
            Utils.GZipEncodePage();
            lbltotaluploadrecords.Text = "";
            lbltotladuplicaterecords.Text = "";
            lblVehicleRegNo.Text = "";

            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {

                UserType = Convert.ToInt32(Session["UserType"]);
                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                strUserID = Session["UID"].ToString();

                Id = Session["UID"].ToString();
                HSRP_StateID = Session["UserHSRPStateID"].ToString();
                if (!IsPostBack)
                {
                    btnSync.Enabled = false;
                    try
                    {
                        if (HSRP_StateID == "2")
                        {
                            lblEmb.Visible = true;
                            ddlEmbossingCenter.Visible = true;
                        }
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


            }



        }
        private void FilldropDownListClient()
        {
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            if (UserType.Equals(0))
            {
                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + intHSRPStateID + " and ActiveStatus!='N' Order by RTOLocationName";
                Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), CnnString, "--Select Location--");
            }
            else
            {


                SQLString = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID where  a.LocationType!='District' and UserRTOLocationMapping.UserID='" + strUserID + "' ";
                DataSet dss = Utils.getDataSet(SQLString, CnnString);
                dropDownListClient.DataSource = dss;
                dropDownListClient.DataBind();
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (HSRP_StateID == "2")
            {
                if (ddlEmbossingCenter.SelectedItem.ToString().Equals("--Select Embossing Center--"))
                {
                    llbMSGError0.Visible = true;
                    llbMSGError0.Text = "";
                    llbMSGError0.Text = "Please Select Embossing Center...";
                    return;
                }
            }
            llbMSGError.Text = string.Empty;
            try
            {
                DataTable dtExcelRecords = new DataTable();
                if ((FileUpload1.PostedFile != null) && (FileUpload1.PostedFile.ContentLength > 0))
                {
                    InsertDataInstage();
                }
                else
                {
                    llbMSGError.Text = "Please select a file to upload.";
                    llbMSGSuccess.Text = "";
                }
            }
            catch (Exception ex)
            {
                llbMSGError.Text = "Error in Upload File :- " + ex.Message.ToString();
                //AddLog(ex.Message.ToString());
            }
        }
        string fileLocation = string.Empty;
        private void InsertDataInstage()
        {
            try
            {
                string filename = "Dealer-" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                string fileExtension = System.IO.Path.GetExtension(FileUpload1.PostedFile.FileName);
                string fileLocation = System.Configuration.ConfigurationManager.AppSettings["DealerFolder"].ToString();
                fileLocation += filename.Replace("\\\\", "\\");



                if (fileExtension != ".xls" && fileExtension != ".xlsx")
                {
                    llbMSGError.Text = "";
                    llbMSGError.Text = "Please UpLoad Excel File. ";
                    return;
                }
                IExcelDataReader excelReader;
                DataTable dtExcelRecords = new DataTable();
                FileUpload1.PostedFile.SaveAs(fileLocation);


                FileStream stream = File.Open(fileLocation, FileMode.Open, FileAccess.Read);
                excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                //if (fileExtension == ".xls")
                //{
                //    excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                //    excelReader.IsFirstRowAsColumnNames = true;

                //    //CnnString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                //}
                //else 

                //excelReader.IsFirstRowAsColumnNames = true;
                if (fileExtension == ".xlsx")
                {
                    llbMSGError.Text = "The Excel File must be in .xls Format..Kindly Convert Your .xlsx File into .xls format";

                    excelReader.Close();
                    return;
                    // excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    // excelReader.IsFirstRowAsColumnNames = true;
                    //ExcelFile.Load(Path.ChangeExtension("Workbook","xlsx")).
                    //Save(Path.ChangeExtension("Workbook", "xls"));
                    //CnnString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                }







                excelReader.IsFirstRowAsColumnNames = true;
                DataSet result = excelReader.AsDataSet();


                excelReader.Close();
                ///>>>> Validation Check On All Excel Sheets
                if (result.Tables[0].Rows.Count > 0 || result != null)
                {
                    ValidationCheckOnRecords(result.Tables[0]);
                    if (FlagIsDirty)
                    {
                        return;
                    }
                    InsertionOfRecords(result.Tables[0]);
                }
                else
                {
                    llbMSGError.Text = "No Data IN Excel File";
                }
                if (File.Exists(fileLocation))
                {

                    File.Delete(fileLocation);

                }


            }
            catch (Exception ee)
            {
                llbMSGError.Text = ee.Message.ToString();
                lbltotladuplicaterecords.Text = countDuplicate.ToString();
                if (File.Exists(fileLocation))
                {

                    File.Delete(fileLocation);

                }
                return;
            }


        }




        int countDuplicate = 0, countupload = 0, errorinexcel = 0;
        string tt = string.Empty;
        private void InsertionOfRecords(DataTable dt)
        {

            string DealerID = string.Empty;
            string DealerName = string.Empty;
            string Dealercode = string.Empty;
            string HSRPRecord_AuthorizationDate = string.Empty;
            string HSRPRecord_CreationDate = string.Empty;
            string VehicleClass = string.Empty;
            string OrderType = string.Empty;
            string AffixationCode = string.Empty;
            string VehicleRegNo = string.Empty;
            string OwnerName = string.Empty;
            string Address = string.Empty;
            string MobileNo = string.Empty;
            string VehicleType = string.Empty;
            string HSRPRecord_AuthorizationNo = string.Empty;
            string EngineNo = string.Empty;
            string ChassisNo = string.Empty;
            string vehiclemake = string.Empty;
            string ModelName = string.Empty;
            string PRICE = string.Empty;

            string ArrVehicle = string.Empty;
            StringBuilder sb = new StringBuilder();

            foreach (DataRow dr in dt.Rows)
            {

                DealerID = dr["DealerID"].ToString().Trim();
                DealerName = dr["DealerName"].ToString().Trim();
                Dealercode = dr["Dealercode"].ToString().Trim();
                HSRPRecord_AuthorizationDate = dr["HSRPRecord_AuthorizationDate"].ToString().Trim();
                //Double cd = Convert.ToDouble(HSRPRecord_AuthorizationDate);
                //DateTime d = DateTime.FromOADate(cd);

                HSRPRecord_CreationDate = dr["HSRPRecord_CreationDate"].ToString().Trim();
                //Double hcd = Convert.ToDouble(HSRPRecord_CreationDate);
                //DateTime hd = DateTime.FromOADate(hcd);

                DateTime d;
                double dResult;
                if (DateTime.TryParseExact(HSRPRecord_AuthorizationDate, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out d))
                {
                    DateTime d1 = System.DateTime.Now;
                    TimeSpan t = d - d1;
                    if (t.TotalDays > 0)
                    {
                        d = System.DateTime.Now;
                    }
                }
                else if ((double.TryParse(HSRPRecord_AuthorizationDate, out dResult)))
                {
                    //  Double cd = Convert.ToDouble(HSRPRecord_AuthorizationDate);

                    d = DateTime.FromOADate(dResult);
                    DateTime d1 = System.DateTime.Now;
                    TimeSpan t = d - d1;
                    if (t.TotalDays > 0)
                    {
                        d = System.DateTime.Now;
                    }

                }


                DateTime hd;
                if (DateTime.TryParseExact(HSRPRecord_CreationDate, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out hd))
                {
                    DateTime d1 = System.DateTime.Now;
                    TimeSpan t = hd - d1;
                    if (t.TotalDays > 0)
                    {
                        hd = System.DateTime.Now;
                    }
                }
                else if ((double.TryParse(HSRPRecord_CreationDate, out dResult)))
                {
                    //  Double cd = Convert.ToDouble(HSRPRecord_AuthorizationDate);

                    hd = DateTime.FromOADate(dResult);
                    DateTime d1 = System.DateTime.Now;
                    TimeSpan t = hd - d1;
                    if (t.TotalDays > 0)
                    {
                        hd = System.DateTime.Now;
                    }
                }



                VehicleClass = dr["VehicleClass"].ToString().Trim();
                OrderType = dr["OrderType"].ToString().Trim();
                AffixationCode = dr["AffixationCode"].ToString().Trim();
                tt = dr["VehicleRegNo"].ToString().Trim().Replace(" ","");
                string strVehicle = "select count(*) from hsrprecords where hsrp_stateid='" + DropDownListStateName.SelectedValue.ToString() + "' and vehicleregno='" + tt + "'";
                int Iresult = Utils.getScalarCount(strVehicle, CnnStringupload);
                if (Iresult > 0)
                {
                    ArrVehicle = ArrVehicle + "|"+ tt;
                    llbMSGError.Visible = true;
                    llbMSGError.Text = "";
                    llbMSGError.Text = "Duplicate Records Found.";
                    txtDuplicateRecords.Text = ArrVehicle.ToString();
                    //return;
                }
                else 
                {
                    VehicleRegNo = tt;

                //byte[] asciiBytes = Encoding.ASCII.GetBytes(tt);
                //foreach (Byte b in asciiBytes)
                //{

                //    if (b >= 32 && b <= 47 || b >= 58 && b <= 64 || b >= 91 && b <= 96 || b >= 123 && b <= 126 || b >= 161 && b <= 255)
                //    {

                //    }
                //    else
                //    {
                //        VehicleRegNo = VehicleRegNo + char.ConvertFromUtf32(b);
                //    }

                //}



                OwnerName = dr["OwnerName"].ToString().Trim();
                Address = dr["Address"].ToString().Trim();
                MobileNo = dr["MobileNo"].ToString().Trim();
                //========================================================
                VehicleType = dr["VehicleType"].ToString().Trim();


                // NB,OB,OS,DR,DF - Non-Transport,Transport-
                //====================================================
                HSRPRecord_AuthorizationNo = dr["HSRPRecord_AuthorizationNo"].ToString().Trim();
                EngineNo = dr["EngineNo"].ToString().Trim();
                ChassisNo = dr["ChassisNo"].ToString().Trim();
                vehiclemake = dr["vehiclemake"].ToString().Trim();
                ModelName = dr["ModelName"].ToString().Trim();
                PRICE = dr["PRICE"].ToString().Trim();




                string SQLString1 = "select vehicleRegNo from dbo.Vendor_HSRPRecords where vehicleRegNo='" + VehicleRegNo.ToString() + "'";

                int count = Utils.ExecNonQuery(SQLString1, CnnStringupload);
                if (count > 0)
                {
                    countDuplicate = countDuplicate + 1;
                    lbltotladuplicaterecords.Text = countDuplicate.ToString();
                }
                else
                {

                    //if (!string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["DealerID"].ToString()))
                    //{
                    // string DealerID = "select DealerID from [" + getExcelSheetName + "]";
                    string SQLString2 = "select sno from delhi_dealermaster where sno='" + DealerID + "'";

                    DataTable dt1 = Utils.GetDataTable(SQLString2, CnnStringupload);
                    if (dt1.Rows.Count == 0)
                    {
                        SQLString2 = "select dealerid from dealermaster where dealerid='" + DealerID + "'";
                        dt1 = Utils.GetDataTable(SQLString2, CnnStringupload);
                        if (dt1.Rows.Count == 0)
                        {
                            lblVehicleRegNo.Visible = true;
                            llbMSGError0.Text = "DealerID Not Found";
                            string vehicleRegNo = VehicleRegNo;
                            Label1.Text = (int.Parse(Label1.Text) + 1).ToString();
                            if (int.Parse(Label1.Text) % 10 == 0)
                            {
                                lblVehicleRegNo.Text += vehicleRegNo.ToString() + "\n";
                                //ShowDuplicateRecords.Text += VehicleRegno.ToString() + ",";
                            }
                            else
                            {
                                lblVehicleRegNo.Text += vehicleRegNo.ToString() + ",";
                            }
                        }

                    }
                    if (HSRP_StateID == "2")
                    {
                        strEmbID = ddlEmbossingCenter.SelectedItem.ToString();
                    }
                    else
                    {
                        strEmbID = "";
                    }

                    if ((VehicleClass.ToUpper() == "TRANSPORT" || VehicleClass.ToUpper() == "NON-TRANSPORT") && (OrderType.ToUpper() == "NB" || OrderType.ToUpper() == "OB" || OrderType.ToUpper() == "DB" || OrderType.ToUpper() == "DR" || OrderType.ToUpper() == "DF" || OrderType.ToUpper() == "OS") && (VehicleType.ToUpper() == "SCOOTER" || VehicleType.ToUpper() == "MOTOR CYCLE" || VehicleType.ToUpper() == "LMV" || VehicleType.ToUpper() == "LMV(CLASS)" || VehicleType.ToUpper() == "THREE WHEELER" || VehicleType.ToUpper() == "MCV/HCV/TRAILERS" || VehicleType.ToUpper() == "TRACTOR") && dt1.Rows.Count > 0)
                    {
                        sb.Append("update HSRPRecords set EngineNo='" + EngineNo + "',ChassisNo='" + ChassisNo + "',vehiclemake='" + vehiclemake + "' where hsrp_stateid=2 and VehicleRegNo='" + tt + "' and DealerID='" + DealerID + "' and OrderStatus='New Order'");
                       

                       
                        countupload = countupload + 1;
                        lbltotaluploadrecords.Text = countupload.ToString();
                    }
                    else
                    {
                        errorinexcel++;
                        llbMSGError.Text = "Vehicle No" + " " + VehicleRegNo + " Has Some Error in Vehicle Type/Vehicle Class/Order Type";
                    }
                }

             

            }

                

        }
            if (sb.ToString() != "")
            {

                Utils.ExecNonQuery(sb.ToString(), CnnStringupload);

            }
            if (countupload > 0)
            {
                llbMSGSuccess.Text = "Record Save Sucessfully.";
                string a = Label1.Text;
                lbltotaluploadrecords.Text = countupload.ToString();
                lbltotladuplicaterecords.Text = countDuplicate.ToString();
                btnSync.Enabled = true;

            }
            else
            {


                lbltotladuplicaterecords.Text = countDuplicate.ToString();

            }
        }

        private void ValidationCheckOnRecords(DataTable ExcelSheet)
        {
            for (int i = 1; i < ExcelSheet.Rows.Count; i++)
            {



                if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["DealerName"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has <b>Dealer Name</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;
                }
                String HSRPRecord_AuthorizationDate = ExcelSheet.Rows[i]["HSRPRecord_AuthorizationDate"].ToString().Trim();

                if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["HSRPRecord_AuthorizationDate"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has <b>AUTHDATE</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;
                }

                DateTime result;
                double dResult;
                if (DateTime.TryParseExact(HSRPRecord_AuthorizationDate, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
                {

                }
                else if (double.TryParse(HSRPRecord_AuthorizationDate, out dResult))
                {
                    //  Double cd = Convert.ToDouble(HSRPRecord_AuthorizationDate);
                    try
                    {
                        DateTime d = DateTime.FromOADate(dResult);

                        if ((d.Month > 12) || (d.Day > 31))
                        {
                            llbMSGError.Text = "Excel Sheet : Has <b>AUTHDATE:" + ExcelSheet.Rows[i]["ClosedDate"].ToString() + "";

                            i = i + 2;
                            llbMSGError.Text = "</b> Authorization Date Format Wrong At Row : " + i + " It Should Be In MM/DD/yyyy format";
                            FlagIsDirty = true;
                            break;
                        }
                    }
                    catch (Exception ee)
                    {
                        llbMSGError.Text = "</b> Authorization Date Format Wrong At Row : " + i + " It Should Be In MM/DD/yyyy format";
                        FlagIsDirty = true;
                        break;
                    }

                }
                else
                {
                    llbMSGError.Text = "</b> Authorization Date Format Wrong At Row : " + i + " It Should Be In MM/DD/yyyy format";
                    FlagIsDirty = true;
                    break;
                }






                String HSRPRecord_CreationDate = ExcelSheet.Rows[i]["HSRPRecord_CreationDate"].ToString().Trim();

                if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["HSRPRecord_CreationDate"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has <b>CREATIONDATE</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;
                }

                if (DateTime.TryParseExact(HSRPRecord_CreationDate, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
                {


                }
                else if (double.TryParse(HSRPRecord_CreationDate, out dResult))
                {
                    //  Double cd = Convert.ToDouble(HSRPRecord_AuthorizationDate);
                    try
                    {
                        DateTime d = DateTime.FromOADate(dResult);

                        if ((d.Month > 12) || (d.Day > 31))
                        {
                            llbMSGError.Text = "Excel Sheet : Has <b>CREATIONDATE:" + ExcelSheet.Rows[i]["ClosedDate"].ToString() + "";

                            i = i + 2;
                            llbMSGError.Text = "</b> Creation Date Format Wrong At Row : " + i + " It Should Be In MM/DD/yyyy format";
                            FlagIsDirty = true;
                            break;
                        }
                        else
                        {

                        }
                    }
                    catch (Exception ee)
                    {
                        llbMSGError.Text = "</b> Creation Date Format Wrong At Row : " + i + " It Should Be In MM/DD/yyyy format";
                        FlagIsDirty = true;
                        break;
                    }

                }
                else
                {
                    llbMSGError.Text = "</b> Creation Date Format Wrong At Row : " + i + " It Should Be In MM/DD/yyyy format";
                    FlagIsDirty = true;
                    break;
                }


                if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["VehicleClass"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has <b>VEHICLECLASS</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;
                }

                if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["OrderType"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has <b>OrderType</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;
                }


                if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["VehicleRegNo"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has <b>VehicleRegNo</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;
                }
                if (ExcelSheet.Rows[i]["VehicleRegNo"].ToString().Trim().Replace(" ", "").Length > 10)
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has <b>VehicleRegNo</b> Field more than 10 characters At Row : " + i;
                    FlagIsDirty = true;
                    return;
                }
                //if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["OwnerName"].ToString().Trim()))
                //{
                //    i = i + 2;
                //    llbMSGError.Text = "Excel Sheet : Has <b>Owner Name</b> Field Empty At Row : " + i;
                //    FlagIsDirty = true;
                //    return;
                //}
                if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["VehicleType"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has <b>VehicleType</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;
                }
                //if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["EngineNo"].ToString().Trim()))
                //{
                //    i = i + 2;
                //    llbMSGError.Text = "Excel Sheet : Has <b>Engine No</b> Field Empty At Row : " + i;
                //    FlagIsDirty = true;
                //    return;
                //}

                //if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["ChassisNo"].ToString().Trim()))
                //{
                //    i = i + 2;
                //    llbMSGError.Text = "Excel Sheet : Has <b>Chassis No</b> Field Empty At Row : " + i;
                //    FlagIsDirty = true;
                //    return;
                //}

                string str = ExcelSheet.Rows[i]["PRICE"].ToString().Trim();
                double num;
                if (string.IsNullOrWhiteSpace(str))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has <b>Show Room Price</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;
                }
                if (!double.TryParse(str, out num))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has <b>Show Room Price</b> Field wrong price At Row : " + i;
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

        protected void btnSync_Click(object sender, EventArgs e)
        {
            using (var conn = new SqlConnection(CnnString))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();

                try
                {
                    cmd.CommandText = "DataUpload_Insert_Vendor";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                    llbMSGSuccess.Text = "Record Sync Sucessfully.";
                    string strQuery = "select vehicleregno from Vendor_HSRPRecords where process='D' and remarks='allready in system'";
                    DataTable dtVehicle = Utils.GetDataTable(strQuery, CnnString);
                    string strVeh = string.Empty;
                    if (dtVehicle.Rows.Count > 0)
                    {
                        
                        strVeh = dtVehicle.Rows[0][0].ToString();
                        for (int i = 1; i < dtVehicle.Rows.Count; i++)
                        {
                            strVeh = strVeh + "," + dtVehicle.Rows[i][0].ToString();

                        }
                        llbMSGError.Text = strVeh + " are already exist";
                        llbMSGSuccess.Text = "";
                        string strUpdateQuery = "update  Vendor_HSRPRecords set remarks='allready in system Send' where process='D' and remarks='allready in system'";
                        Utils.ExecNonQuery(strUpdateQuery, CnnString);
                    }
                }
                catch (Exception ex)
                {
                    llbMSGError.Text = "Error in Sync :- " + ex.Message.ToString();
                    //AddLog(ex.Message.ToString());
                }
            }
        }
    }
}
