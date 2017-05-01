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
    public partial class DailyAPEmbossingReport : System.Web.UI.Page
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
                            FilldropDownListOrganization();

                            labelOrganization.Enabled = false;
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
                //AuthorizationDate = new DateTime(Convert.ToInt32(StringAuthDate[2].Split(' ')[0]), Convert.ToInt32(StringAuthDate[0]), Convert.ToInt32(StringAuthDate[1]));
                string FromDate = From + " 00:00:00"; // Convert.ToDateTime();

                String[] StringOrderDate = HSRPAuthDate.SelectedDate.ToString().Split('/');
                string Mon = ("0" + StringOrderDate[0]);
                string Monthdate = Mon.Replace("00", "0").Replace("01", "1");
                string FromDate1 = StringOrderDate[1] + "-" + Monthdate + "-" + StringOrderDate[2].Split(' ')[0];

                String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
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

                string filename = "Andhra Pradesh Embossing Report" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                Workbook book = new Workbook();

                // Specify which Sheet should be opened and the size of window by default
                book.ExcelWorkbook.ActiveSheetIndex = 1;
                book.ExcelWorkbook.WindowTopX = 100;
                book.ExcelWorkbook.WindowTopY = 200;
                book.ExcelWorkbook.WindowHeight = 7000;
                book.ExcelWorkbook.WindowWidth = 8000;

                // Some optional properties of the Document
                book.Properties.Author = "HSRP";
                book.Properties.Title = "Daily Embossing Report";
                book.Properties.Created = DateTime.Now;


                // Add some styles to the Workbook
                WorksheetStyle style = book.Styles.Add("HeaderStyle");
                style.Font.FontName = "Tahoma";
                style.Font.Size = 10;
                style.Font.Bold = true;
                style.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                style.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                style.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                style.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                style.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);

                WorksheetStyle style4 = book.Styles.Add("HeaderStyle1");
                style4.Font.FontName = "Tahoma";
                style4.Font.Size = 10;
                style4.Font.Bold = false;
                style4.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                style4.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                style4.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                style4.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                style4.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


                WorksheetStyle style6 = book.Styles.Add("HeaderStyle6");
                style6.Font.FontName = "Tahoma";
                style6.Font.Size = 10;
                style6.Font.Bold = true;
                style6.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                style6.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                style6.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                style6.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                style6.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


                WorksheetStyle style8 = book.Styles.Add("HeaderStyle8");
                style8.Font.FontName = "Tahoma";
                style8.Font.Size = 10;
                style8.Font.Bold = true;
                style8.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                style8.Interior.Color = "#D4CDCD";
                style8.Interior.Pattern = StyleInteriorPattern.Solid;

                WorksheetStyle style5 = book.Styles.Add("HeaderStyle5");
                style5.Font.FontName = "Tahoma";
                style5.Font.Size = 10;
                style5.Font.Bold = true;
                style5.Alignment.Horizontal = StyleHorizontalAlignment.Left;
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

                Worksheet sheet = book.Worksheets.Add("AP Data Embossing Report");
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
                //row.Index = 0;
                //row.Cells.Add(new WorksheetCell("", "HeaderStyle5"));
                //row.Cells.Add(new WorksheetCell(" ", "HeaderStyle5"));
                //WorksheetCell cell5 = row.Cells.Add("ANNEXURE");
                //cell5.MergeAcross = 3; // Merge two cells together
                //cell5.StyleID = "HeaderStyle5";

               // row.Index = 1;
                //row.Cells.Add(new WorksheetCell("", "HeaderStyle5"));
                //row.Cells.Add(new WorksheetCell(" ", "HeaderStyle5"));
                //WorksheetCell cell4 = row.Cells.Add("(shedule 2 ,Clause 5.5.1(a))");
                //cell4.MergeAcross = 3; // Merge two cells together
                //cell4.StyleID = "HeaderStyle5";

                //row.Index = 2;
               // row.Index = 1;
               // row.Cells.Add(new WorksheetCell("", "HeaderStyle5"));
               // row.Cells.Add(new WorksheetCell("", "HeaderStyle5"));
               //// WorksheetCell cell5 = row.Cells.Add("ANNEXURE I");
               // //cell5.MergeAcross = 3; // Merge two cells togetherto 
               //// cell5.StyleID = "HeaderStyle5";

               // row = sheet.Table.Rows.Add();


               // row.Index = 2;
               // row.Cells.Add(new WorksheetCell("", "HeaderStyle5"));
               // row.Cells.Add(new WorksheetCell("", "HeaderStyle5"));
               // //WorksheetCell cell6 = row.Cells.Add("(shedule 2 ,Clause 5.5.1 (a))");
               // //cell6.MergeAcross = 3; // Merge two cells togetherto 
               //// cell6.StyleID = "HeaderStyle5";

               // row = sheet.Table.Rows.Add();


                row.Index = 3;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                row.Cells.Add(new WorksheetCell("Report :", "HeaderStyle3"));
                WorksheetCell cell = row.Cells.Add("Daily Report from Embossing Stations");
                cell.MergeAcross = 8; // Merge two cells togetherto 
                cell.StyleID = "HeaderStyle3";

                row = sheet.Table.Rows.Add();

                row.Index = 4;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("State:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("Andhra Pradesh", "HeaderStyle2"));
                                //  Skip one row, and add some text
                row.Index = 5;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("Generated Date time:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(DateTime.Now.ToString(), "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("Report Date :", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(From.ToString() + " - " + From1.ToString(), "HeaderStyle2"));
                row = sheet.Table.Rows.Add();

                //DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                //row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                //row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                //row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                //row.Cells.Add(new WorksheetCell("", "HeaderStyle2")); 
                //row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                //row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                //row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                //row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                //row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                //row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                //row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                //row.Cells.Add(new WorksheetCell("Report Date :", "HeaderStyle2"));
                //row.Cells.Add(new WorksheetCell(From.ToString() + " - " + From1.ToString(), "HeaderStyle2"));
                //row = sheet.Table.Rows.Add();
                //row.Index = 6;
             
                row = sheet.Table.Rows.Add();



                row.Index = 7;
                //row.Cells.Add("Order Date");
                row.Cells.Add(new WorksheetCell("Sl.No.", "HeaderStyle6"));
                WorksheetCell cell11 = row.Cells.Add("Cash Receipt");
                cell11.MergeAcross = 1;
                cell11.StyleID = "HeaderStyle6";
                //row.Cells.Add(new WorksheetCell("Application No", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Vehicle Type", "HeaderStyle6"));
               // row.Cells.Add(new WorksheetCell("Owners Name", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Vehicle Registration Number", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("RTO Location", "HeaderStyle6"));

                //row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                WorksheetCell cell1 = row.Cells.Add("Laser Identification No");
                cell1.MergeAcross = 1; // Merge two cells together
                cell1.StyleID = "HeaderStyle6";
                //row.Cells.Add(new WorksheetCell("Rear Laser Code ", "HeaderStyle"));
                // row.Cells.Add(new WorksheetCell("RP Size", "HeaderStyle"));

                WorksheetCell cell2 = row.Cells.Add("High Security Registration Plate Size");
                cell2.MergeAcross = 1; // Merge two cells together
                cell2.StyleID = "HeaderStyle6";
                // row.Cells.Add(new WorksheetCell("Rear Plate Size", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("3rd Plate  Y/N", "HeaderStyle6"));
                // row.Cells.Add(new WorksheetCell("Colour", "HeaderStyle"));
                WorksheetCell cell3 = row.Cells.Add("Colour Background");
                cell3.MergeAcross = 1; // Merge two cells together
                cell3.StyleID = "HeaderStyle6";
                WorksheetCell cell4 = row.Cells.Add("SMS/Email sent Date");
                cell4.StyleID = "HeaderStyle6";
                //row.Cells.Add(new WorksheetCell("SMS/Email sent Date", "HeaderStyle6"));
                row = sheet.Table.Rows.Add();
                String StringField = String.Empty;
                String StringAlert = String.Empty;


                row.Index = 8;
                //row.Cells.Add("Order Date");
                row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Number", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Date", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
               

               // row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                //row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Front", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Rear", "HeaderStyle6"));
                //row.Cells.Add(new WorksheetCell("Rear Laser Code ", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Front", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Rear", "HeaderStyle6"));
                // row.Cells.Add(new WorksheetCell("Rear Plate Size", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell(" ", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Yellow", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("White", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell(" ", "HeaderStyle"));
               
                row = sheet.Table.Rows.Add();
                //String StringField = String.Empty;
                //String StringAlert = String.Empty;

                //row.Index = 9;

                UserType = Convert.ToInt32(Session["UserType"]);

              //  string upsqlstring1 = "update hsrprecords set   frontplatesize = Null where frontplatesize like '%Select%'" + " update hsrprecords set   rearplatesize = Null where rearplatesize like '%Select%'" + " update hsrprecords set   manufacturername = Null where manufacturername like '%Select%'" + " update hsrprecords set   manufacturermodel = Null where manufacturermodel like '%Select%'";
                //Utils.ExecNonQuery(upsqlstring1, CnnString);

                SQLString = "SELECT    b.ProductColor,VehicleRegNo, a.HSRPRecord_AuthorizationNo, a.OrderEmbossingDate, a.HSRPRecordID, a.OwnerName, a.VehicleType, a.HSRP_Front_LaserCode,Product_1.ProductColor,a.HSRP_Rear_LaserCode, a.VehicleClass, a.StickerMandatory, Product_1.ProductCode AS FrontPlateSize, a.RearPlateSize, b.ProductCode AS RearPlateSize1,a.FrontPlateSize AS FrontPlateSize ,a.Remarks,a.CashReceiptNo AS CashNo,convert(varchar(10),a.HSRPRecord_CreationDate,103) AS CashDate,convert(varchar(50),orderembossingdate,103) as EmbossingDate, r.rtolocationname FROM HSRPRecords a INNER JOIN Product b ON a.RearPlateSize = b.ProductID INNER JOIN  Product Product_1 ON a.FrontPlateSize = Product_1.ProductID inner join rtolocation r on a.RTOLocationID=r.RTOLocationID  WHERE  a.HSRP_StateID= '9' AND a.OrderEmbossingDate  between '" + FromDate + "' and '" + ToDate + "' order by OrderEmbossingDate";
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

                        row.Cells.Add(new WorksheetCell(dtrows["CashNo"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["CashDate"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["VehicleType"].ToString(), DataType.String, "HeaderStyle1"));
                    //    row.Cells.Add(new WorksheetCell(dtrows["OwnerName"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["VehicleRegNo"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["rtolocationname"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["HSRP_Front_LaserCode"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["HSRP_Rear_LaserCode"].ToString(), DataType.String, "HeaderStyle1"));

                        row.Cells.Add(new WorksheetCell(dtrows["FrontPlateSize"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["RearPlateSize1"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["StickerMandatory"].ToString(), DataType.String, "HeaderStyle1"));
                        if (dtrows["ProductColor"].ToString() == "WHITE")
                        {

                            row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell(dtrows["ProductColor"].ToString(), DataType.String, "HeaderStyle1"));
                            
                        }
                        else
                        {
                            row.Cells.Add(new WorksheetCell(dtrows["ProductColor"].ToString(), DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle1"));
                        }
                        row.Cells.Add(new WorksheetCell(dtrows["EmbossingDate"].ToString(), DataType.String, "HeaderStyle1"));
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