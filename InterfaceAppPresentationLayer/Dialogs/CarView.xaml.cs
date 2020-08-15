
using DomainLayer.Domain;
using ModernWpf.Controls;

namespace InterfaceAppPresentationLayer.Dialogs
{
    /// <summary>
    /// Interaction logic for CarView.xaml
    /// </summary>
    public partial class CarView : ContentDialog
    {
        public CarView(Car car)
        {
            InitializeComponent();
            this.Title = "Car #" + car.ID;
            Brand.Text = car.Brand;
            Model.Text = car.Type;
            Color.Text = car.Color;
            PriceFirst.Text = string.Format("€{0:0.00}", car.PriceFirst);
            PriceNight.Text = string.Format("€{0:0.00}", car.PriceNight);
            PriceWedding.Text = string.Format("€{0:0.00}", car.PriceWedding);
            PriceWellness.Text = string.Format("€{0:0.00}", car.PriceWellness);
            Available.IsOn = car.Available;
        } 
    }
}
