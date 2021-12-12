using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System.Collections.Generic;

namespace Roche
{
    public sealed partial class ServerSettingsPage : Page
    {

        public ServerSettingsPage()
            => InitializeComponent();

        public ServerProperties Config { get; set; }

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
            Config = ServerProperties.Load(Paths.ServerPropertiesPath);

            if (e.SourcePageType == typeof(CreateWorldPage))
            {
                var world = (LevelSettings)e.Parameter;

                Config.LevelName = world.LevelName;
                Config.GameMode = world.GameMode;
                Config.LevelType = world.LevelType;
                Config.LevelSeed = world.LevelSeed;
            }

            base.OnNavigatedTo(e);
        }

        private void HandleStartClick(object sender, RoutedEventArgs e)
        {
            Config.Save(Paths.ServerPropertiesPath);

            Frame.Navigate(typeof(MainPage));
        }
    }
}
