using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
namespace HSRP.Transaction
{
    public partial class DispatchFromWhereHouse : System.Web.UI.Page
    {
        

        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        string UserType = string.Empty;
        string HSRPStateID = string.Empty;
        string RTOLocationID = string.Empty;
        int intHSRPStateID;
   
        protected void Page_Load(object sender, EventArgs e)
        {
            LblMessage.Visible = false;
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
              }
              if (!IsPostBack)
              {
                  FilldropDownListOrganization();
                  //DispatchInvoiceCodeNo();
                 
              }
        }

        private void FillPrifix()
        {
            SQLString =  @"select distinct (PlateBarcodeDetail.prifix) as prefix
                    from goodsReceiveInvoiceData 
                    inner join PlateBarcodeDetail on goodsReceiveInvoiceData.PlantReceiveID=PlateBarcodeDetail.AutoId  
                     left join [goodsDispatchInvoiceData] on [goodsDispatchInvoiceData].id=[goodsDispatchInvoiceData].PlantReceiveID";
            Utils.PopulateDropDownList(ddlPrifix, SQLString.ToString(), CnnString, "--Select Prefix--");
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
        protected void DropDownListStateName_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilldropDownListClient();
            FillPrifix();
            fillProductSize();
        }

        private void fillProductSize()
        {


            SQLString = @"select distinct (PlateBarcodeDetail.ProductSize+' '+ProductColor) as productsize
                    from goodsReceiveInvoiceData 
                    inner join PlateBarcodeDetail on goodsReceiveInvoiceData.PlantReceiveID=PlateBarcodeDetail.AutoId  
                     left join [goodsDispatchInvoiceData] on [goodsDispatchInvoiceData].id=[goodsDispatchInvoiceData].PlantReceiveID";
            Utils.PopulateDropDownList(ddlSize, SQLString.ToString(), CnnString, "--Select Product Size--");
        }

