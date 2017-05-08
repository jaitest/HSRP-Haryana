using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.OleDb;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.IO;
using Excel;
using ICSharpCode;
using System.Globalization;


namespace HSRP.Transaction
{
    public partial class UploadHSRPBooking : System.Web.UI.Page
    {
        string SaveLocation = string.Empty;
        string SQLString = string.Empty;
        string CnnString = string.Empty;
        string getExcelSheetName = string.Empty;
        string Id = string.Empty;
        string HSRP_StateID = string.Empty, RTOLocationID = string.Empty;
        bool FlagIsDirty = false;
        int UserType;
        string strUserID = string.Empty;
        int intHSRPStateID;
        string strEmbID = string.Empty;
        string userdealerid = string.Empty;
        string orderno = string.Empty;
        string CnnStringupload = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();

        protected void Page_Load(object sender, EventArgs e)
        {
            //  SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
            Utils.GZipEncodePage();
            lbltotaluploadrecords.Text = "";
            lbltotladuplicaterecords.Text = "";
            lblVehicleRegNo.Text = "";
           

            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {

                UserType = Convert.ToInt32(Session["UserType"]);
                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                strUserID = Session["UID"].ToString();
                userdealerid = Session["userdealerid"].ToString();
                Id = Session["UID"].ToString();
                HSRP_StateID = Session["UserHSRPStateID"].ToString();
                if (!IsPostBack)
                {
                    btnSync.Enabled = false;
                    try
                    {
                        if (HSRP_StateID == "2")
                        {
                            lblEmb.Visible = true;
                            ddlEmbossingCenter.Visible = true;
                        }
                        if (UserType.Equals(0))
                        {
                            // labelOrganization.Visible = true;
                            //  DropDownListStateName.Visible = true;
                           // labelClient.Visible = true;

                              //dropDownListClient.Visible = true;
                            FilldropDownListOrganization();

                           // FilldropDownListClient();
                        }
                        else
                        {

                            // labelOrganization.Visible = true;
                            //  DropDownListStateName.Visible = true;
                            // labelClient.Visible = true;

                            // dropDownListClient.Visible = true;
                            FilldropDownListOrganization();

                           // FilldropDownListClient();
                        }


                    }
                    catch (Exception err)
                    {

                    }
                }
            }
        }



        private void FilldropDownListOrganization()
        {

            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();




        }
        //private void FilldropDownListClient()
        //{
        //    CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        //    if (UserType.Equals(0))
        //    {
        //        int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
        //        SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=2 and ActiveStatus!='N' Order by RTOLocationName";
        //        Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), CnnString, "--Select Location--");
        //    }
        //    else
        //    {


        //        SQLString = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID where  a.LocationType!='District' and UserRTOLocationMapping.UserID='" + strUserID + "' ";
        //        DataSet dss = Utils.getDataSet(SQLString, CnnString);
        //        dropDownListClient.DataSource = dss;
        //        dropDownListClient.DataBind();
        //    }

