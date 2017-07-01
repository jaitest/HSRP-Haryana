using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;


namespace HSRP.Transaction
{
    public partial class GoodsDispatchRegister1 : System.Web.UI.Page
    {

        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        int UserType;
        int HSRPStateID;
        int intHSRPStateID;
        int RTOLocationID;
        int intOrgID;
        int intClientID;
        int UID;
        int DispatchID = 0;
        string DispatchChildID;
        int EditID = 0;
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();

        StringBuilder SBSQL = new StringBuilder();

        protected void Page_Load(object sender, EventArgs e)
        {
            Utils.GZipEncodePage();
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {
                int.TryParse(Session["UserType"].ToString(), out UserType);
                int.TryParse(Session["UserHSRPStateID"].ToString(), out HSRPStateID);
                int.TryParse(Session["UserRTOLocationID"].ToString(), out RTOLocationID);
                int.TryParse(Session["UID"].ToString(), out UID);

                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                //strUserID = Session["UID"].ToString();
                ComputerIP = Request.UserHostAddress;

                LabelCreatedID.Text = Utils.getDataSingleValue("Select UserFirstName + space(2)+UserLastName as UserName From Users where UserID=" + UID.ToString(), CnnString, "UserName");
                LabelCreatedDateTime.Text = DateTime.Now.ToString();

                if (!IsPostBack)
                {
                   // gvEG.Visible = false;
                    BindInvoice();
                    FilldropDownState();
                    FilldropDownRTOLocation();

                    InitialSetting();
                    //H_POheaderID1.Value = Request.QueryString["POheaderID1"].ToString();
                    string id = H_POheaderID1.Value.ToString();
                    if (Request["Mode"].ToString() == "Edit")
                    {
                       // btnAddItem.Visible = false;
                        btnRecord.Text = "Update";
                        EditID = Convert.ToInt32(Request["DispatchID"].ToString());

                        string selectQry = "select GoodsDispatchType,DispatchToStateID,DispatchToRTOLocationID,RTOAddress,Remarks from GoodsDispatchRegister where DispatchID='" + EditID + "' ";
                        DataTable dt = Utils.GetDataTable(selectQry, CnnString);

                        
                        ddlDispatchType.SelectedValue = dt.Rows[0]["GoodsDispatchType"].ToString();
                      
                        SQLString = "select HSRPStateName,HSRP_StateID from HSRPState Order by HSRPStateName";
                        Utils.PopulateDropDownList(dropDownListOrg, SQLString.ToString(), CnnString, "--Select State--");
                        dropDownListOrg.SelectedValue = dt.Rows[0]["DispatchToStateID"].ToString();

                        SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + dt.Rows[0]["DispatchToStateID"].ToString() + " Order by RTOLocationName";
                        Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), CnnString, "--Select RTO--");
                        dropDownListClient.SelectedValue = dt.Rows[0]["DispatchToRTOLocationID"].ToString();

                        txtAddress.Text = dt.Rows[0]["RTOAddress"].ToString();
                        textboxRemark.Text = dt.Rows[0]["Remarks"].ToString();

                       // buildGrid();
                        
                    }
                    else
                    {
                       // btnAddItem.Visible = true;
                       // btnRecord.Text = "Save";
                        //string SQLdispachCode = "select LastNo from Prefix where HSRP_StateID='" + HSRPStateID + "' AND RTOLocationID='" + RTOLocationID + "' AND PrefixFor='Dispatch' ";
                        //int LastNo = Convert.ToInt32(Utils.getScalarCount(SQLdispachCode, CnnString));
                        //LastNo = LastNo + 1;

                        //string updateLastNo = "update Prefix set LastNo='" + LastNo + "' where HSRP_StateID='" + HSRPStateID + "' AND RTOLocationID='" + RTOLocationID + "' AND PrefixFor='Dispatch' ";
                        //Utils.ExecNonQuery(updateLastNo, CnnString);

                        //string dispachCode = string.Empty;
                        //dispachCode = "Dispatch - " + Session["LastNo"].ToString();
                        //textboxDispatchCode.Text = dispachCode;
                        //FilldropDownListOrganization();
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
            OrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            OrderDate.MaxDate = DateTime.Parse(MaxDate);
            CalendarOrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarOrderDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
        }
       protected void gvEG_RowDataBound(object sender, GridViewRowEventArgs e)
        {
           
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ProductName = (DropDownList)e.Row.FindControl("ddlProductName");
                if (ProductName != null)
                {
                    int.TryParse(dropDownListOrg.SelectedValue, out intHSRPStateID);
                    //SQLString = "select ProductID, ProductCode from Product where HSRP_StateID='2' and ProductCode!='STICKER'  and ProductCode!='SNAP LOCK' order by ProductCode";
                    SQLString = "SELECT Invoice_InvoiceHeader.HeaderID, Invoice_InvoiceOrder.ProductID, Product.ProductCode FROM Invoice_InvoiceHeader INNER JOIN Invoice_InvoiceOrder ON Invoice_InvoiceHeader.HeaderID = Invoice_InvoiceOrder.HeaderID INNER JOIN Product ON Invoice_InvoiceOrder.ProductID = Product.ProductID where Invoice_InvoiceHeader.HeaderID='" + dropDownInvoiceNo.SelectedValue + "' and Product.ProductCode!='STICKER'  and Product.ProductCode!='SNAP LOCK' order by Product.ProductCode";
                    Utils.PopulateDropDownList(ProductName, SQLString.ToString(), CnnString, "--Select Product--");
                    //ddlDepartment.SelectedValue = gvEG.DataKeys[e.Row.RowIndex].Values[1].ToString(); 
                }
            }
            if (e.Row.RowType == DataControlRowType.EmptyDataRow)
            {
                DropDownList ProductName = (DropDownList)e.Row.FindControl("ddlProductName");
                if (ProductName != null)
                {
                    int.TryParse(dropDownListOrg.SelectedValue, out intHSRPStateID);
                    //SQLString = "select ProductID, ProductCode from Product where HSRP_StateID='2' and ProductCode!='STICKER'  and ProductCode!='SNAP LOCK' order by ProductCode";
                    SQLString = "SELECT Invoice_InvoiceHeader.HeaderID, Invoice_InvoiceOrder.ProductID, Product.ProductCode FROM Invoice_InvoiceHeader INNER JOIN Invoice_InvoiceOrder ON Invoice_InvoiceHeader.HeaderID = Invoice_InvoiceOrder.HeaderID INNER JOIN Product ON Invoice_InvoiceOrder.ProductID = Product.ProductID where Invoice_InvoiceHeader.HeaderID='" + dropDownInvoiceNo.SelectedValue + "' and Product.ProductCode!='STICKER'  and Product.ProductCode!='SNAP LOCK' order by Product.ProductCode";
                    Utils.PopulateDropDownList(ProductName, SQLString.ToString(), CnnString, "--Select Product--"); 
                }
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                DropDownList ProductName = (DropDownList)e.Row.FindControl("ddlProductName");
                int.TryParse(dropDownListOrg.SelectedValue, out intHSRPStateID);
                //SQLString = "select ProductID, ProductCode from Product where HSRP_StateID='" + intHSRPStateID + "' and ProductCode!='STICKER'  and ProductCode!='SNAP LOCK' order by ProductCode";
                //SQLString = "select ProductID, ProductCode from Product where HSRP_StateID='2' and ProductCode!='STICKER'  and ProductCode!='SNAP LOCK' order by ProductCode";
                SQLString = "SELECT Invoice_InvoiceHeader.HeaderID, Invoice_InvoiceOrder.ProductID, Product.ProductCode FROM Invoice_InvoiceHeader INNER JOIN Invoice_InvoiceOrder ON Invoice_InvoiceHeader.HeaderID = Invoice_InvoiceOrder.HeaderID INNER JOIN Product ON Invoice_InvoiceOrder.ProductID = Product.ProductID where Invoice_InvoiceHeader.HeaderID='" + dropDownInvoiceNo.SelectedValue + "' and Product.ProductCode!='STICKER'  and Product.ProductCode!='SNAP LOCK' order by Product.ProductCode";
                Utils.PopulateDropDownList(ProductName, SQLString.ToString(), CnnString, "--Select Product--"); 
            }



            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList Prefix = (DropDownList)e.Row.FindControl("ddlPrefix");
                if (Prefix != null)
                {  
                    // ======================== Bind Prefix in GridView ===================================================
                    SQLString = "select Prefix, PrefixID from PrefixLaserNo where HSRP_StateID='" + dropDownListOrg.SelectedValue + "' and ActiveStatus='Y'";
                    Utils.PopulateDropDownList(Prefix, SQLString.ToString(), CnnString, "--Select Prefix--");
                }
            }
            if (e.Row.RowType == DataControlRowType.EmptyDataRow)
            {
                DropDownList Prefix = (DropDownList)e.Row.FindControl("ddlPrefix");
                if (Prefix != null)
                { 
                    // ======================== Bind Prefix in GridView ===================================================
                    SQLString = "select Prefix, PrefixID from PrefixLaserNo where HSRP_StateID='" + dropDownListOrg.SelectedValue + "' and ActiveStatus='Y'";
                    Utils.PopulateDropDownList(Prefix, SQLString.ToString(), CnnString, "--Select Prefix--");
                }
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                DropDownList Prefix = (DropDownList)e.Row.FindControl("ddlPrefix"); 
                // ======================== Bind Prefix in GridView ===================================================
                SQLString = "select Prefix, PrefixID from PrefixLaserNo where HSRP_StateID='" + dropDownListOrg.SelectedValue + "' and ActiveStatus='Y'";
                Utils.PopulateDropDownList(Prefix, SQLString.ToString(), CnnString, "--Select Prefix--");
            }


        }


        
        #region DropDown

