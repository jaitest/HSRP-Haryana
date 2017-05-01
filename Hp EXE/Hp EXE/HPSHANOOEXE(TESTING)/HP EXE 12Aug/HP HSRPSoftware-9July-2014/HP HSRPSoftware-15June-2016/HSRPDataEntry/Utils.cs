using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Web;
using iTextSharp.text.html.simpleparser;
using System.Text;

//using Excel = Microsoft.Office.Interop.Excel;
//using Microsoft.Office.Interop.Excel;

namespace HSRPTransferData
{
    class utils
    {
        static string ConnectionStringAPP = string.Empty;
        //string appPath = Path.GetDirectoryName(Application.ExecutablePath) + "tels.ini";
         static string path = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "tels.ini";
         static string strDataSource, strUID, strPWD, strInitialCatalog, strIntegratedSecurity;
         public static string RTOlocationID = string.Empty;
         public static string MacBase = string.Empty;
         public static string UserID = string.Empty;




         public static string GetLocalDBConnectionFromINI()
         {



             HSRPDataEntryNew.clsINI ini = new HSRPDataEntryNew.clsINI(path);
           
             strDataSource = ini.IniReadValue("DATABASE", "DataSource"); //.\SQLExpress
             strUID = ini.IniReadValue("DATABASE", "UID");
             strPWD = ini.IniReadValue("DATABASE", "PWD");
             strInitialCatalog = ini.IniReadValue("DATABASE", "InitialCatalog");
             strIntegratedSecurity = ini.IniReadValue("DATABASE", "IntegratedSecurity");

        
             ConnectionStringAPP = "Data Source=" + strDataSource + ";Initial Catalog=" + strInitialCatalog + ";Integrated Security=True";

            
          // ConnectionStringAPP = "Data Source=" + strDataSource + ";Initial Catalog=" + strInitialCatalog + ";UID=" + strUID + ";PWD=" + strPWD;
             
             return ConnectionStringAPP;
         }






         public static string GetVahanDBConnectionFromINI()
         {


             HSRPDataEntryNew.clsINI ini = new HSRPDataEntryNew.clsINI(path);

             strDataSource = ini.IniReadValue("VAHANDATABASE", "DataSource");
            // strDataSource = ".";
             strUID = ini.IniReadValue("VAHANDATABASE", "UID");
             strPWD = ini.IniReadValue("VAHANDATABASE", "PWD");
             strInitialCatalog = ini.IniReadValue("VAHANDATABASE", "InitialCatalog");
             strIntegratedSecurity = ini.IniReadValue("VAHANDATABASE", "IntegratedSecurity");

             ConnectionStringAPP = "Data Source=" + strDataSource + ";Initial Catalog=" + strInitialCatalog + ";Integrated Security=True";
             // ConnectionStringAPP = "Data Source=" + strDataSource + ";Initial Catalog=" + strInitialCatalog + ";UID=" + strUID + ";PWD=" + strPWD;
             return ConnectionStringAPP;
         }


        #region Commented code 
        //public static string GetLocalDBConnectionFromINI()
        //{



        //    HSRPDataEntryNew.clsINI ini = new HSRPDataEntryNew.clsINI(path);

        //    strDataSource = ini.IniReadValue("DATABASE", "DataSource");
        //    strUID = ini.IniReadValue("DATABASE", "UID");
        //    strPWD = ini.IniReadValue("DATABASE", "PWD");
        //    strInitialCatalog = ini.IniReadValue("DATABASE", "InitialCatalog");
        //    strIntegratedSecurity = ini.IniReadValue("DATABASE", "IntegratedSecurity");

        //    ConnectionStringAPP = "Data Source=" + strDataSource + ";Initial Catalog=" + strInitialCatalog + ";Integrated Security=True";
        //    return ConnectionStringAPP;
        //}






        //public static string GetVahanDBConnectionFromINI()
        //{


        //    HSRPDataEntryNew.clsINI ini = new HSRPDataEntryNew.clsINI(path);

        //    strDataSource = ini.IniReadValue("VAHANDATABASE", "DataSource");
        //    strUID = ini.IniReadValue("VAHANDATABASE", "UID");
        //    strPWD = ini.IniReadValue("VAHANDATABASE", "PWD");
        //    strInitialCatalog = ini.IniReadValue("VAHANDATABASE", "InitialCatalog");
        //    strIntegratedSecurity = ini.IniReadValue("VAHANDATABASE", "IntegratedSecurity");

        //    ConnectionStringAPP = "Data Source=" + strDataSource + ";Initial Catalog=" + strInitialCatalog + ";Integrated Security=True";
        //    return ConnectionStringAPP;
         //}
        #endregion Commented code



