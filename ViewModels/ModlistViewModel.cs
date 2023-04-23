using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ToucanUI.Models;
using ToucanUI.Services;

namespace ToucanUI.ViewModels
{
    public class ModlistViewModel : ViewModelBase
    {
        SpacedockAPI api = new SpacedockAPI();

        private ReadOnlyObservableCollection<Mod> _mods;
        public ReadOnlyObservableCollection<Mod> Mods { get => _mods; set => this.RaiseAndSetIfChanged(ref _mods, value); }
        

        public ObservableCollection<Mod> ModList { get; set; }

        public ReactiveCommand<Mod, Unit> DownloadMod { get; }
        public ReactiveCommand<Unit, Unit> ToggleViewCommand { get; }

        //private SourceList<Mod> _sourceList { get; set; }

        private Mod _selectedMod;
        public Mod SelectedMod
        {
            get => _selectedMod;
            set => this.RaiseAndSetIfChanged(ref _selectedMod, value);
            
        }

        private bool _isClassicViewVisible = true;
        public bool IsClassicViewVisible
        {
            get => _isClassicViewVisible;
            set => this.RaiseAndSetIfChanged(ref _isClassicViewVisible, value);
        }

        private bool _isGridViewVisible = false;
        public bool IsGridViewVisible
        {
            get => _isGridViewVisible;
            set => this.RaiseAndSetIfChanged(ref _isGridViewVisible, value);
        }

        //Parent ViewModel for communication
        public MainWindowViewModel MainViewModel { get; }

        // MODLIST VIEWMODEL CONSTRUCTOR
        public ModlistViewModel(MainWindowViewModel mainViewModel)
        {
            //Added this so MainViewModel acts as the communicator between sidepanel and modlist
            MainViewModel = mainViewModel;

            DownloadMod = ReactiveCommand.Create<Mod>(mod => DownloadModAsync(mod));
            ToggleViewCommand = ReactiveCommand.Create(SwitchView);

            LoadMods(true);

            //This is the filter that is applied to the sourcelist
            var observableSearchFilter = this.WhenAnyValue(viewModel => viewModel.SearchText).Select(SearchName);
            var InstalledFilter = this.WhenAnyValue(x => x.MainViewModel.ControlPanelVM.FilterInstalled).Select(SetInstalledFilter);
            var VersionFilter = this.WhenAnyValue(x => x.MainViewModel.ControlPanelVM.FilterVersion).Select(SetVersionFilter);
            //this converts the modlist (which contains the mods recieved from the API) into an Observable List
            var _sourceList = ModList
               .ToObservableChangeSet()
               .Sort(SortExpressionComparer<Mod>.Ascending(x => x.Name)) //Sorts list by name
               .Filter(observableSearchFilter) //Applies the filter
               .Filter(InstalledFilter)
               .Filter(VersionFilter)
               .AsObservableList();

            _sourceList.Connect()
               .Bind(out _mods) //Binds the newly created sourcelist to the mod ObservableCollection (which is displayed in the axaml)
               .DisposeMany()
               .Subscribe();


        }

        private Func<Mod, bool> SearchName(string name)
        {
            if (string.IsNullOrEmpty(name)) //If searchbar is empty, show all the mods
            {
                return Mod => true;
            }
            else
            {
                return Mod => Mod.Name.Contains(name, StringComparison.OrdinalIgnoreCase);
            }
        }

        private Func<Mod, bool> SetInstalledFilter(bool isOn)
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

        private Func<Mod, bool> SetVersionFilter(bool isOn)
        {
            if (isOn)
            {
                return Mod => Mod.GameVersion.Equals("0.1.0.0");
            }
            else
            {
                return Mod => true;
            }
        }

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


        public void SwitchView()
        {
            IsClassicViewVisible = !IsClassicViewVisible;
            IsGridViewVisible = !IsGridViewVisible;

        }

        // Load the mod list from the API
        private async Task LoadMods(bool useDummyData = false)
        {
            var mods = await api.GetMods(useDummyData);
            ModList = new ObservableCollection<Mod>(mods);
            foreach (var mod in mods)
            {
                Debug.WriteLine("Name: " + mod.Name);
                Debug.WriteLine("Id: " + mod.Id);
                Debug.WriteLine("Game: " + mod.Game);
            }
        }


        // Function to download a mod asynchronously
        public async Task DownloadModAsync(Mod mod)
        {
            mod.IsInstalled = false;
            mod.Progress = 0;

            while (mod.Progress <= 100)
            {
                // Dummy code to simulate downloading
                await Task.Delay(100);
                mod.Progress += 2;
                Debug.WriteLine($"Mod {mod.Name} at {mod.Progress}%");
            }

            mod.IsInstalled = true;
        }

        public bool IsSelected(Mod mod)
        {
            return mod == SelectedMod;
        }



    }
}