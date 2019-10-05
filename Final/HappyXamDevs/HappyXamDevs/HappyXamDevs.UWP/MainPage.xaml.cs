using HappyXamDevs.Services;
using HappyXamDevs.UWP.Services;
using Microsoft.WindowsAzure.MobileServices;
using System;
using Windows.UI.Xaml.Navigation;

namespace HappyXamDevs.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();

            LoadApplication(new HappyXamDevs.App());
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is Uri uriParameter)
            {
                var azureService = Xamarin.Forms.DependencyService.Get<IAzureService>() as AzureService;
                azureService.Client.ResumeWithURL(uriParameter);
            }
        }
    }
}