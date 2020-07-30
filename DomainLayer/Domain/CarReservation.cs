using System;
using System.Collections.Generic;
using System.Text;

namespace DomainLayer.Domain
{
    public class CarReservation
    {
        public CarReservation() { }

        public CarReservation(Reservation reservation, Car car)
        {
            Reservation = reservation;
            ReservationID = reservation.ID;
            Car = car;
            CarID = car.ID;
        }

        public int ReservationID { get; set; }
        public Reservation Reservation { get; set; }

        public int CarID { get; set; }
        public Car Car { get; set; }
    }
}