        //}

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (HSRP_StateID == "2")
            {
                if (ddlEmbossingCenter.SelectedItem.ToString().Equals("--Select Embossing Center--"))
                {
                    llbMSGError0.Visible = true;
                    llbMSGError0.Text = "";
                    llbMSGError0.Text = "Please Select Embossing Center...";
                    return;
                }

                //if (dropDownListClient.SelectedItem.ToString().Equals("--Select Location--"))
                //{
                //    llbMSGError0.Visible = true;
                //    llbMSGError0.Text = "";
                //    llbMSGError0.Text = "Please Select RtoLocation...";
                //    return;
                //}
            }
            llbMSGError.Text = string.Empty;
            try
            {
                DataTable dtExcelRecords = new DataTable();
                if ((FileUpload1.PostedFile != null) && (FileUpload1.PostedFile.ContentLength > 0))
                {
                    InsertDataInstage();
                }
                else
                {
                    llbMSGError.Text = "Please select a file to upload.";
                    llbMSGSuccess.Text = "";
                }
            }
            catch (Exception ex)
            {
                llbMSGError.Text = "Error in Upload File :- " + ex.Message.ToString();
                //AddLog(ex.Message.ToString());
            }
        }
        string fileLocation = string.Empty;
        private void InsertDataInstage()
        {
            try
            {
                string filename = "Dealer-" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                string fileExtension = System.IO.Path.GetExtension(FileUpload1.PostedFile.FileName);
                string fileLocation = System.Configuration.ConfigurationManager.AppSettings["DealerFolder"].ToString();
                fileLocation += filename.Replace("\\\\", "\\");



                if (fileExtension != ".xls" && fileExtension != ".xlsx")
                {
                    llbMSGError.Text = "";
                    llbMSGError.Text = "Please UpLoad Excel File. ";
                    return;
                }
                IExcelDataReader excelReader;
                DataTable dtExcelRecords = new DataTable();
                FileUpload1.PostedFile.SaveAs(fileLocation);


                FileStream stream = File.Open(fileLocation, FileMode.Open, FileAccess.Read);
                excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                if (fileExtension != ".xls")
                {
                    llbMSGError.Text = "The Excel File must be in .xls Format..Kindly Convert Your  File into .xls format";

                    return;
                }


                excelReader.IsFirstRowAsColumnNames = true;
                DataSet result = excelReader.AsDataSet();


                excelReader.Close();
                if (result.Tables[0].Rows.Count > 0 || result != null)
                {
                    ValidationCheckOnRecords(result.Tables[0]);
                    if (FlagIsDirty)
                    {
                        return;
                    }
                    InsertionOfRecords(result.Tables[0]);
                }
                else
                {
                    llbMSGError.Text = "No Data IN Excel File";
                }
                if (File.Exists(fileLocation))
                {

                    File.Delete(fileLocation);

                }


            }
            catch (Exception ee)
            {
                llbMSGError.Text = ee.Message.ToString();
                lbltotladuplicaterecords.Text = countDuplicate.ToString();
                if (File.Exists(fileLocation))
                {

                    File.Delete(fileLocation);

                }
                return;
            }


        }




