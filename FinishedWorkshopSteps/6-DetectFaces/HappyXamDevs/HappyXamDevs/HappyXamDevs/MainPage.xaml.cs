using HappyXamDevs.Services;
using Xamarin.Forms;

namespace HappyXamDevs
{
    public partial class MainPage : ContentPage
    {
        private bool firstAppear = true;

        public MainPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (!firstAppear) return;
            firstAppear = false;

            var azureService = DependencyService.Get<IAzureService>();

            if (!azureService.IsLoggedIn())
            {
                await Navigation.PushModalAsync(new LoginPage(), false);
            }
        }
    }
}