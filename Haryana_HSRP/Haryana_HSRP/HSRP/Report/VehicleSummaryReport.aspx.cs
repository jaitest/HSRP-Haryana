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
    public partial class VehicleSummaryReport : System.Web.UI.Page
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
                        
                    }
                }
            }
        }
        
        private Boolean validate1()
        {
            Boolean blnvalid = true;
            if (ddlMonth.SelectedItem.ToString() == "--Select Month--")
            {
                blnvalid = false;
                Label1.Text = "Please select Month";
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

                        string filename = "Vehicle_Summary_Report" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                        Workbook book = new Workbook();

                        // Specify which Sheet should be opened and the size of window by default
                        book.ExcelWorkbook.ActiveSheetIndex = 1;
                        book.ExcelWorkbook.WindowTopX = 100;
                        book.ExcelWorkbook.WindowTopY = 200;
                        book.ExcelWorkbook.WindowHeight = 7000;
                        book.ExcelWorkbook.WindowWidth = 8000;

                        // Some optional properties of the Document
                        book.Properties.Author = "HSRP";
                        if (ddlMonth.SelectedItem.Text == "ALL")
                        {
                            book.Properties.Title = "VEHICLE WISE ORDER BOOKING " + ddlReport.SelectedValue.ToString().ToUpper() + " FROM FEB.12 TO JUN.13";
                        }
                        else
                        {
                            book.Properties.Title = "VEHICLE WISE ORDER BOOKING " + ddlReport.SelectedValue.ToString().ToUpper() + " OF MONTH " + ddlMonth.SelectedItem.Text;
                        }
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
                        style8.Alignment.Horizontal = StyleHorizontalAlignment.Left;
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

                        Worksheet sheet = book.Worksheets.Add("VEHICLE WISE ORDER BOOKING");
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
                        if (ddlMonth.SelectedItem.Text == "ALL")
                        {
                            WorksheetCell cell = row.Cells.Add("VEHICLE WISE ORDER BOOKING " + ddlReport.SelectedValue.ToString().ToUpper() + " FORM FEB.12 TO JUN.13");
                            cell.MergeAcross = 3; // Merge two cells together
                            cell.StyleID = "HeaderStyle8";
                        }
                        else
                        {
                            WorksheetCell cell = row.Cells.Add("VEHICLE WISE ORDER BOOKING " + ddlReport.SelectedValue.ToString().ToUpper() + " OF  MONTH " + ddlMonth.SelectedItem.Text);
                            cell.MergeAcross = 3; // Merge two cells together
                            cell.StyleID = "HeaderStyle8";
                        }
                        row = sheet.Table.Rows.Add();

                        row.Index = 3;
                        //row.Cells.Add("Order Date");
                        row.Cells.Add(new WorksheetCell("Month/Year", "HeaderStyle6"));
                        //row.Cells.Add(new WorksheetCell("Order Month", "HeaderStyle6"));
                        row.Cells.Add(new WorksheetCell("MOTOR CYCLE", "HeaderStyle6"));
                        row.Cells.Add(new WorksheetCell("SCOOTER", "HeaderStyle6"));
                        row.Cells.Add(new WorksheetCell("TRACTOR", "HeaderStyle6"));
                        row.Cells.Add(new WorksheetCell("THREE WHEELER", "HeaderStyle6"));
                        row.Cells.Add(new WorksheetCell("LMV(CLASS)", "HeaderStyle6"));
                        row.Cells.Add(new WorksheetCell("LMC", "HeaderStyle6"));
                        row.Cells.Add(new WorksheetCell("MCV", "HeaderStyle6"));
                        //row.Cells.Add(new WorksheetCell("Close Date", "HeaderStyle6"));
                        row.Cells.Add(new WorksheetCell("GRAND TOTAL", "HeaderStyle6"));
                        //row.Cells.Add(new WorksheetCell("Delay in No. of Days", "HeaderStyle"));


                        row = sheet.Table.Rows.Add();

                        row.Index = 4;
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
                        row.Index = 5;
                        DataTable dt;
                        int Motor = 0;
                        int scooter = 0;
                        int tractor = 0;
                        int Three = 0;
                        int LMVCLass = 0;
                        int LMV = 0;
                        int MCV = 0;
                        int Grand = 0;
                        if (ddlMonth.SelectedItem.Text == "ALL")
                        {
                            SQLString = "Select * from TimeVehicleSummary order By ID";
                            dt = Utils.GetDataTable(SQLString, CnnString);
                        }
                        else
                        {
                            SQLString = "Select * from TimeVehicleSummary where Month='" + ddlMonth.SelectedItem.ToString() + "' order by ID";
                            dt = Utils.GetDataTable(SQLString, CnnString);
                        }
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dtrows in dt.Rows) // Loop over the rows.
                            {
                                if (ddlReport.SelectedItem.ToString() == "Count Wise")
                                {
                                    row.Cells.Add(new WorksheetCell(dtrows["month"].ToString(), DataType.String, "HeaderStyle"));
                                    row.Cells.Add(new WorksheetCell(dtrows["MotorCycle_Count"].ToString(), DataType.String, "HeaderStyle"));
                                    row.Cells.Add(new WorksheetCell(dtrows["Scooter_Count"].ToString(), DataType.String, "HeaderStyle"));
                                    row.Cells.Add(new WorksheetCell(dtrows["Tractor_Count"].ToString(), DataType.String, "HeaderStyle"));
                                    row.Cells.Add(new WorksheetCell(dtrows["ThreeWheeler_Count"].ToString(), DataType.String, "HeaderStyle"));
                                    row.Cells.Add(new WorksheetCell(dtrows["LMVClass_Count"].ToString(), DataType.String, "HeaderStyle"));
                                    row.Cells.Add(new WorksheetCell(dtrows["LMV_Count"].ToString(), DataType.String, "HeaderStyle"));
                                    row.Cells.Add(new WorksheetCell(dtrows["MCV_Count"].ToString(), DataType.String, "HeaderStyle"));
                                    int Sum = int.Parse(dtrows["MotorCycle_Count"].ToString()) + int.Parse(dtrows["MotorCycle_Count"].ToString()) + int.Parse(dtrows["Scooter_Count"].ToString()) + int.Parse(dtrows["Tractor_Count"].ToString()) + int.Parse(dtrows["ThreeWheeler_Count"].ToString()) + int.Parse(dtrows["LMVClass_Count"].ToString()) + int.Parse(dtrows["LMV_Count"].ToString()) + int.Parse(dtrows["MCV_Count"].ToString());
                                    row.Cells.Add(new WorksheetCell(dtrows["Count_Total"].ToString(), DataType.String, "HeaderStyle"));
                                    Motor = Motor + int.Parse(dtrows["MotorCycle_Count"].ToString());
                                    scooter = scooter + int.Parse(dtrows["Scooter_Count"].ToString());
                                    tractor = tractor + int.Parse(dtrows["Tractor_Count"].ToString());
                                    Three = Three + int.Parse(dtrows["ThreeWheeler_Count"].ToString());
                                    LMVCLass = LMVCLass + int.Parse(dtrows["LMVClass_Count"].ToString());
                                    LMV = LMV + int.Parse(dtrows["LMV_Count"].ToString());
                                    MCV = MCV + int.Parse(dtrows["MCV_Count"].ToString());
                                    Grand = Grand + int.Parse(dtrows["Count_Total"].ToString());
                                }
                                else
                                {
                                    row.Cells.Add(new WorksheetCell(dtrows["month"].ToString(), DataType.String, "HeaderStyle"));
                                    row.Cells.Add(new WorksheetCell(dtrows["MotorCycle_Value"].ToString(), DataType.String, "HeaderStyle"));
                                    row.Cells.Add(new WorksheetCell(dtrows["Scooter_Value"].ToString(), DataType.String, "HeaderStyle"));
                                    row.Cells.Add(new WorksheetCell(dtrows["Tractor_Value"].ToString(), DataType.String, "HeaderStyle"));
                                    row.Cells.Add(new WorksheetCell(dtrows["ThreeWheeler_Value"].ToString(), DataType.String, "HeaderStyle"));
                                    row.Cells.Add(new WorksheetCell(dtrows["LMVClass_Value"].ToString(), DataType.String, "HeaderStyle"));
                                    row.Cells.Add(new WorksheetCell(dtrows["LMV_Value"].ToString(), DataType.String, "HeaderStyle"));
                                    row.Cells.Add(new WorksheetCell(dtrows["MCV_Value"].ToString(), DataType.String, "HeaderStyle"));
                                    int Sum = int.Parse(dtrows["MotorCycle_Value"].ToString()) + int.Parse(dtrows["MotorCycle_Value"].ToString()) + int.Parse(dtrows["Scooter_Value"].ToString()) + int.Parse(dtrows["Tractor_Value"].ToString()) + int.Parse(dtrows["ThreeWheeler_Value"].ToString()) + int.Parse(dtrows["LMVClass_Value"].ToString()) + int.Parse(dtrows["LMV_Value"].ToString()) + int.Parse(dtrows["MCV_Value"].ToString());
                                    row.Cells.Add(new WorksheetCell(dtrows["Value_Total"].ToString(), DataType.String, "HeaderStyle"));
                                    Motor = Motor + int.Parse(dtrows["MotorCycle_Value"].ToString());
                                    scooter = scooter + int.Parse(dtrows["Scooter_Value"].ToString());
                                    tractor = tractor + int.Parse(dtrows["Tractor_Value"].ToString());
                                    Three = Three + int.Parse(dtrows["ThreeWheeler_Value"].ToString());
                                    LMVCLass = LMVCLass + int.Parse(dtrows["LMVClass_Value"].ToString());
                                    LMV = LMV + int.Parse(dtrows["LMV_Value"].ToString());
                                    MCV = MCV + int.Parse(dtrows["MCV_Value"].ToString());
                                    Grand = Grand + int.Parse(dtrows["Value_Total"].ToString());
                                }


                                row = sheet.Table.Rows.Add();
                            }

                            row.Cells.Add(new WorksheetCell("Total", DataType.String, "HeaderStyle"));
                            row.Cells.Add(new WorksheetCell(Motor.ToString(), DataType.String, "HeaderStyle"));
                            row.Cells.Add(new WorksheetCell(scooter.ToString(), DataType.String, "HeaderStyle"));
                            row.Cells.Add(new WorksheetCell(tractor.ToString(), DataType.String, "HeaderStyle"));
                            row.Cells.Add(new WorksheetCell(Three.ToString(), DataType.String, "HeaderStyle"));
                            row.Cells.Add(new WorksheetCell(LMVCLass.ToString(), DataType.String, "HeaderStyle"));
                            row.Cells.Add(new WorksheetCell(LMV.ToString(), DataType.String, "HeaderStyle"));
                            row.Cells.Add(new WorksheetCell(MCV.ToString(), DataType.String, "HeaderStyle"));
                            row.Cells.Add(new WorksheetCell(Grand.ToString(), DataType.String, "HeaderStyle"));

                            row = sheet.Table.Rows.Add();

                            if (ddlMonth.SelectedItem.Text == "ALL")
                            {
                                WorksheetCell cell2 = row.Cells.Add("Note:" + "VEHICLE WISE ORDER BOOKING " + ddlReport.SelectedValue.ToString().ToUpper() + " FORM FEB.12 TO JUN.13");
                                cell2.MergeAcross = 8; // Merge two cells together
                                cell2.StyleID = "HeaderStyle8";
                            }
                            else
                            {

                                WorksheetCell cell2 = row.Cells.Add("Note:" + "VEHICLE WISE ORDER BOOKING "+ddlReport.SelectedValue.ToString().ToUpper()+" OF MONTH " + ddlMonth.SelectedItem.Text);
                                cell2.MergeAcross = 8; // Merge two cells together
                                cell2.StyleID = "HeaderStyle8";
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
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }
    }
}