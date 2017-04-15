using System;
using System.Data;
using System.Windows.Forms;
using HSRPTransferData;
using System.ComponentModel;

using System.IO;
using System.Drawing.Printing;
using System.Drawing;
using System.Net;
using HSRPDataEntryNew.WebReferenceHP;

namespace HSRPDataEntryNew
{
    public partial class OrderClosed : Form
    {
        public OrderClosed()
        {
            InitializeComponent();
         
        }

        HSRPService objWebServiceHP = new HSRPService();
        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();      
        }

        string Query   = string.Empty;
        string Sticker = string.Empty ;
        string Vip     = string.Empty;
        string recno = string.Empty;
        string strorderstatus = string.Empty;
        string qry = string.Empty;
        string strauth = string.Empty;
        DataTable dt1  = new DataTable();
        
        private void btnSave_Click(object sender, EventArgs e)
        {
            
           
            if (string.IsNullOrEmpty(txtVehRegNo.Text))
            {
                MessageBox.Show("Please Insert Vehicle Registration No.");

                txtVehRegNo.Text = "";
                txtVehRegNo.Focus();
                return;
            }

            if (strorderstatus == "New Order")
            {
                if (string.IsNullOrEmpty(txtFlaser.Text))
                {
                    MessageBox.Show("Please Insert Front Laser No.");
                    txtFlaser.Text = "";
                    txtFlaser.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(txtRlaser.Text))
                {
                    MessageBox.Show("Please Insert Rear Laser No.");
                    txtRlaser.Text = "";
                    txtRlaser.Focus();
                    return;
                }

                string Message = objWebServiceHP.ValidateLaserCode(txtFlaser.Text, txtRlaser.Text);
                string[] Message1 = Message.Split('^');
                if (Message1[0].ToString() == "FrontUsed")
                {
                    MessageBox.Show("Front Laser No Already Used.");
                    txtFlaser.Focus();
                    return;
                }
                else if (Message1[0].ToString() == "FrontNotFound")
                {
                    MessageBox.Show("Front Laser No Not Found.");
                    txtFlaser.Focus();
                    return;
                }

                else if (Message1[0].ToString() == "RearUsed")
                {
                    MessageBox.Show("Rear Laser No Already Used.");
                    txtFlaser.Focus();
                    return;
                }
                else if (Message1[0].ToString() == "RearNotFound")
                {
                    MessageBox.Show("Rear Laser No Not Found.");
                    txtFlaser.Focus();
                    return;
                }
                else
                {

                   
                    if (txtFlaser.Text.Length > 10 && txtRlaser.Text.Length > 10)
                    {
                        if (txtFlaser.Text.Trim() == txtRlaser.Text.Trim())
                        {
                            MessageBox.Show("Both Laser No. can not be same..");
                            txtRlaser.Focus();
                            return;
                        }
                        else
                        {
                            updateRecord();

                        }
                    }
                    else
                    {
                        MessageBox.Show("Length Laser No. should be more than 10");
                        txtFlaser.Focus();
                        return;
                    }
                }
            }
            else
            {
                /// For Clossing
                updateRecord();

            }
            
        }
 
      
        public void Refresh()
        {
            txtVehRegNo.Text = "";
            lblEngineno.Text = "";
            lblchassisno.Text = "";
            txtFlaser.Text = "";
            txtRlaser.Text = "";
            //checkBox1.
        }

      
               

        private void txtVehRegNo_TextChanged(object sender, EventArgs e)
        {
            lblMessage.Text = "";
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Refresh();
        }
        
        private void button2_Click_1(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(txtVehRegNo.Text))
            {
                MessageBox.Show("Please Insert Vehicle Registration No.");

                txtVehRegNo.Text = "";
                txtVehRegNo.Focus();
                return;
            }

