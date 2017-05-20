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
using System.Net;

namespace HSRP.Transaction
{
    public partial class UploadDealerDataDamage : System.Web.UI.Page
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
        string BlockVehicle = string.Empty;
        string BlockArrVehicle = string.Empty;
        string strEmbID = string.Empty;
        string HSRPStateID = string.Empty;
        string ProductivityID = string.Empty;
        string UserName = string.Empty;
        string Sticker = string.Empty;
        string VIP = string.Empty;
        string USERID = string.Empty;
        DataTable dt = new DataTable();
        string dealerid = string.Empty;
        string macbase = string.Empty;
        string sql = string.Empty;
        string sql1 = string.Empty;
        string Dealeardetail = string.Empty;

        string sendURL = string.Empty;
        string SQLString2 = string.Empty;
        string CnnStringupload = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        protected void Page_Load(object sender, EventArgs e)
        {

            Utils.GZipEncodePage();
            lbltotaluploadrecords.Text = "";
            lbltotladuplicaterecords.Text = "";
            lblChassisNo.Text = "";
            llbMSGError2.Text = "";

            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {

                UserType = Convert.ToInt32(Session["UserType"]);
                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                strUserID = Session["UID"].ToString();
                Id = Session["UID"].ToString();
                USERID = Session["UID"].ToString();
                HSRP_StateID = Session["UserHSRPStateID"].ToString();
                HSRPStateID = Session["UserHSRPStateID"].ToString();
                RTOLocationID = Session["UserRTOLocationID"].ToString();
                string sqldealerid = "select dealerid from users where hsrp_stateid = 4  and userid = '" + USERID.ToString() + "'";
                DataTable dtdealerid = Utils.GetDataTable(sqldealerid, CnnString);
                if (dtdealerid.Rows.Count > 0)
                {
                    Session["DealerId"] = dtdealerid.Rows[0]["dealerid"].ToString();
                }

                if (!IsPostBack)
                {

                    try
                    {
                        RTOLocationID = Session["UserRTOLocationID"].ToString();
                        labelOrganization.Visible = true;
                        DropDownListStateName.Visible = true;
                        labelClient.Visible = true;

                        dropDownListClient.Visible = true;
                        FilldropDownListOrganization();

                        FilldropDownListClient();


                    }
                    catch (Exception err)
                    {
                        throw err;
                    }


                }
            }
        }

      

