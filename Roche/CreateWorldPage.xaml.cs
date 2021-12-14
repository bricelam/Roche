using System.Collections.Generic;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

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

        public LevelProperties Properties { get; } = new LevelProperties();

        private void HandleCreateClick(object sender, RoutedEventArgs e)
            => Frame.Navigate(typeof(ServerSettingsPage), Properties);
    }
}
