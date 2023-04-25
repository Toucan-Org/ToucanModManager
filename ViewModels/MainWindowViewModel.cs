using DynamicData;
using ReactiveUI;
using System.Collections;
using System.Diagnostics;
using System.Reactive;
using System.Threading.Tasks;
using ToucanUI.Models;
using ToucanUI.Services;
using ToucanUI.Views;

namespace ToucanUI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        // VIEWMODELS
        public HeaderViewModel HeaderVM { get; }
        public ControlPanelViewModel ControlPanelVM { get; }
        public ModlistViewModel ModlistVM { get; }
        public SidePanelViewModel SidePanelVM { get; }
        public FooterViewModel FooterVM { get; }

        // VARIABLES
        private Mod _selectedMod;
        public Mod SelectedMod
        {
            get => _selectedMod;
            set
            {
                
                this.RaiseAndSetIfChanged(ref _selectedMod, value);
                SidePanelVM.SidePanelVisible = true;
               
            }
        }

        private bool _canDownloadMod;
        public bool CanDownloadMod
        {
            get => _canDownloadMod;
            set => this.RaiseAndSetIfChanged(ref _canDownloadMod, value);
        }


        // CONSTRUCTOR
        public MainWindowViewModel()
        {
            HeaderVM = new HeaderViewModel(this);
            ControlPanelVM = new ControlPanelViewModel(this);
            ModlistVM = new ModlistViewModel(this);
            SidePanelVM = new SidePanelViewModel(this);
            FooterVM = new FooterViewModel();
            LoadGameInstallPath();
        }
        
        // METHODS
        // Load the game install path from config
        private void LoadGameInstallPath()
        {
            var configManager = new ConfigurationManager();
            string gamePath = configManager.GetGamePath();
            if (!string.IsNullOrEmpty(gamePath))
            {
                Debug.WriteLine($"Config GamePath found: {gamePath}");
                CanDownloadMod = true;
            }

            else
            {
                // Display a warning asking user to scan for game install path
                ShowNoGameFoundMessageBox();
            }
        }

        // Run on startup to show if a game install path was not found
        private async void ShowNoGameFoundMessageBox()
        {
            // Set a time delay
            await Task.Delay(1000);
            var messageBoxCustomWindow = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxCustomWindow(
                new MessageBoxCustomParams
                {
                    ContentHeader = $"A game install path was not found!",
                    ContentMessage = "Do you want to scan for the game install path automatically or search manually?\r\n" +
                                     "(You will be unable to download/install mods if this is not set)",
                    Icon = MessageBox.Avalonia.Enums.Icon.Info,
                    SizeToContent = SizeToContent.WidthAndHeight,
                    Topmost = true,
                    ButtonDefinitions = new[]
                    {
                        new ButtonDefinition { Name = "Scan Automatically", IsDefault = true },
                        new ButtonDefinition { Name = "Search Manually", IsCancel = true }
                    },
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                });

            var result = await messageBoxCustomWindow.Show();

            if (result == "Scan Automatically")
            {
                HeaderVM.ScanKSP2InstallLocations();
            }
            else if (result == "Search Manually")
            {
                HeaderVM.SetGameInstallPath();
            }
        }


    }
}