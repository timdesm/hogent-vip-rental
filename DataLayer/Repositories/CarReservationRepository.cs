using DomainLayer.Domain;
using DomainLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer.Repositories
{
    public class CarReservationRepository : ICarReservationRepository
    {
        private RentalContext context;

        public CarReservationRepository(RentalContext context)
        {
            this.context = context;
        }

        public void AddCarReservation(CarReservation carReservation)
        {
            context.CarReservations.Add(carReservation);
        }

        public IEnumerable<CarReservation> ReservationCars(int reservationID)
        {
            return context.CarReservations.Where(c => c.ReservationID == reservationID).AsEnumerable<CarReservation>();
        }

        public void RemoveCarReservation(CarReservation carReservation)
        {
            context.CarReservations.Remove(carReservation);
        }
    }
}
