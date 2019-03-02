using System.Threading.Tasks;
using Plugin.Media.Abstractions;

namespace HappyXamDevs.Services
{
    public interface IAzureService
    {
        Task<bool> Authenticate();

        bool IsLoggedIn();

        Task<bool> VerifyHappyFace(MediaFile photo);
    }
}