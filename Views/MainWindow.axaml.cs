using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using System.Reactive;
using ToucanUI.ViewModels;

namespace ToucanUI.Views
{
    public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(MainWindowViewModel viewModel) : this()
        {
            DataContext = viewModel;
            HeaderViewControl.DataContext = viewModel.HeaderVM;
            ControlPanelViewControl.DataContext = viewModel.ControlPanelVM;
            ModlistViewControl.DataContext = viewModel.ModlistVM;
            SidePanelViewControl.DataContext = viewModel.SidePanelVM;
        }

    }
}
