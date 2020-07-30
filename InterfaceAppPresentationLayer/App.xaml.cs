using ControlzEx.Theming;
using ModernWpf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace InterfaceAppPresentationLayer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static bool RepositoryImageMode { get; set; } = false;

        public App() { }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            SetLanguageDictionary();
        }

        public static void SetDarkTheme(bool enable = true)
        {
            if(enable)
                ModernWpf.ThemeManager.Current.ApplicationTheme = ApplicationTheme.Dark;
            else
                ModernWpf.ThemeManager.Current.ApplicationTheme = ApplicationTheme.Light;
        }

        private static void SetLanguageDictionary()
        {
            ResourceDictionary dict = new ResourceDictionary();
            SetLanguage(Thread.CurrentThread.CurrentCulture.ToString());
        }

        public static void SetLanguage(string uniLang)
        {
            ResourceDictionary dict = new ResourceDictionary();
            switch (uniLang)
            {
                case "nl-BE":
                    dict.Source = new Uri("..\\Resources\\StringResources.nl-BE.xaml", UriKind.Relative);
                    break;
                case "nl-NL":
                    dict.Source = new Uri("..\\Resources\\StringResources.nl-BE.xaml", UriKind.Relative);
                    break;
                case "fr-FR":
                    dict.Source = new Uri("..\\Resources\\StringResources.fr-FR.xaml", UriKind.Relative);
                    break;
                case "fr-CA":
                    dict.Source = new Uri("..\\Resources\\StringResources.fr-FR.xaml", UriKind.Relative);
                    break;
                default:
                    dict.Source = new Uri("..\\Resources\\StringResources.xaml", UriKind.Relative);
                    break;
            }
            Application.Current.Resources.MergedDictionaries.Add(dict);
        }
    }
}
