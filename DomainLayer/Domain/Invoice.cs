using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace DomainLayer.Domain
{
    public class Invoice
    {
        public Invoice() { }
        public Invoice(Client client, DateTime invoiceDate, List<InvoiceItem> items, double discountPercent, double vatPercent)
        {
            Client = client;
            ClientID = client.ID;
            InvoiceDate = invoiceDate;
            Items = items;
            DiscountPercent = discountPercent;
            VATPercent = vatPercent;

            SubTotal = 0.0;
            foreach(InvoiceItem item in items) {
                SubTotal += item.Total;
            }
            Discount = Math.Round(SubTotal / 100 * DiscountPercent, 2);
            TotalExc = SubTotal - Discount;
            VAT = Math.Round(TotalExc / 100 * VATPercent, 2);
            TotalInc = TotalExc + VAT;
            PaymentDue = TotalInc;
        }

        public Invoice(int clientID, DateTime invoiceDate, List<InvoiceItem> items, double discountPercent, double vatPercent)
        {
            ClientID = clientID;
            InvoiceDate = invoiceDate;
            Items = items;
            DiscountPercent = discountPercent;
            VATPercent = vatPercent;

            SubTotal = 0.0;
            foreach (InvoiceItem item in items)
            {
                SubTotal += item.Total;
            }
            Discount = Math.Round(SubTotal / 100 * DiscountPercent, 2);
            TotalExc = SubTotal - Discount;
            VAT = Math.Round(TotalExc / 100 * VATPercent, 2);
            TotalInc = TotalExc + VAT;
            PaymentDue = TotalInc;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [ForeignKey("Client")]
        public int ClientID { get; set; }
        public Client Client { get; set; }

        public DateTime InvoiceDate { get; set; }
        public IList<InvoiceItem> Items { get; set; }
        public double DiscountPercent { get; set; }
        public double Discount { get; set; }
        public double VATPercent { get; set; }
        public double VAT { get; set; }

        public double SubTotal { get; set; }
        public double TotalExc { get; set; }
        public double TotalInc { get; set; }

        public double PaymentDue { get; set; }

        public override string ToString()
        {
            return $"Invoice : {ID},{Client},{InvoiceDate},{Items},{DiscountPercent},{Discount},{VATPercent},{VAT},{SubTotal},{TotalExc},{TotalInc},{PaymentDue}";
        }
    }
}
