using DataLayer;
using DomainLayer.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppUnitTest
{
    [TestClass]
    public class WPFReservationTesting
    {
        [TestMethod]
        public void TestRentalManagerAddReservation()
        {
            // Pre-setup
            RentalManager manager = new RentalManager(new UnitOfWork(new RentalContext("development")));
            manager.AddClient("Tim", "De Smet", "tim.de.smet@hotmail.be", "0493100289", "Azaleastraat", "57", "", "9940", "Evergem", "Belgium", ClientType.AGENCY, "Jetstax", "BE0730.671.009");
            Assert.AreNotEqual(manager.GetAllClients().Count, 0);
            Client client = manager.GetAllClients().Last();
            manager.AddCar("Porsche", "Cayenne Limousine", "White", 310, 1500, 1200, 1600, true);
            Assert.AreNotEqual(manager.GetAllCars().Count, 0);
            Car car = manager.GetAllCars().Last();

            // Setup
            manager.AddReservation(client, ReservationArrangementType.AIRPORT, new DateTime(2020, 8, 20, 10, 0, 0), new DateTime(2020, 8, 20, 13, 0, 0), "Gent", "Gent", new List<Car>() { car }, DateTime.Now, 6.0);

            // Check
            Assert.AreNotEqual(manager.GetAllReservations().Count, 0);
            Reservation reservation = manager.GetAllReservations().Last();
            Assert.AreEqual(reservation.ClientID, client.ID);
            Assert.AreEqual(reservation.Arrangement, ReservationArrangementType.AIRPORT);
            Assert.AreEqual(reservation.StartLocation, "Gent");
            Assert.AreEqual(reservation.EndLocation, "Gent");
            Assert.AreEqual(reservation.ReservationDate, new DateTime(2020, 8, 20, 10, 0, 0));
            Assert.AreEqual(reservation.ReservedUntil, new DateTime(2020, 8, 20, 13, 0, 0));
            Assert.IsNotNull(reservation.InvoiceID);
        }

        [TestMethod]
        public void TestRentalManagerGetAllReservation()
        {
            // Setup
            RentalManager manager = new RentalManager(new UnitOfWork(new RentalContext("development")));

            // Check
            Assert.AreNotEqual(manager.GetAllReservations().Count, 0);
        }

        [TestMethod]
        public void TestRentalManagerRemoveInvoice()
        {
            // Pre-setup
            RentalManager manager = new RentalManager(new UnitOfWork(new RentalContext("development")));
            manager.AddClient("Tim", "De Smet", "tim.de.smet@hotmail.be", "0493100289", "Azaleastraat", "57", "", "9940", "Evergem", "Belgium", ClientType.AGENCY, "Jetstax", "BE0730.671.009");
            Assert.AreNotEqual(manager.GetAllClients().Count, 0);
            Client client = manager.GetAllClients().Last();
            manager.AddCar("Porsche", "Cayenne Limousine", "White", 310, 1500, 1200, 1600, true);
            Assert.AreNotEqual(manager.GetAllCars().Count, 0);
            Car car = manager.GetAllCars().Last();
            manager.AddReservation(client, ReservationArrangementType.AIRPORT, new DateTime(2020, 8, 20, 10, 0, 0), new DateTime(2020, 8, 20, 13, 0, 0), "Gent", "Gent", new List<Car>() { car }, DateTime.Now, 6.0);

            // Setup
            Assert.AreNotEqual(manager.GetAllReservations().Count, 0);
            Reservation reservation = manager.GetAllReservations().Last();
            int count = manager.GetAllReservations().Count;
            manager.RemoveReservation(reservation);

            // Check
            Assert.AreEqual(manager.GetAllReservations().Count, count - 1);
        }
    }
}
