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
    public partial class HPMonthlyMISReport : System.Web.UI.Page
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
                            labelClient.Visible = true;

                            //dropDownListClient.Visible = true;
                            FilldropDownListOrganization();
                            //FilldropDownListClient();
                            FilldropDownDistrict();

                        }
                        else
                        {

                            FilldropDownListOrganization();
                            labelClient.Visible = true;
                            //dropDownListClient.Visible = true;

                            labelOrganization.Enabled = false;
                            DropDownListStateName.Enabled = false;
                            labelClient.Enabled = true;
                            // labelDate.Visible = false;

                            // FilldropDownListClient();
                            FilldropDownDistrict();
                            FilldropDownListOrganization();
                        }

                        for (int i = 2011; i <= System.DateTime.Now.Year; i++)
                        {
                            if (i == 2011)
                            {
                                ddlYear.Items.Add("--Select Year--");
                            }
                            else
                            {
                                ddlYear.Items.Add(i.ToString());
                            }
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
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where ActiveStatus='Y' and HSRP_StateID='3' Order by HSRPStateName";
                Utils.PopulateDropDownList(DropDownListStateName, SQLString.ToString(), CnnString, "--Select State--");
                // DropDownListStateName.SelectedIndex = DropDownListStateName.Items.Count - 1;
            }
            else
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID='3' and ActiveStatus='Y' Order by HSRPStateName";
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
            Utils.PopulateDropDownList(dropDownListDistrict, SQLString.ToString(), CnnString, "--Select District--");
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

                string filename = "HSRPDailyOperationsSummary-" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                Workbook book = new Workbook();

                // Specify which Sheet should be opened and the size of window by default
                book.ExcelWorkbook.ActiveSheetIndex = 1;
                book.ExcelWorkbook.WindowTopX = 100;
                book.ExcelWorkbook.WindowTopY = 200;
                book.ExcelWorkbook.WindowHeight = 7000;
                book.ExcelWorkbook.WindowWidth = 8000;

                // Some optional properties of the Document
                book.Properties.Author = "HSRP";
                book.Properties.Title = "HSRP Daily Operations Summary";
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

                Worksheet sheet = book.Worksheets.Add("HSRP Daily Operations Summary");
                sheet.Table.Columns.Add(new WorksheetColumn(60));
                sheet.Table.Columns.Add(new WorksheetColumn(195));
                sheet.Table.Columns.Add(new WorksheetColumn(100));
                sheet.Table.Columns.Add(new WorksheetColumn(90));

                sheet.Table.Columns.Add(new WorksheetColumn(90));
                sheet.Table.Columns.Add(new WorksheetColumn(140));
                sheet.Table.Columns.Add(new WorksheetColumn(162));
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
                WorksheetCell cell = row.Cells.Add("HSRP Daily Operations Summary");
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

                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
              //  row.Cells.Add(new WorksheetCell("Report Date:", "HeaderStyle2"));
               // row.Cells.Add(new WorksheetCell(ReportDate, "HeaderStyle2"));
                row = sheet.Table.Rows.Add();


                row.Index = 7;
                //row.Cells.Add("Order Date");
                row.Cells.Add(new WorksheetCell("S.No.", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Distt", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Location", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("RTO CODE", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("April", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("May", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("June", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("July", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("August", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Sept", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Oct", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Nov", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("December", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Total Emb", "HeaderStyle6"));

                row = sheet.Table.Rows.Add();
                String StringField = String.Empty;

                String StringAlert = String.Empty;
                row.Index = 9;
                UserType = Convert.ToInt32(Session["UserType"]);

                SQLString = "select  (select rtolocationname from rtolocation where hsrp_stateid=3 and Rtolocationid='" +
                    dropDownListDistrict.SelectedValue + "') as Disst, dd.rtolocationname as RTOLocationName,dd.RtoLocationCode, count(dd.Apr_closed)  as [April],count(dd.Apr_emb) as [Total Emb],count(dd.May_closed) as [May]" +
                    ",count(dd.June_closed) as [June],count(dd.july_closed) as [July]" +
                                     ",count(dd.August_closed) as [August],count(dd.Sept_closed) as [Sept],count(dd.Oct_closed) as [Oct],count(dd.Nov_closed) as [Nov]" +
                                     ",count(dd.Dec_closed) as [Dec] from (select rr.RtoLocationName as rtolocationname,rr.rtolocationcode as RtoLocationCode," +
                                     "(case When ordercloseddate between '"+ddlYear.SelectedItem+"-04-01 00:00:00' and '"+ddlYear.SelectedItem+"-04-30 23:59:59' and OrderStatus='closed' then 'Apr_closed' end ) 'Apr_closed'," +
                                     "(case when orderembossingdate between '"+ddlYear.SelectedItem+"-04-01 00:00:00' and '"+ddlYear.SelectedItem+"-12-31 23:59:59' and OrderStatus='Embossing Done' then 'Apr_emb'end ) 'Apr_emb'," +
                                     "(case when ordercloseddate between '"+ddlYear.SelectedItem+"-05-01 00:00:00' and '"+ddlYear.SelectedItem+"-05-31 23:59:59' and OrderStatus='closed' then 'May_closed' end) 'May_closed'," +
                                     "(case when ordercloseddate between '"+ddlYear.SelectedItem+"-06-01 00:00:00' and '"+ddlYear.SelectedItem+"-06-30 23:59:59' and OrderStatus='closed' then 'June_closed' end)'June_closed'," +
                                     "(case when ordercloseddate between '"+ddlYear.SelectedItem+"-07-01 00:00:00' and '"+ddlYear.SelectedItem+"-07-31 23:59:59' and OrderStatus='closed' then 'july_closed' end) 'july_closed'," +
                                     "(case when ordercloseddate between '"+ddlYear.SelectedItem+"-08-01 00:00:00' and '"+ddlYear.SelectedItem+"-08-31 23:59:59' and OrderStatus='closed' then 'August_closed' end) 'August_closed'," +
                                     "(case when ordercloseddate between '"+ddlYear.SelectedItem+"-09-01 00:00:00' and '"+ddlYear.SelectedItem+"-09-30 23:59:59' and OrderStatus='closed' then 'Sept_closed' end) 'Sept_closed'," +
                                     "(case when ordercloseddate between '"+ddlYear.SelectedItem+"-10-01 00:00:00' and '"+ddlYear.SelectedItem+"-10-31 23:59:59' and OrderStatus='closed' then 'Oct_closed' end) 'Oct_closed'," +
                                     "(case when ordercloseddate between '"+ddlYear.SelectedItem+"-11-01 00:00:00' and '"+ddlYear.SelectedItem+"-11-30 23:59:59' and OrderStatus='closed' then 'Nov_closed' end) 'Nov_closed'," +
                                     "(case when ordercloseddate between '"+ddlYear.SelectedItem+"-12-01 00:00:00' and '"+ddlYear.SelectedItem+"-12-31 23:59:59' and OrderStatus='closed' then 'Dec_closed' end) 'Dec_closed'" +

                                     "from hsrpRecords  inner join rtolocation rr on hsrpRecords.RTOLocationID=rr.rtolocationid and rr.distrelation='" + dropDownListDistrict.SelectedValue + "' where hsrpRecords.HSRP_StateID=3 and RTOLocationCode is not null)dd group by dd.RtoLocationname, dd.RtoLocationCode";

                int sno = 0;
                int Total = 0;
                int TotalAprilRecord = 0;
                int TotalMayRecord = 0;
                int TotalJuneRecord = 0;
                int TotalJulyRecord = 0;
                int TotalAugRecord = 0;
                int TotalSeptRecord = 0;
                int TotalOctRecord = 0;
                int TotalNovRecord = 0;
                int TotalDecRecord = 0;
                int TotalEmb = 0;


                dt = Utils.GetDataTable(SQLString, CnnString);
                string RTOColName = string.Empty;
                if (dt.Rows.Count > 0)
                {
                    // sno = sno + 1;
                    foreach (DataRow dtrows in dt.Rows) // Loop over the rows.
                    {
                        sno = sno + 1;
                        row.Cells.Add(new WorksheetCell(Convert.ToInt16(sno).ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["Disst"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["RTOLocationName"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["RtoLocationCode"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["April"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["May"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["June"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["July"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["August"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["Sept"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["Oct"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["Nov"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["Dec"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["Total Emb"].ToString(), DataType.String, "HeaderStyle"));
                        int intApril = int.Parse(dtrows["April"].ToString());
                        int intMay = int.Parse(dtrows["May"].ToString());
                        int intJune = int.Parse(dtrows["June"].ToString());
                        int intJuly = int.Parse(dtrows["July"].ToString());
                        int intAug = int.Parse(dtrows["August"].ToString());
                        int intSept = int.Parse(dtrows["Sept"].ToString());
                        int intOct = int.Parse(dtrows["Oct"].ToString());
                        int intNov = int.Parse(dtrows["Nov"].ToString());
                        int intDec = int.Parse(dtrows["Dec"].ToString());
                        int intEmb = int.Parse(dtrows["Total Emb"].ToString());
                        TotalAprilRecord = TotalAprilRecord + intApril;
                        TotalMayRecord = TotalMayRecord + intMay;
                        TotalJuneRecord = TotalJuneRecord + intJune;
                        TotalJulyRecord = TotalJulyRecord + intJuly;
                        TotalAugRecord = TotalAugRecord + intAug;
                        TotalSeptRecord = TotalSeptRecord + intSept;
                        TotalOctRecord = TotalOctRecord + intOct;
                        TotalNovRecord = TotalNovRecord + intNov;
                        TotalDecRecord = TotalDecRecord + intDec;
                        TotalEmb = TotalEmb + intEmb;



                        //Total = Total + intApril + intMay + intJune + intJuly + intAug + intSept + intOct + intNov + intDec;


                        row = sheet.Table.Rows.Add();
                    }

                    row = sheet.Table.Rows.Add();
                    row = sheet.Table.Rows.Add();
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle9"));
                    row.Cells.Add(new WorksheetCell("Total Vehicles:", "HeaderStyle9"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle9"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle9"));
                    row.Cells.Add(new WorksheetCell((TotalAprilRecord).ToString(), "HeaderStyle9"));
                    row.Cells.Add(new WorksheetCell((TotalMayRecord).ToString(), "HeaderStyle9"));
                    row.Cells.Add(new WorksheetCell((TotalJuneRecord).ToString(), "HeaderStyle9"));
                    row.Cells.Add(new WorksheetCell((TotalJulyRecord).ToString(), "HeaderStyle9"));
                    row.Cells.Add(new WorksheetCell((TotalAugRecord).ToString(), "HeaderStyle9"));
                    row.Cells.Add(new WorksheetCell((TotalSeptRecord).ToString(), "HeaderStyle9"));
                    row.Cells.Add(new WorksheetCell((TotalOctRecord).ToString(), "HeaderStyle9"));
                    row.Cells.Add(new WorksheetCell((TotalNovRecord).ToString(), "HeaderStyle9"));
                    row.Cells.Add(new WorksheetCell((TotalDecRecord).ToString(), "HeaderStyle9"));
                    row.Cells.Add(new WorksheetCell((TotalEmb).ToString(), "HeaderStyle9"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle9"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle9"));
                    //row.Cells.Add(new WorksheetCell((totProduction).ToString(), "HeaderStyle9"));
                    //row.Cells.Add(new WorksheetCell((Total).ToString(), "HeaderStyle9"));

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
            SqlConnection con = new SqlConnection(CnnString);
            con.Open();
            SqlCommand cmd = new SqlCommand("select  (select rtolocationname from rtolocation where hsrp_stateid=3 and Rtolocationid='" +
                    dropDownListDistrict.SelectedValue + "') as Disst, dd.rtolocationname as RTOLocationName,dd.RtoLocationCode, count(dd.Apr_closed)  as [APRIL],count(dd.May_closed) as [May]" +
                    ",count(dd.June_closed) as [June],count(dd.july_closed) as [July]" +
                                     ",count(dd.August_closed) as [August],count(dd.Sept_closed) as [Sept],count(dd.Oct_closed) as [Oct],count(dd.Nov_closed) as [Nov]" +
                                     ",count(dd.Dec_closed) as [Dec],count(dd.Apr_emb) as [Total Emb] from (select rr.RtoLocationName as rtolocationname,rr.rtolocationcode as RtoLocationCode," +
                                     "(case When ordercloseddate between '"+ddlYear.SelectedItem+"-04-01 00:00:00' and '"+ddlYear.SelectedItem+"-04-30 23:59:59' and OrderStatus='closed' then 'Apr_closed' end ) 'Apr_closed'," +
                                     "(case when orderembossingdate between '"+ddlYear.SelectedItem+"-04-01 00:00:00' and '"+ddlYear.SelectedItem+"-12-31 23:59:59' and OrderStatus='Embossing Done' then 'Apr_emb'end ) 'Apr_emb'," +
                                     "(case when ordercloseddate between '"+ddlYear.SelectedItem+"-05-01 00:00:00' and '"+ddlYear.SelectedItem+"-05-31 23:59:59' and OrderStatus='closed' then 'May_closed' end) 'May_closed'," +
                                     "(case when ordercloseddate between '"+ddlYear.SelectedItem+"-06-01 00:00:00' and '"+ddlYear.SelectedItem+"-06-30 23:59:59' and OrderStatus='closed' then 'June_closed' end)'June_closed'," +
                                     "(case when ordercloseddate between '"+ddlYear.SelectedItem+"-07-01 00:00:00' and '"+ddlYear.SelectedItem+"-07-31 23:59:59' and OrderStatus='closed' then 'july_closed' end) 'july_closed'," +
                                     "(case when ordercloseddate between '"+ddlYear.SelectedItem+"-08-01 00:00:00' and '"+ddlYear.SelectedItem+"-08-31 23:59:59' and OrderStatus='closed' then 'August_closed' end) 'August_closed'," +
                                     "(case when ordercloseddate between '"+ddlYear.SelectedItem+"-09-01 00:00:00' and '"+ddlYear.SelectedItem+"-09-30 23:59:59' and OrderStatus='closed' then 'Sept_closed' end) 'Sept_closed'," +
                                     "(case when ordercloseddate between '"+ddlYear.SelectedItem+"-10-01 00:00:00' and '"+ddlYear.SelectedItem+"-10-31 23:59:59' and OrderStatus='closed' then 'Oct_closed' end) 'Oct_closed'," +
                                     "(case when ordercloseddate between '"+ddlYear.SelectedItem+"-11-01 00:00:00' and '"+ddlYear.SelectedItem+"-11-30 23:59:59' and OrderStatus='closed' then 'Nov_closed' end) 'Nov_closed'," +
                                     "(case when ordercloseddate between '"+ddlYear.SelectedItem+"-12-01 00:00:00' and '"+ddlYear.SelectedItem+"-12-31 23:59:59' and OrderStatus='closed' then 'Dec_closed' end) 'Dec_closed'" +

                                     "from hsrpRecords  inner join rtolocation rr on hsrpRecords.RTOLocationID=rr.rtolocationid and rr.distrelation='" + dropDownListDistrict.SelectedValue + "' where hsrpRecords.HSRP_StateID=3 and RTOLocationCode is not null)dd group by dd.RtoLocationname, dd.RtoLocationCode", con);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds, "hi");
            Session["totalrow"] = ds.Tables[0].Rows.Count;
            GridView1.DataSource = ds;
            GridView1.DataBind();
            con.Close();

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
