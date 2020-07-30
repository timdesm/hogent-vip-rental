using ModernWpf.Controls;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace InterfaceAppPresentationLayer.Pages
{
    /// <summary>
    /// Interaction logic for Reservations.xaml
    /// </summary>
    public partial class Reservations : ModernWpf.Controls.Page
    {
        private DataTable reservationTable;

        public Reservations()
        {
            InitializeComponent();

            reservationTable = new DataTable();
            reservationTable.Clear();
            reservationTable.Columns.Add(new DataColumn("Id", typeof(int)));
            reservationTable.Columns.Add(new DataColumn("Client", typeof(string)));
            reservationTable.Columns.Add(new DataColumn("Cars", typeof(string)));
            reservationTable.Columns.Add(new DataColumn("Date", typeof(DateTime)));
            reservationTable.Columns.Add(new DataColumn("Length", typeof(TimeSpan)));
            reservationTable.Columns.Add(new DataColumn("Arrangements", typeof(string)));
            DataTable.ItemsSource = reservationTable.DefaultView;
        }

        private void DataTableAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {

        }
    }
}
