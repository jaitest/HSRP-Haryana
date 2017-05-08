using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using DataProvider;
using CarlosAg.ExcelXmlWriter;
using System.Drawing;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;
using System.Text;
using System.Configuration;
using System.Globalization;

namespace HSRP.Report
{
    public partial class DailyCashCollectionRTOLocationWise_WithMonth : System.Web.UI.Page
    {
        string strPath = string.Empty;
        string strMonth = string.Empty;
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        int UserType;
        string HSRPStateID;
        int RTOLocationID;
        int intHSRPStateID;
        int intRTOLocationID;
        string OrderStatus = string.Empty;
        DateTime AuthorizationDate;
        DateTime OrderDate1;
        DateTime OrderDate2;
        protected void Page_Load(object sender, EventArgs e)
        {
            Utils.GZipEncodePage();
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {
                UserType = Convert.ToInt32(Session["UserType"]);
                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                strUserID = Session["UID"].ToString();
                ComputerIP = Request.UserHostAddress;
                HSRPStateID = Session["UserHSRPStateID"].ToString();
                if (!IsPostBack)
                {
                    if (strUserID == "990")
                    {
                        //btnAllLocationPdf.Visible = false;
                    }
                    else
                    {
                        //btnAllLocationPdf.Visible = false;
                    }
                    InitialSetting();
                    try
                    {
                        InitialSetting();
                        if (UserType.Equals(0))
                        {
                            labelOrganization.Visible = true;
                            DropDownListStateName.Visible = true;
                            labelClient.Visible = true;
                            //btnAllLocationPdf.Visible = false;
                            dropDownListClient.Visible = true;
                            FilldropDownListOrganization();
                            FilldropDownListClient();
                            fillYear();
                            fillmonth();
                            // labelClient.Visible = false;
                            // dropDownListClient.Visible = false;

                        }
                        else
                        {
                            FilldropDownListOrganization();
                            FilldropDownListClient();
                            //btnAllLocationPdf.Visible = false;

                            labelClient.Visible = true;
                            dropDownListClient.Visible = true;
                            labelOrganization.Enabled = false;
                            DropDownListStateName.Enabled = false;
                            labelClient.Enabled = false;
                            // labelDate.Visible = false;

                            FilldropDownListClient();
                            FilldropDownListOrganization();
                            fillYear();
                            fillmonth();
                        }
                    }
                    catch (Exception err)
                    {
                        throw err;
                    }
                }
            }
        }

        private void InitialSetting()
        {

            
        }
        #region DropDown

