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

namespace HSRP.Transaction
{
    public partial class ViewHSRPRecords : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        string UserType = string.Empty;
        string HSRPStateID = string.Empty;
        string RTOLocationID = string.Empty;
       

        protected void Page_Load(object sender, EventArgs e)
        {
           // Utils.GZipEncodePage();
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {
                RTOLocationID = Session["UserRTOLocationID"].ToString();
                UserType = Session["UserType"].ToString();
                HSRPStateID = Session["UserHSRPStateID"].ToString();
                lblErrMsg.Text = string.Empty;
                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                strUserID = Session["UID"].ToString();
                ComputerIP = Request.UserHostAddress;

                if (!IsPostBack)
                {
                    try
                    {
                        InitialSetting();
                        if (UserType=="0")
                        {
                            labelHSRPState.Visible = true;
                            dropDownListHSRPState.Visible = true;
                            FilldropDownListHSRPState();
                        }
                        else if (UserType == "0")
                        {
                            //labelHSRPState.Visible = false;
                            //dropDownListHSRPState.Visible = false;
                            //labelRTOLocation.Visible = true;
                            //dropDownListRTOLocation.Visible = true;

                            //FilldropDownListHSRPState();
                           // FilldropDownListRTOLocation();
                            labelHSRPState.Visible = true;
                            dropDownListHSRPState.Visible = true;
                            FilldropDownListHSRPState();
                        }
                        else
                        {
                            //pdfexl.Visible = true;
                            //ButtonGo.Visible = false;
                            //labelHSRPState.Visible = false;
                            //dropDownListHSRPState.Visible = false;
                            //labelRTOLocation.Visible = false;
                            //dropDownListRTOLocation.Visible = false;
                            labelHSRPState.Visible = true;
                            dropDownListHSRPState.Visible = true;
                            FilldropDownListHSRPState();
                           
                        }
                    }
                    catch (Exception err)
                    {
                        lblErrMsg.Text = "Error on Page Load" + err.Message.ToString();
                    }
                }
            }
        }


        protected void ButtonGo_Click(object sender, EventArgs e)
        {
            if (dropDownListRTOLocation.SelectedItem.Text != "--Select RTO Location--" || dropDownListHSRPState.SelectedItem.Text !="--Select State--" )
            {
                ShowGrid();
            }
            else
            {
                Grid1.Items.Clear();
                lblErrMsg.Text = String.Empty;
                lblErrMsg.Text = "Please Select RTO Location and State Also";
                return;
            }
        }

        #region DropDown

