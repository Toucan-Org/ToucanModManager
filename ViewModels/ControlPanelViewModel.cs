using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToucanUI.ViewModels
{
    public class ControlPanelViewModel : ViewModelBase
    {
        public MainWindowViewModel MainViewModel { get; }

        public ControlPanelViewModel(MainWindowViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;
        }
    }
}
