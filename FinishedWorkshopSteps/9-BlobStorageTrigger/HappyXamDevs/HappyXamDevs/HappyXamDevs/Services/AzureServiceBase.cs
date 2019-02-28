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
#error REPLACE [YOUR API KEY HERE]
#error REPLACE [YOUR FACE API BASE URL]
        private readonly FaceClient faceApiClient = new FaceClient(new ApiKeyServiceClientCredentials("[YOUR API KEY HERE]"))
        {
            Endpoint = "[YOUR FACE API BASE URL]"
            //Example Face API Base Url: "https://westus.api.cognitive.microsoft.com/"
        };

#error REPLACE [YOUR AZURE APP NAME HERE]
        protected const string AzureAppName = "[YOUR AZURE APP NAME HERE]";
        protected readonly static string FunctionAppUrl = $"https://{AzureAppName}.azurewebsites.net";

        public MobileServiceClient Client { get; }

        protected AzureServiceBase()
        {
            Client = new MobileServiceClient(FunctionAppUrl);
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
            }
        }

        protected abstract Task AuthenticateUser();

        public async Task<bool> Authenticate()
        {
            if (IsLoggedIn()) return true;
            await AuthenticateUser();

            if (Client.CurrentUser != null)
            {
                Application.Current.Properties[AuthTokenKey] = Client.CurrentUser.MobileServiceAuthenticationToken;
                Application.Current.Properties[UserIdKey] = Client.CurrentUser.UserId;
                await Application.Current.SavePropertiesAsync();
            }

            return IsLoggedIn();
        }

        public bool IsLoggedIn()
        {
            TryLoadUserDetails();
            return Client.CurrentUser != null;
        }

        public async Task UploadPhoto(MediaFile photo)
        {
            using (var photoStream = photo.GetStream())
            {
                var bytes = new byte[photoStream.Length];
                await photoStream.ReadAsync(bytes, 0, Convert.ToInt32(photoStream.Length));

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
            using (var photoStream = photo.GetStream())
            {
                var faceAttributes = new List<FaceAttributeType> { FaceAttributeType.Emotion };

                var faces = await faceApiClient.Face.DetectWithStreamAsync(photoStream, returnFaceAttributes: faceAttributes);

                var areHappyFacesDetected = faces.Any(f => f.FaceAttributes.Emotion.Happiness > 0.75);
                return areHappyFacesDetected;
            }
        }
    }
}