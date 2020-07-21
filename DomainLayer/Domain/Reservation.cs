using System;
using System.Collections.Generic;
using System.Text;

namespace DomainLayer.Domain
{
    public class Reservation
    {
        public Reservation() { }
        public Reservation(Client client, List<Car> cars, DateTime orderDate, DateTime reservationDate, string startLocation, string endLocation, ReservationArrangementType arrangement, DateTime reservedUntil, DateTime reservationEnded, Invoice invoice)
        {
            Client = client;
            Cars = cars;
            OrderDate = orderDate;
            ReservationDate = reservationDate;
            StartLocation = startLocation;
            EndLocation = endLocation;
            Arrangement = arrangement;
            ReservedUntil = reservedUntil;
            ReservationEnded = reservationEnded;
            Invoice = invoice;
        }

        public int ID { get; set; }
        public Client Client { get; set; }
        public IList<Car> Cars { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime ReservationDate { get; set; }
        public string StartLocation { get; set; }
        public string EndLocation { get; set; }
        public ReservationArrangementType Arrangement { get; set; }
        public DateTime ReservedUntil { get; set; }
        public DateTime ReservationEnded { get; set; }
        public Invoice Invoice { get; set; }

        public override string ToString()
        {
            return $"Reservation : {ID},{Client},{Cars},{OrderDate},{ReservationDate},{StartLocation},{EndLocation},{Arrangement},{ReservedUntil},{ReservationEnded},{Invoice}";
        }
    }
}
