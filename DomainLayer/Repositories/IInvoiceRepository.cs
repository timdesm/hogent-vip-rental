using DomainLayer.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainLayer.Repositories
{
    public interface IInvoiceRepository
    {
        void AddInvoice(Invoice invoice);
        IEnumerable<Invoice> FindAll();
        Invoice Find(int ID);
        void RemoveInvoice(int id);
        void RemoveInvoice(Invoice invoice);
        IEnumerable<Invoice> FindUnpaid();
        Invoice GetLastInvoice();
    }
}
