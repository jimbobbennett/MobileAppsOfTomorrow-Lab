using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace HappyXamDevs
{
    public partial class App : Application
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

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }
    }
}