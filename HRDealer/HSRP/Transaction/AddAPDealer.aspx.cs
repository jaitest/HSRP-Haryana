using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using DataProvider;
using System.Net.Mail;
using System.Net;
using System.IO;
using System.Text;

namespace HSRP.Master
{
    public partial class AddAPDealer : System.Web.UI.Page
    {
        int intHSRPStateID = 0;
        string SQLString1 = string.Empty;
        string StringDealerName = String.Empty;
        string StringDealerCode = String.Empty;
        string StringPersonName = String.Empty;
        string StringMobileNo = String.Empty;
        string StringAddress = String.Empty;
        string StringCity = String.Empty;
        string StringState = String.Empty;
        string StringAreaDealer = String.Empty;
        string StringrtolocationId = String.Empty;
        string ActiveStatus = "0";

        string StringcheckBoxTwoWheeler = "0";
        string StringcheckBoxFourWheeler = "0";
        string StringcheckBoxCommercialVehicle = "0";


        string StringcheckBoxTwoWheeler1 = "0";
        string StringcheckBoxFourWheeler1 = "0";
        string StringcheckBoxCommercialVehicle1 = "0";

        string Dealercode = string.Empty;
        string StringChargingType = "OurCenter";

        int DealerID;

        String StringMode = String.Empty;
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string HsrpStateId = string.Empty;
        int UserType;
        protected void Page_Load(object sender, EventArgs e)
        {
            HsrpStateId = Session["UserHSRPStateID"].ToString();
            UserType = Convert.ToInt32(Session["UserType"]);
            Utils.GZipEncodePage();
            lblSucMess.Text = "";
            lblErrMess.Text = "";


            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }

            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            StringMode = "New"; //Request["Mode"].ToString();
            if (StringMode.Equals("Edit"))
            {
                int.TryParse(Request.QueryString["DealerID"].ToString(), out DealerID);
                //buttonUpdate.Visible = true;
                buttonSave.Visible = false;
            }
            else
            {
                buttonSave.Visible = true;
                //buttonUpdate.Visible = false;
            }

            if (!Page.IsPostBack)
            {
                if (HsrpStateId == "4")
                {
                   
                }
                BL.blEntryDetail bl = new BL.blEntryDetail();
                bl.DailyEnityDeatail(Session["UID"].ToString(), "Dealer.aspx");

                ContactPerson();
               

                if (StringMode.Equals("Edit"))
                {
                    UpdateUserDetail(DealerID);
                }

                textBoxMobileNo.Attributes.Add("onkeypress", "return isNumberKey(event);");
                FilldropDownListRTOLocation();
                FilldropDownListState();
                FilldropDownListDealer();
            }


        }



        private void UpdateUserDetail(int DealerID)
        {
            SQLString = "Select * From DealerMaster Where DealerID=" + DealerID.ToString();            

            Utils dbLink = new Utils();
            dbLink.strProvider = CnnString;
            dbLink.CommandTimeOut = 600;
            dbLink.sqlText = SQLString.ToString();
            SqlDataReader PReader = dbLink.GetReader();
            while (PReader.Read())
            {

                ddlDealermaster.SelectedValue = PReader["DealerID"].ToString();
               
                textBoxAddress.Text = PReader["Address"].ToString();
                textBoxCity.Text = PReader["City"].ToString();
                DropDownListStateName.SelectedValue = PReader["hsrp_stateid"].ToString();
                if (PReader["rtolocationid"].ToString() != "")
                    DropdownRTOName.SelectedValue = PReader["rtolocationid"].ToString();
                textBoxPersonName.Text = PReader["ContactPerson"].ToString();
                textBoxMobileNo.Text = PReader["ContactMobileNo"].ToString();
               
                if (PReader["IsDealingInTwoWheeler"].ToString().Equals("True"))
                {
                    checkBoxTwoWheeler.Checked = true;
                }
                if (PReader["IsDealingInFourWheeler"].ToString().Equals("True"))
                {
                    checkBoxFourWheeler.Checked = true;
                }
                if (PReader["IsDealingInCommercial"].ToString().Equals("True"))
                {
                    checkBoxCommercialVehicle.Checked = true;
                }
                if (PReader["ChargingType"].ToString().Equals("ShowRoom"))
                {
                    radioButtonShowRoom.Checked = true;
                }
                else
                {
                    radioButtonCenter.Checked = true;
                }
                if (PReader["IsActive"].ToString().Equals("True"))
                {
                    checkBoxStatus.Checked = true;
                }
            }
            PReader.Close();
            dbLink.CloseConnection();
        }

