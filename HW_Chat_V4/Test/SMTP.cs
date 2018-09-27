using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;

namespace AuthenticationService
{
    class SMTP
    {
        static string smtpServer = "smtp.gmail.com";
        static string from = "spogoretskyitest@gmail";
        static string password = "dfiogjnb";
        static string caption = "Chat: Regisrtation code";
        static string message = "Please, enter registration code into Login form!\n";
        static string attachFile = null;
        static int port = 587;

        public static void SendMail(string mailto, int code)
        {
            string fullMessage = String.Format("{0}Your registration code: {1}", message, code);
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(from);
                mail.To.Add(new MailAddress(mailto));
                mail.Subject = caption;
                mail.Body = fullMessage;
                if (!string.IsNullOrEmpty(attachFile))
                    mail.Attachments.Add(new Attachment(attachFile));
                SmtpClient client = new SmtpClient();
                client.Host = smtpServer;
                client.Port = port;
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(from.Split('@')[0], password);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Send(mail);
                mail.Dispose();
            }
            catch (Exception e)
            {
                throw new Exception("Mail.Send: " + e.Message);
            }
        }
    }
}
