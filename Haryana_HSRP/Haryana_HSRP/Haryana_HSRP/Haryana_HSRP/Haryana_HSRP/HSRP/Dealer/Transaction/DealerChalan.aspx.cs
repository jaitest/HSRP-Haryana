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
using System.Data.SqlClient;
using System.Collections;

namespace HSRP.Dealer.Transaction
{
    public partial class DealerChalan : System.Web.UI.Page
    {


        string SQLString = string.Empty;
        string UserType = string.Empty;
        string HSRPStateID = string.Empty;
        string RTOLocationID = string.Empty;
        int intHSRPStateID;
        //string ConnectionStringdealer1 = string.Empty;
        string CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        string ConnectionStringdealer1 = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStringDealerHSRP"].ToString();

        protected void Page_Load(object sender, EventArgs e)
        {
            HSRPStateID = Session["UserHSRPStateID"].ToString();

            LblMessage.Text = "";
            if (!IsPostBack)
            {
                BL.blEntryDetail bl = new BL.blEntryDetail();
                bl.DailyEnityDeatail(Session["UID"].ToString(), "DealerChallan.aspx");

                string SQLString = "select userfirstname from users where userid='" + Session["UID"].ToString() + "'";

                DataTable dt = Utils.GetDataTable(SQLString, CnnString);
                Session["username"] = dt.Rows[0]["userfirstname"].ToString();
                InitialSetting();
                //  FilldropDownListClient();
                FilldropDownListOrganization(); 
               // DealerNameShow();
            }

        }


