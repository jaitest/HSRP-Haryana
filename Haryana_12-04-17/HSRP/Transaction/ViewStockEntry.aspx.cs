using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CarlosAg.ExcelXmlWriter;
using System.Drawing;


namespace HSRP.Transaction
{
    public partial class ViewStockEntry : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        string UserType = string.Empty;
        string HSRPStateID = string.Empty;
        string RTOLocationID = string.Empty;
        int intHSRPStateID;
    


        string AllLocation = string.Empty;
        string OrderStatus = string.Empty;
      
        DateTime OrderDate1;
        DataProvider.BAL bl = new DataProvider.BAL();
        string StickerManditory = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            Utils.GZipEncodePage();
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {


                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();

                RTOLocationID = Session["UserRTOLocationID"].ToString();
                UserType = Session["UserType"].ToString();
                HSRPStateID = Session["UserHSRPStateID"].ToString();

                lblErrMsg.Text = string.Empty;
                strUserID = Session["UID"].ToString();
                ComputerIP = Request.UserHostAddress;

                if (!IsPostBack)
                {
                    InitialSetting();
                    try
                    {
                        if (UserType == "0")
                        {
                            labelOrganization.Visible = true;
                            DropDownListStateName.Visible = true;
                            DropDownListStateName.Enabled = true;
                            labelClient.Visible = true;
                            dropDownListClient.Visible = true;
                            FilldropDownListOrganization();
                            FilldropDownListClient();
                            FilldropDownListUser();
                        }
                        else
                        {
                            labelOrganization.Enabled = false;
                            DropDownListStateName.Enabled = false;
                            labelClient.Enabled = false;
                            FilldropDownListClient();
                            FilldropDownListOrganization();
                            FilldropDownListUser();
                            //buildGrid();
                        }
                    }
                    catch (Exception err)
                    {
                        lblErrMsg.Text = "Error on Page Load" + err.Message.ToString();
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
            OrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(-3.00);
            OrderDate.MaxDate = DateTime.Parse(MaxDate);
            CalendarOrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarOrderDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
        }

        #region DropDown

        private void FilldropDownListOrganization()
        {
            if (UserType == "0")
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where ActiveStatus='Y' Order by HSRPStateName";
                Utils.PopulateDropDownList(DropDownListStateName, SQLString.ToString(), CnnString, "--Select State--");
                // DropDownListStateName.SelectedIndex = DropDownListStateName.Items.Count - 1;

            }
            else
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID=" + HSRPStateID + " and ActiveStatus='Y' Order by HSRPStateName";
                DataTable dts = Utils.GetDataTable(SQLString, CnnString);
                DropDownListStateName.DataSource = dts;
                DropDownListStateName.DataBind();
            }
        }
        private void FilldropDownListClient()
        {
            if (UserType == "0")
            {

                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);

                //SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + HSRPStateID + " and ActiveStatus!='N'   Order by RTOLocationName";
                SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + intHSRPStateID + " and ActiveStatus!='N'   Order by RTOLocationName";
                Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), CnnString, "--Select RTO Name--");


            }
            else
            {
                string UserID = Convert.ToString(Session["UID"]);
                SQLString = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, (a.RTOLocationCode+', ') as RTOCode, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID where UserRTOLocationMapping.UserID='" + UserID + "' ";
                DataTable dss = Utils.GetDataTable(SQLString, CnnString);
                labelOrganization.Visible = true;
                DropDownListStateName.Visible = true;
                labelClient.Visible = true;
                dropDownListClient.Visible = true;
                dropDownListClient.DataSource = dss;
                dropDownListClient.DataBind();
                string RTOCode = string.Empty;
                if (dss.Rows.Count > 0)
                {
                    for (int i = 0; i <= dss.Rows.Count - 1; i++)
                    {
                        RTOCode += dss.Rows[i]["RTOCode"].ToString();
                    }
                }
            }
        }
        private void FilldropDownListUser()
        {
            int intHSRPStateID;
            int intRTOLocationID;
            if (UserType == "0")
            {
                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                int.TryParse(dropDownListClient.SelectedValue, out  intRTOLocationID);
            }
            else if (UserType == "1")
            {
                intHSRPStateID = Convert.ToInt32(HSRPStateID);
                int.TryParse(dropDownListClient.SelectedValue, out  intRTOLocationID);
            }
            else
            {
                intHSRPStateID = Convert.ToInt32(HSRPStateID);
                intRTOLocationID = Convert.ToInt32(RTOLocationID);
            }

            SQLString = "select UserID,ISNULL(UserFirstName,'')+Space(2)+ISNULL(UserLastName,'') as Names from Users where HSRP_StateID='" + intHSRPStateID + "' and RTOLocationID=" + intRTOLocationID + "  order by UserFirstName";
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            Utils.PopulateDropDownList(dropDownListUser, SQLString, CnnString, "--Select User--");
            dropDownListUser.SelectedIndex = 0;
        }
       


        protected void dropDownListClient_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (DropDownListStateName.SelectedItem.Text != "--Select Client--")
            {
                ShowGrid();
            }
            else
            {
                Grid1.Items.Clear();
                lblErrMsg.Text = String.Empty;
                lblErrMsg.Text = "Please Select Client.";
                return;
            }
        }
        #endregion

        private void ShowGrid()
        {

            if (String.IsNullOrEmpty(dropDownListClient.SelectedValue) || dropDownListClient.SelectedValue.Equals("--Select Client--"))
            {
                Grid1.Items.Clear();
                lblErrMsg.Text = String.Empty;
                lblErrMsg.Text = "Please Select Client.";
                return;
            }
            buildGrid();
        }


        #region Grid
        public void buildGrid()
        {
            try
            {

                ClearGrid();
                Grid1.Items.Clear();

                String[] StringAuthDate = OrderDate.SelectedDate.ToString().Split('/');
                string MonTo = ("0" + StringAuthDate[0]);
                string MonthdateTO = MonTo.Replace("00", "0").Replace("01", "1");
                String FromDateTo = StringAuthDate[1] + "-" + MonthdateTO + "-" + StringAuthDate[2].Split(' ')[0];
                String From = StringAuthDate[0] + "/" + StringAuthDate[1] + "/" + StringAuthDate[2].Split(' ')[0];
                //AuthorizationDate = new DateTime(Convert.ToInt32(StringAuthDate[2].Split(' ')[0]), Convert.ToInt32(StringAuthDate[0]), Convert.ToInt32(StringAuthDate[1]));
                string AuthorizationDate = From + " 00:00:00"; // Convert.ToDateTime();

                String[] StringOrderDate = HSRPAuthDate.SelectedDate.ToString().Split('/');
                string Mon = ("0" + StringOrderDate[0]);
                string Monthdate = Mon.Replace("00", "0").Replace("01", "1");
                string FromDate = StringOrderDate[1] + "-" + Monthdate + "-" + StringOrderDate[2].Split(' ')[0];

                String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
                //OrderDate1 = new DateTime(Convert.ToInt32(StringOrderDate[2].Split(' ')[0]), Convert.ToInt32(StringOrderDate[0]), Convert.ToInt32(StringOrderDate[1]));
                string OrderDate1 = From1 + " 23:59:59";

                DateTime StartDate = Convert.ToDateTime(OrderDate.SelectedDate);
                DateTime EndDate = Convert.ToDateTime(HSRPAuthDate.SelectedDate);

                HSRPStateID = DropDownListStateName.SelectedValue;
                RTOLocationID = dropDownListClient.SelectedValue;
                string UID = Session["UID"].ToString();
               // string OrderStatus = dropDownListorderStatus.SelectedItem.Text;

                SQLString = "select stockPlateID, convert(varchar, RecordDate, 110) as RecordDate, EmbosedPlate285_45Yellow,BlanckPlate_340_200Yellow,BlanckPlate_200_100White, (Select (UserFirstName +' '+ userLastName)  from users where userID=PlateStockEntry.UserID) as UserName  from plateStockentry WHERE hsrp_stateID='" + HSRPStateID + "' and RtoLocationID='" + RTOLocationID + "' and stock_date  between '" + AuthorizationDate + "' and '" + OrderDate1 + "'";
            
                DataTable dt = Utils.GetDataTable(SQLString, CnnString);

                if (dt.Rows.Count > 0)
                { 
                    Grid1.DataSource = dt;
                    Grid1.RunningMode = (ComponentArt.Web.UI.GridRunningMode)Enum.Parse(typeof(ComponentArt.Web.UI.GridRunningMode), "Client");
                    Grid1.SearchOnKeyPress = true;
                    Grid1.DataBind();
                    Grid1.RecordCount.ToString();
                }
                else
                {
                    Grid1.Items.Clear();
                    ClearGrid();
                }

            }
            catch (Exception ex)
            {
                lblErrMsg.Text = "Error in Populating Grid :" + ex.Message.ToString();
            }
        }
        public void OnNeedRebind(object sender, EventArgs oArgs)
        {
            System.Threading.Thread.Sleep(200);
            Grid1.DataBind();
        }
        public void OnNeedDataSource(object sender, EventArgs oArgs)
        {
            buildGrid();
        }
        public void OnPageChanged(object sender, ComponentArt.Web.UI.GridPageIndexChangedEventArgs oArgs)
        {
            Grid1.CurrentPageIndex = oArgs.NewIndex;
        }
        public void OnFilter(object sender, ComponentArt.Web.UI.GridFilterCommandEventArgs oArgs)
        {
            Grid1.Filter = oArgs.FilterExpression;
        }
        public void OnSort(object sender, ComponentArt.Web.UI.GridSortCommandEventArgs oArgs)
        {
            Grid1.Sort = oArgs.SortExpression;
        }
        private void ddGridRunningMode_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            Grid1.RunningMode = (ComponentArt.Web.UI.GridRunningMode)Enum.Parse(typeof(ComponentArt.Web.UI.GridRunningMode), "Client");
            buildGrid();
            Grid1.DataBind();
            adjustToRunningMode();
        }
        public void OnGroup(object sender, ComponentArt.Web.UI.GridGroupCommandEventArgs oArgs)
        {
            Grid1.GroupBy = oArgs.GroupExpression;
        }
        private void adjustToRunningMode()
        {

            Grid1.SliderPopupClientTemplateId = "SliderTemplate";
            Grid1.SliderPopupOffsetX = 20;

        }
        #endregion


        protected void DropDownListStateName_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilldropDownListClient();
            dropDownListClient.Visible = true;
            labelClient.Visible = true;

        }
        private void ClearGrid()
        {
            Grid1.Items.Clear();
            lblErrMsg.Text = String.Empty;
            return;
        }

        protected void ButtonGo_Click(object sender, EventArgs e)
        {
            buildGrid();

        }

        protected void dropDownListClient_SelectedIndexChanged1(object sender, EventArgs e)
        {
            if (dropDownListClient.SelectedItem.Text != "--Select RTO Name--")
            {
                dropDownListUser.Enabled = true;
                FilldropDownListUser();
               // UpdatePanelUser.Update();
            }
            else
            {
                dropDownListUser.Enabled = false;
                dropDownListUser.Enabled = false;
                lblErrMsg.Text = string.Empty;
                lblErrMsg.Text = "Please Select User";
            }
        }

        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
           
            try
            {
                LabelError.Text = "";
                String[] StringAuthDate = HSRPAuthDate.SelectedDate.ToString().Split('/');
                String ReportDateEnd = StringAuthDate[0] + "/" + StringAuthDate[1] + "/" + StringAuthDate[2].Split(' ')[0];
                string ReportDate2 = ReportDateEnd + " 23:59:59";
                String DatePrint2 = StringAuthDate[1] + "/" + StringAuthDate[0] + "/" + StringAuthDate[2].Split(' ')[0];


                String[] StringOrderDate = OrderDate.SelectedDate.ToString().Split('/');
                string Mon = ("0" + StringOrderDate[0]);
                string Monthdate = Mon.Replace("00", "0").Replace("01", "1");
                String From2 = StringOrderDate[1] + "-" + Monthdate + "-" + StringOrderDate[2].Split(' ')[0];

                String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
                String DatePrint1 = StringOrderDate[1] + "/" + StringOrderDate[0] + "/" + StringOrderDate[2].Split(' ')[0];

                String DatePrint = DatePrint1 + "   To    " + DatePrint2;

                String ReportDate = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
                string ReportDate1 = ReportDate + " 00:00:00";

                OrderDate1 = new DateTime(Convert.ToInt32(StringOrderDate[2].Split(' ')[0]), Convert.ToInt32(StringOrderDate[0]), Convert.ToInt32(StringOrderDate[1]));

                DateTime StartDate = Convert.ToDateTime(OrderDate.SelectedDate);
                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
              
                DataTable dt = new DataTable();

                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                //int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);

                string filename = "StockEntryReport-" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                Workbook book = new Workbook();

                // Specify which Sheet should be opened and the size of window by default
                book.ExcelWorkbook.ActiveSheetIndex = 1;
                book.ExcelWorkbook.WindowTopX = 100;
                book.ExcelWorkbook.WindowTopY = 200;
                book.ExcelWorkbook.WindowHeight = 7000;
                book.ExcelWorkbook.WindowWidth = 8000;

                // Some optional properties of the Document
                book.Properties.Author = "HSRP";
                book.Properties.Title = "Order Status Report";
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

                Worksheet sheet = book.Worksheets.Add("Order Status Report");
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
                style9.Alignment.Horizontal = StyleHorizontalAlignment.Right;
                style9.Interior.Color = "#FCF6AE";
                style9.Interior.Pattern = StyleInteriorPattern.Solid;


                WorksheetRow row = sheet.Table.Rows.Add();

                row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                row.Cells.Add(new WorksheetCell("Report :", "HeaderStyle3"));
                WorksheetCell cell = row.Cells.Add("Stock Entry Report");
                cell.MergeAcross = 3; // Merge two cells together
                cell.StyleID = "HeaderStyle3";

                row = sheet.Table.Rows.Add();
                row.Index = 3;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("State:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(DropDownListStateName.SelectedItem.ToString(), "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("Location:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(dropDownListClient.SelectedItem.ToString(), "HeaderStyle2"));


                row = sheet.Table.Rows.Add();
                row.Index = 4;

                DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("Date Generated :", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(dates.ToString("dd/M/yyyy"), "HeaderStyle2"));
                //row = sheet.Table.Rows.Add();
                //row.Index = 6;
                //row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                //row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));

                row.Cells.Add(new WorksheetCell("Report Date:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(DatePrint, "HeaderStyle2"));
                row = sheet.Table.Rows.Add();


                row.Index = 5;
                //row.Cells.Add("Order Date");
                row.Cells.Add(new WorksheetCell("S.No.", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("RecordDate", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("EmbosedPlate285_45Yellow", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("BlanckPlate_340_200Yellow", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("BlanckPlate_200_100White", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("UserName", "HeaderStyle6"));



                row = sheet.Table.Rows.Add();
                String StringField = String.Empty;
                String StringAlert = String.Empty;

                row.Index = 7;

                //UserType = Convert.ToInt32(Session["UserType"]);
                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);

                DateTime OrderDate3 = Convert.ToDateTime(ReportDate1);
                //SQLString = "exec Report_AllStateLocationWise25'" + ReportDate1 + "','" + ReportDate2 + "','" + DropDownListStateName.SelectedValue + "'";

                SQLString = "select stockPlateID, convert(varchar, RecordDate, 110) as RecordDate, EmbosedPlate285_45Yellow,BlanckPlate_340_200Yellow,BlanckPlate_200_100White, (Select (UserFirstName +' '+ userLastName)  from users where userID=PlateStockEntry.UserID) as UserName  from plateStockentry WHERE hsrp_stateID='" + DropDownListStateName.SelectedValue + "' and RtoLocationID='" + dropDownListClient.SelectedValue + "' and stock_date  between '" + ReportDate1 + "' and '" + ReportDate2 + "'";

                dt = Utils.GetDataTable(SQLString, CnnString);
                string RTOColName = string.Empty;
                //decimal totalAmount = 0;
                //decimal totalAmount1 = 0;
                //decimal totalAmount2 = 0;
                //decimal TotalRecordSum = 0;
                if (dt.Rows.Count > 0)
                {
                    int sno = 0;
                    foreach (DataRow dtrows in dt.Rows) // Loop over the rows.
                    {
                        sno = sno + 1;
                        row.Cells.Add(new WorksheetCell(Convert.ToInt16(sno).ToString(), DataType.String, "HeaderStyle"));

                        //row.Cells.Add(new WorksheetCell(dtrows["locationname"].ToString(), DataType.String, "HeaderStyle"));

                        row.Cells.Add(new WorksheetCell(dtrows["RecordDate"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["EmbosedPlate285_45Yellow"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["BlanckPlate_340_200Yellow"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["BlanckPlate_200_100White"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["UserName"].ToString(), DataType.String, "HeaderStyle"));
                        //int intNewOrder = int.Parse(dtrows["Dailyneworder"].ToString());
                        //int intEmbOrder = int.Parse(dtrows["Dailyemborder"].ToString());
                        //int intCloseOrder = int.Parse(dtrows["Dailycloseorder"].ToString());
                        //int sum = intNewOrder + intEmbOrder + intCloseOrder;
                        //TotalRecordSum = TotalRecordSum + sum;
                        //row.Cells.Add(new WorksheetCell(sum.ToString(), DataType.String, "HeaderStyle"));

                        //totalAmount = totalAmount + Math.Round(Convert.ToDecimal(dtrows["Dailyneworder"].ToString()));
                        //totalAmount1 = totalAmount1 + Math.Round(Convert.ToDecimal(dtrows["Dailyemborder"].ToString()));
                        //totalAmount2 = totalAmount2 + Math.Round(Convert.ToDecimal(dtrows["Dailycloseorder"].ToString()));

                        row = sheet.Table.Rows.Add();
                    }
                    row = sheet.Table.Rows.Add();
                    row = sheet.Table.Rows.Add();
                    //row.Cells.Add(new WorksheetCell("", "HeaderStyle8"));
                    //row.Cells.Add(new WorksheetCell("Total :", "HeaderStyle8"));
                    //row.Cells.Add(new WorksheetCell((totalAmount).ToString(), "HeaderStyle8"));
                    //row.Cells.Add(new WorksheetCell((totalAmount1).ToString(), "HeaderStyle8"));
                    //row.Cells.Add(new WorksheetCell((totalAmount2).ToString(), "HeaderStyle8"));
                    //row.Cells.Add(new WorksheetCell((TotalRecordSum).ToString(), "HeaderStyle8"));
                    ////row.Cells.Add(new WorksheetCell("", "HeaderStyle8"));

                    //totalAmount = 0;
                    //totalAmount1 = 0;
                    //totalAmount2 = 0;
                    //TotalRecordSum = 0;

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
    }
}