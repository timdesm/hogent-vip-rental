using DataLayer;
using DomainLayer.Domain;
using InterfaceAppPresentationLayer.Classes;
using ModernWpf.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Documents;

namespace InterfaceAppPresentationLayer.Pages
{
    /// <summary>
    /// Interaction logic for NewReservation.xaml
    /// </summary>
    public partial class NewReservation : Page
    {
        DataTable AvailableCarsTable;

        ReservationArrangementType Type;
        DateTime From;
        DateTime Until;


        public NewReservation()
        {
            InitializeComponent();

            InitializeComboBox_Clients();
            InitializeComboxBox_Arrangement();
            InitializeComboxBox_StartLocation();
            InitializeComboxBox_EndLocation();
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

        private void InitializeComboxBox_StartLocation()
        {
            foreach (GarageLocations loc in (GarageLocations[])Enum.GetValues(typeof(GarageLocations)))
            {
                inStartLocation.Items.Add(char.ToUpper(loc.ToString().ToLower()[0]) + loc.ToString().ToLower().Substring(1));
            }
        }
        private void InitializeComboxBox_EndLocation()
        {
            foreach (GarageLocations loc in (GarageLocations[])Enum.GetValues(typeof(GarageLocations)))
            {
                inEndLocation.Items.Add(char.ToUpper(loc.ToString().ToLower()[0]) + loc.ToString().ToLower().Substring(1));
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
            if (inClient.SelectedIndex < 0) { MainWindow.DisplayThrowbackDialog("Reservation Creation Error", "You must selected a client out of the list"); return; }
            if(!int.TryParse(inClient.SelectedItem.ToString().Split(" ")[0].Substring(1), out int clientID)) { MainWindow.DisplayThrowbackDialog("Reservation Creation Error", "Something went wrong while retrieving the client from the list"); return; }
            RentalManager manager = new RentalManager(new UnitOfWork(new RentalContext()));
            Client client = manager.GetClient(clientID);
            if (inArrangement.SelectedIndex < 0 || !Enum.TryParse(inArrangement.SelectedItem.ToString().ToUpper(), out Type)) { MainWindow.DisplayThrowbackDialog("Reservation Creation Error", "You must select a arrangement from the list"); return; }
            if (inFromDate.SelectedDate == null || inFromTime.SelectedIndex < 0 || !TimeSpan.TryParse(inFromTime.SelectedItem.ToString(), out TimeSpan fromTime)) { MainWindow.DisplayThrowbackDialog("Reservation Creation Error", "You must select pickup date and time"); return; }
            if (inUntilDate.SelectedDate == null || inUntilTime.SelectedIndex < 0 || !TimeSpan.TryParse(inUntilTime.SelectedItem.ToString(), out TimeSpan untilTime)) { MainWindow.DisplayThrowbackDialog("Reservation Creation Error", "You must select return date and time"); return; }
            if (inStartLocation.SelectedIndex < 0) { MainWindow.DisplayThrowbackDialog("Reservation Creation Error", "You must select pickup location"); return; }
            if (inEndLocation.SelectedIndex < 0) { MainWindow.DisplayThrowbackDialog("Reservation Creation Error", "You must select return location"); return; }
            if (client.Type == ClientType.PRIVATE && AvailableCars.SelectedItems.Count > 1) { MainWindow.DisplayThrowbackDialog("Reservation Edit Error", "Private clients can select a maximum of 1 car"); return; }

            String startLocation = inStartLocation.SelectedItem.ToString();
            String endLocation = inEndLocation.SelectedItem.ToString();

            List<int> carIDs = new List<int>();
            List<int> selectedCars = AvailableCars.SelectedItems.Cast<DataRowView>().Select(x => AvailableCarsTable.Rows.IndexOf(x.Row)).ToList();
            foreach(int i in selectedCars)
            {
                if(AvailableCarsTable.Rows.Count > 1)
                {
                    DataRow row = AvailableCarsTable.Rows[i];
                    carIDs.Add((int)row[0]);
                }
            }

            List<Car> cars = new List<Car>();
            foreach(int carID in carIDs)
            {
                cars.Add(manager.GetCar(carID));
            }

            try
            {
                manager.AddReservation(client, Type, From, Until, startLocation, endLocation, cars, DateTime.Now, 6);
                MainWindow.DisplayThrowbackDialog("Successfull", "The reservation has been added to the list");
            }
            catch (Exception error)
            {
                MainWindow.DisplayThrowbackDialog("Reservation Creation Error", error.Message);
                return;
            }
        }

        private void inArrangement_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (inArrangement.SelectedIndex >= 0 && Enum.TryParse(inArrangement.SelectedItem.ToString().ToUpper(), out Type))
            {
                inFromDate.IsEnabled = true;
                inFromTime.IsEnabled = true;

                if (Type == ReservationArrangementType.WELLNESS)
                {
                    inUntilDate.IsEnabled = false;
                    inUntilTime.IsEnabled = false;
                    if (inFromDate.SelectedDate != null && inFromTime.SelectedIndex >= 0)
                    {
                        Until = From.AddHours(10); // Static Wellness rent hour (10 hours)
                        inUntilDate.DisplayDateStart = Until;
                        inUntilDate.SelectedDate = Until;
                        inUntilDate.DisplayDateEnd = Until;
                        inUntilTime.SelectedIndex = inUntilTime.Items.IndexOf(Until.ToString("HH:mm"));
                    }
                }
                else
                {
                    if (inFromDate.SelectedDate != null && inFromTime.SelectedIndex >= 0 && TimeSpan.TryParse(inFromTime.SelectedItem.ToString(), out TimeSpan time))
                    {
                        inUntilDate.IsEnabled = true;
                        inUntilTime.IsEnabled = true;
                    }
                }
            }
            else
            {
                inFromDate.IsEnabled = false;
                inFromTime.IsEnabled = false;
            }
        }

        private void inFromDate_SelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            CheckFromIputs();
        }

