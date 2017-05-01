using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using DataProvider;

namespace HSRP.Master
{
    public partial class ReadyforInstallationCashinHand : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string UserType = string.Empty;
        string HSRPRecordID = string.Empty;
        string Mode;
        string ISFrontPlateSize;
        string ISRearPlateSize;
        string StickerMandatory;
        string UserID;
        string HSRPStateID = string.Empty;
        string RTOLocationID = string.Empty;
        int intHSRPStateID;
        int intRTOLocationID;
        string FrontReject;
        string RearReject;
        string FrontRejectLaserNo;
        string RearRejectLaserNo;
        int RTOLocationIDAssign;
        int StateIDAssign;
        string RearPlateSize = string.Empty;
        string FrontPlateSize = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();

            Utils.GZipEncodePage();
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {
                UserType = Session["UserType"].ToString();
            }
            //AutoCompleteExtender1.ContextKey = Session["UserHSRPStateID"].ToString();
            //AutoCompleteExtender2.ContextKey = Session["UserHSRPStateID"].ToString();

            //DropDownListPrefixLaserNo
           
            SQLString = "select count(*) from laserAssignquick where userID='" + Session["UID"].ToString() + "' and recordDate between (convert(varchar,GetDate(),111)+' 00:00:00') and (convert(varchar,GetDate(),111)+' 23:59:59')";
            int totalrecord = Utils.getScalarCount(SQLString, CnnString);
            if (totalrecord > 0)
            {
                lbltotalSavedRecord.Text = Convert.ToString(totalrecord);
            }


            // Page.ClientScript.RegisterStartupScript(Page.GetType(), "show", "ValidationPrefixVisible()", true);
             //if (RadioButtonIndualPlate.Checked == true)
             //{
             //    divFirst.Visible = true;

             //    divSecond.Visible = false;
             //    divThird.Visible = false;
             //}

             //if (RadioButtonCompleteBox.Checked == true)
             //{
             //    divFirst.Visible = false;
             //    divSecond.Visible = true;
             //    divThird.Visible = true;
             //}



