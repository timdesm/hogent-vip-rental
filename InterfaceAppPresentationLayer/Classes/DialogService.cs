using DataLayer;
using DomainLayer.Domain;
using DomainLayer.Utilities;
using InterfaceAppPresentationLayer.Dialogs;
using ModernWpf.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

namespace InterfaceAppPresentationLayer.Classes
{
    public class DialogService
    {
        public async static void OpenReservationViewDialog(Reservation reservation)
        {
            ReservationView dialog = new ReservationView(reservation);
            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                dialog.Hide();
                OpenReservationEditDialog(reservation);
            }
        }

        public async static void OpenReservationEditDialog(Reservation reservation)
        {
            try
            {
                ReservationEdit dialog = new ReservationEdit(reservation);
                var result = await dialog.ShowAsync();
                if (result == ContentDialogResult.Primary)
                {
                    if (dialog.Client.SelectedIndex < 0) { MainWindow.DisplayThrowbackDialog("Reservation Edit Error", "You must selected a client out of the list"); return; }
                    if (!int.TryParse(dialog.Client.SelectedItem.ToString().Split(" ")[0].Substring(1), out int clientID)) { MainWindow.DisplayThrowbackDialog("Reservation Edit Error", "Something went wrong while retrieving the client from the list"); return; }
                    RentalManager manager = new RentalManager(new UnitOfWork(new RentalContext()));
                    List<Car> oldRentedCars = manager.GetReservationCars(reservation.ID);
                    Client client = manager.GetClient(clientID);
                    if (dialog.Arrangement.SelectedIndex < 0 || !Enum.TryParse(typeof(ReservationArrangementType), dialog.Arrangement.SelectedItem.ToString().ToUpper(), out object typeObj)) { MainWindow.DisplayThrowbackDialog("Reservation Edit Error", "You must select a arrangement from the list"); return; }
                    if (dialog.FromDate.SelectedDate == null || dialog.FromTime.SelectedIndex < 0 || !TimeSpan.TryParse(dialog.FromTime.SelectedItem.ToString(), out TimeSpan fromTime)) { MainWindow.DisplayThrowbackDialog("Reservation Edit Error", "You must select pickup date and time"); return; }
                    if (dialog.UntilDate.SelectedDate == null || dialog.UntilTime.SelectedIndex < 0 || !TimeSpan.TryParse(dialog.UntilTime.SelectedItem.ToString(), out TimeSpan untilTime)) { MainWindow.DisplayThrowbackDialog("Reservation Edit Error", "You must select return date and time"); return; }
                    if (dialog.StartLocation.SelectedIndex < 0) { MainWindow.DisplayThrowbackDialog("Reservation Edit Error", "You must select pickup location"); return; }
                    if (dialog.EndLocation.SelectedIndex < 0) { MainWindow.DisplayThrowbackDialog("Reservation Edit Error", "You must select return location"); return; }
                    if (dialog.CarTable.SelectedItems.Count <= 0) { MainWindow.DisplayThrowbackDialog("Reservation Edit Error", "You must select at least one car"); return; }
                    if (client.Type == ClientType.PRIVATE && dialog.CarTable.SelectedItems.Count >1) { MainWindow.DisplayThrowbackDialog("Reservation Edit Error", "Private clients can select a maximum of 1 car"); return; }

                    ReservationArrangementType arragement = (ReservationArrangementType)typeObj;
                    String startLocation = dialog.StartLocation.SelectedItem.ToString();
                    String endLocation = dialog.EndLocation.SelectedItem.ToString();

                    DateTime fromDate = dialog.FromDate.SelectedDate.Value + fromTime;
                    DateTime untilDate = dialog.UntilDate.SelectedDate.Value + untilTime;

                    DateTime returnedDate = DateTime.MinValue;
                    if (dialog.ReturnedDate.SelectedDate != null && dialog.ReturnedTime.SelectedIndex >= 0 && TimeSpan.TryParse(dialog.ReturnedTime.SelectedItem.ToString(), out TimeSpan returnedTime)) { returnedDate = (DateTime)dialog.ReturnedDate.SelectedDate.Value + returnedTime; }

                    List<int> carIDs = new List<int>();
                    List<int> selectedCars = dialog.CarTable.SelectedItems.Cast<DataRowView>().Select(x => dialog.AvailableCarsTable.Rows.IndexOf(x.Row)).ToList();
                    foreach (int i in selectedCars)
                    {
                        if (dialog.AvailableCarsTable.Rows.Count > 1)
                        {
                            DataRow row = dialog.AvailableCarsTable.Rows[i];
                            carIDs.Add((int)row[0]);
                        }
                    }

                    List<Car> cars = new List<Car>();
                    foreach (int carID in carIDs)
                        cars.Add(manager.GetCar(carID));

                    bool areCarsChanged = !(cars.All(oldRentedCars.Contains) && cars.Count == oldRentedCars.Count);

                    if (areCarsChanged)
                    {
                        dialog.Hide();
                        ContentDialog confirmDialog = new ContentDialog
                        {
                            Title = "Override invoice?",
                            Content = "You changed the reserved cars, this will regenerate a new invoice and remove the old one. Are you sure you want to proceed?",
                            PrimaryButtonText = "Yes",
                            CloseButtonText = "No"
                        };
                        var confirmResult = await confirmDialog.ShowAsync();
                        if (confirmResult == ContentDialogResult.Primary)
                        {
                            RentalManager carsManager = new RentalManager(new UnitOfWork(new RentalContext()));
                            carsManager.RemoveInvoice(reservation.InvoiceID);
                            DateTime untilInvoice = untilDate;
                            if (returnedDate > DateTime.MinValue)
                                untilInvoice = returnedDate;
                            reservation.InvoiceID = carsManager.AddInvoice(client, arragement, fromDate, untilInvoice, cars, 6.0).ID;
                            carsManager.UpdateCarReservations(reservation, cars);
                        }
                        else
                        {
                            return;
                        }
                    }

                    reservation.Client = client;
                    reservation.ClientID = client.ID;
                    reservation.Arrangement = arragement;
                    reservation.StartLocation = startLocation;
                    reservation.EndLocation = endLocation;
                    reservation.ReservationDate = fromDate;
                    reservation.ReservedUntil = untilDate;
                    reservation.ReservationEnded = returnedDate;

                    manager.UpdateReservation(reservation);

                    MainWindow.DisplayThrowbackDialog("Reservation Saved", "All changes have been saved"); return;
                }
            }
            catch(Exception ex) { 
                MainWindow.DisplayThrowbackDialog("System Error", ex.Message + "\n" + "Stack trace has been written to the logs");
                LogService.WriteLog(new List<String>() { "Reservation Edit Save Exeption: ", ex.Message, " ", ex.InnerException.ToString(), ex.StackTrace });
            }
        }

