using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iTextSharp.text;
using CarlosAg.ExcelXmlWriter;

namespace HSRP.Transaction
{
    public partial class BankSalaryReport : System.Web.UI.Page
    {
        int UserType1;
        string CnnString1 = string.Empty;
        string HSRP_StateID1 = string.Empty;
        string RTOLocationID1 = string.Empty;
        string strUserID1 = string.Empty;
        string SQLString1 = string.Empty;

        DataTable dt = new DataTable();
        DataTable dt1 = new DataTable();



        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {
                UserType1 = Convert.ToInt32(Session["UserType"]);
                CnnString1 = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                strUserID1 = Session["UID"].ToString();

                HSRP_StateID1 = Session["UserHSRPStateID"].ToString();
                RTOLocationID1 = Session["UserRTOLocationID"].ToString();
                if (!IsPostBack)
                {

                    try
                    {
                        FilldropDownListCompany();
                    }
                    catch (Exception err)
                    {
                        throw err;
                    }
                }
            }
        }

        #region DropDown
        public void FilldropDownListCompany()
        {
            CnnString1 = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            string  SQLString = "select companyid , CompanyName from Company_Name  where ActiveStatus='Y' Order by CompanyName";
            Utils.PopulateDropDownList(DDlCompany_Name, SQLString.ToString(), CnnString1, "--Select Company Name--");

        }      

        #endregion      
        
        protected void btn_download_Click(object sender, EventArgs e)
        {
          try
            {
                lblerror.Text = "";


                if (DDlCompany_Name.SelectedItem.Text.ToString().Equals("--Select Company Name--"))
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Please Select Company Name.";
                    return;
                }

                if (DDLMonth.SelectedItem.Text.ToString().Equals("--Select Month--"))
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Please Select  Month Name.";
                    return;
                }

