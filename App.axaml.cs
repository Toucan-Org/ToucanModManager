using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using System;
using System.Diagnostics;
using System.IO;
using ToucanUI.Services;
using ToucanUI.ViewModels;
using ToucanUI.Views;

namespace ToucanUI
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            var loggerService = new LoggerService();

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow(new MainWindowViewModel());
                desktop.MainWindow.Closed += (sender, e) => loggerService.Close();
            }

            base.OnFrameworkInitializationCompleted();
        }

    }
}