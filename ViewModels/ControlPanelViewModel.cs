using ReactiveUI;
using System.Collections.ObjectModel;


namespace ToucanUI.ViewModels
{
    public class ControlPanelViewModel : ViewModelBase
    {
        // =====================
        // VIEW MODELS
        // =====================    
        public MainWindowViewModel MainViewModel { get; }



        // =====================
        // VARIABLES
        // =====================

        // List to be used when filtering mods
        public ReadOnlyObservableCollection<Mod> filteredList;


        private bool _filterInstalled = false;
        public bool FilterInstalled
        {
            get => _filterInstalled;
            set
            {
                this.RaiseAndSetIfChanged(ref _filterInstalled, value);
                if (value)
                {
                    FilterNotInstalled = false;
                }
            }
        }

        private bool _filterNotInstalled = false;
        public bool FilterNotInstalled
        {
            get => _filterNotInstalled;
            set
            {
                this.RaiseAndSetIfChanged(ref _filterNotInstalled, value);
                if (value)
                {
                    FilterInstalled = false;
                }
            }
        }


        // Is compatible version filter
        private bool _filterVersion = false;
        public bool FilterVersion
        {
            get => _filterVersion;
            set
            {
                this.RaiseAndSetIfChanged(ref _filterVersion, value);
            }

        }

        // Is update available version filter
        private bool _filterUpdateAvailable = false;
        public bool FilterUpdateAvailable
        {
            get => _filterUpdateAvailable;
            set
            {
                this.RaiseAndSetIfChanged(ref _filterUpdateAvailable, value);
            }

        }

        // =====================
        // CONSTRUCTOR
        // =====================
        public ControlPanelViewModel(MainWindowViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;

        }

    }
}
