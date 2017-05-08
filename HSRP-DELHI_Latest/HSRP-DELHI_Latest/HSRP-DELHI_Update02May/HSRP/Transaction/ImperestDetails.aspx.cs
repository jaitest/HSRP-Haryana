using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace HSRP.Transaction
{
    public partial class ImperestDetails : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string Mode = string.Empty;
        string ImperestID = string.Empty;
        int UserType;
        int HSRP_StateID;
        int RTOLocationID;
        int intHSRP_StateID;
        int intRTOLocationID;
        protected void Page_Load(object sender, EventArgs e)
        {
            //txtAmount.Attributes.Add("onKeydown", "return MaskMoney(event)");
            //ScriptManager.GetCurrent(this).RegisterPostBackControl(this.btnSave);
            //Page.Form.Attributes.Add("enctype", "multipart/form-data");

            Utils.GZipEncodePage();
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {
                RTOLocationID = Convert.ToInt32(Session["UserRTOLocationID"].ToString());
                UserType = Convert.ToInt32(Session["UserType"].ToString());
                HSRP_StateID = Convert.ToInt32(Session["UserHSRPStateID"].ToString());
                lblErrMsg.Text = string.Empty;
                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                strUserID = Session["UID"].ToString();
                Mode = Request.QueryString["Mode"].ToString();


                if (!IsPostBack)
                {
                    try
                    {
                        InitialSetting();
                       
                        if (UserType.Equals(0))
                        {
                            dropDownListOrg.Visible = true;
                            FilldropDownListOrganization();
                            dropDownListClient.Visible = true;
                            FilldropDownListClient();
                        }
                        else if (UserType.Equals(1))
                        {
                            dropDownListOrg.Visible = false;
                            dropDownListClient.Visible = true;
                            FilldropDownListClient();
                        }
                        else
                        {
                            dropDownListOrg.Visible = false;
                            dropDownListClient.Visible = false;
                        }

                        if (Mode == "Edit")
                        {
                            LabelFormName.Text = "Update Imperest Details";
                            ImperestID = Request.QueryString["ImperestID"].ToString();
                            btnSave.Visible = false;
                            btnUpdate.Visible = true;

                            SQLString = "select * from ImperestDetails where ImperestID='" + ImperestID + "'";
                            DataTable dt = Utils.GetDataTable(SQLString, CnnString);

                            if (dt.Rows.Count > 0)
                            {
                                dropDownListOrg.SelectedValue=dt.Rows[0]["HSRPStateID"].ToString();

                                SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + dt.Rows[0]["HSRPStateID"].ToString() + " and ActiveStatus!='N' Order by RTOLocationName";
                                Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), CnnString, "--Select RTO--");
                                dropDownListClient.SelectedValue=dt.Rows[0]["RTOLocationID"].ToString();

                                SQLString = "select UserID,ISNULL(UserFirstName,'')+Space(2)+ISNULL(UserLastName,'') as Names from Users where HSRP_StateID='" + dt.Rows[0]["HSRPStateID"].ToString() + "' and RTOLocationID=" + dt.Rows[0]["RTOLocationID"].ToString() + "  order by UserFirstName";
                                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                                Utils.PopulateDropDownList(dropDownListUser, SQLString, CnnString, "--Select User--");
                                dropDownListUser.SelectedValue=dt.Rows[0]["UserID"].ToString();

                                txtAmount.Text=dt.Rows[0]["ImperestAmt"].ToString();
                                txtRemarks.Text=dt.Rows[0]["Remarks"].ToString();
                                OrderDate.SelectedDate = Convert.ToDateTime(dt.Rows[0]["DateOfIssue"]);
                                
                            }

                        }
                        else
                        {
                            LabelFormName.Text = "Add Imperest Details";
                            btnSave.Visible = true;
                            btnUpdate.Visible = false;
                        }
                    }
                    catch (Exception err)
                    {
                        lblErrMsg.Text = "Error on Page Load " + err.Message.ToString();
                    }
                }
            }
        }
        private void InitialSetting()
        {

            string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            string MaxDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();

            OrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            OrderDate.MaxDate = DateTime.Parse(MaxDate);
            //OrderDate.AllowWeekSelection = true;

            // OrderDate.MinDate = (DateTime.Parse(TodayDate)).AddDays(-7.00);
            int remaindate = System.DateTime.Now.Day - 1;
            OrderDate.MinDate = (DateTime.Parse(TodayDate)).AddDays(-remaindate);
            //OrderDate.AllowDaySelection = true;
        }

        #region DropDown

        private void FilldropDownListOrganization()
        {
            if (UserType.Equals(0))
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState Order by HSRPStateName";
            }
            else
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID=" + HSRP_StateID + "Order by HSRPStateName";
            }
            Utils.PopulateDropDownList(dropDownListOrg, SQLString.ToString(), CnnString, "--Select State--");
            dropDownListOrg.SelectedIndex = 0;
        }

        private void FilldropDownListClient()
        {
            if (UserType.Equals(0))
            {
                int.TryParse(dropDownListOrg.SelectedValue, out intHSRP_StateID);
                SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + intHSRP_StateID + " and ActiveStatus!='N' Order by RTOLocationName";
            }
            else
            {
                intHSRP_StateID = HSRP_StateID;
                SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + intHSRP_StateID + " and ActiveStatus!='N' Order by RTOLocationName";
            }
            Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), CnnString, "--Select RTO--");
            ///dropDownListClient.SelectedIndex = dropDownListClient.Items.Count - 1;
        }

        protected void dropDownListOrg_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dropDownListOrg.SelectedItem.Text != "--Select State--")
            {
                dropDownListClient.Visible = true;
                FilldropDownListClient();
            }
            //else
            //{
            //    lblErrMsg.Text = string.Empty;
            //    lblErrMsg.Text = "Please Select State.";
            //    dropDownListClient.Visible = false;
            //}
        }

        private void FilldropDownListUser()
        {
            int intHSRPStateID;
            int intRTOLocationID;
            if (UserType == 0)
            {
                int.TryParse(dropDownListOrg.SelectedValue, out intHSRPStateID);
                int.TryParse(dropDownListClient.SelectedValue, out  intRTOLocationID);
            }
            else if (UserType == 1)
            {
                intHSRPStateID = Convert.ToInt32(HSRP_StateID);
                int.TryParse(dropDownListClient.SelectedValue, out  intRTOLocationID);
            }
            else
            {
                intHSRPStateID = Convert.ToInt32(HSRP_StateID);
                intRTOLocationID = Convert.ToInt32(RTOLocationID);
            }

            SQLString = "select UserID,ISNULL(UserFirstName,'')+Space(2)+ISNULL(UserLastName,'') as Names from Users where HSRP_StateID='" + intHSRPStateID + "' and RTOLocationID=" + intRTOLocationID + "  order by UserFirstName";
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            Utils.PopulateDropDownList(dropDownListUser, SQLString, CnnString, "--Select User--");
            dropDownListUser.SelectedIndex = 0;
        }

        #endregion
        protected void btnSave_Click(object sender, EventArgs e)
        {
            String[] StringAuthDate = OrderDate.SelectedDate.ToString().Split('/');
            String From1 = StringAuthDate[0] + "/" + StringAuthDate[1] + "/" + StringAuthDate[2].Split(' ')[0];
            DateTime BillDate = new DateTime(Convert.ToInt32(StringAuthDate[2].Split(' ')[0]), Convert.ToInt32(StringAuthDate[0]), Convert.ToInt32(StringAuthDate[1]));
            //DateTime BillDate = OrderDate.SelectedDate;
            //string dtt = BillDate.ToString("dd-MMM-yyyy");


            //int Count = Utils.getScalarCount("select COUNT(*) from ExpenseSave where ExpenseID='" + ddlExpense.SelectedValue + "' and BillNo='" + txtBillNo.Text + "'", CnnString);
            //if (Count > 0)
            //{
            //    lblErrMsg.Text = "Duplicate Records";
            //    lblSucMess.Text = "";
            //}
            //else
            //{

                SQLString = "Insert Into ImperestDetails (HSRPStateID,RTOLocationID,UserID,ImperestAmt,DateOfIssue,Remarks) VALUES ('" + dropDownListOrg.SelectedValue + "','" + dropDownListClient.SelectedValue + "','" + dropDownListUser.SelectedValue + "','" + txtAmount.Text + "','" + BillDate + "','" + txtRemarks.Text + "')";
                Utils.ExecNonQuery(SQLString, CnnString);
                BlankFields();
                lblSucMess.Text = "Record Saved Successfully";
            //}

           
        }

        private void BlankFields()
        {
            FilldropDownListUser();
            FilldropDownListClient();
            FilldropDownListOrganization();
            txtAmount.Text = "0";
            txtRemarks.Text = string.Empty;
            lblErrMsg.Text = string.Empty;
            lblSucMess.Text = string.Empty;
        }

        

        protected void dropDownListClient_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dropDownListClient.SelectedItem.Text != "--Select RTO Name--")
            {
                dropDownListUser.Enabled = true;
                FilldropDownListUser();
                // UpdatePanelUser.Update();
            }
            else
            {
                dropDownListUser.Enabled = false;
                dropDownListUser.Enabled = false;
                lblErrMsg.Text = string.Empty;
                lblErrMsg.Text = "Please Select User";
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            ImperestID = Request.QueryString["ImperestID"].ToString();
            String[] StringAuthDate = OrderDate.SelectedDate.ToString().Split('/');
            String From1 = StringAuthDate[0] + "/" + StringAuthDate[1] + "/" + StringAuthDate[2].Split(' ')[0];
            DateTime BillDate = new DateTime(Convert.ToInt32(StringAuthDate[2].Split(' ')[0]), Convert.ToInt32(StringAuthDate[0]), Convert.ToInt32(StringAuthDate[1]));

            SQLString = "UPDATE ImperestDetails SET HSRPStateID='" + dropDownListOrg.SelectedValue + "',RTOLocationID='" + dropDownListClient.SelectedValue + "',UserID='" + dropDownListUser.SelectedValue + "',ImperestAmt='" + txtAmount.Text + "',DateOfIssue='" + BillDate + "',Remarks='" + txtRemarks.Text + "' where ImperestID='" + ImperestID + "'";
            Utils.ExecNonQuery(SQLString, CnnString);
            lblSucMess.Text = "Record Saved Successfully";
        }
    }
}