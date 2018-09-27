using System;
using System.Net;
using System.Net.Mail;
using System.Configuration;
using System.Collections.Specialized;

namespace AuthenticationService
{
    class SMTP
    {
        static string caption = "Chat: Regisrtation code";
        static string message = "Please, enter registration code into Login form!\n";
        static string attachFile = null;
        static string SMTPServer { get; set; }
        static string SMTPRequiresAuthentication { get; set; }
        static string SMTPUseSsl { get; set; }
        static int SMTPPort { get; set; }
        static string SMTPUser { get; set; }
        static string SMTPPassword { get; set; }
        static int SMTPTimeoutInMilliseconds { get; set; }
        static string SmtpPreferredEncoding { get; set; }

        static SMTP()
        {
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            SMTPServer = appSettings["SMTPServer"];
            SMTPRequiresAuthentication = appSettings["SMTPRequiresAuthentication"];
            SMTPUseSsl = appSettings["SMTPUseSsl"];
            SMTPPort = Convert.ToInt32(appSettings["SMTPPort"]);
            SMTPUser = appSettings["SMTPUser"];
            SMTPPassword = appSettings["SMTPPassword"];
            SMTPTimeoutInMilliseconds = Convert.ToInt32(appSettings["SMTPTimeoutInMilliseconds"]);
            SmtpPreferredEncoding = appSettings["SmtpPreferredEncoding"];
        }

        public static void SendMail(string mailto, int code)
        {
            string fullMessage = String.Format("{0}Your registration code: {1}", message, code);
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(SMTPUser);
                mail.To.Add(new MailAddress(mailto));
                Console.WriteLine("Mail to: {0}", mailto);
                mail.Subject = caption;
                Console.WriteLine(caption);
                mail.Body = fullMessage;
                if (!string.IsNullOrEmpty(attachFile)) mail.Attachments.Add(new Attachment(attachFile));
                SmtpClient client = new SmtpClient();
                client.Host = SMTPServer;
                Console.WriteLine(SMTPServer);
                client.Port = SMTPPort;
                client.EnableSsl = Convert.ToBoolean(SMTPUseSsl);
                client.Credentials = new NetworkCredential(SMTPUser, SMTPPassword);
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
