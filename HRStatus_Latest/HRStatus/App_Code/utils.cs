using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.IO;

/// <summary>
/// Summary description for utils
/// </summary>
public class utils
{
        public string strProvider = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        public string sqlText;
        public int CommandTimeOut = 0;
        public SqlConnection objConnection;
        static string path = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "Ctrls.ini";
        static string Servername = string.Empty;
        static string Database = string.Empty;
        static string Userid = string.Empty;
        static string Password = string.Empty;




        public SqlDataReader GetReader()
        {
            SqlDataReader dr;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = 0;
            MakeConnection();
            OpenConnection();
            cmd = GetCommand();
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
                conn.Open();
                ReturnValue = Convert.ToString(cmd.ExecuteScalar());
                conn.Close();
            }

            return ReturnValue;
        }

        public static int ExecNonQuery(string SQLString, string CnnString)
        {
            int count = 0;
            using (SqlConnection connection = new SqlConnection(CnnString))
            {
                SqlCommand command = new SqlCommand(SQLString, connection);
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
        public static DataTable GetDataTable(string SQLString, string CnnStringx)
        {
            utils dbLink = new utils();
            dbLink.strProvider = CnnStringx.ToString();
            dbLink.CommandTimeOut = 0;
            dbLink.sqlText = SQLString.ToString();
            SqlDataReader dr = dbLink.GetReader();
            DataTable tb = new DataTable();
            tb.Load(dr);
            dr.Close();
            dbLink.CloseConnection();
            return tb;

        }
    }