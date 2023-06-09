using Avalonia;
using Avalonia.Threading;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using ToucanUI.Services;

namespace ToucanUI.ViewModels
{
    public class ModlistViewModel : ViewModelBase
    {
        // =====================
        // VIEW MODELS
        // =====================
        public MainWindowViewModel MainViewModel { get; }



        // =====================
        // SERVICES
        // =====================
        SpacedockAPI api = new SpacedockAPI();
        private readonly ConfigurationManager _configManager;



        // =====================
        // VARIABLES
        // =====================

        // The current category (All, Top, New, Featured) (currently hardcoded to All due to API issues)
        private SpacedockAPI.Category _category;

        public SpacedockAPI.Category Category
        {
            get => _category;
            set
            {
                _category = value;
                this.RaiseAndSetIfChanged(ref _category, value);

                // Clear the side panel when the category changes
                MainViewModel.SelectedMod = null;
                MainViewModel.SidePanelVM.SidePanelVisible = false;

                Debug.WriteLine(Category.ToString());
                // Fetch mods with the new category
                FetchMods(Category);
            }
        }

        // List for filtering mods
        private ReadOnlyObservableCollection<ModViewModel> _mods;
        public ReadOnlyObservableCollection<ModViewModel> Mods { get => _mods; set => this.RaiseAndSetIfChanged(ref _mods, value); }

        // The main mod list
        public ObservableCollection<ModViewModel> ModList { get; set; }

        // List of mods selected via checkbox for bulk actions
        public SourceList<ModViewModel> SelectedBulkMods { get; set; }
        public ReadOnlyObservableCollection<ModViewModel> SelectedBulkModsReadOnly { get; private set; }


        // Tracks if Classic view should be visible
        private bool _isClassicViewVisible = true;
        public bool IsClassicViewVisible
        {
            get => _isClassicViewVisible;
            set => this.RaiseAndSetIfChanged(ref _isClassicViewVisible, value);
        }

        // Tracks if Grid view should be visible
        private bool _isGridViewVisible = false;
        public bool IsGridViewVisible
        {
            get => _isGridViewVisible;
            set => this.RaiseAndSetIfChanged(ref _isGridViewVisible, value);
        }

        // Tracks if data is being fetched from the API
        private bool _isFetchingData;
        public bool IsFetchingData
        {
            get => _isFetchingData;
            set => this.RaiseAndSetIfChanged(ref _isFetchingData, value);
        }

        // Sets the fetching message
        private string _fetchingMessage;
        public string FetchingMessage
        {
            get => _fetchingMessage;
            set => this.RaiseAndSetIfChanged(ref _fetchingMessage, value);
        }

        // Tracks if data fetch failed (in order to display error messages)
        private bool _isFetchFailed;
        public bool IsFetchFailed
        {
            get => _isFetchFailed;
            set => this.RaiseAndSetIfChanged(ref _isFetchFailed, value);
        }


        // Static fetch phrases list
        private static readonly List<string> _fetchPhrases = new List<string>()
        {
            "Intercepting Mod Telemetry...",
            "Fetching mod data from Spacedock...",
            "Calculating Δv...",
            "Initializing Warp Drive...",
            "Executing Gravity Turn...",
            "Scanning the Modiverse...",
            "Deploying orbital repairs...",
            "Colonizing Duna...",
            "Avoiding catastrophic meltdown...",
            "Slaying the Kraken, please wait...",
            "Preparing for launch...",
            "Simulating aerodynamic forces...",
            "Increasing thrust...",
            "Deploying backup parachutes...",
            "Breaking the sound barrier...",
            "Calculating Hohmann transfer...",
        };

