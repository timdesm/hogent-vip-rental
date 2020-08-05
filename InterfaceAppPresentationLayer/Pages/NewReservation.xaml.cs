using DataLayer;
using DomainLayer.Domain;
using ModernWpf.Controls;
using System.Data;
using System.Threading.Tasks;

namespace InterfaceAppPresentationLayer.Pages
{
    /// <summary>
    /// Interaction logic for NewReservation.xaml
    /// </summary>
    public partial class NewReservation : Page
    {
        DataTable AvailableCarsTable;

        public NewReservation()
        {
            InitializeComponent();

            InitializeComboBox_Clients();
            InitializeAvailbaleCars();
        }

        private void InitializeComboBox_Clients()
        {
            RentalManager manager = new RentalManager(new UnitOfWork(new RentalContext()));
            foreach (Client client in manager.GetAllClients())
            {
                if (string.IsNullOrWhiteSpace(client.CompanyName))
                    inClient.Items.Add("#" + client.ID + " " + client.FirstName + " " + client.LastName + " - " + char.ToUpper(client.Type.ToString().ToLower()[0]) + client.Type.ToString().ToLower().Substring(1));
                else
                    inClient.Items.Add("#" + client.ID + " (" + client.CompanyName +  ") " + client.FirstName + " " + client.LastName + " - " + char.ToUpper(client.Type.ToString().ToLower()[0]) + client.Type.ToString().ToLower().Substring(1));
            }
        }

        private void InitializeAvailbaleCars()
        {
            AvailableCarsTable = new DataTable();
            AvailableCarsTable.Clear();
            AvailableCarsTable.Columns.Add(new DataColumn("ID", typeof(int)));
            AvailableCarsTable.Columns.Add(new DataColumn("Brand", typeof(string)));
            AvailableCarsTable.Columns.Add(new DataColumn("Model", typeof(string)));
            AvailableCarsTable.Columns.Add(new DataColumn("Color", typeof(string)));
            AvailableCarsTable.Columns.Add(new DataColumn("Price", typeof(double)));
            AvailableCars.ItemsSource = AvailableCarsTable.DefaultView;

            AvailableCarsTable.Rows.Clear();
            RentalManager manager = new RentalManager(new UnitOfWork(new RentalContext()));
            foreach (Car car in manager.GetAllCars())
            {
                AddTableRow(car.ID, car.Brand, car.Type, car.Color, car.PriceFirst);
            }
        }

        private void AddTableRow(int id, string brand, string type, string color, double price)
        {
            DataRow row = AvailableCarsTable.NewRow();
            row[0] = id;
            row[1] = brand;
            row[2] = type;
            row[3] = color;
            row[4] = price;
            AvailableCarsTable.Rows.Add(row);
        }

        private void Submit_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }
    }
}
