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
using System.Data.SqlClient;

namespace HSRP.Report
{
    public partial class ProductionSheetStatusReport : System.Web.UI.Page
    {
        String strSqlString = String.Empty;
        string HsrpStateid = string.Empty;
        String strCnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        string UserType = string.Empty;
        string strUserID = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            HsrpStateid = Session["UserHSRPStateID"].ToString();
            UserType = Session["UserType"].ToString();
            strUserID = Session["UID"].ToString();
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
            int Total=0;
            int TotalPending=0;
            int TotalEmbossing=0;
            int GranTotal=0;
            HttpContext context = HttpContext.Current;
            string filename = "ProductionSheet_Status" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".pdf";

            string SQLString = String.Empty;
            String StringField = String.Empty;
            String StringAlert = String.Empty;
            DateTime strRecievedDate ;
            DateTime dateOrder;
            DateTime dateEmbossing;
            string strOrderDate = string.Empty;
            string strEmbossingDate = string.Empty;

            string LastRecordDate = string.Empty;
            StringBuilder bb = new StringBuilder();

            //Creates an instance of the iTextSharp.text.Document-object:
            Document document = new Document();

            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);


            string PdfFolder = ConfigurationManager.AppSettings["PdfFolder"].ToString() + filename;
            PdfWriter.GetInstance(document, new FileStream(PdfFolder, FileMode.Create));
            document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
            //Opens the document:
            document.Open();
            string strQuery = "exec ProductionSheetStatus '" + ddlStateName.SelectedValue.ToString() + "'";
            //string strQuery = " SELECT R.RTOLOCATIONNAME,T.pdfdownloaddate,T.PDFFILENAME,U.USERLOGINNAME "+
            //                  " FROM(select RTOLocationID,PDFFILENAME,PDFDownloadUSERID,max(pdfdownloaddate) AS pdfdownloaddate,"+
            //                            " ROW_NUMBER() over (PARTITION BY RTOLocationID order by max(pdfdownloaddate) DESC) AS SELECTEDROW "+
            //                            " from  HSRPRecords where hsrp_stateid='"+ddlStateName.SelectedValue.ToString()+"' "+
            //                            " GROUP BY RTOLocationID,PDFFILENAME,PDFDownloadUSERID) T "+
            //                  " LEFT JOIN RTOLOCATION R ON  R.RTOLocationID =T.RTOLocationID "+
            //                   " LEFT JOIN USERS U ON U.USERID=T.PDFDownloadUSERID "+
            //                    " WHERE T.SELECTEDROW=1 "+
            //                     " ORDER BY RTOLOCATIONNAME ";
            DataTable dtResult = Utils.GetDataTable(strQuery, strCnnString);


            string Sql = "select * from users where userid='" + strUserID + "'";
            DataTable dtUser = Utils.GetDataTable(Sql,strCnnString);
            string User = dtUser.Rows[0]["UserLoginName"].ToString();
            PdfPTable table = new PdfPTable(22);

            //actual width of table in points
            table.TotalWidth = 800f;
            table.LockedWidth = true;
            
            GenerateCell(table, 22, 1, 1, 1, 1, 1, 0, "Production Sheet Status", 30, 0);
            GenerateCell(table, 22, 1, 1, 0, 1, 1, 0, "State: " +ddlStateName.SelectedItem.ToString(), 30, 0);
            GenerateCell(table, 11, 1, 1, 1, 1, 0, 0, "Report Genrate Date: " + System.DateTime.Now.ToString(), 30, 0);
            GenerateCell(table, 11, 1, 1, 1, 1, 2, 0, "Generated By :" + User, 30, 0);
            GenerateCell(table, 1, 1, 1, 0, 1, 1, 0, "SR.N.", 50, 0);
            GenerateCell(table, 2, 0, 1, 0, 1, 1, 0, "District", 50, 0);
            GenerateCell(table, 2, 0, 1, 0, 1, 1, 0, "Location Name", 50, 0);
            GenerateCell(table, 5, 0, 1, 0, 1, 1, 0, "Production SheetReleased Till", 50, 0);
            GenerateCell(table, 2, 0, 1, 0, 1, 1, 0, "Last Collection Date", 50, 0);
            GenerateCell(table, 2, 0, 1, 0, 1, 1, 0, "Download Cash Date", 50, 0);
            GenerateCell(table, 3, 0, 1, 0, 1, 1, 0, "Download By", 60, 0);
            GenerateCell(table, 1, 0, 1, 0, 1, 1, 0, "Total Record", 50, 0);
            GenerateCell(table, 1, 0, 1, 0, 1, 1, 0, "Embossed", 50, 0);
            GenerateCell(table, 1, 0, 1, 0, 1, 1, 0, "Pending", 50, 0);
            
