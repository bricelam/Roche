using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace Roche
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();

            MakeOperatorCommand = new RelayCommand<string>(player => Server.MakeOperator(player));
            MakeMemberCommand = new RelayCommand<string>(player => Server.MakeMember(player));
            SetCreativeCommand = new RelayCommand<string>(player => Server.SetGameMode(player, GameMode.Creative));
            SetSurvivalCommand = new RelayCommand<string>(player => Server.SetGameMode(player, GameMode.Survival));
            SetAdventureCommand = new RelayCommand<string>(player => Server.SetGameMode(player, GameMode.Adventure));
        }

        public List<Difficulty> Difficulties = new()
        {
            Difficulty.Peaceful,
            Difficulty.Easy,
            Difficulty.Normal,
            Difficulty.Hard
        };

        public ServerProperties Properties { get; private set; }
        public Server Server { get; private set; }
        public string ServerAddress { get; private set; }
        public ObservableCollection<string> Players { get; } = new();

        public ICommand MakeOperatorCommand { get; }
        public ICommand MakeMemberCommand { get; }
        public ICommand SetCreativeCommand { get; }
        public ICommand SetSurvivalCommand { get; }
        public ICommand SetAdventureCommand { get; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is not Server server)
                throw new Exception("Missing server");

            Properties = ServerProperties.Load(Paths.ServerPropertiesPath);

            Server = server;
            Server.PlayerConnected += HandlePlayerConnected;
            Server.PlayerDisconnected += HandlePlayerDisconnected;

            // TODO: Join page
            // Check: CheckNetIsolation LoopbackExempt -s
            // Enable: CheckNetIsolation LoopbackExempt -a -n=Microsoft.MinecraftUWP_8wekyb3d8bbwe
            // Launch: shell:AppsFolder\Microsoft.MinecraftUWP_8wekyb3d8bbwe!App
            base.OnNavigatedTo(e);
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

        private void HandleDifficultyChanged(object sender, SelectionChangedEventArgs e)
            => Server.SetDifficulty((Difficulty)e.AddedItems[0]);

        private void HandleAllowCheatsToggled(object sender, RoutedEventArgs e)
            => Server.AllowCheats(((ToggleSwitch)sender).IsOn);
    }
}
