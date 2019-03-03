using System.Collections.Generic;
using System.Threading.Tasks;
using HappyXamDevs.Models;
using Plugin.Media.Abstractions;

namespace HappyXamDevs.Services
{
    public interface IAzureService
    {
        Task<bool> Authenticate();

        Task DownloadPhoto(PhotoMetadataModel photoMetadata);

        Task<IEnumerable<PhotoMetadataModel>> GetAllPhotoMetadata();

        bool IsLoggedIn();

        Task UploadPhoto(MediaFile photo);

        Task<bool> VerifyHappyFace(MediaFile photo);
    }
}