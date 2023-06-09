using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Enums;
using MessageBox.Avalonia.Models;
using ReactiveUI;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Reactive;
using System.Text.Json;
using System.Threading.Tasks;
using ToucanUI.Services;

namespace ToucanUI.ViewModels
{
    public class HeaderViewModel : ViewModelBase
    {
        // =====================
        // VIEW MODELS
        // =====================
        public MainWindowViewModel MainViewModel { get; }



        // =====================
        // VARIABLES
        // =====================



        // =====================
        // SERVICES
        // =====================
        private readonly KSP2Service _ksp2Service;
        private readonly ConfigurationManager _configManager;

        // =====================
        // COMMANDS
        // =====================

        // File
        public ReactiveCommand<Unit, Unit> LaunchCommand { get; }
        public ReactiveCommand<Unit, Unit> RefreshModlistCommand { get; }
        public ReactiveCommand<Unit, Unit> ExitCommand { get; }


        // Edit
        public ReactiveCommand<Unit, Unit> ScanKSP2InstallLocationsCommand { get; }
        public ReactiveCommand<Unit, Unit> SetGameInstallPathCommand { get; }
        public ReactiveCommand<Unit, Unit> ClearConfigFileCommand { get; }
        public ReactiveCommand<Unit, Unit> ViewConfigFileCommand { get; }

        // View


        // Help
        public ReactiveCommand<Unit, Unit> ToucanUpdateCheckCommand { get; }


        // =====================
        // CONSTRUCTOR
        // =====================
        public HeaderViewModel(MainWindowViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;

            // Services
            _ksp2Service = new KSP2Service();
            _configManager = new ConfigurationManager();

            UpdateTimePlayed(_configManager.GetTimePlayed());

            // Commands

            ToucanUpdateCheckCommand = ReactiveCommand.Create(IsUpdateAvailable);
            ScanKSP2InstallLocationsCommand = ReactiveCommand.Create(ScanKSP2InstallLocations);
            SetGameInstallPathCommand = ReactiveCommand.Create(SetGameInstallPath);
            ClearConfigFileCommand = ReactiveCommand.Create(ClearConfigFile);
            ViewConfigFileCommand = ReactiveCommand.Create(ViewConfigFile);

            LaunchCommand = ReactiveCommand.Create(LaunchApplication);
            RefreshModlistCommand = ReactiveCommand.Create(RefreshModlist);
            ExitCommand = ReactiveCommand.Create(ExitApplication);
        }



        // =====================
        // METHODS
        // =====================

        // Check if an update is available on Github for Toucan Mod Manager
        public async void IsUpdateAvailable()
        {
            var latestReleaseUrl = $"{MainViewModel.FooterVM.ToucanWebsite}/releases/latest";
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0");
                    var response = await client.GetAsync(latestReleaseUrl);
                    response.EnsureSuccessStatusCode();
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var releaseJson = JsonDocument.Parse(responseContent);

                    var latestReleaseTagName = releaseJson.RootElement.GetProperty("tag_name").GetString();

                    Debug.WriteLine(releaseJson);

                    if (!string.Equals(latestReleaseTagName, MainViewModel.FooterVM.ToucanVersion, StringComparison.OrdinalIgnoreCase))
                    {
                        Debug.WriteLine($"Update {latestReleaseTagName} available! (Currently on {MainViewModel.FooterVM.ToucanVersion}");
                    }

                }
            }
            catch (HttpRequestException)
            {
                // Error connecting to GitHub or retrieving the release information
                Debug.WriteLine("Something went wrong retrieving Toucan Update Information!");
            }

        }


        // Method to update the time played values
        private void UpdateTimePlayed(int timePlayed)
        {
            Debug.WriteLine($"Time played: {timePlayed}");

            // Calculate hours, minutes, and seconds from the stored seconds
            int totalSeconds = timePlayed;
            int hours = totalSeconds / 3600;
            int minutes = (totalSeconds % 3600) / 60;
            int seconds = totalSeconds % 60;

            this.MainViewModel.FooterVM.Hours = hours.ToString();
            Debug.WriteLine($"Hours: {MainViewModel.FooterVM.Hours}");

            this.MainViewModel.FooterVM.Minutes = minutes.ToString();
            Debug.WriteLine($"Minutes: {MainViewModel.FooterVM.Minutes}");

            this.MainViewModel.FooterVM.Seconds = seconds.ToString();
            Debug.WriteLine($"Seconds: {MainViewModel.FooterVM.Seconds}");
        }



        // File
        private async void LaunchApplication()
        {
            try
            {
                string gamePath = _configManager.GetGamePath();
                Debug.WriteLine($"Launching Application at {gamePath}");
                DateTime launchTime = DateTime.Now;

                // Launch the application 
                Process process = Process.Start(gamePath);

                // Disable most of the UI
                MainViewModel.ValidGameFound = false;

                // Run the process on a separate thread asynchronously
                await Task.Run(() =>
                {
                    // Wait for the application to exit
                    process.WaitForExit();
                });

                // Calculate time played
                TimeSpan timePlayed = DateTime.Now - launchTime;
                int timePlayedSeconds = (int)timePlayed.TotalSeconds;
                Debug.WriteLine($"Time played: {timePlayedSeconds}");

                

                // Store the "Time played" in the config
                _configManager.SetTimePlayed(timePlayedSeconds);

                // Update the UI
                UpdateTimePlayed(_configManager.GetTimePlayed());

                MainViewModel.ValidGameFound = true;
            }

            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

        }

        private async void RefreshModlist()
        {
            MainViewModel.SelectedMod = null;
            MainViewModel.SidePanelVM.SidePanelVisible = false;
            await MainViewModel.ModlistVM.FetchMods(MainViewModel.ModlistVM.Category);
        }

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

        // Edit
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

        // View


        // Help


        // =====================
        // DIALOG BOXES
        // =====================

        // Dialog to confirm the user wants to clear the config file
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
                UpdateTimePlayed(0);

                // Set the ValidGameFound to false
                MainViewModel.ValidGameFound = false;
            }
        }

        // Dialog to show the user the game version and path
        private async Task ShowViewConfigFileMessageBox()
        {
            var configManager = new ConfigurationManager();
            string contentMessage;
            string gamePath = configManager.GetGamePath();
            string gameVersion = configManager.GetGameVersion();
            int timePlayed = configManager.GetTimePlayed();

            if (gamePath == "" || gameVersion == "")
            {
                contentMessage = "No configuration file found.";
            }

            else
            {
                contentMessage = $"**Game Path:** {gamePath}\r\n\r\n" + $"**Game Version:** {gameVersion}\r\n\r\n" + $"**Time Played:** {timePlayed}";
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

        // Dialog to show found game version and path
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
                MainViewModel.ValidGameFound = true;

            }
        }

        // Dialog to show error message
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
