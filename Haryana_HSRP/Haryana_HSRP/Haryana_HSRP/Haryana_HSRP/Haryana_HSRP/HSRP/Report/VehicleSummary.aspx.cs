using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.OleDb;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.IO;
using CarlosAg.ExcelXmlWriter;
using System.Drawing;


namespace HSRP.Report
{
public partial class VehicleSummary : System.Web.UI.Page
{

    int UserType1;
    string CnnString1 = string.Empty;
    string HSRP_StateID1 = string.Empty;
    string RTOLocationID1 = string.Empty;
    string strUserID1 = string.Empty;
    int intHSRPStateID1;
    int intRTOLocationID1;
    string SQLString1 = string.Empty;
    string OrderType;
    string recordtype = string.Empty;
    //DateTime OrderDate1;

    DataTable dt = new DataTable();
    DataTable dt1 = new DataTable();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UID"] == null)
        {
            Response.Redirect("~/Login.aspx", true);
        }
        else
        {

            UserType1 = Convert.ToInt32(Session["UserType"]);
            CnnString1 = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            strUserID1 = Session["UID"].ToString();

            HSRP_StateID1 = Session["UserHSRPStateID"].ToString();
            RTOLocationID1 = Session["UserRTOLocationID"].ToString();
            if (!IsPostBack)
            {

                try
                {
                    if (UserType1.Equals(0))
                    {
                        // labelOrganization.Visible = true;
                        DropDownListStateName.Visible = true;
                        //dropDownListClient.Visible = true;
                        FilldropDownListOrganization();
                        //FilldropDownListClient();
                        OrderDatefrom.SelectedDate = System.DateTime.Now;
                        OrderDateto.SelectedDate = System.DateTime.Now;


                    }
                    else
                    {

                        // labelOrganization.Visible = true;
                        DropDownListStateName.Visible = true;
                        //dropDownListClient.Visible = true;
                        FilldropDownListOrganization();
                        //FilldropDownListClient();

                        OrderDatefrom.SelectedDate = System.DateTime.Now;
                        OrderDateto.SelectedDate = System.DateTime.Now;

                    }


                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }
    }


    private void FilldropDownListOrganization()
    {
        if (UserType1.Equals(0))
        {
            SQLString1 = "select HSRPStateName,HSRP_StateID from HSRPState  where ActiveStatus='Y' Order by HSRPStateName";
            Utils.PopulateDropDownList(DropDownListStateName, SQLString1.ToString(), CnnString1, "--Select State--");
        }
        else
        {
            SQLString1 = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID=" + HSRP_StateID1 + " and ActiveStatus='Y' Order by HSRPStateName";
            DataSet dts = Utils.getDataSet(SQLString1, CnnString1);
            DropDownListStateName.DataSource = dts;
            DropDownListStateName.DataBind();
        }
    }


    private void InitialSetting()
    {
        string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
        string MaxDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
        OrderDateto.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
        OrderDateto.MaxDate = DateTime.Parse(MaxDate);
        CalendarOrderDateto.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
        CalendarOrderDateto.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
        OrderDatefrom.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
        OrderDatefrom.MaxDate = DateTime.Parse(MaxDate);
        CalendarOrderDatefrom.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
        CalendarOrderDatefrom.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);

    }

