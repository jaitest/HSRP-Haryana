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



namespace HSRP.Transaction
{
    public partial class Upload_LaserStock_Quantity : System.Web.UI.Page
    {

        string SaveLocation = string.Empty;
        string SQLString = string.Empty;
        string CnnString = string.Empty;
        string getExcelSheetName = string.Empty;
        string Id = string.Empty;
        int intHSRPStateID;
        string strUserID = string.Empty;
        string HSRP_StateID = string.Empty, RTOLocationID = string.Empty, RTOLocationName = string.Empty;
        bool FlagIsDirty = false;
        string ExcelSheetName = string.Empty;
        string orderno = string.Empty;
        int UserType1;
        string USERID = string.Empty;
        string UserType = string.Empty;
        
        
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

               // UserType = Convert.ToInt32(Session["UserType"]);
                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                strUserID = Session["UID"].ToString();
                UserType1 = Convert.ToInt32(Session["UserType"]);
                HSRP_StateID = Session["UserHSRPStateID"].ToString();
                RTOLocationID = Session["UserRTOLocationID"].ToString();
                RTOLocationName = Session["RTOLocationName"].ToString();
                USERID = Session["UID"].ToString();
                UserType = Session["UserType"].ToString();

              //  Id = Session["UID"].ToString();
               // HSRP_StateID = Session["UserHSRPStateID"].ToString();
                if (!IsPostBack)
                {
                    try
                    {
                        if (UserType1.Equals(0))
                        {                                  
                            FilldropDownListOrganization();
                            FilldropDownListClient();
                        }
                        else
                        {
                            labelOrganization.Visible = true;
                            DropDownListStateName.Visible = true;                        
                            FilldropDownListOrganization();
                            FilldropDownListClient();
                        }


                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }



        private void FilldropDownListOrganization()
        {
                
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            if (UserType1.Equals(0))
                     {
                        SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where ActiveStatus='Y' Order by HSRPStateName";
                         Utils.PopulateDropDownList(DropDownListStateName, SQLString.ToString(), CnnString, "--Select State--");
                     }
               else
                     {
                         SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID='"+HSRP_StateID.ToString()+"' and ActiveStatus='Y' Order by HSRPStateName";
                         DataSet dts = Utils.getDataSet(SQLString, CnnString);
                         DropDownListStateName.DataSource = dts;
                         DropDownListStateName.DataBind();        
                      }
          
        }

        private void FilldropDownListClient()
        {
            try
            {
                if (UserType == "0")
                {
                    SQLString = "select distinct EmbCenterName,NAVEMBID from RTOLocation Where navembid is not null and  hsrp_stateid='" + HSRP_StateID + "'  Order by EmbCenterName ";
                    DataTable dts = Utils.GetDataTable(SQLString, CnnString);
                    dropDownListClient.DataSource = dts;
                    dropDownListClient.DataTextField = "EmbCenterName";
                    dropDownListClient.DataValueField = "NAVEMBID";
                    dropDownListClient.DataBind();
                    dropDownListClient.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Emb Center--", "0"));

                }
                else
                {
                    SQLString = "select distinct EmbCenterName,NAVEMBID from RTOLocation Where HSRP_StateID=" + HSRP_StateID + " and navembid is not null and activestatus='Y'  Order by EmbCenterName ";
                    DataTable dts = Utils.GetDataTable(SQLString, CnnString);
                    dropDownListClient.DataSource = dts;
                    dropDownListClient.DataTextField = "EmbCenterName";
                    dropDownListClient.DataValueField = "NAVEMBID";
                    dropDownListClient.DataBind();
                    dropDownListClient.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Emb Center--", "0"));
                }
            }
            catch (Exception)
            {

                throw;
            }
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
             try
             {
                 ExcelSheetName = FileUpload2.PostedFile.FileName + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString();
                 string filename = System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                 string fileExtension = System.IO.Path.GetExtension(FileUpload2.PostedFile.FileName);
                 string fileLocation = System.Configuration.ConfigurationManager.AppSettings["HSRPExcel"].ToString();
                 fileLocation += filename.Replace("\\\\", "\\");
                 if (fileExtension != ".xls" && fileExtension != ".xlsx")
                 {
                     llbMSGError.Text = "";
                     llbMSGError.Text = "Please UpLoad Excel File. ";
                     return;
                 }
                 IExcelDataReader excelReader;
                 DataTable dtExcelRecords = new DataTable();
                 FileUpload2.PostedFile.SaveAs(fileLocation);


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

               
                 ///>>>> Validation Check On All Excel Sheets
                 if (result.Tables[0].Rows.Count > 0 || result != null)
                 {
                     //ValidationCheckOnRecords(result.Tables[0]);
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

        int countDuplicate = 0, countupload = 0,errorinexcel=0;

        DateTime dojdate;
        int orderno1;
        private void InsertionOfRecords( DataTable dtExcelRecords)
        {
            if (dropDownListClient.SelectedItem.Text == "--Select State Name--")
            {
                llbMSGError.Text = "Please Select State Name";
                return;
            }
            if (dropDownListClient.SelectedItem.Text == "--Select Embossing Code--")
            {
                llbMSGError.Text = "Please Select Embossing Center";
                return;
            }
            string hsrp_statatid = dropDownListClient.SelectedValue.ToString();
            RTOLocationID = Session["UserRTOLocationID"].ToString();
            //string rtolocationid=string.Empty;
            string navembid = dropDownListClient.SelectedValue.ToString();
            string strsno = string.Empty;
            string strauditdate = string.Empty;
            string strlocationname = string.Empty;
            string LaserFrom = string.Empty;
           // string LaserTo = string.Empty;
            string BoxNo = string.Empty;
            string ERP_ProductCode = string.Empty;
            string ProductDescription = string.Empty;
           // string Quantity = string.Empty;
           // string strstatus = string.Empty;
           // string ERPQuantity = string.Empty;
            string strstatus = "Yes";
            //string strstatus = Session["RTOLocationName"].ToString();
            string Userid = string.Empty;
           
            StringBuilder sb = new StringBuilder();
           

            foreach (DataRow dr in dtExcelRecords.Rows)
            {
                double dResult;

                strsno = dr["Sr. No"].ToString().Trim();
                //if (txtlocation.Text.ToString() == "" || txtlocation.Text.ToString() == null)
                //{
                //    llbMSGError.Text = "Please Input Location Name";
                //    return;
                //}
                //else 
                //{
                   
                //}
                strlocationname = dr["Location Name"].ToString().Trim();
                if (double.TryParse(dr["Dateof Audit"].ToString(), out dResult))
                {
                    dojdate = DateTime.FromOADate(dResult);
                }
                LaserFrom = dr["LaserNo"].ToString().Trim();               
                BoxNo = dr["BoxNo"].ToString().Trim();
                ERP_ProductCode = dr["ERPProductCode"].ToString().Trim();
                ProductDescription = dr["Product Description"].ToString().Trim();
                //Quantity = dr["Quantity"].ToString().Trim();
               // ERPQuantity = dr["ERPQuantity"].ToString().Trim();
               // strstatus = dr["Location"].ToString().Trim();
                Userid = Session["UID"].ToString();

                //string SQLString1 = "select count(*) from dbo.APNewSOP_SBIMIS where HSRPNO='" + HSRPNO.ToString() + "'";
                //int Iresult = Utils.getScalarCount(SQLString1, CnnStringupload);

                //if (Iresult > 0)
                //{
                //    ArrVehicle = ArrVehicle + "|" + HSRPNO.ToString();
                //    llbMSGError.Visible = true;
                //    llbMSGSuccess.Text = "";
                //    llbMSGError.Text = "Duplicate Records Found.";
                //    countDuplicate = countDuplicate + 1;
                //    txtduploicateempid.Visible = true;
                //    txtduploicateempid.Text = ArrVehicle;
                //}
                //else
                //{


                sb.Append("Insert into EmbossingCenterLaserStock([SNo],[HSRP_STATEID],[RTOLocationID],[NAVEMBID],[RTOLocationName],[LaserFrom],[BoxNO],[ERP_ProductCode],[ProductDescription],[STATUS],[CreateBy],[Audit_Date]) values ('" + strsno + "','" + HSRP_StateID + "','" + RTOLocationID + "','" + navembid + "','" + strlocationname + "','" + LaserFrom + "','" + BoxNo + "','" + ERP_ProductCode + "','" + ProductDescription + "','" + strstatus + "','" + Userid + "','" + dojdate + "')");
                    countupload = countupload + 1;
               // }
               
         }

            int i = 0;
            if (sb.ToString() != "")
            {
                i = Utils.ExecNonQuery(sb.ToString(), CnnStringupload);
            }

            if (countupload == i)
            {
                llbMSGSuccess.Visible = true;
                llbMSGSuccess.Text = "Record save successfully";
                lbltotaluploadrecords.Text = countupload.ToString();
                lbltotladuplicaterecords.Text = countDuplicate.ToString();
            }
          
        }
        int j;
        private void ValidationCheckOnRecords(DataTable ExcelSheet)
        {
            for (int i = 1; i < ExcelSheet.Rows.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["LaserFrom"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : " + getExcelSheetName.Substring(0, getExcelSheetName.Length - 1) + " has <b>LaserFrom<b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;
                }

                if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["LaserTo"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : " + getExcelSheetName.Substring(0, getExcelSheetName.Length - 1) + " has <b>LaserTo</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;
                }
                if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["BoxNo"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : " + getExcelSheetName.Substring(0, getExcelSheetName.Length - 1) + " has <b>BoxNo</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;
                }

                

                if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["ERPProductCode"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : " + getExcelSheetName.Substring(0, getExcelSheetName.Length - 1) + " has <b>ERP_ProductCode</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;
                }


                if (string.IsNullOrEmpty(ExcelSheet.Rows[i]["ProductDescription"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : " + getExcelSheetName.Substring(0, getExcelSheetName.Length - 1) + " has <b>ProductDescription</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;
                }

                if (string.IsNullOrEmpty(ExcelSheet.Rows[i]["Quantity"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : " + getExcelSheetName.Substring(0, getExcelSheetName.Length - 1) + " has <b>Quantity</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;
                }

                if (string.IsNullOrEmpty(ExcelSheet.Rows[i]["ERPQuantity"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : " + getExcelSheetName.Substring(0, getExcelSheetName.Length - 1) + " has <b>ERPQuantity</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;
                }
                if (string.IsNullOrEmpty(ExcelSheet.Rows[i]["Location"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : " + getExcelSheetName.Substring(0, getExcelSheetName.Length - 1) + " has <b>ERPQuantity</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;
                }

            }

           
        }

        protected void DropDownListStateName_SelectedIndexChanged(object sender, EventArgs e)
        {
            //FilldropDownListClient();
        }

        protected void dropDownListClient_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //protected void Button1_Click(object sender, EventArgs e)
        //{
            
        //}

       
  
    }
    }
