using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;

public class MailHelper
{
    static MailMessage mMailMessage;
    static Attachment att;
    public static void SendMailMessage(string from, string to, string bcc, string cc, string subject, string body, string pathfile)
    {
        // Instantiate a new instance of MailMessage
        // body = "Respected Sir <br /> <br /> I am Sending You mail of Acumulative Collection and Production till Date Report. <br /> As you get the Attachment the same. <br /> Please Find the Aattachment. <br /> <br /> Regard <br /> <br /> Ambrish Singh Yadav";
        mMailMessage = new MailMessage();
        // Set the sender address of the mail message
        mMailMessage.From = new MailAddress(from);
        // Set the recepient address of the mail message
        // mMailMessage.To.Add(new MailAddress(to));
        string[] toAddresses = Regex.Split(to, "[,;] *");

        foreach (string toAddress in toAddresses)
        {
            // message.To.Add(toAddress);
            mMailMessage.To.Add(new MailAddress(toAddress));
        }
        // Check if the bcc value is null or an empty string
        if ((bcc != null) && (bcc != string.Empty))
        {
            // Set the Bcc address of the mail message
            mMailMessage.Bcc.Add(new MailAddress(bcc));
        }      // Check if the cc value is null or an empty value
        if ((cc != null) && (cc != string.Empty))
        {
            // Set the CC address of the mail message
            mMailMessage.CC.Add(new MailAddress(cc));
        }       // Set the subject of the mail message
        mMailMessage.Subject = subject;
        // Set the body of the mail message
        mMailMessage.Body = body;

        // Set the format of the mail message body as HTML
        mMailMessage.IsBodyHtml = true;
        // Set the priority of the mail message to normal
        mMailMessage.Priority = MailPriority.Normal;

        // Instantiate a new instance of SmtpClient
        SmtpClient mSmtpClient = new SmtpClient();
        // Send the mail message
        mSmtpClient.EnableSsl = true;
        //mSmtpClient.Port = 587;

        mSmtpClient.Host = "smtp.gmail.com";
        mSmtpClient.Port = 25;

        //NetworkCredential myCredentials = new NetworkCredential("noreply@rosmertaengg.com","rosengg123");
        NetworkCredential myCredentials = new NetworkCredential("hsrpcomplaint@gmail.com", "hsrp@4321");
        mSmtpClient.Credentials = myCredentials;


        GC.Collect();
        GC.WaitForPendingFinalizers();

        mSmtpClient.Send(mMailMessage);


        mMailMessage.Dispose();
        mSmtpClient.Dispose();


    }
}
