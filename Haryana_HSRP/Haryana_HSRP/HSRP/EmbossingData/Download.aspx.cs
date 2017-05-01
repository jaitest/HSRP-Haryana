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
    public partial class Download : System.Web.UI.Page
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
        string strFrmDateString = string.Empty;
        string strToDateString = string.Empty;
        DataProvider.BAL bl = new DataProvider.BAL();
        BAL obj = new BAL();
        string StickerManditory = string.Empty;
        StringBuilder sb = new StringBuilder();
        CheckBox chk;
        string ddlListValue = string.Empty;
        string txtTransporter = string.Empty;
        string lblErrMsg = string.Empty;
        string txtLorryNo = string.Empty;
        string ddllistStatename = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            RTOLocationID = Session["UserRTOLocationID"].ToString();
            UserType = Session["UserType"].ToString();
            HSRPStateID = Session["UserHSRPStateID"].ToString();

            ddlListValue = Request.QueryString["ddlValue"];
            ddllistStatename = Request.QueryString["ddllistStatename"];
            GridView GridView1=(GridView)Session["gridview"];

            txtLorryNo = Request.QueryString["txtLorryNo"];
            txtTransporter = Request.QueryString["txtTransport"];
            
            try
            {
                #region Validation
                if (string.IsNullOrEmpty(txtTransporter))
                {
                    lblErrMsg = "Please Enter Transporter .";
                    return;

                }
                if (string.IsNullOrEmpty(txtLorryNo))
                {
                  
                    return;
                }
                #endregion

                string currentdate = DateTime.Now.ToString("dd/MM/yyyy");
                string RtoName = string.Empty;
                RtoName = ddlListValue;
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

                Int64 totalamount = 0;


                //Opens the document:
                object Exicse;
                object Vatamt;
                object TotalAmount;
                object TotalAMT;
                object Qty;
                object SecondaryCess;
                object Educess;
                object TotalWeight;
                string vehicle = string.Empty;
                string strHsrpRecordId = string.Empty;
                string strInvoiceNo = string.Empty;
                string strEmbStationName = string.Empty;
                string strEmbAddress = string.Empty;
                string strEmbCity = string.Empty;
                string strEmbId = string.Empty;

                #region Set ChallanNo
                try
                {

                    if (GridView1.Rows.Count == 0)
                    {
                        //lblErrMsg.Text = "No Record Found.";
                        return;

                    }
                    // Validate checked recirds
                    int ChkBoxCount = 0;
                    for (int i = 0; i < GridView1.Rows.Count; i++)
                    {
                        chk = GridView1.Rows[i].Cells[0].FindControl("CHKSelect") as CheckBox;

                        if (chk.Checked == true)
                        {
                            ChkBoxCount = ChkBoxCount + 1;
                        }
                    }
                    if (ChkBoxCount == 0)
                    {
                        //lblErrMsg.Text = "Please select atleast 1 record.";
                        return;
                    }
                    string strGetInvoiceNo = string.Empty;
                    string strSelectEmbStation = "SELECT DISTINCT [NAVEMBID],[EmbCenterName],RTOLocationName,city FROM [vw_RTOLocationWiseEmbosingCenters] WHERE RTOLocationId='" + ddlListValue + "'";
                    DataTable dtEmbData = Utils.GetDataTable(strSelectEmbStation, CnnString);
                    if (dtEmbData.Rows.Count <= 0)
                    {
                        //lblErrMsg.Text = "Embossing Station not found";
                        return;
                    }
                    strEmbStationName = dtEmbData.Rows[0]["EmbCenterName"].ToString();
                    strEmbAddress = dtEmbData.Rows[0]["RTOLocationName"].ToString();
                    strEmbCity = dtEmbData.Rows[0]["city"].ToString();
                    strEmbId = dtEmbData.Rows[0]["NAVEMBID"].ToString();

                    if (HSRPStateID == "9" || HSRPStateID == "11" || HSRPStateID == "4")
                    {
                        strGetInvoiceNo = "select (isnull(prefixtext,'')+right('00000'+ convert(varchar,lastno+1),5)) from [hsrpstate] where hsrp_stateid= '" + HSRPStateID + "' and prefixfor='Cash Receipt No' ";
                    }
                    else
                    {
                        strGetInvoiceNo = "select (isnull(prefixtext,'')+right('00000'+ convert(varchar,lastno+1),5)) from [EmbossingCenters] where Emb_Center_Id= '" + strEmbId + "' and prefixfor='Cash Receipt No' ";
                    }
                    strInvoiceNo = (Utils.getScalarValue(strGetInvoiceNo, CnnString));
                }
                catch
                {
                    //lblErrMsg.Text = "Embossing Station not found";
                    return;
                }

                string strGetFinYear = "SELECT [dbo].[fnGetFiscalYear] ( GetDate() )";
                strInvoiceNo = strInvoiceNo + "/" + (Utils.getScalarValue(strGetFinYear, CnnString)).Replace("20", string.Empty);

                #endregion
                int iChkCount = 0;
                StringBuilder sbx = new StringBuilder();
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    chk = GridView1.Rows[i].Cells[0].FindControl("CHKSelect") as CheckBox;

                    if (chk.Checked == true)
                    {
                        iChkCount = iChkCount + 1;
                        Label lblVehicleRegNo = GridView1.Rows[i].Cells[1].FindControl("lblVehicleRegNo") as Label;

                        Label OrderStatus = GridView1.Rows[i].Cells[4].FindControl("lblOrderStatus") as Label;

                        string strRecordId = GridView1.DataKeys[i]["hsrprecordid"].ToString();
                        if (strHsrpRecordId == "")
                        {
                            strHsrpRecordId = strRecordId;
                        }
                        else
                        {
                            strHsrpRecordId = strHsrpRecordId + "," + strRecordId;
                        }

                        // sbx.Append("update hsrprecords set ChallanNo='" + strInvoiceNo + "', ChallanCreatedBy='" + strUserID + "', challandate=getdate(),Invoice_Flag='Y' where  hsrprecordid ='" + strRecordId + "';");

                    }
                }
                if (iChkCount > 0)
                {
                    // Utils.ExecNonQuery(sbx.ToString(), CnnString);

                }
                if (HSRPStateID == "9" || HSRPStateID == "11" || HSRPStateID == "4")
                {
                    string strUpdateInvoiceNo = "update hsrpstate set lastno=lastno+1 where hsrp_stateid= '" + HSRPStateID + "' and prefixfor='Cash Receipt No'";
                    // Utils.ExecNonQuery(strUpdateInvoiceNo, CnnString);
                }
                else
                {
                    string strUpdateInvoiceNo = "update [EmbossingCenters] set lastno=lastno+1 where [Emb_Center_Id]= '" + strEmbId + "' and prefixfor='Cash Receipt No'";
                    //  Utils.ExecNonQuery(strUpdateInvoiceNo, CnnString);
                }
                DataTable GetAddress;
                string Address;
                GetAddress = Utils.GetDataTable("select * from HSRPState WHERE HSRP_StateID='" + HSRPStateID + "'", CnnString);

                if ((GetAddress.Rows[0]["pincode"].ToString() != "") || (GetAddress.Rows[0]["pincode"] != null))
                {
                    Address = GetAddress.Rows[0]["Address1"].ToString() + " - " + GetAddress.Rows[0]["pincode"];
                }
                else
                {
                    Address = GetAddress.Rows[0]["Address1"].ToString();
                }

                string sqlstring = string.Empty;
                sqlstring = "insert into InvoiceMaster(InvoiceNo,InvoiceDate,Amount,BuyerName,clientName,hsrp_stateid) values('" + strInvoiceNo + "', getdate(),'" + totalamount + "','" + RtoName + "','" + Address.ToString().Trim() + "','" + HSRPStateID + "')";
                //Utils.ExecNonQuery(sqlstring, CnnString);

                string strSelectAddress = "SELECT RTOLocationAddress FROM [RTOLocation] WHERE RTOLocationId='" + ddlListValue + "'";
                DataTable dtAddress = Utils.GetDataTable(strSelectAddress, CnnString);

                string strRTOAddress = dtAddress.Rows[0]["RTOLocationAddress"].ToString();

                PdfPTable table2 = new PdfPTable(10);
                PdfPTable table1 = new PdfPTable(10);
                PdfPTable table = new PdfPTable(10);
                PdfPTable table3 = new PdfPTable(2);

                //actual width of table in points
                table.TotalWidth = 1000f;
                string OldRegPlate = string.Empty;



                DataTable dt = new DataTable();

                // SQlQuery = "select vehicletype+' (Damage Front)' as DescriptionOfGoods,count(*) as qty,round((sum(EXCISEBASIC)/count(*)),3) amt,round(sum(EXCISEBASIC),2)bamt,round(sum(Vat_Amount),2)Vatamt,round(sum(EXCISEAMT),2) excise,round(sum(cessamt),2)cess,round(sum(shecessamt),2)shecess  from hsrprecords  where hsrp_stateid='" + HSRPStateID + "' and ordertype in ('DF') and HsrpRecordID in(" + strHsrpRecordId + ") group by vehicletype union   select vehicletype+' (Damage Rear)',count(*) as qty,round((sum(EXCISEBASIC)/count(*)),2) amt,round(sum(EXCISEBASIC),2)bamt,round(sum(Vat_Amount),2)Vatamt,round(sum(EXCISEAMT),2) excise,round(sum(cessamt),2)cess,round(sum(shecessamt),2)shecess   from hsrprecords  where hsrp_stateid='" + HSRPStateID + "' and HsrpRecordID in(" + strHsrpRecordId + ") and  ordertype in ('DR') group by vehicletype union select vehicletype+' (Both Plates)',(count(*)*2)/2 qty,round((sum(EXCISEBASIC)/count(*)),2) amt,round(sum(EXCISEBASIC),2)bamt,round(sum(Vat_Amount),2)Vatamt,round(sum(EXCISEAMT),2) excise,round(sum(cessamt),2)cess,round(sum(shecessamt),2)shecess   from hsrprecords  where hsrp_stateid='" + HSRPStateID + "' and HsrpRecordID in(" + strHsrpRecordId + ") and ordertype in ('NB','OB','DB') group by vehicletype";
                SQlQuery = "select a.vehicletype+' (Damage Front)' as DescriptionOfGoods,count(*) as qty,hsrpweight*count(*) as HSRPWeight,round((sum(EXCISEBASIC)/count(*)),3) amt,round(sum(EXCISEBASIC),2)bamt,round(sum(Vat_Amount),2)Vatamt,round(sum(EXCISEAMT),2) excise,round(sum(cessamt),2)cess,round(sum(shecessamt),2)shecess  from hsrprecords a,hsrpweightmaster b where a.VehicleType=b.vehicletype and hsrp_stateid='" + HSRPStateID + "' and a.OrderType in ('DF') and b.ordertype='DF' and HsrpRecordID in(" + strHsrpRecordId + ") group by a.vehicletype,b.hsrpweight union select a.vehicletype+' (Damage Rear)',count(*) as qty,hsrpweight*count(*),round((sum(EXCISEBASIC)/count(*)),2) amt,round(sum(EXCISEBASIC),2)bamt,round(sum(Vat_Amount),2)Vatamt,round(sum(EXCISEAMT),2) excise,round(sum(cessamt),2)cess,round(sum(shecessamt),2)shecess   from hsrprecords a,hsrpweightmaster b  where a.vehicletype=b.vehicletype and hsrp_stateid='" + HSRPStateID + "' and HsrpRecordID in(" + strHsrpRecordId + ") and  a.ordertype in ('DR') and b.ordertype='DR' group by a.VehicleType,b.hsrpweight union select a.vehicletype+' (Both Plates)',(count(*)*2)/2 qty,hsrpweight*(count(*)*2)/2,round((sum(EXCISEBASIC)/count(*)),2) amt,round(sum(EXCISEBASIC),2)bamt,round(sum(Vat_Amount),2)Vatamt,round(sum(EXCISEAMT),2) excise,round(sum(cessamt),2)cess,round(sum(shecessamt),2)shecess   from hsrprecords a,hsrpweightmaster b  where a.vehicletype=b.vehicletype and hsrp_stateid='" + HSRPStateID + "' and HsrpRecordID in(" + strHsrpRecordId + ") and a.ordertype in ('NB','OB','DB') and b.ordertype='NB' group by a.VehicleType,b.hsrpweight";
                dt = Utils.GetDataTable(SQlQuery, CnnString);


                BAL obj = new BAL();
                //  string UserID = Session["UID"].ToString();
                decimal dTotalAmount = 0;
                if (true)
                {
                    #region Upper Part of PDF

                    string strStateId = ddlListValue;
                    string strLocId = ddlListValue;



                    DataTable dataSetFillHSRPDeliveryChallan = new DataTable();


                    string strEccNo = GetAddress.Rows[0]["ExciseNo"].ToString();
                    string strDivision = GetAddress.Rows[0]["Division"].ToString();
                    string strRange = GetAddress.Rows[0]["Range"].ToString();
                    string strCommissionerate = GetAddress.Rows[0]["Commissionerate"].ToString();
                    string strTin = GetAddress.Rows[0]["Tinno"].ToString();
                    string strTariffHead = GetAddress.Rows[0]["CH"].ToString();

                    GenerateCell(table, 10, 0, 0, 0, 0, 1, 1, "", 50, 0);

                    GenerateCell(table, 10, 1, 1, 1, 1, 1, 1, "Retail Invoice(For removal of Excisable goods Under Rule-11 of C E Rules-2002)", 30, 0);

                    string strCompName = GetAddress.Rows[0]["CompanyName"].ToString();
                    GenerateCell(table, 3, 1, 1, 0, 0, 0, 0, strCompName, 0, 0);

                    GenerateCell(table, 3, 0, 1, 0, 0, 1, 0, "Invoice No", 0, 0);

                    GenerateCell(table, 4, 0, 1, 0, 0, 0, 1, "Dated", 0, 0);

                    string strPhone = string.Empty;
                    if (ddllistStatename.Equals("9"))
                    {
                        strPhone = "Phone No : 04042226000";
                    }
                    else if (ddllistStatename.Equals("2"))
                    {

                    }
                    else if (ddllistStatename.Equals("4"))
                    {

                    }
                    GenerateCell(table, 3, 1, 1, 0, 0, 0, 1, Address.ToString() + " , " +
                    GetAddress.Rows[0]["city"].ToString() + " E-Mail : info@linkutsav.com \r\n" + strPhone, 0, 0);

                    if (HSRPStateID == "9")
                    {
                        GenerateCell(table, 3, 0, 1, 0, 0, 1, 1, "AP/ " + strInvoiceNo, 0, 0);
                    }
                    else if (HSRPStateID == "11")
                    {
                        GenerateCell(table, 3, 0, 1, 0, 0, 1, 1, "TG/ " + strInvoiceNo, 0, 0);
                    }
                    else if (HSRPStateID == "6")
                    {
                        GenerateCell(table, 3, 0, 1, 0, 0, 1, 1, "UK/ " + strInvoiceNo, 0, 0);
                    }
                    else
                    {
                        GenerateCell(table, 3, 0, 1, 0, 0, 1, 1, strInvoiceNo, 0, 0);
                    }
                    GenerateCell(table, 4, 0, 1, 0, 0, 0, 1, currentdate, 0, 0);

                    GenerateCell(table, 3, 1, 1, 0, 0, 0, 1, "", 0, 0);

                    GenerateCell(table, 3, 0, 1, 0, 0, 0, 1, "", 0, 0);

                    GenerateCell(table, 4, 0, 1, 0, 0, 0, 1, "Other Reference(s)", 0, 0);

                    if (!strEmbStationName.Equals("Chintal"))
                    {
                        GenerateCell(table, 3, 1, 1, 1, 0, 0, 1, "", 20, 0);
                    }
                    else
                    {
                        GenerateCell(table, 3, 1, 1, 1, 0, 0, 1, "", 20, 0);
                    }

                    GenerateCell(table, 7, 0, 1, 1, 1, 1, 0, "Registration Details", 0, 0);


                    if (strEmbStationName.Equals("Lalkua Plant"))
                    {
                        GenerateCell(table, 3, 1, 1, 0, 0, 0, 1, strEmbStationName + " RTO PREMISES", 20, 0);
                    }
                    if (strEmbStationName.Equals("Chintal"))
                    {
                        GenerateCell(table, 3, 1, 1, 0, 0, 0, 1, " ", 20, 0);
                    }
                    else if (ddllistStatename.ToString() == "9")
                    {
                        GenerateCell(table, 3, 1, 1, 0, 0, 0, 1, "Vijayawada Manufacturing Unit", 20, 0);
                    }
                    else
                    {
                        GenerateCell(table, 3, 1, 1, 0, 1, 0, 1, "", 20, 0);
                    }
                    GenerateCell(table, 1, 0, 0, 0, 1, 0, 1, "", 20, 0);

                    GenerateCell(table, 6, 1, 1, 0, 1, 0, 0, "Company's", 20, 100);


                    GenerateCell(table, 3, 1, 1, 0, 0, 0, 0, "", 0, 0);

                    GenerateCell(table, 1, 0, 0, 0, 1, 0, 0, "ECC No.", 0, 0);

                    GenerateCell(table, 6, 1, 1, 0, 1, 0, 1, strEccNo, 0, 0);




                    if (strEmbStationName.Equals("Chintal"))
                    {
                        GenerateCell(table, 3, 1, 1, 0, 0, 0, 0, "Cash Customer Affixation Center: " + strEmbAddress, 0, 0);
                    }

                    else
                    {
                        GenerateCell(table, 3, 1, 1, 0, 0, 0, 0, "", 0, 0);
                    }

                    GenerateCell(table, 1, 0, 0, 0, 1, 0, 0, "Division-", 0, 0);

                    GenerateCell(table, 6, 1, 1, 0, 1, 0, 1, strDivision, 0, 0);




                    if (strEmbStationName.Equals("Chintal"))
                    {
                        GenerateCell(table, 3, 1, 1, 0, 0, 0, 0, strEmbCity, 0, 0);
                    }
                    else
                    {
                        GenerateCell(table, 3, 1, 1, 0, 0, 0, 0, "Cash Customer Affixation Center:" + strRTOAddress, 0, 0);
                    }


                    GenerateCell(table, 1, 0, 0, 0, 1, 0, 0, "Range", 0, 0);

                    GenerateCell(table, 6, 1, 1, 0, 1, 0, 1, strRange, 0, 0);

                    GenerateCell(table, 3, 1, 1, 0, 0, 0, 0, "", 0, 0);
                    GenerateCell(table, 1, 0, 0, 0, 0, 0, 0, "Comm.", 0, 0);
                    GenerateCell(table, 6, 1, 1, 0, 0, 0, 1, strCommissionerate, 0, 0);




                    GenerateCell(table, 3, 1, 1, 0, 0, 0, 0, "", 0, 0);
                    GenerateCell(table, 1, 0, 0, 1, 1, 0, 0, "TIN-", 0, 0);
                    GenerateCell(table, 6, 1, 1, 1, 1, 0, 1, strTin, 0, 0);


                    GenerateCell(table, 3, 1, 1, 0, 0, 0, 0, "", 0, 0);
                    GenerateCell(table, 7, 0, 1, 0, 0, 0, 0, "Transporter : " + txtTransporter, 0, 0);




                    GenerateCell(table, 3, 1, 1, 1, 0, 0, 0, "", 0, 0);
                    GenerateCell(table, 7, 0, 1, 1, 0, 0, 0, "Lorry No : " + txtLorryNo, 0, 0);



                    #endregion

                    #region BlankRow
                    GenerateCell(table, 10, 1, 1, 1, 0, 0, 1, "", 20, 0);
                    #endregion

                    #region Column Heading Creation

                    GenerateCell(table, 1, 1, 0, 1, 0, 1, 0, "SI.NO.", 0, 0);

                    GenerateCell(table, 5, 1, 1, 1, 0, 1, 0, "Description of Goods", 0, 0);
                    GenerateCell(table, 1, 0, 0, 1, 0, 1, 0, "Tariff Head", 0, 0);
                    GenerateCell(table, 1, 1, 1, 1, 0, 1, 0, "Qty-:Set | Weight", 0, 0);
                    GenerateCell(table, 1, 0, 1, 1, 0, 1, 0, "Rate", 0, 0);
                    GenerateCell(table, 1, 0, 1, 1, 0, 1, 0, "Amount (Rs.)", 0, 0);


                    #endregion

                    // 0=Bold,1=Normal
                    #region 1st Row

                    GenerateCell(table, 1, 1, 0, 1, 0, 1, 1, "1", 0, 0);

                    GenerateCell(table, 5, 1, 1, 1, 0, 1, 0, "HSRP SET FOR:", 0, 0);
                    GenerateCell(table, 1, 0, 0, 1, 0, 0, 0, "", 0, 0);
                    GenerateCell(table, 1, 1, 1, 1, 0, 0, 0, "", 0, 0);
                    for (int i = 0; i < 2; i++)
                    {
                        GenerateCell(table, 1, 0, 1, 1, 0, 0, 0, "", 0, 0);
                    }
                    #endregion

                    #region 2nd Row
                    if (ddllistStatename.ToString() == "9")
                    {

                    }

                    Exicse = dt.Compute("sum(excise)", "");
                    ExicseAmount = String.Format("{0:0.00}", Exicse);



                    TotalAmount = dt.Compute("sum(bamt)", "");
                    Vatamt = dt.Compute("sum(Vatamt)", "");
                    TotalAMT = dt.Compute("sum(amt)", "");
                    Qty = dt.Compute("sum(qty)", "");
                    TotalWeight = dt.Compute("sum(HSRPWeight)", "");
                    Educess = dt.Compute("sum(cess)", "");
                    SecondaryCess = dt.Compute("sum(shecess)", "");

                    string Newamount = String.Format("{0:0.00}", TotalAmount);
                    string VatamtNew = String.Format("{0:0.00}", Vatamt);
                    string strTotalAMT = String.Format("{0:0.00}", TotalAMT);
                    string strEducess = String.Format("{0:0.00}", Educess);
                    string strSecondaryCess = string.Format("{0:0.00}", SecondaryCess);

                    for (int iResult = 0; iResult < dt.Rows.Count; iResult++)
                    {
                        GenerateCell(table, 1, 1, 0, 0, 0, 1, 1, "", 0, 0);
                        GenerateCell(table, 5, 1, 1, 0, 0, 1, 0, dt.Rows[iResult]["DescriptionOfGoods"].ToString(), 0, 0);
                        GenerateCell(table, 1, 0, 0, 0, 0, 1, 1, strTariffHead, 0, 0);//HSRPWeight
                        GenerateCell(table, 1, 1, 1, 0, 0, 1, 1, dt.Rows[iResult]["qty"].ToString() + " | " + dt.Rows[iResult]["HSRPWeight"].ToString(), 0, 0);
                        GenerateCell(table, 1, 0, 1, 0, 0, 1, 1, dt.Rows[iResult]["amt"].ToString(), 0, 0);
                        GenerateCell(table, 1, 0, 1, 0, 0, 2, 1, String.Format("{0:0.00}", dt.Rows[iResult]["bamt"]), 0, 0);

                        //Amount = Convert.ToDecimal(dt.Rows[iResult]["bamt"].ToString());
                    }


                    decimal PreTotalAmount = 0;
                    PreTotalAmount = 200;// PreTotalAmount + Convert.ToDecimal(ExicseAmount) + Convert.ToDecimal(Newamount) + Convert.ToDecimal(strEducess) + Convert.ToDecimal(strSecondaryCess);
                    dTotalAmount = 100;// PreTotalAmount + Convert.ToDecimal(VatamtNew);

                    Int64 Damount = Convert.ToInt64(Math.Round(dTotalAmount, 2));

                    #endregion


                    #region Blank


                    for (int j = 0; j < 4; j++)
                    {
                        GenerateCell(table, 1, 1, 0, 0, 0, 1, 1, "", 0, 0);

                        GenerateCell(table, 5, 1, 1, 0, 0, 1, 1, "", 0, 0);
                        GenerateCell(table, 1, 0, 0, 0, 0, 1, 1, "", 0, 0);
                        GenerateCell(table, 1, 1, 1, 0, 0, 1, 1, "", 0, 0);
                        GenerateCell(table, 1, 0, 1, 0, 0, 1, 1, "", 0, 0);
                        GenerateCell(table, 1, 0, 1, 0, 0, 1, 1, "", 0, 0);

                    }



                    GenerateCell(table, 1, 1, 0, 0, 0, 1, 1, "", 0, 0);

                    GenerateCell(table, 5, 1, 1, 0, 0, 1, 1, "Sub Total", 0, 0);
                    GenerateCell(table, 1, 0, 0, 0, 0, 1, 1, "", 0, 0);
                    GenerateCell(table, 1, 1, 1, 0, 0, 1, 1, "", 0, 0);
                    GenerateCell(table, 1, 0, 1, 0, 0, 1, 1, "", 0, 0);
                    GenerateCell(table, 1, 0, 1, 1, 1, 2, 1, Newamount, 0, 0);

                    GenerateCell(table, 1, 1, 0, 0, 0, 1, 1, "", 0, 0);

                    GenerateCell(table, 5, 1, 1, 0, 0, 1, 1, "", 0, 0);
                    GenerateCell(table, 1, 0, 0, 0, 0, 1, 1, "", 0, 0);
                    GenerateCell(table, 1, 1, 1, 0, 0, 1, 1, "", 0, 0);
                    GenerateCell(table, 1, 0, 1, 0, 0, 1, 1, "", 0, 0);
                    GenerateCell(table, 1, 0, 1, 0, 0, 1, 1, "", 0, 0);


                    GenerateCell(table, 1, 1, 0, 0, 0, 1, 1, "", 0, 0);

                    GenerateCell(table, 5, 1, 1, 0, 0, 1, 1, "Excise Duty", 0, 0);
                    GenerateCell(table, 1, 0, 0, 0, 0, 1, 1, GetAddress.Rows[0]["ExciseDuty"].ToString() + "%", 0, 0);
                    GenerateCell(table, 1, 1, 1, 0, 0, 1, 1, "", 0, 0);
                    GenerateCell(table, 1, 0, 1, 0, 0, 1, 1, "", 0, 0);
                    GenerateCell(table, 1, 0, 1, 0, 0, 2, 1, ExicseAmount.ToString(), 0, 0);

                    //GenerateCell(table, 1, 1, 0, 0, 0, 1, 1, "", 0, 0);
                    //GenerateCell(table, 5, 1, 1, 0, 0, 1, 1, "Educational Cess", 0, 0);
                    //GenerateCell(table, 1, 0, 0, 0, 0, 1, 1, GetAddress.Rows[0]["EducationCess"].ToString() + "%", 0, 0);
                    //GenerateCell(table, 1, 1, 1, 0, 0, 1, 1, "", 0, 0);
                    //GenerateCell(table, 1, 0, 1, 0, 0, 1, 1, "", 0, 0);
                    //GenerateCell(table, 1, 0, 1, 0, 0, 2, 1, strEducess, 0, 0);


                    //GenerateCell(table, 1, 1, 0, 0, 0, 1, 1, "", 0, 0);
                    //GenerateCell(table, 5, 1, 1, 0, 0, 1, 1, "Secondary Educational Cess", 0, 0);
                    //GenerateCell(table, 1, 0, 0, 0, 0, 1, 1, GetAddress.Rows[0]["HEducationCess"].ToString() + "%", 0, 0);
                    //GenerateCell(table, 1, 1, 1, 0, 0, 1, 1, "", 0, 0);
                    //GenerateCell(table, 1, 0, 1, 0, 0, 1, 1, "", 0, 0);
                    //GenerateCell(table, 1, 0, 1, 0, 0, 2, 1, strSecondaryCess, 0, 0);



                    GenerateCell(table, 1, 1, 0, 0, 0, 1, 1, "", 0, 0);

                    GenerateCell(table, 5, 1, 1, 0, 0, 1, 1, "Total", 0, 0);
                    GenerateCell(table, 1, 0, 0, 0, 0, 1, 1, "", 0, 0);
                    GenerateCell(table, 1, 1, 1, 0, 0, 1, 1, "", 0, 0);
                    GenerateCell(table, 1, 0, 1, 0, 0, 1, 1, "", 0, 0);
                    GenerateCell(table, 1, 0, 1, 1, 1, 2, 1, PreTotalAmount.ToString(), 0, 0);



                    GenerateCell(table, 1, 1, 0, 0, 0, 1, 1, "", 0, 0);

                    GenerateCell(table, 5, 1, 1, 0, 0, 1, 1, "", 0, 0);
                    GenerateCell(table, 1, 0, 0, 0, 0, 1, 1, "", 0, 0);
                    GenerateCell(table, 1, 1, 1, 0, 0, 1, 1, "", 0, 0);
                    GenerateCell(table, 1, 0, 1, 0, 0, 1, 1, "", 0, 0);
                    GenerateCell(table, 1, 0, 1, 0, 0, 1, 1, "", 0, 0);



                    GenerateCell(table, 1, 1, 0, 0, 1, 1, 1, "", 0, 0);
                    GenerateCell(table, 5, 1, 1, 0, 1, 1, 1, "VAT", 0, 0);
                    GenerateCell(table, 1, 0, 0, 0, 1, 1, 1, GetAddress.Rows[0]["CSTVAT"].ToString() + "%", 0, 0);
                    GenerateCell(table, 1, 1, 1, 0, 1, 1, 1, "", 0, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, "", 0, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 2, 1, Vatamt.ToString(), 0, 0);

                    #endregion

                    #region Bottom

                    #region Total Price

                    GenerateCell(table, 1, 1, 0, 0, 0, 1, 1, "", 0, 0);

                    HSRP.Transaction.InvoiceTransaction.NumToWord trans = new HSRP.Transaction.InvoiceTransaction.NumToWord();
                    string totalinwords = trans.changeNumericToWords(Damount);

                    GenerateCell(table, 8, 1, 1, 0, 0, 2, 1, "Total Invoice Value", 0, 0);

                    GenerateCell(table, 1, 0, 1, 0, 0, 2, 0, dTotalAmount.ToString(), 0, 0);
                    GenerateCell(table, 1, 1, 0, 0, 0, 1, 1, "", 0, 0);
                    GenerateCell(table, 8, 1, 1, 0, 0, 2, 1, "Total RoundOff Invoice Value", 0, 0);
                    GenerateCell(table, 1, 0, 1, 0, 0, 2, 0, Damount.ToString(), 0, 0);
                    #endregion

                    #region Declaration

                    GenerateCell(table, 10, 1, 1, 1, 0, 0, 1, "Declaration: Certified that the particulars given above are true & correct and the amount indicated represents the price actually charged and there is no additional flow of money either directly or indirectly from the buyer.", 0, 0);

                    #endregion

                    #region Vat

                    double dnetval = (Damount * 100.0) / (114.5);

                    double dVatval = Math.Round(Damount - dnetval);

                    string strVatinWords = trans.changeNumericToWords(dVatval);

                    GenerateCell(table, 4, 1, 0, 1, 0, 0, 1, "", 0, 0);
                    GenerateCell(table, 6, 1, 1, 1, 0, 2, 1, "E. & O.E", 0, 0);

                    #endregion




                    #region LinkAutoTech

                    GenerateCell(table, 4, 1, 0, 0, 0, 0, 1, "Total amount in words : " + totalinwords, 0, 0);

                    GenerateCell(table, 6, 1, 1, 0, 0, 2, 0, "For " + GetAddress.Rows[0]["CompanyName"].ToString(), 0, 0);

                    #endregion

                    #region Blank

                    //1

                    for (int i = 0; i < 5; i++)
                    {
                        GenerateCell(table, 4, 1, 0, 0, 0, 1, 0, "", 0, 0);

                        GenerateCell(table, 6, 1, 1, 0, 0, 2, 0, "", 0, 0);

                    }

                    #endregion

                    #region Sign

                    GenerateCell(table, 4, 1, 0, 0, 1, 1, 0, "", 0, 0);

                    GenerateCell(table, 6, 1, 1, 0, 1, 2, 0, "Authorized Signatory", 0, 0);

                    #endregion

                    #endregion


                    GenerateCell(table3, 1, 0, 0, 0, 0, 0, 0, "", 30, 0);
                    GenerateCell(table3, 1, 0, 0, 0, 0, 0, 0, "", 30, 0);
                    GenerateCell(table3, 1, 0, 0, 0, 0, 0, 0, "", 30, 0);
                    GenerateCell(table3, 1, 0, 0, 0, 0, 0, 0, "", 30, 0);

                    GenerateCell(table3, 1, 0, 0, 0, 0, 0, 0, "", 30, 0);
                    GenerateCell(table3, 1, 0, 0, 0, 0, 0, 0, "", 30, 0);
                    GenerateCell(table3, 1, 0, 0, 0, 0, 0, 0, "", 30, 0);
                    GenerateCell(table3, 1, 0, 0, 0, 0, 0, 0, "", 30, 0);

                    GenerateCell(table3, 1, 0, 0, 0, 0, 0, 0, "", 30, 0);
                    GenerateCell(table3, 1, 0, 0, 0, 0, 0, 0, "", 30, 0);
                    GenerateCell(table3, 1, 0, 0, 0, 0, 0, 0, "", 30, 0);
                    GenerateCell(table3, 1, 0, 0, 0, 0, 0, 0, "", 30, 0);

                    // GenerateCell(table3, 2, 0, 0, 0, 0, 0, 1, "Created By :- " + username, 30, 0);

                    GenerateCell(table3, 2, 0, 0, 0, 0, 2, 1, strUserID, 0, 0);
                    //getdate();
                    GridView1.Visible = false;

                    document.Add(table1);
                    document.Add(table);
                    document.Add(table3);
                    document.NewPage();

                    document.Close();


                    context.Response.ContentType = "Application/pdf";
                   context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                   context.Response.WriteFile(PdfFolder);

                   context.Response.End();
                 
               
                }

            }
            catch (Exception ex)
            {


           

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
            }
            table.AddCell(newCellPDF);
        }

    }
}