          private void FilldropDownListOrganization()
        {
            if (UserType == "0")
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where ActiveStatus='Y' Order by HSRPStateName";
                Utils.PopulateDropDownList(DropDownListStateName, SQLString.ToString(), CnnString, "--Select State--");
                DropDownListStateName.SelectedIndex = DropDownListStateName.Items.Count - 1;
            }
            else
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID=" + HSRPStateID + " and ActiveStatus='Y' Order by HSRPStateName";
                DataSet dts = Utils.getDataSet(SQLString, CnnString);
                DropDownListStateName.DataSource = dts;
                DropDownListStateName.DataBind();
                FilldropDownListClient();
            }
        }

        private void FilldropDownListClient()
        {
          
                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + intHSRPStateID + " and ActiveStatus!='N'  Order by RTOLocationName";

                Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), CnnString, "--Select Location--");
                dropDownListClient.SelectedIndex = dropDownListClient.Items.Count - 1;
          
        }
       
        

        protected void DropDownListStateName_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilldropDownListClient();
        }

        protected void DropDownListLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            DealerNameShow();
        }

        private void DealerNameShow()
        {
            string SQLString = "select DealerID,DealerName from dbo.dealerMaster where [HSRP_StateID]='" + DropDownListStateName.SelectedValue.ToString() + "' and [RTOLocationID]='" + dropDownListClient.SelectedValue.ToString() + "' order by DealerID";

            DataTable dt = Utils.GetDataTable(SQLString, ConnectionStringdealer1);
            ddlDealerName.DataSource = dt;
            ddlDealerName.DataTextField = "DealerName";
            ddlDealerName.DataValueField = "DealerID";
            ddlDealerName.DataBind();
            ddlDealerName.Items.Insert(0, "--Select Dealer Name--");
            ddlDealerName.Items[0].Value = "0";
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
        DataTable dt;
        protected void ButtonGo_Click(object sender, EventArgs e)
        {


            show();

        }

        public void show()
        {

            String[] StringAuthDate = OrderDate.SelectedDate.ToString().Split('/');
            string MonTo = ("0" + StringAuthDate[0]);
            string MonthdateTO = MonTo.Replace("00", "0").Replace("01", "1");
            String FromDateTo = StringAuthDate[1] + "-" + MonthdateTO + "-" + StringAuthDate[2].Split(' ')[0];
            String From = StringAuthDate[0] + "/" + StringAuthDate[1] + "/" + StringAuthDate[2].Split(' ')[0];
            //AuthorizationDate = new DateTime(Convert.ToInt32(StringAuthDate[2].Split(' ')[0]), Convert.ToInt32(StringAuthDate[0]), Convert.ToInt32(StringAuthDate[1]));
            string AuthorizationDate = From + " 00:00:00"; // Convert.ToDateTime();

            String[] StringOrderDate = HSRPAuthDate.SelectedDate.ToString().Split('/');
            string Mon = ("0" + StringOrderDate[0]);
            string Monthdate = Mon.Replace("00", "0").Replace("01", "1");
            string FromDate = StringOrderDate[1] + "-" + Monthdate + "-" + StringOrderDate[2].Split(' ')[0];

            String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
            //OrderDate1 = new DateTime(Convert.ToInt32(StringOrderDate[2].Split(' ')[0]), Convert.ToInt32(StringOrderDate[0]), Convert.ToInt32(StringOrderDate[1]));
            string OrderDate1 = From1 + " 23:59:59";

            string SQLString = "select a.hsrprecordid,a.vehicleregno,b.hsrp_front_lasercode,b.hsrp_rear_lasercode from Vendor_HSRPRecords a inner join HSRPRecords as b on" +
" a.vehicleregno=b.vehicleregno where a.hsrp_stateid='" + DropDownListStateName.SelectedValue + "' and a.inv_status ='N' and b.hsrp_front_lasercode!='' and  b.hsrp_rear_lasercode!='' and  a.dealername like'" + ddlDealerName.SelectedItem.ToString() + "%' and a.HSRPRecord_CreationDate between '" + AuthorizationDate + "' and '" + OrderDate1 + "' ";
            dt = Utils.GetDataTable(SQLString, CnnString);
            if (dt.Rows.Count > 0)
            {
                btnSave.Visible = true;
                Button1.Visible = true;
                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
            else
            {
                btnSave.Visible = false;
                lblErrMsg.Text = "Record Not Found";
            }
        }
        StringBuilder sb = new StringBuilder();
        CheckBox chk;

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int invoNo;
            string tmpbillno = string.Empty;
            string SQLString = "select max(invno) as invoiceNo from dealer_InvoiceExcise";
            invoNo = Utils.getScalarCount(SQLString, CnnString);
            invoNo = invoNo + 1;
            string SQLString1 = "insert into dealer_InvoiceExcise(invno,invdate,dispatechedBy) values('" + invoNo + "',GETDATE(),'" + Session["UID"].ToString() + "')";
            int k = Utils.ExecNonQuery(SQLString1, CnnString);


            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                //  string lblVehicleRegNo,FLasercode,RearLaserCode;
                // tmpbillno = Session["UID"].ToString()+ System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() ;
                chk = GridView1.Rows[i].Cells[0].FindControl("CHKSelect") as CheckBox;
                if (chk.Checked == true)
                {
                    //Label lblVehicleRegNo = GridView1.Rows[i].Cells[0].FindControl("lblVehicleRegNo") as Label;
                    //TextBox RearLaserCode = GridView1.Rows[i].Cells[1].FindControl("txtFLaserCode") as TextBox;
                    //TextBox FLasercode = GridView1.Rows[i].Cells[2].FindControl("txtRLaserCode") as TextBox;
                    Label id = GridView1.Rows[i].Cells[4].FindControl("id") as Label;
                    //  labtext = GridView1.Rows[i].Cells[0].Text;
                    sb.Append("update Vendor_HSRPRecords set inv_status ='Y',excise_invno='" + invoNo.ToString() + "' where hsrprecordid='" + id.Text + "'" + Environment.NewLine);
                }
                // Label1.Text = labtext;


            }
            int j = Utils.ExecNonQuery(sb.ToString(), CnnString);
            if (j > 0)
            {

                LblMessage.Text = "Updated Records Sucessfull";
                printInvoice1(invoNo);
                show();
            }
            else
            {
                LblMessage.Text = "Updated Records Not Sucessfull";
            }

        }




        protected void Button1_Click(object sender, EventArgs e)
        {

            string chkb = string.Empty;
            
            //foreach (GridViewRow di in GridView1.Rows)
            //{

            //    CheckBox chkBx = (CheckBox)di.FindControl("CHKSelect");

            //    if (chkBx != null && chkBx.Checked)
            //    {
            //        // chkb = GridView1.Rows[0].Cells[1].FindControl("VehicleRegNo").ToString();
            //        //chkb = (Label)GridView1.Rows[di].FindControl("VehicleRegNo");
            //        // string iAddressId = (string)GridView1.Rows[0].Cells[2].Text;
            //        Label dd = (Label)di.FindControl("lblVehicleRegNo");
            //        VehicleRegNo.Add(dd);
            //        Label dsd = (Label)di.FindControl("txtFLaserCode");
            //        VehicleRegNo.Add(dsd);
            //        Label ddsd = (Label)di.FindControl("txtRlaserCode");
            //        VehicleRegNo.Add(ddsd);
            //    }
            //}



            float Amount = 0;
            string filename = "Order_Open_Records" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".pdf";
            String StringField = String.Empty;
            String StringAlert = String.Empty;
            StringBuilder bb = new StringBuilder();
            // Document document = new Document();
            //  iTextSharp.text.Document document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 0, 0, 0, 0);
            Document document = new Document(new iTextSharp.text.Rectangle(188f, 124f), -30, -30, 8, 0);
            document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
            // iTextSharp.text.Font times = new Font(bfTimes, 12, Font.ITALIC, Color.Black);
            //Creates a Writer that listens to this document and writes the document to the Stream of your choice:
            string PdfFolder = ConfigurationManager.AppSettings["PdfFolder"].ToString() + filename;
            PdfWriter.GetInstance(document, new FileStream(PdfFolder, FileMode.Create));
            //Opens the document:
            document.Open();

            //Adds content to the document:
            // document.Add(new Paragraph("Ignition Log Report"));
            PdfPTable table = new PdfPTable(3);
            //actual width of table in points
           // var colWidthPercentages = new[] { 7f, 12f, 8f, 14f, 10f, 25f, 25f, 30f, 15f, 34f, 15f, 34f, 13f };
           // table.SetWidths(colWidthPercentages);
            table.TotalWidth = 6900f;

            PdfPCell cell120911 = new PdfPCell(new Phrase("HSRP Order Booking Report", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
            cell120911.Colspan = 3;
            cell120911.BorderWidthLeft = 0f;
            cell120911.BorderWidthRight = 0f;
            cell120911.BorderWidthTop = 0f;
            cell120911.BorderWidthBottom = 0f;
            //cell120911.HorizontalAlignment = Element.ALIGN_LEFT;
            cell120911.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            table.AddCell(cell120911);

            //PdfPCell cell12091 = new PdfPCell(new Phrase("State Name : " + dt.Rows[0]["HSRPStateName"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
            //cell12091.Colspan = 9;
            //cell12091.BorderWidthLeft = 1f;
            //cell12091.BorderWidthRight = 0f;
            //cell12091.BorderWidthTop = 1f;
            //cell12091.BorderWidthBottom = 0f;
            //cell12091.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            //table.AddCell(cell12091);


            PdfPCell cell1209 = new PdfPCell(new Phrase("Vehicle No", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
            cell1209.Colspan = 1;
            cell1209.BorderWidthLeft = 0.8f;
            cell1209.BorderWidthRight = .8f;
            cell1209.BorderWidthTop = 0.8f;
            cell1209.BorderWidthBottom = 0.8f;

            cell1209.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            table.AddCell(cell1209);

            PdfPCell cell12233 = new PdfPCell(new Phrase("Front Laser Code", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
            cell12233.Colspan = 1;
            cell12233.BorderWidthLeft = 0f;
            cell12233.BorderWidthRight = .8f;
            cell12233.BorderWidthTop = 0.8f;
            cell12233.BorderWidthBottom = 0.8f; 
            cell12233.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            table.AddCell(cell12233); 

            PdfPCell cell1206 = new PdfPCell(new Phrase("Rear Laser Code", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
            cell1206.Colspan = 1;
            cell1206.BorderWidthLeft = 0f;
            cell1206.BorderWidthRight = .8f;
            cell1206.BorderWidthTop = 0.8f;
            cell1206.BorderWidthBottom = 0.8f;
            cell1206.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right 
            table.AddCell(cell1206);

            foreach (GridViewRow di in GridView1.Rows)
            {
                List<string> lblvehicleRegNo = new List<string>();
                ArrayList VehicleRegNo = new ArrayList();
                ArrayList LaserFront = new ArrayList();
                ArrayList LaserRear = new ArrayList();

                CheckBox chkBx = (CheckBox)di.FindControl("CHKSelect");

                if (chkBx != null && chkBx.Checked)
                {
                    // chkb = GridView1.Rows[0].Cells[1].FindControl("VehicleRegNo").ToString();
                    //chkb = (Label)GridView1.Rows[di].FindControl("VehicleRegNo");
                    // string iAddressId = (string)GridView1.Rows[0].Cells[2].Text;
                    Label dd = (Label)di.FindControl("lblVehicleRegNo");
                    VehicleRegNo.Add(dd);

                    Label dsd = (Label)di.FindControl("txtFLaserCode");
                    LaserFront.Add(dsd);
                    Label ddsd = (Label)di.FindControl("txtRlaserCode");
                    LaserRear.Add(ddsd);
                    
                    string vehic = VehicleRegNo[0].ToString();
                    //string[] strv = (String[])VehicleRegNo.ToArray(typeof(string));
                    

                    PdfPCell cell1209340 = new PdfPCell(new Phrase(dd.Text, new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    cell1209340.Colspan = 1;
                    cell1209340.BorderWidthLeft = .8f;
                    cell1209340.BorderWidthRight = .8f;
                    cell1209340.BorderWidthTop = .8f;
                    cell1209340.BorderWidthBottom = .5f;
                    cell1209340.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell1209340);

                    PdfPCell cell1209340z = new PdfPCell(new Phrase(dsd.Text, new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    cell1209340z.Colspan = 1;
                    cell1209340z.BorderWidthLeft = .8f;
                    cell1209340z.BorderWidthRight = .8f;
                    cell1209340z.BorderWidthTop = .8f;
                    cell1209340z.BorderWidthBottom = .5f;
                    cell1209340z.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell1209340z);


                    PdfPCell cell1209340zq = new PdfPCell(new Phrase(ddsd.Text, new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    cell1209340zq.Colspan = 1;
                    cell1209340zq.BorderWidthLeft = .8f;
                    cell1209340zq.BorderWidthRight = .8f;
                    cell1209340zq.BorderWidthTop = .8f;
                    cell1209340zq.BorderWidthBottom = .5f;
                    cell1209340zq.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell1209340zq);


                }
            }
            document.Add(table);
            document.Close(); 
            HttpContext context = HttpContext.Current; 
            context.Response.ContentType = "Application/pdf";
            context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
            context.Response.WriteFile(PdfFolder);
            context.Response.End();



            //int j = 0; 
            //for (int i = 0; i <= dt.Rows.Count - 1; i++)
            //{
            //    try
            //    {
            //        string mean = "MO.C = MOTOR CYCLE,  L.CL =LMV(CLASS),  TRAC =TRACTOR,  SCOO = SCOOTER,  T.Whe.= THREE WHEELER,  Trailers =MCV/HCV/TRAILERS,  T = Transport, N.T.= Non-Transport                                                                                 Net Amount : '" + Amount + "'";
            //        PdfPCell cell1209340 = new PdfPCell(new Phrase(mean, new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
            //        cell1209340.Colspan = 14;
            //        cell1209340.BorderWidthLeft = .8f;
            //        cell1209340.BorderWidthRight = .8f;
            //        cell1209340.BorderWidthTop = .8f;
            //        cell1209340.BorderWidthBottom = .5f;
            //        cell1209340.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            //        table.AddCell(cell1209340);

            //        PdfPCell cell12241 = new PdfPCell(new Phrase("(Sign)", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
            //        cell12241.Colspan = 7;
            //        cell12241.BorderWidthLeft = 0f;
            //        cell12241.BorderWidthRight = 0f;
            //        cell12241.BorderWidthTop = 0f;
            //        cell12241.BorderWidthBottom = 0f;
            //        cell12241.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            //        table.AddCell(cell12241);
            //        document.Add(table);
            //        // document.Add(table1); 

            //        document.Close();

            //        HttpContext context = HttpContext.Current;

            //        context.Response.ContentType = "Application/pdf";
            //        context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
            //        context.Response.WriteFile(PdfFolder);
            //        context.Response.End();

            //    }
            //    catch
            //    {
            //    }
            //}
        }
         
        protected void CHKSelect1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk1 = GridView1.HeaderRow.FindControl("CHKSelect1") as CheckBox;
            if (chk1.Checked == true)
            {
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    //  string lblVehicleRegNo,FLasercode,RearLaserCode;
                    chk = GridView1.Rows[i].Cells[0].FindControl("CHKSelect") as CheckBox;
                    chk.Checked = true;
                }
            }
            else if (chk1.Checked == false)
            {
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    //  string lblVehicleRegNo,FLasercode,RearLaserCode;
                    chk = GridView1.Rows[i].Cells[0].FindControl("CHKSelect") as CheckBox;
                    chk.Checked = false;
                }
            }

        }
        DataSet dt1 = new DataSet();

        decimal LMVAmount = 0, MOTORCYCLEAmount = 0, THREEWHEELERAmount = 0, ScooterAmount = 0;
        decimal LMVTax = 0, MOTORCYCLETax = 0, THREEWHEELERTax = 0, ScooterTax = 0;
        decimal AMOUNT = 0;
        decimal totalamount = 0;
        decimal to = 0,QtyTo=0;

        private void printInvoice1(int invoNo)
        {
            

            HSRPStateID = Session["UserHSRPStateID"].ToString();

            DataTable GetAddress;
            string Address;
            string vat = string.Empty;
            string stateaddress = string.Empty;
            string Division = string.Empty;
            string Range = string.Empty;
            string Commission = string.Empty;
            GetAddress = Utils.GetDataTable("select * from HSRPState WHERE HSRP_StateID='" + HSRPStateID + "'", CnnString);
            stateaddress = GetAddress.Rows[0]["Address3"].ToString();
            vat = GetAddress.Rows[0]["CSTVAT"].ToString();
            Division = GetAddress.Rows[0]["Division"].ToString();
            Range = GetAddress.Rows[0]["Range"].ToString();
            Commission = GetAddress.Rows[0]["Commissionerate"].ToString();

            if (GetAddress.Rows[0]["pincode"] != "" || GetAddress.Rows[0]["pincode"] != null)
            {
                Address = " - " + GetAddress.Rows[0]["pincode"];
            }
            else
            {
                Address = "";
            }


            string HSRPRecordID = string.Empty;
            //HSRPRecordID = Request.QueryString["HSRPRecordID"].ToString();
            //string OrderStatus = "Closed";
            //if (OrderStatus == "Closed")
            //{

            DataTable DealerInvoice1 = new DataTable();
            BAL obj = new BAL();
            //string id = "1";

            string SQLString = "select TOP 1* from dbo.dealer_InvoiceExcise ORDER BY ID DESC ";
            dt = Utils.GetDataTable(SQLString, CnnString);
            if (dt.Rows.Count > 0)
            {


                string filename = "HSRP INVOICE" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".pdf";


                String StringField = String.Empty;
                String StringAlert = String.Empty;

                StringBuilder bb = new StringBuilder();

                //Creates an instance of the iTextSharp.text.Document-object:
                Document document = new Document(PageSize.A4, 10, 10, 10, 10);

                BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);


                string PdfFolder = ConfigurationManager.AppSettings["PdfFolder"].ToString() + filename;
                PdfWriter.GetInstance(document, new FileStream(PdfFolder, FileMode.Create));

                //Opens the document:
                document.Open();

                PdfPTable table2 = new PdfPTable(7);
                PdfPTable table1 = new PdfPTable(7);
                PdfPTable table = new PdfPTable(7);

                //var colWidthPercentages = new[] { 5f, 12f, 8f, 14f, 10f, 25f, 25f, 30f, 12f, 15f, 23f, 15f, 23f, 20f };
                //table2.SetWidths(colWidthPercentages);

                //var colWidthPercentages = new[] { 5f, 12f, 8f, 14f, 10f, 25f, 25f, 30f, 12f, 15f, 23f, 15f, 23f, 20f };
                //table2.SetWidths(colWidthPercentages);

                //table.TotalWidth = 6900f;

                //actual width of table in points
                //table.TotalWidth = 1000f;
                // table.TotalHeight = 1000f;

                //fix the absolute width of the table
                PdfPCell cell1211 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell1211.Colspan = 1;
                cell1211.BorderWidthLeft = 0f;
                cell1211.BorderWidthRight = 0f;
                cell1211.BorderWidthTop = 0f;
                cell1211.BorderWidthBottom = 0f;

                cell1211.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table1.AddCell(cell1211);
                PdfPCell cell12111 = new PdfPCell(new Phrase("EXCISE INVOICE", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell12111.Colspan = 6;
                cell12111.BorderWidthLeft = 0f;
                cell12111.BorderWidthRight = 0f;
                cell12111.BorderWidthTop = 0f;
                cell12111.BorderWidthBottom = 0f;

                cell12111.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table1.AddCell(cell12111);


                PdfPCell cell13699 = new PdfPCell(new Phrase("----------------------------------------------------------------------------------------------------------------------------", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell13699.Colspan = 7;

                cell13699.BorderWidthLeft = 0f;
                cell13699.BorderWidthRight = 0f;
                cell13699.BorderWidthBottom = 0f;
                cell13699.BorderColor = BaseColor.WHITE;
                cell13699.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                table2.AddCell(cell13699);

                PdfPCell cell12112 = new PdfPCell(new Phrase("Assessee", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell12112.Colspan = 1;

                cell12112.BorderWidthLeft = 1f;
                cell12112.BorderWidthRight = 1f;
                cell12112.BorderWidthTop = 1f;
                cell12112.BorderWidthBottom = 1f;

                cell12112.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table2.AddCell(cell12112);
                PdfPCell cell12113 = new PdfPCell(new Phrase("EXCISE INVOICE", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell12113.Colspan = 6;
                cell12113.BorderWidthLeft = 0f;
                cell12113.BorderWidthRight = 0f;
                cell12113.BorderWidthTop = 0f;
                cell12113.BorderWidthBottom = 0f;

                cell12113.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table2.AddCell(cell12113);

                PdfPCell cell12111a = new PdfPCell(new Phrase("(ISSUE OF INVOICE UNDER RULE 11 OF CENTRAL EXCISE RULES 2002)", new iTextSharp.text.Font(bfTimes, 9f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell12111a.Colspan = 6;
                cell12111a.BorderWidthLeft = 0f;
                cell12111a.BorderWidthRight = 0f;
                cell12111a.BorderWidthTop = 0f;
                cell12111a.BorderWidthBottom = 0f;
                cell12111a.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table1.AddCell(cell12111a);


                PdfPCell cell122 = new PdfPCell(new Phrase("Link Utsav Registration Plates Pvt. Ltd.", new iTextSharp.text.Font(bfTimes, 12f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell122.Colspan = 7;
                cell122.FixedHeight = 30f;
                cell122.BorderWidthLeft = 1f;
                cell122.BorderWidthRight = .8f;
                cell122.BorderWidthTop = .8f;
                cell122.BorderWidthBottom = 0f; 
                cell122.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell122);

                PdfPCell cell123 = new PdfPCell(new Phrase(stateaddress, new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell123.Colspan = 7;
                cell123.BorderWidthLeft = 1f;
                cell123.BorderWidthRight = .8f;
                cell123.BorderWidthTop = 0f;
                cell123.BorderWidthBottom = .8f;

                cell123.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell123);

                PdfPCell cell124 = new PdfPCell(new Phrase("VAT TIN  : ", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell124.Colspan = 1;
                cell124.BorderWidthLeft = 1f;
                cell124.BorderWidthRight = 0f;
                cell124.BorderWidthTop = 0f;
                cell124.BorderWidthBottom = 0f;

                cell124.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell124);

                PdfPCell cell125 = new PdfPCell(new Phrase(GetAddress.Rows[0]["TinNo"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell125.Colspan = 2;
                cell125.BorderWidthLeft = 0f;
                cell125.BorderWidthRight = 0f;
                cell125.BorderWidthTop = 0f;
                cell125.BorderWidthBottom = 0f;

                cell125.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell125);

                PdfPCell cell126 = new PdfPCell(new Phrase("Range  :"+Range, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell126.Colspan = 2;
                cell126.BorderWidthLeft = 0f;
                cell126.BorderWidthRight = 0f;
                cell126.BorderWidthTop = 0f;
                cell126.BorderWidthBottom = 0f;

                cell126.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell126);

                PdfPCell cell127 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell127.Colspan = 2;
                cell127.BorderWidthLeft = 0f;
                cell127.BorderWidthRight = 1f;
                cell127.BorderWidthTop = 0f;
                cell127.BorderWidthBottom = 0f;

                cell127.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell127);



                PdfPCell cell128 = new PdfPCell(new Phrase("Excise Regn No.  : ", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell128.Colspan = 1;
                cell128.BorderWidthLeft = 1f;
                cell128.BorderWidthRight = 0f;
                cell128.BorderWidthTop = 0f;
                cell128.BorderWidthBottom = 0f;

                cell128.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell128);

                PdfPCell cell129 = new PdfPCell(new Phrase(GetAddress.Rows[0]["CERC"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell129.Colspan = 2;
                cell129.BorderWidthLeft = 0f;
                cell129.BorderWidthRight = 0f;
                cell129.BorderWidthTop = 0f;
                cell129.BorderWidthBottom = 0f;
                
                cell129.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell129);

                PdfPCell cell130 = new PdfPCell(new Phrase("Division  : " +Division, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell130.Colspan = 2;
                cell130.BorderWidthLeft = 0f;
                cell130.BorderWidthRight = 0f;
                cell130.BorderWidthTop = 0f;
                cell130.BorderWidthBottom = 0f;

                cell130.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell130);

                PdfPCell cell131 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell131.Colspan = 2;
                cell131.BorderWidthLeft = 0f;
                cell131.BorderWidthRight = 1f;
                cell131.BorderWidthTop = 0f;
                cell131.BorderWidthBottom = 0f;

                cell131.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell131);


                PdfPCell cell132 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell132.Colspan = 1;
                cell132.BorderWidthLeft = 1f;
                cell132.BorderWidthRight = 0f;
                cell132.BorderWidthTop = 0f;
                cell132.BorderWidthBottom = 0f;

                cell132.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell132);

                PdfPCell cell133 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell133.Colspan = 2;
                cell133.BorderWidthLeft = 0f;
                cell133.BorderWidthRight = 0f;
                cell133.BorderWidthTop = 0f;
                cell133.BorderWidthBottom = 0f;

                cell133.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell133);

                PdfPCell cell134 = new PdfPCell(new Phrase("Commissionerate : "+Commission, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell134.Colspan = 2;
                cell134.BorderWidthLeft = 0f;
                cell134.BorderWidthRight = 0f;
                cell134.BorderWidthTop = 0f;
                cell134.BorderWidthBottom = 0f;

                cell134.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell134);

                PdfPCell cell135 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell135.Colspan = 2;
                cell135.BorderWidthLeft = 0f;
                cell135.BorderWidthRight = 1f;
                cell135.BorderWidthTop = 0f;
                cell135.BorderWidthBottom = 0f;

                cell135.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell135);


                DataTable add = Utils.GetDataTable("select (address+' '+city+' '+State) as address from dbo.dealerMaster where Dealername='" + ddlDealerName.SelectedItem.ToString() + "'", ConnectionStringdealer1);
                string address1=string.Empty;
                if (add.Rows.Count>0)
                {
                    address1 = add.Rows[0]["address"].ToString();
                }
                else
                {
                    address1 = "";
                }


                PdfPCell cell12 = new PdfPCell(new Phrase("Buyer ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell12.Colspan = 3;
                cell12.FixedHeight = 18f;
                cell12.BorderWidthLeft = 1f;
                cell12.BorderWidthRight = .8f;
                cell12.BorderWidthTop = .8f;
                cell12.BorderWidthBottom = 0f;

                cell12.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12);

                PdfPCell cell1207 = new PdfPCell(new Phrase("Invoice No : " + dt.Rows[0]["invno"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell1207.Colspan = 2;
                cell1207.BorderWidthLeft = 0f;
                cell1207.BorderWidthRight = .8f;
                cell1207.BorderWidthTop = .8f;
                cell1207.BorderWidthBottom = 0f;

                cell1207.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1207);

            
                PdfPCell cell1208 = new PdfPCell(new Phrase("Date: " + DateTime.Parse(dt.Rows[0]["invdate"].ToString()).ToString("dd/MM/yyyy"), new iTextSharp.text.Font(bfTimes, 5f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell1208.Colspan = 2;
                cell1208.BorderWidthLeft = 0f;
                cell1208.BorderWidthRight = 1f;
                cell1208.BorderWidthTop = .8f;
                cell1208.BorderWidthBottom = 0f;

                cell1208.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1208);



                PdfPCell cell1203 = new PdfPCell(new Phrase(""+ddlDealerName.SelectedItem.ToString() , new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell1203.Colspan = 3;
                cell1203.FixedHeight = 18f;
                cell1203.BorderWidthLeft = 1f;
                cell1203.BorderWidthRight = .8f;
                cell1203.BorderWidthTop = 0f;
                cell1203.BorderWidthBottom = 0f;
                cell1203.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1203);

                PdfPCell cell2 = new PdfPCell(new Phrase("Delivery Note", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell2.Colspan = 2;

                cell2.BorderWidthLeft = 0f;
                cell2.BorderWidthRight = .8f;
                cell2.BorderWidthTop = .8f;
                cell2.BorderWidthBottom = 0f;
                cell2.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell2);

                // string getTinNo1 = Utils.getScalarValue("select TinNo from hsrpstate where HSRP_StateID='" + HSRPStateID + "' ", CnnString);

                PdfPCell cell21111 = new PdfPCell(new Phrase("Mode/Terms of Payment: ", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell21111.Colspan = 2;
                cell21111.BorderWidthLeft = 0f;
                cell21111.BorderWidthRight = 1f;
                cell21111.BorderWidthTop = .8f;
                cell21111.BorderWidthBottom = 0f;
                cell21111.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell21111);


                PdfPCell cell1204 = new PdfPCell(new Phrase("" + address1, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell1204.Colspan = 3;
                cell1204.FixedHeight = 18f;
                cell1204.BorderWidthLeft = 1f;
                cell1204.BorderWidthRight = .8f;
                cell1204.BorderWidthTop = 0f;
                cell1204.BorderWidthBottom = 0f;
                cell1204.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1204);


                PdfPCell cell1209 = new PdfPCell(new Phrase("Supplier's Ref.", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell1209.Colspan = 2;
                cell1209.BorderWidthLeft = 0f;
                cell1209.BorderWidthRight = .8f;
                cell1209.BorderWidthTop = .8f;
                cell1209.BorderWidthBottom = 0f;

                cell1209.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1209);


                PdfPCell cell12115 = new PdfPCell(new Phrase("Other Reference (s): ", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell12115.Colspan = 2;
                cell12115.BorderWidthLeft = 0f;
                cell12115.BorderWidthRight = 1f;
                cell12115.BorderWidthTop = .8f;
                cell12115.BorderWidthBottom = 0f;

                cell12115.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12115);


                PdfPCell cell1205 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell1205.Colspan = 3;
                cell1205.FixedHeight = 18f;
                cell1205.BorderWidthLeft = 1f;
                cell1205.BorderWidthRight = .8f;
                cell1205.BorderWidthTop = 0f;
                cell1205.BorderWidthBottom = 0f;
                cell1205.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1205);


                PdfPCell cell1213 = new PdfPCell(new Phrase("Buyer's Order No.", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell1213.Colspan = 2;
                cell1213.BorderWidthLeft = 0f;
                cell1213.BorderWidthRight = .8f;
                cell1213.BorderWidthTop = .8f;
                cell1213.BorderWidthBottom = 0f;

                cell1213.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1213);


                PdfPCell cell1214 = new PdfPCell(new Phrase("Dated ", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell1214.Colspan = 2;
                cell1214.BorderWidthLeft = 0f;
                cell1214.BorderWidthRight = 1f;
                cell1214.BorderWidthTop = .8f;
                cell1214.BorderWidthBottom = 0f;

                cell1214.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1214);

                PdfPCell cell1206 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell1206.Colspan = 3;
                cell1206.FixedHeight = 18f;
                cell1206.BorderWidthLeft = 1f;
                cell1206.BorderWidthRight = .8f;
                cell1206.BorderWidthTop = 0f;
                cell1206.BorderWidthBottom = 0f;
                cell1206.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1206);


                PdfPCell cell1215 = new PdfPCell(new Phrase("Dispatch Document No", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell1215.Colspan = 2;
                cell1215.BorderWidthLeft = 0f;
                cell1215.BorderWidthRight = .8f;
                cell1215.BorderWidthTop = .8f;
                cell1215.BorderWidthBottom = 0f;

                cell1215.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1215);


                PdfPCell cell1216 = new PdfPCell(new Phrase("Dated ", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell1216.Colspan = 2;
                cell1216.BorderWidthLeft = 0f;
                cell1216.BorderWidthRight = 1f;
                cell1216.BorderWidthTop = .8f;
                cell1216.BorderWidthBottom = 0f;

                cell1216.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1216);




                PdfPCell cell12062 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell12062.Colspan = 3;
                cell12062.FixedHeight = 18f;
                cell12062.BorderWidthLeft = 1f;
                cell12062.BorderWidthRight = .8f;
                cell12062.BorderWidthTop = 0f;
                cell12062.BorderWidthBottom = 0f;
                cell12062.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12062);


                PdfPCell cell12063 = new PdfPCell(new Phrase("Dispatch Through : " + Session["username"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell12063.Colspan = 2;
                cell12063.BorderWidthLeft = 0f;
                cell12063.BorderWidthRight = .8f;
                cell12063.BorderWidthTop = .8f;
                cell12063.BorderWidthBottom = 0f;

                cell12063.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12063);


                PdfPCell cell12064 = new PdfPCell(new Phrase("Destination ", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell12064.Colspan = 2;
                cell12064.BorderWidthLeft = 0f;
                cell12064.BorderWidthRight = 1f;
                cell12064.BorderWidthTop = .8f;
                cell12064.BorderWidthBottom = 0f;

                cell12064.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12064);


                PdfPCell cell12069 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell12069.Colspan = 3;
                cell12069.FixedHeight = 18f;
                cell12069.BorderWidthLeft = 1f;
                cell12069.BorderWidthRight = .8f;
                cell12069.BorderWidthTop = 0f;
                cell12069.BorderWidthBottom = 0f;
                cell12069.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12069);


                PdfPCell cell120610 = new PdfPCell(new Phrase("Date & Time of Issue of Invoice " + DateTime.Parse(dt.Rows[0]["invdate"].ToString()).ToString("dd/MM/yyyy"), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell120610.Colspan = 2;
                cell120610.BorderWidthLeft = 0f;
                cell120610.BorderWidthRight = .8f;
                cell120610.BorderWidthTop = .8f;
                cell120610.BorderWidthBottom = 0f;

                cell120610.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell120610);

                PdfPCell cell120611 = new PdfPCell(new Phrase("Moter vehicle No", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell120611.Colspan = 2;
                cell120611.BorderWidthLeft = 0f;
                cell120611.BorderWidthRight = .8f;
                cell120611.BorderWidthTop = .8f;
                cell120611.BorderWidthBottom = 0f;

                cell120611.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell120611);



                PdfPCell cell120612 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell120612.Colspan = 3;
                cell120612.FixedHeight = 18f;
                cell120612.BorderWidthLeft = 1f;
                cell120612.BorderWidthRight = .8f;
                cell120612.BorderWidthTop = 0f;
                cell120612.BorderWidthBottom = 0f;
                cell120612.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell120612);


                PdfPCell cell120613 = new PdfPCell(new Phrase("Date & Time of Removal of goods " + DateTime.Parse(dt.Rows[0]["invdate"].ToString()).ToString("dd/MM/yyyy"), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell120613.Colspan = 2;
                cell120613.BorderWidthLeft = 0f;
                cell120613.BorderWidthRight = .8f;
                cell120613.BorderWidthTop = .8f;
                cell120613.BorderWidthBottom = 0f;

                cell120613.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell120613);

                PdfPCell cell120614 = new PdfPCell(new Phrase("Authenticated By", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell120614.Colspan = 2;
                cell120614.BorderWidthLeft = 0f;
                cell120614.BorderWidthRight = .8f;
                cell120614.BorderWidthTop = .8f;
                cell120614.BorderWidthBottom = 0f;

                cell120614.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell120614);


                PdfPCell cell120615 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell120615.Colspan = 3;
                cell120615.FixedHeight = 18f;
                cell120615.BorderWidthLeft = 1f;
                cell120615.BorderWidthRight = .8f;
                cell120615.BorderWidthTop = 0f;
                cell120615.BorderWidthBottom = 0f;
                cell120615.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell120615);


                PdfPCell cell120616 = new PdfPCell(new Phrase("Mode/Terms of Payment", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell120616.Colspan = 2;
                cell120616.BorderWidthLeft = 0f;
                cell120616.BorderWidthRight = .8f;
                cell120616.BorderWidthTop = .8f;
                cell120616.BorderWidthBottom = 0f;

                cell120616.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell120616);

                PdfPCell cell120617 = new PdfPCell(new Phrase("Link Utsav Registration Plates Pvt. Ltd.", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell120617.Colspan = 2;
                cell120617.BorderWidthLeft = 0f;
                cell120617.BorderWidthRight = .8f;
                cell120617.BorderWidthTop = .8f;
                cell120617.BorderWidthBottom = 0f;

                cell120617.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell120617);


                PdfPCell cell120618 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell120618.Colspan = 3;
                cell120618.FixedHeight = 18f;
                cell120618.BorderWidthLeft = 1f;
                cell120618.BorderWidthRight = .8f;
                cell120618.BorderWidthTop = 0f;
                cell120618.BorderWidthBottom = 0f;
                cell120618.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell120618);

                PdfPCell cell1206181 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell1206181.Colspan = 2;
                cell1206181.FixedHeight = 18f;
                cell1206181.BorderWidthLeft = 0f;
                cell1206181.BorderWidthRight = .8f;
                cell1206181.BorderWidthTop = 0f;
                cell1206181.BorderWidthBottom = 0f;
                cell1206181.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1206181);


                PdfPCell cell120619 = new PdfPCell(new Phrase("Authorised Signatory", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell120619.Colspan = 2;
                cell120619.BorderWidthLeft = 0f;
                cell120619.BorderWidthRight = 0.8f;
                cell120619.BorderWidthTop = 0f;
                cell120619.BorderWidthBottom = 0f;

                cell120619.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell120619);




                PdfPCell cell1248 = new PdfPCell(new Phrase("S.No.", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell1248.Colspan = 1;
                cell1248.BorderWidthLeft = 1f;
                cell1248.BorderWidthRight = .8f;
                cell1248.BorderWidthTop = .8f;
                cell1248.BorderWidthBottom = 0f;

                cell1248.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1248);


                PdfPCell cell1249 = new PdfPCell(new Phrase("Description & Specification of Goods", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell1249.Colspan = 2;
                cell1249.BorderWidthLeft = 0f;
                cell1249.BorderWidthRight = .8f;
                cell1249.BorderWidthTop = .8f;
                cell1249.BorderWidthBottom = 0f;

                cell1249.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1249);

                PdfPCell cell1250 = new PdfPCell(new Phrase("Tariff /HSN classification", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell1250.Colspan = 1;
                cell1250.BorderWidthLeft = 0f;
                cell1250.BorderWidthRight = .8f;
                cell1250.BorderWidthTop = .8f;
                cell1250.BorderWidthBottom = 0f;

                cell1250.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1250);

                PdfPCell cell1251 = new PdfPCell(new Phrase("Quantity", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell1251.Colspan = 1;
                cell1251.BorderWidthLeft = 0f;
                cell1251.BorderWidthRight = .8f;
                cell1251.BorderWidthTop = .8f;
                cell1251.BorderWidthBottom = 0f;

                cell1251.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1251);

                PdfPCell cell1252 = new PdfPCell(new Phrase("Price Per Unit", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell1252.Colspan = 1;
                cell1252.BorderWidthLeft = 0f;
                cell1252.BorderWidthRight = .8f;
                cell1252.BorderWidthTop = .8f;
                cell1252.BorderWidthBottom = 0f;

                cell1252.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1252);

                PdfPCell cell1253 = new PdfPCell(new Phrase("Amount Rs.", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell1253.Colspan = 1;
                cell1253.BorderWidthLeft = 0f;
                cell1253.BorderWidthRight = 1f;
                cell1253.BorderWidthTop = .8f;
                cell1253.BorderWidthBottom = 0f;

                cell1253.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1253);


                string ReportTimees = "select vehicletype,count(*) as Quantity from [Vendor_HSRPRecords] where excise_invno='" + dt.Rows[0]["invno"].ToString() + "' group by vehicletype";
                DataTable dt3 = Utils.GetDataTable(ReportTimees, CnnString);
                // int time = Convert.ToInt16(dtreport.Rows[0]["currenttime"].ToString());


                dt = Utils.GetDataTable("select dbo.hsrpplateamt ('" + Session["UserHSRPStateID"].ToString() + "','LMV','Non-Transport','NB') as Amount", CnnString);
                LMVAmount = decimal.Parse(dt.Rows[0]["Amount"].ToString());

                dt = Utils.GetDataTable("select dbo.hsrpplateamt ('" + Session["UserHSRPStateID"].ToString() + "','MOTOR CYCLE','Non-Transport','NB') as Amount", CnnString);
                MOTORCYCLEAmount = decimal.Parse(dt.Rows[0]["Amount"].ToString());

                dt = Utils.GetDataTable("select dbo.hsrpplateamt ('" + Session["UserHSRPStateID"].ToString() + "','THREE WHEELER','Non-Transport','NB') as Amount", CnnString);
                THREEWHEELERAmount = decimal.Parse(dt.Rows[0]["Amount"].ToString());

                dt = Utils.GetDataTable("select dbo.hsrpplateamt ('" + Session["UserHSRPStateID"].ToString() + "','SCOOTER','Non-Transport','NB') as Amount", CnnString);
                ScooterAmount = decimal.Parse(dt.Rows[0]["Amount"].ToString());


                //dt = Utils.GetDataTable("select dbo.hsrpplatetax ('" + Session["UserHSRPStateID"].ToString() + "','LMV','Non-Transport','NB') as Amount", CnnString);
                //LMVTax = decimal.Parse(dt.Rows[0]["tax"].ToString());

                //dt = Utils.GetDataTable("select dbo.hsrpplatetax ('" + Session["UserHSRPStateID"].ToString() + "','MOTOR CYCLE','Non-Transport','NB') as Amount", CnnString);
                //MOTORCYCLETax = decimal.Parse(dt.Rows[0]["tax"].ToString());

                //dt = Utils.GetDataTable("select dbo.hsrpplatetax ('" + Session["UserHSRPStateID"].ToString() + "','THREE WHEELER','Non-Transport','NB') as Amount", CnnString);
                //THREEWHEELERTax = decimal.Parse(dt.Rows[0]["tax"].ToString());

                //dt = Utils.GetDataTable("select dbo.hsrpplatetax ('" + Session["UserHSRPStateID"].ToString() + "','SCOOTER','Non-Transport','NB') as Amount", CnnString);
                //ScooterTax = decimal.Parse(dt.Rows[0]["tax"].ToString());

                int k = 0;
                if (dt3.Rows.Count > 0)
                {
                    for (int i = 0; i < dt3.Rows.Count; i++)
                    {
                        k = k + 1;
                        PdfPCell cell12555 = new PdfPCell(new Phrase("" + k.ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell12555.Colspan = 1;
                        cell12555.BorderWidthLeft = 1f;
                        cell12555.BorderWidthRight = 0f;
                        cell12555.BorderWidthTop = .8f;
                        cell12555.BorderWidthBottom = 0f;

                        cell12555.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell12555);



                        PdfPCell cell1255 = new PdfPCell(new Phrase("" + dt3.Rows[i]["vehicletype"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1255.Colspan = 2;
                        cell1255.BorderWidthLeft = .8f;
                        cell1255.BorderWidthRight = .8f;
                        cell1255.BorderWidthTop = .8f;
                        cell1255.BorderWidthBottom = 0f;

                        cell1255.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1255);


                        PdfPCell cell1257 = new PdfPCell(new Phrase("83100090", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1257.Colspan = 1;
                        cell1257.BorderWidthLeft = 0f;
                        cell1257.BorderWidthRight = .8f;
                        cell1257.BorderWidthTop = .8f;
                        cell1257.BorderWidthBottom = 0f;

                        cell1257.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1257);


                        PdfPCell cell1256 = new PdfPCell(new Phrase("" + dt3.Rows[i]["Quantity"].ToString() + " Set", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1256.Colspan = 1;
                        cell1256.BorderWidthLeft = 0f;
                        cell1256.BorderWidthRight = .8f;
                        cell1256.BorderWidthTop = .8f;
                        cell1256.BorderWidthBottom = 0f;

                        cell1256.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1256);



                        if (dt3.Rows[i]["vehicletype"].ToString() == "LMV")
                        {
                            AMOUNT = LMVAmount + LMVTax;
                            totalamount = (LMVAmount + LMVTax) * decimal.Parse(dt3.Rows[i]["Quantity"].ToString());
                        }
                        else if (dt3.Rows[i]["vehicletype"].ToString() == "MOTOR CYCLE")
                        {
                            AMOUNT = MOTORCYCLEAmount + MOTORCYCLETax;
                            totalamount = (MOTORCYCLEAmount + MOTORCYCLETax) * decimal.Parse(dt3.Rows[i]["Quantity"].ToString());
                        }
                        else if (dt3.Rows[i]["vehicletype"].ToString() == "SCOOTER")
                        {
                            AMOUNT = ScooterAmount + ScooterTax;
                            totalamount = (ScooterAmount + ScooterTax) * decimal.Parse(dt3.Rows[i]["Quantity"].ToString());
                        }
                        else if (dt3.Rows[i]["vehicletype"].ToString() == "THREE WHEELER")
                        {
                            AMOUNT = THREEWHEELERAmount + THREEWHEELERTax;
                            totalamount = (THREEWHEELERAmount + THREEWHEELERTax) * decimal.Parse(dt3.Rows[i]["Quantity"].ToString());
                        }


                     
                        PdfPCell cell1258 = new PdfPCell(new Phrase(AMOUNT.ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1258.Colspan = 1;
                        cell1258.BorderWidthLeft = 0f;
                        cell1258.BorderWidthRight = .8f;
                        cell1258.BorderWidthTop = .8f;
                        cell1258.BorderWidthBottom = 0f;

                        cell1258.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1258);



                        PdfPCell cell1259 = new PdfPCell(new Phrase(Math.Round(totalamount,2).ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1259.Colspan = 1;
                        cell1259.BorderWidthLeft = 0f;
                        cell1259.BorderWidthRight = 1f;
                        cell1259.BorderWidthTop = .8f;
                        cell1259.BorderWidthBottom = 0f;

                        cell1259.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1259);

                        QtyTo = decimal.Parse(dt3.Rows[i]["Quantity"].ToString()) + QtyTo;
                        to= Math.Round(totalamount + to,2);
                    }
                }


                PdfPCell cell2377 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell2377.Colspan = 1;
              
                cell2377.BorderWidthLeft = 1f;
                cell2377.BorderWidthRight = 0f;
                cell2377.BorderWidthTop = 0f;
                cell2377.BorderWidthBottom = 0f;

                cell2377.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell2377);



                PdfPCell cell2388 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell2388.Colspan = 2;
                cell2388.BorderWidthLeft = .8f;
                cell2388.BorderWidthRight = .8f;
                cell2388.BorderWidthTop = 0f;
                cell2388.BorderWidthBottom = 0f;

                cell2388.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell2388);

                PdfPCell cell2399 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell2399.Colspan = 1;
                cell2399.BorderWidthLeft = 0f;
                cell2399.BorderWidthRight = .8f;
                cell2399.BorderWidthTop = 0f;
                cell2399.BorderWidthBottom = 0f;

                cell2399.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell2399);

                // int FQt5 = 0;
                //if (dataSetFillHSRPDeliveryChallan.Rows[0]["ISFrontPlateSize"].ToString() != "NULL" || dataSetFillHSRPDeliveryChallan.Rows[0]["ISFrontPlateSize"].ToString() != " ")
                //{

                //    if (dataSetFillHSRPDeliveryChallan.Rows[0]["ISFrontPlateSize"].ToString() == "Y")
                //    {
                //        FQt = 1;
                //    }
                //    else
                //    {
                //        FQt = 0;
                //    }
                //}

                PdfPCell cell2400 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell2400.Colspan = 1;
                cell2400.BorderWidthLeft = 0f;
                cell2400.BorderWidthRight = .8f;
                cell2400.BorderWidthTop = 0f;
                cell2400.BorderWidthBottom = 0f;

                cell2400.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell2400);

                //decimal amount4 = Convert.ToDecimal(dataSetFillHSRPDeliveryChallan.Rows[0]["TotalAmount"]);
                //decimal amount5 = Math.Round(amount4, 2);
                // decimal amount9 = 0;
                PdfPCell cell2411 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell2411.Colspan = 1;
                cell2411.BorderWidthLeft = 0f;
                cell2411.BorderWidthRight = .8f;
                cell2411.BorderWidthTop = 0f;
                cell2411.BorderWidthBottom = 0f;

                cell2411.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell2411);



                PdfPCell cell2422 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell2422.Colspan = 1;
                cell2422.BorderWidthLeft = 0f;
                cell2422.BorderWidthRight = 1f;
                cell2422.BorderWidthTop = 0f;
                cell2422.BorderWidthBottom = 0f;

                cell2422.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell2422);




                //PdfPCell cell12589 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                //cell12589.Colspan = 7;
                //cell12589.PaddingTop = 8f;
                //cell12589.BorderWidthLeft = .8f;
                //cell12589.BorderWidthRight = .8f;
                //cell12589.BorderWidthTop = 0f;
                //cell12589.BorderWidthBottom = 0f;

                //cell12589.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                //table.AddCell(cell12589);

                PdfPCell cell125555 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell125555.Colspan = 1;
                cell125555.BorderWidthLeft = 1f;
                cell125555.BorderWidthRight = 0f;
                cell125555.BorderWidthTop = 0f;
                cell125555.BorderWidthBottom = 0f;

                cell125555.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell125555);



                PdfPCell cell12554 = new PdfPCell(new Phrase("Output Basic Excise Duty", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell12554.Colspan = 2;
                cell12554.BorderWidthLeft = .8f;
                cell12554.BorderWidthRight = .8f;
                cell12554.BorderWidthTop = 0f;
                cell12554.BorderWidthBottom = 0f;

                cell12554.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12554);

                PdfPCell cell12565 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell12565.Colspan = 1;
                cell12565.BorderWidthLeft = 0f;
                cell12565.BorderWidthRight = .8f;
                cell12565.BorderWidthTop = 0f;
                cell12565.BorderWidthBottom = 0f;

                cell12565.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12565);


                PdfPCell cell12575 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell12575.Colspan = 1;
                cell12575.BorderWidthLeft = 0f;
                cell12575.BorderWidthRight = .8f;
                cell12575.BorderWidthTop = 0f;
                cell12575.BorderWidthBottom = 0f;

                cell12575.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12575);


                PdfPCell cell12585 = new PdfPCell(new Phrase("12 %", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell12585.Colspan = 1;
                cell12585.BorderWidthLeft = 0f;
                cell12585.BorderWidthRight = .8f;
                cell12585.BorderWidthTop = 0f;
                cell12585.BorderWidthBottom = 0f;

                cell12585.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12585);
                decimal OBED;
               
                OBED = Math.Round(to * 12 / 100,2);

                PdfPCell cell12595 = new PdfPCell(new Phrase(OBED.ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell12595.Colspan = 1;
                cell12595.BorderWidthLeft = 0f;
                cell12595.BorderWidthRight = 1f;
                cell12595.BorderWidthTop = 0f;
                cell12595.BorderWidthBottom = 0f;

                cell12595.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12595);


                PdfPCell cell1 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell1.Colspan = 1;
                cell1.BorderWidthLeft = 1f;
                cell1.BorderWidthRight = 0f;
                cell1.BorderWidthTop = 0f;
                cell1.BorderWidthBottom = 0f;

                cell1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1);



                PdfPCell cell11 = new PdfPCell(new Phrase("Output Education Cess", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell11.Colspan = 2;
                cell11.BorderWidthLeft = .8f;
                cell11.BorderWidthRight = .8f;
                cell11.BorderWidthTop = 0f;
                cell11.BorderWidthBottom = 0f;

                cell11.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell11);

                PdfPCell cell13 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell13.Colspan = 1;
                cell13.BorderWidthLeft = 0f;
                cell13.BorderWidthRight = .8f;
                cell13.BorderWidthTop = 0f;
                cell13.BorderWidthBottom = 0f;

                cell13.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell13);



                PdfPCell cell14 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell14.Colspan = 1;
                cell14.BorderWidthLeft = 0f;
                cell14.BorderWidthRight = .8f;
                cell14.BorderWidthTop = 0f;
                cell14.BorderWidthBottom = 0f;

                cell14.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell14);

                decimal OEC;
                OEC = Math.Round(OBED * 2 / 100, 2);

                PdfPCell cell15 = new PdfPCell(new Phrase("2%", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell15.Colspan = 1;
                cell15.BorderWidthLeft = 0f;
                cell15.BorderWidthRight = .8f;
                cell15.BorderWidthTop = 0f;
                cell15.BorderWidthBottom = 0f;

                cell15.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell15);



                PdfPCell cell125954 = new PdfPCell(new Phrase(OEC.ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell125954.Colspan = 1;
                cell125954.BorderWidthLeft = 0f;
                cell125954.BorderWidthRight = 1f;
                cell125954.BorderWidthTop = 0f;
                cell125954.BorderWidthBottom = 0f;

                cell125954.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell125954);

                PdfPCell cell19 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell19.Colspan = 1;
                cell19.BorderWidthLeft = 1f;
                cell19.BorderWidthRight = 0f;
                cell19.BorderWidthTop = 0f;
                cell19.BorderWidthBottom = 0f;

                cell19.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell19);



                PdfPCell cell20 = new PdfPCell(new Phrase("Output She Cess", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell20.Colspan = 2;
                cell20.BorderWidthLeft = .8f;
                cell20.BorderWidthRight = .8f;
                cell20.BorderWidthTop = 0f;
                cell20.BorderWidthBottom = 0f;

                cell20.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell20);

                PdfPCell cell21 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell21.Colspan = 1;
                cell21.BorderWidthLeft = 0f;
                cell21.BorderWidthRight = .8f;
                cell21.BorderWidthTop = 0f;
                cell21.BorderWidthBottom = 0f;

                cell21.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell21);



                PdfPCell cell22 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell22.Colspan = 1;
                cell22.BorderWidthLeft = 0f;
                cell22.BorderWidthRight = .8f;
                cell22.BorderWidthTop = 0f;
                cell22.BorderWidthBottom = 0f;

                cell22.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell22);


                PdfPCell cell23 = new PdfPCell(new Phrase("1%", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell23.Colspan = 1;
                cell23.BorderWidthLeft = 0f;
                cell23.BorderWidthRight = .8f;
                cell23.BorderWidthTop = 0f;
                cell23.BorderWidthBottom = 0f;

                cell23.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell23);

                decimal OSC;
                OSC = Math.Round(OBED * 1 / 100, 2);

                PdfPCell cell24 = new PdfPCell(new Phrase(OSC.ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell24.Colspan = 1;
                cell24.BorderWidthLeft = 0f;
                cell24.BorderWidthRight = 1f;
                cell24.BorderWidthTop = 0f;
                cell24.BorderWidthBottom = 0f;

                cell24.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell24);



                PdfPCell cell236 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell236.Colspan = 1;
                cell236.BorderWidthLeft = 1f;
                cell236.BorderWidthRight = 0f;
                cell236.BorderWidthTop = 0f;
                cell236.BorderWidthBottom = 0f;

                cell236.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell236);



                PdfPCell cell235 = new PdfPCell(new Phrase("Output Vat  @ "+vat, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell235.Colspan = 2;
                cell235.BorderWidthLeft = .8f;
                cell235.BorderWidthRight = .8f;
                cell235.BorderWidthTop = 0f;
                cell235.BorderWidthBottom = 0f;

                cell235.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell235);

                PdfPCell cell234 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell234.Colspan = 1;
                cell234.BorderWidthLeft = 0f;
                cell234.BorderWidthRight = .8f;
                cell234.BorderWidthTop = 0f;
                cell234.BorderWidthBottom = 0f;

                cell234.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell234);


                PdfPCell cell233 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell233.Colspan = 1;
                cell233.BorderWidthLeft = 0f;
                cell233.BorderWidthRight = .8f;
                cell233.BorderWidthTop = 0f;
                cell233.BorderWidthBottom = 0f;

                cell233.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell233);


                PdfPCell cell231 = new PdfPCell(new Phrase("13.125%", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell231.Colspan = 1;
                cell231.BorderWidthLeft = 0f;
                cell231.BorderWidthRight = .8f;
                cell231.BorderWidthTop = 0f;
                cell231.BorderWidthBottom = 0f;

                cell231.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell231);

                double VO;
                decimal alltotal = to + OBED + OSC + OEC;
                VO = Math.Round(double.Parse(alltotal.ToString()) * 13.125 / 100, 2);

                PdfPCell cell232 = new PdfPCell(new Phrase(VO.ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell232.Colspan = 1;
                cell232.BorderWidthLeft = 0f;
                cell232.BorderWidthRight = 1f;
                cell232.BorderWidthTop = 0f;
                cell232.BorderWidthBottom = 0f;

                cell232.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell232);


                PdfPCell cell237 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell237.Colspan = 1;
                cell237.FixedHeight = 200f;
                cell237.BorderWidthLeft = 1f;
                cell237.BorderWidthRight = 0f;
                cell237.BorderWidthTop = 0f;
                cell237.BorderWidthBottom = 0f;

                cell237.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell237);



                PdfPCell cell238 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell238.Colspan = 2;
                cell238.BorderWidthLeft = .8f;
                cell238.BorderWidthRight = .8f;
                cell238.BorderWidthTop = 0f;
                cell238.BorderWidthBottom = 0f;

                cell238.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell238);

                PdfPCell cell239 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell239.Colspan = 1;
                cell239.BorderWidthLeft = 0f;
                cell239.BorderWidthRight = .8f;
                cell239.BorderWidthTop = 0f;
                cell239.BorderWidthBottom = 0f;

                cell239.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell239);

                // int FQt5 = 0;
                //if (dataSetFillHSRPDeliveryChallan.Rows[0]["ISFrontPlateSize"].ToString() != "NULL" || dataSetFillHSRPDeliveryChallan.Rows[0]["ISFrontPlateSize"].ToString() != " ")
                //{

                //    if (dataSetFillHSRPDeliveryChallan.Rows[0]["ISFrontPlateSize"].ToString() == "Y")
                //    {
                //        FQt = 1;
                //    }
                //    else
                //    {
                //        FQt = 0;
                //    }
                //}

                PdfPCell cell240 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell240.Colspan = 1;
                cell240.BorderWidthLeft = 0f;
                cell240.BorderWidthRight = .8f;
                cell240.BorderWidthTop = 0f;
                cell240.BorderWidthBottom = 0f;

                cell240.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell240);

                //decimal amount4 = Convert.ToDecimal(dataSetFillHSRPDeliveryChallan.Rows[0]["TotalAmount"]);
                //decimal amount5 = Math.Round(amount4, 2);
                // decimal amount9 = 0;
                PdfPCell cell241 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell241.Colspan = 1;
                cell241.BorderWidthLeft = 0f;
                cell241.BorderWidthRight = .8f;
                cell241.BorderWidthTop = 0f;
                cell241.BorderWidthBottom = 0f;

                cell241.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell241);



                PdfPCell cell242 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell242.Colspan = 1;
                cell242.BorderWidthLeft = 0f;
                cell242.BorderWidthRight = 1f;
                cell242.BorderWidthTop = 0f;
                cell242.BorderWidthBottom = 0f;

                cell242.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell242);








                PdfPCell cell1328 = new PdfPCell(new Phrase("Total", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell1328.Colspan = 4;
                cell1328.BorderWidthLeft = 1f;
                cell1328.BorderWidthRight = .8f;
                cell1328.BorderWidthTop = .8f;
                cell1328.BorderWidthBottom = 0f;

                cell1328.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1328);


                //Transaction.InvoiceTransaction.NumToWord numtowords = new Transaction.InvoiceTransaction.NumToWord();
                //string totalinwords = numtowords.changeNumericToWords(sum);


                PdfPCell cell1330 = new PdfPCell(new Phrase(""+QtyTo.ToString()+" Set", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell1330.Colspan = 1;
                cell1330.BorderWidthLeft = 0f;
                cell1330.BorderWidthRight = .8f;
                cell1330.BorderWidthTop = .8f;
                cell1330.BorderWidthBottom = 0f;

                cell1330.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1330);



                PdfPCell cell1331 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell1331.Colspan = 1;
                cell1331.BorderWidthLeft = 0f;
                cell1331.BorderWidthRight = .8f;
                cell1331.BorderWidthTop = .8f;
                cell1331.BorderWidthBottom = 0f;

                cell1331.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1331);

                decimal grantamount = to + OBED + OEC + OSC + decimal.Parse(VO.ToString());
                decimal grantamount1 = System.Decimal.Round(grantamount, 2);

                PdfPCell cell1332 = new PdfPCell(new Phrase("" + grantamount1.ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell1332.Colspan = 1;
                cell1332.BorderWidthLeft = 0f;
                cell1332.BorderWidthRight = 1f;
                cell1332.BorderWidthTop = .8f;
                cell1332.BorderWidthBottom = 0f;

                cell1332.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1332);





                PdfPCell cell1338 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell1338.Colspan = 4;
                cell1338.Rowspan = 5;
                cell1338.BorderWidthLeft = 1f;
                cell1338.BorderWidthRight = .8f;
                cell1338.BorderWidthTop = .8f;
                cell1338.BorderWidthBottom = 0f;

                cell1338.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1338);

                //PdfPCell cell1339 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                //cell1339.Colspan = 1;
                //cell1339.BorderWidthLeft = 0f;
                //cell1339.BorderWidthRight = .8f;
                //cell1339.BorderWidthTop = .8f;
                //cell1339.BorderWidthBottom = 0f;

                //cell1339.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                //table.AddCell(cell1339);

                //PdfPCell cell1340 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                //cell1340.Colspan = 1;
                //cell1340.BorderWidthLeft = 0f;
                //cell1340.BorderWidthRight = .8f;
                //cell1340.BorderWidthTop = .8f;
                //cell1340.BorderWidthBottom = 0f;

                //cell1340.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                //table.AddCell(cell1340);

                //PdfPCell cell1341 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                //cell1341.Colspan = 1;
                //cell1341.BorderWidthLeft = 0f;
                //cell1341.BorderWidthRight = .8f;
                //cell1341.BorderWidthTop = .8f;
                //cell1341.BorderWidthBottom = 0f;

                //cell1341.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                //table.AddCell(cell1341);

                PdfPCell cell1342 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell1342.Colspan = 2;
                cell1342.Rowspan = 5;
                cell1342.BorderWidthLeft = 0f;
                cell1342.BorderWidthRight = .8f;
                cell1342.BorderWidthTop = .8f;
                cell1342.BorderWidthBottom = 0f;

                cell1342.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1342);


                // decimal totalAmount1 = System.Decimal.Round(TotAmt, 0);
                PdfPCell cell1344 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell1344.Colspan = 1;
                cell1344.Rowspan = 5;
                cell1344.BorderWidthLeft = 0f;
                cell1344.BorderWidthRight = 1f;
                cell1344.BorderWidthTop = .8f;
                cell1344.BorderWidthBottom = 0f;

                cell1344.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1344);

                // HSRP.Dealer.Transaction


                HSRP.Transaction.InvoiceTransaction.NumToWord trans = new HSRP.Transaction.InvoiceTransaction.NumToWord();
                double grantamount2 = double.Parse(grantamount1.ToString());
                string totalinwords = trans.changeNumericToWords(grantamount2);


                PdfPCell cell1348 = new PdfPCell(new Phrase("Amount chargeable (in words) : " + totalinwords + " Paise Only", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell1348.Colspan = 7;
                cell1348.BorderWidthLeft = 1f;
                cell1348.BorderWidthRight = .8f;
                cell1348.BorderWidthTop = .8f;
                cell1348.BorderWidthBottom = 0f;

                cell1348.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1348);



                double Duty = double.Parse(OBED.ToString());
                string Duty1 = trans.changeNumericToWords(Duty);

                PdfPCell cell13455 = new PdfPCell(new Phrase("Amount of Duty (in words) : " + Duty1 + " Paise Only", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell13455.Colspan = 7;
                cell13455.BorderWidthLeft = 1f;
                cell13455.BorderWidthRight = .8f;
                cell13455.BorderWidthTop = .8f;
                cell13455.BorderWidthBottom = 0f;

                cell13455.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell13455);


                decimal Cess = OEC +OSC;
                string Cess1 = trans.changeNumericToWords(double.Parse(Cess.ToString()));

                PdfPCell cell13481 = new PdfPCell(new Phrase("Amount of Cess (in words) : " + Cess1 + " Paise Only", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell13481.Colspan = 7;
                cell13481.BorderWidthLeft = 1f;
                cell13481.BorderWidthRight = .8f;
                cell13481.BorderWidthTop = .8f;
                cell13481.BorderWidthBottom = 0f;

                cell13481.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell13481);



                PdfPCell cell1353 = new PdfPCell(new Phrase("Serial no . in PLA/RG-23 :", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell1353.Colspan = 4;
                cell1353.BorderWidthLeft = 1f;
                cell1353.BorderWidthRight = .8f;
                cell1353.BorderWidthTop = .8f;
                cell1353.BorderWidthBottom = 0f;

                cell1353.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1353);



                PdfPCell cell1358 = new PdfPCell(new Phrase("Link Utsav Registration Plates Pvt. Ltd.", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell1358.Colspan = 3;
                cell1358.BorderWidthLeft = 0f;
                cell1358.BorderWidthRight = 1f;
                cell1358.BorderWidthTop = .8f;
                cell1358.BorderWidthBottom = 0f;

                cell1358.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1358);





                PdfPCell cell1356 = new PdfPCell(new Phrase("Declaration", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell1356.Colspan = 4;
                cell1356.BorderWidthLeft = 1f;
                cell1356.BorderWidthRight = .8f;
                cell1356.BorderWidthTop = 0f;
                cell1356.BorderWidthBottom = 0f;

                cell1356.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1356);


                PdfPCell cell1361 = new PdfPCell(new Phrase("(AUTH. SIGN.)", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                //cell1361.PaddingRight = 100f;
                cell1361.Colspan = 3;
                cell1361.PaddingTop = 20f;
                cell1361.PaddingRight = 0f;
                cell1361.BorderWidthLeft = 0f;
                cell1361.BorderWidthRight = 1f;
                cell1361.BorderWidthTop = 0f;
                cell1361.BorderWidthBottom = 0f;

                cell1361.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1361);



                PdfPCell cell1359 = new PdfPCell(new Phrase("We declare that this invoice shows the actual price of the goods described and that all particulars are true and correct ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell1359.Colspan = 7;
                cell1359.BorderWidthLeft = 1f;
                cell1359.BorderWidthRight = .8f;
                cell1359.BorderWidthTop = .8f;
                cell1359.BorderWidthBottom = 0f;

                cell1359.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1359);





                PdfPCell cell1362 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell1362.Colspan = 7;
                cell1362.BorderWidthLeft = 1f;
                cell1362.BorderWidthRight = .8f;
                cell1362.BorderWidthTop = .8f;
                cell1362.BorderWidthBottom = 1f;

                cell1362.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1362);





                PdfPCell cell1366 = new PdfPCell(new Phrase("CUSTOMER'S NAME : ", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell1366.Colspan = 7;
                cell1366.PaddingRight = 60f;
                cell1366.BorderWidthLeft = 0f;
                cell1366.BorderWidthRight = .8f;
                cell1366.BorderWidthTop = 0f;
                cell1366.BorderWidthBottom = 1f;
                cell1366.BorderColor = BaseColor.WHITE;
                cell1366.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1366);

                PdfPCell cell1367 = new PdfPCell(new Phrase("(SIGN)", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell1367.Colspan = 7;
                cell1367.PaddingRight = 100f;
                cell1367.PaddingTop = 5f;
                cell1367.BorderWidthLeft = 0f;
                cell1367.BorderWidthRight = .8f;
                cell1367.BorderWidthTop = .8f;
                cell1367.BorderWidthBottom = 1f;
                cell1367.BorderColor = BaseColor.WHITE;
                cell1367.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1367);




                //PdfPCell cell1369 = new PdfPCell(new Phrase("VEHICLE No : " + " Date :", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                //cell1369.Colspan = 7;
                //cell1369.PaddingRight = 70f;
                //cell1369.BorderWidthLeft = 0f;
                //cell1369.BorderWidthRight = 1f;
                //cell1369.BorderWidthBottom = 1f;
                //cell1369.BorderColor = BaseColor.WHITE;
                //cell1369.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                //table.AddCell(cell1369);



                document.Add(table1);
                document.Add(table);
                //document.Add(table2);
                //document.Add(table);
                //document.Add(table1);

                document.Close();
                HttpContext context = HttpContext.Current;

                context.Response.ContentType = "Application/pdf";
                context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                context.Response.WriteFile(PdfFolder);
                context.Response.End();

                //ReadFile(PdfFolder);

            }
            //}
            //else
            //{
            //    string script = "<script type=\"text/javascript\">  alert('Embossing is not done');</script>";
            //    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", script);

            //}
            //LinkButton lnkInvc = (LinkButton)Grid1.FindControl("LinkButtonStatus");

        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
               // GridView1.DataBind();
            show();
        }
    }
}