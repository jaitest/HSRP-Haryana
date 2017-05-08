using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;

namespace HSRP.WCFService
{
    [ServiceContract(Namespace = "")]
   // [ServiceBehavior(InstanceContextMode=InstanceContextMode.PerSession)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ServiceForSuggestion
    {
        // To use HTTP GET, add [WebGet] attribute. (Default ResponseFormat is WebMessageFormat.Json)
        // To create an operation that returns XML,
        //     add [WebGet(ResponseFormat=WebMessageFormat.Xml)],
        //     and include the following line in the operation body:
        //         WebOperationContext.Current.OutgoingResponse.ContentType = "text/xml";
        //[System.ServiceModel.Web.WebInvoke(Method = "POST",ResponseFormat = System.ServiceModel.Web.WebMessageFormat.Json)]
        
        [OperationContract]
        public List<string> GetCustomers(string prefixText, int count)
        {
            
            List<string> customers = new List<string>();
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "select top(10) HSRPRecord_AuthorizationNo, HSRPRecordStaggingID from HSRPRecordsStaggingArea where " +
                    "HSRPRecord_AuthorizationNo like @prefix + '%' and OrderStatus='New'";
                    cmd.Parameters.AddWithValue("@prefix", prefixText);
                    cmd.Connection = conn;
                    conn.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(sdr["HSRPRecord_AuthorizationNo"].ToString(), sdr["HSRPRecordStaggingID"].ToString());
                            customers.Add(item);
                        }
                    }
                    conn.Close();
                }
                return customers;
            }
        }

        [OperationContract]
        public List<string> GetCustomers1(string prefixText, int count, string contextKey)
        {
            string []StateAndRto = contextKey.Split('^');
            string splt = StateAndRto[0].Replace('/', ',');
            List<string> customers = new List<string>();
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                using (SqlCommand cmd = new SqlCommand())
                {
                    if (StateAndRto[1]=="5")
                    {
                        cmd.CommandText = "select top(10) HSRPRecord_AuthorizationNo,'' as HSRPRecordStaggingID from View_MP_HSRPRecordsStagging_New where " +
                        "HSRPRecord_AuthorizationNo like @prefix + '%'";
                    }
                    else
                    {
                        cmd.CommandText = "select top(10) HSRPRecord_AuthorizationNo, HSRPRecordStaggingID from HSRPRecordsStaggingArea where hsrp_stateid='"+ StateAndRto[1].ToString() +"' and " +
                        "HSRPRecord_AuthorizationNo like @prefix + '%' and OrderStatus='New'";
                       // cmd.CommandText = "select top(10) HSRPRecord_AuthorizationNo, HSRPRecordStaggingID from HSRPRecordsStaggingArea where RTOLocationID in (" + splt + ") and " +
                       //"HSRPRecord_AuthorizationNo like @prefix + '%' and OrderStatus='New'";
                    }

                    
                    cmd.Parameters.AddWithValue("@prefix", prefixText);
                    cmd.Connection = conn;
                    conn.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(sdr["HSRPRecord_AuthorizationNo"].ToString(), sdr["HSRPRecordStaggingID"].ToString());
                            customers.Add(item);
                        }
                    }
                    conn.Close();
                }
                return customers;
            }
        }

        [OperationContract]
        public List<string> GetVehicleCashinHandRegsNo(string prefixText, int count, string contextKey)
        {

            List<string> customers = new List<string>();
            string splt = contextKey;
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "select top(10) VehicleRegNo, HSRPRecordStaggingID from HSRPRecordsStaggingArea where hsrp_StateID ='" + splt + "' and orderStatus='New' and  " +
                    "VehicleRegNo like @prefix + '%'";

                    cmd.Parameters.AddWithValue("@prefix", prefixText);
                    cmd.Connection = conn;
                    conn.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(sdr["VehicleRegNo"].ToString(), sdr["HSRPRecordStaggingID"].ToString());
                            customers.Add(item);
                        }
                    }
                    conn.Close();
                }
                return customers;
            }
        }
        [OperationContract]
        public List<string> GetCustomers1Edit(string prefixText, int count, string contextKey)
        {
            string splt = contextKey.Replace('/', ',');
            List<string> customers = new List<string>();
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "select top(10) HSRPRecord_AuthorizationNo,HSRPRecordID as HSRPRecordStaggingID from HSRPRecords where RTOLocationID in ("+splt+") and " +
                    "HSRPRecord_AuthorizationNo like @prefix + '%' and OrderStatus='New Order'";
                    cmd.Parameters.AddWithValue("@prefix", prefixText);
                    cmd.Connection = conn;
                    conn.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(sdr["HSRPRecord_AuthorizationNo"].ToString(), sdr["HSRPRecordStaggingID"].ToString());
                            customers.Add(item);
                        }
                    }
                    conn.Close();
                }
                return customers;
            }
        }

        [OperationContract]
        public List<string> GetVehicleRegs(string prefixText, int count, string contextKey)
        {

            List<string> customers = new List<string>();
            string[] StateAndRto = contextKey.Split('^');
            string splt = StateAndRto[0].Replace('/', ',');
            using (SqlConnection conn = new SqlConnection())
            {
                if (StateAndRto[1]=="5")
	                {
                        conn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
	                }
                else
                {
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                }
                using (SqlCommand cmd = new SqlCommand())
                {
                    if (StateAndRto[1] == "5")
                    {
                        cmd.CommandText = "select top(10) VehicleRegNo,'' as HSRPRecordStaggingID from View_MP_HSRPRecordsStagging_New where " +
                        "vehicleregno like @prefix + '%'";
                    }
                    else
                    {
                        cmd.CommandText = "select top(10) VehicleRegNo, HSRPRecordStaggingID from HSRPRecordsStaggingArea where  hsrp_stateid='"+ StateAndRto[1].ToString() +"'  and " +
                        "VehicleRegNo like @prefix + '%' and OrderStatus='New'";
                    }
                    

                    cmd.Parameters.AddWithValue("@prefix", prefixText);
                    cmd.Connection = conn;
                    conn.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(sdr["VehicleRegNo"].ToString(), sdr["HSRPRecordStaggingID"].ToString());
                            customers.Add(item);
                        }
                    }
                    conn.Close();
                }
                return customers;
            }
        }

        [OperationContract]
        public List<string> GetVehicleRegsEdit(string prefixText, int count, string contextKey)
        {

            List<string> customers = new List<string>();
            string[] StateAndRto = contextKey.Split('^');
            string splt = contextKey.Replace('^', ',');
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                using (SqlCommand cmd = new SqlCommand())
                {
                    //cmd.CommandText = "select top(10) VehicleRegNo, HSRPRecordID as HSRPRecordStaggingID from HSRPRecords where  RTOLocationID in ("+splt+") and  " +
                    //"VehicleRegNo like @prefix + '%' and OrderStatus='New Order'";

                    cmd.CommandText = "select top(10) VehicleRegNo, HSRPRecordID as HSRPRecordStaggingID from HSRPRecords where  hsrp_stateid ='"+ StateAndRto[1].ToString() +"' and OrderStatus='New Order' and  " +
                    "VehicleRegNo like @prefix + '%' ";

                    cmd.Parameters.AddWithValue("@prefix", prefixText);
                    cmd.Connection = conn;
                    conn.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(sdr["VehicleRegNo"].ToString(), sdr["HSRPRecordStaggingID"].ToString());
                            customers.Add(item);
                        }
                    }
                    conn.Close();
                }
                return customers;
            }
        }
        
        [OperationContract]
        public List<string> GetCashReceiptNos(string prefixText, int count)
        {

            List<string> customers = new List<string>();
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "select top(5) CashReceiptNo, HSRPRecordStaggingID from HSRPRecordsStaggingArea where " +
                    "CashReceiptNo like @prefix + '%' and OrderStatus='New'";
                    


                    cmd.Parameters.AddWithValue("@prefix", prefixText);
                    cmd.Connection = conn;
                    conn.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(sdr["CashReceiptNo"].ToString(), sdr["HSRPRecordStaggingID"].ToString());
                            customers.Add(item);
                        }
                    }
                    conn.Close();
                }
                return customers;
            }
        }
        [OperationContract]
        public List<string> LaserNo(string prefixText, int count, string contextKey)
        {
            string[] context = contextKey.Split('/');
            string StateID = context[0];
            string FrontPlateSize = context[1];
            string RearPlateSize = context[2];

            List<string> customers = new List<string>();
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "select top(10) LaserNo, InventoryID from RTOInventory where HSRP_StateID='" + StateID + "' and InventoryStatus='New Order' and ProductID='" + FrontPlateSize + "' and " +
                    "LaserNo like '%' + @prefix + '%'";
                    cmd.Parameters.AddWithValue("@prefix", prefixText);
                    cmd.Connection = conn;
                    conn.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(sdr["LaserNo"].ToString(), sdr["InventoryID"].ToString());
                            customers.Add(item);
                        }
                    }
                    conn.Close();
                }
                return customers;
            }
        }
        // Add more operations here and mark them with [OperationContract]

        [OperationContract]
        public List<string> RearLaserNo(string prefixText, int count, string contextKey)
        {
            string[] context = contextKey.Split('/');
            string StateID = context[0];
            string FrontPlateSize = context[1];
            string RearPlateSize = context[2];

            List<string> customers = new List<string>();
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "select top(10) LaserNo, InventoryID from RTOInventory where  HSRP_StateID='" + StateID + "' and InventoryStatus='New Order' and ProductID='" + RearPlateSize + "' and " +
                    "LaserNo like '%' + @prefix + '%'";
                    cmd.Parameters.AddWithValue("@prefix", prefixText);
                    cmd.Connection = conn;
                    conn.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(sdr["LaserNo"].ToString(), sdr["InventoryID"].ToString());
                            customers.Add(item);
                        }
                    }
                    conn.Close();
                }
                return customers;
            }
        }

        /// <summary>
        ///  This Laser No is use for Laser Quick Assign Scrieen, This Laser No is without StateID & RTOLocation.
        /// </summary>
        /// <param name="prefixText"></param>
        /// <param name="count"></param>
        /// <param name="contextKey"></param>
        /// <returns></returns>
        [OperationContract]
        public List<string> VehicleRegNoQuick(string prefixText, int count, string contextKey)
        {
            string[] context = contextKey.Split('/');
            string StateID = context[0];


            List<string> customers = new List<string>();
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "select top 10 VehicleRegNo,RecordID from  LaserAssignQuick where hsrp_StateID='" + StateID + "' and orderstatus='Embossing Done' and " +
                    "VehicleRegNo like '%' + @prefix + '%'";
                    cmd.Parameters.AddWithValue("@prefix", prefixText);
                    cmd.Connection = conn;
                    conn.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(sdr["VehicleRegNo"].ToString(), sdr["RecordID"].ToString());
                            customers.Add(item);
                        }
                    }
                    conn.Close();
                }
                return customers;
            }
        }
        /// <summary>
        ///  This Laser No is use for Laser Quick Assign Scrieen, This Laser No is without StateID & RTOLocation.
        /// </summary>
        /// <param name="prefixText"></param>
        /// <param name="count"></param>
        /// <param name="contextKey"></param>
        /// <returns></returns>
        [OperationContract]

        public List<string> LaserNoQuick(string prefixText, int count, string contextKey)
        {
            string[] context = contextKey.Split('/');
            string StateID = context[0];
            List<string> customers = new List<string>();
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "select top(10) LaserNo, InventoryID from RTOInventory where  hsrp_StateID='" + StateID + "' and InventoryStatus='New Order' and " +
                    "LaserNo like '%' + @prefix + '%'";
                    cmd.Parameters.AddWithValue("@prefix", prefixText);
                    cmd.Connection = conn;
                    conn.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(sdr["LaserNo"].ToString(), sdr["InventoryID"].ToString());
                            customers.Add(item);
                        }
                    }
                    conn.Close();
                }
                return customers;
            }
        }

    }
}
