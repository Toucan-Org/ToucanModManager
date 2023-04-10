using ReactiveUI;
using ToucanUI.Models;

namespace ToucanUI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public HeaderViewModel HeaderVM { get; }
        public ControlPanelViewModel ControlPanelVM { get; }
        public ModlistViewModel ModlistVM { get; }
        public SidePanelViewModel SidePanelVM { get; }

        private Mod _selectedMod;
        public Mod SelectedMod
        {
            get => _selectedMod;
            set
            {
                if (value == _selectedMod)
                {
                    SidePanelVM.SidePanelVisible = false;
                }
                this.RaiseAndSetIfChanged(ref _selectedMod, value);
                SidePanelVM.SidePanelVisible = true;
               
            }
        }

        // In constructor create and refernce both child View Models
        public MainWindowViewModel()
        {
            HeaderVM = new HeaderViewModel(this);
            ControlPanelVM = new ControlPanelViewModel(this);
            ModlistVM = new ModlistViewModel(this);
            SidePanelVM = new SidePanelViewModel(this);
        }

    }
}