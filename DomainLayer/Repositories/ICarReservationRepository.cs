using DomainLayer.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainLayer.Repositories
{
    public interface ICarReservationRepository
    {
        void AddCarReservation(CarReservation carReservation);
        IEnumerable<CarReservation> ReservationCars(int reservationID);
        void RemoveCarReservation(CarReservation carReservation);
    }
}
