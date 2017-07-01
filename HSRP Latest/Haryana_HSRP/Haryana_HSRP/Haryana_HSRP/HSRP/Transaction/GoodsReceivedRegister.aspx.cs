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
    public partial class GoodsReceivedRegister : System.Web.UI.Page
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
        StringBuilder SBSQL = new StringBuilder();
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
                    //FilldropDownListOrganization();
                    DispatchInvoiceCodeNo();

                    if (Request["Mode"].ToString() == "Edit")
                    {
                        Session["DispatchID"] = Convert.ToInt32(Request["DispatchID"].ToString());

                        string selectQry = "select DispatchCode,GoodsDispatchType,DispatchToStateID,DispatchToRTOLocationID,RTOAddress,Remarks from GoodsDispatchRegister where DispatchID='" + Convert.ToInt32(Session["DispatchID"].ToString()) + "' ";
                        DataTable dt = Utils.GetDataTable(selectQry, CnnString);

                        //  textboxDispatchCode.Text = dt.Rows[0]["DispatchCode"].ToString();
                        //txtReceiveCode.Text = dt.Rows[0]["GoodsDispatchType"].ToString();

                        SQLString = "select HSRPStateName from HSRPState where HSRP_StateID='" + dt.Rows[0]["DispatchToStateID"].ToString() + "'";
                        txtStateName.Text = Utils.getScalarValue(SQLString, CnnString);

                        SQLString = "select RTOLocationName from RTOLocation Where RTOLocationID=" + dt.Rows[0]["DispatchToRTOLocationID"].ToString();
                        txtRtoName.Text = Utils.getScalarValue(SQLString, CnnString);


                        txtAddress.Text = dt.Rows[0]["RTOAddress"].ToString();

                        //  buildGrid();

                    }


                }

            }

            //var  rows = GridView1.Rows;
            //foreach (GridViewRow row in rows)
            //{
            //    TextBox t = (TextBox)row.FindControl("TextBox1");
            //    t.Text = 0;
            //}





        }
        DataTable dt = new DataTable();
        private void DispatchInvoiceCodeNo()
        {
            string Query = "select customerid,InvoiceNo from dbo.DeliveryChallan where ActiveStatus='Y' order by InvoiceNo";
            dt = Utils.GetDataTable(Query, CnnString1);
            ddlDispatchcode.DataSource = dt;
            ddlDispatchcode.DataTextField = "InvoiceNo";
            ddlDispatchcode.DataValueField = "customerid";
            ddlDispatchcode.DataBind();
            ddlDispatchcode.Items.Insert(0, "--Select Dispatch InvoiceNo--");
            ddlDispatchcode.Items[0].Value = "0";
        }
        // #region Grid
        //public void buildGrid()
        //{
        //    try
        //    {

        //        intOrgID = HSRPStateID;
        //        intClientID = RTOLocationID;
        //        SQLString = "SELECT a.*,(select ProductCode from Product where ProductID=a.ProductID) as ProductCode FROM GoodsDispatchedDetail as a where [DispatchID]='" + Convert.ToInt32(Session["DispatchID"].ToString()) + "' and AcknowledgementStatus='N'";

        //        DataTable dt = Utils.GetDataTable(SQLString, CnnString);
        //        Grid1.DataSource = dt;
        //        Grid1.RunningMode = (ComponentArt.Web.UI.GridRunningMode)Enum.Parse(typeof(ComponentArt.Web.UI.GridRunningMode), "Client");
        //        Grid1.SearchOnKeyPress = true;
        //        Grid1.DataBind();
        //        Grid1.RecordCount.ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        lblErrMsg.Text = "Error in Populating Grid :" + ex.Message.ToString();
        //    }
        //}
        //public void OnNeedRebind(object sender, EventArgs oArgs)
        //{
        //    System.Threading.Thread.Sleep(200);
        //    Grid1.DataBind();
        //}
        //public void OnNeedDataSource(object sender, EventArgs oArgs)
        //{
        //    buildGrid();
        //}
        //public void OnPageChanged(object sender, ComponentArt.Web.UI.GridPageIndexChangedEventArgs oArgs)
        //{
        //    Grid1.CurrentPageIndex = oArgs.NewIndex;
        //}
        //public void OnFilter(object sender, ComponentArt.Web.UI.GridFilterCommandEventArgs oArgs)
        //{
        //    Grid1.Filter = oArgs.FilterExpression;
        //}
        //public void OnSort(object sender, ComponentArt.Web.UI.GridSortCommandEventArgs oArgs)
        //{
        //    Grid1.Sort = oArgs.SortExpression;
        //}
        //private void ddGridRunningMode_SelectedIndexChanged(object sender, System.EventArgs e)
        //{
        //    Grid1.RunningMode = (ComponentArt.Web.UI.GridRunningMode)Enum.Parse(typeof(ComponentArt.Web.UI.GridRunningMode), "Client");
        //    buildGrid();
        //    Grid1.DataBind();
        //    adjustToRunningMode();
        //}
        //public void OnGroup(object sender, ComponentArt.Web.UI.GridGroupCommandEventArgs oArgs)
        //{
        //    Grid1.GroupBy = oArgs.GroupExpression;
        //}
        //private void adjustToRunningMode()
        //{

        //    Grid1.SliderPopupClientTemplateId = "SliderTemplate";
        //    Grid1.SliderPopupOffsetX = 20;

        //}
        //#endregion

        //public void btnPutRemarks_Click(object sender, EventArgs oArgs)
        //{
        //    test = txtRemarks1.Text.Trim();

        //    //string UpdateStatus = "UPDATE GoodsDispatchedDetail SET AcknowledgementStatus='Y' WHERE DispatchDetailID='" + EditID + "'";
        //    //Utils.ExecNonQuery(UpdateStatus, CnnString);
        //    string insremarks = Session["insRem"].ToString();
        //    insremarks = insremarks.Replace("!@#$%^&*()", test);
        //    Utils.ExecNonQuery(insremarks, CnnString);
        //    ModalPopupExtender1.Hide();
        //    buildGrid();
        //}

        //protected void Grid1_ItemCommand(object sender, ComponentArt.Web.UI.GridItemCommandEventArgs e)
        //{
        //    EditID =Convert.ToInt32(e.Item["DispatchDetailID"].ToString());

        //    if (EditID == 0)
        //    {
        //        lblErrMsg.Text = "that is not valid Record.";
        //        return;
        //    }
        //    string chkStatus = printflag.Value;

        //    if (chkStatus == "Received")
        //    {
        //        string UpdateStatus = "UPDATE GoodsDispatchedDetail SET AcknowledgementStatus='Y' WHERE DispatchDetailID='" + EditID + "'";
        //        Utils.ExecNonQuery(UpdateStatus, CnnString);

        //        string InsertGoodsReceive = "INSERT INTO [GoodsReceivedDetail] ([ReceivedID],[ProductID],[Quantity],[LaserTo],[LaserFrom]) VALUES  ('" + EditID + "','" + e.Item["ProductID"].ToString() + "','" + e.Item["Quantity"].ToString() + "','" + e.Item["LaserTo"].ToString() + "','" + e.Item["LaserFrom"].ToString() + "')";
        //        Utils.ExecNonQuery(InsertGoodsReceive, CnnString);

        //        buildGrid();
        //    }
        //    else if (chkStatus == "Remarks")
        //    {
        //        string InsertGoodsReceive = "INSERT INTO [GoodsReceivedDetail] ([ReceivedID],[ProductID],[Quantity],[LaserTo],[LaserFrom],remarks) VALUES  ('" + EditID + "','" + e.Item["ProductID"].ToString() + "','" + e.Item["Quantity"].ToString() + "','" + e.Item["LaserTo"].ToString() + "','" + e.Item["LaserFrom"].ToString() + "','!@#$%^&*()')";
        //        Session["insRem"] = InsertGoodsReceive;
        //        ModalPopupExtender1.Show();   
        //    }


        //}
        DataTable dt1 = new DataTable();
        protected void ddlDispatchcode_SelectedIndexChanged(object sender, EventArgs e)
        {
            string query = "select (CustomerAddress1+', '+City+' '+State+' '+Pin)as address1,State from Customer where CustomerID='" + ddlDispatchcode.SelectedValue.ToString() + "'";
            dt = Utils.GetDataTable(query, CnnString1);
            if (dt.Rows.Count > 0)
            {
                txtStateName.Text = dt.Rows[0]["State"].ToString();
                txtAddress.Text = dt.Rows[0]["address1"].ToString();
            }
            txtReceiveCode.Text = ddlDispatchcode.SelectedItem.ToString();

            string query1 = "select (users.userfirstname+' '+users.userlastname)as name,rtolocation.rtolocationName from users inner join rtolocation on users.rtolocationid=rtolocation.rtolocationid where users.userid='" + Session["UID"].ToString() + "'";
            //    string query1 = "SELECT contactpersonname FROM PLANT where plantid='"+Session["UID"].ToString()+"'";
            // where plantid=1
            dt1 = Utils.GetDataTable(query1, CnnString);
            if (dt1.Rows.Count > 0)
            {
                txtRtoName.Text = dt1.Rows[0]["name"].ToString() + '-' + dt1.Rows[0]["rtolocationName"].ToString();
            }

            // string query3 = " SELECT * FROM dbo.PlateBarcodeDetail where InvoiceNo='" + ddlDispatchcode.SelectedItem.ToString() + "' and Received ='N' OR Received is NULL";
            string query3 = "SELECT distinct PlateBarcodeDetail.*,plant.PlantName,(select  sum(ManualQuantity) as manualquentity from goodsReceiveInvoiceData where  plantreceiveid=PlateBarcodeDetail.AutoID  group by plantreceiveid) as prequentity  FROM   dbo.PlateBarcodeDetail left join  goodsReceiveInvoiceData   on PlateBarcodeDetail.AutoID=goodsReceiveInvoiceData.plantreceiveid "+
" inner join plant on plant.plantid=PlateBarcodeDetail.plantid" +
    " where InvoiceNo='" + ddlDispatchcode.SelectedItem+ "' and PlateBarcodeDetail.Received <>'Y' OR PlateBarcodeDetail.Received is NULL";
            dt = Utils.GetDataTable(query3, CnnString1);
            if (dt.Rows.Count > 0)
            {
                lblPlantName.Text = dt.Rows[0]["plantname"].ToString();
                GridView1.DataSource = dt;
                GridView1.DataBind();
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
        int suc=0;
        protected void btnSave_Click(object sender, EventArgs e)
        {
            int jj = 0;
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {

                chk = GridView1.Rows[i].Cells[0].FindControl("CHKSelect") as CheckBox;
                if (chk.Checked)
                {
                    jj++;
                }
            }
            if (jj == 0)
            {
                lblErrMsg.Text = "Please select atleast one row";
                return;
            }

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
            //        sb.Append("update PlateBarcodeDetail set Received='Y' where autoid='" + id.Text + "'" + Environment.NewLine);
            //    }
            //    // Label1.Text = labtext;


            //}


           
            //if (j > 0)
            //{
            int qty = 0;
            int qty1 = 0;
            string status=string.Empty;
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
                        Label SerialNo = GridView1.Rows[i].Cells[10].FindControl("lblSerialno") as Label;
                       
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
                                status = "parcial";
                            }
                            //  labtext = GridView1.Rows[i].Cells[0].Text;
                            sb.Append("update PlateBarcodeDetail set Received='" + status + "' where autoid='" + id.Text + "'" + Environment.NewLine);
                            sb1.Append("  insert into  goodsReceiveInvoiceData (PlantReceiveID,PlantID,DispatchCode,ReceivedCode,RtoLocation,Userid,ManualQuantity,SerialNo)values('" + id.Text.ToString() + "','" + plantid.Text.ToString() + "','" + ddlDispatchcode.SelectedItem.ToString() + "','" + txtReceiveCode.Text + "','" + txtRtoName.Text + "','" + Session["UID"].ToString() + "','" + t.Text + "','" + SerialNo.Text + "')" + Environment.NewLine);
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
                if (suc > 0)
                {
                    int k = Utils.ExecNonQuery(sb1.ToString(), CnnString);
                    int j = Utils.ExecNonQuery(sb.ToString(), CnnString1);

                    LblMessage.Text = "Updated Records Sucessfull";
                }
                //printInvoice1(invoNo);
                // show();
            //}
            //else
            //{
            //    LblMessage.Text = "Updated Records Not Sucessfull";
            //}

                string query3 = "SELECT distinct PlateBarcodeDetail.*,(select  sum(ManualQuantity) as manualquentity from goodsReceiveInvoiceData where  plantreceiveid=PlateBarcodeDetail.AutoID  group by plantreceiveid) as prequentity " +
      " FROM   dbo.PlateBarcodeDetail left join  goodsReceiveInvoiceData   on PlateBarcodeDetail.AutoID=goodsReceiveInvoiceData.plantreceiveid " +
      " where InvoiceNo='" + ddlDispatchcode.SelectedItem + "' and PlateBarcodeDetail.Received <>'Y' OR PlateBarcodeDetail.Received is NULL";
                dt = Utils.GetDataTable(query3, CnnString1);
                if (dt.Rows.Count > 0)
                {
                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                }

        }
        //HSRP.Transaction.Class1 obj = new Transaction.Class1();
        //protected void btnAdd_Click(object sender, EventArgs e)
        //{
        //    //List<HSRP.Transaction.Class1> lst = new List<Transaction.Class1>();
        //    //obj.DispatchCode1 = ddlDispatchcode.SelectedValue.ToString();
        //    //obj.StateName1 = txtStateName.Text;
        //    //obj.ShippingAddresh1 = txtAddress.Text;
        //    //obj.ReceiveCode1 = txtReceiveCode.Text;
        //    //obj.Remarks1 = textboxRemark.Text;
        //    //obj.RTOlocation1 = txtRtoName.Text;
        //    //obj.DispatchCode21 = ddlDispatchcode.SelectedItem.ToString();
        //    //if (Session["receive"] != null)
        //    //{
        //    //    lst = (List < HSRP.Transaction.Class1 >) Session["receive"];
        //    //}
        //    //lst.Add(obj);
        //    //Session["receive"] = lst;
        //    string query = " SELECT * FROM dbo.PlateBarcodeDetail where InvoiceNo='" + ddlDispatchcode.SelectedItem.ToString() + "'";
        //    dt = Utils.GetDataTable(query, CnnString1);

        //    GridView1.DataSource = dt;
        //    GridView1.DataBind();
        //}





    }
}