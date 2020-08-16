using DomainLayer.Domain;
using ModernWpf.Controls;
using System;
using System.Windows;

namespace InterfaceAppPresentationLayer.Dialogs
{
    /// <summary>
    /// Interaction logic for ClientView.xaml
    /// </summary>
    public partial class ClientView : ContentDialog
    {
        public ClientView(DomainLayer.Domain.Client client)
        {
            InitializeComponent();
            InitializeComboBox_Country();
            InitializeComboxBox_Type();

            this.Title = this.Title.ToString().Replace("{id}", client.ID + "");

            FirstName.Text = client.FirstName;
            LastName.Text = client.LastName;
            Email.Text = client.Email;
            Phone.Text = client.Phone;
            Street.Text = client.AddressStreet;
            HouseNumber.Text = client.AddressNumber;
            Box.Text = client.AddressBus;
            City.Text = client.AddressCity;
            Zip.Text = client.AddressZip;
            if(Country.Items.Contains(client.AddressCounty)) Country.SelectedIndex = Country.Items.IndexOf(client.AddressCounty);
            Type.SelectedIndex = Type.Items.IndexOf(char.ToUpper(client.Type.ToString().ToLower()[0]) + client.Type.ToString().ToLower().Substring(1));
            Company.Text = client.CompanyName;
            VAT.Text = client.VATNumber;

            if (!string.IsNullOrWhiteSpace(client.CompanyName)) Company.Visibility = Visibility.Visible;
            if (!string.IsNullOrWhiteSpace(client.VATNumber)) VAT.Visibility = Visibility.Visible;
        }

        private void InitializeComboBox_Country()
        {
            Country.Items.Add("Belgium");
            Country.Items.Add("Netherlands");
            Country.Items.Add("France");
            Country.Items.Add("Germany");
            Country.Items.Add("Italy");
        }

        private void InitializeComboxBox_Type()
        {
            foreach (ClientType type in (ClientType[])Enum.GetValues(typeof(ClientType)))
                Type.Items.Add(char.ToUpper(type.ToString().ToLower()[0]) + type.ToString().ToLower().Substring(1));
        }

        private void Type_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            
        }
    }
}
