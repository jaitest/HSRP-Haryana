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

namespace HSRP.Report
{
    public partial class DailyCashCollectionRTOLocationWise_WithDateMP : System.Web.UI.Page
    {
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
                SQLString = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID where  a.LocationType!='District' and UserRTOLocationMapping.UserID='" + strUserID + "' ";

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
            String[] StringAuthDate = HSRPAuthDate.SelectedDate.ToString().Split('/');


            String DatePrint2 = StringAuthDate[1] + "/" + StringAuthDate[0] + "/" + StringAuthDate[2].Split(' ')[0];


            String[] StringOrderDate = OrderDate.SelectedDate.ToString().Split('/');
            string Mon = ("0" + StringOrderDate[0]);
            string Monthdate = Mon.Replace("00", "0").Replace("01", "1");
            String From2 = StringOrderDate[1] + "-" + Monthdate + "-" + StringOrderDate[2].Split(' ')[0];

            String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
            String DatePrint1 = StringOrderDate[1] + "/" + StringOrderDate[0] + "/" + StringOrderDate[2].Split(' ')[0];

            DatePrint = DatePrint1 + "   To    " + DatePrint2;

            //String ReportDate = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];

            string ReportDate1 = From1 + " 00:00:00";

            ReportDate1 = OrderDate.SelectedDate.ToString("MM/dd/yyyy") + " 00:00:00";

            //String ReportDateEnd = StringAuthDate[0] + "/" + StringAuthDate[1] + "/" + StringAuthDate[2].Split(' ')[0];
            String ReportDateEnd = StringAuthDate[1] + "-" + StringAuthDate[0] + "-" + StringAuthDate[2].Split(' ')[0];
            string ReportDate2 = ReportDateEnd + " 23:59:59";
            ReportDate2 = HSRPAuthDate.SelectedDate.ToString("MM/dd/yyyy") + " 23:59:59";
            //OrderDate1 = new DateTime(Convert.ToInt32(StringOrderDate[2].Split(' ')[0]), Convert.ToInt32(StringOrderDate[0]), Convert.ToInt32(StringOrderDate[1]));


            DateTime StartDate = Convert.ToDateTime(OrderDate.SelectedDate);
            int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
            int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);



