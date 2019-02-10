using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HappyXamDevs.Services
{
    public abstract class AzureServiceBase : IAzureService
    {
        private const string AuthTokenKey = "auth-token";
        private const string PhotoResource = "photo";
        private const string UserIdKey = "user-id";
#error REPLACE [YOUR FACE API BASE URL]
        private const string FaceApiBaseUrl = "[YOUR FACE API BASE URL]";
        private readonly FaceClient faceApiClient;  

#error REPLACE [YOUR AZURE APP NAME HERE]
        protected const string AzureAppName = "[YOUR AZURE APP NAME HERE]";
        protected readonly static string FunctionAppUrl = $"https://{AzureAppName}.azurewebsites.net";

        public MobileServiceClient Client { get; }

        protected AzureServiceBase()
        {
            Client = new MobileServiceClient(FunctionAppUrl);

#error REPLACE [YOUR API KEY HERE]
            var creds = new ApiKeyServiceClientCredentials("[YOUR API KEY HERE]");
            faceApiClient = new FaceClient(creds)
            {
                Endpoint = FaceApiBaseUrl
            };
        }

        private void TryLoadUserDetails()
        {
            if (Client.CurrentUser != null) return;

            if (Application.Current.Properties.TryGetValue(AuthTokenKey, out var authToken) &&
                Application.Current.Properties.TryGetValue(UserIdKey, out var userId))
            {
                Client.CurrentUser = new MobileServiceUser(userId.ToString())
                {
                    MobileServiceAuthenticationToken = authToken.ToString()
                };

                MessagingCenter.Send<IAzureService>(this, "LoggedIn");
            }
        }

        protected abstract Task AuthenticateUser();

        public async Task<bool> Authenticate()
        {
            if (IsLoggedIn()) return true;
            await AuthenticateUser();

            if (Client.CurrentUser != null)
            {
                MessagingCenter.Send<IAzureService>(this, "LoggedIn");
                Application.Current.Properties[AuthTokenKey] = Client.CurrentUser.MobileServiceAuthenticationToken;
                Application.Current.Properties[UserIdKey] = Client.CurrentUser.UserId;
                await Application.Current.SavePropertiesAsync();
            }

            return IsLoggedIn();
        }

        public async Task DownloadPhoto(PhotoMetadata photoMetadata)
        {
            if (File.Exists(photoMetadata.FileName))
                return;

            var response = await Client.InvokeApiAsync($"photo/{photoMetadata.Name}",
                                                       HttpMethod.Get,
                                                       new Dictionary<string, string>());

            var photo = response["Photo"].Value<string>();
            var bytes = Convert.FromBase64String(photo);

            using (var fs = new FileStream(photoMetadata.FileName, FileMode.CreateNew))
                await fs.WriteAsync(bytes, 0, bytes.Length);
        }

        public async Task<IEnumerable<PhotoMetadata>> GetAllPhotoMetadata()
        {
            var allMetadata = await Client.InvokeApiAsync<List<PhotoMetadata>>(PhotoResource,
                                                                               HttpMethod.Get,
                                                                               new Dictionary<string, string>());

            foreach (var metadata in allMetadata)
                await DownloadPhoto(metadata);

            return allMetadata;
        }

        public bool IsLoggedIn()
        {
            TryLoadUserDetails();
            return Client.CurrentUser != null;
        }

        public async Task UploadPhoto(MediaFile photo)
        {
            using (var s = photo.GetStream())
            {
                var bytes = new byte[s.Length];
                await s.ReadAsync(bytes, 0, Convert.ToInt32(s.Length));

                var content = new
                {
                    Photo = Convert.ToBase64String(bytes)
                };

                var json = JToken.FromObject(content);

                await Client.InvokeApiAsync(PhotoResource, json);
            }
        }

        public async Task<bool> VerifyHappyFace(MediaFile photo)
        {
            using (var s = photo.GetStream())
            {
                var faceAttributes = new List<FaceAttributeType> { FaceAttributeType.Emotion };
                var faces = await faceApiClient.Face.DetectWithStreamAsync(s, returnFaceAttributes: faceAttributes);
                return faces.Any() && faces.All(f => f.FaceAttributes.Emotion.Happiness > 0.75);
            }
        }
    }
}