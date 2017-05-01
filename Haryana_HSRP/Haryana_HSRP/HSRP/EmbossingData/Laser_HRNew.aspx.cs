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

using System.Net;


namespace HSRP.EmbossingData
{
    public partial class Laser_HRNew : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        string UserType = string.Empty;
        string HSRPStateID = string.Empty;
        string RTOLocationID = string.Empty;
        int intHSRPStateID;
        int IResult;
        string sendURL = string.Empty;
        string SMSText = string.Empty;
        string SqlQuery = string.Empty;
        string trnasportname, pp;

        BaseFont basefont;


        string AllLocation = string.Empty;
        string OrderStatus = string.Empty;


        int iChkCount = 0;
        string vehicle = string.Empty;
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
                            dropDownRtoLocation.Visible = true;
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



        private void FilldropDownListOrganization()
        {
            if (UserType == "0")
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID=4 and ActiveStatus='Y' Order by HSRPStateName";
                Utils.PopulateDropDownList(DropDownListStateName, SQLString.ToString(), CnnString, "--Select State--");
                // DropDownListStateName.SelectedIndex = DropDownListStateName.Items.Count - 1;

            }
            else
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID=4  and ActiveStatus='Y' Order by HSRPStateName";
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
                Utils.PopulateDropDownList(dropDownRtoLocation, SQLString.ToString(), CnnString, "--Select RTO Name--");

