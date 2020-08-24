using DomainLayer.Domain;
using DomainLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public Reservation Find(int id)
        {
            return context.Reservations.Where(r => r.ID == id).Single();
        }

        public IEnumerable<Reservation> FindAll()
        {
            return context.Reservations.OrderBy(r => r.ID).AsEnumerable<Reservation>();
        }

        public void RemoveReservation(int id)
        {
            context.Reservations.Remove(new Reservation() { ID = id });
        }

        public void RemoveReservation(Reservation reservation)
        {
            context.Reservations.Remove(reservation);
        }

        public IEnumerable<Reservation> Find(DateTime from, DateTime until) {
            return context.Reservations.Where(r => r.ReservationDate >= from).Where(r => r.ReservationDate <= until).AsEnumerable<Reservation>();
        }

        public IEnumerable<Reservation> FindActive()
        {
            return context.Reservations.Where(r => r.ReservationEnded < r.ReservationDate).Where(r => r.ReservationDate <= DateTime.Now).AsEnumerable<Reservation>();
        }

        public void UpdateReservation(Reservation reservation)
        {
            context.Reservations.Update(reservation);
        }
    }
}

