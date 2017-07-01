using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace HSRP.Master
{
    public partial class InventoryChallanCreate : System.Web.UI.Page
    {

        string CnnString = string.Empty;
        string CnnString1 = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;

        int UserType;
        int HSRPStateID;
        int RTOLocationID;
        int intOrgID;
        int intClientID;
        int DispatchID = 0;
        string DispatchChildID;
        int EditID;
        string test = string.Empty;
      
        DataTable dt = new DataTable();
        string strquery = string.Empty;
        string client = string.Empty;
        string productname=string.Empty;
        string productcolor=string.Empty;
        string productsize=string.Empty;
        string prefix=string.Empty;
        string strYesNo = string.Empty;
        
        string InvoiceNo=string.Empty;
        string Quantity=string.Empty;
        string LaseredFrom=string.Empty;
        string LaseredTo=string.Empty;
        string Rate=string.Empty;
                   




        protected void Page_Load(object sender, EventArgs e)
        {
            lblErrMsg.Text = "";
            LblMessage.Text = "";
            Utils.GZipEncodePage();
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {
                RTOLocationID = Convert.ToInt32(Session["UserRTOLocationID"].ToString());
                UserType = Convert.ToInt32(Session["UserType"].ToString());
                HSRPStateID = Convert.ToInt32(Session["UserHSRPStateID"].ToString());

                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                CnnString1 = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();

                strUserID = Session["UID"].ToString();
                ComputerIP = Request.UserHostAddress;

                if (!IsPostBack)
                {
                    DataTable dt = new DataTable();
                
                    dt.Columns.Add(new System.Data.DataColumn("Invoice No", typeof(String)));
                    dt.Columns.Add(new System.Data.DataColumn("Client", typeof(String)));
                    dt.Columns.Add(new System.Data.DataColumn("Product Name", typeof(String)));
                    dt.Columns.Add(new System.Data.DataColumn("Product Size", typeof(String)));
                    dt.Columns.Add(new System.Data.DataColumn("Product Color", typeof(String)));
                    dt.Columns.Add(new System.Data.DataColumn("Prefix", typeof(String)));

                    dt.Columns.Add(new System.Data.DataColumn("Quantity", typeof(String)));
                    dt.Columns.Add(new System.Data.DataColumn("Lasered From", typeof(String)));
                    dt.Columns.Add(new System.Data.DataColumn("Lasered To", typeof(String)));
                    dt.Columns.Add(new System.Data.DataColumn("Rate", typeof(String)));
                    dt.Columns.Add(new System.Data.DataColumn("Lasered", typeof(String)));
                    ViewState["CurrentData"] = dt;
                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                    //FilldropDownListOrganization();
                    fillddlclient();
                    //fillddlproductname();
                    //fillddlproductsize();
                    DispatchInvoiceCodeNo();
    
                   
                }

            }

      }


        public void fillddlclient()
        {
          strquery="select RTOLocationName,RTOLocationID from RTOLocation where hsrp_stateid='" + HSRPStateID + "'";

            dt = Utils.GetDataTable(strquery, CnnString);
            ddlclient.DataSource = dt;
            ddlclient.DataTextField = "RTOLocationName";
            ddlclient.DataValueField = "RTOLocationID";
            ddlclient.DataBind();
            ddlclient.Items.Insert(0, "--Select Client Name--");
            ddlclient.Items[0].Value = "0";
        }

        //public void fillddlproductname()
        //{
        //    strquery = "select ProductName from PlateBarcodeDetail";
        //    dt1 = Utils.GetDataTable(strquery, CnnString);
        //    ddlprefixname.DataSource = dt1;
        //    ddlprefixname.DataTextField = "ProductName";
        //    ddlprefixname.DataBind();
        //    ddlprefixname.Items.Insert(0, "--Select Product Name--");
        //}
        //public void fillddlproductsize()
        //{
        //    strquery = "select distinct productsize  from platebarcodedetail";
        //    dt1 = Utils.GetDataTable(strquery, CnnString);
        //    ddlproductsize.DataSource = dt1;
        //    ddlproductsize.DataTextField = "ProductName";
        //    ddlproductsize.DataBind();
        //    ddlproductsize.Items.Insert(0, "--Select Product Name--");
        //}
        //public void fillddlproductcolor()
        //{
        //    strquery = "select distinct productcolor  from platebarcodedetail";
        //    dt1 = Utils.GetDataTable(strquery, CnnString);
        //    ddlproductcolor.DataSource = dt1;
        //    ddlproductcolor.DataTextField = "ProductName";
        //    ddlproductcolor.DataBind();
        //    ddlproductcolor.Items.Insert(0, "--Select Product Name--");

        //}

        public void fillddlproductprefix()
        {

        }

        private void DispatchInvoiceCodeNo()
        {
            string Query = "select customerid,InvoiceNo from dbo.DeliveryChallan where ActiveStatus='Y' order by InvoiceNo";
            dt = Utils.GetDataTable(Query, CnnString1);

        }
        private void BindGrid(int rowcount)
        {

            client = ddlclient.SelectedItem.ToString();
            productname = ddlproductname.SelectedItem.ToString();
            productsize = ddlproductsize.SelectedItem.ToString();
            productcolor = ddlproductcolor.SelectedItem.ToString();
            prefix = ddlprefixname.SelectedItem.ToString();

           
            
            DataRow dr;
            strYesNo = "Y";
            if (OptYes.Checked.Equals(true))
            {
                strYesNo = "Y";
            }

                dt = (DataTable)ViewState["CurrentData"];
                dr = dt.NewRow();
                dr[0] = txtinvoiceno.Text;
                dr[1] = client;
                dr[2] = productname;
                dr[3] = productsize;
                dr[4] = productcolor;
                dr[5] = prefix;
                dr[6] = txtquantity.Text;
                dr[7] = txtlasercodefrom.Text;
                dr[8] = txtlasercodeto.Text;
                dr[9] = txtrate.Text;
                dr[10] = strYesNo;
                dt.Rows.Add(dr);
                GridView1.DataSource = dt;
                GridView1.DataBind();
                ViewState["CurrentData"] = dt;
        }

        private bool validatescreen()
        {
            bool Flag= false;
            if (txtinvoiceno.Text == "")
            {
                lblErrMsg.Text = "";
                lblErrMsg.Text = "Please Enter Invoice No";
                Flag = true;                
            }
            else if (txtlasercodefrom.Text == "")
            {
                lblErrMsg.Text = "";
                lblErrMsg.Text = "Please Enter Lasered From Value";
                Flag = true;
            }
            else if (txtlasercodeto.Text == "")
            {
                lblErrMsg.Text = "";
                lblErrMsg.Text = "Please Enter Lasered To Value";
                Flag = true;
            }
            else if (txtquantity.Text == "")
            {
                lblErrMsg.Text = "";
                lblErrMsg.Text = "Please Enter Quantity";
                Flag = true;
            }
            else if (txtrate.Text == "")
            {
                lblErrMsg.Text = "";
                lblErrMsg.Text = "Please Enter Rate";
                Flag = true;
            }
            else if(ddlclient.SelectedItem.ToString().Equals("--Select Client Name--"))
            {
                lblErrMsg.Text="";
                lblErrMsg.Text="Please Select Client";
                Flag=true;
            }
            else if (ddlprefixname.SelectedItem.ToString().Equals("--Select Prefix Name--"))
            {
                lblErrMsg.Text="";
                lblErrMsg.Text="Please Select Prefix Name";
                Flag=true;
            }
            else if (ddlproductcolor.SelectedItem.ToString().Equals("--Select Product Color--"))
            {
                lblErrMsg.Text="";
                lblErrMsg.Text="Please Select Product Color";
                Flag=true;
            }
            else if (ddlproductname.SelectedItem.ToString().Equals("--Select Product Name--"))
            {
                lblErrMsg.Text = "";
                lblErrMsg.Text = "Please Select Product Name";
                Flag = true;

            }
            else if (ddlproductsize.SelectedItem.ToString().Equals("--Select Product Size--"))
            {
                lblErrMsg.Text="";
                lblErrMsg.Text="Please Select Product Size";
                Flag=true;
            }
            

            return Flag;
        }

        protected void btnadd_Click(object sender, EventArgs e)
        {
            if (ViewState["CurrentData"] != null)
            {
                dt = (DataTable)ViewState["CurrentData"];
                string flag="N";
                int count = dt.Rows.Count;
                for (int i = 0; i < count; i++)
                {
                    if ((int.Parse(dt.Rows[i]["Lasered From"].ToString()) >= int.Parse(txtlasercodefrom.Text) && int.Parse(dt.Rows[i]["Lasered TO"].ToString()) <= int.Parse(txtlasercodeto.Text)) || (int.Parse(dt.Rows[i]["Lasered From"].ToString()) == int.Parse(txtlasercodefrom.Text) || int.Parse(dt.Rows[i]["Lasered TO"].ToString()) == int.Parse(txtlasercodefrom.Text)) || (int.Parse(dt.Rows[i]["Lasered TO"].ToString()) >= int.Parse(txtlasercodeto.Text) && int.Parse(dt.Rows[i]["Lasered From"].ToString()) <= int.Parse(txtlasercodeto.Text) || (int.Parse(dt.Rows[i]["Lasered From"].ToString()) >= int.Parse(txtlasercodefrom.Text) && int.Parse(dt.Rows[i]["Lasered TO"].ToString()) <= int.Parse(txtlasercodefrom.Text)))) 
                     {
                        lblErrMsg.Text = "Duplicate Lasered No.";
                        flag="Y";
                     }
                  
                }
                if(flag=="N")
                {
                    lblErrMsg.Text ="";
                    if (validatescreen())
                    {
                        return;
                    }
                    int abc = int.Parse(txtlasercodefrom.Text) - int.Parse(txtlasercodeto.Text);
                    if (int.Parse(Quantity.ToString()) == abc)
                    {
                        BindGrid(count);
                    }

                        //BindGrid(count);
                    
         
                 }
            }
        }
        protected void OptYes_CheckedChanged(object sender, EventArgs e)
        {
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int serialNo=int.Parse(Utils.getScalarValue("Select Max(SerialNo) from PlateBarcodeDetail",CnnString));
            StringBuilder sb = new StringBuilder();
            dt = (DataTable)ViewState["CurrentData"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                serialNo++;
                InvoiceNo    = dt.Rows[i]["Invoice No"].ToString();
                productname  = dt.Rows[i]["Product Name"].ToString();
                productsize  = dt.Rows[i]["Product Size"].ToString();
                productcolor = dt.Rows[i]["Product Color"].ToString();
                prefix       = dt.Rows[i]["Prefix"].ToString();
                Quantity     = dt.Rows[i]["Quantity"].ToString();
                LaseredFrom  = dt.Rows[i]["Lasered From"].ToString();
                LaseredTo    = dt.Rows[i]["Lasered To"].ToString();
                Rate         = dt.Rows[i]["Rate"].ToString();
                strYesNo     = dt.Rows[i]["Lasered"].ToString();
                client       = dt.Rows[i]["ClientName"].ToString();


                sb.Append("insert into PlateBarcodeDetail(SerialNo,PlantId,InvoiceNo,ProductName,ProductSize,ProductColor,Prifix,Lasered,Laseredcodefrom,Laseredcodeto,Quantity,Rate) values('"+serialNo+"','1','"+InvoiceNo+"','"+productname+"','"+productsize+"','"+productcolor+"','"+prefix+"','"+strYesNo+"','"+LaseredFrom+"','"+LaseredTo+"','"+Quantity+"','"+Rate+"');");
                //sb.Append("insert into DeliveryChallan(    
            }
                SQLString=sb.ToString();    
                if(Utils.ExecNonQuery(SQLString,CnnString)>0)
                {
                    LblMessage.Text="Record Saved Successfully";
                }
                
                  

            }
        }
        
    }
