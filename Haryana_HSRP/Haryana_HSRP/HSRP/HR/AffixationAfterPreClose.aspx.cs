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
using System.Data.SqlClient;
using System.IO;
using CarlosAg.ExcelXmlWriter;
using System.Web;


namespace HSRP.HR
{
    public partial class AffixationAfterPreClose : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        string UserType = string.Empty;
        string HSRPStateID = string.Empty;
        string RTOLocationID = string.Empty;
        int intHSRPStateID;


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
                        }
                        else
                        {
                            hiddenUserType.Value = "1";
                            labelOrganization.Enabled = false;
                            DropDownListStateName.Enabled = false;
                            labelClient.Enabled = false;
                            FilldropDownListOrganization();
                            FilldropDownListClient();

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

                SQLString = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, (a.RTOLocationCode+', ') as RTOCode, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID where UserRTOLocationMapping.UserID='" + UserID + "' order by RTOLocationName";
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
                    //  lblRTOCode.Text = RTOCode.Remove(RTOCode.LastIndexOf(","));

                }
            }
        }

        #endregion

        private void ShowGrid()
        {
            //GridView1.DataSource = null;
            // GridView1.DataBind();
            LblErrorMessage.Text = "";
            LblMessage.Text = "";
            string SQLString = string.Empty;
            string flag = string.Empty;
            flag = "0";
            string type = "1";
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
            string ToDate = From1 + " 23:59:59";

            if (HSRPStateID == "9" || HSRPStateID == "11")
            {
                SQLString = "select hsrprecordid,vehicleregno,vehicletype,hsrp_rear_lasercode,hsrp_Front_lasercode,HSRPRecord_AuthorizationNo,orderstatus from hsrprecords  where hsrp_stateid ='" + HSRPStateID + "'  and Rtolocationid='" + dropDownListClient.SelectedValue.ToString() + "'  and PreCloserDateTime between '" + AuthorizationDate + "' and '" + ToDate + "'  and orderstatus in ('PreClosed','closed')   order by VehicleRegNo asc";
                dt = Utils.GetDataTable(SQLString, CnnString);
            }
            else
            {
                SQLString = "select hsrprecordid,vehicleregno,vehicletype,hsrp_rear_lasercode,hsrp_Front_lasercode,HSRPRecord_AuthorizationNo,orderstatus from hsrprecords where hsrp_stateid ='" + HSRPStateID + "' and Rtolocationid='" + dropDownListClient.SelectedValue.ToString() + "' and PreCloserDateTime between '" + AuthorizationDate + "' and '" + ToDate + "' and orderstatus in ('PreClosed','closed')  order by VehicleRegNo";
                dt = Utils.GetDataTable(SQLString, CnnString);
            }
            if (dt.Rows.Count > 0)
            {
                GridView1.DataSource = dt;
                GridView1.DataBind();
                btnSave.Visible = true;
                foreach (GridViewRow gvr in GridView1.Rows)
                {
                    CheckBox chkboxk = (CheckBox)gvr.Cells[0].FindControl("CHKSelect");
                    TextBox txtOS = (TextBox)gvr.Cells[6].FindControl("txtOS");

                    if (txtOS.Text.ToString() == "Closed")
                    {
                        chkboxk.Enabled = false;
                        //btnSave.Enabled = false;                           
                    }
                    else if (txtOS.Text.ToString() == "PreClosed")
                    {
                        chkboxk.Enabled = true;
                        flag = "1";
                        //btnSave.Enabled = true; 
                    }
                }

                if (flag == "0")
                {
                    btnSave.Enabled = false;
                }
                //btnSave.Visible = true;
                //btnSave.Enabled = true;

            }
            else
            {
                btnSave.Visible = false;
                lblErrMsg.Text = "Record Not Found";
                GridView1.DataSource = null;
                GridView1.DataBind();

            }

        }
        StringBuilder sb = new StringBuilder();
        StringBuilder sb1 = new StringBuilder();
        StringBuilder sby = new StringBuilder();
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
            labelOrderStatus.Visible = false;
            dropdownDuplicateFIle.Visible = false;

            //GridView1.PageIndex = 0;
            string type = "1";
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
            string ToDate = From1 + " 23:59:59";

            if (HSRPStateID == "9" || HSRPStateID == "11")
            {

                ShowGrid();
                btnSave.Enabled = true;

            }

            else
            {

                ShowGrid();
            }

        }
        DataTable dt = new DataTable();
        protected void dropdownDuplicateFIle_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ShowGrid();
        }
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            ShowGrid();
            btnSave.Enabled = true;

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                LblErrorMessage.Text = "";
                LblMessage.Text = "";
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    //  string lblVehicleRegNo,FLasercode,RearLaserCode;
                    // tmpbillno = Session["UID"].ToString()+ System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() ;
                    chk = GridView1.Rows[i].Cells[0].FindControl("CHKSelect") as CheckBox;
                    Label lblVehicleRegNo = GridView1.Rows[i].Cells[1].FindControl("lblVehicleRegNo") as Label;
                    Label lblvehicletype = GridView1.Rows[i].Cells[2].FindControl("lblvehicletype") as Label;
                    TextBox RearLaserCode = GridView1.Rows[i].Cells[3].FindControl("txtFLaserCode") as TextBox;
                    TextBox FLasercode = GridView1.Rows[i].Cells[4].FindControl("txtRLaserCode") as TextBox;
                    Label id = GridView1.Rows[i].Cells[5].FindControl("id") as Label;
                    if (chk.Checked == true)
                    {

                        // TextBox orderstatus = GridView1.Rows[i].Cells[3].FindControl("txtOS") as TextBox;
                        //  labtext = GridView1.Rows[i].Cells[0].Text;
                        // System.DateTime.Now.AddDays(-2).ToString("dd/MMM/yyyy");
                        sb.Append("update hsrprecords set orderstatus='Closed',[ordercloseddate]= getdate(), closeduserid='" + strUserID + "' where HSRPRecordID='" + id.Text + "' and orderstatus='PreClosed' " + Environment.NewLine);

                        sby.Append("update preclosehsrprecords set processstatus='Y' where HSRPRecordID='" + id.Text + "' and processstatus='N' ");
                    }
                    else
                    {
                        //btnSave.Visible = false;
                        // sb.Append("update hsrprecords set orderstatus='Embossing Done',[ordercloseddate]=null , closeduserid='" + strUserID + "' where HSRPRecordID='" + id.Text + "' and orderstatus='PreClosed' and vehicleregno='" + lblVehicleRegNo.Text + "'" + Environment.NewLine);
                    }
                }
                int j = Utils.ExecNonQuery(sb.ToString(), CnnString);
                int k = Utils.ExecNonQuery(sby.ToString(), CnnString);
                //Synct to AP Server 
                if (DropDownListStateName.SelectedValue.ToString() == "9")
                {

                    HSRP.APWebrefrence.HSRPAuthorizationService objAPService = new HSRP.APWebrefrence.HSRPAuthorizationService();
                    for (int i = 0; i < GridView1.Rows.Count; i++)
                    {
                        chk = GridView1.Rows[i].Cells[0].FindControl("CHKSelect") as CheckBox;
                        if (chk.Checked == true)
                        {
                            Label lblVehicleRegNo = GridView1.Rows[i].Cells[1].FindControl("lblVehicleRegNo") as Label;
                            Label lblvehicletype = GridView1.Rows[i].Cells[2].FindControl("lblvehicletype") as Label;
                            TextBox RearLaserCode = GridView1.Rows[i].Cells[3].FindControl("txtFLaserCode") as TextBox;
                            TextBox FLasercode = GridView1.Rows[i].Cells[4].FindControl("txtRLaserCode") as TextBox;
                            Label id = GridView1.Rows[i].Cells[5].FindControl("id") as Label;
                            Label lblAuthNo = GridView1.Rows[i].Cells[6].FindControl("AuthNo") as Label;

                            string responsecodeAp = objAPService.UpdateHSRPAffixation(lblAuthNo.Text, DateTime.Now.ToString("dd/MM/yyyy"));
                            sb1.Append("update hsrprecords set APwebservicerespAff='" + responsecodeAp + "',APwebservicerespAffdate=Getdate() where HSRPRecordID='" + id.Text + "'" + Environment.NewLine);

                        }


                    }
                    Utils.ExecNonQuery(sb1.ToString(), CnnString);
                }

                if (DropDownListStateName.SelectedValue.ToString() == "11")
                {

                    HSRP.TGWebrefrence.HSRPAuthorizationService objTGService = new HSRP.TGWebrefrence.HSRPAuthorizationService();
                    for (int i = 0; i < GridView1.Rows.Count; i++)
                    {
                        chk = GridView1.Rows[i].Cells[0].FindControl("CHKSelect") as CheckBox;
                        if (chk.Checked == true)
                        {
                            Label lblVehicleRegNo = GridView1.Rows[i].Cells[1].FindControl("lblVehicleRegNo") as Label;
                            Label lblvehicletype = GridView1.Rows[i].Cells[2].FindControl("lblvehicletype") as Label;
                            TextBox RearLaserCode = GridView1.Rows[i].Cells[3].FindControl("txtFLaserCode") as TextBox;
                            TextBox FLasercode = GridView1.Rows[i].Cells[4].FindControl("txtRLaserCode") as TextBox;
                            Label id = GridView1.Rows[i].Cells[5].FindControl("id") as Label;
                            Label lblAuthNo = GridView1.Rows[i].Cells[6].FindControl("AuthNo") as Label;

                            // string responsecodeTG = objTGService.UpdateHSRPLaserCodes(lblAuthNo.Text, FLasercode.Text, RearLaserCode.Text, DateTime.Now.ToString("dd/MM/yyyy"));
                            string responsecodeTG = objTGService.UpdateHSRPAffixation(lblAuthNo.Text, DateTime.Now.ToString("dd/MM/yyyy"));
                            sb1.Append("update hsrprecords set APwebservicerespAff='" + responsecodeTG + "',APwebservicerespAffdate=Getdate() where HSRPRecordID='" + id.Text + "'" + Environment.NewLine);

                        }
                    }
                    Utils.ExecNonQuery(sb1.ToString(), CnnString);
                }
                if (j > 0)
                {

                    LblMessage.Text = "Updated Records Sucessfull";
                    //btnGO_Click(sender, e);
                    //btnSave.Enabled = false;
                }
                else
                {
                    LblErrorMessage.Text = "Records not updated successfully";
                    btnSave.Enabled = false;
                }

            }
            catch (Exception ex)
            {
                LblErrorMessage.Text = "Data Not Uploaded Contact Administration with screen shot Error :" + ex.Message.ToString();
            }
            ShowGrid();

        }


        protected void btnexport_Click(object sender, EventArgs e)
        {

            if (HSRPStateID == "9" || HSRPStateID == "11")
            {

                Export();

            }

            else
            {


            }


        }
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        private void Export()
        {
            try
            {
                LblErrorMessage.Text = "";
                LblMessage.Text = "";
                string SQLString = string.Empty;
                string flag = string.Empty;
                flag = "0";
                string type = "1";
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
                string ToDate = From1 + " 23:59:59";


                Workbook book = new Workbook();
                // Specify which Sheet should be opened and the size of window by default
                book.ExcelWorkbook.ActiveSheetIndex = 1;
                book.ExcelWorkbook.WindowTopX = 100;
                book.ExcelWorkbook.WindowTopY = 200;
                book.ExcelWorkbook.WindowHeight = 7000;
                book.ExcelWorkbook.WindowWidth = 8000;

                // Some optional properties of the Document
                book.Properties.Author = "HSRP";
                book.Properties.Title = "Report";
                book.Properties.Created = DateTime.Now;

                #region Fetch Data
                DataTable dt = new DataTable();

                // SqlCommand cmd = new SqlCommand("[USPPreclosedtoClosedReport]", con);
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.Add(new SqlParameter("@reportDate", HSRPAuthDate.SelectedDate));
                //cmd.Parameters.Add(new SqlParameter("@StateId", DropDownListStateName.SelectedValue));
                //cmd.Parameters.Add(new SqlParameter("@RtolocationId", dropDownListClient.SelectedValue));
                // cmd.Parameters.Add(new SqlParameter("@datefrom", OrderDate.SelectedDate));

                // cmd.Parameters.Add(new SqlParameter("@addrecordby", ddlBothDealerHHT.SelectedItem.Text));

                // String StringOrderDate = OrderDate.SelectedDate.ToString("yyyy/MM/dd") + " 00:00:00";
                // String StringAuthDate = HSRPAuthDate.SelectedDate.ToString("yyyy/MM/dd") + " 23:59:59";

                SQLString = "select ROW_NUMBER() Over (Order by PreCloserDateTime) As [S.N.],vehicleregno,Vehicletype,OrderStatus,PreCloserDateTime,OrderClosedDate from hsrprecords  where hsrp_stateid ='" + HSRPStateID + "'  and Rtolocationid='" + dropDownListClient.SelectedValue.ToString() + "'  and PreCloserDateTime between '" + AuthorizationDate + "' and '" + ToDate + "'  and orderstatus in ('closed')   order by PreCloserDateTime asc";


                SqlCommand cmd = new SqlCommand(SQLString, con);
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                //cmd.CommandTimeout = 0;
                //SqlDataAdapter da = new SqlDataAdapter(cmd);F

                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {


                #endregion


                    // Add some styles to the Workbook

                    #region Styles

                    WorksheetStyle style = book.Styles.Add("HeaderStyle");

                    style.Font.FontName = "Tahoma";
                    style.Font.Size = 9;
                    style.Font.Bold = false;
                    style.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                    style.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                    style.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                    style.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                    style.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


                    WorksheetStyle style8 = book.Styles.Add("HeaderStyle8");
                    style8.Font.FontName = "Tahoma";
                    style8.Font.Size = 10;
                    style8.Font.Bold = true;
                    style8.Alignment.Horizontal = StyleHorizontalAlignment.Right;
                    style8.Interior.Color = "#D4CDCD";
                    style8.Interior.Pattern = StyleInteriorPattern.Solid;

                    WorksheetStyle style5 = book.Styles.Add("HeaderStyle5");
                    style5.Font.FontName = "Tahoma";
                    style5.Font.Size = 10;
                    style5.Font.Bold = false;
                    style5.Alignment.Horizontal = StyleHorizontalAlignment.Right;
                    style5.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                    style5.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                    style5.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                    style5.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


                    WorksheetStyle style6 = book.Styles.Add("HeaderStyle6");
                    style6.Font.FontName = "Tahoma";
                    style6.Font.Size = 10;
                    style6.Font.Bold = true;
                    style6.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                    style6.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                    style6.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                    style6.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                    style6.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);

                    WorksheetStyle styleHeader = book.Styles.Add("HeaderStyleHeader");
                    styleHeader.Font.FontName = "Tahoma";
                    styleHeader.Interior.Color = "Red";
                    styleHeader.Font.Size = 10;
                    styleHeader.Font.Bold = true;
                    styleHeader.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                    styleHeader.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                    styleHeader.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                    styleHeader.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                    styleHeader.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


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
                    WorksheetStyle style9 = book.Styles.Add("HeaderStyle9");
                    style9.Font.FontName = "Tahoma";
                    style9.Font.Size = 10;
                    style9.Font.Bold = true;
                    style9.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                    style9.Interior.Color = "#FCF6AE";
                    style9.Interior.Pattern = StyleInteriorPattern.Solid;
                    #endregion

                    Worksheet sheet = book.Worksheets.Add("Report");

                    #region UpperPart of Excel
                    AddColumnToSheet(sheet, 100, dt.Columns.Count);
                    int iIndex = 3;
                    WorksheetRow row = sheet.Table.Rows.Add();
                    row.Index = iIndex++;
                    //  row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                    row.Cells.Add(new WorksheetCell("Report :", "HeaderStyle3"));

                    row.Cells.Add(new WorksheetCell("PreClosed to Closed Report", "HeaderStyle3"));

                    row = sheet.Table.Rows.Add();
                    row.Index = iIndex++;

                    AddNewCell(row, "RTOLocation:", "HeaderStyle2", 1);
                    AddNewCell(row, dropDownListClient.SelectedItem.Text, "HeaderStyle2", 1);
                    row = sheet.Table.Rows.Add();
                    row.Index = iIndex++;

                    DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());

                    AddNewCell(row, "Report Duration:", "HeaderStyle2", 1);
                    AddNewCell(row, HSRPAuthDate.SelectedDate.ToString("dd/MMM/yyyy"), "HeaderStyle2", 1);
                    row = sheet.Table.Rows.Add();

                    row.Index = iIndex++;


                    row.Index = iIndex++;
                    #endregion

                    #region Column Creation and Assign Data
                    string RTOColName = string.Empty;
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        AddNewCell(row, dt.Columns[i].ColumnName.ToString().Remove(dt.Columns[i].ColumnName.ToString().Length - 2, 0), "HeaderStyleHeader", 1);
                    }
                    row = sheet.Table.Rows.Add();
                    row.Index = iIndex++;


                    for (int j = 0; j < dt.Rows.Count; j++)
                    {

                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            AddNewCell(row, dt.Rows[j][i].ToString(), "HeaderStyle6", 1);

                        }
                        row = sheet.Table.Rows.Add();

                    }
                    row = sheet.Table.Rows.Add();


                    #endregion
                    HttpContext context = HttpContext.Current;
                    context.Response.Clear();
                    // Save the file and open it
                    book.Save(Response.OutputStream);
                    lblErrMsg.Text = "";
                    //context.Response.ContentType = "text/csv";
                    context.Response.ContentType = "application/vnd.ms-excel";
                    string filename = "Report" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";

                    context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                    context.Response.End();
                }
                else
                {
                    lblErrMsg.Text = "Record Not Found";
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }

        private static void AddNewCell(WorksheetRow row, string strText, string strStyle, int iCnt)
        {
            for (int i = 0; i < iCnt; i++)
                row.Cells.Add(new WorksheetCell(strText, strStyle));
        }

        private static void AddColumnToSheet(Worksheet sheet, int iWidth, int iCnt)
        {
            for (int i = 0; i < iCnt; i++)
                sheet.Table.Columns.Add(new WorksheetColumn(iWidth));
        }

        //For PDF Generate 

        string strPath = string.Empty;
        private string SetFolder(string strRTO, string strState, string strFile)
        {
            string DateFolder = System.DateTime.Now.Day.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Year.ToString();
            strPath = "D:\\TenderReports";
            if (!Directory.Exists(strPath))
            {
                CreateFolder(DateFolder, strState, strPath);
                Directory.CreateDirectory(strPath + "\\" + strState);
                Directory.CreateDirectory(strPath + "\\" + strState + "\\" + DateFolder);
            }
            else
            {
                if (!Directory.Exists(strPath + "\\" + strState))
                {

                    Directory.CreateDirectory(strPath + "\\" + strState);
                    Directory.CreateDirectory(strPath + "\\" + strState + "\\" + DateFolder);

                }
                else
                {
                    if (!Directory.Exists(strPath + "\\" + strState + "\\" + DateFolder))
                    {

                        Directory.CreateDirectory(strPath + "\\" + strState + "\\" + DateFolder);

                    }
                    else
                    {
                        var files = Directory.GetFiles(strPath + "\\" + strState + "\\" + DateFolder, "*.*", SearchOption.AllDirectories);



                        //foreach (string file in files)
                        //{
                        //    if (file.StartsWith(strPath + "\\" + strState + "\\" + DateFolder + "\\" + strFile))
                        //    {
                        //        File.Delete(file);
                        //    }
                        //}
                    }
                }


            }
            return strPath = strPath + "\\" + strState + "\\" + DateFolder;
        }

        private static void CreateFolder(string strRTO, string strState, string strRTOLocFolderPath)
        {
            Directory.CreateDirectory(strRTOLocFolderPath);
            Directory.CreateDirectory(strRTOLocFolderPath + "\\" + strState);
            Directory.CreateDirectory(strRTOLocFolderPath + "\\" + strState + "\\" + strRTO);
        }


        #region Pdf Function
        private static void PDFRows(BaseFont bfTimes, PdfPTable table1, int iColSpan, String strText, int iFontsize, int ialignMent, string strRowType, int iBorderWidthLeft, int iBorderWidthRight, int iBorderWidthTop, int iBorderWidthBottom, int optionalHeight = 0, int optionalWidth = 0)
        {
            PdfPCell cell;
            if (strRowType == "B")
            {
                cell = new PdfPCell(new iTextSharp.text.Phrase(strText, new iTextSharp.text.Font(bfTimes, iFontsize, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
            }
            else
            {
                cell = new PdfPCell(new iTextSharp.text.Phrase(strText, new iTextSharp.text.Font(bfTimes, iFontsize, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
            }
            cell.Colspan = iColSpan;
            cell.BorderColor = iTextSharp.text.BaseColor.BLACK;
            cell.BorderWidthLeft = iBorderWidthLeft;
            cell.BorderWidthRight = iBorderWidthRight;
            cell.BorderWidthTop = iBorderWidthTop;
            cell.BorderWidthBottom = iBorderWidthBottom;
            cell.NoWrap = false;
            cell.HorizontalAlignment = ialignMent; //0=Left, 1=Centre, 2=Right
            table1.AddCell(cell);
        }
        #endregion

        private static void GenerateCell(PdfPTable table, int iSpan, int iLeftWidth, int iRightWidth, int iTopWidth, int iBottomWidth, int iAllign, int iFont, string strText, int iRowHeight, int iRowWidth)
        {
            PdfPCell newCellPDF = null;
            BaseFont bfTimes1 = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
            if (iFont.Equals(0))
            {
                newCellPDF = new PdfPCell(new Phrase(strText, new iTextSharp.text.Font(bfTimes1, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
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
            newCellPDF.NoWrap = false;
            if (!iRowHeight.Equals(0))
            {
                newCellPDF.FixedHeight = iRowHeight;
            }
            if (!iRowWidth.Equals(0))
            {
            }
            table.AddCell(newCellPDF);
        }





        private void ExportToPDFAll()
        {
            DataTable dtrto = GetRtoLocation();
            for (int iRowNo = 0; iRowNo < dtrto.Rows.Count; iRowNo++)
            {
                string RTOName = dtrto.Rows[iRowNo]["rtolocationname"].ToString();
                string RTOCode = dtrto.Rows[iRowNo]["RTOLocationid"].ToString();
                string FromDate = OrderDate.SelectedDate.ToString("yyyy/MM/dd") + " 00:00:00"; // Convert.ToDateTime();
                string ToDate = HSRPAuthDate.SelectedDate.ToString("yyyy/MM/dd") + " 23:59:59";

                // HttpContext context = HttpContext.Current;
                string filename = "Collection" + RTOName + "_" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".pdf";
                string SQLString = String.Empty;
                Document document = new Document();
                SetFolder(RTOName, dropDownListClient.SelectedItem.ToString(), "Collection");

                string PdfFolder = strPath + "\\" + filename;
                BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);


                // string PdfFolder = ConfigurationManager.AppSettings["PdfFolder"].ToString() + filename;
                PdfWriter.GetInstance(document, new FileStream(PdfFolder, FileMode.Create));
                document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
                document.Open();
                PdfPTable table = new PdfPTable(6);
                var colWidthPercentages = new[] { 40f, 60f, 50f, 50f, 50f, 50f };
                table.SetWidths(colWidthPercentages);
                //string sqlquery="select distrelation from rtolocation where rtolocationid='"+
                string SqlQuery = string.Empty;
                String StringOrderDate = OrderDate.SelectedDate.ToString("yyyy/MM/dd") + " 00:00:00";
                String StringAuthDate = HSRPAuthDate.SelectedDate.ToString("yyyy/MM/dd") + " 23:59:59";


                string sqlquery = "select vehicleregno,Vehicletype,OrderStatus,PreCloserDateTime,OrderClosedDate from hsrprecords  where hsrp_stateid ='" + HSRPStateID + "'  and Rtolocationid='" + dropDownListClient.SelectedValue.ToString() + "'  and PreCloserDateTime between '" + StringOrderDate + "' and '" + StringAuthDate + "'  and orderstatus in ('closed')   order by PreCloserDateTime asc";
                DataTable dts = Utils.GetDataTable(sqlquery, CnnString);


                table.TotalWidth = 750f;
                table.LockedWidth = true;
                //  GenerateCell(table, 10, 0, 0, 0, 0, 1, 1, "", 50, 0);

                // GenerateCell(table, 10, 0, 0, 0, 0, 1, 0, "SCHEDULE-C : Daily Report from Embossing Stations to Registering Authority", 20, 0);
                // GenerateCell(table, 2, 0, 0, 0, 0, 1, 0, "State Name : HARYANA", 20, 0);
                // GenerateCell(table, 2, 0, 0, 0, 0, 0, 0, "RTO Name:      " + RTOName, 15, 0);
                //  GenerateCell(table, 3, 0, 0, 0, 0, 1, 0, "Date Period  :" + OrderDate.SelectedDate.ToString("dd/MMM/yyyy") + "-" + HSRPAuthDate.SelectedDate.ToString("dd/MMM/yyyy"), 15, 0);
                // GenerateCell(table, 3, 0, 0, 0, 0, 1, 0, "Generated Date time: " + System.DateTime.Now.ToString("dd/MMM/yyyy"), 15, 0);
                GenerateCell(table, 6, 0, 0, 0, 0, 1, 0, "", 20, 0);
                GenerateCell(table, 1, 1, 1, 1, 1, 1, 0, "S.No:", 20, 0);
                GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Vehicle Reg No", 40, 0);
                GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Vehicle Type", 20, 0);
                GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "OrderStatus", 20, 0);
                GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "PreCloser DateTime", 20, 0);
                GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Order Closed Date", 20, 0);

                //GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Owner's Name", 20, 0);
                //GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Vechicle Type", 20, 0);

                //GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Vehicle Class", 40, 0);
                //GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Affixation Status", 20, 0);

                //GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Affixation Date", 20, 0);               
                //GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Amount", 20, 0);

                int SnoCounter = 0;


                //vehicleregno,Vehicletype,OrderStatus,PreCloserDateTime,OrderClosedDate

                #region Dynamic Rows
                for (int i = 0; i < dts.Rows.Count; i++)
                {
                    SnoCounter = SnoCounter + 1;
                    GenerateCell(table, 1, 1, 1, 0, 1, 1, 1, SnoCounter.ToString(), 20, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dts.Rows[i]["vehicleregno"].ToString(), 30, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dts.Rows[i]["Vehicletype"].ToString(), 20, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dts.Rows[i]["OrderStatus"].ToString(), 20, 0);

                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dts.Rows[i]["PreCloserDateTime"].ToString(), 20, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dts.Rows[i]["OrderClosedDate"].ToString(), 20, 0);

                    //GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dtrto.Rows[i]["OwnerName"].ToString(), 20, 0);
                    //GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dtrto.Rows[i]["VehicleType"].ToString(), 20, 0);

                    //GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dtrto.Rows[i]["VehicleClass"].ToString(), 20, 0);

                    //GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dtrto.Rows[i]["orderstatus"].ToString(), 20, 0);

                    //// GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dtRecord.Rows[i]["embdate"].ToString(), 20, 0);
                    //GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dtrto.Rows[i]["AffixDate"].ToString(), 20, 0);

                    ////GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dtRecord.Rows[i]["username"].ToString(), 20, 0);

                    //GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dtrto.Rows[i]["NetAmount"].ToString(), 20, 0);

                #endregion


                }
                document.Add(table);
                document.NewPage();
                document.Close();

            }


        }

        private static void StyleForTheFirstTime(Workbook book)
        {
            // Specify which Sheet should be opened and the size of window by default
            book.ExcelWorkbook.ActiveSheetIndex = 1;
            book.ExcelWorkbook.WindowTopX = 100;
            book.ExcelWorkbook.WindowTopY = 200;
            book.ExcelWorkbook.WindowHeight = 7000;
            book.ExcelWorkbook.WindowWidth = 8000;

            // Some optional properties of the Document
            book.Properties.Author = "HSRP";
            book.Properties.Title = "HSRP Affixation Report";
            book.Properties.Created = DateTime.Now;

            #region Style
            WorksheetStyle style = book.Styles.Add("HeaderStyle");
            style.Font.FontName = "Tahoma";
            style.Font.Size = 9;
            style.Font.Bold = false;
            style.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            style.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            style.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            style.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            style.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


            WorksheetStyle style8 = book.Styles.Add("HeaderStyle8");
            style8.Font.FontName = "Tahoma";
            style8.Font.Size = 10;
            style8.Font.Bold = true;
            style8.Alignment.Horizontal = StyleHorizontalAlignment.Right;
            style8.Interior.Color = "#D4CDCD";
            style8.Interior.Pattern = StyleInteriorPattern.Solid;

            WorksheetStyle style5 = book.Styles.Add("HeaderStyle5");
            style5.Font.FontName = "Tahoma";
            style5.Font.Size = 10;
            style5.Font.Bold = false;
            style5.Alignment.Horizontal = StyleHorizontalAlignment.Right;
            style5.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            style5.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            style5.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            style5.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);

            WorksheetStyle style6 = book.Styles.Add("HeaderStyle6");
            style6.Font.FontName = "Tahoma";
            style6.Font.Size = 10;
            style6.Font.Bold = true;
            style6.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            style6.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            style6.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            style6.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            style6.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);

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

            WorksheetStyle style9 = book.Styles.Add("HeaderStyle9");
            style9.Font.FontName = "Tahoma";
            style9.Font.Size = 10;
            style9.Font.Bold = true;
            style9.Alignment.Horizontal = StyleHorizontalAlignment.Right;
            style9.Interior.Color = "#FCF6AE";
            style9.Interior.Pattern = StyleInteriorPattern.Solid;
            #endregion
        }

        protected void btnpdf_Click(object sender, EventArgs e)
        {

            EpsionPrint();
            //try
            //{
            //    lblErrMsg.Text = "";
            //    String StringOrderDate = OrderDate.SelectedDate.ToString("yyyy/MM/dd") + " 00:00:00";
            //    String StringAuthDate = HSRPAuthDate.SelectedDate.ToString("yyyy/MM/dd") + " 23:59:59";
            //    //SQLString = "SELECT CONVERT(varchar(20), a.HsrpRecord_AuthorizationDate, 103) AS HsrpRecord_AuthorizationDate,convert(varchar,ordercloseddate,103) as ordercloseddate,  CONVERT(varchar(20), a.OrderDate, 103) AS OrderDate, a.HSRPRecordID,a.CashReceiptNo,a.InvoiceNo,CONVERT(varchar(20), InvoiceDateTime, 103) AS InvoiceDateTime, VehicleClass,CONVERT(numeric,round(a.RoundOff_NetAmount,0)) as NetAmount, a.HSRPRecord_AuthorizationNo, a.OwnerName, a.VehicleClass, a.VehicleType, a.VehicleRegNo, a.HSRP_Front_LaserCode, a.HSRP_Rear_LaserCode, a.StickerMandatory,  (select P.productCode from Product P Where P.ProductID =a.FrontPlateSize) as FrontPlateCode, (select P.productCode from Product P Where P.ProductID =a.rearPlateSize) as RearPlateCode  FROM HSRPRecords a  where a.HSRP_StateID='" + DropDownListStateName.SelectedValue + "' and a.RTOLocationID='" + dropDownListClient.SelectedValue + "' and a.OrderClosedDate between '" + StringOrderDate + "' and '" + StringAuthDate + "' and  a.OwnerName is not null and a.OwnerName <> '' and Address1 is not null and Address1 <> '' order by OrderClosedDate";

            //    string sqlquery = "select ROW_NUMBER() Over (Order by Hsrprecord_Creationdate) As SNo, vehicleregno,Vehicletype,OrderStatus,PreCloserDateTime from hsrprecords  where hsrp_stateid ='" + HSRPStateID + "'  and Rtolocationid='" + dropDownListClient.SelectedValue.ToString() + "'  and PreCloserDateTime between '" + StringOrderDate + "' and '" + StringAuthDate + "'  and orderstatus in ('closed')   order by PreCloserDateTime asc";


            //    string filename = DropDownListStateName.SelectedItem.ToString() + "_AffixationReport_" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";

            //    Workbook book = new Workbook();
            //    StyleForTheFirstTime(book);
            //    DataTable dtRecord = Utils.GetDataTable(sqlquery, CnnString);
            //    if (dtRecord.Rows.Count > 0)
            //    {
            //        //ExportToPDFAll();
            //       // DownLoadPDF(dtRecord);



            //    }
            //    else
            //    {
            //        lblErrMsg.Text = "No Record For the Selected Date.";
            //    }
            //}
            //catch (Exception ex)
            //{
            //    lblErrMsg.Text = "Error in Populating Grid :" + ex.Message.ToString();
            //}


        }

        string strInvoiceNo = "54321";
        public void DownLoadPDF(DataTable dt)
        {
            String ReportDateFrom = OrderDate.SelectedDate.ToString("yyyy/MM/dd");
            String ReportDateTo = HSRPAuthDate.SelectedDate.ToString("yyyy/MM/dd");
            int i = 0;
            if (dt.Rows.Count > 0)
            {
                i = dt.Rows.Count;
                string filename = DropDownListStateName.SelectedItem.ToString() + "_AffixationReport_" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".pdf";
                String StringField = String.Empty;
                String StringAlert = String.Empty;
                StringBuilder bb = new StringBuilder();
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
                PdfPTable table = new PdfPTable(5);
                //actual width of table in points
                table.TotalWidth = 2250f;

                PdfPCell cell120911 = new PdfPCell(new Phrase("HSRP Affixation Report", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell120911.Colspan = 5;
                cell120911.BorderWidthLeft = 0f;
                cell120911.BorderWidthRight = 0f;
                cell120911.BorderWidthTop = 0f;
                cell120911.BorderWidthBottom = 0f;

                cell120911.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell120911);

                PdfPCell cell12091 = new PdfPCell(new Phrase("State Name : " + DropDownListStateName.SelectedItem, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell12091.Colspan = 3;
                cell12091.BorderWidthLeft = 1f;
                cell12091.BorderWidthRight = 0f;
                cell12091.BorderWidthTop = 1f;
                cell12091.BorderWidthBottom = 0f;

                cell12091.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12091);

                PdfPCell cell12093 = new PdfPCell(new Phrase("Report Date From : " + ReportDateFrom, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell12093.Colspan = 2;
                cell12093.BorderWidthLeft = 0f;
                cell12093.BorderWidthRight = 1f;
                cell12093.BorderWidthTop = 1f;
                cell12093.BorderWidthBottom = 0f;

                cell12093.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12093);

                PdfPCell cell12092 = new PdfPCell(new Phrase("Location Name : " + dropDownListClient.SelectedItem, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell12092.Colspan = 2;
                cell12092.BorderWidthLeft = 1f;
                cell12092.BorderWidthRight = 0f;
                cell12092.BorderWidthTop = 0f;
                cell12092.BorderWidthBottom = 0f;

                cell12092.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12092);

                PdfPCell cell12094 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell12094.Colspan = 2;
                cell12094.BorderWidthLeft = 0f;
                cell12094.BorderWidthRight = 0f;
                cell12094.BorderWidthTop = 0f;
                cell12094.BorderWidthBottom = 0f;

                cell12094.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12094);

                PdfPCell cell12095 = new PdfPCell(new Phrase("Report Date To : " + ReportDateTo, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell12095.Colspan = 2;
                cell12095.BorderWidthLeft = 0f;
                cell12095.BorderWidthRight = 1f;
                cell12095.BorderWidthTop = 0f;
                cell12095.BorderWidthBottom = 0f;

                cell12095.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12095);


                PdfPCell cell1209 = new PdfPCell(new Phrase("S.No.", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell1209.Colspan = 1;
                cell1209.BorderWidthLeft = 1f;
                cell1209.BorderWidthRight = .8f;
                cell1209.BorderWidthTop = 1f;
                cell1209.BorderWidthBottom = 1f;

                cell1209.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1209);


                PdfPCell cell1210 = new PdfPCell(new Phrase("Vehicle No", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell1210.Colspan = 1;
                cell1210.BorderWidthLeft = 0f;
                cell1210.BorderWidthRight = .8f;
                cell1210.BorderWidthTop = 1f;
                cell1210.BorderWidthBottom = 1f;

                cell1210.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1210);



                PdfPCell cell1213 = new PdfPCell(new Phrase("Vehicle Type", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell1213.Colspan = 1;
                cell1213.BorderWidthLeft = 0f;
                cell1213.BorderWidthRight = .8f;
                cell1213.BorderWidthTop = 1f;
                cell1213.BorderWidthBottom = 1f;

                cell1213.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1213);


                PdfPCell cell12233 = new PdfPCell(new Phrase("Order Status", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell12233.Colspan = 1;
                cell12233.BorderWidthLeft = 0f;
                cell12233.BorderWidthRight = .8f;
                cell12233.BorderWidthTop = 1f;
                cell12233.BorderWidthBottom = 1f;

                cell12233.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12233);

                PdfPCell cell122331 = new PdfPCell(new Phrase("Preclosed Date Time", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell122331.Colspan = 1;
                cell122331.BorderWidthLeft = 0f;
                cell122331.BorderWidthRight = .8f;
                cell122331.BorderWidthTop = 1f;
                cell122331.BorderWidthBottom = 1f;

                cell122331.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell122331);


                //PdfPCell cell122332 = new PdfPCell(new Phrase("Vehicle Class", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                //cell122332.Colspan = 1;
                //cell122332.BorderWidthLeft = 0f;
                //cell122332.BorderWidthRight = .8f;
                //cell122332.BorderWidthTop = 1f;
                //cell122332.BorderWidthBottom = 1f;

                //cell122332.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                //table.AddCell(cell122332);

                //PdfPCell cell1206 = new PdfPCell(new Phrase("Vehicle Type", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                //cell1206.Colspan = 1;
                //cell1206.BorderWidthLeft = 0f;
                //cell1206.BorderWidthRight = .8f;
                //cell1206.BorderWidthTop = 1f;
                //cell1206.BorderWidthBottom = 1f;
                //cell1206.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                //table.AddCell(cell1206);

                //PdfPCell cell1221 = new PdfPCell(new Phrase("Vehicle Reg. No", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                //cell1221.Colspan = 1;
                //cell1221.BorderWidthLeft = 0f;
                //cell1221.BorderWidthRight = 1f;
                //cell1221.BorderWidthTop = 1f;
                //cell1221.BorderWidthBottom = 1f;

                //cell1221.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                //table.AddCell(cell1221);


                //PdfPCell cell120933 = new PdfPCell(new Phrase("Front Laser No.", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                //cell120933.Colspan = 1;
                //cell120933.BorderWidthLeft = 1f;
                //cell120933.BorderWidthRight = .8f;
                //cell120933.BorderWidthTop = 1f;
                //cell120933.BorderWidthBottom = 1f;

                //cell120933.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                //table.AddCell(cell120933);



                //PdfPCell cell120935 = new PdfPCell(new Phrase("Rear Laser No.", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                //cell120935.Colspan = 1;
                //cell120935.BorderWidthLeft = 1f;
                //cell120935.BorderWidthRight = .8f;
                //cell120935.BorderWidthTop = 1f;
                //cell120935.BorderWidthBottom = 1f;

                //cell120935.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                //table.AddCell(cell120935);

                //PdfPCell cell120936 = new PdfPCell(new Phrase("Front Plate Size", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                //cell120936.Colspan = 1;
                //cell120936.BorderWidthLeft = 1f;
                //cell120936.BorderWidthRight = .8f;
                //cell120936.BorderWidthTop = 1f;
                //cell120936.BorderWidthBottom = 1f;

                //cell120936.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                //table.AddCell(cell120936);

                //PdfPCell cell120937 = new PdfPCell(new Phrase("Rear Plate Size.", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                //cell120937.Colspan = 1;
                //cell120937.BorderWidthLeft = 1f;
                //cell120937.BorderWidthRight = .8f;
                //cell120937.BorderWidthTop = 1f;
                //cell120937.BorderWidthBottom = 1f;

                //cell120937.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                //table.AddCell(cell120937);

                //PdfPCell cell120938 = new PdfPCell(new Phrase("3RD Sticker", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                //cell120938.Colspan = 1;
                //cell120938.BorderWidthLeft = 1f;
                //cell120938.BorderWidthRight = .8f;
                //cell120938.BorderWidthTop = 1f;
                //cell120938.BorderWidthBottom = 1f;

                //cell120938.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right                
                //table.AddCell(cell120938);

                //PdfPCell cell1209349 = new PdfPCell(new Phrase("Amount", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                //cell1209349.Colspan = 1;
                //cell1209349.BorderWidthLeft = 1f;
                //cell1209349.BorderWidthRight = .8f;
                //cell1209349.BorderWidthTop = 1f;
                //cell1209349.BorderWidthBottom = 1f;

                //cell1209349.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                //table.AddCell(cell1209349);



                i = i - 1;
                while (i >= 0)
                {
                    PdfPCell cell1211 = new PdfPCell(new Phrase(dt.Rows[i]["SNo"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell1211.Colspan = 1;
                    cell1211.BorderWidthLeft = 1f;
                    cell1211.BorderWidthRight = .11f;
                    cell1211.BorderWidthTop = .8f;
                    cell1211.BorderWidthBottom = .8f;

                    cell1211.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell1211);

                    //if (string.IsNullOrEmpty(dt.Rows[i]["Vehicleregno"].ToString()))
                    //{
                    // strInvoiceNo = "BRR/CSH000" + (Convert.ToInt32(strInvoiceNo.Substring(Math.Max(0, strInvoiceNo.Length - 4))) + 1).ToString();
                    PdfPCell cell1212 = new PdfPCell(new Phrase(dt.Rows[i]["VehicleRegNo"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell1212.Colspan = 1;
                    cell1212.BorderWidthLeft = 1f;
                    cell1211.BorderWidthRight = .11f;
                    cell1211.BorderWidthTop = .8f;
                    cell1211.BorderWidthBottom = .8f;

                    cell1212.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell1212);


                    PdfPCell cell12194 = new PdfPCell(new Phrase(dt.Rows[i]["VehicleType"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell12194.Colspan = 1;
                    cell12194.BorderWidthLeft = 0f;
                    cell1211.BorderWidthRight = .11f;
                    cell1211.BorderWidthTop = .8f;
                    cell1211.BorderWidthBottom = .8f;

                    cell12194.HorizontalAlignment = 0; //0=Left, 1=     //PdfPCell cell1218 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));

                    table.AddCell(cell12194);


                    PdfPCell cell1216 = new PdfPCell(new Phrase(dt.Rows[i]["OrderStatus"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell1216.Colspan = 1;
                    cell1216.BorderWidthLeft = 0f;
                    cell1211.BorderWidthRight = .11f;
                    cell1211.BorderWidthTop = .8f;
                    cell1211.BorderWidthBottom = .8f;

                    cell1216.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell1216);


                    PdfPCell cell1222 = new PdfPCell(new Phrase(dt.Rows[i]["PreCloserDateTime"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell1222.Colspan = 1;
                    cell1222.BorderWidthLeft = 0f;
                    cell1211.BorderWidthRight = .11f;
                    cell1211.BorderWidthTop = .8f;
                    cell1211.BorderWidthBottom = .8f;

                    cell1222.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell1222);
                    i--;
                    //}

                }



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
                string closescript1 = "<script>alert('No records found for selected date.')</script>";
                Page.RegisterStartupScript("abc", closescript1);
                return;
            }
        }

        private DataTable GetRtoLocation()
        {

            string sql = "select rtolocationname,rtolocationid from rtolocation where rtolocationid in (select distinct distrelation from rtolocation where hsrp_stateid ='" + HSRPStateID + "'  and Rtolocationid='" + dropDownListClient.SelectedValue.ToString() + "') and RTOLocationID not in (148,331)";
            DataTable dtrto = Utils.GetDataTable(sql, CnnString);
            return dtrto;
        }

        public void EpsionPrint()
        {
            String ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            DataTable GetLocation;
            string LocationName;
            string AffAddress = string.Empty;
            string sqlquery = string.Empty;
            string rtolocationname = string.Empty;
            String StringOrderDate = OrderDate.SelectedDate.ToString("yyyy/MM/dd") + " 00:00:00";
            String StringAuthDate = HSRPAuthDate.SelectedDate.ToString("yyyy/MM/dd") + " 23:59:59";

            string SQLString = "select ROW_NUMBER() Over (Order by Hsrprecord_Creationdate) As SNo, vehicleregno,Vehicletype,Hsrprecord_Creationdate,ordercloseddate from hsrprecords  where hsrp_stateid ='" + HSRPStateID + "'  and Rtolocationid='" + dropDownListClient.SelectedValue.ToString() + "'  and PreCloserDateTime between '" + StringOrderDate + "' and '" + StringAuthDate + "'  and orderstatus in ('closed')   order by Hsrprecord_Creationdate asc";

            GetLocation = Utils.GetDataTable("select rtolocationname,rtolocationid from rtolocation where rtolocationid in (select distinct distrelation from rtolocation where hsrp_stateid ='" + HSRPStateID + "'  and Rtolocationid='" + dropDownListClient.SelectedValue.ToString() + "')", ConnectionString);
            if (GetLocation.Rows[0]["rtolocationname"] != "" || GetLocation.Rows[0]["rtolocationname"] != null)
            {
                LocationName = " - " + GetLocation.Rows[0]["rtolocationname"];
            }
            else
            {
                LocationName = "";
            }

            DataTable dataSetFillHSRPDeliveryChallan = Utils.GetDataTable(SQLString, ConnectionString);
            
            DataProvider.BAL obj = new DataProvider.BAL();
            string filename = "";
            string PdfFolder = "";
            filename = "CASHRECEIPT_" + System.DateTime.Now + ".pdf";
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
                float imageWidth = 218;
                float imageHeight = 360;
                document.SetMargins(0, 0, 5, 0);
                document.SetPageSize(new iTextSharp.text.Rectangle(imageWidth, imageHeight));

                BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);

                // iTextSharp.text.Font times = new Font(bfTimes, 12, Font.ITALIC, Color.Black);
                //Creates a Writer that listens to this document and writes the document to the Stream of your choice:
                PdfFolder = ConfigurationManager.AppSettings["PdfFolder"].ToString() + filename;
                //PdfWriter.GetInstance(document, new FileStream(Server.MapPath(PdfFolder), FileMode.Create));
                PdfWriter.GetInstance(document, new FileStream(PdfFolder, FileMode.Create));
                document.Open();
                
                PdfPTable table01 = new PdfPTable(2);
                PdfPCell cell0 = new PdfPCell(new Phrase("PreClosed To Closed", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.UNDERLINE, iTextSharp.text.BaseColor.BLACK)));
                cell0.Colspan = 2;
                cell0.BorderWidthLeft = 0f;
                cell0.BorderWidthRight = 0f;
                cell0.BorderWidthTop = 0f;
                cell0.BorderWidthBottom = 0f;
                cell0.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table01.AddCell(cell0);
                document.Add(table01);
                //Opens the document:
                
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
                    if (i != 0)
                    {

                        PdfPTable table011 = new PdfPTable(2);
                        PdfPCell cell011 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.UNDERLINE, iTextSharp.text.BaseColor.BLACK)));
                        cell011.Colspan = 2;
                        cell011.BorderWidthLeft = 0f;
                        cell011.BorderWidthRight = 0f;
                        cell011.BorderWidthTop = 0f;
                        cell011.BorderWidthBottom = 0f;
                        cell011.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        table011.AddCell(cell011);
                        document.Add(table011);
                    }

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

                    PdfPCell cell12 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 9f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell12.Colspan = 2;
                    cell12.BorderColor = BaseColor.WHITE;
                    cell12.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell12);

                    //PdfPCell cell1203 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 9f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    //cell1203.Colspan = 2;
                    //cell1203.BorderWidthLeft = 0f;
                    //cell1203.BorderWidthRight = 0f;
                    //cell1203.BorderWidthTop = 0f;
                    //cell1203.BorderWidthBottom = 0f;
                    //cell1203.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    //table.AddCell(cell1203);


                    //PdfPCell cell01 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 9f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    //cell01.Colspan = 2;
                    //cell01.BorderWidthLeft = 0f;
                    //cell01.BorderWidthRight = 0f;
                    //cell01.BorderWidthTop = 0f;
                    //cell01.BorderWidthBottom = 0f;

                    //cell01.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    //table.AddCell(cell01);

                    PdfPCell cellInv02 = new PdfPCell(new Phrase("Location Name.", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cellInv02.Colspan = 0;

                    cellInv02.BorderWidthLeft = 0f;
                    cellInv02.BorderWidthRight = 0f;
                    cellInv02.BorderWidthTop = 0f;
                    cellInv02.BorderWidthBottom = 0f;
                    cellInv02.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cellInv02);



                    PdfPCell cellInv022111 = new PdfPCell(new Phrase(": " + GetLocation.Rows[0]["rtolocationname"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cellInv022111.Colspan = 0;
                    cellInv022111.BorderWidthLeft = 0f;
                    cellInv022111.BorderWidthRight = 0f;
                    cellInv022111.BorderWidthTop = 0f;
                    cellInv022111.BorderWidthBottom = 0f;
                    cellInv022111.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cellInv022111);


                    //PdfPCell cell1 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 9f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    //cell1.Colspan = 2;
                    //cell1.BorderWidthLeft = 0f;
                    //cell1.BorderWidthRight = 0f;
                    //cell1.BorderWidthTop = 0f;
                    //cell1.BorderWidthBottom = 0f;

                    //cell1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    //table.AddCell(cell1);




                    PdfPCell cellInv2 = new PdfPCell(new Phrase("S No.", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cellInv2.Colspan = 0;

                    cellInv2.BorderWidthLeft = 0f;
                    cellInv2.BorderWidthRight = 0f;
                    cellInv2.BorderWidthTop = 0f;
                    cellInv2.BorderWidthBottom = 0f;
                    cellInv2.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cellInv2);



                    PdfPCell cellInv22111 = new PdfPCell(new Phrase(": " + dataSetFillHSRPDeliveryChallan.Rows[i]["SNo"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cellInv22111.Colspan = 0;
                    cellInv22111.BorderWidthLeft = 0f;
                    cellInv22111.BorderWidthRight = 0f;
                    cellInv22111.BorderWidthTop = 0f;
                    cellInv22111.BorderWidthBottom = 0f;
                    cellInv22111.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cellInv22111);

                    
                    PdfPCell cell21 = new PdfPCell(new Phrase("Vehicle Reg No", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell21.Colspan = 0;

                    cell21.BorderWidthLeft = 0f;
                    cell21.BorderWidthRight = 0f;
                    cell21.BorderWidthTop = 0f;
                    cell21.BorderWidthBottom = 0f;
                    cell21.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell21);
                    string CashReceiptDateTime = string.Empty;


                    PdfPCell cell212 = new PdfPCell(new Phrase(": " + dataSetFillHSRPDeliveryChallan.Rows[i]["vehicleregno"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell212.Colspan = 0;

                    cell212.BorderWidthLeft = 0f;
                    cell212.BorderWidthRight = 0f;
                    cell212.BorderWidthTop = 0f;
                    cell212.BorderWidthBottom = 0f;
                    cell212.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell212);



                    PdfPCell cell2 = new PdfPCell(new Phrase("Vehicle Type", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell2.Colspan = 0;

                    cell2.BorderWidthLeft = 0f;
                    cell2.BorderWidthRight = 0f;
                    cell2.BorderWidthTop = 0f;
                    cell2.BorderWidthBottom = 0f;
                    cell2.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell2);



                    PdfPCell cell22111 = new PdfPCell(new Phrase(": " + dataSetFillHSRPDeliveryChallan.Rows[i]["Vehicletype"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell22111.Colspan = 0;
                    cell22111.BorderWidthLeft = 0f;
                    cell22111.BorderWidthRight = 0f;
                    cell22111.BorderWidthTop = 0f;
                    cell22111.BorderWidthBottom = 0f;
                    cell22111.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell22111);

                   
                    PdfPCell cell5 = new PdfPCell(new Phrase("Affix Date", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell5.Colspan = 0;
                    cell5.BorderWidthLeft = 0f;
                    cell5.BorderWidthRight = 0f;
                    cell5.BorderWidthTop = 0f;
                    cell5.BorderWidthBottom = 0f;
                    cell5.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell5);

                    DateTime AuthDate = Convert.ToDateTime(dataSetFillHSRPDeliveryChallan.Rows[i]["Hsrprecord_Creationdate"].ToString());
                    PdfPCell cell55 = new PdfPCell(new Phrase(": " + AuthDate.ToString("dd/MM/yyyy"), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell55.Colspan = 0;
                    cell55.BorderWidthLeft = 0f;
                    cell55.BorderWidthRight = 0f;
                    cell55.BorderWidthTop = 0f;
                    cell55.BorderWidthBottom = 0f;
                    cell55.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell55);

                    PdfPCell cell25 = new PdfPCell(new Phrase("Order Date", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell25.Colspan = 0;

                    cell25.BorderWidthLeft = 0f;
                    cell25.BorderWidthRight = 0f;
                    cell25.BorderWidthTop = 0f;
                    cell25.BorderWidthBottom = 0f;
                    cell25.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell25);

                    DateTime orderdate = Convert.ToDateTime(dataSetFillHSRPDeliveryChallan.Rows[i]["ordercloseddate"].ToString());

                    PdfPCell cell255 = new PdfPCell(new Phrase(": " + orderdate.ToString("dd/MM/yyyy"), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell255.Colspan = 0;

                    cell255.BorderWidthLeft = 0f;
                    cell255.BorderWidthRight = 0f;
                    cell255.BorderWidthTop = 0f;
                    cell255.BorderWidthBottom = 0f;
                    cell255.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell255);                  



                    //PdfPCell cell62an = new PdfPCell(new Phrase("(" + sno.ToString() + ")", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    //cell62an.Colspan = 0;
                    //cell62an.BorderColor = BaseColor.WHITE;
                    //cell62an.BorderWidthLeft = 0f;
                    //cell62an.BorderWidthRight = 0f;
                    //cell62an.BorderWidthTop = 0f;
                    //cell62an.BorderWidthBottom = 0f;
                    //cell62an.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    //table.AddCell(cell62an);

                    //PdfPCell cellsp1 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    //cellsp1.Colspan = 2;
                    //cellsp1.BorderWidthLeft = 0f;
                    //cellsp1.BorderWidthRight = 0f;
                    //cellsp1.BorderWidthTop = 0f;
                    //cellsp1.BorderWidthBottom = 0f;
                    //cellsp1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    //table.AddCell(cellsp1);

                    //PdfPCell cell22 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    //cell22.Colspan = 0;
                    //cell22.BorderWidthLeft = 0f;
                    //cell22.BorderWidthRight = 0f;
                    //cell22.BorderWidthTop = 0f;
                    //cell22.BorderWidthBottom = 0f;
                    //cell22.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    //table.AddCell(cell22);

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
                   // string query = "update hsrprecords set APwebserviceresp='Y' where hsrprecordID='" + dataSetFillHSRPDeliveryChallan.Rows[i]["hsrprecordID"].ToString() + "'";
                   // Utils.ExecNonQuery(query, ConnectionString);
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
                lblErrMsg.Visible = true;
                lblErrMsg.Text = "Record Not Found";
            }
        }

    }
}