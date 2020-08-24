using System;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using DataLayer;
using DomainLayer.Domain;
using InterfaceAppPresentationLayer.Classes;
using InterfaceAppPresentationLayer.Dialogs;
using ModernWpf.Controls;

namespace InterfaceAppPresentationLayer.Pages
{
    /// <summary>
    /// Interaction logic for Cars.xaml
    /// </summary>
    public partial class Cars : ModernWpf.Controls.Page
    {
        private DataTable carTable;

        public Cars()
        {
            InitializeComponent();

            InitializeDataGrid();
            InitializeComboBox_Color();
        }

        private void InitializeDataGrid()
        {
            carTable = new DataTable();
            carTable.Clear();
            carTable.Columns.Add(new DataColumn("ID", typeof(int)));
            carTable.Columns.Add(new DataColumn("Brand", typeof(string)));
            carTable.Columns.Add(new DataColumn("Model", typeof(string)));
            carTable.Columns.Add(new DataColumn("Color", typeof(string)));
            carTable.Columns.Add(new DataColumn("First Hour Price", typeof(double)));
            carTable.Columns.Add(new DataColumn("Night Price", typeof(double)));
            carTable.Columns.Add(new DataColumn("Wedding Price", typeof(double)));
            carTable.Columns.Add(new DataColumn("Wellness Price", typeof(double)));
            carTable.Columns.Add(new DataColumn("Available", typeof(bool)));
            DataTable.ItemsSource = carTable.DefaultView;

            InitializeDataGrid_Data();
        }

        private void InitializeDataGrid_Data()
        {
            RentalManager manager = new RentalManager(new UnitOfWork(new RentalContext()));
            foreach (Car car in manager.GetAllCars())
            {
                AddTableRow(car.ID, car.Brand, car.Type, car.Color, car.PriceFirst, car.PriceNight, car.PriceWedding, car.PriceWellness, car.Available);
            }
        }

        private void InitializeComboBox_Color()
        {
            newColor.ItemsSource = typeof(Color).GetProperties();
        }

        private void AddTableRow(int id, string brand, string type, string color, double priceFirst, double priceNight, double priceWedding, double priceWellness, bool available)
        {
            DataRow row = carTable.NewRow();
            row[0] = id;
            row[1] = brand;
            row[2] = type;
            row[3] = color;
            row[4] = priceFirst;
            row[5] = priceNight;
            row[6] = priceWedding;
            row[7] = priceWellness;
            row[8] = available;
            carTable.Rows.Add(row);
        }

        private void Submit_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            String brand = newBrand.Text;
            String type = newType.Text; 
            Double priceFirst = newPriceFirst.Value;
            Double priceNight = newPriceNight.Value;
            Double priceWedding = newPriceWedding.Value;
            Double priceWellness = newPriceWellness.Value;
            Boolean available = newAvailable.IsOn;

            if (string.IsNullOrWhiteSpace(brand) || string.IsNullOrWhiteSpace(type)) { MainWindow.DisplayThrowbackDialog("Car Creation Error", "Brand and model are required fields and must be filled in"); return; }
            if(newColor.SelectedIndex < 0) { MainWindow.DisplayThrowbackDialog("Car Creation Error", "You must select a car color"); return; }
            String color = typeof(Color).GetProperties()[newColor.SelectedIndex].Name;
            if (priceFirst < 0 || priceNight < 0 || priceWedding < 0 || priceWellness < 0) { MainWindow.DisplayThrowbackDialog("Car Creation Error", "All prices must be 0 or higher"); return; }

            try
            {
                RentalManager manager = new RentalManager(new UnitOfWork(new RentalContext()));
                manager.AddCar(brand, type, color, priceFirst, priceNight, priceWedding, priceWellness, available);
                MainWindow.DisplayThrowbackDialog("Successfull", "The car has been added to the list");
            }
            catch (Exception error)
            {
                MainWindow.DisplayThrowbackDialog("An internal error occurred", error.Message);
                return;
            }
        }

        private void DataMenu_Edit(object sender, RoutedEventArgs e)
        {
            DataRowView dataRowView = (DataRowView)((MenuItem)e.Source).DataContext;
            int carID = Int32.Parse(dataRowView[0].ToString());
            RentalManager manager = new RentalManager(new UnitOfWork(new RentalContext()));
            Car car = manager.GetCar(carID);
            DialogService.OpenCarEditDialog(car);
        }

        private void DataMenu_View(object sender, RoutedEventArgs e)
        {
            DataRowView dataRowView = (DataRowView)((MenuItem)e.Source).DataContext;
            int carID = Int32.Parse(dataRowView[0].ToString());
            RentalManager manager = new RentalManager(new UnitOfWork(new RentalContext()));
            Car car = manager.GetCar(carID);
            DialogService.OpenCarViewDialog(car);
        }

        private async void DataMenu_Delete(object sender, RoutedEventArgs e)
        {
            DataRowView dataRowView = (DataRowView)((MenuItem)e.Source).DataContext;
            int carID = Int32.Parse(dataRowView[0].ToString());
            DeleteDialog dialog = new DeleteDialog("car #" + carID);
            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                RentalManager manager = new RentalManager(new UnitOfWork(new RentalContext()));
            }
        }
    }
}
