using Avalonia.FreeDesktop.DBusIme;
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
    public class ControlPanelViewModel : ViewModelBase
    {
        // VIEWMODELS        
        public MainWindowViewModel MainViewModel { get; }

        // VARIABLES
        public ReadOnlyObservableCollection<Mod> filteredList;
        private bool _filterInstalled = false;

        public bool FilterInstalled
        {
            get => _filterInstalled;
            set
            {
                this.RaiseAndSetIfChanged(ref _filterInstalled, value);
            }

        }
        private bool _filterVersion = false;
        public bool FilterVersion
        {
            get => _filterVersion;
            set
            {
                this.RaiseAndSetIfChanged(ref _filterVersion, value);
            }

        }

        // CONSTRUCTOR
        public ControlPanelViewModel(MainWindowViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;

        }
        
    }
}
