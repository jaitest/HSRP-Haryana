using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace HSRP.Master
{
    public partial class User : System.Web.UI.Page
    {
        string StringFirstName = String.Empty;
        string StringLastName = String.Empty;
        string StringLoginName = String.Empty;
        string StringPassword = String.Empty;
        string StringConfirmPassword = String.Empty;
        string StringAddress1 = String.Empty;
        string StringAddress2 = String.Empty;
        string StringCity = String.Empty;
        string StringState = String.Empty;
        string StringZip = String.Empty;
        string StringLandlineNo = String.Empty;
        string StringMobileNo = String.Empty;
        string StringEmail = String.Empty;
        string status = "N";
        int UserID;
        int HSRP_StateID;
        int RTOLocationID;
        int UserType;
        int intHSRP_StateID;
        int intRTOLocationID;
        String StringMode = String.Empty;
        string DefaultPage = "~/LiveReports/LiveTracking.aspx";
        string CnnString = string.Empty;
        string SQLString = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            Utils.GZipEncodePage();
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {
                int.TryParse(Session["UserType"].ToString(), out UserType);
                int.TryParse(Session["UserHSRPStateID"].ToString(), out HSRP_StateID);
                int.TryParse(Session["UserRTOLocationID"].ToString(), out RTOLocationID);
                lblErrMess.Text = string.Empty;
                lblSucMess.Text = string.Empty;
                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            }

            if (string.IsNullOrEmpty(Request.QueryString["Mode"].ToString()))
            {
                Response.Write("<script language='javascript'> {window.close();} </script>");
            }
            else
            {
                StringMode = Request.QueryString["Mode"].ToString();
            }

            if (StringMode.Equals("Edit"))
            {
                int.TryParse(Request.QueryString["UserID"].ToString(), out UserID);
                buttonUpdate.Visible = true;
                buttonSave.Visible = false;
            }
            else
            {
                buttonSave.Visible = true;
                buttonUpdate.Visible = false;
            }
            if (!Page.IsPostBack)
            {
                if (StringMode.Equals("Edit"))
                {
                    UpdateUserDetail(UserID);
                }
                if (StringMode.Equals("New"))
                {
                    bindUserType(UserType);
                    if (UserType.Equals(0))
                    {
                        FilldropDownListState();
                        FilldropDownListRTOLocation();
                        dropDownListRTOLocation.Enabled = true;
                    }
                    else if (UserType.Equals(1))
                    {
                        dropDownListState.Enabled = false;
                        FilldropDownListRTOLocation();
                        dropDownListRTOLocation.Enabled = true;
                    }
                    else
                    {
                        dropDownListState.Enabled = false;
                        dropDownListRTOLocation.Enabled = false;
                    }
                }
            }
        }

        #region DropDown

        private void bindUserType(int UserType)
        {

            if (UserType == 0)
            {
                dropDownListUserType.Items.Add(new ListItem("--Select User Type--", "--Select User Type--"));
                dropDownListUserType.Items.Add(new ListItem("Super Admin", "0"));
                dropDownListUserType.Items.Add(new ListItem("State Admin", "1"));
                dropDownListUserType.Items.Add(new ListItem("RTO Manager", "2"));
                dropDownListUserType.Items.Add(new ListItem("Supervisor", "3"));
                dropDownListUserType.Items.Add(new ListItem("Laser Code Assigner", "4"));
                dropDownListUserType.Items.Add(new ListItem("Embosser", "5"));
                dropDownListUserType.Items.Add(new ListItem("Store Manager", "6"));
                dropDownListUserType.Items.Add(new ListItem("Cashier", "7"));
                dropDownListUserType.Items.Add(new ListItem("Data Entry Opertor", "8"));

            }
            else if (UserType == 1)
            {
                dropDownListUserType.Items.Add(new ListItem("--Select User Type--", "--Select User Type--"));
                dropDownListUserType.Items.Add(new ListItem("RTO Manager", "2"));
                dropDownListUserType.Items.Add(new ListItem("Supervisor", "3"));
                dropDownListUserType.Items.Add(new ListItem("Laser Code Assigner", "4"));
                dropDownListUserType.Items.Add(new ListItem("Embosser", "5"));
                dropDownListUserType.Items.Add(new ListItem("Store Manager", "6"));
                dropDownListUserType.Items.Add(new ListItem("Cashier", "7"));
                dropDownListUserType.Items.Add(new ListItem("Data Entry Opertor", "8"));
            }
            else if (UserType == 2)
            {
                dropDownListUserType.Items.Add(new ListItem("--Select User Type--", "--Select User Type--"));
                dropDownListUserType.Items.Add(new ListItem("Supervisor", "3"));
                dropDownListUserType.Items.Add(new ListItem("Laser Code Assigner", "4"));
                dropDownListUserType.Items.Add(new ListItem("Embosser", "5"));
                dropDownListUserType.Items.Add(new ListItem("Store Manager", "6"));
                dropDownListUserType.Items.Add(new ListItem("Cashier", "7"));
                dropDownListUserType.Items.Add(new ListItem("Data Entry Opertor", "8"));
            }
            else if (UserType == 3)
            {
                dropDownListUserType.Items.Add(new ListItem("--Select User Type--", "--Select User Type--"));
                dropDownListUserType.Items.Add(new ListItem("Laser Code Assigner", "4"));
                dropDownListUserType.Items.Add(new ListItem("Embosser", "5"));
                dropDownListUserType.Items.Add(new ListItem("Store Manager", "6"));
                dropDownListUserType.Items.Add(new ListItem("Cashier", "7"));
            }
            
          //  FilldropDownListState();
        }

        private void FilldropDownListState()
        {
            if (UserType == 0)
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState Order By HSRPStateName";
            }
            else
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState where HSRP_StateID=" + HSRP_StateID + " Order By HSRPStateName";
            }
            Utils.PopulateDropDownList(dropDownListState, SQLString.ToString(), CnnString, "--Select State--");
            dropDownListState.SelectedIndex = 0;
        }

        private void FilldropDownListRTOLocation()
        {
            if (UserType.Equals(0))
            {
                int.TryParse(dropDownListState.SelectedValue, out intHSRP_StateID);
            }
            else
            {
                intHSRP_StateID = HSRP_StateID;
            }
            SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + intHSRP_StateID + " and ActiveStatus!='N' and LocationType='"+dropDownListLocationType.SelectedValue+"'  Order by RTOLocationName";
            Utils.PopulateDropDownList(dropDownListRTOLocation, SQLString, CnnString, "--Select Location--");
            dropDownListRTOLocation.SelectedIndex = 0;
        }

       protected void dropDownListLocationType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (StringMode=="Edit")
            {
                return;
            }
            if (dropDownListState.SelectedItem.Text != "--Select Location Type--")
            {
                dropDownListLocationType.Enabled = true;
                FilldropDownListRTOLocation();
                UpdatePanelLocation.Update();
            }
            else
            {
                dropDownListRTOLocation.Enabled = false;
                UpdatePanelLocation.Update();
                lblErrMess.Text = string.Empty;
                lblErrMess.Text = "Please select Location Type.";
            }
        }


        #endregion

        private void UpdateUserDetail(int UserID)
        {
            textBoxPassword.Enabled = false;
            textBoxLoginName.ReadOnly = true;
            textBoxConfirmPassword.Enabled = false;
            
            SQLString = "Select * From Users Where UserID=" + UserID.ToString();

            Utils dbLink = new Utils();
            dbLink.strProvider = CnnString;
            dbLink.CommandTimeOut = 600;
            dbLink.sqlText = SQLString.ToString();
            SqlDataReader PReader = dbLink.GetReader();
            while (PReader.Read())
            {
                dropDownListState.Enabled = false;
                FilldropDownListState();
                dropDownListLocationType.SelectedValue = PReader["LocationType"].ToString();
                dropDownListState.SelectedValue = PReader["HSRP_StateID"].ToString();
                dropDownListRTOLocation.Enabled = false;
                FilldropDownListRTOLocation();
                dropDownListRTOLocation.SelectedValue = PReader["RTOLocationID"].ToString();
                textBoxFirstName.Text = PReader["UserFirstName"].ToString();
                textBoxLastName.Text = PReader["UserLastName"].ToString();
                textBoxLoginName.Text = PReader["UserLoginName"].ToString();
                textBoxPassword.Text = PReader["Password"].ToString();
                textBoxConfirmPassword.Text = PReader["Password"].ToString();
                textBoxAddress1.Text = PReader["Address1"].ToString();
                textBoxAddress2.Text = PReader["Address2"].ToString();
                textBoxCity.Text = PReader["City"].ToString();
                textBoxState.Text = PReader["State"].ToString();
                if (PReader["Zip"].ToString().Equals("0"))
                {
                    textBoxZip.Text = String.Empty;
                }
                else
                {
                    textBoxZip.Text = PReader["Zip"].ToString();
                }
                textBoxEmail.Text = PReader["EmailID"].ToString();
                textBoxMobileNo.Text = PReader["MobileNo"].ToString();
                textBoxLandlineNo.Text = PReader["ContactNo"].ToString();

                if (PReader["ActiveStatus"].ToString() == "Y")
                {
                    checkBoxStatus.Checked = true;
                }
                else
                {
                    checkBoxStatus.Checked = false;
                }
                int UserTypeFromDB = Convert.ToInt32(PReader["UserType"].ToString());
                bindUserType(UserType);
                dropDownListUserType.SelectedValue = UserTypeFromDB.ToString();
            }
            PReader.Close();
            dbLink.CloseConnection();
        }

        protected void buttonUpdate_Click(object sender, EventArgs e)
        {

            StringAddress1 = textBoxAddress1.Text.Trim().Replace("'", "''").ToString();
            StringAddress2 = textBoxAddress2.Text.Trim().Replace("'", "''").ToString();
            StringCity = textBoxCity.Text.Trim().Replace("'", "''").ToString();
            StringFirstName = textBoxFirstName.Text.Trim().Replace("'", "''").ToString();
            StringLastName = textBoxLastName.Text.Trim().Replace("'", "''").ToString();
            StringLoginName = textBoxLoginName.Text.Trim().Replace("'", "''").ToString();
            StringPassword = textBoxPassword.Text.Trim().Replace("'", "''").ToString();
            StringEmail = textBoxEmail.Text.Trim().Replace("'", "''").ToString();
            StringMobileNo = textBoxMobileNo.Text.Trim().Replace("'", "''").ToString();
            StringState = textBoxState.Text.Trim().Replace("'", "''").ToString();
            StringZip = textBoxZip.Text.Trim().Replace("'", "''").ToString();
            if (Utils.IsInteger(StringZip).Equals(false))
            {
                StringZip = "0";
            }
            StringLandlineNo = textBoxLandlineNo.Text.Trim().Replace("'", "''").ToString();
            if (checkBoxStatus.Checked == true)
            {
                status = "Y";
            }



            if (string.IsNullOrEmpty(StringFirstName))
            {
                lblErrMess.Text = String.Empty;
                lblErrMess.Text = "Please Provide First Name.";
                return;
            }
            if (string.IsNullOrEmpty(StringLastName))
            {
                lblErrMess.Text = String.Empty;
                lblErrMess.Text = "Please Provide Last Name.";
                return;
            }
            
            if (string.IsNullOrEmpty(StringMobileNo))
            {
                lblErrMess.Text = String.Empty;
                lblErrMess.Text = "Please Provide Mobile Number.";
                return;
            }
            if (string.IsNullOrEmpty(StringAddress1))
            {
                lblErrMess.Text = String.Empty;
                lblErrMess.Text = "Please Provide Address.";
                return;
            }
            if (string.IsNullOrEmpty(StringEmail))
            {
                lblErrMess.Text = String.Empty;
                lblErrMess.Text = "Please Provide Email.";
                return;
            }
            if (string.IsNullOrEmpty(StringCity))
            {
                lblErrMess.Text = String.Empty;
                lblErrMess.Text = "Please Provide City.";
                return;
            }
            if (string.IsNullOrEmpty(StringState))
            {
                lblErrMess.Text = String.Empty;
                lblErrMess.Text = "Please Provide State.";
                return;
            }
           


            UserType = Convert.ToInt32(dropDownListUserType.SelectedValue);
            if (checkBoxStatus.Checked == true)
            {
                status = "Y";
            }

            //if (UserType.Equals(3))
            //{
            //    DefaultPage = "~/LiveReports/GroupLiveTracking.aspx";
            //}

            SQLString = "Update Users Set LocationType='" + dropDownListLocationType.SelectedValue + "', UserType=" + UserType + ",UserFirstName='" + StringFirstName + "',UserLastName='" + StringLastName + "',Address1='" + StringAddress1 + "',Address2='" + StringAddress2 + "',City='" + StringCity + "',State='" + StringState + "',Zip='" + StringZip + "',EmailID='" + StringEmail + "',MobileNo='" + StringMobileNo + "',ContactNo='" + StringLandlineNo + "',ActiveStatus='" + status + "',DefaultPage='" + DefaultPage + "' where UserID=" + UserID + " ";
            if (Utils.ExecNonQuery(SQLString, CnnString) <= 0)
            {
                lblErrMess.Text = "User Not Successfully Updated.";
            }
            else
            {
                lblSucMess.Text = "User is Successfully Updated.";
            }


        }

        protected void buttonSave_Click(object sender, EventArgs e)
        {
            StringAddress1 = textBoxAddress1.Text.Trim().Replace("'", "''").ToString();
            StringAddress2 = textBoxAddress2.Text.Trim().Replace("'", "''").ToString();
            StringCity = textBoxCity.Text.Trim().Replace("'", "''").ToString();
            StringFirstName = textBoxFirstName.Text.Trim().Replace("'", "''").ToString();
            StringLastName = textBoxLastName.Text.Trim().Replace("'", "''").ToString();
            StringPassword = textBoxPassword.Text.Trim().Replace("'", "''").ToString();
            StringLoginName = textBoxLoginName.Text.Trim().Replace("'", "''").ToString();
            StringEmail = textBoxEmail.Text.Trim().Replace("'", "''").ToString();
            StringMobileNo = textBoxMobileNo.Text.Trim().Replace("'", "''").ToString();
            StringState = textBoxState.Text.Trim().Replace("'", "''").ToString();
            StringZip = textBoxZip.Text.Trim().Replace("'", "''").ToString();
            if (Utils.IsInteger(StringZip).Equals(false))
            {
                StringZip = "0";
            }
            StringLandlineNo = textBoxLandlineNo.Text.Trim().Replace("'", "''").ToString();
            if (checkBoxStatus.Checked == true)
            {
                status = "Y";
            }


            if (UserType.Equals(0))
            {
                int.TryParse(dropDownListState.SelectedValue, out intHSRP_StateID);
                int.TryParse(dropDownListRTOLocation.SelectedValue, out intRTOLocationID);
                if (String.IsNullOrEmpty(dropDownListState.SelectedValue) || dropDownListState.SelectedValue.Equals("--Select Organization--"))
                {
                    lblErrMess.Text = String.Empty;
                    lblErrMess.Text = "Please Select organization.";
                    return;
                }
                if (String.IsNullOrEmpty(dropDownListRTOLocation.SelectedValue) || dropDownListRTOLocation.SelectedValue.Equals("--Select Client--"))
                {
                    lblErrMess.Text = String.Empty;
                    lblErrMess.Text = "Please Select Client.";
                    return;
                }
            }
            else if (UserType.Equals(1))
            {
                intHSRP_StateID = HSRP_StateID;
                int.TryParse(dropDownListRTOLocation.SelectedValue, out intRTOLocationID);
                if (String.IsNullOrEmpty(dropDownListRTOLocation.SelectedValue) || dropDownListRTOLocation.SelectedValue.Equals("--Select Client--"))
                {
                    lblErrMess.Text = String.Empty;
                    lblErrMess.Text = "Please Select Client.";
                    return;
                }
            }
            else
            {
                intHSRP_StateID = HSRP_StateID;
                intRTOLocationID = RTOLocationID;
            }

            if (string.IsNullOrEmpty(StringFirstName))
            {
                lblErrMess.Text = String.Empty;
                lblErrMess.Text = "Please Provide First Name.";
                return;
            }
            if (string.IsNullOrEmpty(StringLastName))
            {
                lblErrMess.Text = String.Empty;
                lblErrMess.Text = "Please Provide Last Name.";
                return;
            }
            if (string.IsNullOrEmpty(StringLoginName))
            {
                lblErrMess.Text = String.Empty;
                lblErrMess.Text = "Please Provide Login Name.";
                return;
            }
            if (string.IsNullOrEmpty(StringPassword))
            {
                lblErrMess.Text = String.Empty;
                lblErrMess.Text = "Please Provide Password.";
                return;
            }
            if (string.IsNullOrEmpty(StringMobileNo))
            {
                lblErrMess.Text = String.Empty;
                lblErrMess.Text = "Please Provide Mobile Number.";
                return;
            }
            if (string.IsNullOrEmpty(StringAddress1))
            {
                lblErrMess.Text = String.Empty;
                lblErrMess.Text = "Please Provide Address.";
                return;
            }
            if (string.IsNullOrEmpty(StringEmail))
            {
                lblErrMess.Text = String.Empty;
                lblErrMess.Text = "Please Provide Email.";
                return;
            }
            if (string.IsNullOrEmpty(StringCity))
            {
                lblErrMess.Text = String.Empty;
                lblErrMess.Text = "Please Provide City.";
                return;
            }
            if (string.IsNullOrEmpty(StringState))
            {
                lblErrMess.Text = String.Empty;
                lblErrMess.Text = "Please Provide State.";
                return;
            }
            if (string.IsNullOrEmpty(StringZip))
            {
                lblErrMess.Text = String.Empty;
                lblErrMess.Text = "Please Provide Zip.";
                return;
            }

            int.TryParse(dropDownListUserType.SelectedValue, out UserType);
            SQLString = "Select Count(*) From dbo.Users Where UserLoginName='" + StringLoginName + "'";
            if (Utils.getScalarCount(SQLString, CnnString) > 0)
            {
                lblErrMess.Text = "User already exists.";
                textBoxLoginName.Text = string.Empty;
                textBoxLoginName.Focus();
                return;
            }

            if (UserType.Equals(3))
            {
                DefaultPage = "~/LiveReports/GroupLiveTracking.aspx";
            }

            SQLString = "insert into dbo.Users (HSRP_StateID,RTOLocationID,LocationType,UserType,UserFirstName,UserLastName,UserLoginName,[Password],Address1,Address2,City,[State],Zip,EmailID,MobileNo,ContactNo,ActiveStatus,FirstLoginStatus,DefaultPage) values (" + intHSRP_StateID + "," + intRTOLocationID + ",'"+dropDownListLocationType.SelectedValue+"'," + UserType + ",'" + StringFirstName + "','" + StringLastName + "','" + StringLoginName + "','" + StringPassword + "','" + StringAddress1 + "','" + StringAddress2 + "','" + StringCity + "','" + StringState + "','" + StringZip + "','" + StringEmail + "','" + StringMobileNo + "','" + StringLandlineNo + "','" + status + "','Y','" + DefaultPage + "')";
            if (Utils.ExecNonQuery(SQLString, CnnString) <= 0)
            {

               
                lblErrMess.Text = "User Sucessfully Not Added.";
            }
            else
            {
                UserRTOlocationMapping();
                lblSucMess.Text = "User Created.";
            }

            int UID;
            //SQLString = "Select UserID From Users Where UserLoginName= '" + StringLoginName + "'";
            //DataTable dt = Utils.GetDataTable(SQLString, CnnString);
            //int.TryParse(dt.Rows[0]["UserID"].ToString(), out UID);
            //StringBuilder sb = new StringBuilder();
            //sb.Append("INSERT INTO [UserMenuRelation] ([UserID],[MenuRelationID])VALUES(" + UID + ",4)");
            //sb.Append("INSERT INTO [UserMenuRelation] ([UserID],[MenuRelationID])VALUES(" + UID + ",19);");
            //sb.Append("INSERT INTO [UserMenuRelation] ([UserID],[MenuRelationID])VALUES(" + UID + ",20);");
            //sb.Append("INSERT INTO [UserMenuRelation] ([UserID],[MenuRelationID])VALUES(" + UID + ",21);");
            //sb.Append("INSERT INTO [UserMenuRelation] ([UserID],[MenuRelationID])VALUES(" + UID + ",44);");
            //sb.Append("INSERT INTO [UserMenuRelation] ([UserID],[MenuRelationID])VALUES(" + UID + ",1);");
            //sb.Append("INSERT INTO [UserMenuRelation] ([UserID],[MenuRelationID])VALUES(" + UID + ",6);");
            //sb.Append("INSERT INTO [UserMenuRelation] ([UserID],[MenuRelationID])VALUES(" + UID + ",3);");
            //sb.Append("INSERT INTO [UserMenuRelation] ([UserID],[MenuRelationID])VALUES(" + UID + ",16);");
            //sb.Append("INSERT INTO [UserMenuRelation] ([UserID],[MenuRelationID])VALUES(" + UID + ",23);");
            //sb.Append("INSERT INTO [UserMenuRelation] ([UserID],[MenuRelationID])VALUES(" + UID + ",24);");
            //sb.Append("INSERT INTO [UserMenuRelation] ([UserID],[MenuRelationID])VALUES(" + UID + ",25);");
            //sb.Append("INSERT INTO [UserMenuRelation] ([UserID],[MenuRelationID])VALUES(" + UID + ",30);");
            //sb.Append("INSERT INTO [UserMenuRelation] ([UserID],[MenuRelationID])VALUES(" + UID + ",26);");
            //sb.Append("INSERT INTO [UserMenuRelation] ([UserID],[MenuRelationID])VALUES(" + UID + ",43);");
            //sb.Append("INSERT INTO [UserMenuRelation] ([UserID],[MenuRelationID])VALUES(" + UID + ",48);");
            //sb.Append("INSERT INTO [UserMenuRelation] ([UserID],[MenuRelationID])VALUES(" + UID + ",5);");
            //sb.Append("INSERT INTO [UserMenuRelation] ([UserID],[MenuRelationID])VALUES(" + UID + ",18)");

            //if (Utils.ExecNonQuery(sb.ToString(), CnnString) > 0)
            //{
            //    lblSucMess.Text = lblSucMess.Text + " And Menu Successfully Added";
            //}
            //else
            //{
            //    lblSucMess.Text = lblSucMess.Text + " And Menu Not Added";
            //}

        }

        int UserID1;
        public void UserRTOlocationMapping()
        {

            string query = "select * from Users where UserFirstName='" + textBoxFirstName.Text + "' and UserLoginName='" + textBoxLoginName.Text + "' and RTOLocationID='" + dropDownListRTOLocation.SelectedValue.ToString() + "'";
            DataTable dt = Utils.GetDataTable(query, CnnString);
           
            if (dt.Rows.Count >0)
            {
                UserID1 = int.Parse(dt.Rows[0]["UserID"].ToString());
               // string mm = Request.QueryString["UserID"].ToString();
            }

           // List<string> lst = new List<string>();
           // lst.Add(UserID.ToString());
           // lst.Add(dropDownListRTOLocation.SelectedValue.ToString());
           //// lst.Add();

            int user = 2;
            SQLString = "INSERT INTO UserRTOLocationMapping (UserID,RTOLocationID,CreatedBy) VALUES('" + UserID1 + "','" + dropDownListRTOLocation.SelectedValue.ToString() + "','" + user.ToString() + "')";
            if (Utils.ExecNonQuery(SQLString, CnnString) <= 0)
            {
                lblErrMess.Text = "User Sucessfully Not Added.";
            }
            else
            {
                lblSucMess.Text = "User Created.";
            }
        }
        

    }
}