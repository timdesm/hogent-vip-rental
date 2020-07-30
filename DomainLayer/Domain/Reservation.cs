using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainLayer.Domain
{
    public class Reservation
    {
        public Reservation() { }
        public Reservation(Client client, List<CarReservation> carReservations, DateTime orderDate, DateTime reservationDate, string startLocation, string endLocation, ReservationArrangementType arrangement, DateTime reservedUntil, DateTime reservationEnded, Invoice invoice)
        {
            Client = client;
            CarReservations = carReservations;
            OrderDate = orderDate;
            ReservationDate = reservationDate;
            StartLocation = startLocation;
            EndLocation = endLocation;
            Arrangement = arrangement;
            ReservedUntil = reservedUntil;
            ReservationEnded = reservationEnded;
            Invoice = invoice;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public Client Client { get; set; }
        public IList<CarReservation> CarReservations { get; set; }
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
            return $"Reservation : {ID},{Client},{CarReservations},{OrderDate},{ReservationDate},{StartLocation},{EndLocation},{Arrangement},{ReservedUntil},{ReservationEnded},{Invoice}";
        }
    }
}
