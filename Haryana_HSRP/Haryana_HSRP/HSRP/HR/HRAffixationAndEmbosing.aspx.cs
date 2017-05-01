using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataProvider;

namespace HSRP.HR
{
    public partial class HRAffixationAndEmbosing : System.Web.UI.Page
    {
        string CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
      
       Validate validate = new Validate();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            lblerror.Text = string.Empty;
            lblerror.Text = string.Empty;
            gridRecords.DataSource = null;
            string VechileRegNO = string.Empty;
            string ReturnValue = string.Empty;
            int Result=0;
            VechileRegNO = txtSearch.Text.Trim();
            if (validate.ValidateStringValue(VechileRegNO))
            {
                BAL objBal = new BAL();
                DataTable Dt= new DataTable();
                Dt = objBal.HR_AffixationAndEmbosing(VechileRegNO, 1).Tables[0];
               
                if ( Dt.Columns.Contains("Result"))
                {
                    string[] ReturnValues = Dt.Rows[0]["Result"].ToString().Split('^');
                    Result=Convert.ToInt32( ReturnValues[0]);
                        lblerror.Text = ReturnValues[1];
                }
                else
                {
                    gridRecords.DataSource = Dt;
                    gridRecords.DataBind();
                   
                }

            }
            else
            {
                lblerror.Text = "Enter Vechile No";
            }

        }

        protected void gridRecords_RowCommand(object sender, GridViewCommandEventArgs e)
        {
      string hsrprecordid= string.Empty;
      string precloseduserid=string.Empty;
      string rtolocationid=string.Empty;
      string hsrp_stateid=string.Empty;
      string preclosedatetime = string.Empty;
      string hsrprecordorderstatus=string.Empty;
      string hsrprecordcloseddatetime = string.Empty;
      string hsrprecordembossingdatetime = string.Empty;
      string processstatus=string.Empty;
    //  DateTime processeddatetime;
            if (e.CommandName == "Update")
            {
                try 
                { 
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow gvRow = gridRecords.Rows[index];
                string VechileRegNO = gvRow.Cells[1].Text;

                string Query = "update hsrprecords set orderstatus='PreClose', PreCloserDateTime=GetDate(),closeduserid='" + Session["UID"].ToString() + "' where VehicleRegNo ='" + VechileRegNO + "'";
                int i=   Utils.ExecNonQuery(Query, CnnString);

                string sql = "select [hsrprecordid],[closeduserid],[rtolocationid],[hsrp_stateid],[PreCloserDateTime],[orderstatus],[OrderEmbossingDate],ordercloseddate FROM hsrprecords where VehicleRegNo ='" + VechileRegNO + "' and orderstatus='PreClose' ";
                DataTable dt = Utils.GetDataTable(sql,CnnString);
                if(dt.Rows.Count>0)
                {
                    hsrprecordid = dt.Rows[0]["hsrprecordid"].ToString();
                    precloseduserid = dt.Rows[0]["closeduserid"].ToString();
                    rtolocationid = dt.Rows[0]["rtolocationid"].ToString();
                    hsrp_stateid = dt.Rows[0]["hsrp_stateid"].ToString();
                    preclosedatetime =dt.Rows[0]["PreCloserDateTime"].ToString();
                    hsrprecordorderstatus = dt.Rows[0]["orderstatus"].ToString();
                    hsrprecordcloseddatetime =dt.Rows[0]["ordercloseddate"].ToString();
                    hsrprecordembossingdatetime =dt.Rows[0]["OrderEmbossingDate"].ToString();
                   // processstatus = "OrderProcessed";
                   // processeddatetime = dt.Rows[0]["hsrprecordid"].ToString();


                }

                string INSERT = "insert into preclosehsrprecords([hsrprecordid],[precloseduserid],[rtolocationid],[hsrp_stateid],[preclosedatetime],[hsrprecordorderstatus],[hsrprecordcloseddatetime],[hsrprecordembossingdatetime])values('" + hsrprecordid + "','" + precloseduserid + "','" + rtolocationid + "','" + hsrp_stateid + "','" + preclosedatetime + "','" + hsrprecordorderstatus + "','" + hsrprecordcloseddatetime + "','" + hsrprecordembossingdatetime + "')";
                int j = Utils.ExecNonQuery(INSERT,CnnString);
               }
                catch(Exception ex)
                {
                
                }
             
            }

        }

       
    }
}