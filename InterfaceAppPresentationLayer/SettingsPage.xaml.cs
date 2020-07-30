using ControlzEx.Theming;
using InterfaceAppPresentationLayer.Classes;
using MahApps.Metro.Controls;
using ModernWpf;
using ModernWpf.Controls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using ToggleSwitch = ModernWpf.Controls.ToggleSwitch;

namespace InterfaceAppPresentationLayer
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : ModernWpf.Controls.Page
    {
        public SettingsPage()
        {
            InitializeComponent();

            if (ModernWpf.ThemeManager.Current.ApplicationTheme == ApplicationTheme.Dark)
                themeToggle.IsOn = true;
            themeToggle.Toggled += ThemeSwitch_Toggled;
            InitializeComboBox_Language();
        }

        private void InitializeComboBox_Language()
        {
            string json = FileService.GetFileAsString(@"Resources/unicodeLanguage.json");
            var data = (JObject)JsonConvert.DeserializeObject(json);
            string currentLanguage = data[Thread.CurrentThread.CurrentCulture.ToString()].Value<string>();

            languageBox.Items.Add("Deutsche");
            languageBox.Items.Add("English");
            languageBox.Items.Add("Français");
            languageBox.Items.Add("Nederlands");

            if (currentLanguage != null)
                languageBox.SelectedIndex = languageBox.Items.IndexOf(currentLanguage);
        }

        private void ThemeSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch toggleSwitch = sender as ToggleSwitch;
            if(toggleSwitch != null)
            {
                App.SetDarkTheme(toggleSwitch.IsOn);
            }
        }

        private void languageBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (languageBox.SelectedItem.ToString().Split(new string[] { ": " }, StringSplitOptions.None).Last())
            {
                case "Deutsche":
                    App.SetLanguage("de-DE");
                    break;
                case "Français":
                    App.SetLanguage("fr-FR");
                    break;
                case "Nederlands":
                    App.SetLanguage("nl-NL");
                    break;
                default:
                    App.SetLanguage("en-US");
                    break;
            }
        }
    }
}
