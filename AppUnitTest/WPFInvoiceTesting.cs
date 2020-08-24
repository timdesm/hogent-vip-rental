using DataLayer;
using DomainLayer.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppUnitTest
{
    [TestClass]
    public class WPFInvoiceTesting
    {
        [TestMethod]
        public void TestRentalManagerAddInvoice()
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
            manager.AddInvoice(client, ReservationArrangementType.AIRPORT, new DateTime(2020, 8, 20, 10, 0, 0), new DateTime(2020, 8, 20, 13, 0, 0), new List<Car>() { car }, 6.0);

            // Check
            Invoice invoice = manager.GetAllInvoices().Last();
            List<InvoiceItem> invoiceItems = manager.GetInvoiceItems(invoice.ID);
            Assert.AreEqual(invoice.ClientID, client.ID);
            Assert.AreEqual(invoice.DiscountPercent, 0);
            Assert.AreEqual(invoice.Discount, 0);
            Assert.AreEqual(invoice.VATPercent, 6.0);
            Assert.AreEqual(invoice.VAT, 42.9);
            Assert.AreEqual(invoice.SubTotal, 715);
            Assert.AreEqual(invoice.TotalExc, 715);
            Assert.AreEqual(invoice.TotalInc, 757.9);
            Assert.AreEqual(invoice.PaymentDue, 757.9);
            Assert.AreEqual(invoiceItems.Count, 1);
        }

        [TestMethod]
        public void TestRentalManagerGetAllInvoices()
        {
            RentalManager manager = new RentalManager(new UnitOfWork(new RentalContext("development")));
            Assert.AreNotEqual(manager.GetAllInvoices().Count, 0);
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
            manager.AddInvoice(client, ReservationArrangementType.AIRPORT, new DateTime(2020, 8, 20, 10, 0, 0), new DateTime(2020, 8, 20, 13, 0, 0), new List<Car>() { car }, 6.0);

            // Setup
            Assert.AreNotEqual(manager.GetAllInvoices().Count, 0);
            Invoice invoice = manager.GetAllInvoices().Last();
            int count = manager.GetAllInvoices().Count;
            manager.RemoveInvoice(invoice);

            // Check
            Assert.AreEqual(manager.GetAllInvoices().Count, count - 1);
        }
    }
}
