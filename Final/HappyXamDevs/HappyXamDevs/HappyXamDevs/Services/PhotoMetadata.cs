using Newtonsoft.Json;
using System.IO;
using Xamarin.Essentials;

namespace HappyXamDevs.Services
{
    public class PhotoMetadata
    {
        public string Caption { get; set; }

        public string FileName => Path.Combine(FileSystem.CacheDirectory, $"{Name}.jpg");

        public string Name { get; set; }

        public string[] Tags { get; set; }

        [JsonProperty("_ts")]
        public long Timestamp { get; set; }
    }
}