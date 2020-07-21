using DataLayer.Repositories;
using DomainLayer;
using DomainLayer.Repositories;
using System;

namespace DataLayer
{
    public class UnitOfWork : IUnitOfWork
    {
        private RentalContext context;

        public UnitOfWork(RentalContext context)
        {
            this.context = context;
            Cars = new CarRepository(context);
            Clients = new ClientRepository(context);
            InvoiceItems = new InvoiceItemRepository(context);
            Invoices = new InvoiceRepository(context);
            Reservations = new ReservationRepository(context);
        }

        public ICarRepository Cars { get; set; }
        public IClientRepository Clients { get; set; }
        public IInvoiceItemRepository InvoiceItems { get; set; }
        public IInvoiceRepository Invoices { get; set; }
        public IReservationRepostiory Reservations { get; set; }

        public int Complete()
        {
            try
            {
                return context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
