using Avalonia.ReactiveUI;
using ToucanUI.ViewModels;

namespace ToucanUI.Views
{
    public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();
            this.MinWidth = 1000;
            this.MinHeight = 600;
        }

        public MainWindow(MainWindowViewModel viewModel) : this()
        {
            DataContext = viewModel;
            HeaderViewControl.DataContext = viewModel.HeaderVM;
            //ControlPanelViewControl.DataContext = viewModel.ControlPanelVM;
            ModlistViewControl.DataContext = viewModel.ModlistVM;
            SidePanelViewControl.DataContext = viewModel.SidePanelVM;
        }

    }
}
