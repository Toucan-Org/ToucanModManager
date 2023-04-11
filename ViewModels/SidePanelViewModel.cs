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
        //Create a MainViewModel as a parent
        public MainWindowViewModel MainViewModel { get; }

        public ICommand OpenGitHubCommand { get; }
        public ICommand CloseSidePanelCommand { get; }



        public SidePanelViewModel(MainWindowViewModel mainViewModel) 
        {
            //Has to be passed into the child constructors
            MainViewModel = mainViewModel;

            OpenGitHubCommand = ReactiveCommand.Create(() =>
            {
                // Navigate to your GitHub page
                Process.Start(new ProcessStartInfo("https://github.com/KSP2-Toucan/ToucanModManager") { UseShellExecute = true });
            });

            CloseSidePanelCommand = ReactiveCommand.Create(() =>
            {
                SidePanelVisible = false;
            });

        }

        private bool _sidePanelVisible = false;
        public bool SidePanelVisible
        {
            get => _sidePanelVisible;
            set => this.RaiseAndSetIfChanged(ref _sidePanelVisible, value);
            
        }




        
       
    }
}