            if (!Page.IsPostBack)
            {
                if (RadioButtonIndualPlate.Checked == true)
                {
                    divFirst.Visible = true;

                    divSecond.Visible = false;
                    divThird.Visible = false;
                }

                if (RadioButtonCompleteBox.Checked == true)
                {
                    divFirst.Visible = false;
                    divSecond.Visible = true;
                    divThird.Visible = true;
                }

                TextBoxLaserNoError.Visible = false;
                dropdownPrefixLaserNo(); 
                //Page.ClientScript.RegisterStartupScript(Page.GetType(), "show", "ValidationPrefixVisible('Params');", true);

                RTOLocationID = Session["UserRTOLocationID"].ToString();
                HSRPStateID = Session["UserHSRPStateID"].ToString();
                Mode = Request.QueryString["Mode"];
                // UserID = Request.QueryString["UserID"];
                textboxLaserCodeFrom.Text = "";
                 
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {


        }


        public void blanckfield()
        {
            textboxLaserCodeFrom.Text = "";
            //textBoxRearPlate.Text = "";
            lblErrMess.Text = "";
            // txtVehicleRegNo.Text = "";
        }

        protected void btnSaveStockinHand_Click(object sender, EventArgs e)
        {
            //Page.ClientScript.RegisterStartupScript(Page.GetType(), "show", "ValidationPrefixVisible();", true);

            lblTotalRecord.Text = "";
            TextBoxLaserNoError.Text = "";
            TextBoxLaserNoError.Visible = false;
             
            lblErrMess.Text = "";
            lblSucMess.Text = "";

            HSRPStateID = Session["UserHSRPStateID"].ToString();
            RTOLocationID = Session["UserRTOLocationID"].ToString();
            UserID = Session["UID"].ToString();

            // btnSave.Enabled = false;

            // string vehicleRegNo = txtVehicleRegNo.Text.Trim();
            string FrontLaserCode = txtboxLaserNo.Text.Trim().ToUpper();
            //string RearLaserCode = textBoxRearPlate.Text.Trim().ToUpper();

            if (RadioButtonIndualPlate.Checked == true)
            {
                divFirst.Visible = true;
                if (txtboxLaserNo.Text != "")
                {
                    SQLString = "select count(*) from BlankPlateInstallationCashinHand where FrontLaserPlate ='" + FrontLaserCode + "'";
                    int Records = Utils.getScalarCount(SQLString, CnnString);
                    if (Records == 0)
                    {
                        SQLString = "insert into BlankPlateInstallationCashinHand (FrontLaserPlate,Hsrp_StateID,RTOLocationID,InstallUserID,Remarks) values ('" + FrontLaserCode + "', '" + HSRPStateID + "','" + RTOLocationID + "','" + UserID + "','Cash in Hand')";
                        int i = Utils.ExecNonQuery(SQLString, CnnString);
                        if (i > 0)
                        {
                            lblSucMess.Text = "Record Save Successfully.";
                        }
                        else
                        {
                            lblErrMess.Text = "Record Not Saved!!";
                        }

                    }
                    else
                    {
                        lblErrMess.Text = "Duplicate Records";
                    }
                }
                else
                {
                    lblErrMess.Text = "Please Enter Laser No.!!";
                }
            }
            if (RadioButtonCompleteBox.Checked == true)
            {
                divSecond.Visible = true;
                divThird.Visible = true;
                if (DropDownListPrefixLaserNo.SelectedItem.Text != "-- Select Prefix --") //|| textboxLaserCodeFrom.Text != "" || textboxQuantity.Text != "")
                {
                    if (textboxLaserCodeFrom.Text != "")  
                    {
                        if (textboxQuantity.Text != "")
                        {

                            Int64 LaserCodeTo = Convert.ToInt64(textboxLaserCodeFrom.Text) + Convert.ToInt64(textboxQuantity.Text) - 1;

                            int BatchCheckedBy = Convert.ToInt16(Session["UID"]);
                            string orderStatus = "New Order";
                            //  string Prefix = textboxPrefixFrom.Text;
                            string Prefix = DropDownListPrefixLaserNo.SelectedItem.Text; //DropDownListPrefixLaserNo.SelectedItem.Text;
                            Int64 LaserCodeFrom = Convert.ToInt64(textboxLaserCodeFrom.Text);
                            decimal CurrentCost = 50;
                            decimal Weight = 50;
                            DateTime CreateDateTime = System.DateTime.Now;
                            int totalPlateInbox = Convert.ToInt32(LaserCodeTo) - Convert.ToInt32(textboxLaserCodeFrom.Text);
                            BAL obj = new BAL();
                            int IsExists = -1;
                            string textboxInvNo = "0";
                            int totalRecord = 0;
                            String textboxInvFrom = "";
                            int TotalExistRecord = 0;
                            string Remarks = "";
                            Int64 Laser = Convert.ToInt32(textboxLaserCodeFrom.Text) - 1; 

                            for (int i = 0; i <= totalPlateInbox; i++)
                            {
                                Laser = +Laser + 1;
                                string LaserNo = DropDownListPrefixLaserNo.SelectedItem.Text + Laser.ToString();
                                //obj.InsertHSRPPInventory(UserID, textboxLaserCodeFrom.Text, Prefix, textboxQuantity.Text, CurrentCost, Weight, textboxInvNo, textboxInvFrom, intHSRPStateID, intRTOLocationID, Remarks, Laser, LaserNo, orderStatus, ref IsExists);
                                SQLString = "select  count(*) from BlankPlateInstallationCashinHand a where a.FrontLaserPlate='" + LaserNo + "'";
                                int j = Utils.getScalarCount(SQLString.ToString(), CnnString);
                                if (j == 1)
                                {
                                    SQLString = "select  FrontLaserPlate, (select hsrpStateName from hsrpstate where hsrp_StateID= a.hsrp_StateID) as StateName from BlankPlateInstallationCashinHand a where a.FrontLaserPlate='" + LaserNo + "'";
                                    DataTable dt = Utils.GetDataTable(SQLString.ToString(), CnnString);
                                    if (dt.Rows.Count > 0)
                                    {
                                        lblErrMess.Text = "Record Already Exist!!";
                                        TextBoxLaserNoError.Text += dt.Rows[0]["FrontLaserPlate"].ToString() + "   " + dt.Rows[0]["StateName"].ToString();

                                        TextBoxLaserNoError.Text += System.Environment.NewLine;
                                        lblSucMess.Text = totalRecord.ToString();
                                        TextBoxLaserNoError.Visible = true;

                                    }
                                }
                                else
                                {
                                    SQLString = "insert into BlankPlateInstallationCashinHand (FrontLaserPlate,Hsrp_StateID,RTOLocationID,InstallUserID,Remarks) values ('" + LaserNo + "', '" + HSRPStateID + "','" + RTOLocationID + "','" + UserID + "','Cash in Hand in Box')";
                                    j = Utils.ExecNonQuery(SQLString, CnnString);
                                    if (j > 0)
                                    {
                                        // lblExist.Visible=false;
                                        //TextBoxLaserNoError.Visible = true;
                                        lblSucMess.Visible = true;
                                        //lblExist.Text="";
                                        totalRecord = totalRecord + 1;
                                        lblSucMess.Text = "Total Updated Record :";
                                        lblTotalRecord.Text = totalRecord.ToString();
                                    }

                                }
                            }
                        }
                        else
                        {
                            lblErrMess.Text = "Please Enter Plate No";
                        }
                    }
                    else
                    {
                        lblErrMess.Text = "Please Enter Laser No";
                    }
                }
                else
                {
                    lblErrMess.Text = "Please Select Prefix";
                }
            } 
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            textboxLaserCodeFrom.Text = ""; 
            lblErrMess.Text = "";
            lblSucMess.Text = "";
        }

        public void dropdownPrefixLaserNo()
        {


            SQLString = "Select PrefixID, Prefix from PrefixLaserNo where ActiveStatus='Y' and HSRP_StateID='" + Session["UserHSRPStateID"].ToString() + "' order by Prefix ";
            Utils.PopulateDropDownList(DropDownListPrefixLaserNo, SQLString, CnnString, "-- Select Prefix --");
            //DropDownListProductName.SelectedIndex = DropDownListProductName.Items.Count - 1;
        }
         

        protected void RadioButtonCompleteBox_CheckedChanged1(object sender, EventArgs e)
        {
            TextBoxLaserNoError.Text = "";
            TextBoxLaserNoError.Visible = false;
            lblTotalRecord.Text = "";

            lblErrMess.Text = "";
            lblSucMess.Text = "";
            if (RadioButtonIndualPlate.Checked == true)
            {
                divFirst.Visible = true;

                divSecond.Visible = false;
                divThird.Visible = false;
            }

            if (RadioButtonCompleteBox.Checked == true)
            {
                divFirst.Visible = false;
                divSecond.Visible = true;
                divThird.Visible = true;
            }
        }

        protected void RadioButtonIndualPlate_CheckedChanged1(object sender, EventArgs e)
        {
            lblTotalRecord.Text = "";
            TextBoxLaserNoError.Text = "";
            TextBoxLaserNoError.Visible = false;

            lblErrMess.Text = "";
            lblSucMess.Text = "";
            if (RadioButtonIndualPlate.Checked == true)
            {
                divFirst.Visible = true;

                divSecond.Visible = false;
                divThird.Visible = false;
            }

            if (RadioButtonCompleteBox.Checked == true)
            {
                divFirst.Visible = false;
                divSecond.Visible = true;
                divThird.Visible = true;
            }
        }

        
    }
}
