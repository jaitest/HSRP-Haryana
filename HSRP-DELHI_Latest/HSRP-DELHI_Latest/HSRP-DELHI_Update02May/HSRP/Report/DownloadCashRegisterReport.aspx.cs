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


namespace HSRP.Report
{
    public partial class DownloadCashRegisterReport : System.Web.UI.Page
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
                    try
                    {
                        InitialSetting();
                        if (UserType1.Equals(0))
                        {
                            FilldropDownListOrganization();
                        }
                        else
                        {
                            FilldropDownListOrganization();
                            FilldropDownListRTO();
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
                Utils.PopulateDropDownList(DropDownListStateName, SQLString1.ToString(), CnnString1, "--Select State--");
            }
            else
            {
                SQLString1 = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID=" + HSRP_StateID1 + " and ActiveStatus='Y' Order by HSRPStateName";
                DataSet dts = Utils.getDataSet(SQLString1, CnnString1);
                DropDownListStateName.DataSource = dts;
                DropDownListStateName.DataBind();
            }
        }

        private void FilldropDownListRTO()
        {
            if (UserType1.Equals(0))
            {
                SQLString1 = "select RTOLocationid,Rtolocationname from Rtolocation  where hsrp_stateid='" + DropDownListStateName.SelectedValue.ToString() + "' and ActiveStatus='Y' Order by RtoLocationname";
                Utils.PopulateDropDownList(ddlRto, SQLString1.ToString(), CnnString1, "--Select RTO--");
            }
            else
            {
                SQLString1 = "select RTOLocationid,Rtolocationname from Rtolocation  where hsrp_stateid='" + HSRP_StateID1 + "' and ActiveStatus='Y' Order by RtoLocationname";
                Utils.PopulateDropDownList(ddlRto, SQLString1.ToString(), CnnString1, "--Select RTO--");
            }
        }

        private void FillUserDetails()
        {
            if (UserType1.Equals(0))
            {
                SQLString1 = "select UserLoginName,userid from users where rtolocationid='" + ddlRto.SelectedValue.ToString() + "' order by UserLoginName ";
                Utils.PopulateDropDownList(ddluser, SQLString1.ToString(), CnnString1, "--Select Cashier--");
            }
            else
            {

                SQLString1 = "select UserLoginName,userid from users where rtolocationid='" + ddlRto.SelectedValue.ToString() + "' and ActiveStatus='Y' and userid='" + strUserID1 + "' order by UserLoginName ";
                Utils.PopulateDropDownList(ddluser, SQLString1.ToString(), CnnString1, "--Select Cashier--");
            }
        }
        private void InitialSetting()
        {

            OrderDate.MinDate = new DateTime(2014, 09, 09);
            
            string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            string MaxDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();

            OrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            OrderDate.MaxDate = DateTime.Parse(MaxDate);
            CalendarOrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarOrderDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
        }

        private Boolean validate1()
        {
            Boolean blnvalid = true;
            String getvalue = string.Empty;
            getvalue = DropDownListStateName.SelectedItem.Text;
            if (getvalue == "--Select State--")
            {
                blnvalid = false;

                Label1.Text = "Please select State Name";

            }
            return blnvalid;

        }