                if (ddlyear.SelectedItem.Text.ToString().Equals("--Select Year--"))
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Please Select  Year.";
                    return;
                }

                if (string.IsNullOrEmpty(txtMinAmount.Text.ToString().Trim()))
                {
                    lblerror.Visible = true;
                    lblerror.Text = " Please Enter Minimum Amount.";
                    return;
                }

                if (string.IsNullOrEmpty(txtMaxAmount.Text.ToString().Trim()))
                {
                    lblerror.Visible = true;
                    lblerror.Text = " Please Enetr Maximum Amount.";
                    return;
                }


                #region Fetch Data
                SqlConnection con = new SqlConnection(CnnString1);
                string s = "Detail";
                SqlCommand cmd = new SqlCommand();
                string strParameter = string.Empty;
                cmd = new SqlCommand("BankReportDetail_For_ICICI", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Company_id", Convert.ToInt32(DDlCompany_Name.SelectedValue.ToString()));
                cmd.Parameters.AddWithValue("@Month", Convert.ToInt32(DDLMonth.SelectedValue.ToString()));
                cmd.Parameters.AddWithValue("@year", Convert.ToInt32(ddlyear.SelectedValue.ToString()));
                cmd.Parameters.AddWithValue("@minamt", Convert.ToInt32(txtMinAmount.Text));
                cmd.Parameters.AddWithValue("@maxamt", Convert.ToInt32(txtMaxAmount.Text));             
                cmd.Parameters.AddWithValue("@char", s.ToString());
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                dt.Load(dr);
                con.Close();

              //  DataTable dt1 = new DataTable();
                string s1 = "Totalamount";
                SqlCommand com = new SqlCommand();
                com = new SqlCommand("BankReportDetail_For_ICICI", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Company_id", Convert.ToInt32(DDlCompany_Name.SelectedValue.ToString()));
                com.Parameters.AddWithValue("@Month", Convert.ToInt32(DDLMonth.SelectedValue.ToString()));
                com.Parameters.AddWithValue("@year", Convert.ToInt32(ddlyear.SelectedValue.ToString()));
                com.Parameters.AddWithValue("@minamt", Convert.ToInt32(txtMinAmount.Text));
                com.Parameters.AddWithValue("@maxamt", Convert.ToInt32(txtMaxAmount.Text));
                com.Parameters.AddWithValue("@char", s1.ToString());

                con.Open();
                SqlDataReader drtotal= com.ExecuteReader();
                dt1.Load(drtotal);
                con.Close();

                #endregion

                #region Fetch Data

                if (dt.Rows.Count > 0)
                {
                    string filename = "BankDetailReport_ICICI" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                    Workbook book = new Workbook();

                    // Specify which Sheet should be opened and the size of window by default
                    book.ExcelWorkbook.ActiveSheetIndex = 1;
                    book.ExcelWorkbook.WindowTopX = 100;
                    book.ExcelWorkbook.WindowTopY = 200;
                    book.ExcelWorkbook.WindowHeight = 7000;
                    book.ExcelWorkbook.WindowWidth = 8000;

                    // Some optional properties of the Document
                    book.Properties.Author = "HSRP";
                    book.Properties.Title = "Export Data";
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
                    style3.Alignment.Horizontal = StyleHorizontalAlignment.Center;


                    Worksheet sheet = book.Worksheets.Add("Bank Detail Report");

                    sheet.Table.Columns.Add(new WorksheetColumn(25));
                    sheet.Table.Columns.Add(new WorksheetColumn(100));
                    sheet.Table.Columns.Add(new WorksheetColumn(100));
                    sheet.Table.Columns.Add(new WorksheetColumn(100));
                    sheet.Table.Columns.Add(new WorksheetColumn(100));
                    sheet.Table.Columns.Add(new WorksheetColumn(100));



                    WorksheetRow row = sheet.Table.Rows.Add();


                    //row.Index = 2;
                    //row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                    //row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                    //row.Cells.Add(new WorksheetCell("Rport", "HeaderStyle3"));

                    //row = sheet.Table.Rows.Add();

                    row.Index = 2;
                    //row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                    //row.Cells.Add(new WorksheetCell(" ", "HeaderStyle3"));
                    WorksheetCell cell = row.Cells.Add("DATED :   " + System.DateTime.Now.ToString("dd/MMM/yyyy"));
                    cell.MergeAcross = 4; // Merge two cells together
                    cell.StyleID = "HeaderStyle2";

                    row = sheet.Table.Rows.Add();


                    row.Index = 4;
                    //row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                    //row.Cells.Add(new WorksheetCell(" ", "HeaderStyle3"));
                    WorksheetCell cell1 = row.Cells.Add("TO");
                    cell1.MergeAcross = 4; // Merge two cells together
                    cell1.StyleID = "HeaderStyle2";

                    row = sheet.Table.Rows.Add();

                    row.Index = 5;
                    WorksheetCell cell2 = row.Cells.Add("THE BRANCH MANAGER");
                    cell2.MergeAcross = 4; // Merge two cells together
                    cell2.StyleID = "HeaderStyle2";

                    row = sheet.Table.Rows.Add();

                    row.Index = 6;
                    WorksheetCell cell3 = row.Cells.Add("ICICI BANK, NEHRU PLACE BRANCH', NEW DELHI - 110019");
                    cell3.MergeAcross = 6; // Merge two cells together
                    cell3.StyleID = "HeaderStyle2";

                    row = sheet.Table.Rows.Add();

                    row.Index = 8;
                    WorksheetCell cell4 = row.Cells.Add("Sir,");
                    cell4.MergeAcross = 3; // Merge two cells together
                    cell4.StyleID = "HeaderStyle2";

                    row = sheet.Table.Rows.Add();

                    row.Index = 9;
                    // (One Lac Fourty Three Thousand Four Hundred Thirty Seven  only)
                    WorksheetCell cell5 = row.Cells.Add("  We are issuing a cheque no.    " + System.DateTime.Now.ToString("dd/MM/yyyy") + " amounting Rs." + dt1.Rows[0]["Totalmount"].ToString() + "/- in favor of ICICI.");
                    cell5.MergeAcross = 8; // Merge two cells together
                    cell5.StyleID = "HeaderStyle2";
                    row.AutoFitHeight.ToString();
                    row = sheet.Table.Rows.Add();


                    row.Index = 10;
                    WorksheetCell cell6 = row.Cells.Add("You are requested to debit our account against the above cheque and credit  to following mentioned names.");
                    cell6.MergeAcross = 8; // Merge two cells together
                    cell6.StyleID = "HeaderStyle2";
                    row = sheet.Table.Rows.Add();


                    row.Index = 12;
                    row.Cells.Add(new WorksheetCell("S.No", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Employee Name", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("A/C No.", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Branch Name", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Amount", "HeaderStyle"));


                    //  row = sheet.Table.Rows.Add();
                    String StringField = String.Empty;
                    String StringAlert = String.Empty;

                    // row.Index = 12;
                    string RTOColName = string.Empty;
                    int sno = 0;
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dtrows in dt.Rows) // Loop over the rows.
                        {
                            sno = sno + 1;
                            row = sheet.Table.Rows.Add();
                            row.Cells.Add(new WorksheetCell(sno.ToString(), DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell(dtrows["Name"].ToString(), DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell(dtrows["AccountNo"].ToString(), DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell(dtrows["BranchName"].ToString(), DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell(dtrows["Net_salary"].ToString(), DataType.String, "HeaderStyle1"));

                        }

                        row = sheet.Table.Rows.Add();

                        int i = dt.Rows.Count + 12 + 1;
                        row.Index = i;
                        row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                        row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                        row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                        row.Cells.Add(new WorksheetCell("Total :", "HeaderStyle3"));
                        row.Cells.Add(new WorksheetCell("" + dt1.Rows[0]["Totalmount"].ToString() + "/-", "HeaderStyle3"));
                        row = sheet.Table.Rows.Add();


                        i = i + 2;
                        row.Index = i;
                        WorksheetCell cell7 = row.Cells.Add("Thanking You");
                        cell7.MergeAcross = 4; // Merge two cells together
                        cell.StyleID = "HeaderStyle2";
                        row = sheet.Table.Rows.Add();


                        row.Index = i + 1;
                        WorksheetCell cell8 = row.Cells.Add(DDlCompany_Name.SelectedItem.Text.ToString() + ".");
                        cell8.MergeAcross = 4; // Merge two cells together
                        cell.StyleID = "HeaderStyle2";
                        row = sheet.Table.Rows.Add();


                        row.Index = i + 4;
                        WorksheetCell cell9 = row.Cells.Add("Authorised Signatory");
                        cell9.MergeAcross = 5; // Merge two cells together
                        cell.StyleID = "HeaderStyle2";
                        row = sheet.Table.Rows.Add();





                        HttpContext context = HttpContext.Current;
                        context.Response.Clear();
                        book.Save(Response.OutputStream);
                        context.Response.ContentType = "application/vnd.ms-excel";
                        context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                        context.Response.End();
                    }
                }
                #endregion
                else
                {
 
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btn_detail_NonICIClick(object sender, EventArgs e)
        {
            try
            {
                lblerror.Text = "";


                if (DDlCompany_Name.SelectedItem.Text.ToString().Equals("--Select Company Name--"))
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Please Select Company Name.";
                    return;
                }

                if (DDLMonth.SelectedItem.Text.ToString().Equals("--Select Month--"))
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Please Select  Month Name.";
                    return;
                }

                if (ddlyear.SelectedItem.Text.ToString().Equals("--Select Year--"))
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Please Select  Year.";
                    return;
                }

                if (string.IsNullOrEmpty(txtMinAmount.Text.ToString().Trim()))
                {
                    lblerror.Visible = true;
                    lblerror.Text = " Please Enter Minimum Amount.";
                    return;
                }

                if (string.IsNullOrEmpty(txtMaxAmount.Text.ToString().Trim()))
                {
                    lblerror.Visible = true;
                    lblerror.Text = " Please Enetr Maximum Amount.";
                    return;
                }
            



                #region Fetch Data
                SqlConnection con = new SqlConnection(CnnString1);
                string s = "Detail";
                SqlCommand cmd = new SqlCommand();
                string strParameter = string.Empty;
                cmd = new SqlCommand("BankReportDetail_For_NON_ICICI", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Company_id", Convert.ToInt32(DDlCompany_Name.SelectedValue.ToString()));
                cmd.Parameters.AddWithValue("@Month", Convert.ToInt32(DDLMonth.SelectedValue.ToString()));
                cmd.Parameters.AddWithValue("@year", Convert.ToInt32(ddlyear.SelectedValue.ToString()));
                cmd.Parameters.AddWithValue("@minamt", Convert.ToInt32(txtMinAmount.Text));
                cmd.Parameters.AddWithValue("@maxamt", Convert.ToInt32(txtMaxAmount.Text));
                cmd.Parameters.AddWithValue("@char", s.ToString());
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                dt.Load(dr);
                con.Close();



 
               

                #endregion
                if (dt.Rows.Count > 0)
                {
                    string filename = "BankDetailReport_NONICICI" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                    Workbook book = new Workbook();

                    // Specify which Sheet should be opened and the size of window by default
                    book.ExcelWorkbook.ActiveSheetIndex = 1;
                    book.ExcelWorkbook.WindowTopX = 100;
                    book.ExcelWorkbook.WindowTopY = 200;
                    book.ExcelWorkbook.WindowHeight = 7000;
                    book.ExcelWorkbook.WindowWidth = 8000;

                    // Some optional properties of the Document
                    book.Properties.Author = "HSRP";
                    book.Properties.Title = "Export Data";
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
                    style3.Alignment.Horizontal = StyleHorizontalAlignment.Center;


                    //WorksheetStyle styletotal = book.Styles.Add("HeaderStyleTotal");
                    //styletotal.Font.FontName = "Tahoma";
                    //styletotal.Font.Size = 12;
                    //styletotal.Font.Bold = true;
                    //styletotal.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                    


                   


                    Worksheet sheet = book.Worksheets.Add("Bank Detail Report");

                    sheet.Table.Columns.Add(new WorksheetColumn(25));
                    sheet.Table.Columns.Add(new WorksheetColumn(40));
                    sheet.Table.Columns.Add(new WorksheetColumn(40));
                    sheet.Table.Columns.Add(new WorksheetColumn(40));
                    sheet.Table.Columns.Add(new WorksheetColumn(100));
                    sheet.Table.Columns.Add(new WorksheetColumn(100));
                    sheet.Table.Columns.Add(new WorksheetColumn(100));
                    sheet.Table.Columns.Add(new WorksheetColumn(100));
                    sheet.Table.Columns.Add(new WorksheetColumn(100));
                    sheet.Table.Columns.Add(new WorksheetColumn(100));
                    sheet.Table.Columns.Add(new WorksheetColumn(100));
                    sheet.Table.Columns.Add(new WorksheetColumn(100));
                    sheet.Table.Columns.Add(new WorksheetColumn(100));
                    sheet.Table.Columns.Add(new WorksheetColumn(100));
                    sheet.Table.Columns.Add(new WorksheetColumn(100));
                    sheet.Table.Columns.Add(new WorksheetColumn(100));
                


                    WorksheetRow row = sheet.Table.Rows.Add();

                     //row.Index = 2;
                    //row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                    //row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                    //row.Cells.Add(new WorksheetCell("Rport", "HeaderStyle3"));
                    //row = sheet.Table.Rows.Add();

                    row.Index = 2;
                     WorksheetCell cell = row.Cells.Add("DATED :   " + System.DateTime.Now.ToString("dd/MMM/yyyy"));
                    cell.MergeAcross = 4; // Merge two cells together
                    cell.StyleID = "HeaderStyle2";

                    row = sheet.Table.Rows.Add();


                    row.Index = 4;
                    
                    WorksheetCell cell1 = row.Cells.Add("TO");
                    cell1.MergeAcross = 4; // Merge two cells together
                    cell1.StyleID = "HeaderStyle2";

                    row = sheet.Table.Rows.Add();

                    row.Index = 5;
                    WorksheetCell cell2 = row.Cells.Add("THE BRANCH MANAGER");
                    cell2.MergeAcross = 4; // Merge two cells together
                    cell2.StyleID = "HeaderStyle2";

                    row = sheet.Table.Rows.Add();

                    row.Index = 6;
                    WorksheetCell cell3 = row.Cells.Add("ICICI Bank, Nehru Place, New Delhi--19");
                    cell3.MergeAcross = 5; // Merge two cells together
                    cell3.StyleID = "HeaderStyle2";

                    row = sheet.Table.Rows.Add();



                    row.Index = 8;
                    WorksheetCell cellsub = row.Cells.Add("Sub  : Transfer of Funds from Current Account 629405042485");
                    cellsub.MergeAcross = 5; // Merge two cells together
                    cellsub.StyleID = "HeaderStyle2";
                    row = sheet.Table.Rows.Add();                    			



                    row.Index = 9;
                    WorksheetCell cell4 = row.Cells.Add("Dear Sir,");
                    cell4.MergeAcross = 3; // Merge two cells together
                    cell4.StyleID = "HeaderStyle2";
                    row = sheet.Table.Rows.Add();

                    ////////////////////////totalamt
                    //DataTable dt1 = new DataTable();
                    string s1 = "Totalamount";
                    SqlCommand com = new SqlCommand();
                    com = new SqlCommand("BankReportDetail_For_NON_ICICI", con);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@Company_id", Convert.ToInt32(DDlCompany_Name.SelectedValue.ToString()));
                    com.Parameters.AddWithValue("@Month", Convert.ToInt32(DDLMonth.SelectedValue.ToString()));
                    com.Parameters.AddWithValue("@year", Convert.ToInt32(ddlyear.SelectedValue.ToString()));
                    com.Parameters.AddWithValue("@minamt", Convert.ToInt32(txtMinAmount.Text));
                    com.Parameters.AddWithValue("@maxamt", Convert.ToInt32(txtMaxAmount.Text));
                    com.Parameters.AddWithValue("@char", s1.ToString());
                    con.Open();
                    SqlDataReader dr1 = com.ExecuteReader();
                    dt1.Load(dr1);
                    con.Close();
                    /////////////////////////////////////////

                   

                    row.Index = 10;
                    // (One Lac Fourty Three Thousand Four Hundred Thirty Seven  only)
                    WorksheetCell cell5 = row.Cells.Add("Please find enclosed herewith Cheque no.    Dated" + System.DateTime.Now.ToString("dd/MM/yyyy") + " for Rs." + dt1.Rows[0]["Totalmount"].ToString() + "/-  only.");
                    cell5.MergeAcross = 12; // Merge two cells together
                    cell5.StyleID = "HeaderStyle2";
                   
                    row = sheet.Table.Rows.Add();


                   

                    row.Index = 12;
                    WorksheetCell cell6 = row.Cells.Add("You are requested to transfer Funds to the following given Account Numbers :");
                    cell6.MergeAcross = 8; // Merge two cells together
                    cell6.StyleID = "HeaderStyle2";
                    row = sheet.Table.Rows.Add();


                    row.Index = 14;
                    WorksheetCell cell14 = row.Cells.Add("For Non- ICICI Bank Accounts");
                    cell14.MergeAcross = 5; // Merge two cells together
                    cell14.StyleID = "HeaderStyle2";
                    row = sheet.Table.Rows.Add();


                    row.Index = 16;

                    row.Cells.Add(new WorksheetCell("S.No", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Amount", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Senactype", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Senaccno", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("SenName", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("SMS EML", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Detail", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("OoR7002", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("BenIFSC", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Benactype", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("BenAccno", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("BenAccName", "HeaderStyle"));

                    //  row = sheet.Table.Rows.Add();

                    String StringField = String.Empty;
                    String StringAlert = String.Empty;


                    string RTOColName = string.Empty;
                    int sno = 0;
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dtrows in dt.Rows) // Loop over the rows.
                        {
                            sno = sno + 1;
                            row = sheet.Table.Rows.Add();
                            row.Cells.Add(new WorksheetCell(sno.ToString(), DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell(dtrows["Net_salary"].ToString(), DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell("11", DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell("629405042485", DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell(DDlCompany_Name.SelectedItem.Text.ToString(), DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell("finance@linkutsav.com", DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell(DDlCompany_Name.SelectedItem.Text.ToString(), DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell(dtrows["ifscode"].ToString(), DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell("11", DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell(dtrows["accountno"].ToString(), DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell(dtrows["Name"].ToString(), DataType.String, "HeaderStyle1"));


                        }

                        row = sheet.Table.Rows.Add();


                        int i = dt.Rows.Count + 16 + 1;
                        row.Index = i;
                        row.Cells.Add(new WorksheetCell("Total :", "HeaderStyle3"));
                        row.Cells.Add(new WorksheetCell("" + dt1.Rows[0]["Totalmount"].ToString() + "/-", "HeaderStyle3"));
                        row = sheet.Table.Rows.Add();


                        i = i + 2;
                        row.Index = i;
                        WorksheetCell cell7 = row.Cells.Add("Thanking You");
                        cell7.MergeAcross = 3; // Merge two cells together
                        cell7.StyleID = "HeaderStyle2";
                        row = sheet.Table.Rows.Add();

                        row.Index = i + 1;
                        WorksheetCell cell8 = row.Cells.Add("With Regards");
                        cell8.MergeAcross = 3; // Merge two cells together
                        cell8.StyleID = "HeaderStyle2";
                        row = sheet.Table.Rows.Add();



                        row.Index = i + 2;
                        WorksheetCell cell18 = row.Cells.Add(DDlCompany_Name.SelectedItem.Text.ToString() + ".");
                        cell18.MergeAcross = 4; // Merge two cells together
                        cell18.StyleID = "HeaderStyle2";
                        row = sheet.Table.Rows.Add();


                        row.Index = i + 4;
                        WorksheetCell cell9 = row.Cells.Add("Authorised Signatory");
                        cell9.MergeAcross = 5; // Merge two cells together
                        cell9.StyleID = "HeaderStyle2";
                        row = sheet.Table.Rows.Add();


                        HttpContext context = HttpContext.Current;
                        context.Response.Clear();
                        book.Save(Response.OutputStream);
                        context.Response.ContentType = "application/vnd.ms-excel";
                        context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                        context.Response.End();
                    }

                    else
                    {

 
                    }


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

          

        }
    }
}