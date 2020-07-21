using DomainLayer.Domain;
using DomainLayer.Repositories;
using System;
using System.Collections.Generic;
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
    }
}
