using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using DataProvider;
using HSRP;
using CarlosAg.ExcelXmlWriter;


namespace HSRP.Transaction
{
    public partial class InvoiceMasterNo : System.Web.UI.Page
    { 
 
        int HSRP_StateID;
        string SQLString = string.Empty;  
        string UserType = string.Empty;      
       
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
       
        string HSRPStateID = string.Empty;
        string RTOLocationID = string.Empty;
        int intHSRPStateID;
        string SQlQuery = string.Empty;
        string ExicseAmount = string.Empty;
        string AllLocation = string.Empty;
        string OrderStatus = string.Empty;
      
    
        string CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();

        // string CnnString = string.Empty;
        // CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString(); 
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

                lblErrMess.Text = string.Empty;
                strUserID = Session["UID"].ToString();
                ComputerIP = Request.UserHostAddress;

                if (!IsPostBack)
                {
                    //InitialSetting();
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
                            //hiddenUserType.Value = "1";
                            labelOrganization.Enabled = false;
                            DropDownListStateName.Enabled = false;
                            labelClient.Enabled = false;
                            FilldropDownListOrganization();
                            FilldropDownListClient();

                        }

                    }
                    catch (Exception err)
                    {
                        lblErrMess.Text = "Error on Page Load" + err.Message.ToString();
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

                SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + intHSRPStateID + " and ActiveStatus!='N' and RTOLocationName in('BURARI','MAYAPURI')   Order by RTOLocationName";
                Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), CnnString, "--Select RTO Name--");

                //dataLabellbl.Visible = false;
                //TRRTOHide.Visible = false;
            }
            else
            {
                string UserID = Convert.ToString(Session["UID"]);

                SQLString = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, (a.RTOLocationCode+', ') as RTOCode, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID and a.ActiveStatus ='Y'  where UserRTOLocationMapping.UserID='" + UserID + "' and  a.RTOLocationName in('BURARI','MAYAPURI') order by a.rtolocationname";
                DataTable dss = Utils.GetDataTable(SQLString, CnnString);

                labelOrganization.Visible = true;
                DropDownListStateName.Visible = true;
                labelClient.Visible = true;
                dropDownListClient.Visible = true;

                dropDownListClient.DataSource = dss;
                dropDownListClient.DataBind();
                //dataLabellbl.Visible = true;

                string RTOCode = string.Empty;
                if (dss.Rows.Count > 0)
                {
                    for (int i = 0; i <= dss.Rows.Count - 1; i++)
                    {
                        RTOCode += dss.Rows[i]["RTOCode"].ToString();

                    }
                  //  lblRTOCode.Text = RTOCode.Remove(RTOCode.LastIndexOf(","));

                }
            }
        }

        #endregion

        public void insertinvoiceno()
        {
            try
            {
                string strInvoiceNo = string.Empty;
                string sqlstring = string.Empty;
                string strEmbId = string.Empty;
                string currentdate = DateTime.Now.ToString("dd/MM/yyyy");
                string Remarks = string.Empty;
                Int32 totalamount = 0;
                string RtoName = string.Empty;
                RtoName = dropDownListClient.SelectedItem.ToString();

                string strSelectEmbStation = "SELECT DISTINCT [NAVEMBID],[EmbCenterName],address1,city FROM [vw_RTOLocationWiseEmbosingCenters] WHERE RTOLocationId='" + dropDownListClient.SelectedValue + "'";
                DataTable dtEmbData = Utils.GetDataTable(strSelectEmbStation, CnnString);
                if (dtEmbData.Rows.Count <= 0)
                {
                    lblErrMess.Text = "Embossing Station not found";
                    return;
                }
                strEmbId = dtEmbData.Rows[0]["NAVEMBID"].ToString();

                string strGetInvoiceNo = "select (isnull(prefixtext,'')+right('00000'+ convert(varchar,lastno+1),5)) from [EmbossingCenters] " +
                                           "where Emb_Center_Id= '" + strEmbId + "' and prefixfor='Cash Receipt No' ";
                strInvoiceNo = (Utils.getScalarValue(strGetInvoiceNo, CnnString));

                string strGetFinYear = "SELECT [dbo].[fnGetFiscalYear] ( GetDate() )";
                strInvoiceNo = strInvoiceNo + "/" + (Utils.getScalarValue(strGetFinYear, CnnString)).Replace("20", string.Empty);

                string strUpdateInvoiceNo = "update [EmbossingCenters] set lastno=lastno+1 where [Emb_Center_Id]= '" + strEmbId + "' and prefixfor='Cash Receipt No'";
                Utils.ExecNonQuery(strUpdateInvoiceNo, CnnString);

                DataTable GetAddress;
                string Address;
                GetAddress = Utils.GetDataTable("select * from HSRPState WHERE HSRP_StateID='" + DropDownListStateName.SelectedValue + "'", CnnString);

                if ((GetAddress.Rows[0]["pincode"].ToString() != "") || (GetAddress.Rows[0]["pincode"] != null))
                {
                    Address = " - " + GetAddress.Rows[0]["pincode"];
                }
                else
                {
                    Address = "";
                }
                Remarks = "Transfer";
                sqlstring = "insert into InvoiceMaster(InvoiceNo,InvoiceDate,Amount,BuyerName,clientName,hsrp_stateid,dispatchedLocation) values('" + strInvoiceNo + "', Convert(date,('" + currentdate + "'),103),'" + totalamount + "','" + Remarks + "','" + GetAddress.Rows[0]["Address1"].ToString() + "','" + DropDownListStateName.SelectedValue + "','" + dropDownListClient.SelectedItem.Text + "')";
                Utils.ExecNonQuery(sqlstring, CnnString);

                lblinvoicemsg.Text = "New Invoice No :" + strInvoiceNo;
            }
            catch
            {
                lblErrMess.Text = "Embossing Station not found";
                    return;
            }
        }

        protected void buttonUpdateinvoice_Click(object sender, EventArgs e)
        {
            insertinvoiceno();
        }

        protected void DropDownListStateName_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilldropDownListClient();
            dropDownListClient.Visible = true;
            labelClient.Visible = true;
        }

        protected void btnexcel_Click(object sender, EventArgs e)
        {
            exportreportinvoice();
        }

        public void exportreportinvoice()
        {
            try
            {
                
                int ISum = 0;
                //String[] StringAuthDate = OrderDate.SelectedDate.ToString().Split('/');
                //string MonTo = ("0" + StringAuthDate[0]);
                //string MonthdateTO = MonTo.Replace("00", "0").Replace("01", "1");
                //String FromDateTo = StringAuthDate[1] + "-" + MonthdateTO + "-" + StringAuthDate[2].Split(' ')[0];
                //String From = StringAuthDate[0] + "/" + StringAuthDate[1] + "/" + StringAuthDate[2].Split(' ')[0];
                //string FromDate = From + " 00:00:01"; // Convert.ToDateTime();

                //String[] StringOrderDate = HSRPAuthDate.SelectedDate.ToString().Split('/');
                //string Mon = ("0" + StringOrderDate[0]);
                //string Monthdate = Mon.Replace("00", "0").Replace("01", "1");
                //string FromDate1 = StringOrderDate[1] + "-" + Monthdate + "-" + StringOrderDate[2].Split(' ')[0];

                //String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
                ////OrderDate1 = new DateTime(Convert.ToInt32(StringOrderDate[2].Split(' ')[0]), Convert.ToInt32(StringOrderDate[0]), Convert.ToInt32(StringOrderDate[1]));
                //string ToDate = From1 + " 23:59:59";


                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                //int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);
               // DataTable StateName;
               // DataTable dts;
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                DataTable dt2 = new DataTable();

                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                //int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);

                string filename = "Invoice Report" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                Workbook book = new Workbook();

                // Specify which Sheet should be opened and the size of window by default
                book.ExcelWorkbook.ActiveSheetIndex = 1;
                book.ExcelWorkbook.WindowTopX = 100;
                book.ExcelWorkbook.WindowTopY = 200;
                book.ExcelWorkbook.WindowHeight = 7000;
                book.ExcelWorkbook.WindowWidth = 8000;

                // Some optional properties of the Document
                book.Properties.Author = "HSRP";
                book.Properties.Title = "Invoice Report";
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
                style3.Alignment.Horizontal = StyleHorizontalAlignment.Left;

                Worksheet sheet = book.Worksheets.Add("Invoice Report");
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
                row.Cells.Add(new WorksheetCell(" ", "HeaderStyle3"));
                WorksheetCell cell = row.Cells.Add("Invoice  Report");
                cell.MergeAcross = 3; // Merge two cells together
                cell.StyleID = "HeaderStyle3";

                row = sheet.Table.Rows.Add();

                row = sheet.Table.Rows.Add();
                row.Index = 3;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("State:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(DropDownListStateName.SelectedItem.ToString(), "HeaderStyle2"));

                row = sheet.Table.Rows.Add();
                //  Skip one row, and add some text
                row.Index = 4;

               
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("Generated Date time:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(DateTime.Now.ToString(), "HeaderStyle2"));
                row = sheet.Table.Rows.Add();



                row.Index = 7;
                //row.Cells.Add("Order Date");
                row.Cells.Add(new WorksheetCell("S.No.", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("InvoiceNo", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Invoice Date", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Buyer Name", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Client Name", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Dispatch Location", "HeaderStyle"));
                //row.Cells.Add(new WorksheetCell("HSRP Front LaserCode", "HeaderStyle"));
                //row.Cells.Add(new WorksheetCell("HSRP Rear LaserCode", "HeaderStyle"));
                row = sheet.Table.Rows.Add();
                String StringField = String.Empty;
                String StringAlert = String.Empty;

                //row.Index = 9;

              //  UserType = Convert.ToInt32(Session["UserType"]);


                //SQLString = "select rtolocationname,count(*) as Orders  from [hsrpdemo].[dbo].[rejectplatEntry] b,hsrpdemo.dbo.RTOLocation c  where b.rtolocationid=c.RTOLocationID and b.hsrp_stateid='" + DropDownListStateName.SelectedValue.ToString() + "' and EntryDate between '" + FromDate + "' and '" + ToDate + "' group by rtolocationname order by 1";

                //SQLString = "select vehicleregno,convert(varchar(15),orderdate,103) as orderdate, convert(varchar(15),aptgvehrecdate,103) as aptgvehrecdate,RejectFlag,DATEADD(day,3,aptgvehrecdate) as date,HSRP_Front_LaserCode,HSRP_Rear_LaserCode from hsrprecords where convert(date,DATEADD(day,3,aptgvehrecdate),103)<=convert(date,getdate(),103) and HSRP_StateID='" + DropDownListStateName.SelectedValue.ToString() + "' and OrderStatus='New Order' and isnull(vehicleregno,'')!='' and isnull(HSRP_Front_LaserCode,'')!='' and isnull(HSRP_Rear_LaserCode,'')!=''";
                //SQLString = "select InvoiceNo,InvoiceDate,BuyerName,ClientName,DispatchedLocation from invoicemaster where  BuyerName like '%Transfer%'";

                SQLString = "select a.InvoiceNo,a.InvoiceDate,a.BuyerName,a.ClientName,a.DispatchedLocation from invoicemaster a, vw_RTOLocationWiseEmbosingCenters b where a.DispatchedLocation =b.EmbCenterName and b.RTOLocationId='" + dropDownListClient.SelectedValue + "' and BuyerName like '%Transfer%' and a.DispatchedLocation is not null";

                dt = Utils.GetDataTable(SQLString, CnnString);


                string RTOColName = string.Empty;
                int sno = 0;
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++) // Loop over the rows.
                    {
                        sno = sno + 1;
                        row = sheet.Table.Rows.Add();
                        row.Cells.Add(new WorksheetCell(sno.ToString(), DataType.String, "HeaderStyle1"));

                        row.Cells.Add(new WorksheetCell(dt.Rows[i]["InvoiceNo"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dt.Rows[i]["InvoiceDate"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dt.Rows[i]["BuyerName"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dt.Rows[i]["ClientName"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dt.Rows[i]["DispatchedLocation"].ToString(), DataType.String, "HeaderStyle1"));
                        //row.Cells.Add(new WorksheetCell(dt.Rows[i]["HSRP_Front_LaserCode"].ToString(), DataType.String, "HeaderStyle1"));
                       // row.Cells.Add(new WorksheetCell(dt.Rows[i]["HSRP_Rear_LaserCode"].ToString(), DataType.String, "HeaderStyle1"));



                        // ISum = ISum + Convert.ToInt16(dt.Rows[i]["Orders"]);

                    }


                    row = sheet.Table.Rows.Add();
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    // row.Cells.Add(new WorksheetCell("Total", "HeaderStyle"));
                    //  row.Cells.Add(new WorksheetCell(ISum.ToString(), DataType.String, "HeaderStyle1"));
                    row = sheet.Table.Rows.Add();
                    HttpContext context = HttpContext.Current;
                    context.Response.Clear();
                    book.Save(Response.OutputStream);

                    context.Response.ContentType = "application/vnd.ms-excel";

                    context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                    context.Response.End();

                }



            }

            catch (Exception ex)
            {
                lblErrMess.Text = "Error in Populating Grid :" + ex.Message.ToString();
            }
        }
     
    }
}