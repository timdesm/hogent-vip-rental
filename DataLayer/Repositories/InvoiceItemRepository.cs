using DomainLayer.Domain;
using DomainLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Repositories
{
    public class InvoiceItemRepository : IInvoiceItemRepository
    {
        private RentalContext context;

        public InvoiceItemRepository(RentalContext context)
        {
            this.context = context;
        }

        public void AddInvoiceItem(InvoiceItem invoiceItem)
        {
            context.InvoiceItems.Add(invoiceItem);
        }
    }
}
