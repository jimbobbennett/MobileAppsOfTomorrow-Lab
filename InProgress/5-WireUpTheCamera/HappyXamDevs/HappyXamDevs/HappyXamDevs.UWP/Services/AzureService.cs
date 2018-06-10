using HappyXamDevs.Services;
using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;

namespace HappyXamDevs.UWP.Services
{
    public class AzureService : AzureServiceBase
    {
        protected override async Task AuthenticateUser()
        {
            await Client.LoginAsync(MobileServiceAuthenticationProvider.Facebook,
                                    AzureAppName);
        }
    }
}