        private void FilldropDownListHSRPState()
        {
            if (UserType == "0")
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState where ActiveStatus='Y' Order by HSRPStateName";
                 Utils.PopulateDropDownList(dropDownListHSRPState, SQLString.ToString(), CnnString, "--Select State--");
                dropDownListHSRPState.SelectedIndex = 0;
            }
            else
            {
                dropDownListHSRPState.Enabled = false;
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID=" + HSRPStateID + " and ActiveStatus='Y' Order by HSRPStateName";
                DataTable dss = Utils.GetDataTable(SQLString, CnnString);
                dropDownListHSRPState.DataSource = dss;
                dropDownListHSRPState.DataBind();

                FilldropDownListRTOLocation();
            }
          
        }

        private void FilldropDownListRTOLocation()
        {
            if (UserType == "0")
            {

                SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + dropDownListHSRPState.SelectedValue.ToString() + " and ActiveStatus!='N' Order by RTOLocationName";
                //SQLString = "select RTOLocation.RTOLocationName,RTOLocation.RTOLocationID from RTOLocation inner join UserRTOLocationMapping on RTOLocation.RTOLocationID=UserRTOLocationMapping.RTOLocationID Where RTOLocation.HSRP_StateID=" + intHSRPStateID + " and UserRTOLocationMapping.UserID='" + strUserID + "' and ActiveStatus!='N' Order by RTOLocation.RTOLocationName";
               
            }
            else
            {
                dropDownListRTOLocation.Visible = true;
                labelRTOLocation.Visible = true;
                UpdateRTOLocation.Update();
                
               // SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + intHSRPStateID + " and ActiveStatus!='N' Order by RTOLocationName";
                SQLString = "select RTOLocation.RTOLocationName,RTOLocation.RTOLocationID from RTOLocation inner join UserRTOLocationMapping on RTOLocation.RTOLocationID=UserRTOLocationMapping.RTOLocationID Where RTOLocation.HSRP_StateID=" + dropDownListHSRPState.SelectedValue.ToString() + " and UserRTOLocationMapping.UserID='" + strUserID + "' and ActiveStatus!='N' Order by RTOLocation.RTOLocationName";
               
            }
            Utils.PopulateDropDownList(dropDownListRTOLocation, SQLString.ToString(), CnnString, "--Select RTO Location--");
            dropDownListRTOLocation.SelectedIndex = 0;
        }

        protected void dropDownListHSRPState_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dropDownListHSRPState.SelectedItem.Text != "--Select State--")
            {
                dropDownListRTOLocation.Visible = true;
                labelRTOLocation.Visible = true;
                FilldropDownListRTOLocation();
                UpdateRTOLocation.Update();
            }
            else
            {
                lblErrMsg.Text = string.Empty;
                lblErrMsg.Text = "Please Select State";
                dropDownListHSRPState.Visible = false;
                labelRTOLocation.Visible = false;
                UpdateRTOLocation.Update();
                Grid1.Items.Clear();
            }
        }

        
        #endregion

        private void ShowGrid()
        {

            if (String.IsNullOrEmpty(dropDownListRTOLocation.SelectedValue) || dropDownListRTOLocation.SelectedValue.Equals("--Select RTO Location--"))
            {
                Grid1.Items.Clear();
                lblErrMsg.Text = String.Empty;
                lblErrMsg.Text = "Please Select RTO Location.";
                return;
            }
            buildGrid();
        }


        #region Grid
        public void buildGrid()
        {
            try
            {
                if (UserType == "0")
                {
                    HSRPStateID = dropDownListHSRPState.SelectedValue;
                   RTOLocationID=dropDownListRTOLocation.SelectedValue;
                }
                else if (UserType == "0")
                {
                    //intHSRPStateID = HSRPStateID;
                    RTOLocationID = dropDownListRTOLocation.SelectedValue;
                }

                SQLString = "SELECT [HSRPRecordID],[HSRP_StateID],ChassisNo,HSRPRecord_AuthorizationNo,convert(varchar, HSRPRecord_AuthorizationDate, 105) as HSRPRecord_AuthorizationDate, convert(varchar, OrderEmbossingDate, 105) as OrderEmbossingDate, convert(varchar, OrderEmbossingDate, 105) as OrderEmbossingDate,HSRP_Front_LaserCode, HSRP_Rear_LaserCode,VehicleRegNo,NetAmount,[OwnerName] ,EngineNo,VehicleType,[MobileNo],[EmailID],OrderStatus,CONVERT(varchar(20), HSRPRecords.OrderDate,103)  as   OrderDate FROM [HSRPRecords] Where HSRP_StateID=" + HSRPStateID + " and RTOLocationID=" + RTOLocationID + " and OrderStatus!='Closed' order by HSRPRecord_AuthorizationNo";
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
                lblErrMsg.Text = "that is not valid Record.";
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


                    //string filename = "CASH RECEIPT-" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".pdf";
                    string filename = "CASHRECEIPT_No-" + dataSetFillHSRPDeliveryChallan.Rows[0]["CashReceiptNo"].ToString().Replace("/","-") + ".pdf";
                    

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
                    table.TotalWidth = 585f;

                    //fix the absolute width of the table



                    PdfPCell cell12 = new PdfPCell(new Phrase("" + GetAddress.Rows[0]["CompanyName"].ToString(), new iTextSharp.text.Font(bfTimes, 12f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell12.Colspan = 4;
                    cell12.BorderColor = BaseColor.WHITE;
                    cell12.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell12);

                    PdfPCell cell1203 = new PdfPCell(new Phrase("" + GetAddress.Rows[0]["Address1"].ToString() + " , " + GetAddress.Rows[0]["city"].ToString() + Address.ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    cell1203.Colspan = 4;
                    cell1203.BorderWidthLeft = 0f;
                    cell1203.BorderWidthRight = 0f;
                    cell1203.BorderWidthTop = 0f;
                    cell1203.BorderWidthBottom = 0f;
                    cell1203.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell1203);

                    //PdfPCell cell = new PdfPCell(new Phrase("WE HEREBY CONFIRM TO HAVE INSTALLED THE HSRP SET AS DETAILED BELOW : ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    //cell.Colspan = 4;
                    //cell.BorderColor = BaseColor.WHITE;
                    //cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    //table.AddCell(cell);

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


                    

                    PdfPCell cell22 = new PdfPCell(new Phrase("TIME", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell22.Colspan = 1;

                    cell22.BorderWidthLeft = 0f;
                    cell22.BorderWidthRight = 0f;
                    cell22.BorderWidthTop = 0f;
                    cell22.BorderWidthBottom = 0f;
                    cell22.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell22);

                    PdfPCell cell222 = new PdfPCell(new Phrase(": " + Convert.ToDateTime(dataSetFillHSRPDeliveryChallan.Rows[0]["HSRPRecord_CreationDate"]).ToString("hh:mm:ss"), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
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


                    string Message = "\u2022" + " Vehicle Owner is required to visit along with vehicle for the affixation of HSRP , on the fourth working day from the date of  issuance of cash receipt.";

                    PdfPCell cell64 = new PdfPCell(new Phrase(Message, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell64.Colspan = 4;
                    cell64.BorderWidthLeft = 0f;
                    cell64.BorderWidthRight = 0f;
                    cell64.BorderWidthTop = 0f;
                    cell64.BorderWidthBottom = 0f;
                    cell64.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell64);


                    string MessageSec = "\u2022" + " Before leaving the counter verify your data on cash receipt.";

                    PdfPCell cell65a = new PdfPCell(new Phrase(MessageSec, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell65a.Colspan = 4;
                    cell65a.BorderWidthLeft = 0f;
                    cell65a.BorderWidthRight = 0f;
                    cell65a.BorderWidthTop = 0f;
                    cell65a.BorderWidthBottom = 0f;
                    cell65a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell65a);


                    PdfPCell cell63 = new PdfPCell(new Phrase("" + GetAddress.Rows[0]["CompanyName"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell63.Colspan = 4;
                    cell63.BorderWidthLeft = 0f;
                    cell63.BorderWidthRight = 0f;
                    cell63.BorderWidthTop = 0f;
                    cell63.BorderWidthBottom = 0f;
                    cell63.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell63);

                    PdfPCell cellsp4 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cellsp4.Colspan = 4;
                    cellsp4.BorderWidthLeft = 0f;
                    cellsp4.BorderWidthRight = 0f;
                    cellsp4.BorderWidthTop = 0f;
                    cellsp4.BorderWidthBottom = 0f;
                    cellsp4.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cellsp4);

                    PdfPCell cellsp5 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cellsp5.Colspan = 4;
                    cellsp5.BorderWidthLeft = 0f;
                    cellsp5.BorderWidthRight = 0f;
                    cellsp5.BorderWidthTop = 0f;
                    cellsp5.BorderWidthBottom = 0f;
                    cellsp5.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cellsp5);

                    PdfPCell cell62 = new PdfPCell(new Phrase("(AUTH. SIGH.)", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell62.Colspan = 4;
                    cell62.BorderWidthLeft = 0f;
                    cell62.BorderWidthRight = 0f;
                    cell62.BorderWidthTop = 0f;
                    cell62.BorderWidthBottom = 0f;
                    cell62.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell62);






                    //////////////////////////////////////Duplicate CashRecipt/////////////////////////////////////////////////////////////////////////////////////////////


                    PdfPCell cell2195 = new PdfPCell(new Phrase("----------------------------------------------------------------------------------------------------------------------------", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell2195.Colspan = 4;
                    cell2195.BorderWidthLeft = 0f;
                    cell2195.BorderWidthRight = 0f;
                    cell2195.BorderWidthTop = 0f;
                    cell2195.BorderWidthBottom = 0f;
                    cell2195.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell2195);

                    PdfPCell cellsp1 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cellsp1.Colspan = 4;
                    cellsp1.BorderWidthLeft = 0f;
                    cellsp1.BorderWidthRight = 0f;
                    cellsp1.BorderWidthTop = 0f;
                    cellsp1.BorderWidthBottom = 0f;
                    cellsp1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cellsp1);

                    PdfPCell cellsp2 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cellsp2.Colspan = 4;
                    cellsp2.BorderWidthLeft = 0f;
                    cellsp2.BorderWidthRight = 0f;
                    cellsp2.BorderWidthTop = 0f;
                    cellsp2.BorderWidthBottom = 0f;
                    cellsp2.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cellsp2);

                    PdfPCell cellsp3 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cellsp3.Colspan = 4;
                    cellsp3.BorderWidthLeft = 0f;
                    cellsp3.BorderWidthRight = 0f;
                    cellsp3.BorderWidthTop = 0f;
                    cellsp3.BorderWidthBottom = 0f;
                    cellsp3.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cellsp3);

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
            
            
            
            
            
        }


        //ambrish

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
        string FromDate1;
        string ToDate;


      
        protected void LinkbuttonSearch_Click(object sender, EventArgs e)
        {

          
            int i = 0;

            String[] StringAuthDate = OrderDate.SelectedDate.ToString().Split('/');
            string MonTo = ("0" + StringAuthDate[0]);
            string MonthdateTO = MonTo.Replace("00", "0").Replace("01", "1");
            String ReportDateFrom = StringAuthDate[1] + "/" + MonthdateTO + "/" + StringAuthDate[2].Split(' ')[0];
            String From = StringAuthDate[0] + "/" + StringAuthDate[1] + "/" + StringAuthDate[2].Split(' ')[0];
           
            FromDate1 = (From + " 00:00:00");


            String[] StringOrderDate = HSRPAuthDate.SelectedDate.ToString().Split('/');
            string Mon = ("0" + StringOrderDate[0]);
            string Monthdate = Mon.Replace("00", "0").Replace("01", "1");
            String FromDate = StringOrderDate[1] + "-" + Monthdate + "-" + StringOrderDate[2].Split(' ')[0];
            String ReportDateTo = StringOrderDate[1] + "/" + Monthdate + "/" + StringOrderDate[2].Split(' ')[0];
            String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
           
            ToDate = (From1 + " 23:59:59");

            



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


            HSRPStateID = dropDownListHSRPState.SelectedValue;
            RTOLocationID = dropDownListRTOLocation.SelectedValue;

            if (RadioButtonOrderDate.Checked == true)
            {
                SQLString = " SELECT  CONVERT(varchar(20), HSRPRecords.OrderDate,103)  as OrderDate,HSRPRecord_AuthorizationNo,  convert(varchar,  HSRPRecords.HSRPRecord_AuthorizationDate, 105) as HSRPRecord_AuthorizationDate,   convert(varchar, OrderClosedDate, 105) as InvoiceDateTime, convert(varchar, OrderEmbossingDate, 105) as OrderEmbossingDate,HSRPRecords.HSRP_Front_LaserCode, HSRPRecords.HSRP_Rear_LaserCode, HSRPRecords.OrderStatus,HSRPRecords.[HSRPRecordID],HSRPRecords.[HSRP_StateID],HSRPRecords.HSRPRecord_AuthorizationNo,  HSRPRecords.OwnerName,  HSRPRecords.VehicleRegNo,  HSRPRecords.ChassisNo,  HSRPRecords.EngineNo,  HSRPRecords.VehicleType,  HSRPRecords.MobileNo,  HSRPRecords.NetAmount FROM HSRPRecords  where HSRPRecords.HSRP_StateID=" + HSRPStateID + " and HSRPRecords.RTOLocationID=" + RTOLocationID + " and   OrderStatus!='Closed' and HSRPRecords.OrderDate between '" + FromDate1 + "' and '" + ToDate + "' order by OrderDate desc";
                //SQLString = " SELECT  CONVERT(varchar(20), HSRPRecords.OrderDate,103)  as OrderDate,HSRPRecord_AuthorizationNo,  convert(varchar,  HSRPRecords.HSRPRecord_AuthorizationDate, 105) as HSRPRecord_AuthorizationDate,   convert(varchar, OrderClosedDate, 105) as InvoiceDateTime, convert(varchar, OrderEmbossingDate, 105) as OrderEmbossingDate,HSRPRecords.HSRP_Front_LaserCode, HSRPRecords.HSRP_Rear_LaserCode, HSRPRecords.OrderStatus,HSRPRecords.[HSRPRecordID],HSRPRecords.[HSRP_StateID],HSRPRecords.HSRPRecord_AuthorizationNo,  HSRPRecords.OwnerName,  HSRPRecords.VehicleRegNo,  HSRPRecords.ChassisNo,  HSRPRecords.EngineNo,  HSRPRecords.VehicleType,  HSRPRecords.MobileNo,  HSRPRecords.NetAmount, HSRPState.HSRPStateName, RTOLocation.RTOLocationName FROM HSRPRecords inner join HSRPState on HSRPRecords.HSRP_StateID = HSRPState.HSRP_StateID inner join RTOLocation on  HSRPRecords.UserRTOLocationID = RTOLocation.RTOLocationID where HSRPRecords.HSRP_StateID=" + HSRPStateID + " and HSRPRecords.UserRTOLocationID=" + RTOLocationID + " and   OrderStatus!='Closed' and HSRPRecords.OrderDate between '" + FromDate1 + "' and '" + ToDate + "' order by OrderDate desc";
            }
            else if (RadioButtonAuthorizationDate.Checked == true)
            {
                SQLString = " SELECT  CONVERT(varchar(20), HSRPRecords.OrderDate,103)  as OrderDate, HSRPRecord_AuthorizationNo,  convert(varchar,  HSRPRecords.HSRPRecord_AuthorizationDate, 105) as HSRPRecord_AuthorizationDate,convert(varchar, OrderClosedDate, 105) as InvoiceDateTime, convert(varchar, OrderEmbossingDate, 105) as OrderEmbossingDate,HSRPRecords.HSRP_Front_LaserCode, HSRPRecords.HSRP_Rear_LaserCode, HSRPRecords.OrderStatus,HSRPRecords.[HSRPRecordID],HSRPRecords.[HSRP_StateID],HSRPRecords.HSRPRecord_AuthorizationNo,  HSRPRecords.OwnerName,  HSRPRecords.VehicleRegNo,  HSRPRecords.ChassisNo,  HSRPRecords.EngineNo,  HSRPRecords.VehicleType,  HSRPRecords.MobileNo,  HSRPRecords.NetAmount FROM HSRPRecords where HSRPRecords.HSRP_StateID=" + HSRPStateID + " and HSRPRecords.RTOLocationID=" + RTOLocationID + " and  OrderStatus!='Closed' and HSRPRecords.HSRPRecord_AuthorizationDate between '" + FromDate1 + "' and '" + ToDate + "' order by OrderDate desc";
            }


            DataTable dt = Utils.GetDataTable(SQLString, CnnString);
            if (dt.Rows.Count > 0)
            {

                Grid1.DataSource = dt;
                Grid1.DataBind();
            }
            else
            {
                string closescript1 = "<script>alert('No records found for selected date.')</script>";
                Page.RegisterStartupScript("abc", closescript1);
                return;
            }




        }




        DateTime OrderDate1, OrderDate2;

        protected void ButtonExcel_Click(object sender, EventArgs e)
        {
            try
            {
                //LabelError.Visible = false;

                int i = 0;
                String[] StringAuthDate = OrderDate.SelectedDate.ToString().Split('/');
                string MonTo = ("0" + StringAuthDate[0]);
                string MonthdateTO = MonTo.Replace("00", "0").Replace("01", "1");
                String ReportDateFrom = StringAuthDate[1] + "/" + MonthdateTO + "/" + StringAuthDate[2].Split(' ')[0];
                String From = StringAuthDate[0] + "/" + StringAuthDate[1] + "/" + StringAuthDate[2].Split(' ')[0];

                FromDate1 = (From + " 00:00:00");
                OrderDate1 = DateTime.Parse(From + " 00:00:00");

                String[] StringOrderDate = HSRPAuthDate.SelectedDate.ToString().Split('/');
                string Mon = ("0" + StringOrderDate[0]);
                string Monthdate = Mon.Replace("00", "0").Replace("01", "1");
                String FromDate = StringOrderDate[1] + "-" + Monthdate + "-" + StringOrderDate[2].Split(' ')[0];
                String ReportDateTo = StringOrderDate[1] + "/" + Monthdate + "/" + StringOrderDate[2].Split(' ')[0];
                String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];

                ToDate = (From1 + " 23:59:59");

                OrderDate2 = DateTime.Parse(From1 + " 23:59:59");




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


                //int.TryParse(dropDownListHSRPState.SelectedValue, out intHSRPStateID);
                //int.TryParse(dropDownListRTOLocation.SelectedValue, out intRTOLocationID);

                HSRPStateID = dropDownListHSRPState.SelectedValue;
                RTOLocationID = dropDownListRTOLocation.SelectedValue;

                if (RadioButtonOrderDate.Checked == true)
                {
                    SQLString = " SELECT      CONVERT(varchar(30), HSRPRecords.HSRPRecord_AuthorizationDate,103) as AuthDate, CONVERT(varchar(30), HSRPRecords.OrderDate,103) as OrderDate,HSRPRecords.OrderStatus,HSRPRecords.[HSRPRecordID],HSRPRecords.[HSRP_StateID],HSRPRecords.HSRPRecord_AuthorizationNo,  HSRPRecords.OwnerName,  HSRPRecords.VehicleRegNo,  HSRPRecords.ChassisNo,  HSRPRecords.EngineNo,  HSRPRecords.VehicleType,  HSRPRecords.MobileNo,  HSRPRecords.NetAmount, HSRPState.HSRPStateName,dbo.RTOLocation.RTOLocationName FROM HSRPRecords inner join 	HSRPState on HSRPRecords.HSRP_StateID = HSRPState.HSRP_StateID inner join RTOLocation on  HSRPRecords.RTOLocationID = RTOLocation.RTOLocationID	where HSRPRecords.HSRP_StateID=" + HSRPStateID + " and HSRPRecords.RTOLocationID=" + RTOLocationID + " and OrderStatus!='Closed' and  HSRPRecords.OrderDate between '" + FromDate1 + "' and '" + ToDate + "' order by OrderDate";

                }
                else if (RadioButtonAuthorizationDate.Checked == true)
                {

                    SQLString = " SELECT     CONVERT(varchar(30), HSRPRecords.HSRPRecord_AuthorizationDate,103) as AuthDate, CONVERT(varchar(30), HSRPRecords.OrderDate,103) as OrderDate,HSRPRecords.OrderStatus,HSRPRecords.[HSRPRecordID],HSRPRecords.[HSRP_StateID],HSRPRecords.HSRPRecord_AuthorizationNo,  HSRPRecords.OwnerName,  HSRPRecords.VehicleRegNo,  HSRPRecords.ChassisNo,  HSRPRecords.EngineNo,  HSRPRecords.VehicleType,  HSRPRecords.MobileNo,  HSRPRecords.NetAmount, HSRPState.HSRPStateName,dbo.RTOLocation.RTOLocationName FROM HSRPRecords inner join 	HSRPState on HSRPRecords.HSRP_StateID = HSRPState.HSRP_StateID inner join RTOLocation on  HSRPRecords.RTOLocationID = RTOLocation.RTOLocationID	where HSRPRecords.HSRP_StateID=" + HSRPStateID + " and HSRPRecords.RTOLocationID=" + RTOLocationID + " and OrderStatus!='Closed' and  HSRPRecords.HSRPRecord_AuthorizationDate between '" + FromDate1 + "' and '" + ToDate + "' order by OrderDate";

                }



               
                DataTable dt = Utils.GetDataTable(SQLString, CnnString);



                if (dt.Rows.Count > 0)
                {

                    i = dt.Rows.Count;


                    string filename = "DailyOrderReport-" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                    Workbook book = new Workbook();

                    // Specify which Sheet should be opened and the size of window by default
                    book.ExcelWorkbook.ActiveSheetIndex = 1;
                    book.ExcelWorkbook.WindowTopX = 100;
                    book.ExcelWorkbook.WindowTopY = 200;
                    book.ExcelWorkbook.WindowHeight = 7000;
                    book.ExcelWorkbook.WindowWidth = 8000;

                    // Some optional properties of the Document
                    book.Properties.Author = "HSRP";
                    book.Properties.Title = "Daily Assign Embossing Report";
                    book.Properties.Created = DateTime.Now;


                    // Add some styles to the Workbook
                    WorksheetStyle style = book.Styles.Add("HeaderStyle");
                    style.Font.FontName = "Tahoma";
                    style.Font.Size = 10;
                    style.Font.Bold = true;
                    style.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                    style.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                    style.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                    style.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                    style.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);

                    WorksheetStyle style6 = book.Styles.Add("HeaderStyle6");
                    style6.Font.FontName = "Tahoma";
                    style6.Font.Size = 10;
                    style6.Font.Bold = false;
                    style6.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                    style6.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                    style6.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                    style6.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                    style6.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);

                    WorksheetStyle style7 = book.Styles.Add("HeaderStyle7");
                    style7.Font.FontName = "Tahoma";
                    style7.Font.Size = 10;
                    style7.Font.Bold = false;
                    style7.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                    style7.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                    style7.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                    style7.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                    style7.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);

                    WorksheetStyle style2 = book.Styles.Add("HeaderStyle2");
                    style2.Font.FontName = "Tahoma";
                    style2.Font.Size = 10;
                    style2.Font.Bold = true;
                    style.Alignment.Horizontal = StyleHorizontalAlignment.Left;


                    WorksheetStyle style3 = book.Styles.Add("HeaderStyle3");
                    style3.Font.FontName = "Tahoma";
                    style3.Font.Size = 12;
                    style3.Font.Bold = true;
                    style3.Alignment.Horizontal = StyleHorizontalAlignment.Left;

                    Worksheet sheet = book.Worksheets.Add("HSRP Daily Order Report");


                    sheet.Table.Columns.Add(new WorksheetColumn(50));
                    sheet.Table.Columns.Add(new WorksheetColumn(120));
                    sheet.Table.Columns.Add(new WorksheetColumn(130));
                    sheet.Table.Columns.Add(new WorksheetColumn(100));
                    sheet.Table.Columns.Add(new WorksheetColumn(135));
                    sheet.Table.Columns.Add(new WorksheetColumn(130));
                    sheet.Table.Columns.Add(new WorksheetColumn(135));
                    sheet.Table.Columns.Add(new WorksheetColumn(130));
                    sheet.Table.Columns.Add(new WorksheetColumn(160));
                    sheet.Table.Columns.Add(new WorksheetColumn(135));
                    WorksheetRow row = sheet.Table.Rows.Add();
                    // row.
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                    row.Cells.Add(new WorksheetCell("Report :", "HeaderStyle3"));
                    WorksheetCell cell = row.Cells.Add("HSRP Daily Order Report");
                    cell.MergeAcross = 3; // Merge two cells together
                    cell.StyleID = "HeaderStyle3";

                    //row.Cells.Add(<br>);
                    row = sheet.Table.Rows.Add();
                    row.Index = 3;
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("State:", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell(GetAddress.Rows[0]["HSRPStateName"].ToString(), "HeaderStyle2"));

                    row = sheet.Table.Rows.Add();

                    row.Index = 4;
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("Location:", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell(dt.Rows[0]["RTOLocationName"].ToString(), "HeaderStyle2"));
                    //row.Cells.Add(<br>); 
                    row = sheet.Table.Rows.Add();

                    // Skip one row, and add some text
                    row.Index = 5;
                    DateTime date = System.DateTime.Now;
                    string formatted = date.ToString("dd/MM/yyyy");

                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("Date Generated :", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell(formatted, "HeaderStyle2"));
                    row = sheet.Table.Rows.Add();
                    row.Index = 6;
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("Report Date From:", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell(OrderDate1.ToString("dd/MM/yyyy"), "HeaderStyle2"));
                    row = sheet.Table.Rows.Add();
                    row.Index = 7;
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("Report Date TO:", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell(OrderDate2.ToString("dd/MM/yyyy"), "HeaderStyle2"));
                    row = sheet.Table.Rows.Add();
                    row.Index = 8;
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    //row.Cells.Add(new WorksheetCell("Order Status:", "HeaderStyle2"));
                    // row.Cells.Add(new WorksheetCell(OrderStatus, "HeaderStyle2"));
                    //row.Cells.Add(<br>); 
                    row = sheet.Table.Rows.Add();


                    row.Index = 10;
                    //row.Cells.Add("Order Date");
                    row.Cells.Add(new WorksheetCell("Sr No", "HeaderStyle"));
                    if (RadioButtonOrderDate.Checked == true)
                    {
                        row.Cells.Add(new WorksheetCell("Order Date", "HeaderStyle"));
                    }
                    else if (RadioButtonAuthorizationDate.Checked == true)
                    {
                        row.Cells.Add(new WorksheetCell("Authorization Date", "HeaderStyle"));
                    }
                  
                    row.Cells.Add(new WorksheetCell("Authorization No", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("OwnerName", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("VehicleRegNo", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("ChassisNo", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("EngineNo", "HeaderStyle"));

                    row.Cells.Add(new WorksheetCell("VehicleType", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("MobileNo.", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("NetAmount", "HeaderStyle"));

                    //row.Cells.Add(new WorksheetCell("Owner Name", "HeaderStyle"));
                    //row.Cells.Add(new WorksheetCell("Mobile No.", "HeaderStyle"));

                    row = sheet.Table.Rows.Add();

                    String StringField = String.Empty;
                    String StringAlert = String.Empty;


                    if (dt.Rows.Count <= 0)
                    {
                        string closescript1 = "<script>alert('No records found for selected Sate.')</script>";
                        Page.RegisterStartupScript("abc", closescript1);
                        return;
                    }
                    row.Index = 11;
                    Int64 DailyTarget = 0;
                    Int64 monthtarget = 0;
                    Int64 dailyactual = 0;
                    Int64 monthactual = 0;

                    Int64 yrstarget = 0;
                    Int64 yrsactual = 0;
                    Int64 MonthlyRejection = 0;
                    Int64 DailyRejection = 0;
                    Int64 YearlyRejection = 0;

                    int sno = 0;

                    foreach (DataRow dtrows in dt.Rows) // Loop over the rows.
                    {
                        sno = sno + 1;
                        row = sheet.Table.Rows.Add();
                        row.Cells.Add(new WorksheetCell(Convert.ToInt16(sno).ToString(), DataType.String, "HeaderStyle"));
                        if (RadioButtonOrderDate.Checked == true)
                        {
                            row.Cells.Add(new WorksheetCell(dtrows["OrderDate"].ToString(), DataType.String, "HeaderStyle6"));
                        }
                        else if (RadioButtonAuthorizationDate.Checked == true)
                        {
                            row.Cells.Add(new WorksheetCell(dtrows["AuthDate"].ToString(), DataType.String, "HeaderStyle6"));
                        }
                      
                        row.Cells.Add(new WorksheetCell(dtrows["HSRPRecord_AuthorizationNo"].ToString(), DataType.String, "HeaderStyle6"));
                        row.Cells.Add(new WorksheetCell(dtrows["OwnerName"].ToString(), DataType.String, "HeaderStyle6"));
                        row.Cells.Add(new WorksheetCell(dtrows["VehicleRegNo"].ToString(), DataType.String, "HeaderStyle6"));

                        row.Cells.Add(new WorksheetCell(dtrows["ChassisNo"].ToString(), DataType.String, "HeaderStyle6"));
                        row.Cells.Add(new WorksheetCell(dtrows["EngineNo"].ToString(), DataType.String, "HeaderStyle6"));
                        row.Cells.Add(new WorksheetCell(dtrows["VehicleType"].ToString(), DataType.String, "HeaderStyle7"));
                        row.Cells.Add(new WorksheetCell(dtrows["MobileNo"].ToString(), DataType.String, "HeaderStyle7"));

                        row.Cells.Add(new WorksheetCell(dtrows["NetAmount"].ToString(), DataType.String, "HeaderStyle7"));
                        // row.Cells.Add(new WorksheetCell(dtrows["MobileNo"].ToString(), DataType.String, "HeaderStyle6"));

                    }
                    row = sheet.Table.Rows.Add();

                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));

                    HttpContext context = HttpContext.Current;
                    context.Response.Clear();
                    // Save the file and open it
                    book.Save(Response.OutputStream);

                    //context.Response.ContentType = "text/csv";
                    context.Response.ContentType = "application/vnd.ms-excel";

                    context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                    context.Response.End();
                }
                else
                {
                    string closescript1 = "<script>alert('No records found for selected date.')</script>";
                    Page.RegisterStartupScript("abc", closescript1);
                    return;
                }
            }

            catch (Exception ex)
            {
                //LabelError.Visible = true;
                lblErrMsg.Text = "Error in Populating Grid :" + ex.Message.ToString();
            }
        }

        protected void ButtonPDF_Click(object sender, EventArgs e)
        {
            int i = 0;
            String[] StringAuthDate = OrderDate.SelectedDate.ToString().Split('/');
            string MonTo = ("0" + StringAuthDate[0]);
            string MonthdateTO = MonTo.Replace("00", "0").Replace("01", "1");
            String ReportDateFrom = StringAuthDate[1] + "/" + MonthdateTO + "/" + StringAuthDate[2].Split(' ')[0];
            String From = StringAuthDate[0] + "/" + StringAuthDate[1] + "/" + StringAuthDate[2].Split(' ')[0];

            FromDate1 = (From + " 00:00:00");
            OrderDate1 = DateTime.Parse(From + " 00:00:00");

            String[] StringOrderDate = HSRPAuthDate.SelectedDate.ToString().Split('/');
            string Mon = ("0" + StringOrderDate[0]);
            string Monthdate = Mon.Replace("00", "0").Replace("01", "1");
            String FromDate = StringOrderDate[1] + "-" + Monthdate + "-" + StringOrderDate[2].Split(' ')[0];
            String ReportDateTo = StringOrderDate[1] + "/" + Monthdate + "/" + StringOrderDate[2].Split(' ')[0];
            String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];

            ToDate = (From1 + " 23:59:59");

            OrderDate2 = DateTime.Parse(From1 + " 23:59:59");





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


            //int.TryParse(dropDownListHSRPState.SelectedValue, out intHSRPStateID);
            //int.TryParse(dropDownListRTOLocation.SelectedValue, out intRTOLocationID);
            HSRPStateID = dropDownListHSRPState.SelectedValue;
            RTOLocationID = dropDownListRTOLocation.SelectedValue;

            if (RadioButtonOrderDate.Checked == true)
            {
                SQLString = " SELECT     CONVERT(varchar(30), HSRPRecords.HSRPRecord_AuthorizationDate,103) as AuthDate, CONVERT(varchar(30), HSRPRecords.OrderDate,103) as OrderDate ,HSRPRecords.OrderStatus,HSRPRecords.[HSRPRecordID],HSRPRecords.[HSRP_StateID],HSRPRecords.HSRPRecord_AuthorizationNo,  HSRPRecords.OwnerName,  HSRPRecords.VehicleRegNo,  HSRPRecords.ChassisNo,  HSRPRecords.EngineNo,  HSRPRecords.VehicleType,  HSRPRecords.MobileNo,  HSRPRecords.NetAmount, HSRPState.HSRPStateName,dbo.RTOLocation.RTOLocationName FROM HSRPRecords inner join 	HSRPState on HSRPRecords.HSRP_StateID = HSRPState.HSRP_StateID inner join RTOLocation on  HSRPRecords.RTOLocationID = RTOLocation.RTOLocationID	where HSRPRecords.HSRP_StateID=" + HSRPStateID + " and HSRPRecords.RTOLocationID=" + RTOLocationID + " and OrderStatus!='Closed' and  HSRPRecords.OrderDate between '" + FromDate1 + "' and '" + ToDate + "' order by OrderDate";
                
            }
            else if (RadioButtonAuthorizationDate.Checked == true)
            {

                SQLString = " SELECT     CONVERT(varchar(30), HSRPRecords.HSRPRecord_AuthorizationDate,103) as AuthDate,CONVERT(varchar(30), HSRPRecords.OrderDate,103) as OrderDate,HSRPRecords.OrderStatus,HSRPRecords.[HSRPRecordID],HSRPRecords.[HSRP_StateID],HSRPRecords.HSRPRecord_AuthorizationNo,  HSRPRecords.OwnerName,  HSRPRecords.VehicleRegNo,  HSRPRecords.ChassisNo,  HSRPRecords.EngineNo,  HSRPRecords.VehicleType,  HSRPRecords.MobileNo,  HSRPRecords.NetAmount, HSRPState.HSRPStateName,dbo.RTOLocation.RTOLocationName FROM HSRPRecords inner join 	HSRPState on HSRPRecords.HSRP_StateID = HSRPState.HSRP_StateID inner join RTOLocation on  HSRPRecords.RTOLocationID = RTOLocation.RTOLocationID	where HSRPRecords.HSRP_StateID=" + HSRPStateID + " and HSRPRecords.RTOLocationID=" + RTOLocationID + " and OrderStatus!='Closed' and  HSRPRecords.HSRPRecord_AuthorizationDate between '" + FromDate1 + "' and '" + ToDate + "' order by OrderDate";

            }

            DataTable dt = Utils.GetDataTable(SQLString, CnnString);
            //Grid1.DataSource = dt;
            //Grid1.DataBind();

            if (dt.Rows.Count > 0)
            {

                i = dt.Rows.Count;


                string filename = "HSRP Order Report" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".pdf";

                //string SQLString = String.Empty;
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
                PdfPTable table = new PdfPTable(9);
                //actual width of table in points
                table.TotalWidth = 1500f;

                PdfPCell cell120911 = new PdfPCell(new Phrase("HSRP Daily Order Report", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell120911.Colspan = 9;
                cell120911.BorderWidthLeft = 0f;
                cell120911.BorderWidthRight = 0f;
                cell120911.BorderWidthTop = 0f;
                cell120911.BorderWidthBottom = 0f;

                cell120911.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell120911);

                PdfPCell cell12091 = new PdfPCell(new Phrase("State Name : " + GetAddress.Rows[0]["HSRPStateName"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell12091.Colspan =5;
                cell12091.BorderWidthLeft = 1f;
                cell12091.BorderWidthRight = 0f;
                cell12091.BorderWidthTop = 1f;
                cell12091.BorderWidthBottom = 0f;

                cell12091.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12091);

                // PdfPCell cell12093 = new PdfPCell(new Phrase("Date : " + DateTime.Now.ToString("dd/MM/yyyy"), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                PdfPCell cell12093 = new PdfPCell(new Phrase("Report Date From : " + OrderDate1.ToString("dd/MM/yyyy"), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell12093.Colspan = 4;
                cell12093.BorderWidthLeft = 0f;
                cell12093.BorderWidthRight = 1f;
                cell12093.BorderWidthTop = 1f;
                cell12093.BorderWidthBottom = 0f;

                cell12093.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12093);

                PdfPCell cell12092 = new PdfPCell(new Phrase("RTOLocation Name : " + dt.Rows[0]["RTOLocationName"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell12092.Colspan = 3;
                cell12092.BorderWidthLeft = 1f;
                cell12092.BorderWidthRight = 0f;
                cell12092.BorderWidthTop = 0f;
                cell12092.BorderWidthBottom = 0f;

                cell12092.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12092);
                PdfPCell cell12094 = new PdfPCell(new Phrase("Order Status : " + OrderDate2.ToString("dd/MM/yyyy"), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell12094.Colspan = 3;
                cell12094.BorderWidthLeft = 0f;
                cell12094.BorderWidthRight = 0f;
                cell12094.BorderWidthTop = 0f;
                cell12094.BorderWidthBottom = 0f;

                cell12094.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12094);

                PdfPCell cell12095 = new PdfPCell(new Phrase("Report Date To : " + OrderDate2.ToString("dd/MM/yyyy"), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell12095.Colspan = 3;
                cell12095.BorderWidthLeft = 0f;
                cell12095.BorderWidthRight = 1f;
                cell12095.BorderWidthTop = 0f;
                cell12095.BorderWidthBottom = 0f;

                cell12095.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12095);

                if (RadioButtonOrderDate.Checked == true)
                {

                    PdfPCell cell12099 = new PdfPCell(new Phrase("Order Date", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    cell12099.Colspan = 1;
                    cell12099.BorderWidthLeft = 1f;
                    cell12099.BorderWidthRight = .8f;
                    cell12099.BorderWidthTop = 1f;
                    cell12099.BorderWidthBottom = 1f;

                    cell12099.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell12099);
                }
                else if (RadioButtonAuthorizationDate.Checked == true)
                {

                    PdfPCell cell12099 = new PdfPCell(new Phrase("Authorization Date", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    cell12099.Colspan = 1;
                    cell12099.BorderWidthLeft = 1f;
                    cell12099.BorderWidthRight = .8f;
                    cell12099.BorderWidthTop = 1f;
                    cell12099.BorderWidthBottom = 1f;

                    cell12099.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell12099);

                }


              


                PdfPCell cell1209 = new PdfPCell(new Phrase("Authorization No", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell1209.Colspan = 1;
                cell1209.BorderWidthLeft = 1f;
                cell1209.BorderWidthRight = .8f;
                cell1209.BorderWidthTop = 1f;
                cell1209.BorderWidthBottom = 1f;

                cell1209.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1209);

                PdfPCell cell1213 = new PdfPCell(new Phrase("Owner Name", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell1213.Colspan = 1;
                cell1213.BorderWidthLeft = 0f;
                cell1213.BorderWidthRight = .8f;
                cell1213.BorderWidthTop = 1f;
                cell1213.BorderWidthBottom = 1f;

                cell1213.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1213);



                PdfPCell cell12233 = new PdfPCell(new Phrase("Vehicle Reg No", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell12233.Colspan = 1;
                cell12233.BorderWidthLeft = 0f;
                cell12233.BorderWidthRight = .8f;
                cell12233.BorderWidthTop = 1f;
                cell12233.BorderWidthBottom = 1f;

                cell12233.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12233);

                PdfPCell cell122331 = new PdfPCell(new Phrase("Chassis No", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell122331.Colspan = 1;
                cell122331.BorderWidthLeft = 0f;
                cell122331.BorderWidthRight = .8f;
                cell122331.BorderWidthTop = 1f;
                cell122331.BorderWidthBottom = 1f;

                cell122331.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell122331);


                PdfPCell cell122332 = new PdfPCell(new Phrase("Engine No", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell122332.Colspan = 1;
                cell122332.BorderWidthLeft = 0f;
                cell122332.BorderWidthRight = .8f;
                cell122332.BorderWidthTop = 1f;
                cell122332.BorderWidthBottom = 1f;

                cell122332.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell122332);

                PdfPCell cell1206 = new PdfPCell(new Phrase("Vehicle Type", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell1206.Colspan = 1;
                cell1206.BorderWidthLeft = 0f;
                cell1206.BorderWidthRight = .8f;
                cell1206.BorderWidthTop = 1f;
                cell1206.BorderWidthBottom = 1f;
                cell1206.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1206);

                PdfPCell cell1221 = new PdfPCell(new Phrase("Mobile No", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell1221.Colspan = 1;
                cell1221.BorderWidthLeft = 0f;
                cell1221.BorderWidthRight = 1f;
                cell1221.BorderWidthTop = 1f;
                cell1221.BorderWidthBottom = 1f;

                cell1221.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1221);


                PdfPCell cell120933 = new PdfPCell(new Phrase("Net Amount", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell120933.Colspan = 1;
                cell120933.BorderWidthLeft = 1f;
                cell120933.BorderWidthRight = .8f;
                cell120933.BorderWidthTop = 1f;
                cell120933.BorderWidthBottom = 1f;

                cell120933.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell120933);

                //PdfPCell cell120934 = new PdfPCell(new Phrase("Mobile No", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                //cell120934.Colspan = 1;
                //cell120934.BorderWidthLeft = 1f;
                //cell120934.BorderWidthRight = .8f;
                //cell120934.BorderWidthTop = 1f;
                //cell120934.BorderWidthBottom = 1f;

                //cell120934.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                //table.AddCell(cell120934);
                //PdfPCell cell1223 = new PdfPCell(new Phrase("Owner Name", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                //cell1223.Colspan = 2;
                //cell1223.BorderWidthLeft = 0f;
                //cell1223.BorderWidthRight = 1f;
                //cell1223.BorderWidthTop = 1f;
                //cell1223.BorderWidthBottom = 1f;
                //cell1223.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                //table.AddCell(cell1223);
                i = i - 1;
                while (i >= 0)
                {

                    if (RadioButtonOrderDate.Checked == true)
                    {

                        PdfPCell cell12111 = new PdfPCell(new Phrase(dt.Rows[i]["OrderDate"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell12111.Colspan = 1;
                        cell12111.BorderWidthLeft = 1f;
                        cell12111.BorderWidthRight = .8f;
                        cell12111.BorderWidthTop = .5f;
                        cell12111.BorderWidthBottom = .5f;

                        cell12111.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell12111);
                    }
                    else if (RadioButtonAuthorizationDate.Checked == true)
                    {

                        PdfPCell cell12111 = new PdfPCell(new Phrase(dt.Rows[i]["AuthDate"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell12111.Colspan = 1;
                        cell12111.BorderWidthLeft = 1f;
                        cell12111.BorderWidthRight = .8f;
                        cell12111.BorderWidthTop = .5f;
                        cell12111.BorderWidthBottom = .5f;

                        cell12111.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell12111);

                    }

                  

                    PdfPCell cell1211 = new PdfPCell(new Phrase(dt.Rows[i]["HSRPRecord_AuthorizationNo"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell1211.Colspan = 1;
                    cell1211.BorderWidthLeft = 1f;
                    cell1211.BorderWidthRight = .8f;
                    cell1211.BorderWidthTop = .5f;
                    cell1211.BorderWidthBottom = .5f;

                    cell1211.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell1211);




                    PdfPCell cell1214 = new PdfPCell(new Phrase(dt.Rows[i]["OwnerName"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell1214.Colspan = 1;
                    cell1214.BorderWidthLeft = 0f;
                    cell1214.BorderWidthRight = .8f;
                    cell1214.BorderWidthTop = .5f;
                    cell1214.BorderWidthBottom = .5f;

                    cell1214.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell1214);

                    PdfPCell cell1219 = new PdfPCell(new Phrase(dt.Rows[i]["VehicleRegNo"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell1219.Colspan = 1;
                    cell1219.BorderWidthLeft = 0f;
                    cell1219.BorderWidthRight = .8f;
                    cell1219.BorderWidthTop = .5f;
                    cell1219.BorderWidthBottom = .5f;

                    cell1219.HorizontalAlignment = 0; //0=Left, 1=     //PdfPCell cell1218 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));

                    table.AddCell(cell1219);



                    //string FPS = dt.Tables[0].Rows[i]["FrontPlateSize"].ToString();
                    //string SQLString1 = "select * from Product where ProductID='" + FPS + "'";

                    //dt1 = Utils.getDataSet(SQLString1, CnnString);




                    PdfPCell cell12193 = new PdfPCell(new Phrase(dt.Rows[i]["ChassisNo"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell12193.Colspan = 1;
                    cell12193.BorderWidthLeft = 0f;
                    cell12193.BorderWidthRight = .8f;
                    cell12193.BorderWidthTop = .5f;
                    cell12193.BorderWidthBottom = .5f;

                    cell12193.HorizontalAlignment = 0; //0=Left, 1=     //PdfPCell cell1218 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));

                    table.AddCell(cell12193);


                    //string FPS1 = dt.Tables[0].Rows[i]["RearPlateSize"].ToString();
                    //string SQLString2 = "select * from Product where ProductID='" + FPS1 + "'";

                    //dt2 = Utils.getDataSet(SQLString2, CnnString);

                    PdfPCell cell12194 = new PdfPCell(new Phrase(dt.Rows[i]["EngineNo"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell12194.Colspan = 1;
                    cell12194.BorderWidthLeft = 0f;
                    cell12194.BorderWidthRight = .8f;
                    cell12194.BorderWidthTop = .5f;
                    cell12194.BorderWidthBottom = .5f;

                    cell12194.HorizontalAlignment = 0; //0=Left, 1=     //PdfPCell cell1218 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));

                    table.AddCell(cell12194);


                    PdfPCell cell1216 = new PdfPCell(new Phrase(dt.Rows[i]["VehicleType"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell1216.Colspan = 1;
                    cell1216.BorderWidthLeft = 0f;
                    cell1216.BorderWidthRight = .8f;
                    cell1216.BorderWidthTop = .5f;
                    cell1216.BorderWidthBottom = .5f;

                    cell1216.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell1216);


                    PdfPCell cell1222 = new PdfPCell(new Phrase(dt.Rows[i]["MobileNo"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell1222.Colspan = 1;
                    cell1222.BorderWidthLeft = 0f;
                    cell1222.BorderWidthRight = .8f;
                    cell1222.BorderWidthTop = .5f;
                    cell1222.BorderWidthBottom = .5f;

                    cell1222.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell1222);




                    PdfPCell cell120935 = new PdfPCell(new Phrase(dt.Rows[i]["NetAmount"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell120935.Colspan = 1;
                    cell120935.BorderWidthLeft = 0f;
                    cell120935.BorderWidthRight = .8f;
                    cell120935.BorderWidthTop = .5f;
                    cell120935.BorderWidthBottom = .5f;

                    cell120935.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell120935);

                    //PdfPCell cell120936 = new PdfPCell(new Phrase(dt.Tables[0].Rows[i]["MobileNo"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    //cell120936.Colspan = 1;
                    //cell120936.BorderWidthLeft = 0f;
                    //cell120936.BorderWidthRight = .8f;
                    //cell120936.BorderWidthTop = .5f;
                    //cell120936.BorderWidthBottom = .5f;

                    //cell120936.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    //table.AddCell(cell120936);
                     
                    i--; 
                } 
                // document.Add(table);
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
            else
            {
                string closescript1 = "<script>alert('No records found for selected date.')</script>";
                Page.RegisterStartupScript("abc", closescript1);
                return;
            } 
        }
        private void ClearGrid()
        {
            Grid1.Items.Clear();
            lblErrMsg.Text = String.Empty;
            return;
        }

        protected void Linkbutton1_Click(object sender, EventArgs e)
        {
            ClearGrid();

            SQLString = "SELECT  CONVERT(varchar(20), HSRPRecords.OrderDate,103)  as OrderDate,HSRPRecord_AuthorizationNo,   convert(varchar,  HSRPRecords.HSRPRecord_AuthorizationDate, 105) as HSRPRecord_AuthorizationDate,      convert(varchar, OrderClosedDate, 105) as InvoiceDateTime,     convert(varchar, OrderEmbossingDate, 105) as OrderEmbossingDate,     HSRPRecords.HSRP_Front_LaserCode, HSRPRecords.HSRP_Rear_LaserCode, HSRPRecords.OrderStatus,     HSRPRecords.[HSRPRecordID],HSRPRecords.[HSRP_StateID],HSRPRecords.HSRPRecord_AuthorizationNo,       HSRPRecords.OwnerName,  HSRPRecords.VehicleRegNo,  HSRPRecords.ChassisNo,  HSRPRecords.EngineNo,        HSRPRecords.VehicleType,  HSRPRecords.MobileNo,  HSRPRecords.NetAmount FROM HSRPRecords where HSRPRecords.HSRP_StateID=" + HSRPStateID + "  and VehicleRegNo like '%" + textboxSearch.Text + "%'  order by vehicleRegNo";

            DataTable dt = Utils.GetDataTable(SQLString, CnnString);
            if (dt.Rows.Count > 0)
            {
                Grid1.DataSource = dt;
                Grid1.RunningMode = (ComponentArt.Web.UI.GridRunningMode)Enum.Parse(typeof(ComponentArt.Web.UI.GridRunningMode), "Client");
                Grid1.SearchOnKeyPress = true;
                Grid1.DataBind();
                Grid1.RecordCount.ToString();
            }
            else
            {
                Grid1.Items.Clear();
                ClearGrid();
            }
        }

        protected void LinkButton2_Click(object sender, EventArgs e)
        {
            //Response.Redirect("Hsrprecords.aspx");
        }

        

        //protected void RadioButtonAuthorizationDate_CheckedChanged(object sender, EventArgs e)
        //{
        //    RadioButtonOrderDate.Checked = false;
        //}

        //protected void RadioButtonOrderDate_CheckedChanged(object sender, EventArgs e)
        //{
        //    RadioButtonAuthorizationDate.Checked = false;
        //}

    }
}