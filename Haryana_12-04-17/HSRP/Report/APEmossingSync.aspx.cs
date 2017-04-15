using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using HSRP;
using System.Data;
using DataProvider;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Configuration;
using System.IO;
using CarlosAg.ExcelXmlWriter;
using System.Drawing;
using System.Globalization;


namespace HSRP.Report
{
    public partial class APEmossingSync : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        string UserType = string.Empty;
        string HSRPStateID = string.Empty;
        string RTOLocationID = string.Empty;
        int intHSRPStateID;
        string trnasportname,pp;
        string transtr, statename1;
        BaseFont basefont;
        string fontpath;
        string  strComplaintID =string.Empty;
        string strSql = string.Empty;

        string AllLocation = string.Empty;
         string OrderStatus = string.Empty;
         DateTime AuthorizationDate;
         DateTime OrderDate1;
         DataProvider.BAL bl = new DataProvider.BAL();
         string StickerManditory = string.Empty;

         string SubmitId = string.Empty;
         string QrySubmitID = string.Empty;

         string State_ID = string.Empty;
         string RTO_ID = string.Empty;
         string HSRPStateIDEdit = string.Empty;
         string RTOLocationIDEdit = string.Empty;
         string fromdate = string.Empty;
         string ToDate = string.Empty;
         string strSqlGo = string.Empty;
         string strsqlgonew = string.Empty;
         DataTable dtcount = new DataTable();
         DataTable dtshow = new DataTable();

         
         DataTable dt = new DataTable();


