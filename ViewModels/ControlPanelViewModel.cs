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
        
        public MainWindowViewModel MainViewModel { get; }

        public ReadOnlyObservableCollection<Mod> filteredList;

        public ControlPanelViewModel(MainWindowViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;

        }

        private String _searchInput;
        public String SearchInput
        {
            get => _searchInput;
            set
            {
                this.RaiseAndSetIfChanged(ref _searchInput, value);
                SearchForMod(value);
            }
        }

        public void SearchForMod(String name)
        {
            //MainViewModel.ModlistVM.DisplayMods.Clear();
            for (int i = 0; i < MainViewModel.ModlistVM.Mods.Count; i++) {
                if (MainViewModel.ModlistVM.Mods[i].Name == name || MainViewModel.ModlistVM.Mods[i].Name.Contains(name))
                {
                    MainViewModel.SelectedMod = MainViewModel.ModlistVM.Mods[i];
                   
                }
              
            }

            if(name == "")
            {
                MainViewModel.SelectedMod = null;
                MainViewModel.SidePanelVM.SidePanelVisible = false;
            }
        }


        //https://docs.avaloniaui.net/guides/deep-dives/reactiveui/binding-to-sorted-filtered-list
        public void Filter()
        {
            MainViewModel.ModlistVM.FilterMods();

        }



    }
}
