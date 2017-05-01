using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace HSRPTransferData
{
    class utils
    {


        static string ConnectionStringAPP = string.Empty;
        //string appPath = Path.GetDirectoryName(Application.ExecutablePath) + "tels.ini";
        static string path = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "tels.ini";
        static string strDataSource, strUID, strPWD, strInitialCatalog, strIntegratedSecurity;


         static string  DataFolder=string.Empty;
         static string DataFileFolder=string.Empty;
         static string EXLFileFolder=string.Empty;
         static string Stateid=string.Empty;
         static string StateName=string.Empty;
        static string  RTOLocationCode=string.Empty;
        static string  CompanyName=string.Empty;
        static string RTOLocationAddress=string.Empty;
        static string ReceiptSizeA4=string.Empty;
        static string PrinterName1 = string.Empty;
        static string location = string.Empty;
        static string ThridStickerPrinterType = string.Empty;
        static string ThridStickerPrinterName = string.Empty;

        public static string GetLocalDBConnectionFromINI()
        {


           HSRPDataEntry.clsINI ini = new HSRPDataEntry.clsINI(path);

            DataFolder = ini.IniReadValue("Location", "DataFolder");
            DataFileFolder = ini.IniReadValue("Location", "DataFileFolder");
            EXLFileFolder = ini.IniReadValue("Location", "EXLFileFolder");
            Stateid = ini.IniReadValue("Location", "Stateid");
            StateName = ini.IniReadValue("Location", "StateName");
            RTOLocationCode = ini.IniReadValue("Location", "RTOLocationCode");
            CompanyName = ini.IniReadValue("Location", "CompanyName");
            RTOLocationAddress = ini.IniReadValue("Location", "RTOLocationAddress");
            ReceiptSizeA4 = ini.IniReadValue("Location", "ReceiptSizeA4");
            PrinterName1 = ini.IniReadValue("Location", "PrinterName");
            ThridStickerPrinterType = ini.IniReadValue("Location", "ThridStickerPrinterType");
            ThridStickerPrinterName = ini.IniReadValue("Location", "ThridStickerPrinterName");
            location = DataFolder + "^" + DataFileFolder + "^" + DataFileFolder + "^" + Stateid + "^" + StateName + "^" + RTOLocationCode + "^" + CompanyName + "^" + RTOLocationAddress + "^" + ReceiptSizeA4 + "^" + PrinterName1 + "^" + ThridStickerPrinterType + "^" + ThridStickerPrinterName;
            return location;
        }

        //public string strProvider = System.Configuration.ConfigurationSettings.AppSettings["ConnectionStringAPP"].ToString();
        //public static string getCnnHSRPApp = System.Configuration.ConfigurationSettings.AppSettings["ConnectionStringAPP"].ToString();
        //public static string getCnnHSRPVahan = System.Configuration.ConfigurationSettings.AppSettings["ConnectionStringHSRP_VAHAN"].ToString();
        //public static string getStateId = System.Configuration.ConfigurationSettings.AppSettings["Stateid"].ToString();
        //public static string getStateName = System.Configuration.ConfigurationSettings.AppSettings["StateName"].ToString();
        //public static string getRtoLocationCode = System.Configuration.ConfigurationSettings.AppSettings["RTOLocationCode"].ToString();
        //public static string getCompanyName = System.Configuration.ConfigurationSettings.AppSettings["CompanyName"].ToString();
        //public static string getRTOLocationAddress = System.Configuration.ConfigurationSettings.AppSettings["RTOLocationAddress"].ToString();
        ////public static string getOldDataURLHP62 = System.Configuration.ConfigurationSettings.AppSettings["OldDataURL-HP62"].ToString();
        ////public static string getOldDataURLHP63 = System.Configuration.ConfigurationSettings.AppSettings["OldDataURL-HP63"].ToString();
        //public static string getPrintSize = System.Configuration.ConfigurationSettings.AppSettings["ReceiptSizeA4"].ToString();
        //public static string PrinterName = System.Configuration.ConfigurationSettings.AppSettings["PrinterName"].ToString();

        //public SqlDataReader GetReader()
        //{
        //    SqlDataReader dr;
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandTimeout = 0;
        //    MakeConnection();
        //    OpenConnection();
        //    cmd = GetCommand();
        //    cmd.CommandTimeout = 0;
            
        //    dr = cmd.ExecuteReader();
        //    return dr;
        //}
        //SqlCommand GetCommand()
        //{
        //    return new SqlCommand(sqlText, objConnection);
        //}
        //public void MakeConnection()
        //{

        //    objConnection = new SqlConnection(strProvider);

        //}
        //public void OpenConnection()
        //{
        //    objConnection.Open();
        //}
        //public void CloseConnection()
        //{
        //    objConnection.Close();
        //}

        //public static int getScalarCount(string SQLString, string CnnString)
        //{
        //    int ReturnValue = 0;
        //    using (SqlConnection conn = new SqlConnection(CnnString))
        //    {
        //        SqlCommand cmd = new SqlCommand(SQLString, conn);

        //        conn.Open();
        //        ReturnValue = (int)cmd.ExecuteScalar();
        //        conn.Close();
        //    }
        //    return ReturnValue;
        //}

        //public static string getScalarValue(string SQLString, string CnnString)
        //{

        //    string ReturnValue = string.Empty;
        //    using (SqlConnection conn = new SqlConnection(CnnString))
        //    {
        //        SqlCommand cmd = new SqlCommand(SQLString, conn);
        //        conn.Open();
        //        ReturnValue = (string)cmd.ExecuteScalar();
        //    }
        //    return ReturnValue;
        //}

        //public static int ExecNonQuery(string SQLString, string CnnString)
        //{
        //    int count = 0;
        //    using (SqlConnection connection = new SqlConnection(CnnString))
        //    {
        //        SqlCommand command = new SqlCommand(SQLString, connection);
        //        command.Connection.Open();
        //        count = command.ExecuteNonQuery();
        //        command.Connection.Close();
        //    }
        //    return count;
        //}
        //public static bool CheckAndCreateFolderForReport(string ReportFolderPath)
        //{
        //    bool ReturnVal = false;

        //    if (System.IO.Directory.Exists(ReportFolderPath))
        //    {
        //        ReturnVal = true;
        //    }
        //    else
        //    {

        //        System.IO.Directory.CreateDirectory(ReportFolderPath);
        //        ReturnVal = true;
        //    }


        //    return ReturnVal;

        //}


        //public static DataSet getDataSet(string SQLString, string CnnString)
        //{
        //    SqlConnection Conn = new SqlConnection(CnnString);
        //    SqlDataAdapter DA = new SqlDataAdapter(SQLString, Conn);
        //    DA.SelectCommand.CommandTimeout = 0;
        //    Conn.Open();
        //    DataSet ReturnDs = new DataSet();
        //    DA.Fill(ReturnDs, "Table1");
        //    Conn.Close();
        //    return ReturnDs;
        //}
        //public static string getDataSingleValue(string SQLString, string CnnString, string colname)
        //{
        //    string SingleValue = "";
        //    SqlDataReader reader = null;
        //    try
        //    {
        //        utils dbLink = new utils();
        //        dbLink.strProvider = CnnString.ToString();
        //        dbLink.CommandTimeOut = 0;
        //        dbLink.sqlText = SQLString.ToString();
        //        reader = dbLink.GetReader();

        //        while (reader.Read())
        //        {
        //            SingleValue = reader[colname].ToString();
        //        }
        //        // Call Close when done reading.
        //        // reader.Close();
        //        if (SingleValue == "")
        //        {
        //            SingleValue = "0";
        //        }
        //        reader.Close();
        //        dbLink.CloseConnection();

        //        return SingleValue;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        reader.Close();
        //    }
        //}
        //public static DataTable GetDataTableVahan(string SQLString, string CnnString)
        //{
        //    utils dbLink = new utils();
        //    dbLink.strProvider = CnnString.ToString();
        //    dbLink.CommandTimeOut = 0;
        //    dbLink.sqlText = SQLString.ToString();
        //    SqlDataReader dr = dbLink.GetReader();
        //    DataTable tb = new DataTable();
        //    tb.Load(dr);
        //    dr.Close();
        //    dbLink.CloseConnection();
        //    return tb;

        //}
        //public static DataTable GetDataTable(string SQLString, string CnnString)
        //{
        //    utils dbLink = new utils();
        //    dbLink.strProvider = CnnString.ToString();
        //    dbLink.CommandTimeOut = 0;
        //    dbLink.sqlText = SQLString.ToString();
        //    SqlDataReader dr = dbLink.GetReader();
        //    DataTable tb = new DataTable();
        //    tb.Load(dr);
        //    dr.Close();
        //    dbLink.CloseConnection();
        //    return tb;

        //}



    }
}