        public void ContactPerson()
        {
            //    string contactPer = "select contactPersionID,ContactPersionName from ContactPersion"; 
            //    Utils.PopulateDropDownList(ddlSalesPerson, contactPer.ToString(), CnnString, "--Contact Person Name--"); 
        }

        protected void buttonUpdate_Click(object sender, EventArgs e)
        {

            StringDealerName = ddlDealermaster.SelectedItem.ToString();
           
            StringPersonName = textBoxPersonName.Text.Trim().Replace("'", "''").ToString();
            StringMobileNo = textBoxMobileNo.Text.Trim().Replace("'", "''").ToString();
            StringAddress = textBoxAddress.Text.Trim().Replace("'", "''").ToString();
            StringCity = textBoxCity.Text.Trim().Replace("'", "''").ToString();
            StringState = DropDownListStateName.SelectedItem.ToString();
            StringAreaDealer = "HR";// textBoxDealerArea.Text.Trim().Replace("'", "''").ToString();
      

            if (string.IsNullOrEmpty(StringDealerName))
            {
                lblErrMess.Text = String.Empty;
                lblErrMess.Text = "Please Provide Dealer Name.";
                return;
            }
          
            if (string.IsNullOrEmpty(StringPersonName))
            {
                lblErrMess.Text = String.Empty;
                lblErrMess.Text = "Please Provide Contact Person Name.";
                return;
            }
            if (string.IsNullOrEmpty(txtDealerloginid.Text.ToString()))
            {
                lblErrMess.Text = String.Empty;
                lblErrMess.Text = "Please Enter Dealer Login Id";
                return;
            }
            if (string.IsNullOrEmpty(txtEmailid.Text.ToString()))
            {
                lblErrMess.Text = String.Empty;
                lblErrMess.Text = "Please Provide Email-Id.";
                return;
            }

            if (string.IsNullOrEmpty(StringMobileNo))
            {
                lblErrMess.Text = String.Empty;
                lblErrMess.Text = "Please Provide Contact Person Mobile Number.";
                return;
            }
            if (string.IsNullOrEmpty(StringAddress))
            {
                lblErrMess.Text = String.Empty;
                lblErrMess.Text = "Please Provide Address.";
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
            if (StringState.Equals("--Select State--"))
            {
                lblErrMess.Text = String.Empty;
                lblErrMess.Text = "Please Provide State.";
                return;
            }
            if (string.IsNullOrEmpty(StringAreaDealer))
            {
                lblErrMess.Text = String.Empty;
                lblErrMess.Text = "Please Provide Dealer Area.";
                return;
            }

          
            SQLString = "Update DealerMaster Set DealerName='" + StringDealerName + "'," +
                "Address='" + StringAddress + "',City='" + StringCity + "',State='" + StringState + "',ContactPerson='" + StringPersonName + "'" +
            ",ContactMobileNo='" + StringMobileNo + "',AreaOfDealer='" + StringAreaDealer + "',IsDealingInTwoWheeler=" + StringcheckBoxTwoWheeler + "," +
            "IsDealingInFourWheeler=" + StringcheckBoxFourWheeler + ",IsDealingInCommercial=" + StringcheckBoxCommercialVehicle + "," +
            "ChargingType='" + StringChargingType + "',IsActive=" + ActiveStatus + " where DealerID=" + dealerid + " ";

            if (Utils.ExecNonQuery(SQLString, CnnString) > 0)
            {
                string Sqlstring = "Select * From Users where UserLoginName='" + txtDealerloginid.Text + "'";
                DataTable dt = Utils.GetDataTable(Sqlstring, CnnString);
                if (dt.Rows.Count > 0)
                {
                    lblSucMess.Text = string.Empty;
                    lblSucMess.Text = "Someone already has that username. Try another?.";
                    //lblUserNamePassword.Text = "Login User Name-'" + dt.Rows[0]["UserLoginName"] + "' \n Password-'" + dt.Rows[0]["Password"] + "'";
                }
                else
                {
                    string password = GetPassword();
                    string SQLString2 = "insert into dbo.Users (HSRP_StateID,RTOLocationID,LocationType,UserType,UserFirstName,UserLastName,UserLoginName,[Password],Address1,Address2,City,[State],Zip,EmailID,MobileNo,ContactNo,ActiveStatus,FirstLoginStatus,DefaultPage,dealerid,withoutMAC) values (" + DropDownListStateName.SelectedValue + "," + DropdownRTOName.SelectedValue + ",'Sub-Urban','8','" + ddlDealermaster.SelectedItem + "','','" + txtDealerloginid.Text + "','" + password + "','" + textBoxAddress.Text + "','','" + textBoxCity.Text + "','" + StringState + "','','" + txtEmailid.Text + "','" + StringMobileNo + "','','Y','Y','~/LiveReports/LiveTracking.aspx','" + dealerid + "','Y')";
                    if (Utils.ExecNonQuery(SQLString2, CnnString) > 0)
                    {
                        if (textBoxMobileNo.Text.Length > 0)
                        {
                            //string SMSText = "Dear Customer, Your account ID " + txtDealerloginid.Text + " has been successfully created. Your password is " + password + ". Kindly do not share your account details with anybody. Visit  www.hsrpap.com/hsrpap/login.aspx Thank You - LAPL";
                            //string sendURL = "http://103.16.101.52:8080/bulksms/bulksms?username=sse-aphsrp&password=aphsrp&type=0&dlr=1&destination=" + textBoxMobileNo.Text.ToString() + "&source=APHSRP&message=" + SMSText;

                            string SMSText = "Dear Customer, Your account ID " + txtDealerloginid.Text + " has been successfully created. Your password is " + password + ". Kindly do not share your account details with anybody.  Thank You -LURPL";
                             string sendURL = "http://103.16.101.52:8080/bulksms/bulksms?username=sse-hsrphr&password=hsrphr&type=0&dlr=1&destination=" + textBoxMobileNo.Text.ToString() + "&source=HRHSRP&message=" + SMSText;
                          
                            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(sendURL);
                            myRequest.Method = "GET";
                            WebResponse myResponse = myRequest.GetResponse();
                            StreamReader sr = new StreamReader(myResponse.GetResponseStream(), System.Text.Encoding.UTF8);
                            string result = sr.ReadToEnd();
                            sr.Close();
                            myResponse.Close();
                            System.Threading.Thread.Sleep(350);
                            Utils.ExecNonQuery("insert into HRSMSDetail(RtoLocationID,MobileNo,SentResponseCode,smstext) values('" + DropdownRTOName.SelectedValue + "'," + textBoxMobileNo.Text.ToString() + ",'" + result + "','" + SMSText + "')", CnnString);
                        
                          //  SendMail(ddlDealermaster.SelectedItem.ToString(), txtDealerloginid.Text, password, txtEmailid.Text);
                        }
                        lblUserNamePassword.Text = "Login User Name-" + txtDealerloginid.Text + " \n Password-" + password + "";
                        lblSucMess.Text = string.Empty;
                        lblSucMess.Text = "User Created As Per Your Dealer.";
                        Refresh();
                        FilldropDownListDealer();
                    }
                }
            }
            else
            {
                lblErrMess.Text = string.Empty;
                lblErrMess.Text = "User not Created.";
            }
        }


      

        private void FilldropDownListState()
        {         
                SQLString1 = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID=" + HsrpStateId + " and ActiveStatus='Y' Order by HSRPStateName";
                DataSet dts = Utils.getDataSet(SQLString1, CnnString);
                DropDownListStateName.DataSource = dts;
                DropDownListStateName.DataBind();
           
        }

        private void FilldropDownListRTOLocation()
        {
            
                SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + HsrpStateId + " and ActiveStatus!='N' Order by RTOLocationName";
                DataSet dss = Utils.getDataSet(SQLString, CnnString);
                DropdownRTOName.DataSource = dss;
                DropdownRTOName.DataBind();
                DropdownRTOName.Items.Insert(0, new ListItem("--Select RTO Location--", "--Select RTO Location--"));
           
        }

        private void FilldropDownListDealer()
        {
           
                SQLString = "Select Dealerid,dealername from dealermaster where HSRP_StateID='" + HsrpStateId + "' order by dealername";
                DataSet dss = Utils.getDataSet(SQLString, CnnString);
                ddlDealermaster.DataSource = dss;
                ddlDealermaster.DataBind();
                ddlDealermaster.Items.Insert(0, new ListItem("--Select Dealer Name--", "--Select Dealer Name--"));
           
        }

        protected void buttonSave_Click(object sender, EventArgs e)
        {
            StringDealerName = ddlDealermaster.Text.Trim().Replace("'", "''").ToString();
            //  StringDealerCode = textBoxDealerCode.Text.Trim().Replace("'", "''").ToString();
            StringPersonName = textBoxPersonName.Text.Trim().Replace("'", "''").ToString();
            StringMobileNo = textBoxMobileNo.Text.Trim().Replace("'", "''").ToString();
            StringAddress = textBoxAddress.Text.Trim().Replace("'", "''").ToString();
            StringCity = textBoxCity.Text.Trim().Replace("'", "''").ToString();
            StringState = DropDownListStateName.SelectedValue.Trim().Replace("'", "''").ToString();
            //StringAreaDealer = textBoxDealerArea.Text.Trim().Replace("'", "''").ToString();
            //txtERPClientCode.Text = txtERPClientCode.Text.Trim().Replace("'", "''").ToString();
            //txtERPEmbossingCode.Text = txtERPEmbossingCode.Text.Trim().Replace("'", "''").ToString();
            StringrtolocationId = DropdownRTOName.SelectedValue.Trim().Replace("'", "''").ToString();



            SQLString = "Select Top(1) * from DealerMaster where DealerCode='" + StringDealerCode + "'";
            if (Utils.ExecNonQuery(SQLString, CnnString) > 0)
            {
                lblErrMess.Text = string.Empty;
                lblErrMess.Text = "Dealer code already exists.";
                return;
            }
            if (checkBoxTwoWheeler.Checked)
            {
                StringcheckBoxTwoWheeler = "1";
            }
            if (checkBoxFourWheeler.Checked)
            {
                StringcheckBoxFourWheeler = "1";
            }
            if (checkBoxCommercialVehicle.Checked)
            {
                StringcheckBoxCommercialVehicle = "1";
            }


            if (checkBoxTwoWheeler1.Checked)
            {
                StringcheckBoxTwoWheeler1 = "1";
            }
            if (checkBoxFourWheeler1.Checked)
            {
                StringcheckBoxFourWheeler1 = "1";
            }
            if (checkBoxCommercialVehicle1.Checked)
            {
                StringcheckBoxCommercialVehicle1 = "1";
            }

            if (radioButtonShowRoom.Checked == true)
            {
                if (StringcheckBoxCommercialVehicle1 == "0" && StringcheckBoxFourWheeler1 == "0" && StringcheckBoxTwoWheeler1 == "0")
                {
                    lblErrMess.Text = String.Empty;
                    lblErrMess.Text = "Please Provide Dealer Deals in which category.";
                    return;
                }
            }

            if (radioButtonCenter.Checked == true)
            {
                if (StringcheckBoxCommercialVehicle == "0" && StringcheckBoxFourWheeler == "0" && StringcheckBoxTwoWheeler == "0")
                {
                    lblErrMess.Text = String.Empty;
                    lblErrMess.Text = "Please Provide Dealer Deals in which category.";
                    return;
                }
            }

            if (radioButtonShowRoom.Checked)
            {
                StringChargingType = "ShowRoom";
            }
            if (radioButtonCenter.Checked)
            {
                StringChargingType = "OurCenter";
            }
            if (radioButtonShowRoom.Checked == false && radioButtonCenter.Checked == false)
            {
                lblErrMess.Text = String.Empty;
                lblErrMess.Text = "Please Provide plate affixation place.";
                return;
            }

            if (checkBoxStatus.Checked)
            {
                ActiveStatus = "1";
            }

            if (string.IsNullOrEmpty(StringDealerName))
            {
                lblErrMess.Text = String.Empty;
                lblErrMess.Text = "Please Provide Dealer Name.";
                return;
            }
            if (string.IsNullOrEmpty(StringrtolocationId))
            {
                lblErrMess.Text = String.Empty;
                lblErrMess.Text = "Please Select Rto Location.";
                return;
            }
            
            if (string.IsNullOrEmpty(StringPersonName))
            {
                lblErrMess.Text = String.Empty;
                lblErrMess.Text = "Please Provide Contact Person Name.";
                return;
            }
           

            if (string.IsNullOrEmpty(StringMobileNo))
            {
                lblErrMess.Text = String.Empty;
                lblErrMess.Text = "Please Provide Contact Person Mobile Number.";
                return;
            }
            if (string.IsNullOrEmpty(StringAddress))
            {
                lblErrMess.Text = String.Empty;
                lblErrMess.Text = "Please Provide Address.";
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
            if (StringState.Equals("--Select State--"))
            {
                lblErrMess.Text = String.Empty;
                lblErrMess.Text = "Please Provide State.";
                return;
            }
                        

            if (string.IsNullOrEmpty(StringAreaDealer))
             {
                    lblErrMess.Text = String.Empty;
                    lblErrMess.Text = "Please Provide Dealer Area.";
                    return;
               }
            
            if (radioButtonCenter.Equals(false) && radioButtonShowRoom.Equals(false))
            {
                lblErrMess.Text = String.Empty;
                lblErrMess.Text = "Please Provide Plate Fixation Center.";
                return;

            }

            if (checkBoxTwoWheeler.Checked == true)
            {
                if (txtTwoWheelerRate.Text == "")
                {
                    lblErrMess.Text = String.Empty;
                    lblErrMess.Text = "Please Provide Tow Wheeler Rate.";
                    return;
                }
            }

            if (checkBoxTwoWheeler1.Checked == true)
            {
                if (txtTwoWheelerRate1.Text == "")
                {
                    lblErrMess.Text = String.Empty;
                    lblErrMess.Text = "Please Provide Tow Wheeler Rate.";
                    return;
                }
            }

            if (checkBoxFourWheeler1.Checked == true)
            {
                if (txtLMVUpTo10Lac1.Text == "" || txtLMV10To25Lac1.Text == "" || txtLMV25To50Lac1.Text == "" || txtLMVMoreThan50Lac1.Text == "" || txtLMVClassUpTo10Lac1.Text == "" || txtLMVClass10To25Lac1.Text == "" || txtLMVClass25To50Lac1.Text == "" || txtLMVClassMoreThan50Lac1.Text == "")
                {
                    lblErrMess.Text = String.Empty;
                    lblErrMess.Text = "Please Provide Four Wheeler Rate.";
                    return;
                }
            }

            if (checkBoxFourWheeler.Checked == true)
            {
                if (txtLMVUpTo10Lac.Text == "" || txtLMV10To25Lac.Text == "" || txtLMV25To50Lac.Text == "" || txtLMVMoreThan50Lac.Text == "" || txtLMVClassUpTo10Lac.Text == "" || txtLMVClass10To25Lac.Text == "" || txtLMVClass25To50Lac.Text == "" || txtLMVClassMoreThan50Lac.Text == "")
                {
                    lblErrMess.Text = String.Empty;
                    lblErrMess.Text = "Please Provide Four Wheeler Rate.";
                    return;
                }
            }

            if (checkBoxCommercialVehicle1.Checked == true)
            {
                if (txtLMV1.Text == "" || txtLMVClass1.Text == "" || txtAuto1.Text == "" || txtLCV1.Text == "" || txtMVC1.Text == "" || txtHCV1.Text == "")
                {
                    lblErrMess.Text = String.Empty;
                    lblErrMess.Text = "Please Provide Commercial Vehicle Rate.";
                    return;
                }
            }

            if (checkBoxCommercialVehicle.Checked == true)
            {
                if (txtLMV.Text == "" || txtLMVClass.Text == "" || txtAuto.Text == "" || txtLCV.Text == "" || txtMVC.Text == "" || txtHCV.Text == "")
                {
                    lblErrMess.Text = String.Empty;
                    lblErrMess.Text = "Please Provide Commercial Vehicle Rate.";
                    return;
                }
            }
            string contactPersionID = string.Empty;
          

            string userID = Session["UID"].ToString();
           
            if (Utils.ExecNonQuery(SQLString, CnnString) > 0)
            {
                string SQLString2 = "insert into dbo.Users (HSRP_StateID,RTOLocationID,LocationType,UserType,UserFirstName,UserLastName,UserLoginName,[Password],Address1,Address2,City,[State],Zip,EmailID,MobileNo,ContactNo,ActiveStatus,FirstLoginStatus,DefaultPage) values (" + DropDownListStateName.SelectedValue + "," + DropdownRTOName.SelectedValue + ",'Sub-Urban','8','" + ddlDealermaster.SelectedItem + "','','" + ddlDealermaster.SelectedItem + "','1234','" + textBoxAddress.Text + "','','" + textBoxCity.Text + "','" + StringState + "','','','" + StringMobileNo + "','','Y','Y','~/LiveReports/LiveTracking.aspx')";
                Utils.ExecNonQuery(SQLString2, CnnString);

                SQLString = "SELECT  top 1 DealerId  FROM [DealerMaster]  order by DealerId desc";
                DataTable dt = Utils.GetDataTable(SQLString, CnnString);
                string id = dt.Rows[0]["DealerID"].ToString();

                Refresh();
            }
            else
            {
                lblErrMess.Text = "Record Not Added.";
            }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ViewDealer.aspx");
        }

        protected void checkBoxFourWheeler_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxFourWheeler.Checked == true)
            {
                FourWheeler.Visible = true;
                checkBoxCommercialVehicle.Checked = false;
                //checkBoxTwoWheeler.Checked = false;
                //Commercialvehicle.Visible = false;
                //TwoWheeler.Visible = false;
            }
            else
            {
                FourWheeler.Visible = false;
            }
        }

