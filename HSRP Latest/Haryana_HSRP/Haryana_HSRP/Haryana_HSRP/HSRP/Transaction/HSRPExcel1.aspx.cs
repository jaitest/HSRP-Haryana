using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace HSRP.Transaction
{
    public partial class HSRPExcel1 : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string StringMode = string.Empty;
        string UID = String.Empty;
        string HSRPStateID = String.Empty;
        string HSRPRtoLocationID = String.Empty;


        protected void Page_Load(object sender, EventArgs e)
        {
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            //  InitialSetting();
            UID = Session["UID"].ToString();
            HSRPStateID = Session["UserHSRPStateID"].ToString();
            HSRPRtoLocationID = Session["UserRTOLocationID"].ToString();

            btnSave.Visible = true;
            if (!IsPostBack)
            {
                RTOLocationName();
            }
        }
        DataTable dt = new DataTable();
        public void RTOLocationName()
        {
            SQLString = "select RTOLocationID ,RTOLocationName from dbo.RTOLocation where HSRP_StateID=" + HSRPStateID;
            dt = Utils.GetDataTable(SQLString, CnnString);
            ddlRtoLocation.DataSource = dt;
            ddlRtoLocation.DataTextField = "RTOLocationName";
            ddlRtoLocation.DataValueField = "RTOLocationID";
            ddlRtoLocation.DataBind();
            ddlRtoLocation.Items.Insert(0, "--Select Rto Location--");
            ddlRtoLocation.Items[0].Value = "--Select Rto Location--";

            SQLString = "select HSRPStateName from dbo.HSRPState where HSRP_StateID=" + HSRPStateID;
            txtStateName.Text = Utils.getDataSingleValue(SQLString, CnnString, "HSRPStateName");
        }

        //public void edit()
        //{
        //    SQLString = "select * from dbo.DealerReceiptEntry where Id ='" + Request.QueryString["ID"].ToString() + "'";
        //    dt = Utils.GetDataTable(SQLString, CnnString);
        //    OrderDate.SelectedDate = DateTime.Parse(dt.Rows[0]["ChequeDate"].ToString());
        //    HSRPAuthDate.SelectedDate = DateTime.Parse(dt.Rows[0]["ReceivedDate"].ToString());
        //    ddlDealerName.SelectedValue = dt.Rows[0]["DealerID"].ToString();
        //    txtChequeAmount.Text = dt.Rows[0]["ChequeAmount"].ToString();
        //    txtChequeNo.Text = dt.Rows[0]["ChequeNo"].ToString();
        //    txtDrawnBankName.Text = dt.Rows[0]["DrawnBankForm"].ToString();
        //    txtDeliveredBy.Text = dt.Rows[0]["ChequeDeliveredBy"].ToString();
        //    // dealerID.Value = dt.Rows[0]["DealerID"].ToString();
        //}

        //private void InitialSetting()
        //{

        //    string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
        //    string MaxDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();

        //    HSRPAuthDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
        //    HSRPAuthDate.MaxDate = DateTime.Parse(MaxDate);
        //    CalendarHSRPAuthDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
        //    CalendarHSRPAuthDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
        //    OrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
        //    OrderDate.MaxDate = DateTime.Parse(MaxDate);
        //    CalendarOrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
        //    CalendarOrderDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);

        //    // CheckDeliveredDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00); 
        //}

        protected void btnSave_Click(object sender, EventArgs e)
        {

            if (ddlRtoLocation.SelectedValue == "--Select Rto Location--")
            {
                lblErrMess.Text = "Please select Rto Location.";
                return;
            }

            if ((FileUpload1.PostedFile != null) && (FileUpload1.PostedFile.ContentLength > 0))
            {
                InsertDataInstage();
            }
            else
            {
                lblErrMess.Text = "Please select a file to upload.";
                lblSucMess.Text = "";
            }

        }

        private void InsertDataInstage()
        {
            //string filename = System.IO.Path.GetFileName(FileUpload1.PostedFile.FileName);
            string filename = "HSRP-" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + System.DateTime.Now.Second.ToString() + ".xls";
            string fileExtension = System.IO.Path.GetExtension(FileUpload1.PostedFile.FileName);

            // string fileLocation = Server.MapPath("~/HSRPExcel/" + filename);

            string fileLocation = System.Configuration.ConfigurationManager.AppSettings["HSRPExcel"].ToString();
            fileLocation += filename.Replace("\\\\", "\\");

            if (fileExtension != ".xls" || fileExtension != ".xlsx" || fileExtension != ".csv")
            {
                lblErrMess.Text = "";
                lblErrMess.Text = "Please UpLoad Excel/cvs File. ";

            }

            FileUpload1.PostedFile.SaveAs(fileLocation);

            SQLString = "Insert into HSRPExcelUpload(StateId,RtoLocationID,StateName,RtoLocationName,OrigionalFileName,RenamedFileName," +
            "Remarks,Uploadedby,Status) values(" + HSRPStateID + "," + ddlRtoLocation.SelectedItem.Value + ",'" + txtStateName.Text + "'," +
            "'" + ddlRtoLocation.SelectedItem.Text + "','" + FileUpload1.PostedFile.FileName + "','" + filename + "','" + txtRemarks.Text + "'," +
            "" + UID + ",'New')";
            if (Utils.ExecNonQuery(SQLString, CnnString) > 0)
            {
                lblSucMess.Text = "File Uploaded.";
                lblErrMess.Text = "";
                Refresh();
            }
            else
            {
                lblSucMess.Text = "";
                lblSucMess.Text = "Not Uploaded.";
            }
        }

        public void Refresh()
        {
            ddlRtoLocation.SelectedIndex = 0;
            txtRemarks.Text = "";
        }
    }
}