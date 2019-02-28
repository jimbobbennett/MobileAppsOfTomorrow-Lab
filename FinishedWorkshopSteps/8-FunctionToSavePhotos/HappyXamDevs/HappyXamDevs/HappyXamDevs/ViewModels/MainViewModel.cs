using HappyXamDevs.Services;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace HappyXamDevs.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IAzureService azureService;

        public MainViewModel()
        {
            TakePhotoCommand = new Command(async () => await TakePhoto());
            SelectFromLibraryCommand = new Command(async () => await SelectFromLibrary());
            azureService = DependencyService.Get<IAzureService>();
        }

        public ICommand SelectFromLibraryCommand { get; }
        public ICommand TakePhotoCommand { get; }

        private async Task SelectFromLibrary()
        {
            var options = new PickMediaOptions { PhotoSize = PhotoSize.Medium };
            
            var photo = await CrossMedia.Current.PickPhotoAsync(options);
            
            if (!await ValidatePhoto(photo)) 
                return;

            await azureService.UploadPhoto(photo);
        }

        private async Task TakePhoto()
        {
            var options = new StoreCameraMediaOptions { PhotoSize = PhotoSize.Medium };
            
            var photo = await CrossMedia.Current.TakePhotoAsync(options);
            
            if (!await ValidatePhoto(photo)) 
                return;
            
            await azureService.UploadPhoto(photo);
        }

        private async Task<bool> ValidatePhoto(MediaFile photo)
        {
            if (photo is null) 
                return false;

            var isHappy = await azureService.VerifyHappyFace(photo);
            
            if (isHappy) return true;

            await Application.Current.MainPage.DisplayAlert("Sad panda",
                                                            "I can't find any happy Xamarin developers in this picture. Please try again.",
                                                            "Will do!");
            return false;
        }
    }
}