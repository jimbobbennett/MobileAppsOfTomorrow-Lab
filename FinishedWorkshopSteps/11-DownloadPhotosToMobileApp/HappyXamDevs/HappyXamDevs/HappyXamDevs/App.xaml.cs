using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace HappyXamDevs
{
    public partial class App : Xamarin.Forms.Application
    {
        public App()
        {
            InitializeComponent();

            var navigationPage = new Xamarin.Forms.NavigationPage(new MainPage());
            MainPage = navigationPage;

            navigationPage.BarBackgroundColor = (Color)Resources["CoolPurple"];
            navigationPage.BarTextColor = Color.White;

            navigationPage.On<iOS>().SetPrefersLargeTitles(true);
            navigationPage.On<iOS>().SetUseSafeArea(true);
        }
    }
}