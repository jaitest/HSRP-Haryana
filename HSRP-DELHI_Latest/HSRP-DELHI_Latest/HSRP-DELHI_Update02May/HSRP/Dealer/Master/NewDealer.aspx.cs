using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HSRP.Dealer.Master
{
    public partial class NewDealer : System.Web.UI.Page
    {
        string ActiveStatus = "N";
        string StringDealerName = String.Empty;
        string StringAreaDealer = String.Empty;
        string StringMemberStatus = String.Empty;
        string StringStatus = String.Empty;
        string StringPriortyPeriod = String.Empty;
        string StringRemarks = String.Empty;
        string SQLString = string.Empty;
        string CnnString = string.Empty;
        

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void buttonSave_Click(object sender, EventArgs e)
        {
            try
            {
                StringDealerName = txtDealerName.Text;
                StringAreaDealer = txtArea.Text;
                StringMemberStatus = txtMemberStatus.Text;
                StringStatus = txtStatus.Text;
                StringPriortyPeriod = txtPriortyPeriod.Text;
                StringRemarks = txtRemarks.Text;
                lblErrMess.Text = string.Empty;
                lblSucMess.Text = string.Empty;

                if (string.IsNullOrEmpty(StringDealerName))
                {
                    lblErrMess.Text = String.Empty;
                    lblErrMess.Text = "Please Provide Dealer Name.";
                    return;
                }

                if (string.IsNullOrEmpty(StringAreaDealer))
                {
                    lblErrMess.Text = String.Empty;
                    lblErrMess.Text = "Please Provide Dealer Area.";
                    return;
                }

                if (string.IsNullOrEmpty(StringMemberStatus))
                {
                    lblErrMess.Text = String.Empty;
                    lblErrMess.Text = "Please Provide Member Status";
                    return;
                }

                if (string.IsNullOrEmpty(StringStatus))
                {
                    lblErrMess.Text = String.Empty;
                    lblErrMess.Text = "Please Provide Status";
                    return;
                }

                //if (string.IsNullOrEmpty(StringPriortyPeriod))
                //{
                //    lblErrMess.Text = String.Empty;
                //    lblErrMess.Text = "Please Provide Priorty Period.";
                //    return;
                //}

                //if (string.IsNullOrEmpty(StringRemarks))
                //{
                //    lblErrMess.Text = String.Empty;
                //    lblErrMess.Text = "Please enter remarks";
                //    return;
                //}

                if (chkActiveStatus.Checked)
                {
                    ActiveStatus = "Y";
                }
                string userID = Session["UID"].ToString();
                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();

                SQLString = "sp_insertNewDealer '" + StringDealerName + "','" + StringAreaDealer + "','" + StringMemberStatus + "','" + StringStatus + "','" + StringPriortyPeriod + "','" + ActiveStatus + "','" + StringRemarks + "'";

                if (Utils.ExecNonQuery(SQLString, CnnString) > 0)
                {
                    lblSucMess.Text = "Record Saved Sucessfully.";
                    txtDealerName.Text="";
                    txtArea.Text = "";
                    txtMemberStatus.Text = "";
                    txtStatus.Text = "";
                    txtPriortyPeriod.Text = "";
                    txtRemarks.Text = "";
                }
                else
                {
                    lblErrMess.Text = "Record Not Added.";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
    }
}