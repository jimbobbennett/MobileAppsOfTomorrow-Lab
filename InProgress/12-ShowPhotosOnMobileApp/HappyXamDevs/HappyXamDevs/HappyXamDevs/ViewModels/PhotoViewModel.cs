using HappyXamDevs.Services;
using System.Linq;
using Xamarin.Forms;

namespace HappyXamDevs.ViewModels
{
    public class PhotoViewModel : BaseViewModel
    {
        public string Caption { get; }

        public ImageSource Photo { get; }

        public string Tags { get; }

        public long Timestamp { get; }

        public PhotoViewModel(PhotoMetadata photoMetadata)
        {
            Caption = photoMetadata.Caption;
            Timestamp = photoMetadata.Timestamp;
            Tags = string.Join(" ", photoMetadata.Tags.Select(t => $"#{t}"));
            Photo = ImageSource.FromFile(photoMetadata.FileName);
        }
    }
}