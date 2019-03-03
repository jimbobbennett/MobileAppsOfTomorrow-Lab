using System.Threading.Tasks;
using System.Windows.Input;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace HappyXamDevs.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public MainViewModel()
        {
            TakePhotoCommand = new Command(async () => await TakePhoto());
            SelectFromLibraryCommand = new Command(async () => await SelectFromLibrary());
        }

        public ICommand SelectFromLibraryCommand { get; }

        public ICommand TakePhotoCommand { get; }

        private async Task SelectFromLibrary()
        {
            var options = new PickMediaOptions { PhotoSize = PhotoSize.Medium };
            var photo = await CrossMedia.Current.PickPhotoAsync(options);
        }

        private async Task TakePhoto()
        {
            var options = new StoreCameraMediaOptions
            {
                PhotoSize = PhotoSize.Medium,
                DefaultCamera = CameraDevice.Front
            };
            var photo = await CrossMedia.Current.TakePhotoAsync(options);
        }
    }
}