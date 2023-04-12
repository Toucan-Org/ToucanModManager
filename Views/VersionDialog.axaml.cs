using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace ToucanUI.Views
{
    public partial class VersionDialog : Window
    {
        public VersionDialog()
        {
            InitializeComponent();
        }

        //private void InitializeComponent()
        //{
        //    AvaloniaXamlLoader.Load(this);
        //}

        public string Message
        {
            get => MessageTextBlock.Text;
            set => MessageTextBlock.Text = value;
        }

        private void OnOKButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
