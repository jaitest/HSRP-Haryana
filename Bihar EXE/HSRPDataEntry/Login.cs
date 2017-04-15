using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HSRPTransferData;
using System.Net.NetworkInformation;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Net;

namespace HSRPDataEntry
{

    public partial class Login : Form
    {
        public static string Stateid = string.Empty;
        public static string RTOlocationID = string.Empty;
        public static string UserID = string.Empty;
        public static string RTOLocationAddress = string.Empty;
        public static string username = string.Empty;

        public Login()
        {
            InitializeComponent();
        }

        //string ConnectionString = System.Configuration.ConfigurationSettings.AppSettings["ConnectionString"].ToString();
        private void Login_Load(object sender, EventArgs e)
        {
            utils.GetLocalDBConnectionFromINI();
           // MacAddress();
        
        }
        string detailofuser = string.Empty;
        string strUserLoginID = string.Empty;
        string strPassword = string.Empty;
 
        string MacA = string.Empty;
        string Status = string.Empty;


        public static bool isConnected()
        {
            try
            {
                string myAddress = "www.yahoo.com";
                IPAddress[] addresslist = Dns.GetHostAddresses(myAddress);

                if (addresslist[0].ToString().Length > 6)
                {
                    return true;
                }
                else
                    return false;

            }
            catch
            {
                return false;
            }

        }



        private void btnLogin_Click(object sender, EventArgs e)
        {

            bool i = isConnected();
            if (i == true)
            {
              
        
            
            HSRPDataEntry.HsrpService.HSRPServiceSoapClient objHSRP = new HSRPDataEntry.HsrpService.HSRPServiceSoapClient();
           
           strUserLoginID   = txtUserName.Text.Trim();
           strPassword      = txtPassword.Text.Trim();

           if (strUserLoginID=="")
            {
                MessageBox.Show("Please Provide UserID");
                txtUserName.Focus();
            }
           if (strPassword == "")
           {
               MessageBox.Show("Please Provide Password");
               txtPassword.Focus();

           }

           string macaddre=MacAddress();

          detailofuser= objHSRP.GetLogInDetail(strUserLoginID, strPassword, macaddre);
          string[] detailofuser1 = detailofuser.Split('^');
          if (detailofuser1.Length > 0)
          {
              //  return Status + "^" + MacStatus + "^" + UserID + "^" + StateID + "^" + RtoLocationID;
              Status = detailofuser1[0];
              MacA = detailofuser1[1];
              UserID = detailofuser1[2];
              Stateid = detailofuser1[3];
              RTOlocationID = detailofuser1[4];
              RTOLocationAddress = detailofuser1[5];
              username = detailofuser1[6];
        
          }
                

          
        //   string status = "A";
               //Login(strUserLoginID,strPassword,macaddre);
          if (Status == "Y" || Status == "y")
           {
               txtUserName.Text = "";
               txtPassword.Text = "";
               MDIForm mdi = new MDIForm();
               mdi.Show();

               CashReceiptNB CR = new CashReceiptNB();
              
           }
          else if (Status == "N" || Status == "n")
           {
               MessageBox.Show("Your Machine has been Blocked Please Contact to Administrator");
              
              HSRPDataEntry.mail SendMailobj=new HSRPDataEntry.mail();
              
          

                 string EmailText = "Respected All <br /> <br />Machine-ID <Machineid> has been blocked Please Check My Machine id . <br />My login id is : "+strUserLoginID+" <br /> Regard <br /> <br /> IT Team"; ;
                string eid = "ambrishyad85@gmail.com";
          
                List<string> lst = new List<string>();
               // lst.Add("ambrishyad85@gmail.com");
                //lst.Add("ambrish_yad@yahoo.com");
                //lst.Add("karnvnagpal@gmail.com");
                //lst.Add("corporatesolutions5@gmail.com");
                //lst.Add("ashish.gupta@rosmertatech.com");
                lst.Add("ravikmunjal@gmail.com");
                //lst.Add("infome5050@gmail.com");
                //lst.Add("vp@linkutsav.com");
                //lst.Add("info@linkutsav.com");



          //  "(" + st[p] + ")
               
                    for (int p = 0; p < lst.Count; p++)
                    {
                        string EmailSubject = "Machine Block";
                        
                      
                        SendMailobj.SendMailMessage("recordsmp@gmail.com", lst[p].ToString(), "", "", EmailSubject, EmailText);
                    }
                


              // SendMailobj.SendMailMessage("recordsmp@gmail.com", lst[p].ToString(), "", "", EmailSubject, EmailText, pathfile);
           }
          else if (Status == "A" || Status == "a")
          {
              MessageBox.Show("Your Machine is Un-Authorized......");
          }
          else
          {
              MessageBox.Show("User Name and Password is Worng.......");
          }

      
           //String SqlCheck = "select hsrp_StateID,RTOLocationID,[UserID],[UserLoginName],[Password],[UserFirstName],[UserLastName] ,[MobileNo] ,[emailid],[UserType] FROM [Users] where [UserLoginName]='" + txtUserName.Text.Trim() + "' and Password ='" + txtPassword.Text.Trim() + "'";
           // DataTable dt = utils.GetDataTable(SqlCheck, utils.getCnnHSRPApp );
           // DataTable dt = utils.GetDataTable(SqlCheck, "Data Source=LOCAL;Initial Catalog=hsrp_App_HP;Integrated Security=True");
            //if (dt.Rows.Count > 0)
            //{
            //    Login log = new Login();
            //    this.Visible = false;
            //    int count = dt.Rows.Count;
              
            //  //  CR.sendStateLocation(dt.Rows[0]["hsrp_StateID"].ToString(), dt.Rows[0]["RTOLocationID"].ToString(), dt.Rows[0]["UserID"].ToString());
            //    //OrderBooking or = new OrderBooking();
            //    //or.sendStateLocation(dt.Rows[0]["hsrp_StateID"].ToString(), dt.Rows[0]["RTOLocationID"].ToString(), dt.Rows[0]["UserID"].ToString());
            //    //RejectionPlate RP = new RejectionPlate();
            //    //CR.sendStateLocation(dt.Rows[0]["hsrp_StateID"].ToString(), dt.Rows[0]["RTOLocationID"].ToString(), dt.Rows[0]["UserID"].ToString());
               
            //}
            //else
            //{
            //    Message.Text = "User name or password is wrong";
            //}

            }
            else
            {

                MessageBox.Show("Internet Is Not Connected");

            }
        }

        string Madd;
        public string MacAddress()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in nics)
            {
               IPInterfaceProperties properties = adapter.GetIPProperties();
                Madd=  adapter.GetPhysicalAddress().ToString();
              
               break;

            }
            return Madd;
        }


                         
        
    }
}
