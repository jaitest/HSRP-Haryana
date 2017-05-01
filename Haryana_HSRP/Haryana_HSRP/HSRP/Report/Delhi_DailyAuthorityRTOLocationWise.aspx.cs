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
    public partial class Delhi_DailyAuthorityRTOLocationWise : System.Web.UI.Page
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
                            labelOrganization.Enabled = true;
                            DropDownListStateName.Enabled = false;
                            labelClient.Enabled = true;
                            labelDate.Visible = false;


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

        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            String StringOrderDate = OrderDate.SelectedDate.ToString("yyyy/MM/dd") + " 00:00:00";
            String StringAuthDate = HSRPAuthDate.SelectedDate.ToString("yyyy/MM/dd") + " 23:59:59";

            string filename = DropDownListStateName.SelectedItem.Text + "_DailyAuthorityRTOLocationReport_"+ System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";

            
            string SQLString = "SELECT CONVERT(varchar(20), a.HsrpRecord_AuthorizationDate, 103) AS HsrpRecord_AuthorizationDate,a.address1,a.ordertype, CONVERT(varchar(20), a.OrderDate, 103) AS OrderDate, CONVERT(varchar(20),a.OrderClosedDate,103) as OrderClosedDate,a.HSRPRecordID,a.CashReceiptNo,a.InvoiceNo,CONVERT(varchar(20), InvoiceDateTime, 103) AS InvoiceDateTime, VehicleClass,CONVERT(numeric,round(a.RoundOff_NetAmount,0)) as NetAmount, a.HSRPRecord_AuthorizationNo, a.OwnerName, a.VehicleClass, a.VehicleType, a.VehicleRegNo, a.HSRP_Front_LaserCode, a.HSRP_Rear_LaserCode, a.StickerMandatory,  (select P.productCode from Product P Where P.ProductID =a.FrontPlateSize) as FrontPlateCode, (select P.productCode from Product P Where P.ProductID =a.FrontPlateSize) as RearPlateCode,case when d.vehicleregno is null then '' else '2' end as 'Exists_or_not' " +
                   "FROM HSRPRecords a left join delhiolddata d on d.vehicleregno = a.vehicleregno " +
                   "where a.HSRP_StateID='" + DropDownListStateName.SelectedValue.ToString() + "' and a.RTOLocationID='" + dropDownListClient.SelectedValue + "' and convert(date,a.OrderClosedDate) between '" + StringOrderDate + "' and '" + StringAuthDate + "' and  a.OwnerName is not null and a.OwnerName <> '' and  a.OwnerName is not null and a.OwnerName <> '' and  a.Address1 is not null and a.Address1 <> ''  order by OrderClosedDate";

            Workbook book = new Workbook();
            StyleForTheFirstTime(book);
            DataTable dtRecord = GetDataTableFromQuery(SQLString);
            if (dtRecord.Rows.Count > 0)
            {
                ExportRecordExcel(book, dtRecord);
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
        protected void btnAllLocationPdf_Click(object sender, EventArgs e)
        {
            int iCheckAllRtoHasNoRecord = 0;
            String StringOrderDate = OrderDate.SelectedDate.ToString("yyyy/MM/dd") + " 00:00:00";
            String StringAuthDate = HSRPAuthDate.SelectedDate.ToString("yyyy/MM/dd") + " 23:59:59";

            string filename = DropDownListStateName.SelectedItem.Text + "_DailyAuthorityRTOLocationReport_" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
            Workbook book = new Workbook();
            StyleForTheFirstTime(book);
            DataTable dtrto = Utils.GetDataTable("select rtolocationname,rtolocationid from rtolocation where rtolocationid in (select distinct distrelation from rtolocation where hsrp_stateid=2) and RTOLocationID not in (148)", CnnString);
            for (int iRowNo = 0; iRowNo < dtrto.Rows.Count; iRowNo++)
            {
                string RTOName = dtrto.Rows[iRowNo]["rtolocationname"].ToString();
                string RTOCode = dtrto.Rows[iRowNo]["RTOLocationid"].ToString();
               
                string SQLString = "SELECT CONVERT(varchar(20), a.HsrpRecord_AuthorizationDate, 103) AS HsrpRecord_AuthorizationDate,a.address1,a.ordertype, CONVERT(varchar(20), a.OrderDate, 103) AS OrderDate, CONVERT(varchar(20),a.OrderClosedDate,103) as OrderClosedDate,a.HSRPRecordID,a.CashReceiptNo,a.InvoiceNo,CONVERT(varchar(20), InvoiceDateTime, 103) AS InvoiceDateTime, VehicleClass,CONVERT(numeric,round(a.RoundOff_NetAmount,0)) as NetAmount, a.HSRPRecord_AuthorizationNo, a.OwnerName, a.VehicleClass, a.VehicleType, a.VehicleRegNo, a.HSRP_Front_LaserCode, a.HSRP_Rear_LaserCode, a.StickerMandatory,  (select P.productCode from Product P Where P.ProductID =a.FrontPlateSize) as FrontPlateCode, (select P.productCode from Product P Where P.ProductID =a.FrontPlateSize) as RearPlateCode,case when d.vehicleregno is null then '' else '2' end as 'Exists_or_not' " +
                      "FROM HSRPRecords a left join delhiolddata d on d.vehicleregno = a.vehicleregno " +
                      "where a.HSRP_StateID='" + DropDownListStateName.SelectedValue.ToString() + "' and a.RTOLocationID='" + RTOCode + "' and convert(date,a.OrderClosedDate) between '" + StringOrderDate + "' and '" + StringAuthDate + "' and  a.OwnerName is not null and a.OwnerName <> '' and  a.OwnerName is not null and a.OwnerName <> '' and  a.Address1 is not null and a.Address1 <> ''  order by OrderClosedDate";

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
        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString());
        private DataTable GetDataTableFromQuery(string strSQLString)
        {
            SqlCommand cmd = new SqlCommand(strSQLString, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            cmd.CommandTimeout = 0;
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }
        #region Excel Creation
        private static void StyleForTheFirstTime(Workbook book)
        {
            book.ExcelWorkbook.ActiveSheetIndex = 1;
            book.ExcelWorkbook.WindowTopX = 100;
            book.ExcelWorkbook.WindowTopY = 200;
            book.ExcelWorkbook.WindowHeight = 7000;
            book.ExcelWorkbook.WindowWidth = 8000;

            // Some optional properties of the Document
            book.Properties.Author = "HSRP";
            book.Properties.Title = "HSRP Authority Report Order Closed";
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
                sheet.Table.Columns.Add(new WorksheetColumn(120));
                sheet.Table.Columns.Add(new WorksheetColumn(100));
                sheet.Table.Columns.Add(new WorksheetColumn(100));

                sheet.Table.Columns.Add(new WorksheetColumn(90));
                sheet.Table.Columns.Add(new WorksheetColumn(150));
                sheet.Table.Columns.Add(new WorksheetColumn(92));
                sheet.Table.Columns.Add(new WorksheetColumn(90));
                sheet.Table.Columns.Add(new WorksheetColumn(90));
                sheet.Table.Columns.Add(new WorksheetColumn(140));
                sheet.Table.Columns.Add(new WorksheetColumn(140));
                sheet.Table.Columns.Add(new WorksheetColumn(100));

                WorksheetRow row = sheet.Table.Rows.Add();

                row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                row.Cells.Add(new WorksheetCell("Report :", "HeaderStyle3"));
                WorksheetCell cell = row.Cells.Add("HSRP Authority Report Order Closed");
                cell.MergeAcross = 3; // Merge two cells together
                cell.StyleID = "HeaderStyle3";

                row = sheet.Table.Rows.Add();
                row.Index = 3;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("State:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(DropDownListStateName.SelectedItem.ToString(), "HeaderStyle2"));

                row.Cells.Add(new WorksheetCell("Location:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(strRtoName, "HeaderStyle2"));

                row = sheet.Table.Rows.Add();
                row.Index = 4;

                DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("Date Generated :", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(DateTime.Now.ToString("dd/MMM/yyyy"), "HeaderStyle2"));

                String StringOrderDate = OrderDate.SelectedDate.ToString("yyyy/MM/dd");
                String StringAuthDate = HSRPAuthDate.SelectedDate.ToString("yyyy/MM/dd");

                row.Cells.Add(new WorksheetCell("Report Date:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(StringOrderDate + " To " + StringAuthDate, "HeaderStyle2"));
                row = sheet.Table.Rows.Add();


                row.Index = 5;
                //row.Cells.Add("Order Date");
                row.Cells.Add(new WorksheetCell("S.No.", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Vechicle No", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Front Laser Number", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Rear Laser Number", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Owner's Name", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Owner's Address", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Vechicle Type", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("New Vehicle/Old Vehicle", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Authorisation Date", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Order Date", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Affixation Date", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Old Registration Plate Count", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Plate Removed For Distraction", "HeaderStyle6"));


                row = sheet.Table.Rows.Add();
                String StringField = String.Empty;
                String StringAlert = String.Empty;
                row.Index = 6;





                string RTOColName = string.Empty;
                Int64 totalAmount = 0;
                if (dt.Rows.Count > 0)
                {
                    int sno = 0;
                    string VehicleColor = string.Empty;
                    string Color = string.Empty;


                    foreach (DataRow dtrows in dt.Rows) // Loop over the rows.
                    {
                        if (dtrows["VehicleClass"].ToString() == "Non-Transport")
                        {
                            VehicleColor = "WHITE";
                        }
                        else
                        {
                            VehicleColor = "YELLOW";
                        }

                        sno = sno + 1;
                        row.Cells.Add(new WorksheetCell(Convert.ToInt16(sno).ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["VEHICLeregno"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["HSRP_Front_LaserCode"].ToString(), DataType.String, "HeaderStyle5"));
                        row.Cells.Add(new WorksheetCell(dtrows["HSRP_Rear_LaserCode"].ToString(), DataType.String, "HeaderStyle5"));
                        row.Cells.Add(new WorksheetCell(dtrows["OwnerName"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["address1"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["VEHICLETYPE"].ToString(), DataType.String, "HeaderStyle"));
                        string strVeh = dtrows["ordertype"].ToString().Equals("NB") ? "New Vehicle" : "Old Vehicle";
                        row.Cells.Add(new WorksheetCell(strVeh, DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["HsrpRecord_AuthorizationDate"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["OrderDate"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["OrderClosedDate"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["Exists_or_not"].ToString(), DataType.String, "HeaderStyle"));



                        row = sheet.Table.Rows.Add();
                    }
                }
            }
            catch
            {
            }
        }
        #endregion
    }
}
