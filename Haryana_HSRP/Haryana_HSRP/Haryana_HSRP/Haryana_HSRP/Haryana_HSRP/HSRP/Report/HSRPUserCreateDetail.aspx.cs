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
    public partial class HSRPUserCreateDetail : System.Web.UI.Page
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
                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                strUserID = Session["UID"].ToString();
                ComputerIP = Request.UserHostAddress; 
                labelOrganization.Visible=false;
                    DropDownListStateName.Visible=false;

                    labelClient.Visible=false;
                    dropDownListClient.Visible=false;

                    labelDate.Visible=false; 
                    OrderDate.Visible=false; 
                if (!IsPostBack)
                {
                    InitialSetting();
                    try
                    {
                        InitialSetting();
                         
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
                LabelError.Text = "";

                String[] StringOrderDate = OrderDate.SelectedDate.ToString().Split('/');
                String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
                String ReportDate = StringOrderDate[1] + "/" + StringOrderDate[0] + "/" + StringOrderDate[2].Split(' ')[0];
                OrderDate1 = new DateTime(Convert.ToInt32(StringOrderDate[2].Split(' ')[0]), Convert.ToInt32(StringOrderDate[0]), Convert.ToInt32(StringOrderDate[1]));

                DateTime StartDate = Convert.ToDateTime(OrderDate.SelectedDate);
                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);
                DataTable StateName;
                DataTable dts;
                DataTable dt = new DataTable();

                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);

                string filename = "Report_Daily_Affixcation_Vechial_wise-" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                Workbook book = new Workbook();

                // Specify which Sheet should be opened and the size of window by default
                book.ExcelWorkbook.ActiveSheetIndex = 1;
                book.ExcelWorkbook.WindowTopX = 100;
                book.ExcelWorkbook.WindowTopY = 200;
                book.ExcelWorkbook.WindowHeight = 7000;
                book.ExcelWorkbook.WindowWidth = 8000;

                // Some optional properties of the Document
                book.Properties.Author = "HSRP";
                book.Properties.Title = "HSRP Daily Affixcation Vechial wise";
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

                Worksheet sheet = book.Worksheets.Add("HSRP Daily Affixcation Vechial wise");
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
                // row.
                row.Cells.Add(new WorksheetCell("", "HeaderStyle3")); 
                row.Cells.Add(new WorksheetCell("Report :", "HeaderStyle3"));
                WorksheetCell cell = row.Cells.Add("HSRP User Detail");
                cell.MergeAcross = 3; // Merge two cells together
                cell.StyleID = "HeaderStyle3";

                row = sheet.Table.Rows.Add();
                 
                row.Index = 7;
                //row.Cells.Add("Order Date");
                row.Cells.Add(new WorksheetCell("S.No.", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("State Name", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Total User", "HeaderStyle")); 
                row = sheet.Table.Rows.Add();
                String StringField = String.Empty;
                String StringAlert = String.Empty;

                row.Index = 9;

                UserType = Convert.ToInt32(Session["UserType"]);


                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                
                //SQLString = "SELECT     (UserFirstName +' '+ UserLastName) as FullName, UserLoginName, Password, Address1, MobileNo, EmailID, ActiveStatus, UserType FROM Users where HSRP_StateID='" + intHSRPStateID + "' order by UserLoginName";
                SQLString = "SELECT  COUNT(Users.UserID) as totalusers, HSRPState.HSRPStateName FROM  Users INNER JOIN HSRPState ON Users.HSRP_StateID = HSRPState.HSRP_StateID where Users.ActiveStatus='Y' group by HSRPState.HSRPStateName order by HSRPState.HSRPStateName";
                dt = Utils.GetDataTable(SQLString, CnnString);

                string RTOColName = string.Empty;
                int sno = 0;
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dtrows in dt.Rows) // Loop over the rows.
                    {
                        sno = sno + 1;
                        row = sheet.Table.Rows.Add();
                        row.Cells.Add(new WorksheetCell(Convert.ToInt16(sno).ToString(), DataType.String, "HeaderStyle"));

                        row.Cells.Add(new WorksheetCell(dtrows["HSRPStateName"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["totalusers"].ToString(), DataType.String, "HeaderStyle")); 
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

        //protected void btnExportToExcel_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //    LabelError.Visible = false;

        //    String[] StringOrderDate = OrderDate.SelectedDate.ToString().Split('/');
        //    String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
        //    String ReportDate = StringOrderDate[1] + "/" + StringOrderDate[0] + "/" + StringOrderDate[2].Split(' ')[0];
        //    OrderDate1 = new DateTime(Convert.ToInt32(StringOrderDate[2].Split(' ')[0]), Convert.ToInt32(StringOrderDate[0]), Convert.ToInt32(StringOrderDate[1]));
            
        //    DateTime StartDate = Convert.ToDateTime(OrderDate.SelectedDate); 
        //    int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
        //    int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);

            
        //      UserType = Convert.ToInt32(Session["UserType"]);
        //      if (UserType == 0)
        //      {       
        //          SQLString = "Report_Daily_Affixcation_Vechial_wise'" + OrderDate1 + "','" + intHSRPStateID + "','" + intRTOLocationID + "'";
        //      }
        //      else
        //      {
        //          HSRPStateID = Convert.ToInt32(Session["UserHSRPStateID"].ToString());
        //          RTOLocationID = Convert.ToInt32(Session["UserRTOLocationID"].ToString());

        //          SQLString = "Report_Daily_Affixcation_Vechial_wise'" + OrderDate1 + "','" + HSRPStateID + "','" + RTOLocationID + "'";
        //      }

        //      DataSet dt = Utils.getDataSet(SQLString, CnnString);
        //      if (dt.Tables[0].Rows.Count > 0)
        //      {

        //          int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
        //          int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);

        //          string filename = "Report_Daily_Affixcation_Vechial_wise" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
        //          Workbook book = new Workbook();

        //          // Specify which Sheet should be opened and the size of window by default
        //          book.ExcelWorkbook.ActiveSheetIndex = 1;
        //          book.ExcelWorkbook.WindowTopX = 100;
        //          book.ExcelWorkbook.WindowTopY = 200;
        //          book.ExcelWorkbook.WindowHeight = 7000;
        //          book.ExcelWorkbook.WindowWidth = 8000;

        //          // Some optional properties of the Document
        //          book.Properties.Author = "HSRP";
        //          book.Properties.Title = "HSRP Daily Affixcation Vechial wise";
        //          book.Properties.Created = DateTime.Now;


        //          // Add some styles to the Workbook
        //          WorksheetStyle style = book.Styles.Add("HeaderStyle");
        //          style.Font.FontName = "Tahoma";
        //          style.Font.Size = 10;
        //          style.Font.Bold = false;
        //          style.Alignment.Horizontal = StyleHorizontalAlignment.Center;
        //          style.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
        //          style.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
        //          style.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
        //          style.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


        //          WorksheetStyle style7 = book.Styles.Add("HeaderStyle7");
        //          style7.Font.FontName = "Tahoma";
        //          style7.Font.Size = 10;
        //          style7.Font.Bold = false;
        //          style7.Alignment.Horizontal = StyleHorizontalAlignment.Right;
        //          style7.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
        //          style7.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
        //          style7.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
        //          style7.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


        //          WorksheetStyle style5 = book.Styles.Add("HeaderStyle5");
        //          style5.Font.FontName = "Tahoma";
        //          style5.Font.Size = 10;
        //          style5.Font.Bold = true;
        //          style5.Alignment.Horizontal = StyleHorizontalAlignment.Center;
        //          style5.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
        //          style5.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
        //          style5.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
        //          style5.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


        //          WorksheetStyle style2 = book.Styles.Add("HeaderStyle2");
        //          style2.Font.FontName = "Tahoma";
        //          style2.Font.Size = 10;
        //          style2.Font.Bold = true;
        //          style.Alignment.Horizontal = StyleHorizontalAlignment.Left;


        //          WorksheetStyle style3 = book.Styles.Add("HeaderStyle3");
        //          style3.Font.FontName = "Tahoma";
        //          style3.Font.Size = 12;
        //          style3.Font.Bold = true;
        //          style3.Alignment.Horizontal = StyleHorizontalAlignment.Left;

        //          Worksheet sheet = book.Worksheets.Add("HSRP Daily Affixcation Vechial wise");


        //          sheet.Table.Columns.Add(new WorksheetColumn(50));
        //          sheet.Table.Columns.Add(new WorksheetColumn(120));
        //          sheet.Table.Columns.Add(new WorksheetColumn(130));
        //          sheet.Table.Columns.Add(new WorksheetColumn(100));
        //          sheet.Table.Columns.Add(new WorksheetColumn(115));
        //          sheet.Table.Columns.Add(new WorksheetColumn(130));
        //          sheet.Table.Columns.Add(new WorksheetColumn(115));
        //          sheet.Table.Columns.Add(new WorksheetColumn(130));
        //          WorksheetRow row = sheet.Table.Rows.Add();
        //          // row.
        //          row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
        //          row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
        //          row.Cells.Add(new WorksheetCell("Report :", "HeaderStyle3"));
        //          row.Cells.Add(new WorksheetCell("HSRP Daily Affixcation Vechial wise Report", "HeaderStyle3"));
        //          //row.Cells.Add(<br>);
        //          row = sheet.Table.Rows.Add();
        //          row.Index = 3;
        //          row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
        //          row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
        //          row.Cells.Add(new WorksheetCell("State:", "HeaderStyle2"));
        //          row.Cells.Add(new WorksheetCell(DropDownListStateName.SelectedItem.ToString(), "HeaderStyle2"));

        //          row = sheet.Table.Rows.Add();

        //          row.Index = 4;
        //          row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
        //          row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
        //          row.Cells.Add(new WorksheetCell("Location:", "HeaderStyle2"));
        //          row.Cells.Add(new WorksheetCell(dropDownListClient.SelectedItem.ToString(), "HeaderStyle2"));
        //          //row.Cells.Add(<br>); 
        //          row = sheet.Table.Rows.Add();

        //          // Skip one row, and add some text
        //          row.Index = 5;
        //          DateTime date = System.DateTime.Now;
        //          string formatted = date.ToString("dd/MM/yyyy");

        //          row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
        //          row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
        //          row.Cells.Add(new WorksheetCell("Date Generated :", "HeaderStyle2"));
        //          row.Cells.Add(new WorksheetCell(formatted, "HeaderStyle2"));
        //          row = sheet.Table.Rows.Add();
        //          row.Index = 6;
        //          row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
        //          row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
        //          row.Cells.Add(new WorksheetCell("Report Date:", "HeaderStyle2"));
        //          row.Cells.Add(new WorksheetCell(ReportDate, "HeaderStyle2"));
        //          row = sheet.Table.Rows.Add();

        //          row.Index = 8;
        //          //row.Cells.Add("Order Date");
        //          row.Cells.Add(new WorksheetCell("S.No.", "HeaderStyle5"));
        //          row.Cells.Add(new WorksheetCell("Vehicle Type", "HeaderStyle5"));
        //          row.Cells.Add(new WorksheetCell("Daily Target", "HeaderStyle5"));
        //          row.Cells.Add(new WorksheetCell("Daily Actual", "HeaderStyle5"));
        //          row.Cells.Add(new WorksheetCell("Month Target", "HeaderStyle5"));
        //          row.Cells.Add(new WorksheetCell("Month Actual", "HeaderStyle5"));
        //          row.Cells.Add(new WorksheetCell("Year Target", "HeaderStyle5"));
        //          row.Cells.Add(new WorksheetCell("Year Actual", "HeaderStyle5"));
        //          row = sheet.Table.Rows.Add(); 
        //          String StringField = String.Empty;
        //          String StringAlert = String.Empty;


        //          if (dt.Tables[0].Rows.Count <= 0)
        //          {
        //              LabelError.Text = string.Empty;
        //              LabelError.Text = "Their is no selected data for the selected  date range.";
        //              return;
        //          }
        //          row.Index = 9; 
        //          int sno = 0;

        //          foreach (DataRow dtrows in dt.Tables[0].Rows) // Loop over the rows.
        //          {
        //              sno = sno + 1;
        //              row = sheet.Table.Rows.Add();
        //              row.Cells.Add(new WorksheetCell(Convert.ToInt16(sno).ToString(), DataType.String, "HeaderStyle"));
        //              row.Cells.Add(new WorksheetCell(dtrows["VehicleType"].ToString(), DataType.String, "HeaderStyle"));
        //              row.Cells.Add(new WorksheetCell(dtrows["Daily_Target"].ToString(), DataType.String, "HeaderStyle7"));
        //              row.Cells.Add(new WorksheetCell(dtrows["Daily_Actual"].ToString(), DataType.String, "HeaderStyle7"));
        //              row.Cells.Add(new WorksheetCell(dtrows["month_Target"].ToString(), DataType.String, "HeaderStyle7"));
        //              row.Cells.Add(new WorksheetCell(dtrows["month_Actual"].ToString(), DataType.String, "HeaderStyle7"));
        //              row.Cells.Add(new WorksheetCell(dtrows["yrs_Target"].ToString(), DataType.String, "HeaderStyle7"));
        //              row.Cells.Add(new WorksheetCell(dtrows["yrs_Actual"].ToString(), DataType.String, "HeaderStyle7"));
        //          }
        //          row = sheet.Table.Rows.Add();
        //          HttpContext context = HttpContext.Current;
        //          context.Response.Clear();
        //          // Save the file and open it
        //          book.Save(Response.OutputStream);

        //          //context.Response.ContentType = "text/csv";
        //          context.Response.ContentType = "application/vnd.ms-excel";

        //          context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
        //          context.Response.End();
        //      }
        //      else
        //      {
        //          string closescript1 = "<script>alert('No records found for selected date.')</script>";
        //          Page.RegisterStartupScript("abc", closescript1);
        //          return;
        //      }
        //    }

        //    catch (Exception ex)
        //    {
        //        LabelError.Visible = true;
        //        LabelError.Text = "Error in Populating Grid :" + ex.Message.ToString();
        //    }
        //}


    }

}