            int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
            int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);

            if (strUserID == "3064")
            {
                string aa = "SELECT DATEDIFF(day,'2013-08-01 00:00:00','" + ReportDate1 + "') AS DiffDate";
                aa = Utils.getDataSingleValue(aa, CnnString, "DiffDate");
                //if (ReportDate1 < "2013/08/01 00:00:00")

                if (Convert.ToInt32(aa) < 0)
                    ReportDate1 = "2013/08/01 00:00:00";
            }

           
            if (ddlVehicleReference.SelectedItem.Text == "Both" && DropDownListVehicleModel.SelectedItem.Text == "All")
            {
                SQLString = "SELECT  HSRP_StateID, convert(varchar(15), (select  [dbo] .[GetAffxDate_Insert_Hr] (CONVERT(varchar(20), HsrpRecord_creationdate, 111),'5')),103) as AffDate,CONVERT(varchar(20), HsrpRecord_AuthorizationDate, 103) AS HsrpRecord_AuthorizationDate,  CONVERT(varchar(20), HsrpRecord_creationdate, 103) AS NewOrderDate,CONVERT(varchar(20), OrderEmbossingDate, 103) AS OrderEmbossingDate,CONVERT(varchar(20), InvoiceDateTime, 103) AS InvoiceDateTime,  HSRPRecord_AuthorizationNo,CashReceiptNo, HSRPRecordID, VehicleRegNo, OwnerName, VehicleType, VehicleClass,CONVERT(numeric,round( RoundOff_NetAmount,0)) as NetAmount,OrderDate,EngineNo,ChassisNo,MobileNo FROM HSRPRecords where HSRP_StateID='" + DropDownListStateName.SelectedValue + "' and RTOLocationID='" + dropDownListClient.SelectedValue + "' and  OwnerName is not null and OwnerName <> '' and Address1 is not null and Address1 <> '' and HsrpRecord_creationdate between '" + ReportDate1.ToString() + "' and '" + ReportDate2.ToString() + "' and NetAmount <> 0 order by OrderDate";
            }
            else if (ddlVehicleReference.SelectedItem.Text == "Both")
            {
                SQLString = "SELECT  HSRP_StateID, convert(varchar(15), (select  [dbo] .[GetAffxDate_Insert_Hr] (CONVERT(varchar(20), HsrpRecord_creationdate, 111),'5')),103) as AffDate, CONVERT(varchar(20), HsrpRecord_AuthorizationDate, 103) AS HsrpRecord_AuthorizationDate,  CONVERT(varchar(20), HsrpRecord_creationdate, 103) AS NewOrderDate,CONVERT(varchar(20), OrderEmbossingDate, 103) AS OrderEmbossingDate,CONVERT(varchar(20), InvoiceDateTime, 103) AS InvoiceDateTime,  HSRPRecord_AuthorizationNo,CashReceiptNo, HSRPRecordID, VehicleRegNo, OwnerName, VehicleType, VehicleClass,CONVERT(numeric,round( RoundOff_NetAmount,0)) as NetAmount,OrderDate,EngineNo,ChassisNo,MobileNo FROM HSRPRecords where HSRP_StateID='" + DropDownListStateName.SelectedValue + "' and RTOLocationID='" + dropDownListClient.SelectedValue + "' and  OwnerName is not null and OwnerName <> '' and Address1 is not null and Address1 <> '' and VehicleType='" + DropDownListVehicleModel.SelectedItem.ToString() + "'  and HsrpRecord_creationdate between '" + ReportDate1.ToString() + "' and '" + ReportDate2.ToString() + "' and NetAmount <> 0 order by OrderDate";
            }
            else if (ddlVehicleReference.SelectedItem.Text == "New" || ddlVehicleReference.SelectedItem.Text == "Old" && DropDownListVehicleModel.SelectedItem.Text == "All")
            {
                SQLString = "SELECT  HSRP_StateID,convert(varchar(15), (select  [dbo] .[GetAffxDate_Insert_Hr] (CONVERT(varchar(20), HsrpRecord_creationdate, 111),'5')),103) as AffDate, CONVERT(varchar(20), HsrpRecord_AuthorizationDate, 103) AS HsrpRecord_AuthorizationDate,  CONVERT(varchar(20), HsrpRecord_creationdate, 103) AS NewOrderDate,CONVERT(varchar(20), OrderEmbossingDate, 103) AS OrderEmbossingDate,CONVERT(varchar(20), InvoiceDateTime, 103) AS InvoiceDateTime,  HSRPRecord_AuthorizationNo,CashReceiptNo, HSRPRecordID, VehicleRegNo, OwnerName, VehicleType, VehicleClass,CONVERT(numeric,round( RoundOff_NetAmount,0)) as NetAmount,OrderDate,EngineNo,ChassisNo,MobileNo FROM HSRPRecords where HSRP_StateID='" + DropDownListStateName.SelectedValue + "' and RTOLocationID='" + dropDownListClient.SelectedValue + "' and  OwnerName is not null and OwnerName <> '' and Address1 is not null and Address1 <> '' and vehicleref='" + ddlVehicleReference.SelectedItem.ToString() + "' and HsrpRecord_creationdate between '" + ReportDate1.ToString() + "' and '" + ReportDate2.ToString() + "' and NetAmount <> 0 order by OrderDate";
            }
            else
            {
                SQLString = "SELECT  HSRP_StateID, convert(varchar(15), (select  [dbo] .[GetAffxDate_Insert_Hr] (CONVERT(varchar(20), HsrpRecord_creationdate, 111),'5')),103) as AffDate, CONVERT(varchar(20), HsrpRecord_AuthorizationDate, 103) AS HsrpRecord_AuthorizationDate,  CONVERT(varchar(20), HsrpRecord_creationdate, 103) AS NewOrderDate,CONVERT(varchar(20), OrderEmbossingDate, 103) AS OrderEmbossingDate,CONVERT(varchar(20), InvoiceDateTime, 103) AS InvoiceDateTime,  HSRPRecord_AuthorizationNo,CashReceiptNo, HSRPRecordID, VehicleRegNo, OwnerName, VehicleType, VehicleClass,CONVERT(numeric,round( RoundOff_NetAmount,0)) as NetAmount,OrderDate,EngineNo,ChassisNo,MobileNo FROM HSRPRecords where HSRP_StateID='" + DropDownListStateName.SelectedValue + "' and RTOLocationID='" + dropDownListClient.SelectedValue + "' and  OwnerName is not null and OwnerName <> '' and Address1 is not null and Address1 <> '' and vehicleref='" + ddlVehicleReference.SelectedItem.ToString() + "' and VehicleType='" + DropDownListVehicleModel.SelectedItem.ToString() + "'  and HsrpRecord_creationdate between '" + ReportDate1.ToString() + "' and '" + ReportDate2.ToString() + "' and NetAmount <> 0 order by OrderDate";
            }
            ExportRecordExcel(SQLString);
        }

        private void ExportRecordExcel(string strquery)
        {

            try
            {
                
                string CashCollectionDate = OrderDate.SelectedDate.ToString();

                //string Sqlquery = "select  [dbo] .[GetAffxDate_Insert_Hr] ('" + CashCollectionDate + "','5') as AffDate";
                //DataTable dtDate = Utils.GetDataTable(Sqlquery, CnnString);
                //string AffixiationDate = Convert.ToDateTime(dtDate.Rows[0]["AffDate"]).ToString("dd/MM/yyyy");
                if (ddlVehicleReference.SelectedItem.Text == "Select Vehicle Reference")
                {
                    LabelError.Text = "Please Select Vehicle Reference";
                    return;
                }
                LabelError.Text = "";

                //  SQLString = "SELECT  HSRP_StateID, CONVERT(varchar(20), HsrpRecord_AuthorizationDate, 103) AS HsrpRecord_AuthorizationDate,  CONVERT(varchar(20), OrderDate, 103) AS NewOrderDate,CONVERT(varchar(20), OrderEmbossingDate, 103) AS OrderEmbossingDate,CONVERT(varchar(20), InvoiceDateTime, 103) AS InvoiceDateTime,  HSRPRecord_AuthorizationNo,CashReceiptNo, HSRPRecordID, VehicleRegNo, OwnerName, VehicleType, VehicleClass,CONVERT(numeric,round( RoundOff_NetAmount,0)) as NetAmount,OrderDate,EngineNo,ChassisNo,MobileNo FROM HSRPRecords where HSRP_StateID='" + DropDownListStateName.SelectedValue + "' and RTOLocationID='" + dropDownListClient.SelectedValue + "' and vehicleref='" + ddlVehicleReference.SelectedItem.ToString() + "' and VehicleType='" + DropDownListVehicleModel.SelectedItem.ToString() + "'  and OrderDate between '" + ReportDate1.ToString() + "' and '" + ReportDate2.ToString() + "' order by OrderDate";
                //else
                //  SQLString = "SELECT  HSRP_StateID, CONVERT(varchar(20), HsrpRecord_AuthorizationDate, 103) AS HsrpRecord_AuthorizationDate,  CONVERT(varchar(20), OrderDate, 103) AS NewOrderDate,CONVERT(varchar(20), OrderEmbossingDate, 103) AS OrderEmbossingDate,CONVERT(varchar(20), InvoiceDateTime, 103) AS InvoiceDateTime,  HSRPRecord_AuthorizationNo,CashReceiptNo, HSRPRecordID, VehicleRegNo, OwnerName, VehicleType, VehicleClass,CONVERT(numeric,round( RoundOff_NetAmount,0)) as NetAmount,OrderDate,EngineNo,ChassisNo,MobileNo FROM HSRPRecords where HSRP_StateID='" + DropDownListStateName.SelectedValue + "' and RTOLocationID='" + dropDownListClient.SelectedValue + "' and vehicleref='" + ddlVehicleReference.SelectedItem.ToString() + "' and VehicleType='" + DropDownListVehicleModel.SelectedItem.ToString() + "'  and OrderDate between '2013/08/01 00:00:00' and '" + ReportDate2.ToString() + "' order by OrderDate";


                string filename = "DailyCashCollectionReport-CounterWise-" + dropDownListClient.SelectedItem + "-" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                Workbook book = new Workbook();

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

                Worksheet sheet = book.Worksheets.Add("HSRP Daily Cash Collection");
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


                WorksheetStyle style9 = book.Styles.Add("HeaderStyle9");
                style9.Font.FontName = "Tahoma";
                style9.Font.Size = 10;
                style9.Font.Bold = true;
                style9.Alignment.Horizontal = StyleHorizontalAlignment.Right;
                style9.Interior.Color = "#FCF6AE";
                style9.Interior.Pattern = StyleInteriorPattern.Solid;


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

                row.Cells.Add(new WorksheetCell("Location:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(dropDownListClient.SelectedItem.ToString(), "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("Vehicle Reference:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(ddlVehicleReference.SelectedItem.ToString(), "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("Vehicle Type:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(DropDownListVehicleModel.SelectedItem.ToString(), "HeaderStyle2"));

                row = sheet.Table.Rows.Add();
                row.Index = 4;

                DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
               
                row.Cells.Add(new WorksheetCell("Report Date:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(DatePrint, "HeaderStyle2"));
                row = sheet.Table.Rows.Add();


                row.Index = 5;

                //row.Cells.Add("Order Date");
                row.Cells.Add(new WorksheetCell("S.No.", "HeaderStyle6"));
                //  row.Cells.Add(new WorksheetCell("Authorisation No", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Cash Receipt No", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Authorisation Date", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Order Date", "HeaderStyle6"));
                //row.Cells.Add(new WorksheetCell("Close Date", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Vehicle Reg. No", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Owner's Name", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Vechicle Type", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Vehicle Class", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Affixation Date", "HeaderStyle6"));
                if (DropDownListStateName.SelectedItem.ToString() == "BIHAR")
                {
                    row.Cells.Add(new WorksheetCell("EngineNo", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("ChassisNo", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("MobileNo", "HeaderStyle6"));
                }
               // row.Cells.Add(new WorksheetCell("Amount", "HeaderStyle6"));
               

                row = sheet.Table.Rows.Add();
                String StringField = String.Empty;
                String StringAlert = String.Empty;

                row.Index = 7;

                UserType = Convert.ToInt32(Session["UserType"]);
                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                strUserID = Session["UID"].ToString();


                DataTable dt = Utils.GetDataTable(strquery, CnnString);
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
                        row.Cells.Add(new WorksheetCell(dtrows["HsrpRecord_AuthorizationDate"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["NewOrderDate"].ToString(), DataType.String, "HeaderStyle"));
                        //row.Cells.Add(new WorksheetCell(dtrows["InvoiceDateTime"].ToString(), DataType.String, "HeaderStyle"));

                        row.Cells.Add(new WorksheetCell(dtrows["VehicleRegNo"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["OwnerName"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["VehicleClass"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["VehicleType"].ToString(), DataType.String, "HeaderStyle")); //AffDate
                        row.Cells.Add(new WorksheetCell(dtrows["AffDate"].ToString(), DataType.String, "HeaderStyle"));
                        //row.Cells.Add(new WorksheetCell(AffixiationDate, DataType.String, "HeaderStyle"));

                        //Modification done by Himanshu saini On 19/7/2013
                        if (DropDownListStateName.SelectedItem.ToString() == "BIHAR")
                        {
                            row.Cells.Add(new WorksheetCell(dtrows["EngineNo"].ToString(), DataType.String, "HeaderStyle"));
                            row.Cells.Add(new WorksheetCell(dtrows["ChassisNo"].ToString(), DataType.String, "HeaderStyle"));
                            row.Cells.Add(new WorksheetCell(dtrows["MobileNo"].ToString(), DataType.String, "HeaderStyle"));

                        }
                        string amount = dtrows["NetAmount"].ToString();
                        if (amount == "")
                        {

                           // 6june row.Cells.Add(new WorksheetCell("0", DataType.String, "HeaderStyle"));
                            //totalAmount = totalAmount + Math.Round(Convert.ToDecimal(amount.ToString()));
                        }
                        else
                        {
                            //6june
                            //row.Cells.Add(new WorksheetCell(Math.Round(Convert.ToDecimal(dtrows["NetAmount"].ToString())).ToString(), DataType.Number, "HeaderStyle5"));
                            //totalAmount = totalAmount + Math.Round(Convert.ToDecimal(dtrows["NetAmount"].ToString()));
                        }


                        row = sheet.Table.Rows.Add();
                    }
                    row = sheet.Table.Rows.Add();
                  

                   // row.Cells.Add(new WorksheetCell((totalAmount).ToString(), "HeaderStyle8"));

                    totalAmount = 0;

                    row = sheet.Table.Rows.Add();
                    HttpContext context = HttpContext.Current;
                    context.Response.Clear();
                    // Save the file and open it
                    book.Save(Response.OutputStream);

                    //context.Response.ContentType = "text/csv";
                    context.Response.ContentType = "application/vnd.ms-excel";

                    context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                    context.Response.End();

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

        protected void Button1_Click(object sender, EventArgs e)
        {
            String[] StringAuthDate = HSRPAuthDate.SelectedDate.ToString().Split('/');


            String DatePrint2 = StringAuthDate[1] + "/" + StringAuthDate[0] + "/" + StringAuthDate[2].Split(' ')[0];


            String[] StringOrderDate = OrderDate.SelectedDate.ToString().Split('/');
            string Mon = ("0" + StringOrderDate[0]);
            string Monthdate = Mon.Replace("00", "0").Replace("01", "1");
            String From2 = StringOrderDate[1] + "-" + Monthdate + "-" + StringOrderDate[2].Split(' ')[0];

            String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
            String DatePrint1 = StringOrderDate[1] + "/" + StringOrderDate[0] + "/" + StringOrderDate[2].Split(' ')[0];

            String DatePrint = DatePrint1 + "   To    " + DatePrint2;

            //String ReportDate = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];

            string ReportDate1 = From1 + " 00:00:00";

            ReportDate1 = OrderDate.SelectedDate.ToString("MM/dd/yyyy") + " 00:00:00";

            //String ReportDateEnd = StringAuthDate[0] + "/" + StringAuthDate[1] + "/" + StringAuthDate[2].Split(' ')[0];
            String ReportDateEnd = StringAuthDate[1] + "-" + StringAuthDate[0] + "-" + StringAuthDate[2].Split(' ')[0];
            string ReportDate2 = ReportDateEnd + " 23:59:59";
            ReportDate2 = HSRPAuthDate.SelectedDate.ToString("MM/dd/yyyy") + " 23:59:59";
            //OrderDate1 = new DateTime(Convert.ToInt32(StringOrderDate[2].Split(' ')[0]), Convert.ToInt32(StringOrderDate[0]), Convert.ToInt32(StringOrderDate[1]));


            DateTime StartDate = Convert.ToDateTime(OrderDate.SelectedDate);
            int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
            int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);

            DataTable dt = new DataTable();

            int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
            int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);

            if (strUserID == "3064")
            {
                string aa = "SELECT DATEDIFF(day,'2013-08-01 00:00:00','" + ReportDate1 + "') AS DiffDate";
                aa = Utils.getDataSingleValue(aa, CnnString, "DiffDate");
                //if (ReportDate1 < "2013/08/01 00:00:00")

                if (Convert.ToInt32(aa) < 0)
                    ReportDate1 = "2013/08/01 00:00:00";

            }

                if (ddlVehicleReference.SelectedItem.Text == "Both" && DropDownListVehicleModel.SelectedItem.Text == "All")
                {
                    SQLString = "SELECT  HSRP_StateID,convert(varchar(15), (select  [dbo] .[GetAffxDate_Insert_Hr] (CONVERT(varchar(20), HsrpRecord_creationdate, 111),'5')),103) as AffDate, CONVERT(varchar(20), HsrpRecord_AuthorizationDate, 103) AS HsrpRecord_AuthorizationDate,  CONVERT(varchar(20), HsrpRecord_creationdate, 103) AS NewOrderDate,CONVERT(varchar(20), OrderEmbossingDate, 103) AS OrderEmbossingDate,CONVERT(varchar(20), InvoiceDateTime, 103) AS InvoiceDateTime,  HSRPRecord_AuthorizationNo,CashReceiptNo, HSRPRecordID, VehicleRegNo, OwnerName, VehicleType, VehicleClass,CONVERT(numeric,round( RoundOff_NetAmount,0)) as NetAmount,OrderDate,EngineNo,ChassisNo,MobileNo FROM HSRPRecords where HSRP_StateID='" + DropDownListStateName.SelectedValue + "' and RTOLocationID='" + dropDownListClient.SelectedValue + "' and HsrpRecord_creationdate between '" + ReportDate1.ToString() + "' and '" + ReportDate2.ToString() + "' and NetAmount <> 0 order by OrderDate";
                }
                else if (ddlVehicleReference.SelectedItem.Text == "Both")
                {
                    SQLString = "SELECT  HSRP_StateID,convert(varchar(15), (select  [dbo] .[GetAffxDate_Insert_Hr] (CONVERT(varchar(20), HsrpRecord_creationdate, 111),'5')),103) as AffDate, CONVERT(varchar(20), HsrpRecord_AuthorizationDate, 103) AS HsrpRecord_AuthorizationDate,  CONVERT(varchar(20), HsrpRecord_creationdate, 103) AS NewOrderDate,CONVERT(varchar(20), OrderEmbossingDate, 103) AS OrderEmbossingDate,CONVERT(varchar(20), InvoiceDateTime, 103) AS InvoiceDateTime,  HSRPRecord_AuthorizationNo,CashReceiptNo, HSRPRecordID, VehicleRegNo, OwnerName, VehicleType, VehicleClass,CONVERT(numeric,round( RoundOff_NetAmount,0)) as NetAmount,OrderDate,EngineNo,ChassisNo,MobileNo FROM HSRPRecords where HSRP_StateID='" + DropDownListStateName.SelectedValue + "' and RTOLocationID='" + dropDownListClient.SelectedValue + "' and VehicleType='" + DropDownListVehicleModel.SelectedItem.ToString() + "'  and HsrpRecord_creationdate between '" + ReportDate1.ToString() + "' and '" + ReportDate2.ToString() + "' and NetAmount <> 0 order by OrderDate";
                }
                else if (ddlVehicleReference.SelectedItem.Text == "New" || ddlVehicleReference.SelectedItem.Text == "Old" && DropDownListVehicleModel.SelectedItem.Text == "All")
                {
                    SQLString = "SELECT  HSRP_StateID,convert(varchar(15), (select  [dbo] .[GetAffxDate_Insert_Hr] (CONVERT(varchar(20), HsrpRecord_creationdate, 111),'5')),103) as AffDate, CONVERT(varchar(20), HsrpRecord_AuthorizationDate, 103) AS HsrpRecord_AuthorizationDate,  CONVERT(varchar(20), HsrpRecord_creationdate, 103) AS NewOrderDate,CONVERT(varchar(20), OrderEmbossingDate, 103) AS OrderEmbossingDate,CONVERT(varchar(20), InvoiceDateTime, 103) AS InvoiceDateTime,  HSRPRecord_AuthorizationNo,CashReceiptNo, HSRPRecordID, VehicleRegNo, OwnerName, VehicleType, VehicleClass,CONVERT(numeric,round( RoundOff_NetAmount,0)) as NetAmount,OrderDate,EngineNo,ChassisNo,MobileNo FROM HSRPRecords where HSRP_StateID='" + DropDownListStateName.SelectedValue + "' and RTOLocationID='" + dropDownListClient.SelectedValue + "'  and vehicleref='" + ddlVehicleReference.SelectedItem.ToString() + "' and HsrpRecord_creationdate between '" + ReportDate1.ToString() + "' and '" + ReportDate2.ToString() + "' and NetAmount <> 0 order by OrderDate";
                }
                else
                {
                    SQLString = "SELECT  HSRP_StateID, convert(varchar(15), (select  [dbo] .[GetAffxDate_Insert_Hr] (CONVERT(varchar(20), HsrpRecord_creationdate, 111),'5')),103) as AffDate,CONVERT(varchar(20), HsrpRecord_AuthorizationDate, 103) AS HsrpRecord_AuthorizationDate,  CONVERT(varchar(20), HsrpRecord_creationdate, 103) AS NewOrderDate,CONVERT(varchar(20), OrderEmbossingDate, 103) AS OrderEmbossingDate,CONVERT(varchar(20), InvoiceDateTime, 103) AS InvoiceDateTime,  HSRPRecord_AuthorizationNo,CashReceiptNo, HSRPRecordID, VehicleRegNo, OwnerName, VehicleType, VehicleClass,CONVERT(numeric,round( RoundOff_NetAmount,0)) as NetAmount,OrderDate,EngineNo,ChassisNo,MobileNo FROM HSRPRecords where HSRP_StateID='" + DropDownListStateName.SelectedValue + "' and RTOLocationID='" + dropDownListClient.SelectedValue + "' and vehicleref='" + ddlVehicleReference.SelectedItem.ToString() + "' and VehicleType='" + DropDownListVehicleModel.SelectedItem.ToString() + "'  and HsrpRecord_creationdate between '" + ReportDate1.ToString() + "' and '" + ReportDate2.ToString() + "' and NetAmount <> 0 order by OrderDate";
                }
                ExportRecordExcel(SQLString);
            }
        }
    }
