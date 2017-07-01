using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CarlosAg.ExcelXmlWriter;
using System.Data.SqlClient;


namespace HSRPDataEntryNew
{
    public partial class Backupdata : Form
    {
        public Backupdata()
        {
            InitializeComponent();
        }

        string strQuery = string.Empty;
        DataTable dtResult = new DataTable();
        string strFilename = string.Empty;
        private void Export(DataTable dt,string strExcelSheet)
        {
            try
            {

                Workbook book = new Workbook();
                // Specify which Sheet should be opened and the size of window by default
                book.ExcelWorkbook.ActiveSheetIndex = 1;
                book.ExcelWorkbook.WindowTopX = 100;
                book.ExcelWorkbook.WindowTopY = 200;
                book.ExcelWorkbook.WindowHeight = 7000;
                book.ExcelWorkbook.WindowWidth = 8000;

                // Some optional properties of the Document
                book.Properties.Author = "HSRP";
                book.Properties.Title = "Collection Summary";
                book.Properties.Created = DateTime.Now;

                #region Fetch Data
                


                #endregion


                // Add some styles to the Workbook

                #region Styles

                WorksheetStyle style = book.Styles.Add("HeaderStyle");

                style.Font.FontName = "Tahoma";
                style.Font.Size = 9;
                style.Font.Bold = false;
                style.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                style.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                style.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                style.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                style.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


                WorksheetStyle style8 = book.Styles.Add("HeaderStyle8");
                style8.Font.FontName = "Tahoma";
                style8.Font.Size = 10;
                style8.Font.Bold = true;
                style8.Alignment.Horizontal = StyleHorizontalAlignment.Right;
                style8.Interior.Color = "#D4CDCD";
                style8.Interior.Pattern = StyleInteriorPattern.Solid;

                WorksheetStyle style5 = book.Styles.Add("HeaderStyle5");
                style5.Font.FontName = "Tahoma";
                style5.Font.Size = 10;
                style5.Font.Bold = false;
                style5.Alignment.Horizontal = StyleHorizontalAlignment.Right;
                style5.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                style5.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                style5.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                style5.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


                WorksheetStyle style6 = book.Styles.Add("HeaderStyle6");
                style6.Font.FontName = "Tahoma";
                style6.Font.Size = 10;
                style6.Font.Bold = true;
                style6.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                style6.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                style6.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                style6.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                style6.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);

                WorksheetStyle styleHeader = book.Styles.Add("HeaderStyleHeader");
                styleHeader.Font.FontName = "Tahoma";
                styleHeader.Interior.Color = "Red";
                styleHeader.Font.Size = 10;
                styleHeader.Font.Bold = true;
                styleHeader.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                styleHeader.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                styleHeader.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                styleHeader.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                styleHeader.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


                WorksheetStyle style2 = book.Styles.Add("HeaderStyle2");
                style2.Font.FontName = "Tahoma";
                style2.Font.Size = 10;
                style2.Font.Bold = true;
                style.Alignment.Horizontal = StyleHorizontalAlignment.Left;


                WorksheetStyle style3 = book.Styles.Add("HeaderStyle3");
                style3.Font.FontName = "Tahoma";
                style3.Font.Size = 12;
                style3.Font.Bold = true;
                style3.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                WorksheetStyle style9 = book.Styles.Add("HeaderStyle9");
                style9.Font.FontName = "Tahoma";
                style9.Font.Size = 10;
                style9.Font.Bold = true;
                style9.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                style9.Interior.Color = "#FCF6AE";
                style9.Interior.Pattern = StyleInteriorPattern.Solid;
                #endregion

                Worksheet sheet = book.Worksheets.Add("GovtBackUpDataOfHP");

                #region UpperPart of Excel
                AddColumnToSheet(sheet, 100, dt.Columns.Count);
                int iIndex = 3;
                WorksheetRow row = sheet.Table.Rows.Add();
                row.Index = iIndex++;
                //  row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                row.Cells.Add(new WorksheetCell("Report :", "HeaderStyle3"));

                row.Cells.Add(new WorksheetCell("Backup Report", "HeaderStyle3"));



                row = sheet.Table.Rows.Add();
                row.Index = iIndex++;

                AddNewCell(row, "State:", "HeaderStyle2", 1);
                AddNewCell(row, "HIMACHAL PRADESH", "HeaderStyle2", 1);
                row = sheet.Table.Rows.Add();
                row.Index = iIndex++;

               

                AddNewCell(row, "Report Date:", "HeaderStyle2", 1);
                AddNewCell(row, DateTime.Now.ToString("dd/MMM/yyyy"), "HeaderStyle2", 1);
                row = sheet.Table.Rows.Add();

               

                row.Index = iIndex++;
                #endregion

                #region Column Creation and Assign Data
                string RTOColName = string.Empty;
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    AddNewCell(row, dt.Columns[i].ColumnName.ToString(), "HeaderStyleHeader", 1);
                }
                row = sheet.Table.Rows.Add();
                row.Index = iIndex++;
                for (int j = 0; j < dt.Rows.Count; j++)
                {

                    for (int i = 0; i < dt.Columns.Count; i++)
                    {

                       
                            AddNewCell(row, dt.Rows[j][i].ToString(), "HeaderStyle6", 1);
                            
                       
                    }
                    row = sheet.Table.Rows.Add();

                }
                #endregion
                string filename = strExcelSheet + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                book.Save("D:\\"+filename);              
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }
        private static void AddNewCell(WorksheetRow row, string strText, string strStyle, int iCnt)
        {
            for (int i = 0; i < iCnt; i++)
                row.Cells.Add(new WorksheetCell(strText, strStyle));
        }

        private static void AddColumnToSheet(Worksheet sheet, int iWidth, int iCnt)
        {
            for (int i = 0; i < iCnt; i++)
                sheet.Table.Columns.Add(new WorksheetColumn(iWidth));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            strFilename = "HSRP_DTLS";
            strQuery = "select * from HSRP_DTLS";
            dtResult = HSRPTransferData.utils.GetDataTable(strQuery, HSRPTransferData.utils.getCnnHSRPVahan);
            Export(dtResult,strFilename);
            MessageBox.Show("File is Created in D\\drive.........");
           

        }

        private void button2_Click(object sender, EventArgs e)
        {
            strFilename = "HSRP_DTLS_HIST";
            strQuery = "select * from HSRP_DTLS_HIST";
            dtResult = HSRPTransferData.utils.GetDataTable(strQuery, HSRPTransferData.utils.getCnnHSRPVahan);
            Export(dtResult,strFilename);
            MessageBox.Show("File is Created in D\\drive.........");
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            strFilename = "HSRP_DTLS_SMS";
            strQuery = "select * from HSRP_DTLS_SMS";
            dtResult = HSRPTransferData.utils.GetDataTable(strQuery, HSRPTransferData.utils.getCnnHSRPVahan);
            Export(dtResult,strFilename);
            MessageBox.Show("File is Created in D\\drive.........");
            
        }
 
    }
}
