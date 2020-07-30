using DomainLayer.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainLayer.Repositories
{
    public interface ICarRepository
    {
        void AddCar(Car car);
        IEnumerable<Car> FindAll();
        Car Find(int ID);
    }
}
