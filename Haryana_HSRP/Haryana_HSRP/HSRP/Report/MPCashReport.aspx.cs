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
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Configuration;
using System.Globalization;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Net;
using System.Threading;
using System.IO;


namespace HSRP.Report
{
    public partial class MPCashReport : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string SQLString1 = string.Empty;
        string SQLString2 = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        int UserType;
        string HSRPStateID = string.Empty;

        string strPath = string.Empty;
        string strMonth = string.Empty;
        string strYear = string.Empty;
        // int RTOLocationID;
        int intHSRPStateID;
        // int intRTOLocationID;
        string OrderStatus = string.Empty;
        string UserName;
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
                UserName = Session["UserName"].ToString();

                if (!IsPostBack)
                {
                    InitialSetting();
                    try
                    {
                        InitialSetting();
                        if (UserType.Equals(0))
                        {
                            //labelOrganization.Visible = true;
                            //DropDownListStateName.Visible = true;

                            FilldropDownListOrganization();


                        }
                        else
                        {
                            FilldropDownListOrganization();

                            labelOrganization.Enabled = false;
                            DropDownListStateName.Enabled = false;
                            FilldropDownListOrganization();
                        }
                    }
                    catch (Exception err)
                    {

                    }
                }
            }
        }

        private void InitialSetting()
        {

            string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            string MaxDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            HSRPAuthDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            HSRPAuthDate.MaxDate = DateTime.Parse(MaxDate);
            CalendarHSRPAuthDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarHSRPAuthDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            OrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            OrderDate.MaxDate = DateTime.Parse(MaxDate);
            CalendarOrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarOrderDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
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

        //private void FilldropDownListClient()
        //{
        //    if (UserType.Equals(0))
        //    {
        //        int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
        //        SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + intHSRPStateID + " and ActiveStatus!='N'  Order by RTOLocationName";

        //        Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), CnnString, "--Select Location--");
        //        // dropDownListClient.SelectedIndex = dropDownListClient.Items.Count - 1;
        //    }
        //    else
        //    {
        //        // SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where (RTOLocationID=" + RTOLocationID + " or distRelation=" + RTOLocationID + " ) and ActiveStatus!='N'   Order by RTOLocationName";
        //        SQLString = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID where UserRTOLocationMapping.UserID='" + strUserID + "' ";

        //        DataSet dss = Utils.getDataSet(SQLString, CnnString);
        //        dropDownListClient.DataSource = dss;
        //        dropDownListClient.DataBind();

        //    }
        //}

        #endregion



        string FromDate, ToDate;
        DataSet ds = new DataSet();
       




       

        protected void Button1_Click(object sender, EventArgs e)
        {
            GenerateMPCashCollection();
        }

   


        private void FilldropLocation()
        {
            if (strUserID == "3064")
                UserType = 0;

            if (UserType.Equals(0))
            {
                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + intHSRPStateID + " and ActiveStatus!='N'  Order by RTOLocationName";

                Utils.PopulateDropDownList(ddlLocation, SQLString.ToString(), CnnString, "--Select Location--");
                // dropDownListClient.SelectedIndex = dropDownListClient.Items.Count - 1;
            }
            else
            {
                SQLString = "select RTOLocationName,RTOLocationID from RTOLocation where hsrp_stateid='" + HSRPStateID + "' and activestatus='Y' order by RTOLocationName";
                DataSet dss = Utils.getDataSet(SQLString, CnnString);
                ddlLocation.DataSource = dss;
                ddlLocation.DataBind();
            }
        }


        private DataTable GetRtoLocation()
        {
            DataTable dtrto = GetDataInDT("select rtolocationname,rtolocationid from rtolocation where rtolocationid in (select distinct distrelation from rtolocation where hsrp_stateid='" + DropDownListStateName.SelectedValue.ToString() + "') and RTOLocationID not in (148,331)");
            return dtrto;
        }

        private DataTable GetDataInDT(string strSqlString)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionStringDealerHSRP"].ConnectionString);
            DataTable dt = new DataTable();
            SqlDataAdapter adp = new SqlDataAdapter();
            //   con.Open();
            adp.SelectCommand = new SqlCommand(strSqlString, con);
            adp.SelectCommand.CommandTimeout = 0;
            adp.Fill(dt);
            //  con.Close();
            return dt;

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
            }
            table.AddCell(newCellPDF);
        }


      
        private void GenerateMPCashCollection()
        {
            HttpContext context = HttpContext.Current;
            string CashCollectionDate = OrderDate.SelectedDate.ToString("dd/MMM/yyyy");
            //string AffixiationDate=OrderDatefrom.SelectedDate.AddDays(4).ToString("dd/MMM/yyyy");
            string Sqlquery = "select  [dbo] .[GetAffxDate_Insert_Hr] ('" + CashCollectionDate + "','5') as AffDate";
            DataTable dtDate =Utils.GetDataTable(Sqlquery,CnnString);
            string AffixiationDate = Convert.ToDateTime(dtDate.Rows[0]["AffDate"]).ToString("dd/MMM/yyyy");

            string SQLString = String.Empty;
            // Document document = new Document();
            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);



            DataTable dtrto = GetRtoLocation();
            for (int iRowNo = 0; iRowNo < dtrto.Rows.Count; iRowNo++)
            {
                string RTOName = dtrto.Rows[iRowNo]["rtolocationname"].ToString();
                string RTOCode = dtrto.Rows[iRowNo]["RTOLocationid"].ToString();
                string filename = "MP_CashCollection_" + RTOName + "_" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".pdf";
                Document document = new Document();
                //SetFolder(RTOName, DropDownListStateName.Text, "MP_CashCollection_");

               // string PdfFolder = strPath + "\\" + filename;
                string PdfFolder = ConfigurationManager.AppSettings["PdfFolder"].ToString() + filename;
                PdfWriter.GetInstance(document, new FileStream(PdfFolder, FileMode.Create));

                string StrComapnyName = "select CompanyName,Address1 from hsrpstate where hsrp_stateid='" + DropDownListStateName.SelectedValue.ToString() + "'";

                document.Open();


                DataTable dt = Utils.GetDataTable(StrComapnyName,CnnString);

                string strCompanyName = dt.Rows[0]["CompanyName"].ToString();
                string strAddress = dt.Rows[0]["Address1"].ToString();


                string StrRtoLcatioCode = "select Rtolocationcode from rtolocation where HSRP_StateID='" + DropDownListStateName.SelectedValue.ToString() + "' and Rtolocationid='" + RTOCode + "'";
                DataTable dtrtocode = Utils.GetDataTable(StrRtoLcatioCode,CnnString);
                string RTOCode1 = dtrtocode.Rows[0]["Rtolocationcode"].ToString();

                // As refer by Naveen Sir Data will always come after 16 May 2014




                SQLString = " select row_number() over (order by vehicleregno) as SNo, vehicleregno as VehicleNo,ownername as OwnerName,ordertype as OrderType,CashReceiptNo as CashReceiptNo,TotalAmount as TotalAmount,a.mobileno from hsrprecords a inner join rtolocation as b on a.rtolocationid=b.rtolocationid where a.HSRP_StateID='" + DropDownListStateName.SelectedValue + "' and b.distRelation='" + RTOCode + "' and Vehicleregno like '" +
                    RTOCode1 + "%' and convert(date,HSRPRecord_CreationDate) between '" + OrderDate.SelectedDate.ToString("dd/MMM/yyyy")
                    + "' and '" + HSRPAuthDate.SelectedDate.ToString("dd/MMM/yyyy") + "' and HSRPRecord_CreationDate > '2014/05/16 00:00:00' and netamount > 0";

                DataTable dtResult = Utils.GetDataTable(SQLString, CnnString);
                object sumObject;
                sumObject = dtResult.Compute("Sum(TotalAmount)", "");
                string sum = sumObject.ToString();

                //  document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
                document.Open();
                PdfPTable table = new PdfPTable(6);
                var colWidthPercentages = new[] { 30f, 40f, 45f, 40f, 40f, 50f };
                table.SetWidths(colWidthPercentages);
               
                string SqlQuery = string.Empty;

                table.TotalWidth = 550f;
                table.LockedWidth = true;
                //PdfPTable table = new PdfPTable(8);
                //table.TotalWidth = 1000f;

                //  GenerateCell(table, 10, 0, 0, 0, 0, 1, 1, "", 50, 0);
                // GenerateCell(table, 11, 0, 0, 0, 0, 1, 0, "ANNEXURE 1", 20, 0);
                // GenerateCell(table, 11, 0, 0, 0, 0, 1, 0, "(Shedule 2,Clause 5.5.1(a))", 20, 0);
                GenerateCell(table, 11, 0, 0, 0, 0, 1, 0, "Cash Deposite Report", 30, 0);
                GenerateCell(table, 11, 0, 0, 0, 0, 0, 0, " Location : " + RTOName, 20, 0);

                GenerateCell(table, 8, 0, 0, 0, 0, 0, 0, "Date Period : " + OrderDate.SelectedDate.ToString("dd/MMM/yyyy") + " - " + HSRPAuthDate.SelectedDate.ToString("dd/MMM/yyyy"), 15, 0);
                // GenerateCell(table, 7, 0, 0, 0, 0, 0, 0, "Generated Date time: " + System.DateTime.Now.ToString("dd/MMM/yyyy"), 15, 0);
                GenerateCell(table, 11, 0, 0, 0, 0, 1, 0, "", 20, 0);
                GenerateCell(table, 1, 1, 1, 1, 1, 1, 0, "S.No", 20, 0);

                GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Vehicle No", 20, 0);

                GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Owner Name", 20, 0);

                GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Order Type", 20, 0);
                GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Cash Receipt No", 20, 0);
                GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, " Amount", 20, 0);



                for (int i = 0; i < dtResult.Rows.Count; i++)
                {
                    GenerateCell(table, 1, 1, 1, 0, 1, 1, 1, dtResult.Rows[i]["SNo"].ToString(), 20, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dtResult.Rows[i]["VehicleNo"].ToString().ToUpper(), 20, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dtResult.Rows[i]["OwnerName"].ToString(), 20, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dtResult.Rows[i]["OrderType"].ToString(), 20, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dtResult.Rows[i]["CashReceiptNo"].ToString(), 20, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dtResult.Rows[i]["TotalAmount"].ToString(), 20, 0);
                    //GenerateCell(table, 2, 0, 1, 0, 1, 1, 1, dtResult.Rows[i]["OwnerName"].ToString(), 20, 0);
                    // GenerateCell(table, 2, 0, 1, 0, 1, 1, 1, dtResult.Rows[i]["mobileno"].ToString(), 20, 0);
                }
                GenerateCell(table, 8, 0, 0, 0, 0, 0, 0, "", 20, 0);
                GenerateCell(table, 5, 0, 0, 0, 0, 0, 0, "Total Amount", 20, 0);
                GenerateCell(table, 5, 0, 0, 0, 0, 1, 0, sum, 20, 0);
                GenerateCell(table, 8, 0, 0, 0, 0, 0, 0, "", 20, 0);
                GenerateCell(table, 8, 0, 0, 0, 0, 0, 0, "", 20, 0);
                GenerateCell(table, 8, 0, 0, 0, 0, 0, 0, "", 20, 0);
                GenerateCell(table, 8, 0, 0, 0, 0, 0, 0, "Bank Name :", 20, 0);
                GenerateCell(table, 8, 0, 0, 0, 0, 0, 0, "Bank Account No :", 20, 0);

                GenerateCell(table, 4, 0, 0, 0, 0, 0, 0, "Deposit By : ", 20, 0);
                GenerateCell(table, 4, 0, 0, 0, 0, 1, 0, "Downloded By :" + UserName, 20, 0);
               // GenerateCell(table, 3, 0, 0, 0, 0, 1, 0,  UserName , 20, 0);

                GenerateCell(table, 4, 0, 0, 0, 0, 0, 0, "Deposit Date :", 20, 0);

                GenerateCell(table, 4, 0, 0, 0, 0, 1, 0, "Downloded Date : " + System.DateTime.Now, 15, 0);






                //document.Add(table1);
                document.Add(table);
                document.NewPage();
                document.Close();
                context.Response.ContentType = "Application/pdf";
                context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                context.Response.WriteFile(PdfFolder);
                context.Response.End();

            }


        }

        protected void DropDownListStateName_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilldropLocation();
        }

      

       
    }
}