using System.Threading.Tasks;
using HappyXamDevs.Services;
using Microsoft.WindowsAzure.MobileServices;

[assembly: Xamarin.Forms.Dependency(typeof(HappyXamDevs.UWP.Services.AzureService))]
namespace HappyXamDevs.UWP.Services
{
    public class AzureService : AzureServiceBase
    {
        protected override Task AuthenticateUser()
        {
            return Client.LoginAsync(MobileServiceAuthenticationProvider.Facebook,
                                    "happyxamdevs");
        }
    }
}