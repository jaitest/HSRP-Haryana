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

namespace HSRP.Report
{
    public partial class MPcashcollection : System.Web.UI.Page
    {
        int UserType1;
        string CnnString1 = string.Empty;
        string HSRP_StateID1 = string.Empty;
        string RTOLocationID1 = string.Empty;
        string strUserID1 = string.Empty;
        int intHSRPStateID1;
        int intRTOLocationID1;
        string SQLString1 = string.Empty;
        string OrderType;
        string recordtype = string.Empty;
        //DateTime OrderDate1;
        string SQLString = string.Empty;
        string CompanyName=string.Empty;
        string Address=string.Empty;
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
                    InitialSetting();
                    try
                    {
                        if (UserType1.Equals(0))
                        {
                           
                            ddlState.Visible = true;
                            
                            FilldropDownListOrganization();

                        }
                        else
                        {

                          
                            ddlState.Visible = true;
                           
                            FilldropDownListOrganization();
                            FilldropDownListClient();
                           
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
            if (UserType1.Equals(0))
            {
                SQLString1 = "select HSRPStateName,HSRP_StateID from HSRPState  where ActiveStatus='Y' Order by HSRPStateName";
                Utils.PopulateDropDownList(ddlState, SQLString1.ToString(), CnnString1, "--Select State--");
            }
            else
            {
                SQLString1 = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID=" + HSRP_StateID1 + " and ActiveStatus='Y' Order by HSRPStateName";
                DataSet dts = Utils.getDataSet(SQLString1, CnnString1);
                ddlState.DataSource = dts;
                ddlState.DataBind();
            }
        }

        
        private void FilldropDownListClient()
        {
            if (UserType1.Equals(0))
            {
                int.TryParse(ddlState.SelectedValue, out intHSRPStateID1);
                SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + intHSRPStateID1 + " and ActiveStatus!='N'  Order by RTOLocationName";

                 Utils.PopulateDropDownList(ddlRtoLocation, SQLString.ToString(), CnnString1, "--Select Location--");
                
            }
            else
            {

                SQLString = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID where  a.LocationType!='District' and UserRTOLocationMapping.UserID='" + strUserID1 + "' ";

                 DataSet dss = Utils.getDataSet(SQLString, CnnString1);
                 ddlRtoLocation.DataSource = dss;
                 ddlRtoLocation.DataBind();
            }
        }
        private void InitialSetting()
        {
            string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            string MaxDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            
            OrderDatefrom.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            OrderDatefrom.MaxDate = DateTime.Parse(MaxDate);
            CalendarOrderDatefrom.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarOrderDatefrom.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
        }

        protected void DropDownListStateName_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilldropDownListClient();
        }

