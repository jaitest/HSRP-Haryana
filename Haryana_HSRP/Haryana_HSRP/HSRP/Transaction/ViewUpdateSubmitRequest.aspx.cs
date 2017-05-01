using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using HSRP;
using System.Data;
using DataProvider;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Configuration;
using System.IO;
using CarlosAg.ExcelXmlWriter;
using System.Drawing;


namespace HSRP.Master
{
    public partial class ViewUpdateSubmitRequest : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        string UserType = string.Empty;
        string HSRPStateID = string.Empty;
        string RTOLocationID = string.Empty;
      
         DataProvider.BAL bl = new DataProvider.BAL();
         string StickerManditory = string.Empty;

         string SubmitId = string.Empty;
         string QrySubmitID = string.Empty;

         string State_ID = string.Empty;
         string RTO_ID = string.Empty;
         string HSRPStateIDEdit = string.Empty;
         string RTOLocationIDEdit = string.Empty;
         DataTable dt = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            strUserID = Session["UID"].ToString();
            Utils.GZipEncodePage();
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {
                if (Session["UserHSRPStateID"].ToString() == "")
                {
                   // ButImpData.Visible = true;
                }
                else
                {
                   // ButImpData.Visible = false;
                }

                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
               
                RTOLocationID = Session["UserRTOLocationID"].ToString();
                UserType = Session["UserType"].ToString();
                HSRPStateID = Session["UserHSRPStateID"].ToString();

                lblErrMsg.Text = string.Empty;
               
                ComputerIP = Request.UserHostAddress;

                string UID = Session["UID"].ToString();
                string usrname1 = Utils.getDataSingleValue("Select UserFirstName + space(2)+UserLastName as UserName From Users where UserID=" + UID.ToString(), CnnString, "UserName");

                SQLString = "select * from SubmitRequestDetail";
                DataTable dt = new DataTable();


                dt = Utils.GetDataTable(SQLString, CnnString);

                dt.Columns.Add("S.No", typeof(Int32));
                dt.Columns["S.No"].AutoIncrement = true;


                int counter = 1;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["S.No"] = (counter++).ToString();
                }


                if (dt.Rows.Count > 0)
                {


                    Grid1.DataSource = dt;
                    Grid1.RunningMode = (ComponentArt.Web.UI.GridRunningMode)Enum.Parse(typeof(ComponentArt.Web.UI.GridRunningMode), "Client");
                    Grid1.SearchOnKeyPress = true;
                    Grid1.DataBind();
                    Grid1.RecordCount.ToString();
                }
                else
                {
                    Grid1.Items.Clear();
                   
                }

                 
                if (!IsPostBack)
                {
                      
                    try
                    { 
                        if (UserType == "0")
                        {
                           
                        }
                        else
                        { 
                            hiddenUserType.Value = "1";
                            
                        }


                    }
                    catch (Exception err)
                    {
                        lblErrMsg.Text = "Error on Page Load" + err.Message.ToString();
                    }
                }
            }
        }

        



        

       
       


    }
}