         public string strProvider = GetLocalDBConnectionFromINI();

        public static string getCnnHSRPApp = GetLocalDBConnectionFromINI();

        public static string getCnnHSRPVahan = GetVahanDBConnectionFromINI();
        //public string strProvider = System.Configuration.ConfigurationSettings.AppSettings["ConnectionStringAPP"].ToString();
        //public static string getCnnHSRPApp = System.Configuration.ConfigurationSettings.AppSettings["ConnectionStringAPP"].ToString();

        //public static string getCnnHSRPVahan = System.Configuration.ConfigurationSettings.AppSettings["ConnectionStringHSRP_VAHAN"].ToString();

        public string sqlText;
        public int CommandTimeOut = 0;
        public SqlConnection objConnection;




        public static string CashRecieptPrinterName()
        {

            string ReturnValue = string.Empty;
            using (SqlConnection conn = new SqlConnection(getCnnHSRPApp))
            {
                SqlCommand cmd = new SqlCommand("select CashReceiptPrinterName from APP_Parameters", conn);
                cmd.CommandType = CommandType.Text;
                conn.Open();
                ReturnValue = (string)cmd.ExecuteScalar().ToString();

            }
            return ReturnValue;
        }

        public static string CashRecieptPrinterType()
        {

            string ReturnValue = string.Empty;
            using (SqlConnection conn = new SqlConnection(getCnnHSRPApp))
            {
                SqlCommand cmd = new SqlCommand("select CashPrinterType from APP_Parameters", conn);
                cmd.CommandType = CommandType.Text;
                conn.Open();
                ReturnValue = (string)cmd.ExecuteScalar().ToString();

            }
            return ReturnValue;
        }
        public static string ThridStickerPrinterType()
        {

            string ReturnValue = string.Empty;
            using (SqlConnection conn = new SqlConnection(getCnnHSRPApp))
            {
                SqlCommand cmd = new SqlCommand("select ThirdStickerPrinterType from APP_Parameters", conn);
                cmd.CommandType = CommandType.Text;
                conn.Open();
                ReturnValue = (string)cmd.ExecuteScalar().ToString();

            }
            return ReturnValue;
        }
        public static string ThridStickerPrinterName()
        {

            string ReturnValue = string.Empty;
            using (SqlConnection conn = new SqlConnection(getCnnHSRPApp))
            {
                SqlCommand cmd = new SqlCommand("select ThirdStickerPrinterName from APP_Parameters", conn);
                cmd.CommandType = CommandType.Text;
                conn.Open();
                ReturnValue = (string)cmd.ExecuteScalar().ToString();

            }
            return ReturnValue;
        }



        public static string getStateId()
        {

            string ReturnValue = string.Empty;
            using (SqlConnection conn = new SqlConnection(getCnnHSRPApp))
            {
                SqlCommand cmd = new SqlCommand("select stateid from APP_Parameters", conn);
                cmd.CommandType = CommandType.Text;
                conn.Open();
                ReturnValue = (string)cmd.ExecuteScalar().ToString();

            }
            return ReturnValue;
        }


        public static string getStateName()
        {

            string ReturnValue = string.Empty;
            using (SqlConnection conn = new SqlConnection(getCnnHSRPApp))
            {
                SqlCommand cmd = new SqlCommand("select StateName from APP_Parameters", conn);
                cmd.CommandType = CommandType.Text;
                conn.Open();
                ReturnValue = (string)cmd.ExecuteScalar().ToString();

            }
            return ReturnValue;
        }

        public static string getRtoLocationCode()
        {

            string ReturnValue = string.Empty;
            using (SqlConnection conn = new SqlConnection(getCnnHSRPApp))
            {
                SqlCommand cmd = new SqlCommand("select RTOCode from APP_Parameters", conn);
                cmd.CommandType = CommandType.Text;
                conn.Open();
                ReturnValue = (string)cmd.ExecuteScalar().ToString();

            }
            return ReturnValue;
        }

        public static string getCompanyName()
        {

            string ReturnValue = string.Empty;
            using (SqlConnection conn = new SqlConnection(getCnnHSRPApp))
            {
                SqlCommand cmd = new SqlCommand("select CompanyName from APP_Parameters", conn);
                cmd.CommandType = CommandType.Text;
                conn.Open();
                ReturnValue = (string)cmd.ExecuteScalar().ToString();

            }
            return ReturnValue;
        }

