using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using HSRP;
using System.Data;
using DataProvider;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Configuration;
using System.IO;
using CarlosAg.ExcelXmlWriter;
using System.Drawing;
using System;
using System.Collections;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;



namespace HSRP.BusinessReports
{
    public partial class CustomizedProdSheetDealer : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        string UserType = string.Empty;
        string HSRPStateID = string.Empty;
        string RTOLocationID = string.Empty;
        int intHSRPStateID;
        string SQlQuery = string.Empty;
        string ExicseAmount = string.Empty;
        string AllLocation = string.Empty;
        string OrderStatus = string.Empty;
        string strStateId = string.Empty;
       
       
        string StrSqlQuery = string.Empty;
        string strRunningNo = string.Empty;
        string strEmbId=string.Empty;
       
      
        DataProvider.BAL bl = new DataProvider.BAL();
        BAL obj = new BAL();
        string StickerManditory = string.Empty;
        DataTable dataSetFillHSRPRecord;

       

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
                        }
                        else
                        {
                            hiddenUserType.Value = "1";
                            labelOrganization.Enabled = false;
                            DropDownListStateName.Enabled = false;
                            labelClient.Enabled = false;
                            FilldropDownListClient();
                            FilldropDownListOrganization();
                            //buildGrid();
                        }

                        //ShowGrid();
                    }
                    catch (Exception err)
                    {
                        lblErrMsg.Text = "Error on Page Load" + err.Message.ToString();
                    }
                }
            }
        }

        #region DropDown

        public void FileDetail()
        {

        }

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

                dataLabellbl.Visible = false;
                TRRTOHide.Visible = false;
            }
            else
            {
                string UserID = Convert.ToString(Session["UID"]);

                SQLString = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, (a.RTOLocationCode+', ') as RTOCode, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID and a.activestatus='Y' where UserRTOLocationMapping.UserID='" + UserID + "' order by a.rtolocationname ";
                DataTable dss = Utils.GetDataTable(SQLString, CnnString);

                labelOrganization.Visible = true;
                DropDownListStateName.Visible = true;
                labelClient.Visible = true;
                dropDownListClient.Visible = true;

                dropDownListClient.DataSource = dss;
                dropDownListClient.DataBind();
                dataLabellbl.Visible = true;

                string RTOCode = string.Empty;
                if (dss.Rows.Count > 0)
                {
                    for (int i = 0; i <= dss.Rows.Count - 1; i++)
                    {
                        RTOCode += dss.Rows[i]["RTOCode"].ToString();

                    }
                    lblRTOCode.Text = RTOCode.Remove(RTOCode.LastIndexOf(","));

                }
            }
        }

        #endregion

        DataTable dtInvoiceData = null;
        StringBuilder sb = new StringBuilder();
        CheckBox chk;
        DataTable dt = new DataTable();
        string strInvoiceNo = null;
        private void GetRecords()
        {
            strInvoiceNo = string.Empty;
            dtInvoiceData = new DataTable();
            if (!string.IsNullOrEmpty(txtInvoiceNo.Text))
            {
                string SQLString = "select row_number() over(order by VehicleClass,VehicleType,VehicleRegNo) as SN,ChallanNo,challandate,convert(varchar,HSRPRecord_CreationDate,103) as HSRPRecord_CreationDate,VehicleType,convert(varchar,OrderEmbossingDate,103) as OrderEmbossingDate,hsrp_stateid,rtolocationid" +
                                    ",hsrprecordid,vehicleregno,hsrp_rear_lasercode,hsrp_Front_lasercode,OrderStatus " +
                                    "from hsrprecords where hsrp_stateid='" + DropDownListStateName.SelectedValue.ToString() + "' and [ChallanNo]='" + txtInvoiceNo.Text + "' order by VehicleClass,VehicleType,VehicleRegNo";
                
                dtInvoiceData = Utils.GetDataTable(SQLString, CnnString);
                strInvoiceNo = txtInvoiceNo.Text;
            }
            else if (!dropdownDuplicateFIle.SelectedItem.Text.Equals("--Select PS No--"))
            {
                string SQLString = "select row_number() over(order by VehicleClass,VehicleType,VehicleRegNo) AS SN,newpdfrunningno,orderdate,convert(varchar,HSRPRecord_CreationDate,103) as HSRPRecord_CreationDate,VehicleType,convert(varchar,OrderEmbossingDate,103) as OrderEmbossingDate,hsrp_stateid,rtolocationid" +
                                    ",hsrprecordid,vehicleregno,hsrp_rear_lasercode,hsrp_Front_lasercode,OrderStatus " +
                                    "from hsrprecords where [newpdfrunningno]='" + dropdownDuplicateFIle.SelectedItem + "' order by VehicleClass,VehicleType,VehicleRegNo";
                dtInvoiceData = Utils.GetDataTable(SQLString, CnnString);
                strInvoiceNo = dropdownDuplicateFIle.SelectedItem.Text;
            }
            else
            {
                return;
            }

           
                if (dtInvoiceData.Rows.Count > 0)
                {
                   
                    btnChalan.Visible = true;
                   
                  //  Button2.Visible = true;
                   
                }
                else
                {
                    btnChalan.Visible = false;
                   
                  //  Button2.Visible = false;
                    lblErrMsg.Text = "Record Not Found";
                    

                }

           
        }


       
        protected void DropDownListStateName_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilldropDownListClient();
            dropDownListClient.Visible = true;
            labelClient.Visible = true;

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

        protected void btnGO_Click(object sender, EventArgs e)
        {
            txtInvoiceNo.Text = string.Empty;
            try
            {                
                    labelOrderStatus.Visible = true;
                    dropdownDuplicateFIle.Visible = true;

                    string type = "1";

                    string strDate = OrderDate.SelectedDate.ToString("dd/MM/yyyy");
                    String[] StringAuthDate = strDate.Replace("-", "/").Split('/');
                    string MonTo = ("0" + StringAuthDate[0]);
                    string MonthdateTO = MonTo.Replace("00", "0").Replace("01", "1");
                    String ReportDateFrom = StringAuthDate[1] + "/" + MonthdateTO + "/" + StringAuthDate[2].Split(' ')[0];
                    String From = StringAuthDate[0] + "/" + StringAuthDate[1] + "/" + StringAuthDate[2].Split(' ')[0];
                    //AuthorizationDate = new DateTime(Convert.ToInt32(StringAuthDate[2].Split(' ')[0]), Convert.ToInt32(StringAuthDate[0]), Convert.ToInt32(StringAuthDate[1]));
                    string AuthorizationDate = From + " 00:00:00";
                    String[] StringOrderDate = HSRPAuthDate.SelectedDate.ToString().Replace("-", "/").Split('/');
                    string Mon = ("0" + StringOrderDate[0]);
                    string Monthdate = Mon.Replace("00", "0").Replace("01", "1");
                    String FromDate = StringOrderDate[1] + "-" + Monthdate + "-" + StringOrderDate[2].Split(' ')[0];
                    String ReportDateTo = StringOrderDate[1] + "/" + Monthdate + "/" + StringOrderDate[2].Split(' ')[0];
                    String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
                    string ToDate = From1 + " 23:59:59";


                    string strFrmDateString = OrderDate.SelectedDate.ToShortDateString() + " 00:00:00";
                    string strToDateString = HSRPAuthDate.SelectedDate.ToShortDateString() + " 23:59:59";

                    SQLString = "select  Distinct newpdfrunningno   from hsrprecords  where hsrp_StateID='" + DropDownListStateName.SelectedValue +
                        "' and rtolocationID='" + dropDownListClient.SelectedValue + "' and  newpdfrunningno is not null and erpassigndate between '" +
                        strFrmDateString + "' and  '" + strToDateString + "' ";


                
                labelOrderStatus.Visible = true;
                dropdownDuplicateFIle.Visible = true;
                    Utils.PopulateDropDownList(dropdownDuplicateFIle, SQLString.ToString(), CnnString, "--Select PS No--");
               
                
            }
            catch (Exception ex)
            {
                lblErrMsg.Text = ex.Message;
            }

        }
        
        protected void dropdownDuplicateFIle_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGrid();
        }

        private void BindGrid()
        {
            GetRecords();
            if (dtInvoiceData.Rows.Count > 0)
            {
               
                btnChalan.Visible = true;
                
            }
            else
            {
                btnChalan.Visible = false;
               
            }
        }
        

        protected void btnChalan_Click(object sender, EventArgs e)
        {
            try
            {
                #region Validation
             //   Button2.Visible = true;
                if (string.IsNullOrEmpty(txtInvoiceNo.Text) && (dropdownDuplicateFIle.SelectedIndex<=0))
                {
                    Response.Write("<script> alert('Please Enter PS No.')</script>");
                    return;
                }
               
                #endregion

                 string strStateID = DropDownListStateName.SelectedValue.ToString();
            string RTOLocationID = dropDownListClient.SelectedValue.ToString();
            
            if (strStateID == "--Select State--")
            {
                lblErrMsg.Visible = true;
                lblErrMsg.Text = "";
                lblErrMsg.Text = "Please Select State.";
                return;
            }
            if (RTOLocationID == "--Select RTO Name--")
            {
                lblErrMsg.Visible = true;
                lblErrMsg.Text = "";
                lblErrMsg.Text = "Please Select RTO Location.";
                return;
            }
            lblErrMsg.Text = "";
            
            BAL obj = new BAL();
         
            String[] StringAuthDate = OrderDate.SelectedDate.ToString().Split('/');
            string MonTo = ("0" + StringAuthDate[0]);
            string MonthdateTO = MonTo.Replace("00", "0").Replace("01", "1");
            String ReportDateFrom = StringAuthDate[1] + "/" + MonthdateTO + "/" + StringAuthDate[2].Split(' ')[0];
            String From = StringAuthDate[0] + "/" + StringAuthDate[1] + "/" + StringAuthDate[2].Split(' ')[0];

            string AuthorizationDate = From + " 00:00:00";
            String[] StringOrderDate = HSRPAuthDate.SelectedDate.ToString().Split('/');
            string Mon = ("0" + StringOrderDate[0]);
            string Monthdate = Mon.Replace("00", "0").Replace("01", "1");
            String FromDate = StringOrderDate[1] + "-" + Monthdate + "-" + StringOrderDate[2].Split(' ')[0];
            String ReportDateTo = StringOrderDate[1] + "/" + Monthdate + "/" + StringOrderDate[2].Split(' ')[0];
            String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
           // string From1 = "11/28/14";
            string ToDate = From1 + " 23:59:59";

          
            string from2 = From1 + " 00:00:00";
            string from3 = From + " 00:00:00";
            
            string strVehicleType = string.Empty;
            StringBuilder strSQL = new StringBuilder();
            
            StrSqlQuery = "select NAVEMBID from rtolocation where hsrp_stateid='" + strStateID + "' and RtolocationId='" + RTOLocationID + "'";
            strEmbId = Utils.getScalarValue(StrSqlQuery, CnnString);
            



            string strSel = "select isnull(max(right(ProductionSheetRunningNo,7)),0000000) from EmbossingCenters where State_Id='" + DropDownListStateName.SelectedValue.ToString() + "' and Emb_Center_Id='" + strEmbId + "'";
            string strCom = Utils.getScalarValue(strSel, CnnString);

            string strPRFIX = "select PrefixText from EmbossingCenters where State_Id='" + DropDownListStateName.SelectedValue.ToString() + "' and Emb_Center_Id='" + strEmbId + "'";
            string strPRFIXCom = Utils.getScalarValue(strPRFIX, CnnString);
            

            if (strCom.Equals(0))
            {
                strRunningNo = "0000001";
            }
            else
            {
                strRunningNo = string.Format("{0:0000000}", Convert.ToInt32(strCom) + 1);
            }







            //strSQL.Append("Select a.hsrprecordID,a.roundoff_netamount,a.OrderStatus,a.OrderClosedDate,  a.OrderEmbossingDate,  a.aptgvehrecdate, a.HSRPRecord_AuthorizationNo,convert(varchar, OrderClosedDate, 105) as OrderClosedDate, CONVERT(varchar(20),orderdate ,103) AS OrderBookDate,CONVERT(varchar(20),OrderEmbossingDate ,103) AS OrderEmbossingDate, a.dealerid as ID, left(a.OwnerName,19) as OwnerName, a.MobileNo,(select  AffixCenterDesc from AffixationCenters where Affix_id= a.affix_id ) as AffixCenterDesc, CONVERT(varchar(20), HSRPRecord_AuthorizationDate,103) AS OrderDateAuth, a.OrderDate, a.EngineNo, a.ChassisNo, (select rtolocationname from rtolocation where  rtolocationid =a.rtolocationid) as RTOLocationName,  a.VehicleRegNo,  case a.VehicleType when 'MCV/HCV/TRAILERS' then 'Trailers' when 'THREE WHEELER' then 'T.Whe.' when 'SCOOTER' then 'SCOO' when 'TRACTOR' then 'TRAC' when 'LMV(CLASS)' then 'L.CL' when 'LMV' then 'LMV'  when 'MOTOR CYCLE' then 'MO.C' end as VehicleType, case a.VehicleClass when 'Transport' then 'T' else 'N.T.' end as VehicleClass,  a.HSRP_StateID, (select HSRPStateName from hsrpstate where HSRP_StateID=a.HSRP_StateID)  as HSRPStateName, (select replace(ProductCode,'MM-','') from Product where productid= a.RearPlateSize) AS RearProductCode,(select replace(ProductCode,'MM-','') from Product where productid= a.FrontPlateSize) AS FrontProductCode,a.FrontPlateSize, a.HSRP_Front_LaserCode, a.HSRP_Rear_LaserCode, a.RearPlateSize FROM HSRPRecords AS a   where  isnull(newpdfrunningno,'')='" + dropdownDuplicateFIle.SelectedItem + "'   ");

            strSQL.Append("Select  CONVERT(varchar(20),orderdate ,103) AS OrderBookDate,  CONVERT(varchar(20),aptgvehrecdate,103) as aptgvehrecdate, case a.VehicleClass when 'Transport' then 'T' else 'N.T.' end as VehicleClass, case a.VehicleType when 'MCV/HCV/TRAILERS' then 'Trailers' when 'THREE WHEELER' then 'T.Whe.' when 'SCOOTER' then 'SCOO' when 'TRACTOR' then 'TRAC' when 'LMV(CLASS)' then 'L.CL' when 'LMV' then 'LMV'  when 'MOTOR CYCLE' then 'MO.C' end as VehicleType,  a.VehicleRegNo, a.FrontPlateSize, a.HSRP_Front_LaserCode, a.HSRP_Rear_LaserCode, a.RearPlateSize,CONVERT(varchar(20),OrderEmbossingDate ,103) AS OrderEmbossingDate,a.roundoff_netamount as Amount, a.OrderStatus,convert(varchar, OrderClosedDate, 105) as OrderClosedDate FROM HSRPRecords AS a   where  isnull(newpdfrunningno,'')='" + dropdownDuplicateFIle.SelectedItem + "'   ");
            
                 
                 
                  

                    if (strVehicleType == "")
                    {
                        strSQL.Append(" order by a.VehicleClass,a.VehicleType,a.HSRP_Front_LaserCode,a.hsrp_rear_lasercode");
                    }
                    else
                    {
                        strSQL.Append( strVehicleType+ " order by a.VehicleClass,a.VehicleType,a.HSRP_Front_LaserCode,a.hsrp_rear_lasercode");
                    }
                    

                     dt = Utils.GetDataTable(strSQL.ToString(), CnnString);
                    if (dt.Rows.Count > 0)
                    {
                        SaveAndDownloadFile();

                    } 
            }
            catch (Exception ex)
            {
                lblErrMsg.Text = ex.Message;
            }
        }

        private void SaveAndDownloadFile()
        {
            Workbook book = new Workbook();
            string filename = "ProductionSheetReport" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";

            string strOrderType = string.Empty;

            Export(strOrderType, book, 1);

            HttpContext context = HttpContext.Current;
            context.Response.Clear();
            // Save the file and open it
            book.Save(Response.OutputStream);
            context.Response.ContentType = "application/vnd.ms-excel";

            context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
            context.Response.End();

        }

        int icount = 0;

        private void Export(string strReportType, Workbook book, int iActiveSheet)
        {
            try
            {
              


                // Specify which Sheet should be opened and the size of window by default
                book.ExcelWorkbook.ActiveSheetIndex = iActiveSheet;
                book.ExcelWorkbook.WindowTopX = 100;
                book.ExcelWorkbook.WindowTopY = 200;
                book.ExcelWorkbook.WindowHeight = 7000;
                book.ExcelWorkbook.WindowWidth = 8000;

                // Some optional properties of the Document
                book.Properties.Author = "HSRP";
                book.Properties.Title = "Collection Summary";
                book.Properties.Created = DateTime.Now;


                // Add some styles to the Workbook

                #region Styles
                if (icount <= 0)
                {
                    icount++;
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
                    style9.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                    style9.Interior.Color = "#FCF6AE";
                    style9.Interior.Pattern = StyleInteriorPattern.Solid;

                }
                #endregion

                Worksheet sheet = book.Worksheets.Add("Report");

               
               

                AddColumnToSheet(sheet, 100, dt.Columns.Count);



                int iIndex = 3;
                WorksheetRow row = sheet.Table.Rows.Add();
                row.Index = iIndex++;
                row.Cells.Add(new WorksheetCell("Report :", "HeaderStyle2"));
                WorksheetCell cell = row.Cells.Add("Report");
                cell.MergeAcross = 4; // Merge two cells together
                cell.StyleID = "HeaderStyle2";

                row = sheet.Table.Rows.Add();
                row.Index = iIndex++;

                AddNewCell(row, "State:", "HeaderStyle2", 1);
                AddNewCell(row, DropDownListStateName.SelectedItem.ToString(), "HeaderStyle2", 1);
                row = sheet.Table.Rows.Add();
                row.Index = iIndex++;

                DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());

                AddNewCell(row, "Report Date:", "HeaderStyle2", 1);
                AddNewCell(row, OrderDate.SelectedDate.ToString("dd/MMM/yyyy"), "HeaderStyle2", 1);
                AddNewCell(row, HSRPAuthDate.SelectedDate.ToString("dd/MMM/yyyy"), "HeaderStyle2", 1);
                row.Cells.Add(new WorksheetCell("Report Generated Date:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(System.DateTime.Now.ToString("dd/MMM/yyyy"), "HeaderStyle2"));

                AddNewCell(row, "", "HeaderStyle2", 2);
                row = sheet.Table.Rows.Add();

                row.Index = iIndex++;

                AddNewCell(row, "", "HeaderStyle6", 1);
                // WorksheetCell cell1 = row.Cells.Add("Today's Collection");
                //cell1.MergeAcross = 3; // Merge cells together
                // cell1.StyleID = "HeaderStyle6";
                //WorksheetCell cell2 = row.Cells.Add("Collection and Deposit MTD");
                //cell2.MergeAcross = 3; // Merge cells together
                //cell2.StyleID = "HeaderStyle6";
                //WorksheetCell cell3 = row.Cells.Add("Collection and Deposit Fy YTD");
                //cell3.MergeAcross = 3; // Merge cells together
                //cell3.StyleID = "HeaderStyle6";
                row = sheet.Table.Rows.Add();

                row.Index = iIndex++;

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    //if (dt.Columns[i].ColumnName.ToString().EndsWith("1") || dt.Columns[i].ColumnName.ToString().EndsWith("2"))
                    //{
                    //   // string strCol = dt.Columns[i].ColumnName.ToString();
                    //   // AddNewCell(row, strCol.Remove(strCol.Length - 1), "HeaderStyle6", 1);
                    //}
                    //else
                    //{
                    AddNewCell(row, dt.Columns[i].ColumnName.ToString(), "HeaderStyle6", 1);

                    //}
                }
                row = sheet.Table.Rows.Add();
                row.Index = iIndex++;
                for (int j = 0; j < dt.Rows.Count; j++)
                {

                    for (int i = 0; i < dt.Columns.Count; i++)
                    {

                        //if (dt.Rows[j]["Location"].ToString().Equals("ZZZZZ") && i.Equals(0))
                        //{
                        //    AddNewCell(row, "Total", "HeaderStyle6", 1);
                        //}
                        //else
                        //{
                        AddNewCell(row, dt.Rows[j][i].ToString(), "HeaderStyle6", 1);

                        //}
                    }
                    row = sheet.Table.Rows.Add();

                }

            }

            catch (Exception ex)
            {
                throw ex;
            }

        }

        private static void AddNewCell(WorksheetRow row, string strText, string strStyle, int iCnt)
        {
            for (int i = 0; i < iCnt; i++)
                row.Cells.Add(new WorksheetCell(strText, strStyle));
        }

        private static void AddColumnToSheet(Worksheet sheet, int iWidth, int iCnt)
        {
            for (int i = 0; i < iCnt; i++)
                sheet.Table.Columns.Add(new WorksheetColumn(iWidth));
        }
       
        
       

        

       
        }
    }
                