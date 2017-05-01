using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

namespace HSRP.Dealer.Master
{
    public partial class Dealer : System.Web.UI.Page
    {
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
            StringMode = Request["Mode"].ToString();
            if (StringMode.Equals("Edit"))
            {
                int.TryParse(Request.QueryString["DealerID"].ToString(), out DealerID);
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
                if (HsrpStateId == "4")
                {
                    lblemb.Visible = true;
                    //ddlembossing.Visible = true;
                    dropDownListClient.Visible = true;
                }
                BL.blEntryDetail bl = new BL.blEntryDetail();
                bl.DailyEnityDeatail(Session["UID"].ToString(), "Dealer.aspx");
              
                ContactPerson();
                FilldropDownListClient();

                if (StringMode.Equals("Edit"))
                {
                    UpdateUserDetail(DealerID);
                }
               
                textBoxMobileNo.Attributes.Add("onkeypress", "return isNumberKey(event);");
                FilldropDownListRTOLocation();
            }

          
        }

     

        private void UpdateUserDetail(int DealerID)
        {
            SQLString = "Select * From DealerMaster Where DealerID=" + DealerID.ToString();
           // SQLString = " select dealerMaster.*,DealerPrice from DealerPrice inner join dealerMaster on DealerPrice.DealerId=dealerMaster.DealerId where DealerPrice.DealerId='" + DealerID.ToString() + "'";
           
            Utils dbLink = new Utils();
            dbLink.strProvider = CnnString;
            dbLink.CommandTimeOut = 600;
            dbLink.sqlText = SQLString.ToString();
            SqlDataReader PReader = dbLink.GetReader();
            while (PReader.Read())
            {

                textBoxDealerName.Text = PReader["DealerName"].ToString();
               // textBoxDealerCode.Text = PReader["DealerCode"].ToString();
                textBoxAddress.Text = PReader["Address"].ToString();
                textBoxCity.Text = PReader["City"].ToString();
                ddlState.SelectedValue = PReader["State"].ToString();
                textBoxPersonName.Text = PReader["ContactPerson"].ToString();
                textBoxMobileNo.Text = PReader["ContactMobileNo"].ToString();
                textBoxDealerArea.Text = PReader["AreaOfDealer"].ToString();
                txtERPClientCode.Text = PReader["erpclientcode"].ToString();
                txtERPEmbossingCode.Text = PReader["erpembcode"].ToString();
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

            StringDealerName = textBoxDealerName.Text.Trim().Replace("'", "''").ToString();
       //     StringDealerCode = textBoxDealerCode.Text.Trim().Replace("'", "''").ToString();
            StringPersonName = textBoxPersonName.Text.Trim().Replace("'", "''").ToString();
            StringMobileNo = textBoxMobileNo.Text.Trim().Replace("'", "''").ToString();
            StringAddress = textBoxAddress.Text.Trim().Replace("'", "''").ToString();
            StringCity = textBoxCity.Text.Trim().Replace("'", "''").ToString();
            StringState = ddlState.SelectedValue.Trim().Replace("'", "''").ToString();
            StringAreaDealer = textBoxDealerArea.Text.Trim().Replace("'", "''").ToString();
            txtERPClientCode.Text = txtERPClientCode.Text.Trim().Replace("'", "''").ToString();
            txtERPEmbossingCode.Text = txtERPEmbossingCode.Text.Trim().Replace("'", "''").ToString();

            SQLString = "Select Top(1) * from DealerMaster where DealerCode='" + StringDealerCode + "' and ID <>" + DealerID + "";
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
            if (StringcheckBoxCommercialVehicle == "0" && StringcheckBoxFourWheeler == "0" && StringcheckBoxTwoWheeler == "0")
            {
                lblErrMess.Text = String.Empty;
                lblErrMess.Text = "Please Provide Dealer Deals in which category.";
                return;
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
            //if (string.IsNullOrEmpty(StringDealerCode))
            //{
            //    lblErrMess.Text = String.Empty;
            //    lblErrMess.Text = "Please Provide Dealer Code.";
            //    return;
            //}
            if (string.IsNullOrEmpty(StringPersonName))
            {
                lblErrMess.Text = String.Empty;
                lblErrMess.Text = "Please Provide Contact Person Name.";
                return;
            }
            if (string.IsNullOrEmpty(txtERPClientCode.Text.ToString()))
            {
                lblErrMess.Text = String.Empty;
                lblErrMess.Text = "Please Provide ERP Client Code.";
                return;
            }
            if (string.IsNullOrEmpty(txtERPEmbossingCode.Text.ToString()))
            {
                lblErrMess.Text = String.Empty;
                lblErrMess.Text = "Please Provide ERP Embossing Code.";
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

            SQLString = "Update DealerMaster Set DealerName='" + StringDealerName + "'," +
                "Address='" + StringAddress + "',City='" + StringCity + "',State='" + StringState + "',ContactPerson='" + StringPersonName + "'" +
            ",ContactMobileNo='" + StringMobileNo + "',AreaOfDealer='" + StringAreaDealer + "',IsDealingInTwoWheeler=" + StringcheckBoxTwoWheeler + "," +
            "IsDealingInFourWheeler=" + StringcheckBoxFourWheeler + ",IsDealingInCommercial=" + StringcheckBoxCommercialVehicle + "," +
            "ChargingType='" + StringChargingType + "',erpclientcode='" + txtERPClientCode.Text.ToString().ToUpper() + "',erpembcode='" + txtERPEmbossingCode.Text.ToString().ToUpper() + "',IsActive=" + ActiveStatus + " where DealerID=" + DealerID + " ";
            if (Utils.ExecNonQuery(SQLString, CnnString) <= 0)
            {
                lblSucMess.Text = string.Empty;
                lblSucMess.Text = "Record Updated Sucessfully.";
            }
            else
            {
                lblErrMess.Text = string.Empty;
                lblErrMess.Text = "Record Not Updated.";
            }
        }


        private void FilldropDownListClient()
        {

            SQLString = "select distinct EmbCenterName,NAVEMBID from RTOLocation Where HSRP_StateID=" + HsrpStateId + " and navembid is not null  Order by EmbCenterName ";
            Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), CnnString, "--Select Embossing Center--");


        }

        private void FilldropDownListRTOLocation()
        {
            if (UserType.Equals(0))
            {
                //int.TryParse(ddlState.SelectedValue, out intHSRPStateID);
                SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where ActiveStatus!='N' Order by RTOLocationName";
                Utils.PopulateDropDownList(DropdownRTOName, SQLString.ToString(), CnnString, "--Select RTO Location--");
            }
            else
            {
                SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + HsrpStateId + " and ActiveStatus!='N' Order by RTOLocationName";
                //Utils.PopulateDropDownList(DropdownRTOName, SQLString.ToString(), CnnString, "--Select RTO Location--");
                DataSet dss = Utils.getDataSet(SQLString, CnnString);
                DropdownRTOName.DataSource = dss;
                DropdownRTOName.DataBind();
            }
        }

        protected void buttonSave_Click(object sender, EventArgs e)
        {
            StringDealerName = textBoxDealerName.Text.Trim().Replace("'", "''").ToString();
          //  StringDealerCode = textBoxDealerCode.Text.Trim().Replace("'", "''").ToString();
            StringPersonName = textBoxPersonName.Text.Trim().Replace("'", "''").ToString();
            StringMobileNo = textBoxMobileNo.Text.Trim().Replace("'", "''").ToString();
            StringAddress = textBoxAddress.Text.Trim().Replace("'", "''").ToString();
            StringCity = textBoxCity.Text.Trim().Replace("'", "''").ToString();
            StringState = ddlState.SelectedValue.Trim().Replace("'", "''").ToString();
            StringAreaDealer = textBoxDealerArea.Text.Trim().Replace("'", "''").ToString();
            txtERPClientCode.Text = txtERPClientCode.Text.Trim().Replace("'", "''").ToString();
            txtERPEmbossingCode.Text = txtERPEmbossingCode.Text.Trim().Replace("'", "''").ToString();
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
                if (StringcheckBoxCommercialVehicle == "0" && StringcheckBoxFourWheeler == "0" && StringcheckBoxTwoWheeler == "0" )
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
            //if (string.IsNullOrEmpty(StringDealerCode))
            //{
            //    lblErrMess.Text = String.Empty;
            //    lblErrMess.Text = "Please Provide Dealer Code.";
            //    return;
            //}
            if (string.IsNullOrEmpty(StringPersonName))
            {
                lblErrMess.Text = String.Empty;
                lblErrMess.Text = "Please Provide Contact Person Name.";
                return;
            }
            if (string.IsNullOrEmpty(txtERPClientCode.Text.ToString()))
            {
                lblErrMess.Text = String.Empty;
                lblErrMess.Text = "Please Provide ERP Client Code.";
                return;
            }
            if (string.IsNullOrEmpty(txtERPEmbossingCode.Text.ToString()))
            {
                lblErrMess.Text = String.Empty;
                lblErrMess.Text = "Please Provide ERP Embossing Code.";
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
            if (HsrpStateId == "4")
            {

                if (dropDownListClient.SelectedItem.ToString().Equals("--Select Embossing Center--"))
                {
                    lblErrMess.Text = string.Empty;
                    lblErrMess.Text = "Please Provide Embossing Center.";
                    return;
                }

                if (string.IsNullOrEmpty(StringAreaDealer))
                {
                    lblErrMess.Text = String.Empty;
                    lblErrMess.Text = "Please Provide Dealer Area.";
                    return;
                }
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
                if (txtLMVUpTo10Lac.Text== "" || txtLMV10To25Lac.Text == "" || txtLMV25To50Lac.Text == "" || txtLMVMoreThan50Lac.Text == "" || txtLMVClassUpTo10Lac.Text == "" || txtLMVClass10To25Lac.Text == "" || txtLMVClass25To50Lac.Text == "" || txtLMVClassMoreThan50Lac.Text == "")
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
            if (ddlSalesPerson.SelectedValue == "--Contact Persion Name--")
            {
                contactPersionID = "0";
            }
            else
            {
                contactPersionID = ddlSalesPerson.SelectedValue;

            }

            string userID = Session["UID"].ToString();
            if (HsrpStateId == "4")
            {
                if (dropDownListClient.SelectedItem.ToString().Equals("Gurgaon"))
                {
                    Dealercode = "EC-DLR-GGN";

                }
                else if (dropDownListClient.SelectedItem.ToString().Equals("Faridabad"))
                {
                    Dealercode = "EC-DLR-FBD";

                }
                else if (dropDownListClient.SelectedItem.ToString().Equals("Karnal"))
                {
                    Dealercode = "EC-DLR-KAR";
                }
                else
                {
                    Dealercode = "EC-DLR-SNP";
                }

            }
            //string SQLUser = "select count(*) as total from dealer where userID='" + userID + "' ";
            //DataTable  dt =  Utils.GetDataTable(SQLUser, CnnString);
            //int count = Convert.ToInt16(dt.Rows[0]["total"]);
            //if (count == 0)
            //{
            if (HsrpStateId == "4")
            {

                SQLString = "insert into  DealerMaster (erpembcode,DealerName,Address,City,[State],ContactPerson," +
                    "ContactMobileNo,AreaOfDealer,IsDealingInTwoWheeler,IsDealingInFourWheeler,IsDealingInCommercial," +
                    "ChargingType,IsActive,ContactPersionID,HSRP_StateID ,RTOLocationID,erpclientcode) values" +
                 "('" + txtERPEmbossingCode.Text.ToString().ToUpper() + "','" + StringDealerName + "','" + StringAddress + "','" + StringCity + "'," +
                 "'" + StringState + "','" + StringPersonName + "','" + StringMobileNo + "','" + StringAreaDealer + "'," + StringcheckBoxTwoWheeler + "," +
                 "" + StringcheckBoxFourWheeler + "," + StringcheckBoxCommercialVehicle + ",'" + StringChargingType + "','" + ActiveStatus + "','" + contactPersionID + "', '" + Session["UserHSRPStateID"] + "','" + StringrtolocationId + "','" + txtERPClientCode.Text.ToString().ToUpper() + "')";
            }
            else
            {
                SQLString = "insert into  DealerMaster (DealerName,Address,City,[State],ContactPerson," +
                    "ContactMobileNo,AreaOfDealer,IsDealingInTwoWheeler,IsDealingInFourWheeler,IsDealingInCommercial," +
                    "ChargingType,IsActive,ContactPersionID,HSRP_StateID ,RTOLocationID,erpclientcode,erpembcode) values" +
                 "('" + StringDealerName + "','" + StringAddress + "','" + StringCity + "'," +
                 "'" + StringState + "','" + StringPersonName + "','" + StringMobileNo + "','" + StringAreaDealer + "'," + StringcheckBoxTwoWheeler + "," +
                 "" + StringcheckBoxFourWheeler + "," + StringcheckBoxCommercialVehicle + ",'" + StringChargingType + "','" + ActiveStatus + "','" + contactPersionID + "', '" + Session["UserHSRPStateID"] + "','" + StringrtolocationId + "','" + txtERPClientCode.Text.ToString().ToUpper() + "','" + txtERPEmbossingCode.Text.ToString().ToUpper() + "')";
            }
            if (Utils.ExecNonQuery(SQLString, CnnString) > 0)
            {
                SQLString = "SELECT  top 1 DealerId  FROM [DealerMaster]  order by DealerId desc";
                DataTable dt = Utils.GetDataTable(SQLString, CnnString);
                string id = dt.Rows[0]["DealerID"].ToString();

                SQLString = "INSERT INTO DealerPrice(Dealerid,Twowheeler_rate,LMVUpTo10Lac_rate,LMV10To25Lac_rate,LMV15To50Lac_rate,LMVMoreThan50Lac_rate,LMVClassUpTo10Lac_rate,LMVClass10To25Lac_rate,LMVClass15To50Lac_rate,LMVClassMoreThan50Lac_rate,TransLMV_Rate,transLMVClass_rate,transAuto_rate,transLCV_rate,transMVC_rate,transHCV_rate," +
"SRTwowheeler_rate,SRLMVUpTo10Lac_rate,SRLMV10To25Lac_rate,SRLMV15To50Lac_rate,SRLMVMoreThan50Lac_rate,SRLMVClassUpTo10Lac_rate,SRLMVClass10To25Lac_rate,SRLMVClass15To50Lac_rate,SRLMVClassMoreThan50Lac_rate,SRTransLMV_Rate,SRtransLMVClass_rate,SRtransAuto_rate,SRtransLCV_rate,SRtransMVC_rate,SRtransHCV_rate,HSRP_StateID ,RTOLocationID)" +
" values('" + id + "','" + txtTwoWheelerRate.Text + "','" + txtLMVUpTo10Lac.Text + "','" + txtLMV10To25Lac.Text + "','" + txtLMV25To50Lac.Text + "','" + txtLMVMoreThan50Lac.Text + "','" + txtLMVClassUpTo10Lac.Text + "','" + txtLMVClass10To25Lac.Text + "','" + txtLMVClass25To50Lac.Text + "','" + txtLMVClassMoreThan50Lac.Text + "','" + txtLMV.Text + "','" + txtLMVClass.Text + "','" + txtAuto.Text + "','" + txtLCV.Text + "','" + txtLMV.Text + "','" + txtHCV.Text + "','" + txtTwoWheelerRate1.Text + "','" + txtLMVUpTo10Lac1.Text + "','" + txtLMV10To25Lac1.Text + "','" + txtLMV25To50Lac1.Text + "','" + txtLMVMoreThan50Lac1.Text + "','" + txtLMVClassUpTo10Lac1.Text + "','" + txtLMVClass10To25Lac1.Text + "','" + txtLMVClass25To50Lac1.Text + "','" + txtLMVClassMoreThan50Lac1.Text + "','" + txtLMV1.Text + "','" + txtLMVClass1.Text + "','" + txtAuto1.Text + "','" + txtLCV1.Text + "','" + txtLMV1.Text + "','" + txtHCV1.Text + "', '" + Session["UserHSRPStateID"] + "','" + Session["UserRTOLocationID"] + "')";

                Utils.ExecNonQuery(SQLString, CnnString);
                //SQLString="update DealerMaster
                lblSucMess.Text = "Record Saved Sucessfully.";
                Refresh();
            }
            else
            {
                lblErrMess.Text = "Record Not Added.";
            }
            //}
            //else
            //{
            //    Page.ClientScript.RegisterStartupScript(typeof(Page), "alert", "<script language=JavaScript>alert('User Already Registered!!');</script>");
            //}
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
            textBoxDealerName.Text = "";
           // textBoxDealerCode.Text = "";
            textBoxAddress.Text = "";
            textBoxCity.Text = "";
           
            textBoxPersonName.Text = "";
            textBoxMobileNo.Text = "";
            textBoxDealerArea.Text = "";
            checkBoxCommercialVehicle.Checked = false;
            checkBoxCommercialVehicle1.Checked = false;
            checkBoxFourWheeler.Checked = false;
            checkBoxFourWheeler1.Checked = false;
            checkBoxTwoWheeler.Checked = false;
            checkBoxTwoWheeler1.Checked = false;
            radioButtonCenter.Checked = false;
            radioButtonShowRoom.Checked = false;

            txtTwoWheelerRate.Text="";
            txtLMVUpTo10Lac.Text ="";
            txtLMV10To25Lac.Text ="";
            txtLMV25To50Lac.Text ="";
            txtLMVMoreThan50Lac.Text ="";
            txtLMVClassUpTo10Lac.Text="";
            txtLMVClass10To25Lac.Text ="";
            txtLMVClass25To50Lac.Text ="";
            txtLMVClassMoreThan50Lac.Text="";
            txtLMV.Text ="";
            txtLMVClass.Text="";
            txtAuto.Text ="";
            txtLCV.Text="";
            txtLMV.Text="";
            txtHCV.Text="";
            txtTwoWheelerRate1.Text="";
            txtLMVUpTo10Lac1.Text ="";
            txtLMV10To25Lac1.Text ="";
            txtLMV25To50Lac1.Text="";
            txtLMVMoreThan50Lac1.Text ="";
            txtLMVClassUpTo10Lac1.Text ="";
            txtLMVClass10To25Lac1.Text ="";
            txtLMVClass25To50Lac1.Text ="";
            txtLMVClassMoreThan50Lac1.Text ="";
            txtLMV1.Text ="";
            txtLMVClass1.Text ="";
            txtAuto1.Text ="";
            txtLCV1.Text ="";
            txtLMV1.Text ="";
            txtHCV1.Text = "";

           

        }

        

    }
}