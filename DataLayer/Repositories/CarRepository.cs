using DomainLayer.Domain;
using DomainLayer.Repositories;
using System;
using System.Collections.Generic;
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

    }
}
