using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HappyXamDevs.Services
{
    public abstract class AzureServiceBase : IAzureService
    {
        private const string AuthTokenKey = "auth-token";
        private const string UserIdKey = "user-id";

#error REPLACE [YOUR AZURE APP NAME HERE]
        protected const string AzureAppName = "[YOUR AZURE APP NAME HERE]";
        protected readonly static string FunctionAppUrl = $"https://{AzureAppName}.azurewebsites.net";

        public MobileServiceClient Client { get; }

        protected AzureServiceBase()
        {
            Client = new MobileServiceClient(FunctionAppUrl);
        }

        private void TryLoadUserDetails()
        {
            if (Client.CurrentUser != null) return;

            if (Application.Current.Properties.TryGetValue(AuthTokenKey, out var authToken) &&
                Application.Current.Properties.TryGetValue(UserIdKey, out var userId))
            {
                Client.CurrentUser = new MobileServiceUser(userId.ToString())
                {
                    MobileServiceAuthenticationToken = authToken.ToString()
                };
            }
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

            if (Client.CurrentUser != null)
            {
                Application.Current.Properties[AuthTokenKey] = Client.CurrentUser.MobileServiceAuthenticationToken;
                Application.Current.Properties[UserIdKey] = Client.CurrentUser.UserId;
                await Application.Current.SavePropertiesAsync();
            }

            return IsLoggedIn();
        }

        public bool IsLoggedIn()
        {
            TryLoadUserDetails();
            return Client.CurrentUser != null;
        }
    }
}