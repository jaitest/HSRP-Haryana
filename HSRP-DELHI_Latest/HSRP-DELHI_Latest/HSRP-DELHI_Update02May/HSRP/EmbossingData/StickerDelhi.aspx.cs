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
using System.Web;
using System.Collections.Generic;
using System.Linq;



namespace HSRP.EmbossingData
{
    public partial class StickerDelhi : System.Web.UI.Page
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
        string AuthorizationDate = string.Empty;
        string ToDate = string.Empty;
        string DealerId = string.Empty;
        DataTable dtdealerid = new DataTable();
        string D_id = string.Empty;

        BaseFont basefont;


        string AllLocation = string.Empty;
        string OrderStatus = string.Empty;

        DataProvider.BAL bl = new DataProvider.BAL();
        BAL obj = new BAL();
        string StickerManditory = string.Empty;

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
                            Filldropdowndealer();
                        }
                        else
                        {
                            hiddenUserType.Value = "1";
                            labelOrganization.Enabled = false;
                            DropDownListStateName.Enabled = false;
                            labelClient.Enabled = false;
                            FilldropDownListOrganization();
                            FilldropDownListClient();
                            Filldropdowndealer();
                            
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

                SQLString = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, (a.RTOLocationCode+', ') as RTOCode, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID where UserRTOLocationMapping.UserID='" + UserID + "' order by a.RTOLocationName";
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
                    lblRTOCode.Text = RTOCode; //RTOCode.Remove(RTOCode.LastIndexOf(","));
                }
            }
        }


        private void Filldropdowndealer()
        {
            if (UserType == "0")
            {
                SQLString = "select [NAME OF THE DEALER] from delhi_dealermaster  where ActiveStatus='Y'";
                Utils.PopulateDropDownList(DropDownList1, SQLString.ToString(), CnnString, "All");


            }
            else
            {
                SQLString = "select [NAME OF THE DEALER]  from delhi_dealermaster  where  ActiveStatus='Y'";
                Utils.PopulateDropDownList(DropDownList1, SQLString.ToString(), CnnString, "All");
                //DataTable dts = Utils.GetDataTable(SQLString, CnnString);
                //ddlBothDealerHHT.DataSource = dts;
                //ddlBothDealerHHT.DataBind();
            }
        }
        #endregion


        private void InitialSetting()
        {

            string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            string MaxDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            HSRPAuthDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            HSRPAuthDate.MaxDate = DateTime.Parse(MaxDate);
            CalendarHSRPAuthDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarHSRPAuthDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            OrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            OrderDate.MaxDate = DateTime.Parse(MaxDate);
            CalendarOrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarOrderDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
        }


        protected void btnGO_Click(object sender, EventArgs e)
        {

            string ddVehicleType = DropDownList1.SelectedItem.ToString().ToUpper();

            string DealerName = DropDownList1.SelectedValue;
            string Dname = string.Empty;
            string Did = string.Empty;
            string DealerId = string.Empty;
            try
            {
                string SQLString1 = "select distinct SNO from delhi_dealermaster where isnull(erpclientcode,'HOLD')!='HOLD' and  [NAME OF THE DEALER]='" + DealerName + "'";
                DealerId = Utils.getDataSingleValue(SQLString1, CnnString, "SNO");
            }
            catch (Exception ex)
            { }

            labelOrderStatus.Visible = false;
            dropdownDuplicateFIle.Visible = false;
           

            string type = "1";
            String[] StringAuthDate = OrderDate.SelectedDate.ToString().Split('/');
            string MonTo = ("0" + StringAuthDate[0]);
            string MonthdateTO = MonTo.Replace("00", "0").Replace("01", "1");
            String ReportDateFrom = StringAuthDate[1] + "/" + MonthdateTO + "/" + StringAuthDate[2].Split(' ')[0];
            String From = StringAuthDate[0] + "/" + StringAuthDate[1] + "/" + StringAuthDate[2].Split(' ')[0];
            //AuthorizationDate = new DateTime(Convert.ToInt32(StringAuthDate[2].Split(' ')[0]), Convert.ToInt32(StringAuthDate[0]), Convert.ToInt32(StringAuthDate[1]));
            AuthorizationDate = From + " 00:00:00";
            String[] StringOrderDate = HSRPAuthDate.SelectedDate.ToString().Split('/');
            string Mon = ("0" + StringOrderDate[0]);
            string Monthdate = Mon.Replace("00", "0").Replace("01", "1");
            String FromDate = StringOrderDate[1] + "-" + Monthdate + "-" + StringOrderDate[2].Split(' ')[0];
            String ReportDateTo = StringOrderDate[1] + "/" + Monthdate + "/" + StringOrderDate[2].Split(' ')[0];
            String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
            ToDate = From1 + " 23:59:59";

            ShowGridDelhi();

            //SQLString = "select Distinct  pdffilename  from hsrprecords where hsrp_StateID='" + DropDownListStateName.SelectedValue + "' and rtolocationID='" + dropDownListClient.SelectedValue + "' and  pdffilename is not null and hsrprecord_CreationDate between '" + AuthorizationDate + "' and  '" + ToDate + "'  group by  pdffilename, convert(varchar,pdfdownloaddate,111)";
            //Utils.PopulateDropDownList(dropdownDuplicateFIle, SQLString.ToString(), CnnString, "--Select File Name--");
            //GridView1.DataSource = null;
            //GridView1.DataBind();


           


        }


        private void ShowGridDelhi()
        {

            try
            {
                string DealerTab = DropDownList1.SelectedItem.ToString();
                if (dropDownListClient.SelectedItem.Text != "--Select RTO Name--")
                {
                    if (DealerTab == "All")
                    {
                        ShowGrid();
                    }
                    else
                    {
                        string DealerName = DropDownList1.SelectedValue.ToString();
                        string Dname = string.Empty;
                        string Did = string.Empty;

                        DealerId = "select distinct dealerid ,[NAME OF THE DEALER] as 'DealerName' from delhi_dealermaster a,hsrprecords as h where a.SNO=h.dealerid and a. [NAME OF THE DEALER]='" + DealerName + "' and  h.HSRP_StateID=2 ";
                        dtdealerid = Utils.GetDataTable(DealerId, CnnString);
                        for (int i = 0; dtdealerid.Rows.Count > i; i++)
                        {
                            Did = dtdealerid.Rows[i]["dealerid"].ToString();
                            Dname = dtdealerid.Rows[i]["DealerName"].ToString();
                           
                            if (Dname == DealerName)
                            {
                                Did = dtdealerid.Rows[i]["dealerid"].ToString();
                                string SQLString = "select hsrprecordid,vehicleregno,hsrp_rear_lasercode,hsrp_Front_lasercode,OrderStatus from hsrprecords where OrderStatus='Embossing Done' and DealerId='" + Did + "' and hsrp_StateID='" + DropDownListStateName.SelectedValue + "' and hsrprecord_CreationDate between '" + AuthorizationDate + "' and  '" + ToDate + "'  and StickerMandatory='Y'";

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
                                    Button1.Visible = false;
                                    btnSave.Visible = false;
                                    lblErrMsg.Text = "Record Not Found";
                                    GridView1.DataSource = null;
                                    GridView1.DataBind();

                                }
                            }

                        }
                    }

                }
                else
                {

                    lblErrMsg.Text = String.Empty;
                    lblErrMsg.Text = "Please Select File.";
                    return;
                }
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        private void ShowGrid()
        {


            if (dropDownListClient.SelectedItem.Text != "--Select RTO Name--")
            {

                string SQLString = "select hsrprecordid,vehicleregno,hsrp_rear_lasercode,hsrp_Front_lasercode,OrderStatus from hsrprecords where OrderStatus='Embossing Done' and hsrp_StateID='" + DropDownListStateName.SelectedValue + "' and hsrprecord_CreationDate between '" + AuthorizationDate + "' and  '" + ToDate + "' and StickerMandatory='Y'";

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
                            Button1.Visible = false;
                            btnSave.Visible = false;
                            lblErrMsg.Text = "Record Not Found";
                            GridView1.DataSource = null;
                            GridView1.DataBind();

                        }
                  

                

            }
            else
            {

                lblErrMsg.Text = String.Empty;
                lblErrMsg.Text = "Please Select File.";
                return;
            }
        }
        StringBuilder sb = new StringBuilder();
        CheckBox chk;
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



        protected void DropDownListStateName_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilldropDownListClient();
            dropDownListClient.Visible = true;
            labelClient.Visible = true;

        }



        



       
        DataTable dt = new DataTable();
        protected void dropdownDuplicateFIle_SelectedIndexChanged(object sender, EventArgs e)
        {
              if (HSRPStateID == "2")
                    {
                       // labelSelectType.Visible = true;
                        DropDownList1.Visible = true;
                        ShowGridDelhi();
                        
                    }
                    else
                    {
                        ShowGrid();
                    }
            
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            ShowGrid();

        }

        string PdfFolder;
        string filename;
        Document document;
        PdfPTable table;
        protected void btnSticker_Click(object sender, EventArgs e)
        {
            //int invoNo;
            //string tmpbillno = string.Empty;
            //string SQLString = "select max(invno) as invoiceNo from dealer_InvoiceExcise";
            //invoNo = Utils.getScalarCount(SQLString, CnnString);
            //invoNo = invoNo + 1;
            //string SQLString1 = "insert into dealer_InvoiceExcise(invno,invdate,dispatechedBy) values('" + invoNo + "',GETDATE(),'" + Session["UID"].ToString() + "')";
            //int k = Utils.ExecNonQuery(SQLString1, CnnString);
            HttpContext context = HttpContext.Current;
            String StringField = String.Empty;
            String StringAlert = String.Empty;
            filename = "Sticker" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".pdf";
            StringBuilder bb = new StringBuilder();

            document = new Document(PageSize.A4, 0, 0, 212, 0);
            document.SetPageSize(new iTextSharp.text.Rectangle(500, 400));
            document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
            PdfFolder = ConfigurationManager.AppSettings["PdfFolder"].ToString() + filename;

            PdfWriter.GetInstance(document, new FileStream(PdfFolder, FileMode.Create));
            document.Open();

            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                //  string lblVehicleRegNo,FLasercode,RearLaserCode;
                // tmpbillno = Session["UID"].ToString()+ System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() ;
                chk = GridView1.Rows[i].Cells[0].FindControl("CHKSelect") as CheckBox;
                if (chk.Checked == true)
                {
                    Label lblVehicleRegNo = GridView1.Rows[i].Cells[1].FindControl("lblVehicleRegNo") as Label;
                    TextBox RearLaserCode = GridView1.Rows[i].Cells[2].FindControl("txtFLaserCode") as TextBox;
                    TextBox FLasercode = GridView1.Rows[i].Cells[3].FindControl("txtRLaserCode") as TextBox;
                    Label id = GridView1.Rows[i].Cells[4].FindControl("id") as Label;
                    Label OrderStatus = GridView1.Rows[i].Cells[4].FindControl("lblOrderStatus") as Label;

                    if (OrderStatus.Text == "New Order")
                    {
                        Page.ClientScript.RegisterStartupScript(typeof(Page), "alert", "<script language=JavaScript>alert('Laser code is not assign');</script>");

                    }
                    else
                    {


                        SQLString = " select a.HSRPRecordID,a.vehicleRegNo, a.HSRP_Sticker_LaserCode, a.StickerMandatory, a.VehicleRegNo,a.EngineNo,a.ChassisNo,a.HSRP_Front_LaserCode,a.HSRP_Rear_LaserCode,b.RTOLocationname ,b.RTOLocationCode , HSRPState.HSRPStateName ,HSRPState.statetext from HSRPRecords a inner join rtolocation  as b on a.RTOLocationID =b.RTOLocationID inner join HSRPState  on HSRPState.HSRP_StateID= b.HSRP_StateID where  HSRPRecordID='" + id.Text + "' and  a.HSRP_Front_LaserCode is not null and a.HSRP_Front_LaserCode!=''";
                        DataTable ds = Utils.GetDataTable(SQLString, CnnString);
                        if (ds.Rows.Count > 0)
                        {
                            string Stricker = ds.Rows[0]["StickerMandatory"].ToString();
                            if (Stricker == "Y")
                            {



                                //Opens the document:


                                table = new PdfPTable(1);

                                table.TotalWidth = 300f;

                                StringBuilder sbtrnasportname = new StringBuilder();
                                trnasportname = "TRANSPORT DEPARTMENT";


                                PdfPCell cell6 = new PdfPCell(new Phrase(trnasportname, new iTextSharp.text.Font(basefont, 16f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                                // cell.Colspan = 4;
                                cell6.PaddingTop = 8f;
                                cell6.BorderColor = BaseColor.WHITE;                               
                                cell6.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                                table.AddCell(cell6);
                                StringBuilder sb = new StringBuilder();
                                StringBuilder sb1 = new StringBuilder();


                                string statename = ds.Rows[0]["statetext"].ToString().ToUpper();
                                string HSRPStateName = ds.Rows[0]["HSRPStateName"].ToString().ToUpper();
                                if (HSRPStateName == "HIMACHAL PRADESH" || HSRPStateName == "MADHYA PRADESH" || HSRPStateName == "ANDHRA PRADESH" || HSRPStateName == "UTTRAKHAND")
                                {
                                    PdfPCell cell = new PdfPCell(new Phrase(statename + " " + HSRPStateName, new iTextSharp.text.Font(basefont, 13f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                                    // cell.Colspan = 4;
                                    cell.PaddingTop = 8f;
                                    cell.BorderColor = BaseColor.WHITE;
                                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                                    table.AddCell(cell);
                                }
                                else if (HSRPStateName == "TELANGANA")
                                {
                                    PdfPCell cell = new PdfPCell(new Phrase(statename + " " + HSRPStateName, new iTextSharp.text.Font(basefont, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                                    // cell.Colspan = 4;
                                    cell.PaddingTop = 8f;
                                    cell.BorderColor = BaseColor.WHITE;
                                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                                    table.AddCell(cell);
                                }
                                else if (HSRPStateName == "HARYANA")
                                {
                                    PdfPCell cell = new PdfPCell(new Phrase(statename + " " + HSRPStateName, new iTextSharp.text.Font(basefont, 16f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                                    // cell.Colspan = 4;
                                    cell.PaddingTop = 8f;
                                    cell.BorderColor = BaseColor.WHITE;
                                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                                    table.AddCell(cell);
                                }
                                else
                                {

                                    PdfPCell cell = new PdfPCell(new Phrase(statename + " " + HSRPStateName, new iTextSharp.text.Font(basefont, 20f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                                    // cell.Colspan = 4;
                                    cell.PaddingTop = 8f;
                                    cell.BorderColor = BaseColor.WHITE;
                                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                                    table.AddCell(cell);
                                }


                                StringBuilder sbVehicleRegNo = new StringBuilder();

                                string VehicleRegNo = ds.Rows[0]["VehicleRegNo"].ToString().ToUpper();



                                PdfPCell cell2 = new PdfPCell(new Phrase(VehicleRegNo, new iTextSharp.text.Font(basefont, 30f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                                // cell.Colspan = 4;
                                cell2.PaddingTop = 8f;
                                cell2.BorderColor = BaseColor.WHITE;
                                cell2.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                                table.AddCell(cell2);
                                StringBuilder sbHSRP_Front_LaserCode = new StringBuilder();
                                StringBuilder sbHSRP_Rear_LaserCode = new StringBuilder();



                                string HSRP_Front_LaserCode = ds.Rows[0]["HSRP_Front_LaserCode"].ToString().ToUpper();
                                string HSRP_Rear_LaserCode = ds.Rows[0]["HSRP_Rear_LaserCode"].ToString().ToUpper();
                                PdfPCell cell3 = new PdfPCell(new Phrase("" + HSRP_Front_LaserCode + " - " + HSRP_Rear_LaserCode, new iTextSharp.text.Font(basefont, 13f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                                // cell.Colspan = 4;
                                cell3.PaddingTop = 8f;
                                cell3.PaddingLeft = 195f;//193f
                                cell3.BorderColor = BaseColor.WHITE;
                                cell3.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                table.AddCell(cell3);

                                StringBuilder sbEngineNo = new StringBuilder();
                                string EngineNo = "ENGINE NO - " + ds.Rows[0]["EngineNo"].ToString().ToUpper();


                                PdfPCell cell4 = new PdfPCell(new Phrase(EngineNo, new iTextSharp.text.Font(basefont, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                                // cell.Colspan = 4;
                                cell4.PaddingTop = 8f;
                                cell4.PaddingLeft = 193f;
                                cell4.BorderColor = BaseColor.WHITE;
                                cell4.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                table.AddCell(cell4);

                                StringBuilder sbChassisNo = new StringBuilder();
                                string ChassisNo = "CHASIS NO - " + ds.Rows[0]["ChassisNo"].ToString().ToUpper();
                                PdfPCell cell5 = new PdfPCell(new Phrase(ChassisNo, new iTextSharp.text.Font(basefont, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                                // cell.Colspan = 4;
                                cell5.PaddingTop = 8f;
                                cell5.PaddingLeft = 193f;
                                cell5.BorderColor = BaseColor.WHITE;
                                cell5.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                table.AddCell(cell5);

                                document.Add(table);

                                document.NewPage();



                                 SAVEStickerLog(ds.Rows[0]["vehicleRegNo"].ToString().ToUpper().Trim(), ds.Rows[0]["HSRPRecordID"].ToString());



                            }
                            else
                            {
                                Page.ClientScript.RegisterStartupScript(typeof(Page), "alert", "<script language=JavaScript>alert('Sticker Not Allow !!');</script>");
                            }
                        }
                    }

                }
                // Label1.Text = labtext;

            }

            document.Close();
            context.Response.ContentType = "Application/pdf";
            context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
            context.Response.WriteFile(PdfFolder);
            context.Response.End();




        }

        public void SAVEStickerLog(string vehicleRegNo, string HSRPRecordID)
        {
            SQLString = "insert into stickerLog (vehicleRegNo, hsrprecordID, userID) values ('" + vehicleRegNo + "','" + HSRPRecordID + "','" + Session["UID"] + "')";
            Utils.ExecNonQuery(SQLString, CnnString);

        }

        BaseFont basefont1;
        string fontpath;
        DataTable ds = new DataTable();
        int i = 0;
        //public void mirrersticker()
        //{
           

        //    }


        protected void Button1_Click(object sender, System.EventArgs e)
        {
            HttpContext context = HttpContext.Current;
            String StringField = String.Empty;
            String StringAlert = String.Empty;
            filename = "Sticker" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".pdf";
            StringBuilder bb = new StringBuilder();

            document = new Document(PageSize.A4, 0, 0, 212, 0);
            document.SetPageSize(new iTextSharp.text.Rectangle(500, 400));
            document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
            PdfFolder = ConfigurationManager.AppSettings["PdfFolder"].ToString() + filename;

            PdfWriter.GetInstance(document, new FileStream(PdfFolder, FileMode.Create));
            document.Open();






            //document = new Document(PageSize.A4, 0, 0, 212, 0);
            //document.SetPageSize(new iTextSharp.text.Rectangle(500, 400));
            //document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());

            //document.Open();

            //document.SetPageSize(new iTextSharp.text.Rectangle(400, 300));
            //document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());




            //BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
            //string PdfFolder = ConfigurationManager.AppSettings["PdfFolder"].ToString() + filename;
            //PdfWriter.GetInstance(document, new FileStream(PdfFolder, FileMode.Create));



            //Opens the document:
            document.Open();

            //Adds content to the document:
            // document.Add(new Paragraph("Ignition Log Report"));
            //PdfPTable table = new PdfPTable(1);
            ////actual width of table in points
            //table.TotalWidth = 300f;

            //iTextSharp.text.Font ft = new iTextSharp.text.Font();
            //FontFactory.Register(@"../bin/PRSANSR.TTF", "PRMirror");
            //ft = FontFactory.GetFont("PRMirror");


          //  fontpath = Environment.GetEnvironmentVariable("http://localhost:51047") + "C:\\Users\\user\\Desktop\\rosmerta\\HSRP(28Oct)\\HSRP(28Oct)\\HSRP(28Oct)\\HSRP\\bin\\PRSANSR.TTF";

            //fontpath = ConfigurationManager.AppSettings["DataFolder"].ToString() + "PRSANSR.TTF";
            fontpath = ConfigurationManager.AppSettings["DataFolder"].ToString();
            basefont = BaseFont.CreateFont(fontpath, BaseFont.IDENTITY_H, true);

            for (int j = 0; j < GridView1.Rows.Count; j++)
            {
                //  string lblVehicleRegNo,FLasercode,RearLaserCode;
                // tmpbillno = Session["UID"].ToString()+ System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() ;
                chk = GridView1.Rows[j].Cells[0].FindControl("CHKSelect") as CheckBox;
                if (chk.Checked == true)
                {
                    Label lblVehicleRegNo = GridView1.Rows[j].Cells[1].FindControl("lblVehicleRegNo") as Label;
                    TextBox RearLaserCode = GridView1.Rows[j].Cells[2].FindControl("txtFLaserCode") as TextBox;
                    TextBox FLasercode = GridView1.Rows[j].Cells[3].FindControl("txtRLaserCode") as TextBox;
                    Label id = GridView1.Rows[j].Cells[4].FindControl("id") as Label;
                    Label OrderStatus = GridView1.Rows[j].Cells[4].FindControl("lblOrderStatus") as Label;

                    SQLString = " select a.HSRPRecordID,a.vehicleRegNo, a.HSRP_Sticker_LaserCode, a.StickerMandatory, a.VehicleRegNo,a.EngineNo,a.ChassisNo,a.HSRP_Front_LaserCode,a.HSRP_Rear_LaserCode,b.RTOLocationname ,b.RTOLocationCode , HSRPState.HSRPStateName ,HSRPState.statetext from HSRPRecords a inner join rtolocation  as b on a.RTOLocationID =b.RTOLocationID inner join HSRPState  on HSRPState.HSRP_StateID= b.HSRP_StateID where  HSRPRecordID='" + id.Text + "' and  a.HSRP_Front_LaserCode is not null and a.HSRP_Front_LaserCode!=''";
                    DataTable ds = Utils.GetDataTable(SQLString, CnnString);

                    table = new PdfPTable(1);

                    table.TotalWidth = 300f;


                    if (ds.Rows.Count > 0)
                    {
                        string Stricker = ds.Rows[0]["StickerMandatory"].ToString();
                        if (Stricker == "Y")
                        {

                            StringBuilder sbtrnasportname = new StringBuilder();
                            trnasportname = "TRANSPORT DEPARTMENT";

                            for (i = trnasportname.Length - 1; i >= 0; i--)
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

                            for (i = statename.Length - 1; i >= 0; i--)
                            {
                                sb.Append(statename[i].ToString());
                            }

                            string HSRPStateName = ds.Rows[0]["HSRPStateName"].ToString().ToUpper();

                            for (i = HSRPStateName.Length - 1; i >= 0; i--)
                            {
                                sb1.Append(HSRPStateName[i].ToString());
                            }
                            if (HSRPStateName == "HIMACHAL PRADESH" || HSRPStateName == "MADHYA PRADESH" || HSRPStateName == "ANDHRA PRADESH" || HSRPStateName == "UTTRAKHAND" ||HSRPStateName=="TELANGANA")
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

                            for (i = VehicleRegNo.Length - 1; i >= 0; i--)
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
                            string HSRP_Front_LaserCode = ds.Rows[0]["HSRP_Front_LaserCode"].ToString().ToUpper();

                            for (i = HSRP_Front_LaserCode.Length - 1; i >= 0; i--)
                            {
                                sbHSRP_Front_LaserCode.Append(HSRP_Front_LaserCode[i].ToString());
                            }

                            string HSRP_Rear_LaserCode = ds.Rows[0]["HSRP_Rear_LaserCode"].ToString().ToUpper();

                            for (i = HSRP_Rear_LaserCode.Length - 1; i >= 0; i--)
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
                            string EngineNo = "ENGINE NO - " + ds.Rows[0]["EngineNo"].ToString().ToUpper();

                            for (i = EngineNo.Length - 1; i >= 0; i--)
                            {
                                sbEngineNo.Append(EngineNo[i].ToString());
                            }


                            PdfPCell cell4 = new PdfPCell(new Phrase(sbEngineNo.ToString(), new iTextSharp.text.Font(basefont, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                            // cell.Colspan = 4;
                            cell4.PaddingTop = 8f;
                            cell4.PaddingRight = 245f;
                            cell4.BorderColor = BaseColor.WHITE;
                            cell4.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell4);

                            StringBuilder sbChassisNo = new StringBuilder();
                            string ChassisNo = "CHASSIS NO - " + ds.Rows[0]["ChassisNo"].ToString().ToUpper();

                            for (i = ChassisNo.Length - 1; i >= 0; i--)
                            {
                                sbChassisNo.Append(ChassisNo[i].ToString());
                            }

                            PdfPCell cell5 = new PdfPCell(new Phrase(sbChassisNo.ToString(), new iTextSharp.text.Font(basefont, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                            // cell.Colspan = 4;
                            cell5.PaddingTop = 8f;
                            cell5.PaddingRight = 245f;
                            cell5.BorderColor = BaseColor.WHITE;
                            cell5.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell5);

                            document.Add(table);

                            document.NewPage();


                            //  SAVEStickerLog(ds.Rows[0]["vehicleRegNo"].ToString().ToUpper().Trim(), ds.Rows[0]["HSRPRecordID"].ToString());

                            //    HttpContext context = HttpContext.Current;
                            i = 0;
                            ds.Clear();
                        }
                        else
                        {
                            Page.ClientScript.RegisterStartupScript(typeof(Page), "alert", "<script language=JavaScript>alert('Sticker Not Allow !!');</script>");
                        }
                    }
                }

            }
            // Label1.Text = labtext;
            document.Close();
            context.Response.ContentType = "Application/pdf";
            context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
            context.Response.WriteFile(PdfFolder);
            context.Response.End();
        }

        protected void ddlBothDealerHHT_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowGridDelhi();
        }

        protected void btntest_Click(object sender, EventArgs e)
        {
            string SQLString = "select hsrprecordid,vehicleregno,hsrp_rear_lasercode,hsrp_Front_lasercode,OrderStatus from hsrprecords where OrderStatus='Embossing Done' and hsrp_StateID=9 and vehicleregno in ('ap07ch1323',	'ap07mc0848',	'ap07cp6901',	'ap07cp6866',	'ap07cp5689',	'ap07cp6368',	'ap07cp5400',	'ap07cp5337',	'ap07cp5795')";

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
                Button1.Visible = false;
                btnSave.Visible = false;
                lblErrMsg.Text = "Record Not Found";
                GridView1.DataSource = null;
                GridView1.DataBind();

            }
        }

            
        }

     
    }

