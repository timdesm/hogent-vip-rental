using DataLayer;
using DomainLayer.Domain;
using ModernWpf.Controls;
using System;
using System.Data;
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
            reservationTable.Columns.Add(new DataColumn("Invoice ID", typeof(DateTime)));
            reservationTable.Columns.Add(new DataColumn("Total Inc.", typeof(double)));
            reservationTable.Columns.Add(new DataColumn("Status", typeof(double)));
            DataTable.ItemsSource = reservationTable.DefaultView;

            Task.Run(() => InitializeDataGrid_Data());
        }

        private void InitializeDataGrid_Data()
        {
            RentalManager manager = new RentalManager(new UnitOfWork(new RentalContext()));
            foreach (Reservation reservation in manager.GetAllReservations())
            {
                String cars = "";
                AddTableRow(reservation.ID, reservation.Client.FirstName + " " + reservation.Client.LastName, cars, reservation.ReservationDate, reservation.ReservedUntil, reservation.Arrangement.ToString(), reservation.StartLocation, reservation.EndLocation, reservation.OrderDate, reservation.Invoice.ID, reservation.Invoice.TotalInc, (reservation.Invoice.PaymentDue == 0) ? "Payed" : "Unpaid");
            }
        }

        private void AddTableRow(int id, string client, string cars, DateTime from, DateTime until, string arrangement, string startLocation, string endLocation, DateTime orderDate, int invoiceID, double totalInc, string status)
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

        private void DataTableAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {

        }
    }
}
