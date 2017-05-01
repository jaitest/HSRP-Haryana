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


namespace HSRP.Master
{
    public partial class StickerPrintAll : System.Web.UI.Page
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
        protected void Page_Load(object sender, EventArgs e)
        {
            Utils.GZipEncodePage();
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {
                if (Session["UserHSRPStateID"].ToString() == "6")
                {
                    ButImpData.Visible = true;
                }
                else
                {
                    ButImpData.Visible = false;
                }

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

                        ShowGrid();
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

                SQLString = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, (a.RTOLocationCode+', ') as RTOCode, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID where UserRTOLocationMapping.UserID='" + UserID + "' ";
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

        private void ShowGrid()
        {
            if (dropDownListClient.SelectedItem.Text != "--Select RTO Name--")
            {
                SQLString = "select distinct pdffilename, (select UserFirstName +' '+ userLastName  from users where userID=pdfdownloaduserID) as userName, convert(varchar,pdfdownloadDate, 111) as pdfdownloadDate  from hsrprecords where hsrp_StateID='" + DropDownListStateName.SelectedValue + "' and rtolocationid='" + dropDownListClient.SelectedValue + "' and pdffilename is not null order by pdfdownloaddate desc";
                DataTable dts = Utils.GetDataTable(SQLString, CnnString);
                Grid1.DataSource = dts;
                Grid1.DataBind(); 
            }
            else
            {
                Grid1.Items.Clear();
                lblErrMsg.Text = String.Empty;
                lblErrMsg.Text = "Please Select Location.";
                return;
            }
        }



        protected void Grid1_ItemCommand(object sender, ComponentArt.Web.UI.GridItemCommandEventArgs e)
        {

        }
        protected void DropDownListStateName_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilldropDownListClient();
            dropDownListClient.Visible = true;
            labelClient.Visible = true;

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
            ShowGrid();
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

        protected void btnDownloadRecords_Click(object sender, EventArgs e)
        {
            lblErrMsg.Text = "";
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
            try
            {
                SQLString = "select * from hsrprecords where FrontPlateSize ='--Select Front Plate--'";
                Utils.ExecNonQuery(SQLString, CnnString);

                SQLString = "update hsrprecords set RearPlateSize ='0' where RearPlateSize ='--Select Rear Plate--'";
                Utils.ExecNonQuery(SQLString, CnnString);

            }
            catch
            {
            }



            SQLString = "select a.HSRPRecordID,a.vehicleRegNo, a.HSRP_Sticker_LaserCode, a.StickerMandatory, a.VehicleRegNo,a.EngineNo,a.ChassisNo,a.HSRP_Front_LaserCode,a.HSRP_Rear_LaserCode,b.RTOLocationname ,b.RTOLocationCode , HSRPState.HSRPStateName ,HSRPState.statetext from HSRPRecords a inner join rtolocation  as b on a.RTOLocationID =b.RTOLocationID inner join HSRPState  on HSRPState.HSRP_StateID= b.HSRP_StateID where  a.hsrp_StateID='"+DropDownListStateName.SelectedValue+"' and a.rtolocationID='"+dropDownListClient.SelectedValue+"' and  a.HSRP_Front_LaserCode is not null and a.HSRP_Front_LaserCode!=''";
            DataTable ds = Utils.GetDataTable(SQLString, CnnString);
            
            if (dropDownListorderStatus.SelectedItem.Text == "TVS Printer")
            {

                if (ds.Rows.Count > 0)
                {
                    string Stricker = ds.Rows[0]["StickerMandatory"].ToString();
                    if (Stricker == "Y")
                    {
                        string filename = "Sticker" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".pdf";

                        String StringField = String.Empty;
                        String StringAlert = String.Empty;

                        StringBuilder bb = new StringBuilder();

                        Document document = new Document(PageSize.A4, 0, 0, 220, 0);

                        document.SetPageSize(new iTextSharp.text.Rectangle(400, 300));
                        document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
                        BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
                        string PdfFolder = ConfigurationManager.AppSettings["PdfFolder"].ToString() + filename;
                        PdfWriter.GetInstance(document, new FileStream(PdfFolder, FileMode.Create));  
                        document.Open();

                        PdfPTable table = new PdfPTable(1); 
                        for (int ti = 0; ti < ds.Rows.Count; ti++)
                        {     
                        table.TotalWidth = 300f; 
                        fontpath = ConfigurationManager.AppSettings["DataFolder"].ToString() + "PRSANSR.TTF";
                        basefont = BaseFont.CreateFont(fontpath, BaseFont.IDENTITY_H, true); 
                        StringBuilder sbtrnasportname = new StringBuilder();
                        trnasportname = "TRANSPORT DEPARTMENT";

                        for (int i = trnasportname.Length - 1; i >= 0; i--)
                        {
                            sbtrnasportname.Append(trnasportname[i].ToString());
                        }

                        //fix the absolute width of the table
                        PdfPCell cell6 = new PdfPCell(new Phrase(sbtrnasportname.ToString(), new iTextSharp.text.Font(basefont, 17f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                        // cell.Colspan = 4;
                        cell6.PaddingRight = 240f;
                        cell6.BorderColor = BaseColor.WHITE;
                        cell6.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell6);
                        StringBuilder sb = new StringBuilder();
                        StringBuilder sb1 = new StringBuilder();
                        string statename = ds.Rows[0]["statetext"].ToString().ToUpper();

                        for (int i = statename.Length - 1; i >= 0; i--)
                        {
                            sb.Append(statename[i].ToString());
                        }

                        string HSRPStateName = ds.Rows[ti]["HSRPStateName"].ToString().ToUpper();

                        for (int i = HSRPStateName.Length - 1; i >= 0; i--)
                        {
                            sb1.Append(HSRPStateName[i].ToString());
                        }
                        if (HSRPStateName == "HIMACHAL PRADESH" || HSRPStateName == "MADHYA PRADESH" || HSRPStateName == "ANDHRA PRADESH" || HSRPStateName == "UTTRAKHAND")
                        {
                            PdfPCell cell = new PdfPCell(new Phrase(sb1.ToString() + " " + sb.ToString(), new iTextSharp.text.Font(basefont, 13f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                            cell.PaddingRight = 240f;
                            cell.PaddingTop = 8f;
                            cell.BorderColor = BaseColor.WHITE;
                            cell.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell);
                        }
                        else if (HSRPStateName == "HARYANA")
                        {
                            PdfPCell cell = new PdfPCell(new Phrase(sb1.ToString() + " " + sb.ToString(), new iTextSharp.text.Font(basefont, 16f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                            // cell.Colspan = 4;
                            cell.PaddingRight = 243f;
                            cell.PaddingTop = 8f;
                            cell.BorderColor = BaseColor.WHITE;
                            cell.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell);
                        }
                        else
                        {
                            PdfPCell cell = new PdfPCell(new Phrase(sb1.ToString() + " " + sb.ToString(), new iTextSharp.text.Font(basefont, 18f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                            // cell.Colspan = 4;
                            //cell.PaddingRight = 240f;
                            cell.PaddingTop = 8f;
                            cell.BorderColor = BaseColor.WHITE;
                            cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell);
                        }
                        StringBuilder sbVehicleRegNo = new StringBuilder();

                        string VehicleRegNo = ds.Rows[0]["VehicleRegNo"].ToString().ToUpper();

                        for (int i = VehicleRegNo.Length - 1; i >= 0; i--)
                        {
                            sbVehicleRegNo.Append(VehicleRegNo[i].ToString());
                        }

                        PdfPCell cell2 = new PdfPCell(new Phrase(sbVehicleRegNo.ToString(), new iTextSharp.text.Font(basefont, 27f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                        // cell.Colspan = 4;
                        cell2.PaddingTop = 8f;
                        cell2.BorderColor = BaseColor.WHITE;
                        cell2.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell2);
                        StringBuilder sbHSRP_Front_LaserCode = new StringBuilder();
                        StringBuilder sbHSRP_Rear_LaserCode = new StringBuilder();
                        string HSRP_Front_LaserCode = ds.Rows[ti]["HSRP_Front_LaserCode"].ToString().ToUpper();

                        for (int i = HSRP_Front_LaserCode.Length - 1; i >= 0; i--)
                        {
                            sbHSRP_Front_LaserCode.Append(HSRP_Front_LaserCode[i].ToString());
                        }

                        string HSRP_Rear_LaserCode = ds.Rows[0]["HSRP_Rear_LaserCode"].ToString().ToUpper();

                        for (int i = HSRP_Rear_LaserCode.Length - 1; i >= 0; i--)
                        {
                            sbHSRP_Rear_LaserCode.Append(HSRP_Rear_LaserCode[i].ToString());
                        }

                        PdfPCell cell3 = new PdfPCell(new Phrase("" + sbHSRP_Rear_LaserCode.ToString() + " - " + sbHSRP_Front_LaserCode.ToString(), new iTextSharp.text.Font(basefont, 13f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                        // cell.Colspan = 4;
                        cell3.PaddingTop = 8f;
                        cell3.BorderColor = BaseColor.WHITE;
                        cell3.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell3);

                        StringBuilder sbEngineNo = new StringBuilder();
                        string EngineNo = "ENGINE NO - " + ds.Rows[ti]["EngineNo"].ToString().ToUpper();

                        for (int i = EngineNo.Length - 1; i >= 0; i--)
                        {
                            sbEngineNo.Append(EngineNo[i].ToString());
                        }


                        PdfPCell cell4 = new PdfPCell(new Phrase(sbEngineNo.ToString(), new iTextSharp.text.Font(basefont, 13f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                        // cell.Colspan = 4;
                        cell4.PaddingTop = 8f;
                        cell4.PaddingRight = 245f;
                        cell4.BorderColor = BaseColor.WHITE;
                        cell4.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell4);

                        StringBuilder sbChassisNo = new StringBuilder();
                        string ChassisNo = "CHASIS NO - " + ds.Rows[0]["ChassisNo"].ToString().ToUpper();

                        for (int i = ChassisNo.Length - 1; i >= 0; i--)
                        {
                            sbChassisNo.Append(ChassisNo[i].ToString());
                        }

                        PdfPCell cell5 = new PdfPCell(new Phrase(sbChassisNo.ToString(), new iTextSharp.text.Font(basefont, 13f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                        // cell.Colspan = 4;
                        cell5.PaddingTop = 8f;
                        cell5.PaddingRight = 245f;
                        cell5.BorderColor = BaseColor.WHITE;
                        cell5.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell5); 
                        }

                        PdfPCell cell4w = new PdfPCell(new Phrase("", new iTextSharp.text.Font(basefont, 13f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                        // cell.Colspan = 4;
                        cell4w.PaddingTop = 8f;
                        cell4w.PaddingRight = 245f;
                        cell4w.BorderColor = BaseColor.WHITE;
                        cell4w.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell4w);

                        PdfPCell cell4wa = new PdfPCell(new Phrase("", new iTextSharp.text.Font(basefont, 13f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                        // cell.Colspan = 4;
                        cell4wa.PaddingTop = 8f;
                        cell4wa.PaddingRight = 245f;
                        cell4wa.BorderColor = BaseColor.WHITE;
                        cell4wa.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell4wa);

                        PdfPCell cell4wz = new PdfPCell(new Phrase("", new iTextSharp.text.Font(basefont, 13f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                        // cell.Colspan = 4;
                        cell4wz.PaddingTop = 8f;
                        cell4wz.PaddingRight = 245f;
                        cell4wz.BorderColor = BaseColor.WHITE;
                        cell4wz.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell4wz);

                        PdfPCell cell4waa = new PdfPCell(new Phrase("", new iTextSharp.text.Font(basefont, 13f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                        // cell.Colspan = 4;
                        cell4waa.PaddingTop = 8f;
                        cell4waa.PaddingRight = 245f;
                        cell4waa.BorderColor = BaseColor.WHITE;
                        cell4waa.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell4waa);


                        PdfPCell cell4wzq = new PdfPCell(new Phrase("", new iTextSharp.text.Font(basefont, 13f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                        // cell.Colspan = 4;
                        cell4wzq.PaddingTop = 8f;
                        cell4wzq.PaddingRight = 245f;
                        cell4wzq.BorderColor = BaseColor.WHITE;
                        cell4wzq.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell4wzq);

                        PdfPCell cell4waax = new PdfPCell(new Phrase("", new iTextSharp.text.Font(basefont, 13f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                        // cell.Colspan = 4;
                        cell4waax.PaddingTop = 8f;
                        cell4waax.PaddingRight = 245f;
                        cell4waax.BorderColor = BaseColor.WHITE;
                        cell4waax.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell4waax);


                        document.Add(table);
                        document.Close();
                        HttpContext context = HttpContext.Current;

                        context.Response.ContentType = "Application/pdf";
                        context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                        context.Response.WriteFile(PdfFolder);
                        context.Response.End();
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(typeof(Page), "alert", "<script language=JavaScript>alert('Sticker Not Allow !!');</script>");
                    }
                }
            }

            if (dropDownListorderStatus.SelectedItem.Text == "Zebra Printer")
            {

            }
            lblErrMsg.Text = "No Record Found!!";
        }
    }
}