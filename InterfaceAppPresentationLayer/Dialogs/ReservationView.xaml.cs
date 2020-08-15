using DataLayer;
using DomainLayer.Domain;
using ModernWpf.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Controls;

namespace InterfaceAppPresentationLayer.Dialogs
{
    /// <summary>
    /// Interaction logic for ReservationView.xaml
    /// </summary>
    public partial class ReservationView : ContentDialog
    {
        private DataTable carTable;

        public ReservationView(Reservation reservation)
        {
            InitializeComponent();
            this.Title = "Reservation #" + reservation.ID;
            InitializeDataGrid();

            RentalManager manager = new RentalManager(new UnitOfWork(new RentalContext()));
            Invoice invoice = manager.GetInvoice(reservation.InvoiceID);
            Client client = manager.GetClient(reservation.ClientID);
            List<Car> reservationCars = manager.GetReservationCars(reservation.ID);

            Client.Text = client.FirstName + " " + client.LastName;
            if(!string.IsNullOrWhiteSpace(client.CompanyName))
                Client.Text = "(" + client.CompanyName + ") " +  client.FirstName + " " + client.LastName;
            Arrangement.Text = char.ToUpper(reservation.Arrangement.ToString().ToLower()[0]) + reservation.Arrangement.ToString().ToLower().Substring(1);
            StartLocation.Text = char.ToUpper(reservation.StartLocation.ToLower()[0]) + reservation.StartLocation.ToLower().Substring(1);
            EndLocation.Text = char.ToUpper(reservation.EndLocation.ToLower()[0]) + reservation.EndLocation.ToLower().Substring(1);
            From.Text = reservation.ReservationDate.ToString();
            Until.Text = reservation.ReservedUntil.ToString();

            foreach (Car car in reservationCars)
                AddTableRow(car.ID, car.Brand, car.Type, car.Color, car.Available);
        }

        private void InitializeDataGrid()
        {
            carTable = new DataTable();
            carTable.Clear();
            carTable.Columns.Add(new DataColumn("ID", typeof(int)));
            carTable.Columns.Add(new DataColumn("Brand", typeof(string)));
            carTable.Columns.Add(new DataColumn("Model", typeof(string)));
            carTable.Columns.Add(new DataColumn("Color", typeof(string)));
            carTable.Columns.Add(new DataColumn("Available", typeof(bool)));
            CarTable.ItemsSource = carTable.DefaultView;
        }

        private void AddTableRow(int id, string brand, string type, string color, bool available)
        {
            DataRow row = carTable.NewRow();
            row[0] = id;
            row[1] = brand;
            row[2] = type;
            row[3] = color;
            row[4] = available;
            carTable.Rows.Add(row);
        }

        private async void CarMenu_View(object sender, System.Windows.RoutedEventArgs e)
        {
            this.Hide();

            DataRowView dataRowView = (DataRowView)((MenuItem)e.Source).DataContext;
            int carID = Int32.Parse(dataRowView[0].ToString());
            RentalManager manager = new RentalManager(new UnitOfWork(new RentalContext()));
            Car car = manager.GetCar(carID);
            CarView dialog = new CarView(car);
            var result = await dialog.ShowAsync();
        }
    }
}
