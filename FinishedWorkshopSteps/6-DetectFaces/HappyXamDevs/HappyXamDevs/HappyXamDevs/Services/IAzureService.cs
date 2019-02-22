using Plugin.Media.Abstractions;
using System.Threading.Tasks;

namespace HappyXamDevs.Services
{
    public interface IAzureService
    {
        Task<bool> Authenticate();

        bool IsLoggedIn();

        Task<bool> VerifyHappyFace(MediaFile photo);
    }
}