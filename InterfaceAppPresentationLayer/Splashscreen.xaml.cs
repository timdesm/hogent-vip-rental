using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InterfaceAppPresentationLayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Splashscreen : Window
    {
        Splashscreen splashScreen;
        MainWindow mainWindow;

        public Splashscreen()
        {
            InitializeComponent();
            splashScreen = this;
            mainWindow = new MainWindow();
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += worker_DoWork;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.RunWorkerAsync();
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            for(int i = 0; i < 201; i++)
            {
                (sender as BackgroundWorker).ReportProgress(i / 2);
                Thread.Sleep(12);
            }
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressBar.Value = e.ProgressPercentage;
            if(e.ProgressPercentage >= 100)
            {
                splashScreen.Close();
                mainWindow.ShowDialog();
            }
        }
    }
}
