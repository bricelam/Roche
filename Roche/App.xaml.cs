using System;
using System.IO;
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

            var serverDir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                @".roche");

            if (!Directory.Exists(serverDir))
            {
                _shellWindow.Frame.Navigate(typeof(WelcomePage));
            }
            else if (!Directory.Exists(Path.Combine(serverDir, "worlds")))
            {
                _shellWindow.Frame.Navigate(typeof(NewWorldPage));
            }
            else
            {
                _shellWindow.Frame.Navigate(typeof(ConfigurationPage));
            }
        }
    }
}
