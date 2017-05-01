using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace HSRP.Transaction
{
    public partial class HSRPUpdateExcel : System.Web.UI.Page
    {

        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string StringMode = string.Empty;
        string UID = String.Empty;
        string ID = String.Empty;
        string HSRPRtoLocationID = String.Empty;


        protected void Page_Load(object sender, EventArgs e)
        {
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            //  InitialSetting();
            UID = Session["UID"].ToString();
            ID = Request.QueryString["Id"].ToString();

            btnSave.Visible = true;
            if (!IsPostBack)
            {

                StringMode = Request.QueryString["Mode"].ToString();

                if (StringMode.Equals("Edit"))
                {
                    edit();
                    btnSave.Visible = true;
                }
            }
        }
        DataTable dt = new DataTable();
        //public void RTOLocationName(string ID)
        //{
        //    SQLString = "select RTOLocationName from dbo.RTOLocation where HSRP_StateID=" + ID;
        //    labelRtolOcation.Text = Utils.getDataSingleValue(SQLString, CnnString, "RTOLocationName");
        //}

        public void edit()
        {
            SQLString = "select HEU.StateName,HEU.RtoLocationName,HEU.OrigionalFileName,U.UserLoginName " +
                "from dbo.HSRPExcelUpload HEU inner join Users U on U.UserID=HEU.Uploadedby where HEU.Id =" + ID;
            dt = Utils.GetDataTable(SQLString, CnnString);
            txtStateName.Text = dt.Rows[0]["StateName"].ToString();
            labelRtolOcation.Text = dt.Rows[0]["RtoLocationName"].ToString();
            labelfileName.Text = dt.Rows[0]["OrigionalFileName"].ToString();
            LabelUserName.Text = dt.Rows[0]["UserLoginName"].ToString();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (ddlStatus.SelectedValue=="--Select Status--")
            {
                lblErrMess.Text = "Please select correct file Sataus.";
                return;
            }
           InsertDataInstage();
        }

        private void InsertDataInstage()
        {
            SQLString = "Update HSRPExcelUpload set Checker_Remarks='" + txtRemarks.Text + "',CheckedDatetime='" + System.DateTime.Now + "'" +
                ",CheckedBy=" + UID + ",Status='" + ddlStatus.SelectedValue + "' where ID=" + ID + "";
            if (Utils.ExecNonQuery(SQLString, CnnString) > 0)
            {
                lblSucMess.Text = "File Updated.";
                lblErrMess.Text = "";
                Refresh();
            }
            else
            {
                lblSucMess.Text = "";
                lblSucMess.Text = "Not Updated.";
            }
        }

        public void Refresh()
        {
            txtStateName.Text = "";
            labelRtolOcation.Text = "";
            labelfileName.Text = "";
            txtRemarks.Text = "";
        }
    }
}