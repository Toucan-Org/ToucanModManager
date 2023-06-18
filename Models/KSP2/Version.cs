using Newtonsoft.Json;
using System;
using System.Text.Json;

namespace ToucanUI.Models.KSP2
{
    public class Version
    {

        [JsonProperty("id")]
        public int VersionID { get; set; }

        [JsonProperty("game_version")]
        public string GameVersion { get; set; }

        [JsonProperty("friendly_version")]
        public string? FriendlyVersion { get; set; }

        [JsonProperty("download_path")]
        public string? DownloadPath { get; set; }

        [JsonProperty("changelog")]
        public string? Changelog { get; set; }

        [JsonProperty("downloads")]
        public int Downloads { get; set; }

        [JsonProperty("created")]
        public DateTimeOffset Created { get; set; }

        [JsonProperty("is_installed")]
        public bool IsInstalled { get; set; }

        // Converts the DownloadSize from Bytes to a more suitable size format

        // If its being read from the offline cache, then the download size is already fetched
        public static Version FromJson(string jsonString)
        {
            var version = JsonConvert.DeserializeObject<Version>(jsonString);

            // Ensure the base URL is appended to the DownloadPath
            if (!version.DownloadPath.StartsWith("https://spacedock.info"))
            {
                version.DownloadPath = $"https://spacedock.info{version.DownloadPath}";
            }

            return version;
        }

    }
}
