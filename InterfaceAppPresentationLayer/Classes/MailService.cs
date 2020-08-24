using DomainLayer.Domain;
using DomainLayer.Utilities;
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

        public static void Send_WelcomeMail(string email)
        {
            if(RegexUtilities.IsValidEmail(email, true))
            {
                try
                {
                    SendMail(new List<string>() { email }, "Welcome to Rudy's VIP service", "Test message");
                }
                catch(Exception ex) { LogService.WriteLog(new List<String>() { "Mail Service Exeption (Weclome mail): ", ex.Message, " ", ex.InnerException.ToString(), ex.StackTrace }); }
            }
        }

        public static void Send_NewReservation(string email, Reservation reservation)
        {
            if (RegexUtilities.IsValidEmail(email, true))
            {
                try
                {
                    SendMail(new List<string>() { email }, "Your reservation has been placed", "Test message");
                }
                catch (Exception ex) { LogService.WriteLog(new List<String>() { "Mail Service Exeption (New Reservation): ", ex.Message, " ", ex.InnerException.ToString(), ex.StackTrace }); }
            }
        }

        public static void Send_NewInvoice(string email, Invoice invoice)
        {
            if (RegexUtilities.IsValidEmail(email, true))
            {
                try
                {
                    SendMail(new List<string>() { email }, "You have a new invoice", "Test message");
                }
                catch (Exception ex) { LogService.WriteLog(new List<String>() { "Mail Service Exeption (New Invoice): ", ex.Message, " ", ex.InnerException.ToString(), ex.StackTrace }); }
            }
        }

        public static void Send_UpdatedReservation(string email, Reservation reservation)
        {
            if (RegexUtilities.IsValidEmail(email, true))
            {
                try
                {
                    SendMail(new List<string>() { email }, "Your reservation has been changed", "Test message");
                }
                catch (Exception ex) { LogService.WriteLog(new List<String>() { "Mail Service Exeption (Update Reservation): ", ex.Message, " ", ex.InnerException.ToString(), ex.StackTrace }); }
            }
        }

        public static void Send_Log(string email, String message)
        {
            if (RegexUtilities.IsValidEmail(email, true))
            {
                try
                {
                    SendMail(new List<string>() { email }, "Log from RudysVIPManager", message);
                }
                catch (Exception ex) { LogService.WriteLog(new List<String>() { "Mail Service Exeption (Log): ", ex.Message, " ", ex.InnerException.ToString(), ex.StackTrace }); }
            }
        }

    }
}
