using Plugin.Media.Abstractions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HappyXamDevs.Services
{
    public interface IAzureService
    {
        Task<bool> Authenticate();

        Task DownloadPhoto(PhotoMetadata photoMetadata);

        Task<IEnumerable<PhotoMetadata>> GetAllPhotoMetadata();

        bool IsLoggedIn();

        Task UploadPhoto(MediaFile photo);

        Task<bool> VerifyHappyFace(MediaFile photo);
    }
}