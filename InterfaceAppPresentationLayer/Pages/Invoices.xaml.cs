using DataLayer;
using DomainLayer.Domain;
using ModernWpf.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace InterfaceAppPresentationLayer.Pages
{
    /// <summary>
    /// Interaction logic for Invoices.xaml
    /// </summary>
    public partial class Invoices : ModernWpf.Controls.Page
    {
        private DataTable invoiceTable;

        public Invoices()
        {
            InitializeComponent();

            invoiceTable = new DataTable();
            invoiceTable.Clear();
            invoiceTable.Columns.Add(new DataColumn("ID", typeof(int)));
            invoiceTable.Columns.Add(new DataColumn("Client", typeof(string)));
            invoiceTable.Columns.Add(new DataColumn("Date", typeof(DateTime)));
            invoiceTable.Columns.Add(new DataColumn("Items", typeof(int)));
            invoiceTable.Columns.Add(new DataColumn("Sub total", typeof(string)));
            invoiceTable.Columns.Add(new DataColumn("Discount", typeof(string)));
            invoiceTable.Columns.Add(new DataColumn("Total Exc", typeof(string)));
            invoiceTable.Columns.Add(new DataColumn("VAT", typeof(string)));
            invoiceTable.Columns.Add(new DataColumn("Total Inc", typeof(string)));
            invoiceTable.Columns.Add(new DataColumn("Status", typeof(string)));
            DataTable.ItemsSource = invoiceTable.DefaultView;

            InitializeDataGrid_Data();
        }

        private void InitializeDataGrid_Data()
        {
            RentalManager manager = new RentalManager(new UnitOfWork(new RentalContext()));
            foreach (DomainLayer.Domain.Invoice invoice in manager.GetAllInvoices())
            {
                Client client = manager.GetClient(invoice.ClientID);
                List<InvoiceItem> invoiceItems = manager.GetInvoiceItems(invoice.ID);
                String clientStr = client.FirstName + " " + client.LastName;
                if (!string.IsNullOrWhiteSpace(client.CompanyName))
                    clientStr = "(" + client.CompanyName + ") " + clientStr;

                AddTableRow(invoice.ID, clientStr, invoice.InvoiceDate, invoiceItems.Count, invoice.SubTotal, invoice.Discount, invoice.TotalExc, invoice.VAT, invoice.TotalInc, (invoice.PaymentDue == 0) ? "Paid" : "Unpaid");
            }
        }

        private void AddTableRow(int id, string client, DateTime date, int items, Double subtotal, Double discount, Double totalExc, Double vat, Double totalInc, String status)
        {
            DataRow row = invoiceTable.NewRow();
            row[0] = id;
            row[1] = client;
            row[2] = date;
            row[3] = items;
            row[4] = string.Format("€{0:0.00}", subtotal);
            row[5] = string.Format("€{0:0.00}", discount);
            row[6] = string.Format("€{0:0.00}", totalExc);
            row[7] = string.Format("€{0:0.00}", vat);
            row[8] = string.Format("€{0:0.00}", totalInc);
            row[9] = status;
            invoiceTable.Rows.Add(row);
        }

        private void DataMenu_View(object sender, RoutedEventArgs e)
        {
            DataRowView dataRowView = (DataRowView)((MenuItem)e.Source).DataContext;
            int invoiceID = Int32.Parse(dataRowView[0].ToString());
            RentalManager manager = new RentalManager(new UnitOfWork(new RentalContext()));
        }

        private void DataMenu_MarkPaid(object sender, RoutedEventArgs e)
        {
            DataRowView dataRowView = (DataRowView)((MenuItem)e.Source).DataContext;
            int invoiceID = Int32.Parse(dataRowView[0].ToString());
            RentalManager manager = new RentalManager(new UnitOfWork(new RentalContext()));
        }
    }
}
