using System.Collections.Generic;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace Roche
{
    public sealed partial class ServerSettingsPage : Page
    {
        public ServerSettingsPage()
            => InitializeComponent();

        public ServerProperties Properties { get; set; }

        public List<Difficulty> Difficulties = new()
        {
            Difficulty.Peaceful,
            Difficulty.Easy,
            Difficulty.Normal,
            Difficulty.Hard
        };

        // TODO: Display icons
        public List<PermissionLevel> PlayerPermissionLevels = new()
        {
            // 👋 Visitor
            PermissionLevel.Visitor,

            // ⭐ Member
            PermissionLevel.Member,

            // 👑 Operator
            PermissionLevel.Operator
        };

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Properties = ServerProperties.Load(Paths.ServerPropertiesPath);

            if (e.Parameter is LevelProperties world)
            {
                Properties.LevelName = world.LevelName;
                Properties.GameMode = world.GameMode;
                Properties.LevelSeed = world.LevelSeed;
            }

            base.OnNavigatedTo(e);
        }

        private async void HandleStartClick(object sender, RoutedEventArgs e)
        {
            _startButton.IsEnabled = false;
            _startButton.Content = "Starting...";

            Properties.Save(Paths.ServerPropertiesPath);

            var server = new Server(Paths.ServerPath);
            await server.StartAsync();

            Frame.Navigate(typeof(MainPage), server);
        }
    }
}
