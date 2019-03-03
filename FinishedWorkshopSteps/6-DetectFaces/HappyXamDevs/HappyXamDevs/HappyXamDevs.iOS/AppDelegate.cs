using Foundation;
using HappyXamDevs.iOS.Services;
using HappyXamDevs.Services;
using Microsoft.WindowsAzure.MobileServices;
using UIKit;
using Xamarin.Forms;

namespace HappyXamDevs.iOS
{
    [Register(nameof(AppDelegate))]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }

        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            var azureService = DependencyService.Get<IAzureService>() as AzureService;
            return azureService.Client.ResumeWithURL(url);
        }
    }
}