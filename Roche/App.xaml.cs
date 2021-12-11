using System;
using System.IO;
using System.Linq;
using Microsoft.UI.Xaml;

namespace Roche
{
    public partial class App : Application
    {
        private ShellWindow _shellWindow;

        public App()
            => InitializeComponent();

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            _shellWindow = new ShellWindow();
            _shellWindow.Activate();

            if (!Directory.Exists(Paths.ServerDir))
            {
                _shellWindow.Frame.Navigate(typeof(WelcomePage));
            }
            else if (!Directory.Exists(Paths.WorldsDir)
                || !Directory.EnumerateFileSystemEntries(Paths.WorldsDir).Any())
            {
                _shellWindow.Frame.Navigate(typeof(CreateWorldPage));
            }
            else
            {
                _shellWindow.Frame.Navigate(typeof(ServerSettingsPage));
            }
        }
    }
}
