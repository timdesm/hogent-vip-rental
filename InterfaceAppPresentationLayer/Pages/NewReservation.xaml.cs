using DataLayer;
using DomainLayer.Domain;
using InterfaceAppPresentationLayer.Classes;
using ModernWpf.Controls;
using System;
using System.Data;

namespace InterfaceAppPresentationLayer.Pages
{
    /// <summary>
    /// Interaction logic for NewReservation.xaml
    /// </summary>
    public partial class NewReservation : Page
    {
        DataTable AvailableCarsTable;

        DateTime From;
        DateTime Until;

        public NewReservation()
        {
            InitializeComponent();

            InitializeComboBox_Clients();
            InitializeComboxBox_Arrangement();
            InitializeService.InitializeSelection(inFromTime);
            InitializeService.InitializeSelection(inUntilTime);

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

        private void InitializeComboxBox_Arrangement()
        {
            foreach (ReservationArrangementType type in (ReservationArrangementType[])Enum.GetValues(typeof(ReservationArrangementType)))
            {
                inArrangement.Items.Add(char.ToUpper(type.ToString().ToLower()[0]) + type.ToString().ToLower().Substring(1));
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

        private void LoadAvailableCars(DateTime from, DateTime until, double hourRange)
        {
            AvailableCarsTable.Rows.Clear();
            RentalManager manager = new RentalManager(new UnitOfWork(new RentalContext()));
            foreach (Car car in manager.GetAvailableCars(from, until, hourRange))
            {
                AddTableRow(car.ID, car.Brand, car.Type, car.Color, car.PriceFirst);
            }
        }

        private void Submit_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }
        
        private void inFromDate_SelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (inFromDate.SelectedDate != null && inFromTime.SelectedIndex >= 0 && TimeSpan.TryParse(inFromTime.SelectedItem.ToString(), out TimeSpan time))
            {
                From = inFromDate.SelectedDate ?? DateTime.Now;
                From = From + time;

                inUntilDate.IsEnabled = true;
                inUntilDate.SelectedDate = From;
                inUntilDate.DisplayDateStart = From;
                DateTime maxUntil = From.AddHours(11);
                inUntilDate.DisplayDateEnd = maxUntil;

                if (inUntilDate.SelectedDate != null && inUntilTime.SelectedIndex >= 0 && TimeSpan.TryParse(inUntilTime.SelectedItem.ToString(), out TimeSpan untilTime))
                {
                    LoadAvailableCars(From, Until, 6.0);
                }
            }
            else
                inUntilDate.IsEnabled = false;
        }

        private void inFromTime_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if(inFromDate.SelectedDate != null && inFromTime.SelectedIndex >= 0 && TimeSpan.TryParse(inFromTime.SelectedItem.ToString(), out TimeSpan time))
            {
                From = inFromDate.SelectedDate??DateTime.Now;
                From = From + time;

                inUntilDate.IsEnabled = true;
                inUntilDate.SelectedDate = From;
                inUntilDate.DisplayDateStart = From;
                DateTime maxUntil = From.AddHours(11);
                inUntilDate.DisplayDateEnd = maxUntil;

                if (inUntilDate.SelectedDate != null && inUntilTime.SelectedIndex >= 0 && TimeSpan.TryParse(inUntilTime.SelectedItem.ToString(), out TimeSpan untilTime))
                {
                    LoadAvailableCars(From, Until, 6.0);
                }
            }
            else
                inUntilDate.IsEnabled = false;
        }

        private void inUntilDate_SelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (inFromDate.SelectedDate != null && inFromTime.SelectedIndex >= 0 && TimeSpan.TryParse(inFromTime.SelectedItem.ToString(), out TimeSpan time))
            {
                if (inUntilDate.SelectedDate != null && inUntilTime.SelectedIndex >= 0 && TimeSpan.TryParse(inUntilTime.SelectedItem.ToString(), out TimeSpan untilTime))
                {
                    Until = inUntilDate.SelectedDate ?? DateTime.Now;
                    Until = Until + untilTime;
                }
            }
        }

        private void inUntilTime_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (inFromDate.SelectedDate != null && inFromTime.SelectedIndex >= 0 && TimeSpan.TryParse(inFromTime.SelectedItem.ToString(), out TimeSpan time))
            {
                if (inUntilDate.SelectedDate != null && inUntilTime.SelectedIndex >= 0 && TimeSpan.TryParse(inUntilTime.SelectedItem.ToString(), out TimeSpan untilTime))
                {
                    Until = inUntilDate.SelectedDate ?? DateTime.Now;
                    Until = Until + untilTime;
                }
            }
        } 
    }
}
