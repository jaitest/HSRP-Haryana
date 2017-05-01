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
using CarlosAg.ExcelXmlWriter;
using System.Drawing;


namespace HSRP.Report
{
    public partial class DailyRTOReport : System.Web.UI.Page
    {
        int UserType1;
        string CnnString1 = string.Empty;
        string HSRP_StateID1 = string.Empty;
        string RTOLocationID1 = string.Empty;
        string strUserID1 = string.Empty;
        int intHSRPStateID1;
        int intRTOLocationID1;
        string SQLString1 = string.Empty;
        string OrderType;
        string recordtype = string.Empty;
        //DateTime OrderDate1;
        string SQLString = string.Empty;

        DataTable dt = new DataTable();
        DataTable dt1 = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {

                UserType1 = Convert.ToInt32(Session["UserType"]);
                CnnString1 = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                strUserID1 = Session["UID"].ToString();

                HSRP_StateID1 = Session["UserHSRPStateID"].ToString();
                RTOLocationID1 = Session["UserRTOLocationID"].ToString();
                if (!IsPostBack)
                {
                    InitialSetting();
                    try
                    {
                        if (UserType1.Equals(0))
                        {
                           
                            ddlState.Visible = true;
                            
                            FilldropDownListOrganization();

                        }
                        else
                        {

                          
                            ddlState.Visible = true;
                           
                            FilldropDownListOrganization();
                            FilldropDownListClient();
                           
                        }


                    }
                    catch (Exception err)
                    {
                        throw err;
                    }
                }
            }
        }

        private void FilldropDownListOrganization()
        {
            if (UserType1.Equals(0))
            {
                SQLString1 = "select HSRPStateName,HSRP_StateID from HSRPState  where ActiveStatus='Y' Order by HSRPStateName";
                Utils.PopulateDropDownList(ddlState, SQLString1.ToString(), CnnString1, "--Select State--");
            }
            else
            {
                SQLString1 = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID=" + HSRP_StateID1 + " and ActiveStatus='Y' Order by HSRPStateName";
                DataSet dts = Utils.getDataSet(SQLString1, CnnString1);
                ddlState.DataSource = dts;
                ddlState.DataBind();
            }
        }

        
        private void FilldropDownListClient()
        {
            if (UserType1.Equals(0))
            {
                int.TryParse(ddlState.SelectedValue, out intHSRPStateID1);
                SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + intHSRPStateID1 + " and ActiveStatus!='N'  Order by RTOLocationName";

                 Utils.PopulateDropDownList(ddlRtoLocation, SQLString.ToString(), CnnString1, "--Select Location--");
                
            }
            else
            {

                SQLString = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID where  a.LocationType!='District' and UserRTOLocationMapping.UserID='" + strUserID1 + "' ";

                 DataSet dss = Utils.getDataSet(SQLString, CnnString1);
                 ddlRtoLocation.DataSource = dss;
                 ddlRtoLocation.DataBind();
            }
        }
        private void InitialSetting()
        {
            string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            string MaxDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            
            OrderDatefrom.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            OrderDatefrom.MaxDate = DateTime.Parse(MaxDate);
            CalendarOrderDatefrom.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarOrderDatefrom.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
        }

