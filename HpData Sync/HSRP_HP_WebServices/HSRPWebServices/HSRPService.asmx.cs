using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Serialization;
using System.IO;
using System.Data.Sql;
using System.Text;
using System.ComponentModel;
using System.Net;

namespace HSRPWebServices
{
    /// <summary>
    /// Summary description for HP HSRPService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/", Description = "Web Service For Haryana and Himachal Cash Collection Exe")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    
    public class HSRPService : System.Web.Services.WebService
    {
        utils obj = new utils();
     
        DataTable dt1 = new DataTable("dt");
        SqlDataReader PReader;




        //[WebMethod]
        //public DataTable GetDatabeseRtowise()
        //{

        //    string sqlText = "select DatabaseName,RtolocationName from tblHpDataBaseNameRtowise ";

        //    string CnnString = obj.strProvider;

        //    DataTable dt = new DataTable();
        //    dt = utils.GetDataTable(sqlText, CnnString);

        //    dt.TableName = "tblHpDataBaseNameRtowise";

        //    return dt;

        //}
       
       [WebMethod]
        public DataTable GetSMSRecords(DataTable dt)
        {
            StringBuilder str = new StringBuilder();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
               str.Append("'" + dt.Rows[i]["AUTH_NO"]+ "',");                
            }

            var index = str.ToString().LastIndexOf(',');
            if (index >= 0)
                str.Remove(index, 1);

            //SecondSMSText, SecondSMSSentDateTime, SecondSMSServerResponseID, SecondSMSServerResponseText, ThirdSMSText, ThirdSMSSentDateTime, ThirdSMSServerResponseID, ThirdSMSServerResponseText, FourthSMSText, FourthSMSSentDateTime, FourthSMSServerResponseID, FourthSMSServerResponseText, FifthSMSText, FifthSMSDateTime, FifthSMSServerResponseID, FifthSMSServerResponseText

            string sqlText = "select  Auth_NO,  FirstSMSText, FirstSMSSentDateTime, FirstSMSServerResponseID,FirstSMSServerResponseText  from  SMSlog_HP where hsrp_stateid='3' and AUTH_NO in(" + str.ToString() + ") and  (Auth_NO is not null or Auth_NO='')  and ( FirstSMSText is not null or FirstSMSText='' ) and (FirstSMSSentDateTime  is not null or FirstSMSSentDateTime='' )   and ( FirstSMSServerResponseID is not null or FirstSMSServerResponseID='') and (FirstSMSServerResponseText is not null or  FirstSMSServerResponseText='') ";
           
            string CnnString = obj.strProvider;

            DataTable dtbl = utils.GetDataTable(sqlText, CnnString);

            dtbl.TableName = "HSRPRecords";

            return dtbl;

        }       





        [WebMethod]
        public DataTable GetRecords(DataTable dt)
        {
            StringBuilder str = new StringBuilder();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
               str.Append("'" + dt.Rows[i]["AUTH_NO"]+ "',");
                
            }

            var index = str.ToString().LastIndexOf(',');
            if (index >= 0)
                str.Remove(index, 1);
           

           //string sqlText = "Select   HSRPRecord_AuthorizationNo,hsrp_front_lasercode,HSRP_Rear_LaserCode,  OrderEmbossingDate, OrderClosedDate from hsrprecords  where hsrp_stateid='3' and HSRPRecord_AuthorizationNo in(" + str.ToString() + ") and (hsrp_front_lasercode is not  null  or isnull(hsrp_front_lasercode,'')!='' and HSRP_Rear_LaserCode is not  null or isnull(HSRP_Rear_LaserCode,'')!='' and OrderEmbossingDate is not null or isnull(OrderEmbossingDate,'')!='' and OrderClosedDate is not null or  isnull(OrderClosedDate,'')!='') ";

           // string sqlText = "Select   HSRPRecord_AuthorizationNo,hsrp_front_lasercode,HSRP_Rear_LaserCode,  OrderEmbossingDate, OrderClosedDate from hsrprecords  where hsrp_stateid='3' and HSRPRecord_AuthorizationNo in(" + str.ToString() + ")  and( hsrp_front_lasercode !=''  or isnull(hsrp_front_lasercode,'')!='' ) and ( isnull(HSRP_Rear_LaserCode,'')!='' or  hsrp_rear_lasercode !='')and( OrderEmbossingDate !='' or isnull(OrderEmbossingDate,'')!='') and (OrderClosedDate !='' or  isnull(OrderClosedDate,'')!='' ) ";

            //and( OrderEmbossingDate !='' or isnull(OrderEmbossingDate,'')!='') and (OrderClosedDate !='' or  isnull(OrderClosedDate,'')!='' )

            string sqlText = "Select  hsrprecord_creationdate, RoundOff_NetAmount,   HSRPRecord_AuthorizationNo,hsrp_front_lasercode,HSRP_Rear_LaserCode,  OrderEmbossingDate, OrderClosedDate from hsrprecords  where hsrp_stateid='3' and HSRPRecord_AuthorizationNo in(" + str.ToString() + ")  and( hsrp_front_lasercode !=''  or isnull(hsrp_front_lasercode,'')!='' ) and ( isnull(HSRP_Rear_LaserCode,'')!='' or  hsrp_rear_lasercode !='') and( OrderEmbossingDate !='' or isnull(OrderEmbossingDate,'')!='') ";

            string CnnString = obj.strProvider;

            DataTable dtbl = utils.GetDataTable(sqlText, CnnString);

            dtbl.TableName = "HSRPRecords";

            return dtbl;

        }       




          }
}