        protected void checkBoxCommercialVehicle_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxCommercialVehicle.Checked == true)
            {
                Commercialvehicle.Visible = true;
                // checkBoxFourWheeler.Checked = false;
                //checkBoxTwoWheeler.Checked = false;

                //FourWheeler.Visible = false;
                //TwoWheeler.Visible = false;
            }
            else
            {
                Commercialvehicle.Visible = false;
            }
        }

        protected void checkBoxTwoWheeler_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxTwoWheeler.Checked == true)
            {

                TwoWheeler.Visible = true;
                // checkBoxCommercialVehicle.Checked = false;
                //checkBoxFourWheeler.Checked = false;
                //Commercialvehicle.Visible = false;
                //FourWheeler.Visible = false;
            }
            else
            {
                TwoWheeler.Visible = false;
            }
        }

        protected void checkBoxTwoWheeler1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxTwoWheeler1.Checked == true)
            {

                TwoWheeler1.Visible = true;
                //checkBoxCommercialVehicle1.Checked = false;
                //checkBoxFourWheeler1.Checked = false;
                //Commercialvehicle1.Visible = false;
                //FourWheeler1.Visible = false;
            }
            else
            {
                TwoWheeler1.Visible = false;
            }
        }

        protected void checkBoxFourWheeler1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxFourWheeler1.Checked == true)
            {
                FourWheeler1.Visible = true;
                //checkBoxCommercialVehicle1.Checked = false;
                //checkBoxTwoWheeler1.Checked = false;
                //Commercialvehicle1.Visible = false;
                //TwoWheeler1.Visible = false;
            }
            else
            {
                FourWheeler1.Visible = false;
            }
        }

        protected void checkBoxCommercialVehicle1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxCommercialVehicle1.Checked == true)
            {
                Commercialvehicle1.Visible = true;
                //checkBoxFourWheeler1.Checked = false;
                //checkBoxTwoWheeler1.Checked = false;

                //FourWheeler1.Visible = false;
                //TwoWheeler1.Visible = false;
            }
            else
            {
                Commercialvehicle1.Visible = false;
            }
        }

        protected void radioButtonShowRoom_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonShowRoom.Checked == true)
            {

                radioButtonShowRoom1.Visible = true;
                radioButtonCenter1.Visible = true;
                radioButtonCenter.Checked = true;


            }
            else
            {
                radioButtonShowRoom1.Visible = false;
                //radioButtonCenter1.Visible = false;
            }
        }

        protected void radioButtonCenter_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonCenter.Checked == true)
            {
                radioButtonShowRoom1.Visible = false;
                radioButtonCenter1.Visible = true;
                radioButtonShowRoom.Checked = false;
            }
            else
            {
                radioButtonShowRoom1.Visible = false;
                radioButtonCenter1.Visible = false;
            }
        }

        public void Refresh()
        {
            textBoxAddress.Text = "";
            textBoxCity.Text = "";
            textBoxPersonName.Text = "";
            textBoxMobileNo.Text = "";
            DropDownListStateName.ClearSelection();
            ddlDealermaster.ClearSelection();
            DropdownRTOName.ClearSelection();
            textBoxPersonName.Text = "";
            textBoxMobileNo.Text = "";
            txtDealerloginid.Text = "";
        }

        protected void DropDownListStateName_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilldropDownListDealer();
            FilldropDownListRTOLocation();
        }
        public static string dealerid;
        protected void ddlDealermaster_SelectedIndexChanged(object sender, EventArgs e)
        {
            string Querygetdealer = "Select * from users where  dealerId='" + ddlDealermaster.SelectedValue + "'";
            DataTable dt = Utils.GetDataTable(Querygetdealer, CnnString);
            if (dt.Rows.Count > 0)
            {
                buttonSave.Visible = false;
                lblUserNamePassword.Visible = true;
                lblUserNamePassword.Text = "";
                lblUserNamePassword.Text = "User Already Created. Login User Name-" + dt.Rows[0]["UserLoginName"].ToString() + " \n Password-" + dt.Rows[0]["Password"].ToString() + "";
                return;
            }
            else
            {
                UpdateUserDetail(Convert.ToInt32(ddlDealermaster.SelectedValue));
                dealerid = ddlDealermaster.SelectedValue;
                lblUserNamePassword.Text = "";
            }

        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            Refresh();
            lblUserNamePassword.Text = "";
        }

        public string GetPassword()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(RandomString(4, true));
            builder.Append(RandomNumber(1000, 9999));
            builder.Append(RandomString(2, false));
            return builder.ToString();
        }
        private string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }
        private int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        public void SendMail(string Dealershipname, string username, string password, string dealerEmailid)
        {
            try
            {
                string ActivationUrl = String.Empty;
                //ActivationUrl = "www.hsrpap.com/hsrpap/login.aspx";
                ActivationUrl = "www.hsrpap.com/hsrphr/login.aspx";
                string ActivationUrl1 = String.Empty;
                //ActivationUrl1 = "online@hsrpap.com";
                ActivationUrl1 = "online@hsrphr.com";
                MailMessage msg = new MailMessage();
               // msg.From = new MailAddress("automailer@hsrpap.com", "HSRP-AP");
                msg.From = new MailAddress("automailer@hsrphr.com", "HSRP-HR");
                msg.To.Add(dealerEmailid);
                msg.Subject = "Confirmation email for activation the account";
                msg.Body = "Dear M/S <b>“" + Dealershipname + "”</b><br /><br />We have received your application for “Pay Direct-LURPL”. Your application has been successfully updated. Thank you for showing your interest in “Pay Direct-LURPL”.<br />Now that you’ve signed up you will receive updates via email and SMS.<br /><br /><br />User Id :-    " + username + "<br />Password :- " + password + "<br /><br />It is recommended that you please change your password, immediately after you have logged in to your account. Do not share password with anyone.<br />We would request you to please visit the below link and log in to your account.<br /><a href='" + ActivationUrl + "'>www.hsrpap.com/hsrpap/login.aspx</a><br /><br /> If you have any query or for further business enquiry please email us on <a href='" + ActivationUrl1 + "'>online@hsrpap.com</a><br /> Or contact Support Team on following numbers:-<br /><br />1.       +91-7306132943<br /><br />2.       +91-9396844878<br /><br />3.       +91-9100026696<br /><br />4.       +91-9100026691<br /><br /><br /><br /><br /><br /><br />Thanking You<br /><br />Team-LURPL";
                msg.IsBodyHtml = true;
                SmtpClient smtpServer = new SmtpClient();
               // smtpServer.Credentials = new NetworkCredential("automailer@hsrpap.com", "Link@1234");
                smtpServer.Credentials = new NetworkCredential("automailer@hsrphr.com", "Link@1234");
                smtpServer.Port = 587;
                smtpServer.Host = "mail.hsrphr.com";
                smtpServer.EnableSsl = false;
                smtpServer.Timeout = 100000;
                smtpServer.Send(msg);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}