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
    public partial class UploadLaserData : System.Web.UI.Page
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
            //SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
            //SpreadsheetInfo.FreeLimitReached +=(sender, e) => e.FreeLimitReachedAction = FreeLimitReachedAction.ContinueAsTrial;
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



        protected void btnupload_Click(object sender, EventArgs e)
        {
            llbMSGError.Text = string.Empty;
           System.Threading.Thread.Sleep(2000);
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
        

        string fileLocation = string.Empty;
        private void InsertDataInstage()
        {
            try
            {
                string filename = "LaserData-" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                string fileExtension = System.IO.Path.GetExtension(FileUpload1.PostedFile.FileName);

                string fileLocation = System.Configuration.ConfigurationManager.AppSettings["LaserFolder"].ToString();
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
                //    //CnnString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                //}
                if (fileExtension == ".xlsx")
                {
                    excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    //ExcelFile.Load(Path.ChangeExtension("Workbook","xlsx")).
                    //Save(Path.ChangeExtension("Workbook", "xls"));
                    //CnnString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                }

                excelReader.IsFirstRowAsColumnNames = true;
             
                DataSet result = excelReader.AsDataSet();

                //5. Data Reader methods
              
                //6. Free resources (IExcelDataReader is IDisposable)
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
                else { 
                llbMSGError.Text="No Data IN Excel File";
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
    
        int countDuplicate = 0, countupload = 0,errorinexcel=0;
        string tt = string.Empty;
        private void InsertionOfRecords(DataTable dt)
        {

            
            string VehicleRegno = string.Empty;
            string FrontLaserNo = string.Empty;
            string RearLaserNo = string.Empty;
            string OrderStatus = string.Empty;
            string ClosedDate = string.Empty;


            StringBuilder sb = new StringBuilder();
            foreach (DataRow dr in dt.Rows)
            {
               
                //VehicleRegno = dr["VehicleRegno"].ToString().Trim();
                FrontLaserNo = dr["FrontLaserNo"].ToString().Trim();
                RearLaserNo = dr["RearLaserNo"].ToString().Trim();
                
                OrderStatus = dr["OrderStatus"].ToString().Trim();
                ClosedDate = dr["ClosedDate"].ToString().Trim();
             
                DateTime d ;
                double dResult;
                if(DateTime.TryParseExact(ClosedDate, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out d))
                {
                }
                else if ((double.TryParse(ClosedDate, out dResult)))
                {
                    //  Double cd = Convert.ToDouble(HSRPRecord_AuthorizationDate);
              
                         d = DateTime.FromOADate(dResult);
                } 

               
                tt = dr["VehicleRegNo"].ToString().Trim();
                if (FrontLaserNo == "")
                {
                    break;
                }
                VehicleRegno = string.Empty;

                byte[] asciiBytes = Encoding.ASCII.GetBytes(tt);
                foreach (Byte b in asciiBytes)
                {
                
                    if (b >= 32 && b <= 47 || b >= 58 && b <= 64 || b >= 91 && b <= 96 || b >= 123 && b <= 126 || b >= 161 && b <= 255)
                    {

                    }
                    else
                    {
                        VehicleRegno = VehicleRegno + char.ConvertFromUtf32(b);
                    }

                }


                string SQLString1 = "select count(*) from dbo.DelhiData_Upload where Flaser='" + FrontLaserNo + "' and Rlaser='" + RearLaserNo + "'";
          
                int count = Utils.getScalarCount(SQLString1, CnnStringupload);
               
                if (count > 0)
                {
                    countDuplicate = countDuplicate + 1;
                    lbltotladuplicaterecords.Text = countDuplicate.ToString();
                }

               
                
                else
                {
                    if (OrderStatus.ToUpper() == "EMBOSSING DONE" || OrderStatus.ToUpper() == "CLOSED")
                    {
                       SQLString1 = "Insert into dbo.DelhiData_Upload(State,Rtolocation,UserID,vehicle,Flaser,Rlaser,Closeddate,Process,OrderStatus) values ('" + DropDownListStateName.SelectedValue.ToString() + "','" + dropDownListClient.SelectedValue.ToString() + "','" + strUserID + "','" + VehicleRegno + "','" + FrontLaserNo + "','" + RearLaserNo + "','" + d + "' ,'N','" + OrderStatus + "')";
                        Utils.ExecNonQuery(SQLString1, CnnStringupload);
                        countupload = countupload + 1;
                        lbltotaluploadrecords.Text = countupload.ToString();
                    }
                    else
                    {
                        errorinexcel++;
                        llbMSGError.Text = "Vehicle No" + " " + VehicleRegno + " Has Some Error in Order Type";
                    }                   
                }
            }

            string SQLString = string.Empty;
            string CnnString = string.Empty;

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
            for (int i = 1; i < ExcelSheet.Rows.Count; i++)
            {

                if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["VehicleRegNo"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has <b>VehicleRegno</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    break;
                }
                if (ExcelSheet.Rows[i]["VehicleRegNo"].ToString().Trim().Replace(" ", "").Length > 10)
                {

                    llbMSGError.Text = "Excel Sheet : Has <b>VehicleRegno:" + ExcelSheet.Rows[i]["VehicleRegNo"].ToString() + "";
                    i = i + 2;
                    llbMSGError.Text = "</b> Field more than 10 characters At Row : " + i;
                    FlagIsDirty = true;
                    break;
                }

                if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["FrontLaserNo"].ToString().Trim()))
                {

                    llbMSGError.Text = "Excel Sheet : Has <b>FrontLaserNo:" + ExcelSheet.Rows[i]["FrontLaserNo"].ToString() + "";
                    i = i + 2;
                    llbMSGError.Text = "</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    break;
                }

                if (ExcelSheet.Rows[i]["FrontLaserNo"].ToString().Trim().Replace(" ", "").Length < 11 || ExcelSheet.Rows[i]["FrontLaserNo"].ToString().Trim().Replace(" ", "").Length > 12)
                {
                    llbMSGError.Text = "Excel Sheet : Has <b>FrontLaserNo : " + ExcelSheet.Rows[i]["FrontLaserNo"].ToString() + "";
                    i = i + 2;
                    llbMSGError.Text += " </b> Field Not Valid At Row : " + i;
                    FlagIsDirty = true;
                    break;
                }

                if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["RearLaserNo"].ToString().Trim()))
                {

                    llbMSGError.Text = "Excel Sheet : Has <b>RearLaserNo:" + ExcelSheet.Rows[i]["RearLaserNo"].ToString() + "";
                    i = i + 2;
                    llbMSGError.Text = "</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    break;
                }

                if (ExcelSheet.Rows[i]["RearLaserNo"].ToString().Trim().Replace(" ", "").Length < 11 || ExcelSheet.Rows[i]["RearLaserNo"].ToString().Trim().Replace(" ", "").Length > 12)
                {

                    llbMSGError.Text = "Excel Sheet : Has <b>RearLaserNo:" + ExcelSheet.Rows[i]["RearLaserNo"].ToString() + "";
                    i = i + 2;
                    llbMSGError.Text = "</b> Field Not Valid At Row : " + i;
                    FlagIsDirty = true;
                    break;
                }
                if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["OrderStatus"].ToString().Trim()))
                {

                    llbMSGError.Text = "Excel Sheet : Has <b>OrderStatus:" + ExcelSheet.Rows[i]["OrderStatus"].ToString() + "";
                    i = i + 2;
                    llbMSGError.Text = "</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    break;
                }

                if (ExcelSheet.Rows[i]["OrderStatus"].ToString().ToUpper().Trim() != "EMBOSSING DONE" && ExcelSheet.Rows[i]["OrderStatus"].ToString().Trim().ToUpper() != "CLOSED")
                {
                   // llbMSGError.Text = "Excel Sheet : Has <b>OrderStatus:" + ExcelSheet.Rows[i]["OrderStatus"].ToString() + "";
                    i = i + 2;
                    llbMSGError.Text = "Incorrect OrderStatus At Row  : " + i;
                    FlagIsDirty = true;
                    break;
                }


                String ClosedDate = ExcelSheet.Rows[i]["ClosedDate"].ToString().Trim();                

                if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["ClosedDate"].ToString().Trim()))

                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has <b>ClosedDate</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;
                }

                DateTime result;
                double dResult;
                if (DateTime.TryParseExact(ClosedDate, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
                {

                }
                else if (double.TryParse(ClosedDate, out dResult))
                {
                    //  Double cd = Convert.ToDouble(HSRPRecord_AuthorizationDate);
                    try
                    {
                        DateTime d = DateTime.FromOADate(dResult);

                        if ((d.Month > 12) || (d.Day > 31))
                        {
                            llbMSGError.Text = "Excel Sheet : Has <b>ClosedDate:" + ExcelSheet.Rows[i]["ClosedDate"].ToString() + "";

                            i = i + 2;
                            llbMSGError.Text = "</b> Closed Date Format Wrong At Row : " + i + " It Should Be In MM/DD/yyyy format";
                            FlagIsDirty = true;
                            break;
                        }
                    }
                    catch (Exception ee)
                    {
                        llbMSGError.Text = "</b> Closed Date Format Wrong At Row : " + i + " It Should Be In MM/DD/yyyy format";
                        FlagIsDirty = true;
                        break;
                    }

                }
                else
                {
                    llbMSGError.Text = "</b> Closed Date Format Wrong At Row : " + i + " It Should Be In MM/DD/yyyy format";
                    FlagIsDirty = true;
                    break;
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

       
        }
    }