            GenerateCell(table, 2, 0, 1, 0, 1, 1, 0, "Last EmbossingDate", 50, 0);
            

            for (int ICount = 0; ICount < dtResult.Rows.Count; ICount++)
            {

                GenerateCell(table, 1, 1, 1, 0, 1, 1, 0,Convert.ToString(ICount+1), 20, 0);
                GenerateCell(table, 2, 0, 1, 0, 1, 1, 0, dtResult.Rows[ICount]["District"].ToString(), 20, 0);
                GenerateCell(table, 2, 0, 1, 0, 1, 1, 0, dtResult.Rows[ICount]["RTOLOCATIONNAME"].ToString(), 20, 0);
                GenerateCell(table, 5, 0, 1, 0, 1, 1, 0, dtResult.Rows[ICount]["MAXAFFIXDATE"].ToString(), 20, 0);

                dateOrder = Convert.ToDateTime(dtResult.Rows[ICount]["orderdate"]);
                strOrderDate = dateOrder.ToString("dd/MM/yyyy");
                GenerateCell(table, 2, 0, 1, 0, 1, 1, 0, strOrderDate, 20, 0);

                if (string.IsNullOrEmpty(dtResult.Rows[ICount]["pdfdownloaddate"].ToString()))
                {
                    GenerateCell(table, 2, 0, 1, 0, 1, 1, 0, "", 20, 0);
                }
                else
                {
                    strRecievedDate = Convert.ToDateTime(dtResult.Rows[ICount]["pdfdownloaddate"]);
                    LastRecordDate = strRecievedDate.ToString("dd/MM/yyyy HH:MM:ss");
                    GenerateCell(table, 2, 0, 1, 0, 1, 1, 0, LastRecordDate, 20, 0);
                }
                
                GenerateCell(table, 3, 0, 1, 0, 1, 1, 0, dtResult.Rows[ICount]["USERLOGINNAME"].ToString(), 30, 0);
                GenerateCell(table, 1, 0, 1, 0, 1, 1, 0, dtResult.Rows[ICount]["TOTAL"].ToString(), 20, 0);
                GenerateCell(table, 1, 0, 1, 0, 1, 1, 0, dtResult.Rows[ICount]["EMBOSSED"].ToString(), 20, 0);
                GenerateCell(table, 1, 0, 1, 0, 1, 1, 0, dtResult.Rows[ICount]["PENDING"].ToString(), 20, 0);

               
                if (string.IsNullOrEmpty(dtResult.Rows[ICount]["orderEmbossingDate"].ToString()))
                {
                    GenerateCell(table, 2, 0, 1, 0, 1, 1, 0, "", 20, 0);
                }
                else
                {
                    dateEmbossing = Convert.ToDateTime(dtResult.Rows[ICount]["orderEmbossingDate"]);
                    strEmbossingDate = dateEmbossing.ToString("dd/MM/yyyy");
                    GenerateCell(table, 2, 0, 1, 0, 1, 1, 0, strEmbossingDate, 20, 0);
                }
                
                    
                
               TotalEmbossing=TotalEmbossing+ Convert.ToInt16(dtResult.Rows[ICount]["EMBOSSED"]);
               TotalPending=TotalPending+Convert.ToInt16(dtResult.Rows[ICount]["PENDING"]);

               Total = Total + Convert.ToInt16(dtResult.Rows[ICount]["TOTAL"]);
               //GranTotal = TotalEmbossing + TotalPending + Total;
               
            }
            GenerateCell(table, 1, 1, 1, 0, 1, 1, 0, "Grand Total", 20, 0);
            GenerateCell(table, 3, 0, 1, 0, 1, 1, 0, "", 20, 0);
            GenerateCell(table, 5, 0, 1, 0, 1, 1, 0, "", 20, 0);
            GenerateCell(table, 2, 0, 1, 0, 1, 1, 0, "", 20, 0);
            GenerateCell(table, 3, 0, 1, 0, 1, 1, 0, "", 20, 0);
            GenerateCell(table, 1, 0, 1, 0, 1, 1, 0, Total.ToString(), 20, 0);
            GenerateCell(table, 1, 0, 1, 0, 1, 1, 0, TotalEmbossing.ToString(), 20, 0);
            GenerateCell(table, 1, 0, 1, 0, 1, 1, 0, TotalPending.ToString(), 20, 0);
            GenerateCell(table, 2, 0, 1, 0, 1, 1, 0, "", 20, 0);
            GenerateCell(table, 2, 0, 1, 0, 1, 1, 0, "", 20, 0);
            //GenerateCell(table, 20, 1, 1, 0, 1, 1, 0, "", 20, 0);
            document.Add(table);
            

            document.Close();


            context.Response.ContentType = "Application/pdf";
            context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
            context.Response.WriteFile(PdfFolder);
            context.Response.End();


        }
    }
}