        protected void btnPDF_Click(object sender, EventArgs e)
        {

            if (ddluser.SelectedItem.ToString() =="--Select Cashier--")
            {
                Label1.Visible = true;
                Label1.Text = "";
                Label1.Text = "Please Select Cashier.....";
                return;

            }

            DateTime FromDate = new DateTime(2014, 09, 09);
            string strFromDate = FromDate.ToString("yyyy-MM-dd");
            string strFromDateNew = strFromDate + " 00:00:00";
            DateTime Todate = OrderDate.SelectedDate.AddDays(-1);
            string strTodate = Todate.ToString("yyyy-MM-dd");
            string strToDateNew = strTodate  +  " 23:59:59";

            string SqlQuery = "exec GetOpeningBalanceForCashRegister '" + DropDownListStateName.SelectedValue + "','" + ddluser.SelectedValue.ToString() + "','" + strFromDateNew + "','" + strToDateNew + "'";
            DataTable dtOpeningCash = Utils.GetDataTable(SqlQuery, CnnString1);
            string strOpening = dtOpeningCash.Rows[0]["OpeningBalance"].ToString();
            #region FrontQuery

            #endregion

            #region Sql Query For Report
            SQLString1 = "exec ReportCashInHandOpeningBalanceNew '" + DropDownListStateName.SelectedValue + "','" + ddlRto.SelectedValue + "','" + OrderDate.SelectedDate.ToString("yyyy-MM-dd") + "','" +ddluser.SelectedValue.ToString() + "'";
            #endregion
            DataTable dtRecord = Utils.GetDataTable(SQLString1, CnnString1);

            if (dtRecord.Rows.Count > 0)
            {

            #region Common Code For PDf
            string filename = ddlRto.SelectedItem.ToString() + "_CashRegister_" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".pdf";

            iTextSharp.text.Document document = new iTextSharp.text.Document();

            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);

            // iTextSharp.text.Font times = new Font(bfTimes, 12, Font.ITALIC, Color.Black);
            //Creates a Writer that listens to this document and writes the document to the Stream of your choice:
            string PdfFolder = ConfigurationManager.AppSettings["PdfFolder"].ToString() + filename;
            PdfWriter.GetInstance(document, new FileStream(PdfFolder, FileMode.Create));

            //Opens the document:
            document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
            document.Open();


            PdfPTable table1 = new PdfPTable(9);

            var colWidthPercentages = new[] { 25f, 80f, 25f, 25f, 30f, 40f, 50f, 50f, 40f };
            table1.SetWidths(colWidthPercentages);

            table1.TotalWidth = 820f;
            table1.LockedWidth = true;
            #endregion

            String strReportDate = OrderDate.SelectedDate.ToString("dd-MM-yyyy");
            //SQLString1 = "select convert(numeric,OpeningBalance) as OpeningBalance  from dbo.CashInHandOpeningBalance where Hsrp_stateid='" + DropDownListStateName.SelectedValue.ToString() + "' and Rtolocationid='" + ddlRto.SelectedValue.ToString() + "'";
            //String StrOpeningBalance = Utils.getScalarValue(SQLString1, CnnString1);
            var varTotalAmount = dtRecord.Compute("Sum(Total)","vehicletype <> ''");
            int ITotalAmount = Convert.ToInt32(varTotalAmount);
            if (string.IsNullOrEmpty(strOpening))
            {
                strOpening = "0";
            }
            int strFinalAmount = (int.Parse(strOpening) + ITotalAmount);

            #region Fixed Rows Part 1
            #region PDF Header
            PDFRows(bfTimes, table1, 2, "RTOLocation:    " + ddlRto.SelectedItem.ToString(), 8, 1, "B", 0, 0, 0, 0);
            PDFRows(bfTimes, table1, 2, "CashierName:    " +  ddluser.SelectedItem.ToString(), 8, 1, "B", 0, 0, 0, 0);
            PDFRows(bfTimes, table1, 2, "ReportDateTime:  " +  System.DateTime.Now, 8, 1, "B", 0, 0, 0, 0);
            PDFRows(bfTimes, table1, 2, "Print No:", 8, 1, "B", 0, 0, 0, 0);
            PDFRows(bfTimes, table1, 1, "FormSr.No:", 8, 1, "B", 0, 0, 0, 0);
            #endregion
            #region Row 1
            PDFRows(bfTimes, table1, 9, "CASH REGISTER", 8, 1, "B", 1, 1, 1, 1);
            #endregion
            #region Row 2

            PDFRows(bfTimes, table1, 1, "", 8, 0, "N", 1, 1, 0, 1, optionalWidth: 10);
            PDFRows(bfTimes, table1, 1, "", 8, 0, "N", 0, 1, 0, 1);
            PDFRows(bfTimes, table1, 1, "", 8, 0, "N", 0, 1, 0, 1);
            PDFRows(bfTimes, table1, 1, "", 8, 0, "N", 0, 1, 0, 1);
            PDFRows(bfTimes, table1, 1, "", 8, 0, "N", 0, 1, 0, 1, optionalWidth: 10);
            PDFRows(bfTimes, table1, 1, "CR.", 8,1, "B", 0, 1, 0, 1);
            PDFRows(bfTimes, table1, 1, "", 8, 0, "N", 0, 1, 0, 1);
            PDFRows(bfTimes, table1, 1, "", 8, 0, "N", 0, 1, 0, 1);
            PDFRows(bfTimes, table1, 1, "DR.", 8, 1, "B", 0, 1, 0, 1, optionalWidth: 10);

            #endregion
            #region Row 3
            PDFRows(bfTimes, table1, 1, "Month & Date", 8, 0, "B", 1, 1, 0, 1, optionalWidth: 10);
            PDFRows(bfTimes, table1, 3, "Particulars", 8, 1, "B", 0, 1, 0, 1);
            PDFRows(bfTimes, table1, 1, "", 8, 0, "N", 0, 1, 0, 1);
            PDFRows(bfTimes, table1, 1, "Cash", 8, 1, "B", 0, 1, 0, 1);
            PDFRows(bfTimes, table1, 1, "Month & Date", 8, 0, "B", 0, 1, 0, 1, optionalWidth: 10);
            PDFRows(bfTimes, table1, 1, "Particulars", 8, 0, "B", 0, 1, 0, 1);
            PDFRows(bfTimes, table1, 1, "Cash", 8, 1, "B", 0, 1, 0, 1);
            #endregion
            #region Row 4
            PDFRows(bfTimes, table1, 1, strReportDate, 8, 1, "N", 1, 1, 0, 1, optionalWidth: 10);
            PDFRows(bfTimes, table1, 1, "OPENING CASH", 8, 1, "B", 0, 1, 0, 1);
            PDFRows(bfTimes, table1, 1, "", 8, 0, "N", 0, 1, 0, 1);
            PDFRows(bfTimes, table1, 1, "", 8, 0, "N", 0, 1, 0, 1);
            PDFRows(bfTimes, table1, 1, "", 8, 0, "N", 0, 1, 0, 1, optionalWidth: 10);
            PDFRows(bfTimes, table1, 1, strOpening, 8, 1, "B", 0, 1, 0, 1);  //Opening Balance
            PDFRows(bfTimes, table1, 1, strReportDate, 8, 1, "B", 0, 1, 0,1);
            PDFRows(bfTimes, table1, 1, "", 8, 0, "N", 0, 1, 0, 1);
            PDFRows(bfTimes, table1, 1, "", 8, 0, "N", 0, 1, 0, 1, optionalWidth: 10);
            #endregion
            #region Row 5
            PDFRows(bfTimes, table1, 1, "", 8, 0, "N", 1, 1, 0, 1, optionalWidth: 10);
            PDFRows(bfTimes, table1, 1, "Revenue", 8, 1, "B", 0, 1, 0, 1);
            PDFRows(bfTimes, table1, 1, "QTY.", 8, 1, "B", 0, 1, 0, 1);
            PDFRows(bfTimes, table1, 1, "RATE", 8, 1, "B", 0, 1, 0, 1);
            PDFRows(bfTimes, table1, 1, "Total", 8, 1, "B", 0, 1, 0, 1, optionalWidth: 10);
            PDFRows(bfTimes, table1, 1, "", 8, 0, "N", 0, 1, 0, 1);
            PDFRows(bfTimes, table1, 1, "", 8, 0, "N", 0, 1, 0, 1);
            PDFRows(bfTimes, table1, 1, "", 8, 0, "N", 0, 1, 0, 1);
            PDFRows(bfTimes, table1, 1, "", 8, 0, "N", 0, 1, 0, 1, optionalWidth: 10);
            #endregion
            #endregion

            #region Dynamics Rows
            for (int iRowCount = 0; iRowCount < dtRecord.Rows.Count; iRowCount++)// dtRecord.Rows.Count; iRowCount++)
            {
                //
                PDFRows(bfTimes, table1, 1, (iRowCount + 1).ToString(), 8, 1, "N",1, 1, 0, 0, optionalWidth: 10);
                PDFRows(bfTimes, table1, 1, dtRecord.Rows[iRowCount]["VehicleType"].ToString(), 8, 0, "N", 0, 1, 0, 0);
                PDFRows(bfTimes, table1, 1, dtRecord.Rows[iRowCount]["Qty"].ToString(), 8, 2, "N", 0, 1, 0, 0);
                PDFRows(bfTimes, table1, 1, dtRecord.Rows[iRowCount]["rate"].ToString(), 8, 2, "N", 0, 1, 0, 0);
                PDFRows(bfTimes, table1, 1, dtRecord.Rows[iRowCount]["total"].ToString(), 8, 2, "N", 0, 1, 0, 0);
                PDFRows(bfTimes, table1, 1, "", 8, 0, "N", 0, 1, 0, 0);
                PDFRows(bfTimes, table1, 1, "", 8, 0, "N", 0, 1, 0, 1);
                PDFRows(bfTimes, table1, 1, "", 8, 0, "N", 0, 1, 0, 1);
                PDFRows(bfTimes, table1, 1, "", 8, 0, "N", 0, 1, 0, 1);
            }
            #endregion

            #region Fixed Rows Part 2
            #region Row 1
            PDFRows(bfTimes, table1, 1, " ", 8, 0, "N", 1, 1, 1, 1, optionalWidth: 10);
            PDFRows(bfTimes, table1, 1, "Collection during the day", 8, 0, "B", 0, 1, 1, 1);
            PDFRows(bfTimes, table1, 1, "", 8, 0, "N", 0, 1, 1, 1);
            PDFRows(bfTimes, table1, 1, "", 8, 0, "N", 0, 1, 1, 1);
            PDFRows(bfTimes, table1, 1, "", 8, 0, "N", 0, 1, 1, 1, optionalWidth: 10);
            PDFRows(bfTimes, table1, 1, ITotalAmount.ToString(), 8, 1, "B", 0, 1, 1, 1);
            PDFRows(bfTimes, table1, 1, "", 8, 0, "B", 0, 1, 0, 1);
            PDFRows(bfTimes, table1, 1, "Cash Deposited in Bank", 8, 0, "N", 0, 1, 0, 1);
            PDFRows(bfTimes, table1, 1, dtRecord.Rows[0]["BankDeposit"].ToString(), 8, 1, "B", 0, 1, 0, 1, optionalWidth: 10);
            #endregion
            Int64 IntBankDeposit = Convert.ToInt64(dtRecord.Rows[0]["BankDeposit"]);
            Int64 IntCashInHand = strFinalAmount - IntBankDeposit;
            #region Row 2 (3 Blank Rows)
            for (int ICount = 0; ICount < 3; ICount++)
            {
                PDFRows(bfTimes, table1, 1, "   ", 8, 0, "N", 1, 1, 0, 0);

                for (int IRowCount = 0; IRowCount < 8; IRowCount++)
                {
                    PDFRows(bfTimes, table1, 1, "   ", 8, 0, "N", 0, 1, 0, 0);
                }
            }
            #endregion
            //#region Row 3
            //PDFRows(bfTimes, table1, 1, "", 8, 0, "N", 1, 1, 0, 1);
            //PDFRows(bfTimes, table1, 2, "", 8, 0, "B", 1, 1, 0, 1, optionalWidth: 10);
            //PDFRows(bfTimes, table1, 1, "", 8, 0, "N", 0, 1, 0, 1);
            //PDFRows(bfTimes, table1, 1, "", 8, 0, "N", 0, 1, 0, 1);            
            //PDFRows(bfTimes, table1, 1, "", 8, 0, "N", 0, 1, 0, 1);
            //PDFRows(bfTimes, table1, 1, "", 8, 0, "N", 0, 1, 0, 1);
            //PDFRows(bfTimes, table1, 1, "", 8, 0, "N", 0, 1, 0, 1, optionalWidth: 10);
            //PDFRows(bfTimes, table1, 1, "", 8, 0, "N", 0, 1, 0, 1);            
           // #endregion
            #region Row 4
            PDFRows(bfTimes, table1, 1, "  ", 8, 0, "N", 1, 1, 0, 1, optionalWidth: 10);
            PDFRows(bfTimes, table1, 1, "    ", 8, 0, "N", 0, 1, 0, 1);
            PDFRows(bfTimes, table1, 1, "   ", 8, 0, "N", 0, 1, 0, 1);
            PDFRows(bfTimes, table1, 1, "  ", 8, 0, "N", 0, 1, 0, 1);
            PDFRows(bfTimes, table1, 1, "  ", 8, 0, "N", 0, 1, 0, 1, optionalWidth: 10);
            PDFRows(bfTimes, table1, 1, "  ", 8, 0, "N", 0, 1, 0, 1);
            PDFRows(bfTimes, table1, 1, "  ", 8, 0, "N", 0, 1, 0, 1);
            PDFRows(bfTimes, table1, 1, "  ", 8, 0, "N", 0, 1, 0, 1);
            PDFRows(bfTimes, table1, 1, ITotalAmount.ToString(), 8, 1, "B", 0, 1, 0, 1, optionalWidth: 10);
            #endregion
            #region Row 5
            PDFRows(bfTimes, table1, 1, "", 8, 0, "N",1, 1, 0, 1, optionalWidth: 10);
            PDFRows(bfTimes, table1, 1, "", 8, 0, "N", 0, 1, 0, 1);
            PDFRows(bfTimes, table1, 1, "", 8, 0, "N", 0, 1, 0, 1);
            PDFRows(bfTimes, table1, 1, "", 8, 0, "N", 0, 1, 0, 1);
            PDFRows(bfTimes, table1, 1, "", 8, 0, "N", 0, 1, 0, 1, optionalWidth: 10);
            PDFRows(bfTimes, table1, 1, "", 8, 0, "N", 0, 1, 0, 1);
            PDFRows(bfTimes, table1, 1, "", 8, 0, "N", 0, 1, 0, 1);
            PDFRows(bfTimes, table1, 1, "Cash in hand C/F", 8, 0, "B", 0, 1, 0, 1);
            PDFRows(bfTimes, table1, 1, IntCashInHand.ToString(), 8, 1, "B", 0, 1, 0, 1, optionalWidth: 10);
            #endregion
            #region Row 6
            //PDFRows(bfTimes, table1, 1, "", 8, 0, "N", 1, 1, 0, 1, optionalWidth: 10);
            //PDFRows(bfTimes, table1, 1, "", 8, 0, "N", 0, 1, 0, 1);
            //PDFRows(bfTimes, table1, 1, "", 8, 0, "N", 0, 1, 0, 1);
            //PDFRows(bfTimes, table1, 1, "", 8, 0, "N", 0, 1, 0, 1);
            //PDFRows(bfTimes, table1, 1, "", 8, 0, "N", 0, 1, 0, 1, optionalWidth: 10);
            //PDFRows(bfTimes, table1, 1, "", 8, 1, "B", 0, 1, 0, 1); //FinalAmount
            //PDFRows(bfTimes, table1, 1, "", 8, 0, "N", 0, 1, 0, 1);
            //PDFRows(bfTimes, table1, 1, "", 8, 0, "N", 0, 1, 0, 1);
            //PDFRows(bfTimes, table1, 1, "", 8, 1, "B", 0, 1, 0, 1, optionalWidth: 10); //Final Amount
            #endregion
            #region Row 7
            PDFRows(bfTimes, table1, 1, "", 8, 0, "N", 1, 1, 0, 1);            
            PDFRows(bfTimes, table1, 1, "BANK DEPOSIT SLIP", 8, 0, "B", 0, 1, 0, 1, optionalWidth: 10);
            PDFRows(bfTimes, table1, 1, "", 8, 0, "N", 0, 1, 0, 1);
            PDFRows(bfTimes, table1, 1, "", 8, 0, "N", 0, 1, 0, 1);
            PDFRows(bfTimes, table1, 1, "", 8, 0, "N", 0, 1, 0, 1);
            PDFRows(bfTimes, table1, 1, "", 8, 0, "N", 0, 1, 0, 1, optionalWidth: 10);
            PDFRows(bfTimes, table1, 1, "", 8, 0, "N", 0, 1, 0, 1);
            PDFRows(bfTimes, table1, 1, "", 8, 0, "N", 0, 1, 0, 1);
            PDFRows(bfTimes, table1, 1, "", 8, 0, "N", 0, 1, 0, 1);
            #endregion
            #region Row 8
            PDFRows(bfTimes, table1, 1, "    ", 8, 0, "N", 1, 1, 0, 1);
            PDFRows(bfTimes, table1, 1, "    " ,8, 0, "N", 0, 1, 0, 0); 
          
            for (int iRowCount = 0; iRowCount < 7; iRowCount++)
            {
                PDFRows(bfTimes, table1, 1, "", 8, 0, "N", 0, 1, 0, 1, optionalWidth: 10);
            }
            #endregion
            #region Row 9
            PDFRows(bfTimes, table1, 1, "", 8, 0, "N", 1, 1, 0, 1);
            PDFRows(bfTimes, table1, 1, "", 8, 0, "N", 0, 1, 0, 0);
            for (int iRowCount = 0; iRowCount < 4; iRowCount++)
            {
                PDFRows(bfTimes, table1, 1, "", 8, 0, "N", 0, 1, 0, 1);
            }
            PDFRows(bfTimes, table1, 3, "Cash handed over to Mr.", 8, 0, "B", 0, 1, 0, 1);
            #endregion
            #region Row 10
            PDFRows(bfTimes, table1, 1, "", 8, 0, "N", 1, 1, 0, 1);
            PDFRows(bfTimes, table1, 1, "", 8, 0, "N", 0, 1, 0, 0);
          
            for (int iRowCount = 0; iRowCount < 4; iRowCount++)
            {
                PDFRows(bfTimes, table1, 1, "", 8, 0, "N", 0, 1, 0, 1);
            }
            PDFRows(bfTimes, table1, 3, "For Deposit in Bank", 8, 0, "B", 0, 1, 0, 1);
            #endregion
            #region Row 11
            PDFRows(bfTimes, table1, 1, "   ", 8, 0, "N", 1, 1, 0, 1);
            PDFRows(bfTimes, table1, 1, "   ", 8, 0, "N", 0, 1, 0, 0);
            for (int iRowCount = 0; iRowCount < 7; iRowCount++)
            {
                PDFRows(bfTimes, table1, 1, "   ", 8, 0, "N", 0, 1, 0, 1, optionalWidth: 10);
            }
            PDFRows(bfTimes, table1, 1, "  ", 8, 0, "N", 1, 1, 0, 1);
            PDFRows(bfTimes, table1, 1, "  ", 8, 0, "N", 0, 1, 0, 0);
            for (int iRowCount = 0; iRowCount < 7; iRowCount++)
            {
                PDFRows(bfTimes, table1, 1, "   ", 8, 0, "N", 0, 1, 0, 1, optionalWidth: 10);
            }
            #endregion
            #region Row 12
            PDFRows(bfTimes, table1, 1, "   ", 8, 0, "N", 1, 1, 0, 1);
            PDFRows(bfTimes, table1, 1, "   ", 8, 0, "N", 0, 1, 0, 1);
          
            for (int iRowCount = 0; iRowCount < 4; iRowCount++)
            {
                PDFRows(bfTimes, table1, 1, "    ", 8, 0, "N", 0, 1, 0, 1);
            }
            PDFRows(bfTimes, table1, 3, "CASHIER SIGNATURE", 8, 0, "B", 0, 1, 0, 1);
            #endregion
            #region Row 13
            PDFRows(bfTimes, table1, 1, "   ", 8, 0, "N", 1, 1, 0, 1);
            PDFRows(bfTimes, table1, 1, "   ", 8, 0, "N", 0, 1,0, 1);          
            for (int iRowCount = 0; iRowCount < 7; iRowCount++)
            {
                PDFRows(bfTimes, table1, 1, "", 8, 0, "N", 0, 1, 0, 1, optionalWidth: 10);
            }
            #endregion
            #endregion

            document.Add(table1);
            document.Close();
            HttpContext context = HttpContext.Current;
            context.Response.ContentType = "Application/pdf";
            context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
            context.Response.WriteFile(PdfFolder);
            context.Response.End();

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

        protected void DropDownListStateName_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilldropDownListRTO();
        }

        protected void ddlRto_SelectedIndexChanged1(object sender, EventArgs e)
        {

            lblCashier.Visible = true;
            ddluser.Visible = true;
            FillUserDetails();

        }

       
    }
}