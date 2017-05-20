using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using CarlosAg.ExcelXmlWriter;

namespace HSRP.Transaction
{


    public partial class DLViewDealerOrder : System.Web.UI.Page
    {
        Utils bl = new Utils();
        string HSRPStateID = string.Empty;
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        string userdealerid = string.Empty;
        int UserType;
        int icount = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            Utils.GZipEncodePage();

            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {
                HSRPStateID = Session["UserHSRPStateID"].ToString();
                UserType = Convert.ToInt32(Session["UserType"].ToString());
                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                strUserID = Session["UID"].ToString();
                userdealerid = Session["dealerid"].ToString();
                ComputerIP = Request.UserHostAddress;


                if (!IsPostBack)
                {
                    try
                    {
                        // orderno();
                        InitialSetting();


                        // Utils.user_log(strUserID, "View Organization", ComputerIP, "Page load", CnnString);
                    }
                    catch (Exception err)
                    {
                        lblErrMsg.Text = "Error on Page Load" + err.Message.ToString();
                    }
                }
            }
        }
        
        private void InitialSetting()
        {

            string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            string MaxDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            //HSRPAuthDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            //HSRPAuthDate.MaxDate = DateTime.Parse(MaxDate);
            //CalendarHSRPAuthDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            //CalendarHSRPAuthDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            OrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            OrderDate.MaxDate = DateTime.Parse(MaxDate);
            CalendarOrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarOrderDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
        }
        
        protected void btnGO_Click(object sender, EventArgs e)
        {
            llbMSGError.Text = "";
            ShowGrid();
            
        }

        public void ShowGrid()
        {

            SQLString = "select  ROW_NUMBER() OVER (ORDER BY OrderNo) AS [Sr No.],vehicleregno,OwnerName,OrderNo,MobileNo,	VehicleType,VehicleClass,	ChassisNo,	EngineNo, OrderType,RoundOff_NetAmount, ExShowRoomPrice from HSRPRecords where  convert(date,hsrprecord_creationdate)='" + OrderDate.SelectedDate + "' AND CreatedBy='" + strUserID + "'";
            DataTable dt = Utils.GetDataTable(SQLString.ToString(), CnnString.ToString());
            if (dt.Rows.Count > 0)
            {
                grdid.DataSource = dt;
                grdid.DataBind();
                grdid.Visible = true;
                btnExportExcel.Visible = true;
            }
            else
            {
                llbMSGError.Text = "";
                llbMSGError.Text = "Record Not Found";
            }
        }

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            SQLString = " Select ROW_NUMBER() OVER (ORDER BY OrderNo) AS [Sr No.],vehicleregno,OwnerName,OrderNo,MobileNo,	VehicleType,VehicleClass,	ChassisNo,	EngineNo, OrderType,RoundOff_NetAmount, ExShowRoomPrice from HSRPRecords where  convert(date,hsrprecord_creationdate)='" + OrderDate.SelectedDate + "' AND CreatedBy='" + strUserID + "'";
            DataTable dt = Utils.GetDataTable(SQLString.ToString(), CnnString.ToString());
            SaveAndDownloadFile(dt);
        }          
        
        public string StateName()
        {
            SQLString = "select HSRPStateName from HSRPState where  HSRP_StateID='" + HSRPStateID + "'";
            return Utils.GetDataTable(SQLString.ToString(), CnnString.ToString()).Rows[0]["HSRPStateName"].ToString();
        }

        private void SaveAndDownloadFile(DataTable dt)
        {
            Workbook book = new Workbook();
            string filename = "DealerOrder" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";

            string strOrderType = string.Empty;

            Export(dt, book, 1);

            HttpContext context = HttpContext.Current;
            context.Response.Clear();
            // Save the file and open it
            book.Save(Response.OutputStream);
            context.Response.ContentType = "application/vnd.ms-excel";

            context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
            context.Response.End();

        }
        
        
        private void Export(DataTable dt, Workbook book, int iActiveSheet)
        {
            try
            {
                // Specify which Sheet should be opened and the size of window by default
                book.ExcelWorkbook.ActiveSheetIndex = iActiveSheet;
                book.ExcelWorkbook.WindowTopX = 100;
                book.ExcelWorkbook.WindowTopY = 200;
                book.ExcelWorkbook.WindowHeight = 7000;
                book.ExcelWorkbook.WindowWidth = 8000;

                // Some optional properties of the Document
                book.Properties.Author = "HSRP";
                book.Properties.Title = "HSRP Dealer Order Report";
                book.Properties.Created = DateTime.Now;


                // Add some styles to the Workbook

                #region Styles
                if (icount <= 0)
                {
                    icount++;
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
                    WorksheetStyle style9 = book.Styles.Add("HeaderStyle9");
                    style9.Font.FontName = "Tahoma";
                    style9.Font.Size = 10;
                    style9.Font.Bold = true;
                    style9.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                    style9.Interior.Color = "#FCF6AE";
                    style9.Interior.Pattern = StyleInteriorPattern.Solid;

                }
                #endregion

                Worksheet sheet = book.Worksheets.Add("Report");
                if (dt.Rows.Count > 0)
                {
                    AddColumnToSheet(sheet, 100, dt.Columns.Count);
                    int iIndex = 3;
                    WorksheetRow row = sheet.Table.Rows.Add();                    
                    row.Index = iIndex++;
                    row.Cells.Add(new WorksheetCell("Report :", "HeaderStyle2"));
                    WorksheetCell cell = row.Cells.Add("HSRP Dealer Order Report");
                    cell.MergeAcross = 4; // Merge two cells together
                    cell.StyleID = "HeaderStyle2";
                    row = sheet.Table.Rows.Add();
                    row.Index = iIndex++;                  
                    row = sheet.Table.Rows.Add();

                    row.Index = iIndex++;
                    AddNewCell(row, "State:", "HeaderStyle2", 1);
                    AddNewCell(row, StateName().ToString(), "HeaderStyle2", 1);

                    row.Index = iIndex++;
                    AddNewCell(row, "Location:", "HeaderStyle2", 1);
                    AddNewCell(row, Session["RTOLocationName"].ToString(), "HeaderStyle2", 1);

                    row.Index = iIndex++;
                    AddNewCell(row, "Report Generated Date::", "HeaderStyle2", 1);
                    AddNewCell(row, OrderDate.SelectedDate.ToString(), "HeaderStyle2", 1);
                    
                    row.Index = iIndex++;
                    AddNewCell(row, "", "HeaderStyle", 1);
                    row = sheet.Table.Rows.Add();

                    
                    row.Index = iIndex++;
                    AddNewCell(row, "", "HeaderStyle6", 1);
                    row = sheet.Table.Rows.Add();

                    row.Index = iIndex++;
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        AddNewCell(row, dt.Columns[i].ColumnName.ToString(), "HeaderStyle6", 1);

                    }
                    row = sheet.Table.Rows.Add();
                    
                    row.Index = iIndex++;
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {

                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            AddNewCell(row, dt.Rows[j][i].ToString(), "HeaderStyle", 1);
                        }
                        row = sheet.Table.Rows.Add();
                    }
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        private static void AddNewCell(WorksheetRow row, string strText, string strStyle, int iCnt)
        {
            for (int i = 0; i < iCnt; i++)
                row.Cells.Add(new WorksheetCell(strText, strStyle));
        }

        private static void AddColumnToSheet(Worksheet sheet, int iWidth, int iCnt)
        {
            for (int i = 0; i < iCnt; i++)
                sheet.Table.Columns.Add(new WorksheetColumn(iWidth));
        }
    
    }
}