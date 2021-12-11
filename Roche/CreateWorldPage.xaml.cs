using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;

namespace Roche
{
    public sealed partial class CreateWorldPage : Page
    {
        public CreateWorldPage()
            => InitializeComponent();

        public List<GameMode> GameModes = new()
        {
            GameMode.Survival,
            GameMode.Creative,
            GameMode.Adventure
        };

        public List<LevelType> LevelTypes = new()
        {
            LevelType.Flat,

            // TODO: Display "Infinite"
            LevelType.Default
        };

        public LevelSettings Settings { get; } = new LevelSettings();

        private void HandleNextClick(object sender, RoutedEventArgs e)
            => Frame.Navigate(typeof(ServerSettingsPage), Settings);
    }
}
