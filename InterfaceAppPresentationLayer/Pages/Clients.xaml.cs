using DataLayer;
using DomainLayer.Domain;
using InterfaceAppPresentationLayer.Classes;
using ModernWpf.Controls;
using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace InterfaceAppPresentationLayer.Pages
{
    /// <summary>
    /// Interaction logic for Clients.xaml
    /// </summary>
    public partial class Clients : ModernWpf.Controls.Page
    {
        private DataTable clientTable;

        public Clients()
        {
            InitializeComponent();

            clientTable = new DataTable();
            clientTable.Clear();
            clientTable.Columns.Add(new DataColumn("ID", typeof(int)));
            clientTable.Columns.Add(new DataColumn("First Name", typeof(string)));
            clientTable.Columns.Add(new DataColumn("Last Name", typeof(string)));
            clientTable.Columns.Add(new DataColumn("Email", typeof(string)));
            clientTable.Columns.Add(new DataColumn("Phone", typeof(string)));
            clientTable.Columns.Add(new DataColumn("Company", typeof(string)));
            clientTable.Columns.Add(new DataColumn("VAT", typeof(string)));
            clientTable.Columns.Add(new DataColumn("Type", typeof(string)));
            clientTable.Columns.Add(new DataColumn("Street", typeof(string)));
            clientTable.Columns.Add(new DataColumn("Number", typeof(string)));
            clientTable.Columns.Add(new DataColumn("Box", typeof(string)));
            clientTable.Columns.Add(new DataColumn("City", typeof(string)));
            clientTable.Columns.Add(new DataColumn("Zip", typeof(string)));
            clientTable.Columns.Add(new DataColumn("Country", typeof(string)));
            DataTable.ItemsSource = clientTable.DefaultView;

            InitializeDataGrid();
        }

        private void InitializeDataGrid()
        {
            RentalManager manager = new RentalManager(new UnitOfWork(new RentalContext()));
            foreach (Client client in manager.GetAllClients())
            {
                AddTableRow(client.ID, client.FirstName, client.LastName, client.Email, client.Phone, client.CompanyName, client.VATNumber, char.ToUpper(client.Type.ToString().ToLower()[0]) + client.Type.ToString().ToLower().Substring(1), client.AddressStreet, client.AddressNumber, client.AddressBus, client.AddressCity, client.AddressZip, client.AddressCounty);
            }
        }

        private void AddTableRow(int id, string firstName, string lastName, string email, string phone, string company, string vat, string type, string street, string number, string box, string city, string zip, string country)
        {
            DataRow row = clientTable.NewRow();
            row[0] = id;
            row[1] = firstName;
            row[2] = lastName;
            row[3] = email;
            row[4] = phone;
            row[5] = company;
            row[6] = vat;
            row[7] = type;
            row[8] = street;
            row[9] = number;
            row[10] = box;
            row[11] = city;
            row[12] = zip;
            row[13] = country;
            clientTable.Rows.Add(row);
        }

        private void ClientsMenu_Edit(object sender, System.Windows.RoutedEventArgs e)
        {
            DataRowView dataRowView = (DataRowView)((MenuItem)e.Source).DataContext;
            int clientID = Int32.Parse(dataRowView[0].ToString());
            RentalManager manager = new RentalManager(new UnitOfWork(new RentalContext()));
            Client client = manager.GetClient(clientID);
            DialogService.OpenClientEditDialog(client);
        }

        private void ClientsMenu_View(object sender, System.Windows.RoutedEventArgs e)
        {
            DataRowView dataRowView = (DataRowView)((MenuItem)e.Source).DataContext;
            int clientID = Int32.Parse(dataRowView[0].ToString());
            RentalManager manager = new RentalManager(new UnitOfWork(new RentalContext()));
            Client client = manager.GetClient(clientID);
            DialogService.OpenClientViewDialog(client);
        }

        private void ClientsMenu_Delete(object sender, System.Windows.RoutedEventArgs e)
        {

        }
    }
}
