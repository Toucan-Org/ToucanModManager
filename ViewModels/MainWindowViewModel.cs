using Avalonia.Controls;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using ToucanUI.Models;
using ToucanUI.Services;

namespace ToucanUI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        // =====================
        // VIEW MODELS
        // =====================
        public HeaderViewModel HeaderVM { get; }
        public ControlPanelViewModel ControlPanelVM { get; }
        public ModlistViewModel ModlistVM { get; }
        public SidePanelViewModel SidePanelVM { get; }
        public FooterViewModel FooterVM { get; }



        // =====================
        // VARIABLES
        // =====================

        // Keeps track of currently selected mod
        private ModViewModel _selectedMod;
        public ModViewModel SelectedMod
        {
            get => _selectedMod;
            set
            {
                // Deselect the previously selected mod in the side panel
                if (_selectedMod != null && _selectedMod != value)
                {
                    _selectedMod.IsSelectedSidePanel = false;
                }

                this.RaiseAndSetIfChanged(ref _selectedMod, value);
                SidePanelVM.SidePanelVisible = true; // Show the side panel

                if (SelectedMod != null)
                {
                    SelectedMod.IsSelectedSidePanel = true;
                    Task.Run(() => SelectedMod.ModObject.InitializeDownloadSizesAsync()); // If download sizes for versions haven't been fetched, retrieve them
                }
            }
        }

        // Tracks whether the client is in Offline Mode or not
        private bool _isOfflineMode;
        public bool IsOfflineMode
        {
            get => _isOfflineMode;
            set => this.RaiseAndSetIfChanged(ref _isOfflineMode, value);
        }

        // Tracks if BepInEx mod has been installed or not
        private bool _isBepinexInstalled;
        public bool IsBepinexInstalled
        {
            get => _isBepinexInstalled;
            set => this.RaiseAndSetIfChanged(ref _isBepinexInstalled, value);
        }

        // Track selected mods for bulk actions (i.e Install/Update/Delete selected)
        public ObservableCollection<Mod> SelectedBulkMods { get; } = new ObservableCollection<Mod>();

        // Keeps track of whether a user is allowed to download mods or not (if a valid game exe isn't found in the config)
        private bool _validGameFound;
        public bool ValidGameFound
        {
            get => _validGameFound;
            set => this.RaiseAndSetIfChanged(ref _validGameFound, value);
        }


        // =====================
        // Commands
        // =====================



        // =====================
        // CONSTRUCTOR
        // =====================
        public MainWindowViewModel()
        {
            FooterVM = new FooterViewModel(this);
            HeaderVM = new HeaderViewModel(this);
            ControlPanelVM = new ControlPanelViewModel(this);
            ModlistVM = new ModlistViewModel(this);
            SidePanelVM = new SidePanelViewModel(this);


            CheckGameInstallPath();
        }



        // =====================
        // METHODS
        // =====================


        // Check the game install path from config
        private void CheckGameInstallPath()
        {
            var configManager = new ConfigurationManager();
            string gamePath = configManager.GetGamePath();
            if (!string.IsNullOrEmpty(gamePath))
            {
                Debug.WriteLine($"Config GamePath found: {gamePath}");
                ValidGameFound = true;
            }

            else
            {
                // Display a warning asking user to scan for game install path
                ShowNoGameFoundMessageBox();
                ValidGameFound = false;
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