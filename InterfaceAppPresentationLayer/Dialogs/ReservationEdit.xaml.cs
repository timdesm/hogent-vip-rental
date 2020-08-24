using DataLayer;
using DomainLayer.Domain;
using InterfaceAppPresentationLayer.Classes;
using ModernWpf.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace InterfaceAppPresentationLayer.Dialogs
{
    /// <summary>
    /// Interaction logic for ReservationEdit.xaml
    /// </summary>
    public partial class ReservationEdit : ContentDialog
    {
        public DataTable AvailableCarsTable;
        Client client;
        List<Car> reservationCars;

        ReservationArrangementType Type;
        DateTime From;
        DateTime Until;

        public ReservationEdit(Reservation reservation)
        {
            InitializeComponent();
            InitializeComboBox_Clients();
            InitializeComboxBox_Arrangement();
            InitializeComboxBox_StartLocation();
            InitializeComboxBox_EndLocation();
            InitializeAvailbaleCars();
            InitializeService.InitializeSelection(FromTime);
            InitializeService.InitializeSelection(UntilTime);

            this.Title = this.Title.ToString().Replace("{id}", reservation.ID + "");
            Type = reservation.Arrangement;
            From = reservation.ReservationDate;
            Until = reservation.ReservedUntil;

            RentalManager manager = new RentalManager(new UnitOfWork(new RentalContext()));
            client = manager.GetClient(reservation.ClientID);
            reservationCars = manager.GetReservationCars(reservation.ID);

            Client.SelectedIndex = manager.GetAllClients().IndexOf(client);
            Arrangement.SelectedIndex = Arrangement.Items.IndexOf(char.ToUpper(reservation.Arrangement.ToString().ToLower()[0]) + reservation.Arrangement.ToString().ToLower().Substring(1));
            StartLocation.SelectedIndex = StartLocation.Items.IndexOf(char.ToUpper(reservation.StartLocation.ToLower()[0]) + reservation.StartLocation.ToLower().Substring(1));
            EndLocation.SelectedIndex = EndLocation.Items.IndexOf(char.ToUpper(reservation.EndLocation.ToLower()[0]) + reservation.EndLocation.ToLower().Substring(1));
            FromTime.SelectedIndex = FromTime.Items.IndexOf(reservation.ReservationDate.ToString("HH:00"));
            UntilTime.SelectedIndex = UntilTime.Items.IndexOf(reservation.ReservedUntil.ToString("HH:00"));
            if (reservation.ReservationEnded > DateTime.MinValue)
            {
                ReturnedDate.SelectedDate = reservation.ReservationEnded.Date;
                ReturnedTime.SelectedIndex = ReturnedTime.Items.IndexOf(reservation.ReservationEnded.ToString("HH:00"));
            }

            FromDate.SelectedDate = reservation.ReservationDate.Date;
            UntilDate.SelectedDate = reservation.ReservedUntil.Date;
        }

        private void InitializeComboBox_Clients()
        {
            RentalManager manager = new RentalManager(new UnitOfWork(new RentalContext()));
            foreach (Client client in manager.GetAllClients())
            {
                if (string.IsNullOrWhiteSpace(client.CompanyName))
                    Client.Items.Add("#" + client.ID + " " + client.FirstName + " " + client.LastName + " - " + char.ToUpper(client.Type.ToString().ToLower()[0]) + client.Type.ToString().ToLower().Substring(1));
                else
                    Client.Items.Add("#" + client.ID + " (" + client.CompanyName + ") " + client.FirstName + " " + client.LastName + " - " + char.ToUpper(client.Type.ToString().ToLower()[0]) + client.Type.ToString().ToLower().Substring(1));
            }
        }

        private void InitializeComboxBox_Arrangement()
        {
            foreach (ReservationArrangementType type in (ReservationArrangementType[])Enum.GetValues(typeof(ReservationArrangementType)))
                Arrangement.Items.Add(char.ToUpper(type.ToString().ToLower()[0]) + type.ToString().ToLower().Substring(1));
        }

        private void InitializeComboxBox_StartLocation()
        {
            foreach (GarageLocations loc in (GarageLocations[])Enum.GetValues(typeof(GarageLocations)))
                StartLocation.Items.Add(char.ToUpper(loc.ToString().ToLower()[0]) + loc.ToString().ToLower().Substring(1));
        }
        private void InitializeComboxBox_EndLocation()
        {
            foreach (GarageLocations loc in (GarageLocations[])Enum.GetValues(typeof(GarageLocations)))
                EndLocation.Items.Add(char.ToUpper(loc.ToString().ToLower()[0]) + loc.ToString().ToLower().Substring(1));
        }

        private void InitializeAvailbaleCars()
        {
            AvailableCarsTable = new DataTable();
            AvailableCarsTable.Clear();
            AvailableCarsTable.Columns.Add(new DataColumn("ID", typeof(int)));
            AvailableCarsTable.Columns.Add(new DataColumn("Brand", typeof(string)));
            AvailableCarsTable.Columns.Add(new DataColumn("Model", typeof(string)));
            AvailableCarsTable.Columns.Add(new DataColumn("Color", typeof(string)));
            AvailableCarsTable.Columns.Add(new DataColumn("First h", typeof(string)));
            AvailableCarsTable.Columns.Add(new DataColumn("Extra h", typeof(string)));
            AvailableCarsTable.Columns.Add(new DataColumn("Night", typeof(string)));
            AvailableCarsTable.Columns.Add(new DataColumn("Wedding", typeof(string)));
            AvailableCarsTable.Columns.Add(new DataColumn("Wellness", typeof(string)));
            CarTable.ItemsSource = AvailableCarsTable.DefaultView;

            AvailableCarsTable.Rows.Clear();
        }

        private void AddTableRow(int id, string brand, string type, string color, string first, string extra, string night, string wedding, string wellness)
        {
            DataRow row = AvailableCarsTable.NewRow();
            row[0] = id;
            row[1] = brand;
            row[2] = type;
            row[3] = color;
            row[4] = first;
            row[5] = extra;
            row[6] = night;
            row[7] = wedding;
            row[8] = wellness;
            AvailableCarsTable.Rows.Add(row);
        }

        private void FromDate_SelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            CheckFromIputs();
        }

        private void FromTime_SelectedTimeChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            CheckFromIputs();
        }

        private void UntilDate_SelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            CheckUntilInput();
        }

        private void UntilTime_SelectedTimeChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            CheckUntilInput();
        }

        private void CheckFromIputs()
        {
            if (FromDate.SelectedDate != null && FromTime.SelectedIndex >= 0 && TimeSpan.TryParse(FromTime.SelectedItem.ToString(), out TimeSpan time))
            {
                From = FromDate.SelectedDate ?? DateTime.Now;
                From = From + time;

                if (Type == ReservationArrangementType.WELLNESS)
                {
                    UntilDate.IsEnabled = false;
                    UntilTime.IsEnabled = false;
                    Until = FromDate.SelectedDate.Value.Date.AddHours(11); // Static Wellness rent hour (10 hours)
                    UntilDate.DisplayDateStart = Until.Date;
                    UntilDate.DisplayDate = Until.Date;
                    UntilDate.SelectedDate = Until.Date;
                    UntilDate.DisplayDateEnd = Until.Date;
                    UntilTime.SelectedIndex = UntilTime.Items.IndexOf(Until.ToString("HH:00"));
                }
                else
                {
                    UntilDate.IsEnabled = true;
                    UntilTime.IsEnabled = true;
                    UntilDate.SelectedDate = FromDate.SelectedDate.Value.Date;
                    UntilDate.DisplayDateStart = FromDate.SelectedDate.Value.Date;
                    DateTime maxUntil = FromDate.SelectedDate.Value.Date.AddHours(11);
                    UntilDate.DisplayDateEnd = maxUntil;
                }

                if (UntilDate.SelectedDate != null && UntilTime.SelectedIndex >= 0 && TimeSpan.TryParse(UntilTime.SelectedItem.ToString(), out TimeSpan untilTime))
                {
                    LoadAvailableCars(From, Until, 6.0);
                }
            }
            else
            {
                UntilDate.IsEnabled = false;
                UntilDate.IsEnabled = false;
            }
        }

        private void CheckUntilInput()
        {
            if (FromDate.SelectedDate != null && FromTime.SelectedIndex >= 0 && TimeSpan.TryParse(FromTime.SelectedItem.ToString(), out TimeSpan time))
            {
                if (UntilDate.SelectedDate != null && UntilTime.SelectedIndex >= 0 && TimeSpan.TryParse(UntilTime.SelectedItem.ToString(), out TimeSpan untilTime))
                {
                    Until = UntilDate.SelectedDate.Value.Date;
                    Until = Until + untilTime;
                    LoadAvailableCars(From, Until, 6.0);
                }
            }
        }

        private void LoadAvailableCars(DateTime from, DateTime until, double hourRange)
        {
            AvailableCarsTable.Rows.Clear();
            RentalManager manager = new RentalManager(new UnitOfWork(new RentalContext()));
            List<Car> availableCars = manager.GetAvailableCars(from, until, hourRange);
            foreach (Car car in reservationCars)
            {
                if(!availableCars.Where(c => c.ID == car.ID).Any())
                    AddTableRow(car.ID, car.Brand, car.Type, car.Color, (car.PriceFirst > 0) ? string.Format("€{0:0.00}", car.PriceFirst) : "N/A", (car.PriceFirst > 0) ? string.Format("€{0:0.00}", car.PriceFirst * 0.65) : "N/A", (car.PriceNight > 0) ? string.Format("€{0:0.00}", car.PriceNight) : "N/A", (car.PriceWedding > 0) ? string.Format("€{0:0.00}", car.PriceWedding) : "N/A", (car.PriceWellness > 0) ? string.Format("€{0:0.00}", car.PriceWellness) : "N/A");
            }
            foreach (Car car in availableCars)
                AddTableRow(car.ID, car.Brand, car.Type, car.Color, (car.PriceFirst > 0) ? string.Format("€{0:0.00}", car.PriceFirst) : "N/A", (car.PriceFirst > 0) ? string.Format("€{0:0.00}", car.PriceFirst * 0.65) : "N/A", (car.PriceNight > 0) ? string.Format("€{0:0.00}", car.PriceNight) : "N/A", (car.PriceWedding > 0) ? string.Format("€{0:0.00}", car.PriceWedding) : "N/A", (car.PriceWellness > 0) ? string.Format("€{0:0.00}", car.PriceWellness) : "N/A");
            foreach (Car car in reservationCars)
            {
                int index = GetDataGridIndex(car);
                object item = CarTable.Items[index];
                CarTable.SelectedItems.Add(item);
                CarTable.ScrollIntoView(item);
            }
        }

        private int GetDataGridIndex(Car car)
        {
            return 0;
        }
    }
}
