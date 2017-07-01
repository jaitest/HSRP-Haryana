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

namespace HSRP.Transaction
{
    public partial class InventoryStatus1 : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        string UserType = string.Empty;
        string HSRPStateID = string.Empty;
        string RTOLocationID = string.Empty;
       

        protected void Page_Load(object sender, EventArgs e)
        {
           // Utils.GZipEncodePage();
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {
                RTOLocationID = Session["UserRTOLocationID"].ToString();
                UserType = Session["UserType"].ToString();
                HSRPStateID = Session["UserHSRPStateID"].ToString();
                lblErrMsg.Text = string.Empty;
                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                strUserID = Session["UID"].ToString();
                ComputerIP = Request.UserHostAddress;

                if (!IsPostBack)
                {
                    try
                    {
                       // InitialSetting();
                        if (UserType=="0")
                        {
                            labelHSRPState.Visible = true;
                            dropDownListHSRPState.Visible = true;
                            FilldropDownListHSRPState();
                        }
                        else if (UserType == "0")
                        {
                            //labelHSRPState.Visible = false;
                            //dropDownListHSRPState.Visible = false;
                            //labelRTOLocation.Visible = true;
                            //dropDownListRTOLocation.Visible = true;

                            //FilldropDownListHSRPState();
                           // FilldropDownListRTOLocation();
                            labelHSRPState.Visible = true;
                            dropDownListHSRPState.Visible = true;
                            FilldropDownListHSRPState();
                        }
                        else
                        {
                            //pdfexl.Visible = true;
                            //ButtonGo.Visible = false;
                            //labelHSRPState.Visible = false;
                            //dropDownListHSRPState.Visible = false;
                            //labelRTOLocation.Visible = false;
                            //dropDownListRTOLocation.Visible = false;
                            labelHSRPState.Visible = true;
                            dropDownListHSRPState.Visible = true;
                            FilldropDownListHSRPState();
                           
                        }
                    }
                    catch (Exception err)
                    {
                        lblErrMsg.Text = "Error on Page Load" + err.Message.ToString();
                    }
                }
            }
        }



        #region DropDown

