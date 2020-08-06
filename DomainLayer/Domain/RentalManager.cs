using DomainLayer.Repositories;
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

        public Client GetClient(int id)
        {
            return uow.Clients.Find(id);
        }

        public void AddCar(string brand, string type, string color, double priceFirst, double priceNight, double priceWedding, double priceWellness, bool available)
        {
            if(string.IsNullOrWhiteSpace(brand) || string.IsNullOrWhiteSpace(brand)) throw new DomainException("Brand and type cant't be empty");
            if(priceFirst < 0 || priceNight < 0 || priceWedding < 0 || priceWellness < 0) throw new DomainException("Price must be 0 or higher");

            uow.Cars.AddCar(new Car(brand, type, color, priceFirst, priceNight, priceWedding, priceWellness, available));
            uow.Complete();
        }

        public Car GetCar(int id)
        {
            return uow.Cars.Find(id);
        }

        public List<Car> GetAllCars()
        {
            return uow.Cars.FindAll().ToList();
        }

        public List<Car> GetAvailableCars(DateTime from, DateTime until, double hourRange)
        {
            return uow.Cars.CarsAvailable(from, until, hourRange).ToList();
        }

        public List<Reservation> GetAllReservations()
        {
            return uow.Reservations.FindAll().ToList();
        }

        public void AddReservation(Client client, ReservationArrangementType arrangement, DateTime from, DateTime until, String startLocation, String endLocation, List<Car> cars, DateTime orderDate, Double vatPercent)
        {
            if(from == null || until == null) throw new DomainException("The pickup and return dates must be given");
            TimeSpan diffTime = until - from;
            double amountHours = diffTime.TotalHours;

            if (client == null) throw new DomainException("There must a client be given");
            if (amountHours > 11) throw new DomainException("Reservations can't be more than 11 hours long");
            if (arrangement == ReservationArrangementType.NIGHT && !IsBetweenTimes(from, TimeSpan.FromHours(20), TimeSpan.FromHours(0))) throw new DomainException("Night arragement must start between 20:00 and 00:00");
            if (arrangement == ReservationArrangementType.WEDDING && !IsBetweenTimes(from, TimeSpan.FromHours(7), TimeSpan.FromHours(15))) throw new DomainException("Wedding arragement must start between 07:00 and 15:00");
            if (arrangement == ReservationArrangementType.WELLNESS && !IsBetweenTimes(from, TimeSpan.FromHours(7), TimeSpan.FromHours(12))) throw new DomainException("Wellness arragement must start between 07:00 and 12:00");
            if (cars.Count <= 0) throw new DomainException("You must select at least one car");

            List<InvoiceItem> invoiceItems = new List<InvoiceItem>();
            foreach(Car car in cars)
            {
                Double price = GetCarPrice(from, until, car, arrangement);

                InvoiceItem ii;
                switch(arrangement)
                {
                    case ReservationArrangementType.NIGHT:
                        if(car.PriceNight <= 0) throw new DomainException("Car with ID " + car.ID + " can't be booked for the night arragement");
                        ii = new InvoiceItem(1, "#" + car.ID + " " + car.Brand + " " + car.Type + " (" + car.Color + ") - Night", price, price);
                        break;
                    case ReservationArrangementType.WEDDING:
                        if (car.PriceWedding <= 0) throw new DomainException("Car with ID " + car.ID + " can't be booked for the wedding arragement");
                        ii = new InvoiceItem(1, "#" + car.ID + " " + car.Brand + " " + car.Type + " (" + car.Color + ") - Wedding", price, price);
                        break;
                    case ReservationArrangementType.WELLNESS:
                        if (car.PriceWellness <= 0) throw new DomainException("Car with ID " + car.ID + " can't be booked for the wellness arragement");
                        ii = new InvoiceItem(1, "#" + car.ID + " " + car.Brand + " " + car.Type + " (" + car.Color + ") - Wellness", price, price);
                        break;
                    default: 
                        ii = new InvoiceItem(1, "#" + car.ID + " " + car.Brand + " " + car.Type + " (" + car.Color + ")", price, price);
                        break;
                }
                uow.InvoiceItems.AddInvoiceItem(ii);
                invoiceItems.Add(ii);
            }

            Invoice invoice = new Invoice(client, orderDate, invoiceItems, GetDiscount(client), vatPercent);
            uow.Invoices.AddInvoice(invoice);

            Reservation reservation = new Reservation(client, new List<CarReservation>(), orderDate, from, startLocation, endLocation, arrangement, until, DateTime.MinValue, invoice);
            uow.Reservations.AddReservation(reservation);

            List<CarReservation> carReservations = new List<CarReservation>();
            foreach(Car car in cars)
            {
                CarReservation cr = new CarReservation(reservation, car);
                uow.CarReservations.AddCarReservation(cr);
                carReservations.Add(cr);
            }
            reservation.CarReservations = carReservations;
            uow.Complete();
        }

        private double GetDiscount(Client client)
        {
            double discount = 0;
            int totalReservations = uow.Clients.GetYealyReservations(client, DateTime.Now.Year);

            switch(client.Type)
            {
                case ClientType.VIP:
                    if (totalReservations >= 2 && totalReservations < 7) discount = 5.0;
                    else if(totalReservations >= 7 && totalReservations < 15) discount = 7.5;
                    else if (totalReservations >= 15) discount = 10.0;
                    break;
                case ClientType.PLANNER:
                    if (totalReservations >= 5 && totalReservations < 10) discount = 7.5;
                    else if (totalReservations >= 10 && totalReservations < 15) discount = 10.0;
                    else if (totalReservations >= 15 && totalReservations < 20) discount = 12.5;
                    else if (totalReservations >= 20 && totalReservations < 25) discount = 15.0;
                    else if (totalReservations >= 25) discount = 25.0;
                    break;
            }
            return discount;
        }

        public double GetCarPrice(DateTime from, DateTime until, Car car, ReservationArrangementType arrangement)
        {
            Double price = 0.0;

            TimeSpan diffTime = until - from;
            Double totalHours = diffTime.TotalHours;
            
            switch (arrangement)
            {
                case ReservationArrangementType.NIGHT:
                    if (totalHours > 7)
                        from.AddHours(7);
                    break;
                case ReservationArrangementType.WEDDING:
                    if (totalHours > 7)
                        from.AddHours(7);
                    break;
                case ReservationArrangementType.WELLNESS:
                    if (totalHours > 10)
                        from.AddHours(10);
                    break;
                default:
                    break;
            }

            TimeSpan fromTime = from.TimeOfDay;
            TimeSpan untilTime = until.TimeOfDay;

            Double normalHours = 0.0;
            Double nightHours = 0.0;

            while(fromTime < untilTime)
            {
                fromTime = fromTime.Add(TimeSpan.FromHours(1));
                if (fromTime >= TimeSpan.FromHours(22) || fromTime <= TimeSpan.FromHours(7))
                    nightHours += 1;
                else
                    normalHours += 1;
            }

            switch (arrangement)
            {
                case ReservationArrangementType.NIGHT:
                    price = car.PriceNight;
                    if (totalHours > 7)
                    {
                        price += normalHours * (car.PriceFirst * 0.65);
                        price += nightHours * (car.PriceFirst * 1.4);
                    }
                    break;
                case ReservationArrangementType.WEDDING:
                    price = car.PriceWedding;
                    if (totalHours > 7)
                    {
                        price += normalHours * (car.PriceFirst * 0.65);
                        price += nightHours * (car.PriceFirst * 1.4);
                    }
                    break;
                case ReservationArrangementType.WELLNESS:
                    price = car.PriceWedding;
                    if (totalHours > 10)
                    {
                        price += normalHours * (car.PriceFirst * 0.65);
                        price += nightHours * (car.PriceFirst * 1.4);
                    }
                    break;
                default: 
                    if(!IsBetweenTimes(from, TimeSpan.FromHours(22), TimeSpan.FromHours(6))) {
                        price = car.PriceFirst;
                        normalHours -= 1;
                    }
                    price += normalHours * (car.PriceFirst * 0.65);
                    price += nightHours * (car.PriceFirst * 1.4);
                    break;
            }

            return Math.Round(price / 5.0) * 5;
        }

        private Boolean IsBetweenTimes(DateTime datum, TimeSpan? start, TimeSpan? end)
        {
            Boolean isBetween = false;
            DateTime StartDate = DateTime.Today;
            DateTime EndDate = DateTime.Today;
            if (start >= end)
                EndDate = EndDate.AddDays(1);
            StartDate = StartDate.Date + start.Value;
            EndDate = EndDate.Date + end.Value;
            if ((datum >= StartDate) && (datum <= EndDate))
                isBetween = true;
            return isBetween;
        }
    }
}
