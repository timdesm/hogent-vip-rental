using DomainLayer.Domain;
using DomainLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer.Repositories
{
    public class CarRepository : ICarRepository
    {
        private RentalContext context;

        public CarRepository(RentalContext context)
        {
            this.context = context;
        }

        public void AddCar(Car car)
        {
            context.Cars.Add(car);
        }

        public IEnumerable<Car> FindAll()
        {
            return context.Cars.OrderBy(c => c.ID).AsEnumerable<Car>();
        }

        public Car Find(int id)
        {
            return context.Cars.Find(id);
        }

        public IEnumerable<Car> CarsAvailable(DateTime from, DateTime until, double hourRange)
        {
            from.AddHours(-hourRange);
            until.AddHours(hourRange);
            List<Reservation> reservations = context.Reservations.Where(r => r.ReservationDate >= from && r.ReservedUntil <= until).ToList();
            List<Car> reservedCars = context.CarReservations.Where(c => reservations.Contains(c.Reservation)).Select(c => c.Car).ToList();
            return context.Cars.Where(c => c.Available).Where(c => !reservedCars.Contains(c)).AsEnumerable<Car>();
        }
    }
}
