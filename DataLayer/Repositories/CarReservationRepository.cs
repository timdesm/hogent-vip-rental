using DomainLayer.Domain;
using DomainLayer.Repositories;
using System;
using System.Collections.Generic;
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
    }
}
