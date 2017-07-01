﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Text;
using System.Configuration;
using System.IO;
using System.Data;
using DataProvider;


namespace HSRP.Transaction
{
    public partial class ViewSearch_stagingarearecord : System.Web.UI.Page
    {
        

        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        int UserType;
        int HSRPStateID;
        int RTOLocationID;



        String ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();

        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {
                RTOLocationID = Convert.ToInt32(Session["UserRTOLocationID"].ToString());
                UserType = Convert.ToInt32(Session["UserType"].ToString());
                HSRPStateID = Convert.ToInt32(Session["UserHSRPStateID"].ToString());
                             

                lblErrMsg.Text = string.Empty;
                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                strUserID = Session["UID"].ToString();
                ComputerIP = Request.UserHostAddress;

                if (!IsPostBack)
                {
                    try
                    {
                        FilldropDownListHSRPState();
                        dropDownListHSRPState.Visible = false;
                        labelHSRPState.Visible = false;
                    }
                    catch (Exception err)
                    {
                        lblErrMsg.Text = "Error on Page Load" + err.Message.ToString();
                    }
                }
            }
        }


     
        #region DropDown

        private void FilldropDownListHSRPState()
        {
            if (UserType.Equals(0))
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState where ActiveStatus='Y' Order by HSRPStateName";
                Utils.PopulateDropDownList(dropDownListHSRPState, SQLString.ToString(), CnnString, "--Select State--");
            }
            else
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID=" + HSRPStateID + " and ActiveStatus='Y' Order by HSRPStateName";
                DataSet dts = Utils.getDataSet(SQLString, CnnString);
                dropDownListHSRPState.DataSource = dts;
                dropDownListHSRPState.DataBind();
            }
        }


        #endregion


        #region Grid
        public void buildGrid()
        {

        }
        public void OnNeedRebind(object sender, EventArgs oArgs)
        {
            System.Threading.Thread.Sleep(200);
            Grid1.DataBind();
        }
        public void OnNeedDataSource(object sender, EventArgs oArgs)
        {
            buildGrid();
        }
        public void OnPageChanged(object sender, ComponentArt.Web.UI.GridPageIndexChangedEventArgs oArgs)
        {
            Grid1.CurrentPageIndex = oArgs.NewIndex;
        }
        public void OnFilter(object sender, ComponentArt.Web.UI.GridFilterCommandEventArgs oArgs)
        {
            Grid1.Filter = oArgs.FilterExpression;
        }
        public void OnSort(object sender, ComponentArt.Web.UI.GridSortCommandEventArgs oArgs)
        {
            Grid1.Sort = oArgs.SortExpression;
        }
        private void ddGridRunningMode_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            Grid1.RunningMode = (ComponentArt.Web.UI.GridRunningMode)Enum.Parse(typeof(ComponentArt.Web.UI.GridRunningMode), "Client");
            buildGrid();
            Grid1.DataBind();
            adjustToRunningMode();
        }
        public void OnGroup(object sender, ComponentArt.Web.UI.GridGroupCommandEventArgs oArgs)
        {
            Grid1.GroupBy = oArgs.GroupExpression;
        }
        private void adjustToRunningMode()
        {

            Grid1.SliderPopupClientTemplateId = "SliderTemplate";
            Grid1.SliderPopupOffsetX = 20;

        }
        #endregion

        #region openfile
        protected void ReadFile(string path)
        {
            // Get the physical Path of the file
            string filepath = path;

            // Create New instance of FileInfo class to get the properties of the file being downloaded
            FileInfo file = new FileInfo(filepath);

            // Checking if file exists
            if (file.Exists)
            {
                // Clear the content of the response
                Response.ClearContent();

                // LINE1: Add the file name and attachment, which will force the open/cance/save dialog to show, to the header
                //Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                Response.AddHeader("Content-Disposition", "inline; filename=" + file.Name);


                // Add the file size into the response header
                Response.AddHeader("Content-Length", file.Length.ToString());

                // Set the ContentType
                Response.ContentType = ReturnExtension(file.Extension.ToLower());

                // Write the file into the response (TransmitFile is for ASP.NET 2.0. In ASP.NET 1.1 you have to use WriteFile instead)
                Response.TransmitFile(file.FullName);

                // End the response
                Response.End();
            }

        }

        private string ReturnExtension(string fileExtension)
        {
            switch (fileExtension)
            {
                case ".htm":
                case ".html":
                case ".log":
                    return "text/HTML";
                case ".txt":
                    return "text/plain";
                case ".docx":
                    return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                case ".doc":
                    return "application/msword";
                case ".tiff":
                case ".tif":
                    return "image/tiff";
                case ".asf":
                    return "video/x-ms-asf";
                case ".avi":
                    return "video/avi";
                case ".zip":
                    return "application/zip";
                case ".xls":
                case ".csv":
                    return "application/vnd.ms-excel";
                case ".gif":
                    return "image/gif";
                case ".jpg":
                case "jpeg":
                    return "image/jpeg";
                case ".bmp":
                    return "image/bmp";
                case ".wav":
                    return "audio/wav";
                case ".mp3":
                    return "audio/mpeg3";
                case ".mpg":
                case "mpeg":
                    return "video/mpeg";
                case ".rtf":
                    return "application/rtf";
                case ".asp":
                    return "text/asp";
                case ".pdf":
                    return "application/pdf";
                case ".fdf":
                    return "application/vnd.fdf";
                case ".ppt":
                    return "application/mspowerpoint";
                case ".dwg":
                    return "image/vnd.dwg";
                case ".msg":
                    return "application/msoutlook";
                case ".xml":
                case ".sdxl":
                    return "application/xml";
                case ".xdp":
                    return "application/vnd.adobe.xdp+xml";
                default:
                    return "application/octet-stream";
            }

        }
        #endregion
        protected void Grid1_ItemCommand(object sender, ComponentArt.Web.UI.GridItemCommandEventArgs e)
        {
            String HSRPRecordID = e.Item["HSRPRecordID"].ToString();
            string OrderStatus = e.Item["OrderStatus"].ToString();
            string PP = e.Control.ID.ToString();


            DataTable GetAddress;

            string Address;
            GetAddress = Utils.GetDataTable("select * from HSRPState WHERE HSRP_StateID='" + HSRPStateID + "'", CnnString);



            if (GetAddress.Rows[0]["pincode"] != "" || GetAddress.Rows[0]["pincode"] != null)
            {
                Address = " - " + GetAddress.Rows[0]["pincode"];
            }
            else
            {
                Address = "";
            }

            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            // string Invoice = Grid1.Items[e].DataItem.ToString();
            // string _tempvalue = ((LinkButton)Grid1.Rows[row.RowIndex].FindControl("LinkButtonStatus")).Text;
            if (String.IsNullOrEmpty(HSRPRecordID))
            {
                lblErrMsg.Text = "That is not valid Record.";
                return;
            }


            if (e.Control.ID.ToString() == "LinkButtonCashReceipt")
            {
                DataTable dataSetFillHSRPDeliveryChallan = new DataTable();
                BAL obj = new BAL();
                if (obj.FillHSRPRecordDeliveryChallan(HSRPRecordID, ref dataSetFillHSRPDeliveryChallan))
                {
                    if (dataSetFillHSRPDeliveryChallan.Rows.Count < 1)
                    {
                        lblErrMsg.Text = "No Such Record Exists.";
                        return;
                    }


                    string rtoid = "select r.rtolocationname  as RtoLocation,r.rtolocationid  from hsrprecords as h inner join rtolocation as r on r.rtolocationid=h.rtolocationid where hsrprecordid='" + HSRPRecordID + "'";
                    DataTable Rtlocation = Utils.GetDataTable(rtoid, CnnString);
                    string RtoLocationName = Rtlocation.Rows[0]["RtoLocation"].ToString();
                    string RtoLocationID = Rtlocation.Rows[0]["RTOLocationID"].ToString();

                    // string AffixactionCenter = "select * from AffixationCenters";

                    //string username = " select * from users where UserID='" + strUserID + "'";

                    string username = "select b.UserFirstName,b.UserLastName,b.UserLoginName from hsrprecords a,users b where a.CreatedBy=b.userid and a.hsrprecordid='" + HSRPRecordID + "'";

                    DataTable dtusername = Utils.GetDataTable(username, CnnString);
                    //string userloginname=dtusername.Rows[0]["UserLoginName"].ToString();

                    string userfirstname = dtusername.Rows[0]["userfirstname"].ToString();

                    string userlastname = dtusername.Rows[0]["userlastname"].ToString();

                    string user = userfirstname + userlastname;

                    String strquery1 = "select Address1 from AffixationCenters where Affix_Id='" + dataSetFillHSRPDeliveryChallan.Rows[0]["Affix_id"].ToString() + "'";

                    string rtoaddress = Utils.getDataSingleValue(strquery1, ConnectionString, "Address1");



                    //string filename = "CASH RECEIPT-" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".pdf";
                    string filename = "CASHRECEIPT_No-" + dataSetFillHSRPDeliveryChallan.Rows[0]["CashReceiptNo"].ToString().Replace("/", "-") + ".pdf";


                    string SQLString = String.Empty;
                    String StringField = String.Empty;
                    String StringAlert = String.Empty;

                    StringBuilder bb = new StringBuilder();

                    //Creates an instance of the iTextSharp.text.Document-object:
                    Document document = new Document();

                    BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);

                    // iTextSharp.text.Font times = new Font(bfTimes, 12, Font.ITALIC, Color.Black);
                    //Creates a Writer that listens to this document and writes the document to the Stream of your choice:
                    string PdfFolder = ConfigurationManager.AppSettings["PdfFolder"].ToString() + filename;
                    PdfWriter.GetInstance(document, new FileStream(PdfFolder, FileMode.Create));

                    //Opens the document:
                    document.Open();

                    //Adds content to the document:
                    // document.Add(new Paragraph("Ignition Log Report"));
                    PdfPTable table = new PdfPTable(4);

                    //actual width of table in points
                    table.TotalWidth = 800f;

                    //fix the absolute width of the table

                    if (HSRPStateID == 1)
                    {
                        string company = "UTSAV SAFETY SYSTEM PVT. LTD. In consitium with Linkpoint Infrastructure Pvt. Ltd.";
                        PdfPCell cell123 = new PdfPCell(new Phrase("" + company, new iTextSharp.text.Font(bfTimes, 12f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell123.Colspan = 4;
                        cell123.BorderColor = BaseColor.WHITE;
                        cell123.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell123);

                    }
                    else
                    {
                        PdfPCell cell12 = new PdfPCell(new Phrase("" + GetAddress.Rows[0]["CompanyName"].ToString(), new iTextSharp.text.Font(bfTimes, 12f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell12.Colspan = 4;
                        cell12.BorderColor = BaseColor.WHITE;
                        cell12.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell12);
                    }


                    DataTable GetAddressukbr = new DataTable();
                    if (HSRPStateID == 6 || HSRPStateID == 1)
                    {
                        GetAddressukbr = Utils.GetDataTable("select rtolocationid,rtolocationname,rtolocationaddress from rtolocation WHERE HSRP_StateID='" + HSRPStateID + "' and rtolocationid='" + RTOLocationID + "'", CnnString);

                    }

                    // string affixation="For affixation kindly visit";

                    //PdfPCell cell1203 = new PdfPCell(new Phrase("For Affixation kindly visit :-  " + GetAddressukbr.Rows[0]["rtolocationname"].ToString() + " , " + GetAddressukbr.Rows[0]["rtolocationaddress"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    //cell1203.Colspan = 4;
                    //cell1203.BorderWidthLeft = 0f;
                    //cell1203.BorderWidthRight = 0f;
                    //cell1203.BorderWidthTop = 0f;
                    //cell1203.BorderWidthBottom = 0f;
                    //cell1203.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    //table.AddCell(cell1203);

                    PdfPCell cell = new PdfPCell(new Phrase("Duplicate  ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell.Colspan = 4;
                    cell.BorderColor = BaseColor.WHITE;
                    cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell);




                    string getTinNonew = Utils.getScalarValue("select TinNo from hsrpstate where HSRP_StateID='" + HSRPStateID + "' ", CnnString);
                    PdfPCell cellTinNo = new PdfPCell(new Phrase("TIN NO." + getTinNonew.ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cellTinNo.Colspan = 4;
                    cellTinNo.BorderColor = BaseColor.WHITE;
                    cellTinNo.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cellTinNo);



                    if (HSRPStateID == 6 || HSRPStateID == 1)
                    {
                        PdfPCell cellRtoLocation = new PdfPCell(new Phrase("RTOLocation :" + GetAddressukbr.Rows[0]["rtolocationname"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cellRtoLocation.Colspan = 4;
                        cellRtoLocation.BorderColor = BaseColor.WHITE;
                        cellRtoLocation.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cellRtoLocation);
                    }






                    PdfPCell cell0 = new PdfPCell(new Phrase("HSRP CASH RECEIPT", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.UNDERLINE, iTextSharp.text.BaseColor.BLACK)));
                    cell0.Colspan = 4;
                    cell0.BorderWidthLeft = 0f;
                    cell0.BorderWidthRight = 0f;
                    cell0.BorderWidthTop = 0f;
                    cell0.BorderWidthBottom = 0f;

                    cell0.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell0);


                    PdfPCell cell1 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell1.Colspan = 4;
                    cell1.BorderWidthLeft = 0f;
                    cell1.BorderWidthRight = 0f;
                    cell1.BorderWidthTop = 0f;
                    cell1.BorderWidthBottom = 0f;

                    cell1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell1);




                    PdfPCell cellInv2 = new PdfPCell(new Phrase("CASH RECEIPT No.", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cellInv2.Colspan = 1;

                    cellInv2.BorderWidthLeft = 0f;
                    cellInv2.BorderWidthRight = 0f;
                    cellInv2.BorderWidthTop = 0f;
                    cellInv2.BorderWidthBottom = 0f;
                    cellInv2.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cellInv2);



                    PdfPCell cellInv22111 = new PdfPCell(new Phrase(": " + dataSetFillHSRPDeliveryChallan.Rows[0]["CashReceiptNo"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cellInv22111.Colspan = 1;
                    cellInv22111.BorderWidthLeft = 0f;
                    cellInv22111.BorderWidthRight = 0f;
                    cellInv22111.BorderWidthTop = 0f;
                    cellInv22111.BorderWidthBottom = 0f;
                    cellInv22111.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cellInv22111);


                    PdfPCell cell21 = new PdfPCell(new Phrase("DATE", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell21.Colspan = 1;

                    cell21.BorderWidthLeft = 0f;
                    cell21.BorderWidthRight = 0f;
                    cell21.BorderWidthTop = 0f;
                    cell21.BorderWidthBottom = 0f;
                    cell21.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell21);
                    string CashReceiptDateTime = string.Empty;

                    if (dataSetFillHSRPDeliveryChallan.Rows[0]["HSRPRecord_CreationDate"].ToString() == "")
                    {
                        CashReceiptDateTime = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        CashReceiptDateTime = Convert.ToDateTime(dataSetFillHSRPDeliveryChallan.Rows[0]["HSRPRecord_CreationDate"]).ToString("dd/MM/yyyy");
                    }
                    PdfPCell cell212 = new PdfPCell(new Phrase(": " + CashReceiptDateTime, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell212.Colspan = 1;

                    cell212.BorderWidthLeft = 0f;
                    cell212.BorderWidthRight = 0f;
                    cell212.BorderWidthTop = 0f;
                    cell212.BorderWidthBottom = 0f;
                    cell212.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell212);



                    PdfPCell cell2 = new PdfPCell(new Phrase("TIN NO.", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell2.Colspan = 1;

                    cell2.BorderWidthLeft = 0f;
                    cell2.BorderWidthRight = 0f;
                    cell2.BorderWidthTop = 0f;
                    cell2.BorderWidthBottom = 0f;
                    cell2.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell2);

                    string getTinNo = Utils.getScalarValue("select TinNo from hsrpstate where HSRP_StateID='" + HSRPStateID + "' ", CnnString);

                    PdfPCell cell22111 = new PdfPCell(new Phrase(": " + getTinNo.ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell22111.Colspan = 1;
                    cell22111.BorderWidthLeft = 0f;
                    cell22111.BorderWidthRight = 0f;
                    cell22111.BorderWidthTop = 0f;
                    cell22111.BorderWidthBottom = 0f;
                    cell22111.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell22111);




                    PdfPCell cell22 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell22.Colspan = 1;

                    cell22.BorderWidthLeft = 0f;
                    cell22.BorderWidthRight = 0f;
                    cell22.BorderWidthTop = 0f;
                    cell22.BorderWidthBottom = 0f;
                    cell22.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell22);

                    PdfPCell cell222 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell222.Colspan = 1;

                    cell222.BorderWidthLeft = 0f;
                    cell222.BorderWidthRight = 0f;
                    cell222.BorderWidthTop = 0f;
                    cell222.BorderWidthBottom = 0f;
                    cell222.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell222);


                    //PdfPCell cell3 = new PdfPCell(new Phrase("USER NAME", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    //cell3.Colspan = 1;
                    //// table.WidthPercentage = 25;
                    //cell3.BorderWidthLeft = 0f;
                    //cell3.BorderWidthRight = 0f;
                    //cell3.BorderWidthTop = 0f;
                    //cell3.BorderWidthBottom = 0f;
                    //cell3.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    //table.AddCell(cell3);

                    //PdfPCell cell4 = new PdfPCell(new Phrase(": " + dataSetFillHSRPDeliveryChallan.Rows[0]["OwnerName"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    //cell4.Colspan = 3;
                    //table.WidthPercentage = 80;
                    //cell4.BorderWidthLeft = 0f;
                    //cell4.BorderWidthRight = 0f;
                    //cell4.BorderWidthTop = 0f;
                    //cell4.BorderWidthBottom = 0f;
                    //cell4.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    //table.AddCell(cell4);






                    PdfPCell cell15 = new PdfPCell(new Phrase("________________________________________________________________________", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell15.Colspan = 4;
                    cell15.BorderWidthLeft = 0f;
                    cell15.BorderWidthRight = 0f;
                    cell15.BorderWidthTop = 0f;
                    cell15.BorderWidthBottom = 0f;
                    table.AddCell(cell15);


                    PdfPCell cell5 = new PdfPCell(new Phrase("AUTH NO.", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell5.Colspan = 1;

                    cell5.BorderWidthLeft = 0f;
                    cell5.BorderWidthRight = 0f;
                    cell5.BorderWidthTop = 0f;
                    cell5.BorderWidthBottom = 0f;
                    cell5.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell5);

                    PdfPCell cell55 = new PdfPCell(new Phrase(": " + dataSetFillHSRPDeliveryChallan.Rows[0]["HSRPRecord_AuthorizationNo"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell55.Colspan = 1;

                    cell55.BorderWidthLeft = 0f;
                    cell55.BorderWidthRight = 0f;
                    cell55.BorderWidthTop = 0f;
                    cell55.BorderWidthBottom = 0f;
                    cell55.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell55);

                    PdfPCell cell25 = new PdfPCell(new Phrase("AUTH. DATE", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell25.Colspan = 1;

                    cell25.BorderWidthLeft = 0f;
                    cell25.BorderWidthRight = 0f;
                    cell25.BorderWidthTop = 0f;
                    cell25.BorderWidthBottom = 0f;
                    cell25.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell25);

                    DateTime AuthDate = Convert.ToDateTime(dataSetFillHSRPDeliveryChallan.Rows[0]["HSRPRecord_AuthorizationDate"].ToString());
                    PdfPCell cell255 = new PdfPCell(new Phrase(": " + AuthDate.ToString("dd/MM/yyyy"), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell255.Colspan = 1;

                    cell255.BorderWidthLeft = 0f;
                    cell255.BorderWidthRight = 0f;
                    cell255.BorderWidthTop = 0f;
                    cell255.BorderWidthBottom = 0f;
                    cell255.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell255);




                    PdfPCell cell6 = new PdfPCell(new Phrase("ORDER BOOKING NO", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell6.Colspan = 1;
                    cell6.BorderWidthLeft = 0f;
                    cell6.BorderWidthRight = 0f;
                    cell6.BorderWidthTop = 0f;
                    cell6.BorderWidthBottom = 0f;
                    cell6.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell6);

                    PdfPCell cell65 = new PdfPCell(new Phrase(": " + dataSetFillHSRPDeliveryChallan.Rows[0]["OrderNo"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell65.Colspan = 1;
                    cell65.BorderWidthLeft = 0f;
                    cell65.BorderWidthRight = 0f;
                    cell65.BorderWidthTop = 0f;
                    cell65.BorderWidthBottom = 0f;
                    cell65.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell65);


                    if (Session["UserHSRPStateID"].ToString() == "4")
                    {
                        try
                        {


                            PdfPCell cell26 = new PdfPCell(new Phrase("Affixation Date", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            //PdfPCell cell26 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell26.Colspan = 1;
                            cell26.BorderWidthLeft = 0f;
                            cell26.BorderWidthRight = 0f;
                            cell26.BorderWidthTop = 0f;
                            cell26.BorderWidthBottom = 0f;
                            cell26.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell26);

                            DateTime PlateAffixationDateother = Convert.ToDateTime(dataSetFillHSRPDeliveryChallan.Rows[0]["OrderDate"]);
                            DateTime PlateAffixationDate = PlateAffixationDateother.AddDays(4);
                            string[] dat = PlateAffixationDate.ToString().Split(' ');
                            string d = dat[0].ToString();
                            if (d != "1/1/1900")
                            {

                                string PlateAffixationDateNew = Convert.ToString(dat[0]);
                                int z = 1;

                                DateTime updatedate = Convert.ToDateTime(d);
                                for (int i = 1; i <= z; i++)
                                {
                                    string[] dates = updatedate.ToString().Split(' ');
                                    string des = dates[0].ToString();

                                    string SQLData = "select count(*) as blockDate from HolidayDateTime where blockDate between '" + des + " 00:00:00' and '" + des + " 23:59:59'";
                                    int dtdate = Utils.getScalarCount(SQLData, CnnString);
                                    if (dtdate > 0)
                                    {
                                        updatedate = Convert.ToDateTime(des).AddDays(i);
                                        z = z + 1;
                                        PdfPCell cell265 = new PdfPCell(new Phrase(": ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                                        //PdfPCell cell265 = new PdfPCell(new Phrase(": " + updatedate.ToString("dd/MM/yyyy"), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                                        cell265.Colspan = 1;
                                        cell265.BorderWidthLeft = 0f;
                                        cell265.BorderWidthRight = 0f;
                                        cell265.BorderWidthTop = 0f;
                                        cell265.BorderWidthBottom = 0f;
                                        cell265.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                        table.AddCell(cell265);
                                    }
                                    else if (RTOLocationID == 507)
                                    {
                                        // PdfPCell cell265 = new PdfPCell(new Phrase(": ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                                        PdfPCell cell265 = new PdfPCell(new Phrase(": " + updatedate.ToString("dd/MM/yyyy"), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                                        cell265.Colspan = 1;
                                        cell265.BorderWidthLeft = 0f;
                                        cell265.BorderWidthRight = 0f;
                                        cell265.BorderWidthTop = 0f;
                                        cell265.BorderWidthBottom = 0f;
                                        cell265.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                        table.AddCell(cell265);
                                    }

                                    else
                                    {
                                        PdfPCell cell265 = new PdfPCell(new Phrase(": ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                                        // PdfPCell cell265 = new PdfPCell(new Phrase(": " + updatedate.ToString("dd/MM/yyyy"), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                                        cell265.Colspan = 1;
                                        cell265.BorderWidthLeft = 0f;
                                        cell265.BorderWidthRight = 0f;
                                        cell265.BorderWidthTop = 0f;
                                        cell265.BorderWidthBottom = 0f;
                                        cell265.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                        table.AddCell(cell265);
                                    }
                                }
                            }
                            else
                            {
                                PdfPCell cell265 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                                cell265.Colspan = 1;
                                cell265.BorderWidthLeft = 0f;
                                cell265.BorderWidthRight = 0f;
                                cell265.BorderWidthTop = 0f;
                                cell265.BorderWidthBottom = 0f;
                                cell265.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                table.AddCell(cell265);
                            }

                            // PdfPCell cell265 = new PdfPCell(new Phrase(": " + PlateAffixationDate.ToString("dd/MM/yyyy"), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            //PdfPCell cell265 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            //cell265.Colspan = 1;
                            //cell265.BorderWidthLeft = 0f;
                            //cell265.BorderWidthRight = 0f;
                            //cell265.BorderWidthTop = 0f;
                            //cell265.BorderWidthBottom = 0f;
                            //cell265.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            //table.AddCell(cell265);
                        }
                        catch
                        {
                        }
                    }
                    else
                    {
                        PdfPCell cell26 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell26.Colspan = 1;
                        cell26.BorderWidthLeft = 0f;
                        cell26.BorderWidthRight = 0f;
                        cell26.BorderWidthTop = 0f;
                        cell26.BorderWidthBottom = 0f;
                        cell26.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell26);

                        DateTime Orddate = Convert.ToDateTime(dataSetFillHSRPDeliveryChallan.Rows[0]["OrderDate"].ToString());


                        PdfPCell cell265 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell265.Colspan = 1;
                        cell265.BorderWidthLeft = 0f;
                        cell265.BorderWidthRight = 0f;
                        cell265.BorderWidthTop = 0f;
                        cell265.BorderWidthBottom = 0f;
                        cell265.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell265);
                    }



                    PdfPCell cell7 = new PdfPCell(new Phrase("OWNER NAME", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell7.Colspan = 1;
                    cell7.BorderWidthLeft = 0f;
                    cell7.BorderWidthRight = 0f;
                    cell7.BorderWidthTop = 0f;
                    cell7.BorderWidthBottom = 0f;
                    cell7.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell7);

                    PdfPCell cell75 = new PdfPCell(new Phrase(": " + dataSetFillHSRPDeliveryChallan.Rows[0]["OwnerName"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell75.Colspan = 1;
                    cell75.BorderWidthLeft = 0f;
                    cell75.BorderWidthRight = 0f;
                    cell75.BorderWidthTop = 0f;
                    cell75.BorderWidthBottom = 0f;
                    cell75.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell75);



                    PdfPCell cell29 = new PdfPCell(new Phrase("OWNER CONTACT NO", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell29.Colspan = 1;
                    cell29.BorderWidthLeft = 0f;
                    cell29.BorderWidthRight = 0f;
                    cell29.BorderWidthTop = 0f;
                    cell29.BorderWidthBottom = 0f;
                    cell29.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell29);

                    PdfPCell cell295 = new PdfPCell(new Phrase(": " + dataSetFillHSRPDeliveryChallan.Rows[0]["MobileNo"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell295.Colspan = 1;
                    cell295.BorderWidthLeft = 0f;
                    cell295.BorderWidthRight = 0f;
                    cell295.BorderWidthTop = 0f;
                    cell295.BorderWidthBottom = 0f;
                    cell295.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell295);



                    PdfPCell cell8 = new PdfPCell(new Phrase("OWNER ADDRESS", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell8.Colspan = 1;
                    cell8.BorderWidthLeft = 0f;
                    cell8.BorderWidthRight = 0f;
                    cell8.BorderWidthTop = 0f;
                    cell8.BorderWidthBottom = 0f;
                    cell8.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell8);

                    PdfPCell cell85 = new PdfPCell(new Phrase(": " + dataSetFillHSRPDeliveryChallan.Rows[0]["Address1"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell85.Colspan = 3;
                    cell85.BorderWidthLeft = 0f;
                    cell85.BorderWidthRight = 0f;
                    cell85.BorderWidthTop = 0f;
                    cell85.BorderWidthBottom = 0f;
                    cell85.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell85);



                    PdfPCell cell9 = new PdfPCell(new Phrase("VEHICLE REG", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell9.Colspan = 1;
                    cell9.BorderWidthLeft = 0f;
                    cell9.BorderWidthRight = 0f;
                    cell9.BorderWidthTop = 0f;
                    cell9.BorderWidthBottom = 0f;
                    cell9.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell9);

                    PdfPCell cell95 = new PdfPCell(new Phrase(": " + dataSetFillHSRPDeliveryChallan.Rows[0]["VehicleRegNo"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell95.Colspan = 1;
                    cell95.BorderWidthLeft = 0f;
                    cell95.BorderWidthRight = 0f;
                    cell95.BorderWidthTop = 0f;
                    cell95.BorderWidthBottom = 0f;
                    cell95.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell95);







                    PdfPCell cell10 = new PdfPCell(new Phrase("VEHICLE MODEL", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell10.Colspan = 1;
                    cell10.BorderWidthLeft = 0f;
                    cell10.BorderWidthRight = 0f;
                    cell10.BorderWidthTop = 0f;
                    cell10.BorderWidthBottom = 0f;
                    cell10.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell10);

                    PdfPCell cell105 = new PdfPCell(new Phrase(": " + dataSetFillHSRPDeliveryChallan.Rows[0]["VehicleType"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell105.Colspan = 1;
                    cell105.BorderWidthLeft = 0f;
                    cell105.BorderWidthRight = 0f;
                    cell105.BorderWidthTop = 0f;
                    cell105.BorderWidthBottom = 0f;
                    cell105.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell105);



                    PdfPCell cell11 = new PdfPCell(new Phrase("ENGINE NO.", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell11.Colspan = 1;
                    cell11.BorderWidthLeft = 0f;
                    cell11.BorderWidthRight = 0f;
                    cell11.BorderWidthTop = 0f;
                    cell11.BorderWidthBottom = 0f;
                    cell11.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell11);

                    PdfPCell cell115 = new PdfPCell(new Phrase(": " + dataSetFillHSRPDeliveryChallan.Rows[0]["EngineNo"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell115.Colspan = 1;
                    cell115.BorderWidthLeft = 0f;
                    cell115.BorderWidthRight = 0f;
                    cell115.BorderWidthTop = 0f;
                    cell115.BorderWidthBottom = 0f;
                    cell115.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell115);



                    PdfPCell cell1113 = new PdfPCell(new Phrase("CHASSIS NO", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell1113.Colspan = 1;
                    cell1113.BorderWidthLeft = 0f;
                    cell1113.BorderWidthRight = 0f;
                    cell1113.BorderWidthTop = 0f;
                    cell1113.BorderWidthBottom = 0f;
                    cell1113.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell1113);

                    PdfPCell cell11135 = new PdfPCell(new Phrase(": " + dataSetFillHSRPDeliveryChallan.Rows[0]["ChassisNo"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell11135.Colspan = 1;
                    cell11135.BorderWidthLeft = 0f;
                    cell11135.BorderWidthRight = 0f;
                    cell11135.BorderWidthTop = 0f;
                    cell11135.BorderWidthBottom = 0f;
                    cell11135.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell11135);



                    PdfPCell cell1112 = new PdfPCell(new Phrase("________________________________________________________________________", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell1112.Colspan = 4;
                    cell1112.BorderWidthLeft = 0f;
                    cell1112.BorderWidthRight = 0f;
                    cell1112.BorderWidthTop = 0f;
                    cell1112.BorderWidthBottom = 0f;
                    cell1112.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell1112);


                    PdfPCell cellspa12 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    cellspa12.Colspan = 1;
                    cellspa12.BorderWidthLeft = 0f;
                    cellspa12.BorderWidthRight = 0f;
                    cellspa12.BorderWidthTop = 0f;
                    cellspa12.BorderWidthBottom = 0f;
                    cellspa12.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cellspa12);


                    PdfPCell cell112 = new PdfPCell(new Phrase("DESCRIPTION", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    cell112.Colspan = 1;
                    cell112.BorderWidthLeft = 0f;
                    cell112.BorderWidthRight = 0f;
                    cell112.BorderWidthTop = 0f;
                    cell112.BorderWidthBottom = 0f;
                    cell112.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell112);

                    PdfPCell cellspa1s2 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    cellspa1s2.Colspan = 1;
                    cellspa1s2.BorderWidthLeft = 0f;
                    cellspa1s2.BorderWidthRight = 0f;
                    cellspa1s2.BorderWidthTop = 0f;
                    cellspa1s2.BorderWidthBottom = 0f;
                    cellspa1s2.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cellspa1s2);

                    PdfPCell cell119 = new PdfPCell(new Phrase("AMOUNT(RS)", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    cell119.Colspan = 1;
                    cell119.BorderWidthLeft = 0f;
                    cell119.BorderWidthRight = 0f;
                    cell119.BorderWidthTop = 0f;
                    cell119.BorderWidthBottom = 0f;
                    cell119.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell119);

                    PdfPCell cellDesc = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    cellDesc.Colspan = 1;
                    cellDesc.BorderWidthLeft = 0f;
                    cellDesc.BorderWidthRight = 0f;
                    cellDesc.BorderWidthTop = 0f;
                    cellDesc.BorderWidthBottom = 0f;
                    cellDesc.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cellDesc);

                    PdfPCell cellDescSet = new PdfPCell(new Phrase("SET OF HSRP PLATE", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    cellDescSet.Colspan = 1;
                    cellDescSet.BorderWidthLeft = 0f;
                    cellDescSet.BorderWidthRight = 0f;
                    cellDescSet.BorderWidthTop = 0f;
                    cellDescSet.BorderWidthBottom = 0f;
                    cellDescSet.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cellDescSet);

                    PdfPCell cellDesc1Sp = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    cellDesc1Sp.Colspan = 1;
                    cellDesc1Sp.BorderWidthLeft = 0f;
                    cellDesc1Sp.BorderWidthRight = 0f;
                    cellDesc1Sp.BorderWidthTop = 0f;
                    cellDesc1Sp.BorderWidthBottom = 0f;
                    cellDesc1Sp.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cellDesc1Sp);

                    PdfPCell cellDescSp = new PdfPCell(new Phrase(" " + dataSetFillHSRPDeliveryChallan.Rows[0]["NetAmount"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    cellDescSp.Colspan = 1;
                    cellDescSp.BorderWidthLeft = 0f;
                    cellDescSp.BorderWidthRight = 0f;
                    cellDescSp.BorderWidthTop = 0f;
                    cellDescSp.BorderWidthBottom = 0f;
                    cellDescSp.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cellDescSp);


                    PdfPCell cell1195 = new PdfPCell(new Phrase("________________________________________________________________________", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell1195.Colspan = 4;
                    cell1195.BorderWidthLeft = 0f;
                    cell1195.BorderWidthRight = 0f;
                    cell1195.BorderWidthTop = 0f;
                    cell1195.BorderWidthBottom = 0f;
                    cell1195.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell1195);

                    PdfPCell cell1201 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    cell1201.Colspan = 4;
                    cell1201.BorderWidthLeft = 0f;
                    cell1201.BorderWidthRight = 0f;
                    cell1201.BorderWidthTop = 0f;
                    cell1201.BorderWidthBottom = 0f;
                    cell1201.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell1201);

                    PdfPCell cell1202 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    cell1202.Colspan = 4;
                    cell1202.BorderWidthLeft = 0f;
                    cell1202.BorderWidthRight = 0f;
                    cell1202.BorderWidthTop = 0f;
                    cell1202.BorderWidthBottom = 0f;
                    cell1202.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell1202);


                    PdfPCell cell120 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    cell120.Colspan = 1;
                    cell120.BorderWidthLeft = 0f;
                    cell120.BorderWidthRight = 0f;
                    cell120.BorderWidthTop = 0f;
                    cell120.BorderWidthBottom = 0f;
                    cell120.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell120);


                    PdfPCell cellNet120 = new PdfPCell(new Phrase("NET AMOUNT", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    cellNet120.Colspan = 1;
                    cellNet120.BorderWidthLeft = 0f;
                    cellNet120.BorderWidthRight = 0f;
                    cellNet120.BorderWidthTop = 0f;
                    cellNet120.BorderWidthBottom = 0f;
                    cellNet120.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cellNet120);

                    PdfPCell cellAmt1205 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    cellAmt1205.Colspan = 1;
                    cellAmt1205.BorderWidthLeft = 0f;
                    cellAmt1205.BorderWidthRight = 0f;
                    cellAmt1205.BorderWidthTop = 0f;
                    cellAmt1205.BorderWidthBottom = 0f;
                    cellAmt1205.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cellAmt1205);

                    PdfPCell cell1205 = new PdfPCell(new Phrase(" " + dataSetFillHSRPDeliveryChallan.Rows[0]["NetAmount"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    cell1205.Colspan = 1;
                    cell1205.BorderWidthLeft = 0f;
                    cell1205.BorderWidthRight = 0f;
                    cell1205.BorderWidthTop = 0f;
                    cell1205.BorderWidthBottom = 0f;
                    cell1205.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell1205);

                    PdfPCell celldupRouCash401 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    celldupRouCash401.Colspan = 1;
                    celldupRouCash401.BorderWidthLeft = 0f;
                    celldupRouCash401.BorderWidthRight = 0f;
                    celldupRouCash401.BorderWidthTop = 0f;
                    celldupRouCash401.BorderWidthBottom = 0f;
                    celldupRouCash401.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    table.AddCell(celldupRouCash401);

                    PdfPCell celldupCash401 = new PdfPCell(new Phrase("ROUND OF AMOUNT", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    celldupCash401.Colspan = 1;
                    celldupCash401.BorderWidthLeft = 0f;
                    celldupCash401.BorderWidthRight = 0f;
                    celldupCash401.BorderWidthTop = 0f;
                    celldupCash401.BorderWidthBottom = 0f;
                    celldupCash401.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(celldupCash401);

                    PdfPCell celldupRouCash402 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    celldupRouCash402.Colspan = 1;
                    celldupRouCash402.BorderWidthLeft = 0f;
                    celldupRouCash402.BorderWidthRight = 0f;
                    celldupRouCash402.BorderWidthTop = 0f;
                    celldupRouCash402.BorderWidthBottom = 0f;
                    celldupRouCash402.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    table.AddCell(celldupRouCash402);

                    decimal roundAmt = Convert.ToDecimal(dataSetFillHSRPDeliveryChallan.Rows[0]["NetAmount"].ToString());
                    roundAmt = Math.Round(roundAmt, 0);

                    PdfPCell celldupCash402 = new PdfPCell(new Phrase(" " + roundAmt, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    celldupCash402.Colspan = 1;
                    celldupCash402.BorderWidthLeft = 0f;
                    celldupCash402.BorderWidthRight = 0f;
                    celldupCash402.BorderWidthTop = 0f;
                    celldupCash402.BorderWidthBottom = 0f;
                    celldupCash402.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(celldupCash402);

                    PdfPCell celldupCash402a = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    celldupCash402a.Colspan = 4;
                    celldupCash402a.BorderWidthLeft = 0f;
                    celldupCash402a.BorderWidthRight = 0f;
                    celldupCash402a.BorderWidthTop = 0f;
                    celldupCash402a.BorderWidthBottom = 0f;
                    celldupCash402a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(celldupCash402a);




                    if (HSRPStateID == 6 || HSRPStateID == 1)
                    {

                        PdfPCell cell1203 = new PdfPCell(new Phrase("For Affixation kindly visit :-  " + GetAddressukbr.Rows[0]["rtolocationname"].ToString() + " , " + GetAddressukbr.Rows[0]["rtolocationaddress"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                        cell1203.Colspan = 4;
                        cell1203.BorderWidthLeft = 0f;
                        cell1203.BorderWidthRight = 0f;
                        cell1203.BorderWidthTop = 0f;
                        cell1203.BorderWidthBottom = 0f;
                        cell1203.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1203);
                    }


                    //string Message = "\u2022" + " Vehicle Owner is required to visit along with vehicle for the affixation of HSRP, on the fourth working day from the date of  issuance of cash receipt.";
                    string Message = "\u2022" + " Vehicle Owner is requested to please check the Correctness of the cash slip.";

                    PdfPCell cell64 = new PdfPCell(new Phrase(Message, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell64.Colspan = 4;
                    cell64.BorderWidthLeft = 0f;
                    cell64.BorderWidthRight = 0f;
                    cell64.BorderWidthTop = 0f;
                    cell64.BorderWidthBottom = 0f;
                    cell64.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell64);


                    string MessageSec = "\u2022" + "Before leaving the counter verify your data on cash receipt.The company shall not be responsible for any clerical mistake what so ever.";

                    PdfPCell cell65a = new PdfPCell(new Phrase(MessageSec, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell65a.Colspan = 4;
                    cell65a.BorderWidthLeft = 0f;
                    cell65a.BorderWidthRight = 0f;
                    cell65a.BorderWidthTop = 0f;
                    cell65a.BorderWidthBottom = 0f;
                    cell65a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell65a);


                    if (Session["UserHSRPStateID"].ToString() == "2")
                    {
                        string MessageSecnew = "\u2022" + " Delhi HSRP HelpLine/Customer Care No:1800-1200-201.";

                        PdfPCell cell65anew = new PdfPCell(new Phrase(MessageSecnew, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell65anew.Colspan = 4;
                        cell65anew.BorderWidthLeft = 0f;
                        cell65anew.BorderWidthRight = 0f;
                        cell65anew.BorderWidthTop = 0f;
                        cell65anew.BorderWidthBottom = 0f;
                        cell65anew.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell65anew);



                        string MessageSecnew1 = "\u2022" + " WebSite: WWW.hsrpdelhi.com";

                        PdfPCell cell65anew1 = new PdfPCell(new Phrase(MessageSecnew1, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell65anew1.Colspan = 4;
                        cell65anew1.BorderWidthLeft = 0f;
                        cell65anew1.BorderWidthRight = 0f;
                        cell65anew1.BorderWidthTop = 0f;
                        cell65anew1.BorderWidthBottom = 0f;
                        cell65anew1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell65anew1);

                    }

                    if (Session["UserHSRPStateID"].ToString() == "6")
                    {
                        string MessageSecnew = "\u2022" + " For Any Query, complaint and suggestion and Tracking Your Vehicle Please Visit www.hsrpuk.com";

                        PdfPCell cell65anew = new PdfPCell(new Phrase(MessageSecnew, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell65anew.Colspan = 4;
                        cell65anew.BorderWidthLeft = 0f;
                        cell65anew.BorderWidthRight = 0f;
                        cell65anew.BorderWidthTop = 0f;
                        cell65anew.BorderWidthBottom = 0f;
                        cell65anew.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell65anew);

                    }
                    if (Session["UserHSRPStateID"].ToString() == "1")
                    {
                        string MessageSecnew = "\u2022" + " For Any Query, complaint and suggestion and Tracking Your Vehicle Please Visit www.hsrpbr.com";

                        PdfPCell cell65anew = new PdfPCell(new Phrase(MessageSecnew, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell65anew.Colspan = 4;
                        cell65anew.BorderWidthLeft = 0f;
                        cell65anew.BorderWidthRight = 0f;
                        cell65anew.BorderWidthTop = 0f;
                        cell65anew.BorderWidthBottom = 0f;
                        cell65anew.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell65anew);

                    }

                    if (HSRPStateID == 1)
                    {
                        string company = "UTSAV SAFETY SYSTEM PVT. LTD. In consitium with Linkpoint Infrastructure Pvt. Ltd.";
                        PdfPCell cell123 = new PdfPCell(new Phrase("" + company, new iTextSharp.text.Font(bfTimes, 5f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell123.Colspan = 4;
                        cell123.BorderColor = BaseColor.WHITE;
                        cell123.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell123);

                        PdfPCell cellsp2 = new PdfPCell(new Phrase("" + GetAddress.Rows[0]["Address1"].ToString(), new iTextSharp.text.Font(bfTimes, 5f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cellsp2.Colspan = 4;
                        cellsp2.BorderWidthLeft = 0f;
                        cellsp2.BorderWidthRight = 0f;
                        cellsp2.BorderWidthTop = 0f;
                        cellsp2.BorderWidthBottom = 0f;
                        cellsp2.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cellsp2);


                        PdfPCell cell63 = new PdfPCell(new Phrase("" + company, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell63.Colspan = 4;
                        cell63.BorderWidthLeft = 0f;
                        cell63.BorderWidthRight = 0f;
                        cell63.BorderWidthTop = 0f;
                        cell63.BorderWidthBottom = 0f;
                        cell63.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell63);

                    }
                    else
                    {
                        PdfPCell cellsp1 = new PdfPCell(new Phrase("" + GetAddress.Rows[0]["CompanyName"].ToString(), new iTextSharp.text.Font(bfTimes, 5f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cellsp1.Colspan = 4;
                        cellsp1.BorderWidthLeft = 0f;
                        cellsp1.BorderWidthRight = 0f;
                        cellsp1.BorderWidthTop = 0f;
                        cellsp1.BorderWidthBottom = 0f;
                        cellsp1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cellsp1);

                        PdfPCell cellsp2 = new PdfPCell(new Phrase("" + GetAddress.Rows[0]["Address1"].ToString(), new iTextSharp.text.Font(bfTimes, 5f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cellsp2.Colspan = 4;
                        cellsp2.BorderWidthLeft = 0f;
                        cellsp2.BorderWidthRight = 0f;
                        cellsp2.BorderWidthTop = 0f;
                        cellsp2.BorderWidthBottom = 0f;
                        cellsp2.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cellsp2);

                        PdfPCell cell63 = new PdfPCell(new Phrase("" + GetAddress.Rows[0]["CompanyName"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell63.Colspan = 4;
                        cell63.BorderWidthLeft = 0f;
                        cell63.BorderWidthRight = 0f;
                        cell63.BorderWidthTop = 0f;
                        cell63.BorderWidthBottom = 0f;
                        cell63.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell63);
                    }


                    PdfPCell cell62 = new PdfPCell(new Phrase("(Cashier Name/Code.) :- " + user, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell62.Colspan = 4;
                    cell62.BorderWidthLeft = 0f;
                    cell62.BorderWidthRight = 0f;
                    cell62.BorderWidthTop = 0f;
                    cell62.BorderWidthBottom = 0f;
                    cell62.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell62);



                    //PdfPCell cellCashierName = new PdfPCell(new Phrase(""+ userloginname, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    //cellCashierName.Colspan = 4;
                    //cellCashierName.BorderWidthLeft = 0f;
                    //cellCashierName.BorderWidthRight = 0f;
                    //cellCashierName.BorderWidthTop = 0f;
                    //cellCashierName.BorderWidthBottom = 0f;
                    //cellCashierName.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    //table.AddCell(cellCashierName);









                    //////////////////////////////////////Duplicate CashRecipt/////////////////////////////////////////////////////////////////////////////////////////////


                    PdfPCell cell2195 = new PdfPCell(new Phrase("-------------------------------------------------------------*--------------------------------------------------------------", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell2195.Colspan = 4;
                    cell2195.BorderWidthLeft = 0f;
                    cell2195.BorderWidthRight = 0f;
                    cell2195.BorderWidthTop = 0f;
                    cell2195.BorderWidthBottom = 0f;
                    cell2195.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell2195);



                    //PdfPCell cellsp3 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    //cellsp3.Colspan = 4;
                    //cellsp3.BorderWidthLeft = 0f;
                    //cellsp3.BorderWidthRight = 0f;
                    //cellsp3.BorderWidthTop = 0f;
                    //cellsp3.BorderWidthBottom = 0f;
                    //cellsp3.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    //table.AddCell(cellsp3);

                    document.Add(table);
                    document.Add(table);



                    document.Close();
                    HttpContext context = HttpContext.Current;
                    e.Control.ID = "LogOut";
                    context.Response.ContentType = "Application/pdf";
                    context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                    context.Response.WriteFile(PdfFolder);
                    context.Response.End();

                }
            }
            //  EpsionCashReceipt
            //if (e.Control.ID.ToString() == "LinkButtonEpsonCashReceipt")
            if (e.Control.ID.ToString() == "LinkButtonEpsionCashReceipt")
            {
                //EpsionCashReceipt(HSRPRecordID);
                //EpsionPrint(HSRPRecordID);

            }
            //  }




        }

      

        private void ClearGrid()
        {
            Grid1.Items.Clear();
            lblErrMsg.Text = String.Empty;
            return;
        }

    

        #region Export Grid to CSV
        public void CreateCSVFile(DataTable dt, string strFilePath)
        {




            // Create the CSV file to which grid data will be exported.

            StreamWriter sw = new StreamWriter(strFilePath, false);

            // First we will write the headers.

            //DataTable dt = m_dsProducts.Tables[0];

            int iColCount = dt.Columns.Count;

            //for (int i = 0; i < iColCount; i++)
            //{

            //    sw.Write(dt.Columns[i]);

            //    if (i < iColCount - 1)
            //    {

            //        sw.Write(";");

            //    }

            //}

            //sw.Write(sw.NewLine);

            // Now write all the rows.

            foreach (DataRow dr in dt.Rows)
            {

                for (int i = 0; i < iColCount; i++)
                {

                    if (!Convert.IsDBNull(dr[i]))
                    {

                        sw.Write(dr[i].ToString());

                    }

                    if (i < iColCount - 1)
                    {

                        sw.Write(";");

                    }

                }

                sw.Write(sw.NewLine);

            }

            sw.Close();

            

            System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
            response.ClearContent();
            response.Clear();
            response.ContentType = "text/plain";
            response.AddHeader("Content-Disposition", "attachment; filename=" + strFilePath + ";");
            response.TransmitFile(strFilePath);
            response.Flush();
            response.End();


        }
        #endregion

        protected void button_Click(object sender, EventArgs e)
        {
            ClearGrid();
            
                try
                {
                  SQLString =  "select top 20	HSRPRecordStaggingID as HSRPRecordID,	HSRP_StateID as HSRP_StateID,	CONVERT(varchar, HSRPRecord_CreationDate,105)  as OrderDate,HSRPRecord_AuthorizationNo, convert(varchar,HSRPRecord_AuthorizationDate, 105) as HSRPRecord_AuthorizationDate, RegistrationDate,	OwnerName,	VehicleClass,	NICVehicleType as VehicleType,	ChassisNo,	EngineNo,	VehicleRegNo,	NICVehicleRegNo,	HSRP_Front_LaserCode as HSRP_Front_LaserCode,	HSRP_Rear_LaserCode as HSRP_Rear_LaserCode  FROM HSRPRecordsStaggingArea   Where hsrp_stateID='" + HSRPStateID + "' and VehicleRegNo like '%" + txtSearchAll.Text + "%'  order by vehicleRegNo";
                    //SQLString = "SELECT top 20 [HSRPRecordID],[HSRP_StateID],CONVERT(varchar, HSRPRecords.OrderDate,105)  as OrderDate,convert(varchar, HSRPRecord_AuthorizationDate, 105) as HSRPRecord_AuthorizationDate, HSRPRecord_AuthorizationNo,convert(varchar, OrderEmbossingDate, 105) as OrderEmbossingDate,HSRP_Front_LaserCode, HSRP_Rear_LaserCode, convert(varchar, OrderClosedDate, 105) as InvoiceDateTime, ChassisNo,HSRPRecord_AuthorizationNo,VehicleRegNo,NetAmount,[OwnerName] ,EngineNo,VehicleType,[MobileNo],[EmailID],OrderStatus FROM [HSRPRecords] Where hsrp_stateID='" + HSRPStateID + "' and VehicleRegNo like '%" + txtSearchAll.Text + "%'  order by vehicleRegNo";
                    DataTable dt = Utils.GetDataTable(SQLString, CnnString);
                    Grid1.DataSource = dt;
                    Grid1.RunningMode = (ComponentArt.Web.UI.GridRunningMode)Enum.Parse(typeof(ComponentArt.Web.UI.GridRunningMode), "Client");
                    Grid1.SearchOnKeyPress = true;
                    Grid1.DataBind();
                    Grid1.RecordCount.ToString();

                }
                catch (Exception ex)
                {
                    lblErrMsg.Text = "Error in Populating Grid :" + ex.Message.ToString();
                }
         
        }



       
    }
}