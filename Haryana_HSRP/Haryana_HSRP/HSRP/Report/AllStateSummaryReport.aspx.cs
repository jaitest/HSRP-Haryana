using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CarlosAg.ExcelXmlWriter;
using System.Data;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Text;
using System.Configuration;
using System.IO;

namespace HSRP.Report
{
    public partial class AllStateSummaryReport : System.Web.UI.Page
    {
        String strSqlString = String.Empty;
        string HsrpStateid = string.Empty;
        String strCnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        string UserType = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            HsrpStateid = Session["UserHSRPStateID"].ToString();
            UserType = Session["UserType"].ToString();
            if (!IsPostBack)
            {
                FillState();
            }
            
        }

        //protected void btnSubmit_Click(object sender, EventArgs e)
        //{
        //    lblError.Text = "";
        //    strSqlString = "[ReportAllSatateComparison_WithStaggingData] '"+ddlStateName.SelectedValue+"'";
        //    DataTable dtResult = Utils.GetDataTable(strSqlString, strCnnString);
        //    if (dtResult.Rows.Count > 0)
        //    {
        //        GridView1.DataSource = dtResult;
        //        GridView1.DataBind();
        //    }
        //    else
        //    {
        //        lblError.Text = "No Record Found";
        //    }
        //}

        #region Drop Dwon
        public void FillState()
        {
            strSqlString = "Select * from HsrpState where hsrp_stateid='"+HsrpStateid+"' and activestatus='Y'";
            DataTable dt = Utils.GetDataTable(strSqlString, strCnnString);
            ddlStateName.DataSource = dt;
            ddlStateName.DataBind();

            if (UserType == "0")
            {
                strSqlString = "select HSRPStateName,HSRP_StateID from HSRPState  where ActiveStatus='Y' Order by HSRPStateName";
                Utils.PopulateDropDownList(ddlStateName, strSqlString.ToString(), strCnnString, "--Select State--");
                // DropDownListStateName.SelectedIndex = DropDownListStateName.Items.Count - 1;

            }
            else
            {
                strSqlString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID=" + HsrpStateid + " and ActiveStatus='Y' Order by HSRPStateName";
                Utils.PopulateDropDownList(ddlStateName, strSqlString.ToString(), strCnnString, "--Select State--");
            }

            //Utils.PopulateDropDownList(ddlStateName, strSqlString, strCnnString);
        }
        #endregion

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            string filename = ddlStateName.SelectedItem.ToString()+"_Govt_Received_DataSummary" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
            Workbook book = new Workbook();
            #region WorkBook Size And Properties
            // Specify which Sheet should be opened and the size of window by default
            book.ExcelWorkbook.ActiveSheetIndex = 1;
            book.ExcelWorkbook.WindowTopX = 100;
            book.ExcelWorkbook.WindowTopY = 200;
            book.ExcelWorkbook.WindowHeight = 7000;
            book.ExcelWorkbook.WindowWidth = 8000;

            // Some optional properties of the Document
            book.Properties.Author = "HSRP";
            book.Properties.Title = "HSRP Total Summary Report StateWise";
            book.Properties.Created = DateTime.Now;
            #endregion
            
            #region Style Of Work Book
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

            #endregion

            Worksheet sheet = book.Worksheets.Add("HSRP Govt Received Data Summary");

            #region Column Declaration
            sheet.Table.Columns.Add(new WorksheetColumn(150));
            sheet.Table.Columns.Add(new WorksheetColumn(150));
            sheet.Table.Columns.Add(new WorksheetColumn(150));
            sheet.Table.Columns.Add(new WorksheetColumn(150));
            sheet.Table.Columns.Add(new WorksheetColumn(150));
            #endregion           

            #region Row 2
            WorksheetRow row = sheet.Table.Rows.Add();

