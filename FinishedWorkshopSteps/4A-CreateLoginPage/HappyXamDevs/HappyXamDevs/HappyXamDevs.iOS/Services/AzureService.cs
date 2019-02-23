using System.Threading.Tasks;
using CoreFoundation;
using HappyXamDevs.Services;
using Microsoft.WindowsAzure.MobileServices;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(HappyXamDevs.iOS.Services.AzureService))]

namespace HappyXamDevs.iOS.Services
{
    public class AzureService : AzureServiceBase
    {
        protected override async Task AuthenticateUser()
        {
            var currentViewController = await GetCurrentViewController();

            await Client.LoginAsync(currentViewController,
                                    MobileServiceAuthenticationProvider.Facebook,
                                    AzureAppName);
        }

        private static Task<UIViewController> GetCurrentViewController()
        {
            var tcs = new TaskCompletionSource<UIViewController>();

            DispatchQueue.MainQueue.DispatchAsync(() =>
            {
                var rootController = UIApplication.SharedApplication.KeyWindow.RootViewController;

                switch (rootController.PresentedViewController)
                {
                    case UINavigationController navigationController:
                        tcs.SetResult(navigationController.TopViewController);
                        break;

                    case UITabBarController tabBarController:
                        tcs.SetResult(tabBarController.SelectedViewController);
                        break;

                    case null:
                        tcs.SetResult(rootController);
                        break;

                    default:
                        tcs.SetResult(rootController.PresentedViewController);
                        break;
                }
            });

            return tcs.Task;
        }
    }
}