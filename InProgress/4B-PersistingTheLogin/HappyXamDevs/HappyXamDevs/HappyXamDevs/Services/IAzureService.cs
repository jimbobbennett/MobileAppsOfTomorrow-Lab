using System.Threading.Tasks;

namespace HappyXamDevs.Services
{
    public interface IAzureService
    {
        Task<bool> Authenticate();

        bool IsLoggedIn();
    }
}