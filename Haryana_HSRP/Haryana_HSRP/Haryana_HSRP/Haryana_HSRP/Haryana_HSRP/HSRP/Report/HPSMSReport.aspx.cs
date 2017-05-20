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
using iTextSharp.text.pdf;
using iTextSharp.text;


namespace HSRP.Report
{
    public partial class HPSMSReport : System.Web.UI.Page
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
                            InitialSetting();
                            DropDownListStateName.Visible = true;
                            //dropDownListClient.Visible = true;
                            FilldropDownListOrganization();
                            //FilldropDownListClient();
                            //OrderDatefrom.SelectedDate = System.DateTime.Now;
                            //OrderDateto.SelectedDate = System.DateTime.Now;


                        }
                        else
                        {

                            // labelOrganization.Visible = true;
                            DropDownListStateName.Visible = true;
                            //dropDownListClient.Visible = true;
                            FilldropDownListOrganization();
                            //FilldropDownListClient();

                            //OrderDatefrom.SelectedDate = System.DateTime.Now;
                            //OrderDateto.SelectedDate = System.DateTime.Now;

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

            SQLString1 = "select RTOLocationName,RTOLocationID from RTOLocation  where HSRP_StateID='3' Order by RTOLocationName";
            Utils.PopulateDropDownList(DropDownListStateName, SQLString1.ToString(), CnnString1, "--Select Location--");


        }


        private void InitialSetting()
        {
            string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            string MaxDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            OrderDateto.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            OrderDateto.MaxDate = DateTime.Parse(MaxDate);
            OrderDateto.MinDate = DateTime.Parse("2014-08-01");
            CalendarOrderDateto.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarOrderDateto.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            OrderDatefrom.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            OrderDatefrom.MaxDate = DateTime.Parse(MaxDate);
            OrderDatefrom.MinDate = DateTime.Parse("2014-08-01");
            CalendarOrderDatefrom.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarOrderDatefrom.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);

        }

        protected void DropDownListStateName_SelectedIndexChanged(object sender, EventArgs e)
        {


        }

        protected void dropDownListClient_SelectedIndexChanged(object sender, EventArgs e)
        {
        }


        protected void DropDownListyearName_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private Boolean validate1()
        {
            Boolean blnvalid = true;
            String getvalue = string.Empty;
            getvalue = DropDownListStateName.SelectedItem.ToString();
            if (getvalue == "--Select Location--")
            {
                blnvalid = false;

                Label1.Text = "Please select RTO Name";

            }
            return blnvalid;

        }

        protected void btnexport_Click(object sender, EventArgs e)
        {
            validate1();
            if (validate1() == false)
            {
                return;
            }

            else
            {
                Label1.Visible = false;

                try
                {
                    string strSQLQuery = string.Empty;
                    string FromDate = OrderDatefrom.SelectedDate.ToString("yyyy/MM/dd") + " 00:00:00"; // Convert.ToDateTime();
                    string ToDate = OrderDateto.SelectedDate.ToString("yyyy/MM/dd") + " 23:59:59";
                    strSQLQuery = "select ROW_NUMBER() over (ORDER BY s.[MobileNo]) AS [S.No],s.[REGN_NO] as [Vehicle],h.OwnerName,s.[MobileNo] as Mobile,s.[CashReceiptSmsText] as [CashReceipt SMS],s.[CashReceiptSMSDateTime],s.[FirstSMSText] as [Embossing SMS],s.[FirstSMSSentDateTime] as EmbossingSMSDateTime,s.[SecondSMSText] as [Affixation SMS],s.[SecondSMSSentDateTime] as AffixationSMSDateTime,s.[ThirdSMSText] as [Reminder1 SMS],s.ThirdSMSSentDateTime as Reminder1SMSDateTime,s.[FourthSMSText] as [Reminder2 SMS],s.FourthSMSSentDateTime as Rmeinder2SMSDateTime,datediff(d,h.hsrprecord_creationdate,h.OrderClosedDate) as [Affixation Days from Collection],Refund from hsrprecords as h inner join [SMSlog_HP] as s on s.[REGN_NO]=h.vehicleregno where h.RTOLocationID='" + DropDownListStateName.SelectedValue.ToString() + "' and hsrprecord_creationdate between '" + FromDate + "' and '" + ToDate + "'";


                    DataTable dt = Utils.GetDataTable(strSQLQuery, CnnString1);
                    if (dt.Rows.Count > 0)
                    {
                        GridDetailsview.DataSource = dt;
                        GridDetailsview.DataBind();
                    }
                    else
                    {
                        Label1.Text = "No Record Found";
                    }
                }

                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        #region Pdf Function
        private static void PDFRows(BaseFont bfTimes, PdfPTable table1, int iColSpan, String strText, int iFontsize, int ialignMent, string strRowType, int iBorderWidthLeft, int iBorderWidthRight, int iBorderWidthTop, int iBorderWidthBottom, int optionalHeight = 0, int optionalWidth = 0)
        {
            PdfPCell cell;
            if (strRowType == "B")
            {
                cell = new PdfPCell(new iTextSharp.text.Phrase(strText, new iTextSharp.text.Font(bfTimes, iFontsize, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
            }
            else
            {
                cell = new PdfPCell(new iTextSharp.text.Phrase(strText, new iTextSharp.text.Font(bfTimes, iFontsize, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
            }
            cell.Colspan = iColSpan;
            cell.BorderColor = iTextSharp.text.BaseColor.BLACK;
            cell.BorderWidthLeft = iBorderWidthLeft;
            cell.BorderWidthRight = iBorderWidthRight;
            cell.BorderWidthTop = iBorderWidthTop;
            cell.BorderWidthBottom = iBorderWidthBottom;
            cell.NoWrap = false;
            cell.HorizontalAlignment = ialignMent; //0=Left, 1=Centre, 2=Right
            table1.AddCell(cell);
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
            }
            table.AddCell(newCellPDF);
        }
        private void ExportToPDF()
        {
            string FromDate = OrderDatefrom.SelectedDate.ToString("yyyy/MM/dd") + " 00:00:00"; // Convert.ToDateTime();
            string ToDate   = OrderDateto.SelectedDate.ToString("yyyy/MM/dd") + " 23:59:59";

            HttpContext context = HttpContext.Current;
            string filename = "HP_SMS_Report" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".pdf";
            string SQLString = String.Empty;
            Document document = new Document();
            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);


            string PdfFolder = ConfigurationManager.AppSettings["PdfFolder"].ToString() + filename;
            PdfWriter.GetInstance(document, new FileStream(PdfFolder, FileMode.Create));
            document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
            document.Open();
            PdfPTable table = new PdfPTable(16);
            var colWidthPercentages = new[] { 20f, 60f, 65f, 65f, 90f, 60f, 70f, 65f, 70f, 45f, 70f,70f,70f,70f,45f,45f };
            table.SetWidths(colWidthPercentages);
            //string sqlquery="select distrelation from rtolocation where rtolocationid='"+
            string SqlQuery = string.Empty;


            SqlQuery = "select ROW_NUMBER() over (ORDER BY s.[MobileNo]) AS [S.No],s.[REGN_NO] as [Vehicle RegNo],h.OwnerName,s.[MobileNo],s.[CashReceiptSmsText],s.[CashReceiptSMSDateTime],s.[FirstSMSText] as EmbossingSMSText,s.[FirstSMSSentDateTime] as EmbossingSMSDateTime,s.[SecondSMSText] as AffixationSMSText,s.[SecondSMSSentDateTime] as AffixationSMSDateTime,s.[ThirdSMSText] as Reminder1SMSText,s.ThirdSMSSentDateTime as Reminder1SMSDateTime,s.[FourthSMSText] as Reminder2SMSText,s.FourthSMSSentDateTime as Rmeinder2SMSDateTime,datediff(d,hsrprecord_creationdate,OrderClosedDate) as AffixationDate,Refund from hsrprecords as h inner join [SMSlog_HP] as s on s.[REGN_NO]=h.vehicleregno where  h.RTOLocationID='" + DropDownListStateName.SelectedValue.ToString() + "' and hsrprecord_creationdate between '"+FromDate+"' and '"+ToDate+"'";
            DataTable dt = Utils.GetDataTable(SqlQuery, CnnString1);

            table.TotalWidth = 800f;
            table.LockedWidth = true;
            //  GenerateCell(table, 10, 0, 0, 0, 0, 1, 1, "", 50, 0);
            GenerateCell(table, 8, 0, 0, 0, 0, 0, 0, "", 20, 0);
            GenerateCell(table, 8, 0, 0, 0, 0, 0, 0, "SMS Report", 20, 0);
            GenerateCell(table, 16, 0, 0, 0, 0, 0, 0, "State Name : HIMACHAL PRADESH                 "  +"RTO Location Name:"+ DropDownListStateName.SelectedItem.ToString(), 20, 0);
            GenerateCell(table, 16, 0, 0, 0, 0, 0, 0, "Date Period  :" + OrderDatefrom.SelectedDate.ToString() + "-" + OrderDateto.SelectedDate.ToString(), 15, 0);
            GenerateCell(table, 16, 0, 0, 0, 0, 0, 0, "Generated Date time: " + System.DateTime.Now.ToString(), 15, 0);
            GenerateCell(table, 1, 1, 1, 1, 1, 1, 0, "S.No:", 20, 0);
            GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Vehicle", 20, 0);
            GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Owner Name", 20, 0);
            GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Mobile", 20, 0);
            GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "CashReceipt SMS", 20, 0);

            GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "CashReceiptSMSDateTime", 30, 0);
            GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Embossing SMS", 20, 0);
            GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "EmbossingSMSDateTime", 30, 0);
            GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Affixation SMS", 20, 0);
            GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "AffixationSMSDateTime", 30, 0);
            GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Reminder1 SMS", 20, 0);
            GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Reminder1SMSDateTime", 30, 0);
            GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Reminder2 SMS", 20, 0);
            GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Rmeinder2SMSDateTime", 20, 0);
            GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Affixation Days from Collection", 50, 0);
            GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Refund", 20, 0);
            



            for (int i = 0; i < dt.Rows.Count; i++)
            {
                GenerateCell(table, 1, 1, 1, 0, 1, 1, 0, dt.Rows[i]["S.No"].ToString(), 20, 0);
                GenerateCell(table, 1, 0, 1, 0, 1, 1, 0, dt.Rows[i]["Vehicle RegNo"].ToString(), 60, 0);
                GenerateCell(table, 1, 0, 1, 0, 1, 1, 0, dt.Rows[i]["OwnerName"].ToString(), 30, 0);
                GenerateCell(table, 1, 0, 1, 0, 1, 1, 0, dt.Rows[i]["MobileNo"].ToString(), 30, 0);

                GenerateCell(table, 1, 0, 1, 0, 1, 1, 0, dt.Rows[i]["CashReceiptSmsText"].ToString(), 90, 0);
                GenerateCell(table, 1, 0, 1, 0, 1, 1, 0, dt.Rows[i]["CashReceiptSMSDateTime"].ToString(), 20, 0);

                GenerateCell(table, 1, 0, 1, 0, 1, 1, 0, dt.Rows[i]["EmbossingSMSText"].ToString(), 90, 0);
                GenerateCell(table, 1, 0, 1, 0, 1, 1, 0, dt.Rows[i]["EmbossingSMSDateTime"].ToString(), 20, 0);

                GenerateCell(table, 1, 0, 1, 0, 1, 1, 0, dt.Rows[i]["AffixationSMSText"].ToString(), 60, 0);
                GenerateCell(table, 1, 0, 1, 0, 1, 1, 0, dt.Rows[i]["AffixationSMSDateTime"].ToString(), 20, 0);
                GenerateCell(table, 1, 0, 1, 0, 1, 1, 0, dt.Rows[i]["Reminder1SMSText"].ToString(), 60, 0);

                GenerateCell(table, 1, 0, 1, 0, 1, 1, 0, dt.Rows[i]["Reminder1SMSDateTime"].ToString(), 20, 0);
                GenerateCell(table, 1, 0, 1, 0, 1, 1, 0, dt.Rows[i]["Reminder2SMSText"].ToString(), 60, 0);
                GenerateCell(table, 1, 0, 1, 0, 1, 1, 0, dt.Rows[i]["Rmeinder2SMSDateTime"].ToString(), 20, 0);
                GenerateCell(table, 1, 0, 1, 0, 1, 1, 0, dt.Rows[i]["AffixationDate"].ToString(), 20, 0);
                GenerateCell(table, 1, 0, 1, 0, 1, 1, 0, dt.Rows[i]["Refund"].ToString(), 20, 0);
                




            }
            document.Add(table);
            document.NewPage();

            document.Close();
            context.Response.ContentType = "Application/pdf";
            context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
            context.Response.WriteFile(PdfFolder);
            context.Response.End();


        }
        protected void btnPDF_Click(object sender, EventArgs e)
        {

            validate1();
            if (validate1() == false)
            {
                Label1.Visible = true;
                return;
            }

            else
            {
                Label1.Visible = false;

                try
                {
                    ExportToPDF();

                }

                catch (Exception ex)
                {
                    throw ex;
                }
            }

        }
    }
}
    



        


    

