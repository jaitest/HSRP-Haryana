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
    public partial class YuEmbossingVehicleWiseCountReport : System.Web.UI.Page
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
                            // labelOrganization.Visible = true;
                            ddlState.Visible = true;
                            //dropDownListClient.Visible = true;
                            FilldropDownListOrganization();
                            //FilldropDownListClient();
                            


                        }
                        else
                        {

                            // labelOrganization.Visible = true;
                            ddlState.Visible = true;
                            //dropDownListClient.Visible = true;
                            FilldropDownListOrganization();
                            //FilldropDownListClient();

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
           
        }


        protected void btnexport_Click(object sender, EventArgs e)
        {
           
            SaveAndDownloadFile();
        }

        private void SaveAndDownloadFile()
        {
            Workbook book = new Workbook();
            string filename = "AffixiationVehicleCount" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";

            string strOrderType = string.Empty;

            //  strOrderType = dropDownListorder.SelectedIndex.Equals(0) ? "All" : dropDownListorder.SelectedIndex.Equals(0) ? "Prod" : "Aff";
            Export(strOrderType, book, 1, "[New_EmbossingVehicleCount]");
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

        private void Export(string strReportType, Workbook book, int iActiveSheet,string strProcName)
        {
            try
            {
                SqlConnection con = new SqlConnection(CnnString1);


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

                Worksheet sheet = book.Worksheets.Add("Aging Report");

                #region Fetch Data
                DataSet ds = new DataSet();

                SqlCommand cmd = new SqlCommand();
                string strParameter = string.Empty;
                cmd = new SqlCommand(strProcName, con);


                cmd.CommandType = CommandType.StoredProcedure;
                //  if (!string.IsNullOrEmpty(strReportType))
                //  cmd.Parameters.Add(new SqlParameter("@flag", strReportType));

                cmd.Parameters.Add(new SqlParameter("@stateid", ddlState.SelectedValue));
                cmd.Parameters.Add(new SqlParameter("@reportDate", OrderDatefrom.SelectedDate));
                cmd.Parameters.Add(new SqlParameter("@flag", ddlordertype.SelectedItem.Text));
                
                

                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                #endregion

                AddColumnToSheet(sheet, 100, dt.Columns.Count);
               


                int iIndex = 3;
                WorksheetRow row = sheet.Table.Rows.Add();
                row.Index = iIndex++;
              //  row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                // row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                row.Cells.Add(new WorksheetCell("Report :", "HeaderStyle2"));
                WorksheetCell cell = row.Cells.Add("Affixiation Vehicle Count Report");
                cell.MergeAcross = 4; // Merge two cells together
                cell.StyleID = "HeaderStyle2";

                row = sheet.Table.Rows.Add();
                row.Index = iIndex++;

               // AddNewCell(row, "", "HeaderStyle2", 2);
                AddNewCell(row, "State:", "HeaderStyle2", 1);
                AddNewCell(row, ddlState.SelectedItem.ToString(), "HeaderStyle2", 1);
                // row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                // row.Cells.Add(new WorksheetCell("State:", "HeaderStyle2"));
                //  row.Cells.Add(new WorksheetCell(ddlState.SelectedItem.ToString(), "HeaderStyle2"));
              //  AddNewCell(row, "", "HeaderStyle2", 9);
                //row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row = sheet.Table.Rows.Add();
                row.Index = iIndex++;

                DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());

               // AddNewCell(row, "", "HeaderStyle2", 2);
                AddNewCell(row, "Report Generation Date:", "HeaderStyle2", 1);
                AddNewCell(row, System.DateTime.Now.ToString("dd/MMM/yyyy"), "HeaderStyle2", 1);
                 row.Cells.Add(new WorksheetCell("Report Date:", "HeaderStyle2"));
                  row.Cells.Add(new WorksheetCell(OrderDatefrom.SelectedDate.ToString("dd/MMM/yyyy"), "HeaderStyle2"));
                
                AddNewCell(row, "", "HeaderStyle2", 2);
                row = sheet.Table.Rows.Add();

                row.Index = iIndex++;

                AddNewCell(row, "", "HeaderStyle6", 1);
                WorksheetCell cell1 = row.Cells.Add(" Today's Embossing Vehicle Count");
                cell1.MergeAcross = 7; // Merge two cells together
                cell1.StyleID = "HeaderStyle6";
                WorksheetCell cell2 = row.Cells.Add("Month to Date Embossing Vehicle Count");
                cell2.MergeAcross = 7; // Merge two cells together
                cell2.StyleID = "HeaderStyle6";
                WorksheetCell cell3 = row.Cells.Add("Last Month Embossing Vehicle Count");
                cell3.MergeAcross = 7; // Merge two cells together
                cell3.StyleID = "HeaderStyle6";
                WorksheetCell cell4 = row.Cells.Add("Financial Year To Date Embossing Vehicle Count");
                cell4.MergeAcross = 7; // Merge two cells together
                cell4.StyleID = "HeaderStyle6";
                //WorksheetCell cell5 = row.Cells.Add("Previous Period Open Order");
                //cell5.MergeAcross = 7; // Merge two cells together
                //cell5.StyleID = "HeaderStyle6";
                row = sheet.Table.Rows.Add();
                
               // AddNewCell(row, "", "HeaderStyle2", 6);

              //  row = sheet.Table.Rows.Add();
              //  row.Index = iIndex++;               
               
              //  row = sheet.Table.Rows.Add();

                row.Index = iIndex++;

            

              //  dt.Columns["rtolocationname"].ColumnName = "Location";
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (dt.Columns[i].ColumnName.ToString().EndsWith("1") || dt.Columns[i].ColumnName.ToString().EndsWith("2") || dt.Columns[i].ColumnName.ToString().EndsWith("3") || dt.Columns[i].ColumnName.ToString().EndsWith("4"))
                    {
                        string strCol = dt.Columns[i].ColumnName.ToString();
                       // string output = strCol.Substring(strCol.Length - 1, 1);
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
                            
                             if (dt.Rows[j]["Location"].ToString().Equals("ZZZZZ") && i.Equals(0))
                            {
                                AddNewCell(row, "Total", "HeaderStyle6", 1);
                            }
                            
                             else
                            {
                                AddNewCell(row, dt.Rows[j][i].ToString(), "HeaderStyle6", 1);
                            }
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