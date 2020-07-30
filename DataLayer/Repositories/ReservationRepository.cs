using DomainLayer.Domain;
using DomainLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Repositories
{
    public class ReservationRepository : IReservationRepostiory
    {
        private RentalContext context;

        public ReservationRepository(RentalContext context)
        {
            this.context = context;
        }

        public void AddReservation(Reservation reservation)
        {
            context.Reservations.Add(reservation);
        }

        public void RemoveReservation(int id)
        {

        }
    }
}
