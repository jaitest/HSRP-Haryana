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
    public partial class HROnlineOrderStatusReport : System.Web.UI.Page
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

                Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), CnnString, "All");
                // dropDownListClient.SelectedIndex = dropDownListClient.Items.Count - 1;
            }
            else
            {
                // SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where (RTOLocationID=" + RTOLocationID + " or distRelation=" + RTOLocationID + " ) and ActiveStatus!='N'   Order by RTOLocationName";
                SQLString = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID where  a.LocationType!='District' and UserRTOLocationMapping.UserID='" + strUserID + "' Order by RTOLocationName ";
                Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), CnnString, "All");

                //DataSet dss = Utils.getDataSet(SQLString, CnnString);
                //dropDownListClient.DataSource = dss;
                //dropDownListClient.DataBind();
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
            if (dropDownListClient.SelectedItem.Text == "All")
            {
                SQLString = "select OrderDate,PaymentOrderId,VehicleRegNo,ChassisNo,EngineNo,OwnerName,Address1,MobileNo,HSRP_Front_LaserCode,HSRP_Rear_LaserCode,OrderStatus,OrderEmbossingDate,OrderClosedDate from hsrprecords where HSRP_StateID='" + HSRPStateID + "' and orderstatus='" + ddlVehicleReference.SelectedItem.ToString() + "' and HsrpRecord_creationdate between '" + StringOrderDate + "' and '" + StringAuthDate + "' and PaymentOrderId is not null";
            }
            else
            {
                SQLString = "select OrderDate,PaymentOrderId,VehicleRegNo,ChassisNo,EngineNo,OwnerName,Address1,MobileNo,HSRP_Front_LaserCode,HSRP_Rear_LaserCode,OrderStatus,OrderEmbossingDate,OrderClosedDate from hsrprecords where HSRP_StateID='" + HSRPStateID + "' and RTOLocationID='" + dropDownListClient.SelectedValue + "' and orderstatus='" + ddlVehicleReference.SelectedItem.ToString() + "' and HsrpRecord_creationdate between '" + StringOrderDate + "' and '" + StringAuthDate + "' and PaymentOrderId is not null";
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
                WorksheetCell cell = row.Cells.Add("Online Order Status Report");
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
                row.Cells.Add(new WorksheetCell("Order Status:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(ddlVehicleReference.SelectedItem.ToString(), "HeaderStyle2"));
               

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
                row.Cells.Add(new WorksheetCell("OrderDate", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("PaymentOrderId", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("VehicleRegNo", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("ChassisNo", "HeaderStyle6"));
                //row.Cells.Add(new WorksheetCell("Close Date", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("EngineNo", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Owner's Name", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Address", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("MobileNo", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("HSRP_Front_LaserCode", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("HSRP_Rear_LaserCode", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("OrderStatus", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("OrderEmbossingDate", "HeaderStyle6"));

                row.Cells.Add(new WorksheetCell("OrderClosedDate", "HeaderStyle6"));
                

                row = sheet.Table.Rows.Add();
                String StringField = String.Empty;
                String StringAlert = String.Empty;

                row.Index = 7;

                UserType = Convert.ToInt32(Session["UserType"]);
                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                strUserID = Session["UID"].ToString();

                string RTOColName = string.Empty;
                
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
                        row.Cells.Add(new WorksheetCell(dtrows["OrderDate"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["PaymentOrderId"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["VehicleRegNo"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["ChassisNo"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["EngineNo"].ToString(), DataType.String, "HeaderStyle"));

                        row.Cells.Add(new WorksheetCell(dtrows["OwnerName"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["Address1"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["MobileNo"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["HSRP_Front_LaserCode"].ToString(), DataType.String, "HeaderStyle"));

                        row.Cells.Add(new WorksheetCell(dtrows["HSRP_Rear_LaserCode"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["OrderStatus"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["OrderEmbossingDate"].ToString(), DataType.String, "HeaderStyle"));

                        row.Cells.Add(new WorksheetCell(dtrows["OrderClosedDate"].ToString(), DataType.String, "HeaderStyle"));
                      


                        row = sheet.Table.Rows.Add();
                    }
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

        
        protected void btnAllLocationPdf_Click(object sender, EventArgs e)
        {
            int iCheckAllRtoHasNoRecord = 0;
            String StringOrderDate = OrderDate.SelectedDate.ToString("yyyy/MM/dd") + " 00:00:00";
            String StringAuthDate = HSRPAuthDate.SelectedDate.ToString("yyyy/MM/dd") + " 23:59:59";

            string filename = DropDownListStateName.SelectedItem.ToString() + "Order Status Report" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
            Workbook book = new Workbook();
            StyleForTheFirstTime(book);
            DataTable dtrto = Utils.GetDataTable("select rtolocationname,rtolocationid from rtolocation where rtolocationid in (select distinct distrelation from rtolocation where hsrp_stateid='" + HSRPStateID + "')", CnnString);
            if (dropDownListClient.SelectedItem.Text == "All")
            {
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
            else
            {

                string RTOName = dropDownListClient.SelectedItem.ToString();
                string RTOCode = dropDownListClient.SelectedValue;
                SQLString = MakeQueryOnTheBasisOfSelectedOption(StringOrderDate, StringAuthDate, RTOCode);

                DataTable dtRecord = Utils.GetDataTable(SQLString, CnnString);
                if (dtRecord.Rows.Count > 0)
                {
                    iCheckAllRtoHasNoRecord++;
                    ExportRecordExcel(book, dtRecord, RTOName, RTOCode);
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
        }

       
    }
}
