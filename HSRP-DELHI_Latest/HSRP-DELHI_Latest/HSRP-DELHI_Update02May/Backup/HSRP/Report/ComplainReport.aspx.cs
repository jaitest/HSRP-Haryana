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


namespace HSRP.Report
{
    public partial class ComplainReport : System.Web.UI.Page
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
        string  strComplaintID =string.Empty;
        string strSql = string.Empty;

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
         string fromdate = string.Empty;
         string ToDate = string.Empty;

         string strcomplaindatetime = string.Empty;
         string strname = string.Empty;
         string strmobileNo = string.Empty;
         string stremailid = string.Empty;
         string strRegNo = string.Empty;
         string strEngineNo = string.Empty;
         string strChessisNo = string.Empty;
         string strRemarks = string.Empty;
         string strStatus = string.Empty;
         string strSolution = string.Empty;
         DataTable dt = new DataTable();


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
                            FilldropDownListOrganization();
                           
                        }
                        else
                        { 
                            hiddenUserType.Value = "1";
                            labelOrganization.Enabled = false;
                            DropDownListStateName.Enabled = false;
                            FilldropDownListOrganization();
                           
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
               
                 
            }
            else
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID=" + HSRPStateID + " and ActiveStatus='Y' Order by HSRPStateName";
                DataTable dts = Utils.GetDataTable(SQLString, CnnString);
                DropDownListStateName.DataSource = dts;
                DropDownListStateName.DataBind();
            }
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
           // buildGrid();
        } 
        
        private void ClearGrid()
        {
           // Grid1.Items.Clear();
            lblErrMsg.Text = String.Empty;
            return;
        }
        
       protected void ButtonGo_Click(object sender, EventArgs e)
        {
            fromdate = OrderDate.SelectedDate.ToString("yyyy-MM-dd");
            ToDate = HSRPAuthDate.SelectedDate.ToString("yyyy-MM-dd");

            if (DropDownListStateName.Text == ("--Select State--"))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('Please Select DropDown..');", true);

            }
            else
            {
               // ClearGrid();
                




                string UID = Session["UID"].ToString();
                // string OrderStatus = dropDownListorderStatus.SelectedItem.Text;



               // SQLString = "select * from Complaint where StateId='" + DropDownListStateName.SelectedValue + "' and Status='Closed' and ComplaintDate between '" + fromdate + "' and  '" + ToDate + "'";
               // DataTable dt = new DataTable();

                SQLString = "SELECT [id],[ComplaintNo],[Region],[StateId],[OwnerName],[MobileNo],[Email],[Regno],[EngineNo],[ChasisNo],left([Remarks],50) as Remarks,[IPAddress],[ComplaintDate],[Status],[Solution],[SolutionDate] FROM [Complaint] where StateId='" + DropDownListStateName.SelectedValue + "' and Status='Closed' and ComplaintDate between '" + fromdate + "' and  '" + ToDate + "'";
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


                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                    
                }
                else
                {
                    
                   // ClearGrid();
                }
            }
           
           
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




        protected void Linkbutton1_Click(object sender, EventArgs e)
        {


            LinkButton btn = sender as LinkButton;
            GridViewRow row = btn.NamingContainer as GridViewRow;
            strComplaintID = GridView1.DataKeys[row.RowIndex].Values[0].ToString();
           

         
            ClientScript.RegisterStartupScript(GetType(), "showModalScript", "AddNewPop('" + strComplaintID + "');", true);    
        }


        protected void Linkbutton2_Click(object sender, EventArgs e)
        {

            LinkButton btn = sender as LinkButton;
            GridViewRow row = btn.NamingContainer as GridViewRow;
            strComplaintID = GridView1.DataKeys[row.RowIndex].Values[0].ToString();



            ClientScript.RegisterStartupScript(GetType(), "showModalScript", "AddNewPopAction('" + strComplaintID + "');", true);
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
           
            
           
        }
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
           
      
        }
    }
}