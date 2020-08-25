using DataLayer;
using DomainLayer.Domain;
using InterfaceAppPresentationLayer.Classes;
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
            reservationTable.Columns.Add(new DataColumn("Returned", typeof(string)));
            reservationTable.Columns.Add(new DataColumn("Arrangements", typeof(string)));
            reservationTable.Columns.Add(new DataColumn("Pickup Location", typeof(string)));
            reservationTable.Columns.Add(new DataColumn("Return Location", typeof(string)));
            reservationTable.Columns.Add(new DataColumn("Order Date", typeof(DateTime)));
            reservationTable.Columns.Add(new DataColumn("Invoice ID", typeof(int)));
            reservationTable.Columns.Add(new DataColumn("Total Inc", typeof(string)));
            reservationTable.Columns.Add(new DataColumn("Status", typeof(string)));
            DataTable.ItemsSource = reservationTable.DefaultView;

            InitializeDataGrid_Data();
        }


        private void InitializeDataGrid_Data()
        {
            reservationTable.Rows.Clear();
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
                AddTableRow(reservation.ID, client.FirstName + " " + client.LastName, sb.ToString().Substring(0, sb.ToString().Length - 1), reservation.ReservationDate, reservation.ReservedUntil, (reservation.ReservationEnded > DateTime.MinValue) ? reservation.ReservationEnded.ToString() : "", char.ToUpper(reservation.Arrangement.ToString().ToLower()[0]) + reservation.Arrangement.ToString().ToLower().Substring(1), reservation.StartLocation, reservation.EndLocation, reservation.OrderDate, invoice.ID, "€" + invoice.TotalInc, (invoice.PaymentDue == 0) ? "Paid" : "Unpaid");
            }
        }

        private void AddTableRow(int id, string client, string cars, DateTime from, DateTime until, string returned, string arrangement, string startLocation, string endLocation, DateTime orderDate, int invoiceID, string totalInc, string status)
        {
            DataRow row = reservationTable.NewRow();
            row[0] = id;
            row[1] = client;
            row[2] = cars;
            row[3] = from;
            row[4] = until;
            row[5] = returned;
            row[6] = arrangement;
            row[7] = startLocation;
            row[8] = endLocation;
            row[9] = orderDate;
            row[10] = invoiceID;
            row[11] = totalInc;
            row[12] = status;
            reservationTable.Rows.Add(row);
        }

        private void DataGrid_Editclick(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView dataRowView = (DataRowView)((Button)e.Source).DataContext;

            } catch(Exception ex) { }
        }

        private void DataGrid_ViewClick(object sender, RoutedEventArgs e)
        {

        }

        private void DataMenu_Edit(object sender, RoutedEventArgs e)
        {
            DataRowView dataRowView = (DataRowView)((MenuItem) e.Source).DataContext;
            int reservationID = Int32.Parse(dataRowView[0].ToString());
            RentalManager manager = new RentalManager(new UnitOfWork(new RentalContext()));
            Reservation reservation = manager.GetReservation(reservationID);
            DialogService.OpenReservationEditDialog(reservation);
        }

        private void DataMenu_View(object sender, RoutedEventArgs e)
        {
            DataRowView dataRowView = (DataRowView)((MenuItem)e.Source).DataContext;
            int reservationID = Int32.Parse(dataRowView[0].ToString());
            RentalManager manager = new RentalManager(new UnitOfWork(new RentalContext()));
            Reservation reservation = manager.GetReservation(reservationID);
            DialogService.OpenReservationViewDialog(reservation);
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
                InitializeDataGrid_Data();
            }
        }
    }
}
