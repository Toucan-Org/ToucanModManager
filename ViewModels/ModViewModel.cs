using ReactiveUI;
using ToucanUI.Models;

namespace ToucanUI.ViewModels
{
    public class ModViewModel : ViewModelBase
    {
        // =====================
        // VARIABLES
        // =====================

        public Mod ModObject { get; set; }


        // If mod is currently selected in the side panel
        private bool _isSelectedSidePanel;
        public bool IsSelectedSidePanel
        {
            get => _isSelectedSidePanel;
            set => this.RaiseAndSetIfChanged(ref _isSelectedSidePanel, value);
        }

        // If mod is selected for bulk actions
        private bool _isSelectedBulk;
        public bool IsSelectedBulk
        {
            get => _isSelectedBulk;
            set
            {
                this.RaiseAndSetIfChanged(ref _isSelectedBulk, value);
            }
        }

        // If mod is installed
        private bool _isInstalled;
        public bool IsInstalled
        {
            get => _isInstalled;
            set => this.RaiseAndSetIfChanged(ref _isInstalled, value);
        }


        // If mod version is modifiable (i.e not installed or installed but not up to date)
        private bool _isModifiable;
        public bool IsModifiable
        {
            get => _isModifiable;
            set => this.RaiseAndSetIfChanged(ref _isModifiable, value);
        }

        // If download is canceled
        public bool IsCanceled { get; set; }

        // Mod download/install progress
        private int _progress;
        public int Progress
        {
            get => _progress;
            set => this.RaiseAndSetIfChanged(ref _progress, value);
        }


        public ModViewModel(Mod mod)
        {
            ModObject = mod;

            IsSelectedSidePanel = false;
            IsSelectedBulk = false;
            IsInstalled = false;
            IsModifiable = true;
            Progress = 0;
        }
    }
}
