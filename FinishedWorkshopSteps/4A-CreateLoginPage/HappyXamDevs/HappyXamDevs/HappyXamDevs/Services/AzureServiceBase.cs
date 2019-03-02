using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;

namespace HappyXamDevs.Services
{
    public abstract class AzureServiceBase : IAzureService
    {
#error REPLACE [YOUR AZURE APP NAME HERE]
        protected const string AzureAppName = "[YOUR AZURE APP NAME HERE]";
        protected readonly static string FunctionAppUrl = $"https://{AzureAppName}.azurewebsites.net";

        public MobileServiceClient Client { get; }

        protected AzureServiceBase()
        {
            Client = new MobileServiceClient(FunctionAppUrl);
        }

        protected abstract Task AuthenticateUser();

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
    }
}