        private void FilldropDownListOrganization()
        {


            SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID=" + HSRP_StateID + " and ActiveStatus='Y' Order by HSRPStateName";
            DataSet dts = Utils.getDataSet(SQLString, CnnString);
            DropDownListStateName.DataSource = dts;
            DropDownListStateName.DataBind();




        }
        private void FilldropDownListClient()
        {
            SQLString = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, a.RTOLocationID FROM RTOLocation a  where  a.RTOLocationID ='" + RTOLocationID + "'and Activestatus='Y' ";
            DataSet dss = Utils.getDataSet(SQLString, CnnString);
            dropDownListClient.DataSource = dss;
            dropDownListClient.DataBind();

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            txtDuplicateRecords.Text = "";
            llbMSGError.Text = "";
            llbMSGError.Text = "";
            llbMSGError2.Text = "";
            llbMSGSuccess.Text = "";
            GridView1.DataSource = null;
            GridView1.Visible = false;


            if (DropDownListStateName.SelectedItem.ToString().Equals("--Select State--"))
            {
                llbMSGError.Visible = true;
                llbMSGError.Text = "";
                llbMSGError.Text = "Please Select State...";
                return;
            }

            if (dropDownListClient.SelectedItem.ToString().Equals("--Select Location--"))
            {
                llbMSGError.Visible = true;
                llbMSGError.Text = "";
                llbMSGError.Text = "Please Select Rto Location...";
                return;
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



                if (fileExtension == ".xlsx")
                {
                    llbMSGError.Text = "The Excel File must be in .xls Format..Kindly Convert Your .xlsx File into .xls format";

                    excelReader.Close();
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
            string fixcharge = string.Empty;


            string ArrVehicle = string.Empty;
            StringBuilder sb = new StringBuilder();

            foreach (DataRow dr in dt.Rows)
            {
                DealerID = dr["DealerID"].ToString().Trim();
                DealerName = dr["DealerName"].ToString().Trim();
                Dealercode = dr["Dealercode"].ToString().Trim();
                HSRPRecord_AuthorizationDate = dr["HSRPRecord_AuthorizationDate"].ToString().Trim();
                HSRPRecord_CreationDate = dr["HSRPRecord_CreationDate"].ToString().Trim();

                DateTime d;
                double dResult;
                if (DateTime.TryParseExact(HSRPRecord_AuthorizationDate, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out d))
                {
                    DateTime d1 = System.DateTime.Now;
                    TimeSpan t = d - d1;
                    if (t.TotalDays > 0)
                    {
                        d = System.DateTime.Now;
                    }
                }
                else if ((double.TryParse(HSRPRecord_AuthorizationDate, out dResult)))
                {

                    // Double cd = Convert.ToDouble(HSRPRecord_AuthorizationDate);
                    d = DateTime.FromOADate(dResult);
                    DateTime d1 = System.DateTime.Now;
                    TimeSpan t = d - d1;
                    if (t.TotalDays > 0)
                    {
                        d = System.DateTime.Now;
                    }

                }


                DateTime hd;
                if (DateTime.TryParseExact(HSRPRecord_CreationDate, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out hd))
                {
                    DateTime d1 = System.DateTime.Now;
                    TimeSpan t = hd - d1;
                    if (t.TotalDays > 0)
                    {
                        hd = System.DateTime.Now;
                    }
                }
                else if ((double.TryParse(HSRPRecord_CreationDate, out dResult)))
                {
                    // Double cd = Convert.ToDouble(HSRPRecord_AuthorizationDate);
                    hd = DateTime.FromOADate(dResult);
                    DateTime d1 = System.DateTime.Now;
                    TimeSpan t = hd - d1;
                    if (t.TotalDays > 0)
                    {
                        hd = System.DateTime.Now;
                    }
                }

                VehicleClass = dr["VehicleClass"].ToString().Trim();
                OrderType = dr["OrderType"].ToString().Trim();
                AffixationCode = dr["AffixationCode"].ToString().Trim();
                EngineNo = dr["EngineNo"].ToString().Trim();
                tt = dr["VehicleRegNo"].ToString().Trim().Replace(" ", "");
                OwnerName = dr["OwnerName"].ToString().Trim();
                Address = dr["Address"].ToString().Trim();
                MobileNo = dr["MobileNo"].ToString().Trim();
                VehicleType = dr["VehicleType"].ToString().Trim();
                HSRPRecord_AuthorizationNo = dr["HSRPRecord_AuthorizationNo"].ToString().Trim();
                ChassisNo = dr["ChassisNo"].ToString().Trim();
                vehiclemake = dr["vehiclemake"].ToString().Trim();
                ModelName = dr["ModelName"].ToString().Trim();
                PRICE = dr["PRICE"].ToString().Trim();

                string strVehicle = "select count(*) from hsrprecords where LTRIM(RTRIM(hsrp_stateid))= LTRIM(RTRIM('4')) and LTRIM(RTRIM(ChassisNo))= LTRIM(RTRIM('" + ChassisNo + "')) and LTRIM(RTRIM(vehicleregno))= LTRIM(RTRIM('" + tt + "')) and orderstatus='CLOSED'";
                int Iresult = Utils.getScalarCount(strVehicle, CnnStringupload);
                if (Iresult > 0)
                {
                    VehicleRegNo = tt;

                    //string SQLString1 = "select ChassisNo from dbo.Vendor_HSRPRecords where LTRIM(RTRIM(ChassisNo))= LTRIM(RTRIM('" + ChassisNo.ToString().Trim() + "'))";
                    //DataTable duplicatecount = Utils.GetDataTable(SQLString1, CnnStringupload);
                    //if (duplicatecount.Rows.Count > 0)
                    //{
                        SQLString2 = "select dealerid  from dealermaster where dealerid='" + DealerID + "'";
                        DataTable dt1 = Utils.GetDataTable(SQLString2, CnnStringupload);
                        if (dt1.Rows.Count > 0)
                        {
                            string excharge = "exec  Business_Rule_DealerHR " + Convert.ToInt32(dt1.Rows[0]["dealerid"]) + " , '" + VehicleType + "','" + VehicleClass + "'," + PRICE + "";
                            DataTable dtexcharge = Utils.GetDataTable(excharge, CnnStringupload);
                            if (dtexcharge.Rows.Count > 0)
                            {
                                if (string.IsNullOrEmpty(dtexcharge.Rows[0]["extracharges"].ToString()))
                                {
                                    llbMSGError.Text = "Please Contact To Administrator.";
                                    return;
                                }
                                else
                                {
                                    fixcharge = dtexcharge.Rows[0]["extracharges"].ToString();
                                }
                            }
                            else
                            {
                                if (VehicleClass.ToUpper().Trim() == "NON-TRANSPORT" && VehicleType.Trim().ToUpper() == "SCOOTER" || VehicleType.Trim().ToUpper() == "MOTOR CYCLE")
                                {
                                    fixcharge = "115.00";
                                }

                                if (VehicleClass.Trim() == "NON-TRANSPORT" && VehicleType.Trim() == "L.M.V. (CAR)" || VehicleType.Trim().ToUpper() == "LMV" || VehicleType.Trim().ToUpper() == "LMV(CLASS)" || VehicleType.Trim() == "MCV/HCV/TRAILERS" || VehicleType.Trim() == "TRACTOR")
                                {
                                    fixcharge = "287.50";
                                }
                                if (VehicleClass.ToUpper().Trim() == "TRANSPORT")
                                {
                                    fixcharge = "345.00";
                                }


                            }

                        }
                        else
                        {
                            lblChassisNo.Visible = true;
                            llbMSGError.Text = "DealerID Not Found";

                            string Engine = EngineNo;
                            Label1.Text = (int.Parse(Label1.Text) + 1).ToString();
                            if (int.Parse(Label1.Text) % 10 == 0)
                            {
                                lblChassisNo.Text += ChassisNo.ToString() + "\n";

                            }
                            else
                            {
                                lblChassisNo.Text += ChassisNo.ToString() + ",";
                            }
                            return;
                        }


                        strEmbID = "";


                        if (string.IsNullOrEmpty(fixcharge.ToString().Trim()))
                        {
                            llbMSGError.Text = "Please Contact To Administrator.";
                            return;
                        }

                        CheckBlockRecord(tt);
                        if (string.IsNullOrEmpty(BlockVehicle))
                        {
                            if ((VehicleClass.ToUpper().Trim() == "TRANSPORT" || VehicleClass.ToUpper().Trim() == "NON-TRANSPORT") && ( OrderType.ToUpper().Trim() == "DB" || OrderType.ToUpper().Trim() == "DR" || OrderType.ToUpper().Trim() == "DF" || OrderType.ToUpper().Trim() == "OS" ) && (VehicleType.ToUpper().Trim() == "SCOOTER" || VehicleType.ToUpper().Trim() == "MOTOR CYCLE" || VehicleType.ToUpper().Trim() == "LMV" || VehicleType.ToUpper().Trim() == "LMV(CLASS)" || VehicleType.ToUpper().Trim() == "THREE WHEELER" || VehicleType.ToUpper().Trim() == "MCV/HCV/TRAILERS" || VehicleType.ToUpper().Trim() == "TRACTOR") && dt1.Rows.Count > 0)
                            {

                                sb.Append("Insert into dbo.Vendor_HSRPRecords (HSRP_StateID,RTOLocationID,DealerID,DealerName,dealercode,HSRPRecord_AuthorizationDate,HSRPRecord_CreationDate,VehicleClass,OrderType,affixationcode,VehicleRegNo,OwnerName,Address1,MobileNo,VehicleType,HSRPRecord_AuthorizationNo,EngineNo,ChassisNo,vehiclemake,modelname,Plate_NetAmount,[CreatedBy],UserId,NAVEMBID,fixingcharge) values('" + DropDownListStateName.SelectedValue.ToString() + "','" + dropDownListClient.SelectedValue.ToString() + "','" + DealerID + "','" + DealerName + "','" + Dealercode + "','" + d + "'," +
                                " GETDATE(),'" + VehicleClass + "','" + OrderType + "','" + AffixationCode + "','" + VehicleRegNo + "','" + OwnerName + "','" + Address + "'," +
                                "'" + MobileNo + "','" + VehicleType + "','" + HSRPRecord_AuthorizationNo + "','" + EngineNo + "','" + ChassisNo + "','" + vehiclemake + "'," +
                                "'" + ModelName + "'," + PRICE + ",'" + Id + "','" + strUserID + "','" + strEmbID + "', " + fixcharge + ");");
                                countupload = countupload + 1;
                                lbltotaluploadrecords.Text = countupload.ToString();

                            }
                            else
                            {
                                errorinexcel++;
                                llbMSGError.Text = "ChassisNo" + " " + ChassisNo + " Has Some Error in Vehicle Type/Vehicle Class/Order Type";
                            }


                        }
                        else
                        {
                            //BlockArrVehicle = BlockArrVehicle + "|" + tt;
                            //txtblockrecords.Text = BlockArrVehicle.ToString();
                            ////----check Block
                        }
                       
                    //}
                    //else
                    //{
                    //    countDuplicate = countDuplicate + 1;
                    //    lbltotladuplicaterecords.Text = duplicatecount.Rows[0]["ChassisNo"].ToString();
                       

                    //}

                   

                }
                    
                else
                {
                    ArrVehicle = ArrVehicle + "|" + ChassisNo;
                    llbMSGError2.Visible = true;
                    llbMSGError2.Text = "";
                    llbMSGError2.Text = "Records  Not Found.";
                    txtDuplicateRecords.Text = ArrVehicle.ToString();

                    countDuplicate = countDuplicate + 1;
                  
           


                }



            }
            if (sb.ToString() != "")
            {

                Utils.ExecNonQuery(sb.ToString(), CnnStringupload);

            }
            if (countupload > 0)
            {
                llbMSGSuccess.Text = "Record Save Sucessfully.";
                string a = Label1.Text;
                lbltotaluploadrecords.Text = countupload.ToString();
                lbltotladuplicaterecords.Text = countDuplicate.ToString();


            }
            else
            {


                lbltotladuplicaterecords.Text = countDuplicate.ToString();

            }
        }

        private void ValidationCheckOnRecords(DataTable ExcelSheet)
        {
            for (int i = 0; i < ExcelSheet.Rows.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["ChassisNo"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has <b>Chassis No</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;
                }

                for (int jj = 0; jj < ExcelSheet.Rows.Count; jj++)
                {
                    if (i == jj)
                    {

                    }
                    else
                    {
                        if (ExcelSheet.Rows[i]["ChassisNo"].ToString().Trim().ToUpper() == ExcelSheet.Rows[jj]["ChassisNo"].ToString().Trim().ToUpper())
                        {
                            i = i + 2;
                            llbMSGError.Text = "Excel Sheet : Has <b>Chassis No</b> Duplicate At Row : " + i;
                            FlagIsDirty = true;
                            return;

                        }

                    }

                }

            }
            //validation for vehiclerergno .
            for (int i = 0; i < ExcelSheet.Rows.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["OrderType"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has <b>OrderType</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;
                }

                if (ExcelSheet.Rows[i]["OrderType"].ToString().Trim().ToUpper() == "NB") 
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has <b>OrderType</b> Field Empty Wrong At Row : " + i;
                    FlagIsDirty = true;
                    return;
                }

                if ( ExcelSheet.Rows[i]["OrderType"].ToString().Trim() == "OB") 
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has <b>OrderType</b> Field Empty Wrong At Row : " + i;
                    FlagIsDirty = true;
                    return;
                }
                if (ExcelSheet.Rows[i]["OrderType"].ToString().Trim().ToUpper() == "OS") 
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has <b>OrderType</b> Field Empty Wrong At Row : " + i;
                    FlagIsDirty = true;
                    return;
                }
               
                

                if (ExcelSheet.Rows[i]["VehicleRegNo"].ToString().Trim() != "")
                {
                    string strVehicleNo = ExcelSheet.Rows[i]["VehicleRegNo"].ToString().Trim();
                    if (ExcelSheet.Rows[i]["VehicleRegNo"].ToString().Trim().Length < 4)
                    {
                        i = i + 2;
                        llbMSGError.Text = "Excel Sheet : Has <b>VehicleRegNo</b> Field Wrong At Row : " + i;
                        FlagIsDirty = true;
                        return;
                    }

                    string strCheck = strVehicleNo.Substring(0, 2);
                    if (strCheck.ToUpper().ToString().Trim() != "HR")
                    {

                        if (strCheck.ToUpper().ToString().Trim() != "HY")
                        {
                            i = i + 2;
                            llbMSGError.Text = "Excel Sheet : Has <b>VehicleRegNo</b> Field  Wrong At Row : " + i;
                            FlagIsDirty = true;
                            return;

                        }

                    }

                    if (ExcelSheet.Rows[i]["VehicleRegNo"].ToString().Trim().Length > 10)
                    {
                        i = i + 2;
                        llbMSGError.Text = "Excel Sheet : Has <b>VehicleRegNo</b> Field Wrong At Row : " + i;
                        FlagIsDirty = true;
                        return;
                    }



                }

            }
            //validation for vehiclerergno .