        int countDuplicate = 0, countupload = 0, errorinexcel = 0;
        string tt = string.Empty;
        private void InsertionOfRecords(DataTable dt)
        {

            string DealerID = string.Empty;
            string DealerName = string.Empty;
            string Dealercode = string.Empty;
            string HSRPRecord_AuthorizationDate = string.Empty;
            string HSRPRecord_CreationDate = string.Empty;
            string VehicleClass = string.Empty;
            string OrderType = string.Empty;
            string AffixationCode = string.Empty;
            string VehicleRegNo = string.Empty;
            string OwnerName = string.Empty;
            string Address = string.Empty;
            string MobileNo = string.Empty;
            string VehicleType = string.Empty;
            string HSRPRecord_AuthorizationNo = string.Empty;
            string EngineNo = string.Empty;
            string ChassisNo = string.Empty;
            string vehiclemake = string.Empty;
            string ModelName = string.Empty;
            string PRICE = string.Empty;


            string ArrVehicle = string.Empty;
            StringBuilder sb = new StringBuilder();
            orderno = "DL" + System.DateTime.Now.ToString("yyyyMMddHHmmssfff");
            foreach (DataRow dr in dt.Rows)
            {

                DealerName = dr["DealerName"].ToString().Trim();
                Dealercode = dr["Dealercode"].ToString().Trim();

                DateTime d;
                double dResult;

                DateTime hd;

                VehicleClass = dr["VehicleClass"].ToString().Trim();
                OrderType = dr["OrderType"].ToString().Trim();
                AffixationCode = dr["AffixationCenter"].ToString().Trim();
                tt = dr["VehicleRegNo"].ToString().Trim().Replace(" ", "");
                string strVehicle = "select count(*) from Vendor_HSRPRecords where hsrp_stateid='2' and vehicleregno='" + tt + "' and OrderType='" + OrderType + "'";
                int Iresult = Utils.getScalarCount(strVehicle, CnnStringupload);
                if (Iresult > 0)
                {
                    ArrVehicle = ArrVehicle + "|" + tt;
                    llbMSGError.Visible = true;
                    llbMSGError.Text = "";
                    llbMSGError.Text = "Duplicate Records Found.";
                    txtDuplicateRecords.Text = ArrVehicle.ToString();

                }
                else
                {
                    VehicleRegNo = tt;





                    OwnerName = dr["OwnerName"].ToString().Trim();
                    Address = dr["Address"].ToString().Trim();
                    MobileNo = dr["MobileNo"].ToString().Trim();
                    //========================================================
                    VehicleType = dr["VehicleType"].ToString().Trim();


                    // NB,OB,OS,DR,DF - Non-Transport,Transport-
                    //====================================================
                    HSRPRecord_AuthorizationNo = dr["HSRPApplicationNo"].ToString().Trim();
                    EngineNo = dr["EngineNo"].ToString().Trim();
                    ChassisNo = dr["ChassisNo"].ToString().Trim();
                    vehiclemake = dr["vehiclemake"].ToString().Trim();
                    ModelName = dr["ModelName"].ToString().Trim();
                    PRICE = dr["ExShowRoomPrice"].ToString().Trim();
                    tt = dr["VehicleRegNo"].ToString().Trim().Replace(" ", "");
                    strVehicle = "select count(*) from hsrprecords where hsrp_stateid='2' and vehicleregno='" + tt + "' and OrderType='" + OrderType + "'";
                    Iresult = Utils.getScalarCount(strVehicle, CnnStringupload);
                    if (Iresult > 0)
                    {
                        ArrVehicle = ArrVehicle + "|" + tt;
                        llbMSGError.Visible = true;
                        llbMSGError.Text = "";
                        llbMSGError.Text = "Duplicate Records Found.";
                        txtDuplicateRecords.Text = ArrVehicle.ToString();
                        //return;
                    }
                    else
                    {

                        ////if (!string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["DealerID"].ToString()))
                        ////{
                        //// string DealerID = "select DealerID from [" + getExcelSheetName + "]";
                        string SQLString2 = "select sno from delhi_dealermaster where sno='" + DealerID + "'";

                        DataTable dt1 = Utils.GetDataTable(SQLString2, CnnStringupload);
                        if (dt1.Rows.Count == 0)
                        {
                            // SQLString2 = "select dealerid from dealermaster where dealerid='" + DealerID + "'";
                            // dt1 = Utils.GetDataTable(SQLString2, CnnStringupload);
                            if (dt1.Rows.Count == 0)
                            {
                                ////  lblVehicleRegNo.Visible = true;
                                //  llbMSGError0.Text = "DealerID Not Found";
                                //  string vehicleRegNo = VehicleRegNo;
                                //  Label1.Text = (int.Parse(Label1.Text) + 1).ToString();
                                //  if (int.Parse(Label1.Text) % 10 == 0)
                                //  {
                                //      lblVehicleRegNo.Text += vehicleRegNo.ToString() + "\n";
                                //      //ShowDuplicateRecords.Text += VehicleRegno.ToString() + ",";
                                //  }
                                //  else
                                //  {
                                //      lblVehicleRegNo.Text += vehicleRegNo.ToString() + ",";
                                //  }
                            }

                        }
                        if (HSRP_StateID == "2")
                        {
                            if (ddlEmbossingCenter.SelectedItem.Text == "BURARI")
                            {
                                strEmbID = "EC138";
                            }
                            if (ddlEmbossingCenter.SelectedItem.Text == "MAYAPURI")
                            {
                                strEmbID = "EC148";
                            }

                        }
                        else
                        {
                            strEmbID = "";
                        }
                        string rtocode = VehicleRegNo.Substring(0, 4);
                        string x = rtocode.Substring(3, 1);
                        Int32 intValue;
                        if (Int32.TryParse(x, out intValue))
                        {

                            rtocode = VehicleRegNo.Substring(0, 4);
                        }
                        else
                        {
                            rtocode = VehicleRegNo.Substring(0, 3);
                        }
                        int rtolocationid = Utils.getScalarCount("select RTOLocationID from rtolocation where RTOLocationCode ='" + rtocode + "'", CnnString);


                        if ((VehicleClass.ToUpper() == "TRANSPORT" || VehicleClass.ToUpper() == "NON-TRANSPORT") && (VehicleType.ToUpper() == "SCOOTER" || VehicleType.ToUpper() == "MOTOR CYCLE" || VehicleType.ToUpper() == "LMV" || VehicleType.ToUpper() == "LMV(CLASS)" || VehicleType.ToUpper() == "THREE WHEELER" || VehicleType.ToUpper() == "MCV/HCV/TRAILERS" || VehicleType.ToUpper() == "TRACTOR") && (OrderType.ToUpper() == "NB" || OrderType.ToUpper() == "OB" || OrderType.ToUpper() == "DB" || OrderType.ToUpper() == "DR" || OrderType.ToUpper() == "DF" || OrderType.ToUpper() == "OS"))
                        {
                            sb.Append("Insert into Vendor_HSRPRecords (HSRP_StateID,RTOLocationID,DealerName,dealercode,VehicleClass,OrderType,affixationcode,VehicleRegNo,OwnerName,Address1,MobileNo,VehicleType,HSRPRecord_AuthorizationNo,EngineNo,ChassisNo,vehiclemake,modelname,Plate_NetAmount,[CreatedBy],UserId,NAVEMBID,DealerID,orderno) values ('2','" + rtolocationid.ToString() + "','" + DealerName + "','" + Dealercode + "'," +
                            "'" + VehicleClass + "','" + OrderType + "','" + AffixationCode + "','" + VehicleRegNo + "','" + OwnerName + "','" + Address + "'," +
                            "'" + MobileNo + "','" + VehicleType + "','" + HSRPRecord_AuthorizationNo + "','" + EngineNo + "','" + ChassisNo + "','" + vehiclemake + "'," +
                            "'" + ModelName + "'," + PRICE + ",'" + Id + "','" + strUserID + "','" + strEmbID + "','" + userdealerid + "','" + orderno + "');");
                            //SQLString1 = "Insert into dbo.Vendor_HSRPRecords (HSRP_StateID,RTOLocationID,DealerID,DealerName,dealercode,HSRPRecord_AuthorizationDate,HSRPRecord_CreationDate,VehicleClass,OrderType,affixationcode,VehicleRegNo,OwnerName,Address1,MobileNo,VehicleType,HSRPRecord_AuthorizationNo,EngineNo,ChassisNo,vehiclemake,modelname,Plate_NetAmount,[CreatedBy],UserId,NAVEMBID) values ('" + DropDownListStateName.SelectedValue.ToString() + "','" + dropDownListClient.SelectedValue.ToString() + "','" + DealerID + "','" + DealerName + "','" + Dealercode + "','" + d + "'," +
                            //"'" + hd + "','" + VehicleClass + "','" + OrderType + "','" + AffixationCode + "','" + VehicleRegNo + "','" + OwnerName + "','" + Address + "'," +
                            //"'" + MobileNo + "','" + VehicleType + "','" + HSRPRecord_AuthorizationNo + "','" + EngineNo + "','" + ChassisNo + "','" + vehiclemake + "'," +
                            //"'" + ModelName + "'," + PRICE + ",'" + Id + "','" + strUserID + "','"+strEmbID+"')";


                            countupload = countupload + 1;
                            lbltotaluploadrecords.Text = countupload.ToString();
                        }
                        else
                        {
                            errorinexcel++;
                            llbMSGError.Text = "Vehicle No " + VehicleRegNo + " Has Wrong Vehicle Type : '" + VehicleType + "'/Vehicle Class: '" + VehicleClass + "'/Order Type :'" + OrderType + "' Refer Help for Correct Values, update your xls again and reload";
                        }
                    }



                }



            }
            if (sb.ToString() != "")
            {

                Utils.ExecNonQuery(sb.ToString(), CnnStringupload);

            }
            if (countupload > 0)
            {
                llbMSGSuccess.Text = "Record Save Sucessfully. Your Order No : " + orderno;
                string a = Label1.Text;
                lbltotaluploadrecords.Text = countupload.ToString();
                lbltotladuplicaterecords.Text = countDuplicate.ToString();
                btnSync.Enabled = true;

            }
            else
            {


                lbltotladuplicaterecords.Text = countDuplicate.ToString();

            }
        }
        int j;
        private void ValidationCheckOnRecords(DataTable ExcelSheet)
        {
          
            for (int i = 0; i < ExcelSheet.Rows.Count; i++)
            {
                string VihcleRegNo = ExcelSheet.Rows[i]["VehicleRegNo"].ToString().Trim();
                string HSRPApplicationNo = ExcelSheet.Rows[i]["HSRPApplicationNo"].ToString().Trim();              
                string OrderType = ExcelSheet.Rows[i]["OrderType"].ToString().Trim(); 
                string strHSRPApplicationNo = HSRPApplicationNo.Substring(0, 2);
                if (i != 0)
                {

                    for (j = i;  j < ExcelSheet.Rows.Count; j++)
                    {

                   

                    if (i == 1)
                    {
                        j = 0;
                    }
                    else
                    {
                        j--;
                    }

                        if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[j]["VehicleRegNo"].ToString().Trim()))
                        {
                          
                            llbMSGError.Text = "";
                            llbMSGError.Text = "Excel Sheet : Has <b>VehicleRegNo</b> Field Empty At Row : ";
                            FlagIsDirty = true;
                            return;
                        }

                      if (VihcleRegNo == ExcelSheet.Rows[j]["VehicleRegNo"].ToString().Trim())
                        {
                            
                            llbMSGError.Text = "";
                            llbMSGError.Text = "Excel Sheet : Has <b>VehicleRegNo</b> Field Have Duplicate value ";
                            FlagIsDirty = true;
                            return;
                        }


                      if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[j]["HSRPApplicationNo"].ToString().Trim()))
                      {

                          llbMSGError.Text = "";
                          llbMSGError.Text = "Excel Sheet : Has <b>HSRPApplicationNo</b> Field Empty At Row : ";
                          FlagIsDirty = true;
                          return;
                      }