        // Tracks if the user is searching
        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                this.RaiseAndSetIfChanged(ref _searchText, value);
                //Clear the side panel if the user types - fixes visual bug
                MainViewModel.SelectedMod = null;
                MainViewModel.SidePanelVM.SidePanelVisible = false;

            }
        }

        // Flag for skipping bulk actions
        private bool _skipCurrentMod = false;

        // Progress message for the bulk action panel
        private string _bulkActionMessage;
        public string BulkActionMessage
        {
            get => _bulkActionMessage;
            set => this.RaiseAndSetIfChanged(ref _bulkActionMessage, value);
        }

        // Tracks if a bulk action is in progress
        private bool _isBulkActionInProgress;
        public bool IsBulkActionInProgress
        {
            get => _isBulkActionInProgress;
            set => this.RaiseAndSetIfChanged(ref _isBulkActionInProgress, value);
        }


        private int _bulkActionProgressCount;
        public int BulkActionProgressCount
        {
            get => _bulkActionProgressCount;
            set => this.RaiseAndSetIfChanged(ref _bulkActionProgressCount, value);
        }

        public CancellationTokenSource BulkActionCancellationTokenSource { get; set; }

        private ModViewModel _currentBulkActionMod;
        public ModViewModel CurrentBulkActionMod
        {
            get => _currentBulkActionMod;
            set => this.RaiseAndSetIfChanged(ref _currentBulkActionMod, value);
        }




        // =====================
        // COMMANDS
        // =====================
        public ReactiveCommand<ModViewModel, Unit> DownloadModCommand { get; }
        public ReactiveCommand<ModViewModel, Unit> CancelDownloadModCommand { get; }
        public ReactiveCommand<Unit, Unit> ToggleViewCommand { get; }
        public ReactiveCommand<Unit, Unit> SelectAllModsCommand { get; }
        public ReactiveCommand<Unit, Unit> UnselectAllModsCommand { get; }
        public ReactiveCommand<Unit, Unit> InstallSelectedModsCommand { get; }

        public ReactiveCommand<Unit, Unit> SkipBulkActionCommand { get; }
        public ReactiveCommand<Unit, Unit> CancelBulkActionCommand { get; }




        // =====================
        // CONSTRUCTOR
        // =====================
        public ModlistViewModel(MainWindowViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;

            DownloadModCommand = ReactiveCommand.Create<ModViewModel>(mod => DownloadModAsync(mod), mainViewModel.WhenAnyValue(x => x.ValidGameFound));
            CancelDownloadModCommand = ReactiveCommand.Create<ModViewModel>(CancelDownload);
            ToggleViewCommand = ReactiveCommand.Create(SwitchView);
            SelectAllModsCommand = ReactiveCommand.Create(SelectAllMods);
            UnselectAllModsCommand = ReactiveCommand.Create(UnselectAllMods);

            InstallSelectedModsCommand = ReactiveCommand.Create(InstallSelectedMods);
            SkipBulkActionCommand = ReactiveCommand.Create(SkipBulkAction);
            CancelBulkActionCommand = ReactiveCommand.Create(() => CancelBulkAction(BulkActionCancellationTokenSource));


            ModList = new ObservableCollection<ModViewModel>();

            SelectedBulkMods = new SourceList<ModViewModel>();

            // Create a derived ReadOnlyObservableCollection
            SelectedBulkMods.Connect()
                .Bind(out var readOnlyCollection)
                .Subscribe();

            SelectedBulkModsReadOnly = readOnlyCollection;


            // Fetch list of mods from spacedock api
            FetchMods(SpacedockAPI.Category.All);

            var observableSearchFilter = this.WhenAnyValue(viewModel => viewModel.SearchText).Select(SearchName);
            var InstalledFilter = this.WhenAnyValue(x => x.MainViewModel.ControlPanelVM.FilterInstalled).Select(SetInstalledFilter);
            var VersionFilter = this.WhenAnyValue(x => x.MainViewModel.ControlPanelVM.FilterVersion).Select(SetVersionFilter);

            var modListChangeSet = ModList.ToObservableChangeSet();

            var _sourceList = modListChangeSet
                .Sort(SortExpressionComparer<ModViewModel>.Ascending(x => x.ModObject.Name))
                .Filter(observableSearchFilter)
                .Filter(InstalledFilter)
                .Filter(VersionFilter)
                .AsObservableList();

            _sourceList.Connect()
                .Bind(out _mods)
                .DisposeMany()
                .Subscribe();

        }



        // =====================
        // METHODS (Filters)
        // =====================

        // Search bar filter
        private Func<ModViewModel, bool> SearchName(string name)
        {
            if (string.IsNullOrEmpty(name)) //If searchbar is empty, show all the mods
            {
                return Mod => true;
            }
            else
            {
                return Mod => Mod.ModObject.Name.Contains(name, StringComparison.OrdinalIgnoreCase);
            }
        }

        // Set the IsInstalled filter on or off
        private Func<ModViewModel, bool> SetInstalledFilter(bool isOn)
        {
            if (isOn)
            {
                return Mod => Mod.IsInstalled;
            }
            else
            {
                return Mod => true;
            }
        }

        // Set the compatible version filter on/off
        private Func<ModViewModel, bool> SetVersionFilter(bool isOn)
        {
            var configManager = new ConfigurationManager();
            if (isOn)
            {
                return Mod => Mod.ModObject.LatestVersion.GameVersion.Equals(configManager.GetGameVersion());
            }
            else
            {
                return Mod => true;
            }
        }

        // Switch between Classic modlist view or DataGrid view
        public void SwitchView()
        {
            IsClassicViewVisible = !IsClassicViewVisible;
            IsGridViewVisible = !IsGridViewVisible;

        }


        // =====================
        // METHODS (Download/Install)
        // =====================

        // Cancel a download/install
        public void CancelDownload(ModViewModel mod)
        {
            if (mod != null)
            {
                mod.IsModifiable = true;
                Debug.WriteLine($"Cancelled download {mod.ModObject.Name}");
                mod.IsCanceled = true;
            }
        }

        // Load the mod list from the API
        public async Task FetchMods(SpacedockAPI.Category category)
        {
            // Clear the mod list first
            ModList.Clear();
            SelectedBulkMods.Clear();

            // Set the fetching message
            FetchingMessage = _fetchPhrases[new Random().Next(0, _fetchPhrases.Count)];
            IsFetchingData = true;
            IsFetchFailed = false;


            // Get the modlist from the API
            var mods = await api.GetMods(category).ConfigureAwait(false);

            // Update ModList on the UI thread
            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                ModList.Clear();
                foreach (var mod in mods)
                {
                    var modViewModel = new ModViewModel(mod);
                    ModList.Add(modViewModel);
                    Debug.WriteLine($"Added {mod.Name} [{mod.Id}]");

                    // Subscribe to IsSelectedBulk changes
                    modViewModel
                        .WhenAnyValue(x => x.IsSelectedBulk)
                        .Subscribe(isSelected =>
                        {
                            if (isSelected)
                            {
                                SelectedBulkMods.Add(modViewModel);
                            }
                            else
                            {
                                SelectedBulkMods.Remove(modViewModel);
                            }

                            if (SelectedBulkMods.Count > 0)
                            {
                                Debug.WriteLine($"Checked mods: {string.Join(", ", SelectedBulkMods.Items.Select(m => m.ModObject.Name))}");
                            }


                        });
                }
            });

            // If nothing was returned from the API, set the fetching message to an error
            if (ModList.Count < 1)
            {
                
                // Check if Spacedock.info is online
                bool isSpacedockOnline = await api.PingSpacedock();

                if (!isSpacedockOnline)
                {
                    FetchingMessage = "Unable to reach spacedock.info";
                }
                else
                {
                    FetchingMessage = "Something went wrong! Unable to fetch data.";
                }

                IsFetchFailed = true;
                IsFetchingData = false;
                return;
            }

            // If the API returned data, set the fetching message to null
            else
            {
                FetchingMessage = "";
                IsFetchingData = false;
            }


        }

        // Function to download a mod asynchronously
        public async Task DownloadModAsync(ModViewModel mod, CancellationToken cancellationToken = default)
        {
            mod.Progress = 0;
            mod.IsModifiable = false;
            mod.IsCanceled = false;
            mod.IsInstalled = false;

            try
            {
                while (mod.Progress <= 100)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    if (mod.IsCanceled)
                    {
                        mod.Progress = 0;
                        return;
                    }

                    // Dummy code to simulate downloading
                    await Task.Delay(100);
                    mod.Progress += 2;
                    Debug.WriteLine($"Mod {mod.ModObject.Name} at {mod.Progress}%");
                }

                mod.IsInstalled = true;
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine($"Mod {mod.ModObject.Name} download canceled");
                mod.IsCanceled = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Mod {mod.ModObject.Name} download failed: {ex.Message}");
                // Handle other exceptions, e.g., network errors, file I/O errors, etc.
            }
            finally
            {
                if (mod.IsCanceled || !mod.IsInstalled)
                {
                    mod.Progress = 0;
                    mod.IsModifiable = true;
                }
                
            }
        }




        // =====================
        // METHODS (Commands)
        // =====================

        public async void InstallSelectedMods()
        {
            IsBulkActionInProgress = true;

            // Create a new CancellationTokenSource locally within the method
            BulkActionCancellationTokenSource = new CancellationTokenSource();

            // Convert SelectedBulkMods.Items to a List<ModViewModel> to access elements by index
            List<ModViewModel> modList = SelectedBulkMods.Items.ToList();

            // Iterate through the modList
            for (int i = 0; i < modList.Count; i++)
            {
                ModViewModel mod = modList[i];
                CurrentBulkActionMod = mod;

                // Update the progress message
                BulkActionMessage = $"Installing mod {i + 1} of {modList.Count}";

                // Check if the mod is already installed
                if (mod.IsInstalled)
                {
                    // If it is, skip this mod and continue to the next one
                    continue;
                }

                // If the mod is not installed, call the DownloadModAsync function
                try
                {
                    await DownloadModAsync(mod, BulkActionCancellationTokenSource.Token);

                    // Check if the skip flag is set, and reset it to false for the next mod
                    if (_skipCurrentMod)
                    {
                        _skipCurrentMod = false;
                        continue;
                    }

                    BulkActionProgressCount++;
                }
                catch (OperationCanceledException)
                {
                    // Handle cancellation
                    break;
                }
            }

            // Reset the progress message and switch back to the first bulk action panel
            BulkActionMessage = "";
            IsBulkActionInProgress = false;
            BulkActionProgressCount = 0;
        }


        public void SkipBulkAction()
        {
            _skipCurrentMod = true;

            CurrentBulkActionMod.IsCanceled = true;
        
        }

        public void CancelBulkAction(CancellationTokenSource cancellationTokenSource)
        {
            cancellationTokenSource.Cancel();
            SelectedBulkMods.Clear();
            UnselectAllMods();
        }



        private void SelectAllMods()
        {
            foreach (var mod in Mods)
            {
                mod.IsSelectedBulk = true;
            }
        }

        private void UnselectAllMods()
        {
            foreach (var mod in Mods)
            {
                mod.IsSelectedBulk = false;
            }
        }


    }
}