            for (int i = 0; i < ExcelSheet.Rows.Count; i++)
            {

                if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["DealerID"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has <b>DealerID</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;
                }

                if (ExcelSheet.Rows[i]["DealerID"].ToString().Trim() != Session["DealerId"].ToString().Trim())
                {
                    i = i + 2;
                    llbMSGError.Text = " Please Check Dealer Id  At Row : " + i;
                    FlagIsDirty = true;
                    return;
                }


                if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["DealerName"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has <b>Dealer Name</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;
                }


                String HSRPRecord_AuthorizationDate = ExcelSheet.Rows[i]["HSRPRecord_AuthorizationDate"].ToString().Trim();

                if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["HSRPRecord_AuthorizationDate"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has <b>AUTHDATE</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;
                }

                DateTime result;
                double dResult;
                if (DateTime.TryParseExact(HSRPRecord_AuthorizationDate, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
                {

                }
                else if (double.TryParse(HSRPRecord_AuthorizationDate, out dResult))
                {
                    try
                    {
                        DateTime d = DateTime.FromOADate(dResult);

                        if ((d.Month > 12) || (d.Day > 31))
                        {
                            llbMSGError.Text = "Excel Sheet : Has <b>AUTHDATE:" + ExcelSheet.Rows[i]["ClosedDate"].ToString() + "";

                            i = i + 2;
                            llbMSGError.Text = "</b> Authorization Date Format Wrong At Row : " + i + " It Should Be In MM/DD/yyyy format";
                            FlagIsDirty = true;
                            break;
                        }
                    }
                    catch (Exception ee)
                    {
                        llbMSGError.Text = "</b> Authorization Date Format Wrong At Row : " + i + " It Should Be In MM/DD/yyyy format";
                        FlagIsDirty = true;
                        break;
                    }

                }
                else
                {
                    llbMSGError.Text = "</b> Authorization Date Format Wrong At Row : " + i + " It Should Be In MM/DD/yyyy format";
                    FlagIsDirty = true;
                    break;
                }






                String HSRPRecord_CreationDate = ExcelSheet.Rows[i]["HSRPRecord_CreationDate"].ToString().Trim();

                if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["HSRPRecord_CreationDate"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has <b>CREATIONDATE</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;
                }

                if (DateTime.TryParseExact(HSRPRecord_CreationDate, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
                {


                }
                else if (double.TryParse(HSRPRecord_CreationDate, out dResult))
                {

                    try
                    {
                        DateTime d = DateTime.FromOADate(dResult);

                        if ((d.Month > 12) || (d.Day > 31))
                        {
                            llbMSGError.Text = "Excel Sheet : Has <b>CREATIONDATE:" + ExcelSheet.Rows[i]["ClosedDate"].ToString() + "";

                            i = i + 2;
                            llbMSGError.Text = "</b> Creation Date Format Wrong At Row : " + i + " It Should Be In MM/DD/yyyy format";
                            FlagIsDirty = true;
                            break;
                        }
                        else
                        {

                        }
                    }
                    catch (Exception ee)
                    {
                        llbMSGError.Text = "</b> Creation Date Format Wrong At Row : " + i + " It Should Be In MM/DD/yyyy format";
                        FlagIsDirty = true;
                        break;
                    }

                }
                else
                {
                    llbMSGError.Text = "</b> Creation Date Format Wrong At Row : " + i + " It Should Be In MM/DD/yyyy format";
                    FlagIsDirty = true;
                    break;
                }


                if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["VehicleClass"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has <b>VEHICLECLASS</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;
                }

               


                if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["VehicleType"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has <b>VehicleType</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;
                }


                string str = ExcelSheet.Rows[i]["PRICE"].ToString().Trim();
                double num;
                if (string.IsNullOrWhiteSpace(str))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has <b>Show Room Price</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;
                }
                if (!double.TryParse(str, out num))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has <b>Show Room Price</b> Field wrong price At Row : " + i;
                    FlagIsDirty = true;
                    return;
                }
            }


        }

        protected void DropDownListStateName_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilldropDownListClient();
        }

