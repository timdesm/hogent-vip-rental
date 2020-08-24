using DomainLayer.Domain;
using ModernWpf.Controls;
using System;
using System.Configuration;
using System.Linq;
using System.Windows.Controls.Primitives;

namespace InterfaceAppPresentationLayer.Dialogs
{
    /// <summary>
    /// Interaction logic for ClientEdit.xaml
    /// </summary>
    public partial class ClientEdit : ContentDialog
    {
        public ClientEdit(Client client)
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
            if (Country.Items.Contains(client.AddressCounty)) Country.SelectedIndex = Country.Items.IndexOf(client.AddressCounty);
            Type.SelectedIndex = Type.Items.IndexOf(char.ToUpper(client.Type.ToString().ToLower()[0]) + client.Type.ToString().ToLower().Substring(1));
            Company.Text = client.CompanyName;
            VAT.Text = client.VATNumber;
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

        private void ChangeCompanyFieldVisibility(bool visible = false)
        {
            if (visible) {
                Company.Visibility = System.Windows.Visibility.Visible;
                VAT.Visibility = System.Windows.Visibility.Visible;
            }
            else {
                Company.Visibility = System.Windows.Visibility.Collapsed;
                VAT.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private void Type_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            ClientType type = (ClientType) Enum.Parse(typeof(ClientType), Type.SelectedItem.ToString().Split(new string[] { ": " }, StringSplitOptions.None).Last().ToUpper());
            if (type == ClientType.PRIVATE || type == ClientType.VIP) ChangeCompanyFieldVisibility(false);
            else ChangeCompanyFieldVisibility(true);
        }

        
    }
}
