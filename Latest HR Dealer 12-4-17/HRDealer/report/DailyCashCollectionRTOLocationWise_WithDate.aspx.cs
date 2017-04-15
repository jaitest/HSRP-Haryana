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

namespace HSRP.Report
{
    public partial class DailyCashCollectionRTOLocationWise_WithDate : System.Web.UI.Page
    {
        string strPath = string.Empty;
        string strMonth = string.Empty;
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        int UserType;
        string HSRPStateID;
        
        int intHSRPStateID;
        int intRTOLocationID;
        string OrderStatus = string.Empty;
      
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
                        btnAllLocationPdf.Visible = true;
                    }
                    else
                    {
                        btnAllLocationPdf.Visible = true;
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

                            dropDownListClient.Visible = true;
                            FilldropDownListOrganization();
                            FilldropDownListClient();
                            // labelClient.Visible = false;
                            // dropDownListClient.Visible = false;

                        }
                        else
                        {
                            FilldropDownListOrganization();
                            FilldropDownListClient();

                            labelClient.Visible = true;
                            dropDownListClient.Visible = true;
                            labelOrganization.Enabled = false;
                            DropDownListStateName.Enabled = false;
                            labelClient.Enabled = false;
                            // labelDate.Visible = false;

                            FilldropDownListClient();
                            FilldropDownListOrganization();
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

            string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            string MaxDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            HSRPAuthDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            HSRPAuthDate.MaxDate = DateTime.Parse(MaxDate);
            CalendarHSRPAuthDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarHSRPAuthDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            OrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            OrderDate.MaxDate = DateTime.Parse(MaxDate);
            CalendarOrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarOrderDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
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

        #endregion

        protected void DropDownListStateName_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilldropDownListClient();
        }
        String DatePrint = string.Empty;
        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            String StringOrderDate = OrderDate.SelectedDate.ToString("yyyy/MM/dd") + " 00:00:00";
            String StringAuthDate = HSRPAuthDate.SelectedDate.ToString("yyyy/MM/dd") + " 23:59:59";
            DateTime StartDate = Convert.ToDateTime(OrderDate.SelectedDate);

