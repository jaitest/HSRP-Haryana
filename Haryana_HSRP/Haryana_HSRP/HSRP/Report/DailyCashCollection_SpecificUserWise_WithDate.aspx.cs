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
    public partial class DailyCashCollection_SpecificUserWise_WithDate : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        int UserType;
        string HSRPStateID;
        string RTOLocationID;
        int intHSRPStateID;
        int intRTOLocationID;
        //int intUserID    ;
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
                RTOLocationID = Session["UserRTOLocationID"].ToString();
                if (!IsPostBack)
                {
                    InitialSetting();
                    try
                    {
                        InitialSetting();
                       // if (UserType.Equals(0))
                        {
                        //    labelOrganization.Visible = true;
                        //    DropDownListStateName.Visible = true;
                        //    labelClient.Visible = true;
                        //    dropDownListClient.Visible = true;
                        //    FilldropDownListOrganization();
                            

                        //    FilldropDownListClient();

                        //    FilldropDownListUserName();
                        //    // labelClient.Visible = false;
                        //    // dropDownListClient.Visible = false;

                        //}
                        //else
                        //{
                            FilldropDownListOrganization();
                            DropDownListStateName.SelectedValue = HSRPStateID;
                            FilldropDownListClient();
                            dropDownListClient.SelectedValue = RTOLocationID.ToString();
                            labelClient.Visible = true;
                            dropDownListClient.Visible = true;
                            dropDownListClient.Enabled = false;
                            labelOrganization.Enabled = false;
                            DropDownListStateName.Enabled = false;
                            labelClient.Enabled = false;
                            // labelDate.Visible = false;                          

                            FilldropDownListUserName();
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
            //HSRPAuthDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            //HSRPAuthDate.MaxDate = DateTime.Parse(MaxDate);
            //CalendarHSRPAuthDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            //CalendarHSRPAuthDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            //OrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            //OrderDate.MaxDate = DateTime.Parse(MaxDate);
            //CalendarOrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            //CalendarOrderDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
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

                Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), CnnString, "--Select Location--");
            
            }
        }

        private void FilldropDownListUserName()
        {
            if (UserType.Equals(0))
            {
                int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);
                SQLString = "select userID, (userFirstName +' ' +UserLastName) as UserName from Users Where HSRP_StateID=" + DropDownListStateName.SelectedValue + "and RTOLocationID=" + dropDownListClient.SelectedValue + " and userid='"+strUserID+"'  and ActiveStatus!='N'  Order by userFirstName";
                Utils.PopulateDropDownList(dropDownListUser, SQLString.ToString(), CnnString, "--Select User--");
                // dropDownListClient.SelectedIndex = dropDownListClient.Items.Count - 1;
            }
            else
            {
                lbluser.Visible = true;
                dropDownListUser.Visible = true;
                int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);
                SQLString = "select userID, (userFirstName +' ' +UserLastName) as UserName from Users Where HSRP_StateID=" + DropDownListStateName.SelectedValue + "and RTOLocationID=" + dropDownListClient.SelectedValue + " and userid='" + strUserID + "'  and ActiveStatus!='N'  Order by userFirstName";
                Utils.PopulateDropDownList(dropDownListUser, SQLString.ToString(), CnnString, "--Select User--");
                
                //SQLString = "select userID, (userFirstName +' ' +UserLastName) as UserName from Users Where HSRP_StateID=" + Session["UserHSRPStateID"].ToString() + "and RTOLocationID=" + Session["UserRTOLocationID"].ToString() + "  and ActiveStatus!='N'  Order by userFirstName";
                //Utils.PopulateDropDownList(dropDownListUser, SQLString.ToString(), CnnString, "--Select User--");
                //dropDownListClient.SelectedIndex = dropDownListClient.Items.Count - 1;
                
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

                //String[] StringAuthDate = OrderDate.SelectedDate.ToString().Split('/');
                //string MonTo = ("0" + StringAuthDate[0]);
                //string MonthdateTO = MonTo.Replace("00", "0").Replace("01", "1");
                //String FromDateTo = StringAuthDate[1] + "-" + MonthdateTO + "-" + StringAuthDate[2].Split(' ')[0];
                //String From = StringAuthDate[2].Split(' ')[0] + "/" + StringAuthDate[0] + "/" + StringAuthDate[1];
                ////AuthorizationDate = new DateTime(Convert.ToInt32(StringAuthDate[2].Split(' ')[0]), Convert.ToInt32(StringAuthDate[0]), Convert.ToInt32(StringAuthDate[1]));
                //string FromDate = From + " 00:00:00"; // Convert.ToDateTime();

                //String[] StringOrderDate = HSRPAuthDate.SelectedDate.ToString().Split('/');
                //string Mon = ("0" + StringOrderDate[0]);
                //string Monthdate = Mon.Replace("00", "0").Replace("01", "1");
                //string FromDate1 = StringOrderDate[1] + "-" + Monthdate + "-" + StringOrderDate[2].Split(' ')[0];

                //String From1 = StringOrderDate[2].Split(' ')[0] + "-" + StringOrderDate[0] + "-" + StringOrderDate[1];
                ////OrderDate1 = new DateTime(Convert.ToInt32(StringOrderDate[2].Split(' ')[0]), Convert.ToInt32(StringOrderDate[0]), Convert.ToInt32(StringOrderDate[1]));
                //string ToDate = From1 + " 23:59:59";
                
                string FromDate = DateTime.Now.AddDays(int.Parse(DropDownListdate.SelectedValue)).ToString("yyyy/MM/dd") + " 00:00:00"; // Convert.ToDateTime();
                string ToDate = DateTime.Now.ToString("yyyy/MM/dd") + " 23:59:59";
              
              //  DateTime StartDate = Convert.ToDateTime(OrderDate.SelectedDate);
                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);
                //int.TryParse(dropDownListUser.SelectedValue, out intUserID);
                DataTable dt = new DataTable();

                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);
                //int.TryParse(dropDownListUser.SelectedValue, out UserID);

                string filename = "DailyCashCollectionReport-CounterWise-" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                Workbook book = new Workbook();
               
                // Specify which Sheet should be opened and the size of window by default
                book.ExcelWorkbook.ActiveSheetIndex = 1;
                book.ExcelWorkbook.WindowTopX = 100;
                book.ExcelWorkbook.WindowTopY = 200;
                book.ExcelWorkbook.WindowHeight = 7000;
                book.ExcelWorkbook.WindowWidth = 8000;

                // Some optional properties of the Document
                book.Properties.Author = "HSRP";
                book.Properties.Title = "HSRP Daily Cash Collection";
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

                Worksheet sheet = book.Worksheets.Add("HSRP Daily Cash Collection");
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
                WorksheetCell cell = row.Cells.Add("HSRP Daily Cash Collection");
                cell.MergeAcross = 3; // Merge two cells together
                cell.StyleID = "HeaderStyle3";

                row = sheet.Table.Rows.Add();
                row.Index = 3;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("State:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(DropDownListStateName.SelectedItem.ToString(), "HeaderStyle2"));

                //row = sheet.Table.Rows.Add();
                //row.Index = 4;
                //row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                //row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("Location:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(dropDownListClient.SelectedItem.ToString(), "HeaderStyle2"));

                row = sheet.Table.Rows.Add();
                row.Index = 4;

                DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("Date Generated :", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(dates.ToString("M/dd/yyyy"), "HeaderStyle2"));
                //row = sheet.Table.Rows.Add();
                //row.Index = 6;
                //row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                //row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));

                row.Cells.Add(new WorksheetCell("Report Date:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(DateTime.Now.ToString(), "HeaderStyle2"));
                row = sheet.Table.Rows.Add();

                row.Index = 5;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("Reported By", "HeaderStyle2"));
                if (UserType.Equals(0))
                {
                    row.Cells.Add(new WorksheetCell(dropDownListUser.SelectedItem.Text, "HeaderStyle2"));
                }
                else
                {
                    row.Cells.Add(new WorksheetCell(Session["UserName"].ToString(), "HeaderStyle2"));
                }
                row = sheet.Table.Rows.Add();

                row.Index = 7;
                //row.Cells.Add("Order Date");
                row.Cells.Add(new WorksheetCell("S.No.", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Location Name", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Authorisation No", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Cash Receipt No", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Authorisation Date", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Order Date", "HeaderStyle6"));
                //row.Cells.Add(new WorksheetCell("Close Date", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Vehicle Reg. No", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Owner's Name", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Vechicle Type", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Vehicle Class", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Vehicle Make", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Model Name","HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Amount", "HeaderStyle6"));
                //row.Cells.Add(new WorksheetCell("Delay in No. of Days", "HeaderStyle"));

                row = sheet.Table.Rows.Add();
                String StringField = String.Empty;
                String StringAlert = String.Empty;

                row.Index = 8;

                UserType = Convert.ToInt32(Session["UserType"]);
                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);


                //SQLString = "SELECT  HSRP_StateID, CONVERT(varchar(20), HsrpRecord_AuthorizationDate, 103) AS HsrpRecord_AuthorizationDate,  CONVERT(varchar(20), OrderDate, 103) AS NewOrderDate,CONVERT(varchar(20), OrderEmbossingDate, 103) AS OrderEmbossingDate,CONVERT(varchar(20), InvoiceDateTime, 103) AS InvoiceDateTime,  HSRPRecord_AuthorizationNo,CashReceiptNo, HSRPRecordID, VehicleRegNo, OwnerName, VehicleType, VehicleClass,CONVERT(numeric,round( RoundOff_NetAmount,0)) as NetAmount,OrderDate FROM HSRPRecords where HSRP_StateID='" + DropDownListStateName.SelectedValue + "' and RTOLocationID='" + dropDownListClient.SelectedValue +"' and OrderDate between '" + ReportDate1 + "' and '" + ReportDate2 + "' order by OrderDate";
                if (UserType.Equals(0))
                {
                    if (HSRPStateID == "9")
                    {
                        SQLString = "SELECT  HSRP_StateID,(select RtolocationName from rtolocation where rtolocationID = hsrprecords.RTOLocationID) as rtolocation, " +
                                  " CONVERT(varchar(20), HsrpRecord_AuthorizationDate, 103) AS HsrpRecord_AuthorizationDate,  CONVERT(varchar(20), OrderDate, 103) AS NewOrderDate," +
                                  " CONVERT(varchar(20), OrderEmbossingDate, 103) AS OrderEmbossingDate,CONVERT(varchar(20), InvoiceDateTime, 103) AS InvoiceDateTime, " +
                                  " HSRPRecord_AuthorizationNo,CashReceiptNo, HSRPRecordID, VehicleRegNo, OwnerName, VehicleType,[ManufacturerName],[ManufacturerModel]," +
                                  " VehicleClass,CONVERT(numeric,round( RoundOff_NetAmount,0)) as NetAmount,OrderDate FROM HSRPRecords where OrderStatus ='New Order' and hsrprecords.createdby ='" + dropDownListUser.SelectedValue + "' and OrderDate between '" + FromDate + "' and '" + ToDate + "' order by OrderDate";
                    }
                    else
                    {
                        SQLString = "SELECT  HSRP_StateID,(select RtolocationName from rtolocation where rtolocationID = hsrprecords.RTOLocationID) as rtolocation, " +
                                    " CONVERT(varchar(20), HsrpRecord_AuthorizationDate, 103) AS HsrpRecord_AuthorizationDate,  CONVERT(varchar(20), OrderDate, 103) AS NewOrderDate," +
                                    " CONVERT(varchar(20), OrderEmbossingDate, 103) AS OrderEmbossingDate,CONVERT(varchar(20), InvoiceDateTime, 103) AS InvoiceDateTime, " +
                                     " HSRPRecord_AuthorizationNo,CashReceiptNo, HSRPRecordID, VehicleRegNo, OwnerName, VehicleType,VehicleMakerMaster.VehicleMakerDescription,VehicleModelMaster.VehicleModelDescription," +
                                       " VehicleClass,CONVERT(numeric,round( RoundOff_NetAmount,0)) as NetAmount,OrderDate FROM HSRPRecords left join VehicleMakerMaster" +
                                      " on HSRPRecords.manufacturername = VehicleMakerMaster.VehicleMakerID" +
                                      " left join VehicleModelMaster" +
                                                  " on HSRPRecords.manufacturermodel = VehicleModelMaster.VehicleModelID where OrderStatus ='New Order' and hsrprecords.createdby ='" + dropDownListUser.SelectedValue + "' and OrderDate between '" + FromDate + "' and '" + ToDate + "' order by OrderDate";
                    }
                }
                else
                {
                    if (HSRPStateID == "9")
                    {
                        SQLString = "SELECT  HSRP_StateID,(select RtolocationName from rtolocation where rtolocationID = hsrprecords.RTOLocationID) as rtolocation, " +
                                  " CONVERT(varchar(20), HsrpRecord_AuthorizationDate, 103) AS HsrpRecord_AuthorizationDate,  CONVERT(varchar(20), OrderDate, 103) AS NewOrderDate," +
                                  " CONVERT(varchar(20), OrderEmbossingDate, 103) AS OrderEmbossingDate,CONVERT(varchar(20), InvoiceDateTime, 103) AS InvoiceDateTime, " +
                                  " HSRPRecord_AuthorizationNo,CashReceiptNo, HSRPRecordID, VehicleRegNo, OwnerName, VehicleType,[ManufacturerName],[ManufacturerModel]," +
                                  " VehicleClass,CONVERT(numeric,round( RoundOff_NetAmount,0)) as NetAmount,OrderDate FROM HSRPRecords where OrderStatus ='New Order' and hsrprecords.createdby ='" + Session["UID"].ToString() + "' and OrderDate between '" + FromDate + "' and '" + ToDate + "' order by OrderDate";
                    }
                    else
                    {
                        SQLString = "SELECT  HSRP_StateID,(select RtolocationName from rtolocation where rtolocationID = hsrprecords.RTOLocationID) as rtolocation, " +
                                    " CONVERT(varchar(20), HsrpRecord_AuthorizationDate, 103) AS HsrpRecord_AuthorizationDate,  CONVERT(varchar(20), OrderDate, 103) AS NewOrderDate," +
                                    " CONVERT(varchar(20), OrderEmbossingDate, 103) AS OrderEmbossingDate,CONVERT(varchar(20), InvoiceDateTime, 103) AS InvoiceDateTime, " +
                                     " HSRPRecord_AuthorizationNo,CashReceiptNo, HSRPRecordID, VehicleRegNo, OwnerName, VehicleType,VehicleMakerMaster.VehicleMakerDescription,VehicleModelMaster.VehicleModelDescription," +
                                       " VehicleClass,CONVERT(numeric,round( RoundOff_NetAmount,0)) as NetAmount,OrderDate FROM HSRPRecords left join VehicleMakerMaster" +
                                      " on HSRPRecords.manufacturername = VehicleMakerMaster.VehicleMakerID" +
                                      " left join VehicleModelMaster" +
                                      " on HSRPRecords.manufacturermodel = VehicleModelMaster.VehicleModelID where OrderStatus ='New Order' and hsrprecords.createdby ='" + Session["UID"].ToString() + "' and OrderDate between '" + FromDate + "' and '" + ToDate + "' order by OrderDate";
                    }
                }
                dt = Utils.GetDataTable(SQLString, CnnString);
                string RTOColName = string.Empty;
                decimal totalAmount = 0;
                if (dt.Rows.Count > 0)
                {
                    int sno = 0;
                    foreach (DataRow dtrows in dt.Rows) // Loop over the rows.
                    {
                        sno = sno + 1;
                        if (sno == 43)
                        {
                          int  ssno = sno;
                        }
                        row.Cells.Add(new WorksheetCell(Convert.ToInt32(sno).ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["rtolocation"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["HSRPRecord_AuthorizationNo"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["CashReceiptNo"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["HsrpRecord_AuthorizationDate"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["NewOrderDate"].ToString(), DataType.String, "HeaderStyle"));
                        //row.Cells.Add(new WorksheetCell(dtrows["InvoiceDateTime"].ToString(), DataType.String, "HeaderStyle"));

                        row.Cells.Add(new WorksheetCell(dtrows["VehicleRegNo"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["OwnerName"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["VehicleClass"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["VehicleType"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["VehicleMakerDescription"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["VehicleModelDescription"].ToString(), DataType.String, "HeaderStyle"));
                        string amount = dtrows["NetAmount"].ToString();
                        if (amount == "")
                        {

                            row.Cells.Add(new WorksheetCell("0", DataType.String, "HeaderStyle"));
                            //totalAmount = totalAmount + Math.Round(Convert.ToDecimal(amount.ToString()));
                        }
                        else
                        {
                            row.Cells.Add(new WorksheetCell(Math.Round(Convert.ToDecimal(dtrows["NetAmount"].ToString())).ToString(), DataType.Number, "HeaderStyle5"));
                            totalAmount = totalAmount + Math.Round(Convert.ToDecimal(dtrows["NetAmount"].ToString()));
                        } 
                        row = sheet.Table.Rows.Add();
                    } 
                    row = sheet.Table.Rows.Add();
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle8"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle8"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle8"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle8"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle8"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle8"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle8"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle8"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle8"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle8"));

                    row.Cells.Add(new WorksheetCell("Total :", "HeaderStyle8"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle8"));
                    //row.Cells.Add(new WorksheetCell("", "HeaderStyle8"));
                    //row.Cells.Add(new WorksheetCell("", "HeaderStyle8"));
                    row.Cells.Add(new WorksheetCell((totalAmount).ToString(), "HeaderStyle8"));

                    totalAmount = 0;

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

        protected void dropDownListClient_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            FilldropDownListUserName();
        }

        protected void dropDownListUser_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}