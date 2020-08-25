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
            message.IsBodyHtml = true;
            smtp.Port = Port;
            smtp.Host = Host;
            smtp.EnableSsl = isSSL;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(Username, Password);
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Send(message);
        }

        public static void Send_WelcomeMail(string email, string firstName, string lastName, ClientType type, string phone, string company)
        {
            if(RegexUtilities.IsValidEmail(email, true))
            {
                try
                {
                    String message = FileService.GetFileAsString(@"Resources/Emails/welcome.html");
                    String name = firstName + " " + lastName;
                    if (!string.IsNullOrWhiteSpace(company) && firstName == "N/A" && lastName == "N/A")
                        name = company;
                    message = message.Replace("{{name}}", name);
                    message = message.Replace("{{first_name}}", firstName);
                    message = message.Replace("{{last_name}}", lastName);
                    message = message.Replace("{{type}}", char.ToUpper(type.ToString().ToLower()[0]) + type.ToString().ToLower().Substring(1));
                    message = message.Replace("{{email}}", email);
                    message = message.Replace("{{phone}}", (!string.IsNullOrWhiteSpace(phone)) ? phone : "N/A");
                    SendMail(new List<string>() { email }, "Welcome to VIP Service ", message);
                }
                catch(Exception ex) { LogService.WriteLog(new List<String>() { "Mail Service Exeption (Weclome mail): ", ex.Message, " ", (ex.InnerException != null) ? ex.InnerException.ToString() : "", ex.StackTrace }); }
            }
        }

        public static void Send_NewReservation(string email, Reservation reservation, List<Car> cars)
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

        public static void Send_NewInvoice(string email, string firstName, string lastName, string company, Invoice invoice, List<InvoiceItem> items)
        {
            if (RegexUtilities.IsValidEmail(email, true))
            {
                try
                {
                    String message = FileService.GetFileAsString(@"Resources/Emails/invoice.html");
                    String name = firstName + " " + lastName;
                    if (!string.IsNullOrWhiteSpace(company) && firstName == "N/A" && lastName == "N/A")
                        name = company;
                    string htmlItems = "";
                    foreach(InvoiceItem ii in items)
                        htmlItems += "<tr><td width=\"80%\" class=\"purchase_item\"><span class=\"f-fallback\" >" + ii.Description + "</span></td><td class=\"align-right\" width =\"20 %\" ><span class=\"f-fallback\" >" + string.Format("€{0:0.00}", ii.Total) + "</span></td></tr>";
                    message = message.Replace("{{name}}", name);
                    message = message.Replace("{{invoice_id}}", "#" + invoice.ID);
                    message = message.Replace("{{date}}", invoice.InvoiceDate.ToString("dd/MM/yyyy"));
                    message = message.Replace("{{items}}", htmlItems);
                    message = message.Replace("{{discount}}", string.Format("€{0:0.00}", invoice.Discount));
                    message = message.Replace("{{vat}}", string.Format("€{0:0.00}", invoice.VAT));
                    message = message.Replace("{{total}}", string.Format("€{0:0.00}", invoice.TotalInc));
                    message = message.Replace("{{action_url}}", "https://hogent.timdesmet.be/project/invoice?id=" + invoice.ID  + "&action=pay");
                    SendMail(new List<string>() { email }, "You have a new invoice", message);
                }
                catch (Exception ex) { LogService.WriteLog(new List<String>() { "Mail Service Exeption (Weclome mail): ", ex.Message, " ", (ex.InnerException != null) ? ex.InnerException.ToString() : "", ex.StackTrace }); }
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
