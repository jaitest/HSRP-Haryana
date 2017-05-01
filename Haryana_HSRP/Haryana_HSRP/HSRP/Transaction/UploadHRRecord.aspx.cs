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



namespace HSRP.Transaction
{
    public partial class UploadHRRecord : System.Web.UI.Page
    {

        string SaveLocation = string.Empty;
        string SQLString = string.Empty;
        string CnnString = string.Empty;
        string getExcelSheetName = string.Empty;
        string Id = string.Empty;
        int UserType;
        int intHSRPStateID;
        string strUserID = string.Empty;
        string HSRP_StateID = string.Empty, RTOLocationID = string.Empty;
        bool FlagIsDirty = false;

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
                //ComputerIP = Request.UserHostAddress;

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
                            labelClient.Visible = true;

                            dropDownListClient.Visible = true;
                            FilldropDownListOrganization();

                            FilldropDownListClient();
                        }
                        else
                        {

                            labelOrganization.Visible = true;
                            DropDownListStateName.Visible = true;
                            labelClient.Visible = true;

                            dropDownListClient.Visible = true;
                            FilldropDownListOrganization();

                            FilldropDownListClient();
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
               
                // DropDownListStateName.SelectedIndex = DropDownListStateName.Items.Count - 1;
            
                      }
            

