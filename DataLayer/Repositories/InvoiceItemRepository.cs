using DomainLayer.Domain;
using DomainLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public InvoiceItem Find(int ID)
        {
            return context.InvoiceItems.Where(ii => ii.ID == ID).Last();
        }

        public IEnumerable<InvoiceItem> FindInvoiceItems(int invoiceID)
        {
            return context.InvoiceItems.Where(ii => ii.InvoiceID == invoiceID).AsEnumerable<InvoiceItem>();
        }

        public void RemoveInvoiceItem(int id)
        {
            context.InvoiceItems.Remove(new InvoiceItem() { ID = id });
        }
    }
}
