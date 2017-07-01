using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HSRP.Transaction
{
    public partial class BlockVehicleRegNo : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        int UserType;
        int HSRP_StateID;
        int RTOLocationID;

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.GetCurrent(this).RegisterPostBackControl(this.btnSave);
            Page.Form.Attributes.Add("enctype", "multipart/form-data");
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
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
                strUserID = Session["UID"].ToString();


                if (!IsPostBack)
                {
                    try
                    {


                    }
                    catch (Exception err)
                    {
                        lblErrMsg.Text = "Error on Page Load " + err.Message.ToString();
                    }
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int i = 0;
            lblErrMsg.Text = string.Empty;
            lblSucMess.Text = string.Empty;


            try
            {
                if (string.IsNullOrEmpty(txtvehicleRegNo.Text.Trim().ToString()))
                {
                    lblErrMsg.Text = "Please Enter Vehicle Regstration No.";
                    lblSucMess.Text = "";
                    return;
                }
                else
                {
                    string SQLString1 = "select Count(VehicleregNo) as co from HSRPExcelUpload where Rtrim(LTRIM(vehicleregno)) = Rtrim(LTRIM('" + txtvehicleRegNo.Text.Trim().ToString() + "'))";
                    i = Utils.getScalarCount(SQLString1, CnnString);
                    if (i > 0)
                    {
                        lblErrMsg.Text = "Vehicle Regstration No Already Exists.";
                        lblSucMess.Text = "";
                        return;

                    }

                    SQLString = "insert   into hsrpexcelupload(StateID, rtolocationid, uploaddatetime, uploadedby, vehicleregNo)  values('" + HSRP_StateID + "','" + RTOLocationID + "',getdate()," + strUserID + ",'" + txtvehicleRegNo.Text.Trim().ToString() + "') ";
                    i = Utils.ExecNonQuery(SQLString, CnnString);
                }
                if (i > 0)
                {
                    lblErrMsg.Text = "";
                    lblSucMess.Text = "Record Saved successfull.";
                    txtvehicleRegNo.Text = "";

                }
                else
                {
                    lblErrMsg.Text = "Record not Saved.";
                    lblSucMess.Text = "";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



    }
}