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
    public partial class UploadTgOnlinePaymentRecods : System.Web.UI.Page
    {
        string SaveLocation = string.Empty;
        string SQLString = string.Empty;
        string CnnString = string.Empty;
        string getExcelSheetName = string.Empty;
        string Id = string.Empty;
        string HSRP_StateID = string.Empty, RTOLocationID = string.Empty;
        int RTOLocation_ID;
        bool FlagIsDirty = false;
        int UserType;
        string strUserID = string.Empty;
        
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
           


            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {
               RTOLocation_ID = Convert.ToInt32(Session["UserRTOLocationID"]);

                UserType = Convert.ToInt32(Session["UserType"]);
                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                strUserID = Session["UID"].ToString();
              //  userdealerid = Session["userdealerid"].ToString();
                Id = Session["UID"].ToString();
                HSRP_StateID = Session["UserHSRPStateID"].ToString();
                if (!IsPostBack)
                {
                   

                  
                }
            }
        }



        protected void Button1_Click(object sender, EventArgs e)
        {
         
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
                    llbMSGSuccess.Text = "";
                    llbMSGError.Text = "";
                    llbMSGError.Text = "Please Select a file to Upload.";
                    return;
                   
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
                string filename = "TGOnlinePaymentRecord-" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                string fileExtension = System.IO.Path.GetExtension(FileUpload1.PostedFile.FileName);
                string fileLocation = @"C:\\\\DealerFolder\\\\";
                if (!Directory.Exists(fileLocation))
                {
                    Directory.CreateDirectory(fileLocation);
                }
               // string path = System.Configuration.ConfigurationManager.AppSettings["DealerFolder"].ToString();

                fileLocation += filename.Replace("\\\\", "\\");


                

                if (fileExtension != ".xls" && fileExtension != ".xlsx")
                {
                    llbMSGSuccess.Text = "";
                    llbMSGError.Text = "";
                    llbMSGError.Text = "Please Upload Excel File. ";
                    return;
                }
                IExcelDataReader excelReader;
                DataTable dtExcelRecords = new DataTable();
                FileUpload1.PostedFile.SaveAs(fileLocation);

                FileStream stream = File.Open(fileLocation, FileMode.Open, FileAccess.Read);       


                excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                if (fileExtension != ".xls")
                {
                    llbMSGSuccess.Text = "";  
                    llbMSGError.Text = "";
                    llbMSGError.Text = "The Excel File must be in .xls Format..Kindly Convert Your  File into .xls format";

                    return;
                }


                excelReader.IsFirstRowAsColumnNames = true;
                DataSet result = excelReader.AsDataSet();

                if (result == null)
                {
                    llbMSGSuccess.Text = "";
                    llbMSGError.Text = "";
                    llbMSGError.Text = " Excel file has wrong format.";
                    return;
                }

                excelReader.Close();
                if (result.Tables[0].Rows.Count > 0 || result != null)
                {
                  //   ValidationCheckOnRecords(result.Tables[0]);
                    //if (FlagIsDirty)
                    //{
                    //    return;
                    //}
                     InsertionOfRecords(result.Tables[0]);
                     if (FlagIsDirty)
                     {
                         return;
                     }
                }
                else
                {
                    llbMSGError.Text = "";
                    llbMSGError.Text = "Recrd Not Exist in excel file";
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

            string BankId = string.Empty;
            string BankName = string.Empty;
            string TPSLtransactionId = string.Empty;
            string SmTransactionId = string.Empty;
            string BankTransactionId = string.Empty;
            string TotalAmount = string.Empty;
            string Charges = string.Empty;
            string ServiceTax = string.Empty;
            string NetAmount = string.Empty;
            string TransactionDate = string.Empty;
            string TransactionTime = string.Empty;
            string PaymentDate = string.Empty;
            string SrcItc = string.Empty;
            string Servicetax = string.Empty;
            orderno = "TG" + System.DateTime.Now.ToString("yyyyMMddHHmmssfff");
           // string  = Convert.ToString(System.DateTime.Now);
            string UploadedDateTime = System.DateTime.Now.Day.ToString() + "/" + System.DateTime.Now.Month.ToString() + "/" + System.DateTime.Now.Year.ToString();
               
            StringBuilder sb = new StringBuilder();
            //for (int i = 0; i < dt.Columns.Count; i++)
            //{
            //    string str= dt.Columns[i].ColumnName.ToString();
            //}
            

            if (dt.Columns[1].ColumnName.ToString()!= "Bank Id")
            {
                llbMSGSuccess.Text = "";
                llbMSGError.Text = "";
                FlagIsDirty = true;
                llbMSGError.Text = " Excel file has wrong format.";
                return;
            }
            if (dt.Columns[2].ColumnName.ToString()!= "Bank Name")
            {
                llbMSGSuccess.Text = "";
                llbMSGError.Text = "";
                FlagIsDirty = true;
                llbMSGError.Text = " Excel file has wrong format.";
                return;

            }
             if (dt.Columns[3].ColumnName.ToString() != "TPSL Transaction id")
            {
                llbMSGSuccess.Text = "";
                llbMSGError.Text = "";
                FlagIsDirty = true;
                llbMSGError.Text = " Excel file has wrong format.";
                return;
            }

            if (dt.Columns[4].ColumnName.ToString() != "Sm Transaction Id")
            {
                llbMSGSuccess.Text = "";
                llbMSGError.Text = "";
                FlagIsDirty = true;
                llbMSGError.Text = " Excel file has wrong format.";
                return;
            }


            if (dt.Columns[5].ColumnName.ToString() != "Bank Transaction id")
            {
                llbMSGSuccess.Text = "";
                llbMSGError.Text = "";
                FlagIsDirty = true;
                llbMSGError.Text = " Excel file has wrong format.";
                return;
            }


             if (dt.Columns[6].ColumnName.ToString() != "Total Amount")
            {
                llbMSGSuccess.Text = "";
                llbMSGError.Text = "";
                FlagIsDirty = true;
                llbMSGError.Text = " Excel file has wrong format.";
                return;
            }


             if (dt.Columns[7].ColumnName.ToString() != "Charges")
            {
                llbMSGSuccess.Text = "";
                llbMSGError.Text = "";
                FlagIsDirty = true;
                llbMSGError.Text = " Excel file has wrong format.";
                return;
            }

            if (dt.Columns[8].ColumnName.ToString() != "Service Tax")
            {
                llbMSGSuccess.Text = "";
                llbMSGError.Text = "";
                FlagIsDirty = true;
                llbMSGError.Text = " Excel file has wrong format.";
                return;
            }

           if (dt.Columns[9].ColumnName.ToString() != "Net Amount")
            {
                llbMSGSuccess.Text = "";
                llbMSGError.Text = "";
                FlagIsDirty = true;
                llbMSGError.Text = " Excel file has wrong format.";
                return;
            }


             if (dt.Columns[10].ColumnName.ToString() != "Transaction date")
            {
                llbMSGSuccess.Text = "";
                llbMSGError.Text = "";
                FlagIsDirty = true;
                llbMSGError.Text = " Excel file has wrong format.";
                return;
            }


            if (dt.Columns[11].ColumnName.ToString() != "Transaction time")
            {
                llbMSGSuccess.Text = "";
                llbMSGError.Text = "";
                FlagIsDirty = true;
                llbMSGError.Text = " Excel file has wrong format.";
                return;
            }

             if (dt.Columns[12].ColumnName.ToString() != "Payment Date")
            {
                llbMSGSuccess.Text = "";
                llbMSGError.Text = "";
                FlagIsDirty = true;
                llbMSGError.Text = " Excel file has wrong format.";
                return;
            }

             if (dt.Columns[13].ColumnName.ToString() != "SRC ITC ")
            {
                llbMSGSuccess.Text = "";
                llbMSGError.Text = "";
                FlagIsDirty = true;
                llbMSGError.Text = " Excel file has wrong format.";
                return;
            }

          
            
            
                foreach (DataRow dr in dt.Rows) 
                {

                    BankId = dr["Bank Id"].ToString().Trim();
                    BankName = dr["Bank Name"].ToString().Trim();

                    TPSLtransactionId = dr["TPSL Transaction id"].ToString().Trim();
                    SmTransactionId = dr["Sm Transaction Id"].ToString().Trim();
                    BankTransactionId = dr["Bank Transaction id"].ToString().Trim();
                    TotalAmount = dr["Total Amount"].ToString().Trim();

                    Charges = dr["Charges"].ToString().Trim();
                    Servicetax = dr["Service Tax"].ToString().Trim();

                    NetAmount = dr["Net Amount"].ToString().Trim();
                    TransactionDate = dr["Transaction date"].ToString().Trim();
                    TransactionTime = dr["Transaction time"].ToString().Trim();
                    PaymentDate = dr["Payment Date"].ToString().Trim();
                    SrcItc = dr["SRC ITC "].ToString().Trim();


                    string strquery = "select count(*) from TgOnlinePaymentReco where  BankTransactionId='" + BankTransactionId + "' and SmTransactionId='" + SmTransactionId + "' and TPSLtransactionId ='" + TPSLtransactionId + "'";
                    int Iresult = Utils.getScalarCount(strquery, CnnStringupload);
                    if (Iresult > 0)
                    {

                       
                        llbMSGSuccess.Text = "";
                        llbMSGError.Text = "";
                        llbMSGError.Text = " Records Already Exists.";
                        // txtDuplicateRecords.Text = ArrVehicle.ToString();

                    }
                    else
                    {


                        sb.Append("insert into TgOnlinePaymentReco (BankId ,BnakName ,TPSLtransactionId ,SmTransactionId , BankTransactionId ,TotalAmount ,Charges ,ServiceTax ,NetAmount ,TransactionDate  ,TransactionTime ,PaymentDate,SrcItc ,stateId  ,UserId ,RtoLocationId ,UploadedDateTime ,OrderNo) values ('"
                        + Convert.ToInt32(BankId) + "','" + BankName + "','" + TPSLtransactionId + "'," + "'" + SmTransactionId + "','" + BankTransactionId + "','" + TotalAmount
                        + "','" + Charges + "','" + Servicetax + "','" + NetAmount + "'," + "'" + TransactionDate + "','" + TransactionTime + "','" + PaymentDate + "','" + SrcItc
                        + "','" + Convert.ToInt32(HSRP_StateID) + "','" + Convert.ToInt32(Id) + "'," + "'" + Convert.ToInt32(RTOLocation_ID) + "'," + UploadedDateTime + ",'" +
                        Convert.ToString(orderno) + "');");





                        countupload = countupload + 1;

                    }



                }
            if (sb.ToString() != "")
            {

                Utils.ExecNonQuery(sb.ToString(), CnnStringupload);

            }
            if (countupload > 0)
            {
                llbMSGSuccess.Text = "";
                llbMSGError.Text = "";
                llbMSGSuccess.Text = "Record Save Sucessfully. Your Order No : " + orderno;
                lbltotaluploadrecords.Text = countupload.ToString();
                lbltotladuplicaterecords.Text = countDuplicate.ToString();


            }
            else
            {


                lbltotladuplicaterecords.Text = countDuplicate.ToString();

            }
        }





            
         //private void ValidationCheckOnRecords(DataTable ExcelSheet)
         //  {


         //   for (int i = 0; i < ExcelSheet.Rows.Count; i++)
         //   {
              
              
         //       if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["Bank Id"].ToString().Trim()))
         //           {

         //               llbMSGError.Text = "";
         //               llbMSGError.Text = "Excel Sheet : Has <b>Bank id</b> Field Empty At Row : ";
         //               FlagIsDirty = true;
         //               return;
         //           }


         //       if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["Bank Name"].ToString().Trim()))
         //           {

         //               llbMSGError.Text = "";
         //               llbMSGError.Text = "Excel Sheet : Has <b>Bank name</b> Field Empty At Row : ";
         //               FlagIsDirty = true;
         //               return;
         //           }


         //          if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[j]["TPSL Transaction id"].ToString().Trim()))
         //           {

         //               llbMSGError.Text = "";
         //               llbMSGError.Text = "Excel Sheet : Has <b>TPSL Transaction id</b> Field Empty At Row : ";
         //               FlagIsDirty = true;
         //               return;
         //           }


         //           if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["Sm Transaction Id"].ToString().Trim()))
         //           {

         //               llbMSGError.Text = "";
         //               llbMSGError.Text = "Excel Sheet : Has <b>Sm Transaction Id</b> Field Empty At Row : ";
         //               FlagIsDirty = true;
         //               return;
         //           }

         //        if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["Bank Transaction id"].ToString().Trim()))
         //           {

         //               llbMSGError.Text = "";
         //               llbMSGError.Text = "Excel Sheet : Has <b>Bank Transaction id</b> Field Empty At Row : ";
         //               FlagIsDirty = true;
         //               return;
         //           }
                


         //           if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["Total Amount"].ToString().Trim()))
         //           {

         //               llbMSGError.Text = "";
         //               llbMSGError.Text = "Excel Sheet : Has <b>Total Amount</b> Field Empty At Row : ";
         //               FlagIsDirty = true;
         //               return;
         //           }


         //           if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["Net Amount"].ToString().Trim()))
         //           {

         //               llbMSGError.Text = "";
         //               llbMSGError.Text = "Excel Sheet : Has <b>Net Amount</b> Field Empty At Row : ";
         //               FlagIsDirty = true;
         //               return;
         //           }

         //           if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["Transaction date"].ToString().Trim()))
         //           {

         //               llbMSGError.Text = "";
         //               llbMSGError.Text = "Excel Sheet : Has <b>Transaction date</b> Field Empty At Row : ";
         //               FlagIsDirty = true;
         //               return;
         //           }


         //           if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["Transaction time"].ToString().Trim()))
         //           {

         //               llbMSGError.Text = "";
         //               llbMSGError.Text = "Excel Sheet : Has <b>Transaction time</b> Field Empty At Row : ";
         //               FlagIsDirty = true;
         //               return;
         //           }





         //           if (string.IsNullOrWhiteSpace(ExcelSheet.Rows[i]["SRC ITC"].ToString().Trim()))
                    

         //               llbMSGError.Text = "";
         //               llbMSGError.Text = "Excel Sheet : Has <b>SRC ITC</b> Field Empty At Row : ";
         //               FlagIsDirty = true;
         //               return;
         //           }


         //   }
    
        







    }
}