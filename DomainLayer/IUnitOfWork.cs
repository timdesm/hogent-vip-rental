using DomainLayer.Repositories;
using System;

namespace DomainLayer
{
    public interface IUnitOfWork : IDisposable
    {
        ICarRepository Cars { get; }
        ICarReservationRepository CarReservations { get; }
        IClientRepository Clients { get; }
        IInvoiceItemRepository InvoiceItems { get; }
        IInvoiceRepository Invoices { get; }
        IReservationRepostiory Reservations { get; }
        int Complete();
    }
}
