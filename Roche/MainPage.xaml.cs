using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace Roche
{
    [INotifyPropertyChanged]
    public sealed partial class MainPage : Page
    {
        [ObservableProperty]
        bool _started;

        public MainPage()
            => InitializeComponent();

        public ServerProperties Properties { get; private set; }
        public Server Server { get; private set; }
        public string ServerAddress { get; private set; }
        public ObservableCollection<string> Players { get; } = new();

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Properties = ServerProperties.Load(Paths.ServerPropertiesPath);

            Server = new Server(Paths.ServerPath);
            Server.PlayerConnected += HandlePlayerConnected;
            Server.PlayerDisconnected += HandlePlayerDisconnected;
            // TODO: Server Address

            // TODO: Join page
            // Check: CheckNetIsolation LoopbackExempt -s
            // Enable: CheckNetIsolation LoopbackExempt -a -n=Microsoft.MinecraftUWP_8wekyb3d8bbwe
            // Launch: shell:AppsFolder\Microsoft.MinecraftUWP_8wekyb3d8bbwe!App

            await Server.StartAsync();
            Started = true;
        }

        void HandlePlayerConnected(object sender, PlayerEventArgs e)
            => DispatcherQueue.TryEnqueue(
                () =>
                {
                    Players.Add(e.Name);
                });

        void HandlePlayerDisconnected(object sender, PlayerEventArgs e)
            => DispatcherQueue.TryEnqueue(
                () =>
                {
                    Players.Remove(e.Name);
                });

        void HandleStopClick(object sender, RoutedEventArgs e)
        {
            Server.PlayerConnected -= HandlePlayerConnected;
            Server.PlayerDisconnected -= HandlePlayerDisconnected;
            Server.Stop();
            Frame.Navigate(typeof(ServerSettingsPage));
        }
    }
}
