using System;
using System.IO;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace Roche
{
    public sealed partial class MainPage : Page
    {
        MinecraftServer _server;

        public MainPage()
            => InitializeComponent();

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            _server = MinecraftServer.Start(
                Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                    @".roche\bedrock_server.exe"));
        }

        private void HandleStopClick(object sender, RoutedEventArgs e)
        {
            _server.Stop();
            Frame.Navigate(typeof(ConfigurationPage));
        }
    }
}
