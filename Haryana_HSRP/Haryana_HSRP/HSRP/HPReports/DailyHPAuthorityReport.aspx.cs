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

namespace HSRP.HPReports
{
    public partial class DailyHRAuthorityReport : System.Web.UI.Page
    {
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
                //labelOrganization.Visible = false;
                //DropDownListStateName.Visible = false;

                //labelClient.Visible = false;
                //dropDownListClient.Visible = false;

                //labelDate.Visible = false;
                //OrderDate.Visible = false;
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


        #region DropDown

        private void FilldropDownListOrganization()
        {
            if (UserType.Equals(0))
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where ActiveStatus='Y' Order by HSRPStateName";
                Utils.PopulateDropDownList(DropDownListStateName, SQLString.ToString(), CnnString, "--Select State--");
                // DropDownListStateName.SelectedIndex = DropDownListStateName.Items.Count - 1;
            }
            else
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID=" + HSRPStateID + " and ActiveStatus='Y' Order by HSRPStateName";
                DataSet dts = Utils.getDataSet(SQLString, CnnString);
                DropDownListStateName.DataSource = dts;
                DropDownListStateName.DataBind();

            }
        }


        #endregion

        string FromDate, ToDate;
        DataSet ds = new DataSet();

        string mm, dd, yy;




        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {

            try
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


                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                //int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);

                DataTable dt = new DataTable();


                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                //int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);

                string filename = "Schedule-E-HR_Authority_Report" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                Workbook book = new Workbook();

                // Specify which Sheet should be opened and the size of window by default
                book.ExcelWorkbook.ActiveSheetIndex = 1;
                book.ExcelWorkbook.WindowTopX = 100;
                book.ExcelWorkbook.WindowTopY = 200;
                book.ExcelWorkbook.WindowHeight = 7000;
                book.ExcelWorkbook.WindowWidth = 8000;

                // Some optional properties of the Document
                book.Properties.Author = "HSRP";
                book.Properties.Title = "Daily Entry Report";
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

                Worksheet sheet = book.Worksheets.Add("HR Authority Report");
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
                // row.
             

                row.Index = 1;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle6"));
                WorksheetCell cell5 = row.Cells.Add("SCHEDULE-D ");
                cell5.MergeAcross = 3; // Merge two cells togetherto 
                cell5.StyleID = "HeaderStyle6";

                row = sheet.Table.Rows.Add();


                row.Index = 2;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                WorksheetCell cell = row.Cells.Add("Monthly Report From Concessionaire to Registering Authority");
                cell.MergeAcross = 3; // Merge two cells togetherto 
                cell.StyleID = "HeaderStyle3";

                row = sheet.Table.Rows.Add();



                row.Index = 3;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("State:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("HIMACHAL PRADESH", "HeaderStyle2"));

                row = sheet.Table.Rows.Add();
                // Skip one row, and add some text

                WorksheetStyle style6 = book.Styles.Add("HeaderStyle6");
                style6.Font.FontName = "Tahoma";
                style6.Font.Size = 10;
                style6.Font.Bold = true;
                style6.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                style6.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                style6.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                style6.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                style6.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);

                row.Index = 4;

                DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("Generated Datetime:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(DateTime.Now.ToString(), "HeaderStyle2"));
                row = sheet.Table.Rows.Add();
                row.Index = 5;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("Report Month", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(ddlMonth.SelectedItem.ToString() + "-" + ddlYear.SelectedValue.ToString(), "HeaderStyle2"));
                row = sheet.Table.Rows.Add();



                row.Index = 6;
                //row.Cells.Add("Order Date");
                row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle"));

                WorksheetCell cell2 = row.Cells.Add("Applicaton Received");
                cell2.MergeAcross = 1; // Merge two cells togetherto 
                cell2.StyleID = "HeaderStyle6";

                WorksheetCell cell3 = row.Cells.Add("Registration Plates Supplied");
                cell3.MergeAcross = 1; // Merge two cells togetherto 
                cell3.StyleID = "HeaderStyle6";

                //WorksheetCell cell4 = row.Cells.Add("Back log (if any)");
                //cell4.MergeAcross = 1; // Merge two cells togetherto 
                //cell4.StyleID = "HeaderStyle6";

               row.Cells.Add(new WorksheetCell("Back log (if any) ", "HeaderStyle"));

                row = sheet.Table.Rows.Add();



                row.Index = 7;
                //row.Cells.Add("Order Date");
                row.Cells.Add(new WorksheetCell("Sr.No.", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Registering Authority", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("New Registration", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Existing Vehicles", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("New Registration ", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Existing Vehicles", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle6"));
                //row.Cells.Add(new WorksheetCell("O", "HeaderStyle6"));
                row = sheet.Table.Rows.Add();

                String StringField = String.Empty;
                String StringAlert = String.Empty;

                //row.Index = 9;

                UserType = Convert.ToInt32(Session["UserType"]);


                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                // ds= bl.dataEntryreport(FromDate, DropDownListStateName.SelectedValue);
                SQLString = "Report_rtoreport_embossed'" + FromDate + "','" + ToDate + "', 3 ";
                DataTable ds = Utils.GetDataTable(SQLString, CnnString);


                //SQLString = "select  (select RTOLocationName  from RTOLocation where RTOLocationID=a.RTOLocationID group by RTOLocationName) as location ,(select UserFirstName from Users where UserID =CreatedBy and RTOLocationID=a.RTOLocationID) as username,COUNT(CreatedBy) as newentery , Count(OrderEmbossingDate) as Embossing ,Count(OrderClosedDate) as orderClose from HSRPRecords a where (HSRP_StateID ='" + DropDownListStateName.SelectedValue + "' and (CreatedBy is not null or CreatedBy !='') and HSRPRecord_CreationDate between '" + FromDate + "' and '" + ToDate + "') or (HSRP_StateID ='" + DropDownListStateName.SelectedValue + "' and (CreatedBy is not null or CreatedBy !='') and OrderEmbossingDate between '" + FromDate + "' and '" + ToDate + "'  ) or (HSRP_StateID ='" + DropDownListStateName.SelectedValue + "' and (CreatedBy is not null or CreatedBy !='') and OrderClosedDate between '" + FromDate + "' and '" + ToDate + "'  ) group by  a.RTOLocationID  ,CreatedBy order by (select RTOLocationName  from RTOLocation where RTOLocationID=a.RTOLocationID group by RTOLocationName)";

                // dt = Utils.GetDataTable(SQLString, CnnString);


                string RTOColName = string.Empty;
                int sno = 0;


                if (ds.Rows.Count > 0)
                {
                    foreach (DataRow dtrows in ds.Rows) // Loop over the rows.
                    {
                        sno = sno + 1;
                        row = sheet.Table.Rows.Add();
                        row.Cells.Add(new WorksheetCell(sno.ToString(), DataType.String, "HeaderStyle1"));

                        // row.Cells.Add(new WorksheetCell(dtrows["id"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["locationname"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["neworder"].ToString(), DataType.Number, "HeaderStyle15"));
                        row.Cells.Add(new WorksheetCell(dtrows["oldregno"].ToString(), DataType.Number, "HeaderStyle15"));
                        row.Cells.Add(new WorksheetCell(dtrows["newclosed"].ToString(), DataType.Number, "HeaderStyle15"));
                        row.Cells.Add(new WorksheetCell(dtrows["oldclosed"].ToString(), DataType.Number, "HeaderStyle15"));
                        int sum = int.Parse(dtrows["newlog"].ToString()) + int.Parse(dtrows["oldlog"].ToString());
                        row.Cells.Add(new WorksheetCell(sum.ToString(), DataType.Number, "HeaderStyle15"));
                        //row.Cells.Add(new WorksheetCell(dtrows["oldlog"].ToString(), DataType.Number, "HeaderStyle15"));
                    }
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
    }
}