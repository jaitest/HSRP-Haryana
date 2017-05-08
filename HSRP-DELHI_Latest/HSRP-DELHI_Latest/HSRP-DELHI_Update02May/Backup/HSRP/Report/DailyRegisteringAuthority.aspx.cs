using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using DataProvider;
using CarlosAg.ExcelXmlWriter;
using System.Drawing;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Configuration;
using System.IO;

namespace HSRP.Report
{
    public partial class DailyRegisteringAuthority : System.Web.UI.Page
    {
        string strPath = string.Empty;
        string strMonth = string.Empty;
        string strYear = string.Empty;
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string SQLString1 = string.Empty;
        string SQLString2 = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        int UserType;
        string HSRPStateID = string.Empty;
        int RTOLocationID;
        int intHSRPStateID;
        int intRTOLocationID;
        string OrderStatus = string.Empty;
        DateTime AuthorizationDate;
        DateTime OrderDate1;
        DataProvider.BAL bl = new DataProvider.BAL();
        protected void Page_Load(object sender, EventArgs e)
        {
            Utils.GZipEncodePage();
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {
                UserType = Convert.ToInt32(Session["UserType"]);
                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                strUserID = Session["UID"].ToString();
                ComputerIP = Request.UserHostAddress;
                HSRPStateID = Session["UserHSRPStateID"].ToString();
                
                if (!IsPostBack)
                {
                    try
                    {
                        FilldropDownListOrganization();
                    }
                    catch (Exception err)
                    {

                    }
                }
            }
        }

        #region Folder Creation
        private string SetFolder(string strRTO, string strState, string strFile)
        {
            string DateFolder = System.DateTime.Now.Day.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Year.ToString();
            strPath = "D:\\TenderReports";
            if (!Directory.Exists(strPath))
            {
                CreateFolder(DateFolder, strState, strPath);
                Directory.CreateDirectory(strPath + "\\" + strState);
                Directory.CreateDirectory(strPath + "\\" + strState + "\\" + DateFolder);
            }
            else
            {
                if (!Directory.Exists(strPath + "\\" + strState))
                {

                    Directory.CreateDirectory(strPath + "\\" + strState);
                    Directory.CreateDirectory(strPath + "\\" + strState + "\\" + DateFolder);

                }
                else
                {
                    if (!Directory.Exists(strPath + "\\" + strState + "\\" + DateFolder))
                    {

                        Directory.CreateDirectory(strPath + "\\" + strState + "\\" + DateFolder);

                    }
                    else
                    {
                        var files = Directory.GetFiles(strPath + "\\" + strState + "\\" + DateFolder, "*.*", SearchOption.AllDirectories);



                        //foreach (string file in files)
                        //{
                        //    if (file.StartsWith(strPath + "\\" + strState + "\\" + DateFolder + "\\" + strFile))
                        //    {
                        //        File.Delete(file);
                        //    }
                        //}
                    }
                }


            }
            return strPath = strPath + "\\" + strState + "\\" + DateFolder;
        }

        private static void CreateFolder(string strRTO, string strState, string strRTOLocFolderPath)
        {
            Directory.CreateDirectory(strRTOLocFolderPath);
            Directory.CreateDirectory(strRTOLocFolderPath + "\\" + strState);
            Directory.CreateDirectory(strRTOLocFolderPath + "\\" + strState + "\\" + strRTO);
        }

        private DataTable GetRtoLocation()
        {
            string sql = "select rtolocationname,rtolocationid from rtolocation where rtolocationid in (select distinct distrelation from rtolocation where hsrp_stateid='" + DropDownListStateName.SelectedValue + "') and RTOLocationID not in (148,331)";
            DataTable dtrto = Utils.GetDataTable(sql, CnnString);
            return dtrto;
        }


        #endregion

        #region DropDown

