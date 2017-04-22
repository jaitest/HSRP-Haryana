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
        string Stateid = string.Empty;
        string RTOlocationID = string.Empty;
        string UserID = string.Empty;
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
        
        string Madd;
        public string MacAddress()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in nics)
            {
                IPInterfaceProperties properties = adapter.GetIPProperties();
                Madd = adapter.GetPhysicalAddress().ToString();

                break;

            }
            return Madd;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {

            bool i = isConnected();
            if (i == true)
            {
                HSRPDataEntry.HSRPService.HSRPService objHSRP = new HSRPDataEntry.HSRPService.HSRPService();

                strUserLoginID = txtUserName.Text.Trim();
                strPassword = txtPassword.Text.Trim();
                string macaddre = MacAddress();

                #region Validation Check
                if (strUserLoginID == "")
                {
                    MessageBox.Show("Please Provide UserID");
                    txtUserName.Focus();
                }
                if (strPassword == "")
                {
                    MessageBox.Show("Please Provide Password");
                    txtPassword.Focus();

                }
                #endregion             

                #region Checking User Is Valid Or Not
                detailofuser = objHSRP.GetLogInDetail(strUserLoginID, strPassword, macaddre);
                
                string[] detailofuser1 = detailofuser.Split('^');
                if (detailofuser1.Length > 0)
                {
                    Status = detailofuser1[0];
                    MacA = detailofuser1[1];
                    UserID = detailofuser1[2];
                    Stateid = detailofuser1[3];
                    RTOlocationID = detailofuser1[4];
                    utils.RtoLocationId = RTOlocationID;
                }
                #endregion


                if (Status == "Y" || Status == "y")
                {
                    txtUserName.Text = "";
                    txtPassword.Text = "";
                    MDIForm mdi = new MDIForm(UserID);
                    mdi.Show();
                }
                else if (Status == "N" || Status == "n")
                {
                    MessageBox.Show("Your Machine has been Blocked Please Contact to Administrator");

                    HSRPDataEntry.mail SendMailobj = new HSRPDataEntry.mail();

                    string EmailText = "Respected All <br /> <br />Machine-ID <Machineid> has been blocked Please Check My Machine id . <br />My login id is : " + strUserLoginID + " <br /> Regard <br /> <br /> IT Team"; ;
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

                    for (int p = 0; p < lst.Count; p++)
                    {
                        string EmailSubject = "Machine Block";
                        SendMailobj.SendMailMessage("recordsmp@gmail.com", lst[p].ToString(), "", "", EmailSubject, EmailText);
                    }

                }
                else if (Status == "A" || Status == "a")
                {
                    MessageBox.Show("Your Machine is Un-Authorized......");
                }
                else
                {
                    MessageBox.Show("User Name and Password is Worng.......");
                }
            }
            else
            {

                MessageBox.Show("Internet Is Not Connected");

            }
        }

    }
}
