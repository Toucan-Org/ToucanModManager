using ReactiveUI;
using System.Diagnostics;
using System.Windows.Input;


namespace ToucanUI.ViewModels
{
    public class SidePanelViewModel : ViewModelBase
    {
        // =====================
        // VIEW MODELS
        // =====================
        public MainWindowViewModel MainViewModel { get; }



        // =====================
        // COMMANDS
        // =====================
        public ICommand OpenWebsiteCommand { get; }
        public ICommand CloseSidePanelCommand { get; }



        // =====================
        // VARIABLES
        // =====================

        // Tracks if the side panel should be visible or not
        private bool _sidePanelVisible = false;
        public bool SidePanelVisible
        {
            get => _sidePanelVisible;
            set => this.RaiseAndSetIfChanged(ref _sidePanelVisible, value);
        }


        // =====================
        // CONSTRUCTOR
        // =====================
        public SidePanelViewModel(MainWindowViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;

            OpenWebsiteCommand = ReactiveCommand.Create(() =>
            {
                try
                {
                    // Navigate to mod website
                    Process.Start(new ProcessStartInfo(mainViewModel.SelectedMod.ModObject.Website) { UseShellExecute = true });
                }
                catch (System.InvalidOperationException)
                {
                    Console.WriteLine("[WARNING] No Website was found in SelectedMod!");
                }


            });

            CloseSidePanelCommand = ReactiveCommand.Create(() =>
            {
                //This de-selects a selected mod from the listbox
                MainViewModel.SelectedMod = null;
                SidePanelVisible = false;

            });

        }

    }
}
