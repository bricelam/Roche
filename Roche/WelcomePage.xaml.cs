using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Roche
{
    public sealed partial class WelcomePage : Page
    {
        public WelcomePage()
            => InitializeComponent();

        private void HandleNextClick(object sender, RoutedEventArgs e)
            => Frame.Navigate(typeof(DownloadPage));
    }
}