        protected void DropDownListStateName_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilldropDownListClient();
        }


        protected void btnexport_Click(object sender, EventArgs e)
        {
           
            SaveAndDownloadFile();
        }

        private void SaveAndDownloadFile()
        {
            Workbook book = new Workbook();
            string filename = "DAILY_REPORT_FROM-EMBOSSING_STATIONS_TO_REGISTERING_AUTHORITY" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";

            string strOrderType = string.Empty;

            
            Export(strOrderType, book, 1);
            //  Export("D", book, 2);

            HttpContext context = HttpContext.Current;
            context.Response.Clear();
            // Save the file and open it
            book.Save(Response.OutputStream);
            context.Response.ContentType = "application/vnd.ms-excel";

            context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
            context.Response.End();

        }

        int icount = 0;

        private void Export(string strReportType, Workbook book, int iActiveSheet)
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
                book.Properties.Title = "Collection Summary";
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

                Worksheet sheet = book.Worksheets.Add("Daily RTO Report");

                #region Fetch Data
                DataSet ds = new DataSet();


                //string SQLString = "select ROW_NUMBER() over(order by hsrprecord_authorizationno) as [S.No], hsrprecord_authorizationno as [Application no.],vehicletype as [Vehicle Class],OwnerName as [Owner's Name],vehicleregno as [Vehicle Reg. No],HSRP_Front_LaserCode as [Front Laser Plate No.],hsrp_rear_lasercode as [Rear Laser Plate No.],case when StickerMandatory ='Y' then 'YES' else 'NO' end as [3RD RP Y/N], case when VehicleClass = 'Non-Transport' then 'White' else 'Yellow' end as [COLOR], case when orderstatus='Closed' then convert(varchar(15),ordercloseddate,103) end as [AFFIXATION], case when OrderStatus='Closed' then 'AFFIXED' else 'CUSTOMER NOT COME .' end as [BACKGROUND REMARK] from hsrprecords "+
                  //  "where HSRP_StateID='" + ddlState.SelectedValue + "' and RTOLocationId='" + ddlRtoLocation.SelectedValue + "' and convert(date,HSRPRecord_CreationDate)='" + OrderDatefrom.SelectedDate + "' and ((hsrp_front_lasercode is not null and HSRP_Rear_LaserCode is not null) or (hsrp_front_lasercode !='' and HSRP_Rear_LaserCode !=''))";
                string SQLString = "select ROW_NUMBER() over(order by hsrprecord_authorizationno) as [S.No], hsrprecord_authorizationno as [Application no.],vehicletype as [Vehicle Class],OwnerName as [Owner's Name],vehicleregno as [Vehicle Reg. No],HSRP_Front_LaserCode as [Front Laser Plate No.],hsrp_rear_lasercode as [Rear Laser Plate No.],case when StickerMandatory ='Y' then 'YES' else 'NO' end as [3RD RP Y/N], case when VehicleClass = 'Non-Transport' then 'White' else 'Yellow' end as [COLOR], case when orderstatus='Closed' then convert(varchar(15),ordercloseddate,103) end as [AFFIXATION], case when OrderStatus='Closed' then 'AFFIXED' else 'CUSTOMER NOT COME .' end as [BACKGROUND REMARK] from hsrprecords "+
                     "where HSRP_StateID='" + ddlState.SelectedValue + "' and RTOLocationId='" + ddlRtoLocation.SelectedValue + "' and convert(date,HSRPRecord_CreationDate)='" + OrderDatefrom.SelectedDate + "'";
                DataTable dt = Utils.GetDataTable(SQLString, CnnString1);
               
                #endregion

                AddColumnToSheet(sheet, 100, dt.Columns.Count);
               


                int iIndex = 3;
                WorksheetRow row = sheet.Table.Rows.Add();
                row.Index = iIndex++;
              
                row.Cells.Add(new WorksheetCell("Report :", "HeaderStyle2"));
                WorksheetCell cell = row.Cells.Add("DAILY REPORT FROM EMBOSSING STATIONS TO REGISTERING AUTHORITY");
                cell.MergeAcross = 4; // Merge two cells together
                cell.StyleID = "HeaderStyle2";

                row = sheet.Table.Rows.Add();
                row.Index = iIndex++;
                AddNewCell(row, "State:", "HeaderStyle2", 1);
                AddNewCell(row, ddlState.SelectedItem.ToString(), "HeaderStyle2", 1);
                AddNewCell(row, ddlRtoLocation.SelectedItem.ToString(), "HeaderStyle2", 1);
                row = sheet.Table.Rows.Add();
                row.Index = iIndex++;

                DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());

                AddNewCell(row, "Report Generation Date:", "HeaderStyle2", 1);
               AddNewCell(row, System.DateTime.Now.ToString("dd/MMM/yyyy"), "HeaderStyle2", 1);
                 row.Cells.Add(new WorksheetCell("Report Date:", "HeaderStyle2"));
                  row.Cells.Add(new WorksheetCell(OrderDatefrom.SelectedDate.ToString("dd/MMM/yyyy"), "HeaderStyle2"));
                
                AddNewCell(row, "", "HeaderStyle2", 2);
                row = sheet.Table.Rows.Add();

                row.Index = iIndex++;

            
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (dt.Columns[i].ColumnName.ToString().EndsWith("1") || dt.Columns[i].ColumnName.ToString().EndsWith("2") || dt.Columns[i].ColumnName.ToString().EndsWith("3") || dt.Columns[i].ColumnName.ToString().EndsWith("4"))
                    {
                        string strCol = dt.Columns[i].ColumnName.ToString();
                       
                        AddNewCell(row, strCol.Remove(strCol.Length-1), "HeaderStyle6", 1);
                    }
                    else
                    {

                        AddNewCell(row, dt.Columns[i].ColumnName.ToString(), "HeaderStyle6", 1);
                    }
                }
                row = sheet.Table.Rows.Add();
                row.Index = iIndex++;
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                   
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            
                           
                                AddNewCell(row, dt.Rows[j][i].ToString(), "HeaderStyle6", 1);
                           
                        }
                        row = sheet.Table.Rows.Add();
                   
                }              
                
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }

        private static void AddNewCell(WorksheetRow row,string strText,string strStyle,int iCnt)
        {
            for (int i = 0; i < iCnt;i++)
                row.Cells.Add(new WorksheetCell(strText, strStyle));
        }

        private static void AddColumnToSheet(Worksheet sheet,int iWidth,int iCnt)
        {
            for (int i = 0; i < iCnt;i++)
                sheet.Table.Columns.Add(new WorksheetColumn(iWidth));
        }
    }
}