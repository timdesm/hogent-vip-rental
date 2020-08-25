using DataLayer;
using DomainLayer.Domain;
using InterfaceAppPresentationLayer.Classes;
using ModernWpf.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InterfaceAppPresentationLayer.Pages
{
    /// <summary>
    /// Interaction logic for NewClient.xaml
    /// </summary>
    public partial class NewClient : ModernWpf.Controls.Page
    {
        public NewClient()
        {
            InitializeComponent();

            InitializeComboBox_Country();
            InitializeComboxBox_Type();
        }

        private void InitializeComboBox_Country()
        {
            inAddrCountry.Items.Add("Belgium");
            inAddrCountry.Items.Add("Netherlands");
            inAddrCountry.Items.Add("France");
            inAddrCountry.Items.Add("Germany");
            inAddrCountry.Items.Add("Italy");
        }

        private void InitializeComboxBox_Type()
        {
            foreach (ClientType type in (ClientType[])Enum.GetValues(typeof(ClientType)))
            {
                inAccountType.Items.Add(char.ToUpper(type.ToString().ToLower()[0]) + type.ToString().ToLower().Substring(1));
            }
        }

        private void inFirstName_TextChanged(object sender, TextChangedEventArgs e)
        {
            avatarPicture.DisplayName = inFirstName.Text + " " + inLastName.Text;
            prevName.Text = inFirstName.Text + " " + inLastName.Text;
        }

        private void inLastName_TextChanged(object sender, TextChangedEventArgs e)
        {
            avatarPicture.DisplayName = inFirstName.Text + " " + inLastName.Text;
            prevName.Text = inFirstName.Text + " " + inLastName.Text;
        }

        private void inPhone_TextChanged(object sender, TextChangedEventArgs e)
        {
            prevPhone.Text = inPhone.Text;
        }

        private void inEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            prevEmail.Text = inEmail.Text;
        }

        private void inCompany_TextChanged(object sender, TextChangedEventArgs e)
        {
            prevCompany.Text = inCompany.Text;
        }
        private void inVat_TextChanged(object sender, TextChangedEventArgs e)
        {
            prevVAT.Text = inVAT.Text;
        }

        private void inAccountType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch(inAccountType.SelectedItem.ToString().Split(new string[] { ": " }, StringSplitOptions.None).Last())
            {
                case "Private":
                    avatarPicture.Foreground = Brushes.White;
                    changeCompanyFieldVisibility(false);
                    break;
                case "Vip":
                    avatarPicture.Foreground = Brushes.LightGoldenrodYellow;
                    changeCompanyFieldVisibility(false);
                    break;
                case "Agency":
                    avatarPicture.Foreground = Brushes.LightBlue;
                    changeCompanyFieldVisibility(true);
                    break;
                case "Promoter":
                    avatarPicture.Foreground = Brushes.LightCoral;
                    changeCompanyFieldVisibility(true);
                    break;
                case "Planner":
                    avatarPicture.Foreground = Brushes.LightGreen;
                    changeCompanyFieldVisibility(true);
                    break;
                case "Eventagency":
                    avatarPicture.Foreground = Brushes.LightSalmon;
                    changeCompanyFieldVisibility(true);
                    break;
            }
        }

        private void changeCompanyFieldVisibility(bool visible = false)
        {
            if(visible)
            {
                inCompany.Visibility = Visibility.Visible;
                inVAT.Visibility = Visibility.Visible;
                prevCompany.Visibility = Visibility.Visible;
                prevVAT.Visibility = Visibility.Visible;
            }
            else
            {
                inCompany.Visibility = Visibility.Hidden;
                inVAT.Visibility = Visibility.Hidden;
                prevCompany.Visibility = Visibility.Hidden;
                prevVAT.Visibility = Visibility.Hidden;
            }
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            string firstName = inFirstName.Text;
            string lastName = inLastName.Text;
            string email = inEmail.Text;
            string phone = inPhone.Text;
            string addrStreet = inAddrStreet.Text;
            string addrNumber = inAddrNumber.Text;
            string addrBox = inAddrBox.Text;
            string addrZip = inAddrZip.Text;
            string addrCity = inAddrCity.Text;
            string addrCountry = "";
            string company = inCompany.Text;
            string vat = inVAT.Text;
            if (inAddrCountry.SelectedIndex >= 0) addrCountry = inAddrCountry.SelectedItem.ToString().Split(new string[] { ": " }, StringSplitOptions.None).Last();

            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName)) { DisplayThrowbackDialog("You must fill in a first and list name"); return; }
            if (inAccountType.SelectedIndex < 0) { DisplayThrowbackDialog("You must select an account type"); return; }
            if (!Enum.TryParse(typeof(ClientType), inAccountType.SelectedItem.ToString().ToUpper().Split(new string[] { ": " }, StringSplitOptions.None).Last(), out object objtype)) {
                DisplayThrowbackDialog("Something went wrong with the client type, try agian");
                return;
            }
            ClientType type = (ClientType) objtype;
            if(type != ClientType.PRIVATE && type != ClientType.VIP) if(string.IsNullOrWhiteSpace(vat)) { DisplayThrowbackDialog("VAT number must be filled in for that account type"); return; }

            try
            {
                RentalManager manager = new RentalManager(new UnitOfWork(new RentalContext()));
                manager.AddClient(firstName, lastName, email, phone, addrStreet, addrNumber, addrBox, addrZip, addrCity, addrCountry, type, company, vat);
                MailService.Send_WelcomeMail(email, firstName, lastName, type, phone, company); // Send welcome mail
                DisplayThrowbackDialog("New client has been added");
            }
            catch(Exception error)
            {
                DisplayThrowbackDialog(error.Message);
                return;
            }
        }

        private async void DisplayThrowbackDialog(string message)
        {
            ContentDialog errorDialog = new ContentDialog
            {
                Title = "Client Creation Error",
                Content = message,
                CloseButtonText = "Close"
            };
            ContentDialogResult result = await errorDialog.ShowAsync();
        }
    }
}
