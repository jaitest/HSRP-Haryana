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
    public partial class FlatFileReport : System.Web.UI.Page
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
                    
                    try
                    {
                    
                        if (UserType.Equals(0))
                        {
                            labelOrganization.Visible = true;
                            DropDownListStateName.Visible = true;
                           
                            //dropDownListClient.Visible = true;
                            FilldropDownListOrganization();
                            //FilldropDownListClient();
                            FilldropDownDistrict();

                        }
                        else
                        {

                            FilldropDownListOrganization();
                          

                            labelOrganization.Enabled = false;
                            DropDownListStateName.Enabled = false;
                         

                            // FilldropDownListClient();
                            FilldropDownDistrict();
                            FilldropDownListOrganization();
                        }                        
                    }
                    catch (Exception err)
                    {

                    }
                }
            }
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
        //        SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID='3' and ActiveStatus!='N'  Order by RTOLocationName";

        //        Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), CnnString, "--Select Location--");
        //        // dropDownListClient.SelectedIndex = dropDownListClient.Items.Count - 1;
        //    }
        //    else
        //    {
        //         SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where (RTOLocationID=" + RTOLocationID + " or distRelation=" + RTOLocationID + " ) and ActiveStatus!='N'   Order by RTOLocationName";
        //       // SQLString = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID where UserRTOLocationMapping.UserID='" + strUserID + "' ";

        //        DataSet dss = Utils.getDataSet(SQLString, CnnString);
        //        dropDownListClient.DataSource = dss;
        //        dropDownListClient.DataBind();
        //    }
        //}


        private void FilldropDownDistrict()
        {
            SQLString = "select rtolocationid,rtolocationname from rtolocation where hsrp_stateid=3 and rtolocationid in(26,27,28,29,30,31,32,33,34,35,36,37,38)";
           // Utils.PopulateDropDownList(dropDownListDistrict, SQLString.ToString(), CnnString, "--Select District--");
        }

        #endregion

        protected void DropDownListStateName_SelectedIndexChanged(object sender, EventArgs e)
        {
            // FilldropDownListClient();
            FilldropDownDistrict();
        }

        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            try
            {
                LabelError.Text = "";

               
              
               
                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                // int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);
                DataTable StateName;
                DataTable dts;
                DataTable dt = new DataTable();

                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                // int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);

                string filename = "HSRPFlatFileReport-" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                Workbook book = new Workbook();

                // Specify which Sheet should be opened and the size of window by default
                book.ExcelWorkbook.ActiveSheetIndex = 1;
                book.ExcelWorkbook.WindowTopX = 100;
                book.ExcelWorkbook.WindowTopY = 200;
                book.ExcelWorkbook.WindowHeight = 7000;
                book.ExcelWorkbook.WindowWidth = 8000;

                // Some optional properties of the Document
                book.Properties.Author = "HSRP";
                book.Properties.Title = "HSRP Flat File Report";
                book.Properties.Created = DateTime.Now;


                // Add some styles to the Workbook
                WorksheetStyle style = book.Styles.Add("HeaderStyle");
                style.Font.FontName = "Tahoma";
                style.Font.Size = 10;
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
                style8.Alignment.Horizontal = StyleHorizontalAlignment.Center;
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

                Worksheet sheet = book.Worksheets.Add("HSRP Flat File Report");
                sheet.Table.Columns.Add(new WorksheetColumn(60));
                sheet.Table.Columns.Add(new WorksheetColumn(195));
                sheet.Table.Columns.Add(new WorksheetColumn(90));
                sheet.Table.Columns.Add(new WorksheetColumn(90));
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
                row.Cells.Add(new WorksheetCell("Report :", "HeaderStyle3"));
                WorksheetCell cell = row.Cells.Add("HSRP Flat File Report");
                cell.MergeAcross = 3; // Merge two cells together
                cell.StyleID = "HeaderStyle3";

                row = sheet.Table.Rows.Add();
                row.Index = 3;

                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("State:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(DropDownListStateName.SelectedItem.ToString(), "HeaderStyle2"));

                row = sheet.Table.Rows.Add();
                // Skip one row, and add some text
                row.Index = 4;

                DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());

                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("Date Generated :", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(dates.ToString("dd/M/yyyy"), "HeaderStyle2"));
                row = sheet.Table.Rows.Add();
                row.Index = 5;

                if (DropDownList1.SelectedItem.ToString().ToUpper() == "DATA MATCHED BY LOCATION")
                {
                    WorksheetCell cell1 = row.Cells.Add("Vehicle And RTO Location Wise");
                    cell1.MergeAcross = 6; // Merge two cells together
                    cell1.StyleID = "HeaderStyle8";
                }
                else if (DropDownList1.SelectedItem.ToString().ToUpper() == "BOTH")
                {
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    WorksheetCell cell1 = row.Cells.Add("Vehicle And RTO Location Wise");
                    cell1.MergeAcross = 4; // Merge two cells together
                    cell1.StyleID = "HeaderStyle8";             
              
                    WorksheetCell cell2 = row.Cells.Add("Vehicle Wise Only");
                    cell2.MergeAcross = 4; // Merge two cells together
                    cell2.StyleID = "HeaderStyle8";
                }
                else
                {
                    WorksheetCell cell2 = row.Cells.Add("Vehicle Wise Only");
                    cell2.MergeAcross = 6; // Merge two cells together
                    cell2.StyleID = "HeaderStyle8";
                }
                row = sheet.Table.Rows.Add();


                row.Index = 7;
                //row.Cells.Add("Order Date");

                row.Cells.Add(new WorksheetCell("S.No.", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Rto Name", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("2W", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("3W", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("4W", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Tractor", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Comm", "HeaderStyle6"));

                if (DropDownList1.SelectedItem.ToString().ToUpper() == "BOTH")
                {
                    row.Cells.Add(new WorksheetCell("2W", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("3W", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("4W", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("Tractor", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("Comm", "HeaderStyle6"));
                }

                row = sheet.Table.Rows.Add();
                String StringField = String.Empty;

                String StringAlert = String.Empty;
                row.Index = 8;
                UserType = Convert.ToInt32(Session["UserType"]);
                if (DropDownList1.SelectedItem.ToString().ToUpper() == "DATA MATCHED BY LOCATION")
                {
                    
                    SQLString = "select (select top 1 rtolocationname from rtolocation where distrelation=r.distrelation order by rtolocationid) as RTOLocationName, " +
                                " Sum(case when (hsr.vehicletype = 'SCOOTER' or hsr.vehicletype='MOTOR CYCLE')  then 1 else 0 end) as '2w', " +
                                " Sum(case when (hsr.vehicletype = 'THREE WHEELER') then 1 else 0 end) as '3w', " +
                                " Sum(case when (hsr.vehicletype = 'LMV' or hsr.vehicletype='LMV(CLASS)') then 1 else 0 end) as '4w', " +
                                " Sum(case when (hsr.vehicletype = 'TRACTOR') then 1 else 0 end) as 'Tractor', " +
                                " Sum(case when (hsr.vehicletype = 'MCV/HCV/TRAILERS') then 1 else 0 end) as 'Comm' " +
                                " from hsrprecords h inner join HSRPRecordsStaggingArea hsr on hsr.VehicleRegNo=h.vehicleregno and hsr.rtolocationid = h.rtolocationid " +
                                " inner join RTOLocation r on hsr.RTOLocationID=r.RTOLocationID where hsr.hsrp_stateid='" + DropDownListStateName.SelectedValue + "' and hsr.vehicletype is not null" +
                                " and hsr.ordertype is not null group by r.distrelation order by RTOLocationName";
                }
                else if (DropDownList1.SelectedItem.ToString().ToUpper() == "DATA MATCHED WITHOUT LOCATION")
                {
                    
                    SQLString = "select (select top 1 rtolocationname from rtolocation where distrelation=r.distrelation order by rtolocationid) as RTOLocationName, " +
                                " Sum(case when (hsr.vehicletype = 'SCOOTER' or hsr.vehicletype='MOTOR CYCLE')  then 1 else 0 end) as '2w', " +
                                " Sum(case when (hsr.vehicletype = 'THREE WHEELER') then 1 else 0 end) as '3w', " +
                                " Sum(case when (hsr.vehicletype = 'LMV' or hsr.vehicletype='LMV(CLASS)') then 1 else 0 end) as '4w', " +
                                " Sum(case when (hsr.vehicletype = 'TRACTOR') then 1 else 0 end) as 'Tractor', " +
                                " Sum(case when (hsr.vehicletype = 'MCV/HCV/TRAILERS') then 1 else 0 end) as 'Comm' " +
                                " from hsrprecords h inner join HSRPRecordsStaggingArea hsr on hsr.VehicleRegNo=h.vehicleregno " +
                                " inner join RTOLocation r on hsr.RTOLocationID=r.RTOLocationID where hsr.hsrp_stateid='" + DropDownListStateName.SelectedValue + "' and hsr.vehicletype is not null" +
                                " and hsr.ordertype is not null group by r.distrelation order by RTOLocationName";
                }
                else
                {
                    SQLString = ";with dd as (select (select top 1 rtolocationname from rtolocation where distrelation=r.distrelation order by rtolocationid) as RTOLocationName, " +
                                " Sum(case when (hsr.vehicletype = 'SCOOTER' or hsr.vehicletype='MOTOR CYCLE')  then 1 else 0 end) as '2w', " +
                                " Sum(case when (hsr.vehicletype = 'THREE WHEELER') then 1 else 0 end) as '3w', " +
                                " Sum(case when (hsr.vehicletype = 'LMV' or hsr.vehicletype='LMV(CLASS)') then 1 else 0 end) as '4w', " +
                                " Sum(case when (hsr.vehicletype = 'TRACTOR') then 1 else 0 end) as 'Tractor', " +
                                " Sum(case when (hsr.vehicletype = 'MCV/HCV/TRAILERS') then 1 else 0 end) as 'Comm' " +
                                " from hsrprecords h inner join HSRPRecordsStaggingArea hsr on hsr.VehicleRegNo=h.vehicleregno and hsr.rtolocationid = h.rtolocationid" +
                                " inner join RTOLocation r on hsr.RTOLocationID=r.RTOLocationID where hsr.hsrp_stateid='" + DropDownListStateName.SelectedValue + "' and hsr.vehicletype is not null" +
                                " and hsr.ordertype is not null group by r.distrelation)," +

                                "dd1 as (select (select top 1 rtolocationname from rtolocation where distrelation=r.distrelation order by rtolocationid) as RTOLocationName, " +
                                " Sum(case when (hsr.vehicletype = 'SCOOTER' or hsr.vehicletype='MOTOR CYCLE')  then 1 else 0 end) as '2w', " +
                                " Sum(case when (hsr.vehicletype = 'THREE WHEELER') then 1 else 0 end) as '3w', " +
                                " Sum(case when (hsr.vehicletype = 'LMV' or hsr.vehicletype='LMV(CLASS)') then 1 else 0 end) as '4w', " +
                                " Sum(case when (hsr.vehicletype = 'TRACTOR') then 1 else 0 end) as 'Tractor', " +
                                " Sum(case when (hsr.vehicletype = 'MCV/HCV/TRAILERS') then 1 else 0 end) as 'Comm' " +
                                " from hsrprecords h inner join HSRPRecordsStaggingArea hsr on hsr.VehicleRegNo=h.vehicleregno " +
                                " inner join RTOLocation r on hsr.RTOLocationID=r.RTOLocationID where hsr.hsrp_stateid='" + DropDownListStateName.SelectedValue + "' and hsr.vehicletype is not null" +
                                " and hsr.ordertype is not null group by r.distrelation)" +
                                "select dd.rtolocationname,dd.[2w],dd.[3w],dd.[4w],dd.Tractor,dd1.Comm,dd1.[2w],dd1.[3w],dd1.[4w],dd1.Tractor,dd1.Comm from dd inner join dd1 on" +
                                " dd.rtolocationname=dd1.rtolocationname order by dd.rtolocationname";
                }
              


                dt = Utils.GetDataTable(SQLString, CnnString);
                int sno = 0;
                
                if (dt.Rows.Count > 0)
                {
                    
                        foreach (DataRow dtrows in dt.Rows) // Loop over the rows.
                        {
                            sno = sno + 1;
                            row.Cells.Add(new WorksheetCell(Convert.ToInt16(sno).ToString(), DataType.String, "HeaderStyle"));
                            row.Cells.Add(new WorksheetCell(dtrows["RTOLocationName"].ToString(), DataType.String, "HeaderStyle"));
                            row.Cells.Add(new WorksheetCell(dtrows["2W"].ToString(), DataType.String, "HeaderStyle"));
                            row.Cells.Add(new WorksheetCell(dtrows["3W"].ToString(), DataType.String, "HeaderStyle"));
                            row.Cells.Add(new WorksheetCell(dtrows["4W"].ToString(), DataType.String, "HeaderStyle"));
                            row.Cells.Add(new WorksheetCell(dtrows["Tractor"].ToString(), DataType.String, "HeaderStyle"));
                            row.Cells.Add(new WorksheetCell(dtrows["Comm"].ToString(), DataType.String, "HeaderStyle"));
                            if (DropDownList1.SelectedItem.ToString().ToUpper() == "BOTH")
                            {
                                row.Cells.Add(new WorksheetCell(dtrows["2w1"].ToString(), DataType.String, "HeaderStyle"));
                                row.Cells.Add(new WorksheetCell(dtrows["3w1"].ToString(), DataType.String, "HeaderStyle"));
                                row.Cells.Add(new WorksheetCell(dtrows["4w1"].ToString(), DataType.String, "HeaderStyle"));
                                row.Cells.Add(new WorksheetCell(dtrows["Tractor1"].ToString(), DataType.String, "HeaderStyle"));
                                row.Cells.Add(new WorksheetCell(dtrows["Comm1"].ToString(), DataType.String, "HeaderStyle"));
                            }
                            row = sheet.Table.Rows.Add();
                        }
                    
                    row = sheet.Table.Rows.Add();

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
                LabelError.Text = "Error in Populating Grid :" + ex.Message.ToString();
            }
        }

        protected void btngo_Click(object sender, EventArgs e)
        {
            string DataWise = string.Empty;
            SqlConnection con = new SqlConnection(CnnString);
            if (DropDownList1.SelectedItem.ToString().ToUpper() == "DATA MATCHED BY LOCATION")
            {
                DataWise = "Vehicle And RTO Location Wise";
                SQLString = "select (select top 1 rtolocationname from rtolocation where distrelation=r.distrelation order by rtolocationid) as RTOLocationName, " +
                            " Sum(case when (hsr.vehicletype = 'SCOOTER' or hsr.vehicletype='MOTOR CYCLE')  then 1 else 0 end) as '2w', " +
                            " Sum(case when (hsr.vehicletype = 'THREE WHEELER') then 1 else 0 end) as '3w', " +
                            " Sum(case when (hsr.vehicletype = 'LMV' or hsr.vehicletype='LMV(CLASS)') then 1 else 0 end) as '4w', " +
                            " Sum(case when (hsr.vehicletype = 'TRACTOR') then 1 else 0 end) as 'Tractor', " +
                            " Sum(case when (hsr.vehicletype = 'MCV/HCV/TRAILERS') then 1 else 0 end) as 'Comm' " +
                            " from hsrprecords h inner join HSRPRecordsStaggingArea hsr on hsr.VehicleRegNo=h.vehicleregno and hsr.rtolocationid = h.rtolocationid " +
                            " inner join RTOLocation r on hsr.RTOLocationID=r.RTOLocationID where hsr.hsrp_stateid='" + DropDownListStateName.SelectedValue + "' and hsr.vehicletype is not null" +
                            " and hsr.ordertype is not null group by r.distrelation order by RTOLocationName";
            }
            else if (DropDownList1.SelectedItem.ToString().ToUpper() == "DATA MATCHED WITHOUT LOCATION")
            {
                DataWise = "Vehicle Wise Only";
                SQLString = "select (select top 1 rtolocationname from rtolocation where distrelation=r.distrelation order by rtolocationid) as RTOLocationName, " +
                            " Sum(case when (hsr.vehicletype = 'SCOOTER' or hsr.vehicletype='MOTOR CYCLE')  then 1 else 0 end) as '2w', " +
                            " Sum(case when (hsr.vehicletype = 'THREE WHEELER') then 1 else 0 end) as '3w', " +
                            " Sum(case when (hsr.vehicletype = 'LMV' or hsr.vehicletype='LMV(CLASS)') then 1 else 0 end) as '4w', " +
                            " Sum(case when (hsr.vehicletype = 'TRACTOR') then 1 else 0 end) as 'Tractor', " +
                            " Sum(case when (hsr.vehicletype = 'MCV/HCV/TRAILERS') then 1 else 0 end) as 'Comm' " +
                            " from hsrprecords h inner join HSRPRecordsStaggingArea hsr on hsr.VehicleRegNo=h.vehicleregno " +
                            " inner join RTOLocation r on hsr.RTOLocationID=r.RTOLocationID where hsr.hsrp_stateid='" + DropDownListStateName.SelectedValue + "' and hsr.vehicletype is not null" +
                            " and hsr.ordertype is not null group by r.distrelation order by RTOLocationName";
            }
            else
            {
                SQLString = ";with dd as (select (select top 1 rtolocationname from rtolocation where distrelation=r.distrelation order by rtolocationid) as RTOLocationName, " +
                            " Sum(case when (hsr.vehicletype = 'SCOOTER' or hsr.vehicletype='MOTOR CYCLE')  then 1 else 0 end) as '2w', " +
                            " Sum(case when (hsr.vehicletype = 'THREE WHEELER') then 1 else 0 end) as '3w', " +
                            " Sum(case when (hsr.vehicletype = 'LMV' or hsr.vehicletype='LMV(CLASS)') then 1 else 0 end) as '4w', " +
                            " Sum(case when (hsr.vehicletype = 'TRACTOR') then 1 else 0 end) as 'Tractor', " +
                            " Sum(case when (hsr.vehicletype = 'MCV/HCV/TRAILERS') then 1 else 0 end) as 'Comm' " +
                            " from hsrprecords h inner join HSRPRecordsStaggingArea hsr on hsr.VehicleRegNo=h.vehicleregno and hsr.rtolocationid = h.rtolocationid" +
                            " inner join RTOLocation r on hsr.RTOLocationID=r.RTOLocationID where hsr.hsrp_stateid='" + DropDownListStateName.SelectedValue + "' and hsr.vehicletype is not null" +
                            " and hsr.ordertype is not null group by r.distrelation)," +

                            "dd1 as (select (select top 1 rtolocationname from rtolocation where distrelation=r.distrelation order by rtolocationid) as RTOLocationName, " +
                            " Sum(case when (hsr.vehicletype = 'SCOOTER' or hsr.vehicletype='MOTOR CYCLE')  then 1 else 0 end) as '2w', " +
                            " Sum(case when (hsr.vehicletype = 'THREE WHEELER') then 1 else 0 end) as '3w', " +
                            " Sum(case when (hsr.vehicletype = 'LMV' or hsr.vehicletype='LMV(CLASS)') then 1 else 0 end) as '4w', " +
                            " Sum(case when (hsr.vehicletype = 'TRACTOR') then 1 else 0 end) as 'Tractor', " +
                            " Sum(case when (hsr.vehicletype = 'MCV/HCV/TRAILERS') then 1 else 0 end) as 'Comm' " +
                            " from hsrprecords h inner join HSRPRecordsStaggingArea hsr on hsr.VehicleRegNo=h.vehicleregno " +
                            " inner join RTOLocation r on hsr.RTOLocationID=r.RTOLocationID where hsr.hsrp_stateid='" + DropDownListStateName.SelectedValue + "' and hsr.vehicletype is not null" +
                            " and hsr.ordertype is not null group by r.distrelation)" +
                            "select dd.rtolocationname,dd.[2w],dd.[3w],dd.[4w],dd.Tractor,dd1.Comm,dd1.[2w],dd1.[3w],dd1.[4w],dd1.Tractor,dd1.Comm from dd inner join dd1 on" +
                            " dd.rtolocationname=dd1.rtolocationname order by dd.rtolocationname";
            }
            con.Open();
            SqlCommand cmd = new SqlCommand(SQLString, con);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable ds = new DataTable();
            da.Fill(ds);
            Session["totalrow"] = ds.Rows.Count;
            DataRow dr = ds.NewRow();
         
            ds.Rows.InsertAt(dr,0);
            GridView1.DataSource = ds;
            GridView1.DataBind();
            con.Close();
            if (DropDownList1.SelectedItem.ToString().ToUpper() == "BOTH")
            {
                
                GridView1.Rows[0].Cells.Clear();
                GridView1.Rows[0].Cells.Add(new TableCell());
                GridView1.Rows[0].Cells.Add(new TableCell());

                GridView1.Rows[0].Cells[1].ColumnSpan = 5;
                GridView1.Rows[0].Cells[1].Text = "Vehicle And RTO Location Wise";
                GridView1.Rows[0].Cells[1].ControlStyle.BackColor = Color.DeepSkyBlue;
                GridView1.Rows[0].Cells[1].HorizontalAlign = HorizontalAlign.Center;

                GridView1.Rows[0].Cells.Add(new TableCell());
                GridView1.Rows[0].Cells[2].ColumnSpan = 5;
                GridView1.Rows[0].Cells[2].Text = "Vehicle Wise Only";
                GridView1.Rows[0].Cells[2].ControlStyle.BackColor = Color.DeepSkyBlue;
                GridView1.Rows[0].Cells[2].HorizontalAlign = HorizontalAlign.Center;
                GridView1.HeaderRow.Cells[6].Text = "2w";
                GridView1.HeaderRow.Cells[7].Text = "3w";
                GridView1.HeaderRow.Cells[8].Text = "4w";
                GridView1.HeaderRow.Cells[9].Text = "Tractor";
                GridView1.HeaderRow.Cells[10].Text = "Comm";
            }
            else
            {
                int columncount = GridView1.Rows[0].Cells.Count;
                GridView1.Rows[0].Cells.Clear();
                GridView1.Rows[0].Cells.Add(new TableCell());
                GridView1.Rows[0].Cells.Add(new TableCell());

                GridView1.Rows[0].Cells[1].ColumnSpan = columncount-1;
                GridView1.Rows[0].Cells[1].Text = DataWise;
                GridView1.Rows[0].Cells[1].ControlStyle.BackColor = Color.DeepSkyBlue;
                GridView1.Rows[0].Cells[1].HorizontalAlign = HorizontalAlign.Center;
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {                
                if (e.Row.RowIndex > 0)
                {
                    GridViewRow previousRow = GridView1.Rows[e.Row.RowIndex - 1];
                    if (e.Row.Cells[0].Text == previousRow.Cells[0].Text)
                    {
                        if (previousRow.Cells[0].RowSpan == 0)
                        {
                            previousRow.Cells[0].RowSpan += int.Parse(Session["totalrow"].ToString());
                            e.Row.Cells[0].Visible = false;
                        }
                    }
                }
            }
           
               
        }
    }
}
