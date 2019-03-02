using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;

namespace HappyXamDevs.Services
{
    public abstract class AzureServiceBase : IAzureService
    {
#error REPLACE [YOUR AZURE APP NAME HERE]
        protected const string AzureAppName = "[YOUR AZURE APP NAME HERE]";
        protected readonly static string FunctionAppUrl = $"https://{AzureAppName}.azurewebsites.net";

        protected AzureServiceBase()
        {
            Client = new MobileServiceClient(FunctionAppUrl);
        }

        public MobileServiceClient Client { get; }

        public async Task<bool> Authenticate()
        {
            if (IsLoggedIn())
                return true;

            try
            {
                await AuthenticateUser();
            }
            catch (System.InvalidOperationException)
            {
                return false;
            }

            return IsLoggedIn();
        }

        public bool IsLoggedIn()
        {
            return Client.CurrentUser != null;
        }

        protected abstract Task AuthenticateUser();
    }
}