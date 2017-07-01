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
using CarlosAg.ExcelXmlWriter;

namespace HSRP.Transaction
{
    public partial class GetHSRPData : System.Web.UI.Page
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
            //lbltotladuplicaterecords.Text = "";
            //lblVehicleRegNo.Text = "";

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
                    //btnSync.Enabled = false;
                    try
                    {
                        if (HSRP_StateID == "2")
                        {
                           // lblEmb.Visible = true;
                           // ddlEmbossingCenter.Visible = true;
                        }
                        if (UserType.Equals(0))
                        {
                        //    labelOrganization.Visible = true;
                        //    DropDownListStateName.Visible = true;
                        //   // labelClient.Visible = true;

                        //   // dropDownListClient.Visible = true;
                        //    FilldropDownListOrganization();

                        //   // FilldropDownListClient();
                        //}
                        //else
                        //{

                        //    labelOrganization.Visible = true;
                        //    DropDownListStateName.Visible = true;
                        //   // labelClient.Visible = true;

                        //   // dropDownListClient.Visible = true;
                        //    FilldropDownListOrganization();

                            //FilldropDownListClient();
                        }


                    }
                    catch (Exception err)
                    {

                    }
                }
            }
        }


        protected void Button1_Click(object sender, EventArgs e)
        {

          
            llbMSGError.Text = string.Empty;
            try
            {
                //DataTable dtExcelRecords = new DataTable();
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
                string filename = "GetHsrpData-" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
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
               // excelReader = Excel.ExcelReaderFactory.CreateBinaryReader(stream);

                if(fileExtension.Contains(".xlsx"))
                {
                    //2. Reading from a OpenXml Excel file (2007 format; *.xlsx)
                   excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                }
                else
                {
                    //1. Reading from a binary Excel file ('97-2003 format; *.xls)
                    excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                } //...
               
                excelReader.IsFirstRowAsColumnNames = true;
                DataSet result = excelReader.AsDataSet();
                DataTable dt = result.Tables[0].Copy();
               
                //while (excelReader.Read())
                //{
                //    excelReader.GetInt32(0);
                //}

                excelReader.Close();
           
               
             
                if (result.Tables[0].Rows.Count > 0 || result != null)
                {
                   // ValidationCheckOnRecords(result.Tables[0]);
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
               // lbltotladuplicaterecords.Text = countDuplicate.ToString();
                if (File.Exists(fileLocation))
                {

                    File.Delete(fileLocation);

                }
                return;
            }
        }


        int countDuplicate = 0, countupload = 0, errorinexcel = 0;
     
        private void InsertionOfRecords(DataTable dt)
        {

            string filename = "GET HSRP DATA" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
            Workbook book = new Workbook();

            // Specify which Sheet should be opened and the size of window by default
            book.ExcelWorkbook.ActiveSheetIndex = 1;
            book.ExcelWorkbook.WindowTopX = 100;
            book.ExcelWorkbook.WindowTopY = 200;
            book.ExcelWorkbook.WindowHeight = 7000;
            book.ExcelWorkbook.WindowWidth = 8000;

            // Some optional properties of the Document
            book.Properties.Author = "HSRP";
            book.Properties.Title = "GET HSRP DATA";
            book.Properties.Created = DateTime.Now;


            // Add some styles to the Workbook
            WorksheetStyle style = book.Styles.Add("HeaderStyle");
            style.Font.FontName = "Tahoma";
            style.Font.Size = 9;
            style.Font.Bold = false;
            style.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            style.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            style.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            style.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            style.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


            WorksheetStyle style8 = book.Styles.Add("HeaderStyle8");
            style8.Font.FontName = "Tahoma";
            style8.Font.Size = 10;
            style8.Font.Bold = true;
            style8.Alignment.Horizontal = StyleHorizontalAlignment.Right;
            style8.Interior.Color = "#D4CDCD";
            style8.Interior.Pattern = StyleInteriorPattern.Solid;

            WorksheetStyle style5 = book.Styles.Add("HeaderStyle5");
            style5.Font.FontName = "Tahoma";
            style5.Font.Size = 10;
            style5.Font.Bold = false;
            style5.Alignment.Horizontal = StyleHorizontalAlignment.Right;
            style5.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            style5.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            style5.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            style5.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


            WorksheetStyle style6 = book.Styles.Add("HeaderStyle6");
            style6.Font.FontName = "Tahoma";
            style6.Font.Size = 10;
            style6.Font.Bold = true;
            style6.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            style6.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            style6.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            style6.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            style6.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


            WorksheetStyle style2 = book.Styles.Add("HeaderStyle2");
            style2.Font.FontName = "Tahoma";
            style2.Font.Size = 10;
            style2.Font.Bold = true;
            style.Alignment.Horizontal = StyleHorizontalAlignment.Left;


            WorksheetStyle style3 = book.Styles.Add("HeaderStyle3");
            style3.Font.FontName = "Tahoma";
            style3.Font.Size = 12;
            style3.Font.Bold = true;
            style3.Alignment.Horizontal = StyleHorizontalAlignment.Left;

            Worksheet sheet = book.Worksheets.Add("GET HSRP DATA");
            sheet.Table.Columns.Add(new WorksheetColumn(60));
            sheet.Table.Columns.Add(new WorksheetColumn(120));
            sheet.Table.Columns.Add(new WorksheetColumn(100));
            sheet.Table.Columns.Add(new WorksheetColumn(150));

            sheet.Table.Columns.Add(new WorksheetColumn(90));
            sheet.Table.Columns.Add(new WorksheetColumn(90));
            sheet.Table.Columns.Add(new WorksheetColumn(92));
            sheet.Table.Columns.Add(new WorksheetColumn(90));
            sheet.Table.Columns.Add(new WorksheetColumn(90));
            sheet.Table.Columns.Add(new WorksheetColumn(90));


            WorksheetStyle style9 = book.Styles.Add("HeaderStyle9");
            style9.Font.FontName = "Tahoma";
            style9.Font.Size = 10;
            style9.Font.Bold = true;
            style9.Alignment.Horizontal = StyleHorizontalAlignment.Right;
            style9.Interior.Color = "#FCF6AE";
            style9.Interior.Pattern = StyleInteriorPattern.Solid;


            WorksheetRow row = sheet.Table.Rows.Add();

            row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
            row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
            row.Cells.Add(new WorksheetCell("Report :", "HeaderStyle3"));
            WorksheetCell cell = row.Cells.Add("Get HSRP Data");
            cell.MergeAcross = 3; // Merge two cells together
            cell.StyleID = "HeaderStyle3";

            row = sheet.Table.Rows.Add();
            row.Index = 3;
            //row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
            //row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
           // row.Cells.Add(new WorksheetCell("State:", "HeaderStyle2"));
            // row.Cells.Add(new WorksheetCell(DropDownListStateName.SelectedItem.ToString(), "HeaderStyle2"));

            //row = sheet.Table.Rows.Add();
            //row.Index = 4;
            //row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
            //row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
            //row.Cells.Add(new WorksheetCell("Location:", "HeaderStyle2"));
            //row.Cells.Add(new WorksheetCell(dropDownListClient.SelectedItem.ToString(), "HeaderStyle2"));

            //row = sheet.Table.Rows.Add();
            //row.Index = 4;

            //DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            //row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
            //row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
           // row.Cells.Add(new WorksheetCell("Date Generated from:", "HeaderStyle2"));
            //row.Cells.Add(new WorksheetCell(DatePrint1, "HeaderStyle2"));
            //row = sheet.Table.Rows.Add();
            //row.Index = 6;
            //row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
            //row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));

            //row.Cells.Add(new WorksheetCell("To:", "HeaderStyle2"));
            ////row.Cells.Add(new WorksheetCell(DatePrint2, "HeaderStyle2"));
            //row = sheet.Table.Rows.Add();

            //row.Index = 5;
            //row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
            //row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));


            //row = sheet.Table.Rows.Add();

            row.Index = 4;

            row.Cells.Add(new WorksheetCell("S.No", "HeaderStyle6"));
            row.Cells.Add(new WorksheetCell("hsrprecordid", "HeaderStyle6"));
            row.Cells.Add(new WorksheetCell("VehicleRegNo", "HeaderStyle6"));
            //row.Cells.Add(new WorksheetCell("Order Month", "HeaderStyle6"));
            row.Cells.Add(new WorksheetCell("OrderStatus", "HeaderStyle6"));
            row.Cells.Add(new WorksheetCell("VehicleClass", "HeaderStyle6"));
            row.Cells.Add(new WorksheetCell("vehicletype", "HeaderStyle6"));
            row.Cells.Add(new WorksheetCell("hsrp_front_lasercode", "HeaderStyle6"));
            row.Cells.Add(new WorksheetCell("HSRP_Rear_LaserCode", "HeaderStyle6"));
            row.Cells.Add(new WorksheetCell("RoundOff_NetAmount", "HeaderStyle6"));
            row.Cells.Add(new WorksheetCell("Orderdate", "HeaderStyle6"));
            row.Cells.Add(new WorksheetCell("Embossingdate", "HeaderStyle6"));
            row.Cells.Add(new WorksheetCell("OrderClosedDate", "HeaderStyle6"));
    

            row = sheet.Table.Rows.Add();
            String StringField = String.Empty;
            String StringAlert = String.Empty;

            row.Index = 5;
            string hsrprecordid = string.Empty;
            string strhsrprecordid = string.Empty;
            string strvehicleregno = string.Empty;
          

            string ArrVehicle = string.Empty;
            StringBuilder sb = new StringBuilder();
            //DataTable dt11 = new DataTable();
            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                i = i + 1;
                hsrprecordid = dr["hsrprecordid"].ToString().Trim();
                //hsrprecordid = dr["hsrprecordid"].ToString().Trim();
                if (dt.Rows.Count == i)
                {
                    sb.Append(hsrprecordid);
                }
                else
                {
                    sb.Append(hsrprecordid + ",");
                }
              }
            DataTable dt1 = new DataTable();
            
            if (sb.ToString() != "")
            {
                string sqlquery = "select hsrprecordid,VehicleRegNo,OrderStatus,VehicleClass,vehicletype,hsrp_front_lasercode,HSRP_Rear_LaserCode,RoundOff_NetAmount,convert(varchar(20),HSRPRecord_CreationDate,103) as Orderdate,convert(varchar(20),OrderEmbossingDate,103) as Embossingdate,convert(varchar(20),OrderClosedDate,103) as OrderClosedDate from hsrprecords where hsrprecordid in (" + sb + ")";
                dt1 = Utils.GetDataTable(sqlquery, CnnStringupload);
                if (dt1.Rows.Count > 0)
                {
                    int sno = 0;
                    foreach (DataRow dtrows in dt1.Rows) // Loop over the rows.
                    {
                        sno = sno + 1;
                        row.Cells.Add(new WorksheetCell(Convert.ToInt16(sno).ToString(), DataType.String, "HeaderStyle")); 
                        row.Cells.Add(new WorksheetCell(dtrows["hsrprecordid"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["VehicleRegNo"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["OrderStatus"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["VehicleClass"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["vehicletype"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["hsrp_front_lasercode"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["HSRP_Rear_LaserCode"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["RoundOff_NetAmount"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["Orderdate"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["Embossingdate"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["OrderClosedDate"].ToString(), DataType.String, "HeaderStyle"));
                        row = sheet.Table.Rows.Add();
                    }

                 
                    HttpContext context = HttpContext.Current;
                    context.Response.Clear();
                    // Save the file and open it
                    book.Save(Response.OutputStream);

                    //context.Response.ContentType = "text/csv";
                    context.Response.ContentType = "application/vnd.ms-excel";

                    context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                    context.Response.End();
                }

                //Utils.ExecNonQuery(sb.ToString(), CnnStringupload);

            }
            if (countupload > 0)
            {
                llbMSGSuccess.Text = "Record Save Sucessfully.";
               // string a = Label1.Text;
                lbltotaluploadrecords.Text = countupload.ToString();
               // lbltotladuplicaterecords.Text = countDuplicate.ToString();
               // btnSync.Enabled = true;

            }
            else
            {
             //lbltotladuplicaterecords.Text = countDuplicate.ToString();

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
    }
}
