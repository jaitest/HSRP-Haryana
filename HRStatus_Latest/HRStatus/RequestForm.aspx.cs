using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.IO;
using System.Net;

public partial class PlateRates : System.Web.UI.Page
{
    #region Variable Declaration and assignment
    public static String ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
    String SQLString = String.Empty;
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        lblSuccess.Text = "";
        if (!IsPostBack)
        {
            FillRTOLocation();
            //FillVehicleMaker();
        }
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        Session["REG"] = null;
        Response.Redirect("OrderStatus.aspx");
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
        
            string num = "HR-" + System.DateTime.Now.ToString("yyyymmddhhmmss");
            string strSelect = "SELECT * FROM HSRPRECORDS WHERE hsrp_StateID=4 and  vehicleregno='" + txtVehicleRegNo.Text.Replace(" ", "") + "'";
            DataTable dtData = utils.GetDataTable(strSelect, ConnectionString);
            //if (dtData.Rows.Count > 0)
            //{
            //    lblSuccess.Text = "Your HSRP Order Status is Booked Please Contact HSRP Center .";
            //    return;

            //}



            if (dtData.Rows.Count <= 0)
            {
                string strSelectStagging = "SELECT * FROM HSRPRecordsStaggingArea WHERE  VehicleRegNo='" + txtVehicleRegNo.Text.Replace(" ", "") + "' ";
                DataTable dtStaggingData = utils.GetDataTable(strSelectStagging, ConnectionString);
                if (dtStaggingData.Rows.Count > 0)
                {
                    // lblSuccess.Text = "Your request for HSRP has been already received .";

                    //lblSuccess.Text = "Your record has been received already.Pay Order or Visit RTO HSRP Center to Pay HSRP Fee.For Affixation.".ToUpper();
                    lblSuccess.Text = "Your record has been received already.Pay <a href='http://hsrphr.com/payment.htm'>Online</a> or Visit RTO HSRP Center to Pay HSRP Fee.For Affixation.";
                    return;
                }
            }
            else
            {
                // lblSuccess.Text = "Your order has been booked already".ToUpper();
                // lblSuccess.Text = "Your record has been received already.Pay <a href='http://hsrphr.com/payment.htm'>Online</a> or Visit RTO HSRP Center to Pay HSRP Fee.For Affixation.";
                lblSuccess.Text = "Your request for HSRP has been already received .";
                return;
            }
            string savepath = string.Empty;
            string filepath = string.Empty;
            string filename = System.IO.Path.GetFileName(FileUpload1.FileName);
            if (filename == "")
            {
                savepath = @"D:\RC\";
                savepath += filename;
                filepath = savepath;
                // FileUpload1.SaveAs(filepath);
            }
            else
            {
                savepath = @"D:\RC\";
                savepath += FileUpload1.FileName;
                filepath = savepath;
                FileUpload1.SaveAs(filepath);
            }
            if (File.Exists(filename))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('File Already Exits');", true);
            }
            else
            {

                DataTable dtResult = new DataTable();
                SQLString = "select * from [HR_OldDataRequest] where vehicleregno='" + txtVehicleRegNo.Text.Replace(" ", "") + "' and RequestStatus='N'";
                dtResult = utils.GetDataTable(SQLString, ConnectionString);
                if (dtResult.Rows.Count == 0)
                {
                    SQLString = "select * from [HSRPRecordsStaggingArea] where Hsrp_stateId=4 and vehicleregno='" + txtVehicleRegNo.Text.Replace(" ", "") + "'";
                    dtResult = utils.GetDataTable(SQLString, ConnectionString);
                    if (dtResult.Rows.Count > 0)
                    {
                        SQLString = "convert(numeric,select [dbo].[hsrpplateamt] ('4','" + dtResult.Rows[0]["vehicletype"].ToString() + "','" + dtResult.Rows[0]["vehicleclass"].ToString() + "','" + dtResult.Rows[0]["ordertype"].ToString() + "')) as amount";
                        String strAmount = utils.getScalarValue(SQLString, ConnectionString);
                        lblSuccess.Text = "AUTHORIZATION FOR FIXATION OF HSRP ON YOUR VEHICLE HAS BEEN RECEIVED THROUGH AUTHORIZATION NO " + dtResult.Rows[0]["HSRPRecord_AuthorizationNo"].ToString() +
                                  " DATED " + Convert.ToDateTime(dtResult.Rows[0]["orderdate"].ToString()).ToString("dd/MM/yyyy") + " KINDLY," +
                                  " DEPOSIT Rs. " + strAmount + " AT THE CENTER GIVEN BELOW.";
                    }
                    else
                    {
                        SQLString = "insert into [dbo].[HR_OldDataRequest](VehicleRegNo,RtoLocationid,Phone_No,chasisNo,EngineNo,VehicleType,VehicleClass,DealerName)" +
                                    "values('" + txtVehicleRegNo.Text.Replace(" ", "") + "','" + ddlRto.SelectedValue + "','" + txtPhone.Text + "','" + txtChasis.Text + "', " +
                                    "'" + txtEngine.Text + "','" + ddlVehicleType.SelectedValue + "','" + ddlVehicleClass.Text + "','" + filepath + "')";
                        int IResult = utils.ExecNonQuery(SQLString, ConnectionString);
                        if (IResult > 0)
                        {
                            //lblSuccess.Text = "Your Request Has Been Received Please Visit Your Registration Authority";
                            string MobileNo = txtPhone.Text;
                            lblSuccess.Text = "Your Request for HSRP has been received for Authorization Request Please Check Status Page Shortly For Authorization Availabilty, Your HSRP request no is :" + num;
                            if (MobileNo.Length > 0)
                            {

                                string SMSText = "Your HSRP request no is :" + num + ".Please visit www.hsrphr.com to check authorization request status of your vehicle. Transport Department Haryana.";

                                string sendURL = "http://103.16.101.52:8080/bulksms/bulksms?username=sse-hsrphr&password=hsrphr&type=0&dlr=1&destination=" + MobileNo + "&source=HRHSRP&message=" + SMSText;

                                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(sendURL);
                                myRequest.Method = "GET";
                                WebResponse myResponse = myRequest.GetResponse();
                                StreamReader sr = new StreamReader(myResponse.GetResponseStream(), System.Text.Encoding.UTF8);
                                string result = sr.ReadToEnd();
                                sr.Close();
                                myResponse.Close();
                                System.Threading.Thread.Sleep(350);

                                utils.ExecNonQuery("insert into HRSMSDetail(RtoLocationID,VehicleRegNo,MobileNo,SentResponseCode,SmsText) values('" + ddlRto.SelectedValue.ToString() + "','" + txtVehicleRegNo.Text + "'," + MobileNo + ",'" + result + "','" + SMSText + "')", ConnectionString);
                            }


                        }
                    }
                }
                else
                {
                    lblSuccess.Text = "Your Request Is Already In Process";
                }
            }
        }
        
            catch(Exception ex)
        {
            
            }

            }
    
        
   
    

    #region Drop Down
    public void FillRTOLocation()
    {
        SQLString = "select RtoLocationName,Rtolocationid from RtoLocation where Hsrp_stateId=4 order by RtoLocationName";
        DataTable dtResult = utils.GetDataTable(SQLString, ConnectionString);
        ddlRto.DataSource = dtResult;
        ddlRto.DataBind();
        ddlRto.Items.Insert(0, new ListItem("--Select Registration Authority--", "-1"));
    }

    //public void FillVehicleMaker()
    //{
    //    SQLString = "SELECT [VehicleMakerID],[VehicleMakerDescription] FROM [hsrpdemo].[dbo].[VehicleMakerMaster] Where [ActiveStatus]='Y' order by VehicleMakerDescription";
    //    DataTable dtResult = utils.GetDataTable(SQLString, ConnectionString);
    //    ddlMaker.DataSource = dtResult;
    //    ddlMaker.DataBind();
    //    ddlMaker.Items.Insert(0, new ListItem("--Select Vehicle Maker--", "-1"));
    //}

    //public void FillVehicleModel(string StrVehicleMakerId)
    //{
    //    SQLString = "SELECT [VehicleModelID],[VehicleModelDescription]  FROM [hsrpdemo].[dbo].[VehicleModelMaster] Where [ActiveStatus]='Y' and [VehicleMakerID]='" + StrVehicleMakerId + "' order by VehicleModelDescription ";
    //    DataTable dtResult = utils.GetDataTable(SQLString, ConnectionString);
    //    ddlModel.DataSource = dtResult;
    //    ddlModel.DataBind();
    //    ddlModel.Items.Insert(0, new ListItem("--Select Vehicle Model--", "-1"));
    //}

    protected void ddlMaker_SelectedIndexChanged(object sender, EventArgs e)
    {
        //FillVehicleModel(ddlMaker.SelectedValue.ToString());
    }
    #endregion


    
}