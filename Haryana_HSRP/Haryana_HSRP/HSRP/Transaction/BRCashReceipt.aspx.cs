using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Xml;
using System.Data;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Configuration;
using System.Data.SqlClient;
using System.Net;
using System.Text;

namespace HSRP.Transaction
{
    public partial class BRCashReceipt : System.Web.UI.Page
    {
        public static string AddRecordBy = "";
        public static string CounterNo = "";
        static String ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        SqlConnection con = new SqlConnection(ConnectionString);

        string HSRPStateID = string.Empty;
        string RTOLocationID = string.Empty;
        string ProductivityID = string.Empty;
        string UserType = string.Empty;
        string UserName = string.Empty;
        string Sticker = string.Empty;
        string VIP = string.Empty;
        string USERID = string.Empty;
        DataTable dt = new DataTable();
        string macbase = string.Empty;
        string Query = string.Empty;
        int intDepositAmount = 0;
        int inttotcoll = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["UserType"].ToString() == null)
                {
                    Response.Redirect("~/Login.aspx");
                }
                else
                {
                    UserType = Session["UserType"].ToString();

                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx");
            }
            HSRPStateID = Session["UserHSRPStateID"].ToString();
            RTOLocationID = Session["UserRTOLocationID"].ToString();
            UserName = Session["UID"].ToString();
            USERID = Session["UID"].ToString();
            macbase = Session["MacAddress"].ToString();
            if (!IsPostBack)
            {
                if (UserType.Equals("0"))
                {
                    this.FilldropDownListState();
                    this.FilldropDownListUserNames();
                    Datefrom.SelectedDate = System.DateTime.Now;
                    //Dateto.SelectedDate = System.DateTime.Now;
                }
                else
                {
                    this.FilldropDownListState();
                    Datefrom.SelectedDate = System.DateTime.Now;
                    this.FilldropDownListUserNames();
                }
            }
            else
            {

            }
        }
        string SQLString = string.Empty;
        private void FilldropDownListState()
        {
            try
            {
                if (UserType.Equals("0"))
                {
                    SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where ActiveStatus='Y' Order by HSRPStateName";
                    DataSet dts = Utils.getDataSet(SQLString, ConnectionString);
                    ddlState.DataSource = dts;
                    ddlState.DataTextField = "HSRPStateName";
                    ddlState.DataValueField = "HSRP_StateID";
                    ddlState.DataBind();
                    ddlState.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "0"));
                }
                else
                {
                    SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID=" + HSRPStateID + " and ActiveStatus='Y' Order by HSRPStateName";
                    DataSet dts = Utils.getDataSet(SQLString, ConnectionString);
                    ddlState.DataSource = dts;
                    ddlState.DataTextField = "HSRPStateName";
                    ddlState.DataValueField = "HSRP_StateID";
                    ddlState.DataBind();
                    ddlState.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "0"));

                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void FilldropDownListUserNames()
        {
            try
            {
                if (UserType.Equals("0"))
                {
                  //  SQLString = "select p.DealerId ,p.dealername  as DealerName from Dealermaster  p ,users u where p.IsActive ='1' and p.hsrp_stateId='" + ddlState.SelectedValue + "' and p.userid=u.UserID order by UserName ASC";
                    SQLString = "select p.DealerId ,p.dealername  as DealerName from Dealermaster  p  where p.IsActive ='1' and p.hsrp_stateId='" + ddlState.SelectedValue + "' order by dealername ASC";
            
                    DataSet dts = Utils.getDataSet(SQLString, ConnectionString);
                    ddlUserName.DataSource = dts;
                    ddlUserName.DataTextField = "DealerName";
                    ddlUserName.DataValueField = "DealerId";
                    ddlUserName.DataBind();
                    ddlUserName.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "0"));
                }
                else
                {
                    SQLString = "select p.DealerId ,p.dealername  as DealerName from Dealermaster  p  where p.IsActive ='1' and p.hsrp_stateId='" + HSRPStateID + "'  order by dealername ASC";
                    DataSet dts = Utils.getDataSet(SQLString, ConnectionString);
                    ddlUserName.DataSource = dts;
                    ddlUserName.DataTextField = "DealerName";
                    ddlUserName.DataValueField = "DealerId";
                    ddlUserName.DataBind();
                    ddlUserName.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "0"));

                }
            }
            catch (Exception)
            {

                throw;
            }
        }




        protected void Button3_Click(object sender, EventArgs e)
        {
            if (ddlState.SelectedValue.Equals("0"))
            {
                lblErrMess.Visible = true;
                lblErrMess.Text = "";
                lblErrMess.Text = "Please Select State.";
                return;
            }
            if (ddlUserName.SelectedValue.Equals("0"))
            {

                lblErrMess.Visible = true;
                lblErrMess.Text = "";
                lblErrMess.Text = "Please Select UserName.";
                return;
            }
            EpsionPrint();
        }


        public void EpsionPrint()
        {
            String ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            DataTable GetAddress;
            string Address;
            string AffAddress = string.Empty;
            string sqlquery = string.Empty;
            string rtolocationname = string.Empty;




            GetAddress = Utils.GetDataTable("select * from HSRPState WHERE HSRP_StateID='" + Session["UserHSRPStateID"].ToString() + "'", ConnectionString);

            if (GetAddress.Rows[0]["pincode"] != "" || GetAddress.Rows[0]["pincode"] != null)
            {
                Address = " - " + GetAddress.Rows[0]["pincode"];
            }
            else
            {
                Address = "";
            }

           //  and APwebserviceresp is null
            string SQLString = "select hsrprecordID,CashReceiptNo, HSRPRecord_CreationDate,HSRP_StateID,RTOLocationID,HSRPRecord_AuthorizationNo,HSRPRecord_AuthorizationDate,VehicleRegNo,convert(varchar(12),OwnerName)as OwnerName,MobileNo,VehicleClass,OrderType,StickerMandatory,IsVIP,NetAmount,VehicleType from hsrprecords where  convert(date, HSRPRecord_CreationDate) = convert(date,'" + Datefrom.SelectedDate + "') and  DealerId='" + ddlUserName.SelectedValue + "'  order by  HSRPRecord_CreationDate desc";


            DataTable dataSetFillHSRPDeliveryChallan = Utils.GetDataTable(SQLString, ConnectionString);




            DataProvider.BAL obj = new DataProvider.BAL();
            string filename = "";
            string PdfFolder = "";
            filename = "CASHRECEIPT_" + ddlUserName.SelectedItem + "_" + System.DateTime.Now + ".pdf";
            filename = filename.Replace(" ", "_");
            filename = filename.Replace("/", "_");
            filename = filename.Replace(":", "_");

            if (dataSetFillHSRPDeliveryChallan.Rows.Count > 0)
            {
                Document document = new Document();
                //string strquery1 = "select convert(varchar(15),PlateAffixationDate,103) as Date from hsrprecords where HSRPRecord_AuthorizationNo='" + dataSetFillHSRPDeliveryChallan.Rows[0]["HSRPRecord_AuthorizationNo"].ToString() + "'";
                //string date1 = Utils.getDataSingleValue(strquery1, ConnectionString, "Date");

                //string filename = "CASH RECEIPT-" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".pdf";






                String StringField = String.Empty;
                String StringAlert = String.Empty;


                //Creates an instance of the iTextSharp.text.Document-object:
                //Document document = new Document();
                float imageWidth = 216;
                float imageHeight = 360;
                document.SetMargins(0, 0, 5, 0);
                document.SetPageSize(new iTextSharp.text.Rectangle(imageWidth, imageHeight));

                BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);

                // iTextSharp.text.Font times = new Font(bfTimes, 12, Font.ITALIC, Color.Black);
                //Creates a Writer that listens to this document and writes the document to the Stream of your choice:
                PdfFolder = ConfigurationManager.AppSettings["PdfFolder"].ToString() + filename;
                //PdfWriter.GetInstance(document, new FileStream(Server.MapPath(PdfFolder), FileMode.Create));
                PdfWriter.GetInstance(document, new FileStream(PdfFolder, FileMode.Create));

                //Opens the document:
                document.Open();
                int sno = 1;
                for (int i = 0; i < dataSetFillHSRPDeliveryChallan.Rows.Count; i++)
                {
                    //Adds content to the document:
                    // document.Add(new Paragraph("Ignition Log Report"));
                    PdfPTable table = new PdfPTable(2);
                    PdfPTable table1 = new PdfPTable(2);
                    PdfPTable table2 = new PdfPTable(2);
                    //actual width of table in points
                    //table.TotalWidth = 100f;

                    //fix the absolute width of the table

                    PdfPCell cell312 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    cell312.Colspan = 2;
                    cell312.BorderColor = BaseColor.WHITE;
                    cell312.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table1.AddCell(cell312);

                    PdfPCell cell312a = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    cell312a.Colspan = 2;
                    cell312a.BorderColor = BaseColor.WHITE;
                    cell312a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table2.AddCell(cell312a);

                    PdfPCell cell12 = new PdfPCell(new Phrase("LINKPOINT INFRASTRUCTURE PVT. LTD.", new iTextSharp.text.Font(bfTimes, 9f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    //PdfPCell cell12 = new PdfPCell(new Phrase("" + GetAddress.Rows[0]["CompanyName"].ToString(), new iTextSharp.text.Font(bfTimes, 9f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell12.Colspan = 2;
                    cell12.BorderColor = BaseColor.WHITE;
                    cell12.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell12);

                    PdfPCell cell1203 = new PdfPCell(new Phrase(AffAddress, new iTextSharp.text.Font(bfTimes, 9f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell1203.Colspan = 2;
                    cell1203.BorderWidthLeft = 0f;
                    cell1203.BorderWidthRight = 0f;
                    cell1203.BorderWidthTop = 0f;
                    cell1203.BorderWidthBottom = 0f;
                    cell1203.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell1203);

                    PdfPCell cell0 = new PdfPCell(new Phrase("HSRP CASH RECEIPT", new iTextSharp.text.Font(bfTimes, 9f, iTextSharp.text.Font.UNDERLINE, iTextSharp.text.BaseColor.BLACK)));
                    cell0.Colspan = 2;
                    cell0.BorderWidthLeft = 0f;
                    cell0.BorderWidthRight = 0f;
                    cell0.BorderWidthTop = 0f;
                    cell0.BorderWidthBottom = 0f;
                    cell0.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell0);


                    PdfPCell cell1 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 9f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell1.Colspan = 2;
                    cell1.BorderWidthLeft = 0f;
                    cell1.BorderWidthRight = 0f;
                    cell1.BorderWidthTop = 0f;
                    cell1.BorderWidthBottom = 0f;

                    cell1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell1);




                    PdfPCell cellInv2 = new PdfPCell(new Phrase("CASH RECEIPT No.", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cellInv2.Colspan = 0;

                    cellInv2.BorderWidthLeft = 0f;
                    cellInv2.BorderWidthRight = 0f;
                    cellInv2.BorderWidthTop = 0f;
                    cellInv2.BorderWidthBottom = 0f;
                    cellInv2.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cellInv2);



                    PdfPCell cellInv22111 = new PdfPCell(new Phrase(": " + dataSetFillHSRPDeliveryChallan.Rows[i]["CashReceiptNo"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cellInv22111.Colspan = 0;
                    cellInv22111.BorderWidthLeft = 0f;
                    cellInv22111.BorderWidthRight = 0f;
                    cellInv22111.BorderWidthTop = 0f;
                    cellInv22111.BorderWidthBottom = 0f;
                    cellInv22111.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cellInv22111);







                    PdfPCell cell21 = new PdfPCell(new Phrase("DATE", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell21.Colspan = 0;

                    cell21.BorderWidthLeft = 0f;
                    cell21.BorderWidthRight = 0f;
                    cell21.BorderWidthTop = 0f;
                    cell21.BorderWidthBottom = 0f;
                    cell21.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell21);
                    string CashReceiptDateTime = string.Empty;

                    if (dataSetFillHSRPDeliveryChallan.Rows[i]["HSRPRecord_CreationDate"].ToString() == "")
                    {
                        CashReceiptDateTime = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        CashReceiptDateTime = Convert.ToDateTime(dataSetFillHSRPDeliveryChallan.Rows[i]["HSRPRecord_CreationDate"]).ToString("dd/MM/yyyy");
                    }
                    PdfPCell cell212 = new PdfPCell(new Phrase(": " + CashReceiptDateTime, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell212.Colspan = 0;

                    cell212.BorderWidthLeft = 0f;
                    cell212.BorderWidthRight = 0f;
                    cell212.BorderWidthTop = 0f;
                    cell212.BorderWidthBottom = 0f;
                    cell212.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell212);



                    PdfPCell cell2 = new PdfPCell(new Phrase("TIN NO.", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell2.Colspan = 0;

                    cell2.BorderWidthLeft = 0f;
                    cell2.BorderWidthRight = 0f;
                    cell2.BorderWidthTop = 0f;
                    cell2.BorderWidthBottom = 0f;
                    cell2.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell2);

                    string getTinNo = Utils.getScalarValue("select TinNo from hsrpstate where HSRP_StateID='" + HSRPStateID + "' ", ConnectionString);

                    PdfPCell cell22111 = new PdfPCell(new Phrase(": " + getTinNo.ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell22111.Colspan = 0;
                    cell22111.BorderWidthLeft = 0f;
                    cell22111.BorderWidthRight = 0f;
                    cell22111.BorderWidthTop = 0f;
                    cell22111.BorderWidthBottom = 0f;
                    cell22111.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell22111);

                    //PdfPCell cell22 = new PdfPCell(new Phrase("TIME", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    //cell22.Colspan = 0;
                    //cell22.BorderWidthLeft = 0f;
                    //cell22.BorderWidthRight = 0f;
                    //cell22.BorderWidthTop = 0f;
                    //cell22.BorderWidthBottom = 0f;
                    //cell22.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    //table.AddCell(cell22);

                    //PdfPCell cell222 = new PdfPCell(new Phrase(": " + Convert.ToDateTime(dataSetFillHSRPDeliveryChallan.Rows[i]["HSRPRecord_CreationDate"]).ToString("hh:mm:ss"), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    //cell222.Colspan = 0;

                    //cell222.BorderWidthLeft = 0f;
                    //cell222.BorderWidthRight = 0f;
                    //cell222.BorderWidthTop = 0f;
                    //cell222.BorderWidthBottom = 0f;
                    //cell222.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    //table.AddCell(cell222);



                    //string getExciseNo = Utils.getScalarValue("select ExciseNo from hsrpstate where HSRP_StateID='" + HSRPStateID + "' ", ConnectionString);
                    //PdfPCell cell221 = new PdfPCell(new Phrase("EXCISE NO. ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    //cell221.Colspan = 0;
                    //cell221.BorderWidthLeft = 0f;
                    //cell221.BorderWidthRight = 0f;
                    //cell221.BorderWidthTop = 0f;
                    //cell221.BorderWidthBottom = 0f;
                    //cell221.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    //table.AddCell(cell221);

                    //PdfPCell cell2221 = new PdfPCell(new Phrase(": " + getExciseNo.ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    //cell2221.Colspan = 0;

                    //cell2221.BorderWidthLeft = 0f;
                    //cell2221.BorderWidthRight = 0f;
                    //cell2221.BorderWidthTop = 0f;
                    //cell2221.BorderWidthBottom = 0f;
                    //cell2221.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    //table.AddCell(cell2221);



                    PdfPCell cell5 = new PdfPCell(new Phrase("AUTH NO.", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell5.Colspan = 0;
                    cell5.BorderWidthLeft = 0f;
                    cell5.BorderWidthRight = 0f;
                    cell5.BorderWidthTop = 0f;
                    cell5.BorderWidthBottom = 0f;
                    cell5.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell5);

                    PdfPCell cell55 = new PdfPCell(new Phrase(": " + dataSetFillHSRPDeliveryChallan.Rows[i]["HSRPRecord_AuthorizationNo"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell55.Colspan = 0;
                    cell55.BorderWidthLeft = 0f;
                    cell55.BorderWidthRight = 0f;
                    cell55.BorderWidthTop = 0f;
                    cell55.BorderWidthBottom = 0f;
                    cell55.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell55);

                    PdfPCell cell25 = new PdfPCell(new Phrase("AUTH. DATE", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell25.Colspan = 0;

                    cell25.BorderWidthLeft = 0f;
                    cell25.BorderWidthRight = 0f;
                    cell25.BorderWidthTop = 0f;
                    cell25.BorderWidthBottom = 0f;
                    cell25.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell25);

                    //DateTime AuthDate = Convert.ToDateTime(dataSetFillHSRPDeliveryChallan.Rows[0]["HSRPRecord_AuthorizationDate"].ToString());
                    string auths = string.Empty;
                    auths = dataSetFillHSRPDeliveryChallan.Rows[i]["HSRPRecord_AuthorizationDate"].ToString();
                    if (auths == "")
                    {
                        PdfPCell cell255 = new PdfPCell(new Phrase(": " + auths, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell255.Colspan = 0;

                        cell255.BorderWidthLeft = 0f;
                        cell255.BorderWidthRight = 0f;
                        cell255.BorderWidthTop = 0f;
                        cell255.BorderWidthBottom = 0f;
                        cell255.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell255);

                    }
                    else
                    {
                        DateTime AuthDate = Convert.ToDateTime(dataSetFillHSRPDeliveryChallan.Rows[i]["HSRPRecord_AuthorizationDate"].ToString());
                        PdfPCell cell255 = new PdfPCell(new Phrase(": " + AuthDate.ToString("dd/MM/yyyy"), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell255.Colspan = 0;

                        cell255.BorderWidthLeft = 0f;
                        cell255.BorderWidthRight = 0f;
                        cell255.BorderWidthTop = 0f;
                        cell255.BorderWidthBottom = 0f;
                        cell255.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell255);

                    }
                    PdfPCell cell7 = new PdfPCell(new Phrase("OWNER NAME", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell7.Colspan = 0;
                    cell7.BorderWidthLeft = 0f;
                    cell7.BorderWidthRight = 0f;
                    cell7.BorderWidthTop = 0f;
                    cell7.BorderWidthBottom = 0f;
                    cell7.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell7);

                    PdfPCell cell75 = new PdfPCell(new Phrase(": " + dataSetFillHSRPDeliveryChallan.Rows[i]["OwnerName"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell75.Colspan = 0;
                    cell75.BorderWidthLeft = 0f;
                    cell75.BorderWidthRight = 0f;
                    cell75.BorderWidthTop = 0f;
                    cell75.BorderWidthBottom = 0f;
                    cell75.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell75);

                    PdfPCell cell29 = new PdfPCell(new Phrase("CONTACT NO", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell29.Colspan = 0;
                    cell29.BorderWidthLeft = 0f;
                    cell29.BorderWidthRight = 0f;
                    cell29.BorderWidthTop = 0f;
                    cell29.BorderWidthBottom = 0f;
                    cell29.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell29);

                    PdfPCell cell295 = new PdfPCell(new Phrase(": " + dataSetFillHSRPDeliveryChallan.Rows[i]["MobileNo"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell295.Colspan = 0;
                    cell295.BorderWidthLeft = 0f;
                    cell295.BorderWidthRight = 0f;
                    cell295.BorderWidthTop = 0f;
                    cell295.BorderWidthBottom = 0f;
                    cell295.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell295);



                    PdfPCell cell9 = new PdfPCell(new Phrase("VEHICLE REG.", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell9.Colspan = 0;
                    cell9.BorderWidthLeft = 0f;
                    cell9.BorderWidthRight = 0f;
                    cell9.BorderWidthTop = 0f;
                    cell9.BorderWidthBottom = 0f;
                    cell9.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell9);

                    PdfPCell cell95 = new PdfPCell(new Phrase(": " + dataSetFillHSRPDeliveryChallan.Rows[i]["VehicleRegNo"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell95.Colspan = 0;
                    cell95.BorderWidthLeft = 0f;
                    cell95.BorderWidthRight = 0f;
                    cell95.BorderWidthTop = 0f;
                    cell95.BorderWidthBottom = 0f;
                    cell95.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell95);

                    PdfPCell cell10 = new PdfPCell(new Phrase("VEHICLE MODEL", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell10.Colspan = 0;
                    cell10.BorderWidthLeft = 0f;
                    cell10.BorderWidthRight = 0f;
                    cell10.BorderWidthTop = 0f;
                    cell10.BorderWidthBottom = 0f;
                    cell10.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell10);

                    PdfPCell cell105 = new PdfPCell(new Phrase(": " + dataSetFillHSRPDeliveryChallan.Rows[i]["VehicleType"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell105.Colspan = 0;
                    cell105.BorderWidthLeft = 0f;
                    cell105.BorderWidthRight = 0f;
                    cell105.BorderWidthTop = 0f;
                    cell105.BorderWidthBottom = 0f;
                    cell105.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell105);

                    PdfPCell cell11 = new PdfPCell(new Phrase(" VEHICLE CLASS ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell11.Colspan = 0;
                    cell11.BorderWidthLeft = 0f;
                    cell11.BorderWidthRight = 0f;
                    cell11.BorderWidthTop = 0f;
                    cell11.BorderWidthBottom = 0f;
                    cell11.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell11);

                    PdfPCell cell115 = new PdfPCell(new Phrase(": " + dataSetFillHSRPDeliveryChallan.Rows[i]["VehicleClass"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell115.Colspan = 0;
                    cell115.BorderWidthLeft = 0f;
                    cell115.BorderWidthRight = 0f;
                    cell115.BorderWidthTop = 0f;
                    cell115.BorderWidthBottom = 0f;
                    cell115.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell115);



                    PdfPCell cellNet120 = new PdfPCell(new Phrase("NET AMOUNT (Rs.)", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cellNet120.Colspan = 0;
                    cellNet120.BorderWidthLeft = 0f;
                    cellNet120.BorderWidthRight = 0f;
                    cellNet120.BorderWidthTop = 0f;
                    cellNet120.BorderWidthBottom = 0f;
                    cellNet120.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cellNet120);



                    PdfPCell cell1205 = new PdfPCell(new Phrase(" " + dataSetFillHSRPDeliveryChallan.Rows[i]["NetAmount"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell1205.Colspan = 0;
                    cell1205.BorderWidthLeft = 0f;
                    cell1205.BorderWidthRight = 0f;
                    cell1205.BorderWidthTop = 0f;
                    cell1205.BorderWidthBottom = 0f;
                    cell1205.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell1205);

                    PdfPCell celldupCash401 = new PdfPCell(new Phrase("ROUND OF AMOUNT", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    celldupCash401.Colspan = 0;
                    celldupCash401.BorderWidthLeft = 0f;
                    celldupCash401.BorderWidthRight = 0f;
                    celldupCash401.BorderWidthTop = 0f;
                    celldupCash401.BorderWidthBottom = 0f;
                    celldupCash401.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(celldupCash401);


                    decimal roundAmt = Convert.ToDecimal(dataSetFillHSRPDeliveryChallan.Rows[i]["NetAmount"].ToString());
                    roundAmt = Math.Round(roundAmt, 0);

                    PdfPCell celldupCash402 = new PdfPCell(new Phrase(" " + roundAmt, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    celldupCash402.Colspan = 0;
                    celldupCash402.BorderWidthLeft = 0f;
                    celldupCash402.BorderWidthRight = 0f;
                    celldupCash402.BorderWidthTop = 0f;
                    celldupCash402.BorderWidthBottom = 0f;
                    celldupCash402.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(celldupCash402);
                    PdfPCell celldupCash40z1 = new PdfPCell(new Phrase("(Inclusive of All Tax)", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    celldupCash40z1.Colspan = 4;
                    celldupCash40z1.BorderWidthLeft = 0f;
                    celldupCash40z1.BorderWidthRight = 0f;
                    celldupCash40z1.BorderWidthTop = 0f;
                    celldupCash40z1.BorderWidthBottom = 0f;
                    celldupCash40z1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(celldupCash40z1);

                    PdfPCell celldupRouCash402 = new PdfPCell(new Phrase("Disclaimer :", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    celldupRouCash402.Colspan = 2;
                    celldupRouCash402.BorderWidthLeft = 0f;
                    celldupRouCash402.BorderWidthRight = 0f;
                    celldupRouCash402.BorderWidthTop = 0f;
                    celldupRouCash402.BorderWidthBottom = 0f;
                    celldupRouCash402.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(celldupRouCash402);

                    //string Message = "\u2022" + " Vehicle Owner is required to visit along with vehicle for the affixation of HSRP, on the fourth working day from the date of  issuance of cash receipt.";
                    string Message = "\u2022" + " Vehicle Owner is requested to please check the Correctness of the cash slip.";

                    PdfPCell cell64 = new PdfPCell(new Phrase(Message, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell64.Colspan = 2;
                    cell64.BorderWidthLeft = 0f;
                    cell64.BorderWidthRight = 0f;
                    cell64.BorderWidthTop = 0f;
                    cell64.BorderWidthBottom = 0f;
                    cell64.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell64);


                    string MessageSec = "\u2022" + " Before leaving the counter verify your data on cash receipt. The company shall not be responsible for any clarrical mistake what so ever.";

                    PdfPCell cell65a = new PdfPCell(new Phrase(MessageSec, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell65a.Colspan = 4;
                    cell65a.BorderWidthLeft = 0f;
                    cell65a.BorderWidthRight = 0f;
                    cell65a.BorderWidthTop = 0f;
                    cell65a.BorderWidthBottom = 0f;
                    cell65a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell65a);

                    PdfPCell cell63 = new PdfPCell(new Phrase("" + GetAddress.Rows[0]["CompanyName"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell63.Colspan = 2;
                    cell63.BorderWidthLeft = 0f;
                    cell63.BorderWidthRight = 0f;
                    cell63.BorderWidthTop = 0f;
                    cell63.BorderWidthBottom = 0f;
                    cell63.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell63);

                    PdfPCell cell62 = new PdfPCell(new Phrase("(AUTH. SIGH.)", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell62.Colspan = 2;
                    cell62.BorderWidthLeft = 0f;
                    cell62.BorderWidthRight = 0f;
                    cell62.BorderWidthTop = 0f;
                    cell62.BorderWidthBottom = 0f;
                    cell62.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell62);

                    PdfPCell cell62an = new PdfPCell(new Phrase("(" + sno.ToString() + ")", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell62an.Colspan = 0;
                    cell62an.BorderColor = BaseColor.WHITE;
                    cell62an.BorderWidthLeft = 0f;
                    cell62an.BorderWidthRight = 0f;
                    cell62an.BorderWidthTop = 0f;
                    cell62an.BorderWidthBottom = 0f;
                    cell62an.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell62an);

                    PdfPCell cellsp1 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cellsp1.Colspan = 2;
                    cellsp1.BorderWidthLeft = 0f;
                    cellsp1.BorderWidthRight = 0f;
                    cellsp1.BorderWidthTop = 0f;
                    cellsp1.BorderWidthBottom = 0f;
                    cellsp1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cellsp1);

                    PdfPCell cell22 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell22.Colspan = 0;
                    cell22.BorderWidthLeft = 0f;
                    cell22.BorderWidthRight = 0f;
                    cell22.BorderWidthTop = 0f;
                    cell22.BorderWidthBottom = 0f;
                    cell22.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell22);

                    PdfPCell cell222 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell222.Colspan = 0;

                    cell222.BorderWidthLeft = 0f;
                    cell222.BorderWidthRight = 0f;
                    cell222.BorderWidthTop = 0f;
                    cell222.BorderWidthBottom = 0f;
                    cell222.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell222);

                    sno += 1;
                    //////////////////////////////////////Duplicate CashRecipt/////////////////////////////////////////////////////////////////////////////////////////////


                    //PdfPCell cell2195 = new PdfPCell(new Phrase("---------------------------------------------", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    //cell2195.Colspan = 2;
                    //cell2195.BorderWidthLeft = 0f;
                    //cell2195.BorderWidthRight = 0f;
                    //cell2195.BorderWidthTop = 0f;
                    //cell2195.BorderWidthBottom = 0f;
                    //cell2195.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    //table.AddCell(cell2195);


                    document.Add(table1);
                    document.Add(table);
                    document.Add(table2);
                    string query = "update hsrprecords set APwebserviceresp='Y' where hsrprecordID='" + dataSetFillHSRPDeliveryChallan.Rows[i]["hsrprecordID"].ToString() + "'";
                    Utils.ExecNonQuery(query, ConnectionString);

                }

                //document.Add(table2);
                //document.Add(table);

                document.Close();
                HttpContext context = HttpContext.Current;
                context.Response.ContentType = "Application/pdf";
                context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                context.Response.WriteFile(PdfFolder);
                context.Response.End();
            }
            else
            {
                lblErrMess.Visible = true;
                lblErrMess.Text = "Record Not Found";
            }
        }

        protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.FilldropDownListUserNames();
        }

    }
}