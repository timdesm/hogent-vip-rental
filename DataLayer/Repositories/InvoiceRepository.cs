using DomainLayer.Domain;
using DomainLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer.Repositories
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private RentalContext context;

        public InvoiceRepository(RentalContext context)
        {
            this.context = context;
        }

        public void AddInvoice(Invoice invoice)
        {
            context.Invoices.Add(invoice);
        }

        public IEnumerable<Invoice> FindAll()
        {
            return context.Invoices.OrderBy(i => i.ID).AsEnumerable<Invoice>();
        }

        public Invoice Find(int ID)
        {
            return context.Invoices.Where(i => i.ID == ID).Single();
        }

        public void RemoveInvoice(int id)
        {
            context.Invoices.Remove(new Invoice() { ID = id });
        }

        public void RemoveInvoice(Invoice invoice)
        {
            context.Invoices.Remove(invoice);
        }

        public IEnumerable<Invoice> FindUnpaid()
        {
            return context.Invoices.Where(i => i.PaymentDue > 0).AsEnumerable<Invoice>();
        }

        public Invoice GetLastInvoice()
        {
            return context.Invoices.Last();
        }

        public void UpdateInvoice(Invoice invoice)
        {
            context.Invoices.Update(invoice);
        }
    }
}