            row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
            row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
            row.Cells.Add(new WorksheetCell("Report :", "HeaderStyle3"));
            WorksheetCell cell = row.Cells.Add("HSRP Total Summary Report StateWise");
            cell.MergeAcross = 3; // Merge two cells together
            cell.StyleID = "HeaderStyle3";
            #endregion

            #region Row 3
            row = sheet.Table.Rows.Add();
            row.Index = 3;
            row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
            row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
            row.Cells.Add(new WorksheetCell("State:", "HeaderStyle2"));
            row.Cells.Add(new WorksheetCell(ddlStateName.SelectedItem.ToString(), "HeaderStyle2"));
            #endregion

            #region Row 4
            row = sheet.Table.Rows.Add();
            row.Index = 4;

            DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
            row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
            row.Cells.Add(new WorksheetCell("Report Generated Date :", "HeaderStyle2"));
            row.Cells.Add(new WorksheetCell(System.DateTime.Now.ToString("dd/MM/yyyy"), "HeaderStyle2"));
            #endregion

            #region Row 5
            row = sheet.Table.Rows.Add();

            row.Index = 5;
            row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
            row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
            #endregion        
       
            strSqlString = "[ReportAllSatateComparison_WithStaggingData] '" + ddlStateName.SelectedValue + "','"+txtfrom.Text+"','"+txtTo.Text+"'";        
            DataTable dtResult = Utils.GetDataTable(strSqlString, strCnnString);

