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
using System.Configuration;

namespace HSRP.UTKReports
{
    public partial class Tc_Report_RegVsCll : System.Web.UI.Page
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
        string hsrpstate = string.Empty;
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
                       // FilldropDownListLocation();

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
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState where ActiveStatus='Y' Order by HSRPStateName";
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



        private void FilldropDownListLocation()
        {
            SQLString = "select RTOLocationName,RTOLocationID from RTOLocation where HSRP_StateID=" + HSRPStateID + " and ActiveStatus='Y' order by RTOLocationName";
            Utils.PopulateDropDownList(ddllocation, SQLString.ToString(), CnnString, "--Select Location--");
        }

        private void FilldropDownListAllLocation()
        {
            SQLString = "select RTOLocationName,RTOLocationID from RTOLocation where HSRP_StateID=" + HSRPStateID + " order by RTOLocationName";
            Utils.PopulateDropDownList(ddllocation, SQLString.ToString(), CnnString, "--Select Location--");
        }

        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {

            try
            {
                Export();



            }

            catch (Exception ex)
            {
                LabelError.Text = "Error in Populating Grid :" + ex.Message.ToString();
            }
        }
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);  
        private void Export()
        {
            try
            {

                Workbook book = new Workbook();
                // Specify which Sheet should be opened and the size of window by default
                book.ExcelWorkbook.ActiveSheetIndex = 1;
                book.ExcelWorkbook.WindowTopX = 100;
                book.ExcelWorkbook.WindowTopY = 200;
                book.ExcelWorkbook.WindowHeight = 7000;
                book.ExcelWorkbook.WindowWidth = 8000;

                // Some optional properties of the Document
                book.Properties.Author = "HSRP";
                book.Properties.Title = "Report";
                book.Properties.Created = DateTime.Now;

                #region Fetch Data
                DataTable dt = new DataTable();

                SqlCommand cmd = new SqlCommand("[Report_HR_TCRegVSColl]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@hsrp_stateid", DropDownListStateName.SelectedValue));
                cmd.Parameters.Add(new SqlParameter("@month", ddlMonth.SelectedItem.Text));
                cmd.Parameters.Add(new SqlParameter("@year", ddlYear.SelectedItem.Text));
                cmd.Parameters.Add(new SqlParameter("@rep_type", "D"));


                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter(cmd);



                da.Fill(dt);


                #endregion


                // Add some styles to the Workbook

                #region Styles

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

                WorksheetStyle styleHeader = book.Styles.Add("HeaderStyleHeader");
                styleHeader.Font.FontName = "Tahoma";
                styleHeader.Interior.Color = "Red";
                styleHeader.Font.Size = 10;
                styleHeader.Font.Bold = true;
                styleHeader.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                styleHeader.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                styleHeader.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                styleHeader.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                styleHeader.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


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
                #endregion

                Worksheet sheet = book.Worksheets.Add("Report");

                #region UpperPart of Excel
                AddColumnToSheet(sheet, 100, dt.Columns.Count);
                int iIndex = 3;
                WorksheetRow row = sheet.Table.Rows.Add();
                row.Index = iIndex++;
                //  row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                row.Cells.Add(new WorksheetCell("Report :", "HeaderStyle3"));

                row.Cells.Add(new WorksheetCell("TC Report", "HeaderStyle3"));



                row = sheet.Table.Rows.Add();
                row.Index = iIndex++;

                AddNewCell(row, "State:", "HeaderStyle2", 1);
                AddNewCell(row, DropDownListStateName.SelectedItem.Text, "HeaderStyle2", 1);
                row = sheet.Table.Rows.Add();
                row.Index = iIndex++;

                DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());

                // AddNewCell(row, "Report Duration:", "HeaderStyle2", 1);
                // AddNewCell(row, OrderDate.SelectedDate.ToString("dd/MMM/yyyy") + "-" + HSRPAuthDate.SelectedDate.ToString("dd/MMM/yyyy"), "HeaderStyle2", 1);
                //  row = sheet.Table.Rows.Add();

                row.Index = iIndex++;

                // row.Cells.Add(new WorksheetCell("", "HeaderStyle6"));
                //row.Cells.Add(new WorksheetCell("", "HeaderStyle6"));
                //  row.Cells.Add(new WorksheetCell("New Order :", "HeaderStyle3"));
                //WorksheetCell cell = row.Cells.Add("New Order");
                //cell.MergeAcross = 6; // Merge two cells together
                //cell.StyleID = "HeaderStyleHeader";

                //WorksheetCell cellEmb = row.Cells.Add("Embossing Done");
                //cellEmb.MergeAcross = 6; // Merge two cells together
                //cellEmb.StyleID = "HeaderStyleHeader";

                //WorksheetCell cellPendProd = row.Cells.Add("Production Under Process");
                //cellPendProd.MergeAcross = 6; // Merge two cells together
                //cellPendProd.StyleID = "HeaderStyleHeader";

                //WorksheetCell cellAffix = row.Cells.Add("Affixation");
                //cellAffix.MergeAcross = 6; // Merge two cells together
                //cellAffix.StyleID = "HeaderStyleHeader";

                //WorksheetCell cellPendAffix = row.Cells.Add("Pending for Affixation");
                //cellPendAffix.MergeAcross = 6; // Merge two cells together
                //cellPendAffix.StyleID = "HeaderStyleHeader";
                //row = sheet.Table.Rows.Add();

                row.Index = iIndex++;
                #endregion

                #region Column Creation and Assign Data
                string RTOColName = string.Empty;
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    AddNewCell(row, dt.Columns[i].ColumnName.ToString().Remove(dt.Columns[i].ColumnName.ToString().Length - 2, 0), "HeaderStyleHeader", 1);
                }
                row = sheet.Table.Rows.Add();
                row.Index = iIndex++;


                for (int j = 0; j < dt.Rows.Count; j++)
                {

                    for (int i = 0; i < dt.Columns.Count; i++)
                    {

                        //if (dt.Rows[j]["Location"].ToString().Equals("ZZZZZ") && i.Equals(0))
                        //{
                        //    AddNewCell(row, "Total", "HeaderStyle6", 1);
                        //}
                        //else
                        //{
                        AddNewCell(row, dt.Rows[j][i].ToString(), "HeaderStyle6", 1);

                        //}
                    }
                    row = sheet.Table.Rows.Add();

                }
                row = sheet.Table.Rows.Add();


                #endregion
                HttpContext context = HttpContext.Current;
                context.Response.Clear();
                // Save the file and open it
                book.Save(Response.OutputStream);

                //context.Response.ContentType = "text/csv";
                context.Response.ContentType = "application/vnd.ms-excel";
                string filename = "Report" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";

                context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                context.Response.End();
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

        protected void btnGo_Click(object sender, EventArgs e)
        {
            int Imonth = Int32.Parse(ddlMonth.SelectedValue.ToString());
            if (Imonth < 6)
            {
                label1.Visible = true;
                ddllocation.Visible = true;
                btnExportToExcel.Visible = true;
                FilldropDownListAllLocation();

            }
            else
            {
                label1.Visible = true;
                ddllocation.Visible = true;
                btnExportToExcel.Visible = true;
                FilldropDownListLocation();

            }
        }
    }
}