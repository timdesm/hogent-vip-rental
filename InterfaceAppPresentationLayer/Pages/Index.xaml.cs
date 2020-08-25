
using DataLayer;
using DomainLayer.Domain;
using InterfaceAppPresentationLayer.Classes;
using InterfaceAppPresentationLayer.Dialogs;
using ModernWpf.Controls;
using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace InterfaceAppPresentationLayer.Pages
{
    /// <summary>
    /// Interaction logic for Index.xaml
    /// </summary>
    public partial class Index : ModernWpf.Controls.Page
    {
        private DataTable todaysReservationsTable;
        private DataTable activeReservationsTable;
        private DataTable newClientsTable;
        private DataTable unpaidInvoicesTable;

        public Index()
        {
            InitializeComponent();

            InitializeTodaysReservations();
            InitializeActiveReservations();
            InitializeNewestClients();
            InitializeUnpaidInvoices();
        }

        private void InitializeTodaysReservations()
        {
            todaysReservationsTable = new DataTable();
            todaysReservationsTable.Clear();
            todaysReservationsTable.Columns.Add(new DataColumn("ID", typeof(int)));
            todaysReservationsTable.Columns.Add(new DataColumn("Client", typeof(string)));
            todaysReservationsTable.Columns.Add(new DataColumn("From", typeof(DateTime)));
            todaysReservationsTable.Columns.Add(new DataColumn("Until", typeof(DateTime)));
            todaysReservationsTable.Columns.Add(new DataColumn("Arrangement", typeof(string)));
            todaysReservationsTable.Columns.Add(new DataColumn("Start Location", typeof(string)));
            todaysReservationsTable.Columns.Add(new DataColumn("End Location", typeof(string)));
            todaysReservationsTable.Columns.Add(new DataColumn("Total Inc", typeof(string)));
            todaysReservationsTable.Columns.Add(new DataColumn("Payed", typeof(string)));
            TodaysReservations.ItemsSource = todaysReservationsTable.DefaultView;

            LoadTodaysReservations();
        }

        private void LoadTodaysReservations()
        {
            todaysReservationsTable.Rows.Clear();
            RentalManager manager = new RentalManager(new UnitOfWork(new RentalContext()));
            foreach (Reservation reservation in manager.GetReservations(DateTime.Today, DateTime.Today.AddDays(1)))
            {
                DomainLayer.Domain.Invoice invoice = manager.GetInvoice(reservation.InvoiceID);
                Client client = manager.GetClient(reservation.ClientID);
                string clientStr = client.FirstName + " " + client.LastName;
                if (!string.IsNullOrWhiteSpace(client.CompanyName)) clientStr = "(" + client.CompanyName + ") " + clientStr;
                AddTodaysReservationsRow(reservation.ID, clientStr, reservation.ReservationDate, reservation.ReservedUntil, char.ToUpper(reservation.Arrangement.ToString().ToLower()[0]) + reservation.Arrangement.ToString().ToLower().Substring(1), reservation.StartLocation, reservation.EndLocation, string.Format("€{0:0.00}", invoice.TotalInc), (invoice.PaymentDue == 0) ? "Paid" : "Unpaid");
            }
        }

        private void AddTodaysReservationsRow(int id, string client, DateTime from, DateTime until, string arrangement, string startLocation, string endLocation, string totalInc, string payed)
        {
            DataRow row = todaysReservationsTable.NewRow();
            row[0] = id;
            row[1] = client;
            row[2] = from;
            row[3] = until;
            row[4] = arrangement;
            row[5] = startLocation;
            row[6] = endLocation;
            row[7] = totalInc;
            row[8] = payed;
            todaysReservationsTable.Rows.Add(row);
        }

        private void InitializeActiveReservations()
        {
            activeReservationsTable = new DataTable();
            activeReservationsTable.Clear();
            activeReservationsTable.Columns.Add(new DataColumn("ID", typeof(int)));
            activeReservationsTable.Columns.Add(new DataColumn("Client", typeof(string)));
            activeReservationsTable.Columns.Add(new DataColumn("From", typeof(DateTime)));
            activeReservationsTable.Columns.Add(new DataColumn("Until", typeof(DateTime)));
            activeReservationsTable.Columns.Add(new DataColumn("Arrangement", typeof(string)));
            activeReservationsTable.Columns.Add(new DataColumn("Start Location", typeof(string)));
            activeReservationsTable.Columns.Add(new DataColumn("End Location", typeof(string)));
            activeReservationsTable.Columns.Add(new DataColumn("Total Inc", typeof(string)));
            activeReservationsTable.Columns.Add(new DataColumn("Payed", typeof(string)));
            ActiveReservations.ItemsSource = activeReservationsTable.DefaultView;

            LoadActiveReservations();
        }

        private void LoadActiveReservations()
        {
            activeReservationsTable.Rows.Clear();
            RentalManager manager = new RentalManager(new UnitOfWork(new RentalContext()));
            foreach (Reservation reservation in manager.GetActiveReservations())
            {
                DomainLayer.Domain.Invoice invoice = manager.GetInvoice(reservation.InvoiceID);
                Client client = manager.GetClient(reservation.ClientID);
                string clientStr = client.FirstName + " " + client.LastName;
                if (!string.IsNullOrWhiteSpace(client.CompanyName)) clientStr = "(" + client.CompanyName + ") " + clientStr;
                AddActiveReservationsRow(reservation.ID, clientStr, reservation.ReservationDate, reservation.ReservedUntil, char.ToUpper(reservation.Arrangement.ToString().ToLower()[0]) + reservation.Arrangement.ToString().ToLower().Substring(1), reservation.StartLocation, reservation.EndLocation, string.Format("€{0:0.00}", invoice.TotalInc), (invoice.PaymentDue == 0) ? "Paid" : "Unpaid");
            }
        }

        private void AddActiveReservationsRow(int id, string client, DateTime from, DateTime until, string arrangement, string startLocation, string endLocation, string totalInc, string payed)
        {
            DataRow row = activeReservationsTable.NewRow();
            row[0] = id;
            row[1] = client;
            row[2] = from;
            row[3] = until;
            row[4] = arrangement;
            row[5] = startLocation;
            row[6] = endLocation;
            row[7] = totalInc;
            row[8] = payed;
            activeReservationsTable.Rows.Add(row);
        }

        private void ReservationsMenu_Edit(object sender, System.Windows.RoutedEventArgs e)
        {
            DataRowView dataRowView = (DataRowView)((MenuItem)e.Source).DataContext;
            int reservationID = Int32.Parse(dataRowView[0].ToString());
            RentalManager manager = new RentalManager(new UnitOfWork(new RentalContext()));
            Reservation reservation = manager.GetReservation(reservationID);
            DialogService.OpenReservationEditDialog(reservation);
            LoadTodaysReservations();
            LoadActiveReservations();
            LoadUnpaidInvoices();
        }

        private void ReservationsMenu_View(object sender, System.Windows.RoutedEventArgs e)
        {
            DataRowView dataRowView = (DataRowView)((MenuItem)e.Source).DataContext;
            int reservationID = Int32.Parse(dataRowView[0].ToString());
            RentalManager manager = new RentalManager(new UnitOfWork(new RentalContext()));
            Reservation reservation = manager.GetReservation(reservationID);
            DialogService.OpenReservationViewDialog(reservation);
            LoadTodaysReservations();
            LoadActiveReservations();
            LoadUnpaidInvoices();
        }

        private async void ReservationsMenu_Delete(object sender, System.Windows.RoutedEventArgs e)
        {
            DataRowView dataRowView = (DataRowView)((MenuItem)e.Source).DataContext;
            int reservationID = Int32.Parse(dataRowView[0].ToString());
            DeleteDialog dialog = new DeleteDialog("reservation #" + reservationID);
            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                RentalManager manager = new RentalManager(new UnitOfWork(new RentalContext()));
                manager.RemoveReservation(reservationID);
                LoadTodaysReservations();
                LoadActiveReservations();
                LoadUnpaidInvoices();
            }
        }

        private void InitializeNewestClients()
        {
            newClientsTable = new DataTable();
            newClientsTable.Clear();
            newClientsTable.Columns.Add(new DataColumn("ID", typeof(int)));
            newClientsTable.Columns.Add(new DataColumn("First Name", typeof(string)));
            newClientsTable.Columns.Add(new DataColumn("Last Name", typeof(string)));
            newClientsTable.Columns.Add(new DataColumn("Company", typeof(string)));
            newClientsTable.Columns.Add(new DataColumn("Type", typeof(string)));
            NewestClients.ItemsSource = newClientsTable.DefaultView;

            LoadNewestClients();
        }

        private void LoadNewestClients()
        {
            newClientsTable.Rows.Clear();
            RentalManager manager = new RentalManager(new UnitOfWork(new RentalContext()));
            foreach (Client client in manager.GetNewestClients(10))
            {
                AddNewestClientRow(client.ID, client.FirstName, client.LastName, client.CompanyName, char.ToUpper(client.Type.ToString().ToLower()[0]) + client.Type.ToString().ToLower().Substring(1));
            }
        }

        private void AddNewestClientRow(int id, string firstName, string lastName, string company, string type)
        {
            DataRow row = newClientsTable.NewRow();
            row[0] = id;
            row[1] = firstName;
            row[2] = lastName;
            row[3] = company;
            row[4] = type;
            newClientsTable.Rows.Add(row);
        }

        private void NewestClientsMenu_Edit(object sender, System.Windows.RoutedEventArgs e)
        {
            DataRowView dataRowView = (DataRowView)((MenuItem)e.Source).DataContext;
            int clientID = Int32.Parse(dataRowView[0].ToString());
            RentalManager manager = new RentalManager(new UnitOfWork(new RentalContext()));
            Client client = manager.GetClient(clientID);
            DialogService.OpenClientEditDialog(client);
            LoadNewestClients();
        }

        private void NewestClientsMenu_View(object sender, System.Windows.RoutedEventArgs e)
        {
            DataRowView dataRowView = (DataRowView)((MenuItem)e.Source).DataContext;
            int clientID = Int32.Parse(dataRowView[0].ToString());
            RentalManager manager = new RentalManager(new UnitOfWork(new RentalContext()));
            Client client = manager.GetClient(clientID);
            DialogService.OpenClientViewDialog(client);
            LoadNewestClients();
        }

        private void InitializeUnpaidInvoices()
        {
            unpaidInvoicesTable = new DataTable();
            unpaidInvoicesTable.Clear();
            unpaidInvoicesTable.Columns.Add(new DataColumn("ID", typeof(int)));
            unpaidInvoicesTable.Columns.Add(new DataColumn("Client", typeof(string)));
            unpaidInvoicesTable.Columns.Add(new DataColumn("Date", typeof(DateTime)));
            unpaidInvoicesTable.Columns.Add(new DataColumn("Total Exc", typeof(string)));
            unpaidInvoicesTable.Columns.Add(new DataColumn("VAT", typeof(string)));
            unpaidInvoicesTable.Columns.Add(new DataColumn("Total Inc", typeof(string)));
            unpaidInvoicesTable.Columns.Add(new DataColumn("Due", typeof(string)));
            UnpaidInvoices.ItemsSource = unpaidInvoicesTable.DefaultView;

            LoadUnpaidInvoices();
        }

        private void LoadUnpaidInvoices()
        {
            unpaidInvoicesTable.Rows.Clear();
            RentalManager manager = new RentalManager(new UnitOfWork(new RentalContext()));
            foreach (DomainLayer.Domain.Invoice invoice in manager.GetUnpaidInvoices())
            {
                Client client = manager.GetClient(invoice.ClientID);
                string clientStr = client.FirstName + " " + client.LastName;
                if (!string.IsNullOrWhiteSpace(client.CompanyName)) clientStr = "(" + client.CompanyName + ") " + clientStr;
                AddUnpiadInvoiceRow(invoice.ID, clientStr, invoice.InvoiceDate, string.Format("€{0:0.00}", invoice.TotalExc), string.Format("€{0:0.00}", invoice.VAT), string.Format("€{0:0.00}", invoice.TotalInc), string.Format("€{0:0.00}", invoice.PaymentDue));
            }
        }

        private void AddUnpiadInvoiceRow(int id, string client, DateTime date, string totalExc, string vat, string totalInc, string due)
        {
            DataRow row = unpaidInvoicesTable.NewRow();
            row[0] = id;
            row[1] = client;
            row[2] = date;
            row[3] = totalExc;
            row[4] = vat;
            row[5] = totalInc;
            row[6] = due;
            unpaidInvoicesTable.Rows.Add(row);
        }

        private void InvoiceMenu_View(object sender, System.Windows.RoutedEventArgs e)
        {
            DataRowView dataRowView = (DataRowView)((MenuItem)e.Source).DataContext;
            int invoiceID = Int32.Parse(dataRowView[0].ToString());
            RentalManager manager = new RentalManager(new UnitOfWork(new RentalContext()));
            DomainLayer.Domain.Invoice invoice = manager.GetInvoice(invoiceID);
        }

        private void InvoiceMenu_MarkPaid(object sender, System.Windows.RoutedEventArgs e)
        {
            DataRowView dataRowView = (DataRowView)((MenuItem)e.Source).DataContext;
            int invoiceID = Int32.Parse(dataRowView[0].ToString());
            RentalManager manager = new RentalManager(new UnitOfWork(new RentalContext()));
            DomainLayer.Domain.Invoice invoice = manager.GetInvoice(invoiceID);
            if(invoice.PaymentDue > 0)
            {
                invoice.PaymentDue = 0;
                manager.UpdateInvoice(invoice);
                LoadUnpaidInvoices();
                MainWindow.DisplayThrowbackDialog("Invoice Updated", "The invoice has been updated to paid.");
            }
            else
            {
                MainWindow.DisplayThrowbackDialog("Invoice Already Paid", "The invoice you tried to mark as paid is already paid.");
            }
        }
    }
}