        private void FilldropDownListOrganization()
        {
            if (strUserID == "3064")
                UserType = 8;

            if (UserType.Equals(0))
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where ActiveStatus='Y' Order by HSRPStateName";
                Utils.PopulateDropDownList(DropDownListStateName, SQLString.ToString(), CnnString, "--Select State--");
                // DropDownListStateName.SelectedIndex = DropDownListStateName.Items.Count - 1;
            }
            else
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID=" + HSRPStateID + " and ActiveStatus='Y' Order by HSRPStateName";
                DataSet dts = Utils.getDataSet(SQLString, CnnString);
                DropDownListStateName.DataSource = dts;
                DropDownListStateName.DataBind();

            }
        }

        private void FilldropDownListClient()
        {
            if (strUserID == "3064")
                UserType = 0;

            if (UserType.Equals(0))
            {
                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + intHSRPStateID + " and ActiveStatus!='N'  Order by RTOLocationName";

                Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), CnnString, "--Select Location--");
                // dropDownListClient.SelectedIndex = dropDownListClient.Items.Count - 1;
            }
            else
            {
                // SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where (RTOLocationID=" + RTOLocationID + " or distRelation=" + RTOLocationID + " ) and ActiveStatus!='N'   Order by RTOLocationName";
                SQLString = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID where  a.LocationType!='District' and UserRTOLocationMapping.UserID='" + strUserID + "' Order by RTOLocationName ";

                DataSet dss = Utils.getDataSet(SQLString, CnnString);
                dropDownListClient.DataSource = dss;
                dropDownListClient.DataBind();
            }
        }

        private void fillYear()
        {
            int currentYear = DateTime.Now.Year;
            for (int i = 2015; i <= currentYear; i++)
            {
                ddlYear.Items.Add(i.ToString());
            }           
            
        }
        private void fillmonth()
        {
            if (ddlYear.SelectedValue == "2015")
            {
                DateTimeFormatInfo date = new DateTimeFormatInfo();
                for (int i = 4; i < 13; i++)
                {
                    ddlMonth.Items.Add(new System.Web.UI.WebControls.ListItem(date.GetMonthName(i), i.ToString()));
                }
            }
            else
            {
                DateTimeFormatInfo date = new DateTimeFormatInfo();
                for (int i = 1; i < 13; i++)
                {
                    ddlMonth.Items.Add(new System.Web.UI.WebControls.ListItem(date.GetMonthName(i), i.ToString()));
                }
            }
        }

        #endregion

        protected void DropDownListStateName_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilldropDownListClient();
        }
        String DatePrint = string.Empty;
        

        private String MakeQueryOnTheBasisOfSelectedOption(String StringOrderDate, String StringAuthDate, String RtoCode)
        {
            if (ddlVehicleReference.SelectedItem.Text == "Both" && DropDownListVehicleModel.SelectedItem.Text == "All")
            {
                if (strUserID == "990")
                {
                    //    SQLString = "SELECT  HSRP_StateID,OrderStatus,case when orderstatus='Closed' and (orderembossingdate is null or orderembossingdate='') then convert(varchar(10),dateadd(dd,2,orderdate),103) else convert(varchar(10),orderembossingdate,103)end as embdate, CONVERT(varchar(20), HsrpRecord_AuthorizationDate, 103) AS HsrpRecord_AuthorizationDate,  CONVERT(varchar(20), HsrpRecord_creationdate, 103) AS NewOrderDate,CONVERT(varchar(20), OrderEmbossingDate, 103) AS OrderEmbossingDate,CONVERT(varchar(20), InvoiceDateTime, 103) AS InvoiceDateTime,  HSRPRecord_AuthorizationNo,CashReceiptNo, HSRPRecordID, VehicleRegNo, OwnerName, VehicleType, VehicleClass,CONVERT(numeric,round( RoundOff_NetAmount,0)) as NetAmount,OrderDate,EngineNo,ChassisNo,MobileNo,convert(varchar(10),ordercloseddate,103) as AffixDate" +
                    //        ",(select p.ProductCode from product p where h.FrontPlateSize = p.ProductID) as frontplatesize" +
                    //        ",(select p.ProductCode from product p where h.rearPlateSize = p.ProductID) as rearplatesize" +
                    //        ",hsrp_front_lasercode,hsrp_rear_lasercode,ownername,convert(varchar(15),HSRPRecord_CreationDate,103) as CashReceiptDateTime" +
                    //        " FROM HSRPRecords h where HSRP_StateID='" + DropDownListStateName.SelectedValue + "' and RTOLocationID='" + RtoCode + "' and  OwnerName is not null and OwnerName <> '' and Address1 is not null and Address1 <> '' and HsrpRecord_creationdate between '" + StringOrderDate.ToString() + "' and '" + StringAuthDate.ToString() + "' and NetAmount <> 0 order by OrderDate";


                    SQLString = "SELECT  HSRP_StateID,OrderStatus,case when orderstatus='Closed' and (orderembossingdate is null or orderembossingdate='') then convert(varchar(10),dateadd(dd,2,orderdate),103) else convert(varchar(10),orderembossingdate,103)end as embdate, CONVERT(varchar(20), HsrpRecord_AuthorizationDate, 103) AS HsrpRecord_AuthorizationDate,  CONVERT(varchar(20), HsrpRecord_creationdate, 103) AS NewOrderDate,CONVERT(varchar(20), OrderEmbossingDate, 103) AS OrderEmbossingDate,CONVERT(varchar(20), InvoiceDateTime, 103) AS InvoiceDateTime,  HSRPRecord_AuthorizationNo,CashReceiptNo, HSRPRecordID, VehicleRegNo, OwnerName, VehicleType, VehicleClass,CONVERT(numeric,round( RoundOff_NetAmount,0)) as NetAmount,OrderDate,EngineNo,ChassisNo,MobileNo,convert(varchar(10),ordercloseddate,103) as AffixDate" +
                           ",(select p.ProductCode from product p where h.FrontPlateSize = p.ProductID) as frontplatesize" +
                           ",(select p.ProductCode from product p where h.rearPlateSize = p.ProductID) as rearplatesize" +
                           ",hsrp_front_lasercode,hsrp_rear_lasercode,ownername,convert(varchar(15),HSRPRecord_CreationDate,103) as CashReceiptDateTime" +
                           " FROM HSRPRecords h where HSRP_StateID='" + DropDownListStateName.SelectedValue + "' and RTOLocationID='" + RtoCode + "' and  OwnerName is not null and OwnerName <> ''  and HsrpRecord_creationdate between '" + StringOrderDate.ToString() + "' and '" + StringAuthDate.ToString() + "' and NetAmount <> 0 order by OrderDate";



                }

                else
                {


                    SQLString = "SELECT  HSRP_StateID,OrderStatus,case when orderstatus='Closed' and (orderembossingdate is null or orderembossingdate='') then convert(varchar(10),dateadd(dd,2,orderdate),103) else convert(varchar(10),orderembossingdate,103)end as embdate, CONVERT(varchar(20), HsrpRecord_AuthorizationDate, 103) AS HsrpRecord_AuthorizationDate,  CONVERT(varchar(20), HsrpRecord_creationdate, 103) AS NewOrderDate,CONVERT(varchar(20), OrderEmbossingDate, 103) AS OrderEmbossingDate,CONVERT(varchar(20), InvoiceDateTime, 103) AS InvoiceDateTime,  HSRPRecord_AuthorizationNo,CashReceiptNo, HSRPRecordID, VehicleRegNo, OwnerName, VehicleType, VehicleClass,CONVERT(numeric,round( RoundOff_NetAmount,0)) as NetAmount,OrderDate,EngineNo,ChassisNo,MobileNo,convert(varchar(10),ordercloseddate,103) as AffixDate" +
                        ",(select p.ProductCode from product p where h.FrontPlateSize = p.ProductID) as frontplatesize" +
                        ",(select p.ProductCode from product p where h.rearPlateSize = p.ProductID) as rearplatesize" +
                        ",hsrp_front_lasercode,hsrp_rear_lasercode,ownername,convert(varchar(15),HSRPRecord_CreationDate,103) as CashReceiptDateTime,(select (userfirstname+' '+UserLastName) as Name from users u where h.createdby=u.userid) as username" +
                        " FROM HSRPRecords h where HSRP_StateID='" + DropDownListStateName.SelectedValue + "' and RTOLocationID='" + RtoCode + "' and  OwnerName is not null and OwnerName <> ''  and HsrpRecord_creationdate between '" + StringOrderDate.ToString() + "' and '" + StringAuthDate.ToString() + "' and NetAmount <> 0 order by OrderDate";


                    //    SQLString = "SELECT  HSRP_StateID,OrderStatus,case when orderstatus='Closed' and (orderembossingdate is null or orderembossingdate='') then convert(varchar(10),dateadd(dd,2,orderdate),103) else convert(varchar(10),orderembossingdate,103)end as embdate, CONVERT(varchar(20), HsrpRecord_AuthorizationDate, 103) AS HsrpRecord_AuthorizationDate,  CONVERT(varchar(20), HsrpRecord_creationdate, 103) AS NewOrderDate,CONVERT(varchar(20), OrderEmbossingDate, 103) AS OrderEmbossingDate,CONVERT(varchar(20), InvoiceDateTime, 103) AS InvoiceDateTime,  HSRPRecord_AuthorizationNo,CashReceiptNo, HSRPRecordID, VehicleRegNo, OwnerName, VehicleType, VehicleClass,CONVERT(numeric,round( RoundOff_NetAmount,0)) as NetAmount,OrderDate,EngineNo,ChassisNo,MobileNo,convert(varchar(10),ordercloseddate,103) as AffixDate" +
                    //        ",(select p.ProductCode from product p where h.FrontPlateSize = p.ProductID) as frontplatesize" +
                    //        ",(select p.ProductCode from product p where h.rearPlateSize = p.ProductID) as rearplatesize" +
                    //        ",hsrp_front_lasercode,hsrp_rear_lasercode,ownername,convert(varchar(15),HSRPRecord_CreationDate,103) as CashReceiptDateTime,(select (userfirstname+' '+UserLastName) as Name from users u where h.createdby=u.userid) as username" +
                    //        " FROM HSRPRecords h where HSRP_StateID='" + DropDownListStateName.SelectedValue + "' and RTOLocationID='" + RtoCode + "' and  OwnerName is not null and OwnerName <> '' and Address1 is not null and Address1 <> '' and HsrpRecord_creationdate between '" + StringOrderDate.ToString() + "' and '" + StringAuthDate.ToString() + "' and NetAmount <> 0 order by OrderDate";

                    //SQLString = "SELECT  HSRP_StateID,OrderStatus,case when orderstatus='Closed' and (orderembossingdate is null or orderembossingdate='') then convert(varchar(10),dateadd(dd,2,orderdate),103) else convert(varchar(10),orderembossingdate,103)end as embdate, CONVERT(varchar(20), HsrpRecord_AuthorizationDate, 103) AS HsrpRecord_AuthorizationDate,  CONVERT(varchar(20), HsrpRecord_creationdate, 103) AS NewOrderDate,CONVERT(varchar(20), OrderEmbossingDate, 103) AS OrderEmbossingDate,CONVERT(varchar(20), InvoiceDateTime, 103) AS InvoiceDateTime,  HSRPRecord_AuthorizationNo,CashReceiptNo, HSRPRecordID, VehicleRegNo, OwnerName, VehicleType, VehicleClass,CONVERT(numeric,round( RoundOff_NetAmount,0)) as NetAmount,OrderDate,EngineNo,ChassisNo,MobileNo,convert(varchar(10),ordercloseddate,103) as AffixDate" +
                    //   ",(select p.ProductCode from product p where h.FrontPlateSize = p.ProductID) as frontplatesize" +
                    //   ",(select p.ProductCode from product p where h.rearPlateSize = p.ProductID) as rearplatesize" +
                    //   ",hsrp_front_lasercode,hsrp_rear_lasercode,ownername,convert(varchar(15),HSRPRecord_CreationDate,103) as CashReceiptDateTime,(select (userfirstname+' '+UserLastName) as Name from users u where h.createdby=u.userid) as username" +
                    //   " FROM HSRPRecords h where HSRP_StateID='" + DropDownListStateName.SelectedValue + "' and RTOLocationID='" + RtoCode + "'  and HsrpRecord_creationdate between '" + StringOrderDate.ToString() + "' and '" + StringAuthDate.ToString() + "' and NetAmount <> 0 order by OrderDate";
                }



            }
            else if (ddlVehicleReference.SelectedItem.Text == "Both")
            {
                SQLString = "SELECT  HSRP_StateID,OrderStatus,case when orderstatus='Closed' and (orderembossingdate is null or orderembossingdate='') then convert(varchar(10),dateadd(dd,2,orderdate),103) else convert(varchar(10),orderembossingdate,103)end as embdate, CONVERT(varchar(20), HsrpRecord_AuthorizationDate, 103) AS HsrpRecord_AuthorizationDate,  CONVERT(varchar(20), HsrpRecord_creationdate, 103) AS NewOrderDate,CONVERT(varchar(20), OrderEmbossingDate, 103) AS OrderEmbossingDate,CONVERT(varchar(20), InvoiceDateTime, 103) AS InvoiceDateTime,  HSRPRecord_AuthorizationNo,CashReceiptNo, HSRPRecordID, VehicleRegNo, OwnerName, VehicleType, VehicleClass,CONVERT(numeric,round( RoundOff_NetAmount,0)) as NetAmount,OrderDate,EngineNo,ChassisNo,MobileNo,convert(varchar(10),ordercloseddate,103) as AffixDate" +
                    ",(select p.ProductCode from product p where h.FrontPlateSize = p.ProductID) as frontplatesize" +
                    ",(select p.ProductCode from product p where h.rearPlateSize = p.ProductID) as rearplatesize" +
                    ",hsrp_front_lasercode,hsrp_rear_lasercode,convert(varchar(15),HSRPRecord_CreationDate,103) as CashReceiptDateTime,ownername,(select (userfirstname+' '+UserLastName) as Name from users u where h.createdby=u.userid) as username" +
                     " FROM HSRPRecords h where HSRP_StateID='" + DropDownListStateName.SelectedValue + "' and RTOLocationID='" + RtoCode + "' and  OwnerName is not null and OwnerName <> '' and Address1 is not null and Address1 <> '' and VehicleType='" + DropDownListVehicleModel.SelectedItem.ToString() + "'  and HsrpRecord_creationdate between '" + StringOrderDate.ToString() + "' and '" + StringAuthDate.ToString() + "' and NetAmount <> 0 order by OrderDate";
            }
            else if (ddlVehicleReference.SelectedItem.Text == "New" || ddlVehicleReference.SelectedItem.Text == "Old" && DropDownListVehicleModel.SelectedItem.Text == "All")
            {
                SQLString = "SELECT  HSRP_StateID,OrderStatus,case when orderstatus='Closed' and (orderembossingdate is null or orderembossingdate='') then convert(varchar(10),dateadd(dd,2,orderdate),103) else convert(varchar(10),orderembossingdate,103)end as embdate, CONVERT(varchar(20), HsrpRecord_AuthorizationDate, 103) AS HsrpRecord_AuthorizationDate,  CONVERT(varchar(20), HsrpRecord_creationdate, 103) AS NewOrderDate,CONVERT(varchar(20), OrderEmbossingDate, 103) AS OrderEmbossingDate,CONVERT(varchar(20), InvoiceDateTime, 103) AS InvoiceDateTime,  HSRPRecord_AuthorizationNo,CashReceiptNo, HSRPRecordID, VehicleRegNo, OwnerName, VehicleType, VehicleClass,CONVERT(numeric,round( RoundOff_NetAmount,0)) as NetAmount,OrderDate,EngineNo,ChassisNo,MobileNo,convert(varchar(10),ordercloseddate,103) as AffixDate" +
                     ",(select p.ProductCode from product p where h.FrontPlateSize = p.ProductID) as frontplatesize" +
                    ",(select p.ProductCode from product p where h.rearPlateSize = p.ProductID) as rearplatesize" +
                    ",hsrp_front_lasercode,hsrp_rear_lasercode,convert(varchar(15),HSRPRecord_CreationDate,103) as CashReceiptDateTime,ownername,(select (userfirstname+' '+UserLastName) as Name from users u where h.createdby=u.userid) as username" +
                    " FROM HSRPRecords h where HSRP_StateID='" + DropDownListStateName.SelectedValue + "' and RTOLocationID='" + RtoCode + "' and  OwnerName is not null and OwnerName <> '' and Address1 is not null and Address1 <> '' and vehicleref='" + ddlVehicleReference.SelectedItem.ToString() + "' and HsrpRecord_creationdate between '" + StringOrderDate.ToString() + "' and '" + StringAuthDate.ToString() + "' and NetAmount <> 0 order by OrderDate";
            }
            else
            {
                SQLString = "SELECT  HSRP_StateID,OrderStatus,case when orderstatus='Closed' and (orderembossingdate is null or orderembossingdate='') then convert(varchar(10),dateadd(dd,2,orderdate),103) else convert(varchar(10),orderembossingdate,103)end as embdate, CONVERT(varchar(20), HsrpRecord_AuthorizationDate, 103) AS HsrpRecord_AuthorizationDate,  CONVERT(varchar(20), HsrpRecord_creationdate, 103) AS NewOrderDate,CONVERT(varchar(20), OrderEmbossingDate, 103) AS OrderEmbossingDate,CONVERT(varchar(20), InvoiceDateTime, 103) AS InvoiceDateTime,  HSRPRecord_AuthorizationNo,CashReceiptNo, HSRPRecordID, VehicleRegNo, OwnerName, VehicleType, VehicleClass,CONVERT(numeric,round( RoundOff_NetAmount,0)) as NetAmount,OrderDate,EngineNo,ChassisNo,MobileNo,convert(varchar(10),ordercloseddate,103) as AffixDate" +
                    ",(select p.ProductCode from product p where h.FrontPlateSize = p.ProductID) as frontplatesize" +
                    ",(select p.ProductCode from product p where h.rearPlateSize = p.ProductID) as rearplatesize" +
                    ",hsrp_front_lasercode,hsrp_rear_lasercode,ownername,convert(varchar(15),HSRPRecord_CreationDate,103) as CashReceiptDateTime,(select (userfirstname+' '+UserLastName) as Name from users u where h.createdby=u.userid) as username" +
                    " FROM HSRPRecords h where HSRP_StateID='" + DropDownListStateName.SelectedValue + "' and RTOLocationID='" + RtoCode + "' and  OwnerName is not null and OwnerName <> '' and Address1 is not null and Address1 <> '' and vehicleref='" + ddlVehicleReference.SelectedItem.ToString() + "' and VehicleType='" + DropDownListVehicleModel.SelectedItem.ToString() + "'  and HsrpRecord_creationdate between '" + StringOrderDate.ToString() + "' and '" + StringAuthDate.ToString() + "' and NetAmount <> 0 order by OrderDate";
            }
            return SQLString;
        }



        protected void Button1_Click(object sender, EventArgs e)
        {
            String StringOrderDate ="";// OrderDate.SelectedDate.ToString("yyyy/MM/dd") + " 00:00:00";
            String StringAuthDate = "";//HSRPAuthDate.SelectedDate.ToString("yyyy/MM/dd") + " 23:59:59";
            if (strUserID == "3064")
            {
                string aa = "SELECT DATEDIFF(day,'2013-08-01 00:00:00','" + StringOrderDate + "') AS DiffDate";
                aa = Utils.getDataSingleValue(aa, CnnString, "DiffDate");
                //if (ReportDate1 < "2013/08/01 00:00:00")

                if (Convert.ToInt32(aa) < 0)
                    StringOrderDate = "2013/08/01 00:00:00";

            }
            #region Query
            if (ddlVehicleReference.SelectedItem.Text == "Both" && DropDownListVehicleModel.SelectedItem.Text == "All")
            {
                SQLString = "SELECT  ROW_NUMBER() Over (Order by Hsrprecord_Creationdate) As SNo,convert(varchar(15),HSRPRecord_CreationDate,103) as CashReceiptDateTime,HSRPRecord_AuthorizationNo as ApplicationNo,CashReceiptNo,VehicleRegNo, OwnerName, VehicleType, VehicleClass,CONVERT(numeric,round( RoundOff_NetAmount,0)) as NetAmount,OrderDate,EngineNo,ChassisNo,MobileNo,Case when orderstatus='Closed' then 'Affixed' else '' end as AffixationStatus,convert(varchar(10),ordercloseddate,103) as AffixDate FROM HSRPRecords h where HSRP_StateID='" + DropDownListStateName.SelectedValue + "' and RTOLocationID='" + dropDownListClient.SelectedValue + "' and Year(HsrpRecord_creationdate)='"+ddlYear.SelectedValue+"' and Month(HsrpRecord_creationdate)='"+ddlMonth.SelectedValue+"' and NetAmount <> 0 order by Hsrprecord_Creationdate desc";
            }
            else if (ddlVehicleReference.SelectedItem.Text == "Both")
            {
                SQLString = "SELECT  ROW_NUMBER() Over (Order by Hsrprecord_Creationdate) As SNo,convert(varchar(15),HSRPRecord_CreationDate,103) as CashReceiptDateTime,HSRPRecord_AuthorizationNo as ApplicationNo,CashReceiptNo,VehicleRegNo, OwnerName, VehicleType, VehicleClass,CONVERT(numeric,round( RoundOff_NetAmount,0)) as NetAmount,OrderDate,EngineNo,ChassisNo,MobileNo,Case when orderstatus='Closed' then 'Affixed' else '' end as AffixationStatus,convert(varchar(10),ordercloseddate,103) as AffixDate FROM HSRPRecords h where HSRP_StateID='" + DropDownListStateName.SelectedValue + "' and RTOLocationID='" + dropDownListClient.SelectedValue + "' and VehicleType='" + DropDownListVehicleModel.SelectedItem.ToString() + "'  and Year(HsrpRecord_creationdate)='" + ddlYear.SelectedValue + "' and Month(HsrpRecord_creationdate)='" + ddlMonth.SelectedValue + "' and NetAmount <> 0 order by Hsrprecord_Creationdate desc";
            }
            else if (ddlVehicleReference.SelectedItem.Text == "New" || ddlVehicleReference.SelectedItem.Text == "Old" && DropDownListVehicleModel.SelectedItem.Text == "All")
            {
                SQLString = "SELECT  ROW_NUMBER() Over (Order by Hsrprecord_Creationdate) As SNo,convert(varchar(15),HSRPRecord_CreationDate,103) as CashReceiptDateTime,HSRPRecord_AuthorizationNo as ApplicationNo,CashReceiptNo,VehicleRegNo, OwnerName, VehicleType, VehicleClass,CONVERT(numeric,round( RoundOff_NetAmount,0)) as NetAmount,OrderDate,EngineNo,ChassisNo,MobileNo,Case when orderstatus='Closed' then 'Affixed' else '' end as AffixationStatus,convert(varchar(10),ordercloseddate,103) as AffixDate FROM HSRPRecords h where HSRP_StateID='" + DropDownListStateName.SelectedValue + "' and RTOLocationID='" + dropDownListClient.SelectedValue + "'  and vehicleref='" + ddlVehicleReference.SelectedItem.ToString() + "' and Year(HsrpRecord_creationdate)='" + ddlYear.SelectedValue + "' and Month(HsrpRecord_creationdate)='" + ddlMonth.SelectedValue + "' and NetAmount <> 0 order by Hsrprecord_Creationdate desc";
            }
            else
            {
                SQLString = "SELECT  ROW_NUMBER() Over (Order by Hsrprecord_Creationdate) As SNo,convert(varchar(15),HSRPRecord_CreationDate,103) as CashReceiptDateTime,HSRPRecord_AuthorizationNo as ApplicationNo,CashReceiptNo,VehicleRegNo, OwnerName, VehicleType, VehicleClass,CONVERT(numeric,round( RoundOff_NetAmount,0)) as NetAmount,OrderDate,EngineNo,ChassisNo,MobileNo,Case when orderstatus='Closed' then 'Affixed' else '' end as AffixationStatus,convert(varchar(10),ordercloseddate,103) as AffixDate FROM HSRPRecords h where HSRP_StateID='" + DropDownListStateName.SelectedValue + "' and RTOLocationID='" + dropDownListClient.SelectedValue + "' and vehicleref='" + ddlVehicleReference.SelectedItem.ToString() + "' and VehicleType='" + DropDownListVehicleModel.SelectedItem.ToString() + "'  and Year(HsrpRecord_creationdate)='" + ddlYear.SelectedValue + "' and Month(HsrpRecord_creationdate)='" + ddlMonth.SelectedValue + "' and NetAmount <> 0 order by Hsrprecord_Creationdate desc";
            }
            #endregion
            string filename = DropDownListStateName.SelectedItem.ToString() + "_CollectionReport_" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";

            Workbook book = new Workbook();            
            DataTable dtRecord = Utils.GetDataTable(SQLString, CnnString);
            if (dtRecord.Rows.Count > 0)
            {
                DownLoadPDF(dtRecord);
            }
            else
            {
                LabelError.Text = "No Record For the Selected Date.";
            }
        }
        public void DownLoadPDF(DataTable dt)
        {
            String ReportDateFrom = ddlYear.SelectedItem.ToString(); //OrderDate.SelectedDate.ToString("yyyy/MM/dd");
            String ReportDateTo = ddlMonth.SelectedItem.ToString();// HSRPAuthDate.SelectedDate.ToString("yyyy/MM/dd");
            int i = 0;
            if (dt.Rows.Count > 0)
            {
                i = dt.Rows.Count;
                string filename = DropDownListStateName.SelectedItem.ToString() + "_CollectionReport_" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".pdf";
                String StringField = String.Empty;
                String StringAlert = String.Empty;
                StringBuilder bb = new StringBuilder();
                Document document = new Document();
                BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
                // iTextSharp.text.Font times = new Font(bfTimes, 12, Font.ITALIC, Color.Black);
                //Creates a Writer that listens to this document and writes the document to the Stream of your choice:
                string PdfFolder = ConfigurationManager.AppSettings["PdfFolder"].ToString() + filename;
                PdfWriter.GetInstance(document, new FileStream(PdfFolder, FileMode.Create));

                //Opens the document:
                document.Open();

                //Adds content to the document:
                // document.Add(new Paragraph("Ignition Log Report"));
                PdfPTable table = new PdfPTable(14);
                //actual width of table in points
                table.TotalWidth = 2250f;

                PdfPCell cell120911 = new PdfPCell(new Phrase("HSRP Daily Collection Report", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell120911.Colspan = 14;
                cell120911.BorderWidthLeft = 0f;
                cell120911.BorderWidthRight = 0f;
                cell120911.BorderWidthTop = 0f;
                cell120911.BorderWidthBottom = 0f;

                cell120911.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell120911);

                PdfPCell cell12091 = new PdfPCell(new Phrase("State Name : " + DropDownListStateName.SelectedItem, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell12091.Colspan = 8;
                cell12091.BorderWidthLeft = 1f;
                cell12091.BorderWidthRight = 0f;
                cell12091.BorderWidthTop = 1f;
                cell12091.BorderWidthBottom = 0f;

                cell12091.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12091);

                //PdfPCell cell12096 = new PdfPCell(new Phrase("Vehicle Type : " + DropDownListVehicleModel.SelectedItem, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                //cell12096.Colspan = 6;
                //cell12096.BorderWidthLeft = 0f;
                //cell12096.BorderWidthRight = 0f;
                //cell12096.BorderWidthTop = 0f;
                //cell12096.BorderWidthBottom = 0f;

                //cell12091.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                //table.AddCell(cell12096);

                // PdfPCell cell12093 = new PdfPCell(new Phrase("Date : " + DateTime.Now.ToString("dd/MM/yyyy"), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                PdfPCell cell12093 = new PdfPCell(new Phrase("Report Year : " + ReportDateFrom, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell12093.Colspan = 6;
                cell12093.BorderWidthLeft = 0f;
                cell12093.BorderWidthRight = 1f;
                cell12093.BorderWidthTop = 1f;
                cell12093.BorderWidthBottom = 0f;

                cell12093.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12093);

                PdfPCell cell12092 = new PdfPCell(new Phrase("Location Name : " + dropDownListClient.SelectedItem, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell12092.Colspan = 5;
                cell12092.BorderWidthLeft = 1f;
                cell12092.BorderWidthRight = 0f;
                cell12092.BorderWidthTop = 0f;
                cell12092.BorderWidthBottom = 0f;

                cell12092.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12092);

                PdfPCell cell12094 = new PdfPCell(new Phrase("Vehicle Reference : " + ddlVehicleReference.SelectedItem, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell12094.Colspan = 6;
                cell12094.BorderWidthLeft = 0f;
                cell12094.BorderWidthRight = 0f;
                cell12094.BorderWidthTop = 0f;
                cell12094.BorderWidthBottom = 0f;

                cell12094.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12094);

                PdfPCell cell12095 = new PdfPCell(new Phrase("Report Month : " + ReportDateTo, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell12095.Colspan = 5;
                cell12095.BorderWidthLeft = 0f;
                cell12095.BorderWidthRight = 1f;
                cell12095.BorderWidthTop = 0f;
                cell12095.BorderWidthBottom = 0f;

                cell12095.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12095);


                PdfPCell cell1209 = new PdfPCell(new Phrase("S.No.", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell1209.Colspan = 1;
                cell1209.BorderWidthLeft = 1f;
                cell1209.BorderWidthRight = .8f;
                cell1209.BorderWidthTop = 1f;
                cell1209.BorderWidthBottom = 1f;

                cell1209.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1209);


                PdfPCell cell1210 = new PdfPCell(new Phrase("Cash Receipt Date", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell1210.Colspan = 1;
                cell1210.BorderWidthLeft = 0f;
                cell1210.BorderWidthRight = .8f;
                cell1210.BorderWidthTop = 1f;
                cell1210.BorderWidthBottom = 1f;

                cell1210.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1210);



                PdfPCell cell1213 = new PdfPCell(new Phrase("Application No.", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell1213.Colspan = 1;
                cell1213.BorderWidthLeft = 0f;
                cell1213.BorderWidthRight = .8f;
                cell1213.BorderWidthTop = 1f;
                cell1213.BorderWidthBottom = 1f;

                cell1213.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1213);



                PdfPCell cell12233 = new PdfPCell(new Phrase("Cash Receipt No.", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell12233.Colspan = 1;
                cell12233.BorderWidthLeft = 0f;
                cell12233.BorderWidthRight = .8f;
                cell12233.BorderWidthTop = 1f;
                cell12233.BorderWidthBottom = 1f;

                cell12233.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12233);

                PdfPCell cell122331 = new PdfPCell(new Phrase("Vehicle No.", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell122331.Colspan = 1;
                cell122331.BorderWidthLeft = 0f;
                cell122331.BorderWidthRight = .8f;
                cell122331.BorderWidthTop = 1f;
                cell122331.BorderWidthBottom = 1f;

                cell122331.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell122331);


                PdfPCell cell122332 = new PdfPCell(new Phrase("Owner Name", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell122332.Colspan = 1;
                cell122332.BorderWidthLeft = 0f;
                cell122332.BorderWidthRight = .8f;
                cell122332.BorderWidthTop = 1f;
                cell122332.BorderWidthBottom = 1f;

                cell122332.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell122332);

                PdfPCell cell1206 = new PdfPCell(new Phrase("Vehicle Type", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell1206.Colspan = 1;
                cell1206.BorderWidthLeft = 0f;
                cell1206.BorderWidthRight = .8f;
                cell1206.BorderWidthTop = 1f;
                cell1206.BorderWidthBottom = 1f;
                cell1206.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1206);

                PdfPCell cell1221 = new PdfPCell(new Phrase("VehicleClass", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell1221.Colspan = 1;
                cell1221.BorderWidthLeft = 0f;
                cell1221.BorderWidthRight = 1f;
                cell1221.BorderWidthTop = 1f;
                cell1221.BorderWidthBottom = 1f;

                cell1221.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1221);


                PdfPCell cell120933 = new PdfPCell(new Phrase("NetAmount", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell120933.Colspan = 1;
                cell120933.BorderWidthLeft = 1f;
                cell120933.BorderWidthRight = .8f;
                cell120933.BorderWidthTop = 1f;
                cell120933.BorderWidthBottom = 1f;

                cell120933.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell120933);



                PdfPCell cell120935 = new PdfPCell(new Phrase("Engine No", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell120935.Colspan = 1;
                cell120935.BorderWidthLeft = 1f;
                cell120935.BorderWidthRight = .8f;
                cell120935.BorderWidthTop = 1f;
                cell120935.BorderWidthBottom = 1f;

                cell120935.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell120935);
                
                PdfPCell cell120936 = new PdfPCell(new Phrase("Chassis No", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell120936.Colspan = 1;
                cell120936.BorderWidthLeft = 1f;
                cell120936.BorderWidthRight = .8f;
                cell120936.BorderWidthTop = 1f;
                cell120936.BorderWidthBottom = 1f;

                cell120936.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell120936);
                
                PdfPCell cell120937 = new PdfPCell(new Phrase("Mobile No", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell120937.Colspan = 1;
                cell120937.BorderWidthLeft = 1f;
                cell120937.BorderWidthRight = .8f;
                cell120937.BorderWidthTop = 1f;
                cell120937.BorderWidthBottom = 1f;

                cell120937.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell120937);
                
                PdfPCell cell120938 = new PdfPCell(new Phrase("Affixation Status", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell120938.Colspan = 1;
                cell120938.BorderWidthLeft = 1f;
                cell120938.BorderWidthRight = .8f;
                cell120938.BorderWidthTop = 1f;
                cell120938.BorderWidthBottom = 1f;

                cell120938.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right                
                table.AddCell(cell120938);
                
                PdfPCell cell1209349= new PdfPCell(new Phrase("AffixDate", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell1209349.Colspan = 1;
                cell1209349.BorderWidthLeft = 1f;
                cell1209349.BorderWidthRight = .8f;
                cell1209349.BorderWidthTop = 1f;
                cell1209349.BorderWidthBottom = 1f;

                cell1209349.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1209349);
                //PdfPCell cell1223 = new PdfPCell(new Phrase("Owner Name", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                //cell1223.Colspan = 2;
                //cell1223.BorderWidthLeft = 0f;
                //cell1223.BorderWidthRight = 1f;
                //cell1223.BorderWidthTop = 1f;
                //cell1223.BorderWidthBottom = 1f;
                //cell1223.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                //table.AddCell(cell1223);
                i = i - 1;
                while (i >= 0)
                {
                    PdfPCell cell1211 = new PdfPCell(new Phrase(dt.Rows[i]["SNo"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell1211.Colspan = 1;
                    cell1211.BorderWidthLeft = 1f;
                    cell1211.BorderWidthRight = .8f;
                    cell1211.BorderWidthTop = .5f;
                    cell1211.BorderWidthBottom = .5f;

                    cell1211.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell1211);

                    PdfPCell cell1212 = new PdfPCell(new Phrase(dt.Rows[i]["CashReceiptDateTime"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell1212.Colspan = 1;
                    cell1212.BorderWidthLeft = 1f;
                    cell1212.BorderWidthRight = .8f;
                    cell1212.BorderWidthTop = .5f;
                    cell1211.BorderWidthBottom = .5f;

                    cell1212.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell1212);




                    PdfPCell cell1214 = new PdfPCell(new Phrase(dt.Rows[i]["ApplicationNo"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell1214.Colspan = 1;
                    cell1214.BorderWidthLeft = 0f;
                    cell1214.BorderWidthRight = .8f;
                    cell1214.BorderWidthTop = .5f;
                    cell1214.BorderWidthBottom = .5f;

                    cell1214.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell1214);

                    PdfPCell cell1219 = new PdfPCell(new Phrase(dt.Rows[i]["CashReceiptNo"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell1219.Colspan = 1;
                    cell1219.BorderWidthLeft = 0f;
                    cell1219.BorderWidthRight = .8f;
                    cell1219.BorderWidthTop = .5f;
                    cell1219.BorderWidthBottom = .5f;

                    cell1219.HorizontalAlignment = 0; //0=Left, 1=     //PdfPCell cell1218 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));

                    table.AddCell(cell1219);


                    PdfPCell cell12193 = new PdfPCell(new Phrase(dt.Rows[i]["VehicleRegNo"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell12193.Colspan = 1;
                    cell12193.BorderWidthLeft = 0f;
                    cell12193.BorderWidthRight = .8f;
                    cell12193.BorderWidthTop = .5f;
                    cell12193.BorderWidthBottom = .5f;

                    cell12193.HorizontalAlignment = 0; //0=Left, 1=     //PdfPCell cell1218 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));

                    table.AddCell(cell12193);


                    PdfPCell cell12194 = new PdfPCell(new Phrase(dt.Rows[i]["OwnerName"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell12194.Colspan = 1;
                    cell12194.BorderWidthLeft = 0f;
                    cell12194.BorderWidthRight = .8f;
                    cell12194.BorderWidthTop = .5f;
                    cell12194.BorderWidthBottom = .5f;

                    cell12194.HorizontalAlignment = 0; //0=Left, 1=     //PdfPCell cell1218 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));

                    table.AddCell(cell12194);


                    PdfPCell cell1216 = new PdfPCell(new Phrase(dt.Rows[i]["VehicleType"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell1216.Colspan = 1;
                    cell1216.BorderWidthLeft = 0f;
                    cell1216.BorderWidthRight = .8f;
                    cell1216.BorderWidthTop = .5f;
                    cell1216.BorderWidthBottom = .5f;

                    cell1216.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell1216);


                    PdfPCell cell1222 = new PdfPCell(new Phrase(dt.Rows[i]["VehicleClass"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell1222.Colspan = 1;
                    cell1222.BorderWidthLeft = 0f;
                    cell1222.BorderWidthRight = .8f;
                    cell1222.BorderWidthTop = .5f;
                    cell1222.BorderWidthBottom = .5f;

                    cell1222.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell1222);




                    PdfPCell cell120939 = new PdfPCell(new Phrase(dt.Rows[i]["NetAmount"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell120939.Colspan = 1;
                    cell120939.BorderWidthLeft = 0f;
                    cell120939.BorderWidthRight = .8f;
                    cell120939.BorderWidthTop = .5f;
                    cell120939.BorderWidthBottom = .5f;

                    cell120935.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell120939);

                    PdfPCell cell120940 = new PdfPCell(new Phrase(dt.Rows[i]["EngineNo"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell120940.Colspan = 1;
                    cell120940.BorderWidthLeft = 0f;
                    cell120940.BorderWidthRight = .8f;
                    cell120940.BorderWidthTop = .5f;
                    cell120940.BorderWidthBottom = .5f;

                    cell120936.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell120940);

                    PdfPCell cell120941 = new PdfPCell(new Phrase(dt.Rows[i]["ChassisNo"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell120941.Colspan = 1;
                    cell120941.BorderWidthLeft = 0f;
                    cell120941.BorderWidthRight = .8f;
                    cell120941.BorderWidthTop = .5f;
                    cell120941.BorderWidthBottom = .5f;

                    cell120941.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell120941);

                    PdfPCell cell120942 = new PdfPCell(new Phrase(dt.Rows[i]["MobileNo"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell120942.Colspan = 1;
                    cell120942.BorderWidthLeft = 0f;
                    cell120942.BorderWidthRight = .8f;
                    cell120942.BorderWidthTop = .5f;
                    cell120942.BorderWidthBottom = .5f;

                    cell120942.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell120942);

                    PdfPCell cell120943 = new PdfPCell(new Phrase(dt.Rows[i]["AffixationStatus"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell120943.Colspan = 1;
                    cell120943.BorderWidthLeft = 0f;
                    cell120943.BorderWidthRight = .8f;
                    cell120943.BorderWidthTop = .5f;
                    cell120943.BorderWidthBottom = .5f;

                    cell120943.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell120943);

                    PdfPCell cell120944 = new PdfPCell(new Phrase(dt.Rows[i]["AffixDate"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell120944.Colspan = 1;
                    cell120944.BorderWidthLeft = 0f;
                    cell120944.BorderWidthRight = .8f;
                    cell120944.BorderWidthTop = .5f;
                    cell120944.BorderWidthBottom = .5f;

                    cell120944.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell120944);
                    i--;
                }


                //document.Add(table);
                //PdfPCell cell12241 = new PdfPCell(new Phrase("(Sign)", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                //cell12241.Colspan = 7;
                //cell12241.BorderWidthLeft = 0f;
                //cell12241.BorderWidthRight = 0f;
                //cell12241.BorderWidthTop = 0f;
                //cell12241.BorderWidthBottom = 0f;

                //cell12241.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                //table.AddCell(cell12241);

                document.Add(table);
                // document.Add(table1);

                document.Close();
                HttpContext context = HttpContext.Current;

                context.Response.ContentType = "Application/pdf";
                context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                context.Response.WriteFile(PdfFolder);
                context.Response.End();
            }
            else
            {
                string closescript1 = "<script>alert('No records found for selected date.')</script>";
                Page.RegisterStartupScript("abc", closescript1);
                return;
            }
        }
        




        

        private static void GenerateCell(PdfPTable table, int iSpan, int iLeftWidth, int iRightWidth, int iTopWidth, int iBottomWidth, int iAllign, int iFont, string strText, int iRowHeight, int iRowWidth)
        {
            PdfPCell newCellPDF = null;
            BaseFont bfTimes1 = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
            if (iFont.Equals(0))
            {
                newCellPDF = new PdfPCell(new Phrase(strText, new iTextSharp.text.Font(bfTimes1, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
            }
            else if (iFont.Equals(1))
            {
                newCellPDF = new PdfPCell(new Phrase(strText, new iTextSharp.text.Font(bfTimes1, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
            }
            newCellPDF.Colspan = iSpan;
            newCellPDF.BorderWidthLeft = iLeftWidth;
            newCellPDF.BorderWidthRight = iRightWidth;
            newCellPDF.BorderWidthTop = iTopWidth;
            newCellPDF.BorderWidthBottom = iBottomWidth;
            newCellPDF.HorizontalAlignment = iAllign;
            newCellPDF.NoWrap = false;
            if (!iRowHeight.Equals(0))
            {
                newCellPDF.FixedHeight = iRowHeight;
            }
            if (!iRowWidth.Equals(0))
            {
            }
            table.AddCell(newCellPDF);
        }



        private string SetFolder(string strRTO, string strState, string strFile)
        {
            string DateFolder = System.DateTime.Now.Day.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Year.ToString();
            strPath = "D:\\TenderReports";
            if (!Directory.Exists(strPath))
            {
                CreateFolder(DateFolder, strState, strPath);
                Directory.CreateDirectory(strPath + "\\" + strState);
                Directory.CreateDirectory(strPath + "\\" + strState + "\\" + DateFolder);
            }
            else
            {
                if (!Directory.Exists(strPath + "\\" + strState))
                {

                    Directory.CreateDirectory(strPath + "\\" + strState);
                    Directory.CreateDirectory(strPath + "\\" + strState + "\\" + DateFolder);

                }
                else
                {
                    if (!Directory.Exists(strPath + "\\" + strState + "\\" + DateFolder))
                    {

                        Directory.CreateDirectory(strPath + "\\" + strState + "\\" + DateFolder);

                    }
                    else
                    {
                        var files = Directory.GetFiles(strPath + "\\" + strState + "\\" + DateFolder, "*.*", SearchOption.AllDirectories);



                        //foreach (string file in files)
                        //{
                        //    if (file.StartsWith(strPath + "\\" + strState + "\\" + DateFolder + "\\" + strFile))
                        //    {
                        //        File.Delete(file);
                        //    }
                        //}
                    }
                }


            }
            return strPath = strPath + "\\" + strState + "\\" + DateFolder;
        }

        private static void CreateFolder(string strRTO, string strState, string strRTOLocFolderPath)
        {
            Directory.CreateDirectory(strRTOLocFolderPath);
            Directory.CreateDirectory(strRTOLocFolderPath + "\\" + strState);
            Directory.CreateDirectory(strRTOLocFolderPath + "\\" + strState + "\\" + strRTO);
        }


        private DataTable GetRtoLocation()
        {
            string sql = "select rtolocationname,rtolocationid from rtolocation where rtolocationid in (select distinct distrelation from rtolocation where hsrp_stateid='" + DropDownListStateName.SelectedValue + "') and RTOLocationID not in (148,331)";
            DataTable dtrto = Utils.GetDataTable(sql, CnnString);
            return dtrto;
        }

        protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlYear.SelectedValue == "2015")
            {
                ddlMonth.DataSource = "";
                ddlMonth.DataBind();
                DateTimeFormatInfo date = new DateTimeFormatInfo();
                for (int i = 4; i < 13; i++)
                {
                    ddlMonth.Items.Add(new System.Web.UI.WebControls.ListItem(date.GetMonthName(i), i.ToString()));
                }
            }
            else
            {
                ddlMonth.DataSource = "";
                ddlMonth.DataBind();
                DateTimeFormatInfo date = new DateTimeFormatInfo();
                for (int i = 1; i < 13; i++)
                {
                    ddlMonth.Items.Add(new System.Web.UI.WebControls.ListItem(date.GetMonthName(i), i.ToString()));
                }
            }
        }   
        


    }
}
