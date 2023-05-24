using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Reactive;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using ToucanUI.Models;
using ToucanUI.Services;
using ToucanUI.Views;

namespace ToucanUI.ViewModels
{
    public class SidePanelViewModel : ViewModelBase
    {
        // VIEWMODELS
        public MainWindowViewModel MainViewModel { get; }

        // COMMANDS
        public ICommand OpenWebsiteCommand { get; }
        public ICommand CloseSidePanelCommand { get; }

        // VARIABLES
        private bool _sidePanelVisible = false;
        public bool SidePanelVisible
        {
            get => _sidePanelVisible;
            set => this.RaiseAndSetIfChanged(ref _sidePanelVisible, value);
        }


        // CONSTRUCTOR
        public SidePanelViewModel(MainWindowViewModel mainViewModel) 
        {
            MainViewModel = mainViewModel;

            OpenWebsiteCommand = ReactiveCommand.Create(() =>
            {
                try
                {
                    // Navigate to mod website
                    Process.Start(new ProcessStartInfo(mainViewModel.SelectedMod.Website) { UseShellExecute = true });
                }
                catch(System.InvalidOperationException)
                {
                    Debug.WriteLine("No Website was found in SelectedMod!");
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
