using DataLayer;
using DomainLayer.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace AppUnitTest
{
    [TestClass]
    public class WPFClientTesting
    {
        [TestMethod]
        public void TestRentalManagerAddClient()
        {
            // Setup
            RentalManager manager = new RentalManager(new UnitOfWork(new RentalContext("development")));
            manager.AddClient("Tim", "De Smet", "tim.de.smet@hotmail.be", "0493100289", "Azaleastraat", "57", "", "9940", "Evergem", "Belgium", ClientType.AGENCY, "Jetstax", "BE0730.671.009");

            // Check
            Assert.AreNotEqual(manager.GetAllClients().Count, 0);
            Client client = manager.GetAllClients().Last();
            Assert.AreEqual(client.FirstName, "Tim");
            Assert.AreEqual(client.LastName, "De Smet");
            Assert.AreEqual(client.Email, "tim.de.smet@hotmail.be");
            Assert.AreEqual(client.Phone, "0493100289");
            Assert.AreEqual(client.AddressStreet, "Azaleastraat");
            Assert.AreEqual(client.AddressNumber, "57");
            Assert.AreEqual(client.AddressBus, "");
            Assert.AreEqual(client.AddressZip, "9940");
            Assert.AreEqual(client.AddressCity, "Evergem");
            Assert.AreEqual(client.AddressCounty, "Belgium");
            Assert.AreEqual(client.Type, ClientType.AGENCY);
            Assert.AreEqual(client.CompanyName, "Jetstax");
            Assert.AreEqual(client.VATNumber, "BE0730.671.009");
        }

        [TestMethod]
        public void TestRentalManagerClientGetDiscount()
        {
            // Setup
            RentalManager manager = new RentalManager(new UnitOfWork(new RentalContext("development")));
            manager.AddClient("Tim", "De Smet", "tim.de.smet@hotmail.be", "0493100289", "Azaleastraat", "57", "", "9940", "Evergem", "Belgium", ClientType.PLANNER, "Jetstax", "BE0730.671.009");

            // Check
            Assert.AreNotEqual(manager.GetAllClients().Count, 0);
            Client client = manager.GetAllClients().Last();
            Assert.AreEqual(client.GetDiscount(0), 0);
            Assert.AreEqual(client.GetDiscount(4), 0);
            Assert.AreEqual(client.GetDiscount(5), 7.5);
            Assert.AreEqual(client.GetDiscount(8), 7.5);
            Assert.AreEqual(client.GetDiscount(10), 10);
            Assert.AreEqual(client.GetDiscount(11), 10);
            Assert.AreEqual(client.GetDiscount(15), 12.5);
            Assert.AreEqual(client.GetDiscount(18), 12.5);
            Assert.AreEqual(client.GetDiscount(20), 15);
            Assert.AreEqual(client.GetDiscount(24), 15);
            Assert.AreEqual(client.GetDiscount(25), 25);
            Assert.AreEqual(client.GetDiscount(35), 25);
        }

        [TestMethod]
        public void TestRentalManagerGetAllClients()
        {
            RentalManager manager = new RentalManager(new UnitOfWork(new RentalContext("development")));
            Assert.AreNotEqual(manager.GetAllClients().Count, 0);
        }

        [TestMethod]
        public void TestRentalManagerRemoveClient()
        {
            // Pre-setup
            RentalManager manager = new RentalManager(new UnitOfWork(new RentalContext("development")));
            manager.AddCar("Porsche", "Cayenne Limousine", "White", 310, 1500, 1200, 1600, true);

            // Setup
            Assert.AreNotEqual(manager.GetAllClients().Count, 0);
            Client client = manager.GetAllClients().Last();
            int count = manager.GetAllClients().Count;
            manager.RemoveClient(client);

            // Check
            Assert.AreEqual(manager.GetAllClients().Count, count - 1);
        }
    }
}