        private void FilldropDownListHSRPState()
        {
            if (UserType == "0")
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState where ActiveStatus='Y' Order by HSRPStateName";
                 Utils.PopulateDropDownList(dropDownListHSRPState, SQLString.ToString(), CnnString, "--Select State--");
                dropDownListHSRPState.SelectedIndex = 0;
            }
            else
            {
                dropDownListHSRPState.Enabled = false;
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID=" + HSRPStateID + " and ActiveStatus='Y' Order by HSRPStateName";
                DataTable dss = Utils.GetDataTable(SQLString, CnnString);
                dropDownListHSRPState.DataSource = dss;
                dropDownListHSRPState.DataBind();

                FilldropDownListRTOLocation();
            }
          
        }

        private void FilldropDownListRTOLocation()
        {
            if (UserType == "0")
            {

                SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + dropDownListHSRPState.SelectedValue.ToString() + " and ActiveStatus!='N' Order by RTOLocationName";
                //SQLString = "select RTOLocation.RTOLocationName,RTOLocation.RTOLocationID from RTOLocation inner join UserRTOLocationMapping on RTOLocation.RTOLocationID=UserRTOLocationMapping.RTOLocationID Where RTOLocation.HSRP_StateID=" + intHSRPStateID + " and UserRTOLocationMapping.UserID='" + strUserID + "' and ActiveStatus!='N' Order by RTOLocation.RTOLocationName";
               
            }
            else
            {
                dropDownListRTOLocation.Visible = true;
                labelRTOLocation.Visible = true;
               // UpdateRTOLocation.Update();
                
               // SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + intHSRPStateID + " and ActiveStatus!='N' Order by RTOLocationName";
                SQLString = "select RTOLocation.RTOLocationName,RTOLocation.RTOLocationID from RTOLocation inner join UserRTOLocationMapping on RTOLocation.RTOLocationID=UserRTOLocationMapping.RTOLocationID Where RTOLocation.HSRP_StateID=" + dropDownListHSRPState.SelectedValue.ToString() + " and UserRTOLocationMapping.UserID='" + strUserID + "' and ActiveStatus!='N' Order by RTOLocation.RTOLocationName";
               
            }
            Utils.PopulateDropDownList(dropDownListRTOLocation, SQLString.ToString(), CnnString, "--Select RTO Location--");
            dropDownListRTOLocation.SelectedIndex = 0;
           // FillPrefix();
        }

        private void FillPrefix()
        {
            ddlprefix.Visible = true;
            lblPrefix.Visible = true;
            SQLString = "select distinct(prefix) from RTOInventory where hsrp_stateid="+HSRPStateID+"";
            Utils.PopulateDropDownList(ddlprefix, SQLString.ToString(), CnnString, "--Select Prefix--");
            ddlprefix.SelectedIndex = 0;
        }

        protected void dropDownListHSRPState_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dropDownListHSRPState.SelectedItem.Text != "--Select State--")
            {
                dropDownListRTOLocation.Visible = true;
                labelRTOLocation.Visible = true;
                FilldropDownListRTOLocation();
              //  UpdateRTOLocation.Update();
            }
            else
            {
                lblErrMsg.Text = string.Empty;
                lblErrMsg.Text = "Please Select State";
                dropDownListHSRPState.Visible = false;
                labelRTOLocation.Visible = false;
              //  UpdateRTOLocation.Update();
              
            }
        }

        
        #endregion

   


 
        #region openfile
        protected void ReadFile(string path)
        {
            // Get the physical Path of the file
            string filepath = path;

            // Create New instance of FileInfo class to get the properties of the file being downloaded
            FileInfo file = new FileInfo(filepath);

            // Checking if file exists
            if (file.Exists)
            {
                // Clear the content of the response
                Response.ClearContent();

                // LINE1: Add the file name and attachment, which will force the open/cance/save dialog to show, to the header
                //Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                Response.AddHeader("Content-Disposition", "inline; filename=" + file.Name);


                // Add the file size into the response header
                Response.AddHeader("Content-Length", file.Length.ToString());

                // Set the ContentType
                Response.ContentType = ReturnExtension(file.Extension.ToLower());

                // Write the file into the response (TransmitFile is for ASP.NET 2.0. In ASP.NET 1.1 you have to use WriteFile instead)
                Response.TransmitFile(file.FullName);

                // End the response
                Response.End();
            }

        }

        private string ReturnExtension(string fileExtension)
        {
            switch (fileExtension)
            {
                case ".htm":
                case ".html":
                case ".log":
                    return "text/HTML";
                case ".txt":
                    return "text/plain";
                case ".docx":
                    return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                case ".doc":
                    return "application/msword";
                case ".tiff":
                case ".tif":
                    return "image/tiff";
                case ".asf":
                    return "video/x-ms-asf";
                case ".avi":
                    return "video/avi";
                case ".zip":
                    return "application/zip";
                case ".xls":
                case ".csv":
                    return "application/vnd.ms-excel";
                case ".gif":
                    return "image/gif";
                case ".jpg":
                case "jpeg":
                    return "image/jpeg";
                case ".bmp":
                    return "image/bmp";
                case ".wav":
                    return "audio/wav";
                case ".mp3":
                    return "audio/mpeg3";
                case ".mpg":
                case "mpeg":
                    return "video/mpeg";
                case ".rtf":
                    return "application/rtf";
                case ".asp":
                    return "text/asp";
                case ".pdf":
                    return "application/pdf";
                case ".fdf":
                    return "application/vnd.fdf";
                case ".ppt":
                    return "application/mspowerpoint";
                case ".dwg":
                    return "image/vnd.dwg";
                case ".msg":
                    return "application/msoutlook";
                case ".xml":
                case ".sdxl":
                    return "application/xml";
                case ".xdp":
                    return "application/vnd.adobe.xdp+xml";
                default:
                    return "application/octet-stream";
            }

        }
        #endregion
       


     
        string FromDate1;
        string ToDate;


      
        protected void LinkbuttonSearch_Click(object sender, EventArgs e)
        {

              bindgrid();
              GridView1.PageIndex = 0;

        }


        public void bindgrid()
        {
            HSRPStateID = dropDownListHSRPState.SelectedValue;
            RTOLocationID = dropDownListRTOLocation.SelectedValue;
            

            string prefix = ddlprefix.SelectedItem.ToString();
            SQLString = "select top "+ddlrecord.SelectedItem.ToString()+"* ,laserno,inventorystatus,dense_rank() over  (order by laserno) as 'AutoId' from RTOInventory where HSRP_StateID=" + HSRPStateID + " and RTOLocationID=" + RTOLocationID + " and inventorystatus='New Order' and prefix='" + prefix + "' and laserNowithoutprefix like '%" + txtlaserNo.Text + "%'";
            //inventorystatus='New Order' and hsrp_stateid=
            DataTable dt = Utils.GetDataTable(SQLString, CnnString);
            if (dt.Rows.Count > 0)
            {
                ddltype.Visible = true;
                btnSave.Visible = true;
                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
            else
            {
                string closescript1 = "<script>alert('No records found.')</script>";
                Page.RegisterStartupScript("abc", closescript1);
                return;
            }

        }



        DateTime OrderDate1, OrderDate2;

       

        protected void LinkButton2_Click(object sender, EventArgs e)
        {
            //Response.Redirect("Hsrprecords.aspx");
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
          LblMessage.Text = "";
          
          lblDuplicateLaserNo.Text = "";
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

            

            ddltype.Visible = true;
            btnSave.Visible = true;

            
            string status = string.Empty;
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                
                chk = GridView1.Rows[i].Cells[0].FindControl("CHKSelect") as CheckBox;
                if (chk.Checked == true)
                {
                    Label laserno = GridView1.Rows[i].Cells[2].FindControl("LaserNo") as Label;
                    Label InventoryStatus = GridView1.Rows[i].Cells[3].FindControl("InventoryStatus") as Label;
                    TextBox RegNo = GridView1.Rows[i].Cells[4].FindControl("TextBox1") as TextBox;

                    string sqlText = "Select * from rtoinventory where laserno='" + laserno.Text + "'";
                    DataTable dt = new DataTable();
                    dt = Utils.GetDataTable(sqlText, CnnString);

                    if (dt.Rows.Count > 0)
                    {
                        string InventoryID = dt.Rows[0]["InventoryID"].ToString();
                        string ProductID = dt.Rows[0]["ProductID"].ToString();
                        string Prefix = dt.Rows[0]["Prefix"].ToString();
                        string LaserNoWithoutPrefix = dt.Rows[0]["LaserNoWithoutPrefix"].ToString();

                        string HSRPRecordID = dt.Rows[0]["HSRPRecordID"].ToString();
                        string CurrentCost = dt.Rows[0]["CurrentCost"].ToString();
                        string Weight = dt.Rows[0]["Weight"].ToString();

                        string StatusDate = dt.Rows[0]["StatusDate"].ToString();
                        string oldorderid = dt.Rows[0]["oldorderid"].ToString();
                        string OrderCloseDate = dt.Rows[0]["OrderCloseDate"].ToString();

                        if (ddltype.SelectedItem.Text == "Embossed" || ddltype.SelectedItem.Text == "Rejected")
                        {
                            if (RegNo.Text.Trim()=="")
                            {
                                string closescript1 = "<script>alert('Please Enter Registration No.')</script>";
                                Page.RegisterStartupScript("abc", closescript1);
                                return;
                            }
                        }

                        sqlText = "Select count(*) from RTOInventoryStockInHand where LaserNo='" + laserno.Text + "'";
                        int i1 = Utils.getScalarCount(sqlText, CnnString);
                        if (i1 > 0)
                        {
                            lblLaserNoUsed.Visible = true;
                            laserno.Visible = true;
                            lblDuplicateLaserNo.Visible = true;
                            DataTable dt1 = new DataTable();
                            string SqlText = "Select LaserNo from RTOInventoryStockInHand where LaserNo='" + laserno.Text + "'";
                            dt1 = Utils.GetDataTable(SqlText, CnnString);
                            if (dt.Rows.Count > 0)
                            {
                                string LaserNo = dt.Rows[0]["LaserNo"].ToString();
                                Label2.Text = (int.Parse(Label2.Text) + 1).ToString();
                                if (int.Parse(Label2.Text) % 10 == 0)
                                {
                                    lblDuplicateLaserNo.Text += LaserNo.ToString() + "\n";
                                    //ShowDuplicateRecords.Text += VehicleRegno.ToString() + ",";
                                }
                                else
                                {
                                    lblDuplicateLaserNo.Text += LaserNo.ToString() + ",";
                                }
                            }
                            //duplicate records
                        }
                        else
                        {



                            if ((ddltype.SelectedItem.Text == "Embossed" || ddltype.SelectedItem.Text == "Rejected" && RegNo.Text != null) || (ddltype.SelectedItem.Text == "Blank" && RegNo.Text == ""))
                            {
                                sqlText = @"insert into  RTOInventoryStockInHand (InventoryID,ProductID,Prefix,LaserNoWithoutPrefix,LaserNo,HSRPRecordID,HSRP_StateID,RTOLocationID,CurrentCost,Weight,InventoryStatus,StatusDate,oldorderid,OrderCloseDate,RegNo,Status)
                                    values('" + InventoryID + "','" + ProductID + "','" + Prefix + "','" + LaserNoWithoutPrefix + "','" + laserno.Text + "','" + HSRPRecordID + "','" + dropDownListHSRPState.SelectedValue + "','" + dropDownListRTOLocation.SelectedValue + "','" + CurrentCost + "','" + Weight + "','" + InventoryStatus.Text + "','" + DateTime.Now.ToString() + "','" + oldorderid + "','" + OrderCloseDate + "','" + RegNo.Text + "','" + ddltype.SelectedItem.ToString() + "')";

                                LblMessage.Text = "Saved Successfully.";
                            }

                            if (!(Utils.ExecNonQuery(sqlText, CnnString) > 0))
                            {
                                return;
                            }
                        }
                    }

                }
            }

            }
        

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void dropDownListRTOLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillPrefix();
        }

        protected void ddlrecord_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            GridView1.PageSize = int.Parse(ddlrecord.SelectedItem.ToString());
            //bindgrid();
          
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;

            bindgrid();
        }

        

    }
}