        protected void dropDownListClient_SelectedIndexChanged(object sender, EventArgs e)
        {

        }



        protected void btnSync_Click(object sender, EventArgs e)
        {
            llbMSGSuccess.Text = "";
            llbMSGError.Text = "";
            string q = "select  count(*) from Vendor_HSRPRecords where process ='N' and createdby='" + Id.ToString() + "' and hsrp_stateid='" + DropDownListStateName.SelectedValue.ToString() + "'";
            DataTable dtCount = Utils.GetDataTable(q, CnnString);
            if (dtCount.Rows.Count > 0)
            {
                ShowGrid();

            }
            else
            {
                llbMSGError.Visible = true;
                llbMSGError.Text = "Please  Upload  a file then Sync.";
                llbMSGSuccess.Text = "";

            }
        }
        public void CheckBlockRecord(string vehicleregNo)
        {
            BlockVehicle = string.Empty;

            String SqlQry = "select distinct VehicleregNo  from  hsrpexcelupload WHERE  ISNULL(VehicleregNo,'')!=''";
            DataTable dtbl = Utils.GetDataTable(SqlQry, CnnStringupload);
            if (dtbl.Rows.Count > 0)
            {

                for (int i = 0; i < dtbl.Rows.Count; i++)
                {
                    if (vehicleregNo.ToUpper().Trim() == Convert.ToString(dtbl.Rows[i]["VehicleregNo"]).ToUpper().Trim())
                    {
                        BlockVehicle = Convert.ToString(dtbl.Rows[i]["VehicleregNo"]).Trim();
                        return;
                    }
                }

            }


        }

