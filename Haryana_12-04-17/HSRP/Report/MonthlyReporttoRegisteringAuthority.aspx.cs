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
using iTextSharp.text.pdf;


namespace HSRP.Report
{
    public partial class MonthlyReporttoRegisteringAuthority : System.Web.UI.Page
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
                    FilldropDownListYear();
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


        private void FilldropDownListYear()
        {
            for (int i = 2012; i <= System.DateTime.Now.Year; i++)
            {
                ListItem li = new ListItem();
                li.Text = i.ToString();
                li.Text = i.ToString();
                ddlYear.Items.Add(li);
            }
        }

        protected void DropDownListStateName_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnexport_Click(object sender, EventArgs e)
        {
            DataTable GetAddress;
            string Address = string.Empty;

            GetAddress = Utils.GetDataTable("select * from HSRPState WHERE HSRP_StateID='" + HSRP_StateID1 + "'", CnnString1);
            if (GetAddress.Rows[0]["pincode"] != "" || GetAddress.Rows[0]["pincode"] != null)
            {
                Address = GetAddress.Rows[0]["CompanyName"].ToString() + "\n" + GetAddress.Rows[0]["Address1"].ToString();
            }

            #region Commented Query
            //SQLString = "select kk.RtolocationName,dd.[New Order],kk.Closed,dd.[New Order]-kk.Closed as back from (select (select top 1 RtolocationName from rtolocation "+
            //            "where rtolocationid=r.distrelation) RtolocationName,count(*) as 'New Order' from hsrprecords h inner join rtolocation r on h.rtolocationid=r.RTOLocationID "+
            //            "where h.HSRP_StateID=5 and month(h.hsrprecord_creationdate)='" + ddlMonth.SelectedValue + "' and Year(h.hsrprecord_creationdate)='" + ddlYear.SelectedItem + "' and h.hsrprecord_creationdate > '2014/05/16 00:00:00' " +
            //            "group by r.distrelation )dd inner join " +
            //            "(select (select top 1 RtolocationName from rtolocation where rtolocationid=r.distrelation) RtolocationName,count(*) as 'Closed' from hsrprecords h inner join "+
            //            "rtolocation r on h.rtolocationid=r.RTOLocationID  where h.HSRP_StateID=5 and month(h.ordercloseddate)='"+ddlMonth.SelectedValue+"' and Year(h.ordercloseddate)='"+ddlYear.SelectedItem+"' "+
            //            "group by r.distrelation ) kk on dd.RtolocationName=kk.RtolocationName order by kk.rtolocationname";
            #endregion

            SQLString = "select (select top 1 RtolocationName from rtolocation where rtolocationid=r.distrelation) RtolocationName," +
                       "count(*) as 'NewOrder',count(case when h.OrderStatus='Closed' then 1 end) as 'Closed' from hsrprecords h " +
                       "inner join rtolocation r on h.rtolocationid=r.RTOLocationID  and r.rtolocationcode=left(h.vehicleregno,4) where h.HSRP_StateID='" + ddlState.SelectedValue + "' and month(h.hsrprecord_creationdate)='" + ddlMonth.SelectedValue + "'" +
                       "and Year(h.hsrprecord_creationdate)='" + ddlYear.SelectedItem + "' and convert(date,h.hsrprecord_creationdate) > '2014-05-15' and ownername is not null and"+
                        "hsrp_front_lasercode not in ( 'AA271055126','AA271072995','AA170855418','AA210591749','AA210597574','AA210599883','AA170855406','AA170871225','AA210599855','AA170850011','AA251393551','AA170857134','AA170855432','AAA10861157','AA210599873','AA251569860','AA170855444','AA251315167','AA210597584','AA170873704','AA271074025','AA220216982','AA220216956','AA170857252','AA210597560','AA271223713','AA170856472','AA210597552','AA170850164','AA170855430','AA210599869','AA210597558','AA210584310','AA170535357','AA210599851','AA170856464','AA170855429','AA220220312','AA170855414','AA170855412','AA210597564','AA170856484','AA270954164','AA170908478','AA170855407','AAC40050146','AA210599853','AA210915750','AA210597570','AA210599875','AA170855421','AA170856498','AA170856480','AA170855435','AA271020815','AA210597554','AA170855417','AA251611685','AA170855442','AA251197251','AA220220310','AA271032822','AA170855401','AA210597586','AA170911124','AA170855408','AA210597580','AA271248115','AA270942098','AA170855419','AA220220306','AA210584322','AA251395073','AA170856478','AA271062786','AA210597556','AA271049246','AA170855405','AA271055122','AA210597592','AA270739527','AA170855402','AA210597576','AA170855428','AA270758806','AA251385101','AA170851105','AA210597578','AA170856485','AA170855413','AA251203769','AAC60082639','AA170855409','AA210597572','AA271150518','AA210599877','AA170855425','AA170855415','AA170855416','AA270951141','AA271262087','AA170850176','AA210573349','AA210589716','AA210597596','AA210599879','AA170856495','AA210597594','AA210599881','AA251648245','AA170850169','AA270966578','AA270888770','AA170850182','AA270888762','AA210597588','AA170856479','AA170908931','AA270969695','AA210594113','AA170861370','AA210581973','AA271243277','AA170855404','AA170856494','AA271045500','AA170855410','AA170855431','AA170850177','AA120022126','AA251322504','AA271075549','AA170856491','AA220220314','AA210597582','AA220220308','AA170856469','AA170855440','AA210597566','AA170855423','AA210597568','AA210597562','AA170855426','AA170856470') and netamount >0 group by r.distrelation order by RtolocationName";

            DataTable dtRecord = Utils.GetDataTable(SQLString, CnnString1);

            if (dtRecord.Rows.Count > 0)
            {
                string Month = ddlMonth.SelectedItem.ToString() + "'" + ddlYear.SelectedItem.ToString();

                #region Common Code For PDf
                string filename = "Report 3-D_" + ddlState.SelectedItem + "_" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".pdf";

                iTextSharp.text.Document document = new iTextSharp.text.Document();

                BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);

                // iTextSharp.text.Font times = new Font(bfTimes, 12, Font.ITALIC, Color.Black);
                //Creates a Writer that listens to this document and writes the document to the Stream of your choice:
                string PdfFolder = ConfigurationManager.AppSettings["PdfFolder"].ToString() + filename;
                PdfWriter.GetInstance(document, new FileStream(PdfFolder, FileMode.Create));

                //Opens the document:
                document.Open();

                PdfPTable table1 = new PdfPTable(6);
                var colWidthPercentages = new[] { 8f, 35f, 20f, 20f, 15f, 40f };
                table1.SetWidths(colWidthPercentages);

                #endregion
                #region Row 1
                PDFRows(bfTimes, table1, 6, "LINK UTSAV AUTO SYETEMS PVT. LTD." + "\n" + "State Office- E3/39,Arera Colony,Bhopal (MP)", 8, 1, "N", 1, 1, 1, 1);

                #endregion
                #region Row 2

                PDFRows(bfTimes, table1, 3, "Report : 3 (Schedule - D)", 8, 1, "N", 1, 0, 0, 1);
                PDFRows(bfTimes, table1, 3, "Monthly Report to Registering Authority", 8, 1, "N", 1, 1, 0, 1);

                #endregion
                #region Row 3
                //PDFRows(bfTimes, table1, 3, "RTO LOCATION : ", 8, 1, "N", 1, 0, 0, 1);
                //PDFRows(bfTimes, table1, 3, "", 8, 1, "N", 1, 1, 0, 1);
                PDFRows(bfTimes, table1, 3, "MONTH : ", 8, 1, "N", 1, 0, 0, 1);
                PDFRows(bfTimes, table1, 3, Month, 8, 1, "N", 0, 1, 0, 1);
                #endregion
                #region Row 4
                PDFRows(bfTimes, table1, 1, "S.No", 8, 0, "N", 1, 1, 0, 1, optionalWidth: 10);
                PDFRows(bfTimes, table1, 1, "Registering Authority", 8, 0, "N", 0, 1, 0, 1);
                PDFRows(bfTimes, table1, 1, "Application Received/  New Registrations / Existing Vehicles", 8, 0, "N", 0, 1, 0, 1);
                PDFRows(bfTimes, table1, 1, "Registration Plates Sipplied/ New Registration/ Existing Vehicle", 8, 0, "N", 0, 1, 0, 1);
                PDFRows(bfTimes, table1, 1, "Back Log (if any)", 8, 0, "N", 0, 1, 0, 1);
                PDFRows(bfTimes, table1, 1, "Remarks", 8, 0, "N", 0, 1, 0, 1);
                #endregion
                #region Dynamics Rows
                for (int iRowCount = 0; iRowCount < dtRecord.Rows.Count; iRowCount++)
                {
                    PDFRows(bfTimes, table1, 1, (iRowCount + 1).ToString(), 8, 0, "N", 1, 1, 0, 1, optionalWidth: 10);
                    PDFRows(bfTimes, table1, 1, dtRecord.Rows[iRowCount]["Rtolocationname"].ToString(), 8, 0, "N", 0, 1, 0, 1);
                    PDFRows(bfTimes, table1, 1, dtRecord.Rows[iRowCount]["NewOrder"].ToString(), 8, 0, "N", 0, 1, 0, 1);
                    PDFRows(bfTimes, table1, 1, dtRecord.Rows[iRowCount]["Closed"].ToString(), 8, 0, "N", 0, 1, 0, 1);
                    int diff = int.Parse(dtRecord.Rows[iRowCount]["NewOrder"].ToString()) - int.Parse(dtRecord.Rows[iRowCount]["Closed"].ToString());
                    PDFRows(bfTimes, table1, 1, diff.ToString(), 8, 0, "N", 0, 1, 0, 1);
                    string strRemarks = "";
                    if (diff > 0)
                    {
                        strRemarks = "Vehicle Not Brought for affixation";
                    }
                    PDFRows(bfTimes, table1, 1, strRemarks, 8, 0, "N", 0, 1, 0, 1);
                }
                #endregion

                String strTotalNewOrder = dtRecord.Compute("sum(NewOrder)", "").ToString();
                String strTotalClosed = dtRecord.Compute("sum(Closed)", "").ToString();
                #region Total Column
                PDFRows(bfTimes, table1, 2, "Total", 8, 1, "B", 1, 1, 0, 1, optionalWidth: 10);
                PDFRows(bfTimes, table1, 1, strTotalNewOrder, 8, 0, "B", 0, 1, 0, 1, optionalWidth: 10);
                PDFRows(bfTimes, table1, 1, strTotalClosed, 8, 0, "B", 0, 1, 0, 1, optionalWidth: 10);
                PDFRows(bfTimes, table1, 1, (int.Parse(strTotalNewOrder) - int.Parse(strTotalClosed)).ToString(), 8, 0, "B", 0, 1, 0, 1, optionalWidth: 10);
                PDFRows(bfTimes, table1, 1, "", 8, 0, "B", 0, 1, 0, 1, optionalWidth: 10);
                #endregion

                document.Add(table1);
                document.Close();
                HttpContext context = HttpContext.Current;
                context.Response.ContentType = "Application/pdf";
                context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                context.Response.WriteFile(PdfFolder);
                context.Response.End();

            }
        }
        #region Pdf Function
        private static void PDFRows(BaseFont bfTimes, PdfPTable table1, int iColSpan, String strText, int iFontsize, int ialignMent, string strRowType, int iBorderWidthLeft, int iBorderWidthRight, int iBorderWidthTop, int iBorderWidthBottom, int optionalHeight = 0, int optionalWidth = 0)
        {
            PdfPCell cell;
            if (strRowType == "B")
            {
                cell = new PdfPCell(new iTextSharp.text.Phrase(strText, new iTextSharp.text.Font(bfTimes, iFontsize, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
            }
            else
            {
                cell = new PdfPCell(new iTextSharp.text.Phrase(strText, new iTextSharp.text.Font(bfTimes, iFontsize, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
            }
            cell.Colspan = iColSpan;
            cell.BorderColor = iTextSharp.text.BaseColor.BLACK;
            cell.BorderWidthLeft = iBorderWidthLeft;
            cell.BorderWidthRight = iBorderWidthRight;
            cell.BorderWidthTop = iBorderWidthTop;
            cell.BorderWidthBottom = iBorderWidthBottom;
            if (optionalWidth != 0)
            {
                cell.NoWrap = true;
            }
            cell.HorizontalAlignment = ialignMent; //0=Left, 1=Centre, 2=Right
            table1.AddCell(cell);
        }
        #endregion
    }
}