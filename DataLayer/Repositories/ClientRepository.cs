using DomainLayer.Domain;
using DomainLayer.Repositories;
using System;
using System.Collections.Generic;
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

    }
}
