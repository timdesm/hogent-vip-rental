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

        public void SaveChanges()
        {
            uow.Complete();
        }

        public void AddClient(string firstName, string lastName, string email, string phone, string addStreet, string addNumber, string addBox, string addZip, string addCity, string addCountry, ClientType type, string company, string vatNumber)
        {
            if(String.IsNullOrWhiteSpace(firstName) || String.IsNullOrWhiteSpace(lastName)) throw new DomainException("First and Last name can't be empty");
            if (!String.IsNullOrWhiteSpace(email)) if (!RegexUtilities.IsValidEmail(email)) throw new DomainException("Email invalid value");
            if(!RegexUtilities.IsValidPhoneNumber(phone, false)) throw new DomainException("Phone invalid value");
            if (type != ClientType.PRIVATE && type != ClientType.VIP) if(vatNumber == null) throw new DomainException("VAT Number can't be empty for that account type");
            if (type == ClientType.PRIVATE || type == ClientType.VIP) if (vatNumber != null) { vatNumber = ""; company = ""; }

            uow.Clients.AddClient(new Client(type, firstName, lastName, email, phone, addStreet, addNumber, addBox, addZip, addCity, addCountry, company, vatNumber));
            uow.Complete();
        }

        public void UpdateClient(Client client)
        {
            uow.Clients.UpdateClient(client);
            uow.Complete();
        }

        public void UpdateCar(Car car)
        {
            uow.Cars.UpdateCar(car);
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

        public Invoice GetInvoice(int id)
        {
            return uow.Invoices.Find(id);
        }

        public Reservation GetReservation(int id)
        {
            return uow.Reservations.Find(id);
        }

        public List<Car> GetReservationCars(int reservationID)
        {
            List<Car> cars = new List<Car>();
            foreach(CarReservation cr in uow.CarReservations.ReservationCars(reservationID).ToList())
                cars.Add(uow.Cars.Find(cr.CarID));
            return cars;
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

        public Client GetLastClient()
        {
            return uow.Clients.GetLastClient();
        }

        public Car GetLastCar()
        {
            return uow.Cars.GetLastCar();
        }

        public Invoice GetLastInvoice()
        {
            return uow.Invoices.GetLastInvoice();
        }

        public List<Car> GetAllCars()
        {
            return uow.Cars.FindAll().ToList();
        }

        public List<Invoice> GetAllInvoices()
        {
            return uow.Invoices.FindAll().ToList();
        }

        public void RemoveClient(Client client)
        {
            uow.Clients.RemoveClient(client);
            uow.Complete();
        }

        public void UpdateInvoice(Invoice invoice)
        {
            uow.Invoices.UpdateInvoice(invoice);
            uow.Complete();
        }

        public List<Car> GetAvailableCars(DateTime from, DateTime until, double hourRange)
        {
            return uow.Cars.CarsAvailable(from, until, hourRange).ToList();
        }

        public List<Reservation> GetAllReservations()
        {
            return uow.Reservations.FindAll().ToList();
        }

        public List<InvoiceItem> GetInvoiceItems(int invoiceID)
        {
            return uow.InvoiceItems.FindInvoiceItems(invoiceID).ToList();
        }

        public List<Reservation> GetReservations(DateTime from, DateTime until)
        {
            return uow.Reservations.Find(from, until).ToList();
        }

        public List<Reservation> GetActiveReservations()
        {
            return uow.Reservations.FindActive().ToList();
        }

        public List<Invoice> GetUnpaidInvoices()
        {
            return uow.Invoices.FindUnpaid().ToList();
        }

        public void RemoveReservation(int id)
        {
            Reservation reservation = uow.Reservations.Find(id);
            RemoveReservation(reservation);
        }

        public void RemoveReservation(Reservation reservation)
        {
            List<CarReservation> carReservations = uow.CarReservations.ReservationCars(reservation.ID).ToList();
            foreach (CarReservation cr in carReservations)
                uow.CarReservations.RemoveCarReservation(cr);
            RemoveInvoice(uow.Invoices.Find(reservation.InvoiceID));
            uow.Reservations.RemoveReservation(reservation);
            uow.Complete();
        }

        public void UpdateCarReservations(Reservation reservation, List<Car> cars)
        {
            List<CarReservation> carReservations = uow.CarReservations.ReservationCars(reservation.ID).ToList();
            foreach (CarReservation cr in carReservations)
                uow.CarReservations.RemoveCarReservation(cr);
            uow.Complete();
            AddCarReservations(reservation, cars);
        }

        public void UpdateReservation(Reservation reservation)
        {
            uow.Reservations.UpdateReservation(reservation);
            uow.Complete();
        }

        public void RemoveInvoice(int id)
        {
            uow.Invoices.RemoveInvoice(id);
            List<InvoiceItem> invoiceItems = uow.InvoiceItems.FindInvoiceItems(id).ToList();
            foreach (InvoiceItem ii in invoiceItems)
                uow.InvoiceItems.RemoveInvoiceItem(ii);
            uow.Complete();
        }

        public void RemoveInvoice(Invoice invoice)
        {
            int invoiceID = invoice.ID;
            uow.Invoices.RemoveInvoice(invoice);
            List<InvoiceItem> invoiceItems = uow.InvoiceItems.FindInvoiceItems(invoiceID).ToList();
            foreach (InvoiceItem ii in invoiceItems)
                uow.InvoiceItems.RemoveInvoiceItem(ii);
            uow.Complete();
        }

        public Invoice AddInvoice(Client client, ReservationArrangementType arrangement, DateTime from, DateTime until, List<Car> cars, Double vatPercent)
        {
            TimeSpan diffTime = until - from;
            Double totalHours = diffTime.TotalHours;

            List<InvoiceItem> invoiceItems = new List<InvoiceItem>();
            foreach (Car car in cars)
            {
                Double price = car.GetPrice(from, until, arrangement);

                InvoiceItem ii;
                switch (arrangement)
                {
                    case ReservationArrangementType.NIGHT:
                        if (car.PriceNight <= 0) throw new DomainException("Car with ID " + car.ID + " can't be booked for the night arragement");
                        ii = new InvoiceItem(1, "#" + car.ID + " " + car.Brand + " " + car.Type + " (" + car.Color + ") - Night " + totalHours + "h", price, price, 0);
                        break;
                    case ReservationArrangementType.WEDDING:
                        if (car.PriceWedding <= 0) throw new DomainException("Car with ID " + car.ID + " can't be booked for the wedding arragement");
                        ii = new InvoiceItem(1, "#" + car.ID + " " + car.Brand + " " + car.Type + " (" + car.Color + ") - Wedding " + totalHours + "h", price, price, 0);
                        break;
                    case ReservationArrangementType.WELLNESS:
                        if (car.PriceWellness <= 0) throw new DomainException("Car with ID " + car.ID + " can't be booked for the wellness arragement");
                        ii = new InvoiceItem(1, "#" + car.ID + " " + car.Brand + " " + car.Type + " (" + car.Color + ") - Wellness " + totalHours + "h", price, price, 0);
                        break;
                    default:
                        ii = new InvoiceItem(1, "#" + car.ID + " " + car.Brand + " " + car.Type + " (" + car.Color + ") " + totalHours + "h", price, price, 0);
                        break;
                }
                uow.InvoiceItems.AddInvoiceItem(ii);
                invoiceItems.Add(ii);
            }

            Invoice invoice = new Invoice(client.ID, DateTime.Now, invoiceItems, client.GetDiscount(uow.Clients.GetYealyReservations(client, DateTime.Now.Year)), vatPercent);
            uow.Invoices.AddInvoice(invoice);
            uow.Complete();
            foreach (InvoiceItem ii in invoiceItems)
                ii.InvoiceID = invoice.ID;
            uow.Complete();

            return invoice;
        }

        public void AddCarReservations(Reservation reservation, List<Car> cars)
        {
            List<CarReservation> carReservations = new List<CarReservation>();
            foreach (Car car in cars)
            {
                CarReservation cr = new CarReservation(reservation.ID, car.ID);
                uow.CarReservations.AddCarReservation(cr);
                carReservations.Add(cr);
            }
            reservation.CarReservations = carReservations;
            uow.Complete();
        }

        public Reservation AddReservation(Client client, ReservationArrangementType arrangement, DateTime from, DateTime until, String startLocation, String endLocation, List<Car> cars, DateTime orderDate, Double vatPercent)
        {
            if(from == null || until == null) throw new DomainException("The pickup and return dates must be given");
            TimeSpan diffTime = until - from;
            double amountHours = diffTime.TotalHours;

            if (client == null) throw new DomainException("There must a client be given");
            if (amountHours > 11) throw new DomainException("Reservations can't be more than 11 hours long");
            if (arrangement == ReservationArrangementType.NIGHT && !TimeUtilities.IsBetweenTimes(from, TimeSpan.FromHours(20), TimeSpan.FromHours(0))) throw new DomainException("Night arragement must start between 20:00 and 00:00");
            if (arrangement == ReservationArrangementType.WEDDING && !TimeUtilities.IsBetweenTimes(from, TimeSpan.FromHours(7), TimeSpan.FromHours(15))) throw new DomainException("Wedding arragement must start between 07:00 and 15:00");
            if (arrangement == ReservationArrangementType.WELLNESS && !TimeUtilities.IsBetweenTimes(from, TimeSpan.FromHours(7), TimeSpan.FromHours(12))) throw new DomainException("Wellness arragement must start between 07:00 and 12:00");
            if (cars.Count <= 0) throw new DomainException("You must select at least one car");

            Invoice invoice = AddInvoice(client, arrangement, from, until, cars, vatPercent);
            Reservation reservation = new Reservation(client, new List<CarReservation>(), orderDate, from, startLocation, endLocation, arrangement, until, DateTime.MinValue, invoice);
            uow.Reservations.AddReservation(reservation);
            uow.Complete();
            AddCarReservations(reservation, cars);

            return reservation;
        }
    }
}
