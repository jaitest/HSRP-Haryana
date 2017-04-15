﻿using System;
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
    public partial class StockInHandStateWiseReport : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string SQLString1 = string.Empty;
        string SQLString2 = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        int UserType;
        string HSRPStateID = string.Empty;
        // int RTOLocationID;
        int intHSRPStateID;
        // int intRTOLocationID;
        string OrderStatus = string.Empty;
        DateTime AuthorizationDate;
        DateTime OrderDate1;
        DataProvider.BAL bl = new DataProvider.BAL();

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
                            //labelOrganization.Visible = true;
                            //DropDownListStateName.Visible = true;

                            FilldropDownListOrganization();


                        }
                        else
                        {
                          //  FilldropDownListOrganization();

                            labelOrganization.Enabled = true;
                            DropDownListStateName.Enabled = false;
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
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState where  ActiveStatus='Y' Order by HSRPStateName";
                Utils.PopulateDropDownList(DropDownListStateName, SQLString.ToString(), CnnString, "--Select State--");
                // DropDownListStateName.SelectedIndex = DropDownListStateName.Items.Count - 1;
            }
            else
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID='"+HSRPStateID+"' and ActiveStatus='Y' Order by HSRPStateName";
                DataSet dts = Utils.getDataSet(SQLString, CnnString);
                DropDownListStateName.DataSource = dts;
                DropDownListStateName.DataBind();

            }
        }

        //private void FilldropDownListClient()
        //{
        //    if (UserType.Equals(0))
        //    {
        //        int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
        //        SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + intHSRPStateID + " and ActiveStatus!='N'  Order by RTOLocationName";

        //        Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), CnnString, "--Select Location--");
        //        // dropDownListClient.SelectedIndex = dropDownListClient.Items.Count - 1;
        //    }
        //    else
        //    {
        //        // SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where (RTOLocationID=" + RTOLocationID + " or distRelation=" + RTOLocationID + " ) and ActiveStatus!='N'   Order by RTOLocationName";
        //        SQLString = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID where UserRTOLocationMapping.UserID='" + strUserID + "' ";

        //        DataSet dss = Utils.getDataSet(SQLString, CnnString);
        //        dropDownListClient.DataSource = dss;
        //        dropDownListClient.DataBind();

        //    }
        //}

        #endregion



        string FromDate, ToDate;
        DataSet ds = new DataSet();
        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            try
            {
                LabelError.Text = "";

                String[] StringAuthDate = OrderDate.SelectedDate.ToString().Split('/');
                string MonTo = ("0" + StringAuthDate[0]);
                string MonthdateTO = MonTo.Replace("00", "0").Replace("01", "1");
                String FromDateTo = StringAuthDate[1] + "-" + MonthdateTO + "-" + StringAuthDate[2].Split(' ')[0];
                String From = StringAuthDate[0] + "/" + StringAuthDate[1] + "/" + StringAuthDate[2].Split(' ')[0];

               // String From = StringAuthDate[2].Split(' ')[0] + "/" + StringAuthDate[1] + "/" + StringAuthDate[0];
                //AuthorizationDate = new DateTime(Convert.ToInt32(StringAuthDate[2].Split(' ')[0]), Convert.ToInt32(StringAuthDate[0]), Convert.ToInt32(StringAuthDate[1]));
                string FromDate = From + " 00:00:01"; // Convert.ToDateTime();

                String[] StringOrderDate = HSRPAuthDate.SelectedDate.ToString().Split('/');
                string Mon = ("0" + StringOrderDate[0]);
                string Monthdate = Mon.Replace("00", "0").Replace("01", "1");
                string FromDate1 = StringOrderDate[1] + "-" + Monthdate + "-" + StringOrderDate[2].Split(' ')[0];

               String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];

              //  String From1 = StringOrderDate[2].Split(' ')[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[0];
                //OrderDate1 = new DateTime(Convert.ToInt32(StringOrderDate[2].Split(' ')[0]), Convert.ToInt32(StringOrderDate[0]), Convert.ToInt32(StringOrderDate[1]));
                string ToDate = From1 + " 23:59:59";


                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                //int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);
                DataTable StateName;
                DataTable dts;
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                DataTable dt2 = new DataTable();

                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                //int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);

                string filename = "StockInHandStateWiseReport" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                Workbook book = new Workbook();

                // Specify which Sheet should be opened and the size of window by default
                book.ExcelWorkbook.ActiveSheetIndex = 1;
                book.ExcelWorkbook.WindowTopX = 100;
                book.ExcelWorkbook.WindowTopY = 200;
                book.ExcelWorkbook.WindowHeight = 7000;
                book.ExcelWorkbook.WindowWidth = 8000;

                // Some optional properties of the Document
                book.Properties.Author = "HSRP";
                book.Properties.Title = "StockInHandStateWiseReport";
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

                WorksheetStyle style4 = book.Styles.Add("HeaderStyle1");
                style4.Font.FontName = "Tahoma";
                style4.Font.Size = 10;
                style4.Font.Bold = false;
                style4.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                style4.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                style4.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                style4.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                style4.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


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



                Worksheet sheet11 = book.Worksheets.Add("SCHEDULE-F");
                sheet11.Table.Columns.Add(new WorksheetColumn(60));
                sheet11.Table.Columns.Add(new WorksheetColumn(205));
                sheet11.Table.Columns.Add(new WorksheetColumn(100));
                sheet11.Table.Columns.Add(new WorksheetColumn(130));

                sheet11.Table.Columns.Add(new WorksheetColumn(100));
                sheet11.Table.Columns.Add(new WorksheetColumn(120));
                sheet11.Table.Columns.Add(new WorksheetColumn(112));
                sheet11.Table.Columns.Add(new WorksheetColumn(109));
                sheet11.Table.Columns.Add(new WorksheetColumn(105));
                sheet11.Table.Columns.Add(new WorksheetColumn(160));


                Worksheet sheet = book.Worksheets.Add("Export Report");
                sheet.Table.Columns.Add(new WorksheetColumn(60));
                sheet.Table.Columns.Add(new WorksheetColumn(205));
                sheet.Table.Columns.Add(new WorksheetColumn(100));
                sheet.Table.Columns.Add(new WorksheetColumn(130));

                sheet.Table.Columns.Add(new WorksheetColumn(100));
                sheet.Table.Columns.Add(new WorksheetColumn(120));
                sheet.Table.Columns.Add(new WorksheetColumn(112));
                sheet.Table.Columns.Add(new WorksheetColumn(109));
                sheet.Table.Columns.Add(new WorksheetColumn(105));
                sheet.Table.Columns.Add(new WorksheetColumn(160));


                WorksheetRow row = sheet.Table.Rows.Add();


                row.Index = 2;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                row.Cells.Add(new WorksheetCell("SCHEDULE-F", "HeaderStyle3"));

                row = sheet.Table.Rows.Add();

                row.Index = 3;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                row.Cells.Add(new WorksheetCell(" ", "HeaderStyle3"));
                WorksheetCell cell = row.Cells.Add("Stock In Hand State Wise Report");
                cell.MergeAcross = 3; // Merge two cells together
                cell.StyleID = "HeaderStyle3";

                row = sheet.Table.Rows.Add();

                row.Index = 5;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("State:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(DropDownListStateName.SelectedItem.ToString(), "HeaderStyle2"));

                row = sheet.Table.Rows.Add();
                //  Skip one row, and add some text
                row.Index = 6;

                DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("Report Date :", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(From.ToString() + " - " + From1.ToString(), "HeaderStyle2"));
                row = sheet.Table.Rows.Add();
                row.Index = 7;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("Generated Date time:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(DateTime.Now.ToString(), "HeaderStyle2"));
                row = sheet.Table.Rows.Add();



                row.Index = 8;
                //row.Cells.Add("Order Date");
                row.Cells.Add(new WorksheetCell("Sl.No.", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Location", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("200X100 MM-WHITE", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("200X100 MM-YELLOW ", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("285X45 MM-WHITE", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("285X45 MM-YELLOW ", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("340X200 MM-WHITE ", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("340X200 MM-YELLOW ", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("500X120 MM-WHITE ", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("500X120 MM-YELLOW ", "HeaderStyle"));
                
            
                row = sheet.Table.Rows.Add();
                String StringField = String.Empty;
                String StringAlert = String.Empty;

                //row.Index = 9;

                UserType = Convert.ToInt32(Session["UserType"]);

                
                if (ddlplatestatus.SelectedItem.ToString() == "Blank Plate")
                {
                    SQLString = @"select sum( convert(int,pl.BlanckPlate_200_100White)) as BlankPlate_200X100MMWHITE, sum( convert(int,pl.BlanckPlate_200_100Yellow)) as BlankPlate_200X100MMYELLOW,sum( convert(int,pl.BlanckPlate_285_45White)) as BlankPlate_285X45MMWHITE,sum( convert(int,pl.BlanckPlate_285_45Yellow)) as BlankPlate_285X45MMYELLOW,sum(convert(int,pl.BlanckPlate_340_200White)) as BlankPlate_340X200MMWHITE,sum(convert(int,pl.BlanckPlate_340_200Yellow)) as BlankPlate_340X200MMYELLOW,sum(convert(int,pl.BlanckPlate_500_120White)) as BlankPlate_500X120MMWHITE,sum(convert(int,pl.BlanckPlate_500_120Yellow)) as blankPlate_500X120MMYELLOW,
                    (select rtolocation.rtolocationname from rtolocation where rtolocation.rtolocationid=pl.rtolocationid) as Location from PlateStockEntry as pl where HSRP_StateID='" + DropDownListStateName.SelectedValue + "' and Stock_date  between '" + FromDate + "' and '" + ToDate + "' group by rtolocationid";
                }
                if (ddlplatestatus.SelectedItem.ToString() == "Embossed Plate")
                {
                    SQLString = @"select sum( convert(int,pl.EmbosedPlate200_100White)) as EmbosedPlate_200X100MMWHITE, sum( convert(int,pl.EmbosedPlate200_100Yellow))as EmbosedPlate_200X100MMYELLOW,sum( convert(int,pl.EmbosedPlate285_45White))as EmbosedPlate_285X45MMWHITE,sum( convert(int,pl.EmbosedPlate285_45Yellow)) as EmbosedPlate_285X45MMYELLOW,sum(convert(int,pl.EmbosedPlate_340_200White))as EmbosedPlate_340X200MMWHITE,sum(convert(int,pl.EmbosedPlate_340_200Yellow))as EmbosedPlate_340X200MMYELLOW, sum(convert(int,pl.EmbosedPlate_500_120White))as EmbosedPlate_500X120MMWHITE,sum(convert(int,pl.EmbosedPlate_500_120Yellow))as EmbosedPlate_500X120MMYELLOW,
                    (select rtolocation.rtolocationname from rtolocation where rtolocation.rtolocationid=pl.rtolocationid) as Location from PlateStockEntry as pl where HSRP_StateID='" + DropDownListStateName.SelectedValue + "' and Stock_date  between '" + FromDate + "' and '" + ToDate + "' group by rtolocationid";
                }
                if (ddlplatestatus.SelectedItem.ToString() == "Closed Plate")
                {
                    SQLString = @"select sum( convert(int,pl.ClosedPlate_200_100White)) as ClosedPlate_200X100MMWHITE, sum( convert(int,pl.ClosedPlate_200_100Yellow))as ClosedPlate_200X100MMYELLOW,sum( convert(int,pl.ClosedPlate_285_45White))as ClosedPlate_285X45MMWHITE,sum( convert(int,pl.ClosedPlate_285_45Yellow)) as ClosedPlate_285X45MMYELLOW,sum(convert(int,pl. ClosedPlate_340_200White))as ClosedPlate_340X200MMWHITE,sum(convert(int,pl.ClosedPlate_340_200Yellow))as ClosedPlate_340X200MMYELLOW,sum(convert(int,pl. ClosedPlate_500_120White))as  ClosedPlate_500X120MMWHITE,sum(convert(int,pl. ClosedPlate_500_120Yellow))as  ClosedPlate_500X120MMYELLOW,
                    (select rtolocation.rtolocationname from rtolocation where rtolocation.rtolocationid=pl.rtolocationid) as Location from PlateStockEntry as pl where HSRP_StateID='"+DropDownListStateName.SelectedValue+"' and Stock_date  between '"+FromDate+"' and '"+ToDate+"' group by rtolocationid ";
                }

                dt = Utils.GetDataTable(SQLString, CnnString);


                string RTOColName = string.Empty;
                int sno = 0;
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dtrows in dt.Rows) // Loop over the rows.
                    {

                       
                        sno = sno + 1;
                        row = sheet.Table.Rows.Add();
                        row.Cells.Add(new WorksheetCell(sno.ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["Location"].ToString(), DataType.String, "HeaderStyle1"));
                        if (ddlplatestatus.SelectedItem.ToString() == "Blank Plate")
                        {
                            
                            row.Cells.Add(new WorksheetCell(dtrows["BlankPlate_200X100MMWHITE"].ToString(), DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell(dtrows["BlankPlate_200X100MMYELLOW"].ToString(), DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell(dtrows["BlankPlate_285X45MMWHITE"].ToString(), DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell(dtrows["BlankPlate_285X45MMYELLOW"].ToString(), DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell(dtrows["BlankPlate_340X200MMWHITE"].ToString(), DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell(dtrows["BlankPlate_340X200MMYELLOW"].ToString(), DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell(dtrows["BlankPlate_500X120MMWHITE"].ToString(), DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell(dtrows["BlankPlate_500X120MMYELLOW"].ToString(), DataType.String, "HeaderStyle1"));

                        }
                       
                        if (ddlplatestatus.SelectedItem.ToString() == "Embossed Plate")
                        {
                            
                            row.Cells.Add(new WorksheetCell(dtrows["EmbosedPlate_200X100MMWHITE"].ToString(), DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell(dtrows["EmbosedPlate_200X100MMYELLOW"].ToString(), DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell(dtrows["EmbosedPlate_285X45MMWHITE"].ToString(), DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell(dtrows["EmbosedPlate_285X45MMYELLOW"].ToString(), DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell(dtrows["EmbosedPlate_340X200MMWHITE"].ToString(), DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell(dtrows["EmbosedPlate_340X200MMYELLOW"].ToString(), DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell(dtrows["EmbosedPlate_500X120MMWHITE"].ToString(), DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell(dtrows["EmbosedPlate_500X120MMYELLOW"].ToString(), DataType.String, "HeaderStyle1"));
                        }
                        if (ddlplatestatus.SelectedItem.ToString() == "Closed Plate")
                        {
                            
                            row.Cells.Add(new WorksheetCell(dtrows["ClosedPlate_200X100MMWHITE"].ToString(), DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell(dtrows["ClosedPlate_200X100MMYELLOW"].ToString(), DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell(dtrows["ClosedPlate_285X45MMWHITE"].ToString(), DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell(dtrows["ClosedPlate_285X45MMYELLOW"].ToString(), DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell(dtrows["ClosedPlate_340X200MMWHITE"].ToString(), DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell(dtrows["ClosedPlate_340X200MMYELLOW"].ToString(), DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell(dtrows["ClosedPlate_500X120MMWHITE"].ToString(), DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell(dtrows["ClosedPlate_500X120MMYELLOW"].ToString(), DataType.String, "HeaderStyle1"));
                        }


                    }


                    row = sheet.Table.Rows.Add();
                    HttpContext context = HttpContext.Current;
                    context.Response.Clear();
                    book.Save(Response.OutputStream);

                    context.Response.ContentType = "application/vnd.ms-excel";

                    context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                    context.Response.End();

                }



            }

            catch (Exception ex)
            {
                LabelError.Text = "Error in Populating Grid :" + ex.Message.ToString();
            }
        }
    }
}