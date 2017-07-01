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
    public partial class UploadDealerRateMasterBR : System.Web.UI.Page
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
        string CnnStringupload = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();

        protected void Page_Load(object sender, EventArgs e)
        {
          
            Utils.GZipEncodePage();
            lbltotaluploadrecords.Text = "";
            lbltotladuplicaterecords.Text = "";
            

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
                HSRP_StateID = Session["UserHSRPStateID"].ToString();
                if (!IsPostBack)
                {
                   
                    try
                    {
                       
                        if (UserType.Equals(0))
                        {
                            labelOrganization.Visible = true;
                            DropDownListStateName.Visible = true;
                           
                            FilldropDownListOrganization();

                          
                        }
                        else
                        {

                            labelOrganization.Visible = true;
                            DropDownListStateName.Visible = true;
                            
                            FilldropDownListOrganization();

                           
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
            if (UserType.Equals(0))
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where ActiveStatus='Y' Order by HSRPStateName";
                Utils.PopulateDropDownList(DropDownListStateName, SQLString.ToString(), CnnString, "--Select State--");
            }
            else
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID=" + HSRP_StateID + " and ActiveStatus='Y' Order by HSRPStateName";
                DataSet dts = Utils.getDataSet(SQLString, CnnString);
                DropDownListStateName.DataSource = dts;
                DropDownListStateName.DataBind();


            }



        }
       

        protected void Button1_Click(object sender, EventArgs e)
        {
           
          
            llbMSGError.Text = "";
            llbMSGSuccess.Text = "";

            if (DropDownListStateName.SelectedItem.ToString().Equals("--Select State--"))
            {
                llbMSGError.Visible = true;
                llbMSGError.Text = "";
                llbMSGError.Text = "Please Select State...";
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
                ///>>>> Validation Check On All Excel Sheets
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
            string SHOWROMPRICE = string.Empty;
            string VehicleType = string.Empty;
            string VehicleClass = string.Empty;
            string Extracharge = string.Empty;           

            StringBuilder sb = new StringBuilder();

            foreach (DataRow dr in dt.Rows)
            {
                			

                DealerID = dr["DEALERID"].ToString().Trim();
               
                VehicleType = dr["VEHICLETYPE"].ToString().Trim();
                VehicleClass = dr["VEHICLECLASS"].ToString().Trim();
                 SHOWROMPRICE = dr["SHOWROMPRICE"].ToString().Trim();
                 Extracharge = dr["Extracharge"].ToString().Trim();

                string strVehicle = "select count(*) from HSRP_ExtraCharges_RateMaster_Bihar  where  dealerid= '" + DealerID + "' and vehicletype ='" + VehicleType + "' and vehicleclass='" +VehicleClass+ "'";
                int Iresult = Utils.getScalarCount(strVehicle, CnnStringupload);
                if (Iresult > 0)
                {
                    countDuplicate=  Convert.ToInt32(countDuplicate)+ 1;
                 
                }
                else
                { 
                        if ((VehicleClass.ToUpper() == "TRANSPORT" || VehicleClass.ToUpper() == "NON-TRANSPORT") && (VehicleType.ToUpper() == "SCOOTER" || VehicleType.ToUpper() == "MOTOR CYCLE" || VehicleType.ToUpper() == "LMV" || VehicleType.ToUpper() == "LMV(CLASS)" || VehicleType.ToUpper() == "THREE WHEELER" || VehicleType.ToUpper() == "MCV/HCV/TRAILERS" || VehicleType.ToUpper() == "TRACTOR"))
                            {
                                sb.Append("Insert into dbo.HSRP_ExtraCharges_RateMaster_Bihar (dealerid, vehicletype,vehicleclass,exshowroomprice ,extracharges,entrydate,userid) values('" + DealerID + "','" + VehicleType + "','" + VehicleClass + "','" + SHOWROMPRICE + "','" + Extracharge + "' ,getdate(),'" + Id + "');");
                                 countupload = countupload + 1;
                                lbltotaluploadrecords.Text = countupload.ToString();
                         }
                            //else
                            //{
                            //   llbMSGError.Text = "Dealer Id" + " " + DealerID + "Vehicle Type" + " " + VehicleType + "Vehicle Class" + " " + VehicleClass + " Has Some Error in Dealerid/Vehicle Type/Vehicle Class";
                            //}





                }



            }
            if (sb.ToString() != "")
            {

                Utils.ExecNonQuery(sb.ToString(), CnnStringupload);

            }
            if (countupload > 0)
            {
                llbMSGSuccess.Text = "Record Save Sucessfully.";
             
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
                string DEALERID = ExcelSheet.Rows[i]["DEALERID"].ToString().Trim();
                if (string.IsNullOrWhiteSpace(DEALERID))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has <b>DEALER ID</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;
                }
              

                if (Convert.ToInt32(ExcelSheet.Rows[i]["DEALERID"].ToString().Trim()) <= 0)
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has <b>DEALER ID</b> Field Not Valid At Row : " + i;
                    FlagIsDirty = true;
                    return;
                }
                
                if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["VEHICLETYPE"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has <b>VEHICLE TYPE</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;
                }
                string vehicletype=ExcelSheet.Rows[i]["VEHICLETYPE"].ToString().Trim().ToUpper();
                if (vehicletype == "MOTOR CYCLE" ||vehicletype == "SCOOTER" || vehicletype == "TRACTOR" || vehicletype == "THREE WHEELER" || vehicletype == "LMV" || vehicletype == "LMV(CLASS)" || vehicletype == "MCV/HCV/TRAILERS")
                {

                }
                else
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has <b>VEHICLE TYPE </b> Wrong At Row : " + i;
                    FlagIsDirty = true;
                    return;
                }
               
                if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["VEHICLECLASS"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has <b>VEHICLE CLASS </b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;
                }
                string VEHICLECLASS= ExcelSheet.Rows[i]["VEHICLECLASS"].ToString().Trim().ToUpper();

                if(VEHICLECLASS == "TRANSPORT" || VEHICLECLASS == "NON-TRANSPORT")
                {

                }
                else
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has <b>VEHICLE CLASS </b> Wrong At Row : " + i;
                    FlagIsDirty = true;
                    return;
                }

               

                 string str1 = ExcelSheet.Rows[i]["SHOWROMPRICE"].ToString().Trim();
                 double num1;
                 if (string.IsNullOrWhiteSpace(str1))
                 {
                     i = i + 2;
                     llbMSGError.Text = "Excel Sheet : Has <b>Show Room Price</b> Field Empty At Row : " + i;
                     FlagIsDirty = true;
                     return;
                 }
                 if (!double.TryParse(str1, out num1))
                 {
                     i = i + 2;
                     llbMSGError.Text = "Excel Sheet : Has <b>Show Room Price</b> Field wrong price At Row : " + i;
                     FlagIsDirty = true;
                     return;
                 }

                 if (Convert.ToInt32(ExcelSheet.Rows[i]["SHOWROMPRICE"].ToString().Trim()) <= 0)
                 {
                     i = i + 2;
                     llbMSGError.Text = "Excel Sheet : Has <b>Show Room Price</b> Field Not Valid At Row : " + i;
                     FlagIsDirty = true;
                     return;
                 }


                 string EXTRACHARGE = ExcelSheet.Rows[i]["EXTRACHARGE"].ToString().Trim();
                 double numEXTRACHARGE;
                 if (string.IsNullOrWhiteSpace(EXTRACHARGE))
                 {
                     i = i + 2;
                     llbMSGError.Text = "Excel Sheet : Has <b>SEXTRACHARGE</b> Field Empty At Row : " + i;
                     FlagIsDirty = true;
                     return;
                 }
                 if (!double.TryParse(EXTRACHARGE, out numEXTRACHARGE))
                 {
                     i = i + 2;
                     llbMSGError.Text = "Excel Sheet : Has <b>EXTRACHARGE</b> Field wrong  At Row : " + i;
                     FlagIsDirty = true;
                     return;
                 }

                 if (Convert.ToInt32(ExcelSheet.Rows[i]["EXTRACHARGE"].ToString().Trim()) <= 0)
                 {
                     i = i + 2;
                     llbMSGError.Text = "Excel Sheet : Has <b>EXTRACHARGE</b> Field Not Valid At Row : " + i;
                     FlagIsDirty = true;
                     return;
                 }

                string query = "select count(*) from HSRP_ExtraCharges_RateMaster_Bihar  where  dealerid= '" + DEALERID + "' and vehicletype ='" + vehicletype + "' and vehicleclass='" + VEHICLECLASS + "'";
                int k = Utils.getScalarCount(query, CnnStringupload);
                if(k>0)
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has Row No :" + i +"  Already Exist.";
                    FlagIsDirty = true;
                    return;
                }
               

            }
           
          

        }


      

       
      
       
    }
}