            //Utils.PopulateDropDownList(DropDownListStateName, SQLString.ToString(), CnnString, "--Select State--");
            //DropDownListStateName.SelectedIndex = DropDownListStateName.Items.Count - 1;
        }


        private void FilldropDownListClient()
        {
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            if (UserType.Equals(0))
            {
                 int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                 SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID="  + intHSRPStateID + " and ActiveStatus!='N' Order by RTOLocationName";
                 Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), CnnString, "--Select Location--");
            }
            else
            {

                //SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + Session["UserHSRPStateID"].ToString() + " and ActiveStatus!='N' Order by RTOLocationName";
                SQLString = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID where  a.LocationType!='District' and UserRTOLocationMapping.UserID='" + strUserID + "' ";
                DataSet dss = Utils.getDataSet(SQLString, CnnString);
                dropDownListClient.DataSource = dss;
                dropDownListClient.DataBind();
            }
            //Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), CnnString, "--Select Client--");
            //dropDownListClient.SelectedIndex = dropDownListClient.Items.Count - 1;
        }

           protected void btnuploadhrexceldata_Click(object sender, EventArgs e)
             {
              llbMSGError.Text = string.Empty;
            DataTable dtExcelRecords = new DataTable();
            if ((FileUpload2.PostedFile != null) && (FileUpload2.PostedFile.ContentLength > 0))
            {
               
                InsertDataInstage();
               
                
            }
            else
            {
                llbMSGError.Text = "Please select a file to upload.";
                llbMSGSuccess.Text = "";
            }
        }


         string fileLocation = string.Empty;
        private void InsertDataInstage()
        {
            //string filename = System.IO.Path.GetFileName(FileUpload1.PostedFile.FileName);
            string filename = "HSRP-" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
            string fileExtension = System.IO.Path.GetExtension(FileUpload2.PostedFile.FileName);



           //fileLocation = Server.MapPath("~/DealerFolder/" + filename);

            string fileLocation = System.Configuration.ConfigurationManager.AppSettings["HSRPExcel"].ToString();
            fileLocation += filename.Replace("\\\\", "\\");

            if (fileExtension != ".xls" && fileExtension != ".xlsx")
            {
                llbMSGError.Text = "";
                llbMSGError.Text = "Please UpLoad Excel File. ";
                return;
            }

            if (fileExtension == ".xls")
            {
                CnnString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
            }
            else if (fileExtension == ".xlsx")
            {
                CnnString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
            }

           
            
            FileUpload2.PostedFile.SaveAs(fileLocation);


            OleDbConnection con = new OleDbConnection(CnnString);
            OleDbCommand cmd = new OleDbCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Connection = con;
            OleDbDataAdapter dAdapter = new OleDbDataAdapter(cmd);
            DataTable dtExcelRecords = new DataTable();
            con.Open();
            DataTable dtExcelSheetName = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
          
            ////>>>> Loop for all sheets in Excel Sheet
         
            
            for (int i = 0; i < dtExcelSheetName.Rows.Count; i++)
            {
               
                try
                {
                  //// This will give the name of the first sheet. i.e. Sheet1$
                    getExcelSheetName = dtExcelSheetName.Rows[i]["Table_Name"].ToString();

                    if (getExcelSheetName.EndsWith("$"))
                    {

                           // fetching column from excel sheet
                        //cmd.CommandText = "select * from [" + getExcelSheetName + "]";

                        
                        cmd.CommandText = "select HSRPRecord_AuthorizationDate,HSRPRecord_CreationDate," +
                        "HSRPRecord_AuthorizationNo,OwnerName,Address1,MobileNo,OrderType,VehicleClass,VehicleType" +
                        ",ChassisNo,EngineNo,VehicleRegNo,CashReceiptNo,CashReceiptDateTime,NetAmount from [" + getExcelSheetName + "]";

                        
                        dAdapter.SelectCommand = cmd;
                        dAdapter.Fill(dtExcelRecords);

                        DataView ExcelRecordView = new DataView();
                        ExcelRecordView = dtExcelRecords.DefaultView;
                        //ExcelRecordView.RowFilter = "Location<>''";
                       // ExcelRecordView.RowFilter = "[Dealer Name]<>''";
                        dtExcelRecords = ExcelRecordView.ToTable();
                        ///>>>> Validation Check On All Excel Sheets
                        ValidationCheckOnRecords(dtExcelRecords);
                        if (FlagIsDirty)
                        {
                            return;
                        }
                        InsertionOfRecords(dtExcelRecords);
                                     
                    }
                }
                catch (Exception)
                {
                   
                    lbltotladuplicaterecords.Text = countDuplicate.ToString();
                    return;
                }
            }

            


            con.Close();
            con.Dispose();
            if (File.Exists(fileLocation))
            {

                File.Delete(fileLocation);

            }
        }
        int countDuplicate = 0, countupload = 0,errorinexcel=0;
        string tt = string.Empty;
        private void InsertionOfRecords( DataTable dtExcelRecords)
        {
            
            string HSRPRecord_AuthorizationDate = string.Empty;
            string HSRPRecord_CreationDate = string.Empty;
            string HSRPRecord_AuthorizationNo = string.Empty;
            string OwnerName = string.Empty;
            string Address1 = string.Empty;
            string MobileNo = string.Empty;
            string OrderType = string.Empty;
            string VehicleClass = string.Empty;
            string VehicleType = string.Empty;
            string ChassisNo = string.Empty;
            string EngineNo = string.Empty;
            string VehicleRegNo = string.Empty;
            string CashReceiptNo = string.Empty;
            string CashReceiptDateTime = string.Empty;
            string NetAmount = string.Empty;
           
        
        
            

            StringBuilder sb = new StringBuilder();

            foreach (DataRow dr in dtExcelRecords.Rows)
            {
                
              
                //HSRP_StateID = dr["HSRP_StateID"].ToString().Trim();
                //RTOLocationID = dr["RTOLocationID"].ToString().Trim();
                HSRPRecord_AuthorizationDate = dr["HSRPRecord_AuthorizationDate"].ToString().Trim();
                HSRPRecord_CreationDate = dr["HSRPRecord_CreationDate"].ToString().Trim();
              //  OrderDate = dr["HSRPRecord_CreationDate"].ToString().Trim();
                HSRPRecord_AuthorizationNo = dr["HSRPRecord_AuthorizationNo"].ToString().Trim();
                OwnerName = dr["OwnerName"].ToString().Trim();
                Address1 = dr["Address1"].ToString().Trim();
                //VehicleRegNo = String.Empty;
                tt = dr["VehicleRegNo"].ToString().Trim();
                VehicleRegNo = string.Empty;

                byte[] asciiBytes = Encoding.ASCII.GetBytes(tt);
                foreach (Byte b in asciiBytes)
                {
                    
                    if (b >= 32 && b <= 47 || b >= 58 && b <= 64 || b >= 91 && b <= 96 || b >= 123 && b <= 126 || b >= 161 && b <= 255)
                    {
                        
                    }
                    else
                    {
                        VehicleRegNo = VehicleRegNo + char.ConvertFromUtf32(b);
                    }

                }

                
                
                MobileNo = dr["MobileNo"].ToString().Trim();
                OrderType = dr["OrderType"].ToString().Trim();
                VehicleClass = dr["VehicleClass"].ToString().Trim();
                VehicleType = dr["VehicleType"].ToString().Trim();
                ChassisNo = dr["ChassisNo"].ToString().Trim();
                EngineNo = dr["EngineNo"].ToString().Trim();
               
                CashReceiptNo = dr["CashReceiptNo"].ToString().Trim();
                CashReceiptDateTime = dr["CashReceiptDateTime"].ToString().Trim();
                 NetAmount = dr["NetAmount"].ToString().Trim();




                 string SQLString1 = "select count(*) from dbo.HSRPRecords_HR where vehicleRegNo='" + VehicleRegNo.ToString() + "' and (OrderType='" + "NB" + "' or OrderType='" + "OB" + "')";

                // string SQLString2 = "select count(*) from dbo.HSRPRecords_HR where vehicleRegNo='" + VehicleRegNo.ToString() + "' and OrderType='" + "DF" + "' or OrderType='" + "DR" + "' or OrderType='" + "OS" + "' or OrderType='" + "DB" + "' ";
                

                int count= Utils.getScalarCount(SQLString1, CnnStringupload);
               // int count1 = Utils.getScalarCount(SQLString2, CnnStringupload);
               
                if (count > 0)
                {
                    countDuplicate = countDuplicate + 1;
                    lbltotladuplicaterecords.Text = countDuplicate.ToString();
                }


                else
                {

                    if ((VehicleClass.ToUpper() == "TRANSPORT" || VehicleClass.ToUpper() == "NON-TRANSPORT") && (OrderType.ToUpper() == "NB" || OrderType.ToUpper() == "OB" || OrderType.ToUpper() == "DB" || OrderType.ToUpper() == "DR" || OrderType.ToUpper() == "DF" || OrderType.ToUpper() == "OS") && (VehicleType.ToUpper() == "SCOOTER" || VehicleType.ToUpper() == "MOTOR CYCLE" || VehicleType.ToUpper() == "LMV" || VehicleType.ToUpper() == "LMV(CLASS)" || VehicleType.ToUpper() == "THREE WHEELER" || VehicleType.ToUpper() == "MCV/HCV/TRAILERS" || VehicleType.ToUpper() == "TRACTOR"))
                    {

                        SQLString1="Insert into dbo.HSRPRecords_HR ([HSRP_StateID],[RTOLocationID],[HSRPRecord_AuthorizationDate],[HSRPRecord_CreationDate]," +
                       "[OrderDate],[HSRPRecord_AuthorizationNo],[OwnerName],[Address1],[MobileNo],[OrderType],[VehicleClass]" +
                       ",[VehicleType],[ChassisNo],[EngineNo],[VehicleRegNo],[CashReceiptNo],[CashReceiptDateTime],[NetAmount],[duprecord],[CreatedBy]) values ('" + DropDownListStateName.SelectedValue.ToString() + "','" + dropDownListClient.SelectedValue.ToString() + "','" + HSRPRecord_AuthorizationDate + "','" + HSRPRecord_CreationDate + "'," +
                       "'" + HSRPRecord_CreationDate + "','" + HSRPRecord_AuthorizationNo + "','" + OwnerName + "','" + Address1 + "','" + MobileNo + "','" + OrderType + "'," +
                       "'" + VehicleClass + "','" + VehicleType + "','" + ChassisNo + "','" + EngineNo + "','" + VehicleRegNo + "','" + CashReceiptNo + "'," +
                       "'" + CashReceiptDateTime + "'," + NetAmount + ",'" + "N" + "','" + Id + "')";
                       
                        Utils.ExecNonQuery(SQLString1, CnnStringupload);
                        countupload = countupload + 1;
                        lbltotaluploadrecords.Text = countupload.ToString();
                    }


                    else
                    {
                        errorinexcel++;
                        llbMSGError.Text = "Vehicle No" + " " + VehicleRegNo + " Has Some Error in Vehicle Type/Vehicle Class/Order Type";
                    }
                }

             
         }
           
            string SQLString = string.Empty;
            string CnnString = string.Empty;

            if (countupload > 0)
            {
             
                llbMSGSuccess.Text = "Record Save Sucessfully.";
                lbltotaluploadrecords.Text = countupload.ToString();
                lbltotladuplicaterecords.Text = countDuplicate.ToString();
                return;
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
                if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["HSRPRecord_AuthorizationDate"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : " + getExcelSheetName.Substring(0, getExcelSheetName.Length - 1) + " has <b>HSRPRecord_AuthorizationDate<b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;
                }

                if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["VehicleRegno"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : " + getExcelSheetName.Substring(0, getExcelSheetName.Length - 1) + " has <b>VehicleRegno</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;
                }

                if (ExcelSheet.Rows[i]["VehicleRegno"].ToString().Trim().Replace(" ", "").Length > 10)
                {

                    llbMSGError.Text = "Excel Sheet : " + getExcelSheetName.Substring(0, getExcelSheetName.Length - 1) + " has <b>VehicleRegno:" + ExcelSheet.Rows[i]["VehicleRegno"].ToString() + "";
                    i = i + 2;
                    llbMSGError.Text = "</b> Field more than 10 characters At Row : " + i;
                    FlagIsDirty = true;
                    return;
                }

                if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["HSRPRecord_CreationDate"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : " + getExcelSheetName.Substring(0, getExcelSheetName.Length - 1) + " has <b>HSRPRecord_CreationDate</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;
                }

              
                if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["HSRPRecord_AuthorizationNo"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : " + getExcelSheetName.Substring(0, getExcelSheetName.Length - 1) + " has <b>HSRPRecord_AuthorizationNo</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;
                }
                if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["VehicleClass"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : " + getExcelSheetName.Substring(0, getExcelSheetName.Length - 1) + " has <b>VehicleClass</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;
                }

                if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["OrderType"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : " + getExcelSheetName.Substring(0, getExcelSheetName.Length - 1) + " has <b>OrderType</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;
                }


                if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["CashReceiptNo"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : " + getExcelSheetName.Substring(0, getExcelSheetName.Length - 1) + " has <b>CashReceiptNo</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;
                }
                if (ExcelSheet.Rows[i]["VehicleRegNo"].ToString().Trim().Replace(" ", "").Length > 10)
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : " + getExcelSheetName.Substring(0, getExcelSheetName.Length - 1) + " has <b>VehicleRegNo</b> Field more than 10 characters At Row : " + i;
                    FlagIsDirty = true;
                    return;
                }
                if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["VehicleType"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : " + getExcelSheetName.Substring(0, getExcelSheetName.Length - 1) + " has <b>VehicleType</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;
                }
                string str = ExcelSheet.Rows[i]["NetAmount"].ToString().Trim();
                double num;
                if (string.IsNullOrWhiteSpace(str))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : " + getExcelSheetName.Substring(0, getExcelSheetName.Length - 1) + " has <b>NetAmount</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;
                }
                if (!double.TryParse(str, out num))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : " + getExcelSheetName.Substring(0, getExcelSheetName.Length - 1) + " has <b>NetAmount</b> Field wrong price At Row : " + i;
                    FlagIsDirty = true;
                    return;
                }

              //DateTime date=ExcelSheet.Rows[i]["CashReceiptDateTime"].ToString();
              //  if (date<= DateTime.Now.ToString())
              //  {
              //      i = i + 2;
              //      llbMSGError.Text = "Excel Sheet : " + getExcelSheetName.Substring(0, getExcelSheetName.Length - 1) + " has <b>CashReceiptDateTime</b> Field has wrong date : " + i;
              //      FlagIsDirty = true;
              //      return;
              //  }
            }

           
        }

        protected void DropDownListStateName_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilldropDownListClient();
        }

        protected void dropDownListClient_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        
           
    }
    }
