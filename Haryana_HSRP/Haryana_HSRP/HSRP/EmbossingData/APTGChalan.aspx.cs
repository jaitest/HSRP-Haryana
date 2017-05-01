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
    public partial class APTGChalan : System.Web.UI.Page
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
        DataTable dt = new DataTable();
        StringBuilder sb = new StringBuilder();
        CheckBox chk;
        string  txtTran = string.Empty;

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
                    PanelGrid1.Visible = false;
                    PanelGrid2.Visible = false;
                    btnpdf.Visible = false;
                    btnrecordinpdf.Visible = false;
                    btnChalan.Visible = false;
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

        #region DropDown

        private void FilldropDownListOrganization()
        {
            if (UserType == "0")
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where ActiveStatus='Y' Order by HSRPStateName";
                Utils.PopulateDropDownList(DropDownListStateName, SQLString.ToString(), CnnString, "--Select State--");
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

                SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + intHSRPStateID + " and ActiveStatus!='N'   Order by RTOLocationName";
                Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), CnnString, "--Select RTO Name--");
                //dataLabellbl.Visible = false;
                //TRRTOHide.Visible = false;
            }
            else
            {
                // string UserID = Convert.ToString(Session["UID"]);
                SQLString = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, (a.RTOLocationCode+', ') as RTOCode, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID and a.ActiveStatus ='Y'  where UserRTOLocationMapping.UserID='" + strUserID + "' order by a.rtolocationname ";
                DataTable dss = Utils.GetDataTable(SQLString, CnnString);

                labelOrganization.Visible = true;
                DropDownListStateName.Visible = true;
                labelClient.Visible = true;
                dropDownListClient.Visible = true;

                dropDownListClient.DataSource = dss;
                dropDownListClient.DataBind();
                //dataLabellbl.Visible = true;

                string RTOCode = string.Empty;
                if (dss.Rows.Count > 0)
                {
                    for (int i = 0; i <= dss.Rows.Count - 1; i++)
                    {
                        RTOCode += dss.Rows[i]["RTOCode"].ToString();

                    }
                    //lblRTOCode.Text = RTOCode.Remove(RTOCode.LastIndexOf(","));
                }
            }
        }

        #endregion

        private DataTable GetRecords(string strRecordId)
        {
            string strInvoiceNo = string.Empty;
            DataTable dtInvoiceData = new DataTable();
            string SQLString12 = "select row_number() over(order by VehicleClass,VehicleType,VehicleRegNo) as SN,ChallanNo,challandate,convert(varchar,HSRPRecord_CreationDate,103) as HSRPRecord_CreationDate,VehicleType,convert(varchar,OrderEmbossingDate,103) as OrderEmbossingDate,hsrp_stateid,rtolocationid" +
                                   ",hsrprecordid,vehicleregno,hsrp_rear_lasercode,hsrp_Front_lasercode,OrderStatus " +
                                   "from hsrprecords where hsrp_stateid='" + DropDownListStateName.SelectedValue.ToString() + "' and hsrprecordid=" + strRecordId + "  order by VehicleClass,VehicleType,VehicleRegNo";
            dtInvoiceData = Utils.GetDataTable(SQLString12, CnnString);
            return dtInvoiceData;
        }

        protected void btnGO_Click(object sender, EventArgs e)
        {
            try
            {
                PanelGrid1.Visible = false;
                PanelGrid2.Visible = false;
                Grid1.Visible = false;
                Grid2.Visible = false;
                btnrecordinpdf.Visible = false;
                btnChalan.Visible = false;
                btnpdf.Visible = false;
                LblMessage.Text = "";
                lblErrMsg.Text = "";
                Session["aa"] = "abc";
                string strDate = OrderDate.SelectedDate.ToString("dd/MM/yyyy");
                String[] StringAuthDate = strDate.Replace("-", "/").Split('/');
                string MonTo = ("0" + StringAuthDate[0]);
                string MonthdateTO = MonTo.Replace("00", "0").Replace("01", "1");
                String ReportDateFrom = StringAuthDate[1] + "/" + MonthdateTO + "/" + StringAuthDate[2].Split(' ')[0];
                String From = StringAuthDate[0] + "/" + StringAuthDate[1] + "/" + StringAuthDate[2].Split(' ')[0];
                string AuthorizationDate = From + " 00:00:00";
                String[] StringOrderDate = HSRPAuthDate.SelectedDate.ToString().Replace("-", "/").Split('/');
                string Mon = ("0" + StringOrderDate[0]);
                string Monthdate = Mon.Replace("00", "0").Replace("01", "1");
                String FromDate = StringOrderDate[1] + "-" + Monthdate + "-" + StringOrderDate[2].Split(' ')[0];
                String ReportDateTo = StringOrderDate[1] + "/" + Monthdate + "/" + StringOrderDate[2].Split(' ')[0];
                String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
                string ToDate = From1 + " 23:58:00";

                strFrmDateString = OrderDate.SelectedDate.ToShortDateString() + " 00:00:00";
                strToDateString = HSRPAuthDate.SelectedDate.ToShortDateString() + " 23:58:00";

                ShowGrid(strFrmDateString, strToDateString);
            }
            catch (Exception ex)
            {
                lblErrMsg.Text = ex.Message;
            }
        }

        private void ShowGrid(string strFromdate, string strToDate)
        {
            if (dropDownListClient.SelectedItem.ToString() == "--Select RTO Name--")
            {
                lblErrMsg.Visible = true;
                lblErrMsg.Text = "";
                lblErrMsg.Text = "Please select Location.";
                return;
            }
            else
            {
                if (HSRPStateID == "9" || HSRPStateID == "11")
                {
                    SQLString = "select Row_Number() over(order by orderstatus) as SNo, hsrprecordid,HSRPRecord_AuthorizationNo,vehicleregno,hsrp_rear_lasercode,hsrp_Front_lasercode,OrderStatus from hsrprecords where hsrp_StateID='" + DropDownListStateName.SelectedValue + "' and rtolocationID='" + dropDownListClient.SelectedValue + "' and orderstatus ='Embossing Done' and Erpassigndate between '" + strFromdate + "' and '" + strToDate + "' and isnull(aptgvehrecdate,GETDATE())>dateadd(day,-5,GETDATE()) and Invoice_Flag='N' and hsrp_rear_lasercode is not null and hsrp_Front_lasercode is not null order by VehicleClass,VehicleType,VehicleRegNo";
                }
                else
                {
                    SQLString = "select Row_Number() over(order by orderstatus) as SNo, hsrprecordid,HSRPRecord_AuthorizationNo,vehicleregno,hsrp_rear_lasercode,hsrp_Front_lasercode,OrderStatus from hsrprecords where hsrp_StateID='" + DropDownListStateName.SelectedValue + "' and rtolocationID='" + dropDownListClient.SelectedValue + "' and orderstatus ='Embossing Done' and Erpassigndate between '" + strFromdate + "' and '" + strToDate + "' and Invoice_Flag='N' and hsrp_rear_lasercode is not null and hsrp_Front_lasercode is not null order by VehicleClass,VehicleType,VehicleRegNo";
                }

                dt = Utils.GetDataTable(SQLString, CnnString);
                if (dt.Rows.Count > 0)
                {
                    PanelGrid1.Visible = true;
                    btnChalan.Visible = true;
                    Grid1.Visible = true;
                    Grid1.DataSource = dt;
                    Grid1.DataBind();                                    
                }
                else
                {                   
                    lblErrMsg.Text = "Record not found for the date range selected.";
                    Grid1.DataSource = null;
                    Grid1.DataBind();
                    Grid1.Visible = false;
                    PanelGrid1.Visible = false;
                    btnChalan.Visible = false;
                    btnpdf.Visible = false;
                    btnrecordinpdf.Visible = false;
                    
                    PanelGrid2.Visible = false;
                    Grid2.DataSource = null;
                    Grid2.DataBind();
                    Grid2.Visible = false;                                       
                }
                return;
            }
        }

        protected void CHKSelect1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk1 = Grid1.HeaderRow.FindControl("CHKSelect1") as CheckBox;
            if (chk1.Checked == true)
            {
                for (int i = 0; i < Grid1.Rows.Count; i++)
                {
                    chk = Grid1.Rows[i].Cells[0].FindControl("CHKSelect") as CheckBox;
                    chk.Checked = true;
                }
            }
            else if (chk1.Checked == false)
            {
                for (int i = 0; i < Grid1.Rows.Count; i++)
                {
                    chk = Grid1.Rows[i].Cells[0].FindControl("CHKSelect") as CheckBox;
                    chk.Checked = false;
                }
            }
        }

        public void RefreshPage()
        {
            FilldropDownListClient();
            dropDownListClient.Visible = true;
            labelClient.Visible = true;
            

            Grid1.DataSource = null;
            Grid1.DataBind();
            PanelGrid1.Visible = false;
            PanelGrid2.Visible = false;
            Grid2.DataSource = null;
            Grid2.DataBind();
            Grid2.Visible = false;
            btnChalan.Visible = false;
            btnpdf.Visible = false;            
            btnrecordinpdf.Visible = false;
            LblMessage.Text = "";
            lblErrMsg.Text = "";
        }
                

        protected void DropDownListStateName_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshPage();
        }

        public void OpenNewWindow(string url, string ListClint, string StateName, string txtLorryNo, string txtTransporter)
        {
            url = url + "?ddlValue=" + ListClint + "&ddllistStatename=" + StateName + "&txtLorryNo=" + txtLorryNo + "&txtTransport=" + txtTransporter;
            ClientScript.RegisterStartupScript(this.GetType(), "newWindow", String.Format("<script>window.open('{0}');</script>", url));
        }

        protected void btnChalan_Click(object sender, EventArgs e)
        {
            chalan();
        }

        public void chalan()
        {
            try
            {
                #region Validation
                if (string.IsNullOrEmpty(txtTransporter.Text))
                {
                    lblErrMsg.Text = "Please Enter Transporter .";
                    return;

                }
                if (string.IsNullOrEmpty(txtLorryNo.Text))
                {
                    lblErrMsg.Text = "Please Enter Lorry No. .";
                    return;
                }
                #endregion

                string currentdate = DateTime.Now.ToString("dd/MM/yyyy");
                string RtoName = string.Empty;
                RtoName = dropDownListClient.SelectedItem.ToString();
                HttpContext context = HttpContext.Current;
                
                string SQLString = String.Empty;
                String StringField = String.Empty;
                String StringAlert = String.Empty;

                StringBuilder bb = new StringBuilder();                
                
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
                    if (Grid1.Rows.Count == 0)
                    {
                        lblErrMsg.Text = "No Record Found.";
                        return;
                    }
                    // Validate checked recirds
                    int ChkBoxCount = 0;
                    for (int i = 0; i < Grid1.Rows.Count; i++)
                    {
                        chk = Grid1.Rows[i].Cells[0].FindControl("CHKSelect") as CheckBox;

                        if (chk.Checked == true)
                        {
                            ChkBoxCount = ChkBoxCount + 1;
                        }
                    }
                    if (ChkBoxCount == 0)
                    {
                        lblErrMsg.Text = "Please select atleast 1 record.";
                        return;
                    }
                    string strGetInvoiceNo = string.Empty;
                    string strSelectEmbStation = "SELECT DISTINCT [NAVEMBID],[EmbCenterName],RTOLocationName,city FROM [vw_RTOLocationWiseEmbosingCenters] WHERE RTOLocationId='" + dropDownListClient.SelectedValue + "'";
                    DataTable dtEmbData = Utils.GetDataTable(strSelectEmbStation, CnnString);
                    if (dtEmbData.Rows.Count <= 0)
                    {
                        lblErrMsg.Text = "Embossing Station not found";
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
                    lblErrMsg.Text = "Embossing Station not found";
                    return;
                }

                string strGetFinYear = "SELECT [dbo].[fnGetFiscalYear] ( GetDate() )";
                strInvoiceNo = strInvoiceNo + "/" + (Utils.getScalarValue(strGetFinYear, CnnString)).Replace("20", string.Empty);

                #endregion
                int iChkCount = 0;
                StringBuilder sbx = new StringBuilder();
                for (int i = 0; i < Grid1.Rows.Count; i++)
                {
                    chk = Grid1.Rows[i].Cells[0].FindControl("CHKSelect") as CheckBox;

                    if (chk.Checked == true)
                    {
                        iChkCount = iChkCount + 1;
                        Label lblVehicleRegNo = Grid1.Rows[i].Cells[1].FindControl("lblVehicleRegNo") as Label;

                        Label OrderStatus = Grid1.Rows[i].Cells[4].FindControl("lblOrderStatus") as Label;

                        string strRecordId = Grid1.DataKeys[i]["hsrprecordid"].ToString();
                        if (strHsrpRecordId == "")
                        {
                            strHsrpRecordId = strRecordId;
                        }
                        else
                        {
                            strHsrpRecordId = strHsrpRecordId + "," + strRecordId;
                        }

                        sbx.Append("update hsrprecords set ChallanNo='" + strInvoiceNo + "', ChallanCreatedBy='" + strUserID + "', challandate=getdate(),Invoice_Flag='Y' where  hsrprecordid ='" + strRecordId + "';");
                     }
                }
                if (iChkCount > 0)
                {
                    Utils.ExecNonQuery(sbx.ToString(), CnnString);
                }
                if (HSRPStateID == "9" || HSRPStateID == "11" || HSRPStateID == "4")
                {
                    string strUpdateInvoiceNo = "update hsrpstate set lastno=lastno+1 where hsrp_stateid= '" + HSRPStateID + "' and prefixfor='Cash Receipt No'";
                    Utils.ExecNonQuery(strUpdateInvoiceNo, CnnString);
                }
                else
                {
                    string strUpdateInvoiceNo = "update [EmbossingCenters] set lastno=lastno+1 where [Emb_Center_Id]= '" + strEmbId + "' and prefixfor='Cash Receipt No'";
                    Utils.ExecNonQuery(strUpdateInvoiceNo, CnnString);
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
                Utils.ExecNonQuery(sqlstring, CnnString);

                //////////////////////print in lable//////////////////////////////////////////

                LblMessage.Visible = true;
                LblMessage.Text = "Invoice No--" + strInvoiceNo;
                Session["strInvoiceNo"] = strInvoiceNo;

                ////////////////////////////////////////////////////////////////////////////////////
                PanelGrid1.Visible = false;
                Grid1.Visible = false;
                Grid1.DataSource = null;
                Grid1.DataBind();
                btnChalan.Visible = false;

                //if (HSRPStateID == "9" || HSRPStateID == "11")
                //{
                //    SQLString = "select Row_Number() over(order by orderstatus) as SNo, hsrprecordid,HSRPRecord_AuthorizationNo,vehicleregno,hsrp_rear_lasercode,hsrp_Front_lasercode,OrderStatus from hsrprecords where ChallanNo='" + strInvoiceNo + "' order by VehicleClass,VehicleType,VehicleRegNo";
                //}
                //else
                //{
                //    SQLString = "select Row_Number() over(order by orderstatus) as SNo, hsrprecordid,HSRPRecord_AuthorizationNo,vehicleregno,hsrp_rear_lasercode,hsrp_Front_lasercode,OrderStatus from hsrprecords where ChallanNo='" + strInvoiceNo + "' order by VehicleClass,VehicleType,VehicleRegNo";
                //}

                DataTable gridDT = new DataTable();
                SQLString = "select Row_Number() over(order by orderstatus) as SNo, hsrprecordid,HSRPRecord_AuthorizationNo,vehicleregno,hsrp_rear_lasercode,hsrp_Front_lasercode,OrderStatus from hsrprecords where ChallanNo='" + strInvoiceNo + "' order by SNo,VehicleClass,VehicleType,VehicleRegNo ";
                gridDT = Utils.GetDataTable(SQLString, CnnString);
                if (gridDT.Rows.Count > 0)
                {
                    PanelGrid2.Visible = true;
                    Grid2.Visible = true;
                    Grid2.DataSource = gridDT;
                    Grid2.DataBind();
                    btnpdf.Visible = true;
                    btnrecordinpdf.Visible = true;
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
            }
            table.AddCell(newCellPDF);
        }

        protected void btnrecordinpdf_Click(object sender, EventArgs e)
        {
            summary();
        }

        public void summary()
        {
            string chkb = string.Empty;
            float Amount = 0;
            string filename1 = "Challan" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".pdf";
            String StringField1 = String.Empty;
            String StringAlert1 = String.Empty;
            StringBuilder bb1 = new StringBuilder();
            Document document1 = new Document(new iTextSharp.text.Rectangle(188f, 124f), -30, -30, 8, 0);
            document1.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
            BaseFont bfTimes1 = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
            string PdfFolder1 = ConfigurationManager.AppSettings["PdfFolder"].ToString() + filename1;
            PdfWriter.GetInstance(document1, new FileStream(PdfFolder1, FileMode.Create));

            //Opens the document:
            document1.Open();

            //Adds content to the document:
            PdfPTable table = new PdfPTable(6);
            table.TotalWidth = 6900f;

            GenerateCell(table, 6, 0, 0, 0, 0, 1, 0, "HSRP Order Booking Report", 0, 0);

            GenerateCell(table, 1, 1, 1, 1, 0, 0, 1, "SN", 0, 0);
            GenerateCell(table, 1, 1, 1, 1, 0, 0, 1, "Vehicle No", 0, 0);

            GenerateCell(table, 1, 0, 1, 1, 0, 0, 1, "Vehicle Type", 0, 0);
            GenerateCell(table, 1, 0, 1, 1, 0, 0, 1, "Collection Date", 0, 0);
            GenerateCell(table, 1, 0, 1, 1, 0, 0, 1, "Embossing Date", 0, 0);
            GenerateCell(table, 1, 0, 1, 1, 0, 0, 1, "Invoice No", 0, 0);
            int iRecCount = 0;
            foreach (GridViewRow di in Grid2.Rows)
            {
                List<string> lblvehicleRegNo = new List<string>();
                ArrayList VehicleRegNo = new ArrayList();
                ArrayList LaserFront = new ArrayList();
                ArrayList LaserRear = new ArrayList();

                Label dd = (Label)di.FindControl("lblVehicleRegNo");
                VehicleRegNo.Add(dd.Text);

                TextBox dsd = (TextBox)di.FindControl("txtFLaserCode");
                LaserFront.Add(dsd.Text);
                TextBox ddsd = (TextBox)di.FindControl("txtRlaserCode");
                LaserRear.Add(ddsd.Text);
                string strRecordId = Grid2.DataKeys[di.RowIndex].Value.ToString();
                string vehic = VehicleRegNo[0].ToString();
                DataTable dtData = GetRecords(strRecordId);
                if (dtData.Rows.Count > 0)
                {
                    iRecCount++;
                    string strSN = iRecCount.ToString();
                    string lblVehicleRegNo = dtData.Rows[0]["VehicleRegNo"].ToString();
                    string strVehType = dtData.Rows[0]["VehicleType"].ToString();
                    string strCollectionDate = dtData.Rows[0]["HSRPRecord_CreationDate"].ToString();
                    string strEmbDate = dtData.Rows[0]["OrderEmbossingDate"].ToString();
                    string strInvoiceNo = dtData.Rows[0]["ChallanNo"].ToString();

                    string OrderStatus = dtData.Rows[0]["OrderStatus"].ToString();
                    GenerateCell(table, 1, 1, 1, 1, 1, 0, 1, strSN, 0, 0);
                    GenerateCell(table, 1, 1, 1, 1, 1, 0, 1, lblVehicleRegNo, 0, 0);

                    GenerateCell(table, 1, 0, 1, 1, 1, 0, 1, strVehType, 0, 0);
                    GenerateCell(table, 1, 0, 1, 1, 1, 0, 1, strCollectionDate, 0, 0);
                    GenerateCell(table, 1, 0, 1, 1, 1, 0, 1, strEmbDate, 0, 0);

                    GenerateCell(table, 1, 0, 1, 1, 1, 0, 1, strInvoiceNo, 0, 0);
                }
            }
            if (iRecCount > 0)
            {
                document1.Add(table);
                document1.Close();
                HttpContext context1 = HttpContext.Current;
                context1.Response.ContentType = "Application/pdf";
                context1.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename1);
                context1.Response.WriteFile(PdfFolder1);
                context1.Response.End();
            }
            else
            {
                lblErrMsg.Text = "Selected  records have not been Embossed";
                return;
            }
        }

        protected void dropDownListClient_SelectedIndexChanged(object sender, EventArgs e)
        {
           // RefreshPage();

            Grid1.DataSource = null;
            Grid1.DataBind();
            PanelGrid1.Visible = false;
            PanelGrid2.Visible = false;
            Grid2.DataSource = null;
            Grid2.DataBind();
            Grid2.Visible = false;
            btnChalan.Visible = false;
            btnpdf.Visible = false;
            btnrecordinpdf.Visible = false;
            LblMessage.Text = "";
            lblErrMsg.Text = "";
        }

        protected void Grid1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Grid1.PageIndex = e.NewPageIndex;
            try
            {
                string strDate = OrderDate.SelectedDate.ToString("dd/MM/yyyy");
                String[] StringAuthDate = strDate.Replace("-", "/").Split('/');
                string MonTo = ("0" + StringAuthDate[0]);
                string MonthdateTO = MonTo.Replace("00", "0").Replace("01", "1");
                String ReportDateFrom = StringAuthDate[1] + "/" + MonthdateTO + "/" + StringAuthDate[2].Split(' ')[0];
                String From = StringAuthDate[0] + "/" + StringAuthDate[1] + "/" + StringAuthDate[2].Split(' ')[0];
                string AuthorizationDate = From + " 00:00:00";
                String[] StringOrderDate = HSRPAuthDate.SelectedDate.ToString().Replace("-", "/").Split('/');
                string Mon = ("0" + StringOrderDate[0]);
                string Monthdate = Mon.Replace("00", "0").Replace("01", "1");
                String FromDate = StringOrderDate[1] + "-" + Monthdate + "-" + StringOrderDate[2].Split(' ')[0];
                String ReportDateTo = StringOrderDate[1] + "/" + Monthdate + "/" + StringOrderDate[2].Split(' ')[0];
                String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
                string ToDate = From1 + " 23:58:00";

                strFrmDateString = OrderDate.SelectedDate.ToShortDateString() + " 00:00:00";
                strToDateString = HSRPAuthDate.SelectedDate.ToShortDateString() + " 23:58:00";

                ShowGrid(strFrmDateString, strToDateString);
            }
            catch (Exception ex)
            {
                lblErrMsg.Text = ex.Message;
            }
          
        }

        protected void btnpdf_Click(object sender, EventArgs e)
        {
            generatepdffile();
        }

        public void generatepdffile()
        {           
            try
            {
                string currentdate = DateTime.Now.ToString("dd/MM/yyyy");
                string RtoName = string.Empty;
                RtoName = dropDownListClient.SelectedItem.ToString();
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
                    string strGetInvoiceNo = string.Empty;
                    string strSelectEmbStation = "SELECT DISTINCT [NAVEMBID],[EmbCenterName],RTOLocationName,city FROM [vw_RTOLocationWiseEmbosingCenters] WHERE RTOLocationId='" + dropDownListClient.SelectedValue + "'";
                    DataTable dtEmbData = Utils.GetDataTable(strSelectEmbStation, CnnString);
                    if (dtEmbData.Rows.Count <= 0)
                    {
                        lblErrMsg.Text = "Embossing Station not found";
                        return;
                    }
                    strEmbStationName = dtEmbData.Rows[0]["EmbCenterName"].ToString();
                    strEmbAddress = dtEmbData.Rows[0]["RTOLocationName"].ToString();
                    strEmbCity = dtEmbData.Rows[0]["city"].ToString();
                    strEmbId = dtEmbData.Rows[0]["NAVEMBID"].ToString();                    
                }
                catch
                {
                    lblErrMsg.Text = "Embossing Station not found";
                    return;
                }

                strInvoiceNo = Session["strInvoiceNo"].ToString();

                #endregion
               
                StringBuilder sbx = new StringBuilder();
                for (int i = 0; i < Grid2.Rows.Count; i++)
                {      
                    string strRecordId = Grid2.DataKeys[i]["hsrprecordid"].ToString();
                    if (strHsrpRecordId == "")
                    {
                        strHsrpRecordId = strRecordId;
                    }
                    else
                    {
                        strHsrpRecordId = strHsrpRecordId + "," + strRecordId;
                    } 
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

                string strSelectAddress = "SELECT RTOLocationAddress FROM [RTOLocation] WHERE RTOLocationId='" + dropDownListClient.SelectedValue + "'";
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
               
                decimal dTotalAmount = 0;
                if (true)
                {
                    #region Upper Part of PDF

                    string strStateId = DropDownListStateName.SelectedValue;
                    string strLocId = dropDownListClient.SelectedValue;

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
                    if (DropDownListStateName.SelectedValue.Equals("9"))
                    {
                        strPhone = "Phone No : 04042226000";
                    }
                    else if (DropDownListStateName.SelectedValue.Equals("2"))
                    {

                    }
                    else if (DropDownListStateName.SelectedValue.Equals("4"))
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
                    else if (DropDownListStateName.SelectedValue.ToString() == "9")
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
                    GenerateCell(table, 7, 0, 1, 0, 0, 0, 0, "Transporter : " + txtTransporter.Text, 0, 0);

                    GenerateCell(table, 3, 1, 1, 1, 0, 0, 0, "", 0, 0);
                    GenerateCell(table, 7, 0, 1, 1, 0, 0, 0, "Lorry No : " + txtLorryNo.Text, 0, 0);

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
                    if (DropDownListStateName.SelectedValue.ToString() == "9")
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

                    if (string.IsNullOrEmpty(ExicseAmount))
                    {
                        ExicseAmount = "0";
                    }
                    if (string.IsNullOrEmpty(Newamount))
                    {
                        Newamount = "0";
                    } 
                    if (string.IsNullOrEmpty(strEducess))
                    {
                        strEducess = "0";
                    }
                    if (string.IsNullOrEmpty(strSecondaryCess))
                    {
                        strSecondaryCess = "0";
                    }
                    if (string.IsNullOrEmpty(VatamtNew))
                    {
                        VatamtNew = "0";
                    }

                    decimal PreTotalAmount = 0;
                    PreTotalAmount = PreTotalAmount + Convert.ToDecimal(ExicseAmount) + Convert.ToDecimal(Newamount) + Convert.ToDecimal(strEducess) + Convert.ToDecimal(strSecondaryCess);
                    dTotalAmount = PreTotalAmount + Convert.ToDecimal(VatamtNew);

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

                    //GenerateCell(table, 6, 1, 1, 0, 1, 2, 0, "Authorized Signatory", 0, 0);
                    GenerateCell(table, 6, 1, 1, 0, 1, 2, 0, "", 0, 0);

                    #endregion

                    #endregion


                    //GenerateCell(table3, 1, 0, 0, 0, 0, 0, 0, "", 30, 0);
                    //GenerateCell(table3, 1, 0, 0, 0, 0, 0, 0, "", 30, 0);
                    //GenerateCell(table3, 1, 0, 0, 0, 0, 0, 0, "", 30, 0);
                    //GenerateCell(table3, 1, 0, 0, 0, 0, 0, 0, "", 30, 0);

                    //GenerateCell(table3, 1, 0, 0, 0, 0, 0, 0, "", 30, 0);
                    //GenerateCell(table3, 1, 0, 0, 0, 0, 0, 0, "", 30, 0);
                    //GenerateCell(table3, 1, 0, 0, 0, 0, 0, 0, "", 30, 0);
                    //GenerateCell(table3, 1, 0, 0, 0, 0, 0, 0, "", 30, 0);

                    GenerateCell(table3, 1, 0, 0, 0, 0, 0, 0, "", 30, 0);
                    GenerateCell(table3, 1, 0, 0, 0, 0, 0, 0, "", 30, 0);
                    GenerateCell(table3, 1, 0, 0, 0, 0, 0, 0, "", 30, 0);
                    GenerateCell(table3, 1, 0, 0, 0, 0, 0, 0, "", 30, 0);

                    // GenerateCell(table3, 2, 0, 0, 0, 0, 0, 1, "Created By :- " + username, 30, 0);

                    //GenerateCell(table3, 2, 0, 0, 0, 0, 2, 1, strUserID, 0, 0);

                    Grid1.Visible = false;

                    document.Add(table1);
                    document.Add(table);
                    document.Add(table3);
                    document.NewPage();


                    /////////////////////////by jay//////////////////////////////////////////////

                    PdfPTable table4 = new PdfPTable(6);
                    table4.TotalWidth = 7000f;
                    GenerateCell(table4, 6, 0, 0, 0, 0, 1, 0, "HSRP Order Booking Report", 0, 0);
                    GenerateCell(table4, 6, 0, 0, 0, 0, 0, 0, "", 0, 0);
                    GenerateCell(table4, 6, 0, 0, 0, 0, 0, 0, "", 0, 0);


                    GenerateCell(table4, 1, 1, 1, 1, 0, 0, 1, "SN", 0, 0);
                    GenerateCell(table4, 1, 1, 1, 1, 0, 0, 1, "Vehicle No", 0, 0);

                    GenerateCell(table4, 1, 0, 1, 1, 0, 0, 1, "Vehicle Type", 0, 0);
                    GenerateCell(table4, 1, 0, 1, 1, 0, 0, 1, "Collection Date", 0, 0);
                    GenerateCell(table4, 1, 0, 1, 1, 0, 0, 1, "Embossing Date", 0, 0);
                    GenerateCell(table4, 1, 0, 1, 1, 0, 0, 1, "Invoice No", 0, 0);
                    int iRecCount = 0;
                    foreach (GridViewRow di in Grid2.Rows)
                    {
                        List<string> lblvehicleRegNo = new List<string>();
                        ArrayList VehicleRegNo = new ArrayList();
                        ArrayList LaserFront = new ArrayList();
                        ArrayList LaserRear = new ArrayList();

                        Label dd = (Label)di.FindControl("lblVehicleRegNo");
                        VehicleRegNo.Add(dd.Text);

                        TextBox dsd = (TextBox)di.FindControl("txtFLaserCode");
                        LaserFront.Add(dsd.Text);
                        TextBox ddsd = (TextBox)di.FindControl("txtRlaserCode");
                        LaserRear.Add(ddsd.Text);
                        string strRecordId = Grid2.DataKeys[di.RowIndex].Value.ToString();
                        string vehic = VehicleRegNo[0].ToString();
                        DataTable dtData = GetRecords(strRecordId);
                        if (dtData.Rows.Count > 0)
                        {
                            iRecCount++;
                            string strSN = iRecCount.ToString();
                            string lblVehicleRegNo = dtData.Rows[0]["VehicleRegNo"].ToString();
                            string strVehType = dtData.Rows[0]["VehicleType"].ToString();
                            string strCollectionDate = dtData.Rows[0]["HSRPRecord_CreationDate"].ToString();
                            string strEmbDate = dtData.Rows[0]["OrderEmbossingDate"].ToString();
                            //string strInvoiceNo = dtData.Rows[0]["ChallanNo"].ToString();

                            string OrderStatus = dtData.Rows[0]["OrderStatus"].ToString();
                            GenerateCell(table4, 1, 1, 1, 1, 1, 0, 1, strSN, 0, 0);
                            GenerateCell(table4, 1, 1, 1, 1, 1, 0, 1, lblVehicleRegNo, 0, 0);

                            GenerateCell(table4, 1, 0, 1, 1, 1, 0, 1, strVehType, 0, 0);
                            GenerateCell(table4, 1, 0, 1, 1, 1, 0, 1, strCollectionDate, 0, 0);
                            GenerateCell(table4, 1, 0, 1, 1, 1, 0, 1, strEmbDate, 0, 0);
                            GenerateCell(table4, 1, 0, 1, 1, 1, 0, 1, strInvoiceNo, 0, 0);
                        }
                    }
                    ///by jay
                    GenerateCell(table4, 6, 0, 0, 0, 0, 0, 0, "", 0, 0);
                    GenerateCell(table4, 6, 0, 0, 0, 0, 0, 0, "", 0, 0);
                    GenerateCell(table4, 6, 0, 0, 0, 0, 0, 0, "", 0, 0);
                    GenerateCell(table4, 6, 0, 0, 0, 0, 0, 0, "", 0, 0);
                    GenerateCell(table4, 6, 0, 0, 0, 0, 0, 0, "", 0, 0);
                    GenerateCell(table4, 6, 0, 0, 0, 0, 0, 0, "", 0, 0);

                    GenerateCell(table4, 6, 0, 0, 0, 0, 2, 0, "Authorized Signatory", 0, 0);

                    GenerateCell(table4, 6, 0, 0, 0, 0, 0, 0, "", 0, 0);
                    GenerateCell(table4, 6, 0, 0, 0, 0, 0, 0, "", 0, 0);
                    GenerateCell(table4, 6, 0, 0, 0, 0, 0, 0, "", 0, 0);
                    GenerateCell(table4, 6, 0, 0, 0, 0, 0, 0, "", 0, 0);
                    GenerateCell(table4, 6, 0, 0, 0, 0, 0, 0, "", 0, 0);
                    GenerateCell(table4, 6, 0, 0, 0, 0, 0, 0, "", 0, 0);
                    GenerateCell(table4, 6, 0, 0, 0, 0, 0, 0, "", 0, 0);
                    GenerateCell(table4, 6, 0, 0, 0, 0, 0, 0, "", 0, 0);

                    GenerateCell(table4, 6, 0, 0, 0, 0, 2, 1, "Created By :- " + strUserID, 0, 0);

                    document.Add(table4);
                    document.NewPage();

                    ///////////////////////////////////////////////////////////////////////////

                    document.Close();

                    context.Response.ContentType = "Application/pdf";
                    context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                    context.Response.WriteFile(PdfFolder);

                    context.Response.End();

                }

            }
            catch (Exception ex)
            {
                lblErrMsg.Text = ex.Message;
            }
        }

       
   
    }
}