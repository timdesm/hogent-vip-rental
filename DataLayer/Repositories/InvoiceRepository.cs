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

        public Invoice Find(int ID)
        {
            return context.Invoices.Where(i => i.ID == ID).Single();
        }

        public void RemoveInvoice(int id)
        {
            context.Invoices.Remove(new Invoice() { ID = id });
        }
    }
}
