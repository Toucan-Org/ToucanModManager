using Avalonia.Controls;
using ToucanUI.ViewModels;

namespace ToucanUI.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // In this code-behind, create the MainWindowViewModel and set the DataContexts for each child view
            var mainWindowViewModel = new MainWindowViewModel();
            DataContext = mainWindowViewModel;
            HeaderViewControl.DataContext = mainWindowViewModel.HeaderVM;
            ControlPanelViewControl.DataContext = mainWindowViewModel.ControlPanelVM;
            ModlistViewControl.DataContext = mainWindowViewModel.ModlistVM;
            SidePanelViewControl.DataContext = mainWindowViewModel.SidePanelVM;
        }

    }
}