        private void inFromTime_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            CheckFromIputs();
        }

        private void CheckFromIputs()
        {
            if (inFromDate.SelectedDate != null && inFromTime.SelectedIndex >= 0 && TimeSpan.TryParse(inFromTime.SelectedItem.ToString(), out TimeSpan time))
            {
                From = inFromDate.SelectedDate ?? DateTime.Now;
                From = From + time;

                if (Type == ReservationArrangementType.WELLNESS)
                {
                    Until = From.AddHours(10); // Static Wellness rent hour (10 hours)
                    inUntilDate.DisplayDateStart = Until;
                    inUntilDate.SelectedDate = Until;
                    inUntilDate.DisplayDateEnd = Until;
                    inUntilTime.SelectedIndex = inUntilTime.Items.IndexOf(Until.ToString("HH:mm"));
                }
                else
                {
                    inUntilDate.IsEnabled = true;
                    inUntilTime.IsEnabled = true;
                    inUntilDate.SelectedDate = From;
                    inUntilDate.DisplayDateStart = From;
                    DateTime maxUntil = From.AddHours(11);
                    inUntilDate.DisplayDateEnd = maxUntil;
                }

                if (inUntilDate.SelectedDate != null && inUntilTime.SelectedIndex >= 0 && TimeSpan.TryParse(inUntilTime.SelectedItem.ToString(), out TimeSpan untilTime))
                {
                    LoadAvailableCars(From, Until, 6.0);
                }
            }
            else
            {
                inUntilDate.IsEnabled = false;
                inUntilTime.IsEnabled = false;
            }
        }

        private void inUntilDate_SelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            CheckUntilInput();
        }

        private void inUntilTime_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            CheckUntilInput();
        }

        private void CheckUntilInput()
        {
            if (inFromDate.SelectedDate != null && inFromTime.SelectedIndex >= 0 && TimeSpan.TryParse(inFromTime.SelectedItem.ToString(), out TimeSpan time))
            {
                if (inUntilDate.SelectedDate != null && inUntilTime.SelectedIndex >= 0 && TimeSpan.TryParse(inUntilTime.SelectedItem.ToString(), out TimeSpan untilTime))
                {
                    Until = inUntilDate.SelectedDate ?? DateTime.Now;
                    Until = Until + untilTime;
                    LoadAvailableCars(From, Until, 6.0);
                }
            }
        }
    }
}
