
using DataLayer;
using DomainLayer.Domain;
using ModernWpf.Controls;
using System;
using System.Data;
using System.Threading.Tasks;

namespace InterfaceAppPresentationLayer.Pages
{
    /// <summary>
    /// Interaction logic for Index.xaml
    /// </summary>
    public partial class Index : Page
    {
        private DataTable todaysReservationsTable;
        private DataTable newReservationsTable;
        private DataTable newClientsTable;
        private DataTable unpaidInvoicesTable;

        public Index()
        {
            InitializeComponent();

            InitializeTodaysReservations();
            InitializeNewestReservations();
            InitializeNewestClients();
            InitializeUnpaidInvoices();
        }

        private void InitializeTodaysReservations()
        {
            todaysReservationsTable = new DataTable();
            todaysReservationsTable.Clear();
            todaysReservationsTable.Columns.Add(new DataColumn("ID", typeof(int)));
            todaysReservationsTable.Columns.Add(new DataColumn("Client", typeof(string)));
            todaysReservationsTable.Columns.Add(new DataColumn("Cars", typeof(string)));
            todaysReservationsTable.Columns.Add(new DataColumn("From", typeof(DateTime)));
            todaysReservationsTable.Columns.Add(new DataColumn("Until", typeof(DateTime)));
            todaysReservationsTable.Columns.Add(new DataColumn("Arrangements", typeof(string)));
            todaysReservationsTable.Columns.Add(new DataColumn("Start Location", typeof(string)));
            todaysReservationsTable.Columns.Add(new DataColumn("End Location", typeof(string)));
            todaysReservationsTable.Columns.Add(new DataColumn("Order Date", typeof(DateTime)));
            todaysReservationsTable.Columns.Add(new DataColumn("Total Inc.", typeof(double)));
            todaysReservationsTable.Columns.Add(new DataColumn("Payed", typeof(bool)));
            TodaysReservations.ItemsSource = todaysReservationsTable.DefaultView;
        }

        private void InitializeNewestReservations()
        {
            newReservationsTable = new DataTable();
            newReservationsTable.Clear();
            newReservationsTable.Columns.Add(new DataColumn("ID", typeof(int)));
            newReservationsTable.Columns.Add(new DataColumn("Client", typeof(string)));
            newReservationsTable.Columns.Add(new DataColumn("Date", typeof(string)));
            newReservationsTable.Columns.Add(new DataColumn("Arrangement", typeof(string)));
            newReservationsTable.Columns.Add(new DataColumn("Start Location", typeof(string)));
            NewestReservations.ItemsSource = newReservationsTable.DefaultView;
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

            Task.Run(() => LoadNewestClients());
        }

        private void LoadNewestClients()
        {
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

        private void InitializeUnpaidInvoices()
        {
            unpaidInvoicesTable = new DataTable();
            unpaidInvoicesTable.Clear();
            unpaidInvoicesTable.Columns.Add(new DataColumn("ID", typeof(int)));
            unpaidInvoicesTable.Columns.Add(new DataColumn("Client", typeof(string)));
            unpaidInvoicesTable.Columns.Add(new DataColumn("Total Inc.", typeof(double)));
            unpaidInvoicesTable.Columns.Add(new DataColumn("Due", typeof(double)));
            UnpaidInvoices.ItemsSource = unpaidInvoicesTable.DefaultView;
        }
    }
}
