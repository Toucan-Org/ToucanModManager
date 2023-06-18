using Avalonia.Controls;
using Avalonia.Threading;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ToucanUI.Models.KSP2;
using ToucanUI.Services;
using static ToucanUI.Services.InstallManager;

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
        // SERVICES
        // =====================
        InstallManager installer;
        ConfigurationManager configManager = new ConfigurationManager();

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

                    // If download sizes for versions haven't been fetched, retrieve them
                    if (!SelectedMod.VersionViewModels.All(vm => vm.IsDownloadSizeFetched))
                    {
                        Task.Run(() => SelectedMod.InitializeDownloadSizesAsync());
                    }
                }
            }
        }


        // Tracks if BepInEx mod has been installed or not
        private InstallManager.BepInExStatusEnum _bepInExState;
        public InstallManager.BepInExStatusEnum BepInExState
        {
            get => _bepInExState;
            set => this.RaiseAndSetIfChanged(ref _bepInExState, value);
        }

        // Track selected mods for bulk actions (i.e Install/Update/Delete selected)
        public ObservableCollection<Mod> SelectedBulkMods { get; } = new ObservableCollection<Mod>();

        // Keeps track of whether a user is allowed to download mods or not
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

            CheckValidGameFound();
        }



        // =====================
        // METHODS
        // =====================


        // Run at startup to check if KSP2 and BepInEx installed
        private async void CheckValidGameFound()
        {
            // If a valid game path is found
            if (CheckGameInstallPath())
            {
                installer = new InstallManager();

                // Check if BepInEx is installed or not
                BepInExState = await Dispatcher.UIThread.InvokeAsync(async () => await installer.CheckIfBepInEx());

                switch (BepInExState)
                {
                    // If it's already installed, set ValidGameFound
                    case BepInExStatusEnum.Installed:
                        Debug.WriteLine("BepInEx is installed");
                        ValidGameFound = true;
                        break;

                    // Else install it
                    case BepInExStatusEnum.NotInstalled:
                        Debug.WriteLine("BepInEx is not installed");
                        bool bepInExInstalled = await installer.BepInExStatusBox(ModlistVM);
                        if (bepInExInstalled)
                        {
                            BepInExState = BepInExStatusEnum.Installed;
                            ValidGameFound = true;
                        }
                        else
                        {
                            ValidGameFound = false;
                        }
                        break;

                    case BepInExStatusEnum.Error:
                        Debug.WriteLine("Error checking BepInEx");
                        ValidGameFound = false;
                        break;

                    default:
                        Debug.WriteLine("Unexpected BepInEx status");
                        ValidGameFound = false;
                        break;
                }
            }

            else
            {
               await ShowNoGameFoundMessageBox();
            }
        }


        // Check the game install path from config
        private bool CheckGameInstallPath()
        {
            string gamePath = configManager.GetGamePath();
            if (!string.IsNullOrEmpty(gamePath))
            {
                Debug.WriteLine($"Config GamePath found: {gamePath}");
                return true;
            }

            return false;
        }

        // Run on startup to show if a game install path was not found
        private async Task ShowNoGameFoundMessageBox()
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
                await HeaderVM.ScanKSP2InstallLocations();
            }
            else if (result == "Search Manually")
            {
                await HeaderVM.SetGameInstallPath();
            }

            CheckValidGameFound();
        }


    }
}