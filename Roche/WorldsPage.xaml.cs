using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace Roche
{
    public sealed partial class WorldsPage : Page
    {
        public WorldsPage()
            => InitializeComponent();

        public List<string> Worlds { get; private set; } = new List<string>();

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Worlds = Directory.EnumerateDirectories(Paths.WorldsDir).Select(Path.GetFileName).ToList();
        }

        private void HandleNewClick(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
            => Frame.Navigate(typeof(CreateWorldPage));

        private void HandleNextClick(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
            => Frame.Navigate(typeof(ServerSettingsPage), _worldsListBox.SelectedItem);
    }
}
