using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text;

namespace InterfaceAppPresentationLayer.Classes
{
    class MailService
    {
        private static string Host = "mail.timdesmet.be";
        private static int Port = 587;
        private static Boolean isSSL = true;
        private static string Username = "hogent-projects@timdesmet.be";
        private static string Password = "WbZ6Kvbjt";
        private static string From = "hogent-projects@timdesmet.be";

        public static void SendMail(List<string> to, string subject, string htmlMessage)
        {
            MailMessage message = new MailMessage();
            SmtpClient smtp = new SmtpClient();
            message.From = new MailAddress(From);
            foreach(string toAddress in to)
                message.To.Add(new MailAddress(toAddress));
            message.Subject = subject;
            message.Body = htmlMessage;
            smtp.Port = Port;
            smtp.Host = Host;
            smtp.EnableSsl = isSSL;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(Username, Password);
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Send(message);
        }
    }
}