        private void FilldropDownListOrganization()
        {
            if (UserType.Equals(0))
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where   ActiveStatus='Y' Order by HSRPStateName";
            }
            else
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID='" + HSRPStateID + "' and ActiveStatus='Y' Order by HSRPStateName";               
            }
            Utils.PopulateDropDownList(DropDownListStateName, SQLString.ToString(), CnnString, "--Select State--");
        }

        protected void DropDownListStateName_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilldropDownListClient();
        }

        private void FilldropDownListClient()
        {
            SQLString = "select rtolocationname,rtolocationid from rtolocation  where hsrp_stateid='" + DropDownListStateName.SelectedValue + "' and rtolocationid in (select distinct distrelation from rtolocation where  hsrp_stateid='" + DropDownListStateName.SelectedValue + "') Order by rtolocationname";
            Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), CnnString, "--Select RTO--");
        }


        #endregion

        string FromDate, ToDate;
        DataSet ds = new DataSet();

        string mm, dd, yy;

        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {

            try
            {
                Date();


                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                //int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);

                DataTable dt = new DataTable();


                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                //int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);

                string filename = "Schedule-E-MonthlyStatementFromConcessionaireToRegisteringAuthority-MonthWise-"+dropDownListClient.SelectedItem.Text+"-" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                Workbook book = new Workbook();

                // Specify which Sheet should be opened and the size of window by default
                book.ExcelWorkbook.ActiveSheetIndex = 1;
                book.ExcelWorkbook.WindowTopX = 100;
                book.ExcelWorkbook.WindowTopY = 200;
                book.ExcelWorkbook.WindowHeight = 7000;
                book.ExcelWorkbook.WindowWidth = 8000;

                // Some optional properties of the Document
                book.Properties.Author = "HSRP";
                book.Properties.Title = "Monthly Statement Form Concessionaire To Registering Authority";
                book.Properties.Created = DateTime.Now;

                // Add some styles to the Workbook
                WorksheetStyle style = book.Styles.Add("HeaderStyle");
                style.Font.FontName = "Tahoma";
                style.Font.Size = 10;
                style.Font.Bold = true;
                style.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                style.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                style.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                style.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                style.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);

                WorksheetStyle style4 = book.Styles.Add("HeaderStyle1");
                style4.Font.FontName = "Tahoma";
                style4.Font.Size = 10;
                style4.Font.Bold = false;
                style4.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                style4.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                style4.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                style4.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                style4.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


                WorksheetStyle style15 = book.Styles.Add("HeaderStyle15");
                style15.Font.FontName = "Tahoma";
                style15.Font.Size = 10;
                style15.Font.Bold = false;
                style15.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                style15.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                style15.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                style15.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                style15.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


                WorksheetStyle style16 = book.Styles.Add("HeaderStyle16");
                style16.Font.FontName = "Tahoma";
                style16.Font.Size = 10;
                style16.Font.Bold = false;
                style16.Alignment.Horizontal = StyleHorizontalAlignment.Right;
                style16.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                style16.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                style16.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                style16.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


                WorksheetStyle style8 = book.Styles.Add("HeaderStyle8");
                style8.Font.FontName = "Tahoma";
                style8.Font.Size = 10;
                style8.Font.Bold = false;
                style8.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                style8.Interior.Color = "#D4CDCD";
                style8.Interior.Pattern = StyleInteriorPattern.Solid;

                WorksheetStyle style5 = book.Styles.Add("HeaderStyle5");
                style5.Font.FontName = "Tahoma";
                style5.Font.Size = 10;
                style5.Font.Bold = true;
                style5.Alignment.Horizontal = StyleHorizontalAlignment.Right;
                style5.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                style5.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                style5.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                style5.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


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

                Worksheet sheet = book.Worksheets.Add("Monthly Statement From Concessionaire To Registering Authority");
                sheet.Table.Columns.Add(new WorksheetColumn(60));
                sheet.Table.Columns.Add(new WorksheetColumn(205));
                sheet.Table.Columns.Add(new WorksheetColumn(100));
                sheet.Table.Columns.Add(new WorksheetColumn(130));

                sheet.Table.Columns.Add(new WorksheetColumn(100));
                sheet.Table.Columns.Add(new WorksheetColumn(120));
                sheet.Table.Columns.Add(new WorksheetColumn(112));
                sheet.Table.Columns.Add(new WorksheetColumn(109));
                sheet.Table.Columns.Add(new WorksheetColumn(105));
                sheet.Table.Columns.Add(new WorksheetColumn(160));

                WorksheetRow row = sheet.Table.Rows.Add();

                row.Index = 2;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                WorksheetCell cellA = row.Cells.Add("SCHEDULE-E");
                cellA.MergeAcross = 3; // Merge two cells togetherto 
                cellA.StyleID = "HeaderStyle3";

                row = sheet.Table.Rows.Add();


                row.Index = 4;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                WorksheetCell cell = row.Cells.Add("Monthly Statement From Concessionaire To Registering Authority");
                cell.MergeAcross = 3; // Merge two cells togetherto 
                cell.StyleID = "HeaderStyle3";

                row = sheet.Table.Rows.Add();


                row.Index = 5;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("State:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(DropDownListStateName.SelectedItem.Text, "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("RTO Location:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(dropDownListClient.SelectedItem.Text, "HeaderStyle2"));

                row = sheet.Table.Rows.Add();
                // Skip one row, and add some text
                row.Index = 6;

                DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("Generated Datetime:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(DateTime.Now.ToString(), "HeaderStyle2"));
                row = sheet.Table.Rows.Add();
                row.Index = 7;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("Report Month", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(ddlMonth.SelectedItem.ToString() + "-" + ddlYear.SelectedValue.ToString(), "HeaderStyle2"));
                row = sheet.Table.Rows.Add();



               // row.Index = 7; 


                row.Index = 9;
                //row.Cells.Add("Order Date");
                row.Cells.Add(new WorksheetCell("Sr.No.", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Category", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("No. of New Vehicle Affixed", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Price Charged", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("No. of Existing Vehicles Affixed", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Price Charged", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Replacements", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Price Charged", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Total Amount", "HeaderStyle")); 
                row = sheet.Table.Rows.Add();

                String StringField = String.Empty;
                String StringAlert = String.Empty;

                //row.Index = 9;

                UserType = Convert.ToInt32(Session["UserType"]);


                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                // ds= bl.dataEntryreport(FromDate, DropDownListStateName.SelectedValue);
                //SQLString = "Report_Monthly_Statement_Registering_Authority'" + FromDate + "','" + ToDate + "','"+intHSRPStateID+"','0'";
                SQLString = "Report_Monthly_StatementLocationWiseRegisteringAuthority'" + FromDate + "','" + ToDate + "','" + intHSRPStateID + "','" + dropDownListClient.SelectedValue + "'";
                DataTable ds = Utils.GetDataTable(SQLString, CnnString);


                //SQLString = "select  (select RTOLocationName  from RTOLocation where RTOLocationID=a.RTOLocationID group by RTOLocationName) as location ,(select UserFirstName from Users where UserID =CreatedBy and RTOLocationID=a.RTOLocationID) as username,COUNT(CreatedBy) as newentery , Count(OrderEmbossingDate) as Embossing ,Count(OrderClosedDate) as orderClose from HSRPRecords a where (HSRP_StateID ='" + DropDownListStateName.SelectedValue + "' and (CreatedBy is not null or CreatedBy !='') and HSRPRecord_CreationDate between '" + FromDate + "' and '" + ToDate + "') or (HSRP_StateID ='" + DropDownListStateName.SelectedValue + "' and (CreatedBy is not null or CreatedBy !='') and OrderEmbossingDate between '" + FromDate + "' and '" + ToDate + "'  ) or (HSRP_StateID ='" + DropDownListStateName.SelectedValue + "' and (CreatedBy is not null or CreatedBy !='') and OrderClosedDate between '" + FromDate + "' and '" + ToDate + "'  ) group by  a.RTOLocationID  ,CreatedBy order by (select RTOLocationName  from RTOLocation where RTOLocationID=a.RTOLocationID group by RTOLocationName)";
                // dt = Utils.GetDataTable(SQLString, CnnString);


                string RTOColName = string.Empty;
                int sno = 0; 

                if (ds.Rows.Count > 0)
                {

                    Int32 totalNewscnaplock = 0;
                    Int32 totaloldscnaplock = 0;
                    Int32 totalReplacementScnaplock = 0;

                    Decimal totalPriceNewscnaplock = 0;

                    Decimal totalPriceOldcnaplock = 0;

                    Decimal totalPriceReplacementcnaplock = 0;

                    Int32 Total = 0;

                    Int32 totalNewPrice = 0;
                    Int32 totalOldPrice = 0;
                    Int32 ReplacementPrice = 0;

                    foreach (DataRow dtrows in ds.Rows) // Loop over the rows.
                    {
                        sno = sno + 1;
                        string Type = dtrows["vehicleandplatetype"].ToString();
                        row = sheet.Table.Rows.Add();
                        

                        totalNewPrice = Convert.ToInt32( dtrows["newvehiclecash"].ToString());
                        totalOldPrice = Convert.ToInt32(dtrows["oldvehiclecash"].ToString());
                        ReplacementPrice = Convert.ToInt32(dtrows["replaceveihclecash"].ToString());

                        row.Cells.Add(new WorksheetCell(sno.ToString(), DataType.String, "HeaderStyle1"));

                        Total = totalNewPrice + totalOldPrice + ReplacementPrice;

                         
                        if (Type == "2W")
                        { 
                            row.Cells.Add(new WorksheetCell("Complete Set of Registration Plates Inclusive of Snap Lock and fixing for 2 Wheelers- Schooters, Motorcycles, Mopeds", DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell(dtrows["newvehicleorder"].ToString(), DataType.String, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["newvehiclecash"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["oldvehicleorder"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["oldvehiclecash"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["replacevehicle"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["replaceveihclecash"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(Total.ToString(), DataType.Number, "HeaderStyle16"));
                        }
                        if (Type == "3W")
                        { 
                            row.Cells.Add(new WorksheetCell("Complete Set of Registration Plates Inclusive of Snap Lock, 3rd Registration Plate and fixing for 3 wheelers and invalid carriages.", DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell(dtrows["newvehicleorder"].ToString(), DataType.String, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["newvehiclecash"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["oldvehicleorder"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["oldvehiclecash"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["replacevehicle"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["replaceveihclecash"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(Total.ToString(), DataType.Number, "HeaderStyle16"));
                        }
                        if (Type == "4W")
                        { 
                            row.Cells.Add(new WorksheetCell("Complete Set of Registration Plates Inclusive of Snap Lock 3rd Registration Plate and fixing for Light Motor Vehicles/Passenger Cars, Mopeds", DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell(dtrows["newvehicleorder"].ToString(), DataType.String, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["newvehiclecash"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["oldvehicleorder"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["oldvehiclecash"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["replacevehicle"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["replaceveihclecash"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(Total.ToString(), DataType.Number, "HeaderStyle16"));
                        }
                        if (Type == "4HW")
                        { 
                            row.Cells.Add(new WorksheetCell("Complete Set of Registration Plates Inclusive of Snap Lock 3rd Registration Plate and fixing for Medium Commercial Vehicles/Havy Commercial Vehicles/Trailer/combination/others.", DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell(dtrows["newvehicleorder"].ToString(), DataType.String, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["newvehiclecash"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["oldvehicleorder"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["oldvehiclecash"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["replacevehicle"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["replaceveihclecash"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(Total.ToString(), DataType.Number, "HeaderStyle16"));
                        }
                        if (Type == "500X120")
                        { 
                            row.Cells.Add(new WorksheetCell("Registration Plate Size 500X100 mm", DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell(dtrows["newvehicleorder"].ToString(), DataType.String, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["newvehiclecash"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["oldvehicleorder"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["oldvehiclecash"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["replacevehicle"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["replaceveihclecash"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(Total.ToString(), DataType.Number, "HeaderStyle16"));

                            totalNewscnaplock = +totalNewscnaplock+ Convert.ToInt32(dtrows["newvehicleorder"]);
                            totaloldscnaplock = +totaloldscnaplock+Convert.ToInt32(dtrows["oldvehicleorder"]);
                            totalReplacementScnaplock = + totalReplacementScnaplock+ Convert.ToInt32(dtrows["replacevehicle"]);
                             
                        }
                        if (Type == "340X200")
                        { 
                            row.Cells.Add(new WorksheetCell("Registration Plate Size 300X200 mm", DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell(dtrows["newvehicleorder"].ToString(), DataType.String, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["newvehiclecash"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["oldvehicleorder"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["oldvehiclecash"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["replacevehicle"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["replaceveihclecash"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(Total.ToString(), DataType.Number, "HeaderStyle16"));

                            totalNewscnaplock = + totalNewscnaplock + Convert.ToInt32(dtrows["newvehicleorder"]);
                            totaloldscnaplock = totaloldscnaplock+Convert.ToInt32(dtrows["oldvehicleorder"]);
                            totalReplacementScnaplock = +totalReplacementScnaplock+Convert.ToInt32(dtrows["replacevehicle"]);
                        }
                        if (Type == "200X100")
                        {
                            row.Cells.Add(new WorksheetCell("Registration Plate Size 200X100 mm", DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell(dtrows["newvehicleorder"].ToString(), DataType.String, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["newvehiclecash"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["oldvehicleorder"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["oldvehiclecash"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["replacevehicle"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["replaceveihclecash"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(Total.ToString(), DataType.Number, "HeaderStyle16"));
                            
                            totalNewscnaplock = + totalNewscnaplock + Convert.ToInt32(dtrows["newvehicleorder"]);
                            totaloldscnaplock = totaloldscnaplock + Convert.ToInt32(dtrows["oldvehicleorder"]);
                            totalReplacementScnaplock = + totalReplacementScnaplock + Convert.ToInt32(dtrows["replacevehicle"]);
                        }
                        if (Type == "285X45")
                        { 
                            row.Cells.Add(new WorksheetCell("Registration Plate Size 285X45 mm", DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell(dtrows["newvehicleorder"].ToString(), DataType.String, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["newvehiclecash"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["oldvehicleorder"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["oldvehiclecash"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["replacevehicle"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["replaceveihclecash"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(Total.ToString(), DataType.Number, "HeaderStyle16"));

                            totalNewscnaplock = + totalNewscnaplock + Convert.ToInt32(dtrows["newvehicleorder"]);
                            totaloldscnaplock = + totaloldscnaplock + Convert.ToInt32(dtrows["oldvehicleorder"]);
                            totalReplacementScnaplock = +totalReplacementScnaplock + Convert.ToInt32(dtrows["replacevehicle"]);
                        }
                        if (Type == "Sticker")
                        { 
                            row.Cells.Add(new WorksheetCell("3rd Registration Plate Sticker Inclusive of Printing", DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell(dtrows["newvehicleorder"].ToString(), DataType.String, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["newvehiclecash"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["oldvehicleorder"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["oldvehiclecash"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["replacevehicle"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(dtrows["replaceveihclecash"].ToString(), DataType.Number, "HeaderStyle16"));
                            row.Cells.Add(new WorksheetCell(Total.ToString(), DataType.Number, "HeaderStyle16"));
                            //row.Cells.Add(new WorksheetCell(Total.ToString(), DataType.Number, "HeaderStyle16"));
                        }
                    }
                    row = sheet.Table.Rows.Add();
                    decimal snapLockRate = 0;
                    //SQLString = "Report_Monthly_Statement_Registering_Authority'" + FromDate + "','" + ToDate + "','" + intHSRPStateID + "','0'";
                    SQLString = "SELECT ProductCost.Cost FROM  Product  INNER JOIN ProductCost ON Product.ProductID = ProductCost.ProductID where Product.productcode ='SNAP LOCK' and ProductCost.hsrp_stateID='" + intHSRPStateID + "'";
                    DataTable dsRate = Utils.GetDataTable(SQLString, CnnString);
                    if (dsRate.Rows.Count > 0)
                    {
                        snapLockRate = Convert.ToDecimal(dsRate.Rows[0]["cost"]);
                    }

                    sno = sno + 1;
                    row.Cells.Add(new WorksheetCell(sno.ToString(), DataType.String, "HeaderStyle1"));
                    row.Cells.Add(new WorksheetCell("Snap Locks.", DataType.String, "HeaderStyle1"));
                    totalNewscnaplock = 2 * totalNewscnaplock;
                     row.Cells.Add(new WorksheetCell(totalNewscnaplock.ToString(), DataType.Number, "HeaderStyle16"));
                    totalPriceNewscnaplock = Convert.ToInt32(totalNewscnaplock) * Convert.ToDecimal(snapLockRate);
                    totalPriceNewscnaplock = System.Decimal.Round(totalPriceNewscnaplock, 0);
                    row.Cells.Add(new WorksheetCell(totalPriceNewscnaplock.ToString(), DataType.Number, "HeaderStyle16"));
                    
                    totaloldscnaplock = 2 * totaloldscnaplock;
                    row.Cells.Add(new WorksheetCell(totaloldscnaplock.ToString(), DataType.Number, "HeaderStyle16"));
                    totalPriceOldcnaplock = totaloldscnaplock * Convert.ToDecimal(snapLockRate);
                    totalPriceOldcnaplock = System.Decimal.Round(totalPriceOldcnaplock, 0);
                    row.Cells.Add(new WorksheetCell(totalPriceOldcnaplock.ToString(), DataType.Number, "HeaderStyle16"));

                    totalReplacementScnaplock = 2 * totalReplacementScnaplock;
                    row.Cells.Add(new WorksheetCell(totalReplacementScnaplock.ToString(), DataType.Number, "HeaderStyle16"));
                    totalPriceReplacementcnaplock = totalReplacementScnaplock * Convert.ToDecimal(snapLockRate);
                    totalPriceReplacementcnaplock = System.Decimal.Round(totalPriceReplacementcnaplock, 0);
                    row.Cells.Add(new WorksheetCell(totalReplacementScnaplock.ToString(), DataType.Number, "HeaderStyle16"));

                    decimal TotalSnapLocak = totalPriceNewscnaplock + totalPriceOldcnaplock + totalPriceReplacementcnaplock;
                    row.Cells.Add(new WorksheetCell(TotalSnapLocak.ToString(), DataType.Number, "HeaderStyle16"));

                    row = sheet.Table.Rows.Add();

                    row = sheet.Table.Rows.Add();
                    HttpContext context = HttpContext.Current;
                    context.Response.Clear();
                    // Save the file and open it
                    book.Save(Response.OutputStream);

                    //context.Response.ContentType = "text/csv";
                    context.Response.ContentType = "application/vnd.ms-excel"; 
                    context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                    context.Response.End(); 
                }



            }

            catch (Exception ex)
            {
                LabelError.Text = "Error in Populating Grid :" + ex.Message.ToString();
            }
        }

        private void Date()
        {
            if (ddlMonth.SelectedValue.ToString() == "01")
            {
                mm = ddlMonth.SelectedValue.ToString();
                yy = ddlYear.SelectedValue.ToString();
                FromDate = yy + "/" + mm + "/" + "01";
                ToDate = yy + "/" + mm + "/" + "31";
            }
            else if (ddlMonth.SelectedValue.ToString() == "02")
            {
                mm = ddlMonth.SelectedValue.ToString();
                yy = ddlYear.SelectedValue.ToString();

                if (int.Parse(yy) % 4 == 0)
                {
                    FromDate = yy + "/" + mm + "/" + "01";
                    ToDate = yy + "/" + mm + "/" + "29";
                }
                else
                {
                    FromDate = yy + "/" + mm + "/" + "01";
                    ToDate = yy + "/" + mm + "/" + "28";
                }
            }
            else if (ddlMonth.SelectedValue.ToString() == "03")
            {
                mm = ddlMonth.SelectedValue.ToString();
                yy = ddlYear.SelectedValue.ToString();
                FromDate = yy + "/" + mm + "/" + "01";
                ToDate = yy + "/" + mm + "/" + "31";
            }
            else if (ddlMonth.SelectedValue.ToString() == "04")
            {
                mm = ddlMonth.SelectedValue.ToString();
                yy = ddlYear.SelectedValue.ToString();
                FromDate = yy + "/" + mm + "/" + "01";
                ToDate = yy + "/" + mm + "/" + "30";
            }
            else if (ddlMonth.SelectedValue.ToString() == "05")
            {
                mm = ddlMonth.SelectedValue.ToString();
                yy = ddlYear.SelectedValue.ToString();
                FromDate = yy + "/" + mm + "/" + "01";
                ToDate = yy + "/" + mm + "/" + "31";
            }
            else if (ddlMonth.SelectedValue.ToString() == "06")
            {
                mm = ddlMonth.SelectedValue.ToString();
                yy = ddlYear.SelectedValue.ToString();
                FromDate = yy + "/" + mm + "/" + "01";
                ToDate = yy + "/" + mm + "/" + "30";
            }
            else if (ddlMonth.SelectedValue.ToString() == "07")
            {
                mm = ddlMonth.SelectedValue.ToString();
                yy = ddlYear.SelectedValue.ToString();
                FromDate = yy + "/" + mm + "/" + "01";
                ToDate = yy + "/" + mm + "/" + "31";
            }
            else if (ddlMonth.SelectedValue.ToString() == "08")
            {
                mm = ddlMonth.SelectedValue.ToString();
                yy = ddlYear.SelectedValue.ToString();
                FromDate = yy + "/" + mm + "/" + "01";
                ToDate = yy + "/" + mm + "/" + "31";
            }
            else if (ddlMonth.SelectedValue.ToString() == "09")
            {
                mm = ddlMonth.SelectedValue.ToString();
                yy = ddlYear.SelectedValue.ToString();
                FromDate = yy + "/" + mm + "/" + "01";
                ToDate = yy + "/" + mm + "/" + "30";
            }
            else if (ddlMonth.SelectedValue.ToString() == "10")
            {
                mm = ddlMonth.SelectedValue.ToString();
                yy = ddlYear.SelectedValue.ToString();
                FromDate = yy + "/" + mm + "/" + "01";
                ToDate = yy + "/" + mm + "/" + "31";
            }
            else if (ddlMonth.SelectedValue.ToString() == "11")
            {
                mm = ddlMonth.SelectedValue.ToString();
                yy = ddlYear.SelectedValue.ToString();
                FromDate = yy + "/" + mm + "/" + "01";
                ToDate = yy + "/" + mm + "/" + "30";
            }
            else if (ddlMonth.SelectedValue.ToString() == "12")
            {
                mm = ddlMonth.SelectedValue.ToString();
                yy = ddlYear.SelectedValue.ToString();
                FromDate = yy + "/" + mm + "/" + "01";
                ToDate = yy + "/" + mm + "/" + "31";
            }
        }
        private static void GenerateCell(PdfPTable table, int iSpan, int iLeftWidth, int iRightWidth, int iTopWidth, int iBottomWidth, int iAllign, int iFont, string strText, int iRowHeight, int iRowWidth)
        {
            PdfPCell newCellPDF = null;
            BaseFont bfTimes1 = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
            if (iFont.Equals(0))
            {
                newCellPDF = new PdfPCell(new Phrase(strText, new iTextSharp.text.Font(bfTimes1, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
            }
            else if (iFont.Equals(1))
            {
                newCellPDF = new PdfPCell(new Phrase(strText, new iTextSharp.text.Font(bfTimes1, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
            }
            newCellPDF.Colspan = iSpan;
            newCellPDF.BorderWidthLeft = iLeftWidth;
            newCellPDF.BorderWidthRight = iRightWidth;
            newCellPDF.BorderWidthTop = iTopWidth;
            newCellPDF.BorderWidthBottom = iBottomWidth;
            newCellPDF.HorizontalAlignment = iAllign;
            if (!iRowHeight.Equals(0))
            {
                newCellPDF.FixedHeight = iRowHeight;
            }
            if (!iRowWidth.Equals(0))
            {
            }
            table.AddCell(newCellPDF);
        }
        protected void btnExportToExcel0_Click(object sender, EventArgs e)
        {
            Date();
            Int32 totalNewscnaplock1 = 0;
            Int32 totaloldscnaplock1 = 0;
            Int32 totalReplacementScnaplock1 = 0;

            Decimal totalPriceNewscnaplock1 = 0;

            Decimal totalPriceOldcnaplock1 = 0;

            Decimal totalPriceReplacementcnaplock1 = 0;
            string VehicleType = string.Empty;
            Int32 Total1 = 0;
            int sno = 0;
            Int32 totalNewPrice1 = 0;
            Int32 totalOldPrice1 = 0;
            Int32 ReplacementPrice1 = 0;
            int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
            string filename = "MonthlyStatementFromConcessionaireToRegisteringAuthority-MonthWise" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".pdf";
            string SQLString = String.Empty;
            Document document = new Document();
            HttpContext context = HttpContext.Current;
            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);


            string PdfFolder = ConfigurationManager.AppSettings["PdfFolder"].ToString() + filename;
            PdfWriter.GetInstance(document, new FileStream(PdfFolder, FileMode.Create));
            document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
            document.Open();
            PdfPTable table = new PdfPTable(9);
            var colWidthPercentages = new[] { 10f, 95f, 20f, 25f, 25f, 35f, 25f, 35f, 15f };
            table.SetWidths(colWidthPercentages);
            SQLString = "Report_Monthly_StatementLocationWiseRegisteringAuthority'" + FromDate + "','" + ToDate + "','" + intHSRPStateID + "','" + dropDownListClient.SelectedValue + "'";
            DataTable ds = Utils.GetDataTable(SQLString, CnnString);

            
            GenerateCell(table, 9, 0, 0, 0, 0,1 , 0, "SCHEDULE-E :  Monthly Statement From Concessionaire To Registering Authority", 15, 0);
            GenerateCell(table, 3, 0, 0, 0, 0, 0, 0, "State:      " + DropDownListStateName.SelectedItem.ToString(), 15, 0);
            GenerateCell(table, 3, 0, 0, 0, 0, 0, 0, "RTO Name:      " + dropDownListClient.SelectedItem.ToString(), 15, 0);
            GenerateCell(table, 3, 0, 0, 0, 0, 0, 0, "Month:      " + ddlMonth.SelectedItem.ToString() +","+ ddlYear.SelectedValue, 15, 0);
            GenerateCell(table, 3, 0, 0, 0, 0, 0, 0, "Report Generated Date :      " + System.DateTime.Now.ToString("dd/MM/yyyy"), 15, 0);

            GenerateCell(table, 9, 0, 0, 0, 0, 1, 0, "", 50, 0);
            GenerateCell(table, 1, 1, 1, 1, 1, 1, 0, "S.No:", 50, 0);
            GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Category", 50, 0);
            GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Number of new Vehicles Affixed", 50, 0);
            GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Price Charged", 50, 0);
            GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Number of existing Vehicles Affixed", 50, 0);
            GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Price Charged", 50, 0);
            GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Replacements", 50, 0);
            GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Price Charged", 50, 0);
            GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Total Amount", 50, 0);

            for (int i = 0; i < ds.Rows.Count; i++)
            {
                VehicleType = ds.Rows[i]["vehicleandplatetype"].ToString();
                sno = i + 1;
                totalNewPrice1 = Convert.ToInt32(ds.Rows[i]["newvehiclecash"].ToString());
                totalOldPrice1 = Convert.ToInt32(ds.Rows[i]["oldvehiclecash"].ToString());
                ReplacementPrice1 = Convert.ToInt32(ds.Rows[i]["replaceveihclecash"].ToString());
                Total1 = totalNewPrice1 + totalOldPrice1 + ReplacementPrice1;
                GenerateCell(table, 1, 1, 1, 0, 1, 1, 1, sno.ToString(), 20, 0);

                if (VehicleType == "2W")
                {
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, "Complete Set of Registration Plates Inclusive of Snap Lock and fixing for 2 Wheelers- Schooters, Motorcycles, Mopeds", 60, 0);


                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["newvehicleorder"].ToString(), 20, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["newvehiclecash"].ToString(), 20, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["oldvehicleorder"].ToString(), 20, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["oldvehiclecash"].ToString(), 20, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["replacevehicle"].ToString(), 20, 0);

                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["replaceveihclecash"].ToString(), 20, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, Total1.ToString(), 20, 0);


                }
                if (VehicleType == "3W")
                {
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, "Complete Set of Registration Plates Inclusive of Snap Lock, 3rd Registration Plate and fixing for 3 wheelers and invalid carriages.", 60, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["newvehicleorder"].ToString(), 20, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["newvehiclecash"].ToString(), 20, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["oldvehicleorder"].ToString(), 20, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["oldvehiclecash"].ToString(), 20, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["replacevehicle"].ToString(), 20, 0);

                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["replaceveihclecash"].ToString(), 20, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, Total1.ToString(), 20, 0);
                }
                if (VehicleType == "4W")
                {

                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, "Complete Set of Registration Plates Inclusive of Snap Lock 3rd Registration Plate and fixing for Light Motor Vehicles/Passenger Cars.", 60, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["newvehicleorder"].ToString(), 20, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["newvehiclecash"].ToString(), 20, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["oldvehicleorder"].ToString(), 20, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["oldvehiclecash"].ToString(), 20, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["replacevehicle"].ToString(), 20, 0);

                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["replaceveihclecash"].ToString(), 20, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, Total1.ToString(), 20, 0);


                }

                if (VehicleType == "4HW")
                {
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, "Complete Set of Registration Plates Inclusive of Snap Lock 3rd Registration Plate and fixing for Medium Commercial Vehicles/Havy Commercial Vehicles/Trailer/combination/others.", 60, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["newvehicleorder"].ToString(), 20, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["newvehiclecash"].ToString(), 20, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["oldvehicleorder"].ToString(), 20, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["oldvehiclecash"].ToString(), 20, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["replacevehicle"].ToString(), 20, 0);

                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["replaceveihclecash"].ToString(), 20, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, Total1.ToString(), 20, 0);

                }
                if (VehicleType == "500X120")
                {

                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, "Registration Plate Size 500X120 mm", 20, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["newvehicleorder"].ToString(), 20, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["newvehiclecash"].ToString(), 20, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["oldvehicleorder"].ToString(), 20, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["oldvehiclecash"].ToString(), 20, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["replacevehicle"].ToString(), 20, 0);

                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["replaceveihclecash"].ToString(), 20, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, Total1.ToString(), 20, 0);
                    totalNewscnaplock1 = +totalNewscnaplock1 + Convert.ToInt32(ds.Rows[i]["newvehicleorder"]);
                    totaloldscnaplock1 = +totaloldscnaplock1 + Convert.ToInt32(ds.Rows[i]["oldvehicleorder"]);
                    totalReplacementScnaplock1 = +totalReplacementScnaplock1 + Convert.ToInt32(ds.Rows[i]["replacevehicle"]);

                }
                if (VehicleType == "340X200")
                {
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, "Registration Plate Size 300X200 mm", 20, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["newvehicleorder"].ToString(), 20, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["newvehiclecash"].ToString(), 20, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["oldvehicleorder"].ToString(), 20, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["oldvehiclecash"].ToString(), 20, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["replacevehicle"].ToString(), 20, 0);

                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["replaceveihclecash"].ToString(), 20, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, Total1.ToString(), 20, 0);
                    totalNewscnaplock1 = +totalNewscnaplock1 + Convert.ToInt32(ds.Rows[i]["newvehicleorder"]);
                    totaloldscnaplock1 = totaloldscnaplock1 + Convert.ToInt32(ds.Rows[i]["oldvehicleorder"]);
                    totalReplacementScnaplock1 = +totalReplacementScnaplock1 + Convert.ToInt32(ds.Rows[i]["replacevehicle"]);

                }
                if (VehicleType == "200X100")
                {
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, "Registration Plate Size 200X100 mm", 20, 0);

                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["newvehicleorder"].ToString(), 20, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["newvehiclecash"].ToString(), 20, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["oldvehicleorder"].ToString(), 20, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["oldvehiclecash"].ToString(), 20, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["replacevehicle"].ToString(), 20, 0);

                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["replaceveihclecash"].ToString(), 20, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, Total1.ToString(), 20, 0);
                    totalNewscnaplock1 = +totalNewscnaplock1 + Convert.ToInt32(ds.Rows[i]["newvehicleorder"]);
                    totaloldscnaplock1 = totaloldscnaplock1 + Convert.ToInt32(ds.Rows[i]["oldvehicleorder"]);
                    totalReplacementScnaplock1 = +totalReplacementScnaplock1 + Convert.ToInt32(ds.Rows[i]["replacevehicle"]);
                }
                if (VehicleType == "285X45")
                {
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, "Registration Plate Size 285X45 mm", 20, 0);

                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["newvehicleorder"].ToString(), 20, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["newvehiclecash"].ToString(), 20, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["oldvehicleorder"].ToString(), 20, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["oldvehiclecash"].ToString(), 20, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["replacevehicle"].ToString(), 20, 0);

                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["replaceveihclecash"].ToString(), 20, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, Total1.ToString(), 20, 0);
                    totalNewscnaplock1 = +totalNewscnaplock1 + Convert.ToInt32(ds.Rows[i]["newvehicleorder"]);
                    totaloldscnaplock1 = totaloldscnaplock1 + Convert.ToInt32(ds.Rows[i]["oldvehicleorder"]);
                    totalReplacementScnaplock1 = +totalReplacementScnaplock1 + Convert.ToInt32(ds.Rows[i]["replacevehicle"]);
                }
                if (VehicleType == "Sticker")
                {
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, " 3rd Registration Plate Sticker Inclusive of Printing", 30, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["newvehicleorder"].ToString(), 20, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["newvehiclecash"].ToString(), 20, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["oldvehicleorder"].ToString(), 20, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["oldvehiclecash"].ToString(), 20, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["replacevehicle"].ToString(), 20, 0);

                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["replaceveihclecash"].ToString(), 20, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, Total1.ToString(), 20, 0);

                }
            }
            decimal snapLockRate = 0;
            SQLString = "SELECT ProductCost.Cost FROM  Product  INNER JOIN ProductCost ON Product.ProductID = ProductCost.ProductID where Product.productcode ='SNAP LOCK' and ProductCost.hsrp_stateID='" + intHSRPStateID + "'";
            DataTable dsRate = Utils.GetDataTable(SQLString, CnnString);
            if (dsRate.Rows.Count > 0)
            {
                snapLockRate = Convert.ToDecimal(dsRate.Rows[0]["cost"]);
            }
            sno = sno + 1;
            GenerateCell(table, 1, 1, 1, 0, 1, 1, 1, sno.ToString(), 20, 0);
            GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, "Snap Locks.", 20, 0);

            totalNewscnaplock1 = 2 * totalNewscnaplock1;
            GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, totalNewscnaplock1.ToString(), 20, 0);

            totalPriceNewscnaplock1 = Convert.ToInt32(totalNewscnaplock1) * Convert.ToDecimal(snapLockRate);
            totalPriceNewscnaplock1 = System.Decimal.Round(totalPriceNewscnaplock1, 0);
            GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, totalPriceNewscnaplock1.ToString(), 20, 0);

            totaloldscnaplock1 = 2 * totaloldscnaplock1;
            GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, totaloldscnaplock1.ToString(), 20, 0);

            totalPriceOldcnaplock1 = totaloldscnaplock1 * Convert.ToDecimal(snapLockRate);
            totalPriceOldcnaplock1 = System.Decimal.Round(totalPriceOldcnaplock1, 0);
            GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, totalPriceOldcnaplock1.ToString(), 20, 0);



            totalReplacementScnaplock1 = 2 * totalReplacementScnaplock1;
            GenerateCell(table, 1, 0, 1, 0, 1, 1, 0, totalReplacementScnaplock1.ToString(), 20, 0);


            totalPriceReplacementcnaplock1 = totalReplacementScnaplock1 * Convert.ToDecimal(snapLockRate);
            totalPriceReplacementcnaplock1 = System.Decimal.Round(totalPriceReplacementcnaplock1, 0);
            GenerateCell(table, 1, 0, 1, 0, 1, 1, 0, totalPriceReplacementcnaplock1.ToString(), 20, 0);


            decimal TotalSnapLocak = totalPriceNewscnaplock1 + totalPriceOldcnaplock1 + totalPriceReplacementcnaplock1;
            GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, TotalSnapLocak.ToString(), 20, 0);


            document.Add(table);
            document.NewPage();

            document.Close();
            context.Response.ContentType = "Application/pdf";
            context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
            context.Response.WriteFile(PdfFolder);
            context.Response.End();


        }

        public void ExportAllPdf()
        {
            Date();
            Int32 totalNewscnaplock1 = 0;
            Int32 totaloldscnaplock1 = 0;
            Int32 totalReplacementScnaplock1 = 0;

            Decimal totalPriceNewscnaplock1 = 0;

            Decimal totalPriceOldcnaplock1 = 0;

            Decimal totalPriceReplacementcnaplock1 = 0;
            string VehicleType = string.Empty;
            Int32 Total1 = 0;
            int sno = 0;
            Int32 totalNewPrice1 = 0;
            Int32 totalOldPrice1 = 0;
            Int32 ReplacementPrice1 = 0;
            int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);

              DataTable dtrto = GetRtoLocation();
              for (int iRowNo = 0; iRowNo < dtrto.Rows.Count; iRowNo++)
              {
                  string RTOName = dtrto.Rows[iRowNo]["rtolocationname"].ToString();
                  string RTOCode = dtrto.Rows[iRowNo]["RTOLocationid"].ToString();
                  string filename = "MonthlyStatementFromConcessionaireToRegisteringAuthority-MonthWise" + RTOName + "-" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".pdf";
                  string SQLString = String.Empty;
                  Document document = new Document();
                 // HttpContext context = HttpContext.Current;
                  SetFolder(RTOName, DropDownListStateName.SelectedItem.ToString(), "MonthlyStatementFromConcessionaireToRegisteringAuthority-MonthWise");

                  string PdfFolder = strPath + "\\" + filename; 
                  BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);


                 // string PdfFolder = ConfigurationManager.AppSettings["PdfFolder"].ToString() + filename;
                  PdfWriter.GetInstance(document, new FileStream(PdfFolder, FileMode.Create));
                  document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
                  document.Open();
                  PdfPTable table = new PdfPTable(9);
                  var colWidthPercentages = new[] { 10f, 95f, 20f, 25f, 25f, 35f, 25f, 35f, 15f };
                  table.SetWidths(colWidthPercentages);
                  SQLString = "Report_Monthly_StatementLocationWiseRegisteringAuthority'" + FromDate + "','" + ToDate + "','" + intHSRPStateID + "','" + RTOCode + "'";
                  DataTable ds = Utils.GetDataTable(SQLString, CnnString);


                  GenerateCell(table, 9, 0, 0, 0, 0, 1, 0, "SCHEDULE-E :  Monthly Statement From Concessionaire To Registering Authority", 15, 0);
                  GenerateCell(table, 3, 0, 0, 0, 0, 0, 0, "State:      " + DropDownListStateName.SelectedItem.ToString(), 15, 0);
                  GenerateCell(table, 3, 0, 0, 0, 0, 0, 0, "RTO Name:      " + RTOName, 15, 0);
                  GenerateCell(table, 3, 0, 0, 0, 0, 0, 0, "Month:      " + ddlMonth.SelectedItem.ToString() + "," + ddlYear.SelectedValue, 15, 0);
                 // GenerateCell(table, 3, 0, 0, 0, 0, 0, 0, "Report Generated Date :      " + System.DateTime.Now.ToString("dd/MM/yyyy"), 15, 0);

                  GenerateCell(table, 9, 0, 0, 0, 0, 1, 0, "", 50, 0);
                  GenerateCell(table, 1, 1, 1, 1, 1, 1, 0, "S.No:", 50, 0);
                  GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Category", 50, 0);
                  GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Number of new Vehicles Affixed", 50, 0);
                  GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Price Charged", 50, 0);
                  GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Number of existing Vehicles Affixed", 50, 0);
                  GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Price Charged", 50, 0);
                  GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Replacements", 50, 0);
                  GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Price Charged", 50, 0);
                  GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Total Amount", 50, 0);

                  for (int i = 0; i < ds.Rows.Count; i++)
                  {
                      VehicleType = ds.Rows[i]["vehicleandplatetype"].ToString();
                      sno = i + 1;
                      totalNewPrice1 = Convert.ToInt32(ds.Rows[i]["newvehiclecash"].ToString());
                      totalOldPrice1 = Convert.ToInt32(ds.Rows[i]["oldvehiclecash"].ToString());
                      ReplacementPrice1 = Convert.ToInt32(ds.Rows[i]["replaceveihclecash"].ToString());
                      Total1 = totalNewPrice1 + totalOldPrice1 + ReplacementPrice1;
                      GenerateCell(table, 1, 1, 1, 0, 1, 1, 1, sno.ToString(), 20, 0);

                      if (VehicleType == "2W")
                      {
                          GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, "Complete Set of Registration Plates Inclusive of Snap Lock and fixing for 2 Wheelers- Schooters, Motorcycles, Mopeds", 60, 0);


                          GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["newvehicleorder"].ToString(), 20, 0);
                          GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["newvehiclecash"].ToString(), 20, 0);
                          GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["oldvehicleorder"].ToString(), 20, 0);
                          GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["oldvehiclecash"].ToString(), 20, 0);
                          GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["replacevehicle"].ToString(), 20, 0);

                          GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["replaceveihclecash"].ToString(), 20, 0);
                          GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, Total1.ToString(), 20, 0);


                      }
                      if (VehicleType == "3W")
                      {
                          GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, "Complete Set of Registration Plates Inclusive of Snap Lock, 3rd Registration Plate and fixing for 3 wheelers and invalid carriages.", 60, 0);
                          GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["newvehicleorder"].ToString(), 20, 0);
                          GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["newvehiclecash"].ToString(), 20, 0);
                          GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["oldvehicleorder"].ToString(), 20, 0);
                          GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["oldvehiclecash"].ToString(), 20, 0);
                          GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["replacevehicle"].ToString(), 20, 0);

                          GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["replaceveihclecash"].ToString(), 20, 0);
                          GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, Total1.ToString(), 20, 0);
                      }
                      if (VehicleType == "4W")
                      {

                          GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, "Complete Set of Registration Plates Inclusive of Snap Lock 3rd Registration Plate and fixing for Light Motor Vehicles/Passenger Cars.", 60, 0);
                          GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["newvehicleorder"].ToString(), 20, 0);
                          GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["newvehiclecash"].ToString(), 20, 0);
                          GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["oldvehicleorder"].ToString(), 20, 0);
                          GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["oldvehiclecash"].ToString(), 20, 0);
                          GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["replacevehicle"].ToString(), 20, 0);

                          GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["replaceveihclecash"].ToString(), 20, 0);
                          GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, Total1.ToString(), 20, 0);


                      }

                      if (VehicleType == "4HW")
                      {
                          GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, "Complete Set of Registration Plates Inclusive of Snap Lock 3rd Registration Plate and fixing for Medium Commercial Vehicles/Havy Commercial Vehicles/Trailer/combination/others.", 60, 0);
                          GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["newvehicleorder"].ToString(), 20, 0);
                          GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["newvehiclecash"].ToString(), 20, 0);
                          GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["oldvehicleorder"].ToString(), 20, 0);
                          GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["oldvehiclecash"].ToString(), 20, 0);
                          GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["replacevehicle"].ToString(), 20, 0);

                          GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["replaceveihclecash"].ToString(), 20, 0);
                          GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, Total1.ToString(), 20, 0);

                      }
                      if (VehicleType == "500X120")
                      {

                          GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, "Registration Plate Size 500X120 mm", 20, 0);
                          GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["newvehicleorder"].ToString(), 20, 0);
                          GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["newvehiclecash"].ToString(), 20, 0);
                          GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["oldvehicleorder"].ToString(), 20, 0);
                          GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["oldvehiclecash"].ToString(), 20, 0);
                          GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["replacevehicle"].ToString(), 20, 0);

                          GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["replaceveihclecash"].ToString(), 20, 0);
                          GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, Total1.ToString(), 20, 0);
                          totalNewscnaplock1 = +totalNewscnaplock1 + Convert.ToInt32(ds.Rows[i]["newvehicleorder"]);
                          totaloldscnaplock1 = +totaloldscnaplock1 + Convert.ToInt32(ds.Rows[i]["oldvehicleorder"]);
                          totalReplacementScnaplock1 = +totalReplacementScnaplock1 + Convert.ToInt32(ds.Rows[i]["replacevehicle"]);

                      }
                      if (VehicleType == "340X200")
                      {
                          GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, "Registration Plate Size 300X200 mm", 20, 0);
                          GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["newvehicleorder"].ToString(), 20, 0);
                          GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["newvehiclecash"].ToString(), 20, 0);
                          GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["oldvehicleorder"].ToString(), 20, 0);
                          GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["oldvehiclecash"].ToString(), 20, 0);
                          GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["replacevehicle"].ToString(), 20, 0);

                          GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["replaceveihclecash"].ToString(), 20, 0);
                          GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, Total1.ToString(), 20, 0);
                          totalNewscnaplock1 = +totalNewscnaplock1 + Convert.ToInt32(ds.Rows[i]["newvehicleorder"]);
                          totaloldscnaplock1 = totaloldscnaplock1 + Convert.ToInt32(ds.Rows[i]["oldvehicleorder"]);
                          totalReplacementScnaplock1 = +totalReplacementScnaplock1 + Convert.ToInt32(ds.Rows[i]["replacevehicle"]);

                      }
                      if (VehicleType == "200X100")
                      {
                          GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, "Registration Plate Size 200X100 mm", 20, 0);

                          GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["newvehicleorder"].ToString(), 20, 0);
                          GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["newvehiclecash"].ToString(), 20, 0);
                          GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["oldvehicleorder"].ToString(), 20, 0);
                          GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["oldvehiclecash"].ToString(), 20, 0);
                          GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["replacevehicle"].ToString(), 20, 0);

                          GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["replaceveihclecash"].ToString(), 20, 0);
                          GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, Total1.ToString(), 20, 0);
                          totalNewscnaplock1 = +totalNewscnaplock1 + Convert.ToInt32(ds.Rows[i]["newvehicleorder"]);
                          totaloldscnaplock1 = totaloldscnaplock1 + Convert.ToInt32(ds.Rows[i]["oldvehicleorder"]);
                          totalReplacementScnaplock1 = +totalReplacementScnaplock1 + Convert.ToInt32(ds.Rows[i]["replacevehicle"]);
                      }
                      if (VehicleType == "285X45")
                      {
                          GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, "Registration Plate Size 285X45 mm", 20, 0);

                          GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["newvehicleorder"].ToString(), 20, 0);
                          GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["newvehiclecash"].ToString(), 20, 0);
                          GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["oldvehicleorder"].ToString(), 20, 0);
                          GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["oldvehiclecash"].ToString(), 20, 0);
                          GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["replacevehicle"].ToString(), 20, 0);

                          GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["replaceveihclecash"].ToString(), 20, 0);
                          GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, Total1.ToString(), 20, 0);
                          totalNewscnaplock1 = +totalNewscnaplock1 + Convert.ToInt32(ds.Rows[i]["newvehicleorder"]);
                          totaloldscnaplock1 = totaloldscnaplock1 + Convert.ToInt32(ds.Rows[i]["oldvehicleorder"]);
                          totalReplacementScnaplock1 = +totalReplacementScnaplock1 + Convert.ToInt32(ds.Rows[i]["replacevehicle"]);
                      }
                      if (VehicleType == "Sticker")
                      {
                          GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, " 3rd Registration Plate Sticker Inclusive of Printing", 30, 0);
                          GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["newvehicleorder"].ToString(), 20, 0);
                          GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["newvehiclecash"].ToString(), 20, 0);
                          GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["oldvehicleorder"].ToString(), 20, 0);
                          GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["oldvehiclecash"].ToString(), 20, 0);
                          GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["replacevehicle"].ToString(), 20, 0);

                          GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, ds.Rows[i]["replaceveihclecash"].ToString(), 20, 0);
                          GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, Total1.ToString(), 20, 0);

                      }
                  }
                  decimal snapLockRate = 0;
                  SQLString = "SELECT ProductCost.Cost FROM  Product  INNER JOIN ProductCost ON Product.ProductID = ProductCost.ProductID where Product.productcode ='SNAP LOCK' and ProductCost.hsrp_stateID='" + intHSRPStateID + "'";
                  DataTable dsRate = Utils.GetDataTable(SQLString, CnnString);
                  if (dsRate.Rows.Count > 0)
                  {
                      snapLockRate = Convert.ToDecimal(dsRate.Rows[0]["cost"]);
                  }
                  sno = sno + 1;
                  GenerateCell(table, 1, 1, 1, 0, 1, 1, 1, sno.ToString(), 20, 0);
                  GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, "Snap Locks.", 20, 0);

                  totalNewscnaplock1 = 2 * totalNewscnaplock1;
                  GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, totalNewscnaplock1.ToString(), 20, 0);

                  totalPriceNewscnaplock1 = Convert.ToInt32(totalNewscnaplock1) * Convert.ToDecimal(snapLockRate);
                  totalPriceNewscnaplock1 = System.Decimal.Round(totalPriceNewscnaplock1, 0);
                  GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, totalPriceNewscnaplock1.ToString(), 20, 0);

                  totaloldscnaplock1 = 2 * totaloldscnaplock1;
                  GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, totaloldscnaplock1.ToString(), 20, 0);

                  totalPriceOldcnaplock1 = totaloldscnaplock1 * Convert.ToDecimal(snapLockRate);
                  totalPriceOldcnaplock1 = System.Decimal.Round(totalPriceOldcnaplock1, 0);
                  GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, totalPriceOldcnaplock1.ToString(), 20, 0);



                  totalReplacementScnaplock1 = 2 * totalReplacementScnaplock1;
                  GenerateCell(table, 1, 0, 1, 0, 1, 1, 0, totalReplacementScnaplock1.ToString(), 20, 0);


                  totalPriceReplacementcnaplock1 = totalReplacementScnaplock1 * Convert.ToDecimal(snapLockRate);
                  totalPriceReplacementcnaplock1 = System.Decimal.Round(totalPriceReplacementcnaplock1, 0);
                  GenerateCell(table, 1, 0, 1, 0, 1, 1, 0, totalPriceReplacementcnaplock1.ToString(), 20, 0);


                  decimal TotalSnapLocak = totalPriceNewscnaplock1 + totalPriceOldcnaplock1 + totalPriceReplacementcnaplock1;
                  GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, TotalSnapLocak.ToString(), 20, 0);


                  document.Add(table);
                  document.NewPage();

                  document.Close();
                  //context.Response.ContentType = "Application/pdf";
                  //context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                  //context.Response.WriteFile(PdfFolder);
                  //context.Response.End();
              }

        
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            Date();
            decimal snapLockRate1 = 0;
            Int32 totalNewscnaplock2 = 0;
            Int32 totaloldscnaplock2 = 0;
            Int32 totalReplacementScnaplock2 = 0;

            Decimal totalPriceNewscnaplock2 = 0;

            Decimal totalPriceOldcnaplock2 = 0;

            Decimal totalPriceReplacementcnaplock2 = 0;
            totalNewscnaplock2 = 2 * totalNewscnaplock2;
            totalPriceNewscnaplock2 = Convert.ToInt32(totalNewscnaplock2) * Convert.ToDecimal(snapLockRate1);
            totalPriceNewscnaplock2 = System.Decimal.Round(totalPriceNewscnaplock2, 0);
            totaloldscnaplock2 = 2 * totaloldscnaplock2;

            totalPriceOldcnaplock2 = totaloldscnaplock2 * Convert.ToDecimal(snapLockRate1);
            totalPriceOldcnaplock2 = System.Decimal.Round(totalPriceOldcnaplock2, 0);
            totalReplacementScnaplock2 = 2 * totalReplacementScnaplock2;

            totalPriceReplacementcnaplock2 = totalReplacementScnaplock2 * Convert.ToDecimal(snapLockRate1);
            totalPriceReplacementcnaplock2 = System.Decimal.Round(totalPriceReplacementcnaplock2, 0);

            decimal TotalSnapLocak1 = totalPriceNewscnaplock2 + totalPriceOldcnaplock2 + totalPriceReplacementcnaplock2;
            SQLString = "Report_Monthly_StatementLocationWiseRegisteringAuthority'" + FromDate + "','" + ToDate + "','" + DropDownListStateName.SelectedValue.ToString() + "','" + dropDownListClient.SelectedValue + "'";
            DataTable ds = Utils.GetDataTable(SQLString, CnnString);
            ViewState["CurrentTable"] = ds;
            if (ViewState["CurrentTable"] != null)
            {

                DataTable dtResult = (DataTable)ViewState["CurrentTable"];
                DataRow dtNewRow = null;
                dtNewRow = dtResult.NewRow();

                string SQLStringNew = "SELECT ProductCost.Cost FROM  Product  INNER JOIN ProductCost ON Product.ProductID = ProductCost.ProductID where Product.productcode ='SNAP LOCK' and ProductCost.hsrp_stateID='" + DropDownListStateName.SelectedValue.ToString() + "'";
                DataTable dtNew = Utils.GetDataTable(SQLStringNew, CnnString);
                snapLockRate1 = Convert.ToDecimal(dtNew.Rows[0]["cost"]);


                dtNewRow[0] = dtResult.Rows.Count + 1;
                dtNewRow[1] = "Snap Lock";
                dtNewRow[2] = totalNewscnaplock2;
                dtNewRow[3] = totalPriceNewscnaplock2;
                dtNewRow[4] = totaloldscnaplock2;
                dtNewRow[5] = totalReplacementScnaplock2;
                dtNewRow[6] = totalPriceReplacementcnaplock2;
                dtNewRow[7] = TotalSnapLocak1;

                dtResult.Rows.Add(dtNewRow);
                ViewState["CurrentTable"] = dtResult;

               
                GridView1.DataSource = ds;
                GridView1.DataBind();
                GridView1.HeaderRow.Cells[0].Text = "S.No";
                GridView1.HeaderRow.Cells[1].Text = "New Vehicle Count";
                GridView1.HeaderRow.Cells[2].Text = "New Vehicle Collection";
                GridView1.HeaderRow.Cells[3].Text = "Old Vehicle Count";
                GridView1.HeaderRow.Cells[4].Text = "Old Vehicle Collection";
                GridView1.HeaderRow.Cells[5].Text = "Replace Vehicle Count";
                GridView1.HeaderRow.Cells[6].Text = "Replace Vehicle Collection";

            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            SQLString = "Report_Monthly_StatementLocationWiseRegisteringAuthority'" + FromDate + "','" + ToDate + "','" + DropDownListStateName.SelectedValue.ToString() + "','" + dropDownListClient.SelectedValue + "'";
            DataTable ds = Utils.GetDataTable(SQLString, CnnString);
            GridView1.DataSource = ds;
            GridView1.DataBind();
        }

        protected void btnallreportpdf_Click(object sender, EventArgs e)
        {
            ExportAllPdf();
        }
    }
}