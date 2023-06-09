using ReactiveUI;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Input;


namespace ToucanUI.ViewModels
{
    public class FooterViewModel : ViewModelBase
    {

        // =====================
        // VIEW MODELS
        // =====================
        public MainWindowViewModel MainViewModel { get; }


        // =====================
        // VARIABLES
        // =====================

        // These variables are used to display information about Toucan in the footer
        public string ToucanVersion { get; }
        public string ToucanName { get; }
        public string[] ToucanAuthors { get; }
        public string ToucanWebsite { get; }
        public string ToucanLicense { get; }

        private string _hours;
        public string Hours
        {
            get => _hours;
            set => this.RaiseAndSetIfChanged(ref _hours, value);
        }

        private string _minutes;
        public string Minutes
        {
            get => _minutes;
            set => this.RaiseAndSetIfChanged(ref _minutes, value);
        }

        private string _seconds;
        public string Seconds
        {
            get => _seconds;
            set => this.RaiseAndSetIfChanged(ref _seconds, value);
        }


        // =====================
        // COMMANDS
        // =====================
        public ICommand OpenGitHubCommand { get; }


        // =====================
        // CONSTRUCTOR
        // =====================
        public FooterViewModel(MainWindowViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;

            var manifestPath = "Assets/toucan_manifest.json";
            var manifestContent = File.ReadAllText(manifestPath);
            var manifestJson = JsonDocument.Parse(manifestContent);

            ToucanVersion = manifestJson.RootElement.GetProperty("version").GetString();
            ToucanName = manifestJson.RootElement.GetProperty("name").GetString();
            ToucanAuthors = manifestJson.RootElement.GetProperty("authors").EnumerateArray()
                .Select(author => author.GetString())
                .ToArray();
            ToucanWebsite = manifestJson.RootElement.GetProperty("website").GetString();
            ToucanLicense = manifestJson.RootElement.GetProperty("license").GetString();


            OpenGitHubCommand = ReactiveCommand.Create(() =>
            {
                // Navigate to Toucan GitHub page
                Process.Start(new ProcessStartInfo(ToucanWebsite) { UseShellExecute = true });
            });

        }

    }
}
