using System.Threading.Tasks;
using System.Windows.Input;
using HappyXamDevs.Services;
using Xamarin.Forms;

namespace HappyXamDevs.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public LoginViewModel()
        {
            LoginCommand = new Command(async () => await Login());
        }

        public ICommand LoginCommand { get; }

        private async Task Login()
        {
            var azureService = DependencyService.Get<IAzureService>();
            if (await azureService.Authenticate())
            {
                await Application.Current.MainPage.Navigation.PopModalAsync();
            }
        }
    }
}