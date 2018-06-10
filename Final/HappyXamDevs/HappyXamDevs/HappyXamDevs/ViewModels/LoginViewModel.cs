using HappyXamDevs.Services;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace HappyXamDevs.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public ICommand LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new Command(async () => await Login());
        }

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