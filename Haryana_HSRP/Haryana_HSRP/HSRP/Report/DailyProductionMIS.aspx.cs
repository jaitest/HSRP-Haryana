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
    public partial class DailyProductionMIS : System.Web.UI.Page
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
                HSRPStateID =  Session["UserHSRPStateID"].ToString();

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

                            FilldropDownListOrganization();
                            labelClient.Visible = true; 
                            dropDownListClient.Visible = true;

                            labelOrganization.Enabled = false;
                            DropDownListStateName.Enabled = false;
                            labelClient.Enabled = true;
                            labelDate.Visible = false;

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
                style9.Alignment.Horizontal = StyleHorizontalAlignment.Right;
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
                row.Cells.Add(new WorksheetCell("Report Date:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(ReportDate, "HeaderStyle2"));
                row = sheet.Table.Rows.Add();
 

                row.Index = 7;
                //row.Cells.Add("Order Date");
                row.Cells.Add(new WorksheetCell("S.No.", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Location", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Autho.Slip", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Production", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Installation", "HeaderStyle6")); 
                //row.Cells.Add(new WorksheetCell("Balance For Production", "HeaderStyle6"));
                //row.Cells.Add(new WorksheetCell("Balance For Installation", "HeaderStyle6")); 

                 row = sheet.Table.Rows.Add();
                String StringField = String.Empty;

                String StringAlert = String.Empty; 
                row.Index = 9; 
                UserType = Convert.ToInt32(Session["UserType"]);
                if (UserType == 0)
                {
                    int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                    SQLString = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID where  a.LocationType!='District' and   a.HSRP_StateID='" + intHSRPStateID + "'";
                }
                else
                {
                    SQLString = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID where a.LocationType!='District' and  UserRTOLocationMapping.UserID='" + strUserID + "' ";
                }

                StateName = Utils.GetDataTable(SQLString.ToString(), CnnString);
                if (StateName.Rows.Count > 0)
                {
                    float totAuth = 0;
                    float totProduction = 0;
                    float totInstallation = 0; 
                    float totBalProduction = 0;
                    float totBalInstallation = 0;
                    int sno = 0;

                    for (int i = 0; i <= StateName.Rows.Count - 1; i++)
                    {
                        UserType = Convert.ToInt32(Session["UserType"]);
                        if (UserType == 0)
                        {
                            SQLString = "Report_Daily_ProductionMIS'" + OrderDate1 + "','" + intHSRPStateID + "','" + StateName.Rows[i]["RTOLocationID"] + "',0";
                        }
                        else
                        {
                          //HSRPStateID = Convert.ToInt32(Session["UserHSRPStateID"].ToString());
                            RTOLocationID = Convert.ToInt32(Session["UserRTOLocationID"].ToString());
                            SQLString = "Report_Daily_ProductionMIS'" + OrderDate1 + "','" + HSRPStateID + "','" + StateName.Rows[i]["RTOLocationID"] + "',1";
                        }

                        dt = Utils.GetDataTable(SQLString, CnnString); 
                        string RTOColName = string.Empty;
                        if (dt.Rows.Count > 0)
                        { 
                            sno = sno + 1;
                            foreach (DataRow dtrows in dt.Rows) // Loop over the rows.
                            { 
                                totAuth = totAuth + Convert.ToInt64(dtrows["Autho.Slip"]);
                                totProduction = totProduction + Convert.ToInt64(dtrows["Production"]);
                                totInstallation = totInstallation + Convert.ToInt64(dtrows["Installation"]);
                                totBalProduction = totBalProduction + Convert.ToInt64(dtrows["Balance For Production"]);
                                totBalInstallation = totBalInstallation + Convert.ToInt64(dtrows["Balance For Installation"]);

                               //row = sheet.Table.Rows.Add();
                                row.Cells.Add(new WorksheetCell(Convert.ToInt16(sno).ToString(), DataType.String, "HeaderStyle"));
                                if (RTOColName != StateName.Rows[i]["RTOLocationName"].ToString())
                                {
                                    RTOColName = StateName.Rows[i]["RTOLocationName"].ToString();
                                    row.Cells.Add(new WorksheetCell(StateName.Rows[i]["RTOLocationName"].ToString(), "HeaderStyle"));
                                }
                                else
                                {
                                    row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                                } 
                               // row.Cells.Add(new WorksheetCell(dtrows["LOCATION"].ToString(), DataType.String, "HeaderStyle"));
                                row.Cells.Add(new WorksheetCell(dtrows["Autho.Slip"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["Production"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["Installation"].ToString(), DataType.String, "HeaderStyle5"));
                                //row.Cells.Add(new WorksheetCell(dtrows["Balance For Production"].ToString(), DataType.String, "HeaderStyle5"));
                                //row.Cells.Add(new WorksheetCell(dtrows["Balance For Installation"].ToString(), DataType.String, "HeaderStyle5"));  
                            }
                            row = sheet.Table.Rows.Add();
                        } 
                        else
                        {
                            row = sheet.Table.Rows.Add();
                            sno = sno + 1;
                            totAuth = 0;
                            totProduction = 0;
                            totInstallation = 0;
                            totBalProduction = 0;
                            totBalInstallation = 0; 

                            row.Cells.Add(new WorksheetCell(Convert.ToInt16(sno).ToString(), DataType.String, "HeaderStyle"));
                            row.Cells.Add(new WorksheetCell(StateName.Rows[i]["RTOLocationName"].ToString(), "HeaderStyle")); 
                            row.Cells.Add(new WorksheetCell("0", DataType.String, "HeaderStyle5"));
                            row.Cells.Add(new WorksheetCell("0", DataType.String, "HeaderStyle5"));
                            row.Cells.Add(new WorksheetCell("0", DataType.String, "HeaderStyle5"));
                            //row.Cells.Add(new WorksheetCell("0", DataType.String, "HeaderStyle5"));
                            //row.Cells.Add(new WorksheetCell("0", DataType.String, "HeaderStyle5"));
                            //row.Cells.Add(new WorksheetCell(Convert.ToInt32(TotalAmount).ToString(), "HeaderStyle5")); 
                            row = sheet.Table.Rows.Add();
                             
                        }
                    }
                    row = sheet.Table.Rows.Add();
                    row = sheet.Table.Rows.Add(); 
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle9"));
                    row.Cells.Add(new WorksheetCell("Grand Total :", "HeaderStyle9")); 
                    row.Cells.Add(new WorksheetCell((totAuth).ToString(), "HeaderStyle9"));
                    row.Cells.Add(new WorksheetCell((totProduction).ToString(), "HeaderStyle9"));
                    row.Cells.Add(new WorksheetCell((totInstallation).ToString(), "HeaderStyle9"));
                    //row.Cells.Add(new WorksheetCell((totBalProduction).ToString(), "HeaderStyle9")); 
                    //row.Cells.Add(new WorksheetCell((totBalInstallation).ToString(), "HeaderStyle9"));

                    totAuth = 0;
                    totProduction = 0;
                    totInstallation = 0;
                    totBalProduction = 0;
                    totBalInstallation = 0; 

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



        
        public static String TitleCaseString(String s)
        {
            if (s == null) return s;

            String[] words = s.Split(' ');
            for (int i = 0; i < words.Length; i++)
            {
                if (words[i].Length == 0) continue;

                Char firstChar = Char.ToUpper(words[i][0]);
                String rest = "";
                if (words[i].Length > 1)
                {
                    rest = words[i].Substring(1).ToLower();
                }
                words[i] = firstChar + rest;
            }
            return String.Join(" ", words);
        }
    }
}