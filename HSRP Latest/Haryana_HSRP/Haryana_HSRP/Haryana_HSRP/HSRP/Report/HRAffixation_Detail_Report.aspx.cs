
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
using iTextSharp.text.pdf;
using System.Configuration;
using System.IO;
using System.Text;

namespace HSRP.Report
{
    public partial class HRAffixation_Detail_Report : System.Web.UI.Page
    {
        int UserType1;
        string CnnString1 = string.Empty;
        string HSRP_StateID1 = string.Empty;
        string RTOLocationID1 = string.Empty;
        string strUserID1 = string.Empty;
        string SQLString1 = string.Empty;

        string strformdate = string.Empty;
        string strtodate = string.Empty;

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
                   // gridTD.Visible = false;
                   // grd.Visible = false;
                    displaygriddetails();
                    try
                    {
                        if (UserType1.Equals(0))
                        {
                           // DropDownListStateName.Visible = true;
                            FilldropDownListStateName();
                         //   Datefrom.SelectedDate = System.DateTime.Now;
                          //  Dateto.SelectedDate = System.DateTime.Now;
                        }
                        else
                        {
                          //  DropDownListStateName.Visible = true;
                            FilldropDownListStateName();
                          //  Datefrom.SelectedDate = System.DateTime.Now;
                          //  Dateto.SelectedDate = System.DateTime.Now;
                            //FilldropddlRTOLocation(HSRP_StateID1);
                        }
                    }
                    catch (Exception err)
                    {
                        throw err;
                    }

                   
                }
            }
        }

        private void FilldropDownListStateName()
        {
            //if (UserType1.Equals(0))
            //{
            //    SQLString1 = "select HSRPStateName,HSRP_StateID from HSRPState where ActiveStatus='Y' Order by HSRPStateName";
            //    Utils.PopulateDropDownList(DropDownListStateName, SQLString1.ToString(), CnnString1, "--Select State--");
            //}
            //else
            //{
            //    SQLString1 = "select HSRPStateName,HSRP_StateID from HSRPState where HSRP_StateID=" + HSRP_StateID1 + " and ActiveStatus='Y' Order by HSRPStateName";
            //    DataSet dts = Utils.getDataSet(SQLString1, CnnString1);
            //    DropDownListStateName.DataSource = dts;
            //    DropDownListStateName.DataBind();
            //}
        }

        //private void FilldropddlRTOLocation(string StateID)
        //{
        //    if (UserType1.Equals(0))
        //    {
        //        SQLString1 = "Select RTOLocationID,RTOLocationName from RTOLocation where HSRP_StateID='" + StateID + "' and ActiveStatus='Y'";
        //        Utils.PopulateDropDownList(ddlRtoLocation, SQLString1.ToString(), CnnString1, "--Select Location--");
        //    }
        //    else
        //    {
        //        SQLString1 = "select HSRPStateName,HSRP_StateID from HSRPState where HSRP_StateID=" + StateID + " and ActiveStatus='Y' Order by HSRPStateName";
        //        DataSet dts = Utils.getDataSet(SQLString1, CnnString1);
        //        DropDownListStateName.DataSource = dts;
        //        DropDownListStateName.DataBind();
        //    }
        //}

        private void InitialSetting()
        {
            //string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            //string MaxDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();

            //Datefrom.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            //Datefrom.MaxDate = DateTime.Parse(MaxDate);
            //CalendarDatefrom.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            //CalendarDatefrom.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);

            //Dateto.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            //Dateto.MaxDate = DateTime.Parse(MaxDate);
            //CalendarDateto.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            //CalendarDateto.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
        }


       
        public void displaygriddetails()
        {
            try
            {
                string RTOlocationcode = Session["RTOLocationCode"].ToString();
                //strformdate = Session["formdate"].ToString();
                strtodate = Session["todate"].ToString();
                string sqlquery = "select ROW_NUMBER() Over (Order by VehicleRegNo) As [S.NO],rtolocationcode, VehicleRegNo,OwnerName,EngineNo,ChassisNo,VehicleType,VehicleClass,TotalAmount from hsrprecords a, rtolocation b where  a.hsrp_stateid='" + HSRP_StateID1 + "' and a.rtolocationid=b.rtolocationid and b.rtolocationcode='" + RTOlocationcode + "' and convert(date,HSRPRecord_CreationDate,101) = convert(date,'" + strtodate + "')";
                DataTable dt= Utils.GetDataTable(sqlquery, CnnString1);
                if (dt.Rows.Count > 0)
                {
                    grd.Visible = true;
                    grd.DataSource = dt;
                    grd.DataBind();
                    
                }              
            }
            catch (Exception ex)
            {
                throw ex;
            }         
        }

      

       

        protected void DropDownListStateName_TextChanged(object sender, EventArgs e)
        {
            //FilldropddlRTOLocation(DropDownListStateName.SelectedValue);
            gridTD.Visible = false;
            grd.DataSource = null;
            grd.Visible = false;
        }
        int icount = 0;
    
     

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

        protected void btn_download_Click(object sender, EventArgs e)
        {
          //  this.SaveAndDownloadFile();
        }
        bool btndownload1=false;
        protected void btndownloadDetail_Click(object sender, EventArgs e)
        {
            //btndownload1 = true;
           // this.SaveAndDownloadFile2();
        }

       

        protected void grd_RowCommand(object sender, GridViewCommandEventArgs e)
        {
           // string embcentername = string.Empty;
           // string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
           //// StringBuilder strSQL = new StringBuilder();
           // DateTime ReportDate2 = Datefrom.SelectedDate;
           // string ReportDate3 = Convert.ToString(ReportDate2);

           // string state = DropDownListStateName.SelectedValue;
           // GridViewRow oItem = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
           // int RowIndex = oItem.RowIndex;
        }



        //protected void ddlaptg_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        SqlConnection con = new SqlConnection(CnnString1);
        //        #region Fetch Data
        //        DataSet ds = new DataSet();

        //        SqlCommand cmd = new SqlCommand();
        //        string strParameter = string.Empty;
        //        //change sp Name
        //        cmd = new SqlCommand("Business_ReportTypewisesummary_report", con);//Business_ReportTypewisesummary_report

        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.Add(new SqlParameter("@StateId", DropDownListStateName.SelectedValue));
        //        cmd.Parameters.Add(new SqlParameter("@ReportType", DropDownListReportType.SelectedValue));
        //        cmd.Parameters.Add(new SqlParameter("@reportDate", Datefrom.SelectedDate));
        //        cmd.Parameters.Add(new SqlParameter("@reportTo", Dateto.SelectedDate));

        //        cmd.CommandTimeout = 0;
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        da.Fill(ds);

        //        #endregion
        //        if (ds.Tables[0].Rows.Count > 0)
        //        {
        //            gridTD.Visible = true;
        //            grd.DataSource = ds.Tables[0];
        //            grd.DataBind();
        //            grd.Visible = true;
        //            DownloadDetailPanel.Visible = false;
        //            if (ds.Tables.Count == 2)
        //            {

        //                DownloadDetailPanel.Visible = true;
        //                ddlLocation.DataSource = ds.Tables[1];
        //                ddlLocation.DataTextField = "Location";
        //                ddlLocation.DataValueField = "RTOLocationID";
        //                ddlLocation.DataBind();
        //                ddlLocation.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Location--", "0"));
        //                ddlLocation.Items.Insert(1, new System.Web.UI.WebControls.ListItem("All", "All"));
        //                ddlUser.ClearSelection();
        //                ddlUser.DataSource = "";
        //                ddlUser.DataBind();
        //                ddlUser.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select User--", "0"));
        //                ddlUser.Items.Insert(1, new System.Web.UI.WebControls.ListItem("All", "All"));


        //            }

        //        }
        //        else
        //        {
        //            gridTD.Visible = false;
        //            grd.DataSource = null;
        //            grd.Visible = false;

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}


        public void pdf(DataTable dt)
        {
            string RTOlocationcode = Session["RTOLocationCode"].ToString();
            strtodate = Session["todate"].ToString();
            string sqlquery = "select ROW_NUMBER() Over (Order by VehicleRegNo) As [S.NO],rtolocationcode, VehicleRegNo,OwnerName,EngineNo,ChassisNo,VehicleType,VehicleClass,TotalAmount from hsrprecords a, rtolocation b where  a.hsrp_stateid='" + HSRP_StateID1 + "' and a.rtolocationid=b.rtolocationid and b.rtolocationcode='" + RTOlocationcode + "' and convert(date,HSRPRecord_CreationDate,101) = convert(date,'" + strtodate + "')";
            DataTable dt1 = Utils.GetDataTable(sqlquery, CnnString1);
            if (dt1.Rows.Count > 0)
            {
                float Amount = 0;
                string filename = "HSRPProductionSheet- " + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".pdf";
                String StringField = String.Empty;
                String StringAlert = String.Empty;
                StringBuilder bb = new StringBuilder();
                // Document document = new Document();
                //  iTextSharp.text.Document document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 0, 0, 0, 0);
                Document document = new Document(new iTextSharp.text.Rectangle(188f, 124f), -30, -30, 8, 0);
                document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());


                BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
                // iTextSharp.text.Font times = new Font(bfTimes, 12, Font.ITALIC, Color.Black);
                // Creates a Writer that listens to this document and writes the document to the Stream of your choice:
                string PdfFolder = ConfigurationManager.AppSettings["PdfFolder"].ToString() + filename;
                PdfWriter.GetInstance(document, new FileStream(PdfFolder, FileMode.Create));
                //Opens the document:
                document.Open();

                PdfPTable table;

                table = new PdfPTable(9);
                var colWidthPercentages = new[] { 55f, 125f, 100f, 80f, 75f, 100f, 100f, 100f, 100f };
                var colheightpercentage = new[] { 2f };

                table.SetWidths(colWidthPercentages);


                string strQueryDate = "SELECT CONVERT(VARCHAR,GETDATE(),103)";
                DataTable dtDate = Utils.GetDataTable(strQueryDate, CnnString1);

                table.TotalWidth = 800f;
                table.LockedWidth = true;
                PdfPCell cell786a = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell786a.Colspan = 5;
                cell786a.BorderWidthLeft = 0f;
                cell786a.BorderWidthRight = 0f;
                cell786a.BorderWidthTop = 0f;
                cell786a.BorderWidthBottom = 0f;
                //cell120911.HorizontalAlignment = Element.ALIGN_LEFT;
                cell786a.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell786a);

                PdfPCell cell120911 = new PdfPCell(new Phrase("HR Cash Collection Detail Report", new iTextSharp.text.Font(bfTimes, 12f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell120911.Colspan = 4;
                cell120911.BorderWidthLeft = 0f;
                cell120911.BorderWidthRight = 0f;
                cell120911.BorderWidthTop = 0f;
                cell120911.BorderWidthBottom = 0f;
                //cell120911.HorizontalAlignment = Element.ALIGN_LEFT;
                cell120911.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell120911);

                PdfPCell cell1209111 = new PdfPCell(new Phrase("Report Generation Date: " + dtDate.Rows[0][0].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell1209111.Colspan = 3;
                cell1209111.BorderWidthLeft = 0f;
                cell1209111.BorderWidthRight = 0f;
                cell1209111.BorderWidthTop = 0f;
                cell1209111.BorderWidthBottom = 0f;
                //cell120911.HorizontalAlignment = Element.ALIGN_LEFT;
                cell1209111.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1209111);



                PdfPCell cell12091 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell12091.Colspan = 3;
                cell12091.BorderWidthLeft = 0f;
                cell12091.BorderWidthRight = 0f;
                cell12091.BorderWidthTop = 0f;
                cell12091.BorderWidthBottom = 0f;
                cell12091.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12091);

                PdfPCell cell12093 = new PdfPCell(new Phrase("Report Date " + strtodate, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                //PdfPCell cell12093 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell12093.Colspan = 3;
                cell12093.BorderWidthLeft = 0f;
                cell12093.BorderWidthRight = 0f;
                cell12093.BorderWidthTop = 0f;
                cell12093.BorderWidthBottom = 0f;

                cell12093.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12093);

                PdfPCell cell12 = new PdfPCell(new Phrase("SR.No", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell12.Colspan = 1;
                cell12.BorderWidthLeft = 1f;
                cell12.BorderWidthRight = .8f;
                cell12.BorderWidthTop = 0.8f;
                cell12.BorderWidthBottom = 0.8f;
                cell12.FixedHeight = -1;
                cell12.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12);

                PdfPCell cell1210 = new PdfPCell(new Phrase("RTOLocationCode", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell1210.Colspan = 1;
                cell1210.BorderWidthLeft = 0f;
                cell1210.BorderWidthRight = .8f;
                cell1210.BorderWidthTop = 0.8f;
                cell1210.BorderWidthBottom = 0.8f;

                cell1210.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1210);

                PdfPCell cell1213 = new PdfPCell(new Phrase("VehicleRegno", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell1213.Colspan = 1;
                cell1213.BorderWidthLeft = 0f;
                cell1213.BorderWidthRight = .8f;
                cell1213.BorderWidthTop = 0.8f;
                cell1213.BorderWidthBottom = 0.8f;

                cell1213.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1213);


                PdfPCell cell1209 = new PdfPCell(new Phrase("Owner Name", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK))); //6
                cell1209.Colspan = 1;
                cell1209.BorderWidthLeft = 0f;
                cell1209.BorderWidthRight = .8f;
                cell1209.BorderWidthTop = 0.8f;
                cell1209.BorderWidthBottom = 0.8f;

                cell1209.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1209);

                PdfPCell cell12233 = new PdfPCell(new Phrase("Engine No", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell12233.Colspan = 1;
                cell12233.BorderWidthLeft = 0f;
                cell12233.BorderWidthRight = .8f;
                cell12233.BorderWidthTop = 0.8f;
                cell12233.BorderWidthBottom = 0.8f;

                cell12233.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12233);

                PdfPCell cell12234 = new PdfPCell(new Phrase("Chassis No", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell12234.Colspan = 1;
                cell12234.BorderWidthLeft = 0f;
                cell12234.BorderWidthRight = .8f;
                cell12234.BorderWidthTop = 0.8f;
                cell12234.BorderWidthBottom = 0.8f;

                cell12234.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12234);

                PdfPCell cell12235 = new PdfPCell(new Phrase("Vehicle Type", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell12235.Colspan = 1;
                cell12235.BorderWidthLeft = 0f;
                cell12235.BorderWidthRight = .8f;
                cell12235.BorderWidthTop = 0.8f;
                cell12235.BorderWidthBottom = 0.8f;

                cell12235.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12235);

                PdfPCell cell12236 = new PdfPCell(new Phrase("Vehicle Class", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell12236.Colspan = 1;
                cell12236.BorderWidthLeft = 0f;
                cell12236.BorderWidthRight = .8f;
                cell12236.BorderWidthTop = 0.8f;
                cell12236.BorderWidthBottom = 0.8f;

                cell12236.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12236);

                PdfPCell cell12237 = new PdfPCell(new Phrase("Total Amount", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell12237.Colspan = 1;
                cell12237.BorderWidthLeft = 0f;
                cell12237.BorderWidthRight = .8f;
                cell12237.BorderWidthTop = 0.8f;
                cell12237.BorderWidthBottom = 0.8f;

                cell12237.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12237);


                int j = 0;
                int total = 0;
                for (int i = 0; i <= dt1.Rows.Count - 1; i++)
                {
                    total = total + 1;
                    //============================================================ ajay end ======================================================================
                    PdfPCell cell13 = new PdfPCell(new Phrase("" + dt1.Rows[i]["S.NO"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell13.Colspan = 1;
                    cell13.BorderWidthLeft = 0.8f;
                    cell13.BorderWidthRight = 0f;
                    cell13.BorderWidthTop = 0f;
                    cell13.BorderWidthBottom = .5f;
                    cell13.MinimumHeight = 0f;//25
                    cell13.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell13);

                    PdfPCell cell1212 = new PdfPCell(new Phrase(dt1.Rows[i]["RTOLocationCode"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell1212.Colspan = 1;
                    cell1212.MinimumHeight = 0f;//25

                    cell1212.BorderWidthLeft = 0.8f;
                    cell1212.BorderWidthRight = .8f;
                    cell1212.BorderWidthTop = 0f;
                    cell1212.BorderWidthBottom = .5f;

                    cell1212.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell1212);



                    PdfPCell cell1214 = new PdfPCell(new Phrase(dt1.Rows[i]["VehicleRegNo"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell1214.Colspan = 1;
                    cell1214.MinimumHeight = 0f;//25

                    cell1214.BorderWidthLeft = 0f;
                    cell1214.BorderWidthRight = .8f;
                    cell1214.BorderWidthTop = 0f;
                    cell1214.BorderWidthBottom = .5f;
                    cell1214.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell1214);


                    PdfPCell cell1211 = new PdfPCell(new Phrase(dt1.Rows[i]["OwnerName"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK))); //6
                    cell1211.Colspan = 1;
                    cell1211.MinimumHeight = 0f;//25

                    cell1211.BorderWidthLeft = 0f;
                    cell1211.BorderWidthRight = 0.8f;
                    cell1211.BorderWidthTop = 0f;
                    cell1211.BorderWidthBottom = .5f;
                    cell1211.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell1211);



                    PdfPCell cell1219 = new PdfPCell(new Phrase(dt1.Rows[i]["EngineNo"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell1219.Colspan = 1;
                    cell1219.MinimumHeight = 0f;//25
                    cell1219.BorderWidthLeft = 0f;
                    cell1219.BorderWidthRight = .8f;
                    cell1219.BorderWidthTop = 0f;
                    cell1219.BorderWidthBottom = .5f;
                    cell1219.HorizontalAlignment = 0;
                    table.AddCell(cell1219);


                    PdfPCell cell1215 = new PdfPCell(new Phrase(dt1.Rows[i]["ChassisNo"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell1215.Colspan = 1;
                    cell1215.MinimumHeight = 0f;//25
                    cell1215.BorderWidthLeft = 0f;
                    cell1215.BorderWidthRight = .8f;
                    cell1215.BorderWidthTop = 0f;
                    cell1215.BorderWidthBottom = .5f;
                    cell1215.HorizontalAlignment = 0;
                    table.AddCell(cell1215);

                    PdfPCell cell1216 = new PdfPCell(new Phrase(dt1.Rows[i]["VehicleType"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell1216.Colspan = 1;
                    cell1216.MinimumHeight = 0f;//25
                    cell1216.BorderWidthLeft = 0f;
                    cell1216.BorderWidthRight = .8f;
                    cell1216.BorderWidthTop = 0f;
                    cell1216.BorderWidthBottom = .5f;
                    cell1216.HorizontalAlignment = 0;
                    table.AddCell(cell1216);

                    PdfPCell cell1217 = new PdfPCell(new Phrase(dt1.Rows[i]["VehicleClass"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell1217.Colspan = 1;
                    cell1217.MinimumHeight = 0f;//25
                    cell1217.BorderWidthLeft = 0f;
                    cell1217.BorderWidthRight = .8f;
                    cell1217.BorderWidthTop = 0f;
                    cell1217.BorderWidthBottom = .5f;
                    cell1217.HorizontalAlignment = 0;
                    table.AddCell(cell1217);

                    PdfPCell cell1218 = new PdfPCell(new Phrase(dt1.Rows[i]["TotalAmount"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell1218.Colspan = 1;
                    cell1218.MinimumHeight = 0f;//25
                    cell1218.BorderWidthLeft = 0f;
                    cell1218.BorderWidthRight = .8f;
                    cell1218.BorderWidthTop = 0f;
                    cell1218.BorderWidthBottom = .5f;
                    cell1218.HorizontalAlignment = 0;
                    table.AddCell(cell1218);

                }

                try
                {
                    PdfPCell cell1209340 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    cell1209340.Colspan = 16;
                    cell1209340.BorderWidthLeft = .8f;
                    cell1209340.BorderWidthRight = .8f;
                    cell1209340.BorderWidthTop = .8f;
                    cell1209340.BorderWidthBottom = .5f;
                    cell1209340.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell1209340);

                    PdfPCell cell12241 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell12241.Colspan = 7;
                    cell12241.BorderWidthLeft = 0f;
                    cell12241.BorderWidthRight = 0f;
                    cell12241.BorderWidthTop = 0f;
                    cell12241.BorderWidthBottom = 0f;
                    cell12241.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell12241);
                    document.Add(table);
                    document.Close();

                    HttpContext context = HttpContext.Current;
                    context.Response.ContentType = "Application/pdf";
                    context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                    context.Response.WriteFile(PdfFolder);
                    context.Response.End();

                }
                catch
                {

                }
            }
            else
            {
                //lblErrMsg.Text = "No Record Found!!";
            }
        }

        protected void btnPdfDownload_Click(object sender, EventArgs e)
        {
            pdf(dt1);
        }
    }
}