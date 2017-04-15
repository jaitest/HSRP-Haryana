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
    public partial class APTSDownloadProductionWithDate : System.Web.UI.Page
    {
        
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        string UserType = string.Empty;
        string HSRPStateID = string.Empty;
        string RTOLocationID = string.Empty;
        int intHSRPStateID;
       
        string StrSqlQuery = string.Empty;
        string strRunningNo = string.Empty;
        string strEmbId=string.Empty;
        string AllLocation = string.Empty;
        string OrderStatus = string.Empty;
      
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
               // RTOLocationID = Session["UserRTOLocationID"].ToString();
                UserType = Session["UserType"].ToString();
                HSRPStateID = Session["UserHSRPStateID"].ToString();

                lblErrMsg.Text = string.Empty;
                strUserID = Session["UID"].ToString();
                ComputerIP = Request.UserHostAddress;

                if (!IsPostBack)
                {
                    InitialSetting();
                    if (HSRPStateID == "9" || HSRPStateID=="11")
                    {
                        ddlBothDealerHHT.Visible = false;
                        labelSelectType.Visible = false;
                    }
                    else if (HSRPStateID == "4")
                    {
                        ddlBothDealerHHT.Visible = true;
                        labelSelectType.Visible = true;
                        DropDownList1.Visible = true;
                        labelvehicletype.Visible = true;
                    }
                    else if (HSRPStateID == "2")
                    {
                        ddlBothDealerHHT.Visible = true;
                        labelSelectType.Visible = true;
                        DropDownList1.Visible = true;
                        labelvehicletype.Visible = true;
                    }
                    else
                    {
                        DropDownList1.Visible = false;
                        labelvehicletype.Visible = false;
                    }
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
                Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), CnnString, "--ALL--");

                dataLabellbl.Visible = false;
                TRRTOHide.Visible = false;
            }
            else
            {
                string UserID = Convert.ToString(Session["UID"]);

                SQLString = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, (a.RTOLocationCode+', ') as RTOCode, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID where UserRTOLocationMapping.UserID='" + UserID + "' order by RTOLocationName ";
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
                        RTOCode += dss.Rows[i]["RTOCode"].ToString() + ",";

                    }
                    lblRTOCode.Text = RTOCode.Remove(RTOCode.LastIndexOf(","));

                }
            }
        }
        protected void dropDownListClient_SelectedIndexChanged(object sender, EventArgs e)
        {

            //if (DropDownListStateName.SelectedItem.Text != "--Select Client--")
            //{
            //    ShowGrid();
            //}
            //else
            //{
            //    Grid1.Items.Clear();
            //    lblErrMsg.Text = String.Empty;
            //    lblErrMsg.Text = "Please Select Client.";
            //    return;
            //}
        }
        #endregion

      

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



          private DataTable GetRtoLocation()
        {
            string sql = "select rtolocationname,rtolocationid from rtolocation where rtolocationid in (select distinct distrelation from rtolocation where hsrp_stateid='" + DropDownListStateName.SelectedValue + "') and RTOLocationID not in (148,331)";
            DataTable dtrto = Utils.GetDataTable(sql, CnnString);
            return dtrto;
        }

        string RTOCode = string.Empty;
            string RTOName = string.Empty;

        protected void btnDownloadRecords_Click(object sender, EventArgs e)
        {
            
             DataTable dtrto = GetRtoLocation();
             for (int iRowNo = 0; iRowNo < dtrto.Rows.Count; iRowNo++)
             {
                // RTOName = dtrto.Rows[iRowNo]["rtolocationname"].ToString();
               //  RTOCode = dtrto.Rows[iRowNo]["RTOLocationid"].ToString();

                 string strStateID = DropDownListStateName.SelectedValue.ToString();
                 string RTOLocationID = dropDownListClient.SelectedValue.ToString();
                 string strOrderTypeHHTDealer = ddlBothDealerHHT.SelectedItem.Text.ToString();
                 string ddVehicleType = DropDownList1.SelectedItem.ToString().ToUpper();
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




                 string strSel = "select isnull(max(right(newProductionSheetRunningNo,7)),0000000) from EmbossingCenters where State_Id='" + DropDownListStateName.SelectedValue.ToString() + "' and Emb_Center_Id='" + strEmbId + "'";
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


                 if (ddVehicleType == "ALL")
                 {
                     strVehicleType = string.Empty;
                 }

                 if (ddVehicleType == "TWO WHEELER")
                 {
                     strVehicleType = " and vehicletype in ('MOTOR CYCLE','SCOOTER') ";
                 }
                 if (ddVehicleType == "THREE WHEELER")
                 {
                     strVehicleType = " and vehicletype in ('THREE WHEELER') ";
                 }
                 if (ddVehicleType == "FOUR WHEELER")
                 {
                     strVehicleType = " and vehicletype in ('LMV','LMV(CLASS)','MCV/HCV/TRAILERS','TRACTOR') ";
                 }

                 RTOName = dtrto.Rows[iRowNo]["rtolocationname"].ToString();
                 RTOCode = dtrto.Rows[iRowNo]["RTOLocationid"].ToString();
                

       //// Change Date 1.09.2015
                  strSQL.Append("Select a.hsrprecordID,a.roundoff_netamount,a.OrderStatus, a.HSRPRecord_AuthorizationNo,convert(varchar, OrderClosedDate, 105) as OrderClosedDate, CONVERT(varchar(20),orderdate ,103) AS OrderBookDate,CONVERT(varchar(20),OrderEmbossingDate ,103) AS OrderEmbossingDate, a.dealerid as ID, left(a.OwnerName,19) as OwnerName, a.MobileNo,(select  AffixCenterDesc from AffixationCenters where Affix_id= a.affix_id ) as AffixCenterDesc, CONVERT(varchar(20), HSRPRecord_AuthorizationDate,103) AS OrderDateAuth, a.OrderDate, a.EngineNo, a.ChassisNo, (select rtolocationname from rtolocation where  rtolocationid =a.rtolocationid) as RTOLocationName,  a.VehicleRegNo,  case a.VehicleType when 'MCV/HCV/TRAILERS' then 'Trailers' when 'THREE WHEELER' then 'T.Whe.' when 'SCOOTER' then 'SCOO' when 'TRACTOR' then 'TRAC' when 'LMV(CLASS)' then 'L.CL' when 'LMV' then 'LMV'  when 'MOTOR CYCLE' then 'MO.C' end as VehicleType, case a.VehicleClass when 'Transport' then 'T' else 'N.T.' end as VehicleClass,  a.HSRP_StateID, (select HSRPStateName from hsrpstate where HSRP_StateID=a.HSRP_StateID)  as HSRPStateName, (select replace(ProductCode,'MM-','') from Product where productid= a.RearPlateSize) AS RearProductCode,(select replace(ProductCode,'MM-','') from Product where productid= a.FrontPlateSize) AS FrontProductCode,a.FrontPlateSize, a.HSRP_Front_LaserCode, a.HSRP_Rear_LaserCode, a.RearPlateSize FROM HSRPRecords AS a   where  a.hsrp_StateID='" + strStateID + "' and a.RTOLocationID='" + RTOLocationID + "' and convert(date,erpassigndate) ='" + AuthorizationDate + "' and ([HSRP_Front_LaserCode] is not null or [HSRP_Rear_LaserCode] is not null) and isnull(newpdfrunningno,'')=''  and ([HSRP_Front_LaserCode] !='' or [HSRP_Rear_LaserCode] !='') and isnull(rejectflag,'N')='N' ");
                 ////
                 // strSQL.Append("Select a.hsrprecordID,a.roundoff_netamount,a.OrderStatus, a.HSRPRecord_AuthorizationNo,convert(varchar, OrderClosedDate, 105) as OrderClosedDate, CONVERT(varchar(20),orderdate ,103) AS OrderBookDate,CONVERT(varchar(20),OrderEmbossingDate ,103) AS OrderEmbossingDate, a.dealerid as ID, left(a.OwnerName,19) as OwnerName, a.MobileNo,(select  AffixCenterDesc from AffixationCenters where Affix_id= a.affix_id ) as AffixCenterDesc, CONVERT(varchar(20), HSRPRecord_AuthorizationDate,103) AS OrderDateAuth, a.OrderDate, a.EngineNo, a.ChassisNo, (select rtolocationname from rtolocation where  rtolocationid =a.rtolocationid) as RTOLocationName,  a.VehicleRegNo,  case a.VehicleType when 'MCV/HCV/TRAILERS' then 'Trailers' when 'THREE WHEELER' then 'T.Whe.' when 'SCOOTER' then 'SCOO' when 'TRACTOR' then 'TRAC' when 'LMV(CLASS)' then 'L.CL' when 'LMV' then 'LMV'  when 'MOTOR CYCLE' then 'MO.C' end as VehicleType, case a.VehicleClass when 'Transport' then 'T' else 'N.T.' end as VehicleClass,  a.HSRP_StateID, (select HSRPStateName from hsrpstate where HSRP_StateID=a.HSRP_StateID)  as HSRPStateName, (select replace(ProductCode,'MM-','') from Product where productid= a.RearPlateSize) AS RearProductCode,(select replace(ProductCode,'MM-','') from Product where productid= a.FrontPlateSize) AS FrontProductCode,a.FrontPlateSize, a.HSRP_Front_LaserCode, a.HSRP_Rear_LaserCode, a.RearPlateSize FROM HSRPRecords AS a   where  a.hsrp_StateID='" + strStateID + "' and a.RTOLocationID='" + RTOLocationID + "' and convert(date,hsrprecord_creationdate) between '2014-12-31 00:00:00' and '2015-02-19 23:59:59' and orderstatus='New Order' and ([HSRP_Front_LaserCode] is not null or [HSRP_Rear_LaserCode] is not null)   and ([HSRP_Front_LaserCode] !='' or [HSRP_Rear_LaserCode] !='') ");

                 if (strOrderTypeHHTDealer == "Both")
                 {
                     strSQL.Append(" ");
                 }
                 else if (ddlBothDealerHHT.SelectedItem.Text == "Dealer Data")
                 {

                     strSQL.Append(" AND a.Addrecordby like'%dealer%' ");
                 }

                 else if (ddlBothDealerHHT.SelectedItem.Text == "Other")
                 {
                     strSQL.Append(" AND a.Addrecordby not like'%dealer%' ");
                 }

                 if (strVehicleType == "")
                 {
                     strSQL.Append(" order by a.VehicleClass,a.VehicleType,a.HSRP_Front_LaserCode,a.hsrp_rear_lasercode");
                 }
                 else
                 {
                     strSQL.Append(strVehicleType + " order by a.VehicleClass,a.VehicleType,a.HSRP_Front_LaserCode,a.hsrp_rear_lasercode");
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

                     //table = new PdfPTable(14);
                     //var colWidthPercentages = new[] { 55f, 125f, 55f, 200f, 75f, 165f, 165f, 155f, 110f, 210f, 105f, 210f, 105f, 125f };//7,14

                     table = new PdfPTable(11);
                     var colWidthPercentages = new[] { 55f, 125f, 55f, 200f, 75f,  155f, 110f, 210f, 105f, 210f, 105f };//7,14

                    // var colWidthPercentages = new[] { 55f, 125f, 55f, 200f, 75f, 165f,  110f, 210f, 105f, 210f, 105f, 125f };//7,14
                     var colheightpercentage = new[] { 2f };

                     table.SetWidths(colWidthPercentages);


                     string strQueryDate = "SELECT CONVERT(VARCHAR,GETDATE(),103)";
                     DataTable dtDate = Utils.GetDataTable(strQueryDate, CnnString);

                     table.TotalWidth = 800f;
                     table.LockedWidth = true;



                     PdfPCell cell786a = new PdfPCell(new Phrase("Production Sheet No:  " + strPRFIXCom + strRunningNo, new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
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

                     PdfPCell cell12093 = new PdfPCell(new Phrase("Report Date " + ReportDateFrom, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
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

                     //PdfPCell cell1206 = new PdfPCell(new Phrase("Chassis No", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                     //cell1206.Colspan = 1;
                     //cell1206.BorderWidthLeft = 0.8f;
                     //cell1206.BorderWidthRight = .8f;
                     //cell1206.BorderWidthTop = 0.8f;
                     //cell1206.BorderWidthBottom = 0.8f;
                     //cell1206.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right

                     //table.AddCell(cell1206);

                     //PdfPCell cell1221 = new PdfPCell(new Phrase("EngineNo", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                     //cell1221.Colspan = 1;
                     //cell1221.BorderWidthLeft = 0.8f;
                     //cell1221.BorderWidthRight = 0.8f;
                     //cell1221.BorderWidthTop = .8f;
                     //cell1221.BorderWidthBottom = 0.8f;

                     //cell1221.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                     //table.AddCell(cell1221);

                     if (HSRPStateID == "4" && ddlBothDealerHHT.SelectedItem.Text == "Dealer Data")
                     {
                         PdfPCell cell120000 = new PdfPCell(new Phrase("ID", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                         cell120000.Colspan = 1;
                         cell120000.BorderWidthLeft = 0f;
                         cell120000.BorderWidthRight = 0.8f;
                         cell120000.BorderWidthTop = 0.8f;
                         cell120000.BorderWidthBottom = 0f;

                         cell120000.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                         table.AddCell(cell120000);
                     }
                     else if (HSRPStateID.Equals("5") && ddlBothDealerHHT.SelectedItem.Text.Equals("Dealer Data"))
                     {
                         PdfPCell cell111 = new PdfPCell(new Phrase("ID", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                         cell111.Colspan = 1;
                         cell111.BorderWidthLeft = 0f;
                         cell111.BorderWidthRight = 0.8f;
                         cell111.BorderWidthTop = 0.8f;
                         cell111.BorderWidthBottom = 0f;

                         cell111.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                         table.AddCell(cell111);


                     }
                     else if (HSRPStateID == "1" || HSRPStateID == "2" || HSRPStateID == "3" || HSRPStateID == "6" || HSRPStateID == "9" || HSRPStateID == "11" || (HSRPStateID == "4" && ddlBothDealerHHT.SelectedItem.Text.Equals("Both") || ddlBothDealerHHT.SelectedItem.Text.Equals("Other")))
                     {
                         //PdfPCell cell120933 = new PdfPCell(new Phrase("Owner Name", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                         //cell120933.Colspan = 1;
                         //cell120933.BorderWidthLeft = 0f;
                         //cell120933.BorderWidthRight = 0.8f;
                         //cell120933.BorderWidthTop = 0.8f;
                         //cell120933.BorderWidthBottom = 0f;

                         //cell120933.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                         //table.AddCell(cell120933);
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



                             //PdfPCell cell1206a = new PdfPCell(new Phrase("Chassis No", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                             //cell1206a.Colspan = 1;
                             //cell1206a.BorderWidthLeft = 0f;
                             //cell1206a.BorderWidthRight = .8f;
                             //cell1206a.BorderWidthTop = 0.8f;
                             //cell1206a.BorderWidthBottom = 0.8f;
                             //cell1206a.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                             //table.AddCell(cell1206a);

                             //PdfPCell cell1221a = new PdfPCell(new Phrase("EngineNo", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                             //cell1221a.Colspan = 1;
                             //cell1221a.BorderWidthLeft = 0f;
                             //cell1221a.BorderWidthRight = 0.8f;
                             //cell1221a.BorderWidthTop = 0.8f;
                             //cell1221a.BorderWidthBottom = 0.8f;
                             //cell1221a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                             //table.AddCell(cell1221a);


                             if (HSRPStateID == "4" && ddlBothDealerHHT.SelectedItem.Text == "Dealer Data")
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
                             else if (HSRPStateID.Equals("5") && ddlBothDealerHHT.SelectedItem.Text == "Dealer Data")
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

                                 //PdfPCell cell120933a = new PdfPCell(new Phrase("Owner Name", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                                 //cell120933a.Colspan = 1;
                                 //cell120933a.BorderWidthLeft = 0f;
                                 //cell120933a.BorderWidthRight = 0.8f;
                                 //cell120933a.BorderWidthTop = 0.8f;
                                 //cell120933a.BorderWidthBottom = 0f;
                                 //cell120933a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                 //table.AddCell(cell120933a);
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

                         //PdfPCell cell1216 = new PdfPCell(new Phrase(dt.Rows[i]["ChassisNo"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                         //cell1216.Colspan = 1;
                         //cell1216.MinimumHeight = 0f;//25

                         //cell1216.BorderWidthLeft = 0f;
                         //cell1216.BorderWidthRight = .8f;
                         //cell1216.BorderWidthTop = 0f;
                         //cell1216.BorderWidthBottom = .5f;

                         //cell1216.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right

                         //table.AddCell(cell1216);

                         //PdfPCell cell1222 = new PdfPCell(new Phrase(dt.Rows[i]["EngineNo"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                         //cell1222.Colspan = 1;
                         //cell1222.MinimumHeight = 0f;//25

                         //cell1222.BorderWidthLeft = 0f;
                         //cell1222.BorderWidthRight = .8f;
                         //cell1222.BorderWidthTop = 0f;
                         //cell1222.BorderWidthBottom = .5f;

                         //cell1222.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                         //table.AddCell(cell1222);


                         if (HSRPStateID == "2" && ddlBothDealerHHT.SelectedItem.Text == "Dealer Data")
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
                         else if (HSRPStateID == "4" && ddlBothDealerHHT.SelectedItem.Text == "Dealer Data")
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
                         else if (HSRPStateID.Equals("5") && ddlBothDealerHHT.SelectedItem.Text == "Dealer Data")
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
                             //PdfPCell cell1209315 = new PdfPCell(new Phrase(dt.Rows[i]["OwnerName"].ToString() + "/ " + dt.Rows[i]["MobileNo"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                             //cell1209315.Colspan = 1;
                             //cell1209315.MinimumHeight = 0f;//25

                             //cell1209315.BorderWidthLeft = 0f;
                             //cell1209315.BorderWidthRight = .8f;
                             //cell1209315.BorderWidthTop = .5f;
                             //cell1209315.BorderWidthBottom = .5f;

                             //cell1209315.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                             //table.AddCell(cell1209315);

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

                         int count = 1;
                         try
                         {
                             StringBuilder sb = new StringBuilder();
                             for (int i = 0; i <= dt.Rows.Count - 1; i++)
                             {

                                 sb.Append("update hsrprecords set sendtoProductionStatus='Y', NAVPDFFlag='1', NewPdfRunningNo='" + strPRFIXCom + strRunningNo + "', PdfDownloadDate=GetDate(), pdfFileName='" + filename + "', PDFDownloadUserID='" + strUserID + "' where hsrprecordID='" + dt.Rows[i]["hsrprecordID"].ToString() + "';");
                                 count = count + 1;

                             }
                             Utils.ExecNonQuery(sb.ToString(), CnnString);
                             sb.Clear();
                             StrSqlQuery = "update EmbossingCenters set NewProductionSheetRunningNo='" + strRunningNo + "' where State_Id='" + DropDownListStateName.SelectedValue.ToString() + "' and Emb_Center_Id='" + strEmbId + "'";
                             Utils.ExecNonQuery(StrSqlQuery, CnnString);
                         }
                         catch (Exception ex)
                         {

                         }


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
           
        }



        protected void btnGO_Click(object sender, EventArgs e)
        {
            labelOrderStatus.Visible = true;
            dropdownDuplicateFIle.Visible = true;

            string type = "1";
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
            string ToDate = From1 + " 23:59:59";

            SQLString = "select  Distinct pdffilename   from hsrprecords  where hsrp_StateID='" + DropDownListStateName.SelectedValue + "' and rtolocationID='" + dropDownListClient.SelectedValue + "' and  pdffilename is not null and hsrprecord_CreationDate between '" + AuthorizationDate + "' and  '" + ToDate + "' and addrecordby !='Dealer'  group by  pdffilename, convert(varchar,pdfdownloaddate,111)";
            Utils.PopulateDropDownList(dropdownDuplicateFIle, SQLString.ToString(), CnnString, "--Select File Name--");

        }

        protected void btnAllpdf_Click(object sender, EventArgs e)
        {
            if (dropDownListClient.SelectedItem.Text == "--ALL--")
            {

                DataTable dtrto = GetRtoLocation();
                for (int iRowNo = 0; iRowNo < dtrto.Rows.Count; iRowNo++)
                {
                    RTOName = dtrto.Rows[iRowNo]["rtolocationname"].ToString();
                    RTOCode = dtrto.Rows[iRowNo]["RTOLocationid"].ToString();

                    string strStateID = DropDownListStateName.SelectedValue.ToString();
                    string RTOLocationID = dropDownListClient.SelectedValue.ToString();
                    string strOrderTypeHHTDealer = ddlBothDealerHHT.SelectedItem.Text.ToString();
                    string ddVehicleType = DropDownList1.SelectedItem.ToString().ToUpper();
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

                    StrSqlQuery = "select NAVEMBID from rtolocation where hsrp_stateid='" + strStateID + "' and RtolocationId='" + RTOCode + "'";
                    strEmbId = Utils.getScalarValue(StrSqlQuery, CnnString);




                    string strSel = "select isnull(max(right(newProductionSheetRunningNo,7)),0000000) from EmbossingCenters where State_Id='" + DropDownListStateName.SelectedValue.ToString() + "' and Emb_Center_Id='" + strEmbId + "'";
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


                    if (ddVehicleType == "ALL")
                    {
                        strVehicleType = string.Empty;
                    }

                    if (ddVehicleType == "TWO WHEELER")
                    {
                        strVehicleType = " and vehicletype in ('MOTOR CYCLE','SCOOTER') ";
                    }
                    if (ddVehicleType == "THREE WHEELER")
                    {
                        strVehicleType = " and vehicletype in ('THREE WHEELER') ";
                    }
                    if (ddVehicleType == "FOUR WHEELER")
                    {
                        strVehicleType = " and vehicletype in ('LMV','LMV(CLASS)','MCV/HCV/TRAILERS','TRACTOR') ";
                    }


                    strSQL.Append("Select a.hsrprecordID,a.roundoff_netamount,a.OrderStatus, a.HSRPRecord_AuthorizationNo,convert(varchar, OrderClosedDate, 105) as OrderClosedDate, CONVERT(varchar(20),orderdate ,103) AS OrderBookDate,CONVERT(varchar(20),OrderEmbossingDate ,103) AS OrderEmbossingDate, a.dealerid as ID, left(a.OwnerName,19) as OwnerName, a.MobileNo,(select  AffixCenterDesc from AffixationCenters where Affix_id= a.affix_id ) as AffixCenterDesc, CONVERT(varchar(20), HSRPRecord_AuthorizationDate,103) AS OrderDateAuth, a.OrderDate, a.EngineNo, a.ChassisNo, (select rtolocationname from rtolocation where  rtolocationid =a.rtolocationid) as RTOLocationName,  a.VehicleRegNo,  case a.VehicleType when 'MCV/HCV/TRAILERS' then 'Trailers' when 'THREE WHEELER' then 'T.Whe.' when 'SCOOTER' then 'SCOO' when 'TRACTOR' then 'TRAC' when 'LMV(CLASS)' then 'L.CL' when 'LMV' then 'LMV'  when 'MOTOR CYCLE' then 'MO.C' end as VehicleType, case a.VehicleClass when 'Transport' then 'T' else 'N.T.' end as VehicleClass,  a.HSRP_StateID, (select HSRPStateName from hsrpstate where HSRP_StateID=a.HSRP_StateID)  as HSRPStateName, (select replace(ProductCode,'MM-','') from Product where productid= a.RearPlateSize) AS RearProductCode,(select replace(ProductCode,'MM-','') from Product where productid= a.FrontPlateSize) AS FrontProductCode,a.FrontPlateSize, a.HSRP_Front_LaserCode, a.HSRP_Rear_LaserCode, a.RearPlateSize FROM HSRPRecords AS a   where  a.hsrp_StateID='" + strStateID + "' and a.RTOLocationID='" + RTOCode + "' and convert(date,erpassigndate) ='" + AuthorizationDate + "' and isnull(newpdfrunningno,'')='' and ([HSRP_Front_LaserCode] is not null or [HSRP_Rear_LaserCode] is not null)  and ([HSRP_Front_LaserCode] !='' or [HSRP_Rear_LaserCode] !='') ");

                    // strSQL.Append("Select a.hsrprecordID,a.roundoff_netamount,a.OrderStatus, a.HSRPRecord_AuthorizationNo,convert(varchar, OrderClosedDate, 105) as OrderClosedDate, CONVERT(varchar(20),orderdate ,103) AS OrderBookDate,CONVERT(varchar(20),OrderEmbossingDate ,103) AS OrderEmbossingDate, a.dealerid as ID, left(a.OwnerName,19) as OwnerName, a.MobileNo,(select  AffixCenterDesc from AffixationCenters where Affix_id= a.affix_id ) as AffixCenterDesc, CONVERT(varchar(20), HSRPRecord_AuthorizationDate,103) AS OrderDateAuth, a.OrderDate, a.EngineNo, a.ChassisNo, (select rtolocationname from rtolocation where  rtolocationid =a.rtolocationid) as RTOLocationName,  a.VehicleRegNo,  case a.VehicleType when 'MCV/HCV/TRAILERS' then 'Trailers' when 'THREE WHEELER' then 'T.Whe.' when 'SCOOTER' then 'SCOO' when 'TRACTOR' then 'TRAC' when 'LMV(CLASS)' then 'L.CL' when 'LMV' then 'LMV'  when 'MOTOR CYCLE' then 'MO.C' end as VehicleType, case a.VehicleClass when 'Transport' then 'T' else 'N.T.' end as VehicleClass,  a.HSRP_StateID, (select HSRPStateName from hsrpstate where HSRP_StateID=a.HSRP_StateID)  as HSRPStateName, (select replace(ProductCode,'MM-','') from Product where productid= a.RearPlateSize) AS RearProductCode,(select replace(ProductCode,'MM-','') from Product where productid= a.FrontPlateSize) AS FrontProductCode,a.FrontPlateSize, a.HSRP_Front_LaserCode, a.HSRP_Rear_LaserCode, a.RearPlateSize FROM HSRPRecords AS a   where  a.hsrp_StateID='" + strStateID + "' and a.RTOLocationID='" + RTOLocationID + "' and convert(date,hsrprecord_creationdate) between '2014-12-31 00:00:00' and '2015-02-19 23:59:59' and orderstatus='New Order' and ([HSRP_Front_LaserCode] is not null or [HSRP_Rear_LaserCode] is not null)   and ([HSRP_Front_LaserCode] !='' or [HSRP_Rear_LaserCode] !='') ");

                    if (strOrderTypeHHTDealer == "Both")
                    {
                        strSQL.Append(" ");
                    }
                    else if (ddlBothDealerHHT.SelectedItem.Text == "Dealer Data")
                    {

                        strSQL.Append(" AND a.Addrecordby like'%dealer%' ");
                    }

                    else if (ddlBothDealerHHT.SelectedItem.Text == "Other")
                    {
                        strSQL.Append(" AND a.Addrecordby not like'%dealer%' ");
                    }

                    if (strVehicleType == "")
                    {
                        strSQL.Append(" order by a.VehicleClass,a.VehicleType,a.HSRP_Front_LaserCode,a.hsrp_rear_lasercode");
                    }
                    else
                    {
                        strSQL.Append(strVehicleType + " order by a.VehicleClass,a.VehicleType,a.HSRP_Front_LaserCode,a.hsrp_rear_lasercode");
                    }


                    DataTable dt = Utils.GetDataTable(strSQL.ToString(), CnnString);
                    if (dt.Rows.Count > 0)
                    {
                        float Amount = 0;
                        string filename = "HSRPProductionSheet- " + RTOName + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".pdf";
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



                        PdfPCell cell786a = new PdfPCell(new Phrase("Production Sheet No:  " + strPRFIXCom + strRunningNo, new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
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

                        PdfPCell cell12093 = new PdfPCell(new Phrase("Report Date " + ReportDateFrom, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
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

                        if (HSRPStateID == "4" && ddlBothDealerHHT.SelectedItem.Text == "Dealer Data")
                        {
                            PdfPCell cell120000 = new PdfPCell(new Phrase("ID", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                            cell120000.Colspan = 1;
                            cell120000.BorderWidthLeft = 0f;
                            cell120000.BorderWidthRight = 0.8f;
                            cell120000.BorderWidthTop = 0.8f;
                            cell120000.BorderWidthBottom = 0f;

                            cell120000.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell120000);
                        }
                        else if (HSRPStateID.Equals("5") && ddlBothDealerHHT.SelectedItem.Text.Equals("Dealer Data"))
                        {
                            PdfPCell cell111 = new PdfPCell(new Phrase("ID", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                            cell111.Colspan = 1;
                            cell111.BorderWidthLeft = 0f;
                            cell111.BorderWidthRight = 0.8f;
                            cell111.BorderWidthTop = 0.8f;
                            cell111.BorderWidthBottom = 0f;

                            cell111.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell111);


                        }
                        else if (HSRPStateID == "1" || HSRPStateID == "2" || HSRPStateID == "3" || HSRPStateID == "6" || HSRPStateID == "9" || HSRPStateID == "11" || (HSRPStateID == "4" && ddlBothDealerHHT.SelectedItem.Text.Equals("Both") || ddlBothDealerHHT.SelectedItem.Text.Equals("Other")))
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


                                if (HSRPStateID == "4" && ddlBothDealerHHT.SelectedItem.Text == "Dealer Data")
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
                                else if (HSRPStateID.Equals("5") && ddlBothDealerHHT.SelectedItem.Text == "Dealer Data")
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
                            if (HSRPStateID == "2" && ddlBothDealerHHT.SelectedItem.Text == "Dealer Data")
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
                            else if (HSRPStateID == "4" && ddlBothDealerHHT.SelectedItem.Text == "Dealer Data")
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
                            else if (HSRPStateID.Equals("5") && ddlBothDealerHHT.SelectedItem.Text == "Dealer Data")
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
                                PdfPCell cell1209315 = new PdfPCell(new Phrase(dt.Rows[i]["OwnerName"].ToString() + "/ " + dt.Rows[i]["MobileNo"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
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

                            document.NewPage();
                            document.Close();

                            int count = 1;
                            try
                            {
                                StringBuilder sb = new StringBuilder();
                                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                                {

                                    //sb.Append("update hsrprecords set sendtoProductionStatus='Y', NAVPDFFlag='1', NewPdfRunningNo='" + strPRFIXCom + strRunningNo + "', PdfDownloadDate=GetDate(), pdfFileName='" + filename + "', PDFDownloadUserID='" + strUserID + "' where hsrprecordID='" + dt.Rows[i]["hsrprecordID"].ToString() + "';");
                                    // count = count + 1;

                                }
                                //Utils.ExecNonQuery(sb.ToString(), CnnString);
                                //sb.Clear();
                                // StrSqlQuery = "update EmbossingCenters set NewProductionSheetRunningNo='" + strRunningNo + "' where State_Id='" + DropDownListStateName.SelectedValue.ToString() + "' and Emb_Center_Id='" + strEmbId + "'";
                                // Utils.ExecNonQuery(StrSqlQuery, CnnString);
                            }
                            catch (Exception ex)
                            {

                            }

                        }

                        catch
                        {

                        }


                        //HttpContext context = HttpContext.Current;

                        //context.Response.ContentType = "Application/pdf";
                        //context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                        //context.Response.WriteFile(PdfFolder);
                        //context.Response.End();
                    }

                }
            }

            else
            {
                lblErrMsg.Text = "No Record Found!!";
            }
             

        }

        protected void btnrejected_Click(object sender, EventArgs e)
        {
         
             DataTable dtrto = GetRtoLocation();
             for (int iRowNo = 0; iRowNo < dtrto.Rows.Count; iRowNo++)
             {
                // RTOName = dtrto.Rows[iRowNo]["rtolocationname"].ToString();
               //  RTOCode = dtrto.Rows[iRowNo]["RTOLocationid"].ToString();

                 string strStateID = DropDownListStateName.SelectedValue.ToString();
                 string RTOLocationID = dropDownListClient.SelectedValue.ToString();
                 string strOrderTypeHHTDealer = ddlBothDealerHHT.SelectedItem.Text.ToString();
                 string ddVehicleType = DropDownList1.SelectedItem.ToString().ToUpper();
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




                 string strSel = "select isnull(max(right(newProductionSheetRunningNo,7)),0000000) from EmbossingCenters where State_Id='" + DropDownListStateName.SelectedValue.ToString() + "' and Emb_Center_Id='" + strEmbId + "'";
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


                 if (ddVehicleType == "ALL")
                 {
                     strVehicleType = string.Empty;
                 }

                 if (ddVehicleType == "TWO WHEELER")
                 {
                     strVehicleType = " and vehicletype in ('MOTOR CYCLE','SCOOTER') ";
                 }
                 if (ddVehicleType == "THREE WHEELER")
                 {
                     strVehicleType = " and vehicletype in ('THREE WHEELER') ";
                 }
                 if (ddVehicleType == "FOUR WHEELER")
                 {
                     strVehicleType = " and vehicletype in ('LMV','LMV(CLASS)','MCV/HCV/TRAILERS','TRACTOR') ";
                 }

                 RTOName = dtrto.Rows[iRowNo]["rtolocationname"].ToString();
                 RTOCode = dtrto.Rows[iRowNo]["RTOLocationid"].ToString();
                

       //// Change Date 1.09.2015
                  strSQL.Append("Select a.hsrprecordID,a.roundoff_netamount,a.OrderStatus, a.HSRPRecord_AuthorizationNo,convert(varchar, OrderClosedDate, 105) as OrderClosedDate, CONVERT(varchar(20),orderdate ,103) AS OrderBookDate,CONVERT(varchar(20),OrderEmbossingDate ,103) AS OrderEmbossingDate, a.dealerid as ID, left(a.OwnerName,19) as OwnerName, a.MobileNo,(select  AffixCenterDesc from AffixationCenters where Affix_id= a.affix_id ) as AffixCenterDesc, CONVERT(varchar(20), HSRPRecord_AuthorizationDate,103) AS OrderDateAuth, a.OrderDate, a.EngineNo, a.ChassisNo, (select rtolocationname from rtolocation where  rtolocationid =a.rtolocationid) as RTOLocationName,  a.VehicleRegNo,  case a.VehicleType when 'MCV/HCV/TRAILERS' then 'Trailers' when 'THREE WHEELER' then 'T.Whe.' when 'SCOOTER' then 'SCOO' when 'TRACTOR' then 'TRAC' when 'LMV(CLASS)' then 'L.CL' when 'LMV' then 'LMV'  when 'MOTOR CYCLE' then 'MO.C' end as VehicleType, case a.VehicleClass when 'Transport' then 'T' else 'N.T.' end as VehicleClass,  a.HSRP_StateID, (select HSRPStateName from hsrpstate where HSRP_StateID=a.HSRP_StateID)  as HSRPStateName, (select replace(ProductCode,'MM-','') from Product where productid= a.RearPlateSize) AS RearProductCode,(select replace(ProductCode,'MM-','') from Product where productid= a.FrontPlateSize) AS FrontProductCode,a.FrontPlateSize, a.HSRP_Front_LaserCode, a.HSRP_Rear_LaserCode, a.RearPlateSize FROM HSRPRecords AS a   where  a.hsrp_StateID='" + strStateID + "' and a.RTOLocationID='" + RTOLocationID + "' and convert(date,erpassigndate) ='" + AuthorizationDate + "' and ([HSRP_Front_LaserCode] is not null or [HSRP_Rear_LaserCode] is not null) and ([HSRP_Front_LaserCode] !='' or [HSRP_Rear_LaserCode] !='') and isnull(rejectflag,'N')='Y' ");
                 ////
                 // strSQL.Append("Select a.hsrprecordID,a.roundoff_netamount,a.OrderStatus, a.HSRPRecord_AuthorizationNo,convert(varchar, OrderClosedDate, 105) as OrderClosedDate, CONVERT(varchar(20),orderdate ,103) AS OrderBookDate,CONVERT(varchar(20),OrderEmbossingDate ,103) AS OrderEmbossingDate, a.dealerid as ID, left(a.OwnerName,19) as OwnerName, a.MobileNo,(select  AffixCenterDesc from AffixationCenters where Affix_id= a.affix_id ) as AffixCenterDesc, CONVERT(varchar(20), HSRPRecord_AuthorizationDate,103) AS OrderDateAuth, a.OrderDate, a.EngineNo, a.ChassisNo, (select rtolocationname from rtolocation where  rtolocationid =a.rtolocationid) as RTOLocationName,  a.VehicleRegNo,  case a.VehicleType when 'MCV/HCV/TRAILERS' then 'Trailers' when 'THREE WHEELER' then 'T.Whe.' when 'SCOOTER' then 'SCOO' when 'TRACTOR' then 'TRAC' when 'LMV(CLASS)' then 'L.CL' when 'LMV' then 'LMV'  when 'MOTOR CYCLE' then 'MO.C' end as VehicleType, case a.VehicleClass when 'Transport' then 'T' else 'N.T.' end as VehicleClass,  a.HSRP_StateID, (select HSRPStateName from hsrpstate where HSRP_StateID=a.HSRP_StateID)  as HSRPStateName, (select replace(ProductCode,'MM-','') from Product where productid= a.RearPlateSize) AS RearProductCode,(select replace(ProductCode,'MM-','') from Product where productid= a.FrontPlateSize) AS FrontProductCode,a.FrontPlateSize, a.HSRP_Front_LaserCode, a.HSRP_Rear_LaserCode, a.RearPlateSize FROM HSRPRecords AS a   where  a.hsrp_StateID='" + strStateID + "' and a.RTOLocationID='" + RTOLocationID + "' and convert(date,hsrprecord_creationdate) between '2014-12-31 00:00:00' and '2015-02-19 23:59:59' and orderstatus='New Order' and ([HSRP_Front_LaserCode] is not null or [HSRP_Rear_LaserCode] is not null)   and ([HSRP_Front_LaserCode] !='' or [HSRP_Rear_LaserCode] !='') ");

                 if (strOrderTypeHHTDealer == "Both")
                 {
                     strSQL.Append(" ");
                 }
                 else if (ddlBothDealerHHT.SelectedItem.Text == "Dealer Data")
                 {

                     strSQL.Append(" AND a.Addrecordby like'%dealer%' ");
                 }

                 else if (ddlBothDealerHHT.SelectedItem.Text == "Other")
                 {
                     strSQL.Append(" AND a.Addrecordby not like'%dealer%' ");
                 }

                 if (strVehicleType == "")
                 {
                     strSQL.Append(" order by a.VehicleClass,a.VehicleType,a.HSRP_Front_LaserCode,a.hsrp_rear_lasercode");
                 }
                 else
                 {
                     strSQL.Append(strVehicleType + " order by a.VehicleClass,a.VehicleType,a.HSRP_Front_LaserCode,a.hsrp_rear_lasercode");
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

                     //table = new PdfPTable(14);
                     //var colWidthPercentages = new[] { 55f, 125f, 55f, 200f, 75f, 165f, 165f, 155f, 110f, 210f, 105f, 210f, 105f, 125f };//7,14

                     table = new PdfPTable(11);
                     var colWidthPercentages = new[] { 55f, 125f, 55f, 200f, 75f,  155f, 150f, 210f, 150f, 150f, 105f };//7,14

                    // var colWidthPercentages = new[] { 55f, 125f, 55f, 200f, 75f, 165f,  110f, 210f, 105f, 210f, 105f, 125f };//7,14
                     var colheightpercentage = new[] { 2f };

                     table.SetWidths(colWidthPercentages);


                     string strQueryDate = "SELECT CONVERT(VARCHAR,GETDATE(),103)";
                     DataTable dtDate = Utils.GetDataTable(strQueryDate, CnnString);

                     table.TotalWidth = 800f;
                     table.LockedWidth = true;



                     PdfPCell cell786a = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
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

                     PdfPCell cell12093 = new PdfPCell(new Phrase("Report Date " + ReportDateFrom, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
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

                     //PdfPCell cell1206 = new PdfPCell(new Phrase("Chassis No", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                     //cell1206.Colspan = 1;
                     //cell1206.BorderWidthLeft = 0.8f;
                     //cell1206.BorderWidthRight = .8f;
                     //cell1206.BorderWidthTop = 0.8f;
                     //cell1206.BorderWidthBottom = 0.8f;
                     //cell1206.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right

                     //table.AddCell(cell1206);

                     //PdfPCell cell1221 = new PdfPCell(new Phrase("EngineNo", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                     //cell1221.Colspan = 1;
                     //cell1221.BorderWidthLeft = 0.8f;
                     //cell1221.BorderWidthRight = 0.8f;
                     //cell1221.BorderWidthTop = .8f;
                     //cell1221.BorderWidthBottom = 0.8f;

                     //cell1221.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                     //table.AddCell(cell1221);

                     if (HSRPStateID == "4" && ddlBothDealerHHT.SelectedItem.Text == "Dealer Data")
                     {
                         PdfPCell cell120000 = new PdfPCell(new Phrase("ID", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                         cell120000.Colspan = 1;
                         cell120000.BorderWidthLeft = 0f;
                         cell120000.BorderWidthRight = 0.8f;
                         cell120000.BorderWidthTop = 0.8f;
                         cell120000.BorderWidthBottom = 0f;

                         cell120000.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                         table.AddCell(cell120000);
                     }
                     else if (HSRPStateID.Equals("5") && ddlBothDealerHHT.SelectedItem.Text.Equals("Dealer Data"))
                     {
                         PdfPCell cell111 = new PdfPCell(new Phrase("ID", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                         cell111.Colspan = 1;
                         cell111.BorderWidthLeft = 0f;
                         cell111.BorderWidthRight = 0.8f;
                         cell111.BorderWidthTop = 0.8f;
                         cell111.BorderWidthBottom = 0f;

                         cell111.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                         table.AddCell(cell111);


                     }
                     else if (HSRPStateID == "1" || HSRPStateID == "2" || HSRPStateID == "3" || HSRPStateID == "6" || HSRPStateID == "9" || HSRPStateID == "11" || (HSRPStateID == "4" && ddlBothDealerHHT.SelectedItem.Text.Equals("Both") || ddlBothDealerHHT.SelectedItem.Text.Equals("Other")))
                     {
                         //PdfPCell cell120933 = new PdfPCell(new Phrase("Owner Name", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                         //cell120933.Colspan = 1;
                         //cell120933.BorderWidthLeft = 0f;
                         //cell120933.BorderWidthRight = 0.8f;
                         //cell120933.BorderWidthTop = 0.8f;
                         //cell120933.BorderWidthBottom = 0f;

                         //cell120933.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                         //table.AddCell(cell120933);
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



                             //PdfPCell cell1206a = new PdfPCell(new Phrase("Chassis No", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                             //cell1206a.Colspan = 1;
                             //cell1206a.BorderWidthLeft = 0f;
                             //cell1206a.BorderWidthRight = .8f;
                             //cell1206a.BorderWidthTop = 0.8f;
                             //cell1206a.BorderWidthBottom = 0.8f;
                             //cell1206a.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                             //table.AddCell(cell1206a);

                             //PdfPCell cell1221a = new PdfPCell(new Phrase("EngineNo", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                             //cell1221a.Colspan = 1;
                             //cell1221a.BorderWidthLeft = 0f;
                             //cell1221a.BorderWidthRight = 0.8f;
                             //cell1221a.BorderWidthTop = 0.8f;
                             //cell1221a.BorderWidthBottom = 0.8f;
                             //cell1221a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                             //table.AddCell(cell1221a);


                             if (HSRPStateID == "4" && ddlBothDealerHHT.SelectedItem.Text == "Dealer Data")
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
                             else if (HSRPStateID.Equals("5") && ddlBothDealerHHT.SelectedItem.Text == "Dealer Data")
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

                                 //PdfPCell cell120933a = new PdfPCell(new Phrase("Owner Name", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                                 //cell120933a.Colspan = 1;
                                 //cell120933a.BorderWidthLeft = 0f;
                                 //cell120933a.BorderWidthRight = 0.8f;
                                 //cell120933a.BorderWidthTop = 0.8f;
                                 //cell120933a.BorderWidthBottom = 0f;
                                 //cell120933a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                 //table.AddCell(cell120933a);
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

                         //PdfPCell cell1216 = new PdfPCell(new Phrase(dt.Rows[i]["ChassisNo"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                         //cell1216.Colspan = 1;
                         //cell1216.MinimumHeight = 0f;//25

                         //cell1216.BorderWidthLeft = 0f;
                         //cell1216.BorderWidthRight = .8f;
                         //cell1216.BorderWidthTop = 0f;
                         //cell1216.BorderWidthBottom = .5f;

                         //cell1216.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right

                         //table.AddCell(cell1216);

                         //PdfPCell cell1222 = new PdfPCell(new Phrase(dt.Rows[i]["EngineNo"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                         //cell1222.Colspan = 1;
                         //cell1222.MinimumHeight = 0f;//25

                         //cell1222.BorderWidthLeft = 0f;
                         //cell1222.BorderWidthRight = .8f;
                         //cell1222.BorderWidthTop = 0f;
                         //cell1222.BorderWidthBottom = .5f;

                         //cell1222.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                         //table.AddCell(cell1222);


                         if (HSRPStateID == "2" && ddlBothDealerHHT.SelectedItem.Text == "Dealer Data")
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
                         else if (HSRPStateID == "4" && ddlBothDealerHHT.SelectedItem.Text == "Dealer Data")
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
                         else if (HSRPStateID.Equals("5") && ddlBothDealerHHT.SelectedItem.Text == "Dealer Data")
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
                             //PdfPCell cell1209315 = new PdfPCell(new Phrase(dt.Rows[i]["OwnerName"].ToString() + "/ " + dt.Rows[i]["MobileNo"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                             //cell1209315.Colspan = 1;
                             //cell1209315.MinimumHeight = 0f;//25

                             //cell1209315.BorderWidthLeft = 0f;
                             //cell1209315.BorderWidthRight = .8f;
                             //cell1209315.BorderWidthTop = .5f;
                             //cell1209315.BorderWidthBottom = .5f;

                             //cell1209315.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                             //table.AddCell(cell1209315);

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

                        // int count = 1;
                         try
                         {
                             StringBuilder sb = new StringBuilder();
                             //for (int i = 0; i <= dt.Rows.Count - 1; i++)
                             //{

                             //    sb.Append("update hsrprecords set sendtoProductionStatus='Y', NAVPDFFlag='1', NewPdfRunningNo='" + strPRFIXCom + strRunningNo + "', PdfDownloadDate=GetDate(), pdfFileName='" + filename + "', PDFDownloadUserID='" + strUserID + "' where hsrprecordID='" + dt.Rows[i]["hsrprecordID"].ToString() + "';");
                             //    count = count + 1;

                             //}
                             //Utils.ExecNonQuery(sb.ToString(), CnnString);
                             //sb.Clear();
                             //StrSqlQuery = "update EmbossingCenters set NewProductionSheetRunningNo='" + strRunningNo + "' where State_Id='" + DropDownListStateName.SelectedValue.ToString() + "' and Emb_Center_Id='" + strEmbId + "'";
                             //Utils.ExecNonQuery(StrSqlQuery, CnnString);
                         }
                         catch (Exception ex)
                         {

                         }


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
           
        
        }

       
    }
}
 
        
