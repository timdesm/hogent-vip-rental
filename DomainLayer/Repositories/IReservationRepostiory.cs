using DomainLayer.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainLayer.Repositories
{
    public interface IReservationRepostiory
    {
        void AddReservation(Reservation reservation);
        Reservation Find(int id);
        IEnumerable<Reservation> FindAll();
        void RemoveReservation(int id);
    }
}
