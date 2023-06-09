using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;

namespace ToucanUI.Models
{
    public class Version : INotifyPropertyChanged
    {

        // =====================
        // VERSION VARIABLES
        // =====================
        public Mod ParentMod { get; }
        public int VersionID { get; set; }
        public string GameVersion { get; set; }
        public JsonElement VersionJson { get; }
        public string? FriendlyVersion { get; internal set; }
        public string? DownloadPath { get; internal set; }
        public string? Changelog { get; internal set; }
        public int Downloads { get; internal set; }
        public DateTimeOffset Created { get; internal set; }


        // Used to track the download size (in bytes)
        private long? _downloadSize;
        public long? DownloadSize
        {
            get => _downloadSize;
            set
            {
                _downloadSize = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DownloadSizeDisplay));
            }
        }

        // Used to track if the download size is fetched
        private bool _isDownloadSizeFetched;
        public bool IsDownloadSizeFetched
        {
            get => _isDownloadSizeFetched;
            private set
            {
                _isDownloadSizeFetched = value;
                OnPropertyChanged();
            }
        }

        // Used to track if a version is selected
        private bool _isSelectedVersion;
        public bool IsSelectedVersion
        {
            get => _isSelectedVersion;
            set
            {
                if (_isSelectedVersion != value)
                {
                    _isSelectedVersion = value;
                    OnPropertyChanged();

                    if (value && ParentMod != null)
                    {
                        ParentMod.OnVersionSelected(this);
                        Debug.WriteLine($"{ParentMod.Name} - {FriendlyVersion} is selected");
                    }
                }
            }
        }

        // Converts the DownloadSize from Bytes to MiB
        public string DownloadSizeDisplay
        {
            get
            {
                if (DownloadSize != null)
                {
                    double sizeInMiB = (double)DownloadSize / (1024 * 1024);
                    return $"{sizeInMiB:F2} MB";
                }
                else
                {
                    return "N/A";
                }
            }
        }



        // =====================
        // VERSION CONSTRUCTOR
        // =====================
        public Version(JsonElement versionJson, Mod parentMod)
        {
            VersionJson = versionJson;
            ParentMod = parentMod;
        }

        // INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Gets the download size of a mod version
        public async Task GetDownloadSizeAsync()
        {
            if (DownloadSize != null || IsDownloadSizeFetched) return;  // Skip if the download size is already fetched

            using var httpClient = new HttpClient();
            var downloadUrl = DownloadPath;
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/zip"));

            try
            {
                Debug.WriteLine($"Getting download size for {ParentMod.Name} {FriendlyVersion}");
                var request = new HttpRequestMessage(HttpMethod.Head, downloadUrl);
                var response = await httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    response.EnsureSuccessStatusCode();
                    var contentLength = response.Content.Headers.ContentLength;
                    DownloadSize = contentLength;
                }
                else
                {
                    DownloadSize = null;
                }
            }
            catch
            {
                DownloadSize = null;
            }

            IsDownloadSizeFetched = true;
        }


    }
}
