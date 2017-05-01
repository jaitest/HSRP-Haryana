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
    public partial class VehicleCollectionCountReport : System.Web.UI.Page
    {

        int UserType;
        string CnnString = string.Empty;
        string HSRP_StateID = string.Empty;
        string RTOLocationID = string.Empty;
        string strUserID = string.Empty;
        int intHSRPStateID;
        int intRTOLocationID;        
        string SQLString = string.Empty;
        

        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {

                UserType = Convert.ToInt32(Session["UserType"]);
                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                strUserID = Session["UID"].ToString();

                HSRP_StateID = Session["UserHSRPStateID"].ToString();
                RTOLocationID = Session["UserRTOLocationID"].ToString();
                if (!IsPostBack)
                {

                    try
                    {
                        if (UserType.Equals(0))
                        {
                            // labelOrganization.Visible = true;
                            DropDownListStateName.Visible = true;
                            dropDownListClient.Visible = true;
                            FilldropDownListOrganization();
                           // FilldropDownListClient();
                            DropDownListStateName.SelectedValue = "6";
                            DropDownListStateName.Enabled = false;
                           // OrderDatefrom.SelectedDate = System.DateTime.Now;                          
                        }
                        else
                        {
                            // labelOrganization.Visible = true;
                            DropDownListStateName.Visible = true;
                            dropDownListClient.Visible = true;
                            FilldropDownListOrganization();
                           // FilldropDownListClient();
                            DropDownListStateName.SelectedValue="6";
                            DropDownListStateName.Enabled = false;
                           // OrderDatefrom.SelectedDate = System.DateTime.Now;      
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
            if (UserType.Equals(0))
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where ActiveStatus='Y' Order by HSRPStateName";
                Utils.PopulateDropDownList(DropDownListStateName, SQLString.ToString(), CnnString, "--Select State--");
            }
            else
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID=" + HSRP_StateID + " and ActiveStatus='Y' Order by HSRPStateName";
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
                SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + 6 + " and ActiveStatus!='N' Order by RTOLocationName";
                Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), CnnString, "---Select RTO-");
            }
            else
            {
                SQLString = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID where  a.LocationType!='District' and UserRTOLocationMapping.UserID='" + strUserID + "' ";
                Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), CnnString, "--Select RTO--");
                //DataSet dss = Utils.getDataSet(SQLString, CnnString);
                //dropDownListClient.DataSource = dss;
                //dropDownListClient.DataBind();
            }
        }

        public void BuildGrid1()
        {
            try
            {
                DataTable dt;
                String[] StringAuthDate = DropDownList1.SelectedItem.ToString().Split('/');
                String ReportDateEnd = StringAuthDate[1] + "-" + StringAuthDate[0] + "-" + StringAuthDate[2].Split(' ')[0];

                SQLString = "select * from VehicleCollectionCount where Date='" + ReportDateEnd + "'";
                dt = Utils.GetDataTable(SQLString, CnnString);
                //DataGrid1.DataSource = dt;
                //DataGrid1.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
            
            getvalue = dropDownListClient.SelectedItem.Text;
             if (getvalue == "--Select RTO--")
            {
                blnvalid = false;
                
                Label1.Text = "Please select RTO";

            }
            if (ddlReport.SelectedItem.ToString() == "--Select Report Wise--")
            {
                blnvalid = false;
                Label1.Text = "Please Select Report Wise Type";
            }
            return blnvalid;

        }

        protected void btnexport_Click(object sender, EventArgs e)
        {

            {
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
                        //int.TryParse(dropDownListUser.SelectedValue, out UserID);

                        string filename = "Vehicle_Collection_Count_Report" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                        Workbook book = new Workbook();

                        String[] StringAuthDate = DropDownList1.SelectedItem.ToString().Split('/');
                        String ReportDateEnd = StringAuthDate[1] + "-" + StringAuthDate[0] + "-" + StringAuthDate[2];

                        // Specify which Sheet should be opened and the size of window by default
                        book.ExcelWorkbook.ActiveSheetIndex = 1;
                        book.ExcelWorkbook.WindowTopX = 100;
                        book.ExcelWorkbook.WindowTopY = 200;
                        book.ExcelWorkbook.WindowHeight = 7000;
                        book.ExcelWorkbook.WindowWidth = 8000;

                        // Some optional properties of the Document
                        book.Properties.Author = "HSRP";
                        book.Properties.Title = "TOTAL VEHICLE " + ddlReport.SelectedValue.ToString().ToUpper() + " REPORT";
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

                        WorksheetStyle style1 = book.Styles.Add("HeaderStyle10");
                        style1.Font.FontName = "Tahoma";
                        style1.Font.Size = 9;
                        style1.Font.Bold = false;
                        style1.Alignment.Horizontal.Equals("Center");
                        style1.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                        style1.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                        style1.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                        style1.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                        style1.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);

                        WorksheetStyle style8 = book.Styles.Add("HeaderStyle8");
                        style8.Font.FontName = "Tahoma";
                        style8.Font.Size = 8;
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

                        Worksheet sheet = book.Worksheets.Add("TOTAL VEHICLE " + ddlReport.SelectedValue.ToString().ToUpper() + " REPORT");
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
                        SQLString = "Select CompanyName from HSRPState where HSRP_StateID='6'";
                        Utils.getDataSingleValue(SQLString, CnnString, "CompanyName").ToString();               
                        WorksheetCell cell = row.Cells.Add(Utils.getDataSingleValue(SQLString, CnnString, "CompanyName").ToString());
                        cell.MergeAcross = 3; // Merge two cells together
                        cell.StyleID = "HeaderStyle8";


                        row = sheet.Table.Rows.Add();
                        row.Index = 3;
                        row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                        row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                        WorksheetCell cell21 = row.Cells.Add("TOTAL VEHICLE " + ddlReport.SelectedValue.ToString().ToUpper() + " REPORT");
                        cell21.MergeAcross = 3; // Merge two cells together
                        cell21.StyleID = "HeaderStyle8";

                        row = sheet.Table.Rows.Add();

                        row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                        row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                        row.Cells.Add(new WorksheetCell("State:", "HeaderStyle2"));
                        row.Cells.Add(new WorksheetCell(DropDownListStateName.SelectedItem.ToString(), "HeaderStyle2"));

                        //row = sheet.Table.Rows.Add();
                        //row.Index = 4;
                        //row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                        //row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                        row.Cells.Add(new WorksheetCell("Location:", "HeaderStyle2"));
                        row.Cells.Add(new WorksheetCell(dropDownListClient.SelectedItem.ToString().ToUpper(), "HeaderStyle2"));

                        row = sheet.Table.Rows.Add();
                        row.Index = 6;

                        DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                        row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                        row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                        row.Cells.Add(new WorksheetCell("Date Generated:", "HeaderStyle2"));
                        row.Cells.Add(new WorksheetCell(DropDownList1.SelectedItem.ToString(), "HeaderStyle2"));
                        //row = sheet.Table.Rows.Add();
                        //row.Index = 6;
                        //row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                        //row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));

                        
                        row = sheet.Table.Rows.Add();

                        row.Index = 7;
                        //row.Cells.Add("Order Date");
                        row.Cells.Add(new WorksheetCell("LOCATION", "HeaderStyle6"));
                        //row.Cells.Add(new WorksheetCell("Order Month", "HeaderStyle6"));
                        row.Cells.Add(new WorksheetCell("MOTOR CYCLE", "HeaderStyle6"));
                        row.Cells.Add(new WorksheetCell("SCOOTER", "HeaderStyle6"));
                        row.Cells.Add(new WorksheetCell("TRACTOR", "HeaderStyle6"));
                        row.Cells.Add(new WorksheetCell("THREE WHEELER", "HeaderStyle6"));
                        row.Cells.Add(new WorksheetCell("LMV(CLASS)", "HeaderStyle6"));
                        row.Cells.Add(new WorksheetCell("LMV", "HeaderStyle6"));
                        row.Cells.Add(new WorksheetCell("MCV", "HeaderStyle6"));
                        //row.Cells.Add(new WorksheetCell("Close Date", "HeaderStyle6"));
                        row.Cells.Add(new WorksheetCell("GRAND TOTAL", "HeaderStyle6"));
                        //row.Cells.Add(new WorksheetCell("Delay in No. of Days", "HeaderStyle"));


                        row = sheet.Table.Rows.Add();

                        row.Index = 8;
                        if (ddlReport.SelectedItem.Text == "Count Wise")
                        {
                            //row.Index = 5;
                            //row.Cells.Add("Order Date");
                            row.Cells.Add(new WorksheetCell());
                            //row.Cells.Add(new WorksheetCell("Order Month", "HeaderStyle6"));
                            row.Cells.Add(new WorksheetCell("Count", "HeaderStyle6"));
                            row.Cells.Add(new WorksheetCell("Count", "HeaderStyle6"));
                            row.Cells.Add(new WorksheetCell("Count", "HeaderStyle6"));
                            row.Cells.Add(new WorksheetCell("Count", "HeaderStyle6"));
                            row.Cells.Add(new WorksheetCell("Count", "HeaderStyle6"));
                            row.Cells.Add(new WorksheetCell("Count", "HeaderStyle6"));
                            row.Cells.Add(new WorksheetCell("Count", "HeaderStyle6"));
                            //row.Cells.Add(new WorksheetCell("Close Date", "HeaderStyle6"));
                            row.Cells.Add(new WorksheetCell());
                        }
                        else
                        {
                            //row.Index = 5;
                            //row.Cells.Add("Order Date");
                            row.Cells.Add(new WorksheetCell());
                            //row.Cells.Add(new WorksheetCell("Order Month", "HeaderStyle6"));
                            row.Cells.Add(new WorksheetCell("Value", "HeaderStyle6"));
                            row.Cells.Add(new WorksheetCell("Value", "HeaderStyle6"));
                            row.Cells.Add(new WorksheetCell("Value", "HeaderStyle6"));
                            row.Cells.Add(new WorksheetCell("Value", "HeaderStyle6"));
                            row.Cells.Add(new WorksheetCell("Value", "HeaderStyle6"));
                            row.Cells.Add(new WorksheetCell("Value", "HeaderStyle6"));
                            row.Cells.Add(new WorksheetCell("Value", "HeaderStyle6"));
                            //row.Cells.Add(new WorksheetCell("Close Date", "HeaderStyle6"));
                            row.Cells.Add(new WorksheetCell());

                        }
                        String StringField = String.Empty;
                        String StringAlert = String.Empty;
                        row = sheet.Table.Rows.Add();
                        row.Index = 9;

                        DataTable dt;
                            SQLString = "Select * from VehicleCollectionCount where date='" + Convert.ToDateTime(ReportDateEnd) + "' and location ='"+dropDownListClient.SelectedItem.ToString()+"' order by ID";
                            dt = Utils.GetDataTable(SQLString, CnnString);
                            if (dt.Rows.Count > 0)
                            {
                                Label1.Text = "";
                                foreach (DataRow dtrows in dt.Rows) // Loop over the rows.
                                {
                                    if (ddlReport.SelectedItem.ToString() == "Count Wise")
                                    {
                                        row.Cells.Add(new WorksheetCell(dtrows["Location"].ToString(), DataType.String, "HeaderStyle"));
                                        row.Cells.Add(new WorksheetCell(dtrows["MotorCycle_Count"].ToString(), DataType.String, "HeaderStyle"));
                                        row.Cells.Add(new WorksheetCell(dtrows["Scooter_Count"].ToString(), DataType.String, "HeaderStyle"));
                                        row.Cells.Add(new WorksheetCell(dtrows["Tractor_Count"].ToString(), DataType.String, "HeaderStyle"));
                                        row.Cells.Add(new WorksheetCell(dtrows["3W_Count"].ToString(), DataType.String, "HeaderStyle"));
                                        row.Cells.Add(new WorksheetCell(dtrows["LMV_Class_Count"].ToString(), DataType.String, "HeaderStyle"));
                                        row.Cells.Add(new WorksheetCell("0", DataType.String, "HeaderStyle"));
                                        row.Cells.Add(new WorksheetCell(dtrows["MCV_Count"].ToString(), DataType.String, "HeaderStyle"));
                                        row.Cells.Add(new WorksheetCell(dtrows["Total_Count"].ToString(), DataType.String, "HeaderStyle"));
                                    }
                                    else
                                    {
                                        row.Cells.Add(new WorksheetCell(dtrows["Location"].ToString(), DataType.String, "HeaderStyle"));
                                        row.Cells.Add(new WorksheetCell(dtrows["MotorCycle_Sum"].ToString(), DataType.String, "HeaderStyle"));
                                        row.Cells.Add(new WorksheetCell(dtrows["Scooter_Sum"].ToString(), DataType.String, "HeaderStyle"));
                                        row.Cells.Add(new WorksheetCell(dtrows["Tractor_Sum"].ToString(), DataType.String, "HeaderStyle"));
                                        row.Cells.Add(new WorksheetCell(dtrows["3W_Sum"].ToString(), DataType.String, "HeaderStyle"));
                                        row.Cells.Add(new WorksheetCell(dtrows["LMV_Class_Sum"].ToString(), DataType.String, "HeaderStyle"));
                                        row.Cells.Add(new WorksheetCell("0", DataType.String, "HeaderStyle"));
                                        row.Cells.Add(new WorksheetCell(dtrows["MCV_Sum"].ToString(), DataType.String, "HeaderStyle"));
                                        row.Cells.Add(new WorksheetCell(dtrows["Total_Sum"].ToString(), DataType.String, "HeaderStyle"));
                                    }


                                    row = sheet.Table.Rows.Add();
                                }
                                if (ddlReport.SelectedItem.ToString() == "Count Wise")
                                {
                                    row.Cells.Add(new WorksheetCell("Total", DataType.String, "HeaderStyle"));
                                    row.Cells.Add(new WorksheetCell(dt.Rows[0]["MotorCycle_Count"].ToString(), DataType.String, "HeaderStyle"));
                                    row.Cells.Add(new WorksheetCell(dt.Rows[0]["Scooter_Count"].ToString(), DataType.String, "HeaderStyle"));
                                    row.Cells.Add(new WorksheetCell(dt.Rows[0]["Tractor_Count"].ToString(), DataType.String, "HeaderStyle"));
                                    row.Cells.Add(new WorksheetCell(dt.Rows[0]["3W_Count"].ToString(), DataType.String, "HeaderStyle"));
                                    row.Cells.Add(new WorksheetCell(dt.Rows[0]["LMV_Class_Count"].ToString(), DataType.String, "HeaderStyle"));
                                    row.Cells.Add(new WorksheetCell("0", DataType.String, "HeaderStyle"));
                                    row.Cells.Add(new WorksheetCell(dt.Rows[0]["MCV_Count"].ToString(), DataType.String, "HeaderStyle"));
                                    row.Cells.Add(new WorksheetCell(dt.Rows[0]["Total_Count"].ToString(), DataType.String, "HeaderStyle"));
                                }
                                else
                                {
                                    row.Cells.Add(new WorksheetCell("Total", DataType.String, "HeaderStyle"));
                                    row.Cells.Add(new WorksheetCell(dt.Rows[0]["MotorCycle_Sum"].ToString(), DataType.String, "HeaderStyle"));
                                    row.Cells.Add(new WorksheetCell(dt.Rows[0]["Scooter_Sum"].ToString(), DataType.String, "HeaderStyle"));
                                    row.Cells.Add(new WorksheetCell(dt.Rows[0]["Tractor_Sum"].ToString(), DataType.String, "HeaderStyle"));
                                    row.Cells.Add(new WorksheetCell(dt.Rows[0]["3W_Sum"].ToString(), DataType.String, "HeaderStyle"));
                                    row.Cells.Add(new WorksheetCell(dt.Rows[0]["LMV_Class_Sum"].ToString(), DataType.String, "HeaderStyle"));
                                    row.Cells.Add(new WorksheetCell("0", DataType.String, "HeaderStyle"));
                                    row.Cells.Add(new WorksheetCell(dt.Rows[0]["MCV_Sum"].ToString(), DataType.String, "HeaderStyle"));
                                    row.Cells.Add(new WorksheetCell(dt.Rows[0]["Total_Sum"].ToString(), DataType.String, "HeaderStyle"));
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
                            else
                            {
                                Label1.Text = "No Record Found";
                            }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }

        protected void DropDownListStateName_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilldropDownListClient();
            Label1.Visible = false;
        }

        protected void btngo_Click(object sender, EventArgs e)
        {
            //if (DropDownListStateName.SelectedItem.Text != "--Select State--" && dropDownListClient.SelectedItem.Text!="--Select RTO--")
            //{
            //    //Label1.Visible = false;
            //    BuildGrid1();
            //}

            //else
            //{
            //    if (DropDownListStateName.SelectedItem.Text != "--Select State--")
            //    {
            //        //Label1.Visible = false;
            //        BuildGrid1();
            //    }

            //    else
            //    {
            //        //Label1.Visible = false;
            //        DataGrid1.Items.Clear();
            //        return;
            //    }
            //}
        }

        protected void dropDownListClient_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList1.Items.Clear();
            DropDownList1.Items.Add("--Select Date--");
            if (dropDownListClient.SelectedItem.ToString() == "Dehradun")
            {
                DropDownList1.Items.Add("23/03/2012");
                DropDownList1.Items.Add("27/04/2012");
            }
            else if (dropDownListClient.SelectedItem.ToString() == "Haldwani")
            {
                DropDownList1.Items.Add("26/12/2012");
            }
            else if (dropDownListClient.SelectedItem.ToString() == "Rishikesh")
            {
                DropDownList1.Items.Add("25/04/2012");
            }
        }
    }
}