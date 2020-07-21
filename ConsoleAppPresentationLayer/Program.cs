using DataLayer;
using DomainLayer.Domain;
using System;

namespace ConsoleAppPresentationLayer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            RentalManager m = new RentalManager(new UnitOfWork(new RentalContext("production")));
            
        }
    }
}