        protected void Page_Load(object sender, EventArgs e)
        {
            Utils.GZipEncodePage();
            
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {
                if (Session["UserHSRPStateID"].ToString() == "6")
                {
                   // ButImpData.Visible = true;
                }
                else
                {
                   // ButImpData.Visible = false;
                }

                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
               
                RTOLocationID = Session["UserRTOLocationID"].ToString();
                UserType = Session["UserType"].ToString();
                HSRPStateID = Session["UserHSRPStateID"].ToString();

                lblErrMsg.Text = string.Empty;
                strUserID = Session["UID"].ToString();
                ComputerIP = Request.UserHostAddress;
                 
                if (!IsPostBack)
                {
                      InitialSetting();

                     
                    try
                    { 
                        if (UserType == "0")
                        {
                           
                           
                        }
                        else
                        { 
                            hiddenUserType.Value = "1";
                           
                           
                        }
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
           // HSRPAuthDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
           // HSRPAuthDate.MaxDate = DateTime.Parse(MaxDate);
            CalendarHSRPAuthDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarHSRPAuthDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            OrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            OrderDate.MaxDate = DateTime.Parse(MaxDate);
            CalendarOrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarOrderDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
        }

        public void BindData()
        {
            fromdate = OrderDate.SelectedDate.ToString("yyyy/MM/dd");
            ToDate = OrderDate.SelectedDate.ToString("yyyy/MM/dd 23:59:59");

            //strsqlgonew = "select convert(varchar(15),HSRPRecord_CreationDate,103) as Date,HSRPRecord_AuthorizationNo,VehicleRegNo,OwnerName,EngineNo,ChassisNo,TotalAmount,Remarks from hsrprecords where hsrp_stateid=9 and hsrprecord_creationdate between '" + fromdate + "' and '" + ToDate + "' order by HSRPRecord_AuthorizationNo";
            strsqlgonew = "select b.RTOLocationName as RTO,count(a.vehicleregno) as OrderCount,Sum(CASE WHEN (HSRP_Front_LaserCode is not null and HSRP_Front_LaserCode!='') OR ( HSRP_Rear_LaserCode is not null and HSRP_Rear_LaserCode!='') THEN 1 ELSE 0 END) AS AssignedCount from HSRPRecords a, rtolocation b where a.RTOLocationID=b.RTOLocationID and a.hsrp_stateid=9 and a.hsrprecord_creationdate between '"+fromdate+"' and '"+ToDate+"'  and isnull(vehicleregno,'')!=''  group by b.rtolocationname";
            dtshow = Utils.GetDataTable(strsqlgonew, CnnString);
            grdpending.DataSource = dtshow;
            grdpending.DataBind();
                   
        
        }

        

        protected void btnGo_Click(object sender, EventArgs e)
        {
            try
            {
                lblSucessMsg.Text = "";
                if (OrderDate.SelectedDate.ToString() =="")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('Please Select Date..');", true);
                    return;

                }
                else
                {

                    BindData();
 
                }

            }
            catch(Exception ex)
            {

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
                newCellPDF = new PdfPCell(new Phrase(strText, new iTextSharp.text.Font(bfTimes1, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
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
            string FromDate = OrderDate.SelectedDate.ToString("yyyy/MM/dd") + " 00:00:00"; // Convert.ToDateTime();
            string ToDate = OrderDate.SelectedDate.ToString("yyyy/MM/dd") + " 23:59:59";

            HttpContext context = HttpContext.Current;
            string filename = "AP_Cash_Summary" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".pdf";
            string SQLString = String.Empty;
            Document document = new Document();
            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);


            string PdfFolder = ConfigurationManager.AppSettings["PdfFolder"].ToString() + filename;
            PdfWriter.GetInstance(document, new FileStream(PdfFolder, FileMode.Create));
            // document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
            document.Open();
            PdfPTable table = new PdfPTable(8);
            var colWidthPercentages = new[] { 30f,  50f, 50f, 50f, 50f, 50f, 50f, 50f };
            table.SetWidths(colWidthPercentages);
            //string sqlquery="select distrelation from rtolocation where rtolocationid='"+
            string SqlQuery = string.Empty;


            SqlQuery = "select convert(varchar(15),HSRPRecord_CreationDate,103) as Date,HSRPRecord_AuthorizationNo,VehicleRegNo,OwnerName,EngineNo,ChassisNo,TotalAmount,Remarks from hsrprecords where hsrp_stateid=9 and hsrprecord_creationdate between '" + FromDate + "' and '" + ToDate + "' order by HSRPRecord_AuthorizationNo";
            DataTable dt = Utils.GetDataTable(SqlQuery, CnnString);


            string Sql = "select * from users where userid='" + strUserID + "'";
            DataTable dtUser = Utils.GetDataTable(Sql, CnnString);
            string User = dtUser.Rows[0]["UserLoginName"].ToString();


            table.TotalWidth = 550f;
            table.LockedWidth = true;
             GenerateCell(table, 8, 0, 0, 0, 0, 1, 0, "AP Cash Summary", 20, 0);
             GenerateCell(table, 8, 0, 0, 0, 0, 2, 0, "Generated By :" + User, 20, 0);
            // GenerateCell(table, 8, 0, 0, 0, 0, 0, 0, "SMS Report", 20, 0);
            // GenerateCell(table, 16, 0, 0, 0, 0, 0, 0, "State Name : HIMACHAL PRADESH                 " + "RTO Location Name:" + DropDownListStateName.SelectedItem.ToString(), 20, 0);
            GenerateCell(table, 8, 0, 0, 0, 0, 0, 0, "Date Period  :" + OrderDate.SelectedDate.ToString("dd/MM/yyyy"), 20, 0);
            GenerateCell(table, 8, 0, 0, 0, 0, 0, 0, "Generated Date time: " + System.DateTime.Now.ToString(), 20, 0);
            GenerateCell(table, 1, 1, 1, 1, 1, 1, 0, "S.No:", 20, 0);
           // GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Date", 20, 0);
            GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Authorization No", 20, 0);
            GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "VehicleReg No", 20, 0);
            GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "OwnerName", 20, 0);

            GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Engine No", 20, 0);
            GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Chassis No", 20, 0);
            GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Amount", 20, 0);
            GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Remarks", 20, 0);
            int SnoCounter = 0;




            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SnoCounter = SnoCounter + 1;
                GenerateCell(table, 1, 1, 1, 0, 1, 1, 1, SnoCounter.ToString(), 20, 10);
              //  GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dt.Rows[i]["Date"].ToString(), 20, 10);
                GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dt.Rows[i]["HSRPRecord_AuthorizationNo"].ToString(), 20, 10);
                GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dt.Rows[i]["VehicleRegNo"].ToString(), 20, 10);
                GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dt.Rows[i]["OwnerName"].ToString(), 20, 10);

                GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dt.Rows[i]["EngineNo"].ToString(), 20, 10);
                GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dt.Rows[i]["ChassisNo"].ToString(), 20, 10);

                GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dt.Rows[i]["TotalAmount"].ToString(), 20, 10);
                GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dt.Rows[i]["Remarks"].ToString(), 20, 10);



            }
            document.Add(table);
            document.NewPage();

            document.Close();
            context.Response.ContentType = "Application/pdf";
            context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
            context.Response.WriteFile(PdfFolder);
            context.Response.End();


        }

        protected void btnpdf_Click(object sender, EventArgs e)
        {
            ExportToPDF();

        }

        protected void btnSync_Click(object sender, EventArgs e)
        {

        }

      

       

    }
}