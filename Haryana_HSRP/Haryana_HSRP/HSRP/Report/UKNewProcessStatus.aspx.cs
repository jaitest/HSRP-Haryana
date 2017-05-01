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
    public partial class UKNewProcessStatus : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        string UserType = string.Empty;
        string HSRPStateID = string.Empty;
        string RTOLocationID = string.Empty;
        int intHSRPStateID;
        string trnasportname, pp;
        string transtr, statename1;
        BaseFont basefont;
        string fontpath;
        string strComplaintID = string.Empty;
        string strSql = string.Empty;

        string AllLocation = string.Empty;
        string OrderStatus = string.Empty;
        DateTime AuthorizationDate;
        DateTime OrderDate1;
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
            //btnUpdate.Visible = false;
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {
                if (Session["UserHSRPStateID"].ToString() == "6")
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

               // lblErrMsg.Text = string.Empty;
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
                            //labelOrganization.Visible = true;
                            //DropDownListStateName.Visible = true;
                            //DropDownListStateName.Enabled = true;
                            //FilldropDownListOrganization();

                        }
                        else
                        {
                            getreport();
                            Gettotalorder();
                            //hiddenUserType.Value = "1";
                            //labelOrganization.Enabled = false;
                            //DropDownListStateName.Enabled = false;
                            //FilldropDownListOrganization();

                        }
                    }
                    catch (Exception err)
                    {
                        //lblErrMsg.Text = "Error on Page Load" + err.Message.ToString();
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


            strsqlgonew = "select District,rtolocationname as Location,count(*) as VehicleNo, sum(roundoff_netamount) as Amount from hsrprecords a,RTOLocation b where a.rtolocationid=b.rtolocationid and  a.HSRP_StateID in (6) and hsrprecord_creationdate between '" + date + "' and '" + date1 + "' group by District,rtolocationname order by 1,3 desc";
            dtshow = Utils.GetDataTable(strsqlgonew, CnnString);
            GrdAP.DataSource = dtshow;
            GrdAP.DataBind();


            //strSqlGo = "select District,rtolocationname as Location,count(*) as TSCollectionWithoutVehicleNo, sum(roundoff_netamount) as Amount from hsrprecords a,RTOLocation b where a.rtolocationid=b.rtolocationid and  a.HSRP_StateID in (11) and hsrprecord_creationdate between '" + date + "' and '" + date1 + "'  group by District,rtolocationname order by 1,3 desc";
            //dtcount = Utils.GetDataTable(strSqlGo, CnnString);
            //grdTG.DataSource = dtcount;
            //grdTG.DataBind();

      
          

        }

        public void Gettotalorder()
        {

            string date = System.DateTime.Now.ToString("yyyy/MM/dd 00:00:00");
            string date1 =System.DateTime.Now.ToString("yyyy/MM/dd 23:59:59");
            //strSqlGo = "select count(*) as TotalOrderswithoutVehicle,sum(roundoff_netamount) as TotalAmountWithoutVehicle from hsrprecords a,RTOLocation b  where a.rtolocationid=b.rtolocationid and a.hsrp_stateid in (9,11) AND vehicleregno='' and hsrprecord_creationdate between '" + date + "' and '" + date1 + "'";
            //Totaltg = Utils.GetDataTable(strSqlGo, CnnString);
            //GridTS.DataSource = Totaltg;
            //GridTS.DataBind();

            /*For Exception of recordshowing less or access
            select distinct erpassigndate from hsrprecords where hsrprecord_creationdate between '01-Dec-2014 00:00:00' and '01-Dec-2014 23:59:59' 
            and HSRP_StateID=4 and erpassigndate is not null and HSRP_Front_LaserCode=' '*/

            strsqlgonew = "select count(*) as TotalOrders,sum(roundoff_netamount) as TotalAmount from hsrprecords a,RTOLocation b  where a.rtolocationid=b.rtolocationid and a.hsrp_stateid in (6)  and hsrprecord_creationdate between '" + date + "' and '" + date1 + "'";
            Totalap = Utils.GetDataTable(strsqlgonew, CnnString);
            GridShowtotal.DataSource = Totalap;
            GridShowtotal.DataBind();


            //strsqlAP = "select count(*) as APTotalOrders,sum(roundoff_netamount) as APTotalAmount from hsrprecords a,RTOLocation b  where a.rtolocationid=b.rtolocationid and a.hsrp_stateid=9 and hsrprecord_creationdate between '" + date + "' and '" + date1 + "'";
            //TotalAP = Utils.GetDataTable(strsqlAP, CnnString);
            //TotalorderAP.DataSource = TotalAP;
            //TotalorderAP.DataBind();



            //strsqlTG = "select count(*) as TGTotalOrders,sum(roundoff_netamount) as TGTotalAmount from hsrprecords a,RTOLocation b  where a.rtolocationid=b.rtolocationid and a.hsrp_stateid=11 and hsrprecord_creationdate between '" + date + "' and '" + date1 + "'";
            //TotalTG = Utils.GetDataTable(strsqlTG, CnnString);
            //totalorderTG.DataSource = TotalTG;
            //totalorderTG.DataBind();




            //strsqlAP = "select count(*) as APTotalOrders,sum(roundoff_netamount) as APTotalAmount from hsrprecords a,RTOLocation b  where a.rtolocationid=b.rtolocationid and a.hsrp_stateid=9 AND vehicleregno='' and hsrprecord_creationdate between '" + date + "' and '" + date1 + "'";
            //TotalAP = Utils.GetDataTable(strsqlAP, CnnString);
            //grdWVAP.DataSource = TotalAP;
            //grdWVAP.DataBind();



            //strsqlTG = "select count(*) as TGTotalOrders,sum(roundoff_netamount) as TGTotalAmount from hsrprecords a,RTOLocation b  where a.rtolocationid=b.rtolocationid and a.hsrp_stateid=11 AND vehicleregno='' and hsrprecord_creationdate between '" + date + "' and '" + date1 + "'";
            //TotalTG = Utils.GetDataTable(strsqlTG, CnnString);
            //GRDWVTG.DataSource = TotalTG;
            //GRDWVTG.DataBind();


        }

       

      

        









        }
    }


