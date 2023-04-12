using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ToucanUI.Services;

namespace ToucanUI.ViewModels
{
    public class HeaderViewModel : ViewModelBase
    {
        public MainWindowViewModel MainViewModel { get; }
        private readonly KSP2Service _ksp2Service;

        public ReactiveCommand<Unit, Unit> ScanKSP2InstallLocationsCommand { get; }

        public HeaderViewModel(MainWindowViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;
            _ksp2Service = new KSP2Service();
            ScanKSP2InstallLocationsCommand = ReactiveCommand.Create(ScanKSP2InstallLocations);
        }

        private void ScanKSP2InstallLocations()
        {
            string version = _ksp2Service.DetectGameVersion();
            Debug.WriteLine($"Version is {version}");
            // Do something with the version
        }
    }
}
