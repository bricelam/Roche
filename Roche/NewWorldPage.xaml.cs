using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Roche
{
    public sealed partial class NewWorldPage : Page
    {
        public NewWorldPage()
            => InitializeComponent();

        public NewWorld World { get; set; } = new NewWorld();

        private void HandleNextClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ConfigurationPage), World);
        }
    }
}
