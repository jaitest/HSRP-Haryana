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
public partial class OrderStatusReportDealer: System.Web.UI.Page
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
                       // FilldropDownListClient();
                        OrderDatefrom.SelectedDate = System.DateTime.Now;
                        OrderDateto.SelectedDate = System.DateTime.Now;


                    }
                    else
                    {

                        // labelOrganization.Visible = true;
                        DropDownListStateName.Visible = true;
                        //dropDownListClient.Visible = true;
                        FilldropDownListOrganization();
                       // FilldropDownListClient();

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

    //private void FilldropDownListClient()
    //{

    //    if (UserType1.Equals(0))
    //    {
            
    //        SQLString1 = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + HSRP_StateID1 + " and ActiveStatus!='N'  Order by RTOLocationName";

    //        Utils.PopulateDropDownList(ddlRtoLocation, SQLString1.ToString(), CnnString1, "--Select Location--");
    //        // dropDownListClient.SelectedIndex = dropDownListClient.Items.Count - 1;
    //    }
    //    else
    //    {
    //        // SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where (RTOLocationID=" + RTOLocationID + " or distRelation=" + RTOLocationID + " ) and ActiveStatus!='N'   Order by RTOLocationName";
    //        SQLString1 = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID where  a.LocationType!='District' and UserRTOLocationMapping.UserID='" + strUserID1 + "' ";

    //        DataSet dss = Utils.getDataSet(SQLString1, CnnString1);
    //        ddlRtoLocation.DataSource = dss;
    //        ddlRtoLocation.DataBind();
    //    }
    //}

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
       // FilldropDownListClient();
        
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
        else if (ddlRtoLocation.SelectedItem.Text == "")
        {

        }
        return blnvalid;

    }

    protected void btnexport_Click(object sender, EventArgs e)
    {
        if (ddlRtoLocation.SelectedItem.ToString() == "--Select Record Type--")
        {
            Label1.Text = "Please Select Record Type";
            return;
        }
        if (ddlRtoLocation.SelectedItem.ToString() == "HHT")
        {
            Label1.Text = "Please Select Dealer ";
            return;
        }
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

                string filename = "OrderStatus-ReportDealer" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                Workbook book = new Workbook();

                // Specify which Sheet should be opened and the size of window by default
                book.ExcelWorkbook.ActiveSheetIndex = 1;
                book.ExcelWorkbook.WindowTopX = 100;
                book.ExcelWorkbook.WindowTopY = 200;
                book.ExcelWorkbook.WindowHeight = 7000;
                book.ExcelWorkbook.WindowWidth = 8000;

                // Some optional properties of the Document
                book.Properties.Author = "HSRP";
                book.Properties.Title = "HSRP Order Status Report Dealer";
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

                Worksheet sheet = book.Worksheets.Add("HSRP Order Status Report For Dealer");
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
                WorksheetCell cell = row.Cells.Add("Order Status Dealer Report");
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
                       
                            row.Index = 7;
                            //row.Cells.Add("Order Date");
                            row.Cells.Add(new WorksheetCell("SR.No", "HeaderStyle6"));
                            row.Cells.Add(new WorksheetCell("Location Name", "HeaderStyle6"));
                            row.Cells.Add(new WorksheetCell("Vehicle RegNo", "HeaderStyle6"));
                            row.Cells.Add(new WorksheetCell("Order Date", "HeaderStyle6"));
                            row.Cells.Add(new WorksheetCell("Embossing Date", "HeaderStyle6"));
                            row.Cells.Add(new WorksheetCell("Dealer Id", "HeaderStyle6"));
                            row.Cells.Add(new WorksheetCell("Vehicle Type", "HeaderStyle6"));
                            row.Cells.Add(new WorksheetCell("Front Laser Code", "HeaderStyle6"));
                            row.Cells.Add(new WorksheetCell("Rear Laser Code", "HeaderStyle6"));
                            row.Cells.Add(new WorksheetCell("OrderStatus", "HeaderStyle6"));
                            

                            row = sheet.Table.Rows.Add();
                            String StringField = String.Empty;
                            String StringAlert = String.Empty;

                            //row.Index = 8;

                            SQLString1 = "select r.RTOLocationName,VehicleRegNo,convert(varchar(20),OrderDate,103) as OrderDate,convert(varchar(20),OrderEmbossingDate,103) as OrderEmbossingDate ,DealerId,VehicleType,HSRP_Front_LaserCode,HSRP_Rear_LaserCode,OrderStatus from HSRPRecords inner join rtolocation as r on r.rtolocationid=hsrprecords.rtolocationid where hsrprecords.hsrp_stateid='" + DropDownListStateName.SelectedValue.ToString() + "' and addrecordby='Dealer' and HsrpRecord_creationdate between '" + ReportDate1 + "' and '" + ReportDate2 + "' order by r.rtolocationname";
                           
                            int sno1 = 0;
                          
                                                           
                            dt = Utils.GetDataTable(SQLString1, CnnString1);

                            string RTOColName = string.Empty;
                            if (dt.Rows.Count > 0)
                            {

                                foreach (DataRow dtrows in dt.Rows) // Loop over the rows.
                                {
                                    sno1 = sno1 + 1;
                                    row.Cells.Add(new WorksheetCell(sno1.ToString(), DataType.String, "HeaderStyle"));
                                    row.Cells.Add(new WorksheetCell(dtrows["RTOLocationName"].ToString(), DataType.String, "HeaderStyle"));
                                    row.Cells.Add(new WorksheetCell(dtrows["VehicleRegNo"].ToString(), DataType.String, "HeaderStyle"));
                                    row.Cells.Add(new WorksheetCell(dtrows["OrderDate"].ToString(), DataType.String, "HeaderStyle"));
                                    row.Cells.Add(new WorksheetCell(dtrows["OrderEmbossingDate"].ToString(), DataType.String, "HeaderStyle"));
                                    row.Cells.Add(new WorksheetCell(dtrows["DealerId"].ToString(), DataType.String, "HeaderStyle"));
                                    row.Cells.Add(new WorksheetCell(dtrows["VehicleType"].ToString(), DataType.String, "HeaderStyle"));
                                    row.Cells.Add(new WorksheetCell(dtrows["HSRP_Front_LaserCode"].ToString(), DataType.String, "HeaderStyle"));
                                    row.Cells.Add(new WorksheetCell(dtrows["HSRP_Rear_LaserCode"].ToString(), DataType.String, "HeaderStyle"));
                                    row.Cells.Add(new WorksheetCell(dtrows["OrderStatus"].ToString(), DataType.String, "HeaderStyle"));

                                    row = sheet.Table.Rows.Add();
                               
                                }

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