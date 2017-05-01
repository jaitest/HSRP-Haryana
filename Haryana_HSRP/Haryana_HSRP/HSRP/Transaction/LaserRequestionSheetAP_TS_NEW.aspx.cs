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
//using System.Data.OracleClient;


namespace HSRP.Master
{
    public partial class LaserRequestionSheetAP_TS_NEW : System.Web.UI.Page
    {
        
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        string UserType = string.Empty;
        string HSRPStateID = string.Empty;
        string RTOLocationID = string.Empty;
        int intHSRPStateID;
        string trnasportname, pp;
        string transtr, statename1;
        BaseFont basefont;
        string fontpath;

        string AllLocation = string.Empty;
        string OrderStatus = string.Empty;
        DateTime AuthorizationDate;
        DateTime OrderDate1;
        DataProvider.BAL bl = new DataProvider.BAL();
        BAL obj = new BAL();
        string StickerManditory = string.Empty;
        DataTable dataSetFillHSRPRecord;
        string strCompanyName = string.Empty;
        string strSqlQuery = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            Utils.GZipEncodePage();
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {
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
                            labelOrganization.Visible = true;
                            DropDownListStateName.Visible = true;
                            DropDownListStateName.Enabled = true;
                            labelClient.Visible = true;
                            dropDownListClient.Visible = true;
                            FilldropDownListOrganization();
                            FilldropDownListClient();
                        }
                        else
                        {
                            hiddenUserType.Value = "1";
                            labelOrganization.Enabled = false;
                            DropDownListStateName.Enabled = false;
                            labelClient.Enabled = false;
                            FilldropDownListOrganization();
                            FilldropDownListClient();
                            
                            
                        }


                    }
                    catch (Exception err)
                    {
                        lblErrMsg.Text = "Error on Page Load" + err.Message.ToString();
                    }
                }
            }
        }

        #region DropDown

        public void FileDetail()
        {

        }

        private void FilldropDownListOrganization()
        {
            if (UserType == "0")
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where ActiveStatus='Y' Order by HSRPStateName";
                Utils.PopulateDropDownList(DropDownListStateName, SQLString.ToString(), CnnString, "--Select State--");
                // DropDownListStateName.SelectedIndex = DropDownListStateName.Items.Count - 1;

            }
            else
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID=" + HSRPStateID + " and ActiveStatus='Y' Order by HSRPStateName";
                DataTable dts = Utils.GetDataTable(SQLString, CnnString);
                DropDownListStateName.DataSource = dts;
                DropDownListStateName.DataBind();
            }
        }
        private void FilldropDownListClient()
        {

                SQLString = "select distinct EmbCenterName,NAVEMBID from RTOLocation Where HSRP_StateID=" + HSRPStateID + " and navembid is not null  Order by EmbCenterName ";
                Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), CnnString, "--Select Embossing Center--");

             
        }
        
        #endregion

        



        protected void Grid1_ItemCommand(object sender, ComponentArt.Web.UI.GridItemCommandEventArgs e)
        {

        }
        protected void DropDownListStateName_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            labelClient.Visible = true;
            dropDownListClient.Visible = true;
            FilldropDownListClient();

        }

        protected void ButtonGo_Click(object sender, EventArgs e)
        {


        }

        protected void ButImpData_Click(object sender, EventArgs e)
        {

        }

        private void InitialSetting()
        {

            string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            string MaxDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            HSRPAuthDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            HSRPAuthDate.MaxDate = DateTime.Parse(MaxDate);
            CalendarHSRPAuthDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarHSRPAuthDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            OrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(-3.00);
            OrderDate.MaxDate = DateTime.Parse(MaxDate);
            CalendarOrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarOrderDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
        }

        protected void dropDownListClient_SelectedIndexChanged1(object sender, EventArgs e)
        {
            
        }

        protected void Grid1_ItemCommand1(object sender, ComponentArt.Web.UI.GridItemCommandEventArgs e)
        {





        }

        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {

        }

        protected void btnExportToPDF_Click(object sender, EventArgs e)
        {

        }


        protected void Linkbutton1_Click(object sender, EventArgs e)
        {

        }

       

        private static void GenerateCell(PdfPTable table, int iSpan, int iLeftWidth, int iRightWidth, int iTopWidth, int iBottomWidth, int iAllign, int iFont, string strText, int iRowHeight, int iRowWidth)
        {
            PdfPCell newCellPDF = null;
            BaseFont bfTimes1 = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
            if (iFont.Equals(0))
            {
                newCellPDF = new PdfPCell(new Phrase(strText, new iTextSharp.text.Font(bfTimes1, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
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


        private void ExportToPDF(string strStartDate,string strEndDate)
        {
            string strSqlQuery = string.Empty;
            bool Check = false;
            int Itotal = 0;
            string filename = "LaserRequestionSheet-AP-TS " + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".pdf";
            String StringField = String.Empty;
            String StringAlert = String.Empty;
            StringBuilder bb = new StringBuilder();
            // Document document = new Document();
            //  iTextSharp.text.Document document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 0, 0, 0, 0);
            iTextSharp.text.Document document = new Document(new iTextSharp.text.Rectangle(188f, 124f), -30, -30, 8, 0);
            document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());


            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
            // iTextSharp.text.Font times = new Font(bfTimes, 12, Font.ITALIC, Color.Black);
            // Creates a Writer that listens to this document and writes the document to the Stream of your choice:
            string PdfFolder = ConfigurationManager.AppSettings["PdfFolder"].ToString() + filename;
            PdfWriter.GetInstance(document, new FileStream(PdfFolder, FileMode.Create));
            //Opens the document:
            document.Open();
            PdfPTable table;
            int Inttable = 10;
            table = new PdfPTable(Inttable);
            var colWidthPercentages = new[] { 7f, 20f, 15f, 35f, 20f, 35f, 45f, 20f, 45f, 45f };
            string strQuery = string.Empty;
            string strRtoLocationName = string.Empty;

            strSqlQuery="select CompanyName from hsrpstate where hsrp_stateid='"+HSRPStateID+"'";
            strCompanyName = Utils.getDataSingleValue(strSqlQuery, CnnString, "CompanyName");

            strQuery = "Select UserLoginName from users where userid='" + strUserID + "'";
            string strUserName = Utils.getScalarValue(strQuery, CnnString);
            string strReqNo = "select (prefixtext+right('00000'+ convert(varchar,lastno+1),5)) as Reqno from prefix  where hsrp_stateid='" + DropDownListStateName.SelectedValue.ToString() + "' and prefixfor='Req No'";
            string strReqNumber=Utils.getScalarValue(strReqNo,CnnString);
            SQLString = "Exec [laserreqSlip] '" + strStartDate + "' , '" + strEndDate + "','" + DropDownListStateName.SelectedValue.ToString() + "','" + dropDownListClient.SelectedValue.ToString() + "'";
            DataTable dtResult = Utils.GetDataTable(SQLString, CnnString);

            for (int intICount = 0; intICount < dtResult.Rows.Count; intICount++)
            {
                strRtoLocationName = strRtoLocationName+dtResult.Rows[intICount]["RtoLocationName"].ToString();
                strRtoLocationName += (intICount < dtResult.Rows.Count - 1) ? "," : string.Empty;
            }
            GenerateCell(table, 10, 1, 1, 1, 0, 1, 0, strCompanyName, 30, 0);
            GenerateCell(table, 10, 1, 1, 0, 1, 1, 0, "MATERIAL REQUSITION NOTE", 30, 0);
            GenerateCell(table, 10, 1, 1, 0, 1, 1, 0, "Production Sheet Date :" + OrderDate.SelectedDate.ToString("dd/MM/yyyy"),25, 0);
            GenerateCell(table, 5,  1, 1, 0, 1, 0, 0, "REQ.NO:-"  + strReqNumber, 30, 0);
            GenerateCell(table, 5,  0, 1, 0, 1, 0, 0, "Embossing Center :   " +dropDownListClient.SelectedItem.ToString(), 30, 0);
            GenerateCell(table, 10, 1, 1, 0, 1, 0, 0, "RTO Locations :   " +strRtoLocationName,20, 0);
            GenerateCell(table, 10, 1, 1, 0, 1, 0, 0, "", 20, 0);
            GenerateCell(table, 1,  1, 1, 0, 1, 1, 0, "SR.N.", 20, 0);
            GenerateCell(table, 4,  0, 1, 0, 1, 1, 0, "Product Size", 20, 0);
            GenerateCell(table, 1,  0, 1, 0, 1, 1, 0, "Laser Count", 20, 0);
            GenerateCell(table, 2,  0, 1, 0, 1, 1, 0, "Start Laser No", 20, 0);
            GenerateCell(table, 2,  0, 1, 0, 1, 1, 0, "End Laser No", 20, 0);


            if (dtResult.Rows.Count > 0)
            {
                for (int i = 0; i < dtResult.Rows.Count; i++)
                {
                    GenerateCell(table, 1, 1, 1, 0, 1, 1, 0, dtResult.Rows[i]["ID"].ToString(), 20, 0);
                    GenerateCell(table, 4, 0, 1, 0, 1, 1, 0, dtResult.Rows[i]["productcode"].ToString(), 20, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 0, dtResult.Rows[i]["LaserCount"].ToString(), 20, 0);
                    Itotal = Itotal + Convert.ToInt16(dtResult.Rows[i]["LaserCount"]);
                    GenerateCell(table, 2, 0, 1, 0, 1, 1, 0, dtResult.Rows[i]["BeginLaser"].ToString(), 20, 0);
                    GenerateCell(table, 2, 0, 1, 0, 1, 1, 0, dtResult.Rows[i]["EndLaser"].ToString(), 20, 0);

                   
                }




                GenerateCell(table, 1, 1, 1, 0, 1, 1, 0, "Grand Total", 20, 0);
                GenerateCell(table, 4, 0, 1, 0, 1, 1, 0, "", 20, 0);
                GenerateCell(table, 1, 0, 1, 0, 1, 1, 0, Itotal.ToString(), 20, 0);
                GenerateCell(table, 2, 0, 1, 0, 1, 1, 0, "", 20, 0);
                GenerateCell(table, 2, 0, 1, 0, 1, 1, 0, "", 20, 0);

                GenerateCell(table, 1, 1, 0, 0, 1, 1, 0, "REQUESTED BY", 40, 0);
                GenerateCell(table, 4, 0, 0, 0, 1, 1, 0, "AUTHORISED BY", 40, 0);
                GenerateCell(table, 1, 0, 0, 0, 1, 1, 0, " ISSUED BY", 40, 0);
                GenerateCell(table, 2, 0, 0, 0, 1, 1, 0, "", 40, 0);
                GenerateCell(table, 2, 0, 1, 0, 1, 1, 0, "RECEIVED BY", 40, 0);

                GenerateCell(table, 10, 1, 1, 0, 1, 0, 0, "Name", 20, 0);
                GenerateCell(table, 10, 1, 1, 0, 1, 0, 0, "Designation", 20, 0);
                GenerateCell(table, 10, 1, 1, 0, 1, 2, 0, "Download by: " + strUserName, 20, 0);
                GenerateCell(table, 10, 1, 1, 0, 1, 2, 0, "Sheet Genrate Date: " + System.DateTime.Now.ToString("dd/MM/yyyy HH:MM:ss"), 30, 0);
                GenerateCell(table, 10, 1, 1, 0, 1, 1, 0, "", 20, 0);
                document.Add(table);
                // document.Add(table1); 

                document.Close();
                HttpContext context = HttpContext.Current;

                context.Response.ContentType = "Application/pdf";
                context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                context.Response.WriteFile(PdfFolder);
                context.Response.End();
                string Query = "update prefix set lastno=lastno+1 where hsrp_stateid='"+DropDownListStateName.SelectedValue.ToString()+"' and prefixfor='Req No'";
                int u = Utils.ExecNonQuery(Query, CnnString);
            }



            else
            {

                lblErrMsg.Text = "No Record Found";
            }
        }
        protected void btnGO_Click(object sender, EventArgs e)
        {
            string type = "1";
            String[] StringAuthDate = OrderDate.SelectedDate.ToString().Split('/');
            string MonTo = ("0" + StringAuthDate[0]);
            string MonthdateTO = MonTo.Replace("00", "0").Replace("01", "1");
            String ReportDateFrom = StringAuthDate[1] + "/" + MonthdateTO + "/" + StringAuthDate[2].Split(' ')[0];
            String From = StringAuthDate[0] + "/" + StringAuthDate[1] + "/" + StringAuthDate[2].Split(' ')[0];
            //AuthorizationDate = new DateTime(Convert.ToInt32(StringAuthDate[2].Split(' ')[0]), Convert.ToInt32(StringAuthDate[0]), Convert.ToInt32(StringAuthDate[1]));
            string AuthorizationDate = From + " 00:00:00";
            String[] StringOrderDate = HSRPAuthDate.SelectedDate.ToString().Split('/');
            string Mon = ("0" + StringOrderDate[0]);
            string Monthdate = Mon.Replace("00", "0").Replace("01", "1");
            String FromDate = StringOrderDate[1] + "-" + Monthdate + "-" + StringOrderDate[2].Split(' ')[0];
            String ReportDateTo = StringOrderDate[1] + "/" + Monthdate + "/" + StringOrderDate[2].Split(' ')[0];
            String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
            string ToDate = From + " 23:59:59";
            ExportToPDF(AuthorizationDate,ToDate);
            


        }

        protected void btnDownloadRecords_Click(object sender, EventArgs e)
        {

        }
    }
}
 
        
