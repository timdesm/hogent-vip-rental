
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
            this.Title = this.Title.ToString().Replace("{id}", car.ID + "");

            Brand.Text = car.Brand;
            Model.Text = car.Type;
            Color.Text = car.Color;
            PriceFirst.Value = car.PriceFirst;
            PriceNight.Value = car.PriceNight;
            PriceWedding.Value = car.PriceWedding;
            PriceWellness.Value = car.PriceWellness;
            Available.IsOn = car.Available;
        } 
    }
}