        StringBuilder sb = new StringBuilder();
        CheckBox chk;
        protected void CHKSelect1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk1 = GridView1.HeaderRow.FindControl("CHKSelect1") as CheckBox;
            if (chk1.Checked == true)
            {
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {

                    chk = GridView1.Rows[i].Cells[0].FindControl("CHKSelect") as CheckBox;
                    chk.Checked = true;
                }
            }
            else if (chk1.Checked == false)
            {
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {

                    chk = GridView1.Rows[i].Cells[0].FindControl("CHKSelect") as CheckBox;
                    chk.Checked = false;
                }
            }

        }
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            ShowGrid();

        }
        private void ShowGrid()
        {
            string SQLString = "select hsrprecordid , dealerid  , dealercode , vehicleregno , rto.rtolocationname ,ownername,address1,vehicleclass,vehicletype, ordertype ,chassisno,engineno , fixingcharge, vh.Mobileno  from Vendor_HSRPRecords vh join rtolocation rto on  rto.rtolocationid= vh.rtolocationid  where vh.process ='N' and vh.createdby='" + Id.ToString() + "' and vh.hsrp_stateid='" + DropDownListStateName.SelectedValue.ToString() + "'";
            dt = Utils.GetDataTable(SQLString, CnnString);
            if (dt.Rows.Count > 0)
            {
                GridView1.Visible = true;
                btnsave.Visible = true;
                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
            else
            {
                //llbMSGError.Text = ".";
                //llbMSGSuccess.Text = "";
                GridView1.DataSource = null;
                GridView1.DataBind();
                btnsave.Visible = false;

            }

        }
        string strVeh = string.Empty;
        protected void btnsave_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                chk = GridView1.Rows[i].Cells[0].FindControl("CHKSelect") as CheckBox;
                if (chk.Checked == true)
                {
                    string strRecordId = GridView1.DataKeys[i]["hsrprecordid"].ToString();
                    Label lblChasisNo = GridView1.Rows[i].Cells[1].FindControl("lblchasisno") as Label;
                    Label lblVehicletype = GridView1.Rows[i].Cells[1].FindControl("lblvehicletype") as Label;
                    Label lblVehicleclass = GridView1.Rows[i].Cells[1].FindControl("lblvehicleclass") as Label;
                    Label lblordertype = GridView1.Rows[i].Cells[1].FindControl("lblordertype") as Label;
                    Label lblfixcharge = GridView1.Rows[i].Cells[1].FindControl("lblfixcharge") as Label;
                    Label lblMobileNO = GridView1.Rows[i].Cells[1].FindControl("lblMobile") as Label;

                    Label lblvehicleregno = GridView1.Rows[i].Cells[1].FindControl("lblvehicleregno") as Label;

                    SQLString2 = "select dbo.hsrpplateamt ('" + 4 + "','" + lblVehicletype.Text.ToString() + "','" + lblVehicleclass.Text.ToString() + "','" + lblordertype.Text.ToString() + "') as Amount";

                    DataTable dt1 = Utils.GetDataTable(SQLString2, CnnString);
                    string amt = "0";
                    if (dt1.Rows.Count > 0)
                    {
                        amt = dt1.Rows[0]["Amount"].ToString();
                        if (string.IsNullOrEmpty(dt1.Rows[0]["Amount"].ToString()))
                        {
                            llbMSGError.Visible = true;
                            llbMSGError.Text = "Please  Contact With Administrator.";
                            return;
                        }
                    }
                    else
                    {
                        llbMSGError.Visible = true;
                        llbMSGError.Text = "Please  Contact With Administrator.";
                        return;
                    }


                    string sqlquery = "select isnull(sum(DepositAmount),0) as DepositAmount from [BankTransaction] where approvedstatus='Y' and userid='" + USERID + "'";
                    string DepositAmount = Utils.getScalarValue(sqlquery, CnnString);
                    if (DepositAmount.ToString().Trim() == "")
                    {
                        DepositAmount = "0";

                    }

                    decimal decimalDepositAmount = decimal.Parse(DepositAmount);

                    string sqlq = "select isnull(sum(roundoff_netamount),0) as amount from HSRPRecords where createdby='" + USERID + "' and HSRP_StateID=4";
                    string collamt = Utils.getScalarValue(sqlq, CnnString);

                    string sqlfixcharger = "select isnull(sum(fixingcharge),0) as fixingcharge from HSRPRecords where createdby='" + USERID + "' and HSRP_StateID=4";
                    string fixchargeamt = Utils.getScalarValue(sqlfixcharger, CnnString);

                    decimal inttotcoll = decimal.Parse(collamt) + decimal.Parse(fixchargeamt);
                    decimal availableAmount = decimalDepositAmount - inttotcoll;

                    decimal nowtotalamtneed = decimal.Parse(amt) + decimal.Parse(lblfixcharge.Text.ToString());


                    if (availableAmount < nowtotalamtneed)
                    {
                        llbMSGError.Visible = true;
                        llbMSGSuccess.Text = "";
                        llbMSGError.Text = "Please  Contact With Administrator.";
                        ShowGrid();
                        return;
                    }
                    else
                    {

                        using (var conn = new SqlConnection(CnnString))
                        using (var cmd = conn.CreateCommand())
                        {
                            conn.Open();
                            try
                            {
                                cmd.CommandText = "[DataUpload_Insert_Vendor_Damage_hr]";
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@createdBy", Convert.ToInt32(Session["UID"].ToString()));
                                cmd.Parameters.AddWithValue("@recordid", Convert.ToInt32(strRecordId));
                                cmd.Parameters.AddWithValue("@chessisno", lblChasisNo.Text.Trim().ToString());
                                cmd.ExecuteNonQuery();                                                   
                                string strQuery = "select ChassisNo from Vendor_HSRPRecords where process='D' and remarks='allready not in system'";
                                DataTable dtVehicle = Utils.GetDataTable(strQuery, CnnString);
                               
                                if (dtVehicle.Rows.Count > 0)
                                {
                                    strVeh += strVeh + "," + dtVehicle.Rows[0][0].ToString();
                                    llbMSGError.Text = strVeh + " are already not exist";
                                    llbMSGSuccess.Text = "";
                                    string strUpdateQuery = "update  Vendor_HSRPRecords set remarks='allready not in system send' where process='D' and remarks='allready not in system'";
                                    Utils.ExecNonQuery(strUpdateQuery, CnnString);

                                }
                                else
                                {
                                    llbMSGSuccess.Text = "Record Sync Sucessfully.";

                                }




                            }
                            catch (Exception ex)
                            {
                                llbMSGError.Text = "Error in Sync :- " + ex.Message.ToString();

                            }
                        }


                    }



                }

                llbMSGSuccess.Text = "Please select a record.";



            }

            ShowGrid();
        }




    }
}
