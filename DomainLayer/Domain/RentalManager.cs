using DomainLayer.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DomainLayer.Domain
{
    public class RentalManager
    {
        private IUnitOfWork uow;

        public RentalManager(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public void AddClient(string firstName, string lastName, string email, string phone, string addStreet, string addNumber, string addBox, string addZip, string addCity, string addCountry, ClientType type, string company, string vatNumber)
        {
            if(firstName == null || lastName == null) throw new DomainException("First and Last name can't be empty");
            if (email != null) if (!RegexUtilities.IsValidEmail(email)) throw new DomainException("Email invalid value");
            if(!RegexUtilities.IsValidPhoneNumber(phone, false)) throw new DomainException("Phone invalid value");
            if (type != ClientType.PRIVATE && type != ClientType.VIP) if(vatNumber == null) throw new DomainException("VAT Number can't be empty for that account type");
            if (type == ClientType.PRIVATE || type == ClientType.VIP) if (vatNumber != null) { vatNumber = ""; company = ""; }

            uow.Clients.AddClient(new Client(type, firstName, lastName, email, phone, addStreet, addNumber, addBox, addZip, addCity, addCountry, company, vatNumber));
            uow.Complete();
        }

        public List<Client> GetAllClients()
        {
            return uow.Clients.FindAll().ToList();
        }

        public List<Client> GetNewestClients(int amount)
        {
            return uow.Clients.FindNewest(amount).ToList();
        }

        public void AddCar(string brand, string type, string color, double priceFirst, double priceNight, double priceWedding, double priceWellness, bool available)
        {
            if(string.IsNullOrWhiteSpace(brand) || string.IsNullOrWhiteSpace(brand)) throw new DomainException("Brand and type cant't be empty");
            if(priceFirst < 0 || priceNight < 0 || priceWedding < 0 || priceWellness < 0) throw new DomainException("Price must be 0 or higher");

            uow.Cars.AddCar(new Car(brand, type, color, priceFirst, priceNight, priceWedding, priceWellness, available));
            uow.Complete();
        }

        public List<Car> GetAllCars()
        {
            return uow.Cars.FindAll().ToList();
        }

        public List<Car> GetAvailableCars(DateTime from, DateTime until, double hourRange)
        {
            return uow.Cars.CarsAvailable(from, until, hourRange).ToList();
        }
    }
}
