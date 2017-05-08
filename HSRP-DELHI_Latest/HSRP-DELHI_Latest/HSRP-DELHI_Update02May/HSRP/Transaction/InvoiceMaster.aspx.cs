using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using DataProvider;
using HSRP;


namespace HSRP.Transaction
{
    public partial class InvoiceMaster : System.Web.UI.Page
    {
        string Mode;
        int HSRP_StateID;
        string SQLString = string.Empty;
        string UserType = string.Empty;

        string strUserID = string.Empty;
        string ComputerIP = string.Empty;

        string HSRPStateID = string.Empty;
        string RTOLocationID = string.Empty;
        int intHSRPStateID;
        string SQlQuery = string.Empty;
        string ExicseAmount = string.Empty;
        string AllLocation = string.Empty;
        string OrderStatus = string.Empty;

        int EditStateID;
        string CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();

        // string CnnString = string.Empty;
        // CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString(); 
        protected void Page_Load(object sender, EventArgs e)
        {

            Utils.GZipEncodePage();
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {


                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();

                RTOLocationID = Session["UserRTOLocationID"].ToString();
                UserType = Session["UserType"].ToString();
                HSRPStateID = Session["UserHSRPStateID"].ToString();

                lblErrMess.Text = string.Empty;
                strUserID = Session["UID"].ToString();
                ComputerIP = Request.UserHostAddress;

                if (!IsPostBack)
                {
                    //InitialSetting();
                    try
                    {

                        if (UserType == "0")
                        {
                            labelOrganization.Visible = true;
                            DropDownListStateName.Visible = true;
                            DropDownListStateName.Enabled = true;
                            labelClient.Visible = true;
                            dropDownListClient.Visible = true;
                            FilldropDownListOrganization();
                            FilldropDownListClient();
                        }
                        else
                        {
                            //hiddenUserType.Value = "1";
                            labelOrganization.Enabled = false;
                            DropDownListStateName.Enabled = false;
                            labelClient.Enabled = false;
                            FilldropDownListOrganization();
                            FilldropDownListClient();

                        }

                    }
                    catch (Exception err)
                    {
                        lblErrMess.Text = "Error on Page Load" + err.Message.ToString();
                    }
                }
            }
        }

        #region DropDown

      
        private void FilldropDownListOrganization()
        {
            if (UserType == "0")
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where ActiveStatus='Y' Order by HSRPStateName";
                Utils.PopulateDropDownList(DropDownListStateName, SQLString.ToString(), CnnString, "--Select State--");

            }
            else
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID=" + HSRPStateID + " and ActiveStatus='Y' Order by HSRPStateName";
                DataTable dts = Utils.GetDataTable(SQLString, CnnString);
                DropDownListStateName.DataSource = dts;
                DropDownListStateName.DataBind();
            }
        }
        private void FilldropDownListClient()
        {
            if (UserType == "0")
            {

                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);

                SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + intHSRPStateID + " and ActiveStatus!='N' and RTOLocationName in('BURARI','MAYAPURI')   Order by RTOLocationName";
                Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), CnnString, "--Select RTO Name--");

                //dataLabellbl.Visible = false;
                //TRRTOHide.Visible = false;
            }
            else
            {
                string UserID = Convert.ToString(Session["UID"]);

                SQLString = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, (a.RTOLocationCode+', ') as RTOCode, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID and a.ActiveStatus ='Y'  where UserRTOLocationMapping.UserID='" + UserID + "' order by a.rtolocationname ";
                DataTable dss = Utils.GetDataTable(SQLString, CnnString);

                labelOrganization.Visible = true;
                DropDownListStateName.Visible = true;
                labelClient.Visible = true;
                dropDownListClient.Visible = true;

                dropDownListClient.DataSource = dss;
                dropDownListClient.DataBind();
                //dataLabellbl.Visible = true;

                string RTOCode = string.Empty;
                if (dss.Rows.Count > 0)
                {
                    for (int i = 0; i <= dss.Rows.Count - 1; i++)
                    {
                        RTOCode += dss.Rows[i]["RTOCode"].ToString();

                    }
                    //  lblRTOCode.Text = RTOCode.Remove(RTOCode.LastIndexOf(","));

                }
            }
        }

        #endregion

        public void insertinvoiceno()
        {
            try
            {
                string strInvoiceNo = string.Empty;
                string sqlstring = string.Empty;
                string strEmbId = string.Empty;
                string currentdate = DateTime.Now.ToString("dd/MM/yyyy");
                string Remarks = string.Empty;
                Int32 totalamount = 0;
                string RtoName = string.Empty;
                RtoName = dropDownListClient.SelectedItem.ToString();

                string strSelectEmbStation = "SELECT DISTINCT [NAVEMBID],[EmbCenterName],address1,city FROM [vw_RTOLocationWiseEmbosingCenters] WHERE RTOLocationId='" + dropDownListClient.SelectedValue + "'";
                DataTable dtEmbData = Utils.GetDataTable(strSelectEmbStation, CnnString);
                if (dtEmbData.Rows.Count <= 0)
                {
                    lblErrMess.Text = "Embossing Station not found";
                    return;
                }
                strEmbId = dtEmbData.Rows[0]["NAVEMBID"].ToString();

                string strGetInvoiceNo = "select (isnull(prefixtext,'')+right('00000'+ convert(varchar,lastno+1),5)) from [EmbossingCenters] " +
                                           "where Emb_Center_Id= '" + strEmbId + "' and prefixfor='Cash Receipt No' ";
                strInvoiceNo = (Utils.getScalarValue(strGetInvoiceNo, CnnString));

                string strGetFinYear = "SELECT [dbo].[fnGetFiscalYear] ( GetDate() )";
                strInvoiceNo = strInvoiceNo + "/" + (Utils.getScalarValue(strGetFinYear, CnnString)).Replace("20", string.Empty);

                string strUpdateInvoiceNo = "update [EmbossingCenters] set lastno=lastno+1 where [Emb_Center_Id]= '" + strEmbId + "' and prefixfor='Cash Receipt No'";
                Utils.ExecNonQuery(strUpdateInvoiceNo, CnnString);

                DataTable GetAddress;
                string Address;
                GetAddress = Utils.GetDataTable("select * from HSRPState WHERE HSRP_StateID='" + DropDownListStateName.SelectedValue + "'", CnnString);

                if ((GetAddress.Rows[0]["pincode"].ToString() != "") || (GetAddress.Rows[0]["pincode"] != null))
                {
                    Address = " - " + GetAddress.Rows[0]["pincode"];
                }
                else
                {
                    Address = "";
                }
                Remarks = "Transfer Order";
                sqlstring = "insert into InvoiceMaster(InvoiceNo,InvoiceDate,Amount,BuyerName,clientName,hsrp_stateid,dispatchedLocation) values('" + strInvoiceNo + "', Convert(date,('" + currentdate + "'),103),'" + totalamount + "','" + Remarks + "','" + GetAddress.Rows[0]["Address1"].ToString() + "','" + DropDownListStateName.SelectedValue + "','" + dropDownListClient.SelectedItem.Text + "')";
                Utils.ExecNonQuery(sqlstring, CnnString);

                lblinvoicemsg.Text = "New Invoice No :" + strInvoiceNo;
            }
            catch
            {
                lblErrMess.Text = "Embossing Station not found";
                return;
            }
        }

        protected void buttonUpdateinvoice_Click(object sender, EventArgs e)
        {
            insertinvoiceno();
        }

        protected void DropDownListStateName_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilldropDownListClient();
            dropDownListClient.Visible = true;
            labelClient.Visible = true;
        }
    }
}