                      if (HSRPApplicationNo == ExcelSheet.Rows[j]["HSRPApplicationNo"].ToString().Trim())
                      {

                          llbMSGError.Text = "";
                          llbMSGError.Text = "Excel Sheet : Has <b>HSRPApplicationNo</b> Field Have Duplicate value ";
                          FlagIsDirty = true;
                          return;
                      }

                     }

                      if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[j]["OrderType"].ToString().Trim()))
                      {

                          llbMSGError.Text = "";
                          llbMSGError.Text = "Excel Sheet : Has <b>OrderType</b> Field Empty At Row : ";
                          FlagIsDirty = true;
                          return;
                      }



                      if (ExcelSheet.Rows[i]["OrderType"].ToString().Trim() == ExcelSheet.Rows[0]["OrderType"].ToString().Trim())
                      {

                          llbMSGError.Text = "";
                          llbMSGError.Text = "Excel Sheet : Has <b>OrderType</b> Field Have Duplicate value ";
                          FlagIsDirty = true;
                          return;
                      }
                   
                  
                }

                if (strHSRPApplicationNo != "DL" && strHSRPApplicationNo != "dl")
                {
                    llbMSGError.Visible = true;
                    llbMSGError.Text = "";
                    llbMSGError.Text = "Please Enter Valid HSRPApplicationNo.";
                    return;
                }

                if (ExcelSheet.Rows[i]["HSRPApplicationNo"].ToString().Trim().Replace(" ", "").Length != 16)
                {
                   
                    llbMSGError.Text = "";
                    llbMSGError.Text = "Excel Sheet : Has <b>HSRPApplicationNo</b> Field Should be  16 characters At Row .";
                    FlagIsDirty = true;
                    return;
                }

                if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["DealerName"].ToString().Trim()))
                {
                   llbMSGError.Text = "";
                    llbMSGError.Text = "Excel Sheet : Has <b>Dealer Name</b> Field Empty At Row .";
                    FlagIsDirty = true;
                    return;
                }



                if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["VehicleClass"].ToString().Trim()))
                {
                   llbMSGError.Text = "";
                    llbMSGError.Text = "Excel Sheet : Has <b>VEHICLECLASS</b> Field Empty At Row . ";
                    FlagIsDirty = true;
                    return;
                }

                if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["OrderType"].ToString().Trim()))
                {
                     llbMSGError.Text = "";
                    llbMSGError.Text = "Excel Sheet : Has <b>OrderType</b> Field Empty At Row .";
                    FlagIsDirty = true;
                    return;
                }


                if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["VehicleRegNo"].ToString().Trim()))
                {
                   llbMSGError.Text = "";
                    llbMSGError.Text = "Excel Sheet : Has <b>VehicleRegNo</b> Field Empty At Row .";
                    FlagIsDirty = true;
                    return;
                }
                if (ExcelSheet.Rows[i]["VehicleRegNo"].ToString().Trim().Replace(" ", "").Length > 10 || ExcelSheet.Rows[i]["VehicleRegNo"].ToString().Trim().Replace(" ", "").Length < 8)
                {
                    llbMSGError.Text = "";
                    llbMSGError.Text = "Excel Sheet : Has <b>VehicleRegNo</b> Field should Not more than 10 or less then 8 characters At Row . " ;
                    FlagIsDirty = true;
                    return;
                }

                string strVehicleNo = ExcelSheet.Rows[i]["VehicleRegNo"].ToString();
                string strCheck = strVehicleNo.Substring(0, 2);
                if (strCheck != "DL" && strCheck != "dl")
                {
                    llbMSGError.Visible = true;
                    llbMSGError.Text = "";
                    llbMSGError.Text = "Please Enter Valid Vehicle RegNo.";
                    return;
                }

                if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["OwnerName"].ToString().Trim()))
                {
                     llbMSGError.Text = "";
                    llbMSGError.Text = "Excel Sheet : Has <b>Owner Name</b> Field Empty At Row .";
                    FlagIsDirty = true;
                    return;
                }
                if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["VehicleType"].ToString().Trim()))
                {
                   llbMSGError.Text = "";
                    llbMSGError.Text = "Excel Sheet : Has <b>VehicleType</b> Field Empty At Row .";
                    FlagIsDirty = true;
                    return;
                }
                if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["EngineNo"].ToString().Trim()))
                {
                    llbMSGError.Text = "";
                    llbMSGError.Text = "Excel Sheet : Has <b>Engine No</b> Field Empty At Row " ;
                    FlagIsDirty = true;
                    return;
                }

                if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["ChassisNo"].ToString().Trim()))
                {
                    llbMSGError.Text = "";
                    llbMSGError.Text = "Excel Sheet : Has <b>Chassis No</b> Field Empty At Row .";
                    FlagIsDirty = true;
                    return;
                }

                string str = ExcelSheet.Rows[i]["ExShowRoomPrice"].ToString().Trim();
                double num;
                if (string.IsNullOrWhiteSpace(str))
                {
                    llbMSGError.Text = "";
                    llbMSGError.Text = "Excel Sheet : Has <b>Show Room Price</b> Field Empty At Row . ";
                    FlagIsDirty = true;
                    return;
                }
                if (!double.TryParse(str, out num))
                {
                    llbMSGError.Text = "";
                    llbMSGError.Text = "Excel Sheet : Has <b>Show Room Price</b> Field wrong price At Row .";
                    FlagIsDirty = true;
                    return;
                }

            }

        }

        protected void DropDownListStateName_SelectedIndexChanged(object sender, EventArgs e)
        {
           // FilldropDownListClient();
        }



        protected void btnSync_Click(object sender, EventArgs e)
        {
            using (var conn = new SqlConnection(CnnString))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();

                try
                {
                    cmd.CommandText = "DataUpload_Insert_Vendor";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                    llbMSGSuccess.Text = "Record Sync Sucessfully.";
                    string strQuery = "select vehicleregno from Vendor_HSRPRecords where process='D' and remarks='allready in system'";
                    DataTable dtVehicle = Utils.GetDataTable(strQuery, CnnString);
                    string strVeh = string.Empty;
                    if (dtVehicle.Rows.Count > 0)
                    {

                        strVeh = dtVehicle.Rows[0][0].ToString();
                        for (int i = 1; i < dtVehicle.Rows.Count; i++)
                        {
                            strVeh = strVeh + "," + dtVehicle.Rows[i][0].ToString();

                        }
                        llbMSGError.Text = strVeh + " are already exist";
                        llbMSGSuccess.Text = "";
                   }
                }
                catch (Exception ex)
                {
                    llbMSGError.Text = "Error in Sync :- " + ex.Message.ToString();
                    //AddLog(ex.Message.ToString());
                }
            }
        }
    }
}
