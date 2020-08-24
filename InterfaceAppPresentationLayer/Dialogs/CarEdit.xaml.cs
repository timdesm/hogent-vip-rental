using DomainLayer.Domain;
using ModernWpf.Controls;
using System;
using System.Drawing;

namespace InterfaceAppPresentationLayer.Dialogs
{
    /// <summary>
    /// Interaction logic for CarEdit.xaml
    /// </summary>
    public partial class CarEdit : ContentDialog
    {
        public CarEdit(Car car)
        {
            InitializeComponent();
            InitializeComboBox_Color();

            Brand.Text = car.Brand;
            Model.Text = car.Type;
            Color.SelectedIndex = Color.Items.IndexOf(System.Drawing.Color.FromName(car.Color).Name);
            PriceFirst.Value = car.PriceFirst;
            PriceNight.Value = car.PriceNight;
            PriceWedding.Value = car.PriceWedding;
            PriceWellness.Value = car.PriceWellness;
            Available.IsOn = car.Available;
        }

        private void InitializeComboBox_Color()
        {
            Color.ItemsSource = typeof(Color).GetProperties();
        }
    }
}