        private void FilldropDownState()
        {
            if (UserType.ToString() == "1" && UserType.ToString() == "0")
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState where ActiveStatus='Y' Order by HSRPStateName";
            }
            else
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState where ActiveStatus='Y' and HSRP_StateID='" + HSRPStateID + "' Order by HSRPStateName";
            }
            Utils.PopulateDropDownList(dropDownListOrg, SQLString.ToString(), CnnString, "--Select State--");
           
        }

        private void FilldropDownRTOLocation()
        {
            int.TryParse(dropDownListOrg.SelectedValue, out intOrgID);
            SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + intOrgID + " and ActiveStatus='Y' Order by RTOLocationName";

            Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), CnnString, "--Select RTO--");
            Utils.PopulateDropDownList(ddlFromlocation, SQLString.ToString(), CnnString, "--Select RTO--");
            


        }


        #endregion

        protected void dropDownListOrg_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dropDownListOrg.SelectedItem.Text != "--Select State--")
            {
                dropDownListClient.Visible = true;
                 FilldropDownRTOLocation();
              

            }
        }

        protected void dropDownListClient_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dropDownListClient.SelectedItem.Text != "--Select RTO--")
            {
                
                SQLString = "select RTOLocationAddress from RTOLocation Where RTOLocationID=" + dropDownListClient.SelectedValue + " Order by RTOLocationName";
                txtAddress.Text = Utils.getDataSingleValue(SQLString, CnnString, "RTOLocationAddress"); 
            }
        }

      

        public void btnClose_Click(object sender, EventArgs oArgs)
        {
            ModalID.Visible = false;
           // ModalPopupExtender1.Hide();
            //buildGrid();
            //buildGrid();
        }

        public void BindInvoice()
        {   
            //select customerid,InvoiceNo from dbo.DeliveryChallan where ActiveStatus='Y' order by InvoiceNo
            //string  Invoice = "select HeaderID from Invoice_InvoiceHeader where Status='Valid' and ActiveStatus='N'";
            string Invoice = "select customerid,InvoiceNo,headerid from dbo.DeliveryChallan where ActiveStatus='Y' order by InvoiceNo";
            Utils.PopulateDropDownList(dropDownInvoiceNo, Invoice.ToString(), CnnString, " ");
            
        }
        //protected void btnRecord_Click(object sender, EventArgs e)
        //{
        //    String[] StringOrderDate = OrderDate.SelectedDate.ToString().Split('/');
        //    String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
        //    DateTime OrderDate1 = new DateTime(Convert.ToInt32(StringOrderDate[2].Split(' ')[0]), Convert.ToInt32(StringOrderDate[0]), Convert.ToInt32(StringOrderDate[1]));
        //    if (Request["Mode"].ToString() == "Edit")
        //    {
        //        string sqlUpdateDispatchRegister = "update GoodsDispatchRegister set HSRP_StateID='" + HSRPStateID + "',RTOLocationID='" + RTOLocationID + "',GoodsDispatchType='" + ddlDispatchType.SelectedValue + "',DispatchToStateID='" + dropDownListOrg.SelectedValue + "',DispatchToRTOLocationID='" + dropDownListClient.SelectedValue + "',RTOAddress='" + txtAddress.Text + "',DispatchBy='" + UID + "',Remarks='" + textboxRemark.Text + "',VehRegNo='" + txtVehRegNo.Text + "',PONo='" + dropDownInvoiceNo.SelectedItem.Text + "',PODate='" + OrderDate1 + "' where dispatchid='" + Request["DispatchID"].ToString().Trim() + "'";
        //        Utils.ExecNonQuery(sqlUpdateDispatchRegister,CnnString);
                
        //        lblSucMsg.Text = "Record Updated";
               
        //    }
        //    else
        //    {
        //        string LastNo = string.Empty;
                
                 
                 
        //        string  InsertGoodDispatchRegister = "Insert into GoodsDispatchRegister(HSRP_StateID,RTOLocationID,DispatchCode,GoodsDispatchType,DispatchToStateID,DispatchToRTOLocationID,RTOAddress,DispatchDate,DispatchBy,Remarks,dispatchDeliveryStatus,PODate) values ('"+HSRPStateID+"','"+RTOLocationID+"','"+dropDownInvoiceNo.SelectedItem.Text.Trim()+"','"+ddlDispatchType.SelectedItem.Text.Trim()+"','"+dropDownListOrg.SelectedValue+"','"+dropDownListClient.SelectedValue+"','"+txtAddress.Text.Trim()+"',Getdate(),'"+UID+"','"+textboxRemark.Text.Trim()+"','N','"+OrderDate1+"')";
        //        int i = Utils.ExecNonQuery(InsertGoodDispatchRegister, CnnString);
        //        if (i > 0)
        //        {
        //            int MaxDispatchID = Utils.getScalarCount("select Max(DispatchID) as DispatchID from GoodsDispatchRegister", CnnString);

        //            SQLString ="select ID,ProductID,Quantity,Prefix,LaserCodeNoFrom, LaserCodeNoTo from dbo.Invoice_InvoiceOrder where headerID='"+dropDownInvoiceNo.SelectedValue+"'";

        //            DataTable dt = Utils.GetDataTable(SQLString, CnnString);
                    
        //            StringBuilder StrBullder = new StringBuilder();

        //            foreach (DataRow dtrows in dt.Rows)
        //            {
        //                StrBullder.Append("Insert into GoodsDispatchedChild (DispatchID,ProductID,Quantity,Prefix,LaserFrom,LaserTo) values ('"+MaxDispatchID+"','"+dtrows["ProductID"]+"','"+dtrows["Quantity"]+"','"+dtrows["Prefix"]+"','"+dtrows["LaserCodeNoFrom"]+"','"+dtrows["LaserCodeNoTo"]+"');");
        //            }

        //            try
        //            {
        //               int d = Utils.ExecNonQuery(StrBullder.ToString(), CnnString);
        //                if (d>0)
        //                {
        //                    SQLString = "update Invoice_InvoiceHeader set ActiveStatus='Y' where headerID='" + dropDownInvoiceNo.SelectedValue + "'";
        //                    Utils.ExecNonQuery(SQLString, CnnString);
        //                }
        //            }
        //            catch
        //            {
        //            }
                     
        //        } 

                
        //    }
        //}


        public void gridbind()
        {
            
           
        }

        protected void dropDownInvoiceNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            displaygriddata();

        }

        private void displaygriddata()
        {
            if (dropDownInvoiceNo.SelectedItem.Text != "--Select Invoice--")
            {

                //SQLString = "SELECT a.ID, b.ProductCode, a.Quantity, a.LaserCodeNoFrom, a.LaserCodeNoTo, a.Prefix FROM Invoice_InvoiceOrder a INNER JOIN Product b ON a.ProductID = b.ProductID where a.HeaderID='" + dropDownInvoiceNo.SelectedValue + "'";
                // SQLString = "select PlateBarcodeDetail.*,goodsReceiveInvoiceData.id from goodsReceiveInvoiceData inner join PlateBarcodeDetail on goodsReceiveInvoiceData.PlantReceiveID=PlateBarcodeDetail.AutoId where goodsReceiveInvoiceData.ReceivedCode='" + dropDownInvoiceNo.SelectedItem + "' and goodsReceiveInvoiceData.Received ='N'";

                SQLString = "select PlateBarcodeDetail.*,goodsReceiveInvoiceData.id, goodsReceiveInvoiceData.manualquantity,"
                    + " (select sum(manualquantity)"
                                                            + " from [goodsDispatchInvoiceData] "
                                                            + " where plantreceiveid=goodsReceiveInvoiceData.id) as  manualquantity1 "
                    + " from goodsReceiveInvoiceData "
                    + " inner join PlateBarcodeDetail on goodsReceiveInvoiceData.PlantReceiveID=PlateBarcodeDetail.AutoId  "
                    + " left join [goodsDispatchInvoiceData] on [goodsDispatchInvoiceData].id=[goodsDispatchInvoiceData].PlantReceiveID  "
                    + " where goodsReceiveInvoiceData.ReceivedCode='" + dropDownInvoiceNo.SelectedItem + "' and goodsReceiveInvoiceData.Received <>'Y' ";


                DataTable dt = Utils.GetDataTable(SQLString, CnnString);
                if (dt.Rows.Count > 0)
                {

                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                }
            }
        }


        CheckBox chk;
        protected void CHKSelect1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk1 = GridView1.HeaderRow.FindControl("CHKSelect1") as CheckBox;
            if (chk1.Checked == true)
            {
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    //  string lblVehicleRegNo,FLasercode,RearLaserCode;
                    chk = GridView1.Rows[i].Cells[0].FindControl("CHKSelect") as CheckBox;
                    chk.Checked = true;
                }
            }
            else if (chk1.Checked == false)
            {
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    //  string lblVehicleRegNo,FLasercode,RearLaserCode;
                    chk = GridView1.Rows[i].Cells[0].FindControl("CHKSelect") as CheckBox;
                    chk.Checked = false;
                }
            }

        }
        StringBuilder sb = new StringBuilder();
        StringBuilder sb1 = new StringBuilder();

        int suc = 0;
        protected void btnRecord_Click(object sender, EventArgs e)
        {
            int jj = 0;
            string InsertGoodDispatchRegister = "Insert into GoodsDispatchRegister(HSRP_StateID,RTOLocationIDTo,RTOLocationIDFrom,DispatchCode,GoodsDispatchType,DispatchToStateID,DispatchToRTOLocationID,RTOAddress,DispatchDate,DispatchBy,Remarks,dispatchDeliveryStatus) values ('" + dropDownListOrg.SelectedValue.ToString() + "','" + dropDownListClient.SelectedValue.ToString() + "','" + ddlFromlocation.SelectedValue.ToString() + "','" + dropDownInvoiceNo.SelectedItem.Text.Trim() + "','" + ddlDispatchType.SelectedItem.Text.Trim() + "','" + dropDownListOrg.SelectedValue + "','" + dropDownListClient.SelectedValue + "','" + txtAddress.Text.Trim() + "',Getdate(),'" + UID + "','" + textboxRemark.Text.Trim() + "','N')";
            int  l = Utils.ExecNonQuery(InsertGoodDispatchRegister, CnnString);
            if (l > 0)
            {


                SQLString = "  select top 1 DispatchID from GoodsDispatchRegister order by DispatchID desc";

                DataTable dt = Utils.GetDataTable(SQLString, CnnString);

                //for (int i = 0; i < GridView1.Rows.Count; i++)
                //{

                //    chk = GridView1.Rows[i].Cells[0].FindControl("CHKSelect") as CheckBox;
                //    if (chk.Checked)
                //    {
                //        jj++;
                //    }
                //}
                //if (jj == 0)
                //{
                //    lblErrMsg.Text = "Please select atleast one row";
                //    return;
                //}

                //for (int i = 0; i < GridView1.Rows.Count; i++)
                //{
                //    chk = GridView1.Rows[i].Cells[0].FindControl("CHKSelect") as CheckBox;
                //    if (chk.Checked == true)
                //    {
                //        TextBox t;
                //        t = (TextBox)GridView1.Rows[i].Cells[7].FindControl("TextBox1");
                //        Label id = GridView1.Rows[i].Cells[4].FindControl("id") as Label;
                //        //  labtext = GridView1.Rows[i].Cells[0].Text;
                //        //  sb.Append("update DeliveryChallanDetail set ManualQuantity='" + t.Text + "' where  AutoId='" + id.Text + "'");
                //        sb.Append("update goodsReceiveInvoiceData set Received='Y' where autoid='" + id.Text + "'" + Environment.NewLine);
                //    }
                //    // Label1.Text = labtext;


                //}


                //int j = Utils.ExecNonQuery(sb.ToString(), CnnString);
                //if (j > 0)
                //{
                int.TryParse(Session["UserRTOLocationID"].ToString(), out RTOLocationID);
                int.TryParse(Session["UID"].ToString(), out UID);
                int qty = 0;
                int qty1 = 0;
                string status = string.Empty;
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    chk = GridView1.Rows[i].Cells[0].FindControl("CHKSelect") as CheckBox;
                    if (chk.Checked == true)
                    {
                        TextBox t;
                        t = (TextBox)GridView1.Rows[i].Cells[8].FindControl("TextBox1");
                        Label id = GridView1.Rows[i].Cells[4].FindControl("id") as Label;
                        Label plantid = GridView1.Rows[i].Cells[4].FindControl("plantid") as Label;

                       Label lblquentity = GridView1.Rows[i].Cells[6].FindControl("lblquentity") as Label;
                       Label lblPreQuantity = GridView1.Rows[i].Cells[7].FindControl("lblPreQuantity") as Label;
                       
                         if (lblPreQuantity.Text == "")
                         {
                             qty = int.Parse(t.Text);

                         }
                         else
                         {
                             qty = int.Parse(lblPreQuantity.Text) + int.Parse(t.Text);
                         }
                         if (lblPreQuantity.Text == "")
                         {
                             lblPreQuantity.Text = "0";
                         }
                         else
                         {
                             qty1 = int.Parse(lblquentity.Text) - int.Parse(lblPreQuantity.Text);
                         }
                       if (qty <= int.Parse(lblquentity.Text))
                       {
                           if (qty == int.Parse(lblquentity.Text))
                           {
                               status = "Y";
                           }
                           else
                           {
                               status = "Parcial";
                           }
                           //  labtext = GridView1.Rows[i].Cells[0].Text;

                           sb.Append("update goodsReceiveInvoiceData set Received='" + status + "' where id='" + id.Text + "'" + Environment.NewLine);
                           //  string qq = "insert into  goodsDispatchInvoiceData (PlantReceiveID,PlantID,DispatchCode,RtoLocation,Userid,ManualQuantity,ReceiveID)values('" + id.Text.ToString() + "','" + plantid.Text.ToString() + "','" + dropDownInvoiceNo.SelectedValue + "','" + RTOLocationID + "','" + UID + "','" + t.Text + "','" + dt.Rows[0]["DispatchID"].ToString() + "')" + Environment.NewLine;
                           sb1.Append("insert into  goodsDispatchInvoiceData (PlantReceiveID,PlantID,DispatchCode,RtoLocation,Userid,ManualQuantity,ReceiveID)values('" + id.Text.ToString() + "','" + plantid.Text.ToString() + "','" + dropDownInvoiceNo.SelectedValue + "','" + RTOLocationID + "','" + UID + "','" + t.Text + "','" + dt.Rows[0]["DispatchID"].ToString() + "')" + Environment.NewLine);
                           suc++;
                       }
                       else
                       {
                           //string script1 = "<script type='text/javascript' language='javascript'>if(confirm(\"Record save sucessfully . click ok to close form\")) { parent.googlewin.close();} else {return false;}</script>";
                           //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "confirm", script1); 

                           string closescript1 = "<script>alert('You Can Not Select Manual Quantity More Than : " + qty1 + "')</script>";
                           Page.RegisterStartupScript("abc", closescript1);
                       }
                    }
                    // Label1.Text = labtext;

                }
                  //  }
                if (suc > 0)
                {
                    int k = Utils.ExecNonQuery(sb1.ToString(), CnnString);
                    int m = Utils.ExecNonQuery(sb.ToString(), CnnString);
                    lblSucMsg.Text = "Updated Records Sucessfull";
                }

                    //LblMessage.Text = "Updated Records Sucessfull";
                    //printInvoice1(invoNo);
                    // show();
                //}
                //else
                //{
                //    // LblMessage.Text = "Updated Records Not Sucessfull";
                //}

                displaygriddata();
            }

        }

        protected void CalendarOrderDate_Load(object sender, EventArgs e)
        {
            CalendarOrderDate.MinDate = DateTime.Now.AddDays(-2);
            CalendarOrderDate.SelectedDate = DateTime.Now;
            CalendarOrderDate.MaxDate = DateTime.Now;
        }


         
    }
}