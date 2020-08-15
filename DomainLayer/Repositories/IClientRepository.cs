using DomainLayer.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainLayer.Repositories
{
    public interface IClientRepository
    {
        void AddClient(Client client);
        IEnumerable<Client> FindAll();
        Client Find(int ID);
        Client Find(string email);
        IEnumerable<Client> FindNewest(int amount);
        void RemoveClient(int id);
        int GetYealyReservations(Client client, int year);
    }
}