        //private void DispatchInvoiceCodeNo()
        //{
        //    string Query = "select customerid,InvoiceNo from dbo.DeliveryChallan where ActiveStatus='Y' order by InvoiceNo";
        //    DataTable dt = Utils.GetDataTable(Query, CnnString);
        //    ddlDispatchcode.DataSource = dt;
        //    ddlDispatchcode.DataTextField = "InvoiceNo";
        //    ddlDispatchcode.DataValueField = "customerid";
        //    ddlDispatchcode.DataBind();
        //    ddlDispatchcode.Items.Insert(0, "--Select Dispatch InvoiceNo--");
        //    ddlDispatchcode.Items[0].Value = "0";
        //}
        private void FilldropDownListClient()
        {
            if (UserType == "0")
            {

                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);

                //SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + HSRPStateID + " and ActiveStatus!='N'   Order by RTOLocationName";
                SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + intHSRPStateID + " and ActiveStatus!='N'   Order by RTOLocationName";
                Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), CnnString, "--Select RTO Name--");

                //dataLabellbl.Visible = false;
                //TRRTOHide.Visible = false;
            }
            else
            {
                string UserID = Convert.ToString(Session["UID"]);
                SQLString = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, (a.RTOLocationCode+', ') as RTOCode, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID where UserRTOLocationMapping.UserID='" + UserID + "' ";
                DataTable dss = Utils.GetDataTable(SQLString, CnnString);
                //labelOrganization.Visible = true;
                //DropDownListStateName.Visible = true;
                //labelClient.Visible = true;
                //dropDownListClient.Visible = true;

                dropDownListClient.DataSource = dss;
                dropDownListClient.DataBind();
               // dataLabellbl.Visible = true;

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

        int co = 1;
        protected void btnSave_Click(object sender, EventArgs e)
        {
            List<Transaction.DispatchClass> lst = new List<Transaction.DispatchClass>();
            Transaction.DispatchClass DC = new DispatchClass();
            DC.Stateid = DropDownListStateName.SelectedValue.ToString();
            DC.StateName = DropDownListStateName.SelectedItem.ToString();
            DC.Rtlolocaitonid = dropDownListClient.SelectedValue.ToString();
            DC.RtlolocaitonName=dropDownListClient.SelectedItem.ToString();
            //DC.Dispatchto = ddlDispatchcode.SelectedItem.ToString();
            DC.Address = txtAddress.Text;
            //DC.Refno = txtRefNo.Text;
            DC.Productsizeid = ddlSize.SelectedItem.ToString();
            DC.Qty = txtManulQty.Text;
            DC.Prifix = ddlPrifix.SelectedItem.ToString();
            DC.LaserCodeFrom = txtlasercodeFrom.Text;
            DC.LaserCodeEnd = txtlasercodetoManul.Text;
            DC.Id = co.ToString();
            co++;


            if (Session["dispatch"] != null)
            {
                lst = (List<Transaction.DispatchClass>)Session["dispatch"];
            }
            lst.Add(DC);
            Session["dispatch"] = lst;




            if (lst.Count > 0)
            {
                Panel1.Visible = true;
                GridView1.DataSource = lst;
                GridView1.DataBind();

                txtlasercodeFrom.Text = (int.Parse(txtlasercodetoManul.Text)+1).ToString();
                txtManulQty.Text = "0";
                btnSaveDispatch.Visible = true;
            }
            else
            {
                Panel1.Visible = false;
            }
            
        }
        DataTable dt = new DataTable();
        protected void dropDownListClient_SelectedIndexChanged(object sender, EventArgs e)
        {
            SQLString = " select RTOLocationAddress from rtolocation where rtolocationid=" + dropDownListClient.SelectedValue.ToString() + "";
            dt=Utils.GetDataTable(SQLString, CnnString);
            txtAddress.Text = dt.Rows[0]["RTOLocationAddress"].ToString();

                
        }

        protected void ddlPrifix_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] Psize = ddlSize.SelectedItem.ToString().Split();

            SQLString = @"select top 1 lasercodefrom,lasercodeend from [DispatchFromWareHouse] where productSizeid='" + ddlSize.SelectedItem.ToString() + "' and Prifix='" + ddlPrifix.SelectedItem.ToString() + "' order  by DFWHID desc ";
            dt = Utils.GetDataTable(SQLString, CnnString);
            if (dt.Rows.Count > 0)
            {
                txtlasercodeFrom.Text = (int.Parse(dt.Rows[0]["lasercodeend"].ToString())+1).ToString();
                // txtlasercodeto.Text = dt.Rows[0]["lasercodeend"].ToString();
            }
            else
            {
               

                SQLString = @"select top 1  (PlateBarcodeDetail.Laseredcodefrom) as Laseredcodefrom
                    from goodsReceiveInvoiceData 
                    inner join PlateBarcodeDetail on goodsReceiveInvoiceData.PlantReceiveID=PlateBarcodeDetail.AutoId  
                     left join [goodsDispatchInvoiceData] on [goodsDispatchInvoiceData].id=[goodsDispatchInvoiceData].PlantReceiveID where PlateBarcodeDetail.prifix='" + ddlPrifix.SelectedItem.ToString().TrimStart() + "' and ProductSize='" + Psize[0].ToString() + "' and PlateBarcodeDetail.ProductColor='" + Psize[1].ToString() + "'  ";
                dt = Utils.GetDataTable(SQLString, CnnString);
                if (dt.Rows.Count > 0)
                {
                    txtlasercodeFrom.Text = dt.Rows[0]["Laseredcodefrom"].ToString();
                }
            }

          

            SQLString = @"  select  top 1 PlateBarcodeDetail.Laseredcodeto
                    from goodsReceiveInvoiceData 
                    inner join PlateBarcodeDetail on goodsReceiveInvoiceData.PlantReceiveID=PlateBarcodeDetail.AutoId  
                     left join [goodsDispatchInvoiceData] on [goodsDispatchInvoiceData].id=[goodsDispatchInvoiceData].PlantReceiveID where PlateBarcodeDetail.prifix='" + ddlPrifix.SelectedItem.ToString().TrimStart() + "' and ProductSize='" + Psize[0].ToString() + "' and PlateBarcodeDetail.ProductColor='" + Psize[1].ToString() + "' order by autoid desc  ";
            dt = Utils.GetDataTable(SQLString, CnnString);

            if (dt.Rows.Count > 0)
            {
                txtlasercodeto.Text = dt.Rows[0]["Laseredcodeto"].ToString();
            }


            txtQty.Text = (int.Parse(txtlasercodeto.Text) - int.Parse(txtlasercodeFrom.Text)).ToString();
        }

     

        //protected void txtlasercodeto_TextChanged(object sender, EventArgs e)
        //{
        //    txtManulQty.Text = (int.Parse(txtlasercodeto.Text) - int.Parse(txtlasercodeFrom.Text)).ToString();
        //}

        protected void txtManulQty_TextChanged(object sender, EventArgs e)
        {
            txtlasercodetoManul.Text = (int.Parse(txtManulQty.Text) + int.Parse(txtlasercodeFrom.Text)).ToString();
        }
        string query;
        protected void btnSaveDispatch_Click(object sender, EventArgs e)
        {
            //stateid, stateName, rtlolocaitonid, rtlolocaitonName,  address, productsizeid, qty, prifix, lasercodefrom, lasercodeend
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                //string stateid = GridView1.DataKeys[1].Values["stateid"].ToString();
                //string rtlolocaitonid = GridView1.DataKeys[1].Values["stateid"].ToString(); 
                TextBox txtAddress = (TextBox)GridView1.Rows[i].Cells[4].FindControl("txtAddress");
                TextBox txtProductSize = (TextBox)GridView1.Rows[i].Cells[5].FindControl("txtProductSize");
                TextBox txtprifix = (TextBox)GridView1.Rows[i].Cells[5].FindControl("txtprifix");
                TextBox txtlasercodefrom = (TextBox)GridView1.Rows[i].Cells[5].FindControl("txtlasercodefrom");
                TextBox txtlasercodeend = (TextBox)GridView1.Rows[i].Cells[5].FindControl("txtlasercodeend");
                TextBox txtqty = (TextBox)GridView1.Rows[i].Cells[5].FindControl("txtqty");
                Label lblstateid = (Label)GridView1.Rows[i].Cells[6].FindControl("lblstateid");
                Label lblrtlolocaitonid = (Label)GridView1.Rows[i].Cells[7].FindControl("lblrtlolocaitonid");
                sb.Append(@"insert into DispatchFromWareHouse([HSRP_stateID],[RTOLocaitonID],[address] ,[productsizeid],[Prifix],[LaserCodeFrom],[Lasercodeend],[QTY]) 
                                values('" + lblstateid.Text + "','" + lblrtlolocaitonid.Text + "','" + txtAddress.Text + "','" + txtProductSize.Text + "','" + txtprifix.Text + "','" + txtlasercodefrom.Text + "','" + txtlasercodeend.Text + "','" + txtqty.Text + "') ; " + System.Environment.NewLine);
              
            }

            int j = Utils.ExecNonQuery(sb.ToString(), CnnString);
            if (j > 0)
            {
                LblMessage.Visible = true;
                LblMessage.Text = "Record save Sucessfully";
                GridView1.DataSource = "";
                GridView1.DataBind();
                btnSaveDispatch.Visible = false;
                Session["dispatch"] = null;
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int id = int.Parse(btn.CommandArgument.ToString());
           // dt.Rows[id].Delete();

            DataView view = new DataView(dt);
            view.RowFilter = "ID"+id; // MyValue here is a column name

            // Delete these rows.
            foreach (DataRowView row in view)
            {
                row.Delete();
            }
           
        }

        protected void ddlSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillPrifix();
        } 
    }
}