using System;
using System.IO;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Roche
{
    public sealed partial class ConfigurationPage : Page
    {
        private string _path = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            @".roche\server.properties");

        public ServerConfiguration Configuration { get; set; }

        public ConfigurationPage()
            => InitializeComponent();

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Configuration = ServerConfiguration.Load(_path);

            if (e.SourcePageType == typeof(NewWorldPage))
            {
                var world = (NewWorld)e.Parameter;

                Configuration.WorldName = world.WorldName;
                Configuration.GameMode = world.GameMode;
                Configuration.WorldType = world.WorldType;
                Configuration.Seed = world.Seed;
            }

            base.OnNavigatedTo(e);
        }

        private void HandleStartClick(object sender, RoutedEventArgs e)
        {
            Configuration.Save(_path);

            Frame.Navigate(typeof(MainPage));
        }
    }
}
