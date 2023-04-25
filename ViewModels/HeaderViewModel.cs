using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Themes.Fluent;
using MessageBox.Avalonia;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Enums;
using MessageBox.Avalonia.Models;
using ReactiveUI;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using ToucanUI.Services;

namespace ToucanUI.ViewModels
{
    public class HeaderViewModel : ViewModelBase
    {
        // VIEWMODELS
        public MainWindowViewModel MainViewModel { get; }

        // SERVICES
        private readonly KSP2Service _ksp2Service;
        private readonly ConfigurationManager _configManager;

        // FILE - Commands
        public ReactiveCommand<Unit, Unit> ExitCommand { get; }

        // EDIT - Commands
        public ReactiveCommand<Unit, Unit> ScanKSP2InstallLocationsCommand { get; }
        public ReactiveCommand<Unit, Unit> SetGameInstallPathCommand { get; }
        public ReactiveCommand<Unit, Unit> ClearConfigFileCommand { get; }
        public ReactiveCommand<Unit, Unit> ViewConfigFileCommand { get; }


        // VIEW - Commands

        // HELP - Commands

        // CONSTRUCTOR
        public HeaderViewModel(MainWindowViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;

            //Services
            _ksp2Service = new KSP2Service();
            _configManager = new ConfigurationManager();

            //Commands
            ScanKSP2InstallLocationsCommand = ReactiveCommand.Create(ScanKSP2InstallLocations);
            SetGameInstallPathCommand = ReactiveCommand.Create(SetGameInstallPath);
            ClearConfigFileCommand = ReactiveCommand.Create(ClearConfigFile);
            ViewConfigFileCommand = ReactiveCommand.Create(ViewConfigFile);

            ExitCommand = ReactiveCommand.Create(ExitApplication);
        }

        // FILE - Methods
        private async void ExitApplication()
        {
            var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(
                new MessageBoxStandardParams
                {
                    ContentHeader = "Exit Application",
                    ContentMessage = "Are you sure you want to quit?",
                    Icon = MessageBox.Avalonia.Enums.Icon.Warning,
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.YesNo,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                });

            var result = await messageBoxStandardWindow.Show();

            if (result == ButtonResult.Yes)
            {
                if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
                {
                    desktopLifetime.MainWindow.Close();
                }
            }
        }

        // EDIT - Methods
        public async void ScanKSP2InstallLocations()
        {
            (string path, string version) = _ksp2Service.DetectGameVersion();
            Debug.WriteLine($"Version is {version} at {path}");

            if (!string.IsNullOrEmpty(version))
            {
                await ShowSuccessMessageBox(version, path);
            }
            else
            {
                await ShowErrorMessageBox();
            }

        }

        public async void SetGameInstallPath()
        {
            string folderPath = await OpenFolderAsync();
            if (!string.IsNullOrEmpty(folderPath))
            {
                // Search for the exe in that folder path
                (string path, string version) = _ksp2Service.DetectGameVersion(folderPath);
                Debug.WriteLine($"Version is {version} at {path}");

                if (!string.IsNullOrEmpty(version))
                {
                    await ShowSuccessMessageBox(version, path);
                }
                else
                {
                    await ShowErrorMessageBox();
                }
            }
        }

        public async Task<string> OpenFolderAsync()
        {
            var openFolderDialog = new OpenFolderDialog
            {
                Title = "Select the 'Kerbal Space Program 2' folder",
            };

            string resultPath = string.Empty;

            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
            {
                resultPath = await openFolderDialog.ShowAsync(desktopLifetime.MainWindow);
            }

            return resultPath;
        }

        private async void ClearConfigFile()
        {
            // A dialog to ask if the user is sure they want to delete
            Debug.WriteLine("Clearing config file...");

            await ShowConfirmClearConfigMessageBox();            

        }

        private async void ViewConfigFile()
        {
            Debug.WriteLine("Viewing config file...");
            await ShowViewConfigFileMessageBox();
        }

        // VIEW - Methods


        // HELP - Methods


        // DIALOG BOXES
        private async Task ShowConfirmClearConfigMessageBox()
        {
            var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(
                new MessageBoxStandardParams
                {
                    ContentHeader = "Clear/Reset Configuration File",
                    ContentMessage = "Are you sure you want to reset your config file?",
                    Icon = MessageBox.Avalonia.Enums.Icon.Warning,
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.YesNo,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                });

            var result = await messageBoxStandardWindow.Show();

            if (result == ButtonResult.Yes)
            {
                _configManager.ClearConfig();

                // Set the CanDownloadMod to false
                MainViewModel.CanDownloadMod = false;
            }
        }

        private async Task ShowViewConfigFileMessageBox()
        {
            var configManager = new ConfigurationManager();
            string contentMessage;
            string gamePath = configManager.GetGamePath();
            string gameVersion = configManager.GetGameVersion();

            if (gamePath == "" || gameVersion == "")
            {
                contentMessage = "No configuration file found.";
            }

            else
            {
                contentMessage = $"**Game Path:** {gamePath}\r\n\r\n" + $"**Game Version:** {gameVersion}";
            }

            var messageBoxMarkdownWindow = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxCustomWindow(
                new MessageBoxCustomParams
                {
                    ContentHeader = "Configuration File",
                    ContentMessage = contentMessage,
                    Icon = MessageBox.Avalonia.Enums.Icon.Info,
                    ButtonDefinitions = new[]
                    {
                new ButtonDefinition { Name = "Close", IsDefault = true }
                    },
                    Markdown = true,
                    ShowInCenter = true,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                });

            await messageBoxMarkdownWindow.Show();
        }

        private async Task ShowSuccessMessageBox(string version, string path)
        {
            var messageBoxMarkdownWindow = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxCustomWindow(
                new MessageBoxCustomParams
                {
                    ContentHeader = "A valid game version was detected:",
                    ContentMessage = $"✅ **Version:** {version}\r\n\r\n" +
                                     $"✅ **Path:** {path}",
                    Icon = MessageBox.Avalonia.Enums.Icon.Success,
                    ButtonDefinitions = new[]
                    {
                        new ButtonDefinition { Name = "Confirm", IsDefault = true },
                        new ButtonDefinition { Name = "Cancel", IsCancel = true }
                    },
                    Markdown = true,
                    ShowInCenter = true,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                });

            var result = await messageBoxMarkdownWindow.Show();

            if (result == "Confirm")
            {
                // Set the Game Install Path in config
                Debug.WriteLine("Saving config");
                _configManager.SaveConfig(path, version);

                // Update the UI
                MainViewModel.CanDownloadMod = true;

            }
        }

        private async Task ShowErrorMessageBox()
        {
            var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(
                new MessageBoxStandardParams
                {
                    ContentHeader = "A valid game version was not detected!",
                    ContentMessage = "Please ensure you have selected the\r\nroot 'Kerbal Space Program 2' folder.\n\n",
                    Icon = MessageBox.Avalonia.Enums.Icon.Error,
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                });

            var result = await messageBoxStandardWindow.Show();
        }
    }
}
