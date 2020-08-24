using DataLayer;
using DomainLayer.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace AppUnitTest
{
    [TestClass]
    public class WPFCarTesting
    {
        [TestMethod]
        public void TestRentalManagerAddCar()
        {
            // Setup
            RentalManager manager = new RentalManager(new UnitOfWork(new RentalContext("development")));
            manager.AddCar("Porsche", "Cayenne Limousine", "White", 310, 1500, 1200, 1600, true);

            // Check
            Assert.AreNotEqual(manager.GetAllCars().Count, 0);
            Car car = manager.GetAllCars().Last();
            Assert.AreEqual(car.Brand, "Porsche");
            Assert.AreEqual(car.Type, "Cayenne Limousine");
            Assert.AreEqual(car.Color, "White");
            Assert.AreEqual(car.PriceFirst, 310);
            Assert.AreEqual(car.PriceNight, 1500);
            Assert.AreEqual(car.PriceWedding, 1200);
            Assert.AreEqual(car.PriceWellness, 1600);
            Assert.AreEqual(car.Available, true);
        }

        [TestMethod]
        public void TestCarGetPrice()
        {
            // Setup
            RentalManager manager = new RentalManager(new UnitOfWork(new RentalContext("development")));
            manager.AddCar("Porsche", "Cayenne Limousine", "White", 310, 1500, 1200, 1600, true);

            // Check
            Assert.AreNotEqual(manager.GetAllCars().Count, 0);
            Car car = manager.GetAllCars().Last();
            Assert.AreEqual(car.GetPrice(new DateTime(2020, 8, 20, 10, 0, 0), new DateTime(2020, 8, 20, 13, 0, 0), ReservationArrangementType.AIRPORT), 715);
        }

        [TestMethod]
        public void TestRentalManagerGetAllCars()
        {
            RentalManager manager = new RentalManager(new UnitOfWork(new RentalContext("development")));
            Assert.AreNotEqual(manager.GetAllCars().Count, 0);
        }
    }
}
