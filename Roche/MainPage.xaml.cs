using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace Roche
{
    public sealed partial class MainPage : Page
    {
        Server _server;

        public MainPage()
            => InitializeComponent();

        // TODO:
        // Server Name
        // Server Address
        // Port

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            _server = Server.Start(Paths.ServerPath);
        }

        private void HandleStopClick(object sender, RoutedEventArgs e)
        {
            _server.Stop();
            Frame.Navigate(typeof(ServerSettingsPage));
        }
    }
}