    protected void DropDownListStateName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DropDownListStateName.SelectedItem.ToString() != "HARYANA")
        {
            dropDownListorder.Enabled = true;
        }
    }

    protected void dropDownListClient_SelectedIndexChanged(object sender, EventArgs e)
    {

    }



    protected void DropDownListyearName_SelectedIndexChanged(object sender, EventArgs e)
    {
    }


    private Boolean validate1()
    {
        Boolean blnvalid = true;
        String getvalue = string.Empty;
        getvalue = DropDownListStateName.SelectedItem.Text;
        if (getvalue == "--Select State--")
        {
            blnvalid = false;

            Label1.Text = "Please select State Name";

        }
        return blnvalid;

    }

    protected void btnexport_Click(object sender, EventArgs e)
    {
        validate1();
        if (validate1() == false)
        {
            return;
        }

        else
        {
            Label1.Visible = false;

            try
            {

                OrderType = dropDownListorder.SelectedValue.ToString();

                //         if (OrderType == "Dealer" || OrderType == "Normal" || OrderType == "Both")

                if (OrderType == "Dealer")
                {
                    recordtype = "Dealer";
                }
                else if (OrderType == "Normal")
                {
                    recordtype = "null,'','1','Agent','BRROFF','D','Dealer-OFFLINE','DelhiDealerOldData','DelhiOldData','EXL','EXL-Not in Master','HHT','HHTD','HHTOLD','HOLD','OFFLINE','OLD','OLDA','Stock In Hand','Transfer WA','Web Service'";
                }
                else
                {
                    recordtype = "Both";
                }
                            

                //String dateformate=OrderDatefrom.DateTimeFormat
                String[] StringAuthDate = OrderDateto.SelectedDate.ToString().Split('/');
                String ReportDateEnd = StringAuthDate[0] + "-" + StringAuthDate[1] + "-" + StringAuthDate[2].Split(' ')[0];
                string ReportDate2 = ReportDateEnd + " 23:59:59";
                String DatePrint2 = StringAuthDate[1] + "/" + StringAuthDate[0] + "/" + StringAuthDate[2].Split(' ')[0];


                String[] StringOrderDate = OrderDatefrom.SelectedDate.ToString().Split('/');
                string Mon = ("0" + StringOrderDate[0]);
                string Monthdate = Mon.Replace("00", "0").Replace("01", "1");
                String From2 = StringOrderDate[1] + "-" + Monthdate + "-" + StringOrderDate[2].Split(' ')[0];

                String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
                String DatePrint1 = StringOrderDate[1] + "/" + StringOrderDate[0] + "/" + StringOrderDate[2].Split(' ')[0];
                //string DatePrint1 = "select convert(VARCHAR,getdate(),0)";

                String DatePrint = DatePrint1 + "   To   " + DatePrint2;

                String ReportDate = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
                string ReportDate1 = ReportDate + " 00:00:00";

                DateTime StartDate = Convert.ToDateTime(OrderDatefrom.SelectedDate);
                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID1);
                  
                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID1);
                   
                string filename = "Total_Vehicle_Collection_Summary" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                Workbook book = new Workbook();

                // Specify which Sheet should be opened and the size of window by default
                book.ExcelWorkbook.ActiveSheetIndex = 1;
                book.ExcelWorkbook.WindowTopX = 100;
                book.ExcelWorkbook.WindowTopY = 200;
                book.ExcelWorkbook.WindowHeight = 7000;
                book.ExcelWorkbook.WindowWidth = 8000;

                // Some optional properties of the Document
                book.Properties.Author = "HSRP";
                book.Properties.Title = "HSRP Total Vehicle Collection Summary";
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

                Worksheet sheet = book.Worksheets.Add("HSRP Total Vehicle Collection Summary");
                sheet.Table.Columns.Add(new WorksheetColumn(60));
                sheet.Table.Columns.Add(new WorksheetColumn(120));
                sheet.Table.Columns.Add(new WorksheetColumn(100));
                sheet.Table.Columns.Add(new WorksheetColumn(150));

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
                style9.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                style9.Interior.Color = "#FCF6AE";
                style9.Interior.Pattern = StyleInteriorPattern.Solid;


                WorksheetRow row = sheet.Table.Rows.Add();

                row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                row.Cells.Add(new WorksheetCell("Report :", "HeaderStyle3"));
                WorksheetCell cell = row.Cells.Add("HSRP Total Vehicle Collection Summary");
                cell.MergeAcross = 3; // Merge two cells together
                cell.StyleID = "HeaderStyle3";

                row = sheet.Table.Rows.Add();
                row.Index = 3;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("State:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(DropDownListStateName.SelectedItem.ToString(), "HeaderStyle2"));

                row = sheet.Table.Rows.Add();
                row.Index = 4;

                DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("Report Period:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(DatePrint1, "HeaderStyle2"));
                   
                row.Cells.Add(new WorksheetCell("To:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(DatePrint2, "HeaderStyle2"));
                row = sheet.Table.Rows.Add();

                row.Index = 5;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));


                row = sheet.Table.Rows.Add();
                row.Index = 6;

                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(OrderType, "HeaderStyle2"));


                  
                row = sheet.Table.Rows.Add();

                row.Index = 7;
                //row.Cells.Add("Order Date");
                row.Cells.Add(new WorksheetCell("LOCATION", "HeaderStyle6"));
                // row.Cells.Add(new WorksheetCell("Amount", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Scooter", "HeaderStyle6"));
                //row.Cells.Add(new WorksheetCell("Order Month", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("MC", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("TW", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Tractor", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("LMV", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("LMV(Class)", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("HEAVY", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Total", "HeaderStyle6"));

                row = sheet.Table.Rows.Add();
                String StringField = String.Empty;
                String StringAlert = String.Empty;

                //row.Index = 8;


                if (recordtype == "Both")
                {
                    SQLString1 = " select (select rtolocationname from rtolocation where rtolocationid=a.rtolocationid) locationname,"
                        // + " round(sum(netamount),0) as amount, "
                                        + " SUM(CASE WHEN (VehicleType='SCOOTER')  THEN RoundOff_NetAmount ELSE 0 END ) AS [Scooter],"
                                        + "  SUM(CASE WHEN (VehicleType='MOTOR CYCLE')  THEN RoundOff_NetAmount ELSE 0 END ) AS [MC],"
                                        + "SUM(CASE WHEN (VehicleType = 'Three Wheeler') THEN RoundOff_NetAmount ELSE 0 END ) AS [3W],"
                                        + "SUM(CASE WHEN (VehicleType = 'Tractor' )  THEN RoundOff_NetAmount ELSE 0 END ) AS [Tractor],"
                                        + "SUM(CASE WHEN (VehicleType = 'LMV')   THEN RoundOff_NetAmount ELSE 0 END ) AS [4W],"
                                        + " SUM(CASE WHEN (VehicleType='LMV(Class)')  THEN RoundOff_NetAmount ELSE 0 END ) AS [4WClass],"
                                        + "SUM(CASE WHEN (Vehicletype ='MCV/HCV/TRAILERS')  THEN RoundOff_NetAmount ELSE 0 END ) AS [Heavy] , "
                                        + "SUM(CASE WHEN Vehicletype in ('SCOOTER','MOTOR CYCLE','Three Wheeler','Tractor','LMV','LMV(Class)','MCV/HCV/TRAILERS') THEN RoundOff_NetAmount ELSE 0 END ) AS [Total]"
                                        + " from HSRPRecords a where  netamount >0 and a.OrderDate between '" + ReportDate1 + "' and '" + ReportDate2 + "' and  a.HSRP_StateID=" + DropDownListStateName.SelectedValue.ToString() + ""
                                        + " group by a.rtolocationid  "
                                        + " order by (select rtolocationname from rtolocation where rtolocationid=a.rtolocationid) ";
                }

                else if (OrderType == "Normal")
                {
                    SQLString1 = " select (select rtolocationname from rtolocation where rtolocationid=a.rtolocationid) locationname,"
                        // + " round(sum(netamount),0) as amount, "
                                    + " SUM(CASE WHEN (VehicleType='SCOOTER')  THEN RoundOff_NetAmount ELSE 0 END ) AS [Scooter],"
                                    + "  SUM(CASE WHEN (VehicleType='MOTOR CYCLE')  THEN RoundOff_NetAmount ELSE 0 END ) AS [MC],"
                                    + "SUM(CASE WHEN (VehicleType = 'Three Wheeler') THEN RoundOff_NetAmount ELSE 0 END ) AS [3W],"
                                    + "SUM(CASE WHEN (VehicleType = 'Tractor' )  THEN RoundOff_NetAmount ELSE 0 END ) AS [Tractor],"
                                    + "SUM(CASE WHEN (VehicleType = 'LMV')   THEN RoundOff_NetAmount ELSE 0 END ) AS [4W],"
                                    + " SUM(CASE WHEN (VehicleType='LMV(Class)')  THEN RoundOff_NetAmount ELSE 0 END ) AS [4WClass],"
                                    + "SUM(CASE WHEN (Vehicletype ='MCV/HCV/TRAILERS')  THEN RoundOff_NetAmount ELSE 0 END ) AS [Heavy] , "
                                    + "SUM(CASE WHEN Vehicletype in ('SCOOTER','MOTOR CYCLE','Three Wheeler','Tractor','LMV','LMV(Class)','MCV/HCV/TRAILERS') THEN RoundOff_NetAmount ELSE 0 END ) AS [Total]"
                                    + " from HSRPRecords a where a.HSRP_StateID=" + DropDownListStateName.SelectedValue.ToString() + " AND  netamount >0 and a.HSRPRECORD_CREATIONDATE between '" + ReportDate1 + "' and '" + ReportDate2 + "' and   a.addrecordby in (" + recordtype + ")"
                                    + " group by a.rtolocationid  "
                                    + " order by (select rtolocationname from rtolocation where rtolocationid=a.rtolocationid) ";

                }
                else
                {

                    SQLString1 = " select (select rtolocationname from rtolocation where rtolocationid=a.rtolocationid) locationname,"
                        // + " round(sum(netamount),0) as amount, "
                                    + " SUM(CASE WHEN (VehicleType='SCOOTER')  THEN RoundOff_NetAmount ELSE 0 END ) AS [Scooter],"
                                    + "  SUM(CASE WHEN (VehicleType='MOTOR CYCLE')  THEN RoundOff_NetAmount ELSE 0 END ) AS [MC],"
                                    + "SUM(CASE WHEN (VehicleType = 'Three Wheeler') THEN RoundOff_NetAmount ELSE 0 END ) AS [3W],"
                                    + "SUM(CASE WHEN (VehicleType = 'Tractor' )  THEN RoundOff_NetAmount ELSE 0 END ) AS [Tractor],"
                                    + "SUM(CASE WHEN (VehicleType = 'LMV')   THEN RoundOff_NetAmount ELSE 0 END ) AS [4W],"
                                    + " SUM(CASE WHEN (VehicleType='LMV(Class)')  THEN RoundOff_NetAmount ELSE 0 END ) AS [4WClass],"
                                    + "SUM(CASE WHEN (Vehicletype ='MCV/HCV/TRAILERS')  THEN RoundOff_NetAmount ELSE 0 END ) AS [Heavy] , "
                                    + "SUM(CASE WHEN Vehicletype in ('SCOOTER','MOTOR CYCLE','Three Wheeler','Tractor','LMV','LMV(Class)','MCV/HCV/TRAILERS') THEN RoundOff_NetAmount ELSE 0 END ) AS [Total]"
                                    + " from HSRPRecords a where a.HSRP_StateID=" + DropDownListStateName.SelectedValue.ToString() + " AND  netamount >0 and a.HSRPRECORD_CREATIONDATE between '" + ReportDate1 + "' and '" + ReportDate2 + "' and   a.addrecordby in ('" + recordtype + "')"
                                    + " group by a.rtolocationid  "
                                    + " order by (select rtolocationname from rtolocation where rtolocationid=a.rtolocationid) ";

                }
                           
                int sno1 = 0;
                //int Total1 = 0;
                Decimal Totalround = 0;
                int TotalSCOOTERRecord1 = 0;
                int TotalMCRecord1 = 0;
                int TotalTWRecord1 = 0;
                int TotalTractorRecord1 = 0;
                int TotalLMVRecord1 = 0;
                int TotalLMV_CRecord1 = 0;
                int TotalHeavyRecord1 = 0;
                int TotalLocation1 = 0;

                    dt = Utils.GetDataTable(SQLString1, CnnString1);

                    string RTOColName = string.Empty;
                    if (dt.Rows.Count > 0)
                    {

                        foreach (DataRow dtrows in dt.Rows) // Loop over the rows.
                        {
                            sno1 = sno1 + 1;
                            row.Cells.Add(new WorksheetCell(dtrows["locationname"].ToString(), DataType.String, "HeaderStyle"));
                            //row.Cells.Add(new WorksheetCell(dtrows["amount"].ToString(), DataType.String, "HeaderStyle"));
                            row.Cells.Add(new WorksheetCell(dtrows["Scooter"].ToString(), DataType.String, "HeaderStyle"));
                            //row.Cells.Add(new WorksheetCell(dtrows["ORDERMONTH"].ToString(), DataType.String, "HeaderStyle"));
                            row.Cells.Add(new WorksheetCell(dtrows["MC"].ToString(), DataType.String, "HeaderStyle"));
                            row.Cells.Add(new WorksheetCell(dtrows["3W"].ToString(), DataType.String, "HeaderStyle"));
                            row.Cells.Add(new WorksheetCell(dtrows["Tractor"].ToString(), DataType.String, "HeaderStyle"));
                            row.Cells.Add(new WorksheetCell(dtrows["4W"].ToString(), DataType.String, "HeaderStyle"));
                            row.Cells.Add(new WorksheetCell(dtrows["4WClass"].ToString(), DataType.String, "HeaderStyle"));
                            row.Cells.Add(new WorksheetCell(dtrows["Heavy"].ToString(), DataType.String, "HeaderStyle"));
                            row.Cells.Add(new WorksheetCell(dtrows["Total"].ToString(), DataType.String, "HeaderStyle"));
                                
                            //Decimal  intAmount = Decimal.Parse(dtrows["amount"].ToString());
                            int intScooter1 = int.Parse(dtrows["Scooter"].ToString());
                            int intMC1 = int.Parse(dtrows["MC"].ToString());
                            int int3W1 = int.Parse(dtrows["3W"].ToString());
                            int intTractor1 = int.Parse(dtrows["Tractor"].ToString());
                            int int4W1 = int.Parse(dtrows["4W"].ToString());
                            int int4WClass1 = int.Parse(dtrows["4WClass"].ToString());
                            int intHeavy1 = int.Parse(dtrows["Heavy"].ToString());
                            int intTotal1 = int.Parse(dtrows["Total"].ToString());
                            //Totalround = Totalround + intAmount;
                            TotalSCOOTERRecord1 = TotalSCOOTERRecord1 + intScooter1;
                            TotalMCRecord1 = TotalMCRecord1 + intMC1;
                            TotalTWRecord1 = TotalTWRecord1 + int3W1;
                            TotalTractorRecord1 = TotalTractorRecord1 + intTractor1;
                            TotalLMVRecord1 = TotalLMVRecord1 + int4W1;
                            TotalLMV_CRecord1 = TotalLMV_CRecord1 + int4WClass1;
                            TotalHeavyRecord1 = TotalHeavyRecord1 + intHeavy1;
                            TotalLocation1 = TotalLocation1 + intTotal1;

                            row = sheet.Table.Rows.Add();
                        }

                        row = sheet.Table.Rows.Add();
                        // row.Cells.Add(new WorksheetCell("", "HeaderStyle9"));
                        row.Cells.Add(new WorksheetCell("Grand Total:", "HeaderStyle9"));
                        //row.Cells.Add(new WorksheetCell((Totalround).ToString(),"HeaderStyle9"));
                        row.Cells.Add(new WorksheetCell((TotalSCOOTERRecord1).ToString(), "HeaderStyle9"));
                        row.Cells.Add(new WorksheetCell((TotalMCRecord1).ToString(), "HeaderStyle9"));
                        row.Cells.Add(new WorksheetCell((TotalTWRecord1).ToString(), "HeaderStyle9"));
                        row.Cells.Add(new WorksheetCell((TotalTractorRecord1).ToString(), "HeaderStyle9"));
                        row.Cells.Add(new WorksheetCell((TotalLMVRecord1).ToString(), "HeaderStyle9"));
                        row.Cells.Add(new WorksheetCell((TotalLMV_CRecord1).ToString(), "HeaderStyle9"));
                        row.Cells.Add(new WorksheetCell((TotalHeavyRecord1).ToString(), "HeaderStyle9"));
                        row.Cells.Add(new WorksheetCell((TotalLocation1).ToString(), "HeaderStyle9"));


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
        
            }

            catch (Exception ex)
            {
                throw ex;
            }


        }


    }
}
}