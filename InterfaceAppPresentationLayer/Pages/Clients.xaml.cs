using DataLayer;
using DomainLayer.Domain;
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

            InitializeComboBox_FilterType();
            clientTable = new DataTable();
            clientTable.Clear();
            clientTable.Columns.Add(new DataColumn("ID", typeof(int)));
            clientTable.Columns.Add(new DataColumn("First Name", typeof(string)));
            clientTable.Columns.Add(new DataColumn("Last Name", typeof(string)));
            clientTable.Columns.Add(new DataColumn("Company", typeof(string)));
            clientTable.Columns.Add(new DataColumn("VAT", typeof(string)));
            clientTable.Columns.Add(new DataColumn("Type", typeof(ClientType)));
            clientTable.Columns.Add(new DataColumn("City", typeof(string)));
            clientTable.Columns.Add(new DataColumn("Country", typeof(string)));
            DataTable.ItemsSource = clientTable.DefaultView;

            Task.Run(() => InitializeDataGrid());
        }

        private void InitializeDataGrid()
        {
            RentalManager manager = new RentalManager(new UnitOfWork(new RentalContext()));
            foreach (Client client in manager.GetAllClients())
            {
                AddTableRow(client.ID, client.FirstName, client.LastName, client.CompanyName, client.VATNumber, client.Type, client.AddressCity, client.AddressCounty);
            }
        }

        private void InitializeComboBox_FilterType()
        {
            foreach(ClientType type in (ClientType[]) Enum.GetValues(typeof(ClientType)))
            {
                FilterType.Items.Add(char.ToUpper(type.ToString().ToLower()[0]) + type.ToString().ToLower().Substring(1));
            }
        }

        private void AddTableRow(int id, string firstName, string lastName, string company, string vat, ClientType type, string city, string country)
        {
            DataRow row = clientTable.NewRow();
            row[0] = id;
            row[1] = firstName;
            row[2] = lastName;
            row[3] = company;
            row[4] = vat;
            row[5] = type;
            row[6] = city;
            row[7] = country;
            clientTable.Rows.Add(row);
        }

        private void DataTableAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {

        }
    }
}
