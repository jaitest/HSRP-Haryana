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

       



    }
}