        public static string getRTOLocationAddress()
        {

            string ReturnValue = string.Empty;
            using (SqlConnection conn = new SqlConnection(getCnnHSRPApp))
            {
                SqlCommand cmd = new SqlCommand("select RTOLocationAddress from APP_Parameters", conn);
                cmd.CommandType = CommandType.Text;
                conn.Open();
                ReturnValue = (string)cmd.ExecuteScalar().ToString();

            }
            return ReturnValue;
        }

        public SqlDataReader GetReader()
        {
            SqlDataReader dr;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = 0;
            MakeConnection();
            OpenConnection();
            cmd = GetCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 0;

            dr = cmd.ExecuteReader();
            return dr;
        }
        SqlCommand GetCommand()
        {
            return new SqlCommand(sqlText, objConnection);
        }
        public void MakeConnection()
        {

            objConnection = new SqlConnection(strProvider);

        }
        public void OpenConnection()
        {
            objConnection.Open();
        }
        public void CloseConnection()
        {
            objConnection.Close();
        }

        public static int getScalarCount(string SQLString, string CnnString)
        {
            int ReturnValue = 0;
            using (SqlConnection conn = new SqlConnection(CnnString))
            {
                SqlCommand cmd = new SqlCommand(SQLString, conn);
                cmd.CommandType = CommandType.Text;
                conn.Open();
                ReturnValue = (int)cmd.ExecuteScalar();
                conn.Close();
            }
            return ReturnValue;
        }

        public static string getScalarValue(string SQLString, string CnnString)
        {

            string ReturnValue = string.Empty;
            using (SqlConnection conn = new SqlConnection(CnnString))
            {
                SqlCommand cmd = new SqlCommand(SQLString, conn);
                cmd.CommandType = CommandType.Text;
                conn.Open();
                ReturnValue = (string)cmd.ExecuteScalar();
            }
            return ReturnValue;
        }

        public static int ExecNonQuery(string SQLString, string CnnString)
        {
            int count = 0;
            using (SqlConnection connection = new SqlConnection(CnnString))
            {
                SqlCommand command = new SqlCommand(SQLString, connection);
                command.CommandType = CommandType.Text;
                command.Connection.Open();
                count = command.ExecuteNonQuery();
                command.Connection.Close();
            }
            return count;
        }
        public static bool CheckAndCreateFolderForReport(string ReportFolderPath)
        {
            bool ReturnVal = false;

            if (System.IO.Directory.Exists(ReportFolderPath))
            {
                ReturnVal = true;
            }
            else
            {

                System.IO.Directory.CreateDirectory(ReportFolderPath);
                ReturnVal = true;
            }


            return ReturnVal;

        }


        public static DataSet getDataSet(string SQLString, string CnnString)
        {
            SqlConnection Conn = new SqlConnection(CnnString);
            SqlDataAdapter DA = new SqlDataAdapter(SQLString, Conn);
            DA.SelectCommand.CommandTimeout = 0;
            Conn.Open();
            DataSet ReturnDs = new DataSet();
            DA.Fill(ReturnDs, "Table1");
            Conn.Close();
            return ReturnDs;
        }
        public static string getDataSingleValue(string SQLString, string CnnString, string colname)
        {
            string SingleValue = "";
            SqlDataReader reader = null;
            try
            {
                utils dbLink = new utils();
                dbLink.strProvider = CnnString.ToString();
                dbLink.CommandTimeOut = 0;
                dbLink.sqlText = SQLString.ToString();
                reader = dbLink.GetReader();

                while (reader.Read())
                {
                    SingleValue = reader[colname].ToString();
                }
                // Call Close when done reading.
                // reader.Close();
                if (SingleValue == "")
                {
                    SingleValue = "0";
                }
                reader.Close();
                dbLink.CloseConnection();

                return SingleValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                reader.Close();
            }
        }
        public static DataTable GetDataTableVahan(string SQLString, string CnnString)
        {
            utils dbLink = new utils();
            dbLink.strProvider = CnnString.ToString();
            dbLink.CommandTimeOut = 0;
            dbLink.sqlText = SQLString.ToString();
            SqlDataReader dr = dbLink.GetReader();
            DataTable tb = new DataTable();
            tb.Load(dr);
            dr.Close();
            dbLink.CloseConnection();
            return tb;

        }
        public static DataTable GetDataTable(string SQLString, string CnnString)
        {
            utils dbLink = new utils();
            dbLink.strProvider = CnnString.ToString();
            dbLink.CommandTimeOut = 0;
            dbLink.sqlText = SQLString.ToString();
            SqlDataReader dr = dbLink.GetReader();
            DataTable tb = new DataTable();
            tb.Load(dr);
            dr.Close();
            dbLink.CloseConnection();
            return tb;

        }

