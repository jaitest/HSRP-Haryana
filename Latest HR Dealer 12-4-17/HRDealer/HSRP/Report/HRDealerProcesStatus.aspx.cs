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
using System.Globalization;


namespace HSRP.Report
{
    public partial class HRNewProcessStatus : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        string UserType = string.Empty;
        string HSRPStateID = string.Empty;
        string RTOLocationID = string.Empty;
       
        string strComplaintID = string.Empty;
        string strSql = string.Empty;

        string AllLocation = string.Empty;
        string OrderStatus = string.Empty;
       
        DataProvider.BAL bl = new DataProvider.BAL();
        string StickerManditory = string.Empty;

        string SubmitId = string.Empty;
        string QrySubmitID = string.Empty;

        string State_ID = string.Empty;
        string RTO_ID = string.Empty;
        string HSRPStateIDEdit = string.Empty;
        string RTOLocationIDEdit = string.Empty;
        string fromdate = string.Empty;
        string ToDate = string.Empty;
        string strSqlGo = string.Empty;
        string strsqlgonew = string.Empty;
        string strsqlTG = string.Empty;
        string strsqlAP = string.Empty;
        DataTable dtcount = new DataTable();
        DataTable dtshow = new DataTable();
        DataTable Totaltg = new DataTable();
        DataTable Totalap = new DataTable();
        DataTable TotalTG = new DataTable();


        DataTable TotalAPP = new DataTable();
        DataTable TotalAP = new DataTable();
            

        DataTable dt = new DataTable();


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

                strUserID = Session["UID"].ToString();
                ComputerIP = Request.UserHostAddress;

                if (!IsPostBack)
                {
                    InitialSetting();


                    try
                    {
                        if (UserType == "0")
                        {
                            getreport();
                            Gettotalorder();
                           

                        }
                        else
                        {
                            getreport();
                            Gettotalorder();
                           

                        }
                    }
                    catch (Exception err)
                    {
                        
                    }
                }
            }
        }

        





        private void InitialSetting()
        {

           
        }

        protected void dropDownListClient_SelectedIndexChanged1(object sender, EventArgs e)
        {

        }


        public void getreport()
        {
            string date = System.DateTime.Now.ToString("yyyy/MM/dd 00:00:00");
            string date1 = System.DateTime.Now.ToString("yyyy/MM/dd 23:59:59");
            strsqlgonew = "select District,rtolocationname as Location, (select userfirstname from users where userid=createdby) as DealerName, count(*) as VehicleNo, sum(roundoff_netamount) as Amount from hsrprecords a,RTOLocation b where a.rtolocationid=b.rtolocationid and  a.HSRP_StateID in (4) and hsrprecord_creationdate between  '" + date + "' and '" + date1 + "'  and a.createdby in (select userid from users where isnull(dealerid,'')!='' )group by District,rtolocationname,createdby";
           
            dtshow = Utils.GetDataTable(strsqlgonew, CnnString);
            if (dtshow.Rows.Count > 0)
            {
                GrdAP.DataSource = dtshow;
                GrdAP.DataBind();


            }
            else
            {
                lblerror.Visible = true;
                lblerror.Text = "No Record Found.";
            }
           
          

        }

        public void Gettotalorder()
        {

            string date = System.DateTime.Now.ToString("yyyy/MM/dd 00:00:00");
            string date1 =System.DateTime.Now.ToString("yyyy/MM/dd 23:59:59");
            strsqlgonew = " select count(*) as TotalOrders,sum(roundoff_netamount) as TotalAmount from hsrprecords a,RTOLocation b   where a.rtolocationid=b.rtolocationid and a.hsrp_stateid in (4)  and  hsrprecord_creationdate between '" + date + "' and '" + date1 + "' and a.createdby in (select userid from users where isnull(dealerid,'')!='' )";
         
            Totalap = Utils.GetDataTable(strsqlgonew, CnnString);


            if (Totalap.Rows.Count > 0)
            {
                GridShowtotal.DataSource = Totalap;
                GridShowtotal.DataBind();


            }
            else
            {
                lblerror.Visible = true;
                lblerror.Text = "No Record Found.";
            }




        }

       

      

        









        }
    }