            int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
            int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);

            if (strUserID == "3064")
            {
                string aa = "SELECT DATEDIFF(day,'2013-08-01 00:00:00','" + StringOrderDate + "') AS DiffDate";
                aa = Utils.getDataSingleValue(aa, CnnString, "DiffDate");

                if (Convert.ToInt32(aa) < 0)
                {
                    StringOrderDate = "2013/08/01 00:00:00";
                }
            }
            string filename = DropDownListStateName.SelectedItem.ToString() + "_CollectionReport_" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
            Workbook book = new Workbook();
            StyleForTheFirstTime(book);
            SQLString = MakeQueryOnTheBasisOfSelectedOption(StringOrderDate, StringAuthDate, dropDownListClient.SelectedValue.ToString());

            DataTable dtRecord = Utils.GetDataTable(SQLString, CnnString);
            if (dtRecord.Rows.Count > 0)
            {
                ExportRecordExcel(book, dtRecord);
                HttpContext context = HttpContext.Current;
                context.Response.Clear();

                book.Save(Response.OutputStream);
                context.Response.ContentType = "text/csv";
                context.Response.ContentType = "application/vnd.ms-excel";
                context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                context.Response.End();
            }
            else
            {
                LabelError.Text = "No Record For the Selected Date.";
            }

        }

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
                           " FROM HSRPRecords h where HSRP_StateID='" + DropDownListStateName.SelectedValue + "' and RTOLocationID='" + RtoCode + "'   and HsrpRecord_creationdate between '" + StringOrderDate.ToString() + "' and '" + StringAuthDate.ToString() + "' and NetAmount <> 0 order by OrderDate";
               
                
                
                }

                else
                {


                    SQLString = "SELECT  HSRP_StateID,OrderStatus,case when orderstatus='Closed' and (orderembossingdate is null or orderembossingdate='') then convert(varchar(10),dateadd(dd,2,orderdate),103) else convert(varchar(10),orderembossingdate,103)end as embdate, CONVERT(varchar(20), HsrpRecord_AuthorizationDate, 103) AS HsrpRecord_AuthorizationDate,  CONVERT(varchar(20), HsrpRecord_creationdate, 103) AS NewOrderDate,CONVERT(varchar(20), OrderEmbossingDate, 103) AS OrderEmbossingDate,CONVERT(varchar(20), InvoiceDateTime, 103) AS InvoiceDateTime,  HSRPRecord_AuthorizationNo,CashReceiptNo, HSRPRecordID, VehicleRegNo, OwnerName, VehicleType, VehicleClass,CONVERT(numeric,round( RoundOff_NetAmount,0)) as NetAmount,OrderDate,EngineNo,ChassisNo,MobileNo,convert(varchar(10),ordercloseddate,103) as AffixDate" +
                        ",(select p.ProductCode from product p where h.FrontPlateSize = p.ProductID) as frontplatesize" +
                        ",(select p.ProductCode from product p where h.rearPlateSize = p.ProductID) as rearplatesize" +
                        ",hsrp_front_lasercode,hsrp_rear_lasercode,ownername,convert(varchar(15),HSRPRecord_CreationDate,103) as CashReceiptDateTime,(select (userfirstname+' '+UserLastName) as Name from users u where h.createdby=u.userid) as username" +
                        " FROM HSRPRecords h where HSRP_StateID='" + DropDownListStateName.SelectedValue + "' and RTOLocationID='" + RtoCode + "'   and HsrpRecord_creationdate between '" + StringOrderDate.ToString() + "' and '" + StringAuthDate.ToString() + "' and NetAmount <> 0 order by OrderDate";


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
                     " FROM HSRPRecords h where HSRP_StateID='" + DropDownListStateName.SelectedValue + "' and RTOLocationID='" + RtoCode + "'  and Address1 is not null and Address1 <> '' and VehicleType='" + DropDownListVehicleModel.SelectedItem.ToString() + "'  and HsrpRecord_creationdate between '" + StringOrderDate.ToString() + "' and '" + StringAuthDate.ToString() + "' and NetAmount <> 0 order by OrderDate";
            }
            else if (ddlVehicleReference.SelectedItem.Text == "New" || ddlVehicleReference.SelectedItem.Text == "Old" && DropDownListVehicleModel.SelectedItem.Text == "All")
            {
                SQLString = "SELECT  HSRP_StateID,OrderStatus,case when orderstatus='Closed' and (orderembossingdate is null or orderembossingdate='') then convert(varchar(10),dateadd(dd,2,orderdate),103) else convert(varchar(10),orderembossingdate,103)end as embdate, CONVERT(varchar(20), HsrpRecord_AuthorizationDate, 103) AS HsrpRecord_AuthorizationDate,  CONVERT(varchar(20), HsrpRecord_creationdate, 103) AS NewOrderDate,CONVERT(varchar(20), OrderEmbossingDate, 103) AS OrderEmbossingDate,CONVERT(varchar(20), InvoiceDateTime, 103) AS InvoiceDateTime,  HSRPRecord_AuthorizationNo,CashReceiptNo, HSRPRecordID, VehicleRegNo, OwnerName, VehicleType, VehicleClass,CONVERT(numeric,round( RoundOff_NetAmount,0)) as NetAmount,OrderDate,EngineNo,ChassisNo,MobileNo,convert(varchar(10),ordercloseddate,103) as AffixDate" +
                     ",(select p.ProductCode from product p where h.FrontPlateSize = p.ProductID) as frontplatesize" +
                    ",(select p.ProductCode from product p where h.rearPlateSize = p.ProductID) as rearplatesize" +
                    ",hsrp_front_lasercode,hsrp_rear_lasercode,convert(varchar(15),HSRPRecord_CreationDate,103) as CashReceiptDateTime,ownername,(select (userfirstname+' '+UserLastName) as Name from users u where h.createdby=u.userid) as username" +
                    " FROM HSRPRecords h where HSRP_StateID='" + DropDownListStateName.SelectedValue + "' and RTOLocationID='" + RtoCode + "'  and Address1 is not null and Address1 <> '' and vehicleref='" + ddlVehicleReference.SelectedItem.ToString() + "' and HsrpRecord_creationdate between '" + StringOrderDate.ToString() + "' and '" + StringAuthDate.ToString() + "' and NetAmount <> 0 order by OrderDate";
            }
            else
            {
                SQLString = "SELECT  HSRP_StateID,OrderStatus,case when orderstatus='Closed' and (orderembossingdate is null or orderembossingdate='') then convert(varchar(10),dateadd(dd,2,orderdate),103) else convert(varchar(10),orderembossingdate,103)end as embdate, CONVERT(varchar(20), HsrpRecord_AuthorizationDate, 103) AS HsrpRecord_AuthorizationDate,  CONVERT(varchar(20), HsrpRecord_creationdate, 103) AS NewOrderDate,CONVERT(varchar(20), OrderEmbossingDate, 103) AS OrderEmbossingDate,CONVERT(varchar(20), InvoiceDateTime, 103) AS InvoiceDateTime,  HSRPRecord_AuthorizationNo,CashReceiptNo, HSRPRecordID, VehicleRegNo, OwnerName, VehicleType, VehicleClass,CONVERT(numeric,round( RoundOff_NetAmount,0)) as NetAmount,OrderDate,EngineNo,ChassisNo,MobileNo,convert(varchar(10),ordercloseddate,103) as AffixDate" +
                    ",(select p.ProductCode from product p where h.FrontPlateSize = p.ProductID) as frontplatesize" +
                    ",(select p.ProductCode from product p where h.rearPlateSize = p.ProductID) as rearplatesize" +
                    ",hsrp_front_lasercode,hsrp_rear_lasercode,ownername,convert(varchar(15),HSRPRecord_CreationDate,103) as CashReceiptDateTime,(select (userfirstname+' '+UserLastName) as Name from users u where h.createdby=u.userid) as username" +
                    " FROM HSRPRecords h where HSRP_StateID='" + DropDownListStateName.SelectedValue + "' and RTOLocationID='" + RtoCode + "'  and Address1 is not null and Address1 <> '' and vehicleref='" + ddlVehicleReference.SelectedItem.ToString() + "' and VehicleType='" + DropDownListVehicleModel.SelectedItem.ToString() + "'  and HsrpRecord_creationdate between '" + StringOrderDate.ToString() + "' and '" + StringAuthDate.ToString() + "' and NetAmount <> 0 order by OrderDate";
            }
            return SQLString;
        }

        #region Excel Creation
        private static void StyleForTheFirstTime(Workbook book)
        {

            // Specify which Sheet should be opened and the size of window by default
            book.ExcelWorkbook.ActiveSheetIndex = 1;
            book.ExcelWorkbook.WindowTopX = 100;
            book.ExcelWorkbook.WindowTopY = 200;
            book.ExcelWorkbook.WindowHeight = 7000;
            book.ExcelWorkbook.WindowWidth = 8000;

            // Some optional properties of the Document
            book.Properties.Author = "HSRP";
            book.Properties.Title = "HSRP Daily Cash Collection";
            book.Properties.Created = DateTime.Now;


            // Add some styles to the Workbook
            WorksheetStyle style = book.Styles.Add("HeaderStyle");
            style.Font.FontName = "Tahoma";
            style.Font.Size = 9;
            style.Font.Bold = false;
            style.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            style.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            style.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            style.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            style.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


            WorksheetStyle style8 = book.Styles.Add("HeaderStyle8");
            style8.Font.FontName = "Tahoma";
            style8.Font.Size = 10;
            style8.Font.Bold = true;
            style8.Alignment.Horizontal = StyleHorizontalAlignment.Right;
            style8.Interior.Color = "#D4CDCD";
            style8.Interior.Pattern = StyleInteriorPattern.Solid;

            WorksheetStyle style5 = book.Styles.Add("HeaderStyle5");
            style5.Font.FontName = "Tahoma";
            style5.Font.Size = 10;
            style5.Font.Bold = false;
            style5.Alignment.Horizontal = StyleHorizontalAlignment.Right;
            style5.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            style5.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            style5.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            style5.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


            WorksheetStyle style6 = book.Styles.Add("HeaderStyle6");
            style6.Font.FontName = "Tahoma";
            style6.Font.Size = 10;
            style6.Font.Bold = true;
            style6.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            style6.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            style6.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            style6.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            style6.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


            WorksheetStyle style2 = book.Styles.Add("HeaderStyle2");
            style2.Font.FontName = "Tahoma";
            style2.Font.Size = 10;
            style2.Font.Bold = true;
            style.Alignment.Horizontal = StyleHorizontalAlignment.Left;


            WorksheetStyle style3 = book.Styles.Add("HeaderStyle3");
            style3.Font.FontName = "Tahoma";
            style3.Font.Size = 12;
            style3.Font.Bold = true;
            style3.Alignment.Horizontal = StyleHorizontalAlignment.Left;

            WorksheetStyle style9 = book.Styles.Add("HeaderStyle9");
            style9.Font.FontName = "Tahoma";
            style9.Font.Size = 10;
            style9.Font.Bold = true;
            style9.Alignment.Horizontal = StyleHorizontalAlignment.Right;
            style9.Interior.Color = "#FCF6AE";
            style9.Interior.Pattern = StyleInteriorPattern.Solid;


        }
        private void ExportRecordExcel(Workbook book, DataTable dt, string RtoName = null, string RtoCode = null)
        {

            try
            {
                if (ddlVehicleReference.SelectedItem.Text == "Select Vehicle Reference")
                {
                    LabelError.Text = "Please Select Vehicle Reference";
                    return;
                }
                LabelError.Text = "";
                string strRtoName = String.Empty;
                string strRtoCode = String.Empty;
                if (string.IsNullOrEmpty(RtoName))
                {
                    strRtoName = dropDownListClient.SelectedItem.ToString();
                }
                else
                {
                    strRtoName = RtoName;
                }

                Worksheet sheet = book.Worksheets.Add(strRtoName);
                sheet.Table.Columns.Add(new WorksheetColumn(60));
                sheet.Table.Columns.Add(new WorksheetColumn(100));
                sheet.Table.Columns.Add(new WorksheetColumn(100));
                sheet.Table.Columns.Add(new WorksheetColumn(100));

                sheet.Table.Columns.Add(new WorksheetColumn(90));
                sheet.Table.Columns.Add(new WorksheetColumn(90));
                sheet.Table.Columns.Add(new WorksheetColumn(92));
                sheet.Table.Columns.Add(new WorksheetColumn(90));
                sheet.Table.Columns.Add(new WorksheetColumn(90));
                sheet.Table.Columns.Add(new WorksheetColumn(90));
                sheet.Table.Columns.Add(new WorksheetColumn(90));
                sheet.Table.Columns.Add(new WorksheetColumn(90));
                sheet.Table.Columns.Add(new WorksheetColumn(90));
                sheet.Table.Columns.Add(new WorksheetColumn(90));
                sheet.Table.Columns.Add(new WorksheetColumn(90));
                WorksheetRow row = sheet.Table.Rows.Add();

                row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                row.Cells.Add(new WorksheetCell("Report :", "HeaderStyle3"));
                WorksheetCell cell = row.Cells.Add("HSRP Daily Cash Collection");
                cell.MergeAcross = 3; // Merge two cells together
                cell.StyleID = "HeaderStyle3";

                row = sheet.Table.Rows.Add();
                row.Index = 3;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("State:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(DropDownListStateName.SelectedItem.ToString(), "HeaderStyle2"));

                //row = sheet.Table.Rows.Add();
                //row.Index = 4;
                //row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                //row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("Location:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(strRtoName, "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("Vehicle Reference:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(ddlVehicleReference.SelectedItem.ToString(), "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("Vehicle Type:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(DropDownListVehicleModel.SelectedItem.ToString(), "HeaderStyle2"));

                row = sheet.Table.Rows.Add();
                row.Index = 4;

                DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));

                String StringOrderDate = OrderDate.SelectedDate.ToString("yyyy/MM/dd");
                String StringAuthDate = HSRPAuthDate.SelectedDate.ToString("yyyy/MM/dd");

                row.Cells.Add(new WorksheetCell("Report Date:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(StringOrderDate + " To " + StringAuthDate, "HeaderStyle2"));
                row = sheet.Table.Rows.Add();


                row.Index = 5;

                //row.Cells.Add("Order Date");
                row.Cells.Add(new WorksheetCell("S.No.", "HeaderStyle6"));
                //  row.Cells.Add(new WorksheetCell("Authorisation No", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Cash Receipt No", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Cash Receipt Date", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Authorisation Date", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Order Date", "HeaderStyle6"));
                //row.Cells.Add(new WorksheetCell("Close Date", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Vehicle Reg. No", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Owner's Name", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Vechicle Type", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Vehicle Class", "HeaderStyle6"));

                if (DropDownListStateName.SelectedItem.ToString().ToUpper() == "DELHI")
                {
                    row.Cells.Add(new WorksheetCell("Affixation Status", "HeaderStyle6"));

                }
                if (DropDownListStateName.SelectedItem.ToString() == "BIHAR")
                {
                    row.Cells.Add(new WorksheetCell("EngineNo", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("ChassisNo", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("MobileNo", "HeaderStyle6"));
                }
                row.Cells.Add(new WorksheetCell("Embossed Date", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Affixation Date", "HeaderStyle6"));
                if (strUserID == "990")
                {
                    // row.Cells.Add(new WorksheetCell("Created By", "HeaderStyle6"));
                }
                else
                {
                    row.Cells.Add(new WorksheetCell("Created By", "HeaderStyle6"));
                }
                row.Cells.Add(new WorksheetCell("Amount", "HeaderStyle6"));

                //row.Cells.Add(new WorksheetCell("Delay in No. of Days", "HeaderStyle"));

                row = sheet.Table.Rows.Add();
                String StringField = String.Empty;
                String StringAlert = String.Empty;

                row.Index = 7;

                UserType = Convert.ToInt32(Session["UserType"]);
                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                strUserID = Session["UID"].ToString();

                string RTOColName = string.Empty;
                decimal totalAmount = 0;
                if (dt.Rows.Count > 0)
                {
                    int sno = 0;
                    foreach (DataRow dtrows in dt.Rows) // Loop over the rows.
                    {
                        sno = sno + 1;
                        if (sno == 43)
                        {
                            int ssno = sno;
                        }
                        row.Cells.Add(new WorksheetCell(Convert.ToInt16(sno).ToString(), DataType.String, "HeaderStyle"));
                        //  row.Cells.Add(new WorksheetCell(dtrows["HSRPRecord_AuthorizationNo"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["CashReceiptNo"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["CashReceiptDateTime"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["HsrpRecord_AuthorizationDate"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["NewOrderDate"].ToString(), DataType.String, "HeaderStyle"));
                        //row.Cells.Add(new WorksheetCell(dtrows["InvoiceDateTime"].ToString(), DataType.String, "HeaderStyle"));

                        row.Cells.Add(new WorksheetCell(dtrows["VehicleRegNo"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["OwnerName"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["VehicleClass"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["VehicleType"].ToString(), DataType.String, "HeaderStyle"));

                        if (DropDownListStateName.SelectedItem.ToString().ToUpper() == "DELHI")
                        {
                            string status = string.Empty;

                            status = dtrows["OrderStatus"].ToString().ToUpper() == "CLOSED" ? "AFFIXED" : dtrows["OrderStatus"].ToString().ToUpper().Equals("NEW ORDER") ? "" : "CUSTOMER NOT COME .";

                            row.Cells.Add(new WorksheetCell(status, DataType.String, "HeaderStyle"));
                        }
                        //Modification done by Himanshu saini On 19/7/2013
                        if (DropDownListStateName.SelectedItem.ToString() == "BIHAR")
                        {
                            row.Cells.Add(new WorksheetCell(dtrows["EngineNo"].ToString(), DataType.String, "HeaderStyle"));
                            row.Cells.Add(new WorksheetCell(dtrows["ChassisNo"].ToString(), DataType.String, "HeaderStyle"));
                            row.Cells.Add(new WorksheetCell(dtrows["MobileNo"].ToString(), DataType.String, "HeaderStyle"));

                        }
                        row.Cells.Add(new WorksheetCell(dtrows["embdate"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["AffixDate"].ToString(), DataType.String, "HeaderStyle"));
                        if (strUserID == "990")
                        {
                            //row.Cells.Add(new WorksheetCell(dtrows["username"].ToString(), DataType.String, "HeaderStyle"));
                        }
                        else
                        {
                            row.Cells.Add(new WorksheetCell(dtrows["username"].ToString(), DataType.String, "HeaderStyle"));
                        }
                        string amount = dtrows["NetAmount"].ToString();

                        if (amount == "")
                        {

                            row.Cells.Add(new WorksheetCell("0", DataType.String, "HeaderStyle"));
                            //totalAmount = totalAmount + Math.Round(Convert.ToDecimal(amount.ToString()));
                        }
                        else
                        {
                            row.Cells.Add(new WorksheetCell(Math.Round(Convert.ToDecimal(dtrows["NetAmount"].ToString())).ToString(), DataType.Number, "HeaderStyle5"));
                            totalAmount = totalAmount + Math.Round(Convert.ToDecimal(dtrows["NetAmount"].ToString()));
                        }


                        row = sheet.Table.Rows.Add();
                    }
                    row = sheet.Table.Rows.Add();
                    row = sheet.Table.Rows.Add();
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle8"));
                    row.Cells.Add(new WorksheetCell("Total :", "HeaderStyle8"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle8"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle8"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle8"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle8"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle8"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle8"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle8"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle8"));
                    //   row.Cells.Add(new WorksheetCell("", "HeaderStyle8"));
                    if (DropDownListStateName.SelectedItem.ToString().ToUpper() == "DELHI")
                    {
                        row.Cells.Add(new WorksheetCell("", "HeaderStyle8"));
                        row.Cells.Add(new WorksheetCell("", "HeaderStyle8"));
                    }
                    if (DropDownListStateName.SelectedItem.ToString() == "BIHAR")
                    {
                        row.Cells.Add(new WorksheetCell("", "HeaderStyle8"));
                        row.Cells.Add(new WorksheetCell("", "HeaderStyle8"));
                        row.Cells.Add(new WorksheetCell("", "HeaderStyle8"));
                    }
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle8"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle8"));
                    row.Cells.Add(new WorksheetCell((totalAmount).ToString(), "HeaderStyle8"));


                    totalAmount = 0;

                    row = sheet.Table.Rows.Add();
                }
                else
                {
                    LabelError.Text = "No Record Found For The Selected Date";
                }
            }

            catch (Exception ex)
            {
                // LabelError.Text = "Error in Populating Grid :" + ex.Message.ToString();
            }
        }
        #endregion

        protected void Button1_Click(object sender, EventArgs e)
        {
            String StringOrderDate = OrderDate.SelectedDate.ToString("yyyy/MM/dd") + " 00:00:00";
            String StringAuthDate = HSRPAuthDate.SelectedDate.ToString("yyyy/MM/dd") + " 23:59:59";
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
                SQLString = "SELECT  HSRP_StateID,convert(varchar(15),HSRPRecord_CreationDate,103) as CashReceiptDateTime,(select (userfirstname+' '+UserLastName) as Name from users u where h.createdby=u.userid) as username,OrderStatus,case when orderstatus='Closed' and (orderembossingdate is null or orderembossingdate='') then convert(varchar(10),dateadd(dd,2,orderdate),103) else convert(varchar(10),orderembossingdate,103)end as embdate, CONVERT(varchar(20), HsrpRecord_AuthorizationDate, 103) AS HsrpRecord_AuthorizationDate,  CONVERT(varchar(20), HsrpRecord_creationdate, 103) AS NewOrderDate,CONVERT(varchar(20), OrderEmbossingDate, 103) AS OrderEmbossingDate,CONVERT(varchar(20), InvoiceDateTime, 103) AS InvoiceDateTime,  HSRPRecord_AuthorizationNo,CashReceiptNo, HSRPRecordID, VehicleRegNo, OwnerName, VehicleType, VehicleClass,CONVERT(numeric,round( RoundOff_NetAmount,0)) as NetAmount,OrderDate,EngineNo,ChassisNo,MobileNo,convert(varchar(10),ordercloseddate,103) as AffixDate FROM HSRPRecords h where HSRP_StateID='" + DropDownListStateName.SelectedValue + "' and RTOLocationID='" + dropDownListClient.SelectedValue + "' and HsrpRecord_creationdate between '" + StringOrderDate + "' and '" + StringAuthDate + "' and NetAmount <> 0 order by OrderDate";
            }
            else if (ddlVehicleReference.SelectedItem.Text == "Both")
            {
                SQLString = "SELECT  HSRP_StateID,convert(varchar(15),HSRPRecord_CreationDate,103) as CashReceiptDateTime,(select (userfirstname+' '+UserLastName) as Name from users u where h.createdby=u.userid) as username,OrderStatus,case when orderstatus='Closed' and (orderembossingdate is null or orderembossingdate='') then convert(varchar(10),dateadd(dd,2,orderdate),103) else convert(varchar(10),orderembossingdate,103)end as embdate, CONVERT(varchar(20), HsrpRecord_AuthorizationDate, 103) AS HsrpRecord_AuthorizationDate,  CONVERT(varchar(20), HsrpRecord_creationdate, 103) AS NewOrderDate,CONVERT(varchar(20), OrderEmbossingDate, 103) AS OrderEmbossingDate,CONVERT(varchar(20), InvoiceDateTime, 103) AS InvoiceDateTime,  HSRPRecord_AuthorizationNo,CashReceiptNo, HSRPRecordID, VehicleRegNo, OwnerName, VehicleType, VehicleClass,CONVERT(numeric,round( RoundOff_NetAmount,0)) as NetAmount,OrderDate,EngineNo,ChassisNo,MobileNo,convert(varchar(10),ordercloseddate,103) as AffixDate FROM HSRPRecords h where HSRP_StateID='" + DropDownListStateName.SelectedValue + "' and RTOLocationID='" + dropDownListClient.SelectedValue + "' and VehicleType='" + DropDownListVehicleModel.SelectedItem.ToString() + "'  and HsrpRecord_creationdate between '" + StringOrderDate + "' and '" + StringAuthDate + "' and NetAmount <> 0 order by OrderDate";
            }
            else if (ddlVehicleReference.SelectedItem.Text == "New" || ddlVehicleReference.SelectedItem.Text == "Old" && DropDownListVehicleModel.SelectedItem.Text == "All")
            {
                SQLString = "SELECT  HSRP_StateID,convert(varchar(15),HSRPRecord_CreationDate,103) as CashReceiptDateTime,(select (userfirstname+' '+UserLastName) as Name from users u where h.createdby=u.userid) as username,OrderStatus,case when orderstatus='Closed' and (orderembossingdate is null or orderembossingdate='') then convert(varchar(10),dateadd(dd,2,orderdate),103) else convert(varchar(10),orderembossingdate,103)end as embdate, CONVERT(varchar(20), HsrpRecord_AuthorizationDate, 103) AS HsrpRecord_AuthorizationDate,  CONVERT(varchar(20), HsrpRecord_creationdate, 103) AS NewOrderDate,CONVERT(varchar(20), OrderEmbossingDate, 103) AS OrderEmbossingDate,CONVERT(varchar(20), InvoiceDateTime, 103) AS InvoiceDateTime,  HSRPRecord_AuthorizationNo,CashReceiptNo, HSRPRecordID, VehicleRegNo, OwnerName, VehicleType, VehicleClass,CONVERT(numeric,round( RoundOff_NetAmount,0)) as NetAmount,OrderDate,EngineNo,ChassisNo,MobileNo,convert(varchar(10),ordercloseddate,103) as AffixDate FROM HSRPRecords h where HSRP_StateID='" + DropDownListStateName.SelectedValue + "' and RTOLocationID='" + dropDownListClient.SelectedValue + "'  and vehicleref='" + ddlVehicleReference.SelectedItem.ToString() + "' and HsrpRecord_creationdate between '" + StringOrderDate + "' and '" + StringAuthDate + "' and NetAmount <> 0 order by OrderDate";
            }
            else
            {
                SQLString = "SELECT  HSRP_StateID,convert(varchar(15),HSRPRecord_CreationDate,103) as CashReceiptDateTime,(select (userfirstname+' '+UserLastName) as Name from users u where h.createdby=u.userid) as username,OrderStatus,case when orderstatus='Closed' and (orderembossingdate is null or orderembossingdate='') then convert(varchar(10),dateadd(dd,2,orderdate),103) else convert(varchar(10),orderembossingdate,103)end as embdate, CONVERT(varchar(20), HsrpRecord_AuthorizationDate, 103) AS HsrpRecord_AuthorizationDate,  CONVERT(varchar(20), HsrpRecord_creationdate, 103) AS NewOrderDate,CONVERT(varchar(20), OrderEmbossingDate, 103) AS OrderEmbossingDate,CONVERT(varchar(20), InvoiceDateTime, 103) AS InvoiceDateTime,  HSRPRecord_AuthorizationNo,CashReceiptNo, HSRPRecordID, VehicleRegNo, OwnerName, VehicleType, VehicleClass,CONVERT(numeric,round( RoundOff_NetAmount,0)) as NetAmount,OrderDate,EngineNo,ChassisNo,MobileNo,convert(varchar(10),ordercloseddate,103) as AffixDate FROM HSRPRecords h where HSRP_StateID='" + DropDownListStateName.SelectedValue + "' and RTOLocationID='" + dropDownListClient.SelectedValue + "' and vehicleref='" + ddlVehicleReference.SelectedItem.ToString() + "' and VehicleType='" + DropDownListVehicleModel.SelectedItem.ToString() + "'  and HsrpRecord_creationdate between '" + StringOrderDate + "' and '" + StringAuthDate + "' and NetAmount <> 0 order by OrderDate";
            }
            #endregion
            string filename = DropDownListStateName.SelectedItem.ToString() + "_CollectionReport_" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";

            Workbook book = new Workbook();
            StyleForTheFirstTime(book);
            DataTable dtRecord = Utils.GetDataTable(SQLString, CnnString);
            if (dtRecord.Rows.Count > 0)
            {
                ExportRecordExcel(book, dtRecord);
                HttpContext context = HttpContext.Current;
                context.Response.Clear();

                book.Save(Response.OutputStream);
                context.Response.ContentType = "text/csv";
                context.Response.ContentType = "application/vnd.ms-excel";
                context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                context.Response.End();
            }
            else
            {
                LabelError.Text = "No Record For the Selected Date.";
            }
        }
        protected void btnAllLocationPdf_Click(object sender, EventArgs e)
        {
            int iCheckAllRtoHasNoRecord = 0;
            String StringOrderDate = OrderDate.SelectedDate.ToString("yyyy/MM/dd") + " 00:00:00";
            String StringAuthDate = HSRPAuthDate.SelectedDate.ToString("yyyy/MM/dd") + " 23:59:59";

            string filename = DropDownListStateName.SelectedItem.ToString() + "_CollectionReport_" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
            Workbook book = new Workbook();
            StyleForTheFirstTime(book);
            DataTable dtrto = Utils.GetDataTable("select rtolocationname,rtolocationid from rtolocation  where hsrp_stateid='" + HSRPStateID + "'", CnnString);
            for (int iRowNo = 0; iRowNo < dtrto.Rows.Count; iRowNo++)
            {
                string RTOName = dtrto.Rows[iRowNo]["rtolocationname"].ToString();
                string RTOCode = dtrto.Rows[iRowNo]["RTOLocationid"].ToString();
                SQLString = MakeQueryOnTheBasisOfSelectedOption(StringOrderDate, StringAuthDate, RTOCode);

                DataTable dtRecord = Utils.GetDataTable(SQLString, CnnString);
                if (dtRecord.Rows.Count > 0)
                {
                    iCheckAllRtoHasNoRecord++;
                    ExportRecordExcel(book, dtRecord, RTOName, RTOCode);
                }
            }
            if (iCheckAllRtoHasNoRecord > 0)
            {
                HttpContext context = HttpContext.Current;
                context.Response.Clear();
                book.Save(Response.OutputStream);
                context.Response.ContentType = "application/vnd.ms-excel";

                context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                context.Response.End();
            }
            else
            {
                LabelError.Text = "No Record For the Selected Date.";
            }
        }




        #region Pdf Function
        private static void PDFRows(BaseFont bfTimes, PdfPTable table1, int iColSpan, String strText, int iFontsize, int ialignMent, string strRowType, int iBorderWidthLeft, int iBorderWidthRight, int iBorderWidthTop, int iBorderWidthBottom, int optionalHeight = 0, int optionalWidth = 0)
        {
            PdfPCell cell;
            if (strRowType == "B")
            {
                cell = new PdfPCell(new iTextSharp.text.Phrase(strText, new iTextSharp.text.Font(bfTimes, iFontsize, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
            }
            else
            {
                cell = new PdfPCell(new iTextSharp.text.Phrase(strText, new iTextSharp.text.Font(bfTimes, iFontsize, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
            }
            cell.Colspan = iColSpan;
            cell.BorderColor = iTextSharp.text.BaseColor.BLACK;
            cell.BorderWidthLeft = iBorderWidthLeft;
            cell.BorderWidthRight = iBorderWidthRight;
            cell.BorderWidthTop = iBorderWidthTop;
            cell.BorderWidthBottom = iBorderWidthBottom;
            cell.NoWrap = false;
            cell.HorizontalAlignment = ialignMent; //0=Left, 1=Centre, 2=Right
            table1.AddCell(cell);
        }
        #endregion

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


        private void ExportToPDFAll()
        {
            DataTable dtrto = GetRtoLocation();
            for (int iRowNo = 0; iRowNo < dtrto.Rows.Count; iRowNo++)
            {
                string RTOName = dtrto.Rows[iRowNo]["rtolocationname"].ToString();
                string RTOCode = dtrto.Rows[iRowNo]["RTOLocationid"].ToString();
                string FromDate = OrderDate.SelectedDate.ToString("yyyy/MM/dd") + " 00:00:00"; // Convert.ToDateTime();
                string ToDate = HSRPAuthDate.SelectedDate.ToString("yyyy/MM/dd") + " 23:59:59";

                // HttpContext context = HttpContext.Current;
                string filename = "Collection" + RTOName + "_" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".pdf";
                string SQLString = String.Empty;
                Document document = new Document();
                SetFolder(RTOName, DropDownListStateName.SelectedItem.ToString(), "Collection");

                string PdfFolder = strPath + "\\" + filename;
                BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);


                // string PdfFolder = ConfigurationManager.AppSettings["PdfFolder"].ToString() + filename;
                PdfWriter.GetInstance(document, new FileStream(PdfFolder, FileMode.Create));
                document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
                document.Open();
                PdfPTable table = new PdfPTable(12);
                var colWidthPercentages = new[] { 40f, 60f, 50f, 50f, 50f, 50f, 50f, 50f, 50f, 50f, 50f, 50f };
                table.SetWidths(colWidthPercentages);
                //string sqlquery="select distrelation from rtolocation where rtolocationid='"+
                string SqlQuery = string.Empty;
                String StringOrderDate = OrderDate.SelectedDate.ToString("yyyy/MM/dd") + " 00:00:00";
                String StringAuthDate = HSRPAuthDate.SelectedDate.ToString("yyyy/MM/dd") + " 23:59:59";

                SQLString = MakeQueryOnTheBasisOfSelectedOption(StringOrderDate, StringAuthDate, RTOCode);

                DataTable dtRecord = Utils.GetDataTable(SQLString, CnnString);

                table.TotalWidth = 750f;
                table.LockedWidth = true;
                //  GenerateCell(table, 10, 0, 0, 0, 0, 1, 1, "", 50, 0);

                // GenerateCell(table, 10, 0, 0, 0, 0, 1, 0, "SCHEDULE-C : Daily Report from Embossing Stations to Registering Authority", 20, 0);
                // GenerateCell(table, 2, 0, 0, 0, 0, 1, 0, "State Name : HARYANA", 20, 0);
                // GenerateCell(table, 2, 0, 0, 0, 0, 0, 0, "RTO Name:      " + RTOName, 15, 0);
                //  GenerateCell(table, 3, 0, 0, 0, 0, 1, 0, "Date Period  :" + OrderDate.SelectedDate.ToString("dd/MMM/yyyy") + "-" + HSRPAuthDate.SelectedDate.ToString("dd/MMM/yyyy"), 15, 0);
                // GenerateCell(table, 3, 0, 0, 0, 0, 1, 0, "Generated Date time: " + System.DateTime.Now.ToString("dd/MMM/yyyy"), 15, 0);
                GenerateCell(table, 12, 0, 0, 0, 0, 1, 0, "", 20, 0);
                GenerateCell(table, 1, 1, 1, 1, 1, 1, 0, "S.No:", 20, 0);
                GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Cash Receipt No", 40, 0);
                GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Cash Receipt Date", 20, 0);
                GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Authorisation Date", 20, 0);
                GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Order Date", 20, 0);

                GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Vehicle Reg. No", 20, 0);
                GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Owner's Name", 20, 0);
                GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Vechicle Type", 20, 0);

                GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Vehicle Class", 40, 0);
                GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Affixation Status", 20, 0);
               // GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Embossed Date", 20, 0);
                GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Affixation Date", 20, 0);

               // GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Created By", 20, 0);
                GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Amount", 20, 0);

                int SnoCounter = 0;




                #region Dynamic Rows
                for (int i = 0; i < dtRecord.Rows.Count; i++)
                {
                    SnoCounter = SnoCounter + 1;
                    GenerateCell(table, 1, 1, 1, 0, 1, 1, 1, SnoCounter.ToString(), 20, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dtRecord.Rows[i]["CashReceiptNo"].ToString(), 30, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dtRecord.Rows[i]["CashReceiptDateTime"].ToString(), 20, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dtRecord.Rows[i]["HsrpRecord_AuthorizationDate"].ToString(), 20, 0);

                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dtRecord.Rows[i]["OrderDate"].ToString(), 20, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dtRecord.Rows[i]["VehicleRegNo"].ToString(), 20, 0);

                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dtRecord.Rows[i]["OwnerName"].ToString(), 20, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dtRecord.Rows[i]["VehicleType"].ToString(), 20, 0);

                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dtRecord.Rows[i]["VehicleClass"].ToString(), 20, 0);

                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dtRecord.Rows[i]["orderstatus"].ToString(), 20, 0);

                   // GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dtRecord.Rows[i]["embdate"].ToString(), 20, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dtRecord.Rows[i]["AffixDate"].ToString(), 20, 0);

                    //GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dtRecord.Rows[i]["username"].ToString(), 20, 0);

                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dtRecord.Rows[i]["NetAmount"].ToString(), 20, 0);

                #endregion


                }
                document.Add(table);
                document.NewPage();

                document.Close();
                //context.Response.ContentType = "Application/pdf";
                //context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                //context.Response.WriteFile(PdfFolder);
                //context.Response.End();
            }


        }
        protected void btnpdf_Click(object sender, EventArgs e)
        {

            ExportToPDFAll();

        }
    }
}