        public async static void OpenClientViewDialog(Client client)
        {
            ClientView dialog = new ClientView(client);
            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                dialog.Hide();
                OpenClientEditDialog(client);
            }
        }

        public async static void OpenClientEditDialog(Client client)
        {
            ClientEdit dialog = new ClientEdit(client);
            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                string firstName = dialog.FirstName.Text;
                string lastName = dialog.LastName.Text;
                string email = dialog.Email.Text;
                string phone = dialog.Phone.Text;
                string street = dialog.Street.Text;
                string number = dialog.HouseNumber.Text;
                string box = dialog.Box.Text;
                string city = dialog.City.Text;
                string zip = dialog.Zip.Text;
                string country = "";
                string company = dialog.Company.Text;
                string vat = dialog.VAT.Text;

                if (dialog.Country.SelectedIndex >= 0) country = dialog.Country.SelectedItem.ToString().Split(new string[] { ": " }, StringSplitOptions.None).Last();
                if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName)) { MainWindow.DisplayThrowbackDialog("Client Edit Error", "You must fill in a first and list name"); return; }
                if(!RegexUtilities.IsValidEmail(email, false)) { MainWindow.DisplayThrowbackDialog("Client Edit Error", "The given email address is not valid"); return; }
                if (!RegexUtilities.IsValidPhoneNumber(phone, false)) { MainWindow.DisplayThrowbackDialog("Client Edit Error", "The given phone number is not valid"); return; }
                if (dialog.Type.SelectedIndex < 0) { MainWindow.DisplayThrowbackDialog("Client Edit Error", "You must select an account type"); return; }
                if (!Enum.TryParse(typeof(ClientType), dialog.Type.SelectedItem.ToString().ToUpper().Split(new string[] { ": " }, StringSplitOptions.None).Last(), out object objType)) { MainWindow.DisplayThrowbackDialog("Client Edit Error", "Something went wrong with the client type, try agian"); return; }
                ClientType type = (ClientType) objType;
                if(type == ClientType.PRIVATE || type == ClientType.VIP) { company = ""; vat = ""; }

                client.Type = type;
                client.FirstName = firstName;
                client.LastName = lastName;
                client.Email = email;
                client.Phone = phone;
                client.AddressStreet = street;
                client.AddressNumber = number;
                client.AddressBus = box;
                client.AddressCity = city;
                client.AddressZip = zip;
                client.AddressCounty = country;
                client.CompanyName = company;
                client.VATNumber = vat;

                RentalManager manager = new RentalManager(new UnitOfWork(new RentalContext()));
                manager.UpdateClient(client);

                MainWindow.DisplayThrowbackDialog("Client Saved", "All changes have been saved"); return;
            }
        }

        public async static void OpenCarViewDialog(Car car)
        {
            CarView dialog = new CarView(car);
            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                dialog.Hide();
                OpenCarEditDialog(car);
            }
        }

        public async static void OpenCarEditDialog(Car car)
        {
            CarEdit dialog = new CarEdit(car);
            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                string brand = dialog.Brand.Text;
                string type = dialog.Model.Text;
                Double priceFirst = dialog.PriceFirst.Value;
                Double priceNight = dialog.PriceNight.Value;
                Double priceWedding = dialog.PriceWedding.Value;
                Double priceWellness = dialog.PriceWellness.Value;
                Boolean available = dialog.Available.IsOn;

                if (string.IsNullOrWhiteSpace(brand) || string.IsNullOrWhiteSpace(type)) { MainWindow.DisplayThrowbackDialog("Car Creation Error", "Brand and model are required fields and must be filled in"); return; }
                if (dialog.Color.SelectedIndex < 0) { MainWindow.DisplayThrowbackDialog("Car Creation Error", "You must select a car color"); return; }
                String color = typeof(Color).GetProperties()[dialog.Color.SelectedIndex].Name;
                if (dialog.PriceFirst.Value < 0 || priceNight < 0 || priceWedding < 0 || priceWellness < 0) { MainWindow.DisplayThrowbackDialog("Car Creation Error", "All prices must be 0 or higher"); return; }

                car.Brand = brand;
                car.Type = type;
                car.PriceFirst = priceFirst;
                car.PriceNight = priceNight;
                car.PriceWedding = priceWedding;
                car.PriceWellness = priceWellness;
                car.Available = available;

                try
                {
                    RentalManager manager = new RentalManager(new UnitOfWork(new RentalContext()));
                    manager.UpdateCar(car);
                    MainWindow.DisplayThrowbackDialog("Car Saved", "All changes have been saved");
                }
                catch (Exception error)
                {
                    MainWindow.DisplayThrowbackDialog("An internal error occurred", error.Message);
                    return;
                }
            }
        }
    }
}