            checkBox1.Checked = false;
            checkBox1.Enabled = false;
            search_veh();

        }


        private void search_veh()
        {
            DataTable dtCheckOrder = objWebServiceHP.CheckOrderstatus("3", txtVehRegNo.Text.Replace(" ", ""));
            if (dtCheckOrder.Rows.Count > 0)
            {
                if (dtCheckOrder.Rows[0]["OrderStatus"].ToString().ToUpper() == "CLOSED")
                {
                    MessageBox.Show("Vehicle No Already Closed.....");
                    txtVehRegNo.Focus();
                    return;
                }

            }
            else
            {


                DataTable dtInfoVehicle = objWebServiceHP.GetDeatilsUsingVehcileRegNoForClosed("3", txtVehRegNo.Text.Replace(" ", ""));

                if (dtInfoVehicle.Rows.Count > 0)
                {
                    strauth = dtInfoVehicle.Rows[0]["HsrpRecord_AuthorizationNo"].ToString();
                    txtVehRegNo.Text = dtInfoVehicle.Rows[0]["vehicleregno"].ToString();
                    lblchassisno.Text = dtInfoVehicle.Rows[0]["chassisno"].ToString();
                    lblEngineno.Text = dtInfoVehicle.Rows[0]["Engineno"].ToString();
                    strorderstatus = dtInfoVehicle.Rows[0]["OrderStatus"].ToString();
                    if (dtInfoVehicle.Rows[0]["OrderStatus"].ToString() == "Embossing Done")
                    {
                        txtFlaser.Text = dtInfoVehicle.Rows[0]["hsrp_front_lasercode"].ToString();
                        txtRlaser.Text = dtInfoVehicle.Rows[0]["hsrp_rear_lasercode"].ToString();
                        txtFlaser.Enabled = false;
                        txtRlaser.Enabled = false;
                    }
                    else
                    {
                        txtFlaser.Enabled = true;
                        txtRlaser.Enabled = true;
                        txtFlaser.Text = "";
                        txtRlaser.Text = "";

                    }
                    if ((dtInfoVehicle.Rows[0]["Vehicletype"].ToString() == "LMV") || (dtInfoVehicle.Rows[0]["Vehicletype"].ToString() == "LMV(Class)") || (dtInfoVehicle.Rows[0]["Vehicletype"].ToString() == "Three Wheeler") || (dtInfoVehicle.Rows[0]["Vehicletype"].ToString() == "MCV/HCV/TRAILERS"))
                    {
                        checkBox1.Checked = true;
                        checkBox1.Enabled = false;
                    }
                    else
                    {
                        checkBox1.Checked = false;
                        checkBox1.Enabled = false;
                    }

                }
                else
                {
                    MessageBox.Show("Record Not Found");
                    txtVehRegNo.Focus();
                    return;
                }

            }
        }
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
        private void updateRecord()
        {
            string qry = string.Empty;
            string Message = objWebServiceHP.ValidateLaserCodeForOrderClose(txtFlaser.Text, txtRlaser.Text);
            string[] Message1 = Message.Split('^');
            if (Message1[0].ToString() == "FrontUsed")
            {
                MessageBox.Show("Front Laser No Already Used.");
                txtFlaser.Focus();
                return;
            }
            else if (Message1[0].ToString() == "RearUsed")
            {
                MessageBox.Show("Rear Laser No Already Used.");
                txtFlaser.Focus();
                return;
            }

            DataTable dt1 = objWebServiceHP.GetDeatilsUsingVehcileRegNoForClosed("3", txtVehRegNo.Text.Replace(" ", ""));
            if(dt1.Rows.Count > 0)
            {
                txtVehRegNo.Text = dt1.Rows[0]["vehicleregno"].ToString();
                if (dt1.Rows[0]["orderstatus"].ToString() == "Embossing Done")
                {
                    string strResult = objWebServiceHP.UpdateClosedorder("3", txtVehRegNo.Text, txtFlaser.Text, txtRlaser.Text);
                    if (strResult == "Success")
                    {
                        qry = "update OrderBookingOffLine set  ordercloseddate = getdate(),orderstatus='Closed', hsrp_front_lasercode='" + txtFlaser.Text + "', hsrp_rear_lasercode ='" + txtRlaser.Text + "' where vehicleregno ='" + txtVehRegNo.Text + "' and orderstatus='Embossing Done'";
                        int j = utils.ExecNonQuery(qry, utils.getCnnHSRPApp);
                        qry = "update HSRP_DTLS set HSRP_NO_FRONT='" + txtFlaser.Text + "',HSRP_NO_back= '" + txtRlaser.Text + "',HSRP_FIX_DT =getdate() where replace(REGN_NO,' ','') ='" + txtVehRegNo.Text + "'";
                        j = utils.ExecNonQuery(qry, utils.getCnnHSRPVahan);
                        
                    }
                    else
                    {
                        MessageBox.Show("Record Not Saved........");
                        return;
                    }
                }
                else
                {
                     qry = "update OrderBookingOffLine set  ordercloseddate = getdate(),orderembossingdate = getdate(),orderstatus='Closed', hsrp_front_lasercode='" + txtFlaser.Text + "', hsrp_rear_lasercode ='" + txtRlaser.Text + "' where vehicleregno ='" + txtVehRegNo.Text + "' and orderstatus='New Order'";
                    int j = utils.ExecNonQuery(qry, utils.getCnnHSRPApp);
                    qry = "update HSRP_DTLS set HSRP_NO_FRONT='" + txtFlaser.Text + "',HSRP_NO_back= '" + txtRlaser.Text + "',HSRP_ISSUE_DT =getdate(),HSRP_FIX_DT =getdate() where replace(REGN_NO,' ','') ='" + txtVehRegNo.Text + "'";
                    j = utils.ExecNonQuery(qry, utils.getCnnHSRPVahan);
                    MessageBox.Show("Embossing is  not done....");
                    return;
                    

                }

                if (isConnected())
                {              
               
                    string MobileNo =  dt1.Rows[0]["MobileNo"].ToString().Trim();
                    string smsmessage = "Your HSRP " + txtVehRegNo.Text.Trim() + " has been affixed.  Transport Dept HP.";
                    string url = "http://alerts.smseasy.in/api/web2sms.php?workingkey=627691h463b66gov5j83&sender=HPHSRP&to=" + MobileNo + "&message=" + smsmessage + "";
                    HttpWebRequest MyRequest = (HttpWebRequest)WebRequest.Create(url);
                    MyRequest.Method = "GET";
                    WebResponse myRespose = MyRequest.GetResponse();
                    StreamReader sr = new StreamReader(myRespose.GetResponseStream(), System.Text.Encoding.UTF8);
                    string result = sr.ReadToEnd();
                    sr.Close();
                    myRespose.Close();
                   
                    int j1;

                    string strMessage = objWebServiceHP.UpdateHpSmsLogOnClosed(MobileNo, smsmessage, result.ToString(), strauth);
                    if (strMessage == "Success")
                    {
                        qry = "update OrderBookingOffLine set  SecondSMSText ='" + smsmessage + "',SecondSMSDateTime=getdate(),SecondSMSServerResponseID='" + result.ToString() + "'  where vehicleregno = '" + txtVehRegNo.Text + "' and orderstatus='Closed'";
                        j1 = utils.ExecNonQuery(qry, utils.getCnnHSRPApp);
                        qry = "update HSRP_DTLS_SMS set [SecondSMSText]='" + smsmessage + "',[SecondSMSSentDateTime]=getdate(),[SecondSMSServerResponseID]='" + result.ToString() + "' where [MobileNo] ='" + MobileNo + "' and [AUTH_NO]= '" + dt1.Rows[0]["HSRPRecord_AuthorizationNo"].ToString() + "'";
                        j1 = utils.ExecNonQuery(qry, utils.getCnnHSRPVahan);

                        MessageBox.Show("Record Saved and SMS Sent To Client On : " + MobileNo);
                        

                    }
                    else
                    {
                        MessageBox.Show("Record Not Saved....");
                        return;

                    }
                }
                else
                {
                    MessageBox.Show("Record Saved and SMS Not Sent as No Internet.");

                }


                return;
            }
            else
            {
                MessageBox.Show("Record Not Found");
                txtVehRegNo.Focus();
                return;
            }
            
           

        }

        
            
        
    }
       
}
