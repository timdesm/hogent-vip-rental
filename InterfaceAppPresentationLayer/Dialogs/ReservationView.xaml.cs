using DataLayer;
using DomainLayer.Domain;
using InterfaceAppPresentationLayer.Classes;
using ModernWpf.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Controls;

namespace InterfaceAppPresentationLayer.Dialogs
{
    /// <summary>
    /// Interaction logic for ReservationView.xaml
    /// </summary>
    public partial class ReservationView : ContentDialog
    {
        Client client;

        private DataTable carTable;
        private DataTable invoiceItems;

        public ReservationView(Reservation reservation)
        {
            InitializeComponent();
            InitializeDataGrid();

            this.Title = this.Title.ToString().Replace("{id}", reservation.ID + "");

            RentalManager manager = new RentalManager(new UnitOfWork(new RentalContext()));
            Invoice invoice = manager.GetInvoice(reservation.InvoiceID);
            client = manager.GetClient(reservation.ClientID);
            List<Car> reservationCars = manager.GetReservationCars(reservation.ID);
            List<InvoiceItem> invoiceItems = manager.GetInvoiceItems(reservation.InvoiceID);

            Client.Text = client.FirstName + " " + client.LastName;
            if(!string.IsNullOrWhiteSpace(client.CompanyName))
                Client.Text = "(" + client.CompanyName + ") " +  client.FirstName + " " + client.LastName;
            Arrangement.Text = char.ToUpper(reservation.Arrangement.ToString().ToLower()[0]) + reservation.Arrangement.ToString().ToLower().Substring(1);
            StartLocation.Text = char.ToUpper(reservation.StartLocation.ToLower()[0]) + reservation.StartLocation.ToLower().Substring(1);
            EndLocation.Text = char.ToUpper(reservation.EndLocation.ToLower()[0]) + reservation.EndLocation.ToLower().Substring(1);
            From.Text = reservation.ReservationDate.ToString();
            Until.Text = reservation.ReservedUntil.ToString();

            InvoiceID.Text = "#" + reservation.InvoiceID;
            InvoiceDate.Text = invoice.InvoiceDate.ToString();
            InvoiceDiscountPercent.Text = invoice.DiscountPercent + "%";
            InvoiceDiscount.Text = string.Format("€{0:0.00}", invoice.Discount);
            InvoiceTotalExc.Text = string.Format("€{0:0.00}", invoice.TotalExc);
            InvoiceVAT.Text = string.Format("€{0:0.00}", invoice.VAT);
            InvoiceTotalInc.Text = string.Format("€{0:0.00}", invoice.TotalInc);
            InvoiceDue.Text = string.Format("€{0:0.00}", invoice.PaymentDue);

            foreach (Car car in reservationCars)
                AddTableRow(car.ID, car.Brand, car.Type, car.Color, car.Available);
            foreach (InvoiceItem ii in invoiceItems)
                AddInvoiceItemsRow(ii.Amount, ii.Description, ii.UnitPrice, ii.Total);
        }

        private void InitializeDataGrid()
        {
            carTable = new DataTable();
            carTable.Clear();
            carTable.Columns.Add(new DataColumn("ID", typeof(int)));
            carTable.Columns.Add(new DataColumn("Brand", typeof(string)));
            carTable.Columns.Add(new DataColumn("Model", typeof(string)));
            carTable.Columns.Add(new DataColumn("Color", typeof(string)));
            carTable.Columns.Add(new DataColumn("Available", typeof(bool)));
            CarTable.ItemsSource = carTable.DefaultView;

            invoiceItems = new DataTable();
            invoiceItems.Clear();
            invoiceItems.Columns.Add(new DataColumn("Amount", typeof(int)));
            invoiceItems.Columns.Add(new DataColumn("Description", typeof(string)));
            invoiceItems.Columns.Add(new DataColumn("Unit Price", typeof(string)));
            invoiceItems.Columns.Add(new DataColumn("Total Price", typeof(string)));
            InvoiceItems.ItemsSource = invoiceItems.DefaultView;
        }

        private void AddTableRow(int id, string brand, string type, string color, bool available)
        {
            DataRow row = carTable.NewRow();
            row[0] = id;
            row[1] = brand;
            row[2] = type;
            row[3] = color;
            row[4] = available;
            carTable.Rows.Add(row);
        }

        private void AddInvoiceItemsRow(int id, string description, double unitPrice, double totalPrice)
        {
            DataRow row = invoiceItems.NewRow();
            row[0] = id;
            row[1] = description;
            row[2] = unitPrice;
            row[3] = totalPrice;
            invoiceItems.Rows.Add(row);
        }

        private void CarMenu_View(object sender, System.Windows.RoutedEventArgs e)
        {
            this.Hide();

            DataRowView dataRowView = (DataRowView)((MenuItem)e.Source).DataContext;
            int carID = Int32.Parse(dataRowView[0].ToString());
            RentalManager manager = new RentalManager(new UnitOfWork(new RentalContext()));
            Car car = manager.GetCar(carID);
            DialogService.OpenCarViewDialog(car);
        }

        private void ClientView_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.Hide();
            DialogService.OpenClientViewDialog(client);
        }
    }
}