        public static void ExportToPdf(DataTable ExDataTable) //Datatable 
        {
            //Here set page size as A4

            //Document pdfDoc = new Document(PageSize.A4, 10, 10, 10, 10);
            Document pdfDoc = new Document(new iTextSharp.text.Rectangle(188f, 124f), -30, -30, 8, 0);
            pdfDoc.SetPageSize(iTextSharp.text.PageSize.A4);
            string filename = "Report" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".pdf";

            try
            {
                string PdfFolder = "D:\\PdfFile\\" + filename;
                PdfWriter.GetInstance(pdfDoc, new FileStream(PdfFolder, FileMode.Create));
                //PdfWriter.GetInstance(pdfDoc, System.Web.HttpContext.Current.Response.OutputStream);
                pdfDoc.Open();

                //Set Font Properties for PDF File
                Font fnt = FontFactory.GetFont("Times New Roman", 8);
                DataTable dt = ExDataTable;

                if (dt != null)
                {

                    PdfPTable PdfTable = new PdfPTable(dt.Columns.Count);
                    PdfPCell PdfPCell = null;

                    //Here we create PDF file tables

                    for (int rows = 0; rows < dt.Rows.Count; rows++)
                    {
                        if (rows == 0)
                        {
                            for (int column = 0; column < dt.Columns.Count; column++)
                            {
                                PdfPCell = new PdfPCell(new Phrase(new Chunk(dt.Columns[column].ColumnName.ToString(), fnt)));
                                PdfTable.AddCell(PdfPCell);
                            }
                        }
                        for (int column = 0; column < dt.Columns.Count; column++)
                        {
                            PdfPCell = new PdfPCell(new Phrase(new Chunk(dt.Rows[rows][column].ToString(), fnt)));
                            PdfTable.AddCell(PdfPCell);

                        }
                    }

                    // Finally Add pdf table to the document 
                    pdfDoc.Add(PdfTable);
                    MessageBox.Show("Report Created in D:\\PdfFile\\ ");
                }

                pdfDoc.Close();
                //HttpContext context = HttpContext.Current;

                //context.Response.ContentType = "Application/pdf";
                //context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                //context.Response.WriteFile(PdfFolder);
                //context.Response.End();
                //System.Web.HttpContext.Current.Response.ContentType = "application/pdf";

                //Set default file Name as current datetime
                //System.Web.HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename="+"Report"+ DateTime.Now.ToString("yyyyMMdd") + ".pdf");

                //System.Web.HttpContext.Current.Response.Write(pdfDoc);

                //System.Web.HttpContext.Current.Response.Flush();
                //System.Web.HttpContext.Current.Response.End();


            }
            catch (Exception ex)
            {
                System.Web.HttpContext.Current.Response.Write(ex.ToString());
            }
        }

        public static void ExportToExcel(DataTable dt)
        {

            Microsoft.Office.Interop.Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();
            ExcelApp.Application.Workbooks.Add(Type.Missing);
            ExcelApp.Columns.ColumnWidth = 20;
            
                //ExcelApp.Cells[0, 3] ="Report";
                //ExcelApp.Cells[1, 2] = "State :";
                //ExcelApp.Cells[1, 3] = "Himachal Pradesh";
                //ExcelApp.Cells[2, 2] = "Date Generated From :";
                //ExcelApp.Cells[2, 3] = datefrom;
                //ExcelApp.Cells[2, 4] = "Date Generated To :";
                //ExcelApp.Cells[2, 5] = dateto;


            for (int i = 0; i < dt.Columns.Count; i++)
            {
                ExcelApp.Cells[1, i+1] = dt.Columns[i].ColumnName;
            }
            for (int i = 0; i < dt.Rows.Count ; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    ExcelApp.Cells[i + 2, j + 1] = dt.Rows[i][j].ToString();
                }
            }
            string ReportFolderPath = System.Configuration.ConfigurationSettings.AppSettings["EXLFileFolder"].ToString();
            bool folderExists = utils.CheckAndCreateFolderForReport(ReportFolderPath);
            if (folderExists)
            {
                ExcelApp.ActiveWorkbook.SaveCopyAs(ReportFolderPath + "DailyReport-" + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + ".xls");
                ExcelApp.ActiveWorkbook.Saved = true;
                ExcelApp.Quit();
                MessageBox.Show("File Created In Path : " + ReportFolderPath.ToString());
                return;
            }
            else
            {
                MessageBox.Show("Error In file Creation Check Path : " + ReportFolderPath.ToString());
                return;
            }
        }

    }
}
