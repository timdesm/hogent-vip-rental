using DomainLayer.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainLayer.Repositories
{
    public interface IInvoiceItemRepository
    {
        void AddInvoiceItem(InvoiceItem invoiceItem);
        InvoiceItem Find(int ID);
        IEnumerable<InvoiceItem> FindInvoiceItems(int invoiceID);
        void RemoveInvoiceItem(int id);
        void RemoveInvoiceItem(InvoiceItem ii);
    }
}
