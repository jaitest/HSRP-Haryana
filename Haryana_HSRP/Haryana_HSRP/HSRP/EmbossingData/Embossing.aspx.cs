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
using System;
using System.Net;

namespace HSRP.EmbossingData
{
    public partial class Embossing : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        string UserType = string.Empty;
        string HSRPStateID = string.Empty;
        string RTOLocationID = string.Empty;
        int intHSRPStateID;


        string AllLocation = string.Empty;
        string OrderStatus = string.Empty;

        DataProvider.BAL bl = new DataProvider.BAL();
        BAL obj = new BAL();
        string StickerManditory = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
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
                            FilldropDownListOrganization();
                            FilldropDownListClient();
                            
                            //buildGrid();
                        }

                        //ShowGrid();
                    }
                    catch (Exception err)
                    {
                        lblErrMsg.Text = "Error on Page Load" + err.Message.ToString();
                    }
                }
            }
        }

        #region DropDown

        public void FileDetail()
        {

        }

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

                dataLabellbl.Visible = false;
                TRRTOHide.Visible = false;
            }
            else
            {
                string UserID = Convert.ToString(Session["UID"]);

                SQLString = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, (a.RTOLocationCode+', ') as RTOCode, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID where UserRTOLocationMapping.UserID='" + UserID + "' order by RTOLocationName";
                DataTable dss = Utils.GetDataTable(SQLString, CnnString);

                labelOrganization.Visible = true;
                DropDownListStateName.Visible = true;
                labelClient.Visible = true;
                dropDownListClient.Visible = true;

                dropDownListClient.DataSource = dss;
                dropDownListClient.DataBind();
                dataLabellbl.Visible = true;

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

        #endregion

        private void ShowGrid()
        {
            LblErrorMessage.Text = "";
            LblMessage.Text = "";
            string SQLString = string.Empty;
            if (dropdownDuplicateFIle.SelectedItem.Text != "--Select RTO Name--")
            {
                if (DropDownListStateName.SelectedValue.ToString()== "3")
                {
                    SQLString = "select hsrprecordid,vehicleregno,hsrp_rear_lasercode,hsrp_Front_lasercode,HSRPRecord_AuthorizationNo,hsrpRecord_CreationDate,  MobileNo from hsrprecords where hsrp_stateid ='" + DropDownListStateName.SelectedValue.ToString() + "' and PdfFileName='" + dropdownDuplicateFIle.SelectedItem.ToString() + "' and orderstatus ='New Order'   and  hsrpRecord_CreationDate > dateadd(day, -4,GETDATE())  order by VehicleClass,VehicleType,VehicleRegNo";
                  
                }
                else 
                {

                    SQLString = "select hsrprecordid,vehicleregno,hsrp_rear_lasercode,hsrp_Front_lasercode,HSRPRecord_AuthorizationNo,hsrpRecord_CreationDate,  MobileNo from hsrprecords where hsrp_stateid ='" + DropDownListStateName.SelectedValue.ToString() + "' and PdfFileName='" + dropdownDuplicateFIle.SelectedItem.ToString() + "' and orderstatus ='New Order'  order by VehicleClass,VehicleType,VehicleRegNo";
                   
                }
                dt = Utils.GetDataTable(SQLString, CnnString);
              
                if (dt.Rows.Count > 0)
                {
                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                    btnSave.Visible = true;
                    btnSave.Enabled = true;
                    
                }
                else
                {
                    btnSave.Visible = false;
                    lblErrMsg.Text = "Record Not Found";
                    GridView1.DataSource = null;
                    GridView1.DataBind();

                }

            }
            else
            {

                lblErrMsg.Text = String.Empty;
                lblErrMsg.Text = "Please Select File.";
                return;
            }
        }
        StringBuilder sb = new StringBuilder();
        StringBuilder sb1 = new StringBuilder();
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



        protected void DropDownListStateName_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilldropDownListClient();
            dropDownListClient.Visible = true;
            labelClient.Visible = true;

        }


        private void InitialSetting()
        {

            string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            string MaxDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            HSRPAuthDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            HSRPAuthDate.MaxDate = DateTime.Parse(MaxDate);
            CalendarHSRPAuthDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarHSRPAuthDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            OrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(-3.00);
            OrderDate.MaxDate = DateTime.Parse(MaxDate);
            CalendarOrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarOrderDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
        }





        protected void btnGO_Click(object sender, EventArgs e)
        {
            labelOrderStatus.Visible = true;
            dropdownDuplicateFIle.Visible = true;

            string type = "1";
            String[] StringAuthDate = OrderDate.SelectedDate.ToString().Split('/');
            string MonTo = ("0" + StringAuthDate[0]);
            string MonthdateTO = MonTo.Replace("00", "0").Replace("01", "1");
            String ReportDateFrom = StringAuthDate[1] + "/" + MonthdateTO + "/" + StringAuthDate[2].Split(' ')[0];
            String From = StringAuthDate[0] + "/" + StringAuthDate[1] + "/" + StringAuthDate[2].Split(' ')[0];
            //AuthorizationDate = new DateTime(Convert.ToInt32(StringAuthDate[2].Split(' ')[0]), Convert.ToInt32(StringAuthDate[0]), Convert.ToInt32(StringAuthDate[1]));
            string AuthorizationDate = From + " 00:00:00";
            String[] StringOrderDate = HSRPAuthDate.SelectedDate.ToString().Split('/');
            string Mon = ("0" + StringOrderDate[0]);
            string Monthdate = Mon.Replace("00", "0").Replace("01", "1");
            String FromDate = StringOrderDate[1] + "-" + Monthdate + "-" + StringOrderDate[2].Split(' ')[0];
            String ReportDateTo = StringOrderDate[1] + "/" + Monthdate + "/" + StringOrderDate[2].Split(' ')[0];
            String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
            string ToDate = From1 + " 23:59:59";

          
                SQLString = "select  Distinct pdffilename   from hsrprecords  where hsrp_StateID='" + DropDownListStateName.SelectedValue + "' and rtolocationID='" + dropDownListClient.SelectedValue + "' and  pdffilename is not null and Hsrprecord_creationdate between '" + AuthorizationDate + "' and  '" + ToDate + "' and orderstatus='New Order' group by  pdffilename, convert(varchar,pdfdownloaddate,111)";
                Utils.PopulateDropDownList(dropdownDuplicateFIle, SQLString.ToString(), CnnString, "--Select File Name--");
                GridView1.DataSource = null;
                GridView1.DataBind();
                btnSave.Visible = false;
                btnSave.Enabled = false;
           

        }
        DataTable dt = new DataTable();
        protected void dropdownDuplicateFIle_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowGrid();
        }
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            ShowGrid();

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {   LblErrorMessage.Text = "";
                LblMessage.Text = "";
                //Code for hp only

             
                if (DropDownListStateName.SelectedValue.ToString() == "3"  || HSRPStateID=="3")
                {
                    int No = 0;
                   
                    for (int i = 0; i < GridView1.Rows.Count; i++)
                    {
                        chk = GridView1.Rows[i].Cells[0].FindControl("CHKSelect") as CheckBox;
                        if (chk.Checked == true)
                        {
                            Label lblVehicleRegNo = GridView1.Rows[i].Cells[1].FindControl("lblVehicleRegNo") as Label;
                            TextBox FrontLaserCode = GridView1.Rows[i].Cells[2].FindControl("txtFLaserCode") as TextBox;
                            TextBox RearLasercode = GridView1.Rows[i].Cells[3].FindControl("txtRLaserCode") as TextBox;
                            Label id = GridView1.Rows[i].Cells[4].FindControl("id") as Label;
                            Label AuthNo = GridView1.Rows[i].Cells[5].FindControl("AuthNo") as Label;
                            Label RecordCreationdate = GridView1.Rows[i].Cells[6].FindControl("RecordCreationdate") as Label;
                            Label MobileNo = GridView1.Rows[i].Cells[7].FindControl("MobileNo") as Label;

                            sb.Append("update hsrprecords set hsrp_Front_lasercode='" + FrontLaserCode.Text + "' ,hsrp_rear_lasercode='" + RearLasercode.Text + "',orderstatus='Embossing Done',[OrderEmbossingDate]=getdate() , embossinguserid='" + strUserID + "' where HSRPRecordID='" + id.Text + "'" );
                       
                            
                           int j = Utils.ExecNonQuery(sb.ToString(), CnnString);

                           if (j > 0)
                           {
                               No = No + 1;
                               if (MobileNo.Text.Trim().Length == 10)
                               {

                                   DateTime affdate = Convert.ToDateTime(RecordCreationdate.Text.Trim());
                                   string MobNo = MobileNo.Text.ToString().Trim();

                                   string smsmessage = "Affixation date for your HSRP plate(s) for vehicle Reg. No. " + lblVehicleRegNo.Text.Trim() + " is " + affdate.ToString("dd-MM-yyyy") + ". at your HSRP affixation center. Team HSRP";
                                   string url = "http://alerts.smseasy.in/api/web2sms.php?workingkey=627691h463b66gov5j83&sender=HPHSRP&to=" + MobNo + "&message=" + smsmessage + "";
                                   HttpWebRequest MyRequest = (HttpWebRequest)WebRequest.Create(url);
                                   MyRequest.Method = "GET";
                                   WebResponse myRespose = MyRequest.GetResponse();
                                   StreamReader sr = new StreamReader(myRespose.GetResponseStream(), System.Text.Encoding.UTF8);
                                   string result = sr.ReadToEnd();
                                   sr.Close();
                                   myRespose.Close();
                                   string SqlQuery = "update [SMSlog_HP] set [FirstSMSText]='" + smsmessage + "',[FirstSMSSentDateTime]=getdate(),[FirstSMSServerResponseID]='" + result.ToString() + "' where [MobileNo]='" + MobNo + "' and [AUTH_NO]='" + AuthNo.Text.Trim() + "'";
                                   
                                   int k = Utils.ExecNonQuery(SqlQuery.ToString(), CnnString);
                               }

                           }          
                    
                

                        }
                    }

                    if (No > 0)
                    {
                        LblMessage.Text = "Updated Records Successfully";
                        btnGO_Click(sender, e);
                        btnSave.Enabled = false;
                    }
                    else
                    {
                        LblErrorMessage.Text = "Updated Records Not successfully";
                        btnSave.Enabled = false;
                    }                

                    

                }


                    //End Code for hp only 

                else 
                {

                    for (int i = 0; i < GridView1.Rows.Count; i++)
                    {
                        chk = GridView1.Rows[i].Cells[0].FindControl("CHKSelect") as CheckBox;
                        if (chk.Checked == true)
                        {
                            Label lblVehicleRegNo = GridView1.Rows[i].Cells[1].FindControl("lblVehicleRegNo") as Label;
                            TextBox FrontLaserCode = GridView1.Rows[i].Cells[2].FindControl("txtFLaserCode") as TextBox;
                            TextBox RearLasercode = GridView1.Rows[i].Cells[3].FindControl("txtRLaserCode") as TextBox;
                            Label id = GridView1.Rows[i].Cells[4].FindControl("id") as Label;

                            sb.Append("update hsrprecords set hsrp_Front_lasercode='" + FrontLaserCode.Text + "' ,hsrp_rear_lasercode='" + RearLasercode.Text + "',orderstatus='Embossing Done',[OrderEmbossingDate]=getdate() , embossinguserid='" + strUserID + "' where HSRPRecordID='" + id.Text + "'" + Environment.NewLine);
                        }
                    }
                    int j = Utils.ExecNonQuery(sb.ToString(), CnnString);


                    if (j > 0)
                    {

                        LblMessage.Text = "Updated Records Successfully";
                        btnGO_Click(sender, e);
                        btnSave.Enabled = false;
                    }
                    else
                    {
                        LblErrorMessage.Text = "Updated Records Not Successfully";
                        btnSave.Enabled = false;
                    }

               }


               
            }
            catch(Exception ex)
            {
                LblErrorMessage.Text = "Data Not Uploaded Contact Administration with screen shot Error :" + ex.Message.ToString();
            }
        }
    }
}