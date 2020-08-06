using DomainLayer.Domain;
using DomainLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private RentalContext context;

        public ClientRepository(RentalContext context)
        {
            this.context = context;
        }

        public void AddClient(Client client)
        {
            context.Clients.Add(client);
        }

        public IEnumerable<Client> FindAll()
        {
            return context.Clients.OrderBy(s => s.ID).AsEnumerable<Client>();
        }

        public Client Find(int id)
        {
            return context.Clients.Find(id);
        }

        public Client Find(string email)
        {
            return context.Clients.Where(c => c.Email.ToLower() == email.ToLower()).Last();
        }

        public IEnumerable<Client> FindNewest(int amount)
        {
            return (from c in context.Clients orderby c.ID descending select c).Take(amount);
        }

        public void Remove(int id)
        {
            context.Clients.Remove(new Client() { ID = id });
        }

        public int GetYealyReservations(Client client, int year)
        {
            return context.Reservations.Where(r => r.Client == client && r.ReservationDate.Year == year).Count();
        }
    }
}
