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

namespace HSRP.HR
{
    public partial class AffixationPreclosedtoEmbossingDone : System.Web.UI.Page
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
                    SQLString = "select hsrprecordid,vehicleregno,hsrp_rear_lasercode,hsrp_Front_lasercode,HSRPRecord_AuthorizationNo,orderstatus from hsrprecords  where hsrp_stateid ='" + HSRPStateID + "'  and Rtolocationid='" + dropDownListClient.SelectedValue.ToString() + "'  and PreCloserDateTime between '" + AuthorizationDate + "' and '" + ToDate + "'  and orderstatus in ('PreClosed')   order by VehicleClass,VehicleType,VehicleRegNo";
                    dt = Utils.GetDataTable(SQLString, CnnString);
                }
                else
                {
                    SQLString = "select hsrprecordid,vehicleregno,hsrp_rear_lasercode,hsrp_Front_lasercode,HSRPRecord_AuthorizationNo,orderstatus from hsrprecords where hsrp_stateid ='" + HSRPStateID + "' and Rtolocationid='" + dropDownListClient.SelectedValue.ToString() + "' and PreCloserDateTime between '" + AuthorizationDate + "' and '" + ToDate + "' and orderstatus in ('PreClosed')  order by VehicleClass,VehicleType,VehicleRegNo";
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

                        if (txtOS.Text.ToString() == "Closed" )
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

                    if(flag=="0")
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
            GridView1.PageIndex = 0;
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
                    TextBox RearLaserCode = GridView1.Rows[i].Cells[2].FindControl("txtFLaserCode") as TextBox;
                    TextBox FLasercode = GridView1.Rows[i].Cells[3].FindControl("txtRLaserCode") as TextBox;
                    Label id = GridView1.Rows[i].Cells[4].FindControl("id") as Label;
                    if (chk.Checked == true)
                    {                      
                        // TextBox orderstatus = GridView1.Rows[i].Cells[3].FindControl("txtOS") as TextBox;
                        //  labtext = GridView1.Rows[i].Cells[0].Text;

                       // sb.Append("update hsrprecords set orderstatus='Closed',[ordercloseddate]= getdate(), closeduserid='" + strUserID + "' where HSRPRecordID='" + id.Text + "' and orderstatus='PreClosed' " + Environment.NewLine);
                        sb.Append("update hsrprecords set orderstatus='Embossing Done', closeduserid='" + strUserID + "' where HSRPRecordID='" + id.Text + "' and orderstatus='PreClosed' and vehicleregno='" + lblVehicleRegNo.Text + "'" + Environment.NewLine);

                        //sby.Append("update preclosehsrprecords set processstatus='N' where HSRPRecordID='" + id.Text + "' and processstatus='Y' ");
                    }
                    else
                    {
                       // sb.Append("update hsrprecords set orderstatus='Embossing Done',[ordercloseddate]=null , closeduserid='" + strUserID + "' where HSRPRecordID='" + id.Text + "' and orderstatus='PreClosed' and vehicleregno='" + lblVehicleRegNo.Text + "'" + Environment.NewLine);
                    }
                }
                int j = Utils.ExecNonQuery(sb.ToString(), CnnString);
                int k = Utils.ExecNonQuery(sby.ToString(), CnnString);
                //Synct to AP Server 
                //if (DropDownListStateName.SelectedValue.ToString() == "9")
                //{

                //    HSRP.APWebrefrence.HSRPAuthorizationService objAPService = new HSRP.APWebrefrence.HSRPAuthorizationService();
                //    for (int i = 0; i < GridView1.Rows.Count; i++)
                //    {
                //        chk = GridView1.Rows[i].Cells[0].FindControl("CHKSelect") as CheckBox;
                //        if (chk.Checked == true)
                //        {
                //            Label lblVehicleRegNo = GridView1.Rows[i].Cells[1].FindControl("lblVehicleRegNo") as Label;
                //            TextBox RearLaserCode = GridView1.Rows[i].Cells[2].FindControl("txtFLaserCode") as TextBox;
                //            TextBox FLasercode = GridView1.Rows[i].Cells[3].FindControl("txtRLaserCode") as TextBox;
                //            Label id = GridView1.Rows[i].Cells[4].FindControl("id") as Label;
                //            Label lblAuthNo = GridView1.Rows[i].Cells[5].FindControl("AuthNo") as Label;

                //            string responsecodeAp = objAPService.UpdateHSRPAffixation(lblAuthNo.Text, DateTime.Now.ToString("dd/MM/yyyy"));
                //            sb1.Append("update hsrprecords set APwebservicerespAff='" + responsecodeAp + "',APwebservicerespAffdate=Getdate() where HSRPRecordID='" + id.Text + "'" + Environment.NewLine);

                //        }

                //    }
                //    Utils.ExecNonQuery(sb1.ToString(), CnnString);
                //}

                //if (DropDownListStateName.SelectedValue.ToString() == "11")
                //{

                //    HSRP.TGWebrefrence.HSRPAuthorizationService objTGService = new HSRP.TGWebrefrence.HSRPAuthorizationService();
                //    for (int i = 0; i < GridView1.Rows.Count; i++)
                //    {
                //        chk = GridView1.Rows[i].Cells[0].FindControl("CHKSelect") as CheckBox;
                //        if (chk.Checked == true)
                //        {
                //            Label lblVehicleRegNo = GridView1.Rows[i].Cells[1].FindControl("lblVehicleRegNo") as Label;
                //            TextBox RearLaserCode = GridView1.Rows[i].Cells[2].FindControl("txtFLaserCode") as TextBox;
                //            TextBox FLasercode = GridView1.Rows[i].Cells[3].FindControl("txtRLaserCode") as TextBox;
                //            Label id = GridView1.Rows[i].Cells[4].FindControl("id") as Label;
                //            Label lblAuthNo = GridView1.Rows[i].Cells[5].FindControl("AuthNo") as Label;

                //            // string responsecodeTG = objTGService.UpdateHSRPLaserCodes(lblAuthNo.Text, FLasercode.Text, RearLaserCode.Text, DateTime.Now.ToString("dd/MM/yyyy"));
                //            string responsecodeTG = objTGService.UpdateHSRPAffixation(lblAuthNo.Text, DateTime.Now.ToString("dd/MM/yyyy"));
                //            sb1.Append("update hsrprecords set APwebservicerespAff='" + responsecodeTG + "',APwebservicerespAffdate=Getdate() where HSRPRecordID='" + id.Text + "'" + Environment.NewLine);

                //        }
                //    }
                //    Utils.ExecNonQuery(sb1.ToString(), CnnString);
                //}
                //if (j > 0)
                //{

                //    LblMessage.Text = "Updated Records Sucessfull";
                //    btnGO_Click(sender, e);
                //    btnSave.Enabled = false;
                //}
                //else
                //{
                //    LblErrorMessage.Text = "Records not updated successfully";
                //    btnSave.Enabled = false;
                //}
                
            }
            catch(Exception ex)
            {
                LblErrorMessage.Text = "Data Not Uploaded Contact Administration with screen shot Error :" + ex.Message.ToString();
            }
            ShowGrid();
         
        }
    }
}