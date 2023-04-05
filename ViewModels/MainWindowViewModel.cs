namespace ToucanUI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {

        public SidePanelViewModel SidePanelViewModel { get; } = new SidePanelViewModel();
        public string Greeting => "Welcome to Avalonia!";

    }
}