            string RTOColName = string.Empty;
            if (dtResult.Rows.Count > 0)
            {

                #region Row Column Creation
                row = sheet.Table.Rows.Add();
                row.Index = 6;
                for (int iColCount = 0; iColCount < dtResult.Columns.Count; iColCount++)
                {
                    row.Cells.Add(new WorksheetCell(dtResult.Columns[iColCount].ColumnName.ToString(), DataType.String, "HeaderStyle"));
                }               
                #endregion   

                row = sheet.Table.Rows.Add();

                #region Dynamic Row Creation
                for (int iRowCount=0;iRowCount<dtResult.Rows.Count;iRowCount++)
                {
                    for (int iColCount = 0; iColCount < dtResult.Columns.Count; iColCount++)
                    {
                        row.Cells.Add(new WorksheetCell(dtResult.Rows[iRowCount][iColCount].ToString(), DataType.String, "HeaderStyle"));
                    }
                    row = sheet.Table.Rows.Add();
                }
                #endregion

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
        private static void GenerateCell(PdfPTable table, int iSpan, int iLeftWidth, int iRightWidth, int iTopWidth, int iBottomWidth, int iAllign, int iFont, string strText, int iRowHeight, int iRowWidth)
        {
            PdfPCell newCellPDF = null;
            BaseFont bfTimes1 = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
            if (iFont.Equals(0))
            {
                newCellPDF = new PdfPCell(new Phrase(strText, new iTextSharp.text.Font(bfTimes1, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
            }
            else if (iFont.Equals(1))
            {
                newCellPDF = new PdfPCell(new Phrase(strText, new iTextSharp.text.Font(bfTimes1, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
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
                // newCellPDF.Width = iRowWidth;
            }
            table.AddCell(newCellPDF);
        }
        protected void btnstatus_Click(object sender, EventArgs e)
        {
            if (ddlStateName.SelectedItem.ToString() == "--Select State--")
            {
                lblError.Visible = true;
                lblError.Text = "Please Select State. ";
                return;
            }
            HttpContext context = HttpContext.Current;
            string filename = "Govt_ReceivedData" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".pdf";

            string SQLString = String.Empty;
            String StringField = String.Empty;
            String StringAlert = String.Empty;
            DateTime strRecievedDate ;

            string LastRecordDate = string.Empty;
            StringBuilder bb = new StringBuilder();

            //Creates an instance of the iTextSharp.text.Document-object:
            Document document = new Document();

            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);


            string PdfFolder = ConfigurationManager.AppSettings["PdfFolder"].ToString() + filename;
            PdfWriter.GetInstance(document, new FileStream(PdfFolder, FileMode.Create));

            //Opens the document:
            document.Open();
            string strQuery = "select ROW_NUMBER() over ( order by r.rtolocationname) as ID, r.RTOLocationName,s.LastRecorddate,h.VehicleRegNo,h.HSRPRecord_AuthorizationNo,h.HSRPRecord_authorizationdate  from HSRPRecordsStaggingArea h inner join(select RTOLocationID,max(datapulldate) as LastRecorddate from HSRPRecordsStaggingArea where hsrp_stateid='" + ddlStateName.SelectedValue.ToString() + "' group by RTOLocationID) s on s.RTOLocationID=h.RTOLocationID and h.datapulldate=s.LastRecorddate inner join RTOLocation as R on R.RTOLocationID =H.RTOLocationID and r.hsrp_stateid='" + ddlStateName.SelectedValue.ToString() + "' where h.hsrp_stateid='" + ddlStateName.SelectedValue.ToString() + "' order by r.RTOLocationName ";
            DataTable dtResult = Utils.GetDataTable(strQuery,strCnnString);
            PdfPTable table = new PdfPTable(11);

            //actual width of table in points
            table.TotalWidth = 1000f;
            
            GenerateCell(table, 11, 1, 1, 1, 1, 1, 0, "Govt Data Received Report", 30, 0);
            GenerateCell(table, 11, 1, 1, 0, 1, 1, 0, "State: " +ddlStateName.SelectedItem.ToString(), 30, 0);
            GenerateCell(table, 11, 1, 1, 0, 1, 2, 0, "Report Genrate Date: " + System.DateTime.Now.ToString("dd/MM/yyyy"), 30, 0);
            GenerateCell(table, 1, 1, 1, 0, 1, 1, 0, "SR.N.", 20, 0);
            GenerateCell(table, 2, 0, 1, 0, 1, 1, 0, "Location Name", 20, 0);
            GenerateCell(table, 2, 0, 1, 0, 1, 1, 0, "Vehicle No", 20, 0);
            GenerateCell(table, 2, 0, 1, 0, 1, 1, 0, "Auth No", 20, 0);
            GenerateCell(table, 2, 0, 1, 0, 1, 1, 0, "Auth Date", 20, 0);
            GenerateCell(table, 2, 0, 1, 0, 1, 1, 0, "Record Received Date", 20, 0);

            for (int ICount = 0; ICount < dtResult.Rows.Count; ICount++)
            {

                GenerateCell(table, 1, 1, 1, 0, 1, 1, 0, dtResult.Rows[ICount]["ID"].ToString(), 20, 0);
                GenerateCell(table, 2, 0, 1, 0, 1, 1, 0, dtResult.Rows[ICount]["RTOLocationName"].ToString(), 20, 0);
                GenerateCell(table, 2, 0, 1, 0, 1, 1, 0, dtResult.Rows[ICount]["VehicleRegNo"].ToString(), 20, 0);
                GenerateCell(table, 2, 0, 1, 0, 1, 1, 0, dtResult.Rows[ICount]["HSRPRecord_AuthorizationNo"].ToString(), 20, 0);
                GenerateCell(table, 2, 0, 1, 0, 1, 1, 0, dtResult.Rows[ICount]["HSRPRecord_authorizationdate"].ToString(), 20, 0);
                strRecievedDate= Convert.ToDateTime(dtResult.Rows[ICount]["LastRecorddate"]);
                LastRecordDate = strRecievedDate.ToString("dd/MM/yyyy HH:MM:ss");
                GenerateCell(table, 2, 0, 1, 0, 1, 1, 0, LastRecordDate, 20, 0);

               
               
            }
            GenerateCell(table, 10, 1, 1, 0, 1, 1, 0, "", 20, 0);
            document.Add(table);
            

            document.Close();


            context.Response.ContentType = "Application/pdf";
            context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
            context.Response.WriteFile(PdfFolder);
            context.Response.End();


        }
    }
}