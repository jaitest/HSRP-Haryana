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
    public partial class POorder : System.Web.UI.Page
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
                ShowVendorDetail();
               // ShowProductDetail();
                Mode = Request.QueryString["Mode"].ToString();
                if (Mode == "Edit")
                {
                    btnShowPopup.Visible = false;
                     btnUpdate.Visible = true;
                    H_POheaderID1.Value = Request.QueryString["POheaderID1"].ToString();
                    string id = H_POheaderID1.Value.ToString();
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

            if ( !string.IsNullOrEmpty(H_POheaderID1.Value.ToString()))
            {
                ds = blorder.ShowInsertValue(H_POheaderID1.Value.ToString());
                gvEG.DataSource = ds;
                gvEG.DataBind();
            }
            else
            {
                ds = blorder.ShowInsertValue(H_ID.Value.ToString());
                gvEG.DataSource = ds;
                gvEG.DataBind();
            }

             
        } 



        private void editInvoiceOrder(string POheaderID)
        {
            ds = blorder.editVendorOrderbyID(POheaderID.ToString());

            if (ds.Tables[0].Rows.Count > 0)
            {

                ddlVendorName.SelectedValue = ds.Tables[0].Rows[0]["VendorID"].ToString();
                txtBillingAddress.Text = ds.Tables[0].Rows[0]["BillingAddress"].ToString();
                txtShippingAddress.Text = ds.Tables[0].Rows[0]["ShippingAddress"].ToString();
                txtRemark.Text = ds.Tables[0].Rows[0]["Remark"].ToString();
                txtPaymentTerm.Text = ds.Tables[0].Rows[0]["PaymentTerm"].ToString();
                txtDate.Text = ds.Tables[0].Rows[0]["Date"].ToString();

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
           
             lst1.Add(ddlVendorName.SelectedValue.ToString());
             lst1.Add( txtBillingAddress.Text);
             lst1.Add(txtShippingAddress.Text);
             lst1.Add(txtRemark.Text);
             lst1.Add(txtPaymentTerm.Text);
             lst1.Add(txtDate.Text);

             if (chkActive.Checked == true)
            {
                lst1.Add("Valid");
            }
            else
            {
                lst1.Add("In Valid");
            }
            int id = 0;
          
            int k = blorder.InsertVendorHeader(lst1, ref id);
            if (k > 0)
            {
                H_ID.Value = id.ToString();
                divGrid.Visible = true;

                DataSet ds = new DataSet();
                ds = blorder.ShowInsertValue(H_ID.Value.ToString());
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

                ddlVendorName.DataSource = dt;
                ddlVendorName.DataTextField = "Name";
                ddlVendorName.DataValueField = "VendorID";
                ddlVendorName.DataBind();
                ddlVendorName.Items.Insert(0, "Select Vendor Name");
                ddlVendorName.Items[0].Value = "0";

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
                    //ddlDepartment.SelectedValue = gvEG.DataKeys[e.Row.RowIndex].Values[1].ToString();
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
                if (!string.IsNullOrEmpty(H_ID.Value.ToString()))
                {
                    lst1.Add(H_ID.Value.ToString());
                }
                else
                {
                    lst1.Add(H_POheaderID1.Value.ToString());
                }

                j = blorder.InsertVendorOreder(lst1);
                if (j > 0)
                {
                    DataSet ds = new DataSet();
                    ds = blorder.ShowInsertValue(H_ID.Value.ToString());
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
                if (!string.IsNullOrEmpty(H_ID.Value.ToString()))
                {
                    lst1.Add(H_ID.Value.ToString());
                }
                else
                {
                    lst1.Add(H_POheaderID1.Value.ToString());
                }

                j = blorder.InsertVendorOreder(lst1);
                if (j > 0)
                {
                    DataSet ds = new DataSet();
                    ds = blorder.ShowInsertValue(H_ID.Value.ToString());
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
            if (!string.IsNullOrEmpty(H_ID.Value.ToString()))
            {
                lst4.Add(H_ID.Value.ToString());
            }
            else
            {
                lst4.Add(H_POheaderID1.Value.ToString());
            }

            lst4.Add(gvEG.DataKeys[e.RowIndex].Values[0].ToString());
            int k1 = blorder.UpdatePOOrder(lst4);
                     
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
            lst3.Add(ddlVendorName.SelectedValue);
            lst3.Add(txtBillingAddress.Text);
            lst3.Add(txtShippingAddress.Text);
            lst3.Add(txtRemark.Text);
            lst3.Add(txtPaymentTerm.Text);
            lst3.Add(txtDate.Text);
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
                lst3.Add(H_POheaderID1.Value.ToString());
            }

            int k = blorder.UpdateVendorHeader(lst3);
            if (k > 0)
            {
                divGrid.Visible = true;
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

            if (!string.IsNullOrEmpty(H_POheaderID1.Value.ToString()))
            {
                ds = blorder.ShowViewPO3(H_POheaderID1.Value.ToString());
              
            }
            else
            {
                ds = blorder.ShowViewPO3(H_ID.Value.ToString());
               
            }
         
            //ds = blorder.ShowViewPO1(OrderDate1.ToString("dd/MM/yyyy"), POID);

                if (ds.Tables[0].Rows.Count > 0)
                {
                   int i = ds.Tables[0].Rows.Count;
                   string filename = "PURCHASE ORDER" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".pdf";
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
                   PdfPTable table2 = new PdfPTable(7);
                   //actual width of table in points
                   table.TotalWidth = 1500f;

                   PdfPCell cell120911 = new PdfPCell(new Phrase("PURCHASE ORDER", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                   cell120911.Colspan = 7;
                   cell120911.BorderWidthLeft = 0f;
                   cell120911.BorderWidthRight = 0f;
                   cell120911.BorderWidthTop = 0f;
                   cell120911.BorderWidthBottom = 0f;

                   cell120911.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                   table.AddCell(cell120911);

                   //PdfPCell cell12091 = new PdfPCell(new Phrase("AUTO MOTIVE TECH", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                   //cell12091.Colspan = 7;
                   //cell12091.BorderWidthLeft = 0f;
                   //cell12091.BorderWidthRight = 0f;
                   //cell12091.BorderWidthTop = 0f;
                   //cell12091.BorderWidthBottom = 0f;

                   //cell12091.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                   //table.AddCell(cell12091);

                   // PdfPCell cell12093 = new PdfPCell(new Phrase("Date : " + DateTime.Now.ToString("dd/MM/yyyy"), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                   PdfPCell cell12093 = new PdfPCell(new Phrase("ROSMERTA AUTOTECH  PVT. LTD.", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                   cell12093.Colspan = 7;
                   cell12093.BorderWidthLeft = 1f;
                   cell12093.BorderWidthRight = 1f;
                   cell12093.BorderWidthTop = 1f;
                   cell12093.BorderWidthBottom = 0f;

                   cell12093.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                   table.AddCell(cell12093);

                   PdfPCell cell12092 = new PdfPCell(new Phrase("131, Udyog  Vihar Phase -1, Gurgaon -122001, India", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                   cell12092.Colspan = 7;
                   cell12092.BorderWidthLeft = 1f;
                   cell12092.BorderWidthRight = 1f;
                   cell12092.BorderWidthTop = 0f;
                   cell12092.BorderWidthBottom = 0f;

                   cell12092.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                   table.AddCell(cell12092);
                   PdfPCell cell12094 = new PdfPCell(new Phrase("PH : +91-124-4990800", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                   cell12094.Colspan = 7;
                   cell12094.BorderWidthLeft = 1f;
                   cell12094.BorderWidthRight = 1f;
                   cell12094.BorderWidthTop = 0f;
                   cell12094.BorderWidthBottom = 1f;

                   cell12094.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                   table.AddCell(cell12094);

                   PdfPCell cell12095 = new PdfPCell(new Phrase("Vendor's Name & Address : " + ds.Tables[0].Rows[0]["Name"].ToString() + Environment.NewLine + ds.Tables[0].Rows[0]["Address1"].ToString() + Environment.NewLine + ds.Tables[0].Rows[0]["City"].ToString() + " " + ds.Tables[0].Rows[0]["State"].ToString() + " " + ds.Tables[0].Rows[0]["Country"].ToString() + Environment.NewLine + "Mobile No : " + ds.Tables[0].Rows[0]["MobileNo"].ToString() + Environment.NewLine + "Tin No : " + ds.Tables[0].Rows[0]["TinNo"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                   cell12095.Colspan = 4;
                   cell12095.Rowspan = 10;
                   cell12095.BorderWidthLeft = 1f;
                   cell12095.BorderWidthRight = 1f;
                   cell12095.BorderWidthTop = 0f;
                   cell12095.BorderWidthBottom = 1f;

                   cell12095.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                   table.AddCell(cell12095);


                   PdfPCell cell1209 = new PdfPCell(new Phrase("PO NO : " + ds.Tables[0].Rows[0]["POorderID"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                   cell1209.Colspan = 3;
                   cell1209.BorderWidthLeft = 0f;
                   cell1209.BorderWidthRight = 1f;
                   cell1209.BorderWidthTop = 0f;
                   cell1209.BorderWidthBottom = 0f;

                   cell1209.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                   table.AddCell(cell1209);

                   PdfPCell cell1213 = new PdfPCell(new Phrase("PO Date & Time : " + ds.Tables[0].Rows[0]["POOrderDate"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                   cell1213.Colspan = 3;
                   cell1213.BorderWidthLeft = 0f;
                   cell1213.BorderWidthRight = 1f;
                   cell1213.BorderWidthTop = 0f;
                   cell1213.BorderWidthBottom = 0f;

                   cell1213.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                   table.AddCell(cell1213);



                   PdfPCell cell12233 = new PdfPCell(new Phrase("Amendment No : ", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                   cell12233.Colspan = 3;
                   cell12233.BorderWidthLeft = 0f;
                   cell12233.BorderWidthRight = 1f;
                   cell12233.BorderWidthTop = 0f;
                   cell12233.BorderWidthBottom = 0f;

                   cell12233.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                   table.AddCell(cell12233);

                   PdfPCell cell122331 = new PdfPCell(new Phrase("Amendment Date : ", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                   cell122331.Colspan = 3;
                   cell122331.BorderWidthLeft = 0f;
                   cell122331.BorderWidthRight = 1f;
                   cell122331.BorderWidthTop = 0f;
                   cell122331.BorderWidthBottom = 1f;

                   cell122331.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                   table.AddCell(cell122331);


                   PdfPCell cell122332 = new PdfPCell(new Phrase("Billing Address : ", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                   cell122332.Colspan = 3;
                   cell122332.BorderWidthLeft = 0f;
                   cell122332.BorderWidthRight = 1f;
                   cell122332.BorderWidthTop = 0f;
                   cell122332.BorderWidthBottom = 0f;

                   cell122332.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                   table.AddCell(cell122332);

                   PdfPCell cell1206 = new PdfPCell(new Phrase("" + ds.Tables[0].Rows[0]["BillingAddress"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                   cell1206.Colspan = 3;
                   cell1206.Rowspan = 5;
                   cell1206.BorderWidthLeft = 0f;
                   cell1206.BorderWidthRight = 1f;
                   cell1206.BorderWidthTop = 0f;
                   cell1206.BorderWidthBottom = 1f;
                   cell1206.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                   table.AddCell(cell1206);

                   PdfPCell cell1221 = new PdfPCell(new Phrase("We are pleased to place an order on you for the following material or other services detail below, subject to terms & conditions  : ", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                   cell1221.Colspan = 7;
                   cell1221.BorderWidthLeft = 1f;
                   cell1221.BorderWidthRight = 1f;
                   cell1221.BorderWidthTop = 0f;
                   cell1221.BorderWidthBottom = 1f;

                   cell1221.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                   table.AddCell(cell1221);


                   PdfPCell cell120933 = new PdfPCell(new Phrase("GR No :", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
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

                   PdfPCell cell1209331 = new PdfPCell(new Phrase("SR. No.", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                   cell1209331.Colspan = 1;
                   cell1209331.BorderWidthLeft = 1f;
                   cell1209331.BorderWidthRight = .8f;
                   cell1209331.BorderWidthTop = 0f;
                   cell1209331.BorderWidthBottom = 1f;

                   cell1209331.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                   table1.AddCell(cell1209331);



                   PdfPCell cell1209332 = new PdfPCell(new Phrase("Discription of Goods ", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                   cell1209332.Colspan = 3;
                   // cell1209332.Width.re = 200f;
                   cell1209332.BorderWidthLeft = 0f;
                   cell1209332.BorderWidthRight = .8f;
                   cell1209332.BorderWidthTop = 0f;
                   cell1209332.BorderWidthBottom = 1f;

                   cell1209332.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                   table1.AddCell(cell1209332);

                   PdfPCell cell1209333 = new PdfPCell(new Phrase("Unit", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                   cell1209333.Colspan = 1;
                   cell1209333.BorderWidthLeft = 0f;
                   cell1209333.BorderWidthRight = .8f;
                   cell1209333.BorderWidthTop = 0f;
                   cell1209333.BorderWidthBottom = 1f;

                   cell1209333.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                   table1.AddCell(cell1209333);



                   PdfPCell cell1209335 = new PdfPCell(new Phrase("QTY", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                   cell1209335.Colspan = 1;
                   cell1209335.BorderWidthLeft = 0f;
                   cell1209335.BorderWidthRight = .8f;
                   cell1209335.BorderWidthTop = 0f;
                   cell1209335.BorderWidthBottom = 1f;

                   cell1209335.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                   table1.AddCell(cell1209335);

                   PdfPCell cell1209336 = new PdfPCell(new Phrase("Rate (Rs.)", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                   cell1209336.Colspan = 1;
                   cell1209336.BorderWidthLeft = 0f;
                   cell1209336.BorderWidthRight = .8f;
                   cell1209336.BorderWidthTop = 0f;
                   cell1209336.BorderWidthBottom = 1f;

                   cell1209336.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                   table1.AddCell(cell1209336);

                   PdfPCell cell1209337 = new PdfPCell(new Phrase("AMOUNT (Rs.)", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                   cell1209337.Colspan = 1;
                   cell1209337.BorderWidthLeft = 0f;
                   cell1209337.BorderWidthRight = 1f;
                   cell1209337.BorderWidthTop = 0f;
                   cell1209337.BorderWidthBottom = 1f;

                   cell1209337.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                   table1.AddCell(cell1209337);


                   //i = i - 1;
                   int k = 1;
                   for (j = 0; j < i; j++)
                   {





                       PdfPCell cell12093311 = new PdfPCell(new Phrase("" + k++, new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                       cell12093311.Colspan = 1;
                       cell12093311.BorderWidthLeft = 1f;
                       cell12093311.BorderWidthRight = .8f;
                       cell12093311.BorderWidthTop = 0f;
                       cell12093311.BorderWidthBottom = 1f;

                       cell12093311.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                       table1.AddCell(cell12093311);



                       PdfPCell cell12093321 = new PdfPCell(new Phrase(ds.Tables[0].Rows[j]["ProductName"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                       cell12093321.Colspan = 3;
                       // cell1209332.Width.re = 200f;
                       cell12093321.BorderWidthLeft = 0f;
                       cell12093321.BorderWidthRight = .8f;
                       cell12093321.BorderWidthTop = 0f;
                       cell12093321.BorderWidthBottom = 1f;

                       cell12093321.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                       table1.AddCell(cell12093321);

                       PdfPCell cell12093331 = new PdfPCell(new Phrase(ds.Tables[0].Rows[j]["MesurmentUnit"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                       cell12093331.Colspan = 1;
                       cell12093331.BorderWidthLeft = 0f;
                       cell12093331.BorderWidthRight = .8f;
                       cell12093331.BorderWidthTop = 0f;
                       cell12093331.BorderWidthBottom = 1f;

                       cell12093331.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                       table1.AddCell(cell12093331);



                       PdfPCell cell12093351 = new PdfPCell(new Phrase(ds.Tables[0].Rows[j]["Quantity"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                       cell12093351.Colspan = 1;
                       cell12093351.BorderWidthLeft = 0f;
                       cell12093351.BorderWidthRight = .8f;
                       cell12093351.BorderWidthTop = 0f;
                       cell12093351.BorderWidthBottom = 1f;

                       cell12093351.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                       table1.AddCell(cell12093351);

                       PdfPCell cell12093361 = new PdfPCell(new Phrase(ds.Tables[0].Rows[j]["Rate"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                       cell12093361.Colspan = 1;
                       cell12093361.BorderWidthLeft = 0f;
                       cell12093361.BorderWidthRight = .8f;
                       cell12093361.BorderWidthTop = 0f;
                       cell12093361.BorderWidthBottom = 1f;

                       cell12093361.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                       table1.AddCell(cell12093361);

                       PdfPCell cell12093371 = new PdfPCell(new Phrase("" + (int.Parse(ds.Tables[0].Rows[j]["Quantity"].ToString()) * float.Parse(ds.Tables[0].Rows[j]["Rate"].ToString())), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                       cell12093371.Colspan = 1;
                       cell12093371.BorderWidthLeft = 0f;
                       cell12093371.BorderWidthRight = 1f;
                       cell12093371.BorderWidthTop = 0f;
                       cell12093371.BorderWidthBottom = 1f;

                       cell12093371.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                       table1.AddCell(cell12093371);

                       sum = (sum + (int.Parse(ds.Tables[0].Rows[j]["Quantity"].ToString()) * float.Parse(ds.Tables[0].Rows[j]["Rate"].ToString())));





                   }

                   Transaction.InvoiceTransaction.NumToWord numtowords = new Transaction.InvoiceTransaction.NumToWord();
                   string totalinwords = numtowords.changeNumericToWords(sum);




                   PdfPCell cell120933711 = new PdfPCell(new Phrase("Total Amount  ", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                   cell120933711.Colspan = 7;
                   cell120933711.BorderWidthLeft = 1f;
                   cell120933711.BorderWidthRight = 1f;
                   cell120933711.BorderWidthTop = 0f;
                   cell120933711.BorderWidthBottom = 1f;

                   cell120933711.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                   table1.AddCell(cell120933711);

                   PdfPCell cell1209337113 = new PdfPCell(new Phrase("" + sum, new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                   cell1209337113.Colspan = 1;
                   cell1209337113.BorderWidthLeft = 0f;
                   cell1209337113.BorderWidthRight = 1f;
                   cell1209337113.BorderWidthTop = 0f;
                   cell1209337113.BorderWidthBottom = .8f;

                   cell1209337113.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                   table1.AddCell(cell1209337113);

                   PdfPCell cell1209337112 = new PdfPCell(new Phrase("Total (In Words) :  Rs. " + totalinwords + "Only", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                   cell1209337112.Colspan = 7;
                   cell1209337112.BorderWidthLeft = 1f;
                   cell1209337112.BorderWidthRight = 1f;
                   cell1209337112.BorderWidthTop = 0f;
                   cell1209337112.BorderWidthBottom = .8f;

                   cell1209337112.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                   table2.AddCell(cell1209337112);

                   PdfPCell cell120933712 = new PdfPCell(new Phrase("Payment Terms : ", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                   cell120933712.Colspan = 2;
                   cell120933712.BorderWidthLeft = 1f;
                   cell120933712.BorderWidthRight = 1f;
                   cell120933712.BorderWidthTop = 0f;
                   cell120933712.BorderWidthBottom = 0f;

                   cell120933712.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                   table2.AddCell(cell120933712);

                   PdfPCell cell120933713 = new PdfPCell(new Phrase("15 days PDC against Delivery", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                   cell120933713.Colspan = 5;
                   cell120933713.BorderWidthLeft = 0f;
                   cell120933713.BorderWidthRight = 1f;
                   cell120933713.BorderWidthTop = 0f;
                   cell120933713.BorderWidthBottom = 0f;

                   cell120933713.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                   table2.AddCell(cell120933713);

                   PdfPCell cell120933714 = new PdfPCell(new Phrase("Place of : ", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                   cell120933714.Colspan = 2;
                   cell120933714.BorderWidthLeft = 1f;
                   cell120933714.BorderWidthRight = 1f;
                   cell120933714.BorderWidthTop = 0f;
                   cell120933714.BorderWidthBottom = 1f;

                   cell120933714.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                   table2.AddCell(cell120933714);

                   PdfPCell cell120933715 = new PdfPCell(new Phrase(ds.Tables[0].Rows[0]["ShippingAddress"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                   cell120933715.Colspan = 5;
                   cell120933715.BorderWidthLeft = 0f;
                   cell120933715.BorderWidthRight = 1f;
                   cell120933715.BorderWidthTop = 0f;
                   cell120933715.BorderWidthBottom = 1f;

                   cell120933715.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                   table2.AddCell(cell120933715);






                   PdfPCell cell120933716 = new PdfPCell(new Phrase("Price Basis : ", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                   cell120933716.Colspan = 2;
                   cell120933716.BorderWidthLeft = 1f;
                   cell120933716.BorderWidthRight = 0f;
                   cell120933716.BorderWidthTop = 0f;
                   cell120933716.BorderWidthBottom = 0f;

                   cell120933716.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                   table2.AddCell(cell120933716);

                   PdfPCell cell1209337161 = new PdfPCell(new Phrase("For Delhi", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                   cell1209337161.Colspan = 2;
                   cell1209337161.BorderWidthLeft = 0f;
                   cell1209337161.BorderWidthRight = 0f;
                   cell1209337161.BorderWidthTop = 0f;
                   cell1209337161.BorderWidthBottom = 0f;

                   cell1209337161.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                   table2.AddCell(cell1209337161);

                   PdfPCell cell1209337162 = new PdfPCell(new Phrase("Delivery : ", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                   cell1209337162.Colspan = 2;
                   cell1209337162.BorderWidthLeft = 0f;
                   cell1209337162.BorderWidthRight = 0f;
                   cell1209337162.BorderWidthTop = 0f;
                   cell1209337162.BorderWidthBottom = 0f;

                   cell1209337162.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                   table2.AddCell(cell1209337162);

                   PdfPCell cell1209337163 = new PdfPCell(new Phrase("As per Schedule", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                   cell1209337163.Colspan = 1;
                   cell1209337163.BorderWidthLeft = 0f;
                   cell1209337163.BorderWidthRight = 1f;
                   cell1209337163.BorderWidthTop = 0f;
                   cell1209337163.BorderWidthBottom = 0f;

                   cell1209337163.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                   table2.AddCell(cell1209337163);









                   PdfPCell cell120933718 = new PdfPCell(new Phrase("CST : ", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                   cell120933718.Colspan = 2;
                   cell120933718.BorderWidthLeft = 1f;
                   cell120933718.BorderWidthRight = 0f;
                   cell120933718.BorderWidthTop = 0f;
                   cell120933718.BorderWidthBottom = 0f;

                   cell120933718.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                   table2.AddCell(cell120933718);

                   PdfPCell cell1209337181 = new PdfPCell(new Phrase("2% Against From 'C'", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                   cell1209337181.Colspan = 2;
                   cell1209337181.BorderWidthLeft = 0f;
                   cell1209337181.BorderWidthRight = 0f;
                   cell1209337181.BorderWidthTop = 0f;
                   cell1209337181.BorderWidthBottom = 0f;

                   cell1209337181.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                   table2.AddCell(cell1209337181);

                   PdfPCell cell1209337182 = new PdfPCell(new Phrase("Despatch Mode : ", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                   cell1209337182.Colspan = 2;
                   cell1209337182.BorderWidthLeft = 0f;
                   cell1209337182.BorderWidthRight = 0f;
                   cell1209337182.BorderWidthTop = 0f;
                   cell1209337182.BorderWidthBottom = 0f;

                   cell1209337182.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                   table2.AddCell(cell1209337182);

                   PdfPCell cell1209337183 = new PdfPCell(new Phrase("By Road", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                   cell1209337183.Colspan = 1;
                   cell1209337183.BorderWidthLeft = 0f;
                   cell1209337183.BorderWidthRight = 1f;
                   cell1209337183.BorderWidthTop = 0f;
                   cell1209337183.BorderWidthBottom = 0f;

                   cell1209337183.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                   table2.AddCell(cell1209337183);





                   PdfPCell cell120933719 = new PdfPCell(new Phrase("Surcharge : ", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                   cell120933719.Colspan = 2;
                   cell120933719.BorderWidthLeft = 1f;
                   cell120933719.BorderWidthRight = 0f;
                   cell120933719.BorderWidthTop = 0f;
                   cell120933719.BorderWidthBottom = 0f;

                   cell120933719.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                   table2.AddCell(cell120933719);

                   PdfPCell cell1209337191 = new PdfPCell(new Phrase("As Applicable", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                   cell1209337191.Colspan = 2;
                   cell1209337191.BorderWidthLeft = 0f;
                   cell1209337191.BorderWidthRight = 0f;
                   cell1209337191.BorderWidthTop = 0f;
                   cell1209337191.BorderWidthBottom = 0f;

                   cell1209337191.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                   table2.AddCell(cell1209337191);

                   PdfPCell cell1209337192 = new PdfPCell(new Phrase("Final Inspection :", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                   cell1209337192.Colspan = 2;
                   cell1209337192.BorderWidthLeft = 0f;
                   cell1209337192.BorderWidthRight = 0f;
                   cell1209337192.BorderWidthTop = 0f;
                   cell1209337192.BorderWidthBottom = 0f;

                   cell1209337192.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                   table2.AddCell(cell1209337192);


                   PdfPCell cell1209337193 = new PdfPCell(new Phrase("At Our End", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                   cell1209337193.Colspan = 1;
                   cell1209337193.BorderWidthLeft = 0f;
                   cell1209337193.BorderWidthRight = 1f;
                   cell1209337193.BorderWidthTop = 0f;
                   cell1209337193.BorderWidthBottom = 0f;

                   cell1209337193.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                   table2.AddCell(cell1209337193);

                   PdfPCell cell120933720 = new PdfPCell(new Phrase("Exicise Duty : ", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                   cell120933720.Colspan = 2;
                   cell120933720.BorderWidthLeft = 1f;
                   cell120933720.BorderWidthRight = 0f;
                   cell120933720.BorderWidthTop = 0f;
                   cell120933720.BorderWidthBottom = 0f;

                   cell120933720.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                   table2.AddCell(cell120933720);


                   PdfPCell cell1209337201 = new PdfPCell(new Phrase("As Applicable", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                   cell1209337201.Colspan = 2;
                   cell1209337201.BorderWidthLeft = 0f;
                   cell1209337201.BorderWidthRight = 0f;
                   cell1209337201.BorderWidthTop = 0f;
                   cell1209337201.BorderWidthBottom = 0f;

                   cell1209337201.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                   table2.AddCell(cell1209337201);

                   PdfPCell cell1209337202 = new PdfPCell(new Phrase("Pkg. Detail :", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                   cell1209337202.Colspan = 2;
                   cell1209337202.BorderWidthLeft = 0f;
                   cell1209337202.BorderWidthRight = 0f;
                   cell1209337202.BorderWidthTop = 0f;
                   cell1209337202.BorderWidthBottom = 0f;

                   cell1209337202.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                   table2.AddCell(cell1209337202);

                   PdfPCell cell1209337203 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                   cell1209337203.Colspan = 1;
                   cell1209337203.BorderWidthLeft = 0f;
                   cell1209337203.BorderWidthRight = 1f;
                   cell1209337203.BorderWidthTop = 0f;
                   cell1209337203.BorderWidthBottom = 0f;

                   cell1209337203.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                   table2.AddCell(cell1209337203);






                   PdfPCell cell120933721 = new PdfPCell(new Phrase("Freight : ", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                   cell120933721.Colspan = 2;
                   cell120933721.BorderWidthLeft = 1f;
                   cell120933721.BorderWidthRight = 0f;
                   cell120933721.BorderWidthTop = 0f;
                   cell120933721.BorderWidthBottom = 1f;

                   cell120933721.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                   table2.AddCell(cell120933721);

                   PdfPCell cell1209337212 = new PdfPCell(new Phrase("Nil", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                   cell1209337212.Colspan = 5;
                   cell1209337212.BorderWidthLeft = 0f;
                   cell1209337212.BorderWidthRight = 1f;
                   cell1209337212.BorderWidthTop = 0f;
                   cell1209337212.BorderWidthBottom = 1f;

                   cell1209337212.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                   table2.AddCell(cell1209337212);





                   PdfPCell cell120933722 = new PdfPCell(new Phrase("Remark : " + ds.Tables[0].Rows[0]["Remark"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                   cell120933722.Colspan = 7;
                   cell120933722.Rowspan = 3;
                   cell120933722.BorderWidthLeft = 1f;
                   cell120933722.BorderWidthRight = 1f;
                   cell120933722.BorderWidthTop = 0f;
                   cell120933722.BorderWidthBottom = 1f;

                   cell120933722.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                   table2.AddCell(cell120933722);


                   PdfPCell cell120933723 = new PdfPCell(new Phrase("Authorization :", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                   cell120933723.Colspan = 7;
                   cell120933723.Rowspan = 3;
                   cell120933723.BorderWidthLeft = 1f;
                   cell120933723.BorderWidthRight = 1f;
                   cell120933723.BorderWidthTop = 0f;
                   cell120933723.BorderWidthBottom = 0f;

                   cell120933723.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                   table2.AddCell(cell120933723);

                   PdfPCell cell1209337231 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                   cell1209337231.Colspan = 7;
                   cell1209337231.Rowspan = 3;
                   cell1209337231.BorderWidthLeft = 1f;
                   cell1209337231.BorderWidthRight = 1f;
                   cell1209337231.BorderWidthTop = 0f;
                   cell1209337231.BorderWidthBottom = 0f;

                   cell1209337231.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                   table2.AddCell(cell1209337231);

                   PdfPCell cell1209337232 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                   cell1209337232.Colspan = 7;
                   cell1209337232.Rowspan = 3;
                   cell1209337232.BorderWidthLeft = 1f;
                   cell1209337232.BorderWidthRight = 1f;
                   cell1209337232.BorderWidthTop = 0f;
                   cell1209337232.BorderWidthBottom = 0f;

                   cell1209337232.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                   table2.AddCell(cell1209337232);

                   PdfPCell cell120933724 = new PdfPCell(new Phrase("Purchase Executive                                                                  Head-Purchase                                                                     President ", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                   cell120933724.Colspan = 7;
                   cell120933724.Rowspan = 5;
                   cell120933724.BorderWidthLeft = 1f;
                   cell120933724.BorderWidthRight = 1f;
                   cell120933724.BorderWidthTop = 0f;
                   cell120933724.BorderWidthBottom = 1f;

                   cell120933724.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                   table2.AddCell(cell120933724);

                   document.Add(table);
                   document.Add(table1);
                   document.Add(table2);

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