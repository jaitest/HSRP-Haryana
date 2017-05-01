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
using HSRPDataEntryNew.WebReferenceHP;
using Org.BouncyCastle.Utilities.Net;
using System.Net;



namespace HSRPDataEntryNew
{

    public partial class Login : Form
    {
             string Status =string.Empty;
            
            
             string Stateid = string.Empty;
             
        public Login()
        {
            InitializeComponent();
        }

        //string ConnectionString = System.Configuration.ConfigurationSettings.AppSettings["ConnectionString"].ToString();
        private void Login_Load(object sender, EventArgs e)
        {
            
        }
         HSRPService objWebServiceHP = new HSRPService();
        public static bool isConnected()
        {
            try
            {
                string myAddress = "www.yahoo.com";
                System.Net.IPAddress[] addresslist = Dns.GetHostAddresses(myAddress);

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

                string strUserLoginID = string.Empty;
                string strPassword = string.Empty;

                strUserLoginID = txtUserName.Text.Trim();
                strPassword = txtPassword.Text.Trim();

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

                utils.MacBase = MacAddress();
                string detailofuser = objWebServiceHP.GetLogInDetail(strUserLoginID, strPassword, utils.MacBase);
                string[] detailofuser1 = detailofuser.Split('^');
                if (detailofuser1.Length > 0)
                {
                    //  return Status + "^" + MacStatus + "^" + UserID + "^" + StateID + "^" + RtoLocationID;
                    Status = detailofuser1[0];
                   //utils.MacBase= detailofuser1[1];
                    utils.UserID = detailofuser1[2];
                    Stateid = detailofuser1[3];
                   utils.RTOlocationID = detailofuser1[4];

                }

                if (Status == "Y" || Status == "y")
                {
                    txtUserName.Text = "";
                    txtPassword.Text = "";
                    this.Hide();
                    MDIForm mdi = new MDIForm();
                    mdi.Show();

                 
                    //cr.MdiParent = this;
                    //cr.StartPosition = FormStartPosition.CenterScreen;
                    

                    //CashReceiptNB CR = new CashReceiptNB();

                }
                else if (Status == "N" || Status == "n")
                {
                    MessageBox.Show("Your Machine has been Blocked Please Contact to Administrator");
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
                MessageBox.Show("No Internet Connection");
            }
        }
        string Madd = string.Empty;
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
    

    
    }
}
