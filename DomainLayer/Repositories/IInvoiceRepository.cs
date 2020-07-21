using DomainLayer.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainLayer.Repositories
{
    public interface IInvoiceRepository
    {
        void AddInvoice(Invoice invoice);
    }
}
