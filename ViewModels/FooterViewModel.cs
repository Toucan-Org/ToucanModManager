using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform;
using ReactiveUI;
using System.Diagnostics;
using System.Windows.Input;


namespace ToucanUI.ViewModels
{
    public class FooterViewModel : ViewModelBase
    {
        // COMMANDS
        public ICommand OpenGitHubCommand { get; }

        public FooterViewModel()
        {

            OpenGitHubCommand = ReactiveCommand.Create(() =>
            {
                // Navigate to your GitHub page
                Process.Start(new ProcessStartInfo("https://github.com/KSP2-Toucan/ToucanModManager") { UseShellExecute = true });
            });


        }

    }
}
