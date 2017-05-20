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


namespace HSRP.Master
{
    public partial class EditSubmitRequest : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        string UserType = string.Empty;
        string HSRPStateID = string.Empty;
        string RTOLocationID = string.Empty;
        int intHSRPStateID;
        string trnasportname,pp;
        string transtr, statename1;
        BaseFont basefont;
        string fontpath;

        string AllLocation = string.Empty;
         string OrderStatus = string.Empty;
         DateTime AuthorizationDate;
         DateTime OrderDate1;
         DataProvider.BAL bl = new DataProvider.BAL();
         string StickerManditory = string.Empty;

         string SubmitId = string.Empty;
         string QrySubmitID = string.Empty;

         string State_ID = string.Empty;
         string RTO_ID = string.Empty;
         string HSRPStateIDEdit = string.Empty;
         string RTOLocationIDEdit = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            Utils.GZipEncodePage();
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {
                if (Session["UserHSRPStateID"].ToString() == "6")
                {
                   // ButImpData.Visible = true;
                }
                else
                {
                   // ButImpData.Visible = false;
                }

                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
               
                RTOLocationID = Session["UserRTOLocationID"].ToString();
                UserType = Session["UserType"].ToString();
                HSRPStateID = Session["UserHSRPStateID"].ToString();

                lblErrMsg.Text = string.Empty;
                strUserID = Session["UID"].ToString();
                ComputerIP = Request.UserHostAddress;
                 
                if (!IsPostBack)
                {
                      InitialSetting(); 
                    try
                    { 
                        if (UserType == "0")
                        {
                            labelOrganization.Visible = true;
                            DropDownListStateName.Visible = true;
                            DropDownListStateName.Enabled = true;
                            labelClient.Visible = true;
                            dropDownListClient.Visible = true;  
                            FilldropDownListOrganization();
                            FilldropDownListClient(); 
                        }
                        else
                        { 
                            hiddenUserType.Value = "1";
                            labelOrganization.Enabled = false;
                            DropDownListStateName.Enabled = false;
                            labelClient.Enabled = false;
                            FilldropDownListClient();
                            FilldropDownListOrganization();
                            //buildGrid();
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

        private void FilldropDownListOrganization()
        {
            if (UserType == "0")
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where ActiveStatus='Y' Order by HSRPStateName"; 
                Utils.PopulateDropDownList(DropDownListStateName, SQLString.ToString(), CnnString, "--Select State--");
               // DropDownListStateName.SelectedIndex = DropDownListStateName.Items.Count - 1;
                 
            }
            else
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID=" + HSRPStateID + " and ActiveStatus='Y' Order by HSRPStateName";
                DataTable dts = Utils.GetDataTable(SQLString, CnnString);
                DropDownListStateName.DataSource = dts;
                DropDownListStateName.DataBind();
            }
        } 
        private void FilldropDownListClient()
        {
            if (UserType == "0")
            {

                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);

                //SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + HSRPStateID + " and ActiveStatus!='N'   Order by RTOLocationName";
                SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + intHSRPStateID + " and ActiveStatus!='N'   Order by RTOLocationName";
                Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), CnnString, "--Select RTO Name--");
                  
               //  dataLabellbl.Visible = false;
                // TRRTOHide.Visible = false; 
            }
            else
            {
                string UserID = Convert.ToString( Session["UID"]);
                 SQLString = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, (a.RTOLocationCode+', ') as RTOCode, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID where UserRTOLocationMapping.UserID='" + UserID + "' ";
                 DataTable dss = Utils.GetDataTable(SQLString, CnnString); 
                 labelOrganization.Visible = true;
                 DropDownListStateName.Visible = true;
                 labelClient.Visible = true;
                 dropDownListClient.Visible = true; 

                 dropDownListClient.DataSource = dss;
                 dropDownListClient.DataBind();
               //  dataLabellbl.Visible = true;

                string RTOCode = string.Empty;
                if (dss.Rows.Count > 0)
                {
                    for (int i = 0; i <= dss.Rows.Count - 1; i++)
                    {
                        RTOCode += dss.Rows[i]["RTOCode"].ToString();
                        
                    }
                   // lblRTOCode.Text = RTOCode.Remove(RTOCode.LastIndexOf(","));
                    
                }
            }
        } 
        protected void dropDownListClient_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (DropDownListStateName.SelectedItem.Text != "--Select Client--")
            {
                ShowGrid();
            }
            else
            {
                Grid1.Items.Clear();
                lblErrMsg.Text = String.Empty;
                lblErrMsg.Text = "Please Select Client.";
                return;
            } 
        }
        #endregion

        private void ShowGrid()
        {
          
            if (String.IsNullOrEmpty(dropDownListClient.SelectedValue) || dropDownListClient.SelectedValue.Equals("--Select Client--"))
            {
                Grid1.Items.Clear();
                lblErrMsg.Text = String.Empty;
                lblErrMsg.Text = "Please Select Client.";
                return;
            }
            buildGrid();
        }


        #region Grid
        public void buildGrid()
        {
            try
            {
                if (UserType == "0")
                {
                    
                     SQLString = "SELECT CONVERT (varchar(20),HSRPRecord_AuthorizationDate+4,103) as Duedate,  CONVERT (varchar(20),HSRPRecord_AuthorizationDate,103) as HSRPRecord_AuthorizationDate, HSRPRecords.HSRPRecord_AuthorizationNo, HSRPRecords.OwnerName, HSRPRecords.VehicleRegNo, HSRPRecords.OrderStatus, HSRPRecords.HSRP_Front_LaserCode, HSRPRecords.HSRP_Rear_LaserCode, HSRPRecords.HSRPRecordID, RTOInventory.LaserPlateBoxNo FROM HSRPRecords INNER JOIN RTOInventory ON HSRPRecords.HSRPRecordID = RTOInventory.HSRPRecordID where HSRPRecords.RTOLocationID=" + RTOLocationID;

                }
                else
                {
                    
                     SQLString = "SELECT CONVERT (varchar(20),HSRPRecord_AuthorizationDate+4,103) as Duedate,  CONVERT (varchar(20),HSRPRecord_AuthorizationDate,103) as HSRPRecord_AuthorizationDate, HSRPRecords.HSRPRecord_AuthorizationNo, HSRPRecords.OwnerName, HSRPRecords.VehicleRegNo, HSRPRecords.OrderStatus, HSRPRecords.HSRP_Front_LaserCode, HSRPRecords.HSRP_Rear_LaserCode, HSRPRecords.HSRPRecordID, RTOInventory.LaserPlateBoxNo FROM HSRPRecords INNER JOIN RTOInventory ON HSRPRecords.HSRPRecordID = RTOInventory.HSRPRecordID where HSRPRecords.RTOLocationID=" + RTOLocationID+" and OrderStatus='New Order'";
                } 
                DataTable dt = Utils.GetDataTable(SQLString, CnnString); 
                Grid1.DataSource = dt;
                Grid1.RunningMode = (ComponentArt.Web.UI.GridRunningMode)Enum.Parse(typeof(ComponentArt.Web.UI.GridRunningMode), "Client");
                Grid1.SearchOnKeyPress = true;
                Grid1.DataBind();
                Grid1.RecordCount.ToString();
                
            }
            catch (Exception ex)
            {
                lblErrMsg.Text = "Error in Populating Grid :" + ex.Message.ToString();
            }
        }
        public void OnNeedRebind(object sender, EventArgs oArgs)
        {
            System.Threading.Thread.Sleep(200);
            Grid1.DataBind();
        }
        public void OnNeedDataSource(object sender, EventArgs oArgs)
        {
            buildGrid();
        }
        public void OnPageChanged(object sender, ComponentArt.Web.UI.GridPageIndexChangedEventArgs oArgs)
        {
            Grid1.CurrentPageIndex = oArgs.NewIndex;
        }
        public void OnFilter(object sender, ComponentArt.Web.UI.GridFilterCommandEventArgs oArgs)
        {
            Grid1.Filter = oArgs.FilterExpression;
        }
        public void OnSort(object sender, ComponentArt.Web.UI.GridSortCommandEventArgs oArgs)
        {
            Grid1.Sort = oArgs.SortExpression;
        }
        private void ddGridRunningMode_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            Grid1.RunningMode = (ComponentArt.Web.UI.GridRunningMode)Enum.Parse(typeof(ComponentArt.Web.UI.GridRunningMode), "Client");
            buildGrid();
            Grid1.DataBind();
            adjustToRunningMode();
        }
        public void OnGroup(object sender, ComponentArt.Web.UI.GridGroupCommandEventArgs oArgs)
        {
            Grid1.GroupBy = oArgs.GroupExpression;
        }
        private void adjustToRunningMode()
        {

            Grid1.SliderPopupClientTemplateId = "SliderTemplate";
            Grid1.SliderPopupOffsetX = 20;

        }
        #endregion

        protected void Grid1_ItemCommand(object sender, ComponentArt.Web.UI.GridItemCommandEventArgs e)
        {
            String UserID = e.Item["UserID"].ToString();
            string ActiveStatus = e.Item["ActiveStatus"].ToString();
            if (ActiveStatus == "Active")
            {
                ActiveStatus = "N";
            }
            else {
                ActiveStatus = "Y";
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("Update Users Set ActiveStatus='" + ActiveStatus + "' Where UserID=" + UserID);
            Utils.ExecNonQuery(sb.ToString(), CnnString);
            buildGrid();
        } 
        protected void DropDownListStateName_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilldropDownListClient();
            dropDownListClient.Visible = true;
            labelClient.Visible = true; 

        }
        private void ClearGrid()
        {
            Grid1.Items.Clear();
            lblErrMsg.Text = String.Empty;
            return;
        }
        
       protected void ButtonGo_Click(object sender, EventArgs e)
        {

            if (dropDownListorderStatus.Text == ("-- Select Status --") || DropDownListStateName.Text == ("--Select State--") || dropDownListClient.Text==("--Select RTO Name--"))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('Please Select DropDown..');", true);
            
            }
            else
            {
                ClearGrid();
                Grid1.Items.Clear();




                string UID = Session["UID"].ToString();
                string OrderStatus = dropDownListorderStatus.SelectedItem.Text;



                SQLString = "select * from SubmitRequest where REQUESTSTATUS='" + OrderStatus + "'and UserHSRPStateID='" + DropDownListStateName.SelectedValue + "' and UserRTOLocationID='" + dropDownListClient.SelectedValue + "'";
                DataTable dt = new DataTable();


                dt = Utils.GetDataTable(SQLString, CnnString);

                dt.Columns.Add("S.No", typeof(Int32));
                dt.Columns["S.No"].AutoIncrement = true;


                int counter = 1;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["S.No"] = (counter++).ToString();
                }


                if (dt.Rows.Count > 0)
                {


                    Grid1.DataSource = dt;
                    Grid1.RunningMode = (ComponentArt.Web.UI.GridRunningMode)Enum.Parse(typeof(ComponentArt.Web.UI.GridRunningMode), "Client");
                    Grid1.SearchOnKeyPress = true;
                    Grid1.DataBind();
                    Grid1.RecordCount.ToString();
                }
                else
                {
                    Grid1.Items.Clear();
                    ClearGrid();
                }
            }
           
           
        }

       public void SAVEStickerLog( string vehicleRegNo, string HSRPRecordID)
       {
           SQLString = "insert into stickerLog (vehicleRegNo, hsrprecordID, userID) values ('" + vehicleRegNo + "','" + HSRPRecordID + "','" + Session["UID"] + "')";
           Utils.ExecNonQuery(SQLString, CnnString);
           
       }


       #region Export Grid to CSV
       public void CreateCSVFile(DataTable dt, string strFilePath)
       { 
           StreamWriter sw = new StreamWriter(strFilePath, false); 

           int iColCount = dt.Columns.Count; 

           foreach (DataRow dr in dt.Rows)
           {

               for (int i = 0; i < iColCount; i++)
               {
                   if (!Convert.IsDBNull(dr[i]))
                   {
                       sw.Write(dr[i].ToString());
                   }

                   if (i < iColCount - 1)
                   {
                       sw.Write(";");
                   }
               }

               sw.Write(sw.NewLine);

           }

           sw.Close();
            

           System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
           response.ClearContent();
           response.Clear();
           response.ContentType = "text/plain";
           response.AddHeader("Content-Disposition", "attachment; filename=" + strFilePath + ";");
           response.TransmitFile(strFilePath);
           response.Flush();
           response.End(); 
       }
       #endregion


        private void InitialSetting()
        {

            string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            string MaxDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString(); 
           
        }

        protected void dropDownListClient_SelectedIndexChanged1(object sender, EventArgs e)
        {
           
        }

        protected void Grid1_ItemCommand1(object sender, ComponentArt.Web.UI.GridItemCommandEventArgs e)
        {

          
            if (e.Control.ID == "LinkButtonStickerTVS")
            {

                SubmitId = "select SubmitID from SubmitRequest where REQUESTSTATUS='" + dropDownListorderStatus.SelectedItem.Text + "'and UserHSRPStateID='" + DropDownListStateName.SelectedValue + "' and UserRTOLocationID='" + dropDownListClient.SelectedValue + "'";
                DataTable dtsId = new DataTable();
                dtsId = Utils.GetDataTable(SubmitId, CnnString);
                QrySubmitID = dtsId.Rows[0]["SubmitID"].ToString();
                lblSubmit.Text = QrySubmitID;


                DataTable dtState = new DataTable();
                string StateName = "Select HSRPStateName from HSRPState where HSRP_StateID='" + DropDownListStateName.SelectedValue + "' ";
                dtState = Utils.GetDataTable(StateName, CnnString);
                HSRPStateIDEdit = dtState.Rows[0]["HSRPStateName"].ToString();
                Label1.Text = HSRPStateIDEdit;

                DataTable dtRto = new DataTable();
                string RtoName = "Select RTOLocationName from RTOLocation where RTOLocationID='" + dropDownListClient.SelectedValue + "' ";
                dtRto = Utils.GetDataTable(RtoName, CnnString);
                RTOLocationIDEdit = dtRto.Rows[0]["RTOLocationName"].ToString();
                Label2.Text = RTOLocationIDEdit;
               // Response.Redirect("UpdateSubmitRequest.aspx?SubmitID=" + lblSubmit.Text);
                //ClientScript.RegisterStartupScript(GetType(), "showModalScript", "AddNewPop('" + DropDownListStateName.SelectedValue + "','" + dropDownListClient.SelectedItem.ToString() + "','" + QrySubmitID + "');", true);
                ClientScript.RegisterStartupScript(GetType(), "showModalScript", "AddNewPop('" + QrySubmitID + "','" + HSRPStateIDEdit + "','" + RTOLocationIDEdit + "');", true);
            }
            if (e.Control.ID == "ViewRequest")
            {
                
                Response.Redirect("ViewUpdateSubmitRequest.aspx");
            
            }
          

        }



        protected void Linkbutton1_Click(object sender, EventArgs e)
        {
            ClearGrid();
             
           // SQLString = "SELECT CONVERT(varchar(20), HSRPRecord_CreationDate + 4, 103) AS Duedate,HSRPRecords.MobileNo,convert(varchar, HSRPRecord_AuthorizationDate, 105) as HSRPRecord_AuthorizationDate, convert(varchar, OrderClosedDate, 105) as InvoiceDateTime, convert(varchar, OrderEmbossingDate, 105) as OrderEmbossingDate, CONVERT(varchar(20), HSRPRecord_CreationDate,103) AS HSRPRecord_AuthorizationDate, HSRPRecordID,HSRPRecord_AuthorizationNo,OwnerName,VehicleRegNo, CONVERT (varchar(20),HSRPRecord_AuthorizationDate,103) as HSRPRecord_AuthorizationDate,OrderStatus, HSRP_Front_LaserCode,HSRP_Rear_LaserCode,LaserPlateBoxNo  FROM HSRPRecords  Where hsrp_stateID='" + HSRPStateID + "' and VehicleRegNo like '%" + textboxSearch.Text + "%'";

                        //SELECT CONVERT(varchar(20), HSRPRecord_CreationDate + 4, 103) AS Duedate,HSRPRecords.MobileNo,convert(varchar, HSRPRecord_AuthorizationDate, 105) as HSRPRecord_AuthorizationDate, convert(varchar, OrderClosedDate, 105) as InvoiceDateTime, convert(varchar, OrderEmbossingDate, 105) as OrderEmbossingDate, CONVERT(varchar(20), HSRPRecord_CreationDate,103) AS HSRPRecord_AuthorizationDate, HSRPRecordID,HSRPRecord_AuthorizationNo,OwnerName,VehicleRegNo, CONVERT (varchar(20),HSRPRecord_AuthorizationDate,103) as HSRPRecord_AuthorizationDate,OrderStatus, HSRP_Front_LaserCode,HSRP_Rear_LaserCode,LaserPlateBoxNo  FROM HSRPRecords  Where hsrp_stateID='3' and VehicleRegNo like '%9211%'  order by vehicleRegNo
             
            DataTable dt = Utils.GetDataTable(SQLString, CnnString);
            if (dt.Rows.Count > 0)
            {
                Grid1.DataSource = dt;
                Grid1.RunningMode = (ComponentArt.Web.UI.GridRunningMode)Enum.Parse(typeof(ComponentArt.Web.UI.GridRunningMode), "Client");
                Grid1.SearchOnKeyPress = true;
                Grid1.DataBind();
                Grid1.RecordCount.ToString();
            }
            else
            {
                Grid1.Items.Clear();
                ClearGrid();
            }
        }
    }
}