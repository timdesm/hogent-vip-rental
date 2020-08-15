using DataLayer;
using DomainLayer.Domain;
using InterfaceAppPresentationLayer.Dialogs;
using ModernWpf.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace InterfaceAppPresentationLayer.Pages
{
    /// <summary>
    /// Interaction logic for Reservations.xaml
    /// </summary>
    public partial class Reservations : ModernWpf.Controls.Page
    {
        private DataTable reservationTable;

        public Reservations()
        {
            InitializeComponent();

            reservationTable = new DataTable();
            reservationTable.Clear();
            reservationTable.Columns.Add(new DataColumn("ID", typeof(int)));
            reservationTable.Columns.Add(new DataColumn("Client", typeof(string)));
            reservationTable.Columns.Add(new DataColumn("Cars", typeof(string)));
            reservationTable.Columns.Add(new DataColumn("From", typeof(DateTime)));
            reservationTable.Columns.Add(new DataColumn("Until", typeof(DateTime)));
            reservationTable.Columns.Add(new DataColumn("Arrangements", typeof(string)));
            reservationTable.Columns.Add(new DataColumn("Pickup Location", typeof(string)));
            reservationTable.Columns.Add(new DataColumn("Return Location", typeof(string)));
            reservationTable.Columns.Add(new DataColumn("Order Date", typeof(DateTime)));
            reservationTable.Columns.Add(new DataColumn("Invoice ID", typeof(int)));
            reservationTable.Columns.Add(new DataColumn("Total Inc", typeof(string)));
            reservationTable.Columns.Add(new DataColumn("Status", typeof(string)));
            DataTable.ItemsSource = reservationTable.DefaultView;

            Task.Run(() => InitializeDataGrid_Data());
        }

        private void InitializeDataGrid_Data()
        {
            RentalManager manager = new RentalManager(new UnitOfWork(new RentalContext()));
            foreach (Reservation reservation in manager.GetAllReservations())
            {
                DomainLayer.Domain.Invoice invoice = manager.GetInvoice(reservation.InvoiceID);
                Client client = manager.GetClient(reservation.ClientID);
                List<Car> reservationCars = manager.GetReservationCars(reservation.ID);
                StringBuilder sb = new StringBuilder();
                foreach(Car car in reservationCars)
                    sb.Append("#" + car.ID + " " + car.Brand + " " + car.Type + ",");
                

                String cars = reservationCars.ToString();
                AddTableRow(reservation.ID, client.FirstName + " " + client.LastName, sb.ToString().Substring(0, sb.ToString().Length - 1), reservation.ReservationDate, reservation.ReservedUntil, reservation.Arrangement.ToString(), reservation.StartLocation, reservation.EndLocation, reservation.OrderDate, invoice.ID, "€" + invoice.TotalInc, (invoice.PaymentDue == 0) ? "Payed" : "Unpaid");
            }
        }

        private void AddTableRow(int id, string client, string cars, DateTime from, DateTime until, string arrangement, string startLocation, string endLocation, DateTime orderDate, int invoiceID, string totalInc, string status)
        {
            DataRow row = reservationTable.NewRow();
            row[0] = id;
            row[1] = client;
            row[2] = cars;
            row[3] = from;
            row[4] = until;
            row[5] = arrangement;
            row[6] = startLocation;
            row[7] = endLocation;
            row[8] = orderDate;
            row[9] = invoiceID;
            row[10] = totalInc;
            row[11] = status;
            reservationTable.Rows.Add(row);
        }

        private void DataGrid_Editclick(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView dataRowView = (DataRowView)((Button)e.Source).DataContext;

            } catch(Exception ex) { }
        }

        private void DataTableAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {

        }

        private void DataGrid_ViewClick(object sender, RoutedEventArgs e)
        {

        }

        private void DataMenu_Edit(object sender, RoutedEventArgs e)
        {
            DataRowView dataRowView = (DataRowView)((MenuItem) e.Source).DataContext;
            int reservationID = Int32.Parse(dataRowView[0].ToString());
        }

        private async void DataMenu_View(object sender, RoutedEventArgs e)
        {
            DataRowView dataRowView = (DataRowView)((MenuItem)e.Source).DataContext;
            int reservationID = Int32.Parse(dataRowView[0].ToString());
            RentalManager manager = new RentalManager(new UnitOfWork(new RentalContext()));
            Reservation reservation = manager.GetReservation(reservationID);
            ReservationView dialog = new ReservationView(reservation);
            var result = await dialog.ShowAsync();
            if(result == ContentDialogResult.Secondary) { }
                
        }

        private async void DataMenu_Delete(object sender, RoutedEventArgs e)
        {
            DataRowView dataRowView = (DataRowView)((MenuItem)e.Source).DataContext;
            int reservationID = Int32.Parse(dataRowView[0].ToString());

            DeleteDialog dialog = new DeleteDialog("reservation #" + reservationID);
            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary) {
                RentalManager manager = new RentalManager(new UnitOfWork(new RentalContext()));
                manager.RemoveReservation(reservationID);

            }
        }
    }
}
