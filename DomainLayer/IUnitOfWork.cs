using DomainLayer.Repositories;
using System;

namespace DomainLayer
{
    public class IUnitOfWork
    {
        ICarRepository Cars { get; }
        IClientRepository Clients { get; }
        IInvoiceItemRepository InvoiceItems { get; }
        IInvoiceRepository Invoices { get; }
        IReservationRepostiory Reservations { get; }
        int Complete();
    }
}
