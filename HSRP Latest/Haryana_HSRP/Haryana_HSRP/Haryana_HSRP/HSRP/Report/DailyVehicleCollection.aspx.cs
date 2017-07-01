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
    public partial class DailyVehicleCollection : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        int UserType;
        int HSRPStateID;
        int RTOLocationID;
        int intHSRPStateID;
        int intRTOLocationID;
        string OrderStatus = string.Empty;
        DateTime AuthorizationDate;
        DateTime OrderDate1;
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
                HSRPStateID = Convert.ToInt32(Session["UserHSRPStateID"].ToString());
                strUserID = Session["UID"].ToString();

                
                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                ComputerIP = Request.UserHostAddress;

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

                            labelClient.Visible = false; 
                            dropDownListClient.Visible = false;
                            FilldropDownListOrganization();
                            FilldropDownListClient();

                        }
                        else
                        { 
                           
                            FilldropDownListClient();
                            FilldropDownListOrganization(); 
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
                SQLString = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID where UserRTOLocationMapping.UserID='" + strUserID + "' ";

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

        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            try
            {
                string checkboxValue;
                if (CheckBoxStateName.Checked == true)
                {
                    checkboxValue = "Y";
                }
                else
                {
                    checkboxValue = "N";
                }
                if (DropDownListStateName.SelectedItem.Text == "--Select State--" && checkboxValue == "N")
                {
                    Page.ClientScript.RegisterStartupScript(typeof(Page), "alert", "<script language=JavaScript>alert('Please Select State Name or All State Name');</script>");
                }
                else
                {
                    LabelError.Visible = false;

                    String[] StringAuthDate = OrderDate.SelectedDate.ToString().Split('/');
                    string MonTo = ("0" + StringAuthDate[0]);
                    string MonthdateTO = MonTo.Replace("00", "0").Replace("01", "1");
                    String FromDateTo = StringAuthDate[1] + "-" + MonthdateTO + "-" + StringAuthDate[2].Split(' ')[0];
                    String From = StringAuthDate[0] + "/" + StringAuthDate[1] + "/" + StringAuthDate[2].Split(' ')[0];
                    //AuthorizationDate = new DateTime(Convert.ToInt32(StringAuthDate[2].Split(' ')[0]), Convert.ToInt32(StringAuthDate[0]), Convert.ToInt32(StringAuthDate[1]));
                    string FromDate = From + " 00:00:00"; // Convert.ToDateTime();

                    String[] StringOrderDate = HSRPAuthDate.SelectedDate.ToString().Split('/');
                    string Mon = ("0" + StringOrderDate[0]);
                    string Monthdate = Mon.Replace("00", "0").Replace("01", "1");
                    string FromDate1 = StringOrderDate[1] + "-" + Monthdate + "-" + StringOrderDate[2].Split(' ')[0];

                    String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
                    //OrderDate1 = new DateTime(Convert.ToInt32(StringOrderDate[2].Split(' ')[0]), Convert.ToInt32(StringOrderDate[0]), Convert.ToInt32(StringOrderDate[1]));
                    string ToDate = From1 + " 23:59:59";

                    DataTable RTOName; 
                    DataTable dts;
                    DataTable dt = new DataTable();
                    UserType = Convert.ToInt32(Session["UserType"]);
                    

                        string filename = "DailyVehicleAmountCollectionReport-" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                        Workbook book = new Workbook(); 
                        // Specify which Sheet should be opened and the size of window by default
                        book.ExcelWorkbook.ActiveSheetIndex = 1;
                        book.ExcelWorkbook.WindowTopX = 100;
                        book.ExcelWorkbook.WindowTopY = 200;
                        book.ExcelWorkbook.WindowHeight = 7000;
                        book.ExcelWorkbook.WindowWidth = 8000;

                        // Some optional properties of the Document
                        book.Properties.Author = "HSRP";
                        book.Properties.Title = "HSRP Daily Amount Collection Report";
                        book.Properties.Created = DateTime.Now;


                        // Add some styles to the Workbook
                        WorksheetStyle style = book.Styles.Add("HeaderStyle");
                        style.Font.FontName = "Tahoma";
                        style.Font.Size = 10;
                        style.Font.Bold = true;
                        style.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                        style.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                        style.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                        style.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                        style.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1); 

                        WorksheetStyle style5 = book.Styles.Add("HeaderStyle5");
                        style5.Font.FontName = "Tahoma";
                        style5.Font.Size = 10;
                        style5.Font.Bold = true;
                        style5.Alignment.Horizontal = StyleHorizontalAlignment.Right;
                        style5.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                        style5.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                        style5.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                        style5.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);

                     

                        WorksheetStyle style2 = book.Styles.Add("HeaderStyle2");
                        style2.Font.FontName = "Tahoma";
                        style2.Font.Size = 10;
                        style2.Font.Bold = true;
                        style.Alignment.Horizontal = StyleHorizontalAlignment.Left;

                        //style.Interior.Color = "#7F7F7F";
                        //style.Interior.Pattern = StyleInteriorPattern.Solid;
                        WorksheetStyle style8 = book.Styles.Add("HeaderStyle8");
                        style8.Font.FontName = "Tahoma";
                        style8.Font.Size = 10;
                        style8.Font.Bold = true;
                        style8.Alignment.Horizontal = StyleHorizontalAlignment.Right;
                        style8.Interior.Color = "#D4CDCD";
                        style8.Interior.Pattern = StyleInteriorPattern.Solid;

                        WorksheetStyle style9 = book.Styles.Add("HeaderStyle9");
                        style9.Font.FontName = "Tahoma";
                        style9.Font.Size = 10;
                        style9.Font.Bold = true;
                        style9.Alignment.Horizontal = StyleHorizontalAlignment.Right;
                        style9.Interior.Color = "#FCF6AE";
                        style9.Interior.Pattern = StyleInteriorPattern.Solid;


                        WorksheetStyle style3 = book.Styles.Add("HeaderStyle3");
                        style3.Font.FontName = "Tahoma";
                        style3.Font.Size = 12;
                        style3.Font.Bold = true;
                        style3.Alignment.Horizontal = StyleHorizontalAlignment.Left;

                        WorksheetStyle style4 = book.Styles.Add("HeaderStyle4");
                        style4.Font.FontName = "Tahoma";
                        style4.Font.Size = 13;
                        style4.Font.Bold = true;
                        style4.Alignment.Horizontal = StyleHorizontalAlignment.Left; 
                        Worksheet sheet = book.Worksheets.Add("HSRP Vehicle Collection Report"); 
                        sheet.Table.Columns.Add(new WorksheetColumn(50));
                        sheet.Table.Columns.Add(new WorksheetColumn(120));
                        sheet.Table.Columns.Add(new WorksheetColumn(100));
                        sheet.Table.Columns.Add(new WorksheetColumn(90));
                        sheet.Table.Columns.Add(new WorksheetColumn(100));
                        sheet.Table.Columns.Add(new WorksheetColumn(70));
                        WorksheetRow row = sheet.Table.Rows.Add();
                        // row. 
                        row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                        row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                        row.Cells.Add(new WorksheetCell("Report :", "HeaderStyle3"));
                        WorksheetCell cell = row.Cells.Add("HSRP Daily Vehicle Collection Report");
                        cell.MergeAcross = 3; // Merge two cells together
                        cell.StyleID = "HeaderStyle3"; 
                        row = sheet.Table.Rows.Add();
                        row.Index = 3;
                        row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                        row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                        row.Cells.Add(new WorksheetCell("Report Date:", "HeaderStyle2"));
                        row.Cells.Add(new WorksheetCell(DropDownListStateName.SelectedItem.Text, "HeaderStyle2"));
                        row = sheet.Table.Rows.Add(); 

                        row.Index = 4;
                        DateTime date = System.DateTime.Now;
                        string formatted = date.ToString("dd/MM/yyyy"); 
                        row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                        row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                        row.Cells.Add(new WorksheetCell("Date Generated :", "HeaderStyle2"));
                        row.Cells.Add(new WorksheetCell(formatted, "HeaderStyle2"));
                        row = sheet.Table.Rows.Add();
                        row.Index = 5; 
                        row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                        row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                        row.Cells.Add(new WorksheetCell("Report Date:", "HeaderStyle2"));
                        row.Cells.Add(new WorksheetCell("From Date : "+ FromDate + "To Date : " + ToDate + "", "HeaderStyle2"));
                        row = sheet.Table.Rows.Add(); 
                        row.Index = 7;
                        //row.Cells.Add("Order Date");
                        row.Cells.Add(new WorksheetCell("S.No.", "HeaderStyle")); 
                        row.Cells.Add(new WorksheetCell("Location", "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell("Particulars", "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell("No. of Vehicles", "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell("Rate Per Vehicle", "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell("Amount", "HeaderStyle"));
                        row = sheet.Table.Rows.Add(); 
                        String StringField = String.Empty;
                        String StringAlert = String.Empty; 
                     
                        row.Index = 8;
                        int sno = 0;
                        string StateColName = string.Empty;
                        
                            
                        UserType = Convert.ToInt32(Session["UserType"]);
                        if (UserType == 0)
                        {
                            int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                            SQLString = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID where a.LocationType!='District' and    a.HSRP_StateID='" + intHSRPStateID + "'";
                        }
                        else
                        {
                            SQLString = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID where  a.LocationType!='District' and  UserRTOLocationMapping.UserID='" + strUserID + "' ";
                        } 
                             

                        // ajay
                        Int64 NoofVechicleTotalAll = 0;
                        decimal RatePerVehicleTotalAll = 0;
                        decimal TotalAmountTotalAll = 0;
                         
                        RTOName = Utils.GetDataTable(SQLString.ToString(), CnnString);
                        if (RTOName.Rows.Count > 0)
                        { 
                            for (int i = 0; i <= RTOName.Rows.Count - 1; i++)
                            {
                                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);

                                Int64 NoofVechicle = 0;
                                decimal RatePerVehicle = 0;
                                decimal TotalAmount = 0;


                                Int64 NoofVechicle1 = 0;
                                decimal RatePerVehicle1 = 0;
                                decimal TotalAmount1 = 0;
                                SQLString = "select  VehicleType, COUNT(VehicleType) as [No. of Vehicles], SUM(NetAmount) as [Total Amount], (select HSRPStateName from HSRPState where HSRP_StateID='" + intHSRPStateID + "') as StateName  from HSRPRecords  where HSRP_StateID='" + intHSRPStateID + "' and RTOLocationID='" + RTOName.Rows[i]["RTOLocationID"] + "' and hsrprecord_creationdate  between '" + FromDate + "' and '" + ToDate + "' and VehicleType is not null group by VehicleType";
                                //SQLString = "select(select HSRPStateName from HSRPState where HSRP_StateID=HSRPRecords.HSRP_StateID) as StateName, (select rtolocationname from RTOLocation  where HSRP_StateID=HSRPRecords.HSRP_StateID and RTOLocationID ='" + RTOName.Rows[i]["RTOLocationID"] + "' ) as location, VehicleType, COUNT(VehicleType) as [No. of Vehicles], SUM(TotalAmount) as [Total Amount]  from HSRPRecords  where HSRP_StateID='" + intHSRPStateID + "' and OrderDate between '" + ReportStartDate + "' and  '" + ReportEndDate + "' group by HSRPRecords.HSRP_StateID,HSRPRecords.RTOLocationID,HSRPRecords.VehicleType order by location";
                                
                                dt = Utils.GetDataTable(SQLString.ToString(), CnnString);
                                string RTOColName = string.Empty;
                                if (dt.Rows.Count > 0)
                                {
                                    row = sheet.Table.Rows.Add();
                                    foreach (DataRow dtrows in dt.Rows) // Loop over the rows.
                                    {
                                        NoofVechicle = NoofVechicle + Convert.ToInt64(dtrows["No. of Vehicles"]);
                                        TotalAmount = TotalAmount + Convert.ToDecimal(dtrows["Total Amount"].ToString());
                                        RatePerVehicle = Convert.ToDecimal(TotalAmount) / NoofVechicle;


                                        NoofVechicle1 = NoofVechicle1 + Convert.ToInt64(dtrows["No. of Vehicles"]);
                                        TotalAmount1 = TotalAmount1 + Convert.ToDecimal(dtrows["Total Amount"].ToString());
                                        RatePerVehicle1 = Convert.ToDecimal(TotalAmount1) / NoofVechicle1;
                                         
                                            if (RTOColName != RTOName.Rows[i]["RTOLocationName"].ToString())
                                            {

                                                sno = sno + 1;
                                                row.Cells.Add(new WorksheetCell(Convert.ToInt16(sno).ToString(), DataType.String, "HeaderStyle"));
                                                RTOColName = RTOName.Rows[i]["RTOLocationName"].ToString();
                                                row.Cells.Add(new WorksheetCell(RTOName.Rows[i]["RTOLocationName"].ToString(), "HeaderStyle"));
                                            }
                                            else
                                            {
                                                row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                                                row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                                            } 
                                        row.Cells.Add(new WorksheetCell(dtrows["VehicleType"].ToString(), DataType.String, "HeaderStyle"));
                                        row.Cells.Add(new WorksheetCell(dtrows["No. of Vehicles"].ToString(), DataType.Number, "HeaderStyle5"));
                                        row.Cells.Add(new WorksheetCell(Convert.ToString(Math.Round(RatePerVehicle1,2)), DataType.Number, "HeaderStyle5"));
                                      //  row.Cells.Add(new WorksheetCell( Math.Round( Convert.ToDecimal(dtrows["Total Amount"].ToString()),2).ToString(), DataType.Number, "HeaderStyle5"));
                                        row.Cells.Add(new WorksheetCell(Math.Round(Convert.ToDecimal(dtrows["Total Amount"].ToString()),0).ToString(),DataType.Number,"HeaderStyle5"));
                                        //row.Cells.Add(new WorksheetCell(Convert.ToInt32(TotalAmount).ToString(), "HeaderStyle5"));
                                        row = sheet.Table.Rows.Add();
                                        NoofVechicle1 = 0;
                                        TotalAmount1 = 0;
                                        RatePerVehicle1 = 0;

                                    }

                                    NoofVechicleTotalAll = NoofVechicleTotalAll + Convert.ToInt64(NoofVechicle);
                                    TotalAmountTotalAll = Convert.ToDecimal( TotalAmountTotalAll) + TotalAmount; 
                                    row = sheet.Table.Rows.Add(); 
                                    row.Cells.Add(new WorksheetCell("", "HeaderStyle8"));
                                    row.Cells.Add(new WorksheetCell("Total :", "HeaderStyle8")); 
                                    row.Cells.Add(new WorksheetCell("", "HeaderStyle8"));
                                    row.Cells.Add(new WorksheetCell(Convert.ToInt64(NoofVechicle).ToString(), "HeaderStyle8"));
                                    row.Cells.Add(new WorksheetCell("", "HeaderStyle8"));
                                    row.Cells.Add(new WorksheetCell(Math.Round(TotalAmount).ToString(), "HeaderStyle8"));

                                    row = sheet.Table.Rows.Add();

                                    NoofVechicle = 0;
                                    RatePerVehicle = 0;
                                    TotalAmount = 0;
                                }
                                else
                                {   row = sheet.Table.Rows.Add();
                                    sno = sno + 1;
                                    NoofVechicle = 0;
                                    TotalAmount = 0;
                                    RatePerVehicle = 0; 
                                    row.Cells.Add(new WorksheetCell(Convert.ToInt16(sno).ToString(), DataType.String, "HeaderStyle")); 
                                    row.Cells.Add(new WorksheetCell(RTOName.Rows[i]["RTOLocationName"].ToString(), "HeaderStyle"));
                                    row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle5"));
                                    row.Cells.Add(new WorksheetCell("0", DataType.String, "HeaderStyle5"));
                                    row.Cells.Add(new WorksheetCell("0", DataType.String, "HeaderStyle5"));
                                    row.Cells.Add(new WorksheetCell("0", DataType.String, "HeaderStyle5"));
                                    //row.Cells.Add(new WorksheetCell(Convert.ToInt32(TotalAmount).ToString(), "HeaderStyle5"));
                                    row = sheet.Table.Rows.Add();
                                    row = sheet.Table.Rows.Add();
                                    row.Cells.Add(new WorksheetCell("", "HeaderStyle8"));
                                    row.Cells.Add(new WorksheetCell("Total :", "HeaderStyle8")); 
                                    row.Cells.Add(new WorksheetCell("", "HeaderStyle8"));
                                    row.Cells.Add(new WorksheetCell("0", "HeaderStyle8"));
                                    row.Cells.Add(new WorksheetCell("0", "HeaderStyle8"));
                                    row.Cells.Add(new WorksheetCell("0", "HeaderStyle8")); 
                                    row = sheet.Table.Rows.Add(); 
                                    NoofVechicle = 0;
                                    RatePerVehicle = 0;
                                    TotalAmount = 0;
                                }
                            }
                        }
                        row = sheet.Table.Rows.Add();
                        row = sheet.Table.Rows.Add();
                        row.Cells.Add(new WorksheetCell("", "HeaderStyle9"));
                        row.Cells.Add(new WorksheetCell("Grand Total :", "HeaderStyle9"));
                        row.Cells.Add(new WorksheetCell("", "HeaderStyle9"));
                        row.Cells.Add(new WorksheetCell(Convert.ToInt64(NoofVechicleTotalAll).ToString(), "HeaderStyle9"));
                        row.Cells.Add(new WorksheetCell("", "HeaderStyle9"));
                        row.Cells.Add(new WorksheetCell(Math.Round(TotalAmountTotalAll).ToString(), "HeaderStyle9"));
                     
                        NoofVechicleTotalAll = 0;
                        TotalAmountTotalAll = 0;

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
                LabelError.Visible = true;
                LabelError.Text = "Error in Populating Grid :" + ex.Message.ToString();
            } 
        }
    }

}