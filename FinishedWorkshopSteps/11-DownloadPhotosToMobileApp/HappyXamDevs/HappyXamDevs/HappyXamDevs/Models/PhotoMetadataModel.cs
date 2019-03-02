using System.IO;
using Newtonsoft.Json;
using Xamarin.Essentials;

namespace HappyXamDevs.Models
{
    public class PhotoMetadataModel
    {
        public string FileName => Path.Combine(FileSystem.CacheDirectory, $"{Name}.jpg");

        public string Caption { get; set; }

        public string Name { get; set; }

        public string[] Tags { get; set; }

        [JsonProperty("_ts")]
        public long Timestamp { get; set; }
    }
}