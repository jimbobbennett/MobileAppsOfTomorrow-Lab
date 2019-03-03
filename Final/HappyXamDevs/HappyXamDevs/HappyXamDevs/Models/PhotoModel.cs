using System.Linq;
using Xamarin.Forms;

namespace HappyXamDevs.Models
{
    public class PhotoModel
    {
        public PhotoModel(PhotoMetadataModel photoMetadata)
        {
            Caption = photoMetadata.Caption;
            Timestamp = photoMetadata.Timestamp;
            Tags = string.Join(" ", photoMetadata.Tags.Select(t => $"#{t}"));
            Photo = ImageSource.FromFile(photoMetadata.FileName);
        }

        public string Caption { get; }

        public ImageSource Photo { get; }

        public string Tags { get; }

        public long Timestamp { get; }
    }
}