                //dataLabellbl.Visible = false;
                //TRRTOHide.Visible = false;
            }
            else
            {
                string UserID = Convert.ToString(Session["UID"]);

                SQLString = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, (a.RTOLocationCode+', ') as RTOCode, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID and a.activestatus='Y' where UserRTOLocationMapping.UserID='" + UserID + "' order by a.RTOLocationName";
                DataTable dss = Utils.GetDataTable(SQLString, CnnString);

                labelOrganization.Visible = true;
                DropDownListStateName.Visible = true;
                labelClient.Visible = true;
                dropDownRtoLocation.Visible = true;

                dropDownRtoLocation.DataSource = dss;
                dropDownRtoLocation.DataBind();
               // dataLabellbl.Visible = true;

                string RTOCode = string.Empty;
                if (dss.Rows.Count > 0)
                {
                    for (int i = 0; i <= dss.Rows.Count - 1; i++)
                    {
                        RTOCode += dss.Rows[i]["RTOCode"].ToString();
                    }
                   // lblRTOCode.Text = RTOCode; //RTOCode.Remove(RTOCode.LastIndexOf(","));
                }
            }
        }
        private void FillDropDownChallanNo()
        {
            SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + intHSRPStateID + " and ActiveStatus!='N'   Order by RTOLocationName";
            Utils.PopulateDropDownList(dropDownRtoLocation, SQLString.ToString(), CnnString, "--Select RTO Name--");

        }

        #endregion

        private void ShowGrid()
        {
            string RtolocationID = dropDownRtoLocation.SelectedValue;

            if (dropdownChallanNo.SelectedItem.Text != "--Select Challan No--")
            {
                string SQLString = "select hsrprecordid, HSRPRecord_AuthorizationNo,vehicleregno,hsrp_rear_lasercode,hsrp_Front_lasercode,OrderStatus,MobileNo,remarks from hsrprecords where hsrp_StateID='" + DropDownListStateName.SelectedValue + "' and rtolocationID='" + dropDownRtoLocation.SelectedValue + "' and ChallanNo='" + dropdownChallanNo.SelectedItem.ToString() + "' and OrderStatus='Embossing Done' and RecievedAffixationStatus='N' order by vehicleregno";
                dt = Utils.GetDataTable(SQLString, CnnString);
                if (dt.Rows.Count > 0)
                {
                    btnUpdate.Visible = true;
                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                }
                else
                {
                   // Button1.Visible = false;
                   // btnSave.Visible = false;
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
        //CheckBox chk;
        //protected void CHKSelect1_CheckedChanged(object sender, EventArgs e)
        //{
        //    CheckBox chk1 = GridView1.HeaderRow.FindControl("CHKSelect1") as CheckBox;
        //    if (chk1.Checked == true)
        //    {
        //        for (int i = 0; i < GridView1.Rows.Count; i++)
        //        {
        //            //  string lblVehicleRegNo,FLasercode,RearLaserCode;
        //            chk = GridView1.Rows[i].Cells[0].FindControl("CHKSelect") as CheckBox;
        //            chk.Checked = true;
        //        }
        //    }
        //    else if (chk1.Checked == false)
        //    {
        //        for (int i = 0; i < GridView1.Rows.Count; i++)
        //        {
        //            //  string lblVehicleRegNo,FLasercode,RearLaserCode;
        //            chk = GridView1.Rows[i].Cells[0].FindControl("CHKSelect") as CheckBox;
        //            chk.Checked = false;
        //        }
        //    }

        //}



        protected void DropDownListStateName_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilldropDownListClient();
            dropDownRtoLocation.Visible = true;
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
            labelOrderStatus.Visible = true;
            dropdownChallanNo.Visible = true;


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

            SQLString = "select  Distinct ChallanNo   from hsrprecords  where hsrp_StateID='" + DropDownListStateName.SelectedValue + "' and rtolocationID='" + dropDownRtoLocation.SelectedValue + "' and RecievedAffixationStatus='N' and  ChallanNo is not null    group by  ChallanNo";
            Utils.PopulateDropDownList(dropdownChallanNo, SQLString.ToString(), CnnString, "--Select Challan No--");
            // GridView1.DataSource = null;
            // GridView1.DataBind();


        }
        DataTable dt = new DataTable();
        protected void dropdownDuplicateFIle_SelectedIndexChanged(object sender, EventArgs e)
        {

            string SQLString = "select hsrprecordid,vehicleregno,hsrp_rear_lasercode,hsrp_Front_lasercode,OrderStatus from hsrprecords where OrderStatus='Embossing Done' and ChallanNo='" + dropdownChallanNo.SelectedItem.ToString() + "' and hsrp_StateID='" + DropDownListStateName.SelectedValue + "' and rtolocationID='" + dropDownRtoLocation.SelectedValue + "'and RecievedAffixationStatus='N' ";
            dt = Utils.GetDataTable(SQLString, CnnString);

            ShowGrid();
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            ShowGrid();

        }

        private void SetPreviousData()
        {
            int rowIndex = 0;
            if (ViewState["hsrprecords"] != null)
            {
                DataTable dt = (DataTable)ViewState["hsrprecords"];
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        TextBox box1 = (TextBox)GridView1.Rows[rowIndex].Cells[6].FindControl("txtnewprocess");   
                        box1.Text = dt.Rows[i]["Column1"].ToString();
                        rowIndex++;
                    }
                }
            }
        }


        protected void btnUpdate_Click(object sender, System.EventArgs e)
        {
            
            //for (int i = 0; i < GridView1.Rows.Count; i++)
            //{
            //    //find TextBox from each GridViewRow
                
            //    TextBox txt1 = (TextBox)GridView1.Rows[i].FindControl("txtnewprocess");

            //    if (txt1.Text != "")
            //    {
            //        SqlQuery = "update hsrprecords set remarks ='" + txt1.Text.ToString() + "' where ";
            //        //SqlQuery = "insert into hsrprecords (remark) values('" + txt1.Text.ToString() + "')";
            //        Utils.ExecNonQuery(SqlQuery, CnnString);
            //        // save/update record
            //    }
            //}


            string rtolocationid = dropDownRtoLocation.SelectedValue;
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
               
                    HSRP.TGWebrefrence.HSRPAuthorizationService objTGService = new HSRP.TGWebrefrence.HSRPAuthorizationService();
                    HSRP.APWebrefrence.HSRPAuthorizationService objAPService = new HSRP.APWebrefrence.HSRPAuthorizationService();
                    iChkCount = iChkCount + 1;
                    string strRecordId = GridView1.DataKeys[i]["hsrprecordid"].ToString();
                    Label lblVehicleRegNo = GridView1.Rows[i].Cells[1].FindControl("lblVehicleRegNo") as Label;
                    Label OrderStatus = GridView1.Rows[i].Cells[4].FindControl("lblOrderStatus") as Label;
                    string flaser = GridView1.Rows[i].Cells[4].FindControl("lblOrderStatus").ToString();
                    string rlaser = GridView1.Rows[i].Cells[4].FindControl("lblOrderStatus").ToString();
                    Label MobileNo = GridView1.Rows[i].Cells[5].FindControl("lblMobileNo") as Label;
                    Label AuthNo = GridView1.Rows[i].Cells[6].FindControl("lblAuthNo") as Label;
                    Label strremark = GridView1.Rows[i].Cells[6].FindControl("lblnewprocess") as Label;
                
                    string strAuthNo = AuthNo.Text;
                    string reg_no = lblVehicleRegNo.Text;
                    //string PhoneNumber = MobileNo.Text;
                    //string result = string.Empty;
                    string strremarks = strremark.ToString();

                    if (strremarks.ToString() == strremark.ToString())
                    { 
                    
                    }

                    TextBox txt1 = (TextBox)GridView1.Rows[i].FindControl("txtnewprocess");
                    if (txt1.Text != "")
                    {
                        SqlQuery = "update hsrprecords set remarks ='" + txt1.Text.ToString() + "' where HSRPRecord_AuthorizationNo='" + strAuthNo + "' and VehicleRegNo='" + reg_no + "'";
                        //SqlQuery = "insert into hsrprecords (remark) values('" + txt1.Text.ToString() + "')";
                        Utils.ExecNonQuery(SqlQuery, CnnString);
                        // save/update record
                    }


            }

            lblErrMsg.Text = "Record Save Successfully";
            ShowGrid();

        }
    }
}


