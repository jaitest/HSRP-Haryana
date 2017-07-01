using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HSRP.BL
{
    public class blEntryDetail
    {
        string CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        public void DailyEnityDeatail(string uid,string Pagename)
        {
            string q = "insert into DailyEntryDetail(Userid ,FormName) values('" + uid + "','"+Pagename+"')";
            Utils.ExecNonQuery(q, CnnString);
        }
    }
}