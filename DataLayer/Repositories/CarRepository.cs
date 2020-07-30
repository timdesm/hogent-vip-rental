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
    }
}
