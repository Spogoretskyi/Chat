using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;


/// <summary>
/// Отправка письма на почтовый ящик C# mail send
/// </summary>
/// <param name="smtpServer">Имя SMTP-сервера</param>
/// <param name="from">Адрес отправителя</param>
/// <param name="password">пароль к почтовому ящику отправителя</param>
/// <param name="mailto">Адрес получателя</param>
/// <param name="caption">Тема письма</param>
/// <param name="message">Сообщение</param>
/// <param name="attachFile">Присоединенный файл</param>


namespace smtp
{
    class Program
    {
        static void Main(string[] args)
        {
            SendMail("smtp.gmail.com", "spogoretskyitest@gmail", "dfiogjnb", "spogoretskyi@gmail.com", "Hello", "Hello"); 
        }
        public static void SendMail(string smtpServer, string from, string password, string mailto, string caption, string message, string attachFile = null)
        {
    //        try
     //       {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(from);
                mail.To.Add(new MailAddress(mailto));
                mail.Subject = caption;
                mail.Body = message;
                if (!string.IsNullOrEmpty(attachFile))
                    mail.Attachments.Add(new Attachment(attachFile));
                SmtpClient client = new SmtpClient();
                client.Host = smtpServer;
                client.Port = 587;
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(from.Split('@')[0], password);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Send(mail);
                mail.Dispose();
      //      }
      //      catch (Exception e)
      //      {
       //         throw new Exception("Mail.Send: " + e.Message);
       //5     }
        }
    }
}
