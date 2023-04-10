using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToucanUI.ViewModels
{
    public class HeaderViewModel : ViewModelBase
    {
        public MainWindowViewModel MainViewModel { get; }

        public HeaderViewModel(MainWindowViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;
        }
    }
}
