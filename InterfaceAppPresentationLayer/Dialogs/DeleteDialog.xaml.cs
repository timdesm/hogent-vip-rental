using ModernWpf.Controls;

namespace InterfaceAppPresentationLayer.Dialogs
{
    /// <summary>
    /// Interaction logic for DeleteDialog.xaml
    /// </summary>
    public partial class DeleteDialog : ContentDialog
    {
        public DeleteDialog(string type = "content")
        {
            InitializeComponent();
            this.Title = this.Title.ToString().Replace("{type}", type);
        }
    }
}
