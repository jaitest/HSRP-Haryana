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
    public partial class UploadAllEmployeeSalarySheet : System.Web.UI.Page
    {
        string SaveLocation = string.Empty;
        string SQLString = string.Empty;
        string CnnString = string.Empty;
        string getExcelSheetName = string.Empty;
        bool FlagIsDirty = false;      
        int UserType;
        string strUserID = string.Empty;
        string ExcelSheetName = string.Empty;
   
       
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
                if (!IsPostBack)
                {                
                    try
                    {
                      FilldropDownListCompany();
                    }
                    catch (Exception err)
                    {
                        throw err;
                    }
                }
            }
        }



      public void FilldropDownListCompany()
        {
             CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            //if (UserType.Equals(0))
            //{
             SQLString = "select companyid , CompanyName from Company_Name  where ActiveStatus='Y' Order by CompanyName";
                 Utils.PopulateDropDownList(DDlCompany_Name, SQLString.ToString(), CnnString, "--Select Company Name--");
          
          //}
            //else
            //{
            //    SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID=" + HSRP_StateID + " and ActiveStatus='Y' Order by HSRPStateName";
            //    DataSet dts = Utils.getDataSet(SQLString, CnnString);
            //    DDlCompanyName.DataSource = dts;
            //    DDlCompanyName.DataBind();


            //}



        }
       

        protected void Button1_Click(object sender, EventArgs e)
        {
           
            llbMSGError.Text = "";
            llbMSGSuccess.Text = "";

            if (DDlCompany_Name.SelectedItem.Text.ToString().Equals("--Select Company Name--"))
            {

                llbMSGError.Text = "Please  Company Name..";
                return;
            }

            if (DDLMonth.SelectedItem.Text.ToString().Equals("--Select Month--"))
            {
                llbMSGError.Visible = true;
                llbMSGError.Text = "";
                llbMSGError.Text = "Please Select  Month Name...";
                return;
            }

             if (ddlyear.SelectedItem.Text.ToString().Equals("--Select Year--"))
            {
                llbMSGError.Visible = true;
                llbMSGError.Text = "";
                llbMSGError.Text = "Please Select  Year";
                return;
            }
           
           
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
                ExcelSheetName = FileUpload1.PostedFile.FileName;
                string filename = "SalarySheet-" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
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
       
        private void InsertionOfRecords(DataTable dt)
        {

    string EmpCode = string.Empty;
	long ESICNo	 =0;
	long EPFNo  =0;
	long UANNo  =0;
	string BankName = string.Empty;
	long ACountNo =0;
	string BranchName= string.Empty;
	string IFSCode= string.Empty;
	string Name = string.Empty;
	string DOJ	  = string.Empty;
	string Department = string.Empty;
	string Designation = string.Empty;
	string State	 = string.Empty;
	string District = string.Empty;
	string Location  = string.Empty;
	long CTC  =0;
	long ActualSalary =0;
	long SalaryasperWD  =0;
	int ActualDays   =0;
	decimal WD ;
	long Monthly_Basic =0;
	long Monthly_HRA =0;
	long Monthly_Conveyance =0;
	long Monthly_Medical_Allowance  =0;
	long Monthly_CCA = 0;
	long Monthly_Total = 0;
	long Gross_Basic = 0;
	long Gross_HRA   = 0;
	long Gross_Conveyance  = 0;
	long Gross_MedicalAllowance	  = 0;
	long Gross_CCA  = 0;
	long Gross_Total  = 0;
	long EE_ESI  = 0;
	long EE_PF  = 0;	
	long ER_ESI   = 0;	
	long Admin  = 0;	
	long ER_PF  = 0;
	int TDS	 = 0;
    int CUG_HSRP_DED_OTH = 0;
    int SCN_DED = 0;
    int deduction = 0;
    int deduction_Total = 0;
    int Incentives = 0;
    long Net_Salary = 0;
	string Pay_Mode = string.Empty;
	string Status  = string.Empty;
	int UploadedBy  =0;
    int CompanyName = 0;
    int  Month_Name  =0;
	int Year =0;
    

            string ArrVehicle = string.Empty;
            StringBuilder sb = new StringBuilder();

            foreach (DataRow dr in dt.Rows)
            {	   	
                EmpCode= dr["EmpCode"].ToString().Trim();
                ESICNo = Convert.ToInt64(dr["ESICNo"].ToString().Trim());
                EPFNo = Convert.ToInt64(dr["EPFNo"].ToString().Trim());
                UANNo = Convert.ToInt64 (dr["UANNo"].ToString().Trim());
                BankName = dr["BankName"].ToString().Trim();
                ACountNo = Convert.ToInt64(dr["ACcountNo"].ToString().Trim());
                BranchName = dr["BranchName"].ToString().Trim();
                IFSCode= dr["IFSCCode"].ToString().Trim();
                 Name = dr["Name"].ToString().Trim();
                 DOJ = dr["DOJ"].ToString();
                 Department= dr["Department"].ToString().Trim(); 
                 Designation= dr["Designation"].ToString().Trim(); 
                 State= dr["State"].ToString().Trim();                							
                 District= dr["District"].ToString().Trim(); 
                 Location= dr["Location"].ToString().Trim();
                 CTC =Convert.ToInt64(dr["CTC"].ToString().Trim());
                 ActualSalary = Convert.ToInt64(dr["ActualSalary"].ToString().Trim());
                 SalaryasperWD = Convert.ToInt64(dr["SalaryasperWD"].ToString().Trim());
                  ActualDays = Convert.ToInt32(dr["ActualDays"].ToString().Trim());
                  WD =Convert.ToDecimal(dr["WD"].ToString().Trim());
                  Monthly_Basic = Convert.ToInt64(dr["MonthlyBasic"].ToString().Trim());
                  Monthly_HRA = Convert.ToInt64(dr["MonthlyHRA"].ToString().Trim());
                  Monthly_Conveyance = Convert.ToInt64(dr["MonthlyConveyance"].ToString().Trim());
                  Monthly_Medical_Allowance = Convert.ToInt64(dr["MonthlyMedicalAllowance"].ToString().Trim());
                  Monthly_CCA = Convert.ToInt64(dr["MonthlyCCA"].ToString().Trim());
                  Monthly_Total = Convert.ToInt64(dr["MonthlyTotal"].ToString().Trim());
                  Gross_Basic = Convert.ToInt64(dr["GrossBasic"].ToString().Trim());
                  Gross_HRA = Convert.ToInt64(dr["GrossHRA"].ToString().Trim());
                  Gross_Conveyance = Convert.ToInt64(dr["GrossConveyance"].ToString().Trim());
                  Gross_MedicalAllowance = Convert.ToInt64(dr["GrossMedicalAllowance"].ToString().Trim());
                  Gross_CCA = Convert.ToInt64(dr["GrossCCA"].ToString().Trim());
                  Gross_Total = Convert.ToInt64(dr["GrossTotal"].ToString().Trim());
                  EE_ESI = Convert.ToInt64(dr["EEESI"].ToString().Trim());                						
                  EE_PF = Convert.ToInt64(dr["EEPF"].ToString().Trim());
                  ER_ESI = Convert.ToInt64(dr["ERESI"].ToString().Trim());
                  Admin = Convert.ToInt64(dr["Admin"].ToString().Trim());
                  ER_PF = Convert.ToInt64(dr["ERPF"].ToString().Trim());
                 TDS = Convert.ToInt32(dr["TDS"]);
                 CUG_HSRP_DED_OTH = Convert.ToInt32(dr["CUG_HSRP_DED_OTH"]);
                 SCN_DED = Convert.ToInt32(dr["SCNDED"]);
                 deduction = Convert.ToInt32(dr["Deduction"]);
                 deduction_Total = Convert.ToInt32(dr["DeductionTotal"]); ;
                 Incentives = Convert.ToInt32(dr["Incentives"]);
                 Net_Salary = Convert.ToInt64(dr["NetSalary"].ToString().Trim());
                 Pay_Mode = dr["PayMode"].ToString().Trim();
                 //Status =dr["DealerID"].ToString().Trim();
                 UploadedBy = Convert.ToInt32(strUserID.ToString().Trim());
                 CompanyName = Convert.ToInt32( DDlCompany_Name.SelectedValue.ToString());
                 Month_Name =Convert.ToInt32( DDLMonth.SelectedValue.ToString());
                 Year = Convert.ToInt32(ddlyear.SelectedItem.Text.ToString());



                 string strquery = "select count(*) from salarysheet where  LTRIM(RTRIM(Empcode))= LTRIM(RTRIM('" + EmpCode + "'))and LTRIM(RTRIM(Month_Name))= LTRIM(RTRIM(" + Convert.ToInt32(DDLMonth.SelectedValue.ToString()) + ")) and LTRIM(RTRIM(Year))= LTRIM(RTRIM(" + Convert.ToInt32(ddlyear.SelectedValue.ToString()) + ")) and LTRIM(RTRIM(CompanyId))= LTRIM(RTRIM(" + Convert.ToInt32(DDlCompany_Name.SelectedValue.ToString()) + ")) ";
                 int Iresult = Utils.getScalarCount(strquery, CnnStringupload);
                if (Iresult > 0)
                {
                    ArrVehicle = ArrVehicle + "|" + EmpCode;
                    llbMSGError.Visible = true;
                    llbMSGError.Text = "";
                    llbMSGError.Text = "Duplicate Records Found.";                   
                    countDuplicate = countDuplicate + 1;
                    txtduploicateempid.Visible = true;
                   
                    txtduploicateempid.Text = ArrVehicle;
                    
                   
                }
                else
                {
                 sb.Append("Insert into dbo.SalarySheet (EmpCode,ESICNo,	EPFNo ,	UANNo,BankName ,AccountNo ,BranchName ,IFSCode ,	Name  ,	DOJ	 ,	Department ,Designation ,State ,District ,Location ,CTC,ActualSalary ,SalaryasperWD ,ActualDays,WD,Monthly_Basic,Monthly_HRA,Monthly_Conveyance,Monthly_Medical_Allowance,Monthly_CCA ,Monthly_Total,Gross_Basic,Gross_HRA ,Gross_Conveyance ,Gross_MedicalAllowance,Gross_CCA,Gross_Total,EE_ESI,EE_PF,ER_ESI,Admin,ER_PF,TDS,CUG_HSRP_DED_OTH,SCN_DED,deduction ,deduction_Total,Incentives,Net_Salary,Pay_Mode,UploadedDatatime,UploadedBy,CompanyId,Month_Name ,Year,ExcelSheetName) values('" + EmpCode + "','" + ESICNo + "','" + EPFNo + "','" + UANNo + "','" + BankName + "','" + ACountNo + "','" + BranchName + "','" + IFSCode + "','" + Name + "','" + DOJ + "','" + Department + "','" + Designation + "','" + State + "','" + District + "','" + Location + "'," + CTC + "," + ActualSalary + "," + SalaryasperWD + "," + ActualDays + "," + WD + "," + Monthly_Basic + "," + Monthly_HRA + "," + Monthly_Conveyance + "," + Monthly_Medical_Allowance + "," + Monthly_CCA + "," + Monthly_Total + "," + Gross_Basic + "," + Gross_HRA + "," + Gross_Conveyance + "," + Gross_MedicalAllowance + "," + Gross_CCA + "," + Gross_Total + "," + EE_ESI + "," + EE_PF + "," + ER_ESI + "," + Admin + ", " + ER_PF + "," + TDS + "," + CUG_HSRP_DED_OTH + "," + SCN_DED + "," + deduction + "," + deduction_Total + "," + Incentives + "," + Net_Salary + ",'" + Pay_Mode + "', getdate()," + UploadedBy + ",'" + CompanyName + "','" + Month_Name + "'," + Year + " ,'" + ExcelSheetName + "');"); 
                                                      
                    countupload = countupload + 1;
                   ////lbltotaluploadrecords.Text = countupload.ToString();
               }
                
            }

            int i = 10;
            if (sb.ToString() != "")
            {
              i= Utils.ExecNonQuery(sb.ToString(), CnnStringupload);
            }

            if (countupload==i)
            {
                llbMSGSuccess.Visible = true;
                llbMSGSuccess.Text = "Record Save Sucessfully.";
                lbltotaluploadrecords.Text = countupload.ToString();
                lbltotladuplicaterecords.Text = countDuplicate.ToString();
            }

            else
            {
                llbMSGSuccess.Visible = true;
                llbMSGSuccess.Text = "Record  Not Save .";
                lbltotladuplicaterecords.Text = countDuplicate.ToString();

            }
            

        }

               

        private void ValidationCheckOnRecords(DataTable ExcelSheet)
        {
            try
            {

           

            for (int i = 1; i < ExcelSheet.Rows.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["EmpCode"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has <b>EmpCode</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;
                }
                if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["ESICNo"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has <b>ESICNo</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;
                }

                if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["EPFNo"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has <b>EPFNo</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;
                }

                if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["UANNo"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has <b>UANNo</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;
                }

                if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["BankName"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has <b>BankName</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;
                }

                if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["ACcountNo"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has <b>ACcountNo</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;
                }

                if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["BranchName"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has <b>BranchName</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;
                    
                }
                						
                 if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["IFSCCode"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has <b>IFSC Code</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;
                }

                if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["Name"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has <b>Name</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;


                }

                 if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["DOJ"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has <b>DOJ</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;
                }

                if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["Department"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has <b>Department</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;


                }

                 if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["Designation"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has <b>Designation</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;
                }
             	
                if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["State"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has <b>State</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;
                    
                }

                 if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["District"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has <b>District</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;
                }

                if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["Location"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has <b>Location</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;


                }

                 if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["CTC"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has <b>CTC</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;
                }

                if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["ActualSalary"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has <b>ActualSalary</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;


                }
                 if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["SalaryasperWD"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has <b>SalaryasperWD</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;
                }
                				
                if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["ActualDays"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has <b>ActualDays</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;


                }
                 if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["WD"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has <b>WD</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;
                }

                if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["MonthlyBasic"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has <b>MonthlyBasic</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;


                }
                				
                if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["MonthlyHRA"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has <b>MonthlyHRA</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;


                }
                 if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["MonthlyConveyance"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has <b>MonthlyConveyance</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;
                }

                if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["MonthlyMedicalAllowance"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has <b>MonthlyMedicalAllowance</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;


                }
                			
                 if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["MonthlyCCA"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has <b>MonthlyCCA</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;
                }

                if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["MonthlyTotal"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has <b>MonthlyTotal</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;


                }
                 if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["GrossBasic"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has <b>GrossBasic</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;

                }
                if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["GrossHRA"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has <b>GrossHRA</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;


                }
                if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["GrossConveyance"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has <b>GrossConveyance</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;


                }
                   if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["GrossMedicalAllowance"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has <b>GrossMedicalAllowance</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;


                }
                   if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["GrossCCA"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has <b>GrossCCA</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;


                }
                   if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["GrossTotal"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has <b>GrossTotal</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;
				

                }
                   if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["EEESI"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has <b>EEESI</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;


                }
                   if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["EEPF"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has <b>EEPF</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;


                }
                   if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["ERESI"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has <b>ERESI</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;


                }			
                   if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["Admin"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has <b>Admin</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;


                }
                   if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["ERPF"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has <b>ERPF</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;


                }
                   if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["TDS"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has <b>TDS</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;


                }
                			

                if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["CUG_HSRP_DED_OTH"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has <b>CUG_HSRP_DED_OTH</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;


                }

                 if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["SCNDED"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has <b>SCNDED</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;


                }
                 if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["Deduction"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has <b>Deduction</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;


                }
                			
                 if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["DeductionTotal"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has <b>DeductionTotal</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;


                }

                 if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["Incentives"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has <b>Incentives</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;


                }

                 if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["NetSalary"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has <b>NetSalary</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;


                }


                 if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["PayMode"].ToString().Trim()))
                {
                    i = i + 2;
                    llbMSGError.Text = "Excel Sheet : Has <b>PayMode</b> Field Empty At Row : " + i;
                    FlagIsDirty = true;
                    return;


                }
                
            }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }        

        
       
       
    }
}