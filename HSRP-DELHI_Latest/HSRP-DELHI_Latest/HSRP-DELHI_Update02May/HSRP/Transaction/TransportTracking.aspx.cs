using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;
//using System.Web.Mail;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HSRP.Transaction
{
    public partial class TransportTracking : System.Web.UI.Page
    {
        string CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
      
        string StringEmbcode = string.Empty;
        string StringDesignation = string.Empty;
        string Stringdesc = string.Empty;
        string StringTimeout = string.Empty;
        string SQLString = string.Empty;
        string sqlQuery12 = string.Empty;
        string HSRP_StateID = string.Empty;
        string RTOLocationID = string.Empty;
        string USERID = string.Empty;
        string strtimeinto = string.Empty;
        string strtimeoutto = string.Empty; 
        string strempid =string.Empty;
        string Empidd = string.Empty;
        int UserType;
        string status = string.Empty;
        string stateid = string.Empty;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            Utils.GZipEncodePage();
            
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {
                HSRP_StateID = Session["UserHSRPStateID"].ToString();
                RTOLocationID = Session["UserRTOLocationID"].ToString();
                USERID = Session["UID"].ToString();
                userdetails();
                btncomplaint.Visible = true;
            }

          


          
        }

        public void userdetails()
        {
            try
            {
                SQLString = "Select  R.LandlineNo,R.RTOLocationName,R.RTOLocationAddress,R.ContactPersonName,R.MobileNo,R.EmailID,R.Rtocode,R.EmailID2,E.Address1+''+E.Address2 as EmbAddress From RTOLocation R,Embossingcenters E where R.NAVEMBID=E.Emb_Center_Id and R.HSRP_StateID=2 and R.RTOLocationID='" + RTOLocationID + "'";

                DataTable dt = Utils.GetDataTable(SQLString, CnnString);
                if (dt.Rows.Count > 0)
                {

                    lbluname.Text = dt.Rows[0]["ContactPersonName"].ToString();
                    lblmname.Text = dt.Rows[0]["MobileNo"].ToString();
                    lblename.Text = dt.Rows[0]["EmailID"].ToString();
                    lblaname.Text = dt.Rows[0]["RTOLocationAddress"].ToString();
                    lblProductionCenterAddress.Text = dt.Rows[0]["EmbAddress"].ToString();
                    hdnRtoLocationName.Value = dt.Rows[0]["RTOLocationName"].ToString();


                }
            }
            catch(Exception ex)
            {

                lblErrMess.Text = ex.Message.ToString();
            }
        
        }
        protected void btncollection_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect(String.Format("http://180.151.100.242/HSRPDL01/Report/DailyCashCollectionRTOLocationWise_WithDateGov.aspx"));
                return;
            }
            catch (Exception ee)
            {
                // lblMsg.Text = ee.Message;
            }
        }

        protected void btnaffixation_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect(String.Format("http://180.151.100.242/HSRPDL01/Report/DailyAuthorityRTOLocationWiseOrderClosedGov.aspx"));
                return;
            }
            catch (Exception ee)
            {
                // lblMsg.Text = ee.Message;
            }
        }

        protected void btnmlo_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect(String.Format("http://180.151.100.242/HSRPDL01/Report/Delhi_DailyAuthorityRTOLocationWiseGov.aspx"));
                return;
            }
            catch (Exception ee)
            {
                // lblMsg.Text = ee.Message;
            }
        }

        protected void buttonSave_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect(String.Format("http://180.151.100.242/HSRPDL01/Report/Delhi_TaxRoyalty_DailyCashCollectionRTOLocationWise_WithDateGov.aspx"));
                return;
            }
            catch (Exception ee)
            {
                // lblMsg.Text = ee.Message;
            }
        }

        protected void btndealer_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect(String.Format("http://180.151.100.242/HSRPDL01/Report/DailyCashCollectionRTOLocationWise_Dealer.aspx"));
                return;
            }
            catch (Exception ee)
            {
                // lblMsg.Text = ee.Message;
            }
        }

        protected void btntextbox_Click(object sender, EventArgs e)
        {
            Panel1.Visible = true;
            btntextbox.Visible = false;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                string fileName="";
                //if ((FileUpload1.PostedFile != null) && (FileUpload1.PostedFile.ContentLength > 0))
                //{
                //    fileName = Path.GetFileName(FileUpload1.PostedFile.FileName);
                //    string path=@"D:\DelhiSuggestionFiles\";
                //    path=path+fileName;
                //    FileUpload1.PostedFile.SaveAs(path);
                //}
                //else
                //{
                //    fileName = "";
                //}
                SQLString = "insert into DelhiSuggestion(RTOLocationid,SuggestionText,UserId,SuggestionDate,[FileName]) values('" + RTOLocationID + "','"+TextBox1.Text+"','"+USERID+"',GetDate(),'"+fileName+"')";
                int i = Utils.ExecNonQuery(SQLString, CnnString);
                if (i> 0)
                {
                    using (MailMessage mm = new MailMessage("hsrpSuggestions@gmail.com", "dlhsrp@gmail.com"))
                    {
                        string subject1 = "Rto Location Name:'" + hdnRtoLocationName.Value.ToString() + "', Suggestion by MLO";
                        mm.Subject = subject1;
                        mm.Body = TextBox1.Text;
                        if ((FileUpload1.PostedFile != null) && (FileUpload1.PostedFile.ContentLength > 0))
                        {
                            if (FileUpload1.HasFile)
                            {
                                string FileName = Path.GetFileName(FileUpload1.PostedFile.FileName);
                                mm.Attachments.Add(new Attachment(FileUpload1.PostedFile.InputStream, FileName));
                            }
                        }
                        mm.IsBodyHtml = false;
                        SmtpClient smtp = new SmtpClient();
                        smtp.Host = "smtp.gmail.com";
                        smtp.EnableSsl = true;
                        NetworkCredential NetworkCred = new NetworkCredential("hsrpSuggestions@gmail.com", "hsrp@1234");
                        smtp.UseDefaultCredentials = true;
                        smtp.Credentials = NetworkCred;
                        smtp.Port = 587;
                        smtp.Send(mm);                        
                    }
                    lblErrMess.Text = "Email Sent Successfully";
                    Panel1.Visible = false;
                    btntextbox.Visible = true;


                }
            }
            catch (Exception ex)
            {

                lblErrMess.Text = ex.Message.ToString();
            }
        }

        protected void btnmonthwise_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect(String.Format("http://180.151.100.242/HSRPDL01/Report/DailyCashCollectionRTOLocationWise_WithMonth.aspx"));
                return;
            }
            catch (Exception ee)
            {
                // lblMsg.Text = ee.Message;
            }
        }

        protected void btncomplaint_Click(object sender, EventArgs e)
        {
        //    try
        //    {
        //        //System.Threading.Thread.Sleep(900000000);
        //        //Response.Redirect(String.Format(""));
        //        //return;
        //    }
        //    catch (Exception ee)
        //    {
        //        // lblMsg.Text = ee.Message;
        //    }



            try
            {
                Response.Redirect(String.Format("http://180.151.100.242/HSRPDL01/Report/ComplainReportgov.aspx"));
                return;
            }
            catch (Exception ee)
            {
                // lblMsg.Text = ee.Message;
            }
        }
       
    }
}
