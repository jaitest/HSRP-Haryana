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
using System;
using System.Collections;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;



namespace HSRP.EmbossingData
{
    public partial class ReprintProdSheet : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        string UserType = string.Empty;
        string HSRPStateID = string.Empty;
        string RTOLocationID = string.Empty;
        int intHSRPStateID;
        string SQlQuery = string.Empty;
        string ExicseAmount = string.Empty;
        string AllLocation = string.Empty;
        string OrderStatus = string.Empty;
        string strStateId = string.Empty;
       
       
        string StrSqlQuery = string.Empty;
        string strRunningNo = string.Empty;
        string strEmbId=string.Empty;
       
      
        DataProvider.BAL bl = new DataProvider.BAL();
        BAL obj = new BAL();
        string StickerManditory = string.Empty;
        DataTable dataSetFillHSRPRecord;

       

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
                            FilldropDownListClient();
                            FilldropDownListOrganization();
                            //buildGrid();
                        }

                        //ShowGrid();
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
            if (UserType == "0")
            {

                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);

                //SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + HSRPStateID + " and ActiveStatus!='N'   Order by RTOLocationName";
                SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + intHSRPStateID + " and ActiveStatus!='N'   Order by RTOLocationName";
                Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), CnnString, "--Select RTO Name--");

                dataLabellbl.Visible = false;
                TRRTOHide.Visible = false;
            }
            else
            {
                string UserID = Convert.ToString(Session["UID"]);

                SQLString = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, (a.RTOLocationCode+', ') as RTOCode, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID and a.activestatus='Y' where UserRTOLocationMapping.UserID='" + UserID + "' order by a.rtolocationname ";
                DataTable dss = Utils.GetDataTable(SQLString, CnnString);

                labelOrganization.Visible = true;
                DropDownListStateName.Visible = true;
                labelClient.Visible = true;
                dropDownListClient.Visible = true;

                dropDownListClient.DataSource = dss;
                dropDownListClient.DataBind();
                dataLabellbl.Visible = true;

                string RTOCode = string.Empty;
                if (dss.Rows.Count > 0)
                {
                    for (int i = 0; i <= dss.Rows.Count - 1; i++)
                    {
                        RTOCode += dss.Rows[i]["RTOCode"].ToString();

                    }
                    lblRTOCode.Text = RTOCode.Remove(RTOCode.LastIndexOf(","));

                }
            }
        }

        #endregion

        DataTable dtInvoiceData = null;
        StringBuilder sb = new StringBuilder();
        CheckBox chk;
        DataTable dt = new DataTable();
        string strInvoiceNo = null;
        private void GetRecords()
        {
            strInvoiceNo = string.Empty;
            dtInvoiceData = new DataTable();
            if (!string.IsNullOrEmpty(txtInvoiceNo.Text))
            {
                string SQLString = "select row_number() over(order by VehicleClass,VehicleType,VehicleRegNo) as SN,ChallanNo,challandate,convert(varchar,HSRPRecord_CreationDate,103) as HSRPRecord_CreationDate,VehicleType,convert(varchar,OrderEmbossingDate,103) as OrderEmbossingDate,hsrp_stateid,rtolocationid" +
                                    ",hsrprecordid,vehicleregno,hsrp_rear_lasercode,hsrp_Front_lasercode,OrderStatus " +
                                    "from hsrprecords where hsrp_stateid='" + DropDownListStateName.SelectedValue.ToString() + "' and [ChallanNo]='" + txtInvoiceNo.Text + "' order by VehicleClass,VehicleType,VehicleRegNo";
                
                dtInvoiceData = Utils.GetDataTable(SQLString, CnnString);
                strInvoiceNo = txtInvoiceNo.Text;
            }
            else if (!dropdownDuplicateFIle.SelectedItem.Text.Equals("--Select PS No--"))
            {
                string SQLString = "select row_number() over(order by VehicleClass,VehicleType,VehicleRegNo) AS SN,newpdfrunningno,orderdate,convert(varchar,HSRPRecord_CreationDate,103) as HSRPRecord_CreationDate,VehicleType,convert(varchar,OrderEmbossingDate,103) as OrderEmbossingDate,hsrp_stateid,rtolocationid" +
                                    ",hsrprecordid,vehicleregno,hsrp_rear_lasercode,hsrp_Front_lasercode,OrderStatus " +
                                    "from hsrprecords where [newpdfrunningno]='" + dropdownDuplicateFIle.SelectedItem + "' order by VehicleClass,VehicleType,VehicleRegNo";
                dtInvoiceData = Utils.GetDataTable(SQLString, CnnString);
                strInvoiceNo = dropdownDuplicateFIle.SelectedItem.Text;
            }
            else
            {
                return;
            }

           
                if (dtInvoiceData.Rows.Count > 0)
                {
                   
                    btnChalan.Visible = true;
                   
                  //  Button2.Visible = true;
                   
                }
                else
                {
                    btnChalan.Visible = false;
                   
                  //  Button2.Visible = false;
                    lblErrMsg.Text = "Record Not Found";
                    

                }

           
        }


       
        protected void DropDownListStateName_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilldropDownListClient();
            dropDownListClient.Visible = true;
            labelClient.Visible = true;

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

        protected void btnGO_Click(object sender, EventArgs e)
        {
            txtInvoiceNo.Text = string.Empty;
            try
            {                
                    labelOrderStatus.Visible = true;
                    dropdownDuplicateFIle.Visible = true;

                    string type = "1";

                    string strDate = OrderDate.SelectedDate.ToString("dd/MM/yyyy");
                    String[] StringAuthDate = strDate.Replace("-", "/").Split('/');
                    string MonTo = ("0" + StringAuthDate[0]);
                    string MonthdateTO = MonTo.Replace("00", "0").Replace("01", "1");
                    String ReportDateFrom = StringAuthDate[1] + "/" + MonthdateTO + "/" + StringAuthDate[2].Split(' ')[0];
                    String From = StringAuthDate[0] + "/" + StringAuthDate[1] + "/" + StringAuthDate[2].Split(' ')[0];
                    //AuthorizationDate = new DateTime(Convert.ToInt32(StringAuthDate[2].Split(' ')[0]), Convert.ToInt32(StringAuthDate[0]), Convert.ToInt32(StringAuthDate[1]));
                    string AuthorizationDate = From + " 00:00:00";
                    String[] StringOrderDate = HSRPAuthDate.SelectedDate.ToString().Replace("-", "/").Split('/');
                    string Mon = ("0" + StringOrderDate[0]);
                    string Monthdate = Mon.Replace("00", "0").Replace("01", "1");
                    String FromDate = StringOrderDate[1] + "-" + Monthdate + "-" + StringOrderDate[2].Split(' ')[0];
                    String ReportDateTo = StringOrderDate[1] + "/" + Monthdate + "/" + StringOrderDate[2].Split(' ')[0];
                    String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
                    string ToDate = From1 + " 23:59:59";


                    string strFrmDateString = OrderDate.SelectedDate.ToShortDateString() + " 00:00:00";
                    string strToDateString = HSRPAuthDate.SelectedDate.ToShortDateString() + " 23:59:59";

                    SQLString = "select  Distinct newpdfrunningno   from hsrprecords  where hsrp_StateID='" + DropDownListStateName.SelectedValue +
                        "' and rtolocationID='" + dropDownListClient.SelectedValue + "' and  newpdfrunningno is not null and erpassigndate between '" +
                        strFrmDateString + "' and  '" + strToDateString + "' ";


                
                labelOrderStatus.Visible = true;
                dropdownDuplicateFIle.Visible = true;
                    Utils.PopulateDropDownList(dropdownDuplicateFIle, SQLString.ToString(), CnnString, "--Select PS No--");
               
                
            }
            catch (Exception ex)
            {
                lblErrMsg.Text = ex.Message;
            }

        }
        
        protected void dropdownDuplicateFIle_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGrid();
        }

        private void BindGrid()
        {
            GetRecords();
            if (dtInvoiceData.Rows.Count > 0)
            {
                gvRecords.DataSource = dtInvoiceData;
                gvRecords.DataBind();
                btnChalan.Visible = true;
                Button1.Visible = false;
            }
            else
            {
                btnChalan.Visible = false;
                Button1.Visible = false;
                gvRecords.DataSource = null;
                gvRecords.DataBind();
            }
        }


        protected void btnChalan_Click(object sender, EventArgs e)
        {
            try
            {
                #region Validation
             //   Button2.Visible = true;
                if (string.IsNullOrEmpty(txtInvoiceNo.Text) && (dropdownDuplicateFIle.SelectedIndex<=0))
                {
                    Response.Write("<script> alert('Please Enter PS No.')</script>");
                    return;
                }
               
                #endregion

                 string strStateID = DropDownListStateName.SelectedValue.ToString();
            string RTOLocationID = dropDownListClient.SelectedValue.ToString();
            
            if (strStateID == "--Select State--")
            {
                lblErrMsg.Visible = true;
                lblErrMsg.Text = "";
                lblErrMsg.Text = "Please Select State.";
                return;
            }
            if (RTOLocationID == "--Select RTO Name--")
            {
                lblErrMsg.Visible = true;
                lblErrMsg.Text = "";
                lblErrMsg.Text = "Please Select RTO Location.";
                return;
            }
            lblErrMsg.Text = "";
            
            BAL obj = new BAL();
         
            String[] StringAuthDate = OrderDate.SelectedDate.ToString().Split('/');
            string MonTo = ("0" + StringAuthDate[0]);
            string MonthdateTO = MonTo.Replace("00", "0").Replace("01", "1");
            String ReportDateFrom = StringAuthDate[1] + "/" + MonthdateTO + "/" + StringAuthDate[2].Split(' ')[0];
            String From = StringAuthDate[0] + "/" + StringAuthDate[1] + "/" + StringAuthDate[2].Split(' ')[0];

            string AuthorizationDate = From + " 00:00:00";
            String[] StringOrderDate = HSRPAuthDate.SelectedDate.ToString().Split('/');
            string Mon = ("0" + StringOrderDate[0]);
            string Monthdate = Mon.Replace("00", "0").Replace("01", "1");
            String FromDate = StringOrderDate[1] + "-" + Monthdate + "-" + StringOrderDate[2].Split(' ')[0];
            String ReportDateTo = StringOrderDate[1] + "/" + Monthdate + "/" + StringOrderDate[2].Split(' ')[0];
            String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
           // string From1 = "11/28/14";
            string ToDate = From1 + " 23:59:59";

          
            string from2 = From1 + " 00:00:00";
            string from3 = From + " 00:00:00";
            
            string strVehicleType = string.Empty;
            StringBuilder strSQL = new StringBuilder();
            
            StrSqlQuery = "select NAVEMBID from rtolocation where hsrp_stateid='" + strStateID + "' and RtolocationId='" + RTOLocationID + "'";
            strEmbId = Utils.getScalarValue(StrSqlQuery, CnnString);
            



            string strSel = "select isnull(max(right(ProductionSheetRunningNo,7)),0000000) from EmbossingCenters where State_Id='" + DropDownListStateName.SelectedValue.ToString() + "' and Emb_Center_Id='" + strEmbId + "'";
            string strCom = Utils.getScalarValue(strSel, CnnString);

            string strPRFIX = "select PrefixText from EmbossingCenters where State_Id='" + DropDownListStateName.SelectedValue.ToString() + "' and Emb_Center_Id='" + strEmbId + "'";
            string strPRFIXCom = Utils.getScalarValue(strPRFIX, CnnString);
            

            if (strCom.Equals(0))
            {
                strRunningNo = "0000001";
            }
            else
            {
                strRunningNo = string.Format("{0:0000000}", Convert.ToInt32(strCom) + 1);
            }


          

          


            strSQL.Append("Select a.hsrprecordID,a.roundoff_netamount,a.OrderStatus, a.HSRPRecord_AuthorizationNo,convert(varchar, OrderClosedDate, 105) as OrderClosedDate, CONVERT(varchar(20),orderdate ,103) AS OrderBookDate,CONVERT(varchar(20),OrderEmbossingDate ,103) AS OrderEmbossingDate, a.dealerid as ID, left(a.OwnerName,19) as OwnerName, a.MobileNo,(select  AffixCenterDesc from AffixationCenters where Affix_id= a.affix_id ) as AffixCenterDesc, CONVERT(varchar(20), HSRPRecord_AuthorizationDate,103) AS OrderDateAuth, a.OrderDate, a.EngineNo, a.ChassisNo, (select rtolocationname from rtolocation where  rtolocationid =a.rtolocationid) as RTOLocationName,  a.VehicleRegNo,  case a.VehicleType when 'MCV/HCV/TRAILERS' then 'Trailers' when 'THREE WHEELER' then 'T.Whe.' when 'SCOOTER' then 'SCOO' when 'TRACTOR' then 'TRAC' when 'LMV(CLASS)' then 'L.CL' when 'LMV' then 'LMV'  when 'MOTOR CYCLE' then 'MO.C' end as VehicleType, case a.VehicleClass when 'Transport' then 'T' else 'N.T.' end as VehicleClass,  a.HSRP_StateID, (select HSRPStateName from hsrpstate where HSRP_StateID=a.HSRP_StateID)  as HSRPStateName, (select replace(ProductCode,'MM-','') from Product where productid= a.RearPlateSize) AS RearProductCode,(select replace(ProductCode,'MM-','') from Product where productid= a.FrontPlateSize) AS FrontProductCode,a.FrontPlateSize, a.HSRP_Front_LaserCode, a.HSRP_Rear_LaserCode, a.RearPlateSize FROM HSRPRecords AS a   where  isnull(newpdfrunningno,'')='"+dropdownDuplicateFIle.SelectedItem+"'   ");
            
                 
                 
                  

                    if (strVehicleType == "")
                    {
                        strSQL.Append(" order by a.VehicleClass,a.VehicleType,a.HSRP_Front_LaserCode,a.hsrp_rear_lasercode");
                    }
                    else
                    {
                        strSQL.Append( strVehicleType+ " order by a.VehicleClass,a.VehicleType,a.HSRP_Front_LaserCode,a.hsrp_rear_lasercode");
                    }
                    

                    DataTable dt = Utils.GetDataTable(strSQL.ToString(), CnnString);
                if (dt.Rows.Count > 0)
                {
                    float Amount = 0;
                    string filename = "HSRPProductionSheet- " + RTOLocationID + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".pdf";
                    String StringField = String.Empty;
                    String StringAlert = String.Empty;
                    StringBuilder bb = new StringBuilder();
                    // Document document = new Document();
                    //  iTextSharp.text.Document document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 0, 0, 0, 0);
                    Document document = new Document(new iTextSharp.text.Rectangle(188f, 124f), -30, -30, 8, 0);
                    document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());


                    BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
                    // iTextSharp.text.Font times = new Font(bfTimes, 12, Font.ITALIC, Color.Black);
                    // Creates a Writer that listens to this document and writes the document to the Stream of your choice:
                    string PdfFolder = ConfigurationManager.AppSettings["PdfFolder"].ToString() + filename;
                    PdfWriter.GetInstance(document, new FileStream(PdfFolder, FileMode.Create));
                    //Opens the document:
                        document.Open();

                        PdfPTable table;
                   
                        table = new PdfPTable(14);
                        var colWidthPercentages = new[] { 55f, 125f, 55f, 200f, 75f, 165f, 165f, 155f, 110f, 210f, 105f, 210f, 105f, 125f };//7,14
                        var colheightpercentage = new[] { 2f };

                        table.SetWidths(colWidthPercentages);
                       
                 
                        string strQueryDate = "SELECT CONVERT(VARCHAR,GETDATE(),103)";
                        DataTable dtDate = Utils.GetDataTable(strQueryDate, CnnString);

                        table.TotalWidth = 800f;
                        table.LockedWidth = true;



                        PdfPCell cell786a = new PdfPCell(new Phrase("Production Sheet No:  " + dropdownDuplicateFIle.SelectedItem, new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                        cell786a.Colspan = 14;
                        cell786a.BorderWidthLeft = 0f;
                        cell786a.BorderWidthRight = 0f;
                        cell786a.BorderWidthTop = 0f;
                        cell786a.BorderWidthBottom = 0f;
                        //cell120911.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell786a.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell786a);
                        
                    

                        PdfPCell cell1209111 = new PdfPCell(new Phrase("Report Generation Date: " + dtDate.Rows[0][0].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                        cell1209111.Colspan = 6;
                        cell1209111.BorderWidthLeft = 0f;
                        cell1209111.BorderWidthRight = 0f;
                        cell1209111.BorderWidthTop = 0f;
                        cell1209111.BorderWidthBottom = 0f;
                        //cell120911.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell1209111.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1209111);

                        PdfPCell cell120911 = new PdfPCell(new Phrase("Production Sheet", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                        cell120911.Colspan = 10;
                        cell120911.BorderWidthLeft = 0f;
                        cell120911.BorderWidthRight = 0f;
                        cell120911.BorderWidthTop = 0f;
                        cell120911.BorderWidthBottom = 0f;
                        //cell120911.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell120911.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell120911);

                        PdfPCell cell12091 = new PdfPCell(new Phrase("State Name : " + dt.Rows[0]["HSRPStateName"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell12091.Colspan = 6;
                        cell12091.BorderWidthLeft = 1f;
                        cell12091.BorderWidthRight = 0f;
                        cell12091.BorderWidthTop = 1f;
                        cell12091.BorderWidthBottom = 0f;
                        cell12091.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell12091);

                        PdfPCell cell12093 = new PdfPCell(new Phrase("Report Date From : " + ReportDateFrom, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell12093.Colspan = 4;
                        cell12093.BorderWidthLeft = 0f;
                        cell12093.BorderWidthRight = 0f;
                        cell12093.BorderWidthTop = 1f;
                        cell12093.BorderWidthBottom = 0f;

                        cell12093.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell12093);


                        PdfPCell cell120931 = new PdfPCell(new Phrase("ORD:Order Open Date ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell120931.Colspan = 6;
                        cell120931.BorderWidthLeft = 0f;
                        cell120931.BorderWidthRight = 1f;
                        cell120931.BorderWidthTop = 1f;
                        cell120931.BorderWidthBottom = 0f;

                        cell120931.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell120931);


                        PdfPCell cell120913 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell120913.Colspan = 6;
                        cell120913.BorderWidthLeft = 1f;
                        cell120913.BorderWidthRight = 0f;
                        cell120913.BorderWidthTop = 0f;//1
                        cell120913.BorderWidthBottom = 0f;
                        cell120913.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell120913);

                        PdfPCell cell12095 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell12095.Colspan = 4;//7
                        cell12095.BorderWidthLeft = 0f;
                        cell12095.BorderWidthRight = 0f;//1
                        cell12095.BorderWidthTop = 0f;
                        cell12095.BorderWidthBottom = 0f;

                        cell12095.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell12095);


                        PdfPCell cell1209318 = new PdfPCell(new Phrase("VC:Vehicle Class ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1209318.Colspan = 6;
                        cell1209318.BorderWidthLeft = 0f;
                        cell1209318.BorderWidthRight = 1f;
                        cell1209318.BorderWidthTop = 0f;//1
                        cell1209318.BorderWidthBottom = 0f;

                        cell1209318.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1209318);

                        PdfPCell cell1209139 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1209139.Colspan = 6;
                        cell1209139.BorderWidthLeft = 1f;//0
                        cell1209139.BorderWidthRight = 0f;
                        cell1209139.BorderWidthTop = 0f;//1
                        cell1209139.BorderWidthBottom = 0f;
                        cell1209139.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1209139);
                        PdfPCell cell120959 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell120959.Colspan = 4;//7
                        cell120959.BorderWidthLeft = 0f;
                        cell120959.BorderWidthRight = 0f;//1
                        cell120959.BorderWidthTop = 0f;
                        cell120959.BorderWidthBottom = 0f;

                        cell120959.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell120959);


                        PdfPCell cell1209317 = new PdfPCell(new Phrase("VT:Vehicle Type ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1209317.Colspan = 6;
                        cell1209317.BorderWidthLeft = 0f;
                        cell1209317.BorderWidthRight = 1f;
                        cell1209317.BorderWidthTop = 0f;//1
                        cell1209317.BorderWidthBottom = 0f;

                        cell1209317.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1209317);


                        

                        PdfPCell cell12092 = new PdfPCell(new Phrase("RTOLocation Name : " + dt.Rows[0]["RTOLocationName"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell12092.Colspan = 6;//7
                        cell12092.BorderWidthLeft = 1f;
                        cell12092.BorderWidthRight = 0f;
                        cell12092.BorderWidthTop = 0f;
                        cell12092.BorderWidthBottom = 0f;

                        cell12092.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell12092);
                        PdfPCell cell12094 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell12094.Colspan = 4;//3
                        cell12094.BorderWidthLeft = 0f;
                        cell12094.BorderWidthRight = 0f;
                        cell12094.BorderWidthTop = 0f;
                        cell12094.BorderWidthBottom = 0f;

                        cell12094.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell12094);

                        PdfPCell cell120952 = new PdfPCell(new Phrase("OS: Order Satus(New Order/Embossing Done/Closed)", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell120952.Colspan = 9;//7
                        cell120952.BorderWidthLeft = 0f;
                        cell120952.BorderWidthRight = 1f;
                        cell120952.BorderWidthTop = 0f;
                        cell120952.BorderWidthBottom = 0f;

                        cell120952.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell120952);
                    
                    PdfPCell cell12 = new PdfPCell(new Phrase("SR.No", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    cell12.Colspan = 1;
                    cell12.BorderWidthLeft = 1f;                     
                    cell12.BorderWidthRight = .8f;
                    cell12.BorderWidthTop = 0.8f;
                    cell12.BorderWidthBottom = 0.8f;
                    cell12.FixedHeight = -1;
                    cell12.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell12);

                    PdfPCell cell1210 = new PdfPCell(new Phrase("ORD", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    cell1210.Colspan = 1;
                    cell1210.BorderWidthLeft = 0f;
                    cell1210.BorderWidthRight = .8f;
                    cell1210.BorderWidthTop = 0.8f;
                    cell1210.BorderWidthBottom = 0.8f;

                    cell1210.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell1210);

                    PdfPCell cell1213 = new PdfPCell(new Phrase("VC", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    cell1213.Colspan = 1;
                    cell1213.BorderWidthLeft = 0f;
                    cell1213.BorderWidthRight = .8f;
                    cell1213.BorderWidthTop = 0.8f;
                    cell1213.BorderWidthBottom = 0.8f;

                    cell1213.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell1213);


                    PdfPCell cell1209 = new PdfPCell(new Phrase("Vehicle No", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK))); //6
                    cell1209.Colspan = 1;
                    cell1209.BorderWidthLeft = 0f;
                    cell1209.BorderWidthRight = .8f;
                    cell1209.BorderWidthTop = 0.8f;
                    cell1209.BorderWidthBottom = 0.8f;

                    cell1209.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell1209);

                    PdfPCell cell12233 = new PdfPCell(new Phrase("VT", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    cell12233.Colspan = 1;
                    cell12233.BorderWidthLeft = 0f;
                    cell12233.BorderWidthRight = .8f;
                    cell12233.BorderWidthTop = 0.8f;
                    cell12233.BorderWidthBottom = 0.8f;

                    cell12233.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell12233);

                        PdfPCell cell1206 = new PdfPCell(new Phrase("Chassis No", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                        cell1206.Colspan = 1;
                        cell1206.BorderWidthLeft = 0.8f;
                        cell1206.BorderWidthRight = .8f;
                        cell1206.BorderWidthTop = 0.8f;
                        cell1206.BorderWidthBottom = 0.8f;
                        cell1206.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right

                        table.AddCell(cell1206);

                        PdfPCell cell1221 = new PdfPCell(new Phrase("EngineNo", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                        cell1221.Colspan = 1;
                        cell1221.BorderWidthLeft = 0.8f;
                        cell1221.BorderWidthRight = 0.8f;
                        cell1221.BorderWidthTop = .8f;
                        cell1221.BorderWidthBottom = 0.8f;

                        cell1221.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1221);
                    
              
              
                     if (HSRPStateID == "1" || HSRPStateID == "2" || HSRPStateID == "3" || HSRPStateID == "6" || HSRPStateID == "9" || HSRPStateID == "11")
                    {
                        PdfPCell cell120933 = new PdfPCell(new Phrase("Owner Name", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                        cell120933.Colspan = 1;
                        cell120933.BorderWidthLeft = 0f;
                        cell120933.BorderWidthRight = 0.8f;
                        cell120933.BorderWidthTop = 0.8f;
                        cell120933.BorderWidthBottom = 0f;

                        cell120933.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell120933);
                    }
                    

                    PdfPCell cell120935 = new PdfPCell(new Phrase("Front Plate Size", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK))); //6
                    cell120935.Colspan = 1;
                    cell120935.BorderWidthLeft = 0f;
                    cell120935.BorderWidthRight = 0.8f;
                    cell120935.BorderWidthTop = 0.8f;
                    cell120935.BorderWidthBottom = 0f;

                    cell120935.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell120935);

                    PdfPCell cell120936 = new PdfPCell(new Phrase("Front Laser No.", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK))); //6
                    cell120936.Colspan = 1;
                    cell120936.BorderWidthLeft = 0f;
                    cell120936.BorderWidthRight = 0.8f;
                    cell120936.BorderWidthTop = 0.8f;
                    cell120936.BorderWidthBottom = 0f;

                    cell120936.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell120936);


                    PdfPCell cell120937 = new PdfPCell(new Phrase("Rear Plate Size", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    cell120937.Colspan = 1;
                    cell120937.BorderWidthLeft = 0f;
                    cell120937.BorderWidthRight = 0.8f;
                    cell120937.BorderWidthTop = 0.8f;
                    cell120937.BorderWidthBottom = 0f;

                    cell120937.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell120937);

                    PdfPCell cell120938 = new PdfPCell(new Phrase("Rear Laser No.", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    cell120938.Colspan = 1;
                    cell120938.BorderWidthLeft = 0f;
                    cell120938.BorderWidthRight = 0.8f;
                    cell120938.BorderWidthTop = 0.8f;
                    cell120938.BorderWidthBottom = 0f;

                    cell120938.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell120938);


                    if (strStateID == "1")
                    {

                        PdfPCell cellABC = new PdfPCell(new Phrase("Affixation Center", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                        cellABC.Colspan = 1;
                        cellABC.BorderWidthLeft = 0f;
                        cellABC.BorderWidthRight = 0.8f;
                        cellABC.BorderWidthTop = 0.8f;
                        cellABC.BorderWidthBottom = 0f;

                        cellABC.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cellABC);


                    }
                    else
                     {

                        PdfPCell cell120939 = new PdfPCell(new Phrase("Amount", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                        cell120939.Colspan = 1;
                        cell120939.BorderWidthLeft = 0f;
                        cell120939.BorderWidthRight = 0.8f;
                        cell120939.BorderWidthTop = 0.8f;
                        cell120939.BorderWidthBottom = 0f;

                        cell120939.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell120939);
                      }

                        PdfPCell cell120939a = new PdfPCell(new Phrase("OS", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                        cell120939a.Colspan = 1;
                        cell120939a.BorderWidthLeft = 0f;
                        cell120939a.BorderWidthRight = 0.8f;
                        cell120939a.BorderWidthTop = 0.8f;
                        cell120939a.BorderWidthBottom = 0f;

                        cell120939a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell120939a);
                    
                    int j = 0;
                    int total = 0;
                    for (int i = 0; i <= dt.Rows.Count - 1; i++)
                    {

                        j = j + 1;

                        //=========================================================================================================================================
                        if (total == 44)
                        {
                            total = 0;



                            PdfPCell cell120911a = new PdfPCell(new Phrase("State Name : " + dt.Rows[0]["HSRPStateName"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell120911a.Colspan = 6;
                            cell120911a.BorderWidthLeft = 1f;
                            cell120911a.BorderWidthRight = 0f;
                            cell120911a.BorderWidthTop = 1f;
                            cell120911a.BorderWidthBottom = 0f;
                            cell120911a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell120911a);

                            PdfPCell cell12093a = new PdfPCell(new Phrase("Report Date From : " + ReportDateFrom, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell12093a.Colspan = 4;
                            cell12093a.BorderWidthLeft = 0f;
                            cell12093a.BorderWidthRight = 0f;
                            cell12093a.BorderWidthTop = 1f;
                            cell12093a.BorderWidthBottom = 0f;

                            cell12093a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell12093a);


                            PdfPCell cell1234 = new PdfPCell(new Phrase("ORD:Order Open Date ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1234.Colspan = 6;
                            cell1234.BorderWidthLeft = 0f;
                            cell1234.BorderWidthRight = 1f;
                            cell1234.BorderWidthTop = 1f;
                            cell1234.BorderWidthBottom = 0f;

                            cell1234.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1234);


                            PdfPCell cell12345 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell12345.Colspan = 6;
                            cell12345.BorderWidthLeft = 1f;
                            cell12345.BorderWidthRight = 0f;
                            cell12345.BorderWidthTop = 0f;//1
                            cell12345.BorderWidthBottom = 0f;
                            cell12345.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell12345);

                            PdfPCell cell123456 = new PdfPCell(new Phrase("Report Date To : " + ReportDateTo, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell123456.Colspan = 4;//7
                            cell123456.BorderWidthLeft = 0f;
                            cell123456.BorderWidthRight = 0f;//1
                            cell123456.BorderWidthTop = 0f;
                            cell123456.BorderWidthBottom = 0f;

                            cell123456.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell123456);


                            PdfPCell cell1234567 = new PdfPCell(new Phrase("VC:Vehicle Class ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1234567.Colspan = 6;
                            cell1234567.BorderWidthLeft = 0f;
                            cell1234567.BorderWidthRight = 1f;
                            cell1234567.BorderWidthTop = 0f;//1
                            cell1234567.BorderWidthBottom = 0f;

                            cell1234567.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1234567);

                            PdfPCell cell12345678 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell12345678.Colspan = 6;
                            cell12345678.BorderWidthLeft = 1f;//0
                            cell12345678.BorderWidthRight = 0f;
                            cell12345678.BorderWidthTop = 0f;//1
                            cell12345678.BorderWidthBottom = 0f;
                            cell12345678.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell12345678);
                            PdfPCell cell12345679 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell12345679.Colspan = 4;//7
                            cell12345679.BorderWidthLeft = 0f;
                            cell12345679.BorderWidthRight = 0f;//1
                            cell12345679.BorderWidthTop = 0f;
                            cell12345679.BorderWidthBottom = 0f;

                            cell12345679.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell12345679);


                            PdfPCell cell1234567890 = new PdfPCell(new Phrase("VT:Vehicle Type ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1234567890.Colspan = 6;
                            cell1234567890.BorderWidthLeft = 0f;
                            cell1234567890.BorderWidthRight = 1f;
                            cell1234567890.BorderWidthTop = 0f;//1
                            cell1234567890.BorderWidthBottom = 0f;

                            cell1234567890.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1234567890);
                            PdfPCell cell120934 = new PdfPCell(new Phrase("RTOLocation Name : " + dt.Rows[0]["RTOLocationName"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell120934.Colspan = 7;//7
                            cell120934.BorderWidthLeft = 1f;
                            cell120934.BorderWidthRight = 0f;
                            cell120934.BorderWidthTop = 0f;
                            cell120934.BorderWidthBottom = 0f;

                            cell120934.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell120934);
                            PdfPCell cell1209390 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1209390.Colspan = 3;//3
                            cell1209390.BorderWidthLeft = 0f;
                            cell1209390.BorderWidthRight = 0f;
                            cell1209390.BorderWidthTop = 0f;
                            cell1209390.BorderWidthBottom = 0f;

                            cell1209390.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1209390);

                            PdfPCell cell1209391 = new PdfPCell(new Phrase("OS:Order Status (New Order/Embossing Done/Closed)", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1209391.Colspan = 6;//7
                            cell1209391.BorderWidthLeft = 0f;
                            cell1209391.BorderWidthRight = 1f;
                            cell1209391.BorderWidthTop = 0f;
                            cell1209391.BorderWidthBottom = 0f;

                            cell1209391.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1209391);

                            PdfPCell cell12a = new PdfPCell(new Phrase("SR.No", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                            cell12a.Colspan = 1;
                            cell12a.BorderWidthLeft = 1f;
                            cell12a.BorderWidthRight = .8f;
                            cell12a.BorderWidthTop = 0.8f;
                            cell12a.BorderWidthBottom = 0.8f;

                            cell12a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell12a);

                            PdfPCell cell1210a = new PdfPCell(new Phrase("ORD", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                            cell1210a.Colspan = 1;
                            cell1210a.BorderWidthLeft = 0f;
                            cell1210a.BorderWidthRight = .8f;
                            cell1210a.BorderWidthTop = 0.8f;
                            cell1210a.BorderWidthBottom = 0.8f;
                            cell1210a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1210a);


                            PdfPCell cell1213a = new PdfPCell(new Phrase("VC", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                            cell1213a.Colspan = 1;
                            cell1213a.BorderWidthLeft = 0f;
                            cell1213a.BorderWidthRight = .8f;
                            cell1213a.BorderWidthTop = 0.8f;
                            cell1213a.BorderWidthBottom = 0.8f;
                            cell1213a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1213a);


                            PdfPCell cell1209a = new PdfPCell(new Phrase("Vehicle No", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK))); //6
                            cell1209a.Colspan = 1;
                            cell1209a.BorderWidthLeft = 0f;
                            cell1209a.BorderWidthRight = .8f;
                            cell1209a.BorderWidthTop = 0.8f;
                            cell1209a.BorderWidthBottom = 0.8f;
                            cell1209a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1209a);

                            PdfPCell cell12233a = new PdfPCell(new Phrase("VT", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                            cell12233a.Colspan = 1;
                            cell12233a.BorderWidthLeft = 0f;
                            cell12233a.BorderWidthRight = .8f;
                            cell12233a.BorderWidthTop = 0.8f;
                            cell12233a.BorderWidthBottom = 0.8f;
                            cell12233a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell12233a);



                            PdfPCell cell1206a = new PdfPCell(new Phrase("Chassis No", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                            cell1206a.Colspan = 1;
                            cell1206a.BorderWidthLeft = 0f;
                            cell1206a.BorderWidthRight = .8f;
                            cell1206a.BorderWidthTop = 0.8f;
                            cell1206a.BorderWidthBottom = 0.8f;
                            cell1206a.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1206a);

                            PdfPCell cell1221a = new PdfPCell(new Phrase("EngineNo", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                            cell1221a.Colspan = 1;
                            cell1221a.BorderWidthLeft = 0f;
                            cell1221a.BorderWidthRight = 0.8f;
                            cell1221a.BorderWidthTop = 0.8f;
                            cell1221a.BorderWidthBottom = 0.8f;
                            cell1221a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1221a);


                            if (HSRPStateID == "4")
                            {
                                PdfPCell cell120933b = new PdfPCell(new Phrase("ID", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                                cell120933b.Colspan = 1;
                                cell120933b.BorderWidthLeft = 0f;
                                cell120933b.BorderWidthRight = 0.8f;
                                cell120933b.BorderWidthTop = 0.8f;
                                cell120933b.BorderWidthBottom = 0f;
                                cell120933b.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                table.AddCell(cell120933b);
                            }
                            else if (HSRPStateID.Equals("5"))
                            {
                                PdfPCell cell120ab = new PdfPCell(new Phrase("ID", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                                cell120ab.Colspan = 1;
                                cell120ab.BorderWidthLeft = 0f;
                                cell120ab.BorderWidthRight = 0.8f;
                                cell120ab.BorderWidthTop = 0.8f;
                                cell120ab.BorderWidthBottom = 0f;
                                cell120ab.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                table.AddCell(cell120ab);
                            }
                            else
                            {

                                PdfPCell cell120933a = new PdfPCell(new Phrase("Owner Name", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                                cell120933a.Colspan = 1;
                                cell120933a.BorderWidthLeft = 0f;
                                cell120933a.BorderWidthRight = 0.8f;
                                cell120933a.BorderWidthTop = 0.8f;
                                cell120933a.BorderWidthBottom = 0f;
                                cell120933a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                table.AddCell(cell120933a);
                            }
                            PdfPCell cell120935a = new PdfPCell(new Phrase("Front Plate Size", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                            cell120935a.Colspan = 1;
                            cell120935a.BorderWidthLeft = 0f;
                            cell120935a.BorderWidthRight = 0.8f;
                            cell120935a.BorderWidthTop = 0.8f;
                            cell120935a.BorderWidthBottom = 0f;
                            cell120935a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell120935a);

                            PdfPCell cell120936a = new PdfPCell(new Phrase("Front Laser No.", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK))); //6
                            cell120936a.Colspan = 1;
                            cell120936a.BorderWidthLeft = 0f;
                            cell120936a.BorderWidthRight = 0.8f;
                            cell120936a.BorderWidthTop = 0.8f;
                            cell120936a.BorderWidthBottom = 0f;
                            cell120936a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell120936a);


                            PdfPCell cell120937a = new PdfPCell(new Phrase("Rear Plate Size", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                            cell120937a.Colspan = 1;
                            cell120937a.BorderWidthLeft = 0f;
                            cell120937a.BorderWidthRight = 0.8f;
                            cell120937a.BorderWidthTop = 0.8f;
                            cell120937a.BorderWidthBottom = 0f;
                            cell120937a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell120937a);

                            PdfPCell cell120938a = new PdfPCell(new Phrase("Rear Laser No.", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK))); //6
                            cell120938a.Colspan = 1;
                            cell120938a.BorderWidthLeft = 0f;
                            cell120938a.BorderWidthRight = 0.8f;
                            cell120938a.BorderWidthTop = 0.8f;
                            cell120938a.BorderWidthBottom = 0f;
                            cell120938a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell120938a);

                            if (strStateID == "1")
                            {

                                PdfPCell ABCDEF = new PdfPCell(new Phrase("Affixation Center", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                                ABCDEF.Colspan = 1;
                                ABCDEF.BorderWidthLeft = 0f;
                                ABCDEF.BorderWidthRight = 0.8f;
                                ABCDEF.BorderWidthTop = 0.8f;
                                ABCDEF.BorderWidthBottom = 0f;
                                ABCDEF.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                table.AddCell(ABCDEF);
                            }
                            else
                            {

                                PdfPCell cell120934a = new PdfPCell(new Phrase("Amount", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                                cell120934a.Colspan = 1;
                                cell120934a.BorderWidthLeft = 0f;
                                cell120934a.BorderWidthRight = 0.8f;
                                cell120934a.BorderWidthTop = 0.8f;
                                cell120934a.BorderWidthBottom = 0f;
                                cell120934a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                table.AddCell(cell120934a);
                            }

                            PdfPCell cell120934ab = new PdfPCell(new Phrase("OS", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                            cell120934ab.Colspan = 1;
                            cell120934ab.BorderWidthLeft = 0f;
                            cell120934ab.BorderWidthRight = 0.8f;
                            cell120934ab.BorderWidthTop = 0.8f;
                            cell120934ab.BorderWidthBottom = 0f;
                            cell120934ab.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell120934ab);

                        }
                        total = total + 1;
                        //============================================================ ajay end ======================================================================
                        PdfPCell cell13 = new PdfPCell(new Phrase("" + j, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell13.Colspan = 1;
                        cell13.BorderWidthLeft = 0.8f;
                        cell13.BorderWidthRight = 0f;
                        cell13.BorderWidthTop = 0f;
                        cell13.BorderWidthBottom = .5f;
                        cell13.MinimumHeight = 0f;//25
                        cell13.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell13);

                        PdfPCell cell1212 = new PdfPCell(new Phrase(dt.Rows[i]["OrderBookDate"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1212.Colspan = 1;
                        cell1212.MinimumHeight = 0f;//25

                        cell1212.BorderWidthLeft = 0.8f;
                        cell1212.BorderWidthRight = .8f;
                        cell1212.BorderWidthTop = 0f;
                        cell1212.BorderWidthBottom = .5f;

                        cell1212.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1212);



                        PdfPCell cell1214 = new PdfPCell(new Phrase(dt.Rows[i]["VehicleClass"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1214.Colspan = 1;
                        cell1214.MinimumHeight = 0f;//25

                        cell1214.BorderWidthLeft = 0f;
                        cell1214.BorderWidthRight = .8f;
                        cell1214.BorderWidthTop = 0f;
                        cell1214.BorderWidthBottom = .5f;
                        cell1214.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1214);


                        PdfPCell cell1211 = new PdfPCell(new Phrase(dt.Rows[i]["VehicleRegNo"].ToString(), new iTextSharp.text.Font(bfTimes, 12f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK))); //6
                        cell1211.Colspan = 1;
                        cell1211.MinimumHeight = 0f;//25

                        cell1211.BorderWidthLeft = 0f;
                        cell1211.BorderWidthRight = 0.8f;
                        cell1211.BorderWidthTop = 0f;
                        cell1211.BorderWidthBottom = .5f;
                        cell1211.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1211);



                        PdfPCell cell1219 = new PdfPCell(new Phrase(dt.Rows[i]["VehicleType"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1219.Colspan = 1;
                        cell1219.MinimumHeight = 0f;//25

                        cell1219.BorderWidthLeft = 0f;
                        cell1219.BorderWidthRight = .8f;
                        cell1219.BorderWidthTop = 0f;
                        cell1219.BorderWidthBottom = .5f;

                        cell1219.HorizontalAlignment = 0; //0=Left, 1=     //PdfPCell cell1218 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));

                        table.AddCell(cell1219);

                        PdfPCell cell1216 = new PdfPCell(new Phrase(dt.Rows[i]["ChassisNo"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1216.Colspan = 1;
                        cell1216.MinimumHeight = 0f;//25

                        cell1216.BorderWidthLeft = 0f;
                        cell1216.BorderWidthRight = .8f;
                        cell1216.BorderWidthTop = 0f;
                        cell1216.BorderWidthBottom = .5f;

                        cell1216.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right

                        table.AddCell(cell1216);

                        PdfPCell cell1222 = new PdfPCell(new Phrase(dt.Rows[i]["EngineNo"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell1222.Colspan = 1;
                        cell1222.MinimumHeight = 0f;//25

                        cell1222.BorderWidthLeft = 0f;
                        cell1222.BorderWidthRight = .8f;
                        cell1222.BorderWidthTop = 0f;
                        cell1222.BorderWidthBottom = .5f;

                        cell1222.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1222);
                        if (HSRPStateID == "2")
                        {
                            PdfPCell cell12000 = new PdfPCell(new Phrase(dt.Rows[i]["ID"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell12000.Colspan = 1;
                            cell12000.MinimumHeight = 0f;//25

                            cell12000.BorderWidthLeft = 0f;
                            cell12000.BorderWidthRight = .8f;
                            cell12000.BorderWidthTop = .5f;
                            cell12000.BorderWidthBottom = .5f;

                            cell12000.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell12000);
                        }
                        else if (HSRPStateID == "4")
                        {
                            PdfPCell cell12000 = new PdfPCell(new Phrase(dt.Rows[i]["ID"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell12000.Colspan = 1;
                            cell12000.MinimumHeight = 0f;//25

                            cell12000.BorderWidthLeft = 0f;
                            cell12000.BorderWidthRight = .8f;
                            cell12000.BorderWidthTop = .5f;
                            cell12000.BorderWidthBottom = .5f;

                            cell12000.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell12000);
                        }
                        else if (HSRPStateID.Equals("5"))
                        {
                            PdfPCell cell000 = new PdfPCell(new Phrase(dt.Rows[i]["ID"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell000.Colspan = 1;
                            cell000.MinimumHeight = 0f;//25

                            cell000.BorderWidthLeft = 0f;
                            cell000.BorderWidthRight = .8f;
                            cell000.BorderWidthTop = .5f;
                            cell000.BorderWidthBottom = .5f;

                            cell000.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell000);
                        }
                        else
                        {
                            PdfPCell cell1209315 = new PdfPCell(new Phrase(dt.Rows[i]["OwnerName"].ToString() + "/ "  +  dt.Rows[i]["MobileNo"].ToString() , new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1209315.Colspan = 1;
                            cell1209315.MinimumHeight = 0f;//25

                            cell1209315.BorderWidthLeft = 0f;
                            cell1209315.BorderWidthRight = .8f;
                            cell1209315.BorderWidthTop = .5f;
                            cell1209315.BorderWidthBottom = .5f;

                            cell1209315.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1209315);

                        }


                        PdfPCell cell1209316 = new PdfPCell(new Phrase(dt.Rows[i]["FrontProductCode"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                        cell1209316.Colspan = 1;
                        cell1209316.MinimumHeight = 0f; //25

                        cell1209316.BorderWidthLeft = 0f;
                        cell1209316.BorderWidthRight = .8f;
                        cell1209316.BorderWidthTop = .5f;
                        cell1209316.BorderWidthBottom = 0f;
                        cell1209316.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1209316);


                        PdfPCell cell1123 = new PdfPCell(new Phrase(dt.Rows[i]["HSRP_Front_LaserCode"].ToString().Replace(".", ""), new iTextSharp.text.Font(bfTimes, 12f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK))); //6
                        cell1123.Colspan = 1;
                        cell1123.MinimumHeight = 0f;//25

                        cell1123.BorderWidthLeft = 0f;
                        cell1123.BorderWidthRight = .8f;
                        cell1123.BorderWidthTop = .5f;
                        cell1123.BorderWidthBottom = 0f;

                        cell1123.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1123);


                        PdfPCell cell1124 = new PdfPCell(new Phrase(dt.Rows[i]["RearProductCode"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                        cell1124.Colspan = 1;
                        cell1124.MinimumHeight = 0f;//25

                        cell1124.BorderWidthLeft = 0f;
                        cell1124.BorderWidthRight = .8f;
                        cell1124.BorderWidthTop = .5f;
                        cell1124.BorderWidthBottom = 0f;

                        cell1124.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1124);

                        PdfPCell cell1209340 = new PdfPCell(new Phrase(dt.Rows[i]["HSRP_Rear_LaserCode"].ToString().Replace(".", ""), new iTextSharp.text.Font(bfTimes, 12f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK))); // 6
                        cell1209340.Colspan = 1;
                        cell1209340.MinimumHeight = 0f;//25

                        cell1209340.BorderWidthLeft = 0f;
                        cell1209340.BorderWidthRight = .8f;
                        cell1209340.BorderWidthTop = .5f;
                        cell1209340.BorderWidthBottom = 0f;

                        cell1209340.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1209340);


                        string tot = dt.Rows[i]["roundoff_netamount"].ToString();
                        Amount = Amount + float.Parse(tot);

                        if (strStateID == "1")
                        {

                            PdfPCell ABCDEF = new PdfPCell(new Phrase(dt.Rows[i]["AffixCenterDesc"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                            ABCDEF.Colspan = 1;
                            ABCDEF.MinimumHeight = 0f;//25

                            ABCDEF.BorderWidthLeft = 0f;
                            ABCDEF.BorderWidthRight = .8f;
                            ABCDEF.BorderWidthTop = .5f;
                            ABCDEF.BorderWidthBottom = .5f;

                            ABCDEF.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(ABCDEF);
                        }
                        else
                        {
                            PdfPCell cell1209329 = new PdfPCell(new Phrase(dt.Rows[i]["roundoff_netamount"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                            cell1209329.Colspan = 1;
                            cell1209329.MinimumHeight = 0f;//25

                            cell1209329.BorderWidthLeft = 0f;
                            cell1209329.BorderWidthRight = .8f;
                            cell1209329.BorderWidthTop = .5f;
                            cell1209329.BorderWidthBottom = .5f;

                            cell1209329.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1209329);
                        }

                        PdfPCell cell1209329a = new PdfPCell(new Phrase(dt.Rows[i]["Orderstatus"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                        cell1209329a.Colspan = 1;
                        cell1209329a.MinimumHeight = 0f;//25

                        cell1209329a.BorderWidthLeft = 0f;
                        cell1209329a.BorderWidthRight = .8f;
                        cell1209329a.BorderWidthTop = .5f;
                        cell1209329a.BorderWidthBottom = .5f;

                        cell1209329a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1209329a);
                    }

                    try
                    {
                        
                            string mean = "MO.C = MOTOR CYCLE,  L.CL =LMV(CLASS),  TRAC =TRACTOR,  SCOO = SCOOTER,  T.Whe.= THREE WHEELER,  Trailers =MCV/HCV/TRAILERS,  T = Transport, N.T.= Non-Transport";
                            PdfPCell cell1209340 = new PdfPCell(new Phrase(mean, new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                            cell1209340.Colspan = 16;
                            cell1209340.BorderWidthLeft = .8f;
                            cell1209340.BorderWidthRight = .8f;
                            cell1209340.BorderWidthTop = .8f;
                            cell1209340.BorderWidthBottom = .5f;
                            cell1209340.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1209340);
                        
                        PdfPCell cell12241 = new PdfPCell(new Phrase("(Sign)", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell12241.Colspan = 7;
                        cell12241.BorderWidthLeft = 0f;
                        cell12241.BorderWidthRight = 0f;
                        cell12241.BorderWidthTop = 0f;
                        cell12241.BorderWidthBottom = 0f;
                        cell12241.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell12241);
                        document.Add(table);
                        // document.Add(table1); 

                        document.Close();

                  

                        HttpContext context = HttpContext.Current;

                        context.Response.ContentType = "Application/pdf";
                        context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                        context.Response.WriteFile(PdfFolder);
                        context.Response.End();

                    }
                    catch
                    {

                    }
                }
                else
                {
                    lblErrMsg.Text = "No Record Found!!";
                }

            }
            catch (Exception ex)
            {
                lblErrMsg.Text = ex.Message;
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
        protected void Button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtInvoiceNo.Text) && (dropdownDuplicateFIle.SelectedIndex <= 0))
            {
                Response.Write("<script> alert('Please Enter Invoice No.')</script>");
                return;
            }
            HttpContext context = HttpContext.Current;
            string filename = "HSRP-INVOICE" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".pdf";

            string SQLString = String.Empty;
            String StringField = String.Empty;
            String StringAlert = String.Empty;

            StringBuilder bb = new StringBuilder();

            //Creates an instance of the iTextSharp.text.Document-object:
            Document document = new Document();

            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);


            string PdfFolder = ConfigurationManager.AppSettings["PdfFolder"].ToString() + filename;
            PdfWriter.GetInstance(document, new FileStream(PdfFolder, FileMode.Create));

            //Opens the document:
            document.Open();



            //Opens the document:
            GetRecords();
            strStateId = dtInvoiceData.Rows[0]["hsrp_stateid"].ToString();
            string strRTOLocationId = dtInvoiceData.Rows[0]["RTOLocationId"].ToString();

            for (int i = 0; i < dtInvoiceData.Rows.Count; i++)
            {
                //  string lblVehicleRegNo,FLasercode,RearLaserCode;
                // tmpbillno = Session["UID"].ToString()+ System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() ;
               

                string lblVehicleRegNo = dtInvoiceData.Rows[i]["VehicleRegNo"].ToString();

                string OrderStatus = dtInvoiceData.Rows[i]["OrderStatus"].ToString();

                string strRecordId = dtInvoiceData.Rows[i]["hsrprecordid"].ToString();
                string flaser = dtInvoiceData.Rows[i]["HSRP_Front_LaserCode"].ToString();
                string rlaser = dtInvoiceData.Rows[i]["hsrp_rear_lasercode"].ToString();
                String HSRPRecordID = dtInvoiceData.Rows[i]["hsrprecordid"].ToString();
                    //  string OrderStatus = e.Item["OrderStatus"].ToString();
                    PdfPTable table2 = new PdfPTable(7);
                    PdfPTable table1 = new PdfPTable(7);
                    PdfPTable table = new PdfPTable(7);

                    //actual width of table in points
                    table.TotalWidth = 1000f;
                    HSRPStateID = DropDownListStateName.SelectedValue;
                    string OldRegPlate = string.Empty;
                    //if (CheckBoxOldPlate.Checked == true)
                    //{
                    //    OldRegPlate = "Y";
                    //}
                    //else
                    //{
                    //    OldRegPlate = "N";
                    //}

                    DataTable GetAddress;
                    string Address;
                    GetAddress = Utils.GetDataTable("select * from HSRPState WHERE HSRP_StateID='" + strStateId + "'", CnnString);

                    if (GetAddress.Rows[0]["pincode"] != "" || GetAddress.Rows[0]["pincode"] != null)
                    {
                        Address = " - " + GetAddress.Rows[0]["pincode"];
                    }
                    else
                    {
                        Address = "";
                    }



                    BAL obj = new BAL();
                    //  HSRPRecordID = Request.QueryString["HSRPRecordID"].ToString();
                    //  string OrderStatus = Request.QueryString["Status"].ToString();
                    string UserID = Session["UID"].ToString();

                   
                        DataTable dataSetFillHSRPDeliveryChallan = new DataTable();

                        if (obj.FillHSRPRecordDeliveryChallan2(HSRPRecordID, ref dataSetFillHSRPDeliveryChallan))
                        {

                            //fix the absolute width of the table
                            PdfPCell cell1211 = new PdfPCell(new Phrase("Original", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1211.Colspan = 1;
                            cell1211.BorderWidthLeft = 1f;
                            cell1211.BorderWidthRight = 1f;
                            cell1211.BorderWidthTop = 1f;
                            cell1211.BorderWidthBottom = 1f;

                            cell1211.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                            table1.AddCell(cell1211);
                            PdfPCell cell12111 = new PdfPCell(new Phrase("INVOICE", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
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
                            PdfPCell cell12113 = new PdfPCell(new Phrase("INVOICE", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell12113.Colspan = 6;
                            cell12113.BorderWidthLeft = 0f;
                            cell12113.BorderWidthRight = 0f;
                            cell12113.BorderWidthTop = 0f;
                            cell12113.BorderWidthBottom = 0f;

                            cell12113.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                            table2.AddCell(cell12113);


                            PdfPCell cell12 = new PdfPCell(new Phrase("" + GetAddress.Rows[0]["CompanyName"].ToString(), new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell12.Colspan = 5;
                            cell12.BorderWidthLeft = 1f;
                            cell12.BorderWidthRight = .8f;
                            cell12.BorderWidthTop = .8f;
                            cell12.BorderWidthBottom = 0f;

                            cell12.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell12);

                            PdfPCell cell1207 = new PdfPCell(new Phrase("C. E. R/C", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1207.Colspan = 1;
                            cell1207.BorderWidthLeft = 0f;
                            cell1207.BorderWidthRight = 0f;
                            cell1207.BorderWidthTop = .8f;
                            cell1207.BorderWidthBottom = 0f;

                            cell1207.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1207);


                            PdfPCell cell1208 = new PdfPCell(new Phrase(": " + GetAddress.Rows[0]["CERC"].ToString(), new iTextSharp.text.Font(bfTimes, 5f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1208.Colspan = 1;
                            cell1208.BorderWidthLeft = 0f;
                            cell1208.BorderWidthRight = 1f;
                            cell1208.BorderWidthTop = .8f;
                            cell1208.BorderWidthBottom = 0f;

                            cell1208.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1208);



                            PdfPCell cell1203 = new PdfPCell(new Phrase("" + GetAddress.Rows[0]["Address1"].ToString() + " , " + GetAddress.Rows[0]["city"].ToString() + Address.ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                            cell1203.Colspan = 5;
                            cell1203.BorderWidthLeft = 1f;
                            cell1203.BorderWidthRight = .8f;
                            cell1203.BorderWidthTop = 0f;
                            cell1203.BorderWidthBottom = 0f;
                            cell1203.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1203);

                            PdfPCell cell2 = new PdfPCell(new Phrase("TIN NO.", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell2.Colspan = 1;

                            cell2.BorderWidthLeft = 0f;
                            cell2.BorderWidthRight = 0f;
                            cell2.BorderWidthTop = .8f;
                            cell2.BorderWidthBottom = 0f;
                            cell2.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell2);

                            // string getTinNo1 = Utils.getScalarValue("select TinNo from hsrpstate where HSRP_StateID='" + HSRPStateID + "' ", CnnString);

                            PdfPCell cell21111 = new PdfPCell(new Phrase(": " + GetAddress.Rows[0]["TinNo"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell21111.Colspan = 1;
                            cell21111.BorderWidthLeft = 0f;
                            cell21111.BorderWidthRight = 1f;
                            cell21111.BorderWidthTop = .8f;
                            cell21111.BorderWidthBottom = 0f;
                            cell21111.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell21111);


                            PdfPCell cell1204 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                            cell1204.Colspan = 5;
                            cell1204.BorderWidthLeft = 1f;
                            cell1204.BorderWidthRight = .8f;
                            cell1204.BorderWidthTop = 0f;
                            cell1204.BorderWidthBottom = 0f;
                            cell1204.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1204);


                            PdfPCell cell1209 = new PdfPCell(new Phrase("COMMODITY", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1209.Colspan = 1;
                            cell1209.BorderWidthLeft = 0f;
                            cell1209.BorderWidthRight = 0f;
                            cell1209.BorderWidthTop = .8f;
                            cell1209.BorderWidthBottom = 0f;

                            cell1209.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1209);


                            PdfPCell cell12115 = new PdfPCell(new Phrase(": " + GetAddress.Rows[0]["Commodity"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell12115.Colspan = 1;
                            cell12115.BorderWidthLeft = 0f;
                            cell12115.BorderWidthRight = 1f;
                            cell12115.BorderWidthTop = .8f;
                            cell12115.BorderWidthBottom = 0f;

                            cell12115.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell12115);


                            PdfPCell cell1205 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                            cell1205.Colspan = 5;
                            cell1205.BorderWidthLeft = 1f;
                            cell1205.BorderWidthRight = .8f;
                            cell1205.BorderWidthTop = 0f;
                            cell1205.BorderWidthBottom = 0f;
                            cell1205.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1205);


                            PdfPCell cell1213 = new PdfPCell(new Phrase("C. H.", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1213.Colspan = 1;
                            cell1213.BorderWidthLeft = 0f;
                            cell1213.BorderWidthRight = 0f;
                            cell1213.BorderWidthTop = .8f;
                            cell1213.BorderWidthBottom = 0f;

                            cell1213.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1213);


                            PdfPCell cell1214 = new PdfPCell(new Phrase(": " + GetAddress.Rows[0]["CH"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1214.Colspan = 1;
                            cell1214.BorderWidthLeft = 0f;
                            cell1214.BorderWidthRight = 1f;
                            cell1214.BorderWidthTop = .8f;
                            cell1214.BorderWidthBottom = 0f;

                            cell1214.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1214);

                            PdfPCell cell1206 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                            cell1206.Colspan = 5;
                            cell1206.BorderWidthLeft = 1f;
                            cell1206.BorderWidthRight = .8f;
                            cell1206.BorderWidthTop = 0f;
                            cell1206.BorderWidthBottom = 0f;
                            cell1206.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1206);


                            PdfPCell cell1215 = new PdfPCell(new Phrase("RANGE", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1215.Colspan = 1;
                            cell1215.BorderWidthLeft = 0f;
                            cell1215.BorderWidthRight = 0f;
                            cell1215.BorderWidthTop = .8f;
                            cell1215.BorderWidthBottom = 0f;

                            cell1215.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1215);


                            PdfPCell cell1216 = new PdfPCell(new Phrase(": " + GetAddress.Rows[0]["Range"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1216.Colspan = 1;
                            cell1216.BorderWidthLeft = 0f;
                            cell1216.BorderWidthRight = 1f;
                            cell1216.BorderWidthTop = .8f;
                            cell1216.BorderWidthBottom = 0f;

                            cell1216.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1216);



                            PdfPCell cell1217 = new PdfPCell(new Phrase("Book No", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1217.Colspan = 1;
                            cell1217.BorderWidthLeft = 1f;
                            cell1217.BorderWidthRight = .8f;
                            cell1217.BorderWidthTop = .8f;
                            cell1217.BorderWidthBottom = 0f;

                            cell1217.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1217);


                            PdfPCell cell1218 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1218.Colspan = 2;
                            cell1218.BorderWidthLeft = 0f;
                            cell1218.BorderWidthRight = .8f;
                            cell1218.BorderWidthTop = .8f;
                            cell1218.BorderWidthBottom = 0f;

                            cell1218.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1218);

                            PdfPCell cell1219 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1219.Colspan = 1;
                            cell1219.BorderWidthLeft = 0f;
                            cell1219.BorderWidthRight = .8f;
                            cell1219.BorderWidthTop = .8f;
                            cell1219.BorderWidthBottom = 0f;

                            cell1219.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1219);

                            PdfPCell cell1220 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1220.Colspan = 1;
                            cell1220.BorderWidthLeft = 0f;
                            cell1220.BorderWidthRight = .8f;
                            cell1220.BorderWidthTop = .8f;
                            cell1220.BorderWidthBottom = 0f;

                            cell1220.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1220);

                            PdfPCell cell1221 = new PdfPCell(new Phrase("DIVISION", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1221.Colspan = 1;
                            cell1221.BorderWidthLeft = 0f;
                            cell1221.BorderWidthRight = 0f;
                            cell1221.BorderWidthTop = .8f;
                            cell1221.BorderWidthBottom = 0f;

                            cell1221.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1221);

                            PdfPCell cell1222 = new PdfPCell(new Phrase(": " + GetAddress.Rows[0]["Division"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1222.Colspan = 1;
                            cell1222.BorderWidthLeft = 0f;
                            cell1222.BorderWidthRight = 1f;
                            cell1222.BorderWidthTop = .8f;
                            cell1222.BorderWidthBottom = 0f;

                            cell1222.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1222);







                            PdfPCell cell1223 = new PdfPCell(new Phrase("Invoice No", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1223.Colspan = 1;
                            cell1223.BorderWidthLeft = 1f;
                            cell1223.BorderWidthRight = .8f;
                            cell1223.BorderWidthTop = 0f;
                            cell1223.BorderWidthBottom = 0f;

                            cell1223.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1223);


                            PdfPCell cell1224 = new PdfPCell(new Phrase(dataSetFillHSRPDeliveryChallan.Rows[0]["ChallanNo"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1224.Colspan = 2;
                            cell1224.BorderWidthLeft = 0f;
                            cell1224.BorderWidthRight = .8f;
                            cell1224.BorderWidthTop = 0f;
                            cell1224.BorderWidthBottom = 0f;

                            cell1224.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1224);

                            PdfPCell cell1225 = new PdfPCell(new Phrase("Date", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1225.Colspan = 1;
                            cell1225.BorderWidthLeft = 0f;
                            cell1225.BorderWidthRight = .8f;
                            cell1225.BorderWidthTop = 0f;
                            cell1225.BorderWidthBottom = 0f;

                            cell1225.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1225);

                            string OrderClosedDate;
                            if (dataSetFillHSRPDeliveryChallan.Rows[0]["OrderClosedDate"].ToString() == "1/1/1900 12:00:00 AM" || dataSetFillHSRPDeliveryChallan.Rows[0]["OrderClosedDate"].ToString() == null || dataSetFillHSRPDeliveryChallan.Rows[0]["OrderClosedDate"].ToString() == "")
                            {
                                OrderClosedDate = DateTime.Now.ToString("dd/MM/yyyy");
                            }
                            else
                            {
                                OrderClosedDate = Convert.ToDateTime(dataSetFillHSRPDeliveryChallan.Rows[0]["OrderClosedDate"]).ToString("dd/MM/yyyy");
                            }

                            PdfPCell cell1226 = new PdfPCell(new Phrase("" + OrderClosedDate.ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1226.Colspan = 1;
                            cell1226.BorderWidthLeft = 0f;
                            cell1226.BorderWidthRight = .8f;
                            cell1226.BorderWidthTop = 0f;
                            cell1226.BorderWidthBottom = 0f;

                            cell1226.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1226);

                            PdfPCell cell1227 = new PdfPCell(new Phrase("Commissionerate", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1227.Colspan = 1;
                            cell1227.BorderWidthLeft = 0f;
                            cell1227.BorderWidthRight = 0f;
                            cell1227.BorderWidthTop = .8f;
                            cell1227.BorderWidthBottom = 0f;

                            cell1227.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1227);

                            PdfPCell cell1228 = new PdfPCell(new Phrase(": " + GetAddress.Rows[0]["Commissionerate"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1228.Colspan = 1;
                            cell1228.BorderWidthLeft = 0f;
                            cell1228.BorderWidthRight = 1f;
                            cell1228.BorderWidthTop = .8f;
                            cell1228.BorderWidthBottom = 0f;

                            cell1228.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1228);




                            PdfPCell cell1229 = new PdfPCell(new Phrase("P.O. No.", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1229.Colspan = 1;
                            cell1229.BorderWidthLeft = 1f;
                            cell1229.BorderWidthRight = .8f;
                            cell1229.BorderWidthTop = .8f;
                            cell1229.BorderWidthBottom = 0f;

                            cell1229.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1229);


                            PdfPCell cell1230 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1230.Colspan = 2;
                            cell1230.BorderWidthLeft = 0f;
                            cell1230.BorderWidthRight = .8f;
                            cell1230.BorderWidthTop = .8f;
                            cell1230.BorderWidthBottom = 0f;

                            cell1230.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1230);

                            PdfPCell cell1231 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1231.Colspan = 1;
                            cell1231.BorderWidthLeft = 0f;
                            cell1231.BorderWidthRight = .8f;
                            cell1231.BorderWidthTop = .8f;
                            cell1231.BorderWidthBottom = 0f;

                            cell1231.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1231);

                            PdfPCell cell1232 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1232.Colspan = 1;
                            cell1232.BorderWidthLeft = 0f;
                            cell1232.BorderWidthRight = .8f;
                            cell1232.BorderWidthTop = .8f;
                            cell1232.BorderWidthBottom = 0f;

                            cell1232.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1232);

                            PdfPCell cell1233 = new PdfPCell(new Phrase("Party C. E. R/C NO", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1233.Colspan = 1;
                            cell1233.BorderWidthLeft = 0f;
                            cell1233.BorderWidthRight = 0f;
                            cell1233.BorderWidthTop = .8f;
                            cell1233.BorderWidthBottom = 0f;

                            cell1233.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1233);

                            PdfPCell cell1234 = new PdfPCell(new Phrase(": NA", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1234.Colspan = 1;
                            cell1234.BorderWidthLeft = 0f;
                            cell1234.BorderWidthRight = 1f;
                            cell1234.BorderWidthTop = .8f;
                            cell1234.BorderWidthBottom = 0f;

                            cell1234.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1234);





                            PdfPCell cell1235 = new PdfPCell(new Phrase("RR/GR NO", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1235.Colspan = 1;
                            cell1235.BorderWidthLeft = 1f;
                            cell1235.BorderWidthRight = .8f;
                            cell1235.BorderWidthTop = .8f;
                            cell1235.BorderWidthBottom = 0f;

                            cell1235.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1235);


                            PdfPCell cell1236 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1236.Colspan = 2;
                            cell1236.BorderWidthLeft = 0f;
                            cell1236.BorderWidthRight = .8f;
                            cell1236.BorderWidthTop = .8f;
                            cell1236.BorderWidthBottom = 0f;

                            cell1236.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1236);

                            PdfPCell cell1237 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1237.Colspan = 1;
                            cell1237.BorderWidthLeft = 0f;
                            cell1237.BorderWidthRight = .8f;
                            cell1237.BorderWidthTop = .8f;
                            cell1237.BorderWidthBottom = 0f;

                            cell1237.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1237);

                            PdfPCell cell1238 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1238.Colspan = 1;
                            cell1238.BorderWidthLeft = 0f;
                            cell1238.BorderWidthRight = .8f;
                            cell1238.BorderWidthTop = .8f;
                            cell1238.BorderWidthBottom = 0f;

                            cell1238.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1238);

                            PdfPCell cell1239 = new PdfPCell(new Phrase("Party CST/TIN No", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1239.Colspan = 1;
                            cell1239.BorderWidthLeft = 0f;
                            cell1239.BorderWidthRight = 0f;
                            cell1239.BorderWidthTop = .8f;
                            cell1239.BorderWidthBottom = 0f;

                            cell1239.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1239);

                            PdfPCell cell1240 = new PdfPCell(new Phrase(": NA", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1240.Colspan = 1;
                            cell1240.BorderWidthLeft = 0f;
                            cell1240.BorderWidthRight = 1f;
                            cell1240.BorderWidthTop = .8f;
                            cell1240.BorderWidthBottom = 0f;

                            cell1240.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1240);




                            PdfPCell cell1241 = new PdfPCell(new Phrase("Consignee", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1241.Colspan = 1;
                            cell1241.BorderWidthLeft = 1f;
                            cell1241.BorderWidthRight = .8f;
                            cell1241.BorderWidthTop = .8f;
                            cell1241.BorderWidthBottom = 0f;

                            cell1241.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1241);

                            PdfPCell cell1242 = new PdfPCell(new Phrase(dataSetFillHSRPDeliveryChallan.Rows[0]["Address1"].ToString().ToUpper(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1242.Colspan = 5;
                            cell1242.BorderWidthLeft = 0f;
                            cell1242.BorderWidthRight = .8f;
                            cell1242.BorderWidthTop = .8f;
                            cell1242.BorderWidthBottom = 0f;

                            cell1242.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1242);

                            PdfPCell cell1243 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1243.Colspan = 1;
                            cell1243.BorderWidthLeft = 0f;
                            cell1243.BorderWidthRight = 1f;
                            cell1243.BorderWidthTop = .8f;
                            cell1243.BorderWidthBottom = 0f;

                            cell1243.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1243);




                            PdfPCell cell1248 = new PdfPCell(new Phrase("S.N0.", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1248.Colspan = 1;
                            cell1248.BorderWidthLeft = 1f;
                            cell1248.BorderWidthRight = .8f;
                            cell1248.BorderWidthTop = .8f;
                            cell1248.BorderWidthBottom = 0f;

                            cell1248.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1248);


                            PdfPCell cell1249 = new PdfPCell(new Phrase("Description & Specifications of Goods", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1249.Colspan = 2;
                            cell1249.BorderWidthLeft = 0f;
                            cell1249.BorderWidthRight = .8f;
                            cell1249.BorderWidthTop = .8f;
                            cell1249.BorderWidthBottom = 0f;

                            cell1249.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1249);

                            PdfPCell cell1250 = new PdfPCell(new Phrase("No. Of Pkg", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
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





                            PdfPCell cell1255 = new PdfPCell(new Phrase("HSRP NUMBER PLATE SET", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1255.Colspan = 3;
                            cell1255.BorderWidthLeft = 1f;
                            cell1255.BorderWidthRight = .8f;
                            cell1255.BorderWidthTop = .8f;
                            cell1255.BorderWidthBottom = 0f;

                            cell1255.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1255);

                            PdfPCell cell1256 = new PdfPCell(new Phrase("1", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1256.Colspan = 1;
                            cell1256.BorderWidthLeft = 0f;
                            cell1256.BorderWidthRight = .8f;
                            cell1256.BorderWidthTop = .8f;
                            cell1256.BorderWidthBottom = 0f;

                            cell1256.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1256);

                            int FQt = 0;
                            if (dataSetFillHSRPDeliveryChallan.Rows[0]["ISFrontPlateSize"].ToString() != "NULL" || dataSetFillHSRPDeliveryChallan.Rows[0]["ISFrontPlateSize"].ToString() != " ")
                            {

                                if (dataSetFillHSRPDeliveryChallan.Rows[0]["ISFrontPlateSize"].ToString() == "Y")
                                {
                                    FQt = 1;
                                }
                                else
                                {
                                    FQt = 0;
                                }
                            }

                            PdfPCell cell1257 = new PdfPCell(new Phrase(FQt.ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1257.Colspan = 1;
                            cell1257.BorderWidthLeft = 0f;
                            cell1257.BorderWidthRight = .8f;
                            cell1257.BorderWidthTop = .8f;
                            cell1257.BorderWidthBottom = 0f;

                            cell1257.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1257);

                            decimal amount4 = Convert.ToDecimal(dataSetFillHSRPDeliveryChallan.Rows[0]["TotalAmount"]);
                            decimal amount5 = Math.Round(amount4, 2);
                            PdfPCell cell1258 = new PdfPCell(new Phrase(amount5.ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1258.Colspan = 1;
                            cell1258.BorderWidthLeft = 0f;
                            cell1258.BorderWidthRight = .8f;
                            cell1258.BorderWidthTop = .8f;
                            cell1258.BorderWidthBottom = 0f;

                            cell1258.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1258);



                            PdfPCell cell1259 = new PdfPCell(new Phrase(amount5.ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1259.Colspan = 1;
                            cell1259.BorderWidthLeft = 0f;
                            cell1259.BorderWidthRight = 1f;
                            cell1259.BorderWidthTop = .8f;
                            cell1259.BorderWidthBottom = 0f;

                            cell1259.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1259);



                            PdfPCell cell1308 = new PdfPCell(new Phrase("Vehicle Reg No./Auth No :" + dataSetFillHSRPDeliveryChallan.Rows[0]["VehicleRegNo"].ToString() + "/" + dataSetFillHSRPDeliveryChallan.Rows[0]["HSRPRecord_AuthorizationNo"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1308.Colspan = 4;
                            cell1308.BorderWidthLeft = 1f;
                            cell1308.BorderWidthRight = .8f;
                            cell1308.BorderWidthTop = .8f;
                            cell1308.BorderWidthBottom = 0f;

                            cell1308.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1308);





                            PdfPCell cell1310 = new PdfPCell(new Phrase("Sub Total", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1310.Colspan = 1;
                            cell1310.BorderWidthLeft = 0f;
                            cell1310.BorderWidthRight = .8f;
                            cell1310.BorderWidthTop = .8f;
                            cell1310.BorderWidthBottom = 0f;

                            cell1310.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1310);

                            PdfPCell cell1311 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1311.Colspan = 1;
                            cell1311.BorderWidthLeft = 0f;
                            cell1311.BorderWidthRight = .8f;
                            cell1311.BorderWidthTop = .8f;
                            cell1311.BorderWidthBottom = 0f;

                            cell1311.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1311);

                            decimal amount = Convert.ToDecimal(dataSetFillHSRPDeliveryChallan.Rows[0]["TotalAmount"]);
                            decimal amount1 = Math.Round(amount, 2);


                            PdfPCell cell1312 = new PdfPCell(new Phrase(amount1.ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1312.Colspan = 1;
                            cell1312.BorderWidthLeft = 0f;
                            cell1312.BorderWidthRight = 1f;
                            cell1312.BorderWidthTop = .8f;
                            cell1312.BorderWidthBottom = 0f;

                            cell1312.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1312);





                            PdfPCell cell1313 = new PdfPCell(new Phrase("Vehicle Type : " + dataSetFillHSRPDeliveryChallan.Rows[0]["VehicleType"].ToString() + " Vehicle Class : " + dataSetFillHSRPDeliveryChallan.Rows[0]["VehicleClass"].ToString(), new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1313.Colspan = 4;
                            cell1313.BorderWidthLeft = 1f;
                            cell1313.BorderWidthRight = .8f;
                            cell1313.BorderWidthTop = .8f;
                            cell1313.BorderWidthBottom = 0f;

                            cell1313.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1313);




                            PdfPCell cell1315 = new PdfPCell(new Phrase("TAX ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1315.Colspan = 1;
                            cell1315.BorderWidthLeft = 0f;
                            cell1315.BorderWidthRight = .8f;
                            cell1315.BorderWidthTop = .8f;
                            cell1315.BorderWidthBottom = 0f;

                            cell1315.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1315);

                            PdfPCell cell1316 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1316.Colspan = 1;
                            cell1316.BorderWidthLeft = 0f;
                            cell1316.BorderWidthRight = .8f;
                            cell1316.BorderWidthTop = .8f;
                            cell1316.BorderWidthBottom = 0f;

                            cell1316.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1316);

                            PdfPCell cell1317 = new PdfPCell(new Phrase(dataSetFillHSRPDeliveryChallan.Rows[0]["VAT_Amount"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1317.Colspan = 1;
                            cell1317.BorderWidthLeft = 0f;
                            cell1317.BorderWidthRight = 1f;
                            cell1317.BorderWidthTop = .8f;
                            cell1317.BorderWidthBottom = 0f;

                            cell1317.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1317);

                            PdfPCell cell1318 = new PdfPCell(new Phrase("Front Laser Code : " + dataSetFillHSRPDeliveryChallan.Rows[0]["HSRP_Front_LaserCode"].ToString() + "  Rear Laser Code : " + dataSetFillHSRPDeliveryChallan.Rows[0]["HSRP_Rear_LaserCode"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1318.Colspan = 4;
                            cell1318.BorderWidthLeft = 1f;
                            cell1318.BorderWidthRight = .8f;
                            cell1318.BorderWidthTop = .8f;
                            cell1318.BorderWidthBottom = 0f;

                            cell1318.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1318);





                            PdfPCell cell1320 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1320.Colspan = 1;
                            cell1320.BorderWidthLeft = 0f;
                            cell1320.BorderWidthRight = .8f;
                            cell1320.BorderWidthTop = .8f;
                            cell1320.BorderWidthBottom = 0f;

                            cell1320.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1320);

                            PdfPCell cell1321 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1321.Colspan = 1;
                            cell1321.BorderWidthLeft = 0f;
                            cell1321.BorderWidthRight = .8f;
                            cell1321.BorderWidthTop = .8f;
                            cell1321.BorderWidthBottom = 0f;

                            cell1321.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1321);

                            PdfPCell cell1322 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1322.Colspan = 1;
                            cell1322.BorderWidthLeft = 0f;
                            cell1322.BorderWidthRight = 1f;
                            cell1322.BorderWidthTop = .8f;
                            cell1322.BorderWidthBottom = 0f;

                            cell1322.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1322);




                            PdfPCell cell1328 = new PdfPCell(new Phrase("Chassis No : " + dataSetFillHSRPDeliveryChallan.Rows[0]["ChassisNo"].ToString() + "  Engine No : " + dataSetFillHSRPDeliveryChallan.Rows[0]["EngineNo"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1328.Colspan = 4;
                            cell1328.BorderWidthLeft = 1f;
                            cell1328.BorderWidthRight = .8f;
                            cell1328.BorderWidthTop = .8f;
                            cell1328.BorderWidthBottom = 0f;

                            cell1328.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1328);





                            PdfPCell cell1330 = new PdfPCell(new Phrase("Sub total", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1330.Colspan = 1;
                            cell1330.BorderWidthLeft = 0f;
                            cell1330.BorderWidthRight = .8f;
                            cell1330.BorderWidthTop = .8f;
                            cell1330.BorderWidthBottom = 0f;

                            cell1330.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1330);



                            PdfPCell cell1331 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1331.Colspan = 1;
                            cell1331.BorderWidthLeft = 0f;
                            cell1331.BorderWidthRight = .8f;
                            cell1331.BorderWidthTop = .8f;
                            cell1331.BorderWidthBottom = 0f;

                            cell1331.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1331);

                            decimal vatPer = Convert.ToDecimal(dataSetFillHSRPDeliveryChallan.Rows[0]["VAT_Percentage"]);
                            decimal vat = Convert.ToDecimal(dataSetFillHSRPDeliveryChallan.Rows[0]["VAT_Amount"]);
                            decimal TotAmt = amount + vat;
                            decimal totalAmount = System.Decimal.Round(TotAmt, 2);
                            PdfPCell cell1332 = new PdfPCell(new Phrase(totalAmount.ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1332.Colspan = 1;
                            cell1332.BorderWidthLeft = 0f;
                            cell1332.BorderWidthRight = 1f;
                            cell1332.BorderWidthTop = .8f;
                            cell1332.BorderWidthBottom = 0f;

                            cell1332.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1332);





                            PdfPCell cell1338 = new PdfPCell(new Phrase("Terms", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1338.Colspan = 1;
                            cell1338.BorderWidthLeft = 1f;
                            cell1338.BorderWidthRight = .8f;
                            cell1338.BorderWidthTop = .8f;
                            cell1338.BorderWidthBottom = 0f;

                            cell1338.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1338);

                            PdfPCell cell1339 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1339.Colspan = 1;
                            cell1339.BorderWidthLeft = 0f;
                            cell1339.BorderWidthRight = .8f;
                            cell1339.BorderWidthTop = .8f;
                            cell1339.BorderWidthBottom = 0f;

                            cell1339.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1339);

                            PdfPCell cell1340 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1340.Colspan = 1;
                            cell1340.BorderWidthLeft = 0f;
                            cell1340.BorderWidthRight = .8f;
                            cell1340.BorderWidthTop = .8f;
                            cell1340.BorderWidthBottom = 0f;

                            cell1340.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1340);

                            PdfPCell cell1341 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1341.Colspan = 1;
                            cell1341.BorderWidthLeft = 0f;
                            cell1341.BorderWidthRight = .8f;
                            cell1341.BorderWidthTop = .8f;
                            cell1341.BorderWidthBottom = 0f;

                            cell1341.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1341);

                            PdfPCell cell1342 = new PdfPCell(new Phrase("Invoice Value Round of ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1342.Colspan = 2;
                            cell1342.BorderWidthLeft = 0f;
                            cell1342.BorderWidthRight = .8f;
                            cell1342.BorderWidthTop = .8f;
                            cell1342.BorderWidthBottom = 0f;

                            cell1342.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1342);


                            decimal totalAmount1 = System.Decimal.Round(TotAmt, 0);
                            PdfPCell cell1344 = new PdfPCell(new Phrase(totalAmount1.ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1344.Colspan = 1;
                            cell1344.BorderWidthLeft = 0f;
                            cell1344.BorderWidthRight = 1f;
                            cell1344.BorderWidthTop = .8f;
                            cell1344.BorderWidthBottom = 0f;

                            cell1344.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1344);




                            PdfPCell cell1345 = new PdfPCell(new Phrase("1:It is certified that the particulars given above are true and correct and amount indicated represents that prices actually charged and that there is no aditional inflow of any consideration directly or indirectly from the buyer", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1345.Colspan = 7;
                            cell1345.BorderWidthLeft = 1f;
                            cell1345.BorderWidthRight = .8f;
                            cell1345.BorderWidthTop = .8f;
                            cell1345.BorderWidthBottom = 0f;

                            cell1345.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1345);





                            PdfPCell cell1348 = new PdfPCell(new Phrase("2: All disputes arising out of it will be subject to Delhi jurisdiction only", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1348.Colspan = 7;
                            cell1348.BorderWidthLeft = 1f;
                            cell1348.BorderWidthRight = .8f;
                            cell1348.BorderWidthTop = .8f;
                            cell1348.BorderWidthBottom = 0f;

                            cell1348.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1348);




                            PdfPCell cell1353 = new PdfPCell(new Phrase("3: Interest @24% will be charged on all account remaining unpaid after 30 days/due as agreed", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1353.Colspan = 4;
                            cell1353.BorderWidthLeft = 1f;
                            cell1353.BorderWidthRight = .8f;
                            cell1353.BorderWidthTop = .8f;
                            cell1353.BorderWidthBottom = 0f;

                            cell1353.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1353);



                            PdfPCell cell1358 = new PdfPCell(new Phrase(GetAddress.Rows[0]["CompanyName"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1358.Colspan = 3;
                            cell1358.BorderWidthLeft = 0f;
                            cell1358.BorderWidthRight = 1f;
                            cell1358.BorderWidthTop = .8f;
                            cell1358.BorderWidthBottom = 0f;

                            cell1358.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1358);





                            PdfPCell cell1356 = new PdfPCell(new Phrase("4:Goods once sold & accepted will not be taken back", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1356.Colspan = 4;
                            cell1356.BorderWidthLeft = 1f;
                            cell1356.BorderWidthRight = .8f;
                            cell1356.BorderWidthTop = .8f;
                            cell1356.BorderWidthBottom = 0f;

                            cell1356.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1356);


                            PdfPCell cell1361 = new PdfPCell(new Phrase("(AUTH. SIGN.)", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            //cell1361.PaddingRight = 100f;
                            cell1361.Colspan = 3;
                            cell1361.PaddingRight = 0f;
                            cell1361.BorderWidthLeft = 0f;
                            cell1361.BorderWidthRight = 1f;
                            cell1361.BorderWidthTop = 0f;
                            cell1361.BorderWidthBottom = 0f;

                            cell1361.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1361);



                            PdfPCell cell1359 = new PdfPCell(new Phrase("5:Any discrepancy in quality & quantity should be reported within 24 hrs , of receipt of goods", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1359.Colspan = 7;
                            cell1359.BorderWidthLeft = 1f;
                            cell1359.BorderWidthRight = .8f;
                            cell1359.BorderWidthTop = .8f;
                            cell1359.BorderWidthBottom = 0f;

                            cell1359.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1359);





                            PdfPCell cell1362 = new PdfPCell(new Phrase("We hereby certify that goods recived are as per order & requirement and we abide to above said terms", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1362.Colspan = 7;
                            cell1362.BorderWidthLeft = 1f;
                            cell1362.BorderWidthRight = .8f;
                            cell1362.BorderWidthTop = .8f;
                            cell1362.BorderWidthBottom = 1f;

                            cell1362.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1362);





                            PdfPCell cell1366 = new PdfPCell(new Phrase("CUSTOMER'S NAME : " + dataSetFillHSRPDeliveryChallan.Rows[0]["OwnerName"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
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




                            PdfPCell cell1369 = new PdfPCell(new Phrase("VEHICLE No : " + dataSetFillHSRPDeliveryChallan.Rows[0]["VehicleRegNo"].ToString() + " Date :", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1369.Colspan = 7;
                            cell1369.PaddingRight = 70f;
                            cell1369.BorderWidthLeft = 0f;
                            cell1369.BorderWidthRight = 1f;
                            cell1369.BorderWidthBottom = 1f;
                            cell1369.BorderColor = BaseColor.WHITE;
                            cell1369.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1369);



                            document.Add(table1);
                            document.Add(table);

                            document.Add(table2);
                            document.Add(table);

                            //document.Add(table);

                            document.NewPage();

                            //ReadFile(PdfFolder);

                        }
                   
                
            }

            try
            {

                document.Close();


                context.Response.ContentType = "Application/pdf";
                context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                context.Response.WriteFile(PdfFolder);
                context.Response.End();
            }
            catch (Exception)
            {

            }
        }
        protected void txtVehicleNo_TextChanged(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtInvoiceNo.Text) && (dropdownDuplicateFIle.SelectedIndex <= 0))
            {
                Response.Write("<script> alert('Please Enter Invoice No.')</script>");
                return;
            }
            GetRecords();
            string chkb = string.Empty;

                       float Amount = 0;
            string filename1 = "Order_Open_Records" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".pdf";
            String StringField1 = String.Empty;
            String StringAlert1 = String.Empty;
            StringBuilder bb1 = new StringBuilder();
            // Document document = new Document();
            //  iTextSharp.text.Document document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 0, 0, 0, 0);
            Document document1 = new Document(new iTextSharp.text.Rectangle(188f, 124f), -30, -30, 8, 0);
            document1.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
            BaseFont bfTimes1 = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
            // iTextSharp.text.Font times = new Font(bfTimes, 12, Font.ITALIC, Color.Black);
            //Creates a Writer that listens to this document and writes the document to the Stream of your choice:
            string PdfFolder1 = ConfigurationManager.AppSettings["PdfFolder"].ToString() + filename1;
            PdfWriter.GetInstance(document1, new FileStream(PdfFolder1, FileMode.Create));
            //Opens the document:
            document1.Open();

            PdfPTable table = new PdfPTable(6);
            table.TotalWidth = 6900f;

            GenerateCell(table, 6, 0, 0, 0, 0, 1, 0, "HSRP Order Booking Report", 0, 0);

            GenerateCell(table, 1, 1, 1, 1, 0, 0, 1, "SN", 0, 0);
            GenerateCell(table, 1, 1, 1, 1, 0, 0, 1, "Vehicle No", 0, 0);

            GenerateCell(table, 1, 0, 1, 1, 0, 0, 1, "Vehicle Type", 0, 0);
            GenerateCell(table, 1, 0, 1, 1, 0, 0, 1, "Collection Date", 0, 0);
            GenerateCell(table, 1, 0, 1, 1, 0, 0, 1, "Embossing Date", 0, 0);
            GenerateCell(table, 1, 0, 1, 1, 0, 0, 1, "Invoice No", 0, 0);

            foreach (DataRow di in dtInvoiceData.Rows)
            {
                List<string> lblvehicleRegNo = new List<string>();
                ArrayList VehicleRegNo = new ArrayList();
                ArrayList LaserFront = new ArrayList();
                ArrayList LaserRear = new ArrayList();

                string strSN = di["SN"].ToString();
                string lblVehicleRegNo = di["VehicleRegNo"].ToString();
                string strVehType = di["VehicleType"].ToString();
                string strCollectionDate = di["HSRPRecord_CreationDate"].ToString();
                string strEmbDate = di["OrderEmbossingDate"].ToString();
                string strInvoiceNo = di["ChallanNo"].ToString();

                string OrderStatus = di["OrderStatus"].ToString();
                string strRecordId = di["hsrprecordid"].ToString();
                string flaser = di["HSRP_Front_LaserCode"].ToString();
                string rlaser = di["hsrp_rear_lasercode"].ToString();

                if (di["VehicleRegNo"].ToString().Equals(dtInvoiceData.Rows[dtInvoiceData.Rows.Count - 1]["VehicleRegNo"].ToString()))
                {
                    GenerateCell(table, 1, 1, 1, 1, 1, 0, 1, strSN, 0, 0);
                GenerateCell(table, 1, 1, 1, 1, 1, 0, 1, lblVehicleRegNo, 0, 0);

                GenerateCell(table, 1, 0, 1, 1, 1, 0, 1, strVehType, 0, 0);
                GenerateCell(table, 1, 0, 1, 1, 1, 0, 1, strCollectionDate, 0, 0);
                GenerateCell(table, 1, 0, 1, 1, 1, 0, 1, strEmbDate, 0, 0);
                
                    GenerateCell(table, 1, 0, 1, 1, 1, 0, 1, strInvoiceNo, 0, 0);
                }
                else
                {
                    GenerateCell(table, 1, 1, 1, 1, 0, 0, 1, strSN, 0, 0);
                    GenerateCell(table, 1, 1, 1, 1, 0, 0, 1, lblVehicleRegNo, 0, 0);

                    GenerateCell(table, 1, 0, 1, 1, 0, 0, 1, strVehType, 0, 0);
                    GenerateCell(table, 1, 0, 1, 1, 0, 0, 1, strCollectionDate, 0, 0);
                    GenerateCell(table, 1, 0, 1, 1, 0, 0, 1, strEmbDate, 0, 0);

                    GenerateCell(table, 1, 0, 1, 1, 0, 0, 1, strInvoiceNo, 0, 0);
                }
               
            }
            document1.Add(table);
            document1.Close();
            HttpContext context1 = HttpContext.Current;
            context1.Response.ContentType = "Application/pdf";
            context1.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename1);
            context1.Response.WriteFile(PdfFolder1);
            context1.Response.End();
        }

        protected void gvRecords_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvRecords.EditIndex = e.NewPageIndex;
            BindGrid();
        }
        }
    }
                