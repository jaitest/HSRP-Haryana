using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.IO;
using System.Data;
using System.Configuration;

namespace HSRP.Transaction
{
    public partial class UkQuickCapture : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        int UserType;
        int HSRPStateID;
        int RTOLocationID;
        int intOrgID;
        int intClientID;
        int UID;
        string SaveLocation = string.Empty;
        StringBuilder SBSQL = new StringBuilder();
        string Status = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Utils.GZipEncodePage();
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {
                int.TryParse(Session["UserType"].ToString(), out UserType);
                int.TryParse(Session["UserHSRPStateID"].ToString(), out HSRPStateID);
                int.TryParse(Session["UserRTOLocationID"].ToString(), out RTOLocationID);
                int.TryParse(Session["UID"].ToString(), out UID);

                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                //strUserID = Session["UID"].ToString();
                ComputerIP = Request.UserHostAddress;

                string usrname1 = Utils.getDataSingleValue("Select UserFirstName + space(2)+UserLastName as UserName From Users where UserID=" + UID.ToString(), CnnString, "UserName");
               // LabelCreatedID.Text = usrname1;
               // LabelCreatedDateTime.Text = DateTime.Now.ToString("dd MMM yyyy");

                if (!IsPostBack)
                {
                    FilldropDownState();
                    FilldropDownRTOLocation();
                }
            }
        }

        #region DropDown

        private void FilldropDownState()
        {
            if (UserType.Equals(0))
            {
                //SQLString = "select HSRPStateName,HSRP_StateID from HSRPState where ActiveStatus='Y' Order by HSRPStateName";
                //Utils.PopulateDropDownList(dropDownListOrg, SQLString.ToString(), CnnString, "--Select State--");
            }
            else
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID=" + HSRPStateID + " and ActiveStatus='Y' Order by HSRPStateName";
                DataSet dts = Utils.getDataSet(SQLString, CnnString);
                //dropDownListOrg.DataSource = dts;
                //dropDownListOrg.DataBind();
            } 
        } 
        private void FilldropDownRTOLocation()
        {
            if (UserType.Equals(0))
            {
              //  int.TryParse(dropDownListOrg.SelectedValue, out intOrgID);
                SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=6 and ActiveStatus='Y' and LocationType='Sub-Urban' Order by RTOLocationName";
                Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), CnnString, "--Select RTO--");
            }
            else
            {
                 
                //SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where RTOLocationID=" + RTOLocationID + " and ActiveStatus='Y' and LocationType='Sub-Urban' Order by RTOLocationName";
                SQLString = "SELECT distinct (a.RTOLocationName) as RTOLocationName, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID where  a.LocationType!='District' and UserRTOLocationMapping.UserID='" + UID + "' ";
                DataSet dss = Utils.getDataSet(SQLString, CnnString);
                dropDownListClient.DataSource = dss;
                dropDownListClient.DataBind();

             
            }
        }


        #endregion
        

        protected void dropDownListOrg_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (dropDownListOrg.SelectedItem.Text != "--Select State--")
            //{
            //    dropDownListClient.Visible = true;
            //    FilldropDownRTOLocation();
            //}
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtVehicleNo.Text=="")
                {

                    llbMSGError.Text = "Please insert Vehicle Number";
                }
                else if (OrderDate.SelectedDate > HSRPAuthDate.SelectedDate)
                {

                    llbMSGError.Text = "Please Select Correct Date";
                }
                else 
                {
                Status = RadioButtonList1.SelectedItem.ToString();
                  
                string sqlSaveResult = "INSERT INTO UK_QuickCapture(RTOLocationID,OrderStatus,Vehicleregno,Hsrp_Front_LaserCode,Hsrp_Rear_LaserCode,OrderEmbossingDate,OrderClosedDate) VALUES ('" + dropDownListClient.SelectedValue + "','" + Status + "','" + txtVehicleNo.Text + "','" + txtFlaserNo.Text + "','" + txtRno.Text + "','" + OrderDate.SelectedDate + "','" + HSRPAuthDate.SelectedDate + "')";
                Utils.ExecNonQuery(sqlSaveResult, CnnString);
                llbMSGSuccess.Text = "Record Save Successfully";
                ClearTextBox();
                    
                }
                
            }
            catch
            {
                llbMSGError.Text = "Please Select Status";
            }
        }

        private void ClearTextBox()
        {
            dropDownListClient.SelectedIndex = 0;
            txtVehicleNo.Text = "";
            txtFlaserNo.Text = "";
            txtRno.Text = "";
            llbMSGError.Text = "";
         
        }

        protected void RadioButtonList1_SelectedIndexChanged(object sender,System.EventArgs e)
        {
            Status = RadioButtonList1.SelectedItem.ToString();  
        }

        


    }
}