        private static void GenerateCell(PdfPTable table, int iSpan, int iLeftWidth, int iRightWidth, int iTopWidth, int iBottomWidth, int iAllign, int iFont, string strText, int iRowHeight, int iRowWidth)
        {
            PdfPCell newCellPDF = null;
            BaseFont bfTimes1 = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
            if (iFont.Equals(0))
            {
                newCellPDF = new PdfPCell(new Phrase(strText, new iTextSharp.text.Font(bfTimes1, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
            }
           else if (iFont.Equals(1))
            {
                newCellPDF = new PdfPCell(new Phrase(strText, new iTextSharp.text.Font(bfTimes1, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
            }
            newCellPDF.Colspan = iSpan;
            newCellPDF.BorderWidthLeft = iLeftWidth;
            newCellPDF.BorderWidthRight = iRightWidth;
            newCellPDF.BorderWidthTop = iTopWidth;
            newCellPDF.BorderWidthBottom = iBottomWidth;
            newCellPDF.HorizontalAlignment = iAllign;
            if (!iRowHeight.Equals(0))
            {
                newCellPDF.FixedHeight = iRowHeight;
            }
            if (!iRowWidth.Equals(0))
            {
            }
            table.AddCell(newCellPDF);
        }
        protected void btnexport_Click(object sender, EventArgs e)
        {
                string RtoName = ddlRtoLocation.SelectedItem.ToString();
                string CashCollectionDate=OrderDatefrom.SelectedDate.ToString("dd/MMM/yyyy");
                //string AffixiationDate=OrderDatefrom.SelectedDate.AddDays(4).ToString("dd/MMM/yyyy");
                string Sqlquery = "select  [dbo] .[GetAffxDate_Insert_MP1] ('"+CashCollectionDate+"','5') as AffDate";
                DataTable dtDate = Utils.GetDataTable(Sqlquery, CnnString1);
                string AffixiationDate = Convert.ToDateTime(dtDate.Rows[0]["AffDate"]).ToString("dd/MMM/yyyy");
                HttpContext context = HttpContext.Current;
                string filename = "MP_CashCollection" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString()+".pdf";
                string SQLString = String.Empty;
                Document document = new Document();
                BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);


                string PdfFolder = ConfigurationManager.AppSettings["PdfFolder"].ToString() + filename;
                PdfWriter.GetInstance(document, new FileStream(PdfFolder, FileMode.Create));
                document.Open();
                string StrComapnyName="select CompanyName,Address1 from hsrpstate where hsrp_stateid='"+ddlState.SelectedValue.ToString()+"'";
                DataTable dt=Utils.GetDataTable(StrComapnyName,CnnString1);
              
                CompanyName=dt.Rows[0]["CompanyName"].ToString();
                Address=dt.Rows[0]["Address1"].ToString();


                string StrRtoLcatioCode = "select Rtolocationcode from rtolocation where HSRP_StateID='" + ddlState.SelectedValue.ToString() + "' and Rtolocationid='" + ddlRtoLocation.SelectedValue.ToString() + "'";
                DataTable dtrtocode = Utils.GetDataTable(StrRtoLcatioCode, CnnString1);
                string RTOCode = dtrtocode.Rows[0]["Rtolocationcode"].ToString();

                SQLString = "select row_number() over (order by vehicleregno) as SNo, vehicleregno as VehicleNo,ownername as OwnerName,mobileno from hsrprecords where hsrp_stateid='" + ddlState.SelectedValue.ToString() + "' and RTOLocationId='" + ddlRtoLocation.SelectedValue.ToString() + "' and Vehicleregno like '" + RTOCode + "%' and convert(date,HSRPRecord_CreationDate)='" + OrderDatefrom.SelectedDate + "' and HSRPRecord_CreationDate > '2014/05/16 00:00:00' and netamount > 0";
               // SQLString = "select row_number() over (order by vehicleregno) as SNo, vehicleregno as VehicleNo,ownername as OwnerName,mobileno from hsrprecords where hsrp_stateid='" + ddlState.SelectedValue.ToString() + "' and RTOLocationId='" + ddlRtoLocation.SelectedValue.ToString() + "' and convert(date,HSRPRecord_CreationDate)='" + OrderDatefrom.SelectedDate + "' and HSRPRecord_CreationDate > '2014/05/16 00:00:00' and netamount > 0";
                DataTable dtResult = Utils.GetDataTable(SQLString, CnnString1);
               
                
                //PdfPTable table2 = new PdfPTable(8);
               // PdfPTable table1 = new PdfPTable(8);
                PdfPTable table = new PdfPTable(8);
                table.TotalWidth = 1000f;

               GenerateCell(table, 8, 0, 0, 0, 0, 1, 1, "", 50, 0);
               GenerateCell(table, 8, 1, 1, 1, 1, 1, 0, CompanyName, 15, 0);
               GenerateCell(table, 8, 1, 1, 0, 0, 1, 0, " State Office- E3/39,Arera Colony,Bhopal (MP)", 15, 0);
               GenerateCell(table, 8, 1, 1, 1, 0, 1, 0, "Report:1", 15, 0);
               GenerateCell(table, 4, 1, 0, 0, 0, 0, 0, "Location:", 15, 0);
               GenerateCell(table, 4, 0, 1, 0, 0, 0, 0, RtoName, 15, 0);
               GenerateCell(table, 4, 1, 0, 0, 0, 0, 0, "Cash Collection Date:-", 15, 0);
               GenerateCell(table, 4, 0, 1, 0, 0, 0, 0, CashCollectionDate, 15, 0);
               GenerateCell(table, 4, 1, 0, 0, 0, 0, 0, "Affixiation due Date:-", 15, 0);
               GenerateCell(table, 4, 0, 1, 0, 0, 0, 0, AffixiationDate, 15, 0);

               GenerateCell(table, 8, 1, 1, 1, 1, 1, 1, "", 10, 0);
            
               
               GenerateCell(table, 2, 1, 1, 0, 1, 1, 0, "S.No:", 20, 0);
               GenerateCell(table, 2, 0, 1, 0, 1, 1, 0, "Vehicle No", 20, 0);
               GenerateCell(table, 2, 0, 1, 0, 1, 1, 0, "Owner Name", 20, 0);
               GenerateCell(table, 2, 0, 1, 0, 1, 1, 0, "Mobile No", 20, 0);
               for(int i=0; i<dtResult.Rows.Count;i++)
               {
               GenerateCell(table, 2, 1, 1, 0, 1, 1, 1, dtResult.Rows[i]["SNo"].ToString(), 20, 0);
               GenerateCell(table, 2, 0, 1, 0, 1, 1, 1, dtResult.Rows[i]["VehicleNo"].ToString().ToUpper(), 20, 0);
               GenerateCell(table, 2, 0, 1, 0, 1, 1, 1, dtResult.Rows[i]["OwnerName"].ToString(), 20, 0);
               GenerateCell(table, 2, 0, 1, 0, 1, 1, 1, dtResult.Rows[i]["mobileno"].ToString(), 20, 0);
               }
               //document.Add(table1);
               document.Add(table);
               document.NewPage();

               document.Close();
               context.Response.ContentType = "Application/pdf";
               context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
               context.Response.WriteFile(PdfFolder);
               context.Response.End();
               }
            
        }

}
