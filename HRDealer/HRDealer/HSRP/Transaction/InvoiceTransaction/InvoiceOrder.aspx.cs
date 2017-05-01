using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
using HSRP.Transaction.InvoiceTransaction;

namespace HSRP.Master.InvoiceMaster
{
    public partial class InvoiceOrder : System.Web.UI.Page
    {
        string Mode;
        string UserType = string.Empty;
        // HiddenField InvoiceID ;
        DataProvider.BAL blCustomer = new DataProvider.BAL();

        //HiddenField status ;
        DataProvider.BAL blorder = new DataProvider.BAL();
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        List<POorderbl> lst = new List<POorderbl>();
        List<POorderbl> lst2 = new List<POorderbl>();
        List<string> lst1 = new List<string>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                //FillEmployeeGrid();
                lst.Clear();
                lst1.Clear();
                // ShowInvoiceOrder();
                // gridbind();
                //GDPanal.Visible = false;
                ShowCustomerDetail();
                // ShowProductDetail();
                Mode = Request.QueryString["Mode"].ToString();
                if (Mode == "Edit")
                {
                    btnShowPopup.Visible = false;
                    btnUpdate.Visible = true;
                    H_headerID1.Value = Request.QueryString["HeaderID"].ToString();
                    string id = H_headerID1.Value.ToString();
                    editInvoiceOrder(id);

                }
                else if (Mode == "New")
                {
                    btnShowPopup.Visible = true;
                    btnUpdate.Visible = false;
                }

            }
        }


        private void FillEmployeeGrid()
        {
            DataSet ds = new DataSet();

            if (!string.IsNullOrEmpty(H_headerID1.Value.ToString()))
            {
                ds = blorder.ShowInsertValueInvoice(H_headerID1.Value.ToString());
                gvEG.DataSource = ds;
                gvEG.DataBind();
            }
            else
            {
                ds = blorder.ShowInsertValueInvoice(H_ID.Value.ToString());
                gvEG.DataSource = ds;
                gvEG.DataBind();
            }


        }



        private void editInvoiceOrder(string POheaderID)
        {
            ds = blorder.editInvoiceOrderbyID(POheaderID.ToString());

            if (ds.Tables[0].Rows.Count > 0)
            {

                ddlCustomerName.SelectedValue = ds.Tables[0].Rows[0]["CustomerID"].ToString();
                txtTransportName.Text = ds.Tables[0].Rows[0]["TransportName"].ToString();
                //ddlTransportVia.SelectedItem.Text = ds.Tables[0].Rows[0]["TransportName"].ToString();
                txtGoodsReceiptNote.Text = ds.Tables[0].Rows[0]["GoodsReceiptNote"].ToString();
                ddlTransportVia.SelectedItem.Text = ds.Tables[0].Rows[0]["TransportVia"].ToString();
                txtBillingAddress.Text = ds.Tables[0].Rows[0]["BillingAddress"].ToString();
                txtShippingAddress.Text = ds.Tables[0].Rows[0]["ShippingAddress"].ToString();


                HStatus.Value = ds.Tables[0].Rows[0]["Status"].ToString();
                
             
                if (HStatus.Value == "Valid")
                {
                    chkActive.Checked = true;
                }
                else if (HStatus.Value == "In Valid")
                {
                    chkActive.Checked = false;
                }


                gvEG.DataSource = ds;
                gvEG.DataBind();

            }
        }
        string Staus;



        protected void btnShowPopup_Click(object sender, EventArgs e)
        {

            lst1.Add(ddlCustomerName.SelectedValue);
            lst1.Add(txtTransportName.Text);

            lst1.Add(txtGoodsReceiptNote.Text);
            lst1.Add(ddlTransportVia.SelectedItem.ToString());
            lst1.Add(txtBillingAddress.Text);
            lst1.Add(txtShippingAddress.Text);
            if (chkActive.Checked == true)
            {
                Staus = "Valid";
            }
            else
            {
                Staus = "In Valid";
            }
            lst1.Add(Staus);
            int k = blorder.InsertHeader(lst1);

            dt = blorder.FetchHeaderID();
            H_ID.Value = dt.Rows[0]["HeaderID"].ToString();

            if (k > 0)
            {
               
                divGrid.Visible = true;

                DataSet ds = new DataSet();
                ds = blorder.ShowInsertValueInvoice(H_ID.Value.ToString());
                gvEG.DataSource = ds;
                gvEG.DataBind();

                //showgv();
            }
        }



        public void ShowVendorDetail()
        {
            dt = blorder.ShowVendorDetail();
            if (dt.Rows.Count > 0)
            {

                //ddlVendorName.DataSource = dt;
                //ddlVendorName.DataTextField = "Name";
                //ddlVendorName.DataValueField = "VendorID";
                //ddlVendorName.DataBind();
                //ddlVendorName.Items.Insert(0, "Select Vendor Name");
                //ddlVendorName.Items[0].Value = "0";

            }
        }


        protected void gvEG_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            dt = blorder.ShowInvoiceProduct();
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlDepartment = (DropDownList)e.Row.FindControl("ddlProductName");
                if (ddlDepartment != null)
                {
                    ddlDepartment.DataSource = dt;
                    ddlDepartment.DataTextField = "ProductName";
                    ddlDepartment.DataValueField = "ProductID";
                    ddlDepartment.DataBind();
                    ddlDepartment.Items.Insert(0, ".....Select Product Name.....");
                    ddlDepartment.Items[0].Value = "0";
                    ddlDepartment.SelectedValue = gvEG.DataKeys[e.Row.RowIndex].Values[0].ToString();

                  
                }
            }
            if (e.Row.RowType == DataControlRowType.EmptyDataRow)
            {
                DropDownList ddlDepartment = (DropDownList)e.Row.FindControl("ddlProductName");
                if (ddlDepartment != null)
                {
                    ddlDepartment.DataSource = dt;
                    ddlDepartment.DataTextField = "ProductName";
                    ddlDepartment.DataValueField = "ProductID";
                    ddlDepartment.DataBind();
                    ddlDepartment.Items.Insert(0, ".....Select Product Name.....");
                    ddlDepartment.Items[0].Value = "0";
                }
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                DropDownList ddlDepartment = (DropDownList)e.Row.FindControl("ddlProductName");
                ddlDepartment.DataSource = dt;
                ddlDepartment.DataTextField = "ProductName";
                ddlDepartment.DataValueField = "ProductID";
                ddlDepartment.DataBind();
                ddlDepartment.Items.Insert(0, ".....Select Product Name.....");
                ddlDepartment.Items[0].Value = "0";
            }


        }


        protected void gvEG_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Insert"))
            {
                lst1.Add(Convert.ToString(((DropDownList)gvEG.FooterRow.FindControl("ddlProductName")).SelectedValue.ToString()));
                lst1.Add(((TextBox)gvEG.FooterRow.FindControl("txtQuantity")).Text.ToString());
                lst1.Add(((TextBox)gvEG.FooterRow.FindControl("txtRate")).Text.ToString());
                lst1.Add(((TextBox)gvEG.FooterRow.FindControl("txtMesurementUnit")).Text.ToString());
                lst1.Add(((TextBox)gvEG.FooterRow.FindControl("txtLaserCodeNoform")).Text.ToString());
                lst1.Add(((TextBox)gvEG.FooterRow.FindControl("txtLaserCodeNoto")).Text.ToString());


                if (!string.IsNullOrEmpty(H_ID.Value.ToString()))
                {
                    lst1.Add(H_ID.Value.ToString());
                }
                else
                {
                    lst1.Add(H_headerID1.Value.ToString());
                }

                j = blorder.InsertInvoiceOreder(lst1);
                if (j > 0)
                {
                    DataSet ds = new DataSet();
                    ds = blorder.ShowInsertValueInvoice(H_ID.Value.ToString());
                        
                    gvEG.DataSource = ds;
                    gvEG.DataBind();
                }
            }

            if (e.CommandName.Equals("ADD"))
            {
                GridViewRow emptyRow = gvEG.Controls[0].Controls[0] as GridViewRow;

                lst1.Add(Convert.ToString(((DropDownList)emptyRow.FindControl("ddlProductName")).SelectedValue.ToString()));
                lst1.Add(Convert.ToString(((TextBox)emptyRow.FindControl("txtQuantity")).Text));
                lst1.Add(((TextBox)emptyRow.FindControl("txtRate")).Text.ToString());
                lst1.Add(((TextBox)emptyRow.FindControl("txtMesurementUnit")).Text.ToString());
                lst1.Add(((TextBox)emptyRow.FindControl("txtLaserCodeNoform")).Text.ToString());
                lst1.Add(((TextBox)emptyRow.FindControl("txtLaserCodeNoto")).Text.ToString());

                if (!string.IsNullOrEmpty(H_ID.Value.ToString()))
                {
                    lst1.Add(H_ID.Value.ToString());
                }
                else
                {
                    lst1.Add(H_headerID1.Value.ToString());
                }

                j = blorder.InsertInvoiceOreder(lst1);
                if (j > 0)
                {
                    DataSet ds = new DataSet();
                    ds = blorder.ShowInsertValueInvoice(H_ID.Value.ToString());
                    gvEG.DataSource = ds;
                    gvEG.DataBind();
                }

            }





        }


        protected void gvEG_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvEG.EditIndex = e.NewEditIndex;
            FillEmployeeGrid();
        }


        protected void gvEG_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvEG.EditIndex = -1;
            FillEmployeeGrid();
        }

        protected void gvEG_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int ID = int.Parse(gvEG.DataKeys[e.RowIndex].Values[0].ToString());
            int i = blorder.deletePO(ID);
            if (i > 0)
            {
                FillEmployeeGrid();
            }
        }

        protected void gvEG_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

            List<string> lst4 = new List<string>();
            lst4.Add(Convert.ToString(((DropDownList)gvEG.Rows[e.RowIndex].FindControl("ddlProductName")).SelectedValue));
            lst4.Add(Convert.ToString(((TextBox)gvEG.Rows[e.RowIndex].FindControl("txtQuantity")).Text));
            lst4.Add(Convert.ToString(((TextBox)gvEG.Rows[e.RowIndex].FindControl("txtRate")).Text));
            lst4.Add(Convert.ToString(((TextBox)gvEG.Rows[e.RowIndex].FindControl("txtMesurementUnit")).Text));
            lst4.Add(Convert.ToString(((TextBox)gvEG.Rows[e.RowIndex].FindControl("txtLaserCodeNoform")).Text));
            lst4.Add(Convert.ToString(((TextBox)gvEG.Rows[e.RowIndex].FindControl("txtLaserCodeNoform")).Text));
            if (!string.IsNullOrEmpty(H_ID.Value.ToString()))
            {
                lst4.Add(H_ID.Value.ToString());
            }
            else
            {
                lst4.Add(H_headerID1.Value.ToString());
            }

            lst4.Add(gvEG.DataKeys[e.RowIndex].Values[0].ToString());

            int k1 = blorder.UpdateInvoiceOrder(lst4);

            gvEG.EditIndex = -1;
            FillEmployeeGrid();
        }


        POorderbl obj = new POorderbl();

        int kk, j;





        protected void ddlProductName_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;

            GridViewRow row = (GridViewRow)ddl.NamingContainer;

            if (row != null)
            {

                string selectedValue = ((DropDownList)(row.FindControl("ddlProductName"))).SelectedValue.ToString();
                ds = blorder.FetchAddressByProductID(int.Parse(selectedValue));

                ((TextBox)(row.FindControl("txtRate"))).Text = ds.Tables[0].Rows[0]["ProductCost"].ToString();
                ((TextBox)(row.FindControl("txtMesurementUnit"))).Text = ds.Tables[0].Rows[0]["P_MeasurementUnit"].ToString();





            }




        }



        protected void ddlProductName_SelectedIndexChanged1(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;

            GridViewRow row = (GridViewRow)ddl.NamingContainer;

            if (row != null)
            {

                string selectedValue = ((DropDownList)(row.FindControl("ddlProductName"))).SelectedValue.ToString();
                ds = blorder.FetchAddressByProductID(int.Parse(selectedValue));

                ((TextBox)gvEG.FooterRow.FindControl("txtRate")).Text = ds.Tables[0].Rows[0]["ProductCost"].ToString();
                ((TextBox)gvEG.FooterRow.FindControl("txtMesurementUnit")).Text = ds.Tables[0].Rows[0]["P_MeasurementUnit"].ToString();





            }




        }


        protected void lnkAdd_Click(object sender, EventArgs e)
        {
        }

        protected void btnUpdate_Click1(object sender, EventArgs e)
        {

            List<string> lst3 = new List<string>();
            //lst3.Add(ddlVendorName.SelectedValue);
            lst3.Add(ddlCustomerName.SelectedValue);
            lst3.Add(txtTransportName.Text);

            lst3.Add(txtGoodsReceiptNote.Text);
            lst3.Add(ddlTransportVia.SelectedItem.ToString());
            lst3.Add(txtBillingAddress.Text);
            lst3.Add(txtShippingAddress.Text);
            if (chkActive.Checked == true)
            {
                Staus = "Valid";
            }
            else
            {
                Staus = "In Valid";
            }
            lst3.Add(Staus);
            if (!string.IsNullOrEmpty(H_ID.Value))
            {
                lst3.Add(H_ID.Value.ToString());
            }
            else
            {
                lst3.Add(H_headerID1.Value.ToString());
            }
          
            int k = blorder.UpdateInvoiceHeader(lst3);
            if (k > 0)
            {
                divGrid.Visible = true;
                FillEmployeeGrid();
            }
        }

        public void showgv()
        {
            DataTable ds = new DataTable();
            ds = blorder.Show1();
            gvEG.DataSource = ds;
            gvEG.DataBind();
        }
        DateTime OrderDate1;
        float sum = 0;
       



        protected void ddlCustomerName_SelectedIndexChanged1(object sender, EventArgs e)
        {
            int Customerid = int.Parse(ddlCustomerName.SelectedValue);
            ds = blorder.FetchAddressByCustomerID(Customerid);
            txtBillingAddress.Text = ds.Tables[0].Rows[0]["Address1"].ToString();
            txtShippingAddress.Text = ds.Tables[0].Rows[0]["Address2"].ToString();

        }


        public void ShowCustomerDetail()
        {
            dt = blorder.ShowCustomerDetail();
            if (dt.Rows.Count > 0)
            {

                ddlCustomerName.DataSource = dt;
                ddlCustomerName.DataTextField = "Name";
                ddlCustomerName.DataValueField = "CustomerID";
                ddlCustomerName.DataBind();
                ddlCustomerName.Items.Insert(0, "Select Customer Name");
                ddlCustomerName.Items[0].Value = "0";

            }
        }


        float total = 0, to = 0;
        int i = 0;
        protected void btnSavePO_Click(object sender, EventArgs e)
        {
            //String[] StringAuthDate = OrderDate.SelectedDate.ToString().Split('/');
            //string MonTo = ("0" + StringAuthDate[0]);
            //string MonthdateTO = MonTo.Replace("00", "0").Replace("01", "1");
            //String ReportDateFrom = StringAuthDate[1] + "/" + MonthdateTO + "/" + StringAuthDate[2].Split(' ')[0];
            //String From = StringAuthDate[0] + "/" + StringAuthDate[1] + "/" + StringAuthDate[2].Split(' ')[0];
            //OrderDate1 = new DateTime(Convert.ToInt32(StringAuthDate[2].Split(' ')[0]), Convert.ToInt32(StringAuthDate[0]), Convert.ToInt32(StringAuthDate[1]));

            //if (e.Control.ID == "LinkButtonPO")
            //{
            //    String POID = e.Item["POheaderID"].ToString();

            if (!string.IsNullOrEmpty(H_headerID1.Value.ToString()))
            {
                ds = blorder.ShowViewinvoice1(H_headerID1.Value.ToString());

            }
            else
            {
                ds = blorder.ShowViewinvoice1(H_ID.Value.ToString());

            }

            //ds = blorder.ShowViewPO1(OrderDate1.ToString("dd/MM/yyyy"), POID);

            if (ds.Tables[0].Rows.Count > 0)
            {
                i = ds.Tables[0].Rows.Count;
                string filename = "Order Invoice" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".pdf";
                String StringField = String.Empty;
                String StringAlert = String.Empty;
                StringBuilder bb = new StringBuilder();
                Document document = new Document();
                BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
                // iTextSharp.text.Font times = new Font(bfTimes, 12, Font.ITALIC, Color.Black);
                //Creates a Writer that listens to this document and writes the document to the Stream of your choice:
                string PdfFolder = ConfigurationManager.AppSettings["PdfFolder"].ToString() + filename;
                PdfWriter.GetInstance(document, new FileStream(PdfFolder, FileMode.Create));

                //Opens the document:
                document.Open();

                //Adds content to the document:
                // document.Add(new Paragraph("Ignition Log Report"));
                PdfPTable table = new PdfPTable(7);
                PdfPTable table1 = new PdfPTable(8);
                //actual width of table in points
                table.TotalWidth = 1500f;

                PdfPCell cell130911 = new PdfPCell(new Phrase("(See rule 52(3)of the Himachal Pradesh Value Added Tax Rules ,2005)", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell130911.Colspan = 7;
                cell130911.BorderWidthLeft = 0f;
                cell130911.BorderWidthRight = 0f;
                cell130911.BorderWidthTop = 0f;
                cell130911.BorderWidthBottom = 0f;

                cell130911.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell130911);

                PdfPCell cell120911 = new PdfPCell(new Phrase("RETAIL INVOICE/CASH MEMO/BILL", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell120911.Colspan = 7;
                cell120911.BorderWidthLeft = 0f;
                cell120911.BorderWidthRight = 0f;
                cell120911.BorderWidthTop = 0f;
                cell120911.BorderWidthBottom = 0f;

                cell120911.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell120911);

                PdfPCell cell12091 = new PdfPCell(new Phrase("AUTO MOTIVE TECH", new iTextSharp.text.Font(bfTimes, 12f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell12091.Colspan = 7;
                cell12091.BorderWidthLeft = 0f;
                cell12091.BorderWidthRight = 0f;
                cell12091.BorderWidthTop = 0f;
                cell12091.BorderWidthBottom = 0f;

                cell12091.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12091);

                // PdfPCell cell12093 = new PdfPCell(new Phrase("Date : " + DateTime.Now.ToString("dd/MM/yyyy"), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                PdfPCell cell12093 = new PdfPCell(new Phrase("A UNIT OF UTSAV SAFETY SYSTEMS PVT. LTD", new iTextSharp.text.Font(bfTimes, 15f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell12093.Colspan = 7;
                cell12093.BorderWidthLeft = 0f;
                cell12093.BorderWidthRight = 0f;
                cell12093.BorderWidthTop = 0f;
                cell12093.BorderWidthBottom = 0f;

                cell12093.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12093);

                PdfPCell cell12092 = new PdfPCell(new Phrase("Plot No.3A , Phase IV , Gowalthai , Ind .Area , Distt. New Delhi -114201 (Delhi) ", new iTextSharp.text.Font(bfTimes, 12f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell12092.Colspan = 7;
                cell12092.BorderWidthLeft = 0f;
                cell12092.BorderWidthRight = 0f;
                cell12092.BorderWidthTop = 0f;
                cell12092.BorderWidthBottom = 0f;

                cell12092.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12092);
                PdfPCell cell12094 = new PdfPCell(new Phrase("MOBILE NO : 9555246314 PHONE NO : 011-64692588", new iTextSharp.text.Font(bfTimes, 12f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell12094.Colspan = 7;
                cell12094.BorderWidthLeft = 0f;
                cell12094.BorderWidthRight = 0f;
                cell12094.BorderWidthTop = 0f;
                cell12094.BorderWidthBottom = 1f;

                cell12094.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12094);






                PdfPCell cell12095 = new PdfPCell(new Phrase("Buyer's Name & Address : " + ds.Tables[0].Rows[0]["Name"].ToString().ToUpper() + Environment.NewLine + ds.Tables[0].Rows[0]["BillingAddress"].ToString().ToUpper(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell12095.Colspan = 3;
                cell12095.Rowspan = 3;
                cell12095.BorderWidthLeft = 0f;
                cell12095.BorderWidthRight = 0f;
                cell12095.BorderWidthTop = 0f;
                cell12095.BorderWidthBottom = 0f;

                cell12095.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12095);


                PdfPCell cell1209 = new PdfPCell(new Phrase("Invoice No : " + ds.Tables[0].Rows[0]["ID"].ToString().ToUpper(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell1209.Colspan = 2;
                cell1209.BorderWidthLeft = 0f;
                cell1209.BorderWidthRight = 0f;
                cell1209.BorderWidthTop = 0f;
                cell1209.BorderWidthBottom = 0f;

                cell1209.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1209);

                string date = ds.Tables[0].Rows[0]["OrderDate"].ToString();

                PdfPCell cell1213 = new PdfPCell(new Phrase("Date : " + DateTime.Parse(ds.Tables[0].Rows[0]["OrderDate"].ToString()).ToString("dd/MM/yyyy"), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell1213.Colspan = 2;
                cell1213.BorderWidthLeft = 0f;
                cell1213.BorderWidthRight = 0f;
                cell1213.BorderWidthTop = 0f;
                cell1213.BorderWidthBottom = 0f;

                cell1213.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1213);



                PdfPCell cell12233 = new PdfPCell(new Phrase("Vehicle No : ", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell12233.Colspan = 4;
                cell12233.BorderWidthLeft = 0f;
                cell12233.BorderWidthRight = 0f;
                cell12233.BorderWidthTop = 0f;
                cell12233.BorderWidthBottom = 0f;

                cell12233.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12233);

                PdfPCell cell122331 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell122331.Colspan = 4;
                cell122331.BorderWidthLeft = 0f;
                cell122331.BorderWidthRight = 0f;
                cell122331.BorderWidthTop = 0f;
                cell122331.BorderWidthBottom = 0f;

                cell122331.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell122331);


                PdfPCell cell122332 = new PdfPCell(new Phrase("Buyer's TIN / C. S.T.No : " + ds.Tables[0].Rows[0]["TinNo"].ToString().ToUpper() + " / " + ds.Tables[0].Rows[0]["CST"].ToString().ToUpper(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell122332.Colspan = 4;
                cell122332.BorderWidthLeft = 0f;
                cell122332.BorderWidthRight = 0f;
                cell122332.BorderWidthTop = 0f;
                cell122332.BorderWidthBottom = 0f;

                cell122332.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell122332);

                PdfPCell cell1206 = new PdfPCell(new Phrase("Transport Name : " + ds.Tables[0].Rows[0]["TransportName"].ToString().ToUpper(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell1206.Colspan = 3;
                cell1206.BorderWidthLeft = 0f;
                cell1206.BorderWidthRight = 0f;
                cell1206.BorderWidthTop = 0f;
                cell1206.BorderWidthBottom = 0f;
                cell1206.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1206);

                PdfPCell cell1221 = new PdfPCell(new Phrase("Through : " + ds.Tables[0].Rows[0]["TransportVia"].ToString().ToUpper(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell1221.Colspan = 4;
                cell1221.BorderWidthLeft = 0f;
                cell1221.BorderWidthRight = 0f;
                cell1221.BorderWidthTop = 0f;
                cell1221.BorderWidthBottom = 0f;

                cell1221.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1221);


                PdfPCell cell120933 = new PdfPCell(new Phrase("GR No : ", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell120933.Colspan = 3;
                cell120933.BorderWidthLeft = 0f;
                cell120933.BorderWidthRight = 0f;
                cell120933.BorderWidthTop = 0f;
                cell120933.BorderWidthBottom = 0f;

                cell120933.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell120933);

                //PdfPCell cell120934 = new PdfPCell(new Phrase("Mobile No", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                //cell120934.Colspan = 1;
                //cell120934.BorderWidthLeft = 1f;
                //cell120934.BorderWidthRight = .8f;
                //cell120934.BorderWidthTop = 1f;
                //cell120934.BorderWidthBottom = 1f;

                //cell120934.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                //table.AddCell(cell120934);
                //PdfPCell cell1223 = new PdfPCell(new Phrase("Owner Name", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                //cell1223.Colspan = 2;
                //cell1223.BorderWidthLeft = 0f;
                //cell1223.BorderWidthRight = 1f;
                //cell1223.BorderWidthTop = 1f;
                //cell1223.BorderWidthBottom = 1f;
                //cell1223.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                //table.AddCell(cell1223);

                PdfPCell cell1209331 = new PdfPCell(new Phrase("SR. No : ", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell1209331.Colspan = 1;
                cell1209331.BorderWidthLeft = 1f;
                cell1209331.BorderWidthRight = .8f;
                cell1209331.BorderWidthTop = 1f;
                cell1209331.BorderWidthBottom = 0f;

                cell1209331.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table1.AddCell(cell1209331);

                PdfPCell cell1209332 = new PdfPCell(new Phrase("Discription of Goods ", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell1209332.Colspan = 2;
                // cell1209332.Width.re = 200f;
                cell1209332.BorderWidthLeft = 0f;
                cell1209332.BorderWidthRight = .8f;
                cell1209332.BorderWidthTop = 1f;
                cell1209332.BorderWidthBottom = 0f;

                cell1209332.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table1.AddCell(cell1209332);

                PdfPCell cell1209333 = new PdfPCell(new Phrase("From", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell1209333.Colspan = 1;
                cell1209333.BorderWidthLeft = 0f;
                cell1209333.BorderWidthRight = .8f;
                cell1209333.BorderWidthTop = 1f;
                cell1209333.BorderWidthBottom = 0f;

                cell1209333.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table1.AddCell(cell1209333);

                PdfPCell cell1209334 = new PdfPCell(new Phrase("To", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell1209334.Colspan = 1;
                cell1209334.BorderWidthLeft = 0f;
                cell1209334.BorderWidthRight = .8f;
                cell1209334.BorderWidthTop = 1f;
                cell1209334.BorderWidthBottom = 0f;

                cell1209334.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table1.AddCell(cell1209334);

                PdfPCell cell1209335 = new PdfPCell(new Phrase("QTY", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell1209335.Colspan = 1;
                cell1209335.BorderWidthLeft = 0f;
                cell1209335.BorderWidthRight = .8f;
                cell1209335.BorderWidthTop = 1f;
                cell1209335.BorderWidthBottom = 0f;

                cell1209335.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table1.AddCell(cell1209335);

                PdfPCell cell1209336 = new PdfPCell(new Phrase("Rate", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell1209336.Colspan = 1;
                cell1209336.BorderWidthLeft = 0f;
                cell1209336.BorderWidthRight = .8f;
                cell1209336.BorderWidthTop = 1f;
                cell1209336.BorderWidthBottom = 0f;

                cell1209336.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table1.AddCell(cell1209336);

                PdfPCell cell1209337 = new PdfPCell(new Phrase("Amount (Rs.)", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell1209337.Colspan = 1;
                cell1209337.BorderWidthLeft = 0f;
                cell1209337.BorderWidthRight = 1f;
                cell1209337.BorderWidthTop = 1f;
                cell1209337.BorderWidthBottom = 0f;

                cell1209337.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table1.AddCell(cell1209337);


                //i = i - 1;
                int k = 0;
                for (int j = 0; j < i; j++)
                {

                    k++;



                    PdfPCell cell12093311 = new PdfPCell(new Phrase((k).ToString().ToUpper(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    cell12093311.Colspan = 1;
                    cell12093311.BorderWidthLeft = 1f;
                    cell12093311.BorderWidthRight = .8f;
                    cell12093311.BorderWidthTop = 1f;
                    cell12093311.BorderWidthBottom = 0f;

                    cell12093311.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    table1.AddCell(cell12093311);

                    PdfPCell cell12093321 = new PdfPCell(new Phrase(ds.Tables[0].Rows[j]["ProductName"].ToString().ToUpper(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    cell12093321.Colspan = 2;
                    // cell1209332.Width.re = 200f;
                    cell12093321.BorderWidthLeft = 0f;
                    cell12093321.BorderWidthRight = .8f;
                    cell12093321.BorderWidthTop = 1f;
                    cell12093321.BorderWidthBottom = 0f;

                    cell12093321.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    table1.AddCell(cell12093321);

                    PdfPCell cell12093331 = new PdfPCell(new Phrase(ds.Tables[0].Rows[j]["LaserCodeNoFrom"].ToString().ToUpper(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    cell12093331.Colspan = 1;
                    cell12093331.BorderWidthLeft = 0f;
                    cell12093331.BorderWidthRight = .8f;
                    cell12093331.BorderWidthTop = 1f;
                    cell12093331.BorderWidthBottom = 0f;

                    cell12093331.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    table1.AddCell(cell12093331);

                    PdfPCell cell12093341 = new PdfPCell(new Phrase(ds.Tables[0].Rows[j]["LaserCodeNoTo"].ToString().ToUpper(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    cell12093341.Colspan = 1;
                    cell12093341.BorderWidthLeft = 0f;
                    cell12093341.BorderWidthRight = .8f;
                    cell12093341.BorderWidthTop = 1f;
                    cell12093341.BorderWidthBottom = 0f;

                    cell12093341.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    table1.AddCell(cell12093341);

                    PdfPCell cell12093351 = new PdfPCell(new Phrase(ds.Tables[0].Rows[j]["Quantity"].ToString().ToUpper(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    cell12093351.Colspan = 1;
                    cell12093351.BorderWidthLeft = 0f;
                    cell12093351.BorderWidthRight = .8f;
                    cell12093351.BorderWidthTop = 1f;
                    cell12093351.BorderWidthBottom = 0f;

                    cell12093351.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    table1.AddCell(cell12093351);

                    PdfPCell cell12093361 = new PdfPCell(new Phrase(ds.Tables[0].Rows[j]["Rate"].ToString().ToUpper(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    cell12093361.Colspan = 1;
                    cell12093361.BorderWidthLeft = 0f;
                    cell12093361.BorderWidthRight = .8f;
                    cell12093361.BorderWidthTop = 1f;
                    cell12093361.BorderWidthBottom = 0f;

                    cell12093361.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    table1.AddCell(cell12093361);

                    total = float.Parse(ds.Tables[0].Rows[j]["Quantity"].ToString()) * float.Parse(ds.Tables[0].Rows[j]["Rate"].ToString());

                    PdfPCell cell12093371 = new PdfPCell(new Phrase(total.ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    cell12093371.Colspan = 1;
                    cell12093371.BorderWidthLeft = 0f;
                    cell12093371.BorderWidthRight = 1f;
                    cell12093371.BorderWidthTop = 1f;
                    cell12093371.BorderWidthBottom = 0f;

                    cell12093371.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    table1.AddCell(cell12093371);



                    to = to + total;



                }



                PdfPCell cell120933711 = new PdfPCell(new Phrase("Sale/Transfer against centrel form 'C' / 'F' / 'H'", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell120933711.Colspan = 5;
                cell120933711.BorderWidthLeft = 1f;
                cell120933711.BorderWidthRight = 0f;
                cell120933711.BorderWidthTop = 1f;
                cell120933711.BorderWidthBottom = 0f;

                cell120933711.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table1.AddCell(cell120933711);


                PdfPCell cell120933712 = new PdfPCell(new Phrase("TOTAL : ", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell120933712.Colspan = 2;
                cell120933712.BorderWidthLeft = 1f;
                cell120933712.BorderWidthRight = .8f;
                cell120933712.BorderWidthTop = 1f;
                cell120933712.BorderWidthBottom = 1f;

                cell120933712.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table1.AddCell(cell120933712);

                PdfPCell cell1209337122 = new PdfPCell(new Phrase(to.ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell1209337122.Colspan = 1;
                cell1209337122.BorderWidthLeft = 0f;
                cell1209337122.BorderWidthRight = 1f;
                cell1209337122.BorderWidthTop = 1f;
                cell1209337122.BorderWidthBottom = 1f;

                cell1209337122.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table1.AddCell(cell1209337122);


                PdfPCell cell120933713 = new PdfPCell(new Phrase("Form No :         ", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell120933713.Colspan = 5;
                cell120933713.BorderWidthLeft = 1f;
                cell120933713.BorderWidthRight = 0f;
                cell120933713.BorderWidthTop = 0f;
                cell120933713.BorderWidthBottom = 0f;

                cell120933713.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table1.AddCell(cell120933713);

                float vat = to * 1 / 100;

                PdfPCell cell120933714 = new PdfPCell(new Phrase("VAT/C.S.T. : ", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell120933714.Colspan = 2;
                cell120933714.BorderWidthLeft = 1f;
                cell120933714.BorderWidthRight = .8f;
                cell120933714.BorderWidthTop = 0f;
                cell120933714.BorderWidthBottom = 0f;

                cell120933714.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table1.AddCell(cell120933714);

                PdfPCell cell1209337144 = new PdfPCell(new Phrase(vat.ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell1209337144.Colspan = 1;
                cell1209337144.BorderWidthLeft = 0f;
                cell1209337144.BorderWidthRight = 1f;
                cell1209337144.BorderWidthTop = 0f;
                cell1209337144.BorderWidthBottom = 0f;

                cell1209337144.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table1.AddCell(cell1209337144);



                PdfPCell cell120933715 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell120933715.Colspan = 5;
                cell120933715.BorderWidthLeft = 1f;
                cell120933715.BorderWidthRight = 0f;
                cell120933715.BorderWidthTop = 0f;
                cell120933715.BorderWidthBottom = 0f;

                cell120933715.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table1.AddCell(cell120933715);


                PdfPCell cell120933716 = new PdfPCell(new Phrase("TOTAL WITH VAT/CST: ", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell120933716.Colspan = 2;
                cell120933716.BorderWidthLeft = 1f;
                cell120933716.BorderWidthRight = .8f;
                cell120933716.BorderWidthTop = 1f;
                cell120933716.BorderWidthBottom = 0f;

                cell120933716.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table1.AddCell(cell120933716);

                PdfPCell cell1209337166 = new PdfPCell(new Phrase((to + vat).ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell1209337166.Colspan = 1;
                cell1209337166.BorderWidthLeft = 0f;
                cell1209337166.BorderWidthRight = 1f;
                cell1209337166.BorderWidthTop = 1f;
                cell1209337166.BorderWidthBottom = 0f;

                cell1209337166.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table1.AddCell(cell1209337166);




                PdfPCell cell120933717 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell120933717.Colspan = 5;
                cell120933717.BorderWidthLeft = 1f;
                cell120933717.BorderWidthRight = 0f;
                cell120933717.BorderWidthTop = 0f;
                cell120933717.BorderWidthBottom = 0f;

                cell120933717.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table1.AddCell(cell120933717);


                PdfPCell cell120933718 = new PdfPCell(new Phrase("PACKING & FORWARDING : ", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell120933718.Colspan = 2;
                cell120933718.BorderWidthLeft = 1f;
                cell120933718.BorderWidthRight = .8f;
                cell120933718.BorderWidthTop = .8f;
                cell120933718.BorderWidthBottom = 0f;

                cell120933718.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table1.AddCell(cell120933718);


                PdfPCell cell1209337188 = new PdfPCell(new Phrase("0", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell1209337188.Colspan = 1;
                cell1209337188.BorderWidthLeft = 0f;
                cell1209337188.BorderWidthRight = 1f;
                cell1209337188.BorderWidthTop = .8f;
                cell1209337188.BorderWidthBottom = 0f;

                cell1209337188.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table1.AddCell(cell1209337188);


                PdfPCell cell120933719 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell120933719.Colspan = 5;
                cell120933719.BorderWidthLeft = 1f;
                cell120933719.BorderWidthRight = 0f;
                cell120933719.BorderWidthTop = 0f;
                cell120933719.BorderWidthBottom = 1f;

                cell120933719.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table1.AddCell(cell120933719);


                PdfPCell cell120933720 = new PdfPCell(new Phrase("GRAND TOTAL : ", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell120933720.Colspan = 2;
                cell120933720.BorderWidthLeft = 1f;
                cell120933720.BorderWidthRight = .8f;
                cell120933720.BorderWidthTop = 1f;
                cell120933720.BorderWidthBottom = 1f;

                cell120933720.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table1.AddCell(cell120933720);

                PdfPCell cell1209337202 = new PdfPCell(new Phrase((to + vat).ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell1209337202.Colspan = 1;
                cell1209337202.BorderWidthLeft = 0f;
                cell1209337202.BorderWidthRight = 1f;
                cell1209337202.BorderWidthTop = 1f;
                cell1209337202.BorderWidthBottom = 1f;

                cell1209337202.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table1.AddCell(cell1209337202);

                PdfPCell cell120933721 = new PdfPCell(new Phrase("TERMS AND CONDITIONS :", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell120933721.Colspan = 5;
                cell120933721.BorderWidthLeft = 0f;
                cell120933721.BorderWidthRight = 0f;
                cell120933721.BorderWidthTop = 0f;
                cell120933721.BorderWidthBottom = 0f;

                cell120933721.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table1.AddCell(cell120933721);


                PdfPCell cell120933722 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell120933722.Colspan = 3;
                cell120933722.BorderWidthLeft = 0f;
                cell120933722.BorderWidthRight = 0f;
                cell120933722.BorderWidthTop = 0f;
                cell120933722.BorderWidthBottom = 0f;

                cell120933722.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table1.AddCell(cell120933722);



                PdfPCell cell120933723 = new PdfPCell(new Phrase("1. Our responsibility cases , the moment goods leave our premises.", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell120933723.Colspan = 5;
                cell120933723.BorderWidthLeft = 0f;
                cell120933723.BorderWidthRight = 0f;
                cell120933723.BorderWidthTop = 0f;
                cell120933723.BorderWidthBottom = 0f;

                cell120933723.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table1.AddCell(cell120933723);


                PdfPCell cell120933724 = new PdfPCell(new Phrase("Your's Faithfully,", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell120933724.Colspan = 3;
                cell120933724.BorderWidthLeft = 0f;
                cell120933724.BorderWidthRight = 0f;
                cell120933724.BorderWidthTop = 0f;
                cell120933724.BorderWidthBottom = 0f;

                cell120933724.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table1.AddCell(cell120933724);


                PdfPCell cell120933725 = new PdfPCell(new Phrase("2. Goods once sold are not returnable or exchangeable. ", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell120933725.Colspan = 5;
                cell120933725.BorderWidthLeft = 0f;
                cell120933725.BorderWidthRight = 0f;
                cell120933725.BorderWidthTop = 0f;
                cell120933725.BorderWidthBottom = 0f;

                cell120933725.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table1.AddCell(cell120933725);


                PdfPCell cell120933726 = new PdfPCell(new Phrase("AUTO MOTIVE TECH", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell120933726.Colspan = 3;
                cell120933726.BorderWidthLeft = 0f;
                cell120933726.BorderWidthRight = 0f;
                cell120933726.BorderWidthTop = 0f;
                cell120933726.BorderWidthBottom = 0f;

                cell120933726.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table1.AddCell(cell120933726);


                PdfPCell cell120933727 = new PdfPCell(new Phrase("3. Interest @24% per annum will be charged if the bill is not paid within 5 days. ", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell120933727.Colspan = 5;
                cell120933727.BorderWidthLeft = 0f;
                cell120933727.BorderWidthRight = 0f;
                cell120933727.BorderWidthTop = 0f;
                cell120933727.BorderWidthBottom = 0f;

                cell120933727.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table1.AddCell(cell120933727);


                PdfPCell cell120933728 = new PdfPCell(new Phrase("For A UNIT OF UTSAV SAFETY SYSTEM (P) LTD.", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell120933728.Colspan = 3;
                cell120933728.BorderWidthLeft = 0f;
                cell120933728.BorderWidthRight = 0f;
                cell120933728.BorderWidthTop = 0f;
                cell120933728.BorderWidthBottom = 0f;

                cell120933728.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table1.AddCell(cell120933728);


                PdfPCell cell120933729 = new PdfPCell(new Phrase("4. Party shall pay full tax if 'C' form is not received in time. ", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell120933729.Colspan = 5;
                cell120933729.BorderWidthLeft = 0f;
                cell120933729.BorderWidthRight = 0f;
                cell120933729.BorderWidthTop = 0f;
                cell120933729.BorderWidthBottom = 0f;

                cell120933729.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table1.AddCell(cell120933729);


                PdfPCell cell120933730 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell120933730.Colspan = 3;
                cell120933730.BorderWidthLeft = 0f;
                cell120933730.BorderWidthRight = 0f;
                cell120933730.BorderWidthTop = 0f;
                cell120933730.BorderWidthBottom = 0f;

                cell120933730.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table1.AddCell(cell120933730);


                PdfPCell cell120933731 = new PdfPCell(new Phrase("5. All disputes subject to Bilashpur Jurisdiction. ", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell120933731.Colspan = 5;
                cell120933731.BorderWidthLeft = 0f;
                cell120933731.BorderWidthRight = 0f;
                cell120933731.BorderWidthTop = 0f;
                cell120933731.BorderWidthBottom = 0f;

                cell120933731.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table1.AddCell(cell120933731);


                PdfPCell cell120933732 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell120933732.Colspan = 3;
                cell120933732.BorderWidthLeft = 0f;
                cell120933732.BorderWidthRight = 0f;
                cell120933732.BorderWidthTop = 0f;
                cell120933732.BorderWidthBottom = 0f;

                cell120933732.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table1.AddCell(cell120933732);


                PdfPCell cell120933733 = new PdfPCell(new Phrase("Customer Signature", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell120933733.Colspan = 5;
                cell120933733.PaddingTop = 30f;
                cell120933733.BorderWidthLeft = 0f;
                cell120933733.BorderWidthRight = 0f;
                cell120933733.BorderWidthTop = 0f;
                cell120933733.BorderWidthBottom = 1f;

                cell120933733.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table1.AddCell(cell120933733);


                PdfPCell cell120933734 = new PdfPCell(new Phrase("Signature", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell120933734.Colspan = 3;
                cell120933734.PaddingTop = 30f;
                cell120933734.BorderWidthLeft = 0f;
                cell120933734.BorderWidthRight = 0f;
                cell120933734.BorderWidthTop = 0f;
                cell120933734.BorderWidthBottom = 1f;

                cell120933734.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table1.AddCell(cell120933734);

                document.Add(table);
                document.Add(table1);
                // document.Add(table1);

                document.Close();
                HttpContext context = HttpContext.Current;

                context.Response.ContentType = "Application/pdf";
                context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                context.Response.WriteFile(PdfFolder);
                context.Response.End();
            }
            else
            {
                string closescript1 = "<script>alert('No records found for selected date.')</script>";
                Page.RegisterStartupScript("abc", closescript1);
